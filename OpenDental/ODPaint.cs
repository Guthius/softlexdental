using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public static class ODPaint
    {
        /// <summary>
        /// Replaces Graphics.DrawString. Finds a suitable font size to fit the text to the bounding rectangle.
        /// </summary>
        public static void FitText(string text, Font font, Brush brush, RectangleF rectF, StringFormat stringFormat, Graphics graphics)
        {
            float emSize = font.Size;
            while (true)
            {
                using (Font newFont = new Font(font.FontFamily, emSize, font.Style))
                {
                    Size size = TextRenderer.MeasureText(text, newFont);
                    if (size.Width < rectF.Width || emSize < 2)
                    { //does our new font fit? only allow smallest of 2 point font.
                        graphics.DrawString(text, newFont, brush, rectF, stringFormat);
                        return;
                    }
                }
                //text didn't fit so decrement the font size and try again
                emSize -= .1F;
            }
        }
    }
}
