using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Cvxs
    {
        ///<summary></summary>
        public static long Insert(Cvx cvx)
        {
            return Crud.CvxCrud.Insert(cvx);
        }

        ///<summary></summary>
        public static void Update(Cvx cvx)
        {
            Crud.CvxCrud.Update(cvx);
        }

        public static List<Cvx> GetAll()
        {
            string command = "SELECT * FROM cvx";
            return Crud.CvxCrud.SelectMany(command);
        }

        ///<summary>Returns a list of just the codes for use in update or insert logic.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT CvxCode FROM cvx";
            DataTable table = DataCore.GetTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i][0].ToString());
            }
            return retVal;
        }

        ///<summary>Gets one Cvx object directly from the database by CodeValue.  If code does not exist, returns null.</summary>
        public static Cvx GetByCode(string cvxCode)
        {
            string command = "SELECT * FROM Cvx WHERE CvxCode='" + POut.String(cvxCode) + "'";
            return Crud.CvxCrud.SelectOne(command);
        }

        ///<summary>Gets one Cvx by CvxNum directly from the db.</summary>
        public static Cvx GetOneFromDb(string cvxCode)
        {
            string command = "SELECT * FROM cvx WHERE CvxCode='" + POut.String(cvxCode) + "'";
            return Crud.CvxCrud.SelectOne(command);
        }

        ///<summary>Directly from db.</summary>
        public static bool CodeExists(string cvxCode)
        {
            string command = "SELECT COUNT(*) FROM cvx WHERE CvxCode='" + POut.String(cvxCode) + "'";
            string count = Db.GetCount(command);
            if (count == "0")
            {
                return false;
            }
            return true;
        }

        ///<summary>Returns the total count of CVX codes.  CVS codes cannot be hidden, but might in the future be set active/inactive using the IsActive flag.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM cvx";
            return PIn.Long(Db.GetCount(command));
        }

        public static List<Cvx> GetBySearchText(string searchText)
        {
            string[] searchTokens = searchText.Split(' ');
            string command = @"SELECT * FROM cvx ";
            for (int i = 0; i < searchTokens.Length; i++)
            {
                command += (i == 0 ? "WHERE " : "AND ") + "(CvxCode LIKE '%" + POut.String(searchTokens[i]) + "%' OR Description LIKE '%" + POut.String(searchTokens[i]) + "%') ";
            }
            return Crud.CvxCrud.SelectMany(command);
        }
    }
}