using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrLabSpecimens
    {
        ///<summary></summary>
        public static List<EhrLabSpecimen> GetForLab(long ehrLabNum)
        {
            string command = "SELECT * FROM ehrlabspecimen WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            return Crud.EhrLabSpecimenCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static void DeleteForLab(long ehrLabNum)
        {
            EhrLabSpecimenConditions.DeleteForLab(ehrLabNum);
            EhrLabSpecimenRejectReasons.DeleteForLab(ehrLabNum);
            string command = "DELETE FROM ehrlabspecimen WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static EhrLabSpecimen InsertItem(EhrLabSpecimen ehrLabSpecimen)
        {
            ehrLabSpecimen.EhrLabNum = Crud.EhrLabSpecimenCrud.Insert(ehrLabSpecimen);
            for (int i = 0; i < ehrLabSpecimen.ListEhrLabSpecimenCondition.Count; i++)
            {
                ehrLabSpecimen.ListEhrLabSpecimenCondition[i].EhrLabSpecimenNum = ehrLabSpecimen.EhrLabSpecimenNum;
                EhrLabSpecimenConditions.Insert(ehrLabSpecimen.ListEhrLabSpecimenCondition[i]);
            }
            for (int i = 0; i < ehrLabSpecimen.ListEhrLabSpecimenRejectReason.Count; i++)
            {
                ehrLabSpecimen.ListEhrLabSpecimenRejectReason[i].EhrLabSpecimenNum = ehrLabSpecimen.EhrLabSpecimenNum;
                EhrLabSpecimenRejectReasons.Insert(ehrLabSpecimen.ListEhrLabSpecimenRejectReason[i]);
            }
            return ehrLabSpecimen;
        }
    }
}