using System;

namespace OpenDentBusiness
{
    public class ErxLog
    {
        public long ErxLogNum;

        ///<summary>FK to patient.PatNum.</summary>
        public long PatNum;

        ///<summary>Holds up to 16MB.</summary>
        public string MsgText;

        ///<summary>Automatically updated by MySQL every time a row is added or changed.</summary>
        public DateTime DateTStamp;

        ///<summary>FK to provider.ProvNum. The provider that the prescription request was sent by or on behalf of.</summary>
        public long ProvNum;

        ///<summary>FK to Userod.UserNum. The user that created the erx.</summary>
        public long UserNum;
    }
}
