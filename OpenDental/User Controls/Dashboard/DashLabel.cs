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
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class DashLabel : Label, IDashWidgetField
    {
        private SheetField sheetField;

        public void RefreshData(Patient pat, SheetField sheetField)
        {
            this.sheetField = sheetField;
        }

        public void RefreshView()
        {
            Text = sheetField.FieldValue;
            TextAlign = ConvertToContentAlignment(sheetField.TextAlign);

            string fontName = sheetField.FontName;
            if (string.IsNullOrWhiteSpace(fontName))
            {
                fontName = Font.FontFamily?.Name;//Use the control's default font.
            }

            Font = new Font(fontName, 
                sheetField.FontSize > 0 ? sheetField.FontSize : 8, 
                sheetField.FontIsBold ? FontStyle.Bold : FontStyle.Regular);
        }

        private ContentAlignment ConvertToContentAlignment(HorizontalAlignment align)
        {
            switch (align)
            {
                case HorizontalAlignment.Right:
                    return ContentAlignment.MiddleRight;

                case HorizontalAlignment.Center:
                    return ContentAlignment.MiddleCenter;

                default:
                    return ContentAlignment.MiddleLeft;
            }
        }
    }
}
