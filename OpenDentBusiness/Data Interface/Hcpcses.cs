using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Hcpcses
    {
        ///<summary></summary>
        public static long Insert(Hcpcs hcpcs)
        {
            return Crud.HcpcsCrud.Insert(hcpcs);
        }

        ///<summary></summary>
        public static void Update(Hcpcs hcpcs)
        {
            Crud.HcpcsCrud.Update(hcpcs);
        }

        public static List<Hcpcs> GetAll()
        {
            string command = "SELECT * FROM hcpcs";
            return Crud.HcpcsCrud.SelectMany(command);
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT HcpcsCode FROM hcpcs";
            DataTable table = DataCore.GetTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i][0].ToString());
            }
            return retVal;
        }

        ///<summary>Returns the total count of HCPCS codes.  HCPCS codes cannot be hidden.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM hcpcs";
            return PIn.Long(Db.GetCount(command));
        }

        ///<summary>Returns the Hcpcs of the code passed in by looking in cache.  If code does not exist, returns null.</summary>
        public static Hcpcs GetByCode(string hcpcsCode)
        {
            string command = "SELECT * FROM hcpcs WHERE HcpcsCode='" + POut.String(hcpcsCode) + "'";
            return Crud.HcpcsCrud.SelectOne(command);
        }

        ///<summary>Directly from db.</summary>
        public static bool CodeExists(string hcpcsCode)
        {
            string command = "SELECT COUNT(*) FROM hcpcs WHERE HcpcsCode='" + POut.String(hcpcsCode) + "'";
            string count = Db.GetCount(command);
            if (count == "0")
            {
                return false;
            }
            return true;
        }

        public static List<Hcpcs> GetBySearchText(string searchText)
        {
            string[] searchTokens = searchText.Split(' ');
            string command = @"SELECT * FROM hcpcs ";
            for (int i = 0; i < searchTokens.Length; i++)
            {
                command += (i == 0 ? "WHERE " : "AND ") + "(HcpcsCode LIKE '%" + POut.String(searchTokens[i]) + "%' OR DescriptionShort LIKE '%" + POut.String(searchTokens[i]) + "%') ";
            }
            return Crud.HcpcsCrud.SelectMany(command);
        }
    }
}