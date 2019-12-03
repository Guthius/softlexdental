namespace OpenDentBusiness
{
    /// <summary>
    /// Links one procedurecode to one recalltype.
    /// The presence of this trigger is used when determining DatePrevious in the recall table.
    /// </summary>
    public class RecallTrigger 
    {
        public long RecallTriggerNum;

        ///<summary>FK to recalltype.RecallTypeNum</summary>
        public long RecallTypeNum;

        ///<summary>FK to procedurecode.CodeNum</summary>
        public long CodeNum;

        public RecallTrigger Copy()
        {
            return (RecallTrigger)this.MemberwiseClone();
        }
    }
}