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
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI
{
    /// <summary>
    /// Represents a list box where every item is displayed as a clickable link.
    /// </summary>
    public partial class ListBoxClickable : ListBox
    {
        private int hotItem = -1;

        public ListBoxClickable()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 15;
            SelectionMode = SelectionMode.None;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int hotPrevious = hotItem;

            hotItem = IndexFromPoint(e.Location);
            if (hotItem != hotPrevious)
            {
                if (hotPrevious != -1)
                {
                    Invalidate(GetItemRectangle(hotPrevious));
                }

                if (hotItem != -1)
                {
                    Invalidate(GetItemRectangle(hotItem));
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hotItem != -1)
            {
                Invalidate(GetItemRectangle(hotItem));
            }

            hotItem = -1;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);

            var color = hotItem == e.Index ? Color.Firebrick : Color.Black;

            using (var font = new Font(e.Font, FontStyle.Underline))
            {
                if (e.Index != -1 && e.Index <= Items.Count - 1)
                {
                    TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), font, e.Bounds, color, TextFormatFlags.Left);
                }
            }
        }
    }
}
