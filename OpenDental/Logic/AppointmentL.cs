using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class AppointmentL
    {
        /// <summary>
        /// The date currently selected in the appointment module.
        /// The first date of the week when using 'Week' view.
        /// </summary>
        public static DateTime DateSelected;

        /// <summary>
        /// Used by UI when it needs a recall appointment placed on the pinboard ready to schedule.
        /// This method creates the appointment and attaches all appropriate procedures. It's up to
        /// the calling class to then place the appointment on the pinboard. If the appointment 
        /// doesn't get scheduled, it's important to delete it. If a <paramref name="recallId"/> is
        /// not 0 or -1, then it will create an appointment of that recall type. Otherwise it will
        /// only use either a Perio or Prophy recall type.
        /// </summary>
        public static Appointment CreateRecallApt(Patient patient, List<InsPlan> insPlans, long? recallId, List<InsSub> insSubs, DateTime appointmentDateTime = default)
        {
            var recalls = Recalls.GetList(patient.PatNum);

            Recall recall = null;

            if (recallId.HasValue)
            {
                recall = Recalls.GetRecall(recallId.Value);
            }
            else
            {
                foreach (var r in recalls)
                {
                    if (r.RecallTypeNum == RecallTypes.PerioType || r.RecallTypeNum == RecallTypes.ProphyType)
                    {
                        if (!r.IsDisabled)
                        {
                            recall = r;
                        }
                        break;
                    }
                }
            }

            if (recall == null)
                throw new ApplicationException("No special type recall is due.");
            
            if (recall.DateScheduled.Date > DateTime.Today)
                throw new ApplicationException("Recall has already been scheduled for " + recall.DateScheduled.ToShortDateString());
            
            var appointment = new Appointment
            {
                AptDateTime = appointmentDateTime
            };
            var procedureCodes = RecallTypes.GetProcs(recall.RecallTypeNum);

            var procedures = Appointments.FillAppointmentForRecall(appointment, recall, recalls, patient, procedureCodes, insPlans, insSubs);
            foreach (var procedure in procedures)
            {
                //if (Programs.UsingOrion)
                //{
                //    using (var formProcEdit = new FormProcEdit(procedure, patient, Patients.GetFamily(patient.PatNum)))
                //    {
                //        formProcEdit.IsNew = true;
                //        if (formProcEdit.ShowDialog() == DialogResult.Cancel)
                //        {
                //            try
                //            {
                //                Procedures.Delete(procedure.ProcNum);
                //            }
                //            catch (Exception exception)
                //            {
                //                MessageBox.Show(
                //                    exception.Message, 
                //                    "Appointments", 
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
                //            }
                //        }
                //    }
                //}
            }
            return appointment;
        }

        /// <summary>
        /// Returns true if <see cref="PreferenceName.BrokenApptProcedure"/> is set.
        /// </summary>
        public static bool HasBrokenApptProcs() =>
            Preference.GetLong(PreferenceName.BrokenApptProcedure) > 0;

        /// <summary>
        /// Sets status of given <paramref name="appointment"/> to broken. Provide procCode that
        /// should be charted, can be null but will not chart a broken procedure. Also considers 
        /// various broken procedure based prefs. Makes its own securitylog entries.
        /// </summary>
        public static void BreakApptHelper(Appointment appointment, Patient patient, ProcedureCode procedureCode)
        {
            DateTime datePrevious = appointment.DateTStamp;

            // Suppress history is true due to below logic creating a log with a specific HistAppointmentAction instead of the generic changed.
            bool suppressHistory = false;
            if (procedureCode != null)
            {
                suppressHistory = procedureCode.ProcCode.In("D9986", "D9987");
            }

            Appointments.SetAptStatus(appointment, ApptStatus.Broken, suppressHistory);
            if (appointment.AptStatus != ApptStatus.Complete)
            { 
                SecurityLog.Write(patient.PatNum, Permissions.AppointmentEdit,
                    $"{appointment.ProcDescript}, {appointment.AptDateTime}, Broken from the appointments module.", appointment.AptNum, datePrevious);
            }
            else
            {
                SecurityLog.Write(patient.PatNum, Permissions.AppointmentCompleteEdit,
                    $"{appointment.ProcDescript}, {appointment.AptDateTime}, Broken from the appointments module.", appointment.AptNum, datePrevious);
            }

            #region HL7

            //If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
            if (HL7Defs.IsExistingHL7Enabled())
            {
                // S15 - Appt Cancellation event
                var hl7message = MessageConstructor.GenerateSIU(patient, Patients.GetPat(patient.Guarantor), EventTypeHL7.S15, appointment);
               
                // Will be null if there is no outbound SIU message defined, so do nothing
                if (hl7message != null)
                {
                    HL7Msgs.Insert(new HL7Msg
                    {
                        AptNum = appointment.AptNum,
                        HL7Status = HL7MessageStatus.OutPending,//it will be marked outSent by the HL7 service.
                        MsgText = hl7message.ToString(),
                        PatNum = patient.PatNum
                    });
                }
            }

            #endregion

            #region Charting the proc
            if (procedureCode != null)
            {
                switch (procedureCode.ProcCode)
                {
                    case "D9986"://Missed
                        HistAppointments.CreateHistoryEntry(appointment.AptNum, HistAppointmentAction.Missed);
                        break;

                    case "D9987"://Cancelled
                        HistAppointments.CreateHistoryEntry(appointment.AptNum, HistAppointmentAction.Cancelled);
                        break;
                }

                var procedure = new Procedure
                {
                    PatNum = patient.PatNum,
                    ProvNum = procedureCode.ProvNumDefault > 0 ? procedureCode.ProvNumDefault : appointment.ProvNum,
                    CodeNum = procedureCode.CodeNum,
                    ProcDate = DateTime.Today,
                    DateEntryC = DateTime.Now,
                    ProcStatus = ProcStat.C,
                    ClinicNum = appointment.ClinicNum,
                    UserNum = Security.CurrentUser.Id,
                    Note = $"Appt BROKEN for {appointment.ProcDescript}  {appointment.AptDateTime}",
                    PlaceService = (PlaceOfService)Preference.GetInt(PreferenceName.DefaultProcedurePlaceService)//Default proc place of service for the Practice is used. 
                };

                List<InsSub> listInsSubs = InsSubs.RefreshForFam(Patients.GetFamily(patient.PatNum));
                List<InsPlan> listInsPlans = InsPlans.RefreshForSubList(listInsSubs);
                List<PatPlan> listPatPlans = PatPlans.Refresh(patient.PatNum);
                InsPlan insPlanPrimary = null;
                if (listPatPlans.Count > 0)
                {
                    InsSub insSubPrimary = InsSubs.GetSub(listPatPlans[0].InsSubNum, listInsSubs);
                    insPlanPrimary = InsPlans.GetPlan(insSubPrimary.PlanNum, listInsPlans);
                }
                double procFee;
                long feeSch;
                if (insPlanPrimary == null || procedureCode.NoBillIns)
                {
                    feeSch = FeeScheds.GetFeeSched(0, patient.FeeSched, procedure.ProvNum);
                }
                else
                {//Only take into account the patient's insurance fee schedule if the D9986 procedure is not marked as NoBillIns
                    feeSch = FeeScheds.GetFeeSched(insPlanPrimary.FeeSched, patient.FeeSched, procedure.ProvNum);
                }
                procFee = Fees.GetAmount0(procedure.CodeNum, feeSch, procedure.ClinicNum, procedure.ProvNum);
                if (insPlanPrimary != null && insPlanPrimary.PlanType == "p" && !insPlanPrimary.IsMedical)
                {//PPO
                    double provFee = Fees.GetAmount0(procedure.CodeNum, Provider.GetById(procedure.ProvNum).FeeScheduleId, procedure.ClinicNum,
                    procedure.ProvNum);
                    procedure.ProcFee = Math.Max(provFee, procFee);
                }
                else
                {
                    procedure.ProcFee = procFee;
                }
                if (!Preference.GetBool(PreferenceName.EasyHidePublicHealth))
                {
                    procedure.SiteNum = patient.SiteNum;
                }
                Procedures.Insert(procedure);
                //Now make a claimproc if the patient has insurance.  We do this now for consistency because a claimproc could get created in the future.
                List<Benefit> listBenefits = Benefits.Refresh(listPatPlans, listInsSubs);
                List<ClaimProc> listClaimProcsForProc = ClaimProcs.RefreshForProc(procedure.ProcNum);
                Procedures.ComputeEstimates(procedure, patient.PatNum, listClaimProcsForProc, false, listInsPlans, listPatPlans, listBenefits, patient.Age, listInsSubs);
                using (var formProcBroken = new FormProcBroken(procedure))
                {
                    formProcBroken.IsNew = true;
                    formProcBroken.ShowDialog();
                }
            }
            #endregion

            #region BrokenApptAdjustment

            if (Preference.GetBool(PreferenceName.BrokenApptAdjustment))
            {
                var adjustment = new Adjustment
                {
                    DateEntry = DateTime.Today,
                    AdjDate = DateTime.Today,
                    ProcDate = DateTime.Today,
                    ProvNum = appointment.ProvNum,
                    PatNum = patient.PatNum,
                    AdjType = Preference.GetLong(PreferenceName.BrokenAppointmentAdjustmentType),
                    ClinicNum = appointment.ClinicNum
                };

                using (FormAdjust formAdjust = new FormAdjust(patient, adjustment))
                {
                    formAdjust.IsNew = true;
                    formAdjust.ShowDialog();
                }
            }

            #endregion

            #region BrokenApptCommLog

            if (Preference.GetBool(PreferenceName.BrokenApptCommLog))
            {
                Commlog CommlogCur = new Commlog
                {
                    PatNum = patient.PatNum,
                    CommDateTime = DateTime.Now,
                    CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT),
                    Note = $"Appt BROKEN for {appointment.ProcDescript}  {appointment.AptDateTime}",
                    Mode_ = CommItemMode.None,
                    UserNum = Security.CurrentUser.Id
                };

                using (var formCommItem = new FormCommItem(CommlogCur))
                {
                    formCommItem.IsNew = true;
                    formCommItem.ShowDialog();
                }
            }

            #endregion

            AppointmentEvent.Fire(ODEventType.AppointmentEdited, appointment);
            AutomationL.Trigger(AutomationTrigger.BreakAppointment, null, patient.PatNum);
            Recalls.SynchScheduledApptFull(appointment.PatNum);
        }

        public static bool ValidateApptToPinboard(Appointment appointment)
        {
            if (!Security.IsAuthorized(Permissions.AppointmentMove))
                return false;

            if (appointment.AptStatus == ApptStatus.Complete)
            {
                MessageBox.Show(
                    "Not allowed to move completed appointments.",
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (PatRestrictionL.IsRestricted(appointment.PatNum, PatRestrict.ApptSchedule))
                return false;

            return true;
        }

        /// <summary>
        /// Helper method to send given appt to pinboard.
        /// Refreshes Appointment module.
        /// Also does some appointment and security validation.
        /// </summary>
        public static void CopyAptToPinboardHelper(Appointment appt)
        {
            GotoModule.PinToAppt(new List<long>() { appt.AptNum }, appt.PatNum);
        }

        public static bool ValidateApptUnsched(Appointment appt)
        {
            if ((appt.AptStatus != ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentMove)) || 
                (appt.AptStatus == ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit)))
                return false;

            if (PatRestrictionL.IsRestricted(appt.PatNum, PatRestrict.ApptSchedule))
                return false;

            if (appt.AptStatus == ApptStatus.PtNote | appt.AptStatus == ApptStatus.PtNoteCompleted)
                return false;

            return true;
        }

        /// <summary>
        /// Helper method to send given appt to the unscheduled list.
        /// Creates SecurityLogs and considers HL7.
        /// </summary>
        public static void SetApptUnschedHelper(Appointment appointment, Patient patient = null, bool fireApptEvent = true)
        {
            DateTime previousDateTime = appointment.DateTStamp;

            Appointments.SetAptStatus(appointment, ApptStatus.UnschedList);
            
            if (appointment.AptStatus != ApptStatus.Complete)
            {
                SecurityLog.Write(appointment.PatNum, Permissions.AppointmentMove,
                    $"{appointment.ProcDescript}, {appointment.AptDateTime}, Sent to Unscheduled List",
                    appointment.AptNum, previousDateTime);
            }
            else
            {
                SecurityLog.Write(appointment.PatNum, Permissions.AppointmentCompleteEdit,
                    $"{appointment.ProcDescript}, {appointment.AptDateTime}, Sent to Unscheduled List",
                    appointment.AptNum, previousDateTime);
            }

            #region HL7

            // If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined.
            if (HL7Defs.IsExistingHL7Enabled())
            {
                patient = patient ?? Patients.GetPat(appointment.PatNum);

                // S15 - Appt Cancellation event
                MessageHL7 hl7message = MessageConstructor.GenerateSIU(patient, Patients.GetPat(patient.Guarantor), EventTypeHL7.S15, appointment);
                
                // Will be null if there is no outbound SIU message defined, so do nothing
                if (hl7message != null)
                {
                    HL7Msgs.Insert(new HL7Msg
                    {
                        AptNum = appointment.AptNum,
                        HL7Status = HL7MessageStatus.OutPending,
                        MsgText = hl7message.ToString(),
                        PatNum = patient.PatNum
                    });
                }
            }

            #endregion

            if (fireApptEvent)
            {
                AppointmentEvent.Fire(ODEventType.AppointmentEdited, appointment);
            }

            Recalls.SynchScheduledApptFull(appointment.PatNum);
        }

        /// <summary>
        /// Creats a new appointment for the given patient. A valid patient must be passed in.
        /// Set <paramref name="useApptDrawingSettings"/> to true if the user double clicked on the
        /// appointment schedule in order to make a new appointment. It will utilize the global 
        /// static properties to help set required fields for "Scheduled" appointments. Otherwise, 
        /// simply sets the corresponding PatNum and then the status to "Unscheduled".
        /// </summary>
        public static Appointment MakeNewAppointment(Patient patient, bool useApptDrawingSettings)
        {
            // Appointments.MakeNewAppointment may or may not use apptDateTime depending on useApptDrawingSettings,
            // however it's safer to just pass in the appropriate datetime verses DateTime.MinVal.
            DateTime apptDateTime = GetApptDrawingSelectedDateTime();

            // Make the appointment in memory
            var appointment = Appointments.MakeNewAppointment(patient, apptDateTime, ContrAppt.SheetClickedonOp, useApptDrawingSettings);
            if (patient.AskToArriveEarly > 0 && useApptDrawingSettings)
            {
                MessageBox.Show(
                    $"Ask patient to arrive {patient.AskToArriveEarly} minutes early at {appointment.DateTimeAskedToArrive.ToShortTimeString()}.",
                    "Appointment", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            return appointment;
        }

        /// <summary>
        /// Returns the DateTime that the user has clicked on in the ContrAppt. Should only be used
        /// after a double click in the appt module.
        /// </summary>
        public static DateTime GetApptDrawingSelectedDateTime()
        {
            DateTime dateTime = 
                ApptDrawing.IsWeeklyView ? 
                    ContrAppt.WeekStartDate.AddDays(ContrAppt.SheetClickedonDay) : 
                    AppointmentL.DateSelected;

            int minutes = (ContrAppt.SheetClickedonMin / ApptDrawing.MinPerIncr) * ApptDrawing.MinPerIncr;

            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, ContrAppt.SheetClickedonHour, minutes, 0);
        }

        /// <summary>
        /// Returns true if the user switched to a different patient.
        /// </summary>
        public static bool PromptForMerge(Patient patient, out Patient newPatient)
        {
            newPatient = patient;
            if (patient == null)
            {
                return false;
            }

            var patientLinks = PatientLinks.GetLinks(patient.PatNum, PatientLinkType.Merge);
            if (!PatientLinks.WasPatientMerged(patient.PatNum, patientLinks))
            {
                return false;
            }

            // This patient has been merged before. Get a list of all patients that this patient has been merged into.
            var linkedPatients = Patients.GetMultPats(patientLinks.Where(x => x.PatNumTo != patient.PatNum).Select(x => x.PatNumTo).ToList());

            // Notify the user that the currently selected patient has been merged before and then ask them if they want to switch to the correct patient.
            foreach (var linkedPatient in linkedPatients)
            {
                if (patient.PatStatus == PatientStatus.Patient || patient.PatStatus == PatientStatus.Inactive)
                {
                    var result =
                        MessageBox.Show(
                            "The currently selected patient has been merged into another patient.\r\nSwitch to patient " + linkedPatient.GetNameLF() + " #" + linkedPatient.PatNum.ToString() + "?",
                            "",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        newPatient = linkedPatient;
                        return true;
                    }
                }
            }

            // The user has declined every possible patient that the current patient was merged to.
            // Let them keep the merge from patient selected.

            return false;
        }

        public static PlannedApptStatus CreatePlannedAppt(Patient patient, int itemOrder, List<long> preSelectedProcedureIds = null)
        {
            if (patient == null)
            {
                MessageBox.Show(
                    "Error creating planned appointment. No patient is currently selected.", 
                    "Appointment", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return PlannedApptStatus.Failure;
            }

            if (!Security.IsAuthorized(Permissions.AppointmentCreate))
                return PlannedApptStatus.Failure;

            if (PatRestrictionL.IsRestricted(patient.PatNum, PatRestrict.ApptSchedule))
                return PlannedApptStatus.Failure;

            if (PromptForMerge(patient, out patient))
                FormOpenDental.S_Contr_PatientSelected(patient, true, false);

            if (patient.PatStatus == PatientStatus.Archived || patient.PatStatus == PatientStatus.Deceased)
            {
                MessageBox.Show(
                    "Appointments cannot be scheduled for " + patient.PatStatus.ToString().ToLower() + " patients.",
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return PlannedApptStatus.Failure;
            }

            var appointment = new Appointment
            {
                PatNum = patient.PatNum,
                ProvNum = patient.PriProv,
                ClinicNum = patient.ClinicNum,
                AptStatus = ApptStatus.Planned,
                AptDateTime = DateTimeOD.Today
            };

            var procedures = Procedures.GetManyProc(preSelectedProcedureIds, false);

            appointment.Pattern = Appointments.CalculatePattern(patient, appointment.ProvNum, appointment.ProvHyg, procedures);
            appointment.TimeLocked = Preference.GetBool(PreferenceName.AppointmentTimeIsLocked);
            Appointments.Insert(appointment);

            PlannedAppts.Insert(new PlannedAppt
            {
                AptNum = appointment.AptNum,
                PatNum = patient.PatNum,
                ItemOrder = itemOrder
            });

            using (var formApptEdit = new FormApptEdit(appointment.AptNum, listPreSelectedProcNums: preSelectedProcedureIds))
            {
                formApptEdit.IsNew = true;
                if (formApptEdit.ShowDialog() != DialogResult.OK)
                {
                    return PlannedApptStatus.FillGridNeeded;
                }

                // Only set the appointment hygienist to this patient's secondary provider if one was not manually set within the edit window.
                if (appointment.ProvHyg < 1)
                {
                    List<Procedure> myProcList = Procedures.GetProcsForSingle(appointment.AptNum, true);
                    bool allProcsHyg = (myProcList.Count > 0 && myProcList.Select(x => ProcedureCodes.GetProcCode(x.CodeNum)).ToList().All(x => x.IsHygiene));
                    
                    // Automatically set the appointments hygienist to the secondary provider of the patient if one is set.
                    if (allProcsHyg && patient.SecProv != 0)
                    {
                        Appointment aptOld = appointment.Copy();
                        appointment.ProvNum = patient.SecProv;
                        Appointments.Update(appointment, aptOld);
                    }
                }

                Patient patOld = patient.Copy();
                patient.PlannedIsDone = false;
                Patients.Update(patient, patOld);

                FormOpenDental.S_RefreshCurrentModule(isClinicRefresh: false);//if procs were added in appt, then this will display them

                return PlannedApptStatus.Success;
            }
        }

        /// <summary>
        /// Checks for specialty mismatch between patient and op. 
        /// Then prompts user according to behavior defined by <see cref="PreferenceName.ApptSchedEnforceSpecialty"/>.
        /// Returns true if the Appointment is allowed to be scheduled, false otherwise.
        /// </summary>
        public static bool IsSpecialtyMismatchAllowed(long patientId, long clinicId)
        {
            try
            {
                Appointments.HasSpecialtyConflict(patientId, clinicId);//throws exception if we need to prompt user
            }
            catch (ODException exception)
            {
                switch ((ApptSchedEnforceSpecialty)exception.ErrorCode)
                {
                    case ApptSchedEnforceSpecialty.Warn:
                        var result =
                            MessageBox.Show(
                                exception.Message + "\r\nSchedule appointment anyway?",
                                "Specialty Mismatch",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                        if (result == DialogResult.No) return false;
                        break;

                    case ApptSchedEnforceSpecialty.Block:
                        MessageBox.Show(
                            exception.Message, 
                            "Specialty Mismatch", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tests the appointment to see if it is acceptable to send it to the pinboard.
        /// Also asks user appropriate questions to verify that's what they want to do.
        /// Returns false if it will not be going to pinboard after all.
        /// </summary>
        public static bool OKtoSendToPinboard(ApptOther currentApptOther, List<ApptOther> apptOthers)
        {
            if (currentApptOther.AptStatus == ApptStatus.Planned)
            {
                bool isAlreadyScheduled = false;

                foreach (var apptOther in apptOthers)
                {
                    if (apptOther.NextAptNum == currentApptOther.AptNum)
                    {
                        isAlreadyScheduled = true;

                        break;
                    }
                }

                if (isAlreadyScheduled)
                {
                    var result =
                        MessageBox.Show(
                            "The planned appointment is already scheduled. Do you wish to continue?",
                            "Appointment",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.No) return false;
                }
            }
            else
            {//if appointment is not Planned
                switch (currentApptOther.AptStatus)
                {
                    case ApptStatus.Complete:
                        MessageBox.Show(
                            "Not allowed to move a completed appointment from here.",
                            "Appointment", 
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;

                    case ApptStatus.Scheduled:
                        var result =
                            MessageBox.Show(
                                "Do you really want to move a previously scheduled appointment?",
                                "Appointment",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                        if (result == DialogResult.No) return false;
                        break;

                    case ApptStatus.Broken://status gets changed after dragging off pinboard.
                    case ApptStatus.None:
                    case ApptStatus.UnschedList://status gets changed after dragging off pinboard.
                        break;
                }
            }

            // If it's a planned appointment, the planned appointment will end up on the pinboard.
            // The copy will be made after dragging it off the pinboard.

            return true;
        }
    }

    public enum PlannedApptStatus
    {
        /// <summary>
        /// Used when failed validation.
        /// </summary>
        Failure,

        /// <summary>
        /// Used when planned appt was created.
        /// </summary>
        Success,

        /// <summary>
        /// Used when planned appt was not created but we might need to fill a grid.
        /// </summary>
        FillGridNeeded
    }
}
