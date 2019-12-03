namespace OpenDentBusiness
{
    ///<summary>For EHR module, May either be a note attached to an EhrLab or an EhrLabResult.  NTE.*</summary>
    public class EhrLabNote
    {
        public long EhrLabNoteNum;

        ///<summary>FK to ehrlab.EhrLabNum.  Should never be zero.</summary>
        public long EhrLabNum;

        ///<summary>FK to ehrlabresult.EhrLabResult.  May be 0 if this is a Lab Note, will be valued if this is an Ehr Lab Result Note.</summary>
        public long EhrLabResultNum;

        ///<summary>Carret delimited list of comments.  Comments must be formatted text and cannot contain the following 6 characters |^&amp;~\#  NTE.*.*</summary>
        public string Comments;

        public EhrLabNote()
        {
            Comments = "";
        }
    }
}
