using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenDental.UI
{
    public class ODGridColumn : ICloneable
    {
        /// <summary>
        /// When set, all cells in this column will display a combo box with these strings as options which the user can pick from.
        /// </summary>
        public List<string> ListDisplayStrings;

        /// <summary>
        /// Set this to an event method and it will be used when the column header is clicked.
        /// </summary>
        public EventHandler CustomClickEvent; // TODO: Why do we have this here? There should be a ColumnClick event handler on ODGrid for this purpose...

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridColumn"/> class.
        /// </summary>
        public ODGridColumn()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridColumn"/> class.
        /// </summary>
        /// <param name="heading">The heading text of the column.</param>
        /// <param name="width">The width of the column.</param>
        /// <param name="textAlignment"></param>
        /// <param name="sortingStrategy">The strategy to use when sorting cells in this column.</param>
        /// <param name="isEditable">A value indicating whether cells in the column can be edited.</param>
        public ODGridColumn(string heading, int width, HorizontalAlignment textAlignment = HorizontalAlignment.Left, ODGridSortingStrategy sortingStrategy = ODGridSortingStrategy.StringCompare, bool isEditable = false)
        {
            Heading = heading;
            Width = width;
           
            TextAlign = textAlignment;
            SortingStrategy = sortingStrategy;
            IsEditable = isEditable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridColumn"/> class.
        /// </summary>
        /// <param name="heading">The heading text of the column.</param>
        /// <param name="width">The width of the column.</param>
        /// <param name="listDisplayStrings"></param>
        public ODGridColumn(string heading, int width, List<string> listDisplayStrings, int dropDownWidth = 160)
        {
            Heading = heading;
            Width = width;
            ListDisplayStrings = listDisplayStrings;
        }

        /// <summary>
        /// Gets or sets the column heading.
        /// </summary>
        public string Heading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        public int Width { get; set; } = 80;

        /// <summary>
        /// Gets or sets the alignment of the column heading.
        /// </summary>
        public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left; // TODO: What does this apply to? cells? column heading? both??

        /// <summary>
        /// Gets or sets a value indicating whether the column is editable.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the strategy to use when sorting rows using this column.
        /// </summary>
        public ODGridSortingStrategy SortingStrategy { get; set; } = ODGridSortingStrategy.StringCompare;

        /// <summary>
        /// Creates a clone of the column.
        /// </summary>
        /// <returns>A clone of the column.</returns>
        public object Clone()
        {
            var column = (ODGridColumn)MemberwiseClone();
            if (ListDisplayStrings != null)
            {
                column.ListDisplayStrings = new List<string>(ListDisplayStrings);
            }
            return column;
        }

        public ImageList ImageList { get; set; }
        public object Tag { get; set; }
        public int DropDownWidth { get; }
    }
}