using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores an ongoing record of database activity for security purposes.
    /// </summary>
    public class SecurityLog : ODTable
    {
        public long Id;
        public long UserId;
        public long? PatientId;
        public string EventName;
        public int EventId;

        /// <summary>
        /// The date and time of the entry. It's value is set when inserting and can never change.
        /// Even if a user changes the date on their ocmputer, this remains accurate because it uses server time.
        /// </summary>
        public DateTime LogDate;

        /// <summary>
        /// The description of exactly what was done. Varies by permission type.
        /// </summary>
        public string LogMessage;

        public string ComputerName;

        ///<summary>A foreign key to a table associated with the PermType.  0 indicates not in use.  
        ///This is typically used for objects that have specific audit trails so that users can see all audit entries related to a particular object.  
        ///Every permission using FKey should be included and implmented in the CrudAuditPerms enum so that securitylog FKeys are note orphaned.
        ///Additonaly, the tabletype will to have the [CrudTable(CrudAuditPerms=CrudAuditPerm._____] added with the new CrudAuditPerm you created.
        ///For the patient portal, it is used to indicate logs created on behalf of other patients.  
        ///It's uses include:  AptNum with PermType AppointmentCreate, AppointmentEdit, or AppointmentMove tracks all appointment logs for a particular 
        ///appointment.
        ///CodeNum with PermType ProcFeeEdit currently only tracks fee changes.  
        ///PatNum with PermType PatientPortal represents an entry that a patient made on behalf of another patient.
        ///	The PatNum column will represent the patient who is taking the action.  
        ///PlanNum with PermType InsPlanChangeCarrierName tracks carrier name changes.</summary>
        public long? ExternalId;

        ///<summary>Enum:LogSources None, WebSched, InsPlanImport834, FHIR, PatientPortal.</summary>
        ///

        /// <summary>
        /// The source of the log entry.
        /// </summary>
        public string Source;





        ///<summary>PatNum-NameLF</summary>
        [ODTableColumn(IsNotDbColumn = true)]
        public string PatientName;

        ///<summary>Existing LogHash from SecurityLogHash table</summary>
        [ODTableColumn(IsNotDbColumn = true)]
        public string LogHash;

        ///<summary>Not used.</summary>
        public long DefNum;

        ///<summary>Not used.</summary>
        public long DefNumError;

        ///<summary>Used to store the previous DateTStamp or SecDateTEdit of the object FKey refers to.</summary>
        [ODTableColumn(SpecialType = CrudSpecialColType.DateT)]
        public DateTime DateTPrevious;

    }

    /// <summary>
    /// Known entities that create security logs.
    /// </summary>
    public static class LogSources
    {
        /// <summary>
        /// Open Dental and unknown entities.
        /// </summary>
        public const string None = "System";

        /// <summary>
        /// OpenDentalService.
        /// </summary>
        public const string OpenDentalService = "Service";

        /// <summary>
        /// GWT Web Sched application Recall version.
        /// </summary>
        public const string WebSched = "Web Scheduler";

        /// <summary>
        /// X12 834 Insurance Plan Import from the Manage Module.
        /// </summary>
        public const string InsPlanImport834 = "X12 834 Insurance Plan";

        /// <summary>
        /// HL7 is an automated process which the user may not be aware of.
        /// </summary>
        public const string HL7 = "HL7";

        /// <summary>
        /// Database maintenance. This process creates patients which are known to be missing, but 
        /// the user may not be aware that the fix involves patient recreation.
        /// </summary>
        public const string DBM = "DBM";

        /// <summary>
        /// FHIR is an automated process which the user may not be aware of.
        /// </summary>
        public const string FHIR = "FHIR";

        /// <summary>
        /// Patient Portal application.
        /// </summary>
        public const string PatientPortal = "Patient Portal";

        /// <summary>
        /// GWT Web Sched application New Patient Appointment version
        /// </summary>
        [Obsolete]
        public const string WebSchedNewPatAppt = "Web Scheduler (New Patient)";

        /// <summary>
        /// Web Sched application for moving ASAP appointments.
        /// </summary>
        [Obsolete]
        public const string WebSchedASAP = "Web Scheduler (ASAP)";

        /// <summary>
        /// Automated eConfirmation and eReminders
        /// </summary>
        [Obsolete]
        public const string AutoConfirmations = "AutoConfirmations";
        
        /// <summary>
        /// Open Dental messages created for debugging and diagnostic purposes. For example, to 
        /// diagnose an unhandled exception or unexpected behavior that is otherwise too hard to 
        /// diagnose.
        /// </summary>
        public const string Diagnostic = "Diagnostics";

        /// <summary>
        /// Mobile Web application.
        /// </summary>
        [Obsolete]
        public const string MobileWeb = "Mobile Web";

        /// <summary>
        /// When retrieving reports in the background of FormOpenDental
        /// </summary>
        [Obsolete]
        public const string CanadaEobAutoImport = "Canada EOB Auto Import";

        /// <summary>
        /// Broadcast Monitor.
        /// </summary>
        public const string BroadcastMonitor = "Broadcast Monitor";

        /// <summary>
        /// Automatic log off from main form. Used to track when auto log off needs to kill the 
        /// program to force close open forms which are blocked or slow to respond.
        /// </summary>
        public const string AutoLogOff = "Log Off";
    }
}
