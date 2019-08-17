using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary>Cache pattern only used for updates.</summary>
    public class Cdcrecs
    {
        ///<summary></summary>
        public static long Insert(Cdcrec cdcrec)
        {
            return Crud.CdcrecCrud.Insert(cdcrec);
        }

        ///<summary></summary>
        public static void Update(Cdcrec cdcrec)
        {
            Crud.CdcrecCrud.Update(cdcrec);
        }

        public static List<Cdcrec> GetAll()
        {
            string command = "SELECT * FROM cdcrec";
            return Crud.CdcrecCrud.SelectMany(command);
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            DataTable table = DataConnection.GetTable("SELECT CdcRecCode FROM cdcrec");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i].ItemArray[0].ToString());
            }
            return retVal;
        }

        ///<summary>Returns the total count of CDCREC codes.  CDCREC codes cannot be hidden.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM cdcrec";
            return PIn.Long(Db.GetCount(command));
        }
    }
}