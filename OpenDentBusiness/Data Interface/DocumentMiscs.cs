using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DocumentMiscs
    {
        public static DocumentMisc GetByTypeAndFileName(string fileName, DocumentMiscType docMiscType)
        {
            string command = "SELECT * FROM documentmisc "
                + "WHERE DocMiscType=" + POut.Int((int)docMiscType) + " "
                + "AND FileName='" + POut.String(fileName) + "'";
            return Crud.DocumentMiscCrud.SelectOne(command);
        }

        ///<summary></summary>
        public static long Insert(DocumentMisc documentMisc)
        {
            return Crud.DocumentMiscCrud.Insert(documentMisc);
        }

        ///<summary>Appends the passed in rawBase64 string to the RawBase64 column in the db for the UpdateFiles DocMiscType row.</summary>
        public static void AppendRawBase64ForUpdateFiles(string rawBase64)
        {
            string command = "UPDATE documentmisc SET RawBase64=CONCAT(IFNULL(RawBase64, ''),@paramRawBase64) "
                + "WHERE DocMiscType=" + POut.Int((int)DocumentMiscType.UpdateFiles);
            OdSqlParameter paramRawBase64 = new OdSqlParameter("paramRawBase64", OdDbType.Text, rawBase64);
            Db.NonQ(command, paramRawBase64);
        }

        ///<summary></summary>
        public static void DeleteAllForType(DocumentMiscType docMiscType)
        {
            string command = "DELETE FROM documentmisc WHERE DocMiscType=" + POut.Int((int)docMiscType);
            Db.NonQ(command);
        }
    }
}