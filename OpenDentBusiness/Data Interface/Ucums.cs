using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Ucums
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion

        ///<summary></summary>
        public static long Insert(Ucum ucum)
        {
            return Crud.UcumCrud.Insert(ucum);
        }

        ///<summary></summary>
        public static void Update(Ucum ucum)
        {
            Crud.UcumCrud.Update(ucum);
        }

        public static List<Ucum> GetAll()
        {
            string command = "SELECT * FROM ucum ORDER BY UcumCode";
            return Crud.UcumCrud.SelectMany(command);
        }

        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM ucum";
            return PIn.Long(Db.GetCount(command));
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT UcumCode FROM ucum";
            DataTable table = DataCore.GetTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i].ItemArray[0].ToString());
            }
            return retVal;
        }

        public static Ucum GetByCode(string ucumCode)
        {
            string command;

            //because when we search for UnumCode 'a' for 'year [time]' used for age we sometimes get 'A' for 'Ampere [electric current]'
            //since MySQL is case insensitive, so we compare the binary values of 'a' and 'A' which are 0x61 and 0x41 in Hex respectively.
            command = "SELECT * FROM ucum WHERE CAST(UcumCode AS BINARY)=CAST('" + POut.String(ucumCode) + "' AS BINARY)";

            return Crud.UcumCrud.SelectOne(command);
        }

        public static List<Ucum> GetBySearchText(string searchText)
        {
            string[] searchTokens = searchText.Split(' ');
            string command = @"SELECT * FROM ucum ";
            for (int i = 0; i < searchTokens.Length; i++)
            {
                command += (i == 0 ? "WHERE " : "AND ") + "(UcumCode LIKE '%" + POut.String(searchTokens[i]) + "%' OR Description LIKE '%" + POut.String(searchTokens[i]) + "%') ";
            }
            return Crud.UcumCrud.SelectMany(command);
        }
    }
}