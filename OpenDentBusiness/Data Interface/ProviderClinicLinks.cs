using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ProviderClinicLinks
    {
        #region Cache Pattern

        private class ProviderClinicLinkCache : CacheListAbs<ProviderClinicLink>
        {
            protected override List<ProviderClinicLink> GetCacheFromDb()
            {
                string command = "SELECT * FROM providercliniclink";
                return Crud.ProviderClinicLinkCrud.SelectMany(command);
            }
            protected override List<ProviderClinicLink> TableToList(DataTable table)
            {
                return Crud.ProviderClinicLinkCrud.TableToList(table);
            }
            protected override ProviderClinicLink Copy(ProviderClinicLink providerClinicLink)
            {
                return providerClinicLink.Copy();
            }
            protected override DataTable ListToTable(List<ProviderClinicLink> listProviderClinicLinks)
            {
                return Crud.ProviderClinicLinkCrud.ListToTable(listProviderClinicLinks, "ProviderClinicLink");
            }
            protected override void FillCacheIfNeeded()
            {
                ProviderClinicLinks.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static ProviderClinicLinkCache _providerClinicLinkCache = new ProviderClinicLinkCache();

        public static List<ProviderClinicLink> GetDeepCopy(bool isShort = false)
        {
            return _providerClinicLinkCache.GetDeepCopy(isShort);
        }

        public static int GetCount(bool isShort = false)
        {
            return _providerClinicLinkCache.GetCount(isShort);
        }

        public static bool GetExists(Predicate<ProviderClinicLink> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetExists(match, isShort);
        }

        public static int GetFindIndex(Predicate<ProviderClinicLink> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetFindIndex(match, isShort);
        }

        public static ProviderClinicLink GetFirst(bool isShort = false)
        {
            return _providerClinicLinkCache.GetFirst(isShort);
        }

        public static ProviderClinicLink GetFirst(Func<ProviderClinicLink, bool> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetFirst(match, isShort);
        }

        public static ProviderClinicLink GetFirstOrDefault(Func<ProviderClinicLink, bool> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetFirstOrDefault(match, isShort);
        }

        public static ProviderClinicLink GetLast(bool isShort = false)
        {
            return _providerClinicLinkCache.GetLast(isShort);
        }

        public static ProviderClinicLink GetLastOrDefault(Func<ProviderClinicLink, bool> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetLastOrDefault(match, isShort);
        }

        public static List<ProviderClinicLink> GetWhere(Predicate<ProviderClinicLink> match, bool isShort = false)
        {
            return _providerClinicLinkCache.GetWhere(match, isShort);
        }

        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _providerClinicLinkCache.FillCacheFromTable(table);
        }

        ///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
        ///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _providerClinicLinkCache.GetTableFromCache(doRefreshCache);
        }
        #endregion Cache Pattern

        public static List<ProviderClinicLink> GetForProvider(long provNum)
        {
            return GetWhere(x => x.ProvNum == provNum);
        }

        public static List<ProviderClinicLink> GetForClinic(long clinicNum)
        {
            return GetWhere(x => x.ClinicNum == clinicNum);
        }

        ///<summary>Returns the providers that are associated to other clinics but not this one.</summary>
        public static List<long> GetProvsRestrictedToOtherClinics(long clinicNum)
        {
            if (clinicNum == 0)
            {//Consider 0 as 'Headquarters'
                return new List<long>();
            }
            HashSet<long> hashSetProvsThisClinic = new HashSet<long>(GetForClinic(clinicNum).Select(x => x.ProvNum));
            return GetWhere(x => x.ClinicNum != clinicNum && !hashSetProvsThisClinic.Contains(x.ProvNum))
                .Select(x => x.ProvNum).Distinct().ToList();
        }

        ///<summary></summary>
        public static bool Sync(List<ProviderClinicLink> listNew, List<ProviderClinicLink> listDB)
        {
            return Crud.ProviderClinicLinkCrud.Sync(listNew, listDB);
        }
    }
}