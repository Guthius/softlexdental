using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;
using Avalara.AvaTax.RestClient;

namespace OpenDentBusiness
{
    public class Adjustments
    {
        /// <summary>
        /// Gets all adjustments for a single patient.
        /// </summary>
        public static Adjustment[] Refresh(long patNum)
        {
            return Crud.AdjustmentCrud.SelectMany("SELECT * FROM adjustment WHERE PatNum = " + POut.Long(patNum) + " ORDER BY AdjDate").ToArray();
        }

        /// <summary>
        /// Gets one adjustment from the db.
        /// </summary>
        public static Adjustment GetOne(long adjNum) => Crud.AdjustmentCrud.SelectOne(adjNum);

        /// <summary>
        /// Gets the amount used for the specified adjustment (Sums paysplits that have AdjNum passed in).
        /// Pass in PayNum to exclude splits on that payment.
        /// </summary>
        public static double GetAmtAllocated(long adjNum, long excludedPayNum, List<PaySplit> listSplits = null)
        {
            if (listSplits != null)
            {
                return listSplits.FindAll(x => x.PayNum != excludedPayNum).Sum(x => x.SplitAmt);
            }
            else
            {
                string command = "SELECT SUM(SplitAmt) FROM paysplit WHERE AdjNum=" + POut.Long(adjNum);
                if (excludedPayNum != 0)
                {
                    command += " AND PayNum!=" + POut.Long(excludedPayNum);
                }
                return PIn.Double(Db.GetScalar(command));
            }
        }

        public static void DetachFromInvoice(long statementNum)
        {
            Db.NonQ("UPDATE adjustment SET StatementNum=0 WHERE StatementNum=" + POut.Long(statementNum));
        }

        public static void DetachAllFromInvoices(List<long> listStatementNums)
        {
            if (listStatementNums == null || listStatementNums.Count == 0)
            {
                return;
            }
            Db.NonQ("UPDATE adjustment SET StatementNum=0 WHERE StatementNum IN (" + string.Join(",", listStatementNums.Select(x => POut.Long(x))) + ")");
        }

        /// <summary>
        /// Gets all negative or positive adjustments for a patient depending on how isPositive is set.
        /// </summary>
        public static List<Adjustment> GetAdjustForPats(List<long> listPatNums)
        {
            return Crud.AdjustmentCrud.SelectMany("SELECT * FROM adjustment WHERE PatNum IN(" + String.Join(", ", listPatNums) + ")");
        }

        public static void Update(Adjustment adj)
        {
            Crud.AdjustmentCrud.Update(adj);
            CreateOrUpdateSalesTaxIfNeeded(adj);
        }

        public static long Insert(Adjustment adj)
        {
            adj.SecUserNumEntry = Security.CurrentUser.Id;
            long adjNum = Crud.AdjustmentCrud.Insert(adj);
            CreateOrUpdateSalesTaxIfNeeded(adj); // Do the update after the insert so the AvaTax API can include the new adjustment in the calculation
            return adjNum;
        }

        /// <summary>
        /// This will soon be eliminated or changed to only allow deleting on same day as EntryDate.
        /// </summary>
        public static void Delete(Adjustment adj)
        {
            Crud.AdjustmentCrud.Delete(adj.Id);
            CreateOrUpdateSalesTaxIfNeeded(adj);
            PaySplits.UnlinkForAdjust(adj);
        }

        /// <summary>
        /// Loops through the supplied list of adjustments and returns an ArrayList of adjustments for the given proc.
        /// </summary>
        public static ArrayList GetForProc(long procNum, Adjustment[] List)
        {
            ArrayList retVal = new ArrayList();
            for (int i = 0; i < List.Length; i++)
            {
                if (List[i].ProcNum == procNum)
                {
                    retVal.Add(List[i]);
                }
            }
            return retVal;
        }

