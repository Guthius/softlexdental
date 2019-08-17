using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUpdateInProgress : FormBase
    {
        readonly string updateComputerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUpdateInProgress"/> class.
        /// </summary>
        /// <param name="updateComputerName">The name of the computer.</param>
        public FormUpdateInProgress(string updateComputerName)
        {
            InitializeComponent();

            this.updateComputerName = updateComputerName;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUpdateInProgress_Load(object sender, EventArgs e)
        {
            string warningString =
                string.Format(
                    Translation.Language.AnUpdateIsInProgressOnWorkstation, 
                    updateComputerName);

            if (Security.IsAuthorized(Permissions.Setup, DateTime.Now, true))
            {
                warningString += "\r\n\r\n" + 
                    Translation.Language.ClickOverrideToOverrideUpdateMessage;

                overrideButton.Visible = true;
            }

            warningLabel.Text = warningString;
        }

        /// <summary>
        /// Checks whether the update has completed; if so closes the form.
        /// </summary>
        void RetryButton_Click(object sender, EventArgs e)
        {
            Preference.Refresh();

            if (Preference.GetString(PreferenceName.UpdateInProgressOnComputerName) != "")
            {
                MessageBox.Show(
                    string.Format(
                        Translation.Language.WorkstationIsStillUpdating, 
                        updateComputerName),
                    Translation.Language.Update, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Clears the name of the computer that is performing a update and closes the form.
        /// </summary>
        void OverrideButton_Click(object sender, EventArgs e)
        {
            Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");

            MessageBox.Show(
                Translation.Language.YouWillBeAllowedAccessWhenYouRestart,
                Translation.Language.Update, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            DialogResult = DialogResult.Cancel;
        }
    }
}