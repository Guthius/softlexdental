using System;
using System.Drawing;

namespace OpenDental
{
    public class TableAutoItem : ContrTable
    {
        public TableAutoItem()
        {
            MaxRows = 20;
            MaxCols = 3;
            ShowScroll = true;
            FieldsArePresent = true;
            HeadingIsPresent = false;
            InstantClassesPar();
            SetRowHeight(0, 19, 14);
            Fields[0] = "Code";
            Fields[1] = "Description";
            Fields[2] = "Conditions";
            ColWidth[0] = 100;
            ColWidth[1] = 200;
            ColWidth[2] = 400;
            DefaultGridColor = Color.LightGray;
            LayoutTables();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LayoutTables();
        }
    }
}
