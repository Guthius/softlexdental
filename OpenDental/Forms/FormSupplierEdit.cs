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
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplierEdit : FormBase
    {
        /// <summary>
        /// Gets or sets the supplier.
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplierEdit"/> class.
        /// </summary>
        public FormSupplierEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplierEdit_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = Supplier.Name;
            phoneTextBox.Text = Supplier.Phone;
            customerIdTextBox.Text = Supplier.CustomerId;
            textWebsite.Text = Supplier.Website;
            userNameTextBox.Text = Supplier.UserName;
            passwordTextBox.Text = Supplier.Password;
            noteTextBox.Text = Supplier.Note;
        }

        /// <summary>
        /// Deletes the supplier.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Supplier.Id == 0)
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
                Supplier.Delete(Supplier);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
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

            Supplier.Name = name;
            Supplier.Phone = phoneTextBox.Text;
            Supplier.CustomerId = customerIdTextBox.Text;
            Supplier.Website = textWebsite.Text;
            Supplier.UserName = userNameTextBox.Text;
            Supplier.Password = passwordTextBox.Text;
            Supplier.Note = noteTextBox.Text;

            if (Supplier.Id == 0)
            {
                Supplier.Insert(Supplier);
            }
            else
            {
                Supplier.Update(Supplier);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
