using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public partial class ODButtonPanel : UserControl
    {
        const int RowHeight = 18;
        ODButtonPanelItem selectedItem;
        ODButtonPanelItem hotItem;
        StringFormat stringFormat;

        /// <summary>
        /// Raised when a item is clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when a item is clicked.")]
        public event EventHandler<ODButtonPanelItemEventArgs> ItemClick = null;

        /// <summary>
        /// Raised when a button is clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when a button item is clicked.")]
        public event EventHandler<ODButtonPanelItemEventArgs> ButtonClick = null;

        /// <summary>
        /// Raised when a label is clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when a label item is clicked.")]
        public event EventHandler<ODButtonPanelItemEventArgs> LabelClick = null;

        /// <summary>
        /// Raised when a item is clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when an item is clicked.")]
        public event EventHandler<ODButtonPanelItemMouseEventArgs> MouseClickItem = null;

        /// <summary>
        /// Raised when a row is clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when an row is clicked.")]
        public event EventHandler<ODButtonPanelRowMouseEventArgs> MouseClickRow = null;

        /// <summary>
        /// Raised when a item is double clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when an item is clicked.")]
        public event EventHandler<ODButtonPanelItemMouseEventArgs> MouseDoubleClickItem = null;

        /// <summary>
        /// Raised when a row is double clicked.
        /// </summary>
        [Category("Action"), Description("Occurs when an row is clicked.")]
        public event EventHandler<ODButtonPanelRowMouseEventArgs> MouseDoubleClickRow = null;

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ODButtonPanelItemCollection Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODButtonPanel"/> class.
        /// </summary>
        public ODButtonPanel()
        {
            InitializeComponent();

            Items = new ODButtonPanelItemCollection(this);

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);

            DoubleBuffered = true;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (Items == null)
            {
                return;
            }
            Items.Sort();

            // Calculate the size of all the items.
            using (var graphics = CreateGraphics())
            {
                int x = 0, row = 0;
                foreach (var item in Items)
                {
                    if (item.Row > row)
                    {
                        x = 4;
                        row = item.Row;
                    }

                    var size = TextRenderer.MeasureText(item.Text, Font);

                    var width = size.Width + 6;
                    if (item.Type == ODButtonPanelItemType.Button)
                    {
                        width += 6;
                    }

                    item.SetBounds(
                        new Rectangle(
                            x,
                            4 + row * (RowHeight + 2),
                            width, RowHeight));

                    x += width + 2;
                }
            }

            Invalidate();
        }

        /// <summary>Finds and returns the item at the specified point.</summary>
        /// <param name="pt">The point.</param>
        /// <returns>The item at the specified point.</returns>
        public ODButtonPanelItem PointToItem(Point pt)
        {
            foreach (var item in Items)
            {
                if (item.Bounds.Contains(pt))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Don't paint the background. Reduces flicker.
        /// </summary>
        /// <param name="pea"></param>
        protected override void OnPaintBackground(PaintEventArgs pea)
        {
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 1 || Height < 1) return;

            e.Graphics.FillRectangle(SystemBrushes.Window, ClientRectangle);

            try
            {
                foreach (var item in Items)
                {
                    switch (item.Type)
                    {
                        case ODButtonPanelItemType.Button:
                            DrawItemButton(e.Graphics, item);
                            break;

                        case ODButtonPanelItemType.Label:
                            DrawItemLabel(e.Graphics, item);
                            break;
                    }
                }
            }
            catch { }

            ControlPaint.DrawVisualStyleBorder(e.Graphics, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        /// <summary>
        /// Draws a label.
        /// </summary>
        void DrawItemLabel(Graphics graphics, ODButtonPanelItem item)
        {
            stringFormat = stringFormat ??
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

            graphics.DrawString(
                item.Text,
                Font,
                SystemBrushes.ControlText,
                item.Bounds,
                stringFormat);
        }

        /// <summary>
        /// Draws a button.
        /// </summary>
        void DrawItemButton(Graphics graphics, ODButtonPanelItem item)
        {
            Brush backBrush = SystemBrushes.Control;
            if (item == selectedItem || (selectedItem == null && item == hotItem))
            {
                backBrush = SystemBrushes.Window;
            }

            graphics.FillRectangle(backBrush, item.Bounds);

            using (var stringFormat = GetStringFormat(ContentAlignment.MiddleCenter))
            {
                graphics.DrawString(
                    item.Text,
                    Font,
                    SystemBrushes.ControlText,
                    item.Bounds,
                    stringFormat);
            }

            graphics.DrawRectangle(SystemPens.ControlDark, item.Bounds);
        }

        /// <summary>
        /// Constructs a string format based on the specified content alignment.
        /// </summary>
        /// <param name="contentAlignment"></param>
        /// <returns></returns>
        StringFormat GetStringFormat(ContentAlignment contentAlignment)
        {
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

        /// <summary>
        /// Raises the <see cref="MouseClickItem"/> event.
        /// </summary>
        protected virtual void OnMouseClickItem(ODButtonPanelItemMouseEventArgs e) => MouseClickItem?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="MouseClickRow"/> event.
        /// </summary>
        protected virtual void OnMouseClickRow(ODButtonPanelRowMouseEventArgs e) => MouseClickRow?.Invoke(this, e);

        /// <summary>
        /// Detects if the user clicked on a item and if so calls <see cref="OnMouseClickItem(ODButtonPanelItemMouseEventArgs)"/>; otherwise,
        /// calls <see cref="OnMouseClickRow(ODButtonPanelRowMouseEventArgs)"/> instead.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var item = PointToItem(e.Location);
            if (item != null)
            {
                OnMouseClickItem(new ODButtonPanelItemMouseEventArgs(item, e));
            }
            else
            {
                OnMouseClickRow(new ODButtonPanelRowMouseEventArgs(e.Y / RowHeight, e));
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDoubleClickItem"/> event.
        /// </summary>
        protected virtual void OnMouseDoubleClickItem(ODButtonPanelItemMouseEventArgs e) => MouseDoubleClickItem?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="MouseDoubleClickRow"/> event.
        /// </summary>
        protected virtual void OnMouseDoubleClickRow(ODButtonPanelRowMouseEventArgs e) => MouseDoubleClickRow?.Invoke(this, e);

        /// <summary>
        /// Detects if the user clicked on a item and if so calls <see cref="OnMouseDoubleClickItem(ODButtonPanelItemMouseEventArgs)"/>; otherwise,
        /// calls <see cref="OnMouseDoubleClickRow(ODButtonPanelRowMouseEventArgs)"/> instead.
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            var item = PointToItem(e.Location);
            if (item != null)
            {
                OnMouseDoubleClickItem(new ODButtonPanelItemMouseEventArgs(item, e));
            }
            else
            {
                OnMouseDoubleClickRow(new ODButtonPanelRowMouseEventArgs(e.Y / RowHeight, e));
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemClick"/> event.
        /// </summary>
        protected virtual void OnItemClick(ODButtonPanelItemEventArgs e) => ItemClick?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="ButtonClick"/> event.
        /// </summary>
        protected virtual void OnButtonClick(ODButtonPanelItemEventArgs e) => ButtonClick?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="LabelClick"/> event.
        /// </summary>
        protected virtual void OnLabelClick(ODButtonPanelItemEventArgs e) => LabelClick?.Invoke(this, e);

        /// <summary>
        /// Checks whether the mouse is down on a item.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                selectedItem = PointToItem(e.Location);
            }
        }

        /// <summary>
        /// If the mouse went down on a item, check wether the mouse is still on top of the item.
        /// If so calls either the <see cref="OnButtonClick(ODButtonPanelItemEventArgs)"/> or <see cref="OnLabelClick(ODButtonPanelItemMouseEventArgs)"/>
        /// based on the type of the item clicked.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                if (selectedItem != null && selectedItem.Bounds.Contains(e.Location))
                {
                    var iea = new ODButtonPanelItemEventArgs(selectedItem);

                    OnItemClick(iea);
                    switch (selectedItem.Type)
                    {
                        case ODButtonPanelItemType.Button:
                            OnButtonClick(iea);
                            break;

                        case ODButtonPanelItemType.Label:
                            OnLabelClick(iea);
                            break;
                    }
                }

                var prevSelectedItem = selectedItem;

                selectedItem = null;

                if (prevSelectedItem != null)
                {
                    Invalidate(prevSelectedItem.Bounds);
                }
            }
        }
            
        


        /// <summary>
        /// Finds and highlights the item the mouse is over.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var item = PointToItem(e.Location);
            if (item != hotItem)
            {
                if (hotItem != null)
                {
                    hotItem.State = ODButtonPanelItemState.Normal;
                    Invalidate(hotItem.Bounds);
                }

                hotItem = item;
                if (hotItem != null)
                {
                    hotItem.State = ODButtonPanelItemState.Hot;
                    Invalidate(hotItem.Bounds);

                    Cursor = Cursors.Hand;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Resets the state of the item the mouse is over.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hotItem != null)
            {
                hotItem.State = ODButtonPanelItemState.Normal;
                Invalidate(hotItem.Bounds);

                hotItem = null;
            }

            Cursor = Cursors.Default;
        }
    }
}