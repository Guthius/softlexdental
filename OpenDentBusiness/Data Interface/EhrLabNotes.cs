using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class EhrLabNotes
    {
        ///<summary></summary>
        public static List<EhrLabNote> GetForLab(long ehrLabNum)
        {
            string command = "SELECT * FROM ehrlabnote WHERE EhrLabNum = " + POut.Long(ehrLabNum) + " AND EhrLabResultNum=0";
            return Crud.EhrLabNoteCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static List<EhrLabNote> GetForLabResult(long ehrLabResultNum)
        {
            string command = "SELECT * FROM ehrlabnote WHERE EhrLabResultNum=" + POut.Long(ehrLabResultNum);
            return Crud.EhrLabNoteCrud.SelectMany(command);
        }

        ///<summary>Deletes notes for lab results too.</summary>
        public static void DeleteForLab(long ehrLabNum)
        {
            string command = "DELETE FROM ehrlabnote WHERE EhrLabNum = " + POut.Long(ehrLabNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static long Insert(EhrLabNote ehrLabNote)
        {
            return Crud.EhrLabNoteCrud.Insert(ehrLabNote);
        }
    }
}