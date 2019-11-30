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
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public partial class FormTrophyNamePick : ODForm
    {
        private readonly List<TrophyFolderInfo> matches;

        /// <summary>
        ///     <para>
        ///         Gets the name of the folder selected by the user.
        ///     </para>
        ///     <para>
        ///         When blank the calling call will need to generate a new folder.
        ///     </para>
        /// </summary>
        public string PickedName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormTrophyNamePick"/> class.
        /// </summary>
        public FormTrophyNamePick(List<TrophyFolderInfo> matches)
        {
            InitializeComponent();

            this.matches = matches;
        }

        private void FormTrophyNamePick_Load(object sender, EventArgs e) => FillGrid();

        private void FillGrid()
        {
            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            gridMain.Columns.Add(new ODGridColumn("FolderName", 100));
            gridMain.Columns.Add(new ODGridColumn("Last Name", 120));
            gridMain.Columns.Add(new ODGridColumn("First Name", 120));
            gridMain.Columns.Add(new ODGridColumn("Birthdate", 80));
            gridMain.Rows.Clear();

            for (int i = 0; i < matches.Count; i++)
            {
                var row = new ODGridRow();

                row.Cells.Add(matches[i].FolderName);
                row.Cells.Add(matches[i].LastName);
                row.Cells.Add(matches[i].FirstName);
                row.Cells.Add(matches[i].BirthDate.ToShortDateString());

                gridMain.Rows.Add(row);
            }

            gridMain.EndUpdate();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            PickedName = "";

            DialogResult = DialogResult.OK;
        }

        private void FoldersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) => AcceptButton_Click(this, EventArgs.Empty);

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (gridMain.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an item from the list first.",
                    "Trophy", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            PickedName = matches[gridMain.GetSelectedIndex()].FolderName;

            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
    }
}
