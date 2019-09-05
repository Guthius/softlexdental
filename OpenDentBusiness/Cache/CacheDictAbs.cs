using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace OpenDentBusiness
{
    /// <summary>
    /// Provides a cache pattern for S-classes that use a Dictionary to store data instead of a list.
    /// </summary>
    public abstract class CacheDictAbs<T, K, V> : CacheAbs<T>
    {
        protected abstract DataTable DictToTable(Dictionary<K, V> dictionary);
        
        /// <summary>
        /// Return the expected key for the dictionary (typically the primary key).
        /// </summary>
        protected abstract K GetDictKey(T tableBase);
        
        /// <summary>
        /// Return the expected key for the dictionary (typically the tablebase).
        /// </summary>
        protected abstract V GetDictValue(T tableBase);
        
        /// <summary>
        /// Return a deep copy of the expected value for the dictionary (typically the tablebase).
        /// </summary>
        protected abstract V CopyDictValue(V value);

        /// <summary>
        /// A lock object that allows multiple threads to obtain a read lock but allows only one thread to obtain a write lock.
        /// </summary>
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
       
        /// <summary>
        /// The one and only stored copy of the cache items.
        /// </summary>
        private Dictionary<K, V> allItems;
        
        /// <summary>
        /// Keeps track of all dictionary keys that represent key value pairs that belong to the short version of this cache.
        /// This is so that we don't have to store two entire copies of the cache. Will be kept in sync with _dictAllItems.
        /// </summary>
        private List<K> shortKeys;

        /// <summary>
        /// Return true if the object should be included in the short version of this cache and return false if it shouldn't be included.
        /// If no short version of the given cache exists then no need to override this method.
        /// This should be something very simple, e.g. !isHidden, do not make complicated logic or database calls.
        /// </summary>
        protected virtual bool IsInDictShort(T tableBase) => true;

        /// <summary>
        /// Returns the short version of the VALUE_TYPE.
        /// Only useful for caches that have complicated value types that change between short and long versions.
        /// If no short version of the given cache exists then no need to override this method.
        /// This should be something very simple, e.g. !isHidden, do not modify value, make database calls, or perform resource heavy computations.
        /// This method is invoked in the context of a cache read or write lock and should be used sparingly.
        /// </summary>
        protected virtual V GetShortValue(V value) => value;

        /// <summary>
        /// CacheDictNonPkAbs needs to know when a new cache is received.
        /// </summary>
        protected virtual void GotNewCache(List<T> listAllItems)
        {
        }

        /// <summary>
        /// Safe to call anytime and will ensure that _dictAllItems gets filled if needed.
        /// </summary>
        protected void FillDictIfNull()
        {
            if (IsCacheNull())
            {
                FillCacheIfNeeded();
            }
        }

        /// <summary>
        /// This method can be overridden when the extending class needs a more complicated dictionary.
        /// Returns a dictionary that is comprised of all items that are passed in.
        /// If not overridden, returns a shallow dictionary that will be comprised via invoking GetDictKey() and GetDictValue().
        /// E.g. The Defs cache cannot create a correct dictionary by having GetDictValue(T) implemented because it needs more than one T.
        /// </summary>
        protected virtual Dictionary<K, V> GetDictFromList(List<T> listAllItems) =>
            listAllItems.ToDictionary(x => GetDictKey(x), x => GetDictValue(x));

        /// <summary>
        /// This method can be overridden when the extending class needs a more complicated dictionary.
        /// Returns a shallow list that represents all dictionary keys that comprise the short version of the cache.
        /// If not overridden, returns a shallow list that will be comprised via invoking IsInDictShort() and GetDictKey().
        /// </summary>
        protected virtual List<K> GetDictShortKeys(List<T> allItems)
        {
            var shortKeys =
                allItems
                    .FindAll(x => IsInDictShort(x))
                    .Select(x => GetDictKey(x))
                    .ToList();

            return shortKeys;
        }

        /// <summary>
        /// Several methods that manipulate _dictAllItems need the "short" version sometimes.
        /// This helper method simply saves code. Calling method must already be in a Read or
        /// Write lock. If not then this method will throw an exception.
        /// </summary>
        private Dictionary<K, V> GetShallowHelper(bool isShort = false)
        {
            if (!slimLock.IsReadLockHeld && !slimLock.IsWriteLockHeld)
                throw new Exception(nameof(GetShallowHelper) + " requires a lock to be present before invoking the method.");
            
            var dictionary = new Dictionary<K, V>();

            if (isShort)
            {
                foreach (var key in shortKeys)
                {
                    dictionary[key] = GetShortValue(allItems[key]);
                }
            }
            else
            {
                dictionary = allItems;
            }

            return dictionary;
        }

        /// <summary>
        /// Returns true if given key exists in cache.  Optionally set isShort true to check short version of cache only.
        /// </summary>
        public bool GetContainsKey(K key, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                return isShort ? shortKeys.Contains(key) : allItems.ContainsKey(key);
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Optionally set isShort true to check short version of cache only.
        /// </summary>
        public int GetCount(bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                return isShort ? shortKeys.Count : allItems.Keys.Count;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a deep copy of the entire dictionary for looping purposes.
        /// Optionally set isShort true to check short version of cache only.
        /// </summary>
        public Dictionary<K, V> GetDeepCopy(bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                var dict = GetShallowHelper(isShort);
                var dictDeepCopy = new Dictionary<K, V>();

                foreach (var kvp in dict)
                {
                    dictDeepCopy[kvp.Key] = CopyDictValue(kvp.Value);
                }

                return dictDeepCopy;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a deep copy of the dictionary's value for the corresponding key.
        /// Throws KeyNotFoundException if no match is found.
        /// </summary>
        public V GetOne(K key, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                if (isShort)
                {
                    if (!shortKeys.Contains(key))
                    {
                        throw new KeyNotFoundException();
                    }

                    return CopyDictValue(GetShortValue(allItems[key]));
                }
                return CopyDictValue(allItems[key]);
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets a deep copy of the first value in the dictionary that match the predicate passed in.
        /// Optionally set isShort true to check short version of cache only. If value not found then returns null.
        /// </summary>
        public V GetFirstOrDefault(Func<V, bool> match, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                var dictionary = GetShallowHelper(isShort);
                V value = dictionary.Values.FirstOrDefault(match);

                if (value != null)
                {
                    return CopyDictValue(value);
                }
            }
            finally
            {
                slimLock.ExitReadLock();
            }

            return default;
        }

        /// <summary>
        /// Gets a deep copy of all values in the dictionary that match the predicate passed in.
        /// Optionally set isShort true to check short version of cache only. If value not found 
        /// then returns empty list.
        /// </summary>
        public List<V> GetWhere(Func<V, bool> predicate, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                var dictionary = GetShallowHelper(isShort);

                return dictionary.Values.Where(predicate).Select(x => CopyDictValue(x)).ToList();
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets a deep copy of all values in the dictionary that have a KEY that matches the predicate passed in.
        /// Optionally set isShort true to check short version of cache only.  If value not found then returns empty list.
        /// </summary>
        public List<V> GetWhereForKey(Func<K, bool> match, bool isShort = false)
        {
            FillDictIfNull();

            slimLock.EnterReadLock();
            try
            {
                var dictionary = GetShallowHelper(isShort);
                var listKeys = dictionary.Keys.Where(match);
                var listValues = new List<V>();

                foreach (K key in listKeys)
                {
                    listValues.Add(CopyDictValue(dictionary[key]));
                }

                return listValues;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns true if the key is successfully found and removed; otherwise, false.
        /// Removes from dictionary and short list if applicable.
        /// </summary>
        public bool RemoveKey(K key)
        {
            FillDictIfNull();

            //There is no such thing as removing a dictionary entry from one dictionary and not the other. Always remove from both.
            slimLock.EnterWriteLock();
            try
            {
                shortKeys.Remove(key);
                return allItems.Remove(key);
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Tries to add the key value pair to the dictionary. 
        /// Returns true if successfully added; otherwise, false.
        /// Optionally set isShort true to add to short version of cache only.
        /// </summary>
        public bool AddValueForKey(K key, V value, bool isShort = false)
        {
            FillDictIfNull();
            bool wasKeyAdded = false;
            slimLock.EnterWriteLock();
            try
            {
                if (isShort)
                {
                    if (!shortKeys.Contains(key))
                    {
                        wasKeyAdded = true;//Technically we "added" this key to the short list even if it is already within _dictAllItems.
                        shortKeys.Add(key);
                    }
                }
                if (!allItems.ContainsKey(key))
                {
                    wasKeyAdded = true;
                    allItems.Add(key, CopyDictValue(value));
                }
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
            return wasKeyAdded;
        }

        /// <summary>
        /// Forces the key to be set to the value passed in.
        /// Optionally set isShort true to add to short version of cache as well.
        /// </summary>
        public void SetValueForKey(K key, V value, bool isShort = false)
        {
            FillDictIfNull();
            slimLock.EnterWriteLock();
            try
            {
                if (isShort && !shortKeys.Contains(key))
                {
                    shortKeys.Add(key);
                }
                allItems[key] = CopyDictValue(value);
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Fill the dictionary with the data from the base List. 
        /// This has been sealed at this level on purpose so that no concreted implementers can 
        /// override its behavior.
        /// </summary>
        protected sealed override void OnNewCacheReceived(List<T> listAllItems)
        {
            // As of right now, we require T to be a long or a string. A long conversation needs to
            // take place if another type needs to be introduced. The bottom line is that T cannot 
            // be a type where a shallow copy is not equivalent to a deep copy. This allows us to 
            // use the accessor of the dictionary. E.g. _dictAllItems[key]
            if (typeof(K) != typeof(string) && typeof(K) != typeof(long) && !typeof(K).IsEnum)
                throw new Exception("CacheDictAbs requires K to be of type string or long.");

            GotNewCache(listAllItems);

            var dictionary = GetDictFromList(listAllItems);
            var shortKeys = GetDictShortKeys(listAllItems);

            slimLock.EnterWriteLock();
            try
            {
                this.shortKeys = shortKeys;

                allItems = dictionary;
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns if the dictionary is null; otherwise, false.
        /// </summary>
        protected sealed override bool IsCacheNull()
        {
            slimLock.EnterReadLock();
            try
            {
                return allItems == null;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// No longer sealed so that CacheDictNonPkAbs can override.
        /// Once we get rid of the Non PK version this can be resealed.
        /// </summary>
        protected override DataTable CacheToTable()
        {
            FillDictIfNull();

            var dictionary = new Dictionary<K, V>();

            // Deep copy both the key and the value so either could be modified without modifying the cache.
            slimLock.EnterReadLock();
            try
            {
                foreach (K key in allItems.Keys)
                {
                    dictionary.Add(key, CopyDictValue(allItems[key]));
                }
            }
            finally
            {
                slimLock.ExitReadLock();
            }

            return DictToTable(dictionary);
        }
    }
}
