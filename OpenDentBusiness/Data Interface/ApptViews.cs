using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    public class ApptViews
    {
        /// <summary>
        /// Optionally pass in a clinic to filter the list of appointment views returned.
        /// </summary>
        public static List<ApptView> GetForClinic(long clinicNum = 0, bool isShort = true)
        {
            if (clinicNum > 0)
            {
                return GetWhere(x => x.ClinicNum == clinicNum, isShort);
            }
            else
            {
                return GetDeepCopy(isShort);
            }
        }

        /// <summary>
        /// Gets an ApptView from the cache.  If apptviewnum is not valid, then it returns null.
        /// </summary>
        public static ApptView GetApptView(long apptViewNum) => GetFirstOrDefault(x => x.ApptViewNum == apptViewNum);

        public static long Insert(ApptView apptView) => Crud.ApptViewCrud.Insert(apptView);

        public static void Update(ApptView apptView) => Crud.ApptViewCrud.Update(apptView);

        public static void Delete(ApptView Cur)
        {
            Db.NonQ("DELETE FROM apptview WHERE ApptViewNum = '" + POut.Long(Cur.ApptViewNum) + "'");
        }

        private class ApptViewCache : CacheListAbs<ApptView>
        {
            protected override List<ApptView> GetCacheFromDb()
            {
                return Crud.ApptViewCrud.SelectMany("SELECT * FROM apptview ORDER BY ClinicNum,ItemOrder");
            }

            protected override List<ApptView> TableToList(DataTable table) => Crud.ApptViewCrud.TableToList(table);

            protected override ApptView Copy(ApptView apptView) => apptView.Copy();

            protected override DataTable ListToTable(List<ApptView> listApptViews)
            {
                return Crud.ApptViewCrud.ListToTable(listApptViews, "ApptView");
            }

            protected override void FillCacheIfNeeded() => ApptViews.GetTableFromCache(false);
        }

        static ApptViewCache _apptViewCache = new ApptViewCache();

        public static List<ApptView> GetDeepCopy(bool isShort = false) => _apptViewCache.GetDeepCopy(isShort);

        static ApptView GetFirstOrDefault(Func<ApptView, bool> match, bool isShort = false)
        {
            return _apptViewCache.GetFirstOrDefault(match, isShort);
        }

        static List<ApptView> GetWhere(Predicate<ApptView> match, bool isShort = false)
        {
            return _apptViewCache.GetWhere(match, isShort);
        }

        /// <summary>
        /// Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.
        /// </summary>
        public static DataTable RefreshCache() => GetTableFromCache(true);

        /// <summary>
        /// Fills the local cache with the passed in DataTable.
        /// </summary>
        public static void FillCacheFromTable(DataTable table) => _apptViewCache.FillCacheFromTable(table);

        /// <summary>
        /// Always refreshes the ClientWeb's cache.
        /// </summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _apptViewCache.GetTableFromCache(doRefreshCache);
        }
    }
}