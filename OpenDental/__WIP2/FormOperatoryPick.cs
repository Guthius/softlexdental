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
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormOperatoryPick : FormBase
    {
        private readonly List<Operatory> operatories;

        /// <summary>
        /// Gets the ID of the selected operatory.
        /// </summary>
        public long SelectedOperatoryId
        {
            get
            {
                var selectedIndex = operatoriesGrid.GetSelectedIndex();
                if (selectedIndex != -1 && operatoriesGrid.Rows[selectedIndex].Tag is Operatory operatory)
                {
                    return operatory.OperatoryNum;
                }

                return -1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormOperatoryPick"/> class.
        /// </summary>
        /// <param name="operatories">The list of possible operatories to pick from.</param>
        public FormOperatoryPick(List<Operatory> operatories)
        {
            InitializeComponent();

            this.operatories = operatories;
        }

        private void FormOperatoryPick_Load(object sender, EventArgs e) => FillGrid();

        private void OperatoriesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) => AcceptButton_Click(this, EventArgs.Empty);

        private void FillGrid()
        {
            operatoriesGrid.BeginUpdate();
            operatoriesGrid.Columns.Clear();
            operatoriesGrid.Columns.Add(new ODGridColumn("Op Name", 180));
            operatoriesGrid.Columns.Add(new ODGridColumn("Abbrev", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("IsHidden", 64, HorizontalAlignment.Center));
            operatoriesGrid.Columns.Add(new ODGridColumn("Clinic", 85));
            operatoriesGrid.Columns.Add(new ODGridColumn("Provider", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("Hygienist", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("IsHygiene", 64, HorizontalAlignment.Center));
            operatoriesGrid.Rows.Clear();

            foreach (var operatory in operatories)
            {
                var row = new ODGridRow();

                row.Cells.Add(operatory.OpName);
                row.Cells.Add(operatory.Abbrev);
                row.Cells.Add(operatory.IsHidden ? "X" : "");
                row.Cells.Add(Clinic.GetById(operatory.ClinicNum).Abbr);
                row.Cells.Add(Providers.GetAbbr(operatory.ProvDentist));
                row.Cells.Add(Providers.GetAbbr(operatory.ProvHygienist));
                row.Cells.Add(operatory.IsHygiene ? "X" : "");
                row.Tag = operatory;

                operatoriesGrid.Rows.Add(row);
            }

            operatoriesGrid.EndUpdate();
        }

        /// <summary>
        ///     <para>
        ///         Checks whether a operatory has been selected. If no operatory has been selected
        ///         a error message will be displayed.
        ///     </para>
        /// </summary>
        /// <returns>True if a operatory is selected; otherwise, false.</returns>
        private bool IsOperatorySelected()
        {
            if (operatoriesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Operatories", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return false;
            }

            return true;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!IsOperatorySelected())
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
