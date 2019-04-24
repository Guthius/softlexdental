using System;

namespace OpenDental.UI
{
    public class ODToolBarButtonClickEventArgs : EventArgs
    {
        public ODToolBarButton Button { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODToolBarButtonClickEventArgs"/> class.
        /// </summary>
        /// <param name="button">The button.</param>
        public ODToolBarButtonClickEventArgs(ODToolBarButton button)
        {
            Button = button;
        }
    }
}