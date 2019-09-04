using CodeBase;
using System;
using System.ComponentModel;

namespace OpenDentBusiness
{
    public static class PrefNameExtensions
    {
        public static PrefValueType GetValueType(this PreferenceName prefName)
        {
            return prefName.GetAttributeOrDefault<PrefNameAttribute>().ValueType;
        }

        public static string GetValueAsText(this PreferenceName prefName)
        {
            switch (prefName.GetValueType())
            {
                case PrefValueType.NONE:
                    return ""; //nothing to get
                case PrefValueType.STRING:
                    return Preference.GetString(prefName);
                case PrefValueType.LONG:
                    return POut.Long(Preference.GetLong(prefName));
                case PrefValueType.LONG_NEG_ONE_AS_ZERO:
                    return Preference.GetLongHideNegOne(prefName, true);
                case PrefValueType.LONG_NEG_ONE_AS_BLANK:
                    return Preference.GetLongHideNegOne(prefName);
            }
            return Preference.GetString(prefName);
        }

        public static bool Update(this PreferenceName prefName, object value)
        {
            switch (prefName.GetValueType())
            {
                case PrefValueType.NONE:
                    return false; //nothing to save
                case PrefValueType.BOOL:
                    return Preference.Update(prefName, (bool)value);
                case PrefValueType.ENUM:
                case PrefValueType.INT:
                case PrefValueType.COLOR:
                    return Preference.Update(prefName, PIn.Int(value.ToString()));
                case PrefValueType.LONG:
                    return Preference.Update(prefName, PIn.Long(value.ToString()));
                case PrefValueType.LONG_NEG_ONE_AS_ZERO:
                case PrefValueType.LONG_NEG_ONE_AS_BLANK:
                    return Preference.UpdateLongAsNegOne(prefName, value.ToString());
                case PrefValueType.DOUBLE:
                    return Preference.Update(prefName, (double)value);
                case PrefValueType.DATE:
                case PrefValueType.DATETIME:
                    return Preference.Update(prefName, (DateTime)value);
                case PrefValueType.STRING:
                default:
                    return Preference.Update(prefName, value.ToString());
            }
        }
    }

    /// <summary>
    /// PrefName-related attributes.
    /// </summary>
    public class PrefNameAttribute : Attribute
    {
        public PrefValueType ValueType { get; set; } = PrefValueType.NONE;
    }

    public enum PrefValueType
    {
        NONE,
        BOOL,
        STRING,
        ENUM,
        INT,
        LONG,
        LONG_NEG_ONE_AS_ZERO,
        LONG_NEG_ONE_AS_BLANK,
        BYTE,
        DOUBLE,
        DATE,
        DATETIME,
        COLOR
    }

    ///<summary>Used by pref "AppointmentSearchBehavior". </summary>
    public enum SearchBehaviorCriteria
    {
        ///<summary>0 - Based only on provider availability from the schedule.</summary>
        ProviderTime,
        ///<summary>1 - Based on provider schedule availability as well as the availabilty of their operatory (dynamic or directly assigned).</summary>
        ProviderTimeOperatory,
    }

    ///<summary>Used by pref "AccountingSoftware".  0=OpenDental, 1=QuickBooks</summary>
    public enum AccountingSoftware
    {
        OpenDental,
        QuickBooks
    }

    public enum RigorousAccounting
    {
        ///<summary>0 - Auto-splitting payments and enforcing paysplit validity is enforced.</summary>
        [Description("Enforce Fully")]
        EnforceFully,
        ///<summary>1 - Auto-splitting payments is done, paysplit validity isn't enforced.</summary>
        [Description("Auto-Split Only")]
        AutoSplitOnly,
        ///<summary>2 - Neither auto-splitting nor paysplit validity is enforced.</summary>
        [Description("Don't Enforce")]
        DontEnforce
    }

    public enum RigorousAdjustments
    {
        ///<summary>0 - Automatically link adjustments and procedures, adjustment linking enforced.</summary>
        [Description("Enforce Fully")]
        EnforceFully,
        ///<summary>1 - Adjustment links are made automatically, but it can be edited.</summary>
        [Description("Link Only")]
        LinkOnly,
        ///<summary>2 - Adjustment links aren't made, nor are they enforced.</summary>
        [Description("Don't Enforce")]
        DontEnforce
    }

    ///<summary>Used by pref "WebSchedProviderRule". Determines how Web Sched will decide on what provider time slots to show patients.</summary>
    public enum WebSchedProviderRules
    {
        ///<summary>0 - Dynamically picks the first available provider based on the time slot picked by the patient.</summary>
        FirstAvailable,
        ///<summary>1 - Only shows time slots that are available via the patient's primary provider.</summary>
        PrimaryProvider,
        ///<summary>2 - Only shows time slots that are available via the patient's secondary provider.</summary>
        SecondaryProvider,
        ///<summary>3 - Only shows time slots that are available via the patient's last seen hygienist.</summary>
        LastSeenHygienist
    }

    public enum PPOWriteoffDateCalc
    {
        /// <summary>0 - Use the insurance payment date when dating writeoff estimates and adjustments in reports. </summary>
        [Description("Insurance Pay Date")]
        InsPayDate,
        /// <summary>1 - Use the date of the procedure when dating writeoff estimates and adjustments in reports.</summary>
        [Description("Procedure Date")]
        ProcDate,
        /// <summary>2 - Uses initial claim date for writeoff estimates and insurance payment date for writeoff adjustments in reports.</summary>
        [Description("Initial Claim Date/Ins Pay Date")]
        ClaimPayDate
    }
}
