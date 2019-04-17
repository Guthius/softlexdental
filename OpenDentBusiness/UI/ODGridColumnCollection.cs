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
        

        ///<summary></summary>
        public bool Contains(ODGridColumn value)
        {
            // If value is not of type ODGridColumn, this will return false.
            return (List.Contains(value));
        }

        ///<summary></summary>
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.", "value");
        }

        ///<summary></summary>
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.", "value");
        }

        ///<summary></summary>
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("newValue must be of type ODGridColumn.", "newValue");
        }

        ///<summary></summary>
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != typeof(ODGridColumn))
                throw new ArgumentException("value must be of type ODGridColumn.");
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