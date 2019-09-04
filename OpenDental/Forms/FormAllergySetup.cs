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
    public partial class FormAllergySetup : FormBase
    {
        List<AllergyDef> allergiesList;
        
        /// <summary>
        /// Gets or sets a value indicating whether the form is in selection mode.
        /// </summary>
        public bool IsSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the number of the selected allergy.
        /// </summary>
        public long SelectedAllergyDefNum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAllergySetup"/> class.
        /// </summary>
        public FormAllergySetup() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAllergySetup_Load(object sender, EventArgs e)
        {
            if (IsSelectionMode)
            {
                acceptButton.Visible = true;
                cancelButton.Text = Translation.Language.ButtonCancel;
            }
            LoadAllergies();
        }

        /// <summary>
        /// Fills the allergies grid.
        /// </summary>
        void LoadAllergies()
        {
            allergiesList = AllergyDefs.GetAll(showHiddenCheckBox.Checked);

            allergiesGrid.BeginUpdate();
            allergiesGrid.Columns.Clear();
            allergiesGrid.Columns.Add(new ODGridColumn(Translation.Language.Description, 160));
            allergiesGrid.Columns.Add(new ODGridColumn(Translation.Language.Hidden, 60));

            allergiesGrid.Rows.Clear();
            for (int i = 0; i < allergiesList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(allergiesList[i].Description);
                if (allergiesList[i].IsHidden)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }
                allergiesGrid.Rows.Add(row);
            }
            allergiesGrid.EndUpdate();
        }

        /// <summary>
        /// Reloads the list of allergies when visibility of hidden items is toggled.
        /// </summary>
        void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e) => LoadAllergies();
        
        /// <summary>
        /// Closes the form when in selection mode; otherwise, opens the form to edit the selected allergy.
        /// </summary>
        void AllergiesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsSelectionMode)
            {
                SelectedAllergyDefNum = allergiesList[e.Row].Id;
                DialogResult = DialogResult.OK;
            }
            else
            {
                using (var formAllergyDefEdit = new FormAllergyDefEdit())
                {
                    formAllergyDefEdit.AllergyDefCur = allergiesList[allergiesGrid.GetSelectedIndex()];

                    if (formAllergyDefEdit.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAllergies();
                    }
                }
            }
        }

        /// <summary>
        /// Opens the form to add a new allergy.
        /// </summary>
        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formAllergyDefEdit = new FormAllergyDefEdit())
            {
                formAllergyDefEdit.AllergyDefCur = new AllergyDef();

                if (formAllergyDefEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadAllergies();
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (allergiesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.AllergySelectAtLeastOne,
                    Translation.Language.AllergySetup, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            SelectedAllergyDefNum = allergiesList[allergiesGrid.GetSelectedIndex()].Id;

            DialogResult = DialogResult.OK;
        }
    }
}