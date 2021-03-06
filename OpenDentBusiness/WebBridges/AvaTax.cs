﻿using Avalara.AvaTax.RestClient;
using CodeBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OpenDentBusiness
{
    ///<summary>
    /// This is an unusual bridge, currently available for HQ only that will handle sales tax calls to the Avalara API. 
    /// This is a wrapper class for calling the Avalara SDK, as we are not implementing our own interface for the API. 
    /// SDK Documentation: https://github.com/avadev/AvaTax-REST-V2-DotNet-SDK API Documentation: https://developer.avalara.com/avatax/dev-guide/. 
    /// </summary>
    public class AvaTax
    {
        private static Program program;

        private static long GetAvaTaxProgramId()
        {
            if (program == null)
            {
                program = new Program();
            }

            return program.Id;
        }

        /// <summary>
        /// The adjustment type defnum to be associated with SalesTax adjustments.
        /// </summary>
        public static long SalesTaxAdjType => ProgramPreference.GetLong(GetAvaTaxProgramId(), "Sales Tax Adjustment Type");

        /// <summary>
        /// The adjustment type defnum to be associated with SalesTax return adjustments.
        /// </summary>
        public static long SalesTaxReturnAdjType => ProgramPreference.GetLong(GetAvaTaxProgramId(), "Sales Tax Return Adjustment Type");

        /// <summary>
        /// Required by the API when sending transactions to determine the origination company with which to associate the transaction.
        /// </summary>
        public static string CompanyCode => ProgramPreference.GetString(GetAvaTaxProgramId(), "Company Code");

        /// <summary>
        /// The list of two-letter state codes where sales tax will be collected.
        /// </summary>
        public static List<string> ListTaxableStates => ProgramPreference.GetString(GetAvaTaxProgramId(), "Taxable States").Split(',').ToList();

        /// <summary>
        /// Indicates if we are currently using the sandbox or production API.
        /// </summary>
        public static bool IsProduction => ProgramPreference.GetString(GetAvaTaxProgramId(), "Test (T) or Production (P)") == "P";

        /// <summary>
        /// A program property. Indicates the level of detail to log about calls made to the Avalara API.
        /// </summary>
        public static LogLevel LogDetailLevel => (LogLevel)ProgramPreference.GetLong(GetAvaTaxProgramId(), "Log Level");

        /// <summary>
        /// The list of procedure codes we will allow users to pre-pay for in the Pre-payment tool.
        /// </summary>
        public static List<ProcedureCode> ListPrePayProcCodes
        {
            get
            {
                var procedureCodes = new List<ProcedureCode>();

                foreach (var procedureCode in ProgramPreference.GetString(GetAvaTaxProgramId(), "Prepay Proc Codes").Split(','))
                {
                    procedureCodes.Add(ProcedureCodes.GetProcCode(procedureCode));
                }

                return procedureCodes;
            }
        }

        /// <summary>
        /// The list of procedure codes we will allow users to pre-pay for in the Pre-payment tool.
        /// </summary>
        public static List<ProcedureCode> ListDiscountProcCodes
        {
            get
            {
                var procedureCodes = new List<ProcedureCode>();

                foreach (var procedureCode in ProgramPreference.GetString(GetAvaTaxProgramId(), "Discount Proc Codes").Split(','))
                {
                    procedureCodes.Add(ProcedureCodes.GetProcCode(procedureCode));
                }

                return procedureCodes;
            }
        }

        /// <summary>
        /// Returns the Pat Field Def Num to be associated with marking patients sales tax exempt.
        /// </summary>
        public static PatFieldDef TaxExemptPatField =>
            PatFieldDefs.GetFirstOrDefault(
                patFieldDef => patFieldDef.PatFieldDefNum == ProgramPreference.GetLong(GetAvaTaxProgramId(), "Tax Exempt Pat Field Def"));

        /// <summary>
        /// Returns the date after which we will consider tax adjustments "un-locked" and directly editable.
        /// Example: lock date of 3/31/2019, so locked on that date, and unlocked if on 4/1/2019.
        /// </summary>
        public static DateTime TaxLockDate => ProgramPreference.GetDateTime(GetAvaTaxProgramId(), "Tax Lock Date");

        /// <summary>
        /// The API client based on the current environment and credentials provided by the program properties.
        /// </summary>
        private static AvaTaxClient Client
        {
            get
            {
                return 
                    new AvaTaxClient(
                        "AvaTaxClient", "1.0", 
                        Environment.MachineName, 
                        IsProduction ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                            .WithSecurity(
                                ProgramPreference.GetString(GetAvaTaxProgramId(), "Username"),
                                ProgramPreference.GetString(GetAvaTaxProgramId(), "Password"));
            }
        }

        public AvaTax()
        {
        }

        public static bool IsEnabled() => Program.IsEnabled(typeof(AvaTax).FullName);

        /// <summary>
        /// True if we are in HQ, AvaTax is enabled, we tax the customer's state, and either the
        /// customer's tax exempt field is not defined or they are explicitly not tax exempt.
        /// </summary>
        public static bool IsTaxable(long patientId)
        {
            if (!IsEnabled()) return false;

            var patient = Patients.GetPat(patientId);
            if (patient == null)
            {
                return false;
            }

            PatField taxExemptField = null;
            if (TaxExemptPatField != null)
            {
                taxExemptField = PatFields.Refresh(patientId).FirstOrDefault(x => x.FieldName == TaxExemptPatField.FieldName);
            }

            return 
                ListTaxableStates.Count > 0 && 
                ListTaxableStates.Any(x => x == patient.State) && 
                (taxExemptField == null || !SIn.Bool(taxExemptField.FieldValue));
        }

        /// <summary>
        /// True if we are in HQ, AvaTax is enabled, we tax the customer's state, and the procedure has a taxable proccode.
        /// </summary>
        public static bool CanProcedureBeTaxed(Procedure procedure, bool isSilent = false)
        {
            var procedureCode = ProcedureCodes.GetProcCode(procedure.CodeNum);

            bool isTaxable = IsTaxable(procedure.PatNum) && !string.IsNullOrWhiteSpace(procedureCode.TaxCode) && procedure.ProcFee > 0;//repeat charges for prepay use ProcFee=0
            if (isTaxable && !Patients.HasValidUSZipCode(procedure.PatNum))
            {
                if (isSilent)
                {
                    Logger.Write(LogLevel.Error, 
                        $"Invalid ZipCode for PatNum {procedure.PatNum} while running Repeat Charge Tool on {DateTime.Today}");
                }
                else
                {
                    MessageBox.Show(
                        "A valid zip code is required to process sales tax on procedures in this patient's state. Please update the patient information with a valid zip code before continuing.");
                }
            }
            return isTaxable;
        }

        ///<summary>Checks to see if we need to replace the tax code for this state and procedure code with a different tax code.
        ///Returns the override, or if no override is found, returns the original tax code for the procedure.
        ///If there is a formatting error in the preference value, logs the offending section of the preference string,
        ///and attempts to find any other matching entries.  If no matching entries are found after an error
        ///occurs, throws an exception to prevent sending incorrect Avalara transactions.
        ///Exceptions thrown here will bubble up and be recorded into the adjustment note, so the user can see the error message.</summary>
        public static string GetTaxOverrideIfNeeded(Patient pat, ProcedureCode procCode)
        {
            string overrides = ProgramPreference.GetString(GetAvaTaxProgramId(), "Tax Code Overrides");
            if (string.IsNullOrWhiteSpace(overrides))
            {
                return procCode.TaxCode;
            }
            bool hasFormatError = false;
            string[] arrayOverrides = overrides.Split(',');
            foreach (string entry in arrayOverrides)
            {
                string[] parts = entry.Split('-');
                if (parts.Count() != 3)
                {
                    Logger.Write(LogLevel.Error, "Tax Code Override entry is incorrect: " + entry + ".  "
                        + "Fix this entry in AvaTax setup to resume processing overrides.");
                    hasFormatError = true;
                    continue;
                }
                string stateCode = parts[0].Trim();
                string procCodeCur = parts[1].Trim();
                string taxCodeCur = parts[2].Trim();
                if (stateCode.ToLower() == pat.State.ToLower()//Allowing state code to be case insensitive, since our database data is really inconsistent.
                    && procCodeCur == procCode.ProcCode)//We must use case sensitive proc codes, because OD allows (ex D0120 to exist with d0120).
                {
                    return taxCodeCur;
                }
            }
            if (hasFormatError)
            {
                throw new ODException("Unable to parse tax code overrides due to formatting error.");
            }
            return procCode.TaxCode;
        }

        /// <summary>
        /// Pings the API service with the provided test settings to check if the settings will work and the API is available.
        /// </summary>
        public static bool IsApiAvailable(bool isProduction, string username, string password)
        {
            var avaTaxClient = 
                new AvaTaxClient("TestClient", "1.0", Environment.MachineName, isProduction ? AvaTaxEnvironment.Production : AvaTaxEnvironment.Sandbox)
                    .WithSecurity(username, password);

            return avaTaxClient.Ping().authenticated ?? false;
        }

        /// <summary>
        /// Tests our connection to the API client using the stored credentials. Returns true if we can connect; otherwise, false.
        /// </summary>
        public static bool PingAvaTax() => Client.Ping().authenticated ?? false;

        /// <summary>
        /// Returns the tax estimate for this specific patient, procCode, and feeAmts.
        /// Calls AvaTax/CreateTransaction: https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction/ but does
        /// not save the transaction in the Avalara DB.
        /// </summary>
        public static decimal GetEstimate(long procedureCodeId, long patientId, double procedureFee, bool hasExceptions = false)
        {
            if (!IsTaxable(patientId)) return 0;
            
            var procedureCode = ProcedureCodes.GetProcCode(procedureCodeId);
            try
            {
                TransactionBuilder builder = SetUpTransaction(DocumentType.SalesOrder, patientId);//Sales Order is AvaTax's way of getting an estimate

                builder.WithLine((decimal)procedureFee, 1, GetTaxOverrideIfNeeded(Patients.GetPat(patientId), procedureCode), procedureCode.Descript, procedureCode.ProcCode);

                TransactionModel result = Client.CreateTransaction("Lines", builder.GetCreateTransactionModel());
                return result.totalTax.Value;
            }
            catch (Exception exception)
            {
                Logger.Write(LogLevel.Error, $"Error getting estimate from Avatax for PatNum: {patientId}");
                if (hasExceptions)
                {
                    throw exception;
                }

                // For now we just enter $0 because we don't have any proc or adjustment to attach this to, and we already have logging for errors.
                return 0;
            }
        }

        ///<summary>Returns true if the passed in adjustment needs to be subsequently saved to the database. Returns false otherwise. 
        ///Takes in a procedure and a potential sales tax adjustment and gets the expected tax amount for the procedure. If there is not tax
        ///and the adjustment has not been inserted into the database, then we do not create an adjustment. Otherwise, we save the sax and insert the 
        ///adjustment (even if there is an existing tax, and we are setting adjustment amount to 0). Finally all errors encountered are logged in the
        ///adjustment note and on the machine's local logs.</summary>
        public static bool DidUpdateAdjustment(Procedure proc, Adjustment adj)
        {
            string message = "";
            try
            {
                //Get the new sum of all adjustments attached to the proc, excluding sales tax and including the new adjustment amount if applicable
                double procTotal = proc.ProcFeeTotal + Adjustments.GetTotForProc(proc.ProcNum, canIncludeTax: false);
                decimal taxAmt = GetEstimate(proc.CodeNum, proc.PatNum, procTotal, hasExceptions: true);
                if (taxAmt == 0 && adj.Id == 0)
                { //We could be modifying an existing adjustment, in which case we would want to set the 0 value
                    return false;
                }
                adj.AdjAmt = (double)taxAmt;
                return true;
            }
            catch (AvaTaxError at)
            {
                Logger.Write(LogLevel.Error, "Encountered an Avatax error: " + JsonConvert.SerializeObject(at.error.error));
                message = at.error.error.message;
            }
            catch (Exception ex)
            {
                Logger.Write(LogLevel.Error, "Unable to send or receive transaction: " + JsonConvert.SerializeObject(ex));
                message = ex.Message;
            }
            adj.AdjNote = AddNote(adj.AdjNote, "An error occurred processing the transacton: " + message + ".  See local logs for more details.");
            adj.AdjAmt = 0;
            return true;
        }

        public static bool DoCreateReturnAdjustment(Procedure procedure, Adjustment lockedAdj, Adjustment returnAdj)
        {
            try
            {
                double procTotal = procedure.ProcFeeTotal + Adjustments.GetTotForProc(procedure.ProcNum, canIncludeTax: false);
                decimal taxEstNew = AvaTax.GetEstimate(procedure.CodeNum, procedure.PatNum, procTotal, hasExceptions: true);
                returnAdj.AdjAmt = (Adjustments.GetTotTaxForProc(procedure) - (double)taxEstNew) * (-1);
                if (returnAdj.AdjAmt == 0)
                {
                    return false; //no error and we would be refunding $0 to the customer, so no need to create a return adjustment
                }
            }
            catch (Exception e)
            {
                returnAdj.AdjNote = AvaTax.AddNote(returnAdj.AdjNote, "An error occurred processing the transacton: " + e.Message + ".  See local logs for more details.");
            }
            return true;
        }

        ///<summary>Creates a bulk estimate currently specifically for the pre-payment tool.  Takes in a special dictionary of procedurecodes mapped
        ///to a list of tuples containing the qty and single item prices for the provided procedure codes, then builds a single transaction to
        ///hold the data.  This method creates its own procedures and adjustments.</summary>
        public static void CreatePrepaymentTransaction(Dictionary<ProcedureCode, List<TransQtyAmt>> dictProcCodes, Patient patCur, List<Procedure> listCompletedProcs)
        {
            Dictionary<Procedure, int> dictProcsToCreate = new Dictionary<Procedure, int>();
            //Build the transaction for all of the items in a single transaction.
            TransactionBuilder builder = SetUpTransaction(DocumentType.SalesOrder, patCur.PatNum);//Sales Order is AvaTax's way of getting an estimate
            foreach (ProcedureCode procCode in dictProcCodes.Keys)
            {
                Procedure procCur = new Procedure()
                {
                    PatNum = patCur.PatNum,
                    ProvNum = 7,//HQ only this is jordan's provNum.
                    ProcDate = DateTime.Today,
                    DateEntryC = DateTime.Now,
                    DateComplete = DateTime.Now,
                    CodeNum = procCode.CodeNum,
                    ProcStatus = ProcStat.C
                };
                int totalCount = 0;
                foreach (TransQtyAmt pair in dictProcCodes[procCode])
                {
                    totalCount += pair.qty;
                    procCur.BillingNote += "Rate: $" + POut.Double(pair.rate) + " Months: " + POut.Int(pair.qty) + "\r\n";
                    builder.WithLine(pair.qty * (decimal)pair.rate, (decimal)pair.qty,
                        GetTaxOverrideIfNeeded(patCur, procCode), procCode.Descript, procCode.ProcCode);
                }
                //Add note for what day the customer has prepaid through.
                DateTime datePrepaidThrough = DateTimeOD.GetMostRecentValidDate(DateTime.Today.Year, DateTime.Today.Month, patCur.BillingCycleDay)
                    .AddMonths(totalCount).AddDays(-1);
                if (DateTimeOD.Today.Day >= patCur.BillingCycleDay)
                {
                    datePrepaidThrough = datePrepaidThrough.AddMonths(1);
                }
                procCur.BillingNote += $"Prepaid through: {datePrepaidThrough.Date:MM/dd/yyyy}";
                dictProcsToCreate.Add(procCur, totalCount);
            }
            TransactionModel result = Client.CreateTransaction("Lines", builder.GetCreateTransactionModel());
            List<ProcedureCode> listDiscountCodes = AvaTax.ListDiscountProcCodes;
            //Create a single procedure and adjustment for each procCode.
            foreach (KeyValuePair<Procedure, int> entry in dictProcsToCreate)
            {
                //look for previously completed procedures that need to be included on the adjustment calculation.
                List<Procedure> listMatchingCompletedProcs = listCompletedProcs.FindAll(x => x.CodeNum == entry.Key.CodeNum);
                Procedure procCur = entry.Key;
                int count = entry.Value + listMatchingCompletedProcs.Count();
                List<TransactionLineModel> listLines = result.lines.FindAll(x => x.itemCode.Equals(ProcedureCodes.GetProcCode(procCur.CodeNum).ProcCode));
                procCur.ProcFee = (double)listLines.Sum(x => x.lineAmount.Value);
                procCur.TaxAmt = (double)listLines.Sum(x => x.tax.Value);
                procCur.ProcNum = Procedures.Insert(procCur, doCalcTax: false);
                if (count > 5 && listDiscountCodes.Exists(x => x.ProcCode == ProcedureCodes.GetProcCode(procCur.CodeNum).ProcCode))
                {
                    //Create a discount adjustment.
                    if (count >= 6 && count <= 11)
                    {
                        CreateDiscountAdjustment(procCur, .05, 255);//5% discount.  Hard coded ODHQ defnum.
                    }
                    else if (count >= 12 && count <= 23)
                    {
                        CreateDiscountAdjustment(procCur, .10, 206);//10% discount.  Hard coded ODHQ defnum.
                    }
                    else if (count >= 24)
                    {
                        CreateDiscountAdjustment(procCur, .15, 229);//15% discount.  Hard coded ODHQ defnum.
                    }
                    //Create adjustments for the previously completed procedures.
                    foreach (Procedure proc in listMatchingCompletedProcs)
                    {
                        if (count >= 6 && count <= 11)
                        {
                            CreateDiscountAdjustment(proc, .05, 255);//5% discount.  Hard coded ODHQ defnum.
                        }
                        else if (count >= 12 && count <= 23)
                        {
                            CreateDiscountAdjustment(proc, .10, 206);//10% discount.  Hard coded ODHQ defnum.
                        }
                        else if (count >= 24)
                        {
                            CreateDiscountAdjustment(proc, .15, 229);//15% discount.  Hard coded ODHQ defnum.
                        }
                    }
                }
            }
        }


        #region Helpers

        ///<summary>If there is already text in the previous note, adds a newline before adding the next note.</summary>
        private static string AddNote(string prevNote, string nextNote)
        {
            if (!string.IsNullOrEmpty(prevNote))
            {
                prevNote += Environment.NewLine;
            }
            prevNote += DateTime.Now + " " + nextNote;
            return prevNote;
        }

        ///<summary>Sets up a transaction builder to which we can attach line items later, using the base office and patient address info.
        ///Also specifies the transaction DocumentType: https://developer.avalara.com/api-reference/avatax/rest/v2/models/enums/DocumentType/. </summary>
        private static TransactionBuilder SetUpTransaction(DocumentType docType, long patNum)
        {
            Patient patient = Patients.GetPat(patNum);
            TransactionBuilder builder = new TransactionBuilder(
                Client,//AvaTaxClient
                AvaTax.CompanyCode,
                docType,
                POut.Long(patient.PatNum));//We will use the customer's PatNum as the "Customer Code"
                                           //Add main office address.  In the future, if we make this available to customers, then we might need to implement clinics here.
            builder.WithAddress(
                TransactionAddressType.ShipFrom,
                Preference.GetString(PreferenceName.PracticeAddress),
                Preference.GetString(PreferenceName.PracticeAddress2),
                "",
                Preference.GetString(PreferenceName.PracticeCity),
                Preference.GetString(PreferenceName.PracticeST),
                Preference.GetString(PreferenceName.PracticeZip),
                "US"//US only.  Otherwise, in future we might use System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName
            );
            //Add customer address
            builder.WithAddress(
                TransactionAddressType.ShipTo,
                patient.Address,
                patient.Address2,
                "",
                patient.City,
                patient.State,
                patient.Zip,
                "US"//US only.  Otherwise, in future we might use patient.Country
            );
            //Record the user who was logged in when this transaction occurred for later reference
            builder.WithSalespersonCode(Security.CurrentUser.UserName);
            return builder;
        }

        public class TransQtyAmt : Tuple<int, double>
        {
            public int qty { get { return Item1; } }
            public double rate { get { return Item2; } }
            public TransQtyAmt(int qty, double rate) : base(qty, rate) { }
        }

        ///<summary>Creates and inserts a discount adjustment for the passed in procedure.  Used by prepayment tool.</summary>
        public static void CreateDiscountAdjustment(Procedure proc, double discountPercentage, long adjType)
        {
            Adjustment adj = new Adjustment();
            adj.DateEntry = DateTime.Today;
            adj.AdjDate = DateTime.Today;
            adj.ProcDate = proc.ProcDate;
            adj.ProvNum = proc.ProvNum;
            adj.ProcNum = proc.ProcNum;
            adj.ClinicNum = proc.ClinicNum;
            adj.PatNum = proc.PatNum;
            adj.AdjType = adjType;
            adj.AdjAmt = -(proc.ProcFee * discountPercentage);//Flip the sign to make it a negative adjustment.
            Adjustments.Insert(adj);
            Patient patCur = Patients.GetPat(proc.PatNum);
            TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adj, patCur.Guarantor, patCur.ClinicNum);
        }

        #endregion Helpers
    }
}