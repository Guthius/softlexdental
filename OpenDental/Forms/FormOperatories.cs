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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormOperatories : FormBase
    {
        private List<Operatory> operatories;
        private List<Operatory> operatoriesOld;

        /// <summary>
        /// List of conflict appointments to show the user. Only used for the combine operatories tool.
        /// </summary>
        public List<Appointment> ListConflictingAppts = new List<Appointment>();

        public FormOperatories() => InitializeComponent();

        private void FormOperatories_Load(object sender, EventArgs e)
        {
            LoadClinics();

            RefreshList();
        }

        private void RefreshList()
        {
            operatories = Operatories.GetDeepCopy();
            operatoriesOld = operatories.Select(x => x.Copy()).ToList();

            FillGrid();
        }

        private void LoadClinics()
        {
            var clinics = Clinic.GetByUser(Security.CurrentUser, true);

            clinicComboBox.Items.Clear();
            clinicComboBox.Items.Add("All");

            foreach (var clinic in clinics)
            {
                clinicComboBox.Items.Add(clinic);
            }

            clinicComboBox.SelectedIndex = 0;
        }

        private void FillGrid()
        {
            int opNameWidth = 180;
            int clinicWidth = 85;

            operatoriesGrid.BeginUpdate();
            operatoriesGrid.Columns.Clear();
            operatoriesGrid.Columns.Add(new ODGridColumn("Op Name", opNameWidth));
            operatoriesGrid.Columns.Add(new ODGridColumn("Abbrev", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("IsHidden", 64, HorizontalAlignment.Center));
            operatoriesGrid.Columns.Add(new ODGridColumn("Clinic", clinicWidth));
            operatoriesGrid.Columns.Add(new ODGridColumn("Provider", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("Hygienist", 70));
            operatoriesGrid.Columns.Add(new ODGridColumn("IsHygiene", 64, HorizontalAlignment.Center));
            operatoriesGrid.Rows.Clear();

            foreach (var operatory in operatories)
            {
                if (clinicComboBox.SelectedItem is Clinic clinic && operatory.ClinicNum != clinic.Id)
                {
                    continue;
                }

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

        private void OperatoriesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formOperatoryEdit = new FormOperatoryEdit((Operatory)operatoriesGrid.Rows[e.Row].Tag))
            {
                formOperatoryEdit.ShowDialog(this);

                FillGrid();
            }
        }

        private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillGrid();

        private void PickClinicButton_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics())
            {
                formClinics.IsSelectionMode = true;
                if (formClinics.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                for (int i = 1; i < clinicComboBox.Items.Count; i++)
                {
                    if (clinicComboBox.Items[i] is Clinic clinic && clinic.Id == formClinics.SelectedClinicId)
                    {
                        clinicComboBox.SelectedIndex = i;

                        break;
                    }
                }

                FillGrid();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var newOperatory = new Operatory
            {
                IsNew = true,
            };

            if (clinicComboBox.SelectedItem is Clinic clinic)
            {
                newOperatory.ClinicNum = clinic.Id;
            }

            if (operatoriesGrid.SelectedIndices.Length > 0)
            {
                newOperatory.ItemOrder = operatoriesGrid.SelectedIndices[0];
            }
            else
            {
                newOperatory.ItemOrder = operatories.Count;
            }

            using (var formOperatoryEdit = new FormOperatoryEdit(newOperatory))
            {
                if (formOperatoryEdit.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                operatories.Insert(newOperatory.ItemOrder, newOperatory);
            }

            FillGrid();
        }

        private void CombineButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            if (operatoriesGrid.SelectedIndices.Length < 2)
            {
                MessageBox.Show(
                    "Please select multiple items first while holding down the control key.", 
                    "Operatories", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var result =
                MessageBox.Show(
                    "Combine all selected operatories into a single operatory?\r\n\r\n" +
                    "This will affect all appointments set in these operatories and could take a while to run. " +
                    "The next window will let you select which operatory to keep when combining.",
                    "Operatories", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            bool hasNewOperatory = operatoriesGrid.SelectedIndices.ToList().Exists(x => ((Operatory)operatoriesGrid.Rows[x].Tag).IsNew);
            if (hasNewOperatory)
            {
                // This is needed due to the user adding an operatory then clicking combine.
                // The newly added operatory does not hava and OperatoryNum , so sync.
                ReorderAndSync();

                // TODO: CacheManager.Invalidate<Operatory>();

                RefreshList(); // Without this the user could cancel out of a prompt below and quickly close FormOperatories, causing a duplicate op from sync.
            }

            var selectedOperatoryIds = new List<long>();
            for (int i = 0; i < operatoriesGrid.SelectedIndices.Length; i++)
            {
                selectedOperatoryIds.Add(((Operatory)operatoriesGrid.Rows[operatoriesGrid.SelectedIndices[i]].Tag).OperatoryNum);
            }

            // Determine what operatory to keep as the 'master'.
            long masterOperatoryId;
            using (var formOperatoryPick = new FormOperatoryPick(operatories.FindAll(x => selectedOperatoryIds.Contains(x.OperatoryNum))))
            {
                if (formOperatoryPick.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                masterOperatoryId = formOperatoryPick.SelectedOperatoryId;
            }

            var masterOperatory =
                operatories.First(
                    operatory => operatory.OperatoryNum == masterOperatoryId);

            // Determine if any appointment conflict exist and potentially show them.
            var appointmentsToMergeChecked = Operatories.MergeApptCheck(masterOperatoryId, selectedOperatoryIds.FindAll(x => x != masterOperatoryId));
            var appointmentsToMerge = appointmentsToMergeChecked.Select(x => x.Item1).ToList();

            ListConflictingAppts = appointmentsToMergeChecked.Where(x => x.Item2).Select(x => x.Item1).ToList();
            
            // Appointment conflicts exist, can not merge.
            if (ListConflictingAppts.Count > 0)
            {
                result =
                    MessageBox.Show(
                        "Cannot merge operatories due to appointment conflicts.\r\n\r\n" +
                        "These conflicts need to be resolved before combining can occur.\r\n" +
                        "Click OK to view the conflicting appointments.",
                        "Operatories", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel)
                {
                    ListConflictingAppts.Clear();

                    return;
                }

                Close(); // Having ListConflictingAppts filled with appointments will cause outside windows that care to show the corresponding window.

                return;
            }

            // Final prompt, displays number of appointments to move and the 'master' operatory abbreviation.
            int numberOfAppointments = appointmentsToMerge.FindAll(appointment => appointment.Op != masterOperatoryId).Count;
            if (numberOfAppointments > 0)
            {
                result =
                    MessageBox.Show(
                        "Would you like to move " + numberOfAppointments + " appointments from their current operatories to " + masterOperatory.Abbrev + "?\r\n\r\n" +
                        "Warning: This action cannot be undone.",
                        "Operatories",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result == DialogResult.No) return;
            }

            if (!hasNewOperatory)
            {
                ReorderAndSync();

                // TODO: CacheManager.Invalidate<Operatory>();
            }

            try
            {
                Operatories.MergeOperatoriesIntoMaster(masterOperatoryId, selectedOperatoryIds, appointmentsToMerge);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message, 
                    "Operatories", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show(
                "The following operatories and all of their appointments were merged into the " + masterOperatory.Abbrev + " operatory;\r\n" + 
                string.Join(", ", operatories.FindAll(x => x.OperatoryNum != masterOperatoryId && selectedOperatoryIds.Contains(x.OperatoryNum)).Select(x => x.Abbrev)), 
                "Operatories", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            RefreshList();

            FillGrid();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (operatoriesGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "You must first select a row.", 
                    "Operatories", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            int selectedIndex = operatoriesGrid.GetSelectedIndex();
            if (selectedIndex == 0)
            {
                return;
            }

            var op1 = (Operatory)operatoriesGrid.Rows[selectedIndex].Tag;
            var op2 = (Operatory)operatoriesGrid.Rows[selectedIndex - 1].Tag;

            if (!CanReorderOperatories(op1, op2, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Operatories",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int selectedItemOrder = op1.ItemOrder;
            op1.ItemOrder = op2.ItemOrder;
            op2.ItemOrder = selectedItemOrder;

            operatories = operatories.OrderBy(x => x.ItemOrder).ToList();

            SwapGridMainLocations(selectedIndex, selectedIndex - 1);

            operatoriesGrid.SetSelected(selectedIndex - 1, true);
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (operatoriesGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "You must first select a row.",
                    "Operatories",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            int selected = operatoriesGrid.GetSelectedIndex();
            if (selected == operatoriesGrid.Rows.Count - 1)
            {
                return;
            }

            var op1 = (Operatory)operatoriesGrid.Rows[selected].Tag;
            var op2 = (Operatory)operatoriesGrid.Rows[selected + 1].Tag;

            if (!CanReorderOperatories(op1, op2, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage, 
                    "Operatories",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int selectedItemOrder = op1.ItemOrder;
            op1.ItemOrder = op2.ItemOrder;
            op2.ItemOrder = selectedItemOrder;

            operatories = operatories.OrderBy(x => x.ItemOrder).ToList();

            SwapGridMainLocations(selected, selected + 1);

            operatoriesGrid.SetSelected(selected + 1, true);
        }

        /// <summary>
        /// Returns true if the two operatories can be reordered. Returns false and fills the
        /// <paramref name="errorMessage"/> parameter if they cannot.
        /// </summary>
        private bool CanReorderOperatories(Operatory op1, Operatory op2, out string errorMessage)
        {
            errorMessage = "";
            if (!(clinicComboBox.SelectedItem is Clinic clinic))
            {
                return true;
            }

            if (!op1.IsInHQView && !op2.IsInHQView)
            {
                return true;
            }

            errorMessage = 
                "You cannot change the order of the operatories '" + op1.Abbrev + "' and '" + op2.Abbrev + "' with clinic" + " '" + clinic.Abbr + "' selected " +
                "because it is also a member of a Headquarters Appointment View. You must set your clinic selection to 'All' to reorder these operatories.";

            return false;
        }

        /// <summary>
        /// Swaps two rows in the grid for use with the up and down buttons.
        /// </summary>
        private void SwapGridMainLocations(int fromIndex, int toIndex)
        {
            operatoriesGrid.BeginUpdate();
            var dataRow = operatoriesGrid.Rows[fromIndex];
            operatoriesGrid.Rows.RemoveAt(fromIndex);
            operatoriesGrid.Rows.Insert(toIndex, dataRow);
            operatoriesGrid.EndUpdate();
        }

        private void ReorderAndSync()
        {
            for (int i = 0; i < operatories.Count; i++)
            {
                operatories[i].ItemOrder = i;
            }

            Operatories.Sync(operatories, operatoriesOld);
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void FormOperatories_Closing(object sender, CancelEventArgs e)
        {
            ReorderAndSync();

            //CacheManager.Invalidate<Operatory>();
        }
    }
}
