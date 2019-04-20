using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class AlertCategories
    {
        class AlertCategoryCache : CacheListAbs<AlertCategory>
        {
            protected override List<AlertCategory> GetCacheFromDb()
            {
                string command = "SELECT * FROM alertcategory";
                return Crud.AlertCategoryCrud.SelectMany(command);
            }

            protected override List<AlertCategory> TableToList(DataTable table) => Crud.AlertCategoryCrud.TableToList(table);

            protected override AlertCategory Copy(AlertCategory alertCategory) => alertCategory.Copy();

            protected override DataTable ListToTable(List<AlertCategory> listAlertCategories)
            {
                return Crud.AlertCategoryCrud.ListToTable(listAlertCategories, "AlertCategory");
            }

            protected override void FillCacheIfNeeded() => AlertCategories.GetTableFromCache(false);
        }

        static AlertCategoryCache _alertCategoryCache = new AlertCategoryCache();

        public static List<AlertCategory> GetDeepCopy(bool isShort = false) => _alertCategoryCache.GetDeepCopy(isShort);

        /// <summary>
        /// Fills the local cache with the passed in DataTable.
        /// </summary>
        public static void FillCacheFromTable(DataTable table) => _alertCategoryCache.FillCacheFromTable(table);

        /// <summary>
        /// Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.
        /// </summary>
        /// <param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _alertCategoryCache.GetTableFromCache(doRefreshCache);
        }

        /// <summary>
        /// Gets one AlertCategory from the db.
        /// </summary>
        public static AlertCategory GetOne(long alertCategoryNum) => Crud.AlertCategoryCrud.SelectOne(alertCategoryNum);

        public static long Insert(AlertCategory alertCategory) => Crud.AlertCategoryCrud.Insert(alertCategory);

        public static void Update(AlertCategory alertCategory) => Crud.AlertCategoryCrud.Update(alertCategory);

        public static void Delete(long alertCategoryNum) => Crud.AlertCategoryCrud.Delete(alertCategoryNum);

        /// <summary>
        /// Inserts, updates, or deletes db rows to match listNew.
        /// No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.
        /// Doesn't create ApptComm items, but will delete them.
        /// If you use Sync, you must create new AlertCategories items.
        /// </summary>
        public static bool Sync(List<AlertCategory> listNew, List<AlertCategory> listOld) => Crud.AlertCategoryCrud.Sync(listNew, listOld);
    }
}