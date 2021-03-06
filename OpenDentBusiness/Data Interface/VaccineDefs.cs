using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class VaccineDefs
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion

        #region CachePattern

        private class VaccineDefCache : CacheListAbs<VaccineDef>
        {
            protected override List<VaccineDef> GetCacheFromDb()
            {
                string command = "SELECT * FROM vaccinedef ORDER BY CVXCode";
                return Crud.VaccineDefCrud.SelectMany(command);
            }
            protected override List<VaccineDef> TableToList(DataTable table)
            {
                return Crud.VaccineDefCrud.TableToList(table);
            }
            protected override VaccineDef Copy(VaccineDef vaccineDef)
            {
                return vaccineDef.Copy();
            }
            protected override DataTable ListToTable(List<VaccineDef> listVaccineDefs)
            {
                return Crud.VaccineDefCrud.ListToTable(listVaccineDefs, "VaccineDef");
            }
            protected override void FillCacheIfNeeded()
            {
                VaccineDefs.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static VaccineDefCache _vaccineDefCache = new VaccineDefCache();

        public static bool GetExists(Predicate<VaccineDef> match, bool isShort = false)
        {
            return _vaccineDefCache.GetExists(match, isShort);
        }

        public static VaccineDef GetFirstOrDefault(Func<VaccineDef, bool> match, bool isShort = false)
        {
            return _vaccineDefCache.GetFirstOrDefault(match, isShort);
        }

        public static List<VaccineDef> GetDeepCopy(bool isShort = false)
        {
            return _vaccineDefCache.GetDeepCopy(isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _vaccineDefCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _vaccineDefCache.GetTableFromCache(doRefreshCache);
        }

        #endregion

        ///<summary>Gets one VaccineDef from the db.</summary>
        public static VaccineDef GetOne(long vaccineDefNum)
        {
            return Crud.VaccineDefCrud.SelectOne(vaccineDefNum);
        }

        ///<summary></summary>
        public static long Insert(VaccineDef vaccineDef)
        {
            return Crud.VaccineDefCrud.Insert(vaccineDef);
        }

        ///<summary></summary>
        public static void Update(VaccineDef vaccineDef)
        {
            Crud.VaccineDefCrud.Update(vaccineDef);
        }

        ///<summary></summary>
        public static void Delete(long vaccineDefNum)
        {
            //validation
            string command;
            command = "SELECT COUNT(*) FROM VaccinePat WHERE VaccineDefNum=" + POut.Long(vaccineDefNum);
            if (Db.GetCount(command) != "0")
            {
                throw new ApplicationException(Lans.g("FormDrugUnitEdit", "Cannot delete: VaccineDef is in use by VaccinePat."));
            }
            command = "DELETE FROM vaccinedef WHERE VaccineDefNum = " + POut.Long(vaccineDefNum);
            Db.NonQ(command);
        }
    }
}