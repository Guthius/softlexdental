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
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyInventory : FormBase
    {
        private List<SupplyNeeded> suppliesList;
        private int pagesPrinted;
        private bool headingPrinted;
        private int headingPrintHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyInventory"/> class.
        /// </summary>
        public FormSupplyInventory() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        private void FormInventory_Load(object sender, EventArgs e)
        {
            categoriesButton.Enabled = Security.IsAuthorized(Permissions.Setup);
            equipmentButton.Enabled = Security.IsAuthorized(Permissions.EquipmentSetup);

            LoadSuppliesNeeded();
        }

        /// <summary>
        /// Loads the supplies needed and populates the grid.
        /// </summary>
        private void LoadSuppliesNeeded()
        {
            suppliesList = SupplyNeeded.All();

            suppliesGrid.BeginUpdate();
            suppliesGrid.Columns.Clear();
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateAdded, 80));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 300));
            suppliesGrid.Rows.Clear();

            for (int i = 0; i < suppliesList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(suppliesList[i].DateAdded.ToShortDateString());
                row.Cells.Add(suppliesList[i].Description);
                suppliesGrid.Rows.Add(row);
            }
            suppliesGrid.EndUpdate();
        }

        /// <summary>
        /// Opens the form the edit the needed supply.
        /// </summary>
        private void SuppliesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplyNeededEdit = new FormSupplyNeededEdit(suppliesList[e.Row]))
            {
                if (formSupplyNeededEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliesNeeded();
                }
            }
        }

        /// <summary>
        /// Opens the form to edit the supply categories.
        /// </summary>
        private void CategoriesButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formDefinitions = new FormDefinitions(DefinitionCategory.SupplyCats))
            {
                formDefinitions.ShowDialog(this);
            }

            SecurityLog.Write(SecurityLogEvents.Setup, Translation.LanguageSecurity.DefinitionsAccessed);
        }

        /// <summary>
        /// Opens the form to edit the suppliers.
        /// </summary>
        private void SuppliersButton_Click(object sender, EventArgs e)
        {
            using (var formSuppliers = new FormSuppliers())
            {
                formSuppliers.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the form to edit the supplies.
        /// </summary>
        private void SuppliesButton_Click(object sender, EventArgs e)
        {
            using (var formSupplies = new FormSupplies())
            {
                formSupplies.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the equipment list form.
        /// </summary>
        private void EquipmentButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.EquipmentSetup)) return;

            using (var formEquipment = new FormEquipment())
            {
                formEquipment.ShowDialog();
            }  
        }

        /// <summary>
        /// Opens the order list form.
        /// </summary>
        private void OrdersButton_Click(object sender, EventArgs e)
        {
            using (var formSupplyOrders = new FormSupplyOrders())
            {
                formSupplyOrders.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the form to add a needed supply.
        /// </summary>
        private void AddNeededButton_Click(object sender, EventArgs e)
        {
            using (var formSupplyNeededEdit = new FormSupplyNeededEdit(new SupplyNeeded()))
            {
                if (formSupplyNeededEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliesNeeded();
                }
            }
        }

        /// <summary>
        /// Prints the list of supplies needed.
        /// </summary>
        private void PrintButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(
                PrintPage,
                Translation.LanguageSecurity.SuppliesNeededListPrinted, 
                PrintoutOrientation.Portrait);
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
  
            using (var font = new Font("Arial", 13, FontStyle.Bold))
            {
                int y = bounds.Top;
                int center = bounds.X + bounds.Width / 2;

                if (!headingPrinted)
                {
                    var text = Translation.Language.SuppliesNeeded;
                    var textSize = e.Graphics.MeasureString(text, font);

                   
                    e.Graphics.DrawString(text, font, Brushes.Black, center - textSize.Width / 2, y);

                    y += (int)e.Graphics.MeasureString(text, font).Height;
                    y += 20;

                    headingPrinted = true;
                    headingPrintHeight = y;
                }

                y = suppliesGrid.PrintPage(e.Graphics, pagesPrinted, bounds, headingPrintHeight);

                pagesPrinted++;
                if (y == -1)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}
