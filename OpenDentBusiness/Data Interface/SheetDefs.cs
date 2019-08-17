﻿using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SheetDefs
    {
        #region Misc Methods

        ///<summary>Returns true if this sheet def is allowed to bypass the global lock date.</summary>
        public static bool CanBypassLockDate(long sheetDefNum)
        {
            SheetDef sheetDef = SheetDefs.GetFirstOrDefault(x => x.SheetDefNum == sheetDefNum);
            if (sheetDef == null)
            {
                return false;
            }
            return (sheetDef.BypassGlobalLock == BypassLockStatus.BypassAlways);
        }

        ///<summary>SheetType must either by PatientForm of MedicalHistory.</summary>
        public static bool IsWebFormAllowed(SheetTypeEnum sheetType)
        {
            return sheetType.In(SheetTypeEnum.PatientForm, SheetTypeEnum.MedicalHistory);
        }

        ///<summary>Determines if a sheetDef is of a SheetTypeEnum that describes a Dashboard.</summary>
        public static bool IsDashboardType(SheetDef sheetDef)
        {
            return IsDashboardType(sheetDef.SheetType);
        }

        ///<summary>Determines if a SheetTypeEnum is a Dashboard.</summary>
        public static bool IsDashboardType(SheetTypeEnum sheetType)
        {
            if (sheetType.In(SheetTypeEnum.PatientDashboard, SheetTypeEnum.PatientDashboardWidget))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region CachePattern

        private class SheetDefCache : CacheListAbs<SheetDef>
        {
            ///<summary>Ordered by Description and then SheetDefNum to be a deterministic sorting.  This matches the sorting in GetInternalOrCustom().
            ///So the order in the grid matches the order when choosing a sheetdef for use.</summary>
            protected override List<SheetDef> GetCacheFromDb()
            {
                string command = "SELECT * FROM sheetdef ORDER BY Description,SheetDefNum";
                return Crud.SheetDefCrud.SelectMany(command);
            }
            protected override List<SheetDef> TableToList(DataTable table)
            {
                return Crud.SheetDefCrud.TableToList(table);
            }
            protected override SheetDef Copy(SheetDef sheetDef)
            {
                return sheetDef.Copy();
            }
            protected override DataTable ListToTable(List<SheetDef> listSheetDefs)
            {
                return Crud.SheetDefCrud.ListToTable(listSheetDefs, "SheetDef");
            }
            protected override void FillCacheIfNeeded()
            {
                SheetDefs.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static SheetDefCache _sheetDefCache = new SheetDefCache();

        public static List<SheetDef> GetDeepCopy(bool isShort = false)
        {
            return _sheetDefCache.GetDeepCopy(isShort);
        }

        public static List<SheetDef> GetWhere(Predicate<SheetDef> match, bool isShort = false)
        {
            return _sheetDefCache.GetWhere(match, isShort);
        }

        public static SheetDef GetFirstOrDefault(Func<SheetDef, bool> match, bool isShort = false)
        {
            return _sheetDefCache.GetFirstOrDefault(match, isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _sheetDefCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _sheetDefCache.GetTableFromCache(doRefreshCache);
        }

        #endregion

        ///<Summary>Gets one SheetDef from the cache.  Also includes the fields and parameters for the sheetdef.</Summary>
        public static SheetDef GetSheetDef(long sheetDefNum, bool hasExceptions = true)
        {
            //No need to check RemotingRole; no call to db.
            SheetDef sheetdef = GetFirstOrDefault(x => x.SheetDefNum == sheetDefNum);
            if (hasExceptions || sheetdef != null)
            {
                GetFieldsAndParameters(sheetdef);
            }
            return sheetdef;
        }

        ///<summary>Updates the SheetDef only.  Does not included attached fields.</summary>
        public static long Update(SheetDef sheetDef)
        {
            Crud.SheetDefCrud.Update(sheetDef);
            return sheetDef.SheetDefNum;
        }

        ///<summary>Includes all attached fields.  Intelligently inserts, updates, or deletes old fields.</summary>
        public static long InsertOrUpdate(SheetDef sheetDef)
        {
            if (sheetDef.IsNew)
            {
                sheetDef.SheetDefNum = Crud.SheetDefCrud.Insert(sheetDef);
            }
            else
            {
                Crud.SheetDefCrud.Update(sheetDef);
            }
            foreach (SheetFieldDef field in sheetDef.SheetFieldDefs)
            {
                field.SheetDefNum = sheetDef.SheetDefNum;
            }
            SheetFieldDefs.Sync(sheetDef.SheetFieldDefs, sheetDef.SheetDefNum);
            return sheetDef.SheetDefNum;
        }

        ///<summary></summary>
        public static void DeleteObject(long sheetDefNum)
        {
            //validate that not already in use by a refferral.
            string command = "SELECT LName,FName FROM referral WHERE Slip=" + POut.Long(sheetDefNum);
            DataTable table = Db.GetTable(command);
            //int count=PIn.PInt(Db.GetCount(command));
            string referralNames = "";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (i > 0)
                {
                    referralNames += ", ";
                }
                referralNames += table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["LName"].ToString();
            }
            if (table.Rows.Count > 0)
            {
                throw new ApplicationException(Lans.g("sheetDefs", "SheetDef is already in use by referrals. Not allowed to delete.") + " " + referralNames);
            }
            //validate that not already in use by automation.
            command = "SELECT AutomationNum FROM automation WHERE SheetDefNum=" + POut.Long(sheetDefNum);
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                throw new ApplicationException(Lans.g("sheetDefs", "SheetDef is in use by automation. Not allowed to delete."));
            }
            //validate that not already in use by a laboratory
            command = "SELECT Description FROM laboratory WHERE Slip=" + POut.Long(sheetDefNum);
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                throw new ApplicationException(Lans.g("sheetDefs", "SheetDef is in use by laboratories. Not allowed to delete.")
                    + "\r\n" + string.Join(", ", table.Select().Select(x => x["Description"].ToString())));
            }
            //validate that not already in use by clinicPref.
            command = "SELECT ClinicNum FROM clinicpref WHERE ValueString='" + POut.Long(sheetDefNum) + "' AND PrefName='" + POut.String(PreferenceName.SheetsDefaultRx.ToString()) + "'";
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                throw new ApplicationException(Lans.g("sheetDefs", "SheetDef is in use by clinics. Not allowed to delete.")
                    + "\r\n" + string.Join(", ", table.Select().Select(x => Clinics.GetDesc(PIn.Long(x["ClinicNum"].ToString())))));
            }
            command = "DELETE FROM sheetfielddef WHERE SheetDefNum=" + POut.Long(sheetDefNum);
            Db.NonQ(command);
            Crud.SheetDefCrud.Delete(sheetDefNum);
        }

        ///<summary>Sheetdefs and sheetfielddefs are archived separately.
        ///So when we need to use a sheetdef, we must run this method to pull all the associated fields from the archive.
        ///Then it will be ready for printing, copying, etc.</summary>
        public static void GetFieldsAndParameters(SheetDef sheetdef)
        {
            //No need to check RemotingRole; no call to db.
            //images first
            sheetdef.SheetFieldDefs = SheetFieldDefs.GetWhere(x => x.SheetDefNum == sheetdef.SheetDefNum && x.FieldType == SheetFieldType.Image);
            //then all other fields
            sheetdef.SheetFieldDefs.AddRange(SheetFieldDefs.GetWhere(x => x.SheetDefNum == sheetdef.SheetDefNum
                && x.FieldType != SheetFieldType.Image
                && x.FieldType != SheetFieldType.Parameter));//Defs never store parameters. Fields store filled parameters, but that's different.
            sheetdef.Parameters = SheetParameter.GetForType(sheetdef.SheetType);
        }

        ///<summary>Gets all custom sheetdefs(without fields or parameters) for a particular type.</summary>
        public static List<SheetDef> GetCustomForType(SheetTypeEnum sheettype)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.SheetType == sheettype);
        }

        ///<summary>Gets the description from the cache.</summary>
        public static string GetDescription(long sheetDefNum)
        {
            //No need to check RemotingRole; no call to db.
            SheetDef sheetDef = GetFirstOrDefault(x => x.SheetDefNum == sheetDefNum);
            return (sheetDef == null ? "" : sheetDef.Description);
        }

        public static SheetDef GetInternalOrCustom(SheetInternalType sheetInternalType)
        {
            SheetDef retVal = SheetsInternal.GetSheetDef(sheetInternalType);
            SheetDef custom = GetCustomForType(retVal.SheetType).OrderBy(x => x.Description).ThenBy(x => x.SheetDefNum).FirstOrDefault();
            if (custom != null)
            {
                retVal = GetSheetDef(custom.SheetDefNum);
            }
            return retVal;
        }

        public static Dictionary<SheetTypeEnum, SheetDef> GetDefaultSheetDefs()
        {
            //No need to check RemotingRole; no call to db.
            Dictionary<SheetTypeEnum, SheetDef> retVal = new Dictionary<SheetTypeEnum, SheetDef>();
            SheetDef defaultRxDef = GetSheetDef(Preferences.GetDefaultSheetDefNum(SheetTypeEnum.Rx), false);
            if (defaultRxDef == null)
            {
                defaultRxDef = SheetsInternal.GetSheetDef(SheetTypeEnum.Rx);
            }
            retVal.Add(SheetTypeEnum.Rx, defaultRxDef);
            return retVal;
        }

        public static Dictionary<SheetTypeEnum, SheetDef> GetDefaultSheetDefsForClinic(long clinicNum)
        {
            //No need to check RemotingRole; no call to db.
            Dictionary<SheetTypeEnum, SheetDef> retVal = new Dictionary<SheetTypeEnum, SheetDef>();
            ClinicPref clinicPrefCur = ClinicPrefs.GetPref(Preference.GetSheetDefPref(SheetTypeEnum.Rx), clinicNum);
            SheetDef defaultRxDef = SheetsInternal.GetSheetDef(SheetTypeEnum.Rx);
            if (clinicPrefCur != null && PIn.Long(clinicPrefCur.ValueString) != 0)
            {//If ValueString is 0 then we want to keep it as the internal sheet def.
                defaultRxDef = GetSheetDef(PIn.Long(clinicPrefCur.ValueString), false);
            }
            if (clinicPrefCur != null)
            {//If there was a row in the clinicpref table, add whatever the sheetdef was to the retval dictionary.
                retVal.Add(SheetTypeEnum.Rx, defaultRxDef);
            }
            return retVal;
        }

        ///<summary>Passing in a clinicNum of 0 will use the base default sheet def.  Otherwise returns the clinic specific default sheetdef.</summary>
        public static SheetDef GetSheetsDefault(SheetTypeEnum sheetType, long clinicNum = 0)
        {
            //No need to check RemotingRole; no call to db.
            ClinicPref clinicPrefCur = ClinicPrefs.GetPref(Preference.GetSheetDefPref(sheetType), clinicNum);
            SheetDef defaultSheetDef;
            if (clinicPrefCur == null)
            {//If there wasn't a row for the specific clinic, use the base default sheetdef
                defaultSheetDef = GetSheetDef(Preferences.GetDefaultSheetDefNum(sheetType), false);
                if (defaultSheetDef == null)
                {
                    defaultSheetDef = SheetsInternal.GetSheetDef(sheetType);
                }
                return defaultSheetDef;//Return the base default sheetdef
            }
            //Clinic specific sheet def found
            if (PIn.Long(clinicPrefCur.ValueString) == 0)
            {//If ValueString is 0 then we want to keep it as the internal sheet def.
                defaultSheetDef = SheetsInternal.GetSheetDef(sheetType);
            }
            else
            {
                defaultSheetDef = GetSheetDef(PIn.Long(clinicPrefCur.ValueString), false);
            }
            return defaultSheetDef;
        }

        public static Dictionary<long, Dictionary<SheetTypeEnum, SheetDef>> GetAllSheetDefDefaults(List<long> listClinicNums)
        {
            //No need to check RemotingRole; no call to db.
            Dictionary<long, Dictionary<SheetTypeEnum, SheetDef>> retVal = new Dictionary<long, Dictionary<SheetTypeEnum, SheetDef>>();
            retVal.Add(0, GetDefaultSheetDefs());
            foreach (long clinicNum in listClinicNums)
            {
                if (retVal.ContainsKey(clinicNum))
                {
                    continue;//No need to do anything special if they list happened to contain the same clinicnum multiple times.
                }
                Dictionary<SheetTypeEnum, SheetDef> dictDefaultSheetDefs = GetDefaultSheetDefsForClinic(clinicNum);
                if (dictDefaultSheetDefs.Count > 0)
                {
                    retVal.Add(clinicNum, dictDefaultSheetDefs);
                }
            }
            return retVal;
        }
    }
}