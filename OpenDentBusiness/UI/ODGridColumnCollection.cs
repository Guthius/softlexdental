/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
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

namespace OpenDental.UI
{
    public class ODGridColumnCollection : CollectionBase, IEnumerable<ODGridColumn>
    {
        /// <summary>
        /// Returns the GridColumn with the given index.
        /// </summary>
        public ODGridColumn this[int index]
        {
            get => (ODGridColumn)List[index];
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a column to the <see cref="ODGridColumnCollection"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int Add(ODGridColumn column) => List.Add(column);

        /// <summary>
        /// Determines the index of the specified column in the <see cref="ODGridColumnCollection"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int IndexOf(ODGridColumn column) => List.IndexOf(column);

        /// <summary>
        /// Inserts a column to the <see cref="ODGridColumnCollection"/> as the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="column"></param>
        public void Insert(int index, ODGridColumn column) => List.Insert(index, column);

        /// <summary>
        /// Removes the first occurence of the specified column from the <see cref="ODGridColumnCollection"/>.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(ODGridColumn value) => List.Remove(value);
        
        public bool Contains(ODGridColumn value) => List.Contains(value);
        
        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.", nameof(value));
        }

        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.", nameof(value));
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("newValue must be of type ODGridColumn.", nameof(newValue));
        }

        protected override void OnValidate(object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.", nameof(value));
        }

        /// <summary>
        /// Gets the index of the column with the specified heading.
        /// </summary>
        public int GetIndex(string heading)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (((ODGridColumn)List[i]).Heading == heading)
                {
                    return i;
                }
            }
            return -1;
        }

        IEnumerator<ODGridColumn> IEnumerable<ODGridColumn>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }
    }
}
