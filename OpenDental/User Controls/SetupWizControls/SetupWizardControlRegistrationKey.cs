using OpenDentBusiness;
using System;
using System.ComponentModel;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class SetupWizardControlRegistrationKey : SetupWizardControl
    {
        public SetupWizardControlRegistrationKey() => InitializeComponent();

        void SetupWizardControlRegistrationKey_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin, true))
            {
                changeButton.Enabled = false;
            }
            LoadRegistrationKey();
        }

        void LoadRegistrationKey()
        {
            textRegKey.Text = License.FormatKey(Preferences.GetString(PrefName.RegistrationKey));

            IsDone = !string.IsNullOrEmpty(textRegKey.Text);

            Error = "Please click the 'Change' button and type in your registration key.";

            groupProcTools.Enabled = IsDone;
        }

        void ChangeButton_Click(object sender, EventArgs e)
        {
            using (var formRegistrationKey = new FormRegistrationKey())
            {
                formRegistrationKey.ShowDialog(this);
            }

            DataValid.SetInvalid(InvalidType.Prefs);

            textRegKey.Text = License.FormatKey(Preferences.GetString(PrefName.RegistrationKey));

            IsDone = !string.IsNullOrEmpty(textRegKey.Text);
            groupProcTools.Enabled = IsDone;
        }

        void ProcedureCodesButton_Click(object sender, EventArgs e)
        {
            using (var formProcTools = new FormProcTools())
            {
                formProcTools.ShowDialog(this);
            }
        }

        void AdvancedButton_Click(object sender, EventArgs e)
        {
            using (var formUpdateSetup = new FormUpdateSetup())
            {
                formUpdateSetup.ShowDialog(this);

            }
            LoadRegistrationKey();
        }
    }
}