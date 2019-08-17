using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Loincs
    {
        ///<summary></summary>
        public static long Insert(Loinc lOINC)
        {
            return Crud.LoincCrud.Insert(lOINC);
        }

        ///<summary></summary>
        public static void Update(Loinc loinc)
        {
            Crud.LoincCrud.Update(loinc);
        }

        ///<summary></summary>
        public static List<Loinc> GetAll()
        {
            string command = "SELECT * FROM loinc";
            return Crud.LoincCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static List<Loinc> GetBySearchString(string searchText)
        {

            string command = "SELECT * FROM loinc WHERE LoincCode LIKE '%" + POut.String(searchText) + "%' OR NameLongCommon LIKE '%" + POut.String(searchText)
                    + "%' ORDER BY RankCommonTests=0, RankCommonTests";//common tests are at top of list.

            return Crud.LoincCrud.SelectMany(command);
        }

        ///<summary>Returns the count of all LOINC codes.  LOINC codes cannot be hidden.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM loinc";
            return PIn.Long(Db.GetCount(command));
        }

        ///<summary>Gets one Loinc from the db based on LoincCode, returns null if not found.</summary>
        public static Loinc GetByCode(string loincCode)
        {
            string command = "SELECT * FROM loinc WHERE LoincCode='" + POut.String(loincCode) + "'";
            List<Loinc> retVal = Crud.LoincCrud.SelectMany(command);
            if (retVal.Count > 0)
            {
                return retVal[0];
            }
            return null;
        }

        ///<summary>Gets a list of Loinc objects from the db based on codeList.  codeList is a comma-delimited list of LoincCodes in the format "code,code,code,code".  Returns an empty list if none in the loinc table.</summary>
        public static List<Loinc> GetForCodeList(string codeList)
        {
            string[] codes = codeList.Split(',');
            string command = "SELECT * FROM loinc WHERE LoincCode IN(";
            for (int i = 0; i < codes.Length; i++)
            {
                if (i > 0)
                {
                    command += ",";
                }
                command += "'" + POut.String(codes[i]) + "'";
            }
            command += ") ";
            return Crud.LoincCrud.SelectMany(command);
        }

        ///<summary>CAUTION, this empties the entire loinc table. "DELETE FROM loinc"</summary>
        public static void DeleteAll()
        {
            string command = "DELETE FROM loinc";
            Db.NonQ(command);

            command = "ALTER TABLE loinc AUTO_INCREMENT = 1";//resets the primary key to start counting from 1 again.
            Db.NonQ(command);

            return;
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT LoincCode FROM loinc";
            DataTable table = DataConnection.GetTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i].ItemArray[0].ToString());
            }
            return retVal;
        }

        ///<summary>Directly from db.</summary>
        public static bool CodeExists(string LoincCode)
        {
            string command = "SELECT COUNT(*) FROM loinc WHERE LoincCode='" + POut.String(LoincCode) + "'";
            string count = Db.GetCount(command);
            if (count == "0")
            {
                return false;
            }
            return true;
        }
    }
}