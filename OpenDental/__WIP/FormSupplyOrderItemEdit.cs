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
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyOrderItemEdit : FormBase
    {
        private Supply supply;
        private int quantity;

        public SupplyOrderItem SupplyOrderItem { get; set; }

        public List<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrderItemEdit"/> class.
        /// </summary>
        public FormSupplyOrderItemEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrderItemEdit_Load(object sender, EventArgs e)
        {
            supply = Supply.GetById(SupplyOrderItem.SupplyId);

            supplierTextBox.Text = Supplier.GetName(Suppliers, supply.SupplierId);
            categoryTextBox.Text = Defs.GetName(DefinitionCategory.SupplyCats, supply.CategoryId);
            catalogNumberTextBox.Text = supply.CatalogNumber;
            descriptionTextBox.Text = supply.Description;
            quantityTextBox.Text = (quantity = SupplyOrderItem.Quantity).ToString();
            priceTextBox.Value = SupplyOrderItem.Price;
        }

        /// <summary>
        /// Recalculates the value of the subtotal textbox.
        /// </summary>
        void UpdateSubTotal()
        {
            if (int.TryParse(quantityTextBox.Text, out int quantity) && double.TryParse(priceTextBox.Text, out double price))
            {
                subtotalTextBox.Text = (quantity * price).ToString("n");
            }
        }

        /// <summary>
        /// Only allow digits to be entered in the quantity textbox.
        /// </summary>
        void QuantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Updates the subtotal textbox when the quantity changes.
        /// </summary>
        void QuantityTextBox_TextChanged(object sender, EventArgs e) => UpdateSubTotal();

        /// <summary>
        /// Validates the value entered in the quantity textbox.
        /// </summary>
        void QuantityTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(quantityTextBox.Text))
            {
                if (int.TryParse(quantityTextBox.Text, out int result))
                {
                    quantity = result;
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                quantity = 0;
            }
            quantityTextBox.Text = quantity.ToString();
        }

        /// <summary>
        /// Deletes the supply order item.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.SupplyOrderItem, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            SupplyOrderItem.Delete(SupplyOrderItem);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the supply order item and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (quantityTextBox.errorProvider1.GetError(quantityTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.SupplyOrderItem, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            SupplyOrderItem.Quantity = quantity;
            SupplyOrderItem.Price = priceTextBox.Value;

            SupplyOrderItem.Update(SupplyOrderItem);

            DialogResult = DialogResult.OK;
        }
    }
}
