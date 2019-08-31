/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using System;
using System.ComponentModel;

namespace OpenDentBusiness
{
    /// <summary>
    /// Definition Category. Go to the definition setup window in the program to see how each of these categories is used.
    /// If you add a category, make sure to add it to the switch statement of FormDefinitions so the user can edit it.
    /// Add a "NotUsed" description attribute to defs that shouldn't show up in FormDefinitions.
    /// </summary>
    public enum DefinitionCategory
    {
        // TODO: Fix me

        /// <summary>
        /// 0 - Colors to display in Account module.
        /// </summary>
        [Description("Account Colors")]
        AccountColors,
        
        /// <summary>
        /// 1 - Adjustment types.
        /// </summary>
        [Description("Adj Types")]
        AdjTypes,
        
        /// <summary>
        /// 2 - Appointment confirmed types.
        /// </summary>
        [Description("Appt Confirmed")]
        ApptConfirmed,
        
        /// <summary>
        /// 3 - Procedure quick add list for appointments.
        /// </summary>
        [Description("Appt Procs Quick Add")]
        ApptProcsQuickAdd,
        
        /// <summary>
        /// 4 - Billing types.
        /// </summary>
        [Description("Billing Types")]
        BillingTypes,

        /// <summary>
        /// 5 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        ClaimFormats,

        /// <summary>
        /// 6 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        DunningMessages,

        /// <summary>
        /// 7 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        FeeSchedNamesOld,

        /// <summary>
        /// 8 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        MedicalNotes,
        
        /// <summary>
        /// 9 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        OperatoriesOld,
        
        /// <summary>
        /// 10 - Payment types.
        /// </summary>
        [Description("Payment Types")]
        PaymentTypes,
        
        /// <summary>
        /// 11 - Procedure code categories.
        /// </summary>
        [Description("Proc Code Categories")]
        ProcCodeCats,
        
        /// <summary>
        /// 12 - Progress note colors.
        /// </summary>
        [Description("Prog Notes Colors")]
        ProgNoteColors,
        
        /// <summary>
        /// 13 - Statuses for recall, reactivation, unscheduled, and next appointments.
        /// </summary>
        [Description("Recall/Unsched Status")]
        RecallUnschedStatus,

        /// <summary>
        /// 14 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        ServiceNotes,

        /// <summary>
        /// 15 - Discount types.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        DiscountTypes,

        /// <summary>
        /// 16 - Diagnosis types.
        /// </summary>
        [Description("Diagnosis Types")]
        Diagnosis,
        
        /// <summary>
        /// 17 - Colors to display in the Appointments module.
        /// </summary>
        [Description("Appointment Colors")]
        AppointmentColors,
        
        /// <summary>
        /// 18 - Image categories.
        /// </summary>
        [Description("Image Categories")]
        ImageCats,

        /// <summary>
        /// 19 - Not used.
        /// </summary>
        [Obsolete]
        [Description("NotUsed")]
        ApptPhoneNotes,
        
        /// <summary>
        /// 20 - Treatment plan priority names.
        /// </summary>
        [Description("Treat' Plan Priorities")]
        TxPriorities,
        
        /// <summary>
        /// 21 - Miscellaneous color options.
        /// </summary>
        [Description("Misc Colors")]
        MiscColors,
        
        /// <summary>
        /// 22 - Colors for the graphical tooth chart.
        /// </summary>
        [Description("Chart Graphic Colors")]
        ChartGraphicColors,
        
        /// <summary>
        /// 23 - Categories for the Contact list.
        /// </summary>
        [Description("Contact Categories")]
        ContactCategories,
        
        /// <summary>
        /// 24 - Categories for Letter Merge.
        /// </summary>
        [Description("Letter Merge Cats")]
        LetterMergeCats,
        
        /// <summary>
        /// 25 - Types of Schedule Blockouts.
        /// </summary>
        [Description("Blockout Types")]
        BlockoutTypes,
        
        /// <summary>
        /// 26 - Categories of procedure buttons in Chart module
        /// </summary>
        [Description("Proc Button Categories")]
        ProcButtonCats,
        
