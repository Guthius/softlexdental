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
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyOrderEdit : FormBase
    {
        public SupplyOrder Order { get; set; }

        public List<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrderEdit"/> class.
        /// </summary>
        public FormSupplyOrderEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrderEdit_Load(object sender, EventArgs e)
        {
            supplierTextBox.Text = Supplier.GetName(Suppliers, Order.SupplierId);

            if (!Order.DatePlaced.HasValue)
            {
                datePlacedTextBox.Text = DateTime.Today.ToShortDateString();

                Order.UserId = Security.CurrentUser.Id;
            }
            else
            {
                datePlacedTextBox.Text = Order.DatePlaced.Value.ToShortDateString();
            }

            totalTextBox.Value = Order.AmountTotal;
            shippingChargeTextBox.Value = Order.ShippingCharge;
            noteTextBox.Text = Order.Note;

            userComboBox.Items.Clear();
            userComboBox.Items.Add(Translation.Language.None);

            var users = User.All().Where(x => !x.Hidden);
            foreach (var user in users)
            {
                var userBoxItem = new ODBoxItem<User>(user.UserName, user);

                userComboBox.Items.Add(userBoxItem);
                if (Order.UserId == user.Id)
                {
                    userComboBox.SelectedItem = userBoxItem;
                }
            }

            if (userComboBox.SelectedIndex == -1 && 
                userComboBox.Items.Count > 0)
            {
                userComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Deletes the order.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (datePlacedTextBox.Text != "")
            {
                MessageBox.Show(
                    Translation.Language.NotAllowedToDeleteUnlessDateIsBlank,
                    Translation.Language.SupplyOrder, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.SupplyOrder, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            SupplyOrder.Delete(Order);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the order and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (datePlacedTextBox.errorProvider1.GetError(datePlacedTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.SupplyOrder,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (datePlacedTextBox.Text == "")
            {
                Order.DatePlaced = null;
            }
            else
            {
                Order.DatePlaced = DateTime.Parse(datePlacedTextBox.Text);
                if (userComboBox.SelectedIndex > -1)
                {
                    Order.UserId = userComboBox.SelectedTag<User>().Id;
                }
            }

            Order.AmountTotal = totalTextBox.Value;
            Order.Note = noteTextBox.Text;
            Order.ShippingCharge = shippingChargeTextBox.Value;

            SupplyOrder.Update(Order);

            DialogResult = DialogResult.OK;
        }
    }
}