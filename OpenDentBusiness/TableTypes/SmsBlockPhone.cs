namespace OpenDentBusiness
{
    /// <summary>
    /// If a number is entered in this table, then any incoming text message will not be entered into the database.
    /// </summary>
    public class SmsBlockPhone
    {
        public long SmsBlockPhoneNum;

        ///<summary>The phone number to be blocked.</summary>
        public string BlockWirelessNumber;
    }
}
