﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using System;
using CodeBase;

namespace OpenDental
{
    public class ProcedureL
    {
        ///<summary>Sets all procedures for apt complete.  Flags procedures as CPOE as needed (when prov logged in).  Makes a log
        ///entry for each completed proc.  Then fires the CompleteProcedure automation trigger.</summary>
        public static List<Procedure> SetCompleteInAppt(Appointment apt, List<InsPlan> PlanList, List<PatPlan> patPlans, Patient patient,
            List<InsSub> subList, bool removeCompletedProcs)
        {
            List<Procedure> listProcsInAppt = Procedures.SetCompleteInAppt(apt, PlanList, patPlans, patient, subList, removeCompletedProcs);
            AutomationL.Trigger(AutomationTrigger.CompleteProcedure, listProcsInAppt.Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).ToList(), apt.PatNum);
            return listProcsInAppt;
        }

        ///<summary>Returns empty string if no duplicates, otherwise returns duplicate procedure information.  In all places where this is called, we are guaranteed to have the eCW bridge turned on.  So this is an eCW peculiarity rather than an HL7 restriction.  Other HL7 interfaces will not be checking for duplicate procedures unless we intentionally add that as a feature later.</summary>
        public static string ProcsContainDuplicates(List<Procedure> procs)
        {
            bool hasLongDCodes = false;
            HL7Def defCur = HL7Defs.GetOneDeepEnabled();
            if (defCur != null)
            {
                hasLongDCodes = defCur.HasLongDCodes;
            }
            string info = "";
            List<Procedure> procsChecked = new List<Procedure>();
            for (int i = 0; i < procs.Count; i++)
            {
                Procedure proc = procs[i];
                ProcedureCode procCode = ProcedureCodes.GetProcCode(procs[i].CodeNum);
                string procCodeStr = procCode.ProcCode;
                if (procCodeStr.Length > 5
                    && procCodeStr.StartsWith("D")
                    && !hasLongDCodes)
                {
                    procCodeStr = procCodeStr.Substring(0, 5);
                }
                for (int j = 0; j < procsChecked.Count; j++)
                {
                    Procedure procDup = procsChecked[j];
                    ProcedureCode procCodeDup = ProcedureCodes.GetProcCode(procsChecked[j].CodeNum);
                    string procCodeDupStr = procCodeDup.ProcCode;
                    if (procCodeDupStr.Length > 5
                        && procCodeDupStr.StartsWith("D")
                        && !hasLongDCodes)
                    {
                        procCodeDupStr = procCodeDupStr.Substring(0, 5);
                    }
                    if (procCodeDupStr != procCodeStr)
                    {
                        continue;
                    }
                    if (procDup.ToothNum != proc.ToothNum)
                    {
                        continue;
                    }
                    if (procDup.ToothRange != proc.ToothRange)
                    {
                        continue;
                    }
                    if (procDup.ProcFee != proc.ProcFee)
                    {
                        continue;
                    }
                    if (procDup.Surf != proc.Surf)
                    {
                        continue;
                    }
                    if (info != "")
                    {
                        info += ", ";
                    }
                    info += procCodeDupStr;
                }
                procsChecked.Add(proc);
            }

            if (info != "")
            {
                info = "Duplicate procedures: " + info;
            }

            return info;
        }

