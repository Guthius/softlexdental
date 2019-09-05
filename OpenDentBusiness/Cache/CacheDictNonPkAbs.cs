using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace OpenDentBusiness
{
    /// <summary>
    /// Some cache classes need to have a dictionary for faster lookup capabilities and do not want
    /// to use a unique key (PK). This class will be more wasteful and the dictionary will be less 
    /// trustworthy than CachDictAbs so use it only when necessary.
    /// 
    /// It will be more wasteful in the sense that it will store two deep copies of the entire 
    /// cache instead of just one. It will be less trustworthy because the dictionary has the 
    /// potential to be missing items that are present within the list. This class was created in 
    /// order to preserve old behavior for several of our caches that don't care if the dict 
    /// ismissing data at times.
    /// </summary>
    public abstract class CacheDictNonPkAbs<T, TKey, TValue> : CacheDictAbs<T, TKey, TValue>
    {
        protected abstract DataTable ListToTable(List<T> listAllItems);

        /// <summary>
        /// A lock object that allows multiple threads to obtain a read lock but allows only one 
        /// thread to obtain a write lock.
        /// </summary>
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        
        /// <summary>
        /// The list of the actual cached objects.  Will always contain every possible cache item.
        /// This will be used whenever a method out in the wild decides it cannot trust the 
        /// dictionary in the base class to have what it needs. The base dict cannot be trusted 
        /// when multiple Ts that share the same key. E.g. multiple procedure codes with the same 
        /// ProcCode.
        /// </summary>
        private List<T> allItems;

        public List<T> GetDeepCopyList(bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowListHelper(isShort).Select(x => Copy(x)).ToList();
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        ///<summary>Several methods that manipulate _listAllItems need the "short" version sometimes.  This helper method simply saves code.
        ///Calling method must already be in a Read or Write lock, if not then this method will throw an exception.</summary>
        private List<T> GetShallowListHelper(bool isShort = false)
        {
            if (!slimLock.IsReadLockHeld && !slimLock.IsWriteLockHeld)
                throw new Exception(nameof(GetShallowListHelper) + " requires a lock to be present before invoking the method.");

            if (allItems == null) return null;

            return isShort ?
                allItems.FindAll(x => IsInDictShort(x)) :
                allItems;
        }

        /// <summary>
        /// Returns a deep copy of the all items in the cached list that match the predicate. 
        /// Returns an empty list if no matches found. Optionally set isShort true in order to
        /// search through the short versions instead. Use this method when you want to guarantee 
        /// a correct answer from the cache and are unsure if the dictionary can be trusted.
        /// </summary>
        public List<T> GetWhereFromList(Predicate<T> match, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowListHelper(isShort).FindAll(match).Select(x => Copy(x)).ToList();
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets a deep copy of the first value in the list that match the predicate passed in.
        /// Optionally set isShort true to check short version of cache only. If value not found 
        /// then returns null.
        /// </summary>
        public T GetFirstOrDefaultFromList(Func<T, bool> match, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                var item = GetShallowListHelper(isShort).FirstOrDefault(match);
                if (item == null)
                {
                    return item;
                }

                return Copy(item);
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// If you need to override this method then you should probably be extending CacheDictAbs 
        /// instead of CacheDictNonPkAbs. Returns a shallow dictionary that will be comprised via 
        /// invoking GetDictKey() and GetDictValue(). This method is different from 
        /// base.GetDictFromList() due to grouping by KEY_TYPE first and then forcing the 
        /// VALUE_TYPE into being the First(). The base method would throw an exception if there 
        /// were multiple Ts that shared the same key which is why this class exists.
        /// </summary>
        protected override Dictionary<TKey, TValue> GetDictFromList(List<T> listAllItems)
        {
            slimLock.EnterWriteLock();
            try
            {
                allItems = listAllItems;
            }
            finally
            {
                slimLock.ExitWriteLock();
            }

            return listAllItems.GroupBy(x => GetDictKey(x)).ToDictionary(x => x.Key, x => GetDictValue(x.First()));
        }

        protected sealed override void GotNewCache(List<T> listAllItems)
        {
            slimLock.EnterWriteLock();
            try
            {
                allItems = listAllItems;
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// The Non PK version of the CacheDictAbs needs to return the list of all cached items
        /// instead of the dictionary because of duplicates. If the dictionary was utilized then 
        /// clients would not get all available items from the database (only the web service 
        /// would have them)
        /// </summary>
        protected sealed override DataTable CacheToTable() => ListToTable(GetDeepCopyList());
    }
}
