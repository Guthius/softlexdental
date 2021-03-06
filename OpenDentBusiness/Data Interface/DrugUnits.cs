using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DrugUnits
    {
        #region CachePattern

        private class DrugUnitCache : CacheListAbs<DrugUnit>
        {
            protected override List<DrugUnit> GetCacheFromDb()
            {
                string command = "SELECT * FROM drugunit ORDER BY UnitIdentifier";
                return Crud.DrugUnitCrud.SelectMany(command);
            }
            protected override List<DrugUnit> TableToList(DataTable table)
            {
                return Crud.DrugUnitCrud.TableToList(table);
            }
            protected override DrugUnit Copy(DrugUnit drugUnit)
            {
                return drugUnit.Copy();
            }
            protected override DataTable ListToTable(List<DrugUnit> listDrugUnits)
            {
                return Crud.DrugUnitCrud.ListToTable(listDrugUnits, "DrugUnit");
            }
            protected override void FillCacheIfNeeded()
            {
                DrugUnits.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static DrugUnitCache _drugUnitCache = new DrugUnitCache();

        public static bool GetExists(Predicate<DrugUnit> match, bool isShort = false)
        {
            return _drugUnitCache.GetExists(match, isShort);
        }

        public static List<DrugUnit> GetDeepCopy(bool isShort = false)
        {
            return _drugUnitCache.GetDeepCopy(isShort);
        }

        public static DrugUnit GetFirstOrDefault(Func<DrugUnit, bool> match, bool isShort = false)
        {
            return _drugUnitCache.GetFirstOrDefault(match, isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _drugUnitCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _drugUnitCache.GetTableFromCache(doRefreshCache);
        }

        #endregion

        ///<summary>Gets one DrugUnit from the db.</summary>
        public static DrugUnit GetOne(long drugUnitNum)
        {
            return Crud.DrugUnitCrud.SelectOne(drugUnitNum);
        }

        ///<summary></summary>
        public static long Insert(DrugUnit drugUnit)
        {
            return Crud.DrugUnitCrud.Insert(drugUnit);
        }

        ///<summary></summary>
        public static void Update(DrugUnit drugUnit)
        {
            Crud.DrugUnitCrud.Update(drugUnit);
        }

        ///<summary>Surround with a try/catch.  Will fail if drug unit is in use.</summary>
        public static void Delete(long drugUnitNum)
        {
            //validation
            string command;
            //no longer used in labresult
            //command="SELECT COUNT(*) FROM labresult WHERE DrugUnitNum="+POut.Long(drugUnitNum);
            //if(Db.GetCount(command)!="0") {
            //	throw new ApplicationException(Lans.g("FormDrugUnitEdit","Cannot delete: DrugUnit is in use by LabResult."));
            //}
            command = "SELECT COUNT(*) FROM vaccinepat WHERE DrugUnitNum=" + POut.Long(drugUnitNum);
            if (Db.GetCount(command) != "0")
            {
                throw new ApplicationException(Lans.g("FormDrugUnitEdit", "Cannot delete: DrugUnit is in use by VaccinePat."));
            }
            command = "DELETE FROM drugunit WHERE DrugUnitNum = " + POut.Long(drugUnitNum);
            Db.NonQ(command);
        }

        ///<summary>Used along with GetChangedSinceDrugUnitNums</summary>
        public static List<DrugUnit> GetMultDrugUnits(List<long> drugUnitNums)
        {
            if (drugUnitNums.Count == 0)
            {
                return new List<DrugUnit>();
            }
            string strDrugUnitNums = "";
            for (int i = 0; i < drugUnitNums.Count; i++)
            {
                if (i > 0)
                {
                    strDrugUnitNums += "OR ";
                }
                strDrugUnitNums += "DrugUnitNum='" + drugUnitNums[i].ToString() + "' ";
            }
            string command = "SELECT * FROM drugunit WHERE " + strDrugUnitNums;
            DataTable table = Db.GetTable(command);
            return Crud.DrugUnitCrud.TableToList(table);
        }
    }
}