using System.Collections;

namespace OpenDental.UI
{
    public class ODToolBarButtonCollection : CollectionBase
    {
        ODToolBar toolbar;

        /// <summary>
        /// Returns the Button with the given index.
        /// </summary>
        public ODToolBarButton this[int index]
        {
            get => (ODToolBarButton)List[index];
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Returns the Button with the given string tag.
        /// </summary>
        public ODToolBarButton this[string buttonTag]
        {
            get
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (((ODToolBarButton)List[i]).Tag.ToString() == buttonTag)
                    {
                        return ((ODToolBarButton)List[i]);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODToolBarButtonCollection"/> class.
        /// </summary>
        /// <param name="toolbar">The toolbar the collection is bound to.</param>
        internal ODToolBarButtonCollection(ODToolBar toolbar)
        {
            this.toolbar = toolbar;
        }

        /// <summary>
        /// Adds a button to the <see cref="ODToolBarButtonCollection"/>.
        /// </summary>
        /// <param name="button"></param>
        public void Add(ODToolBarButton button)
        {
            if (!List.Contains(button))
            {
                List.Add(button);

                toolbar.PerformLayout();
            }
        }

        /// <summary>
        /// Removes the <see cref="ODToolBarButton"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            List.RemoveAt(index);

            toolbar.PerformLayout();
        }

        /// <summary>
        /// Determines the index of a specific button in the <see cref="ODToolBarButtonCollection"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(ODToolBarButton value) => List.IndexOf(value);

        /// <summary>
        /// Returns the index of the button for the given tag. Returns -1 if a no button is found that matches the tag.
        /// </summary>
        public int IndexOf(object buttonTag)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (((ODToolBarButton)List[i]).Tag == buttonTag)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}