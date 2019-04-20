using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrLabResultsCopyTos
    {
        ///<summary></summary>
        public static List<EhrLabResultsCopyTo> GetForLab(long ehrLabNum)
        {
            string command = "SELECT * FROM ehrlabresultscopyto WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            return Crud.EhrLabResultsCopyToCrud.SelectMany(command);
        }

        ///<summary>Deletes notes for lab results too.</summary>
        public static void DeleteForLab(long ehrLabNum)
        {
            string command = "DELETE FROM ehrlabresultscopyto WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static long Insert(EhrLabResultsCopyTo ehrLabResultsCopyTo)
        {
            return Crud.EhrLabResultsCopyToCrud.Insert(ehrLabResultsCopyTo);
        }
    }
}