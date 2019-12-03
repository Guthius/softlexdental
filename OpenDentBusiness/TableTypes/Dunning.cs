namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Represents a dunning message that will show on statements when printing bills when
    ///         the configured criteria is met.
    ///     </para>
    ///     <para>
    ///         <b>Dunning</b> is the process of methodically communicating with customers to 
    ///         ensure the collection of accounts receivable. Communications progress from gentle 
    ///         reminders to threatening letters and phone calls and more or less intimidating 
    ///         location visits as accounts become more overdue. Laws in each country regulate the 
    ///         form that dunning can take. It is generally unlawful to harass or threaten 
    ///         consumers. It is acceptable to issue firm reminders and to take all allowable 
    ///         collection options.
    ///     </para>
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Dunning_(process)"/>
    public class Dunning
    {
        public long DunningNum;

        /// <summary>
        /// The actual dunning message that will go on the patient bill.
        /// </summary>
        public string DunMessage;

        /// <summary>
        /// FK to definition.DefNum.
        /// </summary>
        public long BillingType;

        /// <summary>
        /// Program forces only 0,30,60,or 90.
        /// </summary>
        public byte AgeAccount;

        /// <summary>
        /// Enum:YN Set Y to only show if insurance is pending.
        /// </summary>
        public YN InsIsPending;

        /// <summary>
        /// A message that will be copied to the NoteBold field of the Statement.
        /// </summary>
        public string MessageBold;

        /// <summary>
        /// An override for the default email subject.
        /// </summary>
        public string EmailSubject;

        /// <summary>
        /// An override for the default email body.
        /// </summary>
        public string EmailBody;

        /// <summary>
        ///     <para>
        ///         The number of days before an account reaches AgeAccount to include this dunning message on statements.
        ///     </para>
        /// Example: If DaysInAdvance=3 and AgeAccount=90, an account that is 87 days old when bills are generated will include this message.
        /// </summary>
        public int DaysInAdvance;

        /// <summary>
        /// The ID of the clinic.
        /// </summary>
        public long ClinicNum;

        /// <summary>
        /// A value indicating whether the message applies specifically to super families.
        /// </summary>
        public bool IsSuperFamily;

        public Dunning Copy()
        {
            return (Dunning)MemberwiseClone();
        }
    }
}
