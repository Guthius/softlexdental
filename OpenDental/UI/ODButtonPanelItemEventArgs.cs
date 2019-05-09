using System;

namespace OpenDental.UI
{
    public class ODButtonPanelItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODButtonPanelItemEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ODButtonPanelItemEventArgs(ODButtonPanelItem item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the panel item.
        /// </summary>
        public ODButtonPanelItem Item { get; }
    }
}