        ///<summary>Checks to see if the appointments provider has at least one mismatch provider on all the completed procedures attached to the appointment.
        ///If so, checks to see if the user has permission to edit a completed procedure. If the user does, then the user has the option to change the provider to match.</summary>
        public static bool DoRemoveCompletedProcs(Appointment apt, List<Procedure> listProcsForAppt, bool checkForAllProcCompl = false)
        {
            if (listProcsForAppt.Count == 0)
            {
                return false;
            }
            if (checkForAllProcCompl && (apt.AptStatus != ApptStatus.Complete || listProcsForAppt.All(x => x.ProcStatus == ProcStat.C)))
            {
                return false;
            }
            List<Procedure> listCompletedProcWithDifferentProv = new List<Procedure>();
            foreach (Procedure proc in listProcsForAppt)
            {
                if (proc.ProcStatus != ProcStat.C || proc.AptNum != apt.AptNum)
                {//should all be complete already. 
                    continue;
                }
                ProcedureCode procCode = ProcedureCodes.GetProcCode(proc.CodeNum);
                long provNum = Procedures.GetProvNumFromAppointment(apt, proc, procCode);
                if (provNum != proc.ProvNum)
                {
                    listCompletedProcWithDifferentProv.Add(proc);
                }
            }
            if (listCompletedProcWithDifferentProv.Count == 0)
            {
                return false;//no completed procedures or prov changed. 
            }
            if (Preference.GetBool(PreferenceName.ProcProvChangesClaimProcWithClaim))
            {
                List<ClaimProc> listClaimProcs = ClaimProcs.RefreshForProcs(listCompletedProcWithDifferentProv.Select(x => x.ProcNum).ToList());
                if (listClaimProcs.Any(x => x.Status == ClaimProcStatus.Received
                     || x.Status == ClaimProcStatus.Supplemental
                     || x.Status == ClaimProcStatus.CapClaim))
                {
                    MsgBox.Show("Procedures", "The appointment provider does not match the provider on at least one procedure that is attached "
                        + "to a claim.\r\nThe provider on the procedure(s) cannot be changed.");
                    return true;
                }
            }
            List<PaySplit> listPaySplit = PaySplits.GetPaySplitsFromProcs(listCompletedProcWithDifferentProv.Select(x => x.ProcNum).ToList());
            if (listPaySplit.Count > 0)
            {
                MsgBox.Show("Procedures", "The appointment provider does not match the provider on at least one completed procedure.\r\n"
                    + "The procedure provider cannot be changed to match the appointment provider because the paysplit provider would no longer match.  "
                    + "Any change to the provider on the completed procedure(s) or paysplit(s) will have to be made manually.");
                return true;//paysplits exist on one of the completed procedures. Per Nathan, don't change the provider. User will need to change manually.
            }
            foreach (Procedure proc in listCompletedProcWithDifferentProv)
            {
                string perm = Permissions.EditCompletedProcedure;
                if (proc.ProcStatus.In(ProcStat.EC, ProcStat.EO))
                {
                    perm = Permissions.EditProcedure;
                }
                if (Security.IsGlobalDateLock(perm, proc.ProcDate))
                {
                    return true;
                }
                if (!Security.IsAuthorized(perm, proc.ProcDate, true, true))
                {
                    MessageBox.Show(Lan.g("Procedures", "The appointment provider does not match the provider on at least one completed procedure.") + "\r\n"
                        + Lans.g("Procedures", "Not authorized for") + ": " + UserGroupPermission.GetDescription(perm) + "\r\n"
                        + Lan.g("Procedures", "Any change to the provider on the completed procedure(s) will have to be made manually."));
                    return true;//user does not have permission to change the provider. Don't change provider.
                }
            }
            //The appointment is set complete, completed procedures exist, and provider does not match appointment.
            //Ask if they would like to change the providers on the completed procedure to match the appointments provider
            if (!MsgBox.Show("Procedures", MsgBoxButtons.YesNo, "The appointment provider does not match the provider on at least one completed procedure.\r\n"
                + "Change the provider on the completed procedure(s) to match the provider on the appointment?"))
            {
                return true;//user does not want to change the providers
            }
            //user wants to change the provider on the completed procedure
            return false;
        }

        public static List<ODBoxItem<ProcStat>> FillProcStatusCombo(ProcStat procCurStatus, bool isProcLocked, long procNum)
        {
            List<ODBoxItem<ProcStat>> listProcStat = new List<ODBoxItem<ProcStat>>();
            if (procCurStatus == ProcStat.D && isProcLocked)
            {//only set this when coming in with this status. 
                listProcStat.Add(new ODBoxItem<ProcStat>("Invalidated", ProcStat.D));
                return listProcStat;
            }
            listProcStat.Add(new ODBoxItem<ProcStat>("Treatment Planned", ProcStat.TP));
            //For the "Complete" option, instead of showing current value,
            //show what the value would represent if set to complete, in case user changes to complete from another status.
            bool isInProcess = ProcMultiVisits.IsProcInProcess(procNum, true);
            listProcStat.Add(new ODBoxItem<ProcStat>("Complete" + (isInProcess ? " (In Process)" : ""), ProcStat.C));
            if (!Preference.GetBool(PreferenceName.EasyHideClinical))
            {
                listProcStat.Add(new ODBoxItem<ProcStat>("Existing-Current Prov", ProcStat.EC));
                listProcStat.Add(new ODBoxItem<ProcStat>("Existing-Other Prov", ProcStat.EO));
                listProcStat.Add(new ODBoxItem<ProcStat>("Referred Out", ProcStat.R));
                listProcStat.Add(new ODBoxItem<ProcStat>("Condition", ProcStat.Cn));
            }
            if (procCurStatus == ProcStat.TPi)
            {//only set this when coming in with that status, users should not choose this status otherwise.
                listProcStat.Add(new ODBoxItem<ProcStat>("Treatment Planned Inactive", ProcStat.TPi));
            }
            return listProcStat;
        }

        public static bool IsQuantityValid(int quantity)
        {
            if (quantity < 1)
            {
                MsgBox.Show("Procedures", "Qty not valid.  Typical value is 1.");
                return false;
            }
            return true;
        }

