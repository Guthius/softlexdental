/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserPassword : FormBase
    {
        private readonly bool isCreate;
        private readonly bool isPasswordReset;

        public PasswordContainer LoginDetails { get; set; }

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
        private void FormUserPassword_Load(object sender, EventArgs e)
        {
            if (isCreate)
            {
                Text = Translation.Language.CreatePassword;
            }

            if (IsInSecurityWindow)
            {
                currentPasswordLabel.Visible = false;
                currentPasswordTextBox.Visible = false;
            }

            if (isPasswordReset)
            {
                currentPasswordLabel.Text = Translation.Language.NewPassword;
                newPasswordLabel.Text = Translation.Language.ReEnterPassword;
                cancelButton.Visible = false;
                acceptButton.Location = cancelButton.Location;

                ControlBox = false;
            }
        }

        /// <summary>
        /// Toggle between showing and hiding the password.
        /// </summary>
        private void ShowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showCheckBox.Checked)
            {
                newPasswordTextBox.PasswordChar = default;
            }
            else
            {
                newPasswordTextBox.PasswordChar = '*';
            }
        }

        /// <summary>
        /// Validates whether a valid password was entered and if so closes the form.
        /// </summary>
        private void AcceptButton_Click(object sender, EventArgs e)
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
            else if (!IsInSecurityWindow && !Authentication.CheckPassword(Security.CurrentUser, currentPasswordTextBox.Text))
            {
                MessageBox.Show(
                    Translation.Language.CurrentPasswordIncorrect,
                    Translation.Language.ChangePassword,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string explanation = User.IsPasswordStrong(newPasswordTextBox.Text);
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

            PasswordIsStrong = string.IsNullOrEmpty(explanation);

            LoginDetails = Authentication.GenerateLoginDetailsSHA512(newPasswordTextBox.Text);

            DialogResult = DialogResult.OK;
        }
    }
}
