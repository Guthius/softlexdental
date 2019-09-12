using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    /// <summary>Used for user-specific settings that are unique to the Oryx bridge.</summary>
    public partial class FormOryxUserSettings : ODForm
    {
        ///<summary>User pref holding the user's Oryx username.</summary>
        private UserPreference _userNamePref;
        ///<summary>User pref holding the user's Oryx password.</summary>
        private UserPreference _passwordPref;
        ///<summary>Oryx program bridge.</summary>
        private Program _progOryx;

        public FormOryxUserSettings()
        {
            InitializeComponent();
        }

        private void FormUserSetting_Load(object sender, EventArgs e)
        {
            _progOryx = Programs.GetCur(ProgramName.Oryx);
            _userNamePref = UserOdPrefs.GetByUserFkeyAndFkeyType(Security.CurrentUser.Id, _progOryx.ProgramNum, UserPreferenceName.ProgramUserName)
                .FirstOrDefault();
            _passwordPref = UserOdPrefs.GetByUserFkeyAndFkeyType(Security.CurrentUser.Id, _progOryx.ProgramNum, UserPreferenceName.ProgramPassword)
                .FirstOrDefault();
            if (_userNamePref != null)
            {
                textUsername.Text = _userNamePref.Value;
            }
            if (_passwordPref != null)
            {
                string passwordPlain;
                Encryption.TryDecrypt(_passwordPref.Value, out passwordPlain);
                textPassword.Text = passwordPlain;
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            _userNamePref = _userNamePref ?? new UserPreference
            {
                Fkey = _progOryx.ProgramNum,
                FkeyType = UserPreferenceName.ProgramUserName,
                UserId = Security.CurrentUser.Id,
            };
            _passwordPref = _passwordPref ?? new UserPreference
            {
                Fkey = _progOryx.ProgramNum,
                FkeyType = UserPreferenceName.ProgramPassword,
                UserId = Security.CurrentUser.Id,
            };
            _userNamePref.Value = textUsername.Text;
            Encryption.TryEncrypt(textPassword.Text, out _passwordPref.Value);
            UserOdPrefs.Upsert(_userNamePref);
            UserOdPrefs.Upsert(_passwordPref);
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