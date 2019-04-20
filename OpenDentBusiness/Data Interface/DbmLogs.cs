using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DbmLogs
    {
        ///<summary>Gets one DbmLog from the db.</summary>
        public static DbmLog GetOne(long dbmLogNum)
        {
            return Crud.DbmLogCrud.SelectOne(dbmLogNum);
        }

        ///<summary></summary>
        public static long Insert(DbmLog dbmLog)
        {
            return Crud.DbmLogCrud.Insert(dbmLog);
        }

        public static void InsertMany(List<DbmLog> listDbmLogs)
        {
            Crud.DbmLogCrud.InsertMany(listDbmLogs);
        }

        ///<summary></summary>
        public static void Update(DbmLog dbmLog)
        {
            Crud.DbmLogCrud.Update(dbmLog);
        }

        ///<summary></summary>
        public static void Delete(long dbmLogNum)
        {
            Crud.DbmLogCrud.Delete(dbmLogNum);
        }
    }
}