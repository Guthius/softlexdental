using System;
using System.Reflection;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class MedLabSpecimens
    {
        ///<summary></summary>
        public static long Insert(MedLabSpecimen medLabSpecimen)
        {
            return Crud.MedLabSpecimenCrud.Insert(medLabSpecimen);
        }

        ///<summary>Deletes all MedLabSpecimen objects from the db for a list of MedLabNums.</summary>
        public static void DeleteAllForLabs(List<long> listLabNums)
        {
            string command = "DELETE FROM medlabspecimen WHERE MedLabNum IN(" + String.Join(",", listLabNums) + ")";
            Db.NonQ(command);
        }
    }
}