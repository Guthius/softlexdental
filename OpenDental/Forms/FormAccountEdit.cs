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
    public partial class FormAccountEdit : FormBase
    {
        private readonly Account account;

        public FormAccountEdit(Account account)
        {
            InitializeComponent();

            this.account = account;
        }

        void FormAccountEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = account.Description;
            bankNumberTextBox.Text = account.BankNumber;
            inactiveCheckBox.Checked = account.Inactive;
            colorButton.BackColor = account.Color;

            for (int i = 0; i < Enum.GetNames(typeof(AccountType)).Length; i++)
            {
                typeListBox.Items.Add(Enum.GetNames(typeof(AccountType))[i]);
                if ((int)account.Type == i)
                {
                    typeListBox.SelectedIndex = i;
                }
            }
        }

        void ColorButton_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = colorButton.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = colorDialog.Color;
                }
            }
        }

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (account.Id == 0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            try
            {
                Account.Delete(account);
            }
            catch (DataException exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Translation.Language.Account, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.Account,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (account.Description != description)
            {
                var result =
                    MessageBox.Show(
                        "This will update the Splits column for all Transactions attached to this account that have a date after the Accounting Lock Date. Are you sure you want to continue?",
                        Translation.Language.Account, 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            account.Description = description;
            account.Type = (AccountType)typeListBox.SelectedIndex;
            account.BankNumber = bankNumberTextBox.Text;
            account.Inactive = inactiveCheckBox.Checked;
            account.Color = colorButton.BackColor;

            if (account.IsNew)
            {
                Account.Insert(account);
            }
            else
            {
                Account.Update(account);
            }

            DialogResult = DialogResult.OK;
        }
    }
}