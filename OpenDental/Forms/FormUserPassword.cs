using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Summary description for FormBasicTemplate.
    /// </summary>
    public partial class FormUserPassword : FormBase
    {
        public PasswordContainer LoginDetails;

        bool isCreate;
        bool isPasswordReset;

        /// <summary>
        /// Gets or sets a value indicating whether this is a security window.
        /// </summary>
        public bool IsInSecurityWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the specified password is considered a strong password.
        /// </summary>
        public bool PasswordIsStrong { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get => newPasswordTextBox.Text;
            set
            {
                newPasswordTextBox.Text = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserPassword"/> class.
        /// </summary>
        /// <param name="isCreate">Set true if creating rather than changing a password.</param>
        public FormUserPassword(bool isCreate, string username, bool isPasswordReset = false)
        {
            InitializeComponent();

            this.isCreate = isCreate;
            this.isPasswordReset = isPasswordReset;

            userNameTextBox.Text = username;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserPassword_Load(object sender, EventArgs e)
        {
            if (isCreate)
            {
                Text = Translation.Language.lang_create_password;
            }

            if (IsInSecurityWindow)
            {
                currentPasswordLabel.Visible = false;
                currentPasswordTextBox.Visible = false;
            }

            if (isPasswordReset)
            {
                currentPasswordLabel.Text = Translation.Language.lang_new_password;
                newPasswordLabel.Text = Translation.Language.lang_re_enter_password;
                cancelButton.Visible = false;
                acceptButton.Location = cancelButton.Location;

                ControlBox = false;
            }
        }

        /// <summary>
        /// Toggle between showing and hiding the password.
        /// </summary>
        void showCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showCheckBox.Checked)
            {
                newPasswordTextBox.PasswordChar = default(char);
            }
            else
            {
                newPasswordTextBox.PasswordChar = '*';
            }
        }

        /// <summary>
        /// Validates whether a valid password was entered and if so closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acceptButton_Click(object sender, System.EventArgs e)
        {
            if (isPasswordReset)
            {
                if (newPasswordTextBox.Text != currentPasswordTextBox.Text || string.IsNullOrWhiteSpace(newPasswordTextBox.Text))
                {
                    MessageBox.Show(
                        Translation.Language.PasswordsMustMatchAndNotBeEmpty,
                        Translation.Language.ChangePassword, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
            }
            else if (!IsInSecurityWindow && !Authentication.CheckPassword(Security.CurUser, currentPasswordTextBox.Text))
            {
                MessageBox.Show(
                    Translation.Language.CurrentPasswordIncorrect,
                    Translation.Language.ChangePassword, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            string explanation = Userods.IsPasswordStrong(newPasswordTextBox.Text);
            if (Preference.GetBool(PreferenceName.PasswordsMustBeStrong))
            {
                if (explanation != "")
                {
                    MessageBox.Show(
                        explanation,
                        Translation.Language.ChangePassword, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
            }

            // If the PasswordsMustBeStrong preference is off, still store whether or not the password is strong in case the preference is turned on later
            PasswordIsStrong = string.IsNullOrEmpty(explanation);
            if (Programs.UsingEcwTightOrFullMode())
            {
                LoginDetails = Authentication.GenerateLoginDetails(newPasswordTextBox.Text, HashTypes.MD5_ECW);
            }
            else
            {
                LoginDetails = Authentication.GenerateLoginDetailsSHA512(newPasswordTextBox.Text);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
