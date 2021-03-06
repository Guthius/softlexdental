using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    public class AppointmentRules
    {
        class AppointmentRuleCache : CacheListAbs<AppointmentRule>
        {
            protected override List<AppointmentRule> GetCacheFromDb()
            {
                return Crud.AppointmentRuleCrud.SelectMany("SELECT * FROM appointmentrule");
            }

            protected override List<AppointmentRule> TableToList(DataTable table)
            {
                return Crud.AppointmentRuleCrud.TableToList(table);
            }

            protected override AppointmentRule Copy(AppointmentRule appointmentRule) => appointmentRule.Clone();


            protected override DataTable ListToTable(List<AppointmentRule> listAppointmentRules)
            {
                return Crud.AppointmentRuleCrud.ListToTable(listAppointmentRules, "AppointmentRule");
            }

            protected override void FillCacheIfNeeded() => AppointmentRules.GetTableFromCache(false);
        }

        static AppointmentRuleCache _appointmentRuleCache = new AppointmentRuleCache();

        public static int GetCount(bool isShort = false)
        {
            return _appointmentRuleCache.GetCount(isShort);
        }

        public static List<AppointmentRule> GetDeepCopy(bool isShort = false)
        {
            return _appointmentRuleCache.GetDeepCopy(isShort);
        }

        public static List<AppointmentRule> GetWhere(Predicate<AppointmentRule> match, bool isShort = false)
        {
            return _appointmentRuleCache.GetWhere(match, isShort);
        }

        public static DataTable RefreshCache() => GetTableFromCache(true);


        /// <summary>
        /// Fills the local cache with the passed in DataTable.
        /// </summary>
        public static void FillCacheFromTable(DataTable table) => _appointmentRuleCache.FillCacheFromTable(table);


        /// <summary>
        /// Always refreshes the ClientWeb's cache.
        /// </summary>
        public static DataTable GetTableFromCache(bool doRefreshCache) => _appointmentRuleCache.GetTableFromCache(doRefreshCache);

        public static long Insert(AppointmentRule appointmentRule) => Crud.AppointmentRuleCrud.Insert(appointmentRule);

        public static void Update(AppointmentRule appointmentRule) => Crud.AppointmentRuleCrud.Update(appointmentRule);

        public static void Delete(AppointmentRule rule)
        {
            Db.NonQ("DELETE FROM appointmentrule WHERE AppointmentRuleNum = " + POut.Long(rule.AppointmentRuleNum));
        }

        /// <summary>
        /// Whenever an appointment is scheduled, the procedures which would be double booked are calculated.
        /// In this method, those procedures are checked to see if the double booking should be blocked.
        /// If double booking is indeed blocked, then a separate function will tell the user which category.
        /// </summary>
        public static bool IsBlocked(ArrayList codes)
        {
            List<AppointmentRule> listRules = GetWhere(x => x.IsEnabled);
            for (int j = 0; j < codes.Count; j++)
            {
                for (int i = 0; i < listRules.Count; i++)
                {
                    if (string.Compare((string)codes[j], listRules[i].CodeStart) < 0)
                    {
                        continue;
                    }
                    if (string.Compare((string)codes[j], listRules[i].CodeEnd) > 0)
                    {
                        continue;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whenever an appointment is blocked from being double booked, this method will tell the user which category.
        /// </summary>
        public static string GetBlockedDescription(ArrayList codes)
        {
            List<AppointmentRule> listRules = GetDeepCopy();
            for (int j = 0; j < codes.Count; j++)
            {
                for (int i = 0; i < listRules.Count; i++)
                {
                    if (!listRules[i].IsEnabled)
                    {
                        continue;
                    }
                    if (string.Compare((string)codes[j], listRules[i].CodeStart) < 0)
                    {
                        continue;
                    }
                    if (string.Compare((string)codes[j], listRules[i].CodeEnd) > 0)
                    {
                        continue;
                    }
                    return listRules[i].RuleDesc;
                }
            }
            return "";
        }
    }
}