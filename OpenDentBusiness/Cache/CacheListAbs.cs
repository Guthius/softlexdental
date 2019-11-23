using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace OpenDentBusiness
{
    public abstract class CacheListAbs<T> : CacheAbs<T>
    {
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private List<T> cacheData;
        private bool isCacheAllowed = true;

        protected abstract DataTable ListToTable(List<T> listAllItems);

        /// <summary>
        /// Set to false in order to no longer allow caching items for this instance.
        /// Instantly clears out any currently cached items.
        /// </summary>
        public bool IsCacheAllowed
        {
            get => isCacheAllowed;
            set
            {
                isCacheAllowed = value;

                if (!isCacheAllowed)
                {
                    slimLock.EnterWriteLock();
                    try
                    {
                        cacheData.Clear();
                    }
                    finally
                    {
                        slimLock.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the object should be included in the short version and returns false if
        /// it shouldn't be included. If no short version of the given cache exists then no need to
        /// override this method. This should be something very simple, e.g. !isHidden, do not make 
        /// complicated logic or database calls.
        /// </summary>
        protected virtual bool IsInListShort(T tableBase) => true;

        /// <summary>
        /// Returns true if _listAllItems is null; otherwise, false.
        /// </summary>
        public bool ListIsNull() => IsCacheNull();

        /// <summary>
        /// Safe to call anytime and will ensure that _listAllItems gets filled if needed.
        /// </summary>
        private void FillListIfNull()
        {
            if (cacheData == null)
            {
                FillCacheIfNeeded();
            }
        }

        /// <summary>
        /// Several methods that manipulate _listAllItems need the "short" version sometimes. This 
        /// helper method simply saves code. Calling method must already be in a Read or Write 
        /// lock, if not then this method will throw an exception.
        /// </summary>
        private List<T> GetShallowHelper(bool isShort = false)
        {
            if (!slimLock.IsReadLockHeld && !slimLock.IsWriteLockHeld)
                throw new Exception(nameof(GetShallowHelper) + " requires a lock to be present before invoking the method.");

            if (cacheData == null) return null;

            return isShort ?
                cacheData.FindAll(x => IsInListShort(x)) :
                cacheData;
        }

        public List<T> GetDeepCopy(bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper(isShort).Select(x => Copy(x)).ToList();
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public int GetCount(bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper(isShort).Count;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public bool GetExists(Predicate<T> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper(isShort).Exists(match);
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public int GetFindIndex(Predicate<T> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper(isShort).FindIndex(match);
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public T GetFirst(bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return Copy(GetShallowHelper(isShort).First());
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public T GetFirst(Func<T, bool> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return Copy(GetShallowHelper(isShort).First(match));
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
        public T GetFirstOrDefault(Func<T, bool> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                var item = GetShallowHelper(isShort).FirstOrDefault(match);
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

        public T GetLast(bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return Copy(GetShallowHelper(isShort).Last());
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        public T GetLastOrDefault(Func<T, bool> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                var item = GetShallowHelper(isShort).LastOrDefault(match);
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
        /// Returns a deep copy of the all items in the cache that match the predicate.
        /// Returns an empty list if no matches found.
        /// Optionally set isShort true in order to search through the short versions instead.
        /// </summary>
        public List<T> GetWhere(Predicate<T> match, bool isShort = false)
        {
            FillListIfNull();

            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper(isShort).FindAll(match).Select(x => Copy(x)).ToList();
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Fill the list with the data from the base List.
        /// This has been sealed at this level on purpose so that no concreted implementers can
        /// override its behavior.
        /// </summary>
        protected sealed override void OnNewCacheReceived(List<T> listAllItems)
        {
            if (!IsCacheAllowed)
            {
                // Technically we have already violated the fact that we just went off and asked 
                // the database with a SELECT * FROM...
                // However, this is the quickest and safest way for our customers to notify us of 
                // a "pattern" violation where we accidentally asked a cache to fill itself when 
                // that cache is in a state where it is absolutely unacceptable to be cached. 
                // E.g. caching userods before being logged in.
                throw new ApplicationException("Caching has been temporarily turned off.  Please call support.");
            }

            slimLock.EnterWriteLock();
            try
            {
                cacheData = listAllItems;
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
        }

        protected sealed override bool IsCacheNull()
        {
            slimLock.EnterReadLock();
            try
            {
                return GetShallowHelper() == null;
            }
            finally
            {
                slimLock.ExitReadLock();
            }
        }

        protected sealed override DataTable CacheToTable() => ListToTable(GetDeepCopy());
    }
}