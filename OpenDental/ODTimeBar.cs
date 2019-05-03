using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public class ODTimeBar : UserControl
    {
        int maxTime = 120;
        int timeIncrement = 5;
        int numberOfCells = 0;
        ODTimeBarCell[] cells;
        int selectedIndex = 0;
        Rectangle sliderBounds;
        Brush[] selectionBrush = new Brush[] { new SolidBrush(Color.FromArgb(250, 200, 100)), new SolidBrush(Color.FromArgb(150, 240, 170)) };
        Pen[] selectionPenLight = new Pen[] { new Pen(Color.FromArgb(240, 180, 50)), new Pen(Color.FromArgb(80, 220, 90)) };
        Pen[] selectionPenDark = new Pen[] { new Pen(Color.FromArgb(255, 128, 0)), new Pen(Color.FromArgb(30, 180, 60)) };
        bool mouseOverSlider;
        bool draggingSlider;
        bool mouseDown;
        ODTimeBarCell mouseDownCell;

        /// <summary>
        /// Represents a single cell on the <see cref="ODTimeBar"/>.
        /// </summary>
        class ODTimeBarCell
        {
            /// <summary>
            /// Gets the index of the cell.
            /// </summary>
            public int Index { get; private set; }

            /// <summary>
            /// Gets the bounds of the cell.
            /// </summary>
            public Rectangle Bounds { get; private set; }

            /// <summary>
            /// Gets the cell type.
            /// </summary>
            public ODTimeBarCellType Type { get; set; } = ODTimeBarCellType.Provider;

            /// <summary>
            /// Initializes a new instance of the <see cref="ODTimeBarCell"/> class.
            /// </summary>
            /// <param name="index">The index of the cell.</param>
            public ODTimeBarCell(int index) => Index = index;

            /// <summary>
            /// Sets the bounds of the cell.
            /// </summary>
            /// <param name="bounds">The new bounds of the cell.</param>
            internal void SetBounds(Rectangle bounds) => Bounds = bounds;
        }

        /// <summary>
        /// Gets or sets the type of the cell with the specified index.
        /// </summary>
        /// <param name="index">The cell index.</param>
        /// <returns></returns>
        public ODTimeBarCellType this[int index]
        {
            get
            {
                if (index < 0 || index >= numberOfCells)
                    throw new IndexOutOfRangeException();

                return cells[index].Type;
            }
            set
            {
                if (index < 0 || index >= numberOfCells)
                    throw new IndexOutOfRangeException();

                if (value != cells[index].Type)
                {
                    cells[index].Type = value;

                    Invalidate(cells[index].Bounds);
                }
            }
        }

        /// <summary>
        /// Gets or sets the time pattern.
        /// </summary>
        public string Pattern
        {
            get
            {
                var stringBuilder = new StringBuilder();

                for (int i = 0; i <= selectedIndex; i++)
                {
                    switch (cells[i].Type)
                    {
                        case ODTimeBarCellType.Hygienist:
                            stringBuilder.Append('/');
                            break;

                        case ODTimeBarCellType.Provider:
                            stringBuilder.Append('X');
                            break;
                    }
                }

                return stringBuilder.ToString();
            }
            set
            {
                if (value == null)
                {
                    Clear();
                }
                else
                {
                    var pattern = value;
                    if (pattern.Length > numberOfCells)
                    {
                        pattern = pattern.Substring(0, numberOfCells);
                    }

                    for (int i = 0; i < pattern.Length; i++)
                    {
                        if (pattern[i] == '/')
                        {
                            cells[i].Type = ODTimeBarCellType.Hygienist;
                        }
                        else if (pattern[i] == 'X')
                        {
                            cells[i].Type = ODTimeBarCellType.Provider;
                        }
                    }

                    var newSelectedIndex = pattern.Length - 1;
                    if (newSelectedIndex > selectedIndex)
                    {
                        SelectedIndex = newSelectedIndex;
                    }
                    else
                    {
                        Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        public void Clear()
        {
            foreach (var cell in cells)
            {
                if (cell != null)
                {
                    cell.Type = ODTimeBarCellType.Provider;
                }
            }
            SelectedIndex = 0;
        }

        /// <summary>
        /// Gets or sets the maximum amount of time that can be selected (in minutes).
        /// </summary>
        public int MaxTime
        {
            get => maxTime;
            set
            {
                var newMaxTime = value;
                if (newMaxTime < 5)
                {
                    newMaxTime = 5;
                }

                // Adjust the max time to the nearest value that is divisible by 5.
                var r = newMaxTime % 5;
                if (r != 0)
                {
                    if (r > 2)
                    {
                        newMaxTime += 5 - r;
                    }
                    else
                    {
                        newMaxTime -= r;
                    }
                }

                if (newMaxTime != maxTime)
                {
                    maxTime = newMaxTime;

                    PerformLayout();

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the amount of time (in minutes) represented by each cell in the time bar.
        /// </summary>
        public int TimeIncrement
        {
            get => timeIncrement;
            set
            {
                var newTimeIncrement = value;

                if (newTimeIncrement != 5 &&
                    newTimeIncrement != 6 &&
                    newTimeIncrement != 10 &&
                    newTimeIncrement != 15)
                {
                    return;
                }

                if (newTimeIncrement != timeIncrement)
                {
                    timeIncrement = newTimeIncrement;

                    PerformLayout();

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of time selected.
        /// </summary>
        public int SelectedTime
        {
            get => (SelectedIndex + 1) * TimeIncrement;
            set
            {
                SelectedIndex = value / TimeIncrement;
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected slot.
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                var newIndex = value;

                if (newIndex < 0) newIndex = 0;
                else if (newIndex >= numberOfCells)
                {
                    newIndex = numberOfCells - 1;
                }

                if (newIndex != selectedIndex)
                {
                    SetSelectedIndex(newIndex);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODTimeBar"/> class.
        /// </summary>
        public ODTimeBar()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);

            DoubleBuffered = true;
        }

        /// <summary>
        /// We don't paint the background for this control.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        /// <summary>
        /// Paints the timebar.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, Bounds);

            // Paint the cells.
            if (numberOfCells > 0 && cells != null && cells.Length >= numberOfCells)
            {
                for (int i = 0; i < numberOfCells; i++)
                {
                    var selected = i <= selectedIndex;
                    var type = (int)cells[i].Type;
                    var brush =
                        selected ?
                            selectionBrush[type] :
                            Brushes.White;

                    e.Graphics.FillRectangle(brush, cells[i].Bounds);
                    if (i < (numberOfCells - 1))
                    {
                        var pen = selected ? selectionPenLight[type] : SystemPens.Control;
                        if (i > 0 && ((i + 1) * TimeIncrement) % 60 == 0)
                        {
                            pen = selected ?
                                selectionPenDark[type] :
                                SystemPens.ControlDark;
                        }

                        e.Graphics.DrawLine(pen,
                            new Point(cells[i].Bounds.Left, cells[i].Bounds.Bottom - 1),
                            new Point(cells[i].Bounds.Right - 1, cells[i].Bounds.Bottom - 1));
                    }
                }
            }

            // Draw the slider.
            if (sliderBounds != Rectangle.Empty)
            {
                e.Graphics.FillRectangle(SystemBrushes.ControlDark, sliderBounds);

                e.Graphics.DrawLine(
                    SystemPens.ControlDarkDark,
                    new Point(sliderBounds.Left + 2, sliderBounds.Y + 2),
                    new Point(sliderBounds.Right - 3, sliderBounds.Y + 2));
            }

            // Draw the border around the bar.
            ControlPaint.DrawVisualStyleBorder(e.Graphics, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        /// <summary>
        /// Updates the layout of the control. Calculates the correct positioning of each cell.
        /// </summary>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            numberOfCells = MaxTime / TimeIncrement;
            if (numberOfCells == 0)
            {
                return;
            }

            if (selectedIndex >= numberOfCells)
            {
                selectedIndex = numberOfCells - 1;
            }

            if (cells == null || cells.Length < numberOfCells)
            {
                cells = new ODTimeBarCell[numberOfCells];
                for (int i = 0; i < numberOfCells; i++)
                {
                    if (cells[i] == null)
                    {
                        cells[i] = new ODTimeBarCell(i);
                    }
                }
            }

            int height = Height - 2;
            int heightPerCell = height / numberOfCells;
            if (heightPerCell < 0)
            {
                heightPerCell = 0;
            }

            // If the height isn't evenly divded by the number of cells there will be some extra space
            // (usually 1 or 2 pixels). We'll distribute this among the cells to ensure that all cells 
            // combined will always use 100% of the height.
            int heightExtra = height - (heightPerCell * numberOfCells);

            // Determine the position of each cell.
            int y = 1;
            for (int i = 0; i < numberOfCells; i++)
            {
                var cellHeight = heightPerCell;

                // Distribute the extra height...
                if (heightExtra > 0)
                {
                    cellHeight++;
                    heightExtra--;
                }

                cells[i].SetBounds(new Rectangle(1, y, Width - 2, cellHeight));
                y += cellHeight;
            }

            sliderBounds = new Rectangle(0, cells[selectedIndex].Bounds.Bottom - 3, Width, 5);
        }

        /// <summary>
        /// Updates the currently selected index.
        /// </summary>
        /// <param name="selectedIndex"></param>
        void SetSelectedIndex(int selectedIndex)
        {
            if (selectedIndex < 0 || cells == null || selectedIndex >= cells.Length)
            {
                return;
            }

            this.selectedIndex = selectedIndex;

            sliderBounds = new Rectangle(0, cells[selectedIndex].Bounds.Bottom - 3, Width, 5);

            OnSelectedIndexChanged(EventArgs.Empty);

            Invalidate();
        }

        /// <summary>
        /// Raised whenever the <see cref="SelectedIndex"/> property changes.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Raises the <see cref="SelectedIndexChanged"/> event.
        /// </summary>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);

            // When the index has changed, the time will also be changed.
            SelectedTimeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raised whenever the <see cref="SelectedTime"/> property changes.
        /// </summary>
        public event EventHandler SelectedTimeChanged;

        /// <summary>
        /// If a mouse button is pressed, check whether the slider was selected or a cell.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                if (sliderBounds.Contains(e.Location))
                {
                    draggingSlider = true;
                }
                else
                {
                    mouseDownCell = null;
                    foreach (var cell in cells)
                    {
                        if (cell != null && cell.Bounds.Contains(e.Location))
                        {
                            mouseDownCell = cell;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stops dragged, or if the mouse was down over a cell, triggers a cell click event.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left && mouseDown)
            {
                if (draggingSlider)
                {
                    draggingSlider = false;
                }
                else
                {
                    if (mouseDownCell != null && mouseDownCell.Bounds.Contains(e.Location))
                    {
                        ClickCell(mouseDownCell);
                        mouseDownCell = null;
                    }
                }
            }
        }

        /// <summary>
        /// Handles a click on a cell. If the cell is outside the selected time range, we move the slider down
        /// to the include the clicked cell in the selection. If the cell is part of the selection we toggle between 
        /// the cell type.
        /// </summary>
        /// <param name="cell">The cell that was clicked.</param>
        void ClickCell(ODTimeBarCell cell)
        {
            if (cell != null)
            {
                if (cell.Index > SelectedIndex)
                {
                    SelectedIndex = cell.Index;
                }
                else
                {
                    cell.Type =
                        (cell.Type == ODTimeBarCellType.Provider) ?
                            ODTimeBarCellType.Hygienist :
                            ODTimeBarCellType.Provider;

                    Invalidate(cell.Bounds);
                }
            }
        }

        /// <summary>
        /// Changes the mouse cursor when the mouse is over the slider.
        /// If the mouse is down on the slider tries to move the slider.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (draggingSlider)
            {
                for (int i = numberOfCells - 1; i >= 0; i--)
                {
                    int y = cells[i].Bounds.Top + (cells[i].Bounds.Height / 2);
                    if (e.Location.Y > y)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                mouseOverSlider = sliderBounds.Contains(e.Location);
                if (mouseOverSlider)
                {
                    Cursor = Cursors.HSplit;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Reset the cursor when the mouse leaves the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (mouseOverSlider)
            {
                mouseOverSlider = false;
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// Identifies the type of a <see cref="ODTimeBarCell"/>. This determines the color of a cell when it is selected.
    /// </summary>
    public enum ODTimeBarCellType
    {
        Provider,
        Hygienist
    }
}