using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class AlertReads
    {
        public static List<AlertRead> Refresh(long patNum)
        {
            return Crud.AlertReadCrud.SelectMany("SELECT * FROM alertread WHERE UserNum = " + POut.Long(patNum));
        }

        public static List<AlertRead> RefreshForAlertNums(long patNum, List<long> listAlertItemNums)
        {
            if (listAlertItemNums == null || listAlertItemNums.Count == 0)
            {
                return new List<AlertRead>();
            }

            return Crud.AlertReadCrud.SelectMany(
                "SELECT * FROM alertread " +
                "WHERE UserNum = " + POut.Long(patNum) + " " +
                "AND  AlertItemNum IN (" + String.Join(",", listAlertItemNums) + ")");
        }

        /// <summary>
        /// Gets one AlertRead from the db.
        /// </summary>
        public static AlertRead GetOne(long alertReadNum) => Crud.AlertReadCrud.SelectOne(alertReadNum);

        public static long Insert(AlertRead alertRead) => Crud.AlertReadCrud.Insert(alertRead);

        public static void Update(AlertRead alertRead) => Crud.AlertReadCrud.Update(alertRead);

        public static void Delete(long alertReadNum) => Crud.AlertReadCrud.Delete(alertReadNum);

        public static void DeleteForAlertItem(long alertItemNum)
        {
            Db.NonQ("DELETE FROM alertread WHERE AlertItemNum = " + POut.Long(alertItemNum));
        }

        /// <summary>
        /// Deletes all alertreads for the listAlertItemNums.
        /// Used by the OpenDentalService AlertRadiologyProceduresThread.
        /// </summary>
        public static void DeleteForAlertItems(List<long> listAlertItemNums)
        {
            if (listAlertItemNums == null || listAlertItemNums.Count == 0)
            {
                return;
            }
            Db.NonQ("DELETE FROM alertread WHERE AlertItemNum IN (" + string.Join(",", listAlertItemNums.Select(x => POut.Long(x))) + ")");
        }

        /// <summary>
        /// Inserts, updates, or deletes db rows to match listNew.
        /// No need to pass in userNum, it's set before remoting role check and passed to
        /// the server if necessary. Doesn't create ApptComm items, but will delete them. 
        /// If you use Sync, you must create new Apptcomm items.
        /// </summary>
        public static bool Sync(List<AlertRead> listNew, List<AlertRead> listOld) => Crud.AlertReadCrud.Sync(listNew, listOld);
    }
}