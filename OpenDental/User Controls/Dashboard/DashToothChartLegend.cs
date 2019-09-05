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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class DashToothChartLegend : PictureBox, IDashWidgetField
    {
        public const int DefaultWidth = 600;
        public const int DefaultHeight = 14;

        private SheetField sheetField;
        private List<Definition> definitions;

        public void RefreshData(Patient pat, SheetField sheetField)
        {
            this.sheetField = sheetField;

            definitions = Definition.GetByCategory(DefinitionCategory.ChartGraphicColors); ;
        }

        public void RefreshView()
        {
            var image = new Bitmap(sheetField.Width, sheetField.Height);
            using (var g = Graphics.FromImage(image))
            {
                SheetPrinting.DrawToothChartLegend(0, 0, sheetField.Width, 0, definitions, g, null);

                if (Image != null)
                {
                    Image.Dispose();
                }
                Image = image;
            }
        }
    }
}
