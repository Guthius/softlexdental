using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrCarePlans
    {
        ///<summary></summary>
        public static List<EhrCarePlan> Refresh(long patNum)
        {
            string command = "SELECT * FROM ehrcareplan WHERE PatNum = " + POut.Long(patNum) + " ORDER BY DatePlanned";
            return Crud.EhrCarePlanCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(EhrCarePlan ehrCarePlan)
        {
            return Crud.EhrCarePlanCrud.Insert(ehrCarePlan);
        }

        ///<summary></summary>
        public static void Update(EhrCarePlan ehrCarePlan)
        {
            Crud.EhrCarePlanCrud.Update(ehrCarePlan);
        }

        ///<summary></summary>
        public static void Delete(long ehrCarePlanNum)
        {
            string command = "DELETE FROM ehrcareplan WHERE EhrCarePlanNum = " + POut.Long(ehrCarePlanNum);
            Db.NonQ(command);
        }
    }
}