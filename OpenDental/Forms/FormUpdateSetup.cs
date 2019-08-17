using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUpdateSetup : FormBase
    {
        string registrationKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUpdateSetup"/> class.
        /// </summary>
        public FormUpdateSetup() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUpdateSetup_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin, true))
            {
                registrationKeyButton.Enabled = false;
                acceptButton.Enabled = false;
            }

            addressTextBox.Text = Preference.GetString(PreferenceName.UpdateServerAddress);
            proxyAddresTextBox.Text = Preference.GetString(PreferenceName.UpdateWebProxyAddress);
            proxyUsernameTextBox.Text = Preference.GetString(PreferenceName.UpdateWebProxyUserName);
            proxyPasswordTextBox.Text = Preference.GetString(PreferenceName.UpdateWebProxyPassword);

            registrationKey = Preference.GetString(PreferenceName.RegistrationKey);
            registrationKeyTextBox.Text = License.FormatKey(registrationKey);
        }

        /// <summary>
        /// Creates a log entry when the form is closing.
        /// </summary>
        void FormUpdateSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            var permission = 
                DialogResult == DialogResult.OK ? 
                    Permissions.SecurityAdmin : 
                    Permissions.Setup;

            SecurityLogs.MakeLogEntry(permission, 0, 
                Translation.LanguageSecurity.UpdateSetupWindowAccessed);
        }

        /// <summary>
        /// Opens the form to change the registration key.
        /// </summary>
        void RegistrationKeyButton_Click(object sender, EventArgs e)
        {
            using (var formRegistrationKey = new FormRegistrationKey())
            {
                formRegistrationKey.ShowDialog();

                DataValid.SetInvalid(InvalidType.Prefs);

                registrationKey = Preference.GetString(PreferenceName.RegistrationKey);
                registrationKeyTextBox.Text = License.FormatKey(registrationKey);
            }
        }

        /// <summary>
        /// Validates and saves the settings and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            bool refreshCache = false;
            if (Preference.Update(PreferenceName.UpdateServerAddress, addressTextBox.Text) |
                Preference.Update(PreferenceName.UpdateWebProxyAddress, proxyAddresTextBox.Text) |
                Preference.Update(PreferenceName.UpdateWebProxyUserName, proxyUsernameTextBox.Text) |
                Preference.Update(PreferenceName.UpdateWebProxyPassword, proxyPasswordTextBox.Text))
            {
                refreshCache = true;
            }

            if (refreshCache)
            {
                Cursor = Cursors.WaitCursor;

                DataValid.SetInvalid(InvalidType.Prefs);

                Cursor = Cursors.Default;
            }

            DialogResult = DialogResult.OK;
        }
    }
}