using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class TableTimeBar : OpenDental.ContrTable
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

        private void TableTimeBar_Load(object sender, System.EventArgs e)
        {
            LayoutTables();
        }

        private void butSlider_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void butSlider_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void butSlider_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
    }
}