using CodeBase;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenDental
{
    public class OutlookBar : Control
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.Container components = null;

        public OutlookBarButton[] Buttons;
        private ImageList imageList32;
        public int SelectedIndex = -1;
        private int currentHot = -1;
        private Font textFont = new Font("Arial", 8);

        [Category("Action"), Description("Occurs when a button is clicked.")]
        public event EventHandler<OutlookBarButtonEventArgs> ButtonClicked = null;


        /// <summary>
        /// Used when click event is cancelled.
        /// </summary>
        private int previousSelected;

        /// <summary>
        /// Class level variable, to avoid allocating and disposing memory repeatedly every frame.
        /// </summary>
        private StringFormat _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookBar"/> class.
        /// </summary>
        public OutlookBar()
        {
            InitializeComponent();

            DoubleBuffered = true; //reduces flicker

            _format = new StringFormat();
            _format.Alignment = StringAlignment.Center;

            Buttons = new OutlookBarButton[7];
            Buttons[0] = new OutlookBarButton(Lan.g(this, "Appts"), imageList32.Images[ODColorTheme.OutlookApptImageIndex]);
            Buttons[1] = new OutlookBarButton(Lan.g(this, "Family"), imageList32.Images[ODColorTheme.OutlookFamilyImageIndex]);
            Buttons[2] = new OutlookBarButton(Lan.g(this, "Account"), imageList32.Images[ODColorTheme.OutlookAcctImageIndex]);
            Buttons[3] = new OutlookBarButton(Lan.g(this, "Treat' Plan"), imageList32.Images[ODColorTheme.OutlookTreatPlanImageIndex]);
            Buttons[4] = new OutlookBarButton(Lan.g(this, "Chart"), imageList32.Images[ODColorTheme.OutlookChartImageIndex]);
            Buttons[5] = new OutlookBarButton(Lan.g(this, "Images"), imageList32.Images[ODColorTheme.OutlookImagesImageIndex]);
            Buttons[6] = new OutlookBarButton(Lan.g(this, "Manage"), imageList32.Images[ODColorTheme.OutlookManageImageIndex]);
            UpdateAll();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutlookBar));
            this.imageList32 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList32
            // 
            this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
            this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList32.Images.SetKeyName(0, "Appt32.gif");
            this.imageList32.Images.SetKeyName(1, "Family32b.gif");
            this.imageList32.Images.SetKeyName(2, "Account32b.gif");
            this.imageList32.Images.SetKeyName(3, "TreatPlan3D.gif");
            this.imageList32.Images.SetKeyName(4, "chart32.gif");
            this.imageList32.Images.SetKeyName(5, "Images32.gif");
            this.imageList32.Images.SetKeyName(6, "Manage32.gif");
            this.imageList32.Images.SetKeyName(7, "TreatPlanMed32.gif");
            this.imageList32.Images.SetKeyName(8, "ChartMed32.gif");
            this.imageList32.Images.SetKeyName(9, "Date-32_Blue.png");
            this.imageList32.Images.SetKeyName(10, "User-Group-32_Blue.png");
            this.imageList32.Images.SetKeyName(11, "Money-Credit-Card-32_Blue.png");
            this.imageList32.Images.SetKeyName(12, "Payments-32_Blue.png");
            this.imageList32.Images.SetKeyName(13, "Dentist-32_Blue.png");
            this.imageList32.Images.SetKeyName(14, "Folder-Picture-32_Blue.png");
            this.imageList32.Images.SetKeyName(15, "Gear-32_Blue.png");
            this.ResumeLayout(false);

        }
        #endregion

        public void RefreshButtons()
        {
            Buttons = new OutlookBarButton[7];
            Buttons[0] = new OutlookBarButton(Lan.g(this, "Appts"), imageList32.Images[ODColorTheme.OutlookApptImageIndex]);
            Buttons[1] = new OutlookBarButton(Lan.g(this, "Family"), imageList32.Images[ODColorTheme.OutlookFamilyImageIndex]);
            Buttons[2] = new OutlookBarButton(Lan.g(this, "Account"), imageList32.Images[ODColorTheme.OutlookAcctImageIndex]);
            Buttons[3] = new OutlookBarButton(Lan.g(this, "Treat' Plan"), imageList32.Images[ODColorTheme.OutlookTreatPlanImageIndex]);
            Buttons[4] = new OutlookBarButton(Lan.g(this, "Chart"), imageList32.Images[ODColorTheme.OutlookChartImageIndex]);
            Buttons[5] = new OutlookBarButton(Lan.g(this, "Images"), imageList32.Images[ODColorTheme.OutlookImagesImageIndex]);
            Buttons[6] = new OutlookBarButton(Lan.g(this, "Manage"), imageList32.Images[ODColorTheme.OutlookManageImageIndex]);

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // We had one customer who was receiving overflow exceptions because the ClientRetangle provided by the system was invalid,
            // due to a graphics device hardware state change when loading the Dexis client application via our Dexis bridge.
            // If we receive an invalid ClientRectangle, then we will simply not draw the button for a frame or two until the system has initialized.
            // A couple of frames later the system should return to normal operation and we will be able to draw the button again.
            try
            {
                CalculateButtonInfo();

                bool isHot;
                bool isSelected;
                bool isPressed;

                e.Graphics.DrawLine(Pens.Gray, Width - 1, 0, Width - 1, Height - 1);

                for (int i = 0; i < Buttons.Length; i++)
                {
                    Point mouseLoc = PointToClient(MousePosition);

                    isHot = Buttons[i].Bounds.Contains(mouseLoc);
                    isPressed = (MouseButtons == MouseButtons.Left && isHot);
                    isSelected = (i == SelectedIndex);

                    DrawButton(Buttons[i], isHot, isPressed, isSelected, e.Graphics);
                }
            }
            catch { }
        }

        /// <summary>Draws one button using the info specified.</summary>
        /// <param name="button">Contains caption, image and bounds info.</param>
        /// <param name="isHot">Is the mouse currently hovering over this button.</param>
        /// <param name="isPressed">Is the left mouse button currently down on this button.</param>
        /// <param name="isSelected">Is this the currently selected button</param>
        private void DrawButton(OutlookBarButton button, bool isHot, bool isPressed, bool isSelected, Graphics g)
        {
            if (!button.Visible)
            {
                g.FillRectangle(
                    ODColorTheme.OutlookBackBrush,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
                return;
            }

            if (isPressed)
            {
                g.FillRectangle(
                    ODColorTheme.OutlookPressedBrush,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }
            else if (isSelected)
            {
                g.FillRectangle(
                    ODColorTheme.OutlookSelectedBrush,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);


                Rectangle gradientRect =
                    new Rectangle(
                        button.Bounds.X,
                        button.Bounds.Y + button.Bounds.Height - 10,
                        button.Bounds.Width,
                        10);

                if (!button.Color1HotBrush.Equals(ODColorTheme.OutlookSelectedBrush.Color) ||
                    !button.Color2HotBrush.Equals(ODColorTheme.OutlookPressedBrush.Color))
                {
                    if (button.HotBrush != null)
                    {
                        button.HotBrush.Dispose();
                    }
                    button.Color1HotBrush = ODColorTheme.OutlookSelectedBrush.Color;
                    button.Color2HotBrush = ODColorTheme.OutlookPressedBrush.Color;
                    button.HotBrush = new LinearGradientBrush(gradientRect, button.Color1HotBrush, button.Color2HotBrush, LinearGradientMode.Vertical);
                }
                g.FillRectangle(button.HotBrush, button.Bounds.X, button.Bounds.Y + button.Bounds.Height - 10, button.Bounds.Width + 1, 10);
            }
            else if (isHot)
            {
                g.FillRectangle(
                    ODColorTheme.OutlookHotBrush,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }
            else
            {
                g.FillRectangle(
                    ODColorTheme.OutlookBackBrush,
                    button.Bounds.X,
                    button.Bounds.Y,
                    button.Bounds.Width + 1,
                    button.Bounds.Height + 1);
            }

            //
            // Outline
            //
            if (isPressed || isSelected || isHot)
            {
                g.FillPolygon(
                    ODColorTheme.OutlookBackBrush, 
                    new Point[] {
                        new Point(button.Bounds.X, button.Bounds.Y),
                        new Point(button.Bounds.X + 3, button.Bounds.Y),
                        new Point(button.Bounds.X, button.Bounds.Y + 3)});

                g.FillPolygon(
                    ODColorTheme.OutlookBackBrush, 
                    new Point[] {
                        new Point(button.Bounds.X + button.Bounds.Width - 2, button.Bounds.Y),
                        new Point(button.Bounds.X + button.Bounds.Width + 1, button.Bounds.Y),
                        new Point(button.Bounds.X + button.Bounds.Width + 1, button.Bounds.Y + 3)});

                g.FillPolygon(
                    ODColorTheme.OutlookBackBrush, 
                    new Point[] {
                        new Point(button.Bounds.X + button.Bounds.Width + 1, button.Bounds.Y + button.Bounds.Height - 3),
                        new Point(button.Bounds.X + button.Bounds.Width + 1, button.Bounds.Y + button.Bounds.Height + 1),
                        new Point(button.Bounds.X + button.Bounds.Width - 3, button.Bounds.Y + button.Bounds.Height + 1)});

                g.FillPolygon(
                    ODColorTheme.OutlookBackBrush,
                    new Point[] {
                        new Point(button.Bounds.X, button.Bounds.Y + button.Bounds.Height - 3),
                        new Point(button.Bounds.X + 3, button.Bounds.Y + button.Bounds.Height + 1),
                        new Point(button.Bounds.X, button.Bounds.Y + button.Bounds.Height + 1)});

                GraphicsHelper.DrawRoundedRectangle(g, 
                    ODColorTheme.OutlookOutlineColor, 
                    button.Bounds, 
                    ODColorTheme.OutlookHoverCornerRadius);
            }

            // Image
            var imageBounds = new Rectangle((Width - button.Image.Width) / 2, button.Bounds.Y + 3, button.Image.Width, button.Image.Height);
            if (button.Image != null)
            {
                // If fails, then would be a bad reason to crash. 
                // Should still draw button text below so user knows what the button is.
                try
                {
                    g.DrawImage(button.Image, imageBounds);
                }
                catch { }
            }

            //Text
            Rectangle textRect = new Rectangle(button.Bounds.X - 1, imageBounds.Bottom + 3, button.Bounds.Width + 2, button.Bounds.Bottom - imageBounds.Bottom + 3);
            g.DrawString(button.Caption, textFont, ODColorTheme.OutlookTextBrush, textRect, _format);
        }

        internal void UpdateAll()
        {
            // Calculates Button info and redraws all.
            //if(!m_BeginUpdate){
            CalculateButtonInfo();
            this.Invalidate();
            //}
        }

        private void CalculateButtonInfo()
        {
            using (Graphics g = CreateGraphics())
            {
                int top = 0;
                int width = this.Width - 2;
                int textHeight = 0;

                for (int i = 0; i < Buttons.Length; i++)
                {
                    //--- Look if multiline text, if is add extra Height to button.
                    SizeF textSize = g.MeasureString(Buttons[i].Caption, textFont, width + 2);

                    textHeight = (int)(Math.Ceiling(textSize.Height));
                    if (textHeight < 26)
                        textHeight = 26;//default to height of 2 lines of text for uniformity.

                    Buttons[i].SetBounds(new Rectangle(0, top, width, 39 + textHeight));
                    top += 39 + textHeight + 1;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateAll();
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

            if (!Buttons[selectedBut].Visible)
            {
                return;
            }

            int oldSelected = SelectedIndex;
            previousSelected = SelectedIndex;
            SelectedIndex = selectedBut;
            Invalidate(); //just invalidate to force a repaint

            OnButtonClicked(new OutlookBarButtonEventArgs(Buttons[SelectedIndex], false));
        }

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
        public string Caption { get; private set; }

        public Image Image { get; private set; }

        public bool Visible { get; private set; }

        /// <summary>
        /// Gets the bounds of the button.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookBarButton"/> class.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="image"></param>
        public OutlookBarButton(string caption, Image image)
        {
            Caption = caption;
            Image = image;
            Bounds = new Rectangle(0, 0, 0, 0);
            Visible = true;
            HotBrush = null;
            Color1HotBrush = Color.Transparent;
            Color2HotBrush = Color.Transparent;
        }

        /// <summary>
        /// Sets the bounds of the button.
        /// </summary>
        /// <param name="bounds"></param>
        internal void SetBounds(Rectangle bounds) => Bounds = bounds;




        /// <summary>
        /// Used inside OutlookBar logic to track theme colors and location.  Do not modify externally!
        /// </summary>
        public LinearGradientBrush HotBrush;

        /// <summary>
        /// Used inside OutlookBar logic to track theme color.  Do not modify externally!
        /// </summary>
        public Color Color1HotBrush;

        /// <summary>
        /// Used inside OutlookBar logic to track theme color.  Do not modify externally!
        /// </summary>
        public Color Color2HotBrush;
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