        public static List<Adjustment> GetForProcs(List<long> listProcNums)
        {
            List<Adjustment> listAdjustments = new List<Adjustment>();
            if (listProcNums == null || listProcNums.Count < 1)
            {
                return listAdjustments;
            }
            return Crud.AdjustmentCrud.SelectMany("SELECT * FROM adjustment WHERE ProcNum IN(" + string.Join(",", listProcNums) + ")");
        }

        /// <summary>
        /// Returns the sales tax adjustment attached to this procedure with a TaxTransID. 
        /// We do not use the AvaTax.SalesTaxAdjType in case the defnum has changed in the past.
        /// </summary>
        public static Adjustment GetSalesTaxForProc(long procNum)
        {
            string command = "SELECT * FROM adjustment WHERE ProcNum=" + POut.Long(procNum) + " AND AdjType=" + POut.Long(AvaTax.SalesTaxAdjType);
            return Crud.AdjustmentCrud.SelectMany(command).FirstOrDefault(x => x.AdjType == AvaTax.SalesTaxAdjType);
        }

        /// <summary>
        /// Sums all adjustments for a proc then returns that sum.
        /// Pass false to canIncludeTax in order to exclude sales tax from the end amount.
        /// </summary>
        public static double GetTotForProc(long procNum, bool canIncludeTax = true)
        {
            string command = "SELECT SUM(AdjAmt) FROM adjustment WHERE ProcNum=" + POut.Long(procNum);
            if (AvaTax.IsEnabled() && !canIncludeTax)
            {
                command += " AND AdjType NOT IN (" + string.Join(",", POut.Long(AvaTax.SalesTaxAdjType), POut.Long(AvaTax.SalesTaxReturnAdjType)) + ")";
            }
            return PIn.Double(Db.GetScalar(command));
        }

        public static double GetTotTaxForProc(Procedure proc)
        {
            if (!AvaTax.CanProcedureBeTaxed(proc))
            {
                return 0;
            }

            string command = 
                "SELECT SUM(AdjAmt) FROM adjustment " +
                "WHERE ProcNum=" + POut.Long(proc.ProcNum) + " " +
                "AND AdjType IN (" + string.Join(",", POut.Long(AvaTax.SalesTaxAdjType), POut.Long(AvaTax.SalesTaxReturnAdjType)) + ")";

            return PIn.Double(Db.GetScalar(command));
        }

        /// <summary>
        /// Creates a new discount adjustment for the given procedure.
        /// </summary>
        public static void CreateAdjustmentForDiscount(Procedure procedure)
        {
            //No need to check RemotingRole; no call to db.
            Adjustment AdjustmentCur = new Adjustment();
            AdjustmentCur.DateEntry = DateTime.Today;
            AdjustmentCur.AdjDate = DateTime.Today;
            AdjustmentCur.ProcDate = procedure.ProcDate;
            AdjustmentCur.ProvNum = procedure.ProvNum;
            AdjustmentCur.PatNum = procedure.PatNum;
            AdjustmentCur.AdjType = Preference.GetLong(PreferenceName.TreatPlanDiscountAdjustmentType);
            AdjustmentCur.ClinicNum = procedure.ClinicNum;
            AdjustmentCur.AdjAmt = -procedure.Discount; // Discount must be negative here.
            AdjustmentCur.ProcNum = procedure.ProcNum;
            Insert(AdjustmentCur);

            Patient pat = Patients.GetPat(procedure.PatNum);
            TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(AdjustmentCur, pat.Guarantor, pat.ClinicNum);
        }

