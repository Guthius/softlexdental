using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class AlertCategoryLinks
    {
        private class AlertCategoryLinkCache : CacheListAbs<AlertCategoryLink>
        {
            protected override List<AlertCategoryLink> GetCacheFromDb()
            {
                return Crud.AlertCategoryLinkCrud.SelectMany("SELECT * FROM alertcategorylink");
            }

            protected override List<AlertCategoryLink> TableToList(DataTable table)
            {
                return Crud.AlertCategoryLinkCrud.TableToList(table);
            }

            protected override AlertCategoryLink Copy(AlertCategoryLink alertCategoryLink) => (AlertCategoryLink)alertCategoryLink.Clone();
            
            protected override DataTable ListToTable(List<AlertCategoryLink> listAlertCategoryLinks)
            {
                return Crud.AlertCategoryLinkCrud.ListToTable(listAlertCategoryLinks, "AlertCategoryLink");
            }

            protected override void FillCacheIfNeeded() => AlertCategoryLinks.GetTableFromCache(false);
        }

        static AlertCategoryLinkCache _alertCategoryLinkCache = new AlertCategoryLinkCache();

        public static List<AlertCategoryLink> GetWhere(Predicate<AlertCategoryLink> match, bool isShort = false)
        {
            return _alertCategoryLinkCache.GetWhere(match, isShort);
        }

        /// <summary>
        /// Fills the local cache with the passed in DataTable.
        /// </summary>
        public static void FillCacheFromTable(DataTable table) => _alertCategoryLinkCache.FillCacheFromTable(table);

        /// <summary>
        /// Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.
        /// </summary>
        /// <param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _alertCategoryLinkCache.GetTableFromCache(doRefreshCache);
        }

        /// <summary>
        /// Gets one AlertCategoryLink from the db.
        /// </summary>
        public static AlertCategoryLink GetOne(long alertCategoryLinkNum) => Crud.AlertCategoryLinkCrud.SelectOne(alertCategoryLinkNum);

        public static List<AlertCategoryLink> GetForCategory(long alertCategoryNum)
        {
            if (alertCategoryNum == 0)
            {
                return new List<AlertCategoryLink>();
            }
            return Crud.AlertCategoryLinkCrud.SelectMany("SELECT * FROM alertcategorylink WHERE AlertCategoryNum = " + POut.Long(alertCategoryNum));
        }

        public static long Insert(AlertCategoryLink alertCategoryLink) => Crud.AlertCategoryLinkCrud.Insert(alertCategoryLink);

        public static void Update(AlertCategoryLink alertCategoryLink) => Crud.AlertCategoryLinkCrud.Update(alertCategoryLink);

        public static void Delete(long alertCategoryLinkNum) => Crud.AlertCategoryLinkCrud.Delete(alertCategoryLinkNum);

        public static void DeleteForCategory(long alertCategoryNum)
        {
            if (alertCategoryNum == 0)
            {
                return;
            }
            Db.NonQ("DELETE FROM alertcategorylink WHERE AlertCategoryNum = " + POut.Long(alertCategoryNum));
        }

        /// <summary>
        /// Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before 
        /// remoting role check and passed to the server if necessary.  Doesn't create ApptComm items, 
        /// but will delete them. If you use Sync, you must create new AlertCategoryLink items.</summary>
        public static bool Sync(List<AlertCategoryLink> listNew, List<AlertCategoryLink> listOld)
        {
            return Crud.AlertCategoryLinkCrud.Sync(listNew, listOld);
        }
    }
}