using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    public class OrthoChartTabs
    {
        private class OrthoChartTabCache : CacheListAbs<OrthoChartTab>
        {
            protected override List<OrthoChartTab> GetCacheFromDb() => 
                Crud.OrthoChartTabCrud.SelectMany("SELECT * FROM orthocharttab ORDER BY ItemOrder");

            protected override List<OrthoChartTab> TableToList(DataTable table) =>
                Crud.OrthoChartTabCrud.TableToList(table);

            protected override OrthoChartTab Copy(OrthoChartTab orthoChartTab) => 
                orthoChartTab.Copy();
            
            protected override DataTable ListToTable(List<OrthoChartTab> listOrthoChartTabs) =>
                Crud.OrthoChartTabCrud.ListToTable(listOrthoChartTabs, "OrthoChartTab");
            
            protected override void FillCacheIfNeeded() => 
                OrthoChartTabs.GetTableFromCache(false);

            protected override bool IsInListShort(OrthoChartTab orthoChartTab) => 
                !orthoChartTab.IsHidden;
        }

        /// <summary>
        /// The object that accesses the cache in a thread-safe manner.
        /// </summary>
        private static readonly OrthoChartTabCache orthoChartTabCache = new OrthoChartTabCache();

        public static List<OrthoChartTab> GetDeepCopy(bool isShort = false) =>
            orthoChartTabCache.GetDeepCopy(isShort);

        public static OrthoChartTab GetFirst(bool isShort = false) => 
            orthoChartTabCache.GetFirst(isShort);

        public static int GetCount(bool isShort = false) => 
            orthoChartTabCache.GetCount(isShort);

        /// <summary>
        /// Refreshes the cache and returns it as a DataTable.
        /// </summary>
        public static DataTable RefreshCache() => GetTableFromCache(true);

        /// <summary>
        /// Always refreshes the ClientWeb's cache.
        /// </summary>
        public static DataTable GetTableFromCache(bool doRefreshCache) => 
            orthoChartTabCache.GetTableFromCache(doRefreshCache);

        /// <summary>
        /// Inserts, updates, or deletes the passed in list against the stale list listOld. 
        /// Returns true if db changes were made.
        /// </summary>
        public static bool Sync(List<OrthoChartTab> listNew, List<OrthoChartTab> listOld) => 
            Crud.OrthoChartTabCrud.Sync(listNew, listOld);
    }
}
