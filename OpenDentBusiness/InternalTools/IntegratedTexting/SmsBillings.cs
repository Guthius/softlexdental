using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SmsBillings
    {
        /// <summary>dateFrom is inclusive. dateTo is exclusive. Used by OD Broadcaster. DO NOT REMOVE!!!</summary>
        public static List<SmsBilling> GetByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            string command = "SELECT * FROM smsbilling WHERE DateUsage >= " + POut.Date(dateFrom, true) + " AND DateUsage < " + POut.Date(dateTo, true);
            return Crud.SmsBillingCrud.SelectMany(command);
        }

        /// <summary>HQ only. Broadcast Monitor. DO NOT REMOVE!!!</summary>
        public static List<SmsBilling> GetAll()
        {
            string command = "SELECT * FROM smsbilling";
            return Crud.SmsBillingCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(SmsBilling smsBilling)
        {
            return Crud.SmsBillingCrud.Insert(smsBilling);
        }

        ///<summary>Can return null. Date usage should be the first of the month, if not will return the SmsBilling for the given patnum where month and year match dateUsage.</summary>
        public static SmsBilling getForPatNum(long patNum, DateTime dateUsage)
        {
            string command = "SELECT * FROM smsbilling WHERE PatNum = " + POut.Long(patNum);
            List<SmsBilling> listSmsBillingAll = Crud.SmsBillingCrud.SelectMany(command);
            return listSmsBillingAll.First(x => x.DateUsage.Year == dateUsage.Year && x.DateUsage.Month == dateUsage.Month);
        }
    }
}