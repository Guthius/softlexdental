using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODButtonPanelItemMouseEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODButtonPanelItemMouseEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mea"></param>
        public ODButtonPanelItemMouseEventArgs(ODButtonPanelItem item, MouseEventArgs mea) : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the panel item.
        /// </summary>
        public ODButtonPanelItem Item { get; }
    }
}
