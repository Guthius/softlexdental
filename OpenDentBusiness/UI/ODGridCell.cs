using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.UI
{
    public class ODGridCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridCell"/> class.
        /// </summary>
        public ODGridCell()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridCell"/> class.
        /// </summary>
        public ODGridCell(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridCell"/> class. 
        /// Meant to be used with combo box columns.
        /// </summary>
        public ODGridCell(string text, int selectedIndex)
        {
            Text = text;
            SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Gets or sets the selected index (in case the cell contains a combobox).
        /// </summary>
        public int SelectedIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets the cell text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color of the cell. The default color is <see cref="Color.Empty"/>. 
        /// If any other color is set, it will override the row color.
        /// </summary>
        public Color ColorText { get; set; } = Color.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to bold the cell value. If null the row state is used.
        /// </summary>
        public bool? Bold { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the underline the cell value. If null the row state is used.
        /// </summary>
        public bool? Underline { get; set; } = null;

        /// <summary>
        /// Gets or sets the color of the cell.
        /// </summary>
        public Color CellColor { get; set; }
    }
}
