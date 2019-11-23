using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormProcButtonQuickEdit : FormBase
    {
        public ProcButtonQuick pbqCur;
        public bool IsNew;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProcButtonQuickEdit"/> class.
        /// </summary>
        public FormProcButtonQuickEdit() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormProcButtonQuickEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = pbqCur.Description;
            procedureCodeTextBox.Text = pbqCur.CodeValue;
            surfacesTextBox.Text = pbqCur.Surf;
            labelCheckBox.Checked = pbqCur.IsLabel;

            if (Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
            {
                surfacesLabel.Visible = false;
                surfacesTextBox.Visible = false;
            }
        }

        /// <summary>
        /// Toggles label mode on and off. When the button is a label the procedure code and surfaces fields will
        /// be disabled since they are irrelevant for labels.
        /// </summary>
        void LabelCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            procedureCodeTextBox.Enabled = !labelCheckBox.Checked;
            surfacesTextBox.Enabled = !labelCheckBox.Checked;
            pickProcedureButton.Enabled = !labelCheckBox.Checked;
        }

        /// <summary>
        /// Opens the form to select a procedure.
        /// </summary>
        void PickProcedureButton_Click(object sender, EventArgs e)
        {
            using (var formProcCodes = new FormProcCodes())
            {
                formProcCodes.IsSelectionMode = true;
                if (formProcCodes.ShowDialog() == DialogResult.OK)
                {
                    procedureCodeTextBox.Text = ProcedureCodes.GetProcCode(formProcCodes.SelectedCodeNum).ProcCode;
                }
            }
        }

        /// <summary>
        /// Deletes the button.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                pbqCur = null;

                DialogResult = DialogResult.Cancel;
            }
            else
            {
                ProcButtonQuicks.Delete(pbqCur.ProcButtonQuickNum);

                pbqCur = null;

                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Saves the button and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            pbqCur.Description = descriptionTextBox.Text;
            pbqCur.CodeValue = procedureCodeTextBox.Text;
            pbqCur.Surf = surfacesTextBox.Text;
            pbqCur.IsLabel = labelCheckBox.Checked;

            if (IsNew)
            {
                ProcButtonQuicks.Insert(pbqCur);
            }
            else
            {
                ProcButtonQuicks.Update(pbqCur);
            }

            DialogResult = DialogResult.OK;
        }
    }
}