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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness; 
using CodeBase;
using System.ComponentModel;

namespace OpenDental
{
    public partial class FormClinics : FormBase
    {
        private List<Clinic> clinics;

        private long targetClinicId = -1;
        private Dictionary<long, int> clinicPatientCounts = new Dictionary<long, int>();

        /// <summary>
        ///     <para>
        ///         Gets or sets a value indicating whether the form is in selection mode.
        ///     </para>
        /// </summary>
        public bool IsSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected clinic.
        /// </summary>
        public long SelectedClinicId { get; set; }

        /// <summary>
        ///     <para>
        ///         Gets or sets the list of selected clinics ID's.
        ///     </para>
        /// </summary>
        public List<long> SelectedClinicIds { get; set; } = new List<long>();

        /// <summary>
        ///     <para>
        ///         Gets or sets a value indicating whether multiple clinics may be selected.
        ///     </para>
        ///     <para>
        ///         Only relevant when <see cref="IsSelectionMode"/> is set to true.
        ///     </para>
        /// </summary>
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormClinics"/> class.
        /// </summary>
        public FormClinics(IEnumerable<Clinic> clinics = null)
        {
            if (clinics != null)
            {
                this.clinics = clinics.ToList();
            }

            InitializeComponent();
        }

        private void FormClinics_Load(object sender, EventArgs e)
        {
            orderAlphabeticalCheckBox.Checked = Preference.GetBool(PreferenceName.ClinicListIsAlphabetical);
            if (clinics == null)
            {
                clinics = Clinic.GetByUser(Security.CurrentUser, true).ToList();
            }

            clinics.Sort(ClinicSort);

            if (IsSelectionMode)
            {
                movePatientsGroupBox.Visible = false;
                clinicOrderGroupBox.Visible = false;
                addButton.Visible = false;
                acceptButton.Visible = true;

                int widthDiff = clinicOrderGroupBox.Width - acceptButton.Width;
                MinimumSize = new Size(MinimumSize.Width - widthDiff, MinimumSize.Height);
                Width -= widthDiff;
                clinicsGrid.Width += widthDiff;
                showHiddenCheckBox.Visible = false;
                showHiddenCheckBox.Checked = false;

                if (IsMultiSelect)
                {
                    selectAllButton.Visible = true;
                    selectNoneButton.Visible = true;
                    clinicsGrid.SelectionMode = GridSelectionMode.Multiple;
                }
            }
            else
            {
                if (orderAlphabeticalCheckBox.Checked)
                {
                    upButton.Enabled = false;
                    downButton.Enabled = false;
                }

                clinicPatientCounts = Clinics.GetClinicalPatientCount();
            }

            FillGrid(false);

            if (SelectedClinicIds != null)
            {
                for (int i = 0; i < clinicsGrid.Rows.Count; i++)
                {
                    if (clinicsGrid.Rows[i].Tag is Clinic clinic)
                    {
                        if (SelectedClinicId == clinic.Id || SelectedClinicIds.Contains(clinic.Id))
                        {
                            clinicsGrid.SetSelected(i, true);
                        }
                    }
                }
            }
        }

