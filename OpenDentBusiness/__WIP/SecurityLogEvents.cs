/**
* Copyright (C) 2019 Dental Stars SRL
* Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; If not, see <http://www.gnu.org/licenses/>
*/

namespace OpenDentBusiness
{
    public static class SecurityLogEvents
    {
        //
        // Modules
        //
        public const string ModuleAppointments = "modules_appointments";
        public const string ModuleFamily = "modules_family";
        public const string ModuleAccount = "modules_account";
        public const string ModuleTreatmentPlan = "modules_treatmentplan";
        public const string ModuleChart = "modules_chart";
        public const string ModuleImages = "modules_images";
        public const string ModuleManagement = "modules_management";

        //
        // Procedures
        //
        public const string ProcedureEdited = "procedure_edited";
        public const string CompletedProcedureCreated = "completed_procedure_created";
        public const string CompletedProcedureEdited = "completed_procedure_edited";

        //
        // Appointments
        //
        public const string AppointmentEdited = "appointment_edited";
        public const string CompletedAppointmentEdited = "completed_appointment_edited";
        public const string AppointmentCreate = "appointment_create";
        public const string AppointmentMove = "appointment_move";

        //
        // Patients
        //
        public const string PatientCreated = "patient_created";

        //
        // Adjustments
        //
        public const string AdjustmentCreated = "adjustment_created";
        public const string AdjustmentEdited = "adjustment_edited";

        //
        // Treatment Plans
        //
        public const string TreatmentPlanDiscountEdited = "treatment_plan_discount_edited";



        public const string PatientFieldEdit = "patient_field_edit";
        public const string UserLogOnOff = "user_log_on_off";
        public const string PatPlanCreate = "pat_plan_create";
        public const string RecallEdit = "recall_edit";
        public const string PatientEdit = "patient_edit"; // Patient Edit
        public const string EditProcedureCode = "procedure_code_edit"; // Procedure Code Edit

        public const string ApptConfirmStatusEdit = "appt_confirm_status_edit"; // Appointment Confirmation Status Edit
        public const string PatPriProvEdit = "pat_pri_prov_edit"; // Patient Primary Provider Edit
        public const string RxCreate = "rx_create"; // Rx Create
        public const string RxEdit = "rx_edit"; // Rx Edit
        public const string InsPayCreate = "ins_pay_create"; // Insurance Payment Create

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking. 
        /// Tracks copying of patient information.
        /// Required by EHR.
        /// </summary>
        public const string Copy = "copy"; // Copy

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks printing of patient information.
        /// Required by EHR.
        /// </summary>
        public const string Printing = "printing"; // Printing

        /// <summary>
        /// There is no user interface in the security window for this permission. It is only used for tracking. FK to CodeNum.
        /// </summary>
        public const string ProcFeeEdit = "proc_fee_edit"; // Proc Fee Edit

        /// <summary>
        /// Edit fees (for logging only). Security log entry for this points to feeNum instead of CodeNum.
        /// </summary>
        public const string LogFeeEdit = "log_fee_edit";

        public const string CommandQuery = "command_query"; // Command Query
        public const string SecurityAdmin = "security_admin";
        public const string AddNewUser = "add_new_user"; // Add New User
        public const string Setup = "setup";

        /// <summary>
        /// There is no user interface in the security window for this permission. 
        /// It is only used for tracking. Currently only used for tracking automatically changing 
        /// the IsCpoe flag on procedures.  Can be enhanced to do more in the future. There is 
        /// only one place where we could have automatically changed IsCpoe without a 
        /// corresponding log of a different permission. That place is in the OnClosing of the 
        /// Procedure Edit window. We update this flag even when the user Cancels out of it.
        /// </summary>
        public const string ProcEdit = "proc_edit"; // Proc Edit

        public const string PaymentCreate = "payment.create";
        public const string Blockouts = "blockouts";
        public const string EmailSend = "email_send"; // Email Send

        /// <summary>
        /// Audit when an insurance subscriber is created.
        /// The naming convention of this permission was decided upon by Nathan and Derek based on 
        /// the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, 
        /// InsPlanChangeAssign. Currently only used in X12 834 insurance plan import.
        /// </summary>
        public const string InsPlanCreateSub = "ins_plan_create_sub"; // Insurance Plan Create Subscriber

        /// <summary>
        /// Audit when an insurance subscriber is edited. The naming convention of this permission 
        /// was decided upon by Nathan and Derek based on the following existing permissions: 
        /// InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign. Currently only used 
        /// in X12 834 insurance plan import.
        /// </summary>
        public const string InsPlanEditSub = "ins_plan_edit_sub"; // Insurance Plan Edit Subscriber

        /// <summary>
        /// Audit when a patient is added to an insurance plan. The naming convention of this 
        /// permission was decided upon by Nathan and Derek based on the following existing 
        /// permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
        /// Currently only used in X12 834 insurance plan import.
        /// </summary>
        public const string InsPlanAddPat = "ins_plan_add_pat"; // Insurance Plan Add Patient


        /// <summary>
        /// Audit trail for images and documents in the image module.
        /// There is no user interface in the security window for this permission because it is only used for tracking.
        /// </summary>
        public const string ImageEdit = "image_edit"; // Image Edit

        public const string OrthoChartEditFull = "ortho_chart_edit_full"; // Ortho Chart Edit (full)
        public const string FeeSchedEdit = "fee_sched_edit"; // Fee Schedule Edit

        /// <summary>
        /// Used to diagnose an error in statement creation. Audit Trail Permission Only
        /// </summary>
        public const string StatementPatNumMismatch = "statement_pat_num_mismatch";
        public const string ProcDelete = "procedure_deleted"; // TP Procedure Delete
        public const string AutoNoteQuickNoteEdit = "auto_note_quick_note_edit"; // Auto/Quick Note Edit
        public const string ChooseDatabase = "choose_database";
        public const string UserQuery = "user_query";
        public const string AuditTrail = "audit_trail"; // Audit Trail
        public const string PatientApptRestrict = "patient_appt_restrict"; // Patient Restriction Edit
        public const string TaskEdit = "task_edit"; // Task Edit
        public const string InsPayEdit = "ins_pay_edit"; // Insurance Payment Edit


        public const string Billing = "";
        public const string Backup = "";
        public const string TimecardDeleteEntry = "";

        /// <summary>
        /// Indicates changes were made to the patient allergies list.
        /// </summary>
        public const string PatientAlergyListEdited = "patient_alergy_list_edited";


        /// <summary>
        /// Logs when aging is being ran and from where.
        /// </summary>
        public const string AgingRan = "aging_ran"; // Aging Ran
    }
}
