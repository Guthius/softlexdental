using System;

namespace OpenDental.UI
{
    public class ODToolBarButtonPageNavEventArgs : EventArgs
    {
        public int NavValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODToolBarButtonClickEventArgs"/> class.
        /// </summary>
        /// <param name="navValue"></param>
        public ODToolBarButtonPageNavEventArgs(int navValue)
        {
            NavValue = navValue;
        }
    }
}