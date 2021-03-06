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
    /// <summary>
    /// A strongly typed collection of ODGridRows
    /// </summary>
    public class ODGridRowCollection : CollectionBase, IEnumerable<ODGridRow>
    {
        /// <summary>
        /// This ID is used to set a unique ID for every row added to the collection.
        /// Not user editable. Set to 1 when Clear() is called.
        /// </summary>
        long incrementId = 1;

        /// <summary>
        /// Returns the GridRow with the given index.
        /// </summary>
        public ODGridRow this[int index]
        {
            get => (ODGridRow)List[index];
            set
            {
                List[index] = value;
            }
        }

        public int Add(ODGridRow value)
        {
            if (value.RowNum == 0)
            {
                value.RowNum = incrementId++;
            }
            return List.Add(value);
        }

        public int IndexOf(ODGridRow value) => List.IndexOf(value);
        

        public void Insert(int index, ODGridRow value)
        {
            if (value.RowNum == 0)
            {
                value.RowNum = incrementId++;
            }
            List.Insert(index, value);
        }

        public void Remove(ODGridRow value) => List.Remove(value);
        
        protected override void OnClear() => incrementId = 1;

        public bool Contains(ODGridRow value) => List.Contains(value);

        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != typeof(ODGridRow))
                throw new ArgumentException("value must be of type ODGridRow.", nameof(value));
        }

        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != typeof(ODGridRow))
                throw new ArgumentException("value must be of type ODGridRow.", nameof(value));
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != typeof(ODGridRow))
                throw new ArgumentException("newValue must be of type ODGridRow.", nameof(newValue));
        }

        protected override void OnValidate(object value)
        {
            if (value.GetType() != typeof(ODGridRow))
                throw new ArgumentException("value must be of type ODGridRow.", nameof(value));
        }

        IEnumerator<ODGridRow> IEnumerable<ODGridRow>.GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }
    }
}
