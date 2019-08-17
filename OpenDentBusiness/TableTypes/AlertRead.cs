using System;

namespace OpenDentBusiness
{
    public class AlertRead
    {
        ///<summary>Primary key.</summary>
        public long AlertReadNum;
        
        ///<summary>FK to alertitem.AlertItemNum.</summary>
        public long AlertItemNum;
        
        ///<summary>FK to userod.UserNum.</summary>
        public long UserNum;

        public AlertRead()
        {
        }

        public AlertRead(long alertItemNum, long userNum)
        {
            AlertItemNum = alertItemNum;
            UserNum = userNum;
        }
    }
}