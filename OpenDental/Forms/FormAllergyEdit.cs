using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAllergyEdit : FormBase
    {
        List<AllergyDef> allergiesList;
        Snomed snomedReaction;

        public Allergy AllergyCur;

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
            allergiesList = AllergyDefs.GetAll(false);

            if (allergiesList.Count < 1)
            {
                MessageBox.Show(
                    "Need to set up at least one Allergy from EHR setup window.",
                    Translation.Language.Allergy, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                DialogResult = DialogResult.Cancel;
                return;
            }

            for (int i = 0; i < allergiesList.Count; i++)
            {
                allergyComboBox.Items.Add(allergiesList[i].Description);
                if (!AllergyCur.IsNew && allergiesList[i].AllergyDefNum == AllergyCur.AllergyDefNum)
                {
                    allergyIndex = i;
                }
            }

            // Get the SNOMED reaction assigned to the allergy.
            snomedReaction = Snomeds.GetByCode(AllergyCur.SnomedReaction);
            if (snomedReaction != null)
            {
                snomedTextBox.Text = snomedReaction.Description;
            }

            if (!AllergyCur.IsNew)
            {
                if (AllergyCur.DateAdverseReaction < DateTime.Parse("01-01-1880"))
                {
                    dateTextBox.Text = "";
                }
                else
                {
                    dateTextBox.Text = AllergyCur.DateAdverseReaction.ToShortDateString();
                }
                allergyComboBox.SelectedIndex = allergyIndex;
                reactionTextBox.Text = AllergyCur.Reaction;
                activeCheckBox.Checked = AllergyCur.StatusIsActive;
            }
            else
            {
                allergyComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Opens the form to select a SNOMED reaction to assign to the allergy.
        /// </summary>
        void snomedBrowseButton_Click(object sender, EventArgs e)
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
        void snomedNoneButton_Click(object sender, EventArgs e)
        {
            snomedReaction = null;
            snomedTextBox.Text = "";
        }

        /// <summary>
        /// Deletes the allergy.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            if (AllergyCur.IsNew)
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

            Allergies.Delete(AllergyCur.AllergyNum);

            SecurityLogs.MakeLogEntry(
                Permissions.PatAllergyListEdit, 
                AllergyCur.PatNum, 
                string.Format(
                    Translation.LanguageSecurity.GenericItemDeleted, 
                    AllergyDefs.GetDescription(AllergyCur.AllergyDefNum)));

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the allergy and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
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

            AllergyCur.DateAdverseReaction = dateTime;
            AllergyCur.AllergyDefNum = allergiesList[allergyComboBox.SelectedIndex].AllergyDefNum;
            AllergyCur.Reaction = reactionTextBox.Text;
            AllergyCur.SnomedReaction = snomedReaction?.SnomedCode ?? "";
            AllergyCur.StatusIsActive = activeCheckBox.Checked;

            if (AllergyCur.IsNew)
            {
                Allergies.Insert(AllergyCur);

                SecurityLogs.MakeLogEntry(
                    Permissions.PatAllergyListEdit, 
                    AllergyCur.PatNum,
                    string.Format(
                        Translation.LanguageSecurity.GenericItemAdded,
                        AllergyDefs.GetDescription(AllergyCur.AllergyDefNum)));
            }
            else
            {
                Allergies.Update(AllergyCur);

                SecurityLogs.MakeLogEntry(
                    Permissions.PatAllergyListEdit, 
                    AllergyCur.PatNum,
                    string.Format(
                        Translation.LanguageSecurity.GenericItemModified,
                        AllergyDefs.GetDescription(AllergyCur.AllergyDefNum)));
            }

            DialogResult = DialogResult.OK;
        }
    }
}