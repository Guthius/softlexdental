using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Cpts
    {
        public static List<Cpt> GetBySearchText(string searchText)
        {
            string[] searchTokens = searchText.Split(' ');
            string command = @"SELECT * FROM cpt ";
            for (int i = 0; i < searchTokens.Length; i++)
            {
                command += (i == 0 ? "WHERE " : "AND ") + "(CptCode LIKE '%" + POut.String(searchTokens[i]) + "%' OR Description LIKE '%" + POut.String(searchTokens[i]) + "%') ";
            }
            return Crud.CptCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(Cpt cpt)
        {
            return Crud.CptCrud.Insert(cpt);
        }

        public static List<Cpt> GetAll()
        {
            string command = "SELECT * FROM cpt";
            return Crud.CptCrud.SelectMany(command);
        }

        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT CptCode FROM cpt";
            DataTable table = DataConnection.ExecuteDataTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i][0].ToString());
            }
            return retVal;
        }

        ///<summary>Gets one Cpt object directly from the database by CptCode.  If code does not exist, returns null.</summary>
        public static Cpt GetByCode(string cptCode)
        {
            string command = "SELECT * FROM cpt WHERE CptCode='" + POut.String(cptCode) + "'";
            return Crud.CptCrud.SelectOne(command);
        }

        ///<summary>Directly from db.</summary>
        public static bool CodeExists(string cptCode)
        {
            string command = "SELECT COUNT(*) FROM cpt WHERE CptCode = '" + POut.String(cptCode) + "'";
            string count = Db.GetCount(command);
            if (count == "0")
            {
                return false;
            }
            return true;
        }

        ///<summary>Returns the total count of CPT codes.  CPT codes cannot be hidden.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM cpt";
            return PIn.Long(Db.GetCount(command));
        }

        ///<summary>Updates an existing CPT code description if versionID is newer than current versionIDs.  If versionID is different than existing versionIDs, it will be added to the comma delimited list.</summary>
        public static void UpdateDescription(string cptCode, string description, string versionID)
        {
            Cpt cpt = Cpts.GetByCode(POut.String(cptCode));
            string[] versionIDs = cpt.VersionIDs.Split(',');
            bool versionIDFound = false;
            string maxVersionID = "";
            for (int i = 0; i < versionIDs.Length; i++)
            {
                if (string.Compare(versionIDs[i], maxVersionID) > 0)
                {//Find max versionID in list
                    maxVersionID = versionIDs[i];
                }
                if (versionIDs[i] == versionID)
                {//Find if versionID is already in list
                    versionIDFound = true;
                }
            }
            if (!versionIDFound)
            {//If the current version isn't already in the list
                cpt.VersionIDs += ',' + versionID;  //VersionID should never be blank for an existing code... should we check?
            }
            if (string.Compare(versionID, maxVersionID) >= 0)
            { //If newest version
                cpt.Description = description;
            }
            Crud.CptCrud.Update(cpt);
        }
    }
}