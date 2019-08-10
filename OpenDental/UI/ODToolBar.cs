using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI
{
    [DefaultEvent("ButtonClick")]
    public class ODToolBar : UserControl
    {
        const int ToolBarHeight = 28;
        const int DropDownButtonWidth = 20;

        bool mouseIsDown;
        ODToolBarButton hotButton;
        ToolTip toolTip;
        StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

        [Category("Action")]
        [Description("Occurs when a button is clicked.")]
        public event EventHandler<ODToolBarButtonClickEventArgs> ButtonClick = null;

        [Category("Action")]
        [Description("Occurs when page navigation has occurred.")]
        public event EventHandler<ODToolBarButtonPageNavEventArgs> PageNav = null;

        public ODToolBarButtonCollection Buttons { get; }

        // TODO: The behaviour of PageNav needs to be changed. a PageNav button has a textbox attached to it.
        //       This is fine, however we can theoretically add multiple PageNav buttons to a toolbar, but
        //       there is only a single textbox which is managed not by the button but right here, by the toolbar.
        //       This means that if there ever is a toolbar with multiple PageNav buttons only the last one
        //       will have a textbox and every other PageNav button will just look weird. 
        //       
        //       We need to either give a textbox to every button and have the buttons manage their own textboxes,
        //       or we need to remove PageNav has a button type and add it to the toolbar as a feature that can
        //       be toggled on/off.

        /// <summary>
        /// Initializes a new instance of the <see cref="ODToolBar"/> class.
        /// </summary>
        public ODToolBar()
        {
            Buttons = new ODToolBarButtonCollection(this);

            DoubleBuffered = true;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

            toolTip = new ToolTip
            {
                InitialDelay = 1100
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            Height = ToolBarHeight + 2;

            base.OnLoad(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Height = ToolBarHeight + 2;

            base.OnSizeChanged(e);
        }

        ODToolBarButton HitTest(int x, int y)
        {
            foreach (ODToolBarButton button in Buttons)
            {
                if (button.Bounds.Contains(x, y))
                {
                    return button;
                }
            }
            return null;
        }

        bool HitTestDrop(ODToolBarButton button, int x, int y)
        {
            var dropRect = 
                new Rectangle(
                    button.Bounds.X + button.Bounds.Width - DropDownButtonWidth, 
                    button.Bounds.Y, 
                    DropDownButtonWidth, 
                    button.Bounds.Height);

            if (dropRect.Contains(x, y))
            {
                return true;
            }
            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!mouseIsDown)
            {
                var button = HitTest(e.X, e.Y);

                if (button == hotButton) return;
                else
                {
                    if (hotButton != null)
                    {
                        hotButton.State = ToolBarButtonState.Normal;
                        Invalidate(hotButton.Bounds);
                    }

                    hotButton = button;
                    if (hotButton != null && hotButton.Enabled)
                    {
                        hotButton.State = ToolBarButtonState.Hover;
                        Invalidate(hotButton.Bounds);
                    }
                    else
                    {
                        toolTip.SetToolTip(this, "");
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!mouseIsDown)
            {
                if (hotButton != null)
                {
                    hotButton.State = ToolBarButtonState.Normal;
                    Invalidate(hotButton.Bounds);

                    hotButton = null;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                return;
            }
            mouseIsDown = true;

            var button = HitTest(e.X, e.Y);
            if (button == null)
            {
                return;
            }

            hotButton = button;
            if (button.Style == ODToolBarButtonStyle.DropDownButton && HitTestDrop(button, e.X, e.Y))
            {
                button.State = ToolBarButtonState.DropPressed;
            }
            else
            {
                button.State = ToolBarButtonState.Pressed;
            }
            Invalidate(button.Bounds);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                return;
            }

            if (!mouseIsDown) return;
            
            mouseIsDown = false;

            var button = HitTest(e.X, e.Y);

            var hotButtonTemp = hotButton;
            if (hotButtonTemp != null)
            {
                if (button == hotButtonTemp && button.Enabled)
                {
                    if (button.Style == ODToolBarButtonStyle.DropDownButton &&
                        button.DropDownMenu != null && 
                        HitTestDrop(button, e.X, e.Y))
                    {
                        var contextMenu = button.DropDownMenu.GetContextMenu();

                        button.State = ToolBarButtonState.Normal;

                        contextMenu.Show(
                            this,
                            new Point(
                                button.Bounds.X,
                                button.Bounds.Y + button.Bounds.Height));

                        Invalidate(button.Bounds);
                    }
                    else if (button.Style == ODToolBarButtonStyle.ToggleButton)
                    {
                        button.Pushed = !button.Pushed;
                        OnButtonClicked(new ODToolBarButtonClickEventArgs(button));
                    }
                    else
                    {
                        OnButtonClicked(new ODToolBarButtonClickEventArgs(button));
                    }
                }

                hotButtonTemp.State = ToolBarButtonState.Normal;
                Invalidate(hotButtonTemp.Bounds);

                hotButton = null;
            }

            if (button != null)
            {
                // Do another check to see if the mouse is still over the button. If this mouse up event resulted
                // in a click and inside of the click handler a modal dialog was shown, the mouse cursor may have moved.
                if (!IsDisposed && button.Bounds.Contains(PointToClient(new Point(MousePosition.X, MousePosition.Y))))
                {
                    hotButton = button;
                    hotButton.State = ToolBarButtonState.Hover;

                    Invalidate(hotButton.Bounds);
                }
            }
        }

        protected virtual void OnButtonClicked(ODToolBarButtonClickEventArgs eventArgs)
        {
            if (eventArgs.Button.DateTimeLastClicked.AddMilliseconds(SystemInformation.DoubleClickTime) > DateTime.Now)
            {
                return;
            }
            eventArgs.Button.DateTimeLastClicked = DateTime.Now;

            ButtonClick?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Calculate the positions of all the buttons.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            int width;
            int x = 1;

            foreach (ODToolBarButton button in Buttons)
            {
                switch (button.Style)
                {
                    case ODToolBarButtonStyle.Separator:
                        width = 5;
                        break;

                    case ODToolBarButtonStyle.PageNav:
                        width = TextRenderer.MeasureText("0000/0000", Font).Width + 10;
                        break;

                    default:
                        width = TextRenderer.MeasureText(button.Text, Font).Width + 6;
                        if (button.Image  != null)
                        {
                            width += button.Image.Width + 4;
                        }
                        if (button.Style == ODToolBarButtonStyle.DropDownButton)
                        {
                            width += string.IsNullOrEmpty(button.Text) ? DropDownButtonWidth - 6 : DropDownButtonWidth;
                        }
                        break;
                }

                button.Bounds = new Rectangle(x, 1, width, ToolBarHeight - 1);

                x += width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Control, Bounds);

            if (DesignMode)
            {
                e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, Width - 1, Height - 1);

                TextRenderer.DrawText(e.Graphics, Name, Font, Bounds, SystemColors.ControlText);

                return;
            }

            //OnPaint gets called a lot and the button collection can change while it is in the middle of drawing which can crash the program.
            //It's easier to just swallow any exception random exception that occurs because OnPaint will most likely get fired again real soon.
            //A better solution might be to surround Buttons with a read / write lock but that would be much more complicated than simply repainting.
            try
            {
                foreach (ODToolBarButton button in Buttons)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, button.Bounds);

                    switch (button.Style)
                    {
                        case ODToolBarButtonStyle.Separator:
                            PaintSeperator(e.Graphics, button);
                            break;

                        case ODToolBarButtonStyle.Label:
                            PaintLabel(e.Graphics, button);
                            break;

                        case ODToolBarButtonStyle.PageNav:
                            PaintPageNav(e.Graphics, button);
                            break;

                        default:
                            PaintButton(e.Graphics, button);
                            break;
                    }
                }
            }
            catch { }

            e.Graphics.DrawLine(SystemPens.ControlDark, 0, Height - 1, Width - 1, Height - 1);
        }

        void PaintButton(Graphics g, ODToolBarButton button)
        {
            var borderRect = new Rectangle(button.Bounds.Left, button.Bounds.Top, button.Bounds.Width - 1, button.Bounds.Height - 1);

            if (button.Enabled)
            {
                switch (button.State)
                {
                    case ToolBarButtonState.Hover:
                        g.FillRectangle(SystemBrushes.Highlight, button.Bounds);
                        if (button.Style == ODToolBarButtonStyle.DropDownButton)
                        {
                            g.DrawLine(SystemPens.Control,
                                new Point(borderRect.Right - DropDownButtonWidth, borderRect.Top),
                                new Point(borderRect.Right - DropDownButtonWidth, borderRect.Bottom));
                        }
                        break;

                    case ToolBarButtonState.Pressed:
                    case ToolBarButtonState.DropPressed:
                        g.FillRectangle(SystemBrushes.Window, button.Bounds);
                        g.DrawRectangle(SystemPens.ControlDark, borderRect);
                        break;
                }
            }

            var textBounds =
                Rectangle.FromLTRB(
                    button.Bounds.Left + 3,
                    button.Bounds.Top,
                    button.Bounds.Right - (button.Style == ODToolBarButtonStyle.DropDownButton ? DropDownButtonWidth : 0),
                    button.Bounds.Bottom);
            
            // Draw the button image.
            if (button.Image != null)
            {
                try
                {
                    var x = button.Bounds.Left + 5;
                    var y = button.Bounds.Top + ((textBounds.Height - button.Image.Height) / 2);

                    if (button.Enabled) g.DrawImage(button.Image, new Point(x, y));
                    else
                    {
                        ControlPaint.DrawImageDisabled(g, button.Image, x, y, SystemColors.Control);
                    }

                    textBounds =
                        Rectangle.FromLTRB(
                            textBounds.Left + button.Image.Width + 5,
                            textBounds.Top,
                            textBounds.Right,
                            textBounds.Bottom);
                }
                catch { }
            }

            // Draw the button text.
            if (!string.IsNullOrEmpty(button.Text))
            {
                var textColor =
                    button.State == ToolBarButtonState.Hover ?
                        SystemBrushes.HighlightText :
                        SystemBrushes.ControlText;

                PaintButtonText(g, button.Text, textColor, textBounds, button.Enabled);
            }

            // Draw the dropdown arrow.
            if (button.Style == ODToolBarButtonStyle.DropDownButton)
            {
                int h = DropDownButtonWidth / 2;

                var triangle = new Point[]
                {
                    new Point(button.Bounds.X + button.Bounds.Width - h - 4, button.Bounds.Y + button.Bounds.Height / 2 - 2),
                    new Point(button.Bounds.X + button.Bounds.Width - h + 4, button.Bounds.Y + button.Bounds.Height / 2 - 2),
                    new Point(button.Bounds.X + button.Bounds.Width - h, button.Bounds.Y + button.Bounds.Height / 2 + 2)
                };

                var triangleBrush =
                    button.Enabled ?
                        (button.State == ToolBarButtonState.Hover ?
                            SystemBrushes.HighlightText :
                            SystemBrushes.ControlText) :
                        SystemBrushes.ControlDark;

                g.FillPolygon(triangleBrush, triangle);
            }

            // TODO: Add style of ToggleButton...
        }

        void PaintButtonText(Graphics g, string text, Brush textColor, Rectangle bounds, bool enabled)
        {
            if (enabled)
            {
                g.DrawString(text, Font, textColor, bounds, stringFormat);
            }
            else
            {
                ControlPaint.DrawStringDisabled(g, text, Font, SystemColors.Control, bounds, stringFormat);
            }
        }

        void PaintLabel(Graphics g, ODToolBarButton button) => PaintButtonText(g, button.Text, SystemBrushes.ControlText, button.Bounds, button.Enabled);

        void PaintPageNav(Graphics g, ODToolBarButton button)
        {
            var textColor =
                button.Enabled ?
                    SystemColors.ControlText :
                    SystemColors.ControlDark;

            var text = string.Format("{0}/{1}", button.PageValue, button.PageMax);

            TextRenderer.DrawText(g, button.Text, Font, button.Bounds, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        void PaintSeperator(Graphics g, ODToolBarButton button)
        {
            g.DrawLine(SystemPens.ControlDark, new Point(button.Bounds.Left + 2, button.Bounds.Top + 2), new Point(button.Bounds.Left + 2, button.Bounds.Bottom - 3));
            g.DrawLine(SystemPens.Window, new Point(button.Bounds.Left + 3, button.Bounds.Top + 2), new Point(button.Bounds.Left + 3, button.Bounds.Bottom - 3));
        }
    }
}