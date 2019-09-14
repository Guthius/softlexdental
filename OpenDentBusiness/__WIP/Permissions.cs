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
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Defines all core system permissions.
    /// </summary>
    public static class Permissions
    {
        /// <summary>
        /// Indicates a user has accesss to the appointments module.
        /// </summary>
        public const string ModuleAppointments = "modules_appointments";
        public const string ModuleFamily = "modules_family";
        public const string ModuleAccount = "modules_account";
        public const string ModuleTreatmentPlan = "modules_treatmentplan";
        public const string ModuleChart = "modules_chart";
        public const string ModuleImages = "modules_images";
        public const string ModuleManagement = "modules_management";

        /// <summary>
        /// Indicates the user has the permission to perform setup operations.
        /// </summary>
        public const string Setup = "setup";

        public const string RxCreate = "rx_create"; // Rx Create
        public const string RxEdit = "rx_edit"; // Rx Edit

        /// <summary>
        /// Uses date restrictions. 
        /// Covers editing AND deleting of completed, EO, and EC procedures. 
        /// Deleting procedures of other statuses are covered by ProcDelete.
        /// </summary>
        public const string EditCompletedProcedure = "procedure_edit_completed"; // Edit Completed Procedure (full)

        /// <summary>
        /// Allows users with this permission to edit an existing EO or EC procedure.
        /// </summary>
        public const string EditProcedure = "procedure_edit"; // Edit EO or EC Procedures

        /// <summary>
        /// Allows users with this permission the ability to edit procedure codes.
        /// Users with the Setup permission have this by default. Logs changes made to individual proc codes
        /// (excluding fee changes) including when run from proc code tools.
        /// </summary>
        public const string EditProcedureCode = "procedure_code_edit"; // Procedure Code Edit

        /// <summary>
        /// Create Completed Procedure (or set complete)
        /// </summary>
        public const string CreateCompletedProcedure = "procedure_create_completed";

        /// <summary>
        /// Choose Database.
        /// </summary>
        public const string ChooseDatabase = "choose_database";

        /// <summary>
        /// Schedules - Practice and Provider
        /// </summary>
        public const string Schedules = "schedules";

        /// <summary>
        /// Blockouts
        /// </summary>
        public const string Blockouts = "blockouts";

        /// <summary>
        /// Claim Sent Edit, uses date restrictions.
        /// </summary>
        public const string ClaimSentEdit = "claim.edit_sent";

        /// <summary>
        /// Payment Create
        /// </summary>
        public const string PaymentCreate = "payment.create";

        /// <summary>
        /// Payment Edit
        /// </summary>
        public const string PaymentEdit = "payment.edit";

        /// <summary>
        /// Adjustment Create
        /// </summary>
        public const string AdjustmentCreate = "adjustment.create";

        /// <summary>
        /// Adjustment Edit
        /// </summary>
        public const string AdjustmentEdit = "adjustment.edit";

        /// <summary>
        /// User Query
        /// </summary>
        public const string UserQuery = "user_query";

        /// <summary>
        /// Reports
        /// </summary>
        public const string Reports = "reports";

        /// <summary>
        /// Security Admin. At least one user must have this permission.
        /// </summary>
        public const string SecurityAdmin = "security_admin";

        /// <summary>
        /// Appointment Create
        /// </summary>
        public const string AppointmentCreate = "appointment_create";

        /// <summary>
        /// Appointment Move
        /// </summary>
        public const string AppointmentMove = "appointment_move";

        /// <summary>
        /// Appointment Edit
        /// </summary>
        public const string AppointmentEdit = "appointment_edit";

        /// <summary>
        /// Backup
        /// </summary>
        public const string Backup = "backup";

        public const string TimecardsEditAll = "timecards_edit_all"; // Edit All Time Cards
        public const string DepositSlips = "deposit_slips"; // Deposit Slips
        public const string Accounting = "accounting"; // Accounting

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string AccountingEdit = "accounting.edit"; // Accounting Edit Entry 

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string AccountingCreate = "accounting.create"; // Accounting Create Entry



        public const string AnesthesiaIntakeMeds = "anesthesia_intake_meds"; // Intake Anesthetic Medications into Inventory
        public const string AnesthesiaControlMeds = "anesthesia_control_meds"; // Edit Anesthetic Records; Edit/Adjust Inventory Counts
        public const string InsPayCreate = "ins_pay_create"; // Insurance Payment Create

        /// <summary>
        /// Uses date restrictions. Edit Batch Insurance Payment.
        /// </summary>
        public const string InsPayEdit = "ins_pay_edit"; // Insurance Payment Edit

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string TreatPlanEdit = ""; // Edit Treatment Plan

        [Obsolete]
        public const string ReportProdInc = "report_prod_inc"; // Reports - Production and Income, Aging

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string TimecardDeleteEntry = "timecard_delete_entry"; // Time Card Delete Entry

        /// <summary>
        /// Uses date restrictions. All other equipment functions are covered by .Setup.
        /// </summary>
        public const string EquipmentDelete = "equipment_delete"; // Equipment Delete

        /// <summary>
        /// Uses date restrictions. Also used in audit trail to log web form importing.
        /// </summary>
        public const string SheetEdit = "sheet_edit"; // Sheet Edit

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string CommlogEdit = "commlog_edit"; // Commlog Edit

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string ImageDelete = "image_delete"; // Image Delete

        /// <summary>
        /// Uses date restrictions.
        /// </summary>
        public const string PerioEdit = "perio_edit"; // Perio Chart Edit

        /// <summary>
        /// Shows the fee textbox in the proc edit window.
        /// </summary>
        public const string ProcEditShowFee = "proc_edit_show_fee"; // Show Procedure Fee

        public const string AdjustmentEditZero = "adjustment_edit_zero"; // Adjustment Edit Zero Amount
        public const string EhrEmergencyAccess = "ehr_emergency_access"; // EHR Emergency Access
        
        /// <summary>Uses date restrictions.
        /// This only applies to non-completed procs.
        /// Deletion of completed procs is covered by ProcComplEdit.
        /// </summary>
        public const string ProcDelete = ""; // TP Procedure Delete

        [Obsolete("HQ Only")]
        public const string EhrKeyAdd = "ehr_key_add"; // Ehr Key Add

        /// <summary>
        /// Allows user to edit all providers.
        /// </summary>
        public const string Providers = "providers"; // Providers

        [Obsolete("Support for eCW will be moved.")]
        public const string EcwAppointmentRevise = "ecw_appointment_revise"; // eCW Appointment Revise
        public const string ProcedureNoteFull = "procedure_note_full"; // Procedure Note (full)
        public const string ReferralAdd = "referral_add"; // Referral Add
        public const string InsPlanChangeSubsc = "ins_plan_change_subsc"; // Insurance Plan Change Subscriber
        public const string RefAttachAdd = "ref_attach_add"; // Referral, Attach to Patient
        public const string RefAttachDelete = "ref_attach_delete"; // Referral, Delete from Patient
        public const string CarrierCreate = "carrier_create"; // Carrier Create
        public const string GraphicalReports = "graphical_reports"; // Reports - Graphical
        public const string AutoNoteQuickNoteEdit = "auto_note_quick_note_edit"; // Auto/Quick Note Edit
        public const string EquipmentSetup = "equipment_setup"; // Equipment Setup
        public const string Billing = "billing"; // Billing
        public const string ProblemEdit = "problem_edit"; // Problem Edit

        /// <summary>
        /// There is no user interface in the security window for this permission. It is only used for tracking. FK to CodeNum.
        /// </summary>
        [Obsolete("Audit.")]
        public const string ProcFeeEdit = "proc_fee_edit"; // Proc Fee Edit

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.  
        /// Only tracks changes to carriername, not any other carrier info.  
        /// FK to PlanNum for tracking.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanChangeCarrierName = ""; // TP InsPlan Change Carrier Name
        
        /// <summary>
        /// (Was named TaskEdit prior to version 14.2.39) 
        /// When editing an existing task: delete the task, edit original description, or double 
        /// click on note rows. Even if you don't have the permission, you can still edit your own 
        /// task description (but not the notes) as long as it's in your inbox and as long as 
        /// nobody but you has added any notes.
        /// </summary>
        public const string TaskNoteEdit = "task_note_edit"; // Task Note Edit
        
        /// <summary>
        /// Add or delete lists and list columns.
        /// </summary>
        public const string WikiListSetup = "wiki_list_setup"; // Wiki List Setup

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking. 
        /// Tracks copying of patient information.
        /// Required by EHR.
        /// </summary>
        [Obsolete("Audit.")]
        public const string Copy = "copy"; // Copy

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks printing of patient information.
        /// Required by EHR.
        /// </summary>
        [Obsolete("Audit.")]
        public const string Printing = "printing"; // Printing

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks viewing of patient medical information.
        /// </summary>
        [Obsolete("Audit.")]
        public const string MedicalInfoViewed = "medical_info_viewed"; // Medical Info Viewed

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks creation and editing of patient problems.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatProblemListEdit = "pat_problem_list_edit"; // Pat Problem List Edit

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks creation and edting of patient medications.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatMedicationListEdit = "pat_medication_list_edit"; // Pat Medication List Edit

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Tracks creation and editing of patient allergies.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatAllergyListEdit = "pat_allergy_list_edit"; // Pat Allergy List Edit

        /// <summary>
        /// There is no user interface in the security window for this permission. 
        /// It is only used for tracking.
        /// Tracks creation and editing of patient family health history.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatFamilyHealthEdit = "pat_family_health_edit"; // Pat Family Health Edit

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Patient Portal access of patient information.
        /// Required by EHR.
        /// </summary>
        public const string PatientPortal = "patient_portal"; // Patient Portal

        /// <summary>
        /// Assign this permission to a staff person who will administer setting up and editing Dental School Students in the system.
        /// </summary>
        [Obsolete("Support for dental schools will be removed.")]
        public const string AdminDentalStudents = "admin_dental_students"; // Student Edit
        
        /// <summary>
        /// Assign this permission to an instructor who will be allowed to assign Grades to Dental
        /// School Students as well as manage classes assigned to them.
        /// </summary>
        [Obsolete("Support for dental schools will be removed.")]
        public const string AdminDentalInstructors = "admin_dental_instructors"; // Instructor Edit
        
        /// <summary>
        /// Uses date restrictions.
        /// Has a unique audit trail so that users can track specific ortho chart edits.
        /// FK to OrthoChartNum.
        /// </summary>
        public const string OrthoChartEditFull = "ortho_chart_edit_full"; // Ortho Chart Edit (full)

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// Mainly used for ortho clinics.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatientFieldEdit = "patient_field_edit"; // Patient Field Edit

        /// <summary>
        /// Assign this permission to a staff person who will edit evaluations in case of an emergency.
        /// This is not meant to be a permanent permission given to a group.
        /// </summary>
        public const string AdminDentalEvaluations = "admin_dental_evaluations"; // Admin Evaluation Edit

        /// <summary>
        /// There is no user interface in the security window for this permission. 
        /// It is only used for tracking.
        /// </summary>
        [Obsolete("Audit.")]
        public const string TreatPlanDiscountEdit = "treat_plan_discount_edit"; // Treat Plan Discount Edit

        /// <summary>
        /// There is no user interface in the security window for this permission.
        /// It is only used for tracking.
        /// </summary>
        [Obsolete("Audit.")]
        public const string UserLogOnOff = "user_log_on_off";

        /// <summary>
        /// Allows user to edit other users' tasks.
        /// </summary>
        public const string TaskEdit = "task_edit"; // Task Edit

        /// <summary>
        /// Allows user to send unsecured email.
        /// </summary>
        public const string EmailSend = "email_send"; // Email Send

        /// <summary>
        /// Allows user to send webmail.
        /// </summary>
        [Obsolete("WebMail is not supported.")]
        public const string WebMailSend = "webmail_send"; // Webmail Send

        /// <summary>
        /// Allows user to run, edit, and write non-released queries.
        /// </summary>
        public const string UserQueryAdmin = "user_query_admin"; // User Query Admin

        /// <summary>
        /// Security permission for assignment of benefits.
        /// </summary>
        public const string InsPlanChangeAssign = "ins_plan_change_assign"; // Insurance Plan Change Assignment of Benefits

        /// <summary>
        /// Audit trail for images and documents in the image module.
        /// There is no user interface in the security window for this permission because it is only used for tracking.
        /// </summary>
        [Obsolete("Audit.")]
        public const string ImageEdit = "image_edit"; // Image Edit

        /// <summary>
        /// Allows editing of all measure events.  Also used to track changes made to events.
        /// </summary>
        public const string EhrMeasureEventEdit = "ehr_measure_event_edit"; // EHR Measure Event Edit

        /// <summary>
        /// Allows users to edit settings in the eServices Setup window.
        /// Also causes the Listener Service monitor thread to start upon logging in.
        /// </summary>
        [Obsolete("OpenDental eServices are not supported.")]
        public const string EServicesSetup = "eservices_setup"; // eServices Setup
        
        /// <summary>
        /// Allows users to edit Fee Schedules throughout the program.
        /// Logs editing of fee schedule properties.
        /// </summary>
        public const string FeeSchedEdit = "fee_sched_edit"; // Fee Schedule Edit
        
        ///<summary>93- Allows user to edit and delete provider specific fees overrides.</summary>
        public const string ProviderFeeEdit = "provider_fee_edit"; // Provider Fee Edit
        
        /// <summary>
        /// Allows user to merge patients.
        /// </summary>
        public const string PatientMerge = "patient_merge"; // Patient Merge
        
        /// <summary>
        /// Only used in Claim History Status Edit
        /// </summary>
        public const string ClaimHistoryEdit = "claim_history_edit"; // Claim History Edit
        
        /// <summary>
        /// Allows user to edit a completed appointment.
        /// </summary>
        public const string AppointmentCompleteEdit = "appointment_completed_edit"; // Completed Appointment Edit

        /// <summary>
        /// Audit trail for deleting webmail messages.
        /// There is no user interface in the security window for this permission.
        /// </summary>
        [Obsolete("Audit.")]
        public const string WebMailDelete = "webmail_delete"; // Webmail Delete

        /// <summary>
        /// Audit trail for saving a patient with required fields missing.
        /// There is no user interface in the security window for this permission.
        /// </summary>
        [Obsolete("Audit.")]
        public const string RequiredFields = "required_fields"; // Required Fields Missing

        /// <summary>
        /// Allows user to merge referrals.
        /// </summary>
        public const string ReferralMerge = "referral_merge"; // Referral Merge

        /// <summary>
        /// There is no user interface in the security window for this permission. 
        /// It is only used for tracking. Currently only used for tracking automatically changing 
        /// the IsCpoe flag on procedures.  Can be enhanced to do more in the future. There is 
        /// only one place where we could have automatically changed IsCpoe without a 
        /// corresponding log of a different permission. That place is in the OnClosing of the 
        /// Procedure Edit window. We update this flag even when the user Cancels out of it.
        /// </summary>
        [Obsolete("Audit.")]
        public const string ProcEdit = "proc_edit"; // Proc Edit

        /// <summary>
        /// Allows user to use the provider merge tool.
        /// </summary>
        public const string ProviderMerge = "provider_merge"; // Provider Merge

        /// <summary>
        /// Allows user to use the medication merge tool.
        /// </summary>
        public const string MedicationMerge = "medication_merge"; // Medication Merge

        /// <summary>
        /// Allow users to use the Quick Add tool in the Account module.
        /// </summary>
        public const string AccountProcsQuickAdd = "account_procs_quick_add"; // Account Procs Quick Add

        /// <summary>
        /// Allow users to send claims.
        /// </summary>
        public const string ClaimSend = "claim_send"; // Claim Send

        /// <summary>
        /// Allow users to create new task lists.
        /// </summary>
        public const string TaskListCreate = "task_list_create"; // TaskList Create

        /// <summary>
        /// Audit when a new patient is added.
        /// </summary>
        [Obsolete("Only for tracking.")]
        public const string PatientCreate = "patient_create";
       
        /// <summary>
        /// Allows changing the settings for graphical repots.
        /// </summary>
        public const string GraphicalReportSetup = "graphic_report_setup"; // Reports - Graphical Setup

        /// <summary>
        /// Audit when a patient is edited.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatientEdit = "patient_edit"; // Patient Edit

        /// <summary>
        /// Audit when an insurance plan is created. 
        /// Currently only used in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanCreate = "ins_plan_create"; // Insurance Plan Create

        /// <summary>
        /// Audit when an insurance plan is edited. 
        /// Currently only used in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanEdit = "ins_plan_edit"; // Insurance Plan Edit

        /// <summary>
        /// Audit when an insurance subscriber is created.
        /// The naming convention of this permission was decided upon by Nathan and Derek based on 
        /// the following existing permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, 
        /// InsPlanChangeAssign. Currently only used in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanCreateSub = "ins_plan_create_sub"; // Insurance Plan Create Subscriber

        /// <summary>
        /// Audit when an insurance subscriber is edited. The naming convention of this permission 
        /// was decided upon by Nathan and Derek based on the following existing permissions: 
        /// InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign. Currently only used 
        /// in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanEditSub = "ins_plan_edit_sub"; // Insurance Plan Edit Subscriber

        /// <summary>
        /// Audit when a patient is added to an insurance plan. The naming convention of this 
        /// permission was decided upon by Nathan and Derek based on the following existing 
        /// permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign.
        /// Currently only used in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanAddPat = "ins_plan_add_pat"; // Insurance Plan Add Patient

        /// <summary>
        /// Audit when a patient is dropped from an insurance plan. The naming convention of this 
        /// permission was decided upon by Nathan and Derek based on the following existing 
        /// permissions: InsPlanChangeSubsc, InsPlanChangeCarrierName, InsPlanChangeAssign. 
        /// Currently only used in X12 834 insurance plan import.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanDropPat = "ins_plan_drop_pat"; // Insurance Plan Drop Patient

        /// <summary>
        /// Allows users to be assigned Insurance Verifications.
        /// </summary>
        public const string InsPlanVerifyList = "ins_plan_verify_list"; // Insurance Plan Verification Assign

        /// <summary>
        /// Allows users to bypass the global lock date to add paysplits.
        /// </summary>
        public const string SplitCreatePastLockDate = "split_create_past_lock_Date"; // Pay Split Create after Global Lock Date

        /// <summary>
        /// Uses date restrictions. Covers editing some fields of completed procs. Limited list 
        /// includes treatment area, diagnosis, add adjustment, Do Not Bill To Ins, Hide Graphics, 
        /// Misc tab, Medical tab, E-claim note, and the Prosthesis Replacement group box.
        /// </summary>
        public const string ProcComplEditLimited = "proc_compl_edit_limited"; // Edit Completed Procedure (limited)

        /// <summary>
        /// Uses date restrictions based on the SecDateEntry field as the claim date.
        /// Covers deleting a claim of any status (Sent, Waiting to Send, Received, etc).
        /// </summary>
        public const string ClaimDelete = "claim_delete"; // Claim Delete

        /// <summary>
        /// Covers editing the Write Off and Write Off Override fields for claimprocs as well as 
        /// deleting/creating claimprocs.
        /// <para>Uses date/days restriction based on the attached proc.DateEntryC; unless it's a 
        /// total payment, then uses claimproc.SecDateEntry.</para>
        /// <para>Applies to all plan types (i.e. PPO, Category%, Capitation, etc).</para>
        /// </summary>
        public const string InsWriteOffEdit = "ins_write_off_edit"; // Insurance Write Off Edit

        /// <summary>
        /// Allows users to change appointment confirmation status.
        /// </summary>
        public const string ApptConfirmStatusEdit = "appt_confirm_status_edit"; // Appointment Confirmation Status Edit

        /// <summary>
        /// Audit trail for when users change graphical settings for another workstation in FormGraphics.cs.
        /// </summary>
        public const string GraphicsRemoteEdit = "graphics_remote_edit";
        
        /// <summary>
        /// Indicates a user has the permission to view audit trails.
        /// </summary>
        public const string AuditTrail = "audit_trail"; // Audit Trail

        /// <summary>
        /// Allows the user to change the presenter on a treatment plan.
        /// </summary>
        public const string TreatPlanPresenterEdit = "treat_plan_presenter_edit"; // Edit Treatment Plan Presenter

        /// <summary>
        /// Allows users to use the Alphabetize Provider button from FormProviderSetup to permanently re-order providers.
        /// </summary>
        public const string ProviderAlphabetize = "provider_alphabetize"; // Providers - Alphabetize

        /// <summary>
        /// Allows editing of claimprocs that are marked as received status.
        /// </summary>
        public const string ClaimProcReceivedEdit = "claim_proc_received_edit"; // Claim Procedure Received Edit

        /// <summary>
        /// Used to diagnose an error in statement creation. Audit Trail Permission Only
        /// </summary>
        [Obsolete("Audit.")]
        public const string StatementPatNumMismatch = "statement_pat_num_mismatch";
        
        /// <summary>
        /// User has access to Mobile Web.
        /// </summary>
        [Obsolete]
        public const string MobileWeb = ""; // Mobile Web

        /// <summary>
        /// For logging purposes only.
        /// Used when PatPlans are created and not otherwise logged.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatPlanCreate = "pat_plan_create";
       
        /// <summary>
        /// Allows the user to change a patient's primary provider, with audit trail logging.
        /// </summary>
        public const string PatPriProvEdit = "pat_pri_prov_edit"; // Patient Primary Provider Edit

        public const string ReferralEdit = "referral_edit"; // Referral Edit

        /// <summary>
        /// Allows users to change a patient's billing type.
        /// </summary>
        public const string PatientBillingEdit = "patient_billing_edit"; // Patient Billing Type Edit

        /// <summary>
        /// Allows viewing annual prod inc of all providers instead of just a single provider.
        /// </summary>
        public const string ReportProdIncAllProviders = "report_prod_inc_all_providers"; // Production and Income - View All Providers

        /// <summary>
        /// Allows running daily reports. DEPRECATED.
        /// </summary>
        [Obsolete]
        public const string ReportDaily = "report_daily"; // Reports - Daily

        /// <summary>
        /// Allows viewing daily prod inc of all providers instead of just a single provider
        /// </summary>
        public const string ReportDailyAllProviders = "report_daily_all_providers"; // Daily payments - View All Providers

        /// <summary>
        /// Allows user to change the appointment schedule flag.
        /// </summary>
        public const string PatientApptRestrict = "patient_appt_restrict"; // Patient Restriction Edit

        /// <summary>
        /// Allows deleting sheets when they're associated to patients.
        /// </summary>
        public const string SheetDelete = "sheet_delete"; // Sheet Delete

        /// <summary>
        /// Allows updating custom tracking on claims.
        /// </summary>
        public const string UpdateCustomTracking = "update_custom_tracking"; // Update Custom Tracking

        /// <summary>
        /// Allows people to set graphics option for the workstation and other computers.
        /// </summary>
        public const string GraphicsEdit = "graphics_edit"; // Graphics Edit

        /// <summary>
        /// Allows user to change the fields within the Ortho tab of the Ins Plan Edit window.
        /// </summary>
        public const string InsPlanOrthoEdit = "ins_plan_ortho_edit"; // Insurance Plan Ortho Edit

        /// <summary>
        /// Allows user to change the provider on claimproc when claimproc is attached to a claim.
        /// </summary>
        public const string ClaimProcClaimAttachedProvEdit = "claim_proc_claim_attached_prov_edit"; // Claim Procedure Provider Edit When Attached to Claim

        /// <summary>
        /// Audit when insurance plans are merged.
        /// </summary>
        [Obsolete("Audit.")]
        public const string InsPlanMerge = "ins_plan_merge"; // Insurance Plan Combine
       
        /// <summary>
        /// Allows user to combine carriers.
        /// </summary>
        public const string InsCarrierCombine = "ins_carrier_combine"; // Insurance Carrier Combine
       
        /// <summary>
        /// Allows user to edit popups. A user without this permission will still be able to edit their own popups.
        /// </summary>
        public const string PopupEdit = "popup_edit"; // Popup Edit (other users)

        /// <summary>
        /// Allows user to select new insplan from list prior to dropping current insplan associated with a patplan.
        /// </summary>
        public const string InsPlanPickListExisting = "ins_plan_pick_list_existing"; // Change existing Ins Plan using Pick From List

        /// <summary>
        /// Allows user to edit their own signed ortho charts even if they don't have full permission.
        /// </summary>
        public const string OrthoChartEditUser = "ortho_chart_edit_user"; // Ortho Chart Edit (same user, signed)

        /// <summary>
        /// Allows user to edit procedure notes that they created themselves if they don't have full permission.
        /// </summary>
        public const string ProcedureNoteUser = "procedure_note_user"; // Procedure Note (same user)

        /// <summary>
        /// Allows user to edit group notes signed by other users. 
        /// If a user does not have this permission, they can still edit group notes that they themselves have signed.
        /// </summary>
        public const string GroupNoteEditSigned = "group_note_edit_signed"; // Group Note Edit (other users, signed)

        /// <summary>
        /// Allows user to lock and unlock wiki pages. Also allows the user to edit locked wiki pages.
        /// </summary>
        public const string WikiAdmin = "wiki_admin"; // Wiki Admin

        /// <summary>
        /// Allows user to create, edit, close, and delete payment plans.
        /// </summary>
        public const string PayPlanEdit = "pay_plan_edit"; // Pay Plan Edit

        /// <summary>
        /// Used for logging when a claim is created, cancelled, or saved.
        /// </summary>
        public const string ClaimEdit = "claim_edit";

        /// <summary>
        /// Allows user to run command queries. Command queries are any non-SELECT queries for any non-temporary table.
        /// </summary>
        public const string CommandQuery = "command_query"; // Command Query

        /// <summary>
        /// Gives user access to the replication setup window.
        /// </summary>
        [Obsolete("Replication is no longer supported")]
        public const string ReplicationSetup = "replication_setup";
        
        /// <summary>
        /// Allows user to edit and delete sent and received pre-auths. Uses date restriction.
        /// </summary>
        public const string PreAuthSentEdit = "pre_auth_sent_edit"; // PreAuth Sent Edit

        /// <summary>
        /// Edit fees (for logging only). Security log entry for this points to feeNum instead of CodeNum.
        /// </summary>
        public const string LogFeeEdit = "log_fee_edit";
       
        /// <summary>
        /// Log ClaimProcEdit
        /// </summary>
        public const string LogSubscriberEdit = "log_subscriber_edit";
       
        /// <summary>
        /// Logs changes to recalls, recalltypes, and recaltriggers.
        /// </summary>
        public const string RecallEdit = "recall_edit";

        /// <summary>
        /// Allows users with this permission the ability to add new users. Security admins have this by default.
        /// </summary>
        public const string AddNewUser = "add_new_user"; // Add New User

        /// <summary>
        /// Allows users with this permission the ability to view claims.
        /// </summary>
        public const string ClaimView = "claim_view"; // Claim View
        
        /// <summary>
        /// Allows users to run the Repeat Charge Tool.
        /// </summary>
        public const string RepeatChargeTool = "repeat_charge_tool"; // Repeating Charge Tool
        
        /// <summary>
        /// Logs when a discount plan is added or dropped from a patient.
        /// </summary>
        public const string DiscountPlanAddDrop = "discount_plan_add_drop";
        
        /// <summary>
        /// Allows users with this permission the ability to sign treatment plans.
        /// </summary>
        public const string TreatPlanSign = "treat_plan_sign"; // Sign Treatment Plan

        /// <summary>
        /// Allows users to search for patients in all clinics even when they are restricted to clinics.
        /// Also allows user to reassign patient clinic.
        /// </summary>
        public const string UnrestrictedSearch = "unrestricted_search"; // Unrestricted Patient Search

        /// <summary>
        /// Allows users to edit patient information for archived patients.
        /// </summary>
        public const string ArchivedPatientEdit = "archived_patient_edit"; // Archived Patient Edit

        /// <summary>
        /// Allows users to use the persistent option in the Commlog drop down.
        /// </summary>
        [Obsolete("HQ Only")]
        public const string CommlogPersistent = "commlog_persistent";

        /// <summary>
        /// Allows users to make changes to Sales Tax type adjustments.
        /// </summary>
        [Obsolete("HQ Only")]
        public const string SalesTaxAdjEdit = "sales_tax_adj_edit"; // Edit Sales Tax Adjustments

        /// <summary>
        /// Allows user to set last verified dates for insurance benefits. Also allows access to FormInsVerificationList.
        /// </summary>
        public const string InsuranceVerification = "insurance_verification"; // Insurance Verification

        /// <summary>
        /// Allows user to view a specific Dashboard Widget. Uses FKey for SheetDef.SheetDefNum.
        /// </summary>
        public const string DashboardWidget = "dashboard_widget"; // Dashboard Widget

        /// <summary>
        /// Prevent users from creating bulk claims from the Procs Not Billed Report if past the lock date.
        /// </summary>
        public const string NewClaimsProcNotBilled = "new_claims_proc_not_billed"; // Procedures Not Billed to Insurance, New Claims button

        /// <summary>
        /// Logs when a credit card is moved from one patient to another.  Makes a log for both patients.  Audit Trail Permission Only.
        /// </summary>
        [Obsolete("Audit.")]
        public const string CreditCardMove = "credit_card_move"; // Credit Card Moved

        /// <summary>
        /// Logs when aging is being ran and from where.
        /// </summary>
        [Obsolete("Audit.")]
        public const string AgingRan = "aging_ran"; // Aging Ran

        /// <summary>
        /// Logging into patient portal. Used for audit trail only.
        /// </summary>
        [Obsolete("Audit.")]
        public const string PatientPortalLogin = "patient_portal_login"; // Patient Portal Login
    }
}
