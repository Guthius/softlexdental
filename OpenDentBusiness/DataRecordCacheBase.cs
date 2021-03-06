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

            CacheManager.Register<TRecord>(this);

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
        ///     <para>
        ///         Establishes a link between the record type identified by 
        ///         <typeparamref name="TOtherRecord"/> and this cache. When the cache for 
        ///         <typeparamref name="TOtherRecord"/> is invalidated through 
        ///         <see cref="CacheManager.Invalidate(Type)"/> this cache will also be 
        ///         invalidated.
        ///     </para>
        /// </summary>
        /// <typeparam name="TOtherRecord">The type of to link this cache to.</typeparam>
        /// <returns></returns>
        public DataRecordCacheBase<TRecord> LinkedTo<TOtherRecord>() where TOtherRecord : DataRecordBase
        {
            if (typeof(TRecord) != typeof(TOtherRecord))
            {
                CacheManager.Register<TOtherRecord>(this);
            }
            return this;
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
}
