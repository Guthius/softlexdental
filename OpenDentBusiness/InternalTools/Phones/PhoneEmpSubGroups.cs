using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class PhoneEmpSubGroups
    {
        //If this table type will exist as cached data, uncomment the CachePattern region below and edit.

        ///<summary></summary>
        public static List<PhoneEmpSubGroup> Refresh(long patNum)
        {
            string command = "SELECT * FROM phoneempsubgroup WHERE PatNum = " + POut.Long(patNum);
            return Crud.PhoneEmpSubGroupCrud.SelectMany(command);
        }

        ///<summary>Gets one PhoneEmpSubGroup from the db.</summary>
        public static PhoneEmpSubGroup GetOne(long phoneEmpSubGroupNum)
        {
            return Crud.PhoneEmpSubGroupCrud.SelectOne(phoneEmpSubGroupNum);
        }

        ///<summary></summary>
        public static List<PhoneEmpSubGroup> GetAll()
        {
            string command = "SELECT * FROM phoneempsubgroup";
            return Crud.PhoneEmpSubGroupCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(PhoneEmpSubGroup phoneEmpSubGroup)
        {
            return Crud.PhoneEmpSubGroupCrud.Insert(phoneEmpSubGroup);
        }

        public static void Update(List<PhoneEmpSubGroup> list)
        {
            list.ForEach(Update);
        }

        ///<summary></summary>
        public static void Update(PhoneEmpSubGroup phoneEmpSubGroup)
        {
            Crud.PhoneEmpSubGroupCrud.Update(phoneEmpSubGroup);
        }

        ///<summary></summary>
        public static void Delete(long phoneEmpSubGroupNum)
        {
            Crud.PhoneEmpSubGroupCrud.Delete(phoneEmpSubGroupNum);
        }

        ///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
        ///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
        public static void Sync(List<PhoneEmpSubGroup> listNew, List<PhoneEmpSubGroup> listOld)
        {
            Crud.PhoneEmpSubGroupCrud.Sync(listNew, listOld);
        }
    }
}