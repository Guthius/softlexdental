using System;

namespace OpenDentBusiness
{
    ///<summary>HL7 messages sent and received.</summary>
    public class HL7Msg
    {
        public long HL7MsgNum;

        ///<summary>Enum:HL7MessageStatus Out/In are relative to Open Dental.  This is in contrast to the names of the old ecw folders, which were relative to the other program.  OutPending, OutSent, InReceived, InProcessed.</summary>
        public HL7MessageStatus HL7Status;

        ///<summary>The actual HL7 message in its entirity.</summary>
        public string MsgText;

        ///<summary>FK to appointment.AptNum.  Many of the messages contain "Visit ID" which is equivalent to our AptNum.</summary>
        public long AptNum;

        ///<summary>Used to determine which messages are old so that they can be cleaned up.</summary>
        public DateTime DateTStamp;

        /// <summary>FK to patient.PatNum.</summary>
        public long PatNum;

        /// <summary></summary>
        public string Note;
    }

    ///<summary></summary>
    public enum HL7MessageStatus
    {
        ///<summary>0</summary>
        OutPending,
        ///<summary>1</summary>
        OutSent,
        ///<summary>2-Tried to send, but there was a problem.  Will keep trying.</summary>
        OutFailed,
        ///<summary>3</summary>
        InProcessed,
        ///<summary>4</summary>
        InFailed
    }
}
