using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Icd10s
    {
        ///<summary></summary>
        public static long Insert(Icd10 icd10)
        {
            return Crud.Icd10Crud.Insert(icd10);
        }

        ///<summary></summary>
        public static void Update(Icd10 icd10)
        {
            Crud.Icd10Crud.Update(icd10);
        }

        public static List<Icd10> GetAll()
        {
            string command = "SELECT * FROM icd10";
            return Crud.Icd10Crud.SelectMany(command);
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT Icd10Code FROM icd10";
            DataTable table = DataCore.GetTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i][0].ToString());
            }
            return retVal;
        }

        ///<summary>Returns the total number of ICD10 codes.  Some rows in the ICD10 table based on the IsCode column.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM icd10 WHERE IsCode!=0";
            return PIn.Long(Db.GetCount(command));
        }

        ///<summary>Gets one ICD10 object directly from the database by CodeValue.  If code does not exist, returns null.</summary>
        public static Icd10 GetByCode(string Icd10Code)
        {
            string command = "SELECT * FROM icd10 WHERE Icd10Code='" + POut.String(Icd10Code) + "'";
            return Crud.Icd10Crud.SelectOne(command);
        }

        ///<summary>Gets all ICD10 objects directly from the database by CodeValues.  If codes don't exist, it will return an empty list.</summary>
        public static List<Icd10> GetByCodes(List<string> listIcd10Codes)
        {
            if (listIcd10Codes == null || listIcd10Codes.Count == 0)
            {
                return new List<Icd10>();
            }
            string command = "SELECT * FROM icd10 WHERE Icd10Code IN('" + string.Join("','", listIcd10Codes) + "')";
            return Crud.Icd10Crud.SelectMany(command);
        }

        ///<summary>Directly from db.</summary>
        public static bool CodeExists(string Icd10Code)
        {
            string command = "SELECT COUNT(*) FROM icd10 WHERE Icd10Code='" + POut.String(Icd10Code) + "'";
            string count = Db.GetCount(command);
            if (count == "0")
            {
                return false;
            }
            return true;
        }

        public static List<Icd10> GetBySearchText(string searchText)
        {
            string[] searchTokens = searchText.Split(' ');
            string command = @"SELECT * FROM icd10 ";
            for (int i = 0; i < searchTokens.Length; i++)
            {
                command += (i == 0 ? "WHERE " : "AND ") + "(Icd10Code LIKE '%" + POut.String(searchTokens[i]) + "%' OR Description LIKE '%" + POut.String(searchTokens[i]) + "%') ";
            }
            return Crud.Icd10Crud.SelectMany(command);
        }

        ///<summary>Gets one Icd10 from the db.</summary>
        public static Icd10 GetOne(long icd10Num)
        {
            return Crud.Icd10Crud.SelectOne(icd10Num);
        }

        ///<summary>Returns the code and description of the icd10.</summary>
        public static string GetCodeAndDescription(string icd10Code)
        {
            if (string.IsNullOrEmpty(icd10Code))
            {
                return "";
            }
            //No need to check RemotingRole; no call to db.
            Icd10 icd10 = GetByCode(icd10Code);
            return (icd10 == null ? "" : (icd10.Icd10Code + "-" + icd10.Description));
        }
    }
}