using System.Collections;
using System.Collections.Generic;

namespace OpenDental.UI
{
    public class ODButtonPanelItemCollection : IList<ODButtonPanelItem>
    {
        readonly List<ODButtonPanelItem> items = new List<ODButtonPanelItem>();
        readonly ODButtonPanel panel;

        /// <summary>
        /// Sort the items based on row and item order...
        /// </summary>
        internal void Sort()
        {
            items.Sort((a, b) =>
            {
                if (a.Row != b.Row)
                {
                    return a.Row.CompareTo(b.Row);
                }

                if (a.Order != b.Order)
                {
                    return a.Order.CompareTo(b.Order);
                }

                return a.Text.CompareTo(b.Text);
            });
        }

        /// <summary>
        /// Gets the number of items contained in the <see cref="ODButtonPanelItemCollection"/>.
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ODButtonPanelItemCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        public ODButtonPanelItem this[int index] { get => ((IList<ODButtonPanelItem>)items)[index]; set => ((IList<ODButtonPanelItem>)items)[index] = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODButtonPanelItemCollection"/> class.
        /// </summary>
        /// <param name="panel"></param>
        internal ODButtonPanelItemCollection(ODButtonPanel panel)
        {
            this.panel = panel;
        }

        /// <summary>
        /// Adds the specified item to the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(ODButtonPanelItem item) => items.Add(item);

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(ODButtonPanelItem item)
        {
            if (items.Remove(item))
            {
                panel?.PerformLayout();
            }
        }

        public int IndexOf(ODButtonPanelItem item) => items.IndexOf(item);

        /// <summary>
        /// Inserts a item into the <see cref="ODButtonPanelItemCollection"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, ODButtonPanelItem item)
        {
            items.Insert(index, item);

            panel?.PerformLayout();
        }

        /// <summary>
        /// Removes the item at the specified index of the <see cref="ODButtonPanelItemCollection"/>.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);

            panel?.PerformLayout();
        }

        /// <summary>
        /// Removes all items from the <see cref="ODButtonPanelItemCollection"/>.
        /// </summary>
        public void Clear()
        {
            items.Clear();

            panel?.PerformLayout();
        }

        /// <summary>
        /// Determines whether a item is in the <see cref="ODButtonPanelItemCollection"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ODButtonPanelItem item) => items.Contains(item);

        public void CopyTo(ODButtonPanelItem[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        bool ICollection<ODButtonPanelItem>.Remove(ODButtonPanelItem item) => items.Remove(item);

        public IEnumerator<ODButtonPanelItem> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}
