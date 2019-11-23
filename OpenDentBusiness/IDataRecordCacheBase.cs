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
}
