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
        public SupplyOrder Order;
        public List<Supplier> ListSupplier;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyOrderEdit"/> class.
        /// </summary>
        public FormSupplyOrderEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyOrderEdit_Load(object sender, EventArgs e)
        {
            supplierTextBox.Text = Suppliers.GetName(ListSupplier, Order.SupplierNum);

            if (Order.DatePlaced.Year > 2200)
            {
                datePlacedTextBox.Text = DateTime.Today.ToShortDateString();
                Order.UserNum = Security.CurUser.UserNum;
            }
            else
            {
                datePlacedTextBox.Text = Order.DatePlaced.ToShortDateString();
            }

            totalTextBox.Value = Order.AmountTotal;
            shippingChargeTextBox.Value = Order.ShippingCharge;
            noteTextBox.Text = Order.Note;

            userComboBox.Items.Clear();
            userComboBox.Items.Add(new ODBoxItem<User>(Translation.Language.None, new User { UserNum = 0 }));

            var usersList = Userods.GetUsers().FindAll(x => !x.IsHidden);
            foreach (var user in usersList)
            {
                var userBoxItem = new ODBoxItem<User>(user.UserName, user);

                userComboBox.Items.Add(userBoxItem);
                if (Order.UserNum == user.UserNum)
                {
                    userComboBox.SelectedItem = userBoxItem;
                }
            }

            if (!usersList.Select(x => x.UserNum).Contains(Order.UserNum))
            {
                userComboBox.IndexSelectOrSetText(-1, () =>
                {
                    return Userods.GetName(Order.UserNum);
                });
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

            SupplyOrders.DeleteObject(Order);

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
                Order.DatePlaced = new DateTime(2500, 1, 1);
                Order.UserNum = 0; // Even if they had set a user, set it back because the order hasn't been placed. 
            }
            else
            {
                Order.DatePlaced = DateTime.Parse(datePlacedTextBox.Text);
                if (userComboBox.SelectedIndex > -1)
                {
                    Order.UserNum = userComboBox.SelectedTag<User>().UserNum;
                }
            }

            Order.AmountTotal = totalTextBox.Value;
            Order.Note = noteTextBox.Text;
            Order.ShippingCharge = shippingChargeTextBox.Value;

            SupplyOrders.Update(Order);

            DialogResult = DialogResult.OK;
        }
    }
}