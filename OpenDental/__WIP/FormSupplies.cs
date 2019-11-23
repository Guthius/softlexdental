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
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplies : FormBase
    {
        private List<Supply> supplies;
        private List<Supplier> suppliers;
        private int pagesPrinted;
        private bool headingPrinted;
        private int headingPrintHeight;
        private List<Supply> displayedSupplies;

        /// <summary>
        /// Gets or sets the ID of the selected supplier.
        /// </summary>
        public long SelectedSupplierId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is in selection mode.
        /// </summary>
        public bool IsSelectionMode { get; set; }

        /// <summary>
        /// Gets the list of selected supplies.
        /// </summary>
        public List<Supply> SelectedSupplies { get; } = new List<Supply>();

        public FormSupplies() => InitializeComponent();

        private void FormSupplies_Load(object sender, EventArgs e)
        {
            suppliers = Supplier.All();
            supplies = Supply.All();
            displayedSupplies = new List<Supply>();

            FillSupplierComboBox();
            FillSuppliesGrid();

            if (IsSelectionMode)
            {
                supplierComboBox.Enabled = false;
                addToOrderButton.Visible = false;
            }
        }

        private void FillSupplierComboBox()
        {
            supplierComboBox.Items.Clear();
            supplierComboBox.Items.Add(Translation.Language.All);
            supplierComboBox.SelectedIndex = 0;

            for (int i = 0; i < suppliers.Count; i++)
            {
                supplierComboBox.Items.Add(suppliers[i].Name);
                if (suppliers[i].Id == SelectedSupplierId)
                {
                    supplierComboBox.SelectedItem = suppliers[i].Name;
                }
            }
        }

        private void FillSuppliesGrid()
        {
            var selectedSupplies = new List<Supply>();
            foreach (int index in suppliesGrid.SelectedIndices)
            {
                selectedSupplies.Add((Supply)suppliesGrid.Rows[index].Tag);
            }

            displayedSupplies.Clear();

            suppliesGrid.BeginUpdate();
            suppliesGrid.Columns.Clear();
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnCategory, 130));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnCatalogNumber, 80));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSupplier, 100));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 200));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPrice, 60, HorizontalAlignment.Right));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnStockQty, 60, HorizontalAlignment.Center));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnOnHandQty, 80, HorizontalAlignment.Center));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnHidden, 40, HorizontalAlignment.Center));
            suppliesGrid.Rows.Clear();

            for (int i = 0; i < supplies.Count; i++)
            {
                var supply = supplies[i];
                if (!MatchesFilterCriteria(supply))
                {
                    continue;
                }

                var row = new ODGridRow();

                // Add the new category header in this row if it doesn't match the previous row's category.
                if (suppliesGrid.Rows.Count == 0 || (suppliesGrid.Rows.Count > 0 && supply.CategoryId != ((Supply)suppliesGrid.Rows[suppliesGrid.Rows.Count - 1].Tag).CategoryId))
                {
                    row.Cells.Add(Defs.GetName(DefinitionCategory.SupplyCats, supply.CategoryId)); 
                }
                else
                {
                    row.Cells.Add("");
                }

                row.Cells.Add(supply.CatalogNumber);
                row.Cells.Add(Supplier.GetName(suppliers, supply.SupplierId));
                row.Cells.Add(supply.Description);
                row.Cells.Add(supply.Price == 0 ? "" : supply.Price.ToString("n"));
                row.Cells.Add(supply.LevelDesired == 0 ? "" : supply.LevelDesired.ToString());
                row.Cells.Add(supply.LevelOnHand.ToString());
                row.Cells.Add(supply.Hidden ? "X" : "");
                row.Tag = supply;

                suppliesGrid.Rows.Add(row);

                displayedSupplies.Add(supply);
            }

            suppliesGrid.EndUpdate();

            for (int i = 0; i < suppliesGrid.Rows.Count; i++)
            {
                if (selectedSupplies.Contains((Supply)suppliesGrid.Rows[i].Tag))
                {
                    suppliesGrid.SetSelected(i, true);
                }
            }

            addToOrderButton.Enabled = showShoppingListCheckBox.Checked && displayedSupplies.Count > 0;
        }

        /// <summary>
        /// Determines whether the specified supply matches the current filter criteria.
        /// </summary>
        /// <param name="supply"></param>
        /// <returns>True if the supply matches criteria; otherwise, false.</returns>
        bool MatchesFilterCriteria(Supply supply)
        {
            if (!showHiddenCheckBox.Checked && supply.Hidden)
                return false;

            if (SelectedSupplierId != 0 && supply.SupplierId != SelectedSupplierId)
                return false;

            if (supplierComboBox.SelectedIndex != 0 && suppliers[supplierComboBox.SelectedIndex - 1].Id != supply.SupplierId)
                return false;

            if (showShoppingListCheckBox.Checked && supply.LevelOnHand >= supply.LevelDesired)
                return false;

            if (!string.IsNullOrEmpty(searchTextBox.Text) &&
                !supply.Description.ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.CatalogNumber.ToString().ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !Defs.GetName(DefinitionCategory.SupplyCats, supply.CategoryId).ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.LevelDesired.ToString().Contains(searchTextBox.Text) &&
                !supply.Price.ToString().ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.SupplierId.ToString().Contains(searchTextBox.Text))
            {
                return false;
            }

            return true;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // No supply categories have been entered, not allowed to enter supply
            if (Definition.GetByCategory(DefinitionCategory.SupplyCats).Count == 0)
            {
                MessageBox.Show(
                    Translation.Language.NoSupplyCategoriesHaveBeenCreated,
                    Translation.Language.Supplies, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            //Includes no items or the ALL item being selected
            if (supplierComboBox.SelectedIndex < 1)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectASupplierFromTheList,
                    Translation.Language.Supplies,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var supply = new Supply
            {
                SupplierId = suppliers[supplierComboBox.SelectedIndex - 1].Id//Selected index -1 to account for ALL being at the top of the list.
            };

            if (suppliesGrid.GetSelectedIndex() > -1)
            {
                supply.CategoryId = ((Supply)suppliesGrid.Rows[suppliesGrid.GetSelectedIndex()].Tag).CategoryId;
            }

            using (var formSupplyEdit = new FormSupplyEdit())
            {
                formSupplyEdit.Supply = supply;
                formSupplyEdit.Suppliers.AddRange(suppliers);

                if (formSupplyEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (formSupplyEdit.Supply == null) return;

                supplies.Add(formSupplyEdit.Supply);//new category, add to bottom of list
            }

            FillSuppliesGrid();
        }

        private void ShowShoppingListCheckBox_Click(object sender, EventArgs e) => FillSuppliesGrid();

        private void ShowHiddenCheckBox_Click(object sender, EventArgs e) => FillSuppliesGrid();

        private void SearchTextBox_TextChanged(object sender, EventArgs e) => FillSuppliesGrid();

        private void SupplierComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillSuppliesGrid();

        private void SuppliesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsSelectionMode)
            {
                SelectedSupplies.Clear();
                for (int i = 0; i < suppliesGrid.SelectedIndices.Length; i++)
                {
                    SelectedSupplies.Add((Supply)suppliesGrid.Rows[suppliesGrid.SelectedIndices[i]].Tag);
                }

                return;
            }

            var selectedSupply = (Supply)suppliesGrid.Rows[e.Row].Tag;
            var oldCategoryId = selectedSupply.CategoryId;

            using (var formSupplyEdit = new FormSupplyEdit())
            {
                formSupplyEdit.Supply = selectedSupply;
                formSupplyEdit.Suppliers.AddRange(suppliers);

                if (formSupplyEdit.ShowDialog(this) == DialogResult.OK)
                {
                    if (formSupplyEdit.Supply == null)
                    {
                        supplies.Remove(selectedSupply);
                    }
                    else if (selectedSupply.CategoryId != oldCategoryId)
                    {
                        supplies.Remove(selectedSupply);

                        int categoryIndex = supplies.FindLastIndex(x => x.CategoryId == selectedSupply.CategoryId);
                        if (categoryIndex == -1)
                        {
                            supplies.Add(selectedSupply);
                        }
                        else
                        {
                            supplies.Insert(categoryIndex + 1, selectedSupply);
                        }
                    }
                }
            }

            int scroll = suppliesGrid.ScrollValue;

            FillSuppliesGrid();

            suppliesGrid.ScrollValue = scroll;
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (suppliesGrid.Rows.Count < 1)
            {
                MessageBox.Show(
                    Translation.Language.SupplyListEmpty,
                    Translation.Language.Supplies, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(
                PrintPage, 
                "Supplies list printed", 
                margins: new Margins(50, 50, 40, 30));
        }

        private void AddToOrderButton_Click(object sender, EventArgs e)
        {
            if (supplierComboBox.SelectedIndex == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectASupplierFromTheList,
                    Translation.Language.Supplies, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var supplyOrders = SupplyOrder.All();

            foreach (var supplyOrder in supplyOrders)
            {
                if (!supplyOrder.DatePlaced.HasValue && supplyOrder.SupplierId == suppliers[supplierComboBox.SelectedIndex - 1].Id)
                {
                    MessageBox.Show(
                        Translation.Language.ThereIsAPendingOrderForThisSupplier,
                        Translation.Language.Supplies, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return;
                }
            }

            var supplyOrderId = SupplyOrder.Insert(new SupplyOrder
            {
                UserId = Security.CurrentUser.Id,
                SupplierId = suppliers[supplierComboBox.SelectedIndex - 1].Id,
                DatePlaced = null,
                Note = ""
            });

            for (int i = 0; i < displayedSupplies.Count; i++)
            {
                SupplyOrderItem.Insert(new SupplyOrderItem
                {
                    SupplyId = displayedSupplies[i].Id,
                    SupplyOrderId = supplyOrderId,
                    Quantity = (int)displayedSupplies[i].LevelDesired - (int)displayedSupplies[i].LevelOnHand,
                    Price = displayedSupplies[i].Price,
                });
            }

            SupplyOrder.UpdateOrderPrice(supplyOrderId);

            MessageBox.Show(
                Translation.Language.ItemsAddedToOrderSuccessfully,
                Translation.Language.Supplies, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            Graphics g = e.Graphics;
            string text;
            Font headingFont = new Font("Arial", 13, FontStyle.Bold);
            Font subHeadingFont = new Font("Arial", 10, FontStyle.Bold);
            int yPos = bounds.Top;
            if (!headingPrinted)
            {
                text = Translation.Language.SupplyList;
                g.DrawString(text, headingFont, Brushes.Black, 425 - g.MeasureString(text, headingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, headingFont).Height;
                if (showShoppingListCheckBox.Checked)
                {
                    text = Translation.Language.ShoppingList;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                if (showHiddenCheckBox.Checked)
                {
                    text = Translation.Language.ShowingHiddenItems;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                else
                {
                    text = Translation.Language.NotShowingHiddenItems;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                if (searchTextBox.Text != "")
                {
                    text = Translation.Language.SearchFilter + ": " + searchTextBox.Text;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                else
                {
                    text = Translation.Language.NoSearchFilter;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                if (supplierComboBox.SelectedIndex < 1)
                {
                    text = Translation.Language.AllSuppliers;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                }
                else
                {
                    text = Translation.Language.Supplier + ": " + suppliers[supplierComboBox.SelectedIndex - 1].Name;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                    if (suppliers[supplierComboBox.SelectedIndex - 1].Phone != "")
                    {
                        text = Translation.Language.Phone + ": " + suppliers[supplierComboBox.SelectedIndex - 1].Phone;
                        g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                        yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                    }
                    if (suppliers[supplierComboBox.SelectedIndex - 1].Name != "")
                    {
                        text = Translation.Language.Note + ": " + suppliers[supplierComboBox.SelectedIndex - 1].Name;
                        g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                        yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                    }
                }
                yPos += 15;
                headingPrinted = true;
                headingPrintHeight = yPos;
            }
            yPos = suppliesGrid.PrintPage(g, pagesPrinted, bounds, headingPrintHeight);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            g.Dispose();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (IsSelectionMode)
            {
                if (suppliesGrid.SelectedIndices.Length < 1)
                {
                    MessageBox.Show(
                        Translation.Language.PleaseSelectASupplyFromTheList,
                        Translation.Language.Supplies, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return;
                }

                SelectedSupplies.Clear();
                for (int i = 0; i < suppliesGrid.SelectedIndices.Length; i++)
                {
                    SelectedSupplies.Add((Supply)suppliesGrid.Rows[suppliesGrid.SelectedIndices[i]].Tag);
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
