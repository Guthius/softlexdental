using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class TableTimeBar : ContrTable
    {
        public TableTimeBar()
        {
            MaxRows = 40;
            MaxCols = 1;
            ShowScroll = false;
            FieldsArePresent = false;
            HeadingIsPresent = false;
            InstantClassesPar();
            SetRowHeight(0, 39, 14);
            ColWidth[0] = 13;
            ColAlign[0] = HorizontalAlignment.Center;
            SetGridColor(Color.LightGray);
            LayoutTables();
        }

        protected override void OnLoad(EventArgs e)
        {
            LayoutTables();
        }
    }
}