        /// <Summary>
        /// 27 - Types of commlog entries.
        /// </Summary>
        [Description("Commlog Types")]
        CommLogTypes,
        
        /// <summary>
        /// 28 - Categories of Supplies
        /// </summary>
        [Description("Supply Categories")]
        SupplyCats,
        
        /// <summary>
        /// 29 - Types of unearned income used in accrual accounting.
        /// </summary>
        [Description("PaySplit Unearned Types")]
        PaySplitUnearnedType,
        
        /// <summary>
        /// 30 - Prognosis types.
        /// </summary>
        [Description("Prognosis")]
        Prognosis,
        
        /// <summary>
        /// 31 - Custom Tracking, statuses such as 'review', 'hold', 'riskmanage', etc.
        /// </summary>
        [Description("Claim Custom Tracking")]
        ClaimCustomTracking,
        
        /// <summary>
        /// 32 - PayType for claims such as 'Check', 'EFT', etc.
        /// </summary>
        [Description("Insurance Payment Types")]
        InsurancePaymentType,
        
        /// <summary>
        /// 33 - Categories of priorities for tasks.
        /// </summary>
        [Description("Task Priorities")]
        TaskPriorities,
        
        /// <summary>
        /// 34 - Categories for fee override colors.
        /// </summary>
        [Description("Fee Colors")]
        FeeColors,
        
        /// <summary>
        /// 35 - Provider specialties.  General, Hygienist, Pediatric, Primary Care Physician, etc.
        /// </summary>
        [Description("Provider Specialties")]
        ProviderSpecialties,
       
        /// <summary>
        /// 36 - Reason why a claim proc was rejected. This must be set on each individual claim proc.
        /// </summary>
        [Description("Claim Payment Tracking")]
        ClaimPaymentTracking,
        
        /// <summary>
        /// 37 - Procedure quick charge list for patient accounts.
        /// </summary>
        [Description("Account Procs Quick Add")]
        AccountQuickCharge,
        
        /// <summary>
        /// 38 - Insurance verification status such as 'Verified', 'Unverified', 'Pending Verification'.
        /// </summary>
        [Description("Insurance Verification Status")]
        InsuranceVerificationStatus,
        
        /// <summary>
        /// 39 - Regions that clinics can be assigned to.
        /// </summary>
        [Description("Regions")]
        Regions,
        
        /// <summary>
        /// 40 - ClaimPayment Payment Groups.
        /// </summary>
        [Description("Claim Payment Groups")]
        ClaimPaymentGroups,
        
        /// <summary>
        /// 41 - Auto Note Categories.  Used to categorize autonotes into custom categories.
        /// </summary>
        [Description("Auto Note Categories")]
        AutoNoteCats,

        /// <summary>
        /// 42 - Web Sched New Patient Appointment Types.  Displays in Web Sched.  Selected type shows in appointment note.
        /// </summary>
        [Description("Web Sched New Pat Appt Types")]
        WebSchedNewPatApptTypes,
        
        /// <summary>
        /// 43 - Custom Claim Status Error Code.
        /// </summary>
        [Description("Claim Error Code")]
        ClaimErrorCode,

        /// <summary>
        /// 44 - Specialties that clinics perform.  Useful for separating patient clones across clinics.
        /// </summary>
        [Description("Clinic Specialties")]
        ClinicSpecialty,

        /// <summary>
        /// 45 - HQ Only job priorities.
        /// </summary>
        [Obsolete]
        [Description("Job Priorities HqOnly")]
        JobPriorities,

        /// <summary>
        /// 46 - Carrier Group Name.
        /// </summary>
        [Description("Carrier Group Names")]
        CarrierGroupNames,
        
        /// <summary>
        /// 47 - PayPlanCategory
        /// </summary>
        [Description("Payment Plan Categories")]
        PayPlanCategories,
        
        /// <summary>
        /// 48 - Associates an insurance payment to an account number.  Currently only used with "Auto Deposits".
        /// </summary>
        [Description("Auto Deposit Account")]
        AutoDeposit,
        
        /// <summary>
        /// 49 - Code Group used for insurance filing.
        /// </summary>
        [Description("Insurance Filing Code Group")]
        InsuranceFilingCodeGroup,
    }
}