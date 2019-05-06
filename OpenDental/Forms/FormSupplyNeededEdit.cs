using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyNeededEdit : FormBase
    {
        public SupplyNeeded Supp;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyNeededEdit"/> class.
        /// </summary>
        public FormSupplyNeededEdit() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyNeededEdit_Load(object sender, EventArgs e)
        {
            dateTextBox.Text = Supp.DateAdded.ToShortDateString();
            descriptionTextBox.Text = Supp.Description;
        }

        /// <summary>
        /// Deletes the needed supply.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Supp.IsNew)
            {
                DialogResult = DialogResult.Cancel;
            }

            SupplyNeededs.DeleteObject(Supp);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the needed supply and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (dateTextBox.errorProvider1.GetError(dateTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.SupplyNeeded,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Supp.DateAdded = PIn.Date(dateTextBox.Text);
            Supp.Description = descriptionTextBox.Text;

            if (Supp.IsNew)
            {
                SupplyNeededs.Insert(Supp);
            }
            else
            {
                SupplyNeededs.Update(Supp);
            }

            DialogResult = DialogResult.OK;
        }
    }
}