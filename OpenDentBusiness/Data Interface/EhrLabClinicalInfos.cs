using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrLabClinicalInfos
    {
        ///<summary></summary>
        public static List<EhrLabClinicalInfo> GetForLab(long ehrLabNum)
        {
            string command = "SELECT * FROM ehrlabclinicalinfo WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            return Crud.EhrLabClinicalInfoCrud.SelectMany(command);
        }

        ///<summary>Deletes notes for lab results too.</summary>
        public static void DeleteForLab(long ehrLabNum)
        {
            string command = "DELETE FROM ehrlabclinicalinfo WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static long Insert(EhrLabClinicalInfo ehrLabClinicalInfo)
        {
            return Crud.EhrLabClinicalInfoCrud.Insert(ehrLabClinicalInfo);
        }
    }
}