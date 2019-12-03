namespace OpenDentBusiness
{
    /// <summary>
    /// Each row is big. 
    /// The entire X12 message text is stored here, since it can be the same for multiple etrans 
    /// objects, and since the messages can be so big.
    /// </summary>
    public class EtransMessageText
    {
        public long EtransMessageTextNum;

        /// <summary>The entire message text, including carriage returns.</summary>
        public string MessageText;
    }
}
