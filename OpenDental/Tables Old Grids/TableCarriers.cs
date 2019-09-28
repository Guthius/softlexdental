using System;
using System.Drawing;

namespace OpenDental.Forms
{
    public class TableCarriers : ContrTable
    {
        public TableCarriers()
        {
            MaxRows = 50;
            MaxCols = 8;
            ShowScroll = true;
            FieldsArePresent = true;
            HeadingIsPresent = false;
            InstantClassesPar();
            SetRowHeight(0, 49, 14);
            Fields[0] = "Carrier Name";
            Fields[1] = "Phone";
            Fields[2] = "Address";
            Fields[3] = "Address2";
            Fields[4] = "City";
            Fields[5] = "ST";
            Fields[6] = "Zip";
            Fields[7] = "ElectID";
            ColWidth[0] = 160;
            ColWidth[1] = 90;
            ColWidth[2] = 130;
            ColWidth[3] = 120;
            ColWidth[4] = 110;
            ColWidth[5] = 60;
            ColWidth[6] = 90;
            ColWidth[7] = 60;
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
