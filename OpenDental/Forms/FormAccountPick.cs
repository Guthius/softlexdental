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
        public Account SelectedAccount;
        public bool IsQuickBooks;
        public List<string> SelectedAccountsQB;

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
                SelectedAccountsQB = new List<string>();
                checkInactive.Visible = false;

                LoadAccountsQuickBooks();

                accountsGrid.SelectionMode = GridSelectionMode.MultiExtended;
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

            var listAccounts = Account.All();
            for (int i = 0; i < listAccounts.Count; i++)
            {
                if (!checkInactive.Checked && listAccounts[i].Inactive)
                {
                    continue;
                }

                var row = new ODGridRow();
                row.Cells.Add(listAccounts[i].Type.ToString());
                row.Cells.Add(listAccounts[i].Description);
                if (listAccounts[i].Type == AccountType.Asset)
                {
                    row.Cells.Add(Account.GetBalance(listAccounts[i].Id, listAccounts[i].Type).ToString("N"));
                }
                else
                {
                    row.Cells.Add("");
                }

                row.Cells.Add(listAccounts[i].BankNumber);
                row.Cells.Add(listAccounts[i].Inactive ? "X" : "");

                if (i < listAccounts.Count - 1 && listAccounts[i].Type != listAccounts[i + 1].Type)
                {
                    row.ColorLborder = Color.Black;
                }

                row.Tag = listAccounts[i];
                row.BackColor = listAccounts[i].Color;

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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                SelectedAccountsQB.Add((string)accountsGrid.Rows[e.Row].Tag);
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
                    SelectedAccountsQB.Add((string)(accountsGrid.Rows[accountsGrid.SelectedIndices[i]].Tag));
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