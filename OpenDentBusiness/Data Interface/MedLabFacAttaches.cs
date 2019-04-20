using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class MedLabFacAttaches
    {
        ///<summary></summary>
        public static long Insert(MedLabFacAttach medLabFacAttach)
        {
            return Crud.MedLabFacAttachCrud.Insert(medLabFacAttach);
        }

        ///<summary>Gets all MedLabFacAttach objects from the db for a MedLab or a MedLabResult.  Only one parameter is required,
        ///EITHER a MedLabNum OR a MedLabResultNum.  The other parameter should be 0.  If both parameters are >0, then list
        ///returned will be all MedLabFacAttaches with EITHER the MedLabNum OR the MedLabResultNum provided.</summary>
        public static List<MedLabFacAttach> GetAllForLabOrResult(long medLabNum, long medLabResultNum)
        {
            string command = "SELECT * FROM medlabfacattach WHERE ";
            if (medLabNum != 0)
            {
                command += "MedLabNum=" + POut.Long(medLabNum);
            }
            if (medLabResultNum != 0)
            {
                if (medLabNum != 0)
                {
                    command += " OR ";
                }
                command += "MedLabResultNum=" + POut.Long(medLabResultNum) + " ";
            }
            command += "ORDER BY MedLabFacAttachNum DESC";
            return Crud.MedLabFacAttachCrud.SelectMany(command);
        }

        public static List<MedLabFacAttach> GetAllForResults(List<long> listResultNums)
        {
            string command = "SELECT * FROM medlabfacattach WHERE MedLabResultNum IN(" + String.Join(",", listResultNums) + ")";
            return Crud.MedLabFacAttachCrud.SelectMany(command);
        }

        ///<summary>Delete all MedLabFacAttach objects for the list of MedLabNums and/or list of MedLabResultNums.  Supply either list or both lists and
        ///the MedLabFacAttach entries for either list will be deleted.  This could leave MedLabFacility entries not attached
        ///to any lab or result, but we won't worry about cleaning those up since the MedLabFacility table will likely always remain very small.</summary>
        public static void DeleteAllForLabsOrResults(List<long> listLabNums, List<long> listResultNums)
        {
            string command = "DELETE FROM medlabfacattach "
                + "WHERE MedLabNum IN(" + String.Join(",", listLabNums) + ") "
                + "OR MedLabResultNum IN(" + String.Join(",", listResultNums) + ")";
            Db.NonQ(command);
        }
    }
}