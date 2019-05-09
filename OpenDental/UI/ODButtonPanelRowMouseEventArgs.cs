using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODButtonPanelRowMouseEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODButtonPanelRowMouseEventArgs"/> class.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="mea"></param>
        public ODButtonPanelRowMouseEventArgs(int row, MouseEventArgs mea) : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            Row = row;
        }

        /// <summary>
        /// Gets the row index.
        /// </summary>
        public int Row { get; }
    }
}