        /// <summary>
        /// Creates a new discount adjustment for the given procedure using the discount plan fee.
        /// </summary>
        public static void CreateAdjustmentForDiscountPlan(Procedure procedure)
        {
            DiscountPlan discountPlan = DiscountPlans.GetPlan(Patients.GetPat(procedure.PatNum).DiscountPlanNum);
            if (discountPlan == null)
            {
                return; //No discount plan.
            }

            // Figure out how much the patient saved and make an adjustment for the difference so that the office find how much money they wrote off.
            double discountAmt = Fees.GetAmount(procedure.CodeNum, discountPlan.FeeSchedNum, procedure.ClinicNum, procedure.ProvNum);
            if (discountAmt == -1)
            {
                return; // No fee entered, don't make adjustment.
            }

            double adjAmt = procedure.ProcFee - discountAmt;
            if (adjAmt <= 0)
            {
                return; // We do not need to create adjustments for 0 dollars.
            }

            Adjustment adjustmentCur = new Adjustment();
            adjustmentCur.DateEntry = DateTime.Today;
            adjustmentCur.AdjDate = DateTime.Today;
            adjustmentCur.ProcDate = procedure.ProcDate;
            adjustmentCur.ProvNum = procedure.ProvNum;
            adjustmentCur.PatNum = procedure.PatNum;
            adjustmentCur.AdjType = discountPlan.DefNum;
            adjustmentCur.ClinicNum = procedure.ClinicNum;
            adjustmentCur.AdjAmt = (-adjAmt);
            adjustmentCur.ProcNum = procedure.ProcNum;
            Insert(adjustmentCur);

            Patient pat = Patients.GetPat(procedure.PatNum);
            TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustmentCur, pat.Guarantor, pat.ClinicNum);
            SecurityLog.Write(procedure.PatNum, SecurityLogEvents.AdjustmentCreated, "Adjustment made for discount plan: " + adjustmentCur.AdjAmt.ToString("f"));
        }

        /// <summary>
        /// (HQ Only) Automatically creates or updates a sales tax adjustment for the passted in procedure. 
        /// If an adjustment is passed in, we go  ahead and update that adjustment, otherwise we check if there is already a sales 
        /// tax adjustment for the given procedure and if not, we create a new one. Pass in false to doCalcTax if we have already 
        /// called the AvaTax API to get the tax estimate recently to avoid redundant calls (currently only pre-payments uses this flag). 
        /// isRepeatCharge indicates if the adjustment is being inserted by the repeat charge tool, currently only used to supress error 
        /// messages in the Avatax API.
        /// </summary>
        public static void CreateOrUpdateSalesTaxIfNeeded(Procedure procedure, Adjustment salesTaxAdj = null, bool doCalcTax = true, bool isRepeatCharge = false)
        {
            if (!AvaTax.CanProcedureBeTaxed(procedure, isRepeatCharge)) // Tests isHQ
            { 
                return;
            }

            if (salesTaxAdj == null)
            {
                salesTaxAdj = Adjustments.GetSalesTaxForProc(procedure.ProcNum);
            }

            // If we didn't find any existing adjustments to modify, create an adjustment instead
            if (salesTaxAdj == null)
            {
                salesTaxAdj = new Adjustment();
                salesTaxAdj.DateEntry = DateTime.Today;
                salesTaxAdj.AdjDate = procedure.ProcDate;
                salesTaxAdj.ProcDate = procedure.ProcDate;
                salesTaxAdj.ProvNum = procedure.ProvNum;
                salesTaxAdj.PatNum = procedure.PatNum;
                salesTaxAdj.AdjType = AvaTax.SalesTaxAdjType;
                salesTaxAdj.ClinicNum = procedure.ClinicNum;
                salesTaxAdj.ProcNum = procedure.ProcNum;
            }

            // If the sales tax adjustment is locked, create a sales tax refund adjustment instead
            if (procedure.ProcDate <= AvaTax.TaxLockDate)
            {
                CreateSalesTaxRefundIfNeeded(procedure, salesTaxAdj);
                return;
            }

            if (!doCalcTax) // Should only ever happen for pre-payments, where we've already called the api to get the tax amount
            { 
                salesTaxAdj.AdjAmt = procedure.TaxAmt;
                Adjustments.Insert(salesTaxAdj);
            }
            else if (AvaTax.DidUpdateAdjustment(procedure, salesTaxAdj))
            {
                string note = $"{DateTime.Now}: Tax amount changed from ${procedure.TaxAmt} to ${salesTaxAdj.AdjAmt}";
                if (!(procedure.TaxAmt - salesTaxAdj.AdjAmt).IsZero())
                {
                    procedure.TaxAmt = salesTaxAdj.AdjAmt;
                    Crud.ProcedureCrud.Update(procedure);
                }
                if (salesTaxAdj.Id == 0)
                {
                    // The only way to get salesTaxAdj.AdjAmt=0 when AvaTax.DidUpdateAdjustment() returns true is if there was an error.
                    if (isRepeatCharge && salesTaxAdj.AdjAmt == 0)
                    { 
                        //This is an error; we would normally not save a new adjustment with amt $0
                        throw new ODException("Encountered an error communicating with AvaTax.  Skip for repeating charges only.  " + salesTaxAdj.AdjNote);
                    }
                    Adjustments.Insert(salesTaxAdj);// This could be an error or a new adjustment/repeating charge, either way we want to insert
                }
                else
                { 
                    // Updating an existing adjustment. We don't need to check isRepeatCharge because of 
                    if (!string.IsNullOrWhiteSpace(salesTaxAdj.AdjNote))
                    {
                        salesTaxAdj.AdjNote += Environment.NewLine;
                    }
                    salesTaxAdj.AdjNote += note; // If we are updating this adjustment, leave a note indicating what changed
                    Adjustments.Update(salesTaxAdj);
                }
            }
            Patient patCur = Patients.GetPat(procedure.PatNum);
            TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(salesTaxAdj, patCur.Guarantor, patCur.ClinicNum);
        }

