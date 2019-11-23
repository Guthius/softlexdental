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
}
