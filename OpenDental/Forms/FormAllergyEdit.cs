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
    public partial class FormAllergyEdit : FormBase
    {
        private List<Allergy> allergies;
        private Snomed snomedReaction;

        /// <summary>
        /// Gets or sets the allergy being edited.
        /// </summary>
        public PatientAllergy Allergy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAllergyEdit"/> class.
        /// </summary>
        public FormAllergyEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        private void FormAllergyEdit_Load(object sender, EventArgs e)
        {
            int allergyIndex = 0;

            allergies = OpenDentBusiness.Allergy.All(false);

            if (allergies.Count < 1)
            {
                MessageBox.Show(
                    "Need to set up at least one Allergy from EHR setup window.",
                    Translation.Language.Allergy, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                DialogResult = DialogResult.Cancel;
                return;
            }

            for (int i = 0; i < allergies.Count; i++)
            {
                allergyComboBox.Items.Add(allergies[i].Description);
                if (!Allergy.IsNew && allergies[i].Id == Allergy.AllergyId)
                {
                    allergyIndex = i;
                }
            }

            // Get the SNOMED reaction assigned to the allergy.
            snomedReaction = Snomeds.GetByCode(Allergy.SnomedReaction);
            if (snomedReaction != null)
            {
                snomedTextBox.Text = snomedReaction.Description;
            }

            if (!Allergy.IsNew)
            {
                dateTextBox.Text = Allergy.DateAdverseReaction?.ToShortDateString() ?? "";
                allergyComboBox.SelectedIndex = allergyIndex;
                reactionTextBox.Text = Allergy.Reaction;
                activeCheckBox.Checked = Allergy.Active;
            }
            else
            {
                allergyComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Opens the form to select a SNOMED reaction to assign to the allergy.
        /// </summary>
        void SnomedBrowseButton_Click(object sender, EventArgs e)
        {
            using (var formSnomeds = new FormSnomeds())
            {
                formSnomeds.IsSelectionMode = true;

                if (formSnomeds.ShowDialog() == DialogResult.OK)
                {
                    snomedReaction = formSnomeds.SelectedSnomed;
                    snomedTextBox.Text = snomedReaction.Description;
                }
            }
        }

        /// <summary>
        /// Clears the selected SNOMED reaction.
        /// </summary>
        void SnomedNoneButton_Click(object sender, EventArgs e)
        {
            snomedReaction = null;
            snomedTextBox.Text = "";
        }

        /// <summary>
        /// Deletes the allergy.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Allergy.IsNew)
            {
                DialogResult = DialogResult.Cancel;

                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.AllergyConfirmDelete,
                    Translation.Language.Allergy, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            PatientAllergy.Delete(Allergy.Id);

            SecurityLog.Write(
                Allergy.PatientId,
                SecurityLogEvents.PatientAlergyListEdited, 
                string.Format(
                    Translation.LanguageSecurity.GenericItemDeleted,
                    OpenDentBusiness.Allergy.GetDescription(Allergy.AllergyId)));

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the allergy and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            var dateTime = DateTime.MinValue;
            if (dateTextBox.Text != "")
            {
                if (!DateTime.TryParse(dateTextBox.Text, out dateTime))
                {
                    MessageBox.Show(
                        Translation.Language.EnterAValidDate,
                        Translation.Language.Allergy,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            Allergy.DateAdverseReaction = dateTime;
            Allergy.AllergyId = allergies[allergyComboBox.SelectedIndex].Id;
            Allergy.Reaction = reactionTextBox.Text;
            Allergy.SnomedReaction = snomedReaction?.SnomedCode ?? "";
            Allergy.Active = activeCheckBox.Checked;

            if (Allergy.IsNew)
            {
                PatientAllergy.Insert(Allergy);

                SecurityLog.Write(
                    Allergy.PatientId,
                    SecurityLogEvents.PatientAlergyListEdited,
                    string.Format(
                        Translation.LanguageSecurity.GenericItemAdded,
                        OpenDentBusiness.Allergy.GetDescription(Allergy.AllergyId)));
            }
            else
            {
                PatientAllergy.Update(Allergy);

                SecurityLog.Write(
                    Allergy.PatientId,
                    SecurityLogEvents.PatientAlergyListEdited,
                    string.Format(
                        Translation.LanguageSecurity.GenericItemModified,
                        OpenDentBusiness.Allergy.GetDescription(Allergy.AllergyId)));
            }

            DialogResult = DialogResult.OK;
        }
    }
}