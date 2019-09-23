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
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAllergyDefEdit : FormBase
    {
        public Allergy AllergyDefCur;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAllergyDefEdit"/> class.
        /// </summary>
        public FormAllergyDefEdit() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAllergyEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = AllergyDefCur?.Description ?? "";
            if (AllergyDefCur.Id > 0)
            {
                hiddenCheckBox.Checked = AllergyDefCur.Hidden;
            }

            for (int i = 0; i < Enum.GetNames(typeof(SnomedAllergy)).Length; i++)
            {
                allergyTypeComboBox.Items.Add(Enum.GetNames(typeof(SnomedAllergy))[i]);
            }
            allergyTypeComboBox.SelectedIndex = (int)AllergyDefCur.SnomedType;

            if (AllergyDefCur.MedicationId.HasValue)
            {
                medicationTextBox.Text = Medication.GetDescription(AllergyDefCur.MedicationId.Value);
            }

            uniiTextBox.Text = AllergyDefCur.UniiCode;
        }

        /// <summary>
        /// Opens the form to select a UNII code.
        /// </summary>
        void UniiBrowseButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement this
        }

        /// <summary>
        /// Clears the UNII code.
        /// </summary>
        void UniiNoneButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement this
        }

        /// <summary>
        /// Opens the form to select a medication.
        /// </summary>
        void MedicationBrowseButton_Click(object sender, EventArgs e)
        {
            using (var formMedications = new FormMedications())
            {
                formMedications.IsSelectionMode = true;
                if (formMedications.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                AllergyDefCur.MedicationId = formMedications.SelectedMedicationNum;

                if (AllergyDefCur.MedicationId.HasValue)
                {
                    medicationTextBox.Text = Medication.GetDescription(AllergyDefCur.MedicationId.Value);
                }
            }
        }

        /// <summary>
        /// Clears the selected medication.
        /// </summary>
        void MedicationNoneButton_Click(object sender, EventArgs e)
        {
            AllergyDefCur.MedicationId = 0;
            medicationTextBox.Text = "";
        }

        /// <summary>
        /// Delete the allergy definition. Only allowed if the allergy is not in use.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (AllergyDefCur.Id > 0)
            {
                if (!Allergy.IsInUse(AllergyDefCur.Id))
                {
                    var result = 
                        MessageBox.Show(
                            Translation.Language.AllergyDefConfirmDelete,
                            Translation.Language.AllergyDef,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel) return;

                    Allergy.Delete(AllergyDefCur.Id);
                }
                else
                {
                    MessageBox.Show(
                        Translation.Language.AllergyDefCannotDeleteInUse,
                        Translation.Language.AllergyDef,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Validates all the fields, saves the allergy def and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.DescriptionCannotBeBlank,
                    Translation.Language.AllergyDef, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (uniiTextBox.Text != "" && medicationTextBox.Text != "")
            {
                MessageBox.Show(
                    Translation.Language.AllergyOnlyOneUniiCodePerAllergyAllowed,
                    Translation.Language.AllergyDef, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            var uniiCode = uniiTextBox.Text.Trim();

            var invalidCharacters = new StringBuilder();
            for (int i = 0; i < uniiCode.Length; i++)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".IndexOf(uniiCode[i]) == -1)
                {
                    invalidCharacters.Append(uniiCode[i]);
                }
            }

            if (invalidCharacters.ToString() != "")
            {
                MessageBox.Show(
                    string.Format(Translation.Language.AllergyUniiCodeContainsInvalidCharacters, invalidCharacters),
                    Translation.Language.AllergyDef,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            
            if (uniiCode.Length > 0 && uniiCode.Length != 10)
            {
                MessageBox.Show(
                    Translation.Language.AllergyUniiCodeMustBe10Characters,
                    Translation.Language.AllergyDef,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            AllergyDefCur.Description = description;
            AllergyDefCur.Hidden = hiddenCheckBox.Checked;
            AllergyDefCur.SnomedType = (SnomedAllergy)allergyTypeComboBox.SelectedIndex;
            AllergyDefCur.UniiCode = uniiCode;

            // TODO: Do UNII check once the table is added

            if (AllergyDefCur.IsNew)
            {
                Allergy.Insert(AllergyDefCur);
            }
            else
            {
                Allergy.Update(AllergyDefCur);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
