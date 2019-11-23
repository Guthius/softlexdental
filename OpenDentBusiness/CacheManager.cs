/**
 * Copyright (C) 2019 Dental Stars SRL
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Maintains a list of every active <see cref="IDataRecordCache"/> instance. Instances
    ///         of <see cref="DataRecordCacheBase{TRecord}"/> register themselves automatically.
    ///     </para>
    ///     <para>
    ///         Custom implementations of <see cref="IDataRecordCache"/> must manually register 
    ///         themselves using the <see cref="Register{T}(IDataRecordCacheBase{T})"/> method.
    ///     </para>
    ///     <para>
    ///         The <see cref="Invalidate{T}"/> and <see cref="Invalidate(Type)"/> methods can be 
    ///         used to notify the cache that a refresh is required. In most cases this will 
    ///         trigger an immediate synchronous cache refill. When refreshing large caches from 
    ///         the UI thread, make sure to wrap the <see cref="Invalidate{T}"/> calls in async 
    ///         tasks to prevent the UI from hanging while the refresh is in progress.
    ///     </para>
    ///     <para>
    ///         In case the cache data needs to be refreshed on every workstation use 
    ///         <see cref="InvalidateEverywhere{T}"/> or 
    ///         <see cref="InvalidateEverywhere{T}(long)"/> instead. These methods write a 
    ///         <see cref="Signal"/> to the database which in turn will trigger a refresh on every 
    ///         active workstation. These refreshes will always occur in the background.
    ///     </para>
    /// </summary>
    public static class CacheManager
    {
        private static readonly Dictionary<Type, List<IDataRecordCache>> dataRecordCaches = 
            new Dictionary<Type, List<IDataRecordCache>>();

        /// <summary>
        /// Invalidates the cache of record type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The data record type.</typeparam>
        public static void Invalidate<T>() where T : DataRecordBase
        {
            lock (dataRecordCaches)
            {
                if (dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCacheList))
                {
                    foreach (var dataRecordCache in dataRecordCacheList)
                    {
                        dataRecordCache.Refresh();

                        OnCacheRefreshed(typeof(T), dataRecordCache);
                    }
                }
            }
        }

        /// <summary>
        /// Invalidates the cache of the record type identified by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The record type.</param>
        public static void Invalidate(Type type)
        {
            if (type.IsClass && !type.IsAbstract && typeof(DataRecordBase).IsAssignableFrom(type))
            {
                lock (dataRecordCaches)
                {
                    if (dataRecordCaches.TryGetValue(type, out var dataRecordCacheList))
                    {
                        foreach (var dataRecordCache in dataRecordCacheList)
                        {
                            dataRecordCache.Refresh();

                            OnCacheRefreshed(type, dataRecordCache);
                        }
                    }
                }
            }

            // TODO: Add support for invalidating and refreshing individual records...
        }

        /// <summary>
        /// Represents a method for respoding to cache refreshes.
        /// </summary>
        /// <param name="type">The data record type.</param>
        /// <param name="dataRecordCache">The cache.</param>
        public delegate void CacheRefreshHandler(Type type, IDataRecordCache dataRecordCache);

        /// <summary>
        /// <para>Raised whenever a cache has been refreshed.</para>
        /// <para>
        /// This event is not gauranteed to be raised on the UI thread. 
        /// </para>
        /// </summary>
        public static event CacheRefreshHandler CacheRefreshed = delegate { };

        /// <summary>
        /// Raises the <see cref="CacheRefreshed"/> event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataRecordCache"></param>
        private static void OnCacheRefreshed(Type type, IDataRecordCache dataRecordCache) => CacheRefreshed.Invoke(type, dataRecordCache);

        /// <summary>
        /// Invalidates the cache of record type <typeparamref name="T"/> on every active workstation.
        /// </summary>
        /// <typeparam name="T">The data record type.</typeparam>
        public static void InvalidateEverywhere<T>() where T : DataRecordBase
        {
            Signal.Insert(new Signal
            {
                Name = SignalName.CacheInvalidate,
                Param1 = typeof(T).FullName
            });

            Invalidate<T>();
        }

        /// <summary>
        /// Invalidates the cache of record type <typeparamref name="T"/> on every active workstation.
        /// </summary>
        /// <typeparam name="T">The data record type.</typeparam>
        /// <param name="recordId">The ID of the record to invalidate.</param>
        public static void InvalidateEverywhere<T>(long recordId) where T : DataRecordBase
        {
            Signal.Insert(new Signal
            {
                Name = SignalName.CacheInvalidate,
                Param1 = typeof(T).FullName,
                ExternalId = recordId
            });

            Invalidate<T>();
        }

        /// <summary>
        /// Gets the list of caches for the given data record type.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <returns></returns>
        private static List<IDataRecordCache> GetTypeCacheList<T>() where T : DataRecordBase
        {
            lock (dataRecordCaches)
            {
                if (!dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCacheList))
                {
                    dataRecordCacheList = new List<IDataRecordCache>();
                    dataRecordCaches[typeof(T)] = dataRecordCacheList;
                }

                return dataRecordCacheList;
            }
        }

        /// <summary>
        /// Registers the specified cache with the cache manager.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="dataRecordCache">The cache.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="dataRecordCache"/> is null.</exception>
        internal static void Register<T>(IDataRecordCache dataRecordCache) where T : DataRecordBase
        {
            if (dataRecordCache == null)
                throw new ArgumentNullException(nameof(dataRecordCache));

            var dataRecordCacheList = GetTypeCacheList<T>();

            lock (dataRecordCacheList)
            {
                if (!dataRecordCacheList.Contains(dataRecordCache))
                {
                    dataRecordCacheList.Add(dataRecordCache);
                }
            }
        }

        /// <summary>
        /// Creates a cache for the specified record type.
        /// </summary>
        /// <typeparam name="T">The record type.</typeparam>
        /// <param name="fillCommandText"></param>
        /// <param name="dataRecordBuilder"></param>
        /// <returns></returns>
        public static DataRecordCache<T> Create<T>(string fillCommandText, DataRecordBuilder<T> dataRecordBuilder) where T : DataRecord =>
            new DataRecordCache<T>(fillCommandText, dataRecordBuilder);
    }
}
