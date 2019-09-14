using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplierEdit : FormBase
    {
        public Supplier Supp;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplierEdit"/> class.
        /// </summary>
        public FormSupplierEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplierEdit_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = Supp.Name;
            phoneTextBox.Text = Supp.Phone;
            customerIdTextBox.Text = Supp.CustomerId;
            textWebsite.Text = Supp.Website;
            userNameTextBox.Text = Supp.UserName;
            passwordTextBox.Text = Supp.Password;
            noteTextBox.Text = Supp.Note;
        }

        /// <summary>
        /// Deletes the supplier.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Supp.SupplierNum == 0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.Supplier, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            try
            {
                Suppliers.DeleteObject(Supp);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    Translation.Language.Supplier, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the supplier and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.EnterAName,
                    Translation.Language.Supplier, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            Supp.Name = name;
            Supp.Phone = phoneTextBox.Text;
            Supp.CustomerId = customerIdTextBox.Text;
            Supp.Website = textWebsite.Text;
            Supp.UserName = userNameTextBox.Text;
            Supp.Password = passwordTextBox.Text;
            Supp.Note = noteTextBox.Text;

            if (Supp.SupplierNum == 0)
            {
                Suppliers.Insert(Supp);
            }
            else
            {
                Suppliers.Update(Supp);
            }

            DialogResult = DialogResult.OK;
        }
    }
}