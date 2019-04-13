using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental
{
    public class FormBase : ODForm
    {
        public FormBase()
        {
            AutoScaleMode = AutoScaleMode.Inherit;
            Padding = new Padding(10, 16, 10, 10);
            Font = new Font("Segoe UI", 9f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(ClientSize.Width - 1, 6),
                Color.FromArgb(40, 110, 240),
                Color.FromArgb(0, 70, 140)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, ClientSize.Width, 6));
            }
        }
    }
}
