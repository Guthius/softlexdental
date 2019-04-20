using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrAptObses
    {
        ///<summary></summary>
        public static List<EhrAptObs> Refresh(long aptNum)
        {
            string command = "SELECT * FROM ehraptobs WHERE AptNum = " + POut.Long(aptNum);
            return Crud.EhrAptObsCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(EhrAptObs ehrAptObs)
        {
            return Crud.EhrAptObsCrud.Insert(ehrAptObs);
        }

        ///<summary></summary>
        public static void Update(EhrAptObs ehrAptObs)
        {
            Crud.EhrAptObsCrud.Update(ehrAptObs);
        }

        ///<summary></summary>
        public static void Delete(long ehrAptObsNum)
        {
            string command = "DELETE FROM ehraptobs WHERE EhrAptObsNum = " + POut.Long(ehrAptObsNum);
            Db.NonQ(command);
        }
    }
}