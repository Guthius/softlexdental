namespace OpenDentBusiness
{
    /// <summary>
    /// The patient does not want to recieve messages for a particular type of communication.
    /// </summary>
    public class CommOptOut
    {
        public long CommOptOutNum;

        ///<summary>FK to patient.PatNum. The patient who is opting out of this form of communication.</summary>
        public long PatNum;

        ///<summary>Enum:CommOptOutType The type of communication for which this patient does not want to receive messages.</summary>
        public CommOptOutType CommType;

        ///<summary>Enum:CommOptOutMode The manner of message that the patient does not want to receive for this type of communication.</summary>
        public CommOptOutMode CommMode;
    }

    public enum CommOptOutType
    {
        None,
        eConfirm,
        eReminder,
    }

    public enum CommOptOutMode
    {
        Text,
        Email,
    }
}
