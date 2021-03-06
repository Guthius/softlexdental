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
using System.Drawing;

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
