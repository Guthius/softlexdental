using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ionic.Zip;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class RxNorms
    {
        ///<summary>RxNorm table is considered to be too small if less than 50 RxNorms in table,
        ///because our default medication list contains 50 items, implying that the user has not imported RxNorms.</summary>
        public static bool IsRxNormTableSmall()
        {
            string command = "SELECT COUNT(*) FROM rxnorm";
            if (PIn.Int(Db.GetCount(command)) < 50)
            {
                return true;
            }
            return false;
        }

        public static RxNorm GetByRxCUI(string rxCui)
        {
            string command = "SELECT * FROM rxnorm WHERE RxCui='" + POut.String(rxCui) + "' AND MmslCode=''";
            return Crud.RxNormCrud.SelectOne(command);
        }

        ///<summary>Never returns multums, only used for displaying after a search.</summary>
        public static List<RxNorm> GetListByCodeOrDesc(string codeOrDesc, bool isExact, bool ignoreNumbers)
        {
            string command = "SELECT * FROM rxnorm WHERE MmslCode='' ";
            if (ignoreNumbers)
            {
                command += "AND Description NOT REGEXP '.*[0-9]+.*' ";
            }
            if (isExact)
            {
                command += "AND (RxCui LIKE '" + POut.String(codeOrDesc) + "' OR Description LIKE '" + POut.String(codeOrDesc) + "')";
            }
            else
            {//Similar matches
                string[] arraySearchWords = codeOrDesc.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (arraySearchWords.Length > 0)
                {
                    command += "AND ("
                        + "RxCui LIKE '%" + POut.String(codeOrDesc) + "%' "
                        + " OR "
                        + "(" + String.Join(" AND ", arraySearchWords.Select(x => "Description LIKE '%" + POut.String(x) + "%'")) + ") "
                        + ")";
                }
            }
            command += " ORDER BY Description";
            return Crud.RxNormCrud.SelectMany(command);
        }

        ///<summary>Used to return the multum code based on RxCui.  If blank, use the Description instead.</summary>
        public static string GetMmslCodeByRxCui(string rxCui)
        {
            string command = "SELECT MmslCode FROM rxnorm WHERE MmslCode!='' AND RxCui='" + rxCui + "'";
            return Db.GetScalar(command);
        }

        ///<summary></summary>
        public static string GetDescByRxCui(string rxCui)
        {
            string command = "SELECT Description FROM rxnorm WHERE MmslCode='' AND RxCui='" + rxCui + "'";
            return Db.GetScalar(command);
        }

        ///<summary>Gets one RxNorm from the db.</summary>
        public static RxNorm GetOne(long rxNormNum)
        {
            return Crud.RxNormCrud.SelectOne(rxNormNum);
        }

        ///<summary></summary>
        public static long Insert(RxNorm rxNorm)
        {
            return Crud.RxNormCrud.Insert(rxNorm);
        }

        ///<summary></summary>
        public static void Update(RxNorm rxNorm)
        {
            Crud.RxNormCrud.Update(rxNorm);
        }

        ///<summary></summary>
        public static List<RxNorm> GetAll()
        {
            string command = "SELECT * FROM rxnorm";
            return Crud.RxNormCrud.SelectMany(command);
        }

        ///<summary>Returns a list of just the codes for use in the codesystem import tool.</summary>
        public static List<string> GetAllCodes()
        {
            List<string> retVal = new List<string>();
            string command = "SELECT RxCui FROM rxnorm";//will return some duplicates due to the nature of the data in the table. This is acceptable.
            DataTable table = DataConnection.ExecuteDataTable(command);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal.Add(table.Rows[i].ItemArray[0].ToString());
            }
            return retVal;
        }

        ///<summary>Returns the count of all RxNorm codes in the database.  RxNorms cannot be hidden.</summary>
        public static long GetCodeCount()
        {
            string command = "SELECT COUNT(*) FROM rxnorm";
            return PIn.Long(Db.GetCount(command));
        }
    }
}