        public static void CreateSalesTaxRefundIfNeeded(Procedure procedure, Adjustment adjustmentExistingLocked)
        {
            Adjustment adjustmentSalesTaxReturn = new Adjustment();
            adjustmentSalesTaxReturn.DateEntry = DateTime.Today;
            adjustmentSalesTaxReturn.AdjDate = DateTime.Today;
            adjustmentSalesTaxReturn.ProcDate = procedure.ProcDate;
            adjustmentSalesTaxReturn.ProvNum = procedure.ProvNum;
            adjustmentSalesTaxReturn.PatNum = procedure.PatNum;
            adjustmentSalesTaxReturn.AdjType = AvaTax.SalesTaxReturnAdjType;
            adjustmentSalesTaxReturn.ClinicNum = procedure.ClinicNum;
            adjustmentSalesTaxReturn.ProcNum = procedure.ProcNum;
            if (AvaTax.DoCreateReturnAdjustment(procedure, adjustmentExistingLocked, adjustmentSalesTaxReturn))
            {
                Insert(adjustmentSalesTaxReturn);

                Patient patCur = Patients.GetPat(procedure.PatNum);
                TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustmentSalesTaxReturn, patCur.Guarantor, patCur.ClinicNum);
            }
        }

        /// <summary>
        /// (HQ Only) When we create, modify, or delete a non-sales tax adjustment that is attached to a procedure, 
        /// we may also need to update sales tax for that procedure. Takes a non-sales tax adjustment and checks if 
        /// the procedure already has a sales tax adjustment. If one already exists, calculate the new tax after the 
        /// change to the passed in adjustment, and update the sales tax accordingly. If not, calculate and  create a 
        /// sales tax adjustment only if the procedure is taxable.
        /// </summary>
        public static void CreateOrUpdateSalesTaxIfNeeded(Adjustment modifiedAdj)
        {
            if (AvaTax.IsEnabled() && modifiedAdj.ProcNum > 0 && modifiedAdj.AdjType != AvaTax.SalesTaxAdjType && modifiedAdj.AdjType != AvaTax.SalesTaxReturnAdjType)
            {
                Adjustment taxAdjForProc = GetSalesTaxForProc(modifiedAdj.ProcNum);
                if (taxAdjForProc != null)
                {
                    Procedure proc = Procedures.GetOneProc(modifiedAdj.ProcNum, false);
                    CreateOrUpdateSalesTaxIfNeeded(proc, taxAdjForProc);
                }
            }
        }

        /// <summary>
        /// Deletes all adjustments for a procedure
        /// </summary>
        public static void DeleteForProcedure(long procNum)
        {
            // Create log for each adjustment that is going to be deleted.
            List<Adjustment> listAdjustments = Crud.AdjustmentCrud.SelectMany("SELECT * FROM adjustment WHERE ProcNum = " + procNum);
            for (int i = 0; i < listAdjustments.Count; i++)
            {
                SecurityLog.Write(
                    listAdjustments[i].PatNum,
                    SecurityLogEvents.AdjustmentEdited, 
                    "Delete adjustment for patient: "
                    + Patients.GetLim(listAdjustments[i].PatNum).GetNameLF() + ", "
                    + listAdjustments[i].AdjAmt.ToString("c"), null, listAdjustments[i].SecDateTEdit);
            }

            // Delete each adjustment for the procedure.
            Db.NonQ("DELETE FROM adjustment WHERE ProcNum = " + POut.Long(procNum));
        }

        /// <summary>
        /// Returns a DataTable of adjustments of a given adjustment type and for a given pat
        /// </summary>
        public static List<Adjustment> GetAdjustForPatByType(long patNum, long adjType)
        {
            string queryBrokenApts = "SELECT * FROM adjustment WHERE PatNum=" + POut.Long(patNum) + " AND AdjType=" + POut.Long(adjType);
            return Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
        }

        /// <summary>
        /// Returns a dictionary of adjustments of a given adjustment type and for the given pats such 
        /// that the key is the patNum. Every patNum given will exist as key with a list in the returned 
        /// dictonary. Only considers adjs where AdjDate is strictly less than the given maxAdjDate.
        /// </summary>
        public static Dictionary<long, List<Adjustment>> GetAdjustForPatsByType(List<long> listPatNums, long adjType, DateTime maxAdjDate)
        {
            if (listPatNums == null || listPatNums.Count == 0)
            {
                return new Dictionary<long, List<Adjustment>>();
            }

            string queryBrokenApts = 
                "SELECT * FROM adjustment " +
                "WHERE PatNum IN (" + string.Join(",", listPatNums) + ") " +
                "AND AdjType=" + POut.Long(adjType) + " " +
                "AND " + DbHelper.DateTConditionColumn("AdjDate", ConditionOperator.LessThan, maxAdjDate);

            List<Adjustment> listAdjs = Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
            Dictionary<long, List<Adjustment>> retVal = new Dictionary<long, List<Adjustment>>();
            foreach (long patNum in listPatNums)
            {
                retVal[patNum] = listAdjs.FindAll(x => x.PatNum == patNum);
            }
            return retVal;
        }

        /// <summary>
        /// Used from ContrAccount and ProcEdit to display and calculate adjustments attached to procs.
        /// </summary>
        public static double GetTotForProc(long procNum, Adjustment[] List, long excludedNum = 0)
        {
            double retVal = 0;
            for (int i = 0; i < List.Length; i++)
            {
                if ((List[i].Id == excludedNum))
                {
                    continue;
                }
                if (List[i].ProcNum == procNum)
                {
                    retVal += List[i].AdjAmt;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Returns the number of finance or billing charges deleted.
        /// </summary>
        public static long UndoFinanceOrBillingCharges(DateTime dateUndo, bool isBillingCharges)
        {
            string adjTypeStr = "Finance";
            long adjTypeDefNum = Preference.GetLong(PreferenceName.FinanceChargeAdjustmentType);
            if (isBillingCharges)
            {
                adjTypeStr = "Billing";
                adjTypeDefNum = Preference.GetLong(PreferenceName.BillingChargeAdjustmentType);
            }

            string command = 
                "SELECT adjustment.AdjAmt,patient.PatNum,patient.Guarantor,patient.LName,patient.FName,patient.Preferred,patient.MiddleI,adjustment.SecDateTEdit " +
                "FROM adjustment " +
                "INNER JOIN patient ON patient.PatNum=adjustment.PatNum " +
                "WHERE AdjDate=" + POut.Date(dateUndo) + " " +
                "AND AdjType=" + POut.Long(adjTypeDefNum);

            DataTable table = Db.GetTable(command);
            List<Action> listActions = new List<Action>();
            int loopCount = 0;
            foreach (DataRow row in table.Rows) // Loops through the rows and creates audit trail entry for every row to be deleted
            {
                listActions.Add(new Action(() =>
                {
                    SecurityLog.Write(PIn.Long(row["PatNum"].ToString()),
                        SecurityLogEvents.AdjustmentEdited, 
                        "Delete adjustment for patient, undo " + adjTypeStr.ToLower() + " charges: "
                        + Patients.GetNameLF(row["LName"].ToString(), row["FName"].ToString(), row["Preferred"].ToString(), row["MiddleI"].ToString())
                        + ", " + PIn.Double(row["AdjAmt"].ToString()).ToString("c"), null, PIn.DateT(row["SecDateTEdit"].ToString()));

                    if (++loopCount % 5 == 0)
                    {
                        BillingEvent.Fire(
                            ODEventType.Billing, 
                            "Creating log entries for " + adjTypeStr.ToLower() + " charges: " + loopCount + " out of " + table.Rows.Count);
                    }
                }));
            }

            ODThread.RunParallel(listActions, TimeSpan.FromMinutes(2));

            BillingEvent.Fire(
                ODEventType.Billing, 
                "Deleting" + " " + table.Rows.Count + " " +  adjTypeStr.ToLower() + " charge adjustments...");

            return Db.NonQ("DELETE FROM adjustment WHERE AdjDate=" + POut.Date(dateUndo) + " AND AdjType=" + POut.Long(adjTypeDefNum));
        }

        /// <summary>
        /// Returns the sum of adjustments for the date range for the passed in operatories or providers.
        /// </summary>
        public static decimal GetAdjustAmtForAptView(DateTime dateStart, DateTime dateEnd, long clinicNum, List<long> listOpNums, List<long> listProvNums)
        {
            string command = GetQueryAdjustmentsForAppointments(dateStart, dateEnd, listOpNums, doGetSum: true);
            if (!listProvNums.IsNullOrEmpty())
            {
                command += "AND adjustment.ProvNum IN(" + string.Join(",", listProvNums) + ") ";
            }
            if (clinicNum > 0)
            {
                command += "AND adjustment.ClinicNum=" + clinicNum;
            }
            return PIn.Decimal(Db.GetScalar(command));
        }

        /// <summary>
        /// Returns a query string used to get adjustments for all patients who have an appointment in 
        /// the date range and in one of the operatories passed in.
        /// </summary>
        public static string GetQueryAdjustmentsForAppointments(DateTime dateStart, DateTime dateEnd, List<long> listOpNums, bool doGetSum)
        {
            if (listOpNums.IsNullOrEmpty())
            {
                return 
                    "SELECT " + (doGetSum ? "SUM(adjustment.AdjAmt)" : "*") + " " +
                    "FROM adjustment " +
                    "WHERE AdjDate BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd) + " ";
            }

            string command = "SELECT "
                + (doGetSum ? "SUM(adjustment.AdjAmt)" : "*")
                + " FROM adjustment WHERE AdjDate BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd)
                    + " AND PatNum IN("
                        + "SELECT PatNum FROM appointment "
                            + "WHERE AptDateTime BETWEEN " + POut.Date(dateStart) + " AND " + POut.Date(dateEnd.AddDays(1))
                            + "AND AptStatus IN (" + POut.Int((int)ApptStatus.Scheduled)
                            + ", " + POut.Int((int)ApptStatus.Complete)
                            + ", " + POut.Int((int)ApptStatus.Broken)
                            + ", " + POut.Int((int)ApptStatus.PtNote)
                            + ", " + POut.Int((int)ApptStatus.PtNoteCompleted) + ")"
                            + " AND Op IN(" + string.Join(",", listOpNums) + ")) ";
            return command;
        }
    }
}