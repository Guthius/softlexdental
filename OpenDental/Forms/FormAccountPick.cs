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
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountPick : FormBase
    {
        /// <summary>
        /// Gets the selected account.
        /// </summary>
        public Account SelectedAccount { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to select quick book accounts.
        /// </summary>
        public bool IsQuickBooks { get; set; }

        /// <summary>
        /// Gets the selected QuickBook accounts.
        /// </summary>
        public List<string> SelectedQuickBookAccounts { get; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAccountPick"/> class.
        /// </summary>
        public FormAccountPick() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAccountPick_Load(object sender, EventArgs e)
        {
            if (IsQuickBooks)
            {
                checkInactive.Visible = false;

                LoadAccountsQuickBooks();

                accountsGrid.SelectionMode = GridSelectionMode.Multiple;
            }
            else
            {
                LoadAccounts();
            }
        }

        /// <summary>
        /// Loads the list of accounts and populates the grid.
        /// </summary>
        void LoadAccounts()
        {
            accountsGrid.BeginUpdate();
            accountsGrid.Columns.Clear();
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnType, 70));
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 170));
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnBalance, 65, HorizontalAlignment.Right));
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnBankNumber, 100));
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnInactive, 70));
            accountsGrid.Rows.Clear();

            var accounts = Account.All();
            for (int i = 0; i < accounts.Count; i++)
            {
                if (!checkInactive.Checked && accounts[i].Inactive)
                {
                    continue;
                }

                var row = new ODGridRow();
                row.Cells.Add(accounts[i].Type.ToString());
                row.Cells.Add(accounts[i].Description);
                if (accounts[i].Type == AccountType.Asset)
                {
                    row.Cells.Add(Account.GetBalance(accounts[i].Id, accounts[i].Type).ToString("N"));
                }
                else
                {
                    row.Cells.Add("");
                }

                row.Cells.Add(accounts[i].BankNumber);
                row.Cells.Add(accounts[i].Inactive ? "X" : "");

                if (i < accounts.Count - 1 && accounts[i].Type != accounts[i + 1].Type)
                {
                    row.ColorLborder = Color.Black;
                }

                row.Tag = accounts[i];
                row.BackColor = accounts[i].Color;

                accountsGrid.Rows.Add(row);
            }

            accountsGrid.EndUpdate();
        }

        /// <summary>
        /// Loads the list of accounts from QuickBooks.
        /// </summary>
        void LoadAccountsQuickBooks()
        {
            accountsGrid.BeginUpdate();
            accountsGrid.Columns.Clear();
            accountsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 200));
            accountsGrid.Rows.Clear();

            Cursor.Current = Cursors.WaitCursor;

            var accountList = new List<string>();
            try
            {
                accountList = QuickBooks.GetListOfAccounts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Translation.Language.PickAccount, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;

            for (int i = 0; i < accountList.Count; i++)
            {
                var row = new ODGridRow();

                row.Cells.Add(accountList[i]);
                row.Tag = accountList[i];

                accountsGrid.Rows.Add(row);
            }

            accountsGrid.EndUpdate();
        }

        /// <summary>
        /// Selects a account when the user double clicks on one in the grid and closes the form.
        /// </summary>
        void AccountsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsQuickBooks)
            {
                SelectedQuickBookAccounts.Add((string)accountsGrid.Rows[e.Row].Tag);
            }
            else
            {
                SelectedAccount = (Account)accountsGrid.Rows[e.Row].Tag;
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Reloads the accounts when the state of the inactive checkbox changes.
        /// </summary>
        void InactiveCheckBox_Click(object sender, EventArgs e) => LoadAccounts();

        /// <summary>
        /// Validates the selection and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (accountsGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectAnAccountFirst,
                    Translation.Language.PickAccount, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (IsQuickBooks)
            {
                for (int i = 0; i < accountsGrid.SelectedIndices.Length; i++)
                {
                    SelectedQuickBookAccounts.Add((string)(accountsGrid.Rows[accountsGrid.SelectedIndices[i]].Tag));
                }
            }
            else
            {
                SelectedAccount = (Account)accountsGrid.Rows[accountsGrid.GetSelectedIndex()].Tag;
            }

            DialogResult = DialogResult.OK;
        }
    }
}