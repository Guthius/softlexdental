﻿/**
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
using System.Collections;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public interface IDataRecordCache
    {
        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        void Refresh();
    }

    public interface IDataRecordCacheBase<TRecord> : IDataRecordCache, IEnumerable<TRecord> where TRecord : DataRecordBase
    {
        /// <summary>
        /// Gets the first record matching the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test for a condition.</param>
        /// <returns>The first record matching the condition.</returns>
        TRecord SelectOne(Func<TRecord, bool> predicate);

        /// <summary>
        /// Selects all records that match the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test for a condition.</param>
        /// <returns>All records matching the specified condition.</returns>
        IEnumerable<TRecord> SelectMany(Func<TRecord, bool> predicate);

        /// <summary>
        /// Gets all records in the cache.
        /// </summary>
        /// <returns>All records in the cache.</returns>
        IEnumerable<TRecord> All();
    }

    public class DataRecordCacheBase<TRecord> : IDataRecordCacheBase<TRecord> where TRecord : DataRecordBase
    {
        protected readonly string fillCommandText;
        protected readonly DataRecordBuilder<TRecord> dataRecordBuilder;
        protected readonly List<TRecord> dataRecords = new List<TRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordCache{T}"/> class.
        /// </summary>
        /// <param name="fillCommandText">The query to fill the cache.</param>
        /// <param name="dataRecordBuilder">The method that constructs the records in the cache.</param>
        public DataRecordCacheBase(string fillCommandText, DataRecordBuilder<TRecord> dataRecordBuilder)
        {
            if (string.IsNullOrWhiteSpace(fillCommandText))
                throw new ArgumentException("The fill command cannot be empty.", nameof(fillCommandText));

            this.fillCommandText = fillCommandText;
            this.dataRecordBuilder = dataRecordBuilder ?? throw new ArgumentNullException(nameof(dataRecordBuilder));

            CacheManager.Register(this);

            Refresh();
        }

        /// <summary>
        /// Gets the first record matching the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test for a condition.</param>
        /// <returns>The first record matching the condition.</returns>
        public TRecord SelectOne(Func<TRecord, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    if (predicate(dataRecord))
                    {
                        return dataRecord;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Selects all records that match the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test for a condition.</param>
        /// <returns>All records matching the specified condition.</returns>
        public IEnumerable<TRecord> SelectMany(Func<TRecord, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    if (predicate(dataRecord))
                    {
                        yield return dataRecord;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all records in the cache.
        /// </summary>
        /// <returns>All records in the cache.</returns>
        public IEnumerable<TRecord> All()
        {
            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    yield return dataRecord;
                }
            }
        }

        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        public void Refresh()
        {
            lock (dataRecords)
            {
                dataRecords.Clear();

                DataConnection.Execute(connection =>
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = fillCommandText;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var dataRecord = dataRecordBuilder(reader);
                                if (dataRecord == null)
                                {
                                    continue;
                                }

                                dataRecords.Add(dataRecord);
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Returns a enumerator that iterates through the <see cref="DataRecordCache{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TRecord> GetEnumerator() => dataRecords.GetEnumerator();

        /// <summary>
        /// Returns a enumerator that iterates through the <see cref="DataRecordCache{T}"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => dataRecords.GetEnumerator();
    }

    public interface IDataRecordCache<TRecord> : IDataRecordCacheBase<TRecord> where TRecord : DataRecord
    {
        /// <summary>
        /// Gets the record with the specified ID.
        /// </summary>
        /// <param name="dataRecordId">The ID of the record.</param>
        /// <returns>The record with the specified ID if it exists; otherwise, null.</returns>
        TRecord GetById(long dataRecordId);
    }

    public class DataRecordCache<TRecord> : DataRecordCacheBase<TRecord>, IDataRecordCache<TRecord> where TRecord : DataRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordCache{T}"/> class.
        /// </summary>
        /// <param name="fillCommandText">The query to fill the cache.</param>
        /// <param name="dataRecordBuilder">The method that constructs the records in the cache.</param>
        public DataRecordCache(string fillCommandText, DataRecordBuilder<TRecord> dataRecordBuilder) : 
            base(fillCommandText, dataRecordBuilder)
        {
        }

        /// <summary>
        /// Gets the record with the specified ID.
        /// </summary>
        /// <param name="dataRecordId">The ID of the record.</param>
        /// <returns>The record with the specified ID if it exists; otherwise, null.</returns>
        public TRecord GetById(long dataRecordId)
        {
            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    if (dataRecord.Id == dataRecordId)
                    {
                        return dataRecord;
                    }
                }
            }

            return null;
        }
    }

    public static class CacheManager
    {
        static readonly Dictionary<Type, List<IDataRecordCache>> dataRecordCaches = new Dictionary<Type, List<IDataRecordCache>>();

        /// <summary>
        /// Invalidates the cache of record type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Invalidate<T>() where T : DataRecord
        {
            lock (dataRecordCaches)
            {
                if (dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCacheList))
                {
                    foreach (var dataRecordCache in dataRecordCacheList)
                    {
                        dataRecordCache.Refresh();
                    }
                }
            }
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
        internal static void Register<T>(IDataRecordCacheBase<T> dataRecordCache) where T : DataRecordBase
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
