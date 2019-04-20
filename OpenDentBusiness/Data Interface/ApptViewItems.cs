using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using OpenDentBusiness;
using System.Linq;

namespace OpenDentBusiness
{
    public class ApptViewItems
    {
        private class ApptViewItemCache : CacheListAbs<ApptViewItem>
        {
            protected override List<ApptViewItem> GetCacheFromDb()
            {
                return Crud.ApptViewItemCrud.SelectMany("SELECT * from apptviewitem ORDER BY ElementOrder");
            }

            protected override List<ApptViewItem> TableToList(DataTable table) => Crud.ApptViewItemCrud.TableToList(table);

            protected override ApptViewItem Copy(ApptViewItem apptViewItem) => apptViewItem.Clone();

            protected override DataTable ListToTable(List<ApptViewItem> listApptViewItems)
            {
                return Crud.ApptViewItemCrud.ListToTable(listApptViewItems, "ApptViewItem");
            }

            protected override void FillCacheIfNeeded() => ApptViewItems.GetTableFromCache(false);
        }

        static ApptViewItemCache _apptViewItemCache = new ApptViewItemCache();

        /// <summary>
        /// Gets a deep copy of all matching items from the cache via ListLong.
        /// Set isShort true to search through ListShort instead.
        /// </summary>
        public static List<ApptViewItem> GetWhere(Predicate<ApptViewItem> match, bool isShort = false)
        {
            return _apptViewItemCache.GetWhere(match, isShort);
        }

        /// <summary>
        /// Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.
        /// </summary>
        public static DataTable RefreshCache() => GetTableFromCache(true);


        /// <summary>
        /// Fills the local cache with the passed in DataTable.
        /// </summary>
        public static void FillCacheFromTable(DataTable table) => _apptViewItemCache.FillCacheFromTable(table);

        /// <summary>
        /// Always refreshes the ClientWeb's cache.
        /// </summary>
        public static DataTable GetTableFromCache(bool doRefreshCache) => _apptViewItemCache.GetTableFromCache(doRefreshCache);

        public static long Insert(ApptViewItem apptViewItem) => Crud.ApptViewItemCrud.Insert(apptViewItem);

        public static void Update(ApptViewItem apptViewItem) => Crud.ApptViewItemCrud.Update(apptViewItem);

        public static void Delete(ApptViewItem apptViewItem)
        {
            Db.NonQ("DELETE from apptviewitem WHERE ApptViewItemNum = '" + POut.Long(apptViewItem.ApptViewItemNum) + "'");
        }

        /// <summary>
        /// Deletes all apptviewitems for the current apptView.
        /// </summary>
        public static void DeleteAllForView(ApptView view)
        {
            Db.NonQ("DELETE from apptviewitem WHERE ApptViewNum = '" + POut.Long(view.ApptViewNum) + "'");
        }

        /// <summary>
        /// Gets all operatories for the appointment view passed in. Pass 0 to get all ops associated with the 'none' view.
        /// Only returns operatories that are associated to the currently selected clinic.
        /// </summary>
        public static List<long> GetOpsForView(long apptViewNum)
        {
            List<long> retVal = new List<long>();
            if (apptViewNum == 0)
            {
                bool hasClinicsEnabled = PrefC.HasClinicsEnabled;

                // Simply return all visible ops.  These are the ops that the 'none' appointment view currently displays.
                // Do not consider operatories that are not associated with the currently selected clinic.
                return 
                    Operatories.GetWhere(x => !hasClinicsEnabled || Clinics.ClinicNum == 0 || x.ClinicNum == Clinics.ClinicNum, true).Select(x => x.OperatoryNum).ToList();
            }
            return GetWhere(x => x.ApptViewNum == apptViewNum && x.OpNum != 0).Select(x => x.OpNum).ToList();
        }

        /// <summary>
        /// Gets all providers for the appointment view passed in. 
        /// Pass 0 to get all provs associated with the 'none' view.
        /// </summary>
        public static List<long> GetProvsForView(long apptViewNum)
        {
            if (apptViewNum == 0)
            {
                // Simply return all visible ops.  These are the ops that the 'none' appointment view currently displays.
                List<Operatory> listVisibleOps = Operatories.GetWhere(x => !PrefC.HasClinicsEnabled || Clinics.ClinicNum == 0 || x.ClinicNum == Clinics.ClinicNum, true);
                List<long> listProvNums = listVisibleOps.Select(x => x.ProvDentist).ToList();
                listProvNums.AddRange(listVisibleOps.Select(x => x.ProvHygienist));

                return listProvNums.Distinct().ToList();
            }
            return GetWhere(x => x.ApptViewNum == apptViewNum && x.ProvNum != 0).Select(x => x.ProvNum).ToList();
        }
    }
}