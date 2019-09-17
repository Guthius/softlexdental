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
    public partial class FormSupplyOrders : FormBase
    {
        List<Supplier> suppliersList;
        List<SupplyOrder> allOrdersList;
        List<SupplyOrder> ordersList;
        DataTable orderItemsTable;
        int pagesPrinted;
        bool headingPrinted;
        int headingPrintHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrders"/> class.
        /// </summary>
        public FormSupplyOrders() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrders_Load(object sender, EventArgs e)
        {
            allOrdersList = SupplyOrders.GetAll();
            ordersList = new List<SupplyOrder>();

            LoadSuppliers();
            LoadOrders();

            ordersGrid.ScrollToEnd();
        }

        /// <summary>
        /// Loads the list of the suppliers and populates the combobox.
        /// </summary>
        void LoadSuppliers()
        {
            suppliersList = Suppliers.GetAll();

            supplierComboBox.Items.Clear();
            supplierComboBox.Items.Add(Translation.Language.All);
            supplierComboBox.SelectedIndex = 0;

            for (int i = 0; i < suppliersList.Count; i++)
            {
                supplierComboBox.Items.Add(suppliersList[i].Name);
            }
        }

        /// <summary>
        /// Loads the list of orders and populates the orders grid.
        /// </summary>
        void LoadOrders()
        {
            FilterListOrder();

            ordersGrid.BeginUpdate();
            ordersGrid.Columns.Clear();
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDatePlaced, 80));
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnAmount, 70, HorizontalAlignment.Right));
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnShipping, 70, HorizontalAlignment.Right));
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSupplier, 120));
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnNote, 200));
            ordersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPlacedBy, 100));
            ordersGrid.Rows.Clear();

            for (int i = 0; i < ordersList.Count; i++)
            {
                var row = new ODGridRow();

                bool isPending = ordersList[i].DatePlaced.Year > 2200;

                if (isPending)
                {
                    row.Cells.Add(Translation.Language.Pending.ToLower());
                }
                else
                {
                    row.Cells.Add(ordersList[i].DatePlaced.ToShortDateString());
                }

                row.Cells.Add(ordersList[i].AmountTotal.ToString("c"));
                row.Cells.Add(ordersList[i].ShippingCharge.ToString("c"));
                row.Cells.Add(Suppliers.GetName(suppliersList, ordersList[i].SupplierId));
                row.Cells.Add(ordersList[i].Note);

                if (isPending || ordersList[i].UserId == 0)
                {
                    row.Cells.Add("");
                }
                else
                {
                    row.Cells.Add(User.GetName(ordersList[i].UserId));
                }

                row.Tag = ordersList[i];

                ordersGrid.Rows.Add(row);
            }
            ordersGrid.EndUpdate();
        }

        /// <summary>
        /// Loads the list of order items and populates the order items grid.
        /// </summary>
        void LoadOrderItems()
        {
            long orderNum = 0;
            if (ordersGrid.GetSelectedIndex() != -1)
            {
                orderNum = ordersList[ordersGrid.GetSelectedIndex()].Id;
            }

            orderItemsTable = SupplyOrderItems.GetItemsForOrder(orderNum);

            orderItemsGrid.BeginUpdate();
            orderItemsGrid.Columns.Clear();
            orderItemsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnCatalogNumber, 80));
            orderItemsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 320));
            orderItemsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnQty, 60, textAlignment: HorizontalAlignment.Center, isEditable: true));
            orderItemsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPricePerUnit, 70, textAlignment: HorizontalAlignment.Right, isEditable: true));
            orderItemsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSubtotal, 70, HorizontalAlignment.Right));
            orderItemsGrid.Rows.Clear();

            for (int i = 0; i < orderItemsTable.Rows.Count; i++)
            {
                int.TryParse(orderItemsTable.Rows[i]["Qty"].ToString(), out int qty);
                double.TryParse(orderItemsTable.Rows[i]["Price"].ToString(), out double price);

                var row = new ODGridRow();
                row.Cells.Add(orderItemsTable.Rows[i]["CatalogNumber"].ToString());
                row.Cells.Add(orderItemsTable.Rows[i]["Descript"].ToString());
                row.Cells.Add(qty.ToString());
                row.Cells.Add(price.ToString("n"));
                row.Cells.Add((qty * price).ToString("n"));

                orderItemsGrid.Rows.Add(row);
            }

            orderItemsGrid.EndUpdate();
        }

        /// <summary>
        /// Filters the list of orders using the current filters.
        /// </summary>
        void FilterListOrder()
        {
            ordersList.Clear();

            long supplierNum = (supplierComboBox.SelectedIndex > 0) ? suppliersList[supplierComboBox.SelectedIndex - 1].Id : 0;

            foreach (var supplyOrder in allOrdersList)
            {
                if (supplierNum == 0)
                {
                    ordersList.Add(supplyOrder);
                }
                else if (supplyOrder.SupplierId == supplierNum)
                {
                    ordersList.Add(supplyOrder);
                }
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        void NewOrderButton_Click(object sender, EventArgs e)
        {
            if (supplierComboBox.SelectedIndex < 1)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectASupplierFirst,
                    Translation.Language.SupplyOrders,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            for (int i = 0; i < ordersList.Count; i++)
            {
                if (ordersList[i].DatePlaced.Year > 2200)
                {
                    MessageBox.Show(
                        Translation.Language.NotAllowedToAddNewOrderWhenOneIsPending,
                        Translation.Language.SupplyOrders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }
            }

            var supplyOrder = new SupplyOrder();
            if (supplierComboBox.SelectedIndex == 0)
            {
                supplyOrder.SupplierId = 0;
            }
            else
            {
                supplyOrder.SupplierId = suppliersList[supplierComboBox.SelectedIndex - 1].Id;//SelectedIndex-1 because "All" is first option.
            }
            supplyOrder.IsNew = true;
            supplyOrder.DatePlaced = new DateTime(2500, 1, 1);
            supplyOrder.Note = "";
            supplyOrder.UserId = 0;

            SupplyOrders.Insert(supplyOrder);

            allOrdersList = SupplyOrders.GetAll();

            LoadOrders();

            ordersGrid.SetSelected(ordersList.Count - 1, true);
            ordersGrid.ScrollToEnd();

            LoadOrderItems();
        }

        /// <summary>
        /// Reload the orders and order items when the selected supplier is changed.
        /// </summary>
        void SupplierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders();

            ordersGrid.ScrollToEnd();

            LoadOrderItems();
        }

        /// <summary>
        /// Load the items of a order when a order is selected from the orders grid.
        /// </summary>
        void OrdersGrid_CellClick(object sender, ODGridClickEventArgs e) => LoadOrderItems();

        /// <summary>
        /// Opens the form to edit a order.
        /// </summary>
        void OrdersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplyOrderEdit = new FormSupplyOrderEdit())
            {
                formSupplyOrderEdit.ListSupplier = suppliersList;
                formSupplyOrderEdit.Order = ordersList[e.Row];
                if (formSupplyOrderEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                allOrdersList = SupplyOrders.GetAll();

                LoadOrders();
                LoadOrderItems();
            }
        }

        /// <summary>
        /// Opens the form to add a new item to the selected order.
        /// </summary>
        void AddOrderItemButton_Click(object sender, EventArgs e)
        {
            if (ordersGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectASupplyOrderToAddItemsTo,
                    Translation.Language.SupplyOrders, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            using (var formSupplies = new FormSupplies())
            {
                formSupplies.IsSelectionMode = true;
                formSupplies.SelectedSupplierId = ordersList[ordersGrid.GetSelectedIndex()].SupplierId;
                if (formSupplies.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                for (int i = 0; i < formSupplies.SelectedSupplies.Count; i++)
                {	
                    if (orderItemsTable.Rows.OfType<DataRow>().Any(x => PIn.Long(x["SupplyNum"].ToString()) == formSupplies.SelectedSupplies[i].Id))
                    {
                        continue;
                    }

                    var supplyOrderItem = new SupplyOrderItem
                    {
                        SupplyId       = formSupplies.SelectedSupplies[i].Id,
                        Quantity             = 1,
                        Price           = formSupplies.SelectedSupplies[i].Price,
                        SupplyOrderId  = ordersList[ordersGrid.GetSelectedIndex()].Id
                    };

                    SupplyOrderItems.Insert(supplyOrderItem);
                }

                UpdatePriceAndRefresh();
            }
        }

        /// <summary>
        /// Opens the form to edit a order item.
        /// </summary>
        void OrderItemsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplyOrderItemEdit = new FormSupplyOrderItemEdit())
            {
                formSupplyOrderItemEdit.ItemCur = SupplyOrderItems.CreateObject(PIn.Long(orderItemsTable.Rows[e.Row]["SupplyOrderItemNum"].ToString()));
                formSupplyOrderItemEdit.ListSupplier = Suppliers.GetAll();

                if (formSupplyOrderItemEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                SupplyOrderItems.Update(formSupplyOrderItemEdit.ItemCur);

                UpdatePriceAndRefresh();
            }
        }

        /// <summary>
        /// Update the quantity, price and total columns of the order items grid when the quantity
        /// or price is modified and reload the orders list.
        /// </summary>
        void OrderItemsGrid_CellLeave(object sender, ODGridClickEventArgs e)
        {
            int.TryParse(orderItemsGrid.Rows[e.Row].Cells[2].Text, out int qtyNew);
            double.TryParse(orderItemsGrid.Rows[e.Row].Cells[3].Text, out double priceNew);

            var supplyOrderItem = SupplyOrderItems.CreateObject(PIn.Long(orderItemsTable.Rows[e.Row]["SupplyOrderItemNum"].ToString()));
            supplyOrderItem.Qty = qtyNew;
            supplyOrderItem.Price = priceNew;

            SupplyOrderItems.Update(supplyOrderItem);
            SupplyOrders.UpdateOrderPrice(supplyOrderItem.SupplyOrderId);

            orderItemsGrid.Rows[e.Row].Cells[2].Text = qtyNew.ToString();
            orderItemsGrid.Rows[e.Row].Cells[3].Text = priceNew.ToString("n");
            orderItemsGrid.Rows[e.Row].Cells[4].Text = (qtyNew * priceNew).ToString("n");
            orderItemsGrid.Invalidate();

            int selectedIndex = ordersGrid.GetSelectedIndex();
            allOrdersList = SupplyOrders.GetAll();

            LoadOrders();

            ordersGrid.SetSelected(selectedIndex, true);
        }

        /// <summary>
        /// Prints the list of items in the selected order.
        /// </summary>
        void PrintButton_Click(object sender, EventArgs e)
        {
            if (orderItemsTable.Rows.Count < 1)
            {
                MessageBox.Show(
                    Translation.Language.SupplyListEmpty,
                    Translation.Language.SupplyOrders,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(
                PrintPage,
                string.Format(
                    Translation.LanguageSecurity.SuppliesOrderFromDatePrinted, 
                    ordersList[ordersGrid.GetSelectedIndex()].DatePlaced.ToShortDateString()),
                margins: new Margins(50, 50, 40, 30)
            );
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
                text = Translation.Language.OrderNumber + ": " + ordersList[ordersGrid.SelectedIndices[0]].Id;
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                text = Translation.Language.Date + ": " + ordersList[ordersGrid.SelectedIndices[0]].DatePlaced.ToShortDateString();
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                Supplier supCur = Suppliers.GetOne(ordersList[ordersGrid.SelectedIndices[0]].SupplierId);
                text = supCur.Name;
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                text = supCur.Phone;
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                text = supCur.Note;
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                yPos += 15;
                headingPrinted = true;
                headingPrintHeight = yPos;
            }

            yPos = orderItemsGrid.PrintPage(g, pagesPrinted, bounds, headingPrintHeight);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// Updates the price of the selected order and reload the data.
        /// </summary>
        void UpdatePriceAndRefresh()
        {
            var supplyOrder = ordersGrid.SelectedTag<SupplyOrder>();
            SupplyOrders.UpdateOrderPrice(ordersList[ordersGrid.GetSelectedIndex()].Id);

            allOrdersList = SupplyOrders.GetAll();

            LoadOrders();

            for (int i = 0; i < ordersGrid.Rows.Count; i++)
            {
                if (supplyOrder != null && ((SupplyOrder)ordersGrid.Rows[i].Tag).Id == supplyOrder.Id)
                {
                    ordersGrid.SetSelected(i, true);
                }
            }

            LoadOrderItems();
        }
    }
}