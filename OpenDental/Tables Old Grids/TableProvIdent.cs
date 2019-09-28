using System;
using System.Drawing;

namespace OpenDental
{
    public class TableProvIdent : ContrTable
    {
        public TableProvIdent()
        {
            MaxRows = 20;
            MaxCols = 3;
            ShowScroll = true;
            FieldsArePresent = true;
            HeadingIsPresent = false;
            InstantClassesPar();
            SetRowHeight(0, 19, 14);
            Fields[0] = "Payor ID";
            Fields[1] = "Type";
            Fields[2] = "ID Number";
            ColWidth[0] = 90;
            ColWidth[1] = 110;
            ColWidth[2] = 100;
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
