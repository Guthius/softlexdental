using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class AlertReads
    {
        ///<summary></summary>
        public static List<AlertRead> Refresh(long patNum)
        {
            string command = "SELECT * FROM alertread WHERE UserNum = " + POut.Long(patNum);
            return Crud.AlertReadCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static List<AlertRead> RefreshForAlertNums(long patNum, List<long> listAlertItemNums)
        {
            if (listAlertItemNums == null || listAlertItemNums.Count == 0)
            {
                return new List<AlertRead>();
            }
            string command = "SELECT * FROM alertread WHERE UserNum = " + POut.Long(patNum) + " ";
            command += "AND  AlertItemNum IN (" + String.Join(",", listAlertItemNums) + ")";
            return Crud.AlertReadCrud.SelectMany(command);
        }

        ///<summary>Gets one AlertRead from the db.</summary>
        public static AlertRead GetOne(long alertReadNum)
        {
            return Crud.AlertReadCrud.SelectOne(alertReadNum);
        }

        ///<summary></summary>
        public static long Insert(AlertRead alertRead)
        {
            return Crud.AlertReadCrud.Insert(alertRead);
        }

        ///<summary></summary>
        public static void Update(AlertRead alertRead)
        {
            Crud.AlertReadCrud.Update(alertRead);
        }

        ///<summary></summary>
        public static void Delete(long alertReadNum)
        {
            Crud.AlertReadCrud.Delete(alertReadNum);
        }

        ///<summary></summary>
        public static void DeleteForAlertItem(long alertItemNum)
        {
            string command = "DELETE FROM alertread "
                + "WHERE AlertItemNum = " + POut.Long(alertItemNum);
            Db.NonQ(command);
        }

        ///<summary>Deletes all alertreads for the listAlertItemNums.  Used by the OpenDentalService AlertRadiologyProceduresThread.</summary>
        public static void DeleteForAlertItems(List<long> listAlertItemNums)
        {
            if (listAlertItemNums == null || listAlertItemNums.Count == 0)
            {
                return;
            }
            string command = "DELETE FROM alertread "
                + "WHERE AlertItemNum IN (" + string.Join(",", listAlertItemNums.Select(x => POut.Long(x))) + ")";
            Db.NonQ(command);
        }

        ///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
        ///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
        public static bool Sync(List<AlertRead> listNew, List<AlertRead> listOld)
        {
            return Crud.AlertReadCrud.Sync(listNew, listOld);
        }
    }
}