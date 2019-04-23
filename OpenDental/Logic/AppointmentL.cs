﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;
using CodeBase;
using System.Linq;

namespace OpenDental
{
    public class AppointmentL
    {
        ///<summary>The date currently selected in the appointment module.  The first date of the week when using 'Week' view.</summary>
        public static DateTime DateSelected;

        private static int compareOpsByOpNum(Operatory op1, Operatory op2)
        {
            return (int)op1.OperatoryNum - (int)op2.OperatoryNum;
        }

        ///<summary>Used by UI when it needs a recall appointment placed on the pinboard ready to schedule.  This method creates the appointment and 
        ///attaches all appropriate procedures.  It's up to the calling class to then place the appointment on the pinboard.  If the appointment doesn't 
        ///get scheduled, it's important to delete it.  If a recallNum is not 0 or -1, then it will create an appt of that recalltype. Otherwise it will
        ///only use either a Perio or Prophy recall type.</summary>
        public static Appointment CreateRecallApt(Patient patCur, List<InsPlan> planList, long recallNum, List<InsSub> subList
            , DateTime aptDateTime = default(DateTime))
        {
            List<Recall> recallList = Recalls.GetList(patCur.PatNum);
            Recall recallCur = null;
            if (recallNum > 0)
            {
                recallCur = Recalls.GetRecall(recallNum);
            }
            else
            {
                for (int i = 0; i < recallList.Count; i++)
                {
                    if (recallList[i].RecallTypeNum == RecallTypes.PerioType || recallList[i].RecallTypeNum == RecallTypes.ProphyType)
                    {
                        if (!recallList[i].IsDisabled)
                        {
                            recallCur = recallList[i];
                        }
                        break;
                    }
                }
            }
            if (recallCur == null)
            {
                //Typically never happens because everyone has a recall.  However, it can happen when patients have custom recalls due
                throw new ApplicationException(Lan.g("AppointmentL", "No special type recall is due."));
            }
            if (recallCur.DateScheduled.Date > DateTime.Today)
            {
                throw new ApplicationException(Lan.g("AppointmentL", "Recall has already been scheduled for ") + recallCur.DateScheduled.ToShortDateString());
            }
            Appointment aptCur = new Appointment();
            aptCur.AptDateTime = aptDateTime;
            List<string> procs = RecallTypes.GetProcs(recallCur.RecallTypeNum);
            List<Procedure> listProcs = Appointments.FillAppointmentForRecall(aptCur, recallCur, recallList, patCur, procs, planList, subList);
            for (int i = 0; i < listProcs.Count; i++)
            {
                if (Programs.UsingOrion)
                {
                    FormProcEdit FormP = new FormProcEdit(listProcs[i], patCur.Copy(), Patients.GetFamily(patCur.PatNum));
                    FormP.IsNew = true;
                    FormP.ShowDialog();
                    if (FormP.DialogResult == DialogResult.Cancel)
                    {
                        //any created claimprocs are automatically deleted from within procEdit window.
                        try
                        {
                            Procedures.Delete(listProcs[i].ProcNum);//also deletes the claimprocs
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        //Do not synch. Recalls based on ScheduleByDate reports in Orion mode.
                        //Recalls.Synch(PatCur.PatNum);
                    }
                }
            }
            return aptCur;
        }

        ///<summary>Returns true if PrefName.BrokenApptMissedProc or PrefName.BrokenApptCancelledProc are true.</summary>
        public static bool HasBrokenApptProcs()
        {
            return PrefC.GetLong(PrefName.BrokenApptProcedure) > 0;
        }

        ///<summary>Sets given appt.AptStatus to broken.
        ///Provide procCode that should be charted, can be null but will not chart a broken procedure.
        ///Also considers various broken procedure based prefs.
        ///Makes its own securitylog entries.</summary>
        public static void BreakApptHelper(Appointment appt, Patient pat, ProcedureCode procCode)
        {
            //suppressHistory is true due to below logic creating a log with a specific HistAppointmentAction instead of the generic changed.
            DateTime datePrevious = appt.DateTStamp;
            bool suppressHistory = false;
            if (procCode != null)
            {
                suppressHistory = (procCode.ProcCode.In("D9986", "D9987"));
            }
            Appointments.SetAptStatus(appt, ApptStatus.Broken, suppressHistory); //Appointments S-Class handles Signalods
            if (appt.AptStatus != ApptStatus.Complete)
            { //seperate log entry for completed appointments.
                SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit, pat.PatNum,
                    appt.ProcDescript + ", " + appt.AptDateTime.ToString()
                    + ", Broken from the Appts module.", appt.AptNum, datePrevious);
            }
            else
            {
                SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit, pat.PatNum,
                    appt.ProcDescript + ", " + appt.AptDateTime.ToString()
                    + ", Broken from the Appts module.", appt.AptNum, datePrevious);
            }
            #region HL7
            //If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
            if (HL7Defs.IsExistingHL7Enabled())
            {
                //S15 - Appt Cancellation event
                MessageHL7 messageHL7 = MessageConstructor.GenerateSIU(pat, Patients.GetPat(pat.Guarantor), EventTypeHL7.S15, appt);
                //Will be null if there is no outbound SIU message defined, so do nothing
                if (messageHL7 != null)
                {
                    HL7Msg hl7Msg = new HL7Msg();
                    hl7Msg.AptNum = appt.AptNum;
                    hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
                    hl7Msg.MsgText = messageHL7.ToString();
                    hl7Msg.PatNum = pat.PatNum;
                    HL7Msgs.Insert(hl7Msg);
#if DEBUG
                    MessageBox.Show("Appointments", messageHL7.ToString());
#endif
                }
            }
            #endregion
            #region Charting the proc
            if (procCode != null)
            {
                switch (procCode.ProcCode)
                {
                    case "D9986"://Missed
                        HistAppointments.CreateHistoryEntry(appt.AptNum, HistAppointmentAction.Missed);
                        break;
                    case "D9987"://Cancelled
                        HistAppointments.CreateHistoryEntry(appt.AptNum, HistAppointmentAction.Cancelled);
                        break;
                }
                Procedure procedureCur = new Procedure();
                procedureCur.PatNum = pat.PatNum;
                procedureCur.ProvNum = (procCode.ProvNumDefault > 0 ? procCode.ProvNumDefault : appt.ProvNum);
                procedureCur.CodeNum = procCode.CodeNum;
                procedureCur.ProcDate = DateTime.Today;
                procedureCur.DateEntryC = DateTime.Now;
                procedureCur.ProcStatus = ProcStat.C;
                procedureCur.ClinicNum = appt.ClinicNum;
                procedureCur.UserNum = Security.CurUser.UserNum;
                procedureCur.Note = Lans.g("AppointmentEdit", "Appt BROKEN for") + " " + appt.ProcDescript + "  " + appt.AptDateTime.ToString();
                procedureCur.PlaceService = (PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used. 
                List<InsSub> listInsSubs = InsSubs.RefreshForFam(Patients.GetFamily(pat.PatNum));
                List<InsPlan> listInsPlans = InsPlans.RefreshForSubList(listInsSubs);
                List<PatPlan> listPatPlans = PatPlans.Refresh(pat.PatNum);
                InsPlan insPlanPrimary = null;
                InsSub insSubPrimary = null;
                if (listPatPlans.Count > 0)
                {
                    insSubPrimary = InsSubs.GetSub(listPatPlans[0].InsSubNum, listInsSubs);
                    insPlanPrimary = InsPlans.GetPlan(insSubPrimary.PlanNum, listInsPlans);
                }
                double procFee;
                long feeSch;
                if (insPlanPrimary == null || procCode.NoBillIns)
                {
                    feeSch = FeeScheds.GetFeeSched(0, pat.FeeSched, procedureCur.ProvNum);
                }
                else
                {//Only take into account the patient's insurance fee schedule if the D9986 procedure is not marked as NoBillIns
                    feeSch = FeeScheds.GetFeeSched(insPlanPrimary.FeeSched, pat.FeeSched, procedureCur.ProvNum);
                }
                procFee = Fees.GetAmount0(procedureCur.CodeNum, feeSch, procedureCur.ClinicNum, procedureCur.ProvNum);
                if (insPlanPrimary != null && insPlanPrimary.PlanType == "p" && !insPlanPrimary.IsMedical)
                {//PPO
                    double provFee = Fees.GetAmount0(procedureCur.CodeNum, Providers.GetProv(procedureCur.ProvNum).FeeSched, procedureCur.ClinicNum,
                    procedureCur.ProvNum);
                    procedureCur.ProcFee = Math.Max(provFee, procFee);
                }
                else
                {
                    procedureCur.ProcFee = procFee;
                }
                if (!PrefC.GetBool(PrefName.EasyHidePublicHealth))
                {
                    procedureCur.SiteNum = pat.SiteNum;
                }
                Procedures.Insert(procedureCur);
                //Now make a claimproc if the patient has insurance.  We do this now for consistency because a claimproc could get created in the future.
                List<Benefit> listBenefits = Benefits.Refresh(listPatPlans, listInsSubs);
                List<ClaimProc> listClaimProcsForProc = ClaimProcs.RefreshForProc(procedureCur.ProcNum);
                Procedures.ComputeEstimates(procedureCur, pat.PatNum, listClaimProcsForProc, false, listInsPlans, listPatPlans, listBenefits, pat.Age, listInsSubs);
                FormProcBroken FormPB = new FormProcBroken(procedureCur);
                FormPB.IsNew = true;
                FormPB.ShowDialog();
            }
            #endregion
            #region BrokenApptAdjustment
            if (PrefC.GetBool(PrefName.BrokenApptAdjustment))
            {
                Adjustment AdjustmentCur = new Adjustment();
                AdjustmentCur.DateEntry = DateTime.Today;
                AdjustmentCur.AdjDate = DateTime.Today;
                AdjustmentCur.ProcDate = DateTime.Today;
                AdjustmentCur.ProvNum = appt.ProvNum;
                AdjustmentCur.PatNum = pat.PatNum;
                AdjustmentCur.AdjType = PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType);
                AdjustmentCur.ClinicNum = appt.ClinicNum;
                FormAdjust FormA = new FormAdjust(pat, AdjustmentCur);
                FormA.IsNew = true;
                FormA.ShowDialog();
            }
            #endregion
            #region BrokenApptCommLog
            if (PrefC.GetBool(PrefName.BrokenApptCommLog))
            {
                Commlog CommlogCur = new Commlog();
                CommlogCur.PatNum = pat.PatNum;
                CommlogCur.CommDateTime = DateTime.Now;
                CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
                CommlogCur.Note = Lan.g("Appointment", "Appt BROKEN for") + " " + appt.ProcDescript + "  " + appt.AptDateTime.ToString();
                CommlogCur.Mode_ = CommItemMode.None;
                CommlogCur.UserNum = Security.CurUser.UserNum;
                FormCommItem FormCI = new FormCommItem(CommlogCur);
                FormCI.IsNew = true;
                FormCI.ShowDialog();
            }
            #endregion
            AppointmentEvent.Fire(ODEventType.AppointmentEdited, appt);
            AutomationL.Trigger(AutomationTrigger.BreakAppointment, null, pat.PatNum);
            Recalls.SynchScheduledApptFull(appt.PatNum);
        }

        public static bool ValidateApptToPinboard(Appointment appt)
        {
            if (!Security.IsAuthorized(Permissions.AppointmentMove))
            {
                return false;
            }
            if (appt.AptStatus == ApptStatus.Complete)
            {
                MsgBox.Show("Appointments", "Not allowed to move completed appointments.");
                return false;
            }
            if (PatRestrictionL.IsRestricted(appt.PatNum, PatRestrict.ApptSchedule))
            {
                return false;
            }
            return true;
        }

        /// <summary>Helper method to send given appt to pinboard.
        /// Refreshes Appointment module.
        /// Also does some appointment and security validation.</summary>
        public static void CopyAptToPinboardHelper(Appointment appt)
        {
            GotoModule.PinToAppt(new List<long>() { appt.AptNum }, appt.PatNum);
        }

        public static bool ValidateApptUnsched(Appointment appt)
        {
            if ((appt.AptStatus != ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentMove)) //seperate permissions for complete appts.
                || (appt.AptStatus == ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit)))
            {
                return false;
            }
            if (PatRestrictionL.IsRestricted(appt.PatNum, PatRestrict.ApptSchedule))
            {
                return false;
            }
            if (appt.AptStatus == ApptStatus.PtNote | appt.AptStatus == ApptStatus.PtNoteCompleted)
            {
                return false;
            }
            return true;
        }

        /// <summary>Helper method to send given appt to the unscheduled list.
        /// Creates SecurityLogs and considers HL7.</summary>
        public static void SetApptUnschedHelper(Appointment appt, Patient pat = null, bool doFireApptEvent = true)
        {
            DateTime datePrevious = appt.DateTStamp;
            Appointments.SetAptStatus(appt, ApptStatus.UnschedList); //Appointments S-Class handles Signalods
            #region SecurityLogs
            if (appt.AptStatus != ApptStatus.Complete)
            { //seperate log entry for editing completed appts.
                SecurityLogs.MakeLogEntry(Permissions.AppointmentMove, appt.PatNum,
                    appt.ProcDescript + ", " + appt.AptDateTime.ToString() + ", Sent to Unscheduled List",
                    appt.AptNum, datePrevious);
            }
            else
            {
                SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit, appt.PatNum,
                    appt.ProcDescript + ", " + appt.AptDateTime.ToString() + ", Sent to Unscheduled List",
                    appt.AptNum, datePrevious);
            }
            #endregion
            #region HL7
            //If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
            if (HL7Defs.IsExistingHL7Enabled())
            {
                if (pat == null)
                {
                    pat = Patients.GetPat(appt.PatNum);
                }
                //S15 - Appt Cancellation event
                MessageHL7 messageHL7 = MessageConstructor.GenerateSIU(pat, Patients.GetPat(pat.Guarantor), EventTypeHL7.S15, appt);
                //Will be null if there is no outbound SIU message defined, so do nothing
                if (messageHL7 != null)
                {
                    HL7Msg hl7Msg = new HL7Msg();
                    hl7Msg.AptNum = appt.AptNum;
                    hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
                    hl7Msg.MsgText = messageHL7.ToString();
                    hl7Msg.PatNum = pat.PatNum;
                    HL7Msgs.Insert(hl7Msg);
#if DEBUG
                    MessageBox.Show("Appointments", messageHL7.ToString());
#endif
                }
            }
            #endregion
            if (doFireApptEvent)
            {
                AppointmentEvent.Fire(ODEventType.AppointmentEdited, appt);
            }
            Recalls.SynchScheduledApptFull(appt.PatNum);
        }

        ///<summary>Creats a new appointment for the given patient.  A valid patient must be passed in.
        ///Set useApptDrawingSettings to true if the user double clicked on the appointment schedule in order to make a new appointment.
        ///It will utilize the global static properties to help set required fields for "Scheduled" appointments.
        ///Otherwise, simply sets the corresponding PatNum and then the status to "Unscheduled".</summary>
        public static Appointment MakeNewAppointment(Patient PatCur, bool useApptDrawingSettings)
        {
            //Appointments.MakeNewAppointment may or may not use apptDateTime depending on useApptDrawingSettings,
            //however it's safer to just pass in the appropriate datetime verses DateTime.MinVal.
            DateTime apptDateTime = GetApptDrawingSelectedDateTime();
            //Make the appointment in memory
            Appointment apptCur = Appointments.MakeNewAppointment(PatCur, apptDateTime, ContrAppt.SheetClickedonOp, useApptDrawingSettings);
            if (PatCur.AskToArriveEarly > 0 && useApptDrawingSettings)
            {
                MessageBox.Show(Lan.g("FormApptsOther", "Ask patient to arrive") + " " + PatCur.AskToArriveEarly
                    + " " + Lan.g("FormApptsOther", "minutes early at") + " " + apptCur.DateTimeAskedToArrive.ToShortTimeString() + ".");
            }
            return apptCur;
        }

        ///<summary>Returns the DateTime that the user has clicked on in the ContrAppt.
        ///Should only be used after a double click in the appt module.</summary>
        public static DateTime GetApptDrawingSelectedDateTime()
        {
            DateTime d;
            if (ApptDrawing.IsWeeklyView)
            {
                d = ContrAppt.WeekStartDate.AddDays(ContrAppt.SheetClickedonDay);
            }
            else
            {
                d = AppointmentL.DateSelected;
            }
            int minutes = (int)(ContrAppt.SheetClickedonMin / ApptDrawing.MinPerIncr) * ApptDrawing.MinPerIncr;
            return new DateTime(d.Year, d.Month, d.Day, ContrAppt.SheetClickedonHour, minutes, 0);
        }

        ///<summary>Returns true if the user switched to a different patient.</summary>
        public static bool PromptForMerge(Patient patCur, out Patient newPatCur)
        {
            newPatCur = patCur;
            if (patCur == null)
            {
                return false;
            }
            List<PatientLink> listMergedPats = PatientLinks.GetLinks(patCur.PatNum, PatientLinkType.Merge);
            if (!PatientLinks.WasPatientMerged(patCur.PatNum, listMergedPats))
            {
                return false;
            }
            //This patient has been merged before.  Get a list of all patients that this patient has been merged into.
            List<Patient> listPats = Patients.GetMultPats(listMergedPats.Where(x => x.PatNumTo != patCur.PatNum)
                    .Select(x => x.PatNumTo).ToList()).ToList();
            //Notify the user that the currently selected patient has been merged before and then ask them if they want to switch to the correct patient.
            foreach (Patient pat in listPats)
            {
                if (pat.PatStatus.In(PatientStatus.Patient, PatientStatus.Inactive)
                    && (MessageBox.Show(Lan.g("ContrAppt", "The currently selected patient has been merged into another patient.\r\n"
                    + "Switch to patient") + " " + pat.GetNameLF() + " #" + pat.PatNum.ToString() + "?", "", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    newPatCur = pat;
                    return true;
                }
            }
            //The user has declined every possible patient that the current patient was merged to.  Let them keep the merge from patient selected.
            return false;
        }

        ///<summary></summary>
        public static PlannedApptStatus CreatePlannedAppt(Patient pat, int itemOrder, List<long> listPreSelectedProcNums = null)
        {
            if (pat == null)
            {
                MsgBox.Show("Appointments", "Error creating planned appointment.  No patient is currently selected.");
                return PlannedApptStatus.Failure;
            }
            if (!Security.IsAuthorized(Permissions.AppointmentCreate))
            {
                return PlannedApptStatus.Failure;
            }
            if (PatRestrictionL.IsRestricted(pat.PatNum, PatRestrict.ApptSchedule))
            {
                return PlannedApptStatus.Failure;
            }
            if (PromptForMerge(pat, out pat))
            {
                FormOpenDental.S_Contr_PatientSelected(pat, true, false);
            }
            if (pat.PatStatus.In(PatientStatus.Archived, PatientStatus.Deceased))
            {
                MsgBox.Show("Appointments", "Appointments cannot be scheduled for " + pat.PatStatus.ToString().ToLower() + " patients.");
                return PlannedApptStatus.Failure;
            }
            Appointment AptCur = new Appointment();
            AptCur.PatNum = pat.PatNum;
            AptCur.ProvNum = pat.PriProv;
            AptCur.ClinicNum = pat.ClinicNum;
            AptCur.AptStatus = ApptStatus.Planned;
            AptCur.AptDateTime = DateTimeOD.Today;
            List<Procedure> listProcs = Procedures.GetManyProc(listPreSelectedProcNums, false);//Returns empty list if null.
                                                                                               //If listProcs is empty then AptCur.Pattern defaults to PrefName.AppointmentWithoutProcsDefaultLength value.
                                                                                               //See Appointments.GetApptTimePatternForNoProcs().
            AptCur.Pattern = Appointments.CalculatePattern(pat, AptCur.ProvNum, AptCur.ProvHyg, listProcs);
            AptCur.TimeLocked = PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
            Appointments.Insert(AptCur);
            PlannedAppt plannedAppt = new PlannedAppt();
            plannedAppt.AptNum = AptCur.AptNum;
            plannedAppt.PatNum = pat.PatNum;
            plannedAppt.ItemOrder = itemOrder;
            PlannedAppts.Insert(plannedAppt);
            FormApptEdit FormApptEdit = new FormApptEdit(AptCur.AptNum, listPreSelectedProcNums: listPreSelectedProcNums);
            FormApptEdit.IsNew = true;
            FormApptEdit.ShowDialog();
            if (FormApptEdit.DialogResult != DialogResult.OK)
            {
                return PlannedApptStatus.FillGridNeeded;
            }
            //Only set the appointment hygienist to this patient's secondary provider if one was not manually set within the edit window.
            if (AptCur.ProvHyg < 1)
            {
                List<Procedure> myProcList = Procedures.GetProcsForSingle(AptCur.AptNum, true);
                bool allProcsHyg = (myProcList.Count > 0 && myProcList.Select(x => ProcedureCodes.GetProcCode(x.CodeNum)).ToList().All(x => x.IsHygiene));
                //Automatically set the appointments hygienist to the secondary provider of the patient if one is set.
                if (allProcsHyg && pat.SecProv != 0)
                {
                    Appointment aptOld = AptCur.Copy();
                    AptCur.ProvNum = pat.SecProv;
                    Appointments.Update(AptCur, aptOld);
                }
            }
            Patient patOld = pat.Copy();
            pat.PlannedIsDone = false;
            Patients.Update(pat, patOld);
            FormOpenDental.S_RefreshCurrentModule(isClinicRefresh: false);//if procs were added in appt, then this will display them
            return PlannedApptStatus.Success;
        }

        /// <summary>Checks for specialty mismatch between pat and op. Then prompts user according to behavior defined by 
        /// PrefName.ApptSchedEnforceSpecialty.  Returns true if the Appointment is allowed to be scheduled, false otherwise.</summary>
        public static bool IsSpecialtyMismatchAllowed(long patNum, long clinicNum)
        {
            try
            {
                Appointments.HasSpecialtyConflict(patNum, clinicNum);//throws exception if we need to prompt user
            }
            catch (ODException odex)
            {//Warn/Block
                switch ((ApptSchedEnforceSpecialty)odex.ErrorCode)
                {
                    case ApptSchedEnforceSpecialty.Warn:
                        if (!MsgBox.Show("Appointment", MsgBoxButtons.YesNo, odex.Message + "\r\nSchedule appointment anyway?", "Specialty Mismatch"))
                        {
                            return false;
                        }
                        break;
                    case ApptSchedEnforceSpecialty.Block:
                        MsgBox.Show("Appointment", odex.Message, "Specialty Mismatch");
                        return false;
                }
            }
            return true;
        }

        /// <summary>Tests the appointment to see if it is acceptable to send it to the pinboard.  Also asks user appropriate questions to verify that's
        /// what they want to do.  Returns false if it will not be going to pinboard after all.</summary>
        public static bool OKtoSendToPinboard(ApptOther AptCur, List<ApptOther> listApptOthers, Control owner)
        {
            if (AptCur.AptStatus == ApptStatus.Planned)
            {//if is a Planned appointment
                bool PlannedIsSched = false;
                for (int i = 0; i < listApptOthers.Count; i++)
                {
                    if (listApptOthers[i].NextAptNum == AptCur.AptNum)
                    {//if the planned appointment is already sched
                        PlannedIsSched = true;
                    }
                }
                if (PlannedIsSched)
                {
                    if (!MsgBox.Show(owner, MsgBoxButtons.OKCancel, "The Planned appointment is already scheduled.  Do you wish to continue?"))
                    {
                        return false;
                    }
                }
            }
            else
            {//if appointment is not Planned
                switch (AptCur.AptStatus)
                {
                    case ApptStatus.Complete:
                        MsgBox.Show(owner, "Not allowed to move a completed appointment from here.");
                        return false;
                    case ApptStatus.Scheduled:
                        if (!MsgBox.Show(owner, MsgBoxButtons.OKCancel, "Do you really want to move a previously scheduled appointment?"))
                        {
                            return false;
                        }
                        break;
                    case ApptStatus.Broken://status gets changed after dragging off pinboard.
                    case ApptStatus.None:
                    case ApptStatus.UnschedList://status gets changed after dragging off pinboard.
                        break;
                }
            }
            //if it's a planned appointment, the planned appointment will end up on the pinboard.  The copy will be made after dragging it off the pinboard.
            return true;
        }
    }

    public enum PlannedApptStatus
    {
        ///<summary>1 - Used when failed validation.</summary>
        Failure,
        ///<summary>2 - Used when planned appt was created.</summary>
        Success,
        ///<summary>3 - Used when planned appt was not created but we might need to fill a grid.</summary>
        FillGridNeeded
    }

}