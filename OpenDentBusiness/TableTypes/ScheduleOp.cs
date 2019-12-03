namespace OpenDentBusiness
{
    ///<summary>Links one schedule block to one operatory.  So for a schedule block to show, it must be linked to one or more operatories.</summary>
    public class ScheduleOp
    {
        public long ScheduleOpNum;

        ///<summary>FK to schedule.ScheduleNum.</summary>
        public long ScheduleNum;

        ///<summary>FK to operatory.OperatoryNum.</summary>
        public long OperatoryNum;
    }
}
