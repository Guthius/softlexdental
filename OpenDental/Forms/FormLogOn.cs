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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenDental
{
    [ODHelpButton]
    public partial class FormLogOn : FormBase
    {
        /// <summary>
        /// Used when temporarily switching users. Currently only used when overriding signed notes.
        /// The user will not be logged on (Security.CurUser is untouched) but CurUserSimpleSwitch will be set to the desired user.
        /// </summary>
        private bool simpleSwitch;

        /// <summary>
        /// Gets set to the user that just successfully logged in when in Simple Switch mode.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Set to true when the calling method has indicated that it will take care of any Security cache refreshing.
        /// </summary>
        readonly bool doRefreshSecurityCache = false;

        /// <summary>
        /// Set userNumSelected to automatically select the corresponding user in the list (if available).
        /// Set isSimpleSwitch true if temporarily switching users for some reason.  This will leave Security.CurUser alone and will instead
        /// indicate which user was chosen / successfully logged in via CurUserSimpleSwitch.
        /// </summary>
        public FormLogOn(long selectedUserId = 0, bool simpleSwitch = false, bool doRefreshSecurityCache = true)
        {
            InitializeComponent();

            Plugin.Trigger(this, "FormLogOn_Initialized", simpleSwitch);

            if (selectedUserId > 0)
            {
                usernameTextBox.Text = User.GetUserName(selectedUserId);
            }
            else if (Security.CurrentUser != null)
            {
                usernameTextBox.Text = Security.CurrentUser.UserName;
            }

            this.doRefreshSecurityCache = doRefreshSecurityCache;
            this.simpleSwitch = simpleSwitch;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormLogOn_Load(object sender, EventArgs e)
        {
            passwordTextBox.Select();

            Plugin.Trigger(this, "FormLogOn_Loaded", simpleSwitch);
        }

        /// <summary>
        /// Validates wether the username and password combination that was enterered is valid
        /// and checks whether the password is valid according to the active password policy.
        /// If everything is OK the current user is updated and the form will be closed.
        /// </summary>
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var userName = usernameTextBox.Text.Trim();
            if (userName.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.LoginFailed,
                    "Softlex Dental",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var password = Plugin.Filter(this, "FormLogOn_PasswordHash", passwordTextBox.Text, userName);

            try
            {
                User = User.CheckUserAndPassword(userName, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Softlex Dental", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (!simpleSwitch)
            {
                Security.CurrentUser = User;
                if (Preference.GetBool(PreferenceName.PasswordsMustBeStrong) && Preference.GetBool(PreferenceName.PasswordsWeakChangeToStrong))
                {
                    if (User.IsPasswordStrong(password) != "")
                    {
                        MessageBox.Show(
                            Translation.Language.YouMustChangeYourPasswordToAStrongPassword,
                            "Softlex Dental",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        if (!SecurityL.ChangePassword(true, doRefreshSecurityCache))
                        {
                            return;
                        }
                    }
                }

                Security.IsUserLoggedIn = true;

                SecurityLog.Write(
                    SecurityLogEvents.UserLogOnOff, 
                    string.Format(
                        Translation.LanguageSecurity.UserHasLoggedIn, 
                        Security.CurrentUser.UserName));
            }

            Plugin.Trigger(this, "FormLogOn_OK");

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Triggers the 'FormLogOn_Cancel' plugin action and closes the dialog.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Plugin.Trigger(this, "FormLogOn_Cancel");

            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Draws the header of the login form.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(
                Brushes.White, 
                new Rectangle(0, 0, ClientSize.Width, 80));

            using (var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(ClientSize.Width - 1, 6),
                Color.FromArgb(40, 110, 240),
                Color.FromArgb(0, 70, 140)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 74, ClientSize.Width, 6));
            }
        }
    }
}
