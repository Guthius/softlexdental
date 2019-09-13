using System;
using System.Linq;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDentBusiness;

namespace OpenDental
{
    /// <summary>
    /// Used for user-specific settings that are unique to the Oryx bridge.
    /// </summary>
    public partial class FormOryxUserSettings : ODForm
    {
        public FormOryxUserSettings()
        {
            InitializeComponent();
        }

        private void FormUserSetting_Load(object sender, EventArgs e)
        {
            textUsername.Text = UserPreference.GetString(Security.CurrentUser.Id, Oryx.Preferences.Username);

            var password = UserPreference.GetString(Security.CurrentUser.Id, Oryx.Preferences.Password);
            if (!string.IsNullOrEmpty(password))
            {
                string passwordPlain;
                Encryption.TryDecrypt(password, out passwordPlain);
                textPassword.Text = passwordPlain;
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            UserPreference.Update(Security.CurrentUser.Id, Oryx.Preferences.Username, textUsername.Text);

            Encryption.TryEncrypt(textPassword.Text, out var password);

            UserPreference.Update(Security.CurrentUser.Id, Oryx.Preferences.Password, password);

            DialogResult = DialogResult.OK;

            Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}