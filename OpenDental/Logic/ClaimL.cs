﻿using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public class ClaimL
    {
        ///<summary>Validates that default clearinghouses are set up correctly.
        ///Shows an error message to the user and returns false if they are not set up; Otherwise, returns true.</summary>
        public static bool CheckClearinghouseDefaults()
        {
            string errorMessage = Clearinghouses.CheckClearinghouseDefaults();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     <para>
        ///         Adds the message passed in to the ErrorMessage of the createClaimDataWrapper 
        ///         passed in. Shows the message passed in to the user if isVerbose is true. Be 
        ///         sure to translate message before calling this function.
        ///     </para>
        /// </summary>
        private static void LogClaimError(CreateClaimDataWrapper createClaimDataWrapper, string message, bool isVerbose)
        {
            if (createClaimDataWrapper.ErrorMessage != "")
            {
                createClaimDataWrapper.ErrorMessage += "\r\n";
            }

            createClaimDataWrapper.ErrorMessage += message;
            createClaimDataWrapper.HasError = true;

            if (isVerbose) MessageBox.Show(message, "Claims", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        ///<summary>Gets a list of CreateClaimItems from the current table and selected items.
        ///If no selections are made, the entire table is converted into CreateClaimItems and is returned.</summary>
        ///<param name="table">This table must have three columns at minimum: ProcNum, ProcNumLab, and chargesDouble</param>
        ///<param name="arraySelectedIndices">Any selected rows in the corresponding table.  An empty array is acceptable.</param>
        ///<returns>List of objects that can be used for creating claims instead of directly utilizing DataTable\ODGrid objects directly.</returns>
        public static List<CreateClaimItem> GetCreateClaimItems(DataTable table, int[] arraySelectedIndices)
        {
            List<DataRow> listRows = table.Select().ToList();
            //If the user manually selected items in the grid then we want to only include the selected rows.
            bool hasSelections = (arraySelectedIndices.Length > 0);
            if (hasSelections)
            {
                listRows = listRows.Where((x, index) => index.In(arraySelectedIndices)).ToList();
            }
            //Create CreateClaimItem objects from the data rows so that we have deep copied and strongly typed objects to work with in other methods.
            return listRows.Select(x => new CreateClaimItem()
            {
                ProcNum = PIn.Long(x["ProcNum"].ToString()),
                ProcNumLab = PIn.Long(x["ProcNumLab"].ToString()),
                ChargesDouble = PIn.Double(x["chargesDouble"].ToString()),
                IsSelected = hasSelections,
            }).ToList();
        }

        ///<summary>Returns a CreateClaimDataWrapper object that is specifically designed for the claim creation process from within the UI.
        ///It contains strongly typed variables which help indicate to the claim creation method how to correctly create the claim.
        ///It also contains variables that indicate to consuming methods what happened during the claim creation process.
        ///It is a requirement to have listCreateClaimItems filled with at least one item.
        ///Optionally set isSelectionRequired to true if the user must have already selected indices within the grid passed in.
        ///This method throws exceptions (specifcally for developers), shows messages, and other UI related actions during claim creation.</summary>
        public static CreateClaimDataWrapper GetCreateClaimDataWrapper(Patient patient, Family family, List<CreateClaimItem> listCreateClaimItems, bool isVerbose, bool isSelectionRequired = false)
        {
            var createClaimDataWrapper = new CreateClaimDataWrapper
            {
                ListCreateClaimItems = listCreateClaimItems ?? throw new ArgumentException("Invalid argument passed in.", nameof(listCreateClaimItems))
            };

            if (!Security.IsAuthorized(Permissions.ClaimView, true))
            {
                LogClaimError(createClaimDataWrapper, "Not authorized for\r\n" + UserGroupPermission.GetDescription(Permissions.ClaimView), isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }

            if (listCreateClaimItems.Count < 1)
            {
                //There is nothing to do because at least one item is required in order to create a claim...
                LogClaimError(createClaimDataWrapper, "Please select procedures first.", isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }

            if (!CheckClearinghouseDefaults())
            {
                createClaimDataWrapper.DoRefresh = false;
                createClaimDataWrapper.HasError = true;
                return createClaimDataWrapper;
            }
            createClaimDataWrapper.Pat = patient;
            createClaimDataWrapper.Fam = family;
            createClaimDataWrapper.ClaimData = AccountModules.GetCreateClaimData(patient, family);
            if (createClaimDataWrapper.ClaimData.ListPatPlans.Count == 0)
            {
                LogClaimError(createClaimDataWrapper, "Patient does not have insurance.", isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }

            int countSelected = 0;
            InsSub sub;
            if (listCreateClaimItems.All(x => !x.IsSelected))
            {
                if (isSelectionRequired)
                {
                    LogClaimError(createClaimDataWrapper, "Please select procedures first.", isVerbose);
                    createClaimDataWrapper.DoRefresh = false;
                    return createClaimDataWrapper;
                }
                //autoselect procedures
                foreach (CreateClaimItem item in listCreateClaimItems)
                {
                    if (item.ProcNum == 0)
                    {
                        continue;//ignore non-procedures
                    }
                    if (item.ChargesDouble == 0)
                    {
                        continue;//ignore zero fee procedures, but user can explicitly select them
                    }
                    //payment rows skipped
                    Procedure proc = Procedures.GetProcFromList(createClaimDataWrapper.ClaimData.ListProcs, item.ProcNum);
                    ProcedureCode procCode = ProcedureCodes.GetFirstOrDefault(x => x.CodeNum == proc.CodeNum) ?? new ProcedureCode();
                    if (procCode.IsCanadianLab)
                    {
                        continue;
                    }
                    int ordinal = PatPlans.GetOrdinal(PriSecMed.Primary, createClaimDataWrapper.ClaimData.ListPatPlans
                        , createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListInsSubs);
                    if (ordinal == 0)
                    { //No primary dental plan. Must be a medical plan.  Use the first medical plan instead.
                        ordinal = 1;
                    }
                    sub = InsSubs.GetSub(PatPlans.GetInsSubNum(createClaimDataWrapper.ClaimData.ListPatPlans, ordinal), createClaimDataWrapper.ClaimData.ListInsSubs);
                    if (Procedures.NeedsSent(proc.ProcNum, sub.InsSubNum, createClaimDataWrapper.ClaimData.ListClaimProcs))
                    {
                        if (CultureInfo.CurrentCulture.Name.EndsWith("CA") && countSelected == 7)
                        {//Canadian. en-CA or fr-CA
                            LogClaimError(
                                createClaimDataWrapper, 
                                "Only the first 7 procedures will be automatically selected. You will need to create another claim for the remaining procedures.", isVerbose);
                            break;//only send 7.
                        }
                        countSelected++;
                        item.IsSelected = true;
                    }
                }
                if (listCreateClaimItems.All(x => !x.IsSelected))
                {//if still none selected
                    LogClaimError(createClaimDataWrapper, "Please select procedures first.", isVerbose);
                    createClaimDataWrapper.DoRefresh = false;
                    return createClaimDataWrapper;
                }
            }
            if (listCreateClaimItems.Any(x => x.IsSelected && x.ProcNum == 0))
            {
                LogClaimError(createClaimDataWrapper, "You can only select procedures.", isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }
            //At this point, all selected items are procedures.  In Canada, the selections may also include labs.
            InsCanadaValidateProcs(createClaimDataWrapper, isVerbose);
            if (createClaimDataWrapper.ListCreateClaimItems.All(x => !x.IsSelected))
            {
                LogClaimError(createClaimDataWrapper, "Please select procedures first.", isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }
            return createClaimDataWrapper;
        }

        ///<summary>Returns the createClaimDataWrapper object that was passed in after manipulating it.
        ///This object will contain information about what happened during the claim creation process (e.g. error messages, refresh indicator, etc).
        ///E.g. CreateClaimDataWrapper.HasError will be true if any errors occurred.  ErrorMessage might contain additional information about the error.
        ///This method assumes that createClaimDataWrapper was set up correctly (refer to GetCreateClaimDataWrapper() for how to set up this object).
        ///Set flag hasPrimaryClaim to true if the primary claim has alrady been created and should not be recreated.
        ///Set flag hasSecondaryClaim to true if the secondary claim has alrady been created and should not be recreated.</summary>
        public static CreateClaimDataWrapper CreateClaimFromWrapper(bool isVerbose, CreateClaimDataWrapper createClaimDataWrapper
            , bool hasPrimaryClaim = false, bool hasSecondaryClaim = false)
        {
            createClaimDataWrapper.ErrorMessage = "";
            if (!Security.IsAuthorized(Permissions.ClaimView, true))
            {
                LogClaimError(createClaimDataWrapper, "Not authorized for" + "\r\n" + UserGroupPermission.GetDescription(Permissions.ClaimView), isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }
            createClaimDataWrapper.ClaimCreatedCount = 0;
            if (!CheckClearinghouseDefaults())
            {
                createClaimDataWrapper.DoRefresh = false;
                createClaimDataWrapper.HasError = true;
                return createClaimDataWrapper;
            }
            if (createClaimDataWrapper.ClaimData.ListPatPlans.Count == 0)
            {
                LogClaimError(createClaimDataWrapper, "Patient does not have insurance.", isVerbose);
                createClaimDataWrapper.DoRefresh = false;
                return createClaimDataWrapper;
            }
            if (!hasPrimaryClaim)
            {
                string claimType = "P";
                //If they have medical insurance and no dental, make the claim type Medical.  This is to avoid the scenario of multiple med ins and no dental.
                if (PatPlans.GetOrdinal(PriSecMed.Medical, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListInsSubs) > 0
                    && PatPlans.GetOrdinal(PriSecMed.Primary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListInsSubs) == 0
                    && PatPlans.GetOrdinal(PriSecMed.Secondary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListInsSubs) == 0)
                {
                    claimType = "Med";
                }

                var claimCur = new Claim
                {
                    DateSent = DateTime.Today,
                    DateSentOrig = DateTime.MinValue,
                    ClaimStatus = "W"
                };

                //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
                claimCur = CreateClaim(claimCur, claimType, isVerbose, createClaimDataWrapper);
                if (claimCur.ClaimNum == 0)
                {
                    createClaimDataWrapper.DoRefresh = true;
                    return createClaimDataWrapper;
                }

                if (isVerbose)
                {//Only provide the user with the option to cancel the claim if attempting to create a single claim manually.

                    using (var formClaimEdit = new FormClaimEdit(claimCur, createClaimDataWrapper.Pat, createClaimDataWrapper.Fam))
                    {
                        formClaimEdit.IsNew = true; // this causes it to delete the claim if cancelling.
                        if (formClaimEdit.ShowDialog() != DialogResult.OK)
                        {
                            createClaimDataWrapper.DoRefresh = true; // will have already been deleted

                            return createClaimDataWrapper;
                        }
                    }

                    double unearnedAmount = (double)PaySplits.GetUnearnedForFam(createClaimDataWrapper.Fam);
                    //If there's unallocated amounts, we want to redistribute the money to other procedures.
                    if (unearnedAmount > 0)
                    {
                        AllocateUnearnedPayment(createClaimDataWrapper.Pat, createClaimDataWrapper.Fam, unearnedAmount, claimCur);
                    }
                }
            }

            if (!hasSecondaryClaim)
            {
                if (PatPlans.GetOrdinal(PriSecMed.Secondary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListInsSubs) > 0 //if there exists a secondary plan
                    && !CultureInfo.CurrentCulture.Name.EndsWith("CA"))//And not Canada (don't create secondary claim for Canada)
                {
                    //ClaimL.CalculateAndUpdate() could have added new claimprocs for additional insurance plans if they were added after the proc was completed
                    createClaimDataWrapper.ClaimData.ListClaimProcs = ClaimProcs.Refresh(createClaimDataWrapper.Pat.PatNum);
                    var claimCur = new Claim
                    {
                        ClaimStatus = "H"
                    };
                    //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
                    claimCur = CreateClaim(claimCur, "S", isVerbose, createClaimDataWrapper);
                    if (claimCur.ClaimNum == 0)
                    {
                        createClaimDataWrapper.DoRefresh = true;
                        return createClaimDataWrapper;
                    }
                }
            }

            createClaimDataWrapper.DoRefresh = true;
            return createClaimDataWrapper;
        }

        public static void AllocateUnearnedPayment(Patient patcur, Family famcur, double unearnedAmt, Claim ClaimCur)
        {
            // do not try to allocate payment if preference is disabled or if there isn't a payment to allocate
            if (!Preference.GetBool(PreferenceName.ShowAllocateUnearnedPaymentPrompt) || ClaimProcs.GetPatPortionForClaim(ClaimCur) <= 0)
            {
                return;
            }

            using (var formProcSelect = new FormProcSelect(patcur.PatNum, false, true, true))
            {
                if (formProcSelect.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var listProcAccountEntries = PaymentEdit.CreateAccountEntries(formProcSelect.ListSelectedProcs);

                var payment = new Payment
                {
                    PayDate = DateTime.Today,
                    PatNum = patcur.PatNum,
                    //Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
                    //disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
                    ClinicNum = 0
                };


                    if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.PatientDefaultClinic)
                    {
                        payment.ClinicNum = patcur.ClinicNum;
                    }
                    else if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.SelectedExceptHQ)
                    {
                        payment.ClinicNum = (Clinics.ClinicId == 0) ? patcur.ClinicNum : Clinics.ClinicId;
                    }
                    else
                    {
                        payment.ClinicNum = Clinics.ClinicId;
                    }
                

                payment.DateEntry = DateTime.Today;//So that it will show properly in the new window.
                payment.PaymentSource = CreditCardSource.None;
                payment.ProcessStatus = ProcessStat.OfficeProcessed;
                payment.PayAmt = 0;

                var paymentTypeDefinitions = Definition.GetByCategory(DefinitionCategory.PaymentTypes); ;
                if (paymentTypeDefinitions.Count > 0)
                {
                    payment.PayType = paymentTypeDefinitions[0].Id;
                }

                Payments.Insert(payment);

                using (var formPayment = new FormPayment(patcur, famcur, payment, false))
                {
                    formPayment.IsNew = true;
                    formPayment.UnearnedAmt = unearnedAmt;
                    formPayment.ListEntriesPayFirst = listProcAccountEntries;
                    formPayment.IsIncomeTransfer = true;
                    formPayment.ShowDialog();
                }
            }

        }

        ///<summary>Only allows up to 7 CreateClaimItems to be selected within createClaimDataWrapper when Canadian.
        ///Shows a message to the user stating this fact if enforced; Otherwise does nothing.</summary>
        private static void InsCanadaValidateProcs(CreateClaimDataWrapper createClaimDataWrapper, bool isVerbose)
        {
            List<CreateClaimItem> listToolBarInsSelectedItems = createClaimDataWrapper.ListCreateClaimItems.FindAll(x => x.IsSelected);
            int procCount = listToolBarInsSelectedItems.Count(x => x.ProcNumLab == 0);//Not a lab
            if (CultureInfo.CurrentCulture.Name.EndsWith("CA") && procCount > 7)
            {//Canadian. en-CA or fr-CA
             //Unselect all but the first 7 procedures with the smallest index numbers.
                int procSelectedCount = 0;
                foreach (CreateClaimItem item in listToolBarInsSelectedItems)
                {
                    item.IsSelected = (procSelectedCount < 7);
                    if (item.ProcNumLab == 0)
                    {//Not a lab
                        procSelectedCount++;
                    }
                }
                string message = "Only the first 7 procedures will be selected. You will need to create another claim for the remaining procedures.";
                LogClaimError(createClaimDataWrapper, message, isVerbose);
            }
        }

        /// <summary>
        ///     <para>
        ///         The only validation that's been done is just to make sure that only procedures 
        ///         are selected. All validation on the procedures selected is done here.
        ///     </para>
        ///     <para>
        ///         Creates and saves claim initially, attaching all selected procedures. But it 
        ///         does not refresh any data. Does not do a final update of the new claim. Does 
        ///         not enter fee amounts.
        ///     </para>
        ///     <para>
        ///         <paramref name="claimType"/> = P, S, Med or Other
        ///     </para>
        ///     <para>
        ///         This method assumes that the specified <paramref name="createClaimDataWrapper"/>
        ///         was set up correctly (refer to 
        ///         <see cref="GetCreateClaimDataWrapper(Patient, Family, List{CreateClaimItem}, bool, bool)"/> 
        ///         for how to set up this object).
        ///     </para>
        ///     <para>
        ///         Returns a 'new' <see cref="Claim"/> object (ClaimNum=0) to indicate that the 
        ///         user does not want to create the claim or there are validation issues.
        ///     </para>
        ///     <para>
        ///         Set <see cref="isKeepStatus"/> to true if you do not want the status overridden to be "U".
        ///     </para>
        ///</summary>
        public static Claim CreateClaim(Claim claim, string claimType, bool isVerbose, CreateClaimDataWrapper createClaimDataWrapper)
        {
            Patient pat = createClaimDataWrapper.Pat;
            InsPlan PlanCur = new InsPlan();
            InsSub SubCur = new InsSub();
            Relat relatOther = Relat.Self;
            string claimTypeDesc = "";
            switch (claimType)
            {
                case "P":
                    SubCur = InsSubs.GetSub(PatPlans.GetInsSubNum(createClaimDataWrapper.ClaimData.ListPatPlans
                        , PatPlans.GetOrdinal(PriSecMed.Primary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans
                        , createClaimDataWrapper.ClaimData.ListInsSubs)), createClaimDataWrapper.ClaimData.ListInsSubs);
                    PlanCur = InsPlans.GetPlan(SubCur.PlanNum, createClaimDataWrapper.ClaimData.ListInsPlans);
                    claimTypeDesc = "primary";
                    break;

                case "S":
                    SubCur = InsSubs.GetSub(PatPlans.GetInsSubNum(createClaimDataWrapper.ClaimData.ListPatPlans
                        , PatPlans.GetOrdinal(PriSecMed.Secondary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans
                        , createClaimDataWrapper.ClaimData.ListInsSubs)), createClaimDataWrapper.ClaimData.ListInsSubs);
                    PlanCur = InsPlans.GetPlan(SubCur.PlanNum, createClaimDataWrapper.ClaimData.ListInsPlans);
                    claimTypeDesc = "secondary";
                    break;

                case "Med":
                    //It's already been verified that a med plan exists
                    SubCur = InsSubs.GetSub(PatPlans.GetInsSubNum(createClaimDataWrapper.ClaimData.ListPatPlans
                        , PatPlans.GetOrdinal(PriSecMed.Medical, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans
                        , createClaimDataWrapper.ClaimData.ListInsSubs)), createClaimDataWrapper.ClaimData.ListInsSubs);
                    PlanCur = InsPlans.GetPlan(SubCur.PlanNum, createClaimDataWrapper.ClaimData.ListInsPlans);
                    claimTypeDesc = "medical";
                    break;

                case "Other":
                    using (var formClaimCreate = new FormClaimCreate(pat.PatNum))
                    {
                        if (formClaimCreate.ShowDialog() != DialogResult.OK)
                        {
                            return new Claim();
                        }

                        PlanCur = formClaimCreate.SelectedPlan;
                        SubCur = formClaimCreate.SelectedSub;
                        relatOther = formClaimCreate.PatRelat;
                    }
                    break;
            }

            var selectedProcedureIds = 
                createClaimDataWrapper.ListCreateClaimItems
                    .Where(x => x.IsSelected)
                    .Select(x => x.ProcNum).ToList();

            var selectedProcedures = createClaimDataWrapper.ClaimData.ListProcs.FindAll(x => x.ProcNum.In(selectedProcedureIds));

            // If we are going to block based on a preference, do that before figuring out other 
            // claim validation. Ignore "No Bill Ins" here, because we want "No Bill Ins" to be the
            // more important block for backwards compatability.
            switch (Preference.GetEnum<ClaimZeroDollarProcBehavior>(PreferenceName.ClaimZeroDollarProcBehavior))
            {
                case ClaimZeroDollarProcBehavior.Warn:
                    if (selectedProcedures.FirstOrDefault(x => x.ProcFee.IsZero() && !Procedures.NoBillIns(x, createClaimDataWrapper.ClaimData.ListClaimProcs, PlanCur.PlanNum)) != null
                        && MessageBox.Show("You are about to make a " + (claimTypeDesc == "" ? "" : (claimTypeDesc + " "))
                            + "claim that will include a $0 procedure. Continue?", "Claims", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        //Nothing to log.  The user hit Cancel.
                        return new Claim();
                    }
                    break;

                case ClaimZeroDollarProcBehavior.Block:
                    if (selectedProcedures.FirstOrDefault(x => x.ProcFee.IsZero() && !Procedures.NoBillIns(x, createClaimDataWrapper.ClaimData.ListClaimProcs, PlanCur.PlanNum)) != null)
                    {
                        MessageBox.Show(
                            "You can't make a claim for a $0 procedure.", 
                            "Claims", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                        //Nothing to log.  The user was just notified.
                        return new Claim();
                    }
                    break;

                case ClaimZeroDollarProcBehavior.Allow:
                default:
                    break;
            }

            Tuple<bool, Claim, string> result = AccountModules.CreateClaim(claim, claimType, createClaimDataWrapper.ClaimData.ListPatPlans
                , createClaimDataWrapper.ClaimData.ListInsPlans, createClaimDataWrapper.ClaimData.ListClaimProcs, createClaimDataWrapper.ClaimData.ListProcs
                , createClaimDataWrapper.ClaimData.ListInsSubs, pat, createClaimDataWrapper.ClaimData.PatNote, selectedProcedures
                , createClaimDataWrapper.ErrorMessage, PlanCur, SubCur, relatOther);

            bool isSuccess = result.Item1;
            claim = result.Item2;
            string claimError = result.Item3;
            if (isSuccess)
            {
                createClaimDataWrapper.ClaimCreatedCount++;
            }
            else if (!string.IsNullOrEmpty(claimError))
            {
                LogClaimError(createClaimDataWrapper, claimError, isVerbose);
            }
            return claim;
        }

        public static void PromptForSecondaryClaim(List<ClaimProc> _listClaimProcsForClaim)
        {
            //Get a list of ProcNums from the primary claim.
            List<long> listProcNumsOnPriClaim = _listClaimProcsForClaim.Where(x => x.ProcNum > 0).Select(x => x.ProcNum).ToList();
            //Get list of ClaimProcs for the list of ProcNums on Primary claim. Make sure the claim procs are not received and are attached to a claim.
            List<ClaimProc> listClaimProcsForPriProcsNotReceived = ClaimProcs.GetForProcs(listProcNumsOnPriClaim)
                .Where(x => x.Status == ClaimProcStatus.NotReceived && x.ClaimNum != 0).ToList();
            if (listClaimProcsForPriProcsNotReceived.Count == 0)
            {
                return;//No unreceived claimprocs for procs on pri claim.
            }
            //We have unreceived claimprocs for the ProcNums that are attached to the primary claim. We need to find out if the claim procs 
            //are attached to a secondary claim with status of 'Sent' or 'Hold Until Pri Received'
            List<Claim> listSecondaryClaims = Claims.GetClaimsFromClaimNums(listClaimProcsForPriProcsNotReceived.Select(x => x.ClaimNum).ToList())
                .Where(x => x.ClaimStatus.In("U", "H") && x.ClaimType == "S").ToList();
            if (listSecondaryClaims.Count == 0)
            {
                return;//No secondary claims for the procedures attached to the primary.
            }
            string msg = "There is at least one unsent secondary claim for the received procedures.\r\nWould you like to:";
            var inputBox = new InputBox(new List<InputBoxParam>()
            {
                new InputBoxParam(InputBoxType.CheckBox,msg,"Change the claim status to 'Waiting to send'",Size.Empty),
                new InputBoxParam(InputBoxType.CheckBox,"","Send secondary claim(s) now",Size.Empty),
                new InputBoxParam(InputBoxType.CheckBox,"","Do nothing",Size.Empty)
            });
            inputBox.setTitle("Outstanding secondary claims");
            inputBox.Size = new Size(450, 200);
            if (inputBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            CheckBox selectedCheckBox = inputBox.Controls.OfType<CheckBox>().Where(x => x.Checked).First();
            if (selectedCheckBox.Text.Contains("Do nothing"))
            {
                return;
            }
            //We need to update claims status to 'Waiting to Send' regardless of what option they check. See Claims.GetQueueList(...) below.
            foreach (Claim claim in listSecondaryClaims)
            {
                claim.ClaimStatus = "W";
                Claims.Update(claim);
            }
            if (selectedCheckBox.Text.Contains("Send secondary claim"))
            {
                //Most likely all of the procedures on the primary claim will have all of the procedures on 1 secondary claim. Expecially since most of time the 
                //claim will be created automatically. The only time they don't get created automatically is when the patient doesn't have a secondary claim 
                //at the time the primary claim gets created. Even if the user created the claim manually, the chances that the procedures on the primary have more than
                //one claim are low.
                ClaimSendQueueItem[] listQueue = Claims.GetQueueList(listSecondaryClaims.Select(x => x.ClaimNum).ToList(), 0, 0);
                SendClaimSendQueueItems(listQueue.ToList(), 0);//Use clearinghouseNum of 0 to indicate automatic calculation of clearinghouse
            }
        }

        ///<summary>Receives the claim and to set the claim dates and totals properly. isIncludeWOPercCoPay=true causes WriteOffs to be posted for 
        ///ClaimProcs associated to Category Percentage or Medicaid/Flat CoPay insurance plans, false does not post WriteOffs for these insurance plan 
        ///types.  isSupplementalPay=true causes claim to not be marked received because Supplemental payments can only be applied to previously 
        ///received claims, false allows the claim to be marked received if all ClaimProcs in listClaimProcsForClaim meet requirements.
        public static void ReceiveEraPayment(Claim claim, Hx835_Claim claimPaid, List<ClaimProc> listClaimProcsForClaim, bool isIncludeWOPercCoPay, bool isSupplementalPay, InsPlan insPlan = null)
        {
            //Recalculate insurance paid, deductible, and writeoff amounts for the claim based on the final claimproc values, then save the results to the database.
            claim.InsPayAmt = 0;
            claim.DedApplied = 0;
            claim.WriteOff = 0;
            InsPlan insPlanCur = insPlan;
            if (!isIncludeWOPercCoPay && insPlanCur == null)
            {//Might not want to include WOs, need to check plan type.
                insPlanCur = InsPlans.RefreshOne(claim.PlanNum);
            }
            foreach (ClaimProc claimProc in listClaimProcsForClaim)
            {
                if (claimProc.Status == ClaimProcStatus.Preauth)
                {//Mimics FormClaimEdit preauth by procedure logic.
                    ClaimProcs.SetInsEstTotalOverride(claimProc.ProcNum, claimProc.PlanNum, claimProc.InsPayEst, new List<ClaimProc>() { claimProc });
                    continue;//SetInsEstTotalOverride() updates claimProc to database.
                }
                claim.InsPayAmt += claimProc.InsPayAmt;
                claim.DedApplied += claimProc.DedApplied;
                //If pref is off, Category Percentage or Medicaid/FlatCopay do not include Writeoff.
                if (!isIncludeWOPercCoPay && insPlanCur != null && insPlanCur.PlanType.In("", "f"))
                {
                    //Do not include WriteOff in claim total.
                    //Also need to change the claimProc directly, this really only matters when automatically recieving the ERA payment.
                    claimProc.WriteOff = 0;
                }
                claim.WriteOff += claimProc.WriteOff;
                if (claimProc.ClaimProcNum == 0)
                {//Total payment claimproc which was created in FormEtrans835Edit just before loading this window.
                    ClaimProcs.Insert(claimProc);
                }
                else
                {//Procedure claimproc, because the estimate already existed before entering payment.
                    ClaimProcs.Update(claimProc);
                }
            }
            if (!isSupplementalPay//Supplemental payments can only be applied to previously received claims
                                  //Split claims mark claimProcs recieved one at a time.
                && listClaimProcsForClaim.All(x => x.Status.In(ClaimProcStatus.Received, ClaimProcStatus.Supplemental, ClaimProcStatus.CapClaim, ClaimProcStatus.Preauth)))
            {
                //Do not mark received until all claim procs are handled.
                claim.ClaimStatus = "R";//Received.
                claim.DateReceived = claimPaid.DateReceived;
            }
            Claims.Update(claim);
            //JM - If we ever decide to enable ERA automation this will need to be considered.
            if (Preference.GetBool(PreferenceName.PromptForSecondaryClaim) && Security.IsAuthorized(Permissions.ClaimSend, true))
            {
                ClaimL.PromptForSecondaryClaim(listClaimProcsForClaim);
            }
        }

        ///<summary>Validates and sends each of the ClaimSendQueueItems passed in. Returns a list of the ClaimSendQueueItem that were sent.</summary>
        internal static List<ClaimSendQueueItem> SendClaimSendQueueItems(List<ClaimSendQueueItem> listClaimSendQueueItems, long hqClearinghouseNum)
        {
            List<ClaimSendQueueItem> retVal = new List<ClaimSendQueueItem>();//a list of queue items to send
            if (listClaimSendQueueItems.Count == 0)
            {
                return retVal;
            }

                long clinicNum0 = Claims.GetClaim(listClaimSendQueueItems[0].ClaimNum).ClinicNum;
                for (int i = 1; i < listClaimSendQueueItems.Count; i++)
                {
                    long clinicNum = Claims.GetClaim(listClaimSendQueueItems[i].ClaimNum).ClinicNum;
                    if (clinicNum0 != clinicNum)
                    {
                        MsgBox.Show("ContrAccount", "All claims must be for the same clinic. You can use the combobox at the top to filter.");//TODO: Wording.
                        return retVal;
                    }
                }
            
            long clearinghouseNum0 = listClaimSendQueueItems[0].ClearinghouseNum;
            EnumClaimMedType medType0 = Claims.GetClaim(listClaimSendQueueItems[0].ClaimNum).MedType;
            int index = 0;
            foreach (ClaimSendQueueItem claimSendItem in listClaimSendQueueItems)
            {//we start with 0 so that we can check medtype match on the first claim
                long clearinghouseNumI = claimSendItem.ClearinghouseNum;
                if (clearinghouseNum0 != clearinghouseNumI)
                {
                    MsgBox.Show("ContrAccount", "All claims must be for the same clearinghouse.");
                    return retVal;
                }
                EnumClaimMedType medTypeI = Claims.GetClaim(claimSendItem.ClaimNum).MedType;
                if (medType0 != medTypeI)
                {
                    MsgBox.Show("ContrAccount", "All claims must have the same MedType.");
                    return retVal;
                }
                Clearinghouse clearh = Clearinghouses.GetClearinghouse(clearinghouseNumI);
                if (clearh.Eformat == ElectronicClaimFormat.x837D_4010 || clearh.Eformat == ElectronicClaimFormat.x837D_5010_dental)
                {
                    if (medTypeI != EnumClaimMedType.Dental)
                    {
                        MsgBox.Show("ContrAccount", "On claim " + POut.Int(index) + ", the MedType does not match the clearinghouse e-format.");
                        return retVal;
                    }
                }
                if (clearh.Eformat == ElectronicClaimFormat.x837_5010_med_inst)
                {
                    if (medTypeI != EnumClaimMedType.Medical && medTypeI != EnumClaimMedType.Institutional)
                    {
                        MsgBox.Show("ContrAccount", "On claim " + POut.Int(index) + ", the MedType does not match the clearinghouse e-format.");
                        return retVal;
                    }
                }
                if (claimSendItem.HasIcd9)
                {
                    string msgText = Lan.g("ContrAccount", "There are ICD-9 codes attached to a procedure.  Would you like to send the claim without the ICD-9 codes? ");
                    if (MessageBox.Show(msgText, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return retVal;
                    }
                }
                //This is done for PromptForSecondaryClaim(...).
                //SendEclaimsToClearinghouse(...) already validates items in listClaimSendQueueItems, SetClaimItemIsValid(...) will just return in this case.
                SetClaimItemIsValid(claimSendItem, clearh);
                if (!claimSendItem.IsValid && claimSendItem.CanSendElect)
                {
                    MsgBox.Show("ContrAccount", "Not allowed to send e-claims with missing information.");
                    return retVal;
                }
                if (claimSendItem.NoSendElect == NoSendElectType.NoSendElect)
                {
                    MsgBox.Show("ContrAccount", "Not allowed to send e-claims.");
                    return retVal;
                }
                if (claimSendItem.NoSendElect == NoSendElectType.NoSendSecondaryElect && claimSendItem.Ordinal != 1)
                {
                    MsgBox.Show("ContrAccount", "Only allowed to send primary insurance e-claims.");
                    return retVal;
                }
                index++;
            }
            foreach (ClaimSendQueueItem claimSendItem in listClaimSendQueueItems)
            {
                ClaimSendQueueItem queueitem = claimSendItem.Copy();
                if (hqClearinghouseNum != 0)
                {
                    queueitem.ClearinghouseNum = hqClearinghouseNum;
                }
                retVal.Add(queueitem);
            }
            Claim claim0 = Claims.GetClaim(listClaimSendQueueItems[0].ClaimNum);
            long claimClinicNum = 0;

                claimClinicNum = claim0.ClinicNum;//All claims for the queueItems have same clinic, due to validation above.
            
            Clearinghouse clearinghouseHq = ClearinghouseL.GetClearinghouseHq(retVal[0].ClearinghouseNum);
            Clearinghouse clearinghouseClin = Clearinghouses.OverrideFields(clearinghouseHq, claimClinicNum);
            EnumClaimMedType medType = claim0.MedType;
            //Already validated that all claims are for the same clearinghouse, clinic, and medType.
            //Validated that medtype matches clearinghouse e-format
            Eclaims.SendBatch(clearinghouseClin, retVal, medType, new FormClaimFormItemEdit(),
                FormClaimPrint.FillRenaissance, new FormTerminalConnection());//this also calls SetClaimSentOrPrinted which creates the etrans entry.
            return retVal;
        }

        ///<summary>Sets the ClaimSendQueueItem.IsValid flag. Checks if the ClaimSendQueueItem passed in has any missing data.</summary>
        public static void SetClaimItemIsValid(ClaimSendQueueItem claimSendQueueItem, Clearinghouse clearinghouseClin)
        {
            if (claimSendQueueItem.IsValid)
            {
                return;//no need to check. ClaimItem is valid already.
            }
            claimSendQueueItem = Eclaims.GetMissingData(clearinghouseClin, claimSendQueueItem);
            if (claimSendQueueItem.MissingData == "")
            {
                claimSendQueueItem.IsValid = true;
            }
        }

        ///<summary>Returns ClaimIsValidState.True if given claim is valid.
        ///Does NOT check for Canadian warnings.</summary>
        public static ClaimIsValidState ClaimIsValid(Claim claim, List<ClaimProc> listClaimProcsForClaim)
        {
            return ClaimIsValid(claim.DateService.ToShortDateString(), claim.ClaimType, claim.ClaimStatus.In("S", "R"), claim.DateSent.ToShortDateString(), listClaimProcsForClaim,
                claim.PlanNum, null, claim.ClaimNote, claim.UniformBillType, claim.CorrectionType
            );
        }

        ///<summary>Returns ClaimIsValidState.True if given claim is valid.
        ///Does NOT check for Canadian warnings.
        ///This should be called when there is a UI that the user can make changes to that might not be saved in the claim object.</summary>
        public static ClaimIsValidState ClaimIsValid(string dateService, string claimType, bool isSentOrReceived, string claimDateSent, List<ClaimProc> listClaimProcsForClaim
            , long claimPlanNum, List<InsPlan> listInsPlans, string claimNote, string claimUniformBillType, ClaimCorrectionType claimCorrectionType)
        {
            if (dateService == "" && claimType != "PreAuth")
            {
                MsgBox.Show("Claims", "Please enter a date of service");
                return ClaimIsValidState.False;
            }
            if (isSentOrReceived && claimDateSent == "")
            {
                MsgBox.Show("Claims", "Please enter date sent.");
                return ClaimIsValidState.False;
            }
            if (claimType == "PreAuth")
            {
                bool hasStatusChanged = false;
                foreach (ClaimProc claimProc in listClaimProcsForClaim)
                {
                    if (claimProc.Status != ClaimProcStatus.Preauth)
                    {
                        claimProc.Status = ClaimProcStatus.Preauth;
                        ClaimProcs.Update(claimProc);
                        hasStatusChanged = true;
                    }
                }
                if (hasStatusChanged)
                {
                    MsgBox.Show("Claims", "Status of procedures was changed back to preauth to match status of claim.");
                    return ClaimIsValidState.FalseClaimProcsChanged;
                }
            }
            if (Preference.GetBool(PreferenceName.ClaimsValidateACN))
            {
                InsPlan plan = InsPlans.GetPlan(claimPlanNum, listInsPlans);//Does a query if listInsPlans is null or if claimPlanNum is not in list.
                if (plan != null && plan.GroupName.Contains("ADDP"))
                {
                    if (!Regex.IsMatch(claimNote, "ACN[0-9]{5,}"))
                    {//ACN with at least 5 digits following
                        MsgBox.Show("Claims", "For an ADDP claim, there must be an ACN number in the note.  Example format: ACN12345");
                        return ClaimIsValidState.False;
                    }
                }
            }
            if (claimUniformBillType != "" && claimCorrectionType != ClaimCorrectionType.Original)
            {
                MsgBox.Show("Claims", "Correction type must be original when type of bill is not blank.");
                return ClaimIsValidState.False;
            }
            return ClaimIsValidState.True;
        }
    }

    ///<summary>Helper class for passing around data required to create a claim.  Also contains informational variables for consuming methods.
    ///This class helps so that we no longer have to pass around DataTables and ODGrids but instead can have a strongly typed object.</summary>
    public class CreateClaimDataWrapper
    {
        ///<summary>The currently selected patient that is having a claim created.</summary>
        public Patient Pat;
        ///<summary>The family of Pat.</summary>
        public Family Fam;
        ///<summary>Pertinent insurance information for the corresponding patient and family.</summary>
        public AccountModules.CreateClaimData ClaimData;
        ///<summary>Technically this is just a list of account items in the sense that it is almost always comprised from account grids.
        ///These account items can represent anything selected in a grid that was showing to the user (typically an account grid from the Account module).
        ///Methods that utilize this class will know how to filter through these items in order to find the ones they care about.
        ///The main purpose of this list is to represent which grid items were selected (or not selected) and to what procedures they are associated to.
        ///This list allows us to stop passing around an ODGrid / DataTable combination which was causing concurrency bugs to get submitted.</summary>
        public List<CreateClaimItem> ListCreateClaimItems;
        ///<summary>A count of how many claims were created.  This variable is handled by helper methods and should not be set manually.</summary>
        public int ClaimCreatedCount;
        ///<summary>An indicator to the consuming method so that they know if they need to refresh their UI or not.
        ///Old comment:  True if the Account module needs to be refreshed (old comment from ContrAccount.toolBarButIns_Click()).
        ///This variable is handled by helper methods and should not be set manually.</summary>
        public bool DoRefresh;
        ///<summary>Set to true if any errors occurred when creating a claim;  Otherwise, false.
        ///ErrorMessage should be the only other value trusted when this is set to true, no other information should be trusted.
        ///This variable is handled by helper methods and should not be set manually.</summary>
        public bool HasError;
        ///<summary>Additional information to help determine what errors happened while trying to create claims.
        ///Will typically be set to a detailed error to display to the user when HasError is true.
        ///However, it can still be empty even when an error occurred so HasError should be the indicator that an error occurred.
        ///This variable is handled by helper methods and should not be set manually.</summary>
        public string ErrorMessage;
    }

    ///<summary>Represents a selected item (or all items) from a grid.  Typically an item from the account grid in the Account module.
    ///Methods that utilize this class will know how to filter through these items in order to find the ones they care about.
    ///Therefore any value is acceptable for any of the variables within this class.  E.g. an item with a ProcNum of 0 represents a non-procedure item.
    ///This object allows us to stop passing around an ODGrid / DataTable combination which was causing concurrency bugs to get submitted.</summary>
    public class CreateClaimItem
    {
        ///<summary>A value greater than 0 will indicate that this item represents a procedure.</summary>
        public long ProcNum;
        ///<summary>A value greater than 0 will indicate that this item represents a lab.  Currently only used by Canadians.</summary>
        public long ProcNumLab;
        ///<summary>The charge associated to this item.  Typically represents a procedure fee.</summary>
        public double ChargesDouble;
        ///<summary>Set to true if claim creation logic should consider this item for the claim that is being created;  Otherwise, false.
        ///This variable is typically set by helper methods unless this item needs to be treated as if it were manually selected.
        ///Canadians have a hard limit of 7 items selected per claim so this value may change regardless of how it was instantiated.</summary>
        public bool IsSelected;
    }

    public enum ClaimIsValidState
    {
        False,
        FalseClaimProcsChanged,
        True,
    }

    public enum BoolOverride
    {
        Undefined,
        False,
        True
    }
}
