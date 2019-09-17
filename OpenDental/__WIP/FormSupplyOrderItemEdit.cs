using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyOrderItemEdit : FormBase
    {
        Supply supply;
        int quantity;

        public SupplyOrderItem ItemCur;
        public List<Supplier> ListSupplier;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrderItemEdit"/> class.
        /// </summary>
        public FormSupplyOrderItemEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrderItemEdit_Load(object sender, EventArgs e)
        {
            supply = Supplies.GetSupply(ItemCur.SupplyId);

            supplierTextBox.Text = Suppliers.GetName(ListSupplier, supply.SupplierId);
            categoryTextBox.Text = Defs.GetName(DefinitionCategory.SupplyCats, supply.CategoryId);
            catalogNumberTextBox.Text = supply.CatalogNumber;
            descriptionTextBox.Text = supply.Description;
            quantityTextBox.Text = (quantity = ItemCur.Quantity).ToString();
            priceTextBox.Value = ItemCur.Price;
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

            SupplyOrderItems.DeleteObject(ItemCur);

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

            ItemCur.Quantity = quantity;
            ItemCur.Price = priceTextBox.Value;

            SupplyOrderItems.Update(ItemCur);

            DialogResult = DialogResult.OK;
        }
    }
}
