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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenDental.UI
{
    public class ODGridCellList : List<ODGridCell>
    {
        public void Add(string value) => Add(new ODGridCell(value));

        /// <summary>
        /// Creates a new ODGridCell with initial value of 'value' and starting index of 'idx'.
        /// Meant to be used with combo box columns.
        /// </summary>
        public void Add(string value, int idx) => Add(new ODGridCell(value, idx));
    }

    public class ODGridRow
    {
        public long RowNum; // Gets incremented when added to an ODGridRowCollection.

        public ODGridRow()
        {
            Cells = new ODGridCellList();
            BackColor = Color.White;
            Bold = false;
            ColorText = Color.Black;
            ColorLborder = Color.Empty;
            Tag = null;
            Note = "";
        }

        public ODGridRow(params string[] cellText)
        {
            Cells = new ODGridCellList();
            cellText.ToList().ForEach(x => Cells.Add(x));
            BackColor = Color.White;
            Bold = false;
            ColorText = Color.Black;
            ColorLborder = Color.Empty;
            Tag = null;
            Note = "";
        }

        public ODGridRow(params ODGridCell[] cellList)
        {
            Cells = new ODGridCellList();
            cellList.ToList().ForEach(x => Cells.Add(x));
            BackColor = Color.White;
            Bold = false;
            ColorText = Color.Black;
            ColorLborder = Color.Empty;
            Tag = null;
            Note = "";
        }

        public ODGridCellList Cells { get; }

        /// <summary>
        /// Background color.
        /// </summary>
        public Color BackColor { get; set; }

        public bool Bold { get; set; }

        /// <summary>
        /// This sets the text color for the whole row.
        /// Each gridCell also has a colorText property that will override this if set.
        /// </summary>
        public Color ColorText { get; set; }

        public Color ColorLborder { get; set; }

        /// <summary>
        /// Used to store any kind of object that is associated with the row.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// This is a very special field. Since most of the tables in OD require the ability to 
        /// attach long notes to each row, this field makes it possible. Any note set here will be
        /// drawn as a sort of subrow. The note can span multiple columns, as defined in 
        /// grid.NoteSpanStart and grid.NoteSpanStop.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// The vertical height of only the note portion of the row in pixels. Usually 0, unless 
        /// you want notes showing.
        /// </summary>
        public int NoteHeight { get; internal set; }

        /// <summary>
        /// The vertical location at which to start drawing this row in pixels.
        /// </summary>
        public int YOffset { get; internal set; }

        /// <summary>
        /// The vertical height of the row in pixels, not counting the note portion of the row.
        /// </summary>
        public int RowHeight { get; internal set; }

        /// <summary>
        /// The row height plus the note height.
        /// </summary>
        public int Height => RowHeight + NoteHeight;

        /// <summary>
        /// If this is a dropdown row, set this to the index of the row that drops this row down. If not, -1.
        /// </summary>
        public ODGridRow DropDownParent { get; set; } = null;

        /// <summary>
        /// Does this row drop down other rows? -1: No; 0: Yes, currently not dropped; 1: Yes, 
        /// currently dropped. If unspecified, this will be automatically set if other rows list it
        /// as a drop down parent.
        /// </summary>
        public ODGridDropDownState DropDownState { get; set; } = ODGridDropDownState.None;

        public ODGridRow Copy() => (ODGridRow)MemberwiseClone();
    }

    /// <summary>
    /// Identifies the state of a dropdown row.
    /// </summary>
    public enum ODGridDropDownState
    {
        /// <summary>
        /// Not a drop down parent.
        /// </summary> 
        None,

        /// <summary>
        /// Not dropped down.
        /// </summary>  
        Up,

        /// <summary>
        /// Dropped down.
        /// </summary>
        Down,
    }
}
