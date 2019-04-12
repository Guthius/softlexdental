using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI {
	public class OdProgressBar:ProgressBar {
		public OdProgressBar() {
			this.SetStyle(ControlStyles.UserPaint,true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent) {
			//Limits flickering
		}

		protected override void OnPaint(PaintEventArgs e) {
			using(Image progressImage=new Bitmap(this.Width,this.Height)) {
				using(Graphics g=Graphics.FromImage(progressImage)) {
					Rectangle rect=new Rectangle(0,0,this.Width,this.Height);
					if(ProgressBarRenderer.IsSupported) {
						ProgressBarRenderer.DrawHorizontalBar(g,rect);
					}
					rect.Inflate(new Size(-1,-1));//Reduce progress bar color size
					rect.Width=(int)(rect.Width*(((double)Value-(double)Minimum)/((double)Maximum-(double)Minimum)));
					if(rect.Width==0) {
						rect.Width=1;//Can't draw a 0 width rectangle.
					}
					LinearGradientBrush brush=new LinearGradientBrush(rect,this.BackColor,this.ForeColor,LinearGradientMode.Vertical);
					g.FillRectangle(brush,1,1,rect.Width,rect.Height);
					e.Graphics.DrawImage(progressImage,0,0);
					brush.Dispose();
				}
			}
		}
	}
}
