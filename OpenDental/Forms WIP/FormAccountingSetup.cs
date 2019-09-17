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
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountingSetup : FormBase
    {
        private readonly List<long> depositAccountIds = new List<long>();
        private List<AccountAutoPay> autoPayList;
        private long accountingIncomeAccountId;
        private long accountingCashIncomeAccount;

        public FormAccountingSetup() => InitializeComponent();

        private void FormAccountingSetup_Load(object sender, EventArgs e)
        {
            LoadSoftlexDentalPreferences();
            LoadQuickBooksPreferences();

            softwareComboBox.SelectedIndex = Preference.GetInt(PreferenceName.AccountingSoftware, (int)AccountingSoftware.OpenDental);
        }

        private void SoftwareComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch ((AccountingSoftware)softwareComboBox.SelectedIndex)
            {
                case AccountingSoftware.OpenDental:
                    accountingTabControl.TabPages.Remove(quickBooksTabPage);
                    accountingTabControl.TabPages.Add(softlexDentalTabPage);
                    break;

                case AccountingSoftware.QuickBooks:
                    accountingTabControl.TabPages.Remove(softlexDentalTabPage);
                    accountingTabControl.TabPages.Add(quickBooksTabPage);
                    break;
            }
        }

        private void AutoPayGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formAccountingAutoPayEdit = new FormAccountingAutoPayEdit())
            {
                // TODO: Fix this...
                formAccountingAutoPayEdit.AutoPay = autoPayList[e.Row];
                formAccountingAutoPayEdit.ShowDialog();
                if (formAccountingAutoPayEdit.AutoPay == null)
                {
                    autoPayList.RemoveAt(e.Row);
                }

                LoadAutoPays();
            }
        }

        private void AutoPayAddButton_Click(object sender, EventArgs e)
        {
            var autoPay = new AccountAutoPay();

            using (var formAccountingAutoPayEdit = new FormAccountingAutoPayEdit())
            {
                formAccountingAutoPayEdit.AutoPay = autoPay;

                if (formAccountingAutoPayEdit.ShowDialog() == DialogResult.OK)
                {
                    autoPayList.Add(autoPay);

                    LoadAutoPays();
                }
                
                // TODO: Fix this...
            }
        }

        private void ChangeIncomeAccountButton_Click(object sender, EventArgs e)
        {
            using (var formAccountPick = new FormAccountPick())
            {
                if (formAccountPick.ShowDialog() == DialogResult.OK)
                {
                    accountingCashIncomeAccount = formAccountPick.SelectedAccount.Id;

                    accountTextBox.Text = Account.GetDescription(accountingCashIncomeAccount);
                }
            }
        }

        private void LoadSoftlexDentalPreferences()
        {
            LoadAutoPays();

            accountingCashIncomeAccount = Preference.GetLong(PreferenceName.AccountingCashIncomeAccount);
            accountTextBox.Text = Account.GetDescription(accountingCashIncomeAccount);

            var tokens = Preference.GetString(PreferenceName.AccountingDepositAccounts).Split(',');
            foreach (var token in tokens)
            {
                if (!string.IsNullOrEmpty(token) && long.TryParse(token, out var accountId))
                {
                    depositAccountIds.Add(accountId);
                }
            }

            accountingIncomeAccountId = Preference.GetLong(PreferenceName.AccountingIncomeAccount);
            depositAccountTextBox.Text = Account.GetDescription(accountingIncomeAccountId);

            LoadDepositAccounts();
        }

        private void LoadAutoPays()
        {
            autoPayList = AccountAutoPay.All();

            autoPayGrid.BeginUpdate();
            autoPayGrid.Columns.Clear();
            autoPayGrid.Columns.Add(new ODGridColumn("Payment Type", 200));
            autoPayGrid.Columns.Add(new ODGridColumn("Pick List", 250));
            autoPayGrid.Rows.Clear();

            foreach (var autoPay in autoPayList)
            {
                var row = new ODGridRow();
                row.Cells.Add(Defs.GetName(DefinitionCategory.PaymentTypes, autoPay.PayType));
                row.Cells.Add(AccountAutoPay.GetPickListDescription(autoPay));
                autoPayGrid.Rows.Add(row);
            }

            autoPayGrid.EndUpdate();
        }

        private bool SaveSoftlexDentalPreferences()
        {
            bool prefsChanged =
                Preference.Update(PreferenceName.AccountingDepositAccounts, string.Join(",", depositAccountIds)) |
                Preference.Update(PreferenceName.AccountingIncomeAccount, accountingIncomeAccountId) |
                Preference.Update(PreferenceName.AccountingCashIncomeAccount, accountingCashIncomeAccount);

            AccountAutoPay.SaveList(autoPayList);

            DataValid.SetInvalid(InvalidType.AccountingAutoPays);

            return prefsChanged;
        }

        private void LoadDepositAccounts()
        {
            depositAccountsListBox.Items.Clear();
            foreach (var depositAccountId in depositAccountIds)
            {
                depositAccountsListBox.Items.Add(Account.GetDescription(depositAccountId));
            }
        }

        private void AccountDepositAddButton_Click(object sender, EventArgs e)
        {
            using (var formAccountPick = new FormAccountPick())
            {
                if (formAccountPick.ShowDialog() == DialogResult.OK)
                {
                    depositAccountIds.Add(formAccountPick.SelectedAccount.Id);

                    LoadDepositAccounts();
                }
            }
        }

        private void AccountDepositRemoveButton_Click(object sender, EventArgs e)
        {
            if (depositAccountsListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item first.", 
                    "Accounting", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            depositAccountIds.RemoveAt(depositAccountsListBox.SelectedIndex);
            LoadDepositAccounts();
        }

        private void ChangeDepositAccountButton_Click(object sender, EventArgs e)
        {
            using (var formAccountPick = new FormAccountPick())
            {
                if (formAccountPick.ShowDialog() == DialogResult.OK)
                {
                    accountingIncomeAccountId = formAccountPick.SelectedAccount.Id;

                    depositAccountTextBox.Text = Account.GetDescription(accountingIncomeAccountId);
                }
            }
        }

        private void LoadQuickBooksPreferences()
        {
            void StringToListBoxItems(ListBox listBox, string data)
            {
                var items = data.Split(',');
                foreach (var item in items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        listBox.Items.Add(item);
                    }
                }
            }

            quickBooksCompanyFileTextBox.Text = Preference.GetString(PreferenceName.QuickBooksCompanyFile);
            quickBooksClassRefsCheckBox.Checked = Preference.GetBool(PreferenceName.QuickBooksClassRefsEnabled);

            StringToListBoxItems(quickBooksIncomeAccountsListBox, Preference.GetString(PreferenceName.QuickBooksIncomeAccount));
            StringToListBoxItems(quickBooksDepositAccountsListBox, Preference.GetString(PreferenceName.QuickBooksDepositAccounts));
            StringToListBoxItems(quickBooksClassRefsListBox, Preference.GetString(PreferenceName.QuickBooksClassRefs));
        }

        private bool SaveQuickBooksPreferences()
        {
            string ListBoxItemsToString(ListBox listBox)
            {
                var items = new List<string>();

                foreach (var item in listBox.Items)
                {
                    if (item is string s)
                    {
                        items.Add(s);
                    }
                }

                return string.Join(",", items);
            }

            if (Preference.Update(PreferenceName.QuickBooksCompanyFile, quickBooksCompanyFileTextBox.Text) |
                Preference.Update(PreferenceName.QuickBooksDepositAccounts, ListBoxItemsToString(quickBooksDepositAccountsListBox)) |
                Preference.Update(PreferenceName.QuickBooksIncomeAccount, ListBoxItemsToString(quickBooksIncomeAccountsListBox)) |
                Preference.Update(PreferenceName.QuickBooksClassRefsEnabled, quickBooksClassRefsCheckBox.Checked) |
                Preference.Update(PreferenceName.QuickBooksClassRefs, ListBoxItemsToString(quickBooksClassRefsListBox)))
            {
                return true;
            }

            return false;
        }

        private bool IsCompanyFileSpecified()
        {
            if (quickBooksCompanyFileTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show(
                    "Browse to your QuickBooks company file first.",
                    "Accounting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return false;
            }

            return true;
        }

        private List<string> PickQuickBookAccounts()
        {
            if (IsCompanyFileSpecified())
            {
                if (Preference.Update(PreferenceName.QuickBooksCompanyFile, quickBooksCompanyFileTextBox.Text.Trim()))
                {
                    DataValid.SetInvalid(InvalidType.Prefs);
                }

                using (var formAccountPick = new FormAccountPick())
                {
                    formAccountPick.IsQuickBooks = true;
                    if (formAccountPick.ShowDialog() == DialogResult.OK)
                    {
                        if (formAccountPick.SelectedQuickBookAccounts != null)
                        {
                            return formAccountPick.SelectedQuickBookAccounts;
                        }
                    }
                }
            }

            return new List<string>();
        }

        private void BrowseCompanyFileButton_Click(object sender, EventArgs e)
        {
            using(var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "QuickBooks Company File";
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "QuickBooks|*.qbw";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    quickBooksCompanyFileTextBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void TestQuickBooksButton_Click(object sender, EventArgs e)
        {
            if (IsCompanyFileSpecified())
            {
                Cursor.Current = Cursors.WaitCursor;

                string result = QuickBooks.TestConnection(quickBooksCompanyFileTextBox.Text.Trim());

                Cursor.Current = Cursors.Default;

                MessageBox.Show(result);
            }
        }

        private void QuickBooksDepositAccountAddButton_Click(object sender, EventArgs e)
        {
            var accounts = PickQuickBookAccounts();
            foreach (var account in accounts)
            {
                // TODO: Check for duplicates?

                quickBooksDepositAccountsListBox.Items.Add(account);
            }
        }

        private void QuickBooksDepositAccountRemoveButton_Click(object sender, EventArgs e)
        {
            if (quickBooksDepositAccountsListBox.SelectedItem is string account)
            {
                quickBooksDepositAccountsListBox.Items.Remove(account);
            }
            else
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Accounting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void QuickBooksIncomeAccountAddButton_Click(object sender, EventArgs e)
        {
            var accounts = PickQuickBookAccounts();
            foreach (var account in accounts)
            {
                // TODO: Check for duplicates?

                quickBooksIncomeAccountsListBox.Items.Add(account);
            }
        }

        private void QuickBooksIncomeAccountRemoveButton_Click(object sender, EventArgs e)
        {
            if (quickBooksIncomeAccountsListBox.SelectedItem is string account)
            {
                quickBooksIncomeAccountsListBox.Items.Remove(account);
            }
            else
            {
                MessageBox.Show(
                    "Please select an item first.", 
                    "Accounting", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        private void QuickBooksClassRefsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (quickBooksClassRefsCheckBox.Checked)
            {
                quickBooksClassRefsLabel.Visible = true;
                quickBooksClassRefsListBox.Visible = true;
                quickBooksClassRefAddButton.Visible = true;
                quickBooksClassRefRemoveButton.Visible = true;
            }
            else
            {
                quickBooksClassRefsLabel.Visible = false;
                quickBooksClassRefsListBox.Visible = false;
                quickBooksClassRefAddButton.Visible = false;
                quickBooksClassRefRemoveButton.Visible = false;
            }
        }

        private void QuickBooksClassRefAddButton_Click(object sender, EventArgs e)
        {
            if (IsCompanyFileSpecified())
            {
                if (Preference.Update(PreferenceName.QuickBooksCompanyFile, quickBooksCompanyFileTextBox.Text.Trim()))
                {
                    DataValid.SetInvalid(InvalidType.Prefs);
                }

                List<string> classes;
                try
                {
                    classes = QuickBooks.GetListOfClasses();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message, 
                        "Accounting", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }

                using (var inputBox = new InputBox("Choose a class", classes, true))
                {
                    inputBox.TopLevel = true;
                    if (inputBox.ShowDialog() == DialogResult.OK)
                    {
                        if (inputBox.SelectedIndices.Count < 1)
                        {
                            MessageBox.Show(
                                "You must choose a class.", 
                                "Accounting", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);

                            return;
                        }

                        foreach (int index in inputBox.SelectedIndices)
                        {
                            var className = classes[index];
                            if (quickBooksClassRefsListBox.Items.Contains(className))
                            {
                                continue;
                            }

                            quickBooksClassRefsListBox.Items.Add(className);
                        }
                    }
                }
            }
        }

        private void QuickBooksClassRefRemoveButton_Click(object sender, EventArgs e)
        {
            if (quickBooksClassRefsListBox.SelectedItem is string className)
            {
                quickBooksClassRefsListBox.Items.Remove(className);
            }
            else
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Accounting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void AcceptButton_Click(object sender, System.EventArgs e)
        {
            if (softwareComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Must select an accounting software.", 
                    "Accounting", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (Preference.Update(PreferenceName.AccountingSoftware, softwareComboBox.SelectedIndex) |
                SaveSoftlexDentalPreferences() |
                SaveQuickBooksPreferences())
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
