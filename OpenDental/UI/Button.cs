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
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CodeBase;
using System.Drawing.Text;

namespace OpenDental.UI
{
    [Obsolete]
    public class Button : System.Windows.Forms.Button
    {
        private ControlState buttonState = ControlState.Normal;
        private bool canClick = false;
        private Point adjustImageLocation;
        private float cornerRadius = 4;

        public Button()
        {
            SetStyle(
                ControlStyles.UserPaint | 
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, 
                true);
        }

        public EnumType.BtnShape BtnShape
        {
            get => EnumType.BtnShape.Rectangle;
            set
            {
            }
        }

        public EnumType.XPStyle BtnStyle
        {
            get => EnumType.XPStyle.Silver;
            set
            {
            }
        }

        public new FlatStyle FlatStyle
        {
            get => base.FlatStyle;
            set => base.FlatStyle = FlatStyle.Standard;
        }

        public bool Autosize { get; set; } = true;

        public Point AdjustImageLocation
        {
            get => adjustImageLocation;
            set
            {
                adjustImageLocation = value;

                Invalidate();
            }
        }

        public float CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;

                Invalidate();
            }
        }

        protected override void OnClick(EventArgs ea)
        {
            Capture = false;

            canClick = false;
            if (ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
            {
                buttonState = ControlState.Hover;
            }
            else
            {
                buttonState = ControlState.Normal;
            }

            Invalidate();

            base.OnClick(ea);
        }

        protected override void OnMouseEnter(EventArgs ea)
        {
            base.OnMouseEnter(ea);

            buttonState = ControlState.Hover;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);

            if (mea.Button == MouseButtons.Left)
            {
                canClick = true;

                buttonState = ControlState.Pressed;

                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);

            if (ClientRectangle.Contains(mea.X, mea.Y))
            {
                if (buttonState == ControlState.Hover && this.Capture && !canClick)
                {
                    canClick = true;

                    buttonState = ControlState.Pressed;

                    Invalidate();
                }
            }
            else
            {
                if (buttonState == ControlState.Pressed)
                {
                    canClick = false;

                    buttonState = ControlState.Hover;

                    Invalidate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs ea)
        {
            base.OnMouseLeave(ea);

            buttonState = ControlState.Normal;

            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs ea)
        {
            base.OnEnabledChanged(ea);

            buttonState = ControlState.Normal;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs p)
        {
            OnPaintBackground(p);

            try
            {
                var outlineRect = new RectangleF(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                switch (buttonState)
                {
                    case ControlState.Normal:
                        if (Enabled)
                        {
                            if (Focused || IsDefault)
                            {
                                DrawBackground(p.Graphics, outlineRect, cornerRadius, ODColorTheme.ButDefaultDarkBrush, ODColorTheme.ButMainBrush, ODColorTheme.ButLightestBrush);
                            }
                            else
                            {
                                DrawBackground(p.Graphics, outlineRect, cornerRadius, ODColorTheme.ButDarkestBrush, ODColorTheme.ButMainBrush, ODColorTheme.ButLightestBrush);
                            }
                        }
                        break;

                    case ControlState.Hover:
                        DrawBackground(p.Graphics, outlineRect, cornerRadius, ODColorTheme.ButHoverDarkBrush, ODColorTheme.ButHoverMainBrush, ODColorTheme.ButHoverLightBrush);
                        break;

                    case ControlState.Pressed:
                        DrawBackground(p.Graphics, outlineRect, cornerRadius, ODColorTheme.ButPressedDarkestBrush, ODColorTheme.ButPressedMainBrush, ODColorTheme.ButPressedLightestBrush);
                        break;
                }

                GraphicsHelper.DrawRoundedRectangle(p.Graphics, ODColorTheme.ButBorderPen, outlineRect, cornerRadius);

                DrawTextAndImage(p.Graphics);

                GraphicsHelper.DrawReflection(p.Graphics, outlineRect, cornerRadius);
            }
            catch
            {
                // We had one customer who was receiving overflow exceptions because the 
                // ClientRetangle provided by the system was invalid, due to a graphics device 
                // hardware state change when loading the Dexis client application via our Dexis 
                // bridge. If we receive an invalid ClientRectangle, then we will simply not draw 
                // the button for a frame or two until the system has initialized. A couple of 
                // frames later the system should return to normal operation and we will be able 
                // to draw the button again.
            }
        }

        private void DrawBackground(Graphics g, RectangleF rect, float radius, SolidBrush darkBrush, SolidBrush mainBrush, SolidBrush lightBrush)
        {
            if (radius < 0)
            {
                radius = 0;
            }
            LinearGradientBrush brush;
            g.SmoothingMode = SmoothingMode.HighQuality;
            //sin(45)=.85. But experimentally, .7 works much better.
            //1/.85=1.18 But experimentally, 1.37 works better. What gives?
            //top
            g.FillRectangle(mainBrush, rect.Left + radius, rect.Top, rect.Width - (radius * 2), radius);
            //UR
            //2 pies of 45 each.
            g.FillPie(mainBrush, rect.Right - (radius * 2), rect.Top, radius * 2, radius * 2, 270, 45);
            brush = new LinearGradientBrush(new PointF(rect.Right - (radius / 2f) - .5f, rect.Top + (radius / 2f) - .5f),
                new PointF(rect.Right, rect.Top + radius),
                mainBrush.Color, darkBrush.Color);
            g.FillPie(brush, rect.Right - (radius * 2), rect.Top, radius * 2, radius * 2, 315, 45);
            brush.Dispose();
            //right
            brush = new LinearGradientBrush(new PointF(rect.Right - radius, rect.Top + radius),
                new PointF(rect.Right, rect.Top + radius), mainBrush.Color, darkBrush.Color);
            g.FillRectangle(brush, rect.Right - radius, rect.Top + radius - .5f, radius, rect.Height - (radius * 2) + 1f);
            brush.Dispose();
            //LR
            g.FillPie(darkBrush, rect.Right - (radius * 2), rect.Bottom - (radius * 2), radius * 2, radius * 2, 0, 90);
            brush = new LinearGradientBrush(new PointF(rect.Right - radius, rect.Bottom - radius),
                new PointF(rect.Right - (radius * .5f) + .5f, rect.Bottom - (radius * .5f) + .5f),
                mainBrush.Color, darkBrush.Color);
            g.FillPolygon(brush, new PointF[] {
                new PointF(rect.Right-radius,rect.Bottom-radius),
                new PointF(rect.Right,rect.Bottom-radius),
                new PointF(rect.Right-radius,rect.Bottom)});
            brush.Dispose();
            //bottom
            brush = new LinearGradientBrush(new PointF(rect.Left + radius, rect.Bottom - radius), new PointF(rect.Left + radius, rect.Bottom),
                mainBrush.Color, darkBrush.Color);
            g.FillRectangle(brush, rect.Left + radius - .5f, rect.Bottom - radius, rect.Width - (radius * 2) + 1f, radius);
            brush.Dispose();
            //LL
            //2 pies of 45 each.
            brush = new LinearGradientBrush(new PointF(rect.Left + (radius / 2f), rect.Bottom - (radius / 2f)),
                new PointF(rect.Left + radius, rect.Bottom),
                mainBrush.Color, darkBrush.Color);
            g.FillPie(brush, rect.Left, rect.Bottom - (radius * 2), radius * 2, radius * 2, 90, 45);
            brush.Dispose();
            g.FillPie(mainBrush, rect.Left, rect.Bottom - (radius * 2), radius * 2, radius * 2, 135, 45);
            //left
            g.FillRectangle(mainBrush, rect.Left, rect.Top + radius, radius, rect.Height - (radius * 2));
            //UL
            g.FillPie(//new SolidBrush(clrLight)
                mainBrush, rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
            //center
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rect.Left - rect.Width / 8f, rect.Top - rect.Height / 2f, rect.Width, rect.Height * 3f / 2f);
            PathGradientBrush pathBrush = new PathGradientBrush(path);
            pathBrush.CenterColor = ODColorTheme.ButCenterColor;
            pathBrush.SurroundColors = new Color[] { Color.FromArgb(0, 255, 255, 255) };//Transparent
            g.FillRectangle(mainBrush, rect.Left + radius - .5f, rect.Top + radius - .5f, rect.Width - (radius * 2) + 1f, rect.Height - (radius * 2) + 1f);
            g.FillRectangle(pathBrush, rect.Left + radius - .5f, rect.Top + radius - .5f, rect.Width - (radius * 2) + 1f, rect.Height - (radius * 2) + 1f);
            //highlights
            brush = new LinearGradientBrush(new PointF(rect.Left + radius, rect.Top), new PointF(rect.Left + radius + rect.Width * 2f / 3f, rect.Top),
                lightBrush.Color, mainBrush.Color);
            g.FillRectangle(brush, rect.Left + radius, rect.Y + radius * 3f / 8f, rect.Width / 2f, radius / 4f);
            brush.Dispose();
            path = new GraphicsPath();
            path.AddLine(rect.Left + radius, rect.Top + radius * 3 / 8, rect.Left + radius, rect.Top + radius * 5 / 8);
            path.AddArc(new RectangleF(rect.Left + radius * 5 / 8, rect.Top + radius * 5 / 8, radius * 3 / 4, radius * 3 / 4), 270, -90);
            path.AddArc(new RectangleF(rect.Left + radius * 3 / 8, rect.Top + radius * 7 / 8, radius * 1 / 4, radius * 1 / 4), 0, 180);
            path.AddArc(new RectangleF(rect.Left + radius * 3 / 8, rect.Top + radius * 3 / 8, radius * 5 / 4, radius * 5 / 4), 180, 90);
            //g.DrawPath(Pens.Red,path);
            g.FillPath(lightBrush, path);
            path.Dispose();
        }

        private void DrawTextAndImage(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            var textBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark;

            var stringFormat = GetStringFormat(TextAlign);
            stringFormat.HotkeyPrefix = ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;

            if (Image != null)
            {
                Rectangle recTxt = new Rectangle();
                Point ImagePoint = new Point(6, 4);
                switch (ImageAlign)
                {
                    case ContentAlignment.MiddleLeft:
                        ImagePoint.X = 6;
                        ImagePoint.Y = ClientRectangle.Height / 2 - Image.Height / 2;
                        recTxt.Width = ClientRectangle.Width - Image.Width;
                        recTxt.Height = ClientRectangle.Height;
                        recTxt.X = Image.Width;
                        recTxt.Y = 0;
                        break;
                    case ContentAlignment.MiddleRight:
                        recTxt.Width = ClientRectangle.Width - Image.Width - 8;
                        recTxt.Height = ClientRectangle.Height;
                        recTxt.X = 0;
                        recTxt.Y = 0;
                        ImagePoint.X = recTxt.Width;
                        recTxt.Width += adjustImageLocation.X;
                        ImagePoint.Y = ClientRectangle.Height / 2 - Image.Height / 2;
                        break;
                    case ContentAlignment.MiddleCenter:// no text in this alignment
                        ImagePoint.X = (this.ClientRectangle.Width - this.Image.Width) / 2;
                        ImagePoint.Y = (this.ClientRectangle.Height - this.Image.Height) / 2;
                        recTxt.Width = 0;
                        recTxt.Height = 0;
                        recTxt.X = this.ClientRectangle.Width;
                        recTxt.Y = this.ClientRectangle.Height;
                        break;
                }
                ImagePoint.X += adjustImageLocation.X;
                ImagePoint.Y += adjustImageLocation.Y;

                if (Enabled)  g.DrawImage(Image, ImagePoint);
                else
                {
                    ControlPaint.DrawImageDisabled(g, Image, ImagePoint.X, ImagePoint.Y, BackColor);
                }

                var glowBounds = new RectangleF(recTxt.X + .5f, recTxt.Y + .5f, recTxt.Width, recTxt.Height);
                if (ImageAlign != ContentAlignment.MiddleCenter)
                {
                    if (Enabled)
                    {
                        g.DrawString(Text, Font, ODColorTheme.ButGlowBrush, glowBounds, stringFormat);
                    }
                    g.DrawString(Text, Font, textBrush, recTxt, stringFormat);
                }
            }
            else
            {
                var glowBounds = new RectangleF(ClientRectangle.X + .5f, ClientRectangle.Y + .5f, ClientRectangle.Width, ClientRectangle.Height);
                if (Enabled)
                {
                    g.DrawString(Text, Font, ODColorTheme.ButGlowBrush, glowBounds, stringFormat);
                }
                g.DrawString(Text, Font, textBrush, ClientRectangle, stringFormat);
            }
            stringFormat.Dispose();
        }

        private StringFormat GetStringFormat(ContentAlignment contentAlignment)
        {
            if (!Enum.IsDefined(typeof(ContentAlignment), (int)contentAlignment))
                throw new InvalidEnumArgumentException(nameof(contentAlignment), (int)contentAlignment, typeof(ContentAlignment));
            
            var stringFormat = new StringFormat();
            switch (contentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleLeft:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopCenter:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.TopLeft:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.BottomRight:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }
            return stringFormat;
        }

        public enum ControlState
        {
            Normal,
            Hover,
            Pressed,
            Default,
            Disabled
        }
    }

    public class EnumType
    {
        public enum XPStyle
        {
            Default,
            Blue,
            OliveGreen,
            Silver
        }

        public enum BtnShape
        {
            Rectangle,
            Ellipse
        }
    }
}
