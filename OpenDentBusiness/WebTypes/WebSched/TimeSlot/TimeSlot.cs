using System;

namespace OpenDentBusiness.WebTypes.WebSched.TimeSlot
{
    [Serializable]
    public class TimeSlot
    {
        public DateTime DateTimeStart;
        public DateTime DateTimeStop;
        public long OperatoryNum;
        public long ProvNum;
        
        ///<summary>FK to definition.DefNum.  This will be a definition of type WebSchedNewPatApptTypes.</summary>
        public long DefNumApptType;

        public TimeSlot()
        {
        }

        public TimeSlot(DateTime dateTimeStart, DateTime dateTimeStop, long operatoryNum = 0, long provNum = 0, long defNumApptType = 0)
        {
            DateTimeStart = dateTimeStart;
            DateTimeStop = dateTimeStop;
            OperatoryNum = operatoryNum;
            ProvNum = provNum;
            DefNumApptType = defNumApptType;
        }
    }
}