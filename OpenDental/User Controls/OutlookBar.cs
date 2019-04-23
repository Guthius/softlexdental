using OpenDental.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class OutlookBar : Control
    {
        const int ButtonHeight = 70;

        public OutlookBarButton[] Buttons;
        public int SelectedIndex = -1;

        int currentHot = -1;

        /// <summary>
        /// Raised whenever a button is clicked.
        /// </summary>
        [Category("Action")] 
        [Description("Occurs when a button is clicked.")]
        public event EventHandler<OutlookBarButtonEventArgs> ButtonClicked = null;

        /// <summary>
        /// Used when click event is cancelled.
        /// </summary>
        int previousSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookBar"/> class.
        /// </summary>
        public OutlookBar()
        {
            DoubleBuffered = true;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

            RefreshButtons();
        }

        void RefreshButtons()
        {
            // TODO: This shouldn't be here...

            Buttons = new OutlookBarButton[7];
            Buttons[0] = new OutlookBarButton("Appts", Resources.Icon32Calendar);
            Buttons[1] = new OutlookBarButton("Family", Resources.Icon32Family);
            Buttons[2] = new OutlookBarButton("Account", Resources.Icon32Account);
            Buttons[3] = new OutlookBarButton("Treat' Plan", Resources.Icon32Listing);
            Buttons[4] = new OutlookBarButton("Chart", Resources.Icon32Tooth);
            Buttons[5] = new OutlookBarButton("Images", Resources.Icon32Images);
            Buttons[6] = new OutlookBarButton("Manage", Resources.Icon32Settings);

            //if (PrefC.GetBool(PrefName.EasyHideClinical))
            //{
            //    Buttons[4].Caption = Lan.g(this, "Procs");
            //}
            //
            //if (Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
            //{
            //    Buttons[3].Image = imageList32.Images[ODColorTheme.OutlookEcwTreatPlanImageIndex];
            //    Buttons[4].Image = imageList32.Images[ODColorTheme.OutlookEcwChartImageIndex];
            //}
        }

        /// <summary>
        /// Paints the bar and all buttons.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
                for (int i = 0; i < Buttons.Length; i++)
                {
                    var pt = PointToClient(MousePosition);

                    var isHot       = Buttons[i].Bounds.Contains(pt);
                    var isPressed   = MouseButtons == MouseButtons.Left && isHot;
                    var isSelected  = i == SelectedIndex;

                    PaintButton(
                        e.Graphics, 
                        Buttons[i], 
                        isHot, 
                        isPressed, 
                        isSelected);
                }

                e.Graphics.DrawLine(SystemPens.ControlDark, Width - 1, 0, Width - 1, Height - 1);
            }
            catch { }
        }

        /// <summary>Draws one button using the info specified.</summary>
        /// <param name="button">Contains caption, image and bounds info.</param>
        /// <param name="isHot">Is the mouse currently hovering over this button.</param>
        /// <param name="isPressed">Is the left mouse button currently down on this button.</param>
        /// <param name="isSelected">Is this the currently selected button</param>
        void PaintButton(Graphics g, OutlookBarButton button, bool isHot, bool isPressed, bool isSelected)
        {
            if (isSelected)
            {
                g.FillRectangle(
                    SystemBrushes.Highlight,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }
            else if (isHot || isPressed)
            {
                g.FillRectangle(
                    SystemBrushes.Window,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }
            else
            {
                g.FillRectangle(
                    SystemBrushes.Control,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }

            // Draw the button image..
            if (button.Image != null)
            {
                try
                {
                    g.DrawImage(
                        button.Image,
                        new Point(
                            button.Bounds.Left + (button.Bounds.Width - button.Image.Width) / 2,
                            button.Bounds.Top + (button.Bounds.Height - button.Image.Height) / 2 - 10));
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(button.Caption))
            {
                var color =
                    isSelected ?
                        SystemColors.HighlightText :
                        SystemColors.ControlText;

                TextRenderer.DrawText(g, 
                    button.Caption, 
                    Font, 
                    new Rectangle(
                        button.Bounds.Left, 
                        button.Bounds.Bottom - 30, 
                        button.Bounds.Width, 
                        20),
                    color);
            }
        }

        /// <summary>
        /// Update the layout of all the buttons.
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            int y = 0;

            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].SetBounds(
                    new Rectangle(0, y, ClientSize.Width, ButtonHeight));

                y += ButtonHeight;
            }

            base.OnLayout(levent);
        }

        /// <summary>
        /// Gets a button index from the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The index of the button at the specified point; or -1 if no button found at the given point.</returns>
        public int IndexFromPoint(Point point)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].Bounds.Contains(point))
                {
                    return i;
                }
            }
            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int index = IndexFromPoint(new Point(e.X, e.Y));
            if (index != currentHot)
            {
                Invalidate();

                currentHot = index;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (currentHot != -1)
            {
                Invalidate();
            }
            currentHot = -1;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (currentHot != -1)
            {
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            int selectedBut = IndexFromPoint(new Point(e.X, e.Y));
            if (selectedBut == -1)
            {
                return;
            }

            int oldSelected = SelectedIndex;
            previousSelected = SelectedIndex;
            SelectedIndex = selectedBut;

            Invalidate(); 

            OnButtonClicked(new OutlookBarButtonEventArgs(Buttons[SelectedIndex], false));
        }

        /// <summary>
        /// Raises the <see cref="ButtonClicked"/> event.
        /// </summary>
        protected virtual void OnButtonClicked(OutlookBarButtonEventArgs e)
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this, e);
                if (e.Cancel)
                {
                    SelectedIndex = previousSelected;
                    Invalidate();
                }
            }
        }
    }

    public class OutlookBarButton
    {
        /// <summary>
        /// Gets the button caption.
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Gets the image of the button.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets the bounds of the button.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookBarButton"/> class.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="image"></param>
        public OutlookBarButton(string caption, Image image = null)
        {
            Caption = caption;
            Image = image;
        }

        /// <summary>
        /// Sets the bounds of the button.
        /// </summary>
        /// <param name="bounds"></param>
        internal void SetBounds(Rectangle bounds) => Bounds = bounds;
    }

    public class OutlookBarButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the button that triggered the event.
        /// </summary>
        public OutlookBarButton Button { get; }

        /// <summary>
        /// Set true to cancel the event.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookBarButtonEventArgs"/> class.
        /// </summary>
        /// <param name="outlookBarButton"></param>
        /// <param name="cancel"></param>
        public OutlookBarButtonEventArgs(OutlookBarButton outlookBarButton, bool cancel)
        {
            Button = outlookBarButton;
            Cancel = cancel;
        }
    }
}