        private void FillGrid(bool reselectRows = true)
        {
            var selectedClinicIds = new List<long>();
            if (reselectRows)
            {
                selectedClinicIds = clinicsGrid.SelectedTags<Clinic>().Select(x => x.Id).ToList();
            }

            clinicsGrid.BeginUpdate();
            clinicsGrid.Columns.Clear();
            clinicsGrid.Columns.Add(new ODGridColumn("Abbr", 120));
            clinicsGrid.Columns.Add(new ODGridColumn("Description", 200));
            clinicsGrid.Columns.Add(new ODGridColumn("Specialty", 150));
            if (!IsSelectionMode)
            {
                clinicsGrid.Columns.Add(new ODGridColumn("Patients", 80, HorizontalAlignment.Center));
                clinicsGrid.Columns.Add(new ODGridColumn("Hidden", 0, HorizontalAlignment.Center));
            }

            clinicsGrid.Rows.Clear();

            var clinicSpecialityDescriptions = Definition.GetByCategory(DefinitionCategory.ClinicSpecialty).ToDictionary(x => x.Id, x => x.Description);

            var selectedClinicIndices = new List<int>();
            foreach (var clinic in clinics)
            {
                if (!showHiddenCheckBox.Checked && clinic.IsHidden)
                {
                    continue;
                }

                var row = new ODGridRow();
                row.Cells.Add(clinic.Abbr);
                row.Cells.Add(clinic.Description);

                string specialties = 
                    string.Join(",",
                        DefLinks.GetListByFKey(clinic.Id, DefLinkType.Clinic).Select(
                                x => clinicSpecialityDescriptions.TryGetValue(x.DefNum, out var specialty) ? specialty : "")
                            .Where(x => !string.IsNullOrWhiteSpace(x)));

                row.Cells.Add(specialties);
                if (!IsSelectionMode)
                {
                    clinicPatientCounts.TryGetValue(clinic.Id, out var patientCount);

                    row.Cells.Add(patientCount.ToString());
                    row.Cells.Add(clinic.IsHidden ? "X" : "");
                }

                row.Tag = clinic;

                clinicsGrid.Rows.Add(row);

                if (selectedClinicIds.Contains(clinic.Id))
                {
                    selectedClinicIndices.Add(clinicsGrid.Rows.Count - 1);
                }
            }
            clinicsGrid.EndUpdate();

            if (reselectRows && selectedClinicIndices.Count > 0)
            {
                if (IsMultiSelect)
                {
                    clinicsGrid.SetSelected(selectedClinicIndices.ToArray(), true);
                }
                else
                {
                    clinicsGrid.SetSelected(selectedClinicIndices[0], true);
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var newClinic = new Clinic
            {
                SortOrder = clinicsGrid.Rows.Count
            };

            using (var formClinicEdit = new FormClinicEdit(newClinic))
            {
                if (formClinicEdit.ShowDialog() == DialogResult.OK)
                {
                    Clinic.Insert(newClinic);

                    clinics.Add(newClinic);
                    clinics.Sort(ClinicSort);

                    CacheManager.Invalidate<Clinic>();
                    
                    clinicPatientCounts[newClinic.Id] = 0;
                }
            }

            FillGrid();
        }

        private void ClinicsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (clinicsGrid.Rows.Count == 0) return;

            if (IsSelectionMode)
            {
                AcceptButton_Click(this, EventArgs.Empty);

                return;
            }

            var clinic = (Clinic)clinicsGrid.Rows[e.Row].Tag;

            using (var formClinicEdit = new FormClinicEdit(clinic))
            {
                if (formClinicEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            FillGrid();
        }

        private void PickClinicButton_Click(object sender, EventArgs e)
        {
            List<Clinic> listClinics = clinicsGrid.GetTags<Clinic>();

            using (var formClinics = new FormClinics())
            {
                formClinics.clinics = listClinics;
                formClinics.IsSelectionMode = true;

                if (formClinics.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                targetClinicId = formClinics.SelectedClinicId;
                moveToTextBox.Text = (listClinics.FirstOrDefault(x => x.Id == targetClinicId)?.Abbr ?? "");
            }
        }

        private void MovePatientsButton_Click(object sender, EventArgs e)
        {
            if (clinicsGrid.SelectedIndices.Length < 1)
            {
                MessageBox.Show(
                    "You must select at least one clinic to move patients from.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (targetClinicId == -1)
            {
                MessageBox.Show(
                    "You must pick a 'To' clinic in the box above to move patients to.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var selectedClinics = clinicsGrid.SelectedTags<Clinic>().ToDictionary(x => x.Id);

            var clinicDest = clinics.FirstOrDefault(x => x.Id == targetClinicId);
            if (clinicDest == null)
            {
                MessageBox.Show(
                    "The clinic could not be found.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (selectedClinics.ContainsKey(clinicDest.Id))
            {
                MessageBox.Show(
                    "The 'To' clinic should not also be one of the 'From' clinics.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var clinicPatientCounts = Clinics.GetClinicalPatientCount(true).Where(x => selectedClinics.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            if (clinicPatientCounts.Sum(x => x.Value) == 0)
            {
                MessageBox.Show(
                    "There are no patients assigned to the selected clinics.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var result =
                MessageBox.Show(
                    "This will move all patients to " + clinicDest.Abbr + " from the following clinics:\r\n" + 
                    string.Join("\r\n", clinicPatientCounts.Select(x => selectedClinics[x.Key].Abbr)) + "\r\n" + 
                    "Continue?", 
                    "Clinics", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            ODProgress.ShowAction(() =>
            {
                int patientsMoved = 0;

                var actions = clinicPatientCounts.Select(x => new Action(() =>
                {
                    Patients.ChangeClinicsForAll(x.Key, clinicDest.Id);

                    SecurityLog.Write(
                        SecurityLogEvents.PatientEdit,
                        "Clinic changed for " + x.Value + " patients from " + (selectedClinics.TryGetValue(x.Key, out Clinic currentClinic) ? currentClinic.Abbr : "") + " to " + clinicDest.Abbr + ".");

                    patientsMoved += x.Value;

                    ClinicEvent.Fire(
                        ODEventType.Clinic, 
                        "Moved patients: " + patientsMoved + " out of " + clinicPatientCounts.Sum(y => y.Value));

                })).ToList();

                ODThread.RunParallel(actions, TimeSpan.FromMinutes(2));
            },
                startingMessage: "Moving patients...",
                eventType: typeof(ClinicEvent),
                odEventType: ODEventType.Clinic);

            this.clinicPatientCounts = Clinics.GetClinicalPatientCount();

            FillGrid();

            MessageBox.Show(
                "Done",
                "Clinics",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (clinicsGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select a clinic first.",
                    "Clinics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            int selectedIndex = clinicsGrid.GetSelectedIndex();
            if (selectedIndex == 0)
            {
                return;
            }

            var c1 = (Clinic)clinicsGrid.Rows[selectedIndex].Tag;
            var c2 = (Clinic)clinicsGrid.Rows[selectedIndex - 1].Tag;

            if (c1.SortOrder == c2.SortOrder) c1.SortOrder--;
            else
            {
                int sourceOrder = c1.SortOrder;
                c1.SortOrder = c2.SortOrder;
                c2.SortOrder = sourceOrder;
            }

            clinics.Sort(ClinicSort);

            FillGrid();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (clinicsGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select a clinic first.",
                    "Clinics", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            int selectedIdx = clinicsGrid.GetSelectedIndex();
            if (selectedIdx == clinicsGrid.Rows.Count - 1)
            {
                return;
            }

            var c1 = (Clinic)clinicsGrid.Rows[selectedIdx].Tag;
            var c2 = (Clinic)clinicsGrid.Rows[selectedIdx + 1].Tag;

            if (c1.SortOrder == c2.SortOrder) c1.SortOrder++;
            else
            {
                int sourceOrder = c1.SortOrder;
                c1.SortOrder = c2.SortOrder;
                c2.SortOrder = sourceOrder;
            }

            clinics.Sort(ClinicSort);

            FillGrid();
        }

        private void OrderAlphabeticalCheckBox_Click(object sender, EventArgs e)
        {
            if (orderAlphabeticalCheckBox.Checked)
            {
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
            else
            {
                upButton.Enabled = true;
                downButton.Enabled = true;
            }

            clinics.Sort(ClinicSort);

            FillGrid();
        }

        /// <summary>
        ///     <para>
        ///         Sorts clinics. Sorts by abbreviation if alphabetical sorting is enabled; 
        ///         otherwise it will sort by the defined sort order. Clinics with the same
        ///         abbreviation or sort order are subsequently sorted by description and ID.
        ///     </para>
        /// </summary>
        private int ClinicSort(Clinic lhs, Clinic rhs)
        {
            int result = 
                orderAlphabeticalCheckBox.Checked ? 
                    lhs.Abbr.CompareTo(rhs.Abbr) : 
                    lhs.SortOrder.CompareTo(rhs.SortOrder);

            if (result == 0) result = lhs.Description.CompareTo(rhs.Description);
            if (result == 0) result = lhs.Id.CompareTo(rhs.Id);
            
            return result;
        }

        private void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e) => FillGrid();

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            clinicsGrid.SetSelected(true);
            clinicsGrid.Focus();
        }

        private void SelectNoneButton_Click(object sender, EventArgs e)
        {
            clinicsGrid.SetSelected(false);
            clinicsGrid.Focus();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (IsSelectionMode && clinicsGrid.SelectedIndices.Length > 0)
            {
                SelectedClinicId = clinicsGrid.SelectedTag<Clinic>()?.Id ?? 0;
                SelectedClinicIds = clinicsGrid.SelectedTags<Clinic>().Select(x => x.Id).ToList();

                DialogResult = DialogResult.OK;
            }

            Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            SelectedClinicId = 0;

            SelectedClinicIds = new List<long>();

            Close();
        }

        /// <summary>
        ///     <para>
        ///         Fetches the list of all clinics, sorts it and ensures that the 
        ///         <see cref="Clinic.SortOrder"/> field of each clinic has the correct value.
        ///     </para>
        /// </summary>
        private void FormClinics_Closing(object sender, CancelEventArgs e)
        {
            if (IsSelectionMode) return;

            if (orderAlphabeticalCheckBox.Checked) return;

            var clinics = Clinic.All().ToList();

            clinics.Sort(ClinicSort);
            for (int i = 0; i < clinics.Count; i++)
            {
                if (clinics[i].SortOrder != i)
                {
                    clinics[i].SortOrder = i;

                    Clinic.Update(clinics[i]);
                }
            }
        }
    }
}
