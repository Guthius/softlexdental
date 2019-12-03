namespace OpenDentBusiness
{
    ///<summary>Links Procedures(groupnotes) to Procedures in a 1-n relationship.</summary>
    public class ProcGroupItem
    {
        public long ProcGroupItemNum;

        ///<summary>FK to procedurelog.ProcNum.</summary>
        public long ProcNum;

        ///<summary>FK to procedurelog.ProcNum.</summary>
        public long GroupNum;
    }
}