        public static bool AreTimesValid(string timeStart, string timeEnd)
        {
            if (Programs.UsingOrion || Preference.GetBool(PreferenceName.ShowFeatureMedicalInsurance))
            {
                if (!ValidateTime(timeStart))
                {
                    MessageBox.Show(
                        "Start time is invalid.", 
                        "Procedures", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return false;
                }

                if (!ValidateTime(timeEnd))
                {
                    MessageBox.Show(
                        "End time is invalid.",
                        "Procedures", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return false;
                }
            }
            else
            {
                if (timeStart != "")
                {
                    try
                    {
                        DateTime.Parse(timeStart);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Start time is invalid.", 
                            "Procedures", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                        return false;
                    }
                }
            }

            return true;
        }

        ///<summary>Empty string is considered valid.</summary>
        public static bool ValidateTime(string time)
        {
            string militaryTime = time;
            if (militaryTime == "")
            {
                return true;
            }
            if (militaryTime.Length < 4)
            {
                militaryTime = militaryTime.PadLeft(4, '0');
            }
            //Test if user typed in military time. Ex: 0830 or 1536
            try
            {
                int hour = PIn.Int(militaryTime.Substring(0, 2));
                int minute = PIn.Int(militaryTime.Substring(2, 2));
                if (hour > 23)
                {
                    return false;
                }
                if (minute > 59)
                {
                    return false;
                }
                return true;
            }
            catch { }
            //Test typical DateTime format. Ex: 1:00 PM
            try
            {
                DateTime.Parse(time);
                return true;
            }
            catch
            {
                return false;
            }
        }

        ///<summary></summary>
        public static bool ValidateProvider(List<ClaimProc> claimProcsForProc, long selectedProviderId, long providerIdForProc)
        {
            //validate for provider change
            if (providerIdForProc != selectedProviderId && Preference.GetBool(PreferenceName.ProcProvChangesClaimProcWithClaim))
            {
                //if selected prov is null (no selection made), no change will happen to the provider
                if (claimProcsForProc.Any(x => x.Status.In(ClaimProcStatus.Received, ClaimProcStatus.Supplemental, ClaimProcStatus.CapClaim)))
                {
                    MessageBox.Show(
                        "The provider cannot be changed when this procedure is attached to a claim.", 
                        "Procedures",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return false;
                }
            }
            return true;
        }

        ///<summary>Only needs to be called when procOld.ProcStatus is C, EO or EC.</summary>
        public static bool CheckPermissionsAndGlobalLockDate(Procedure procOld, Procedure procNew, DateTime procDate, double procFeeOverride = double.MinValue)
        {
            if (!procOld.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC))
            {//that was already complete
                return true;
            }
            //It's not possible for the user to get to this point unless they have permission for ProcComplEditLimited based on the DateEntryC.
            //The following 2 checks are not redundant because they check different dates.
            //ProcComplEditLimited does not check Global Lock date but checks the permission specific date. 
            DateTime dateToUseProcOld = procOld.ProcDate;
            if (procOld.ProcStatus.In(ProcStat.EC, ProcStat.EO))
            {
                dateToUseProcOld = DateTime.Today;//ignore date limitation for EO/EC procedures on Edit Completed Procedure (limited) permissions.
            }
            if (!Security.IsAuthorized(Permissions.ProcComplEditLimited, dateToUseProcOld))
            {//block old date
                return false;
            }
            if (procNew.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC))
            {
                DateTime dateToUseProcCur = procDate;
                if (procNew.ProcStatus.In(ProcStat.EC, ProcStat.EO))
                {
                    //ignore date limitation for EO/EC procedures on Edit Completed Procedure (full) and Edit Completed Procedure (limited) permissions.
                    dateToUseProcCur = DateTime.Today;
                }

                if (!Security.IsAuthorized(Permissions.ProcComplEditLimited, dateToUseProcCur))
                {//block new date, too
                    return false;
                }

                double procFee = procNew.ProcFee;
                if (procFeeOverride != double.MinValue)
                {
                    procFee = procFeeOverride;
                }
                if (procOld.ProcDate != procDate //If user changed the procedure date
                    || !procOld.ProcFee.IsEqual(procFee) //If user changed the procedure fee
                    || procOld.CodeNum != procNew.CodeNum) //If user changed the procedure code
                {
                    string perm = Permissions.EditCompletedProcedure;
                    if (procNew.ProcStatus.In(ProcStat.EO, ProcStat.EC))
                    {
                        perm = Permissions.EditProcedure;
                    }
                    if (!Security.IsAuthorized(perm, procDate, procNew.CodeNum, procFee))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
