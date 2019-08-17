using OpenDentBusiness;
using System;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAllergyDefEdit : FormBase
    {
        public AllergyDef AllergyDefCur;

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
                hiddenCheckBox.Checked = AllergyDefCur.IsHidden;
            }

            for (int i = 0; i < Enum.GetNames(typeof(SnomedAllergy)).Length; i++)
            {
                allergyTypeComboBox.Items.Add(Enum.GetNames(typeof(SnomedAllergy))[i]);
            }
            allergyTypeComboBox.SelectedIndex = (int)AllergyDefCur.SnomedType;

            medicationTextBox.Text = Medication.GetDescription(AllergyDefCur.MedicationNum);
            uniiTextBox.Text = AllergyDefCur.UniiCode;
        }

        /// <summary>
        /// Opens the form to select a UNII code.
        /// </summary>
        void uniiBrowseButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement this
        }

        /// <summary>
        /// Clears the UNII code.
        /// </summary>
        void uniiNoneButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement this
        }

        /// <summary>
        /// Opens the form to select a medication.
        /// </summary>
        void medicationBrowseButton_Click(object sender, EventArgs e)
        {
            using (var formMedications = new FormMedications())
            {
                formMedications.IsSelectionMode = true;
                if (formMedications.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                AllergyDefCur.MedicationNum = formMedications.SelectedMedicationNum;

                medicationTextBox.Text = Medication.GetDescription(AllergyDefCur.MedicationNum);
            }
        }

        /// <summary>
        /// Clears the selected medication.
        /// </summary>
        void medicationNoneButton_Click(object sender, EventArgs e)
        {
            AllergyDefCur.MedicationNum = 0;
            medicationTextBox.Text = "";
        }

        /// <summary>
        /// Delete the allergy definition. Only allowed if the allergy is not in use.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            if (AllergyDefCur.Id > 0)
            {
                if (!AllergyDefs.DefIsInUse(AllergyDefCur.Id))
                {
                    var result = 
                        MessageBox.Show(
                            Translation.Language.AllergyDefConfirmDelete,
                            Translation.Language.AllergyDef,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel) return;

                    AllergyDefs.Delete(AllergyDefCur.Id);
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
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (descriptionTextBox.Text.Trim() == "")
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

            var invalidCharacters = new StringBuilder();
            for (int i = 0; i < uniiTextBox.Text.Length; i++)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".IndexOf(uniiTextBox.Text[i]) == -1)
                {
                    invalidCharacters.Append(uniiTextBox.Text[i]);
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

            if (uniiTextBox.Text != "" && uniiTextBox.Text.Length != 10)
            {
                MessageBox.Show(
                    Translation.Language.AllergyUniiCodeMustBe10Characters,
                    Translation.Language.AllergyDef,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            AllergyDefCur.Description = descriptionTextBox.Text;
            AllergyDefCur.IsHidden = hiddenCheckBox.Checked;
            AllergyDefCur.SnomedType = (SnomedAllergy)allergyTypeComboBox.SelectedIndex;
            AllergyDefCur.UniiCode = uniiTextBox.Text;

            // TODO: Do UNII check once the table is added

            if (AllergyDefCur.Id == 0)
            {
                AllergyDefs.Insert(AllergyDefCur);
            }
            else
            {
                AllergyDefs.Update(AllergyDefCur);
            }

            DialogResult = DialogResult.OK;
        }
    }
}