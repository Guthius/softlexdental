using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplies : FormBase
    {
        List<Supply> suppliesList;
        List<Supply> suppliesListOld;
        List<Supplier> suppliersList;
        int pagesPrinted;
        bool headingPrinted;
        int headingPrintHeight;
        List<Supply> selectedSuppliesList;
        List<Supply> displayedSuppliesList;

        public long SelectedSupplierNum;
        public bool IsSelectMode;
        public List<Supply> ListSelectedSupplies;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplies"/> class.
        /// </summary>
        public FormSupplies() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplies_Load(object sender, EventArgs e)
        {
            ListSelectedSupplies = new List<Supply>();
            suppliersList = Suppliers.GetAll();
            suppliesList = Supplies.GetAll();
            selectedSuppliesList = new List<Supply>();
            suppliesListOld = new List<Supply>();
            displayedSuppliesList = new List<Supply>();

            foreach (Supply supply in suppliesList) // Make deep copy of the list so we can sync later.
            {
                suppliesListOld.Add(supply.Copy());
            }

            FillSupplierComboBox();
            FillSuppliesGrid();

            if (IsSelectMode)
            {
                supplierComboBox.Enabled = false;
                addToOrderButton.Visible = false;
            }
        }

        /// <summary>
        /// Populates the supplier combobox.
        /// </summary>
        void FillSupplierComboBox()
        {
            supplierComboBox.Items.Clear();
            supplierComboBox.Items.Add(Translation.Language.All);
            supplierComboBox.SelectedIndex = 0;

            for (int i = 0; i < suppliersList.Count; i++)
            {
                supplierComboBox.Items.Add(suppliersList[i].Name);
                if (suppliersList[i].SupplierNum == SelectedSupplierNum)
                {
                    supplierComboBox.SelectedItem = suppliersList[i].Name;
                }
            }
        }

        /// <summary>
        /// Populates the supplies grid.
        /// </summary>
        void FillSuppliesGrid()
        {
            List<Supply> selectedSupplies = new List<Supply>();
            foreach (int index in suppliesGrid.SelectedIndices)
            {
                selectedSupplies.Add((Supply)suppliesGrid.Rows[index].Tag);
            }


            displayedSuppliesList.Clear();

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

            for (int i = 0; i < suppliesList.Count; i++)
            {
                var supply = suppliesList[i];
                if (!MatchesFilterCriteria(supply))
                {
                    continue;
                }

                var row = new ODGridRow();

                // Add the new category header in this row if it doesn't match the previous row's category.
                if (suppliesGrid.Rows.Count == 0 || (suppliesGrid.Rows.Count > 0 && supply.Category != ((Supply)suppliesGrid.Rows[suppliesGrid.Rows.Count - 1].Tag).Category))
                {
                    row.Cells.Add(Defs.GetName(DefinitionCategory.SupplyCats, supply.Category)); 
                }
                else
                {
                    row.Cells.Add("");
                }

                row.Cells.Add(supply.CatalogNumber);
                row.Cells.Add(Suppliers.GetName(suppliersList, supply.SupplierNum));
                row.Cells.Add(supply.Descript);

                if (supply.Price == 0)
                {
                    row.Cells.Add("");
                }
                else
                {
                    row.Cells.Add(supply.Price.ToString("n"));
                }

                if (supply.LevelDesired == 0)
                {
                    row.Cells.Add("");
                }
                else
                {
                    row.Cells.Add(supply.LevelDesired.ToString());
                }

                row.Cells.Add(supply.LevelOnHand.ToString());
                if (supply.IsHidden)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }

                row.Tag = supply;
                suppliesGrid.Rows.Add(row);
                displayedSuppliesList.Add(supply);
            }

            suppliesGrid.EndUpdate();

            for (int i = 0; i < suppliesGrid.Rows.Count; i++)
            {
                if (selectedSupplies.Contains((Supply)suppliesGrid.Rows[i].Tag))
                {
                    suppliesGrid.SetSelected(i, true);
                }
            }

            addToOrderButton.Enabled = showShoppingListCheckBox.Checked && displayedSuppliesList.Count > 0;
        }

        /// <summary>
        /// Determines whether the specified supply matches the current filter criteria.
        /// </summary>
        /// <param name="supply"></param>
        /// <returns>True if the supply matches criteria; otherwise, false.</returns>
        bool MatchesFilterCriteria(Supply supply)
        {
            if (!showHiddenCheckBox.Checked && supply.IsHidden)
                return false;

            if (SelectedSupplierNum != 0 && supply.SupplierNum != SelectedSupplierNum)
                return false;

            if (supplierComboBox.SelectedIndex != 0 && suppliersList[supplierComboBox.SelectedIndex - 1].SupplierNum != supply.SupplierNum)
                return false;

            if (showShoppingListCheckBox.Checked && supply.LevelOnHand >= supply.LevelDesired)
                return false;

            if (!string.IsNullOrEmpty(searchTextBox.Text) &&
                !supply.Descript.ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.CatalogNumber.ToString().ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !Defs.GetName(DefinitionCategory.SupplyCats, supply.Category).ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.LevelDesired.ToString().Contains(searchTextBox.Text) &&
                !supply.Price.ToString().ToUpper().Contains(searchTextBox.Text.ToUpper()) &&
                !supply.SupplierNum.ToString().Contains(searchTextBox.Text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens the form to add a new supply.
        /// </summary>
        void AddButton_Click(object sender, EventArgs e)
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
                IsNew = true,
                SupplierNum = suppliersList[supplierComboBox.SelectedIndex - 1].SupplierNum//Selected index -1 to account for ALL being at the top of the list.
            };

            if (suppliesGrid.GetSelectedIndex() > -1)
            {
                supply.Category = ((Supply)suppliesGrid.Rows[suppliesGrid.GetSelectedIndex()].Tag).Category;
            }

            using (var formSupplyEdit = new FormSupplyEdit())
            {
                formSupplyEdit.Supp = supply;
                formSupplyEdit.ListSupplier = suppliersList;

                if (formSupplyEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (formSupplyEdit.Supp == null) return;
                
                formSupplyEdit.Supp.ItemOrder = suppliesList.FindAll(x => x.Category == formSupplyEdit.Supp.Category)
                    .Select(x => x.ItemOrder)
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1; // Last item in the category.

                int idx = suppliesList.FindLastIndex(x => x.Category == formSupplyEdit.Supp.Category);
                if (idx == -1)
                {
                    suppliesList.Add(formSupplyEdit.Supp);//new category, add to bottom of list
                }
                else
                {
                    suppliesList.Insert(idx + 1, formSupplyEdit.Supp);//add to bottom of existing category
                }
            }


            FillSuppliesGrid();
        }

        void ShowShoppingListCheckBox_Click(object sender, EventArgs e) => FillSuppliesGrid();
        
        void ShowHiddenCheckBox_Click(object sender, EventArgs e) => FillSuppliesGrid();

        void SearchTextBox_TextChanged(object sender, EventArgs e) => FillSuppliesGrid();

        void SupplierComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillSuppliesGrid();
        
        /// <summary>
        /// Opens the form to edit a supply when the user double clicks on a supply in the grid.
        /// </summary>
        void SuppliesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            selectedSuppliesList.Clear();
            foreach (int index in suppliesGrid.SelectedIndices)
            {
                selectedSuppliesList.Add((Supply)suppliesGrid.Rows[index].Tag);
            }

            if (IsSelectMode)
            {
                ListSelectedSupplies.Clear();//just in case
                for (int i = 0; i < suppliesGrid.SelectedIndices.Length; i++)
                {
                    ListSelectedSupplies.Add((Supply)suppliesGrid.Rows[suppliesGrid.SelectedIndices[i]].Tag);
                }
                SynchronizeAndClose();
                return;
            }

            Supply selectedSupply = (Supply)suppliesGrid.Rows[e.Row].Tag;
            long oldCategoryDefNum = selectedSupply.Category;

            using (var formSupplyEdit = new FormSupplyEdit())
            {
                formSupplyEdit.Supp = selectedSupply;
                formSupplyEdit.ListSupplier = suppliersList;

                if (formSupplyEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (formSupplyEdit.Supp == null)
                {
                    suppliesList.Remove(selectedSupply);
                }
                else if (selectedSupply.Category != oldCategoryDefNum)
                {//If category changed
                 //Send supply to the bottom the new category
                    suppliesList.Remove(selectedSupply);//Remove so we can reinsert where it needs to be.
                    selectedSupply.ItemOrder = suppliesList.FindAll(x => x.Category == selectedSupply.Category)
                        .Select(x => x.ItemOrder)
                        .OrderByDescending(x => x)
                        .FirstOrDefault() + 1;//Last item in the category.
                    int idx = suppliesList.FindLastIndex(x => x.Category == selectedSupply.Category);
                    if (idx == -1)
                    {
                        suppliesList.Add(selectedSupply);//new category, add to bottom of list
                    }
                    else
                    {
                        suppliesList.Insert(idx + 1, selectedSupply);//add to bottom of existing category
                    }
                }
            }


            int scroll = suppliesGrid.ScrollValue;
            FillSuppliesGrid();
            suppliesGrid.ScrollValue = scroll;
        }

        void UpButton_Click(object sender, EventArgs e)
        {
            if (suppliesGrid.SelectedIndices.Length == 0) return;

            selectedSuppliesList.Clear();
            foreach (int index in suppliesGrid.SelectedIndices)
            {
                selectedSuppliesList.Add((Supply)suppliesGrid.Rows[index].Tag);
            }

            //Loop through selected indices, moving each one as needed.
            for (int i = 0; i < suppliesGrid.SelectedIndices.Length; i++)
            {
                int indexSource = suppliesGrid.SelectedIndices[i];
                int indexDest = indexSource - 1;
                //Top of visible category-------------------------------------------------------------------------------
                if (indexSource == 0)
                {
                    continue;
                }
                Supply supplySource = (Supply)suppliesGrid.Rows[indexSource].Tag;
                Supply supplyDest = (Supply)suppliesGrid.Rows[indexDest].Tag;//Incorrect. 
                                                                             //Top of category---------------------------------------------------------------------------------------
                if (supplySource.Category != supplyDest.Category)
                {
                    continue;//already top of category
                }
                //Move the item up.  Change all item's item orders that it moved past (can be a filtered out result) and change its position in the global list.
                int sourceIdx = suppliesList.IndexOf(supplySource);//sourceIdx = higher entry
                int destIdx = suppliesList.IndexOf(supplyDest);//destIdx = lower entry
                suppliesList[sourceIdx] = supplyDest;
                suppliesList[destIdx] = supplySource;
                suppliesGrid.Rows[indexSource].Tag = supplyDest;//Fix up the tags for future movement.
                suppliesGrid.Rows[indexDest].Tag = supplySource;
                //Item orders are not set here, they are reconciled on OK click.
            }
            int scrollVal = suppliesGrid.ScrollValue;
            FillSuppliesGrid();
            suppliesGrid.ScrollValue = scrollVal;
        }

        void DownButton_Click(object sender, EventArgs e)
        {
            if (suppliesGrid.SelectedIndices.Length == 0) return;
            
            selectedSuppliesList.Clear();
            foreach (int index in suppliesGrid.SelectedIndices)
            {
                selectedSuppliesList.Add((Supply)suppliesGrid.Rows[index].Tag);
            }
            //Loop through selected indices, moving each one as needed.
            for (int i = suppliesGrid.SelectedIndices.Length - 1; i >= 0; i--)
            {
                int indexSource = suppliesGrid.SelectedIndices[i];//to reduce confusion
                int indexDest = indexSource + 1;
                //Bottom of visible category-------------------------------------------------------------------------------
                if (indexSource == suppliesGrid.Rows.Count - 1)
                {
                    continue;
                }
                Supply supplySource = (Supply)suppliesGrid.Rows[indexSource].Tag;
                Supply supplyDest = (Supply)suppliesGrid.Rows[indexDest].Tag;
                //Bottom of category---------------------------------------------------------------------------------------
                if (supplySource.Category != supplyDest.Category)
                {
                    continue;//already bottom of category
                }
                //Move the item down.  Change all item's item orders that it moved past (can be a filtered out result) and change its position in the global list.
                int sourceIdx = suppliesList.IndexOf(supplySource);//Grid Tags are references to this in-memory list, so this will work.
                int destIdx = suppliesList.IndexOf(supplyDest);
                suppliesList[sourceIdx] = supplyDest;
                suppliesList[destIdx] = supplySource;
                suppliesGrid.Rows[indexSource].Tag = supplyDest;//Fix up the tags for future movement.
                suppliesGrid.Rows[indexDest].Tag = supplySource;
                //Item orders are not set here, they are reconciled on OK click.
            }
            int scrollVal = suppliesGrid.ScrollValue;
            FillSuppliesGrid();
            suppliesGrid.ScrollValue = scrollVal;
        }

        /// <summary>
        /// Prints the supply list.
        /// </summary>
        void PrintButton_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Adds a new order with all the items currently showing in the grid as a new pending order.
        /// </summary>
        void AddToOrderButton_Click(object sender, EventArgs e)
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

            var listOrders = SupplyOrders.GetAll();

            for (int i = 0; i < listOrders.Count; i++)
            {
                if (listOrders[i].DatePlaced.Year > 2200 && listOrders[i].SupplierNum == suppliersList[supplierComboBox.SelectedIndex - 1].SupplierNum)
                {
                    MessageBox.Show(
                        Translation.Language.ThereIsAPendingOrderForThisSupplier,
                        Translation.Language.Supplies, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return;
                }
            }

            SynchronizeChanges();

            var supplyOrder = new SupplyOrder
            {
                SupplierNum = suppliersList[supplierComboBox.SelectedIndex - 1].SupplierNum,
                IsNew       = true,
                DatePlaced  = new DateTime(2500, 1, 1), //date used for new 'pending' orders. // TODO: DatePlaced should become nullable, where null == pending
                Note        = "",
                UserNum     = Security.CurUser.Id
            };

            var supplyOrderNum = SupplyOrders.Insert(supplyOrder);
            for (int i = 0; i < displayedSuppliesList.Count; i++)
            {
                var supplyOrderItem = new SupplyOrderItem
                {
                    SupplyNum       = displayedSuppliesList[i].SupplyNum,
                    Qty             = (int)displayedSuppliesList[i].LevelDesired - (int)displayedSuppliesList[i].LevelOnHand,
                    Price           = displayedSuppliesList[i].Price,
                    SupplyOrderNum  = supplyOrderNum
                };
                SupplyOrderItems.Insert(supplyOrderItem);
            }

            SupplyOrders.UpdateOrderPrice(supplyOrderNum);

            MessageBox.Show(
                Translation.Language.ItemsAddedToOrderSuccessfully,
                Translation.Language.Supplies, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }

        void PrintPage(object sender, PrintPageEventArgs e)
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
                    text = Translation.Language.Supplier + ": " + suppliersList[supplierComboBox.SelectedIndex - 1].Name;
                    g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                    yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                    if (suppliersList[supplierComboBox.SelectedIndex - 1].Phone != "")
                    {
                        text = Translation.Language.Phone + ": " + suppliersList[supplierComboBox.SelectedIndex - 1].Phone;
                        g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                        yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                    }
                    if (suppliersList[supplierComboBox.SelectedIndex - 1].Name != "")
                    {
                        text = Translation.Language.Note + ": " + suppliersList[supplierComboBox.SelectedIndex - 1].Name;
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

        /// <summary>
        /// Synchronizes all changes with the database and closes the form.
        /// </summary>
        void SynchronizeAndClose()
        {
            SynchronizeChanges();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Pushes supplies changes to the database.
        /// </summary>
        void SynchronizeChanges()
        {
            int itemOrder = 0;
            for (int i = 0; i < suppliesList.Count; i++)
            {
                if (i > 0 && suppliesList[i - 1].Category != suppliesList[i].Category)
                {
                    itemOrder = 0;
                }
                suppliesList[i].ItemOrder = itemOrder;
                itemOrder++;
            }
            //Nuances of concurency using this sync are, 
            //Deletes always win,
            //last in edits win,
            //Added supplies are unaffected by concurency
            List<Supply> listUpdated = Supplies.Sync(suppliesList, suppliesListOld);
            //SupplyNums will only be 0 if not using MiddleTier, since they get passed by reference and only MiddleTier breaks that reference.
            foreach (Supply supplyAdded in suppliesList.FindAll(x => x.SupplyNum == 0))
            {
                //Each supply is uniquely identified by ItemOrder and Category, because of the logic performed on the middle tier server side.
                //We can use this information to match primary keys.
                Supply supplyUpdated = listUpdated.FirstOrDefault(x => x.ItemOrder == supplyAdded.ItemOrder && x.Category == supplyAdded.Category);
                //This primary key copying is necessary for middle tier to transfer the key values to the local list from the server.
                //This pattern is unusual because of how supplies are added in the UI.
                supplyAdded.SupplyNum = supplyUpdated.SupplyNum;
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (IsSelectMode)
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

                ListSelectedSupplies.Clear();
                for (int i = 0; i < suppliesGrid.SelectedIndices.Length; i++)
                {
                    ListSelectedSupplies.Add((Supply)suppliesGrid.Rows[suppliesGrid.SelectedIndices[i]].Tag);
                }
            }

            SynchronizeAndClose();
        }
    }
}