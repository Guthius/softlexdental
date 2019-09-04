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
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountingAutoPayEdit : FormBase
    {
        private readonly List<long> accountIds = new List<long>();

        public AccountAutoPay AutoPay { get; set; }

        public FormAccountingAutoPayEdit() => InitializeComponent();

        private void FormAccountingAutoPayEdit_Load(object sender, EventArgs e)
        {
            var paymentTypes = Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            foreach (var paymentType in paymentTypes)
            {
                paymentTypeComboBox.Items.Add(paymentType);
                if (paymentType.Id == AutoPay.PayType)
                {
                    paymentTypeComboBox.SelectedItem = paymentType;
                }
            }

            if (AutoPay.AccountIds == null) AutoPay.AccountIds = "";

            var tokens = AutoPay.AccountIds.Split(',');
            foreach (var token in tokens)
            {
                if (!string.IsNullOrEmpty(token) && long.TryParse(token, out var accountId))
                {
                    accountIds.Add(accountId);
                }
            }

            LoadAccounts();
        }

        private void LoadAccounts()
        {
            accountsListBox.Items.Clear();
            for (int i = 0; i < accountIds.Count; i++)
            {
                accountsListBox.Items.Add(Account.GetDescription(accountIds[i]));
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var formAccountPick = new FormAccountPick())
            {
                if (formAccountPick.ShowDialog() == DialogResult.OK)
                {
                    accountIds.Add(formAccountPick.SelectedAccount.Id);

                    LoadAccounts();
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (accountsListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Auto Pay Entry", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            accountIds.RemoveAt(accountsListBox.SelectedIndex);

            LoadAccounts();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (AutoPay.Id != 0)
            {
                AccountAutoPay.Delete(AutoPay.Id);
            }

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (paymentTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a pay type first.",
                    "Auto Pay Entry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (accountIds.Count == 0)
            {
                MessageBox.Show(
                    "Please add at least one account to the pick list first.",
                    "Auto Pay Entry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (paymentTypeComboBox.SelectedItem is Definition paymentType)
            {
                AutoPay.PayType = paymentType.Id;
                AutoPay.AccountIds = string.Join(",", accountIds);

                DialogResult = DialogResult.OK;
            }
        }
    }
}
