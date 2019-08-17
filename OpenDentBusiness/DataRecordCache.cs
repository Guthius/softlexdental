using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public delegate bool DataRecordFilter<T>(T dataRecord);

    public interface IDataRecordCache
    {
        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        void Refresh();
    }

    public interface IDataRecordCache<T> : IDataRecordCache, IEnumerable<T> where T : DataRecord
    {
        T GetById(long dataRecordId);

        /// <summary>
        /// Gets the first record matching the specified filter.
        /// </summary>
        /// <param name="dataRecordFilter">The filte.r</param>
        /// <returns>The first record matching the filter.</returns>
        T SelectOne(DataRecordFilter<T> dataRecordFilter);

        /// <summary>
        /// Selects all records that match the specified filter.
        /// </summary>
        /// <param name="dataRecordFilter">The filter.</param>
        /// <returns>All records matching the specified filter.</returns>
        IEnumerable<T> SelectMany(DataRecordFilter<T> dataRecordFilter);

        /// <summary>
        /// Gets all records in the cache.
        /// </summary>
        /// <returns>All records in the cache.</returns>
        IEnumerable<T> All();
    }

    public class DataRecordCache<T> : IDataRecordCache<T> where T : DataRecord
    {
        readonly string fillCommandText;
        readonly DataRecordBuilder<T> dataRecordBuilder;
        readonly List<T> dataRecords = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordCache{T}"/> class.
        /// </summary>
        /// <param name="fillCommandText">The query to fill the cache.</param>
        /// <param name="dataRecordBuilder">The method that constructs the records in the cache.</param>
        public DataRecordCache(string fillCommandText, DataRecordBuilder<T> dataRecordBuilder)
        {
            if (string.IsNullOrEmpty(fillCommandText))
                throw new ArgumentException("The fill command cannot be empty.", "fillCommandText");

            this.fillCommandText = fillCommandText;
            this.dataRecordBuilder = dataRecordBuilder ?? throw new ArgumentNullException("dataRecordBuilder");

            Refresh();
        }

        /// <summary>
        /// Gets the record with the specified ID.
        /// </summary>
        /// <param name="dataRecordId">The ID of the record.</param>
        /// <returns>The record with the specified ID if the record exists; otherwise, null.</returns>
        public T GetById(long dataRecordId)
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
        /// Gets the first record matching the specified filter.
        /// </summary>
        /// <param name="dataRecordFilter">The filte.r</param>
        /// <returns>The first record matching the filter.</returns>
        public T SelectOne(DataRecordFilter<T> dataRecordFilter)
        {
            if (dataRecordFilter == null) throw new ArgumentNullException("dataRecordFilter");

            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    if (dataRecordFilter(dataRecord))
                    {
                        return dataRecord;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Selects all records that match the specified filter.
        /// </summary>
        /// <param name="dataRecordFilter">The filter.</param>
        /// <returns>All records matching the specified filter.</returns>
        public IEnumerable<T> SelectMany(DataRecordFilter<T> dataRecordFilter)
        {
            if (dataRecordFilter == null) throw new ArgumentNullException("dataRecordFilter");

            lock (dataRecords)
            {
                foreach (var dataRecord in dataRecords)
                {
                    if (dataRecordFilter(dataRecord))
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
        public IEnumerable<T> All()
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
        public IEnumerator<T> GetEnumerator() => dataRecords.GetEnumerator();

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
            if (dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCache))
            {
                dataRecordCache.Refresh();
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
            if (!dataRecordCaches.TryGetValue(typeof(T), out var dataRecordCache))
            {
                dataRecordCache = new DataRecordCache<T>(fillCommandText, dataRecordBuilder);
            }

            return dataRecordCache as DataRecordCache<T>;
        }
    }
}