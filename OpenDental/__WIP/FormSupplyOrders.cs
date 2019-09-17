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
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyOrders : FormBase
    {
        private List<Supplier> suppliers;
        private List<SupplyOrder> allOrders;
        private List<SupplyOrder> orders;
        private DataTable orderItemsTable;
        private int pagesPrinted;
        private bool headingPrinted;
        private int headingPrintHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrders"/> class.
        /// </summary>
        public FormSupplyOrders() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrders_Load(object sender, EventArgs e)
        {
            allOrders = SupplyOrder.All();
            orders = new List<SupplyOrder>();

            LoadSuppliers();
            LoadOrders();

            ordersGrid.ScrollToEnd();
        }

        /// <summary>
        /// Loads the list of the suppliers and populates the combobox.
        /// </summary>
        void LoadSuppliers()
        {
            suppliers = Supplier.All();

            supplierComboBox.Items.Clear();
            supplierComboBox.Items.Add(Translation.Language.All);
            supplierComboBox.SelectedIndex = 0;

            for (int i = 0; i < suppliers.Count; i++)
            {
                supplierComboBox.Items.Add(suppliers[i].Name);
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

            foreach (var order in orders)
            {
                var row = new ODGridRow();

                row.Cells.Add(order.DatePlaced?.ToShortDateString() ?? Translation.Language.Pending.ToLower());
                row.Cells.Add(order.AmountTotal.ToString("c"));
                row.Cells.Add(order.ShippingCharge.ToString("c"));
                row.Cells.Add(Supplier.GetName(suppliers, order.SupplierId));
                row.Cells.Add(order.Note);
                row.Cells.Add(order.UserId.HasValue ? User.GetName(order.UserId.Value) : "");
                row.Tag = order;

                ordersGrid.Rows.Add(row);
            }
            ordersGrid.EndUpdate();
        }

        /// <summary>
        /// Loads the list of order items and populates the order items grid.
        /// </summary>
        void LoadOrderItems()
        {
            long orderId = 0;
            if (ordersGrid.GetSelectedIndex() != -1)
            {
                orderId = orders[ordersGrid.GetSelectedIndex()].Id;
            }

            orderItemsTable = SupplyOrderItem.GetOrderLines(orderId);

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
                int.TryParse(orderItemsTable.Rows[i]["quantity"].ToString(), out int qty);
                double.TryParse(orderItemsTable.Rows[i]["price"].ToString(), out double price);

                var row = new ODGridRow();
                row.Cells.Add(orderItemsTable.Rows[i]["catalog_number"].ToString());
                row.Cells.Add(orderItemsTable.Rows[i]["description"].ToString());
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
            orders.Clear();

            long supplierNum = (supplierComboBox.SelectedIndex > 0) ? suppliers[supplierComboBox.SelectedIndex - 1].Id : 0;

            foreach (var supplyOrder in allOrders)
            {
                if (supplierNum == 0)
                {
                    orders.Add(supplyOrder);
                }
                else if (supplyOrder.SupplierId == supplierNum)
                {
                    orders.Add(supplyOrder);
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

            foreach (var order in orders)
            {
                if (!order.DatePlaced.HasValue)
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
                supplyOrder.SupplierId = suppliers[supplierComboBox.SelectedIndex - 1].Id;//SelectedIndex-1 because "All" is first option.
            }

            supplyOrder.DatePlaced = new DateTime(2500, 1, 1);
            supplyOrder.Note = "";
            supplyOrder.UserId = 0;

            SupplyOrder.Insert(supplyOrder);

            allOrders = SupplyOrder.All();

            LoadOrders();

            ordersGrid.SetSelected(orders.Count - 1, true);
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
                formSupplyOrderEdit.Suppliers = suppliers;
                formSupplyOrderEdit.Order = orders[e.Row];

                if (formSupplyOrderEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                allOrders = SupplyOrder.All();

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
                formSupplies.SelectedSupplierId = orders[ordersGrid.GetSelectedIndex()].SupplierId;
                if (formSupplies.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                foreach (var supply in formSupplies.SelectedSupplies)
                {	
                    if (orderItemsTable.Rows.OfType<DataRow>().Any(x => (long)x["supply_id"] == supply.Id))
                    {
                        continue;
                    }

                    var supplyOrderItem = new SupplyOrderItem
                    {
                        SupplyId = supply.Id,
                        SupplyOrderId = orders[ordersGrid.GetSelectedIndex()].Id,
                        Quantity = 1,
                        Price = supply.Price
                    };

                    SupplyOrderItem.Insert(supplyOrderItem);
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
                formSupplyOrderItemEdit.SupplyOrderItem = SupplyOrderItem.GetById((long)orderItemsTable.Rows[e.Row]["supply_order_item_id"]);
                formSupplyOrderItemEdit.Suppliers = Supplier.All();

                if (formSupplyOrderItemEdit.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                SupplyOrderItem.Update(formSupplyOrderItemEdit.SupplyOrderItem);

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

            var supplyOrderItem = SupplyOrderItem.GetById((long)orderItemsTable.Rows[e.Row]["supply_order_item_id"]);
            supplyOrderItem.Quantity = qtyNew;
            supplyOrderItem.Price = priceNew;

            SupplyOrderItem.Update(supplyOrderItem);
            SupplyOrder.UpdateOrderPrice(supplyOrderItem.SupplyOrderId);

            orderItemsGrid.Rows[e.Row].Cells[2].Text = qtyNew.ToString();
            orderItemsGrid.Rows[e.Row].Cells[3].Text = priceNew.ToString("n");
            orderItemsGrid.Rows[e.Row].Cells[4].Text = (qtyNew * priceNew).ToString("n");
            orderItemsGrid.Invalidate();

            int selectedIndex = ordersGrid.GetSelectedIndex();
            allOrders = SupplyOrder.All();

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
                    orders[ordersGrid.GetSelectedIndex()].DatePlaced?.ToShortDateString() ?? "N/A"),
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
                text = Translation.Language.OrderNumber + ": " + orders[ordersGrid.SelectedIndices[0]].Id;
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                text = Translation.Language.Date + ": " + orders[ordersGrid.SelectedIndices[0]].DatePlaced?.ToShortDateString() ?? "N/A";
                g.DrawString(text, subHeadingFont, Brushes.Black, 425 - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, subHeadingFont).Height;
                Supplier supCur = Supplier.GetById(orders[ordersGrid.SelectedIndices[0]].SupplierId);
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
            SupplyOrder.UpdateOrderPrice(orders[ordersGrid.GetSelectedIndex()].Id);

            allOrders = SupplyOrder.All();

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
