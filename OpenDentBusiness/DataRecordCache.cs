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

    public interface IDataRecordCache<TRecord> : IDataRecordCacheBase<TRecord> where TRecord : DataRecord
    {
        /// <summary>
        /// Gets the record with the specified ID.
        /// </summary>
        /// <param name="dataRecordId">The ID of the record.</param>
        /// <returns>The record with the specified ID if it exists; otherwise, null.</returns>
        TRecord GetById(long dataRecordId);
    }

    public class DataRecordCache<TRecord> : IDataRecordCache<TRecord> where TRecord : DataRecord
    {
        readonly string fillCommandText;
        readonly DataRecordBuilder<TRecord> dataRecordBuilder;
        readonly List<TRecord> dataRecords = new List<TRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordCache{T}"/> class.
        /// </summary>
        /// <param name="fillCommandText">The query to fill the cache.</param>
        /// <param name="dataRecordBuilder">The method that constructs the records in the cache.</param>
        public DataRecordCache(string fillCommandText, DataRecordBuilder<TRecord> dataRecordBuilder)
        {
            if (string.IsNullOrWhiteSpace(fillCommandText))
                throw new ArgumentException("The fill command cannot be empty.", nameof(fillCommandText));

            this.fillCommandText = fillCommandText;
            this.dataRecordBuilder = dataRecordBuilder ?? throw new ArgumentNullException(nameof(dataRecordBuilder));

            Refresh();
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

    public static class CacheManager
    {
        static readonly Dictionary<Type, IDataRecordCache> dataRecordCaches = new Dictionary<Type, IDataRecordCache>();

        /// <summary>
        /// Invalidates the cache of record type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Invalidate<T>() where T : DataRecord
        {
            lock (dataRecordCaches)
            {
                if (dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCache))
                {
                    dataRecordCache.Refresh();
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
        public static DataRecordCache<T> Register<T>(string fillCommandText, DataRecordBuilder<T> dataRecordBuilder) where T : DataRecord
        {
            IDataRecordCache dataRecordCache;

            lock (dataRecordCaches)
            {
                if (!dataRecordCaches.TryGetValue(typeof(T), out dataRecordCache))
                {
                    dataRecordCache = new DataRecordCache<T>(fillCommandText, dataRecordBuilder);

                    dataRecordCaches[typeof(T)] = dataRecordCache;
                }
            }

            return dataRecordCache as DataRecordCache<T>;
        }
    }
}
