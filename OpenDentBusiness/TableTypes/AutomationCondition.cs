namespace OpenDentBusiness
{
    /// <summary>
    /// Each condition evaluates to true or false. 
    /// A series of conditions for a single automation is ANDed together.
    /// </summary>
    public class AutomationCondition
    {
        public long AutomationConditionNum;

        ///<summary>FK to automation.AutomationNum. </summary>
        public long AutomationNum;

        ///<summary>Enum:AutoCondField </summary>
        public AutoCondField CompareField;

        ///<summary>Enum:AutoCondComparison Not all comparisons are allowed with all data types.</summary>
        public AutoCondComparison Comparison;

        ///<summary>.</summary>
        public string CompareString;

        public AutomationCondition Clone()
        {
            return (AutomationCondition)this.MemberwiseClone();
        }
    }

    public enum AutoCondField
    {
        /// <summary>
        /// Typically specify Equals the exact name/description of the sheet.
        /// </summary>
        NeedsSheet,

        /// <summary>
        /// disease
        /// </summary>
        Problem,

        Medication,
        Allergy,

        /// <summary>
        /// Example, 23
        /// </summary>
        Age,

        /// <summary>
        /// Allowed values are M or F, not case sensitive. Enforce at entry time.
        /// </summary>
        Gender,

        Labresult,
        InsuranceNotEffective,
        BillingType,
        IsProcRequired,
        IsControlled,
        IsPatientInstructionPresent,
    }

    public enum AutoCondComparison
    {
        Equals,
        GreaterThan,
        LessThan,
        Contains,

        /// <summary>
        /// Should not be displayed to users to choose from. 
        /// Used when the condition has one and only one 'comparison' to trigger it.  E.g. ins not effective.
        /// </summary>
        None

        //Exists,
        //NotEquals,
    }
}
