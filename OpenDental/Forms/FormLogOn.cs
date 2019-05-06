using OpenDentBusiness;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
        private bool isSimpleSwitch;

        /// <summary>
        /// Gets set to the user that just successfully logged in when in Simple Switch mode.
        /// </summary>
        public User CurUserSimpleSwitch;
        
        /// <summary>
        /// If set AND available, this will be the user automatically selected when the form opens.
        /// </summary>
        string userNameAutoSelect;

        /// <summary>
        /// Set to true when the calling method has indicated that it will take care of any Security cache refreshing.
        /// </summary>
        bool doRefreshSecurityCache = false;

        /// <summary>
        /// Will be true when the calling method needs to refresh the security cache themselves due to changes.
        /// </summary>
        public bool RefreshSecurityCache { get; private set; } = false;

        /// <summary>
        /// Set userNumSelected to automatically select the corresponding user in the list (if available).
        /// Set isSimpleSwitch true if temporarily switching users for some reason.  This will leave Security.CurUser alone and will instead
        /// indicate which user was chosen / successfully logged in via CurUserSimpleSwitch.
        /// </summary>
        public FormLogOn(long userNumSelected = 0, bool isSimpleSwitch = false, bool doRefreshSecurityCache = true)
        {
            InitializeComponent();

            Plugin.Trigger(this, "FormLogOn_Initialized", isSimpleSwitch);

            if (userNumSelected > 0)
            {
                userNameAutoSelect = Userods.GetUserNameNoCache(userNumSelected);
            }
            else if (Security.CurUser != null)
            {
                userNameAutoSelect = Security.CurUser.UserName;
            }

            this.doRefreshSecurityCache = doRefreshSecurityCache;
            this.isSimpleSwitch = isSimpleSwitch;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormLogOn_Load(object sender, EventArgs e)
        {
            var textSelectOnLoad = passwordTextBox;

            if (Preferences.GetBool(PrefName.UserNameManualEntry))
            {
                userListBox.Visible = false;
                userTextBox.Visible = true;

                // Focus should start with user name text box.
                textSelectOnLoad = userTextBox; 
            }
            else
            {
                //Only show the show CEMT user check box if not manually typing user names and there are CEMT users present in the db.
                CEMTUsersCheckBox.Visible = Userods.HasUsersForCEMTNoCache();
            }

            LoadUsers();

            // Attempted fix, customers had issue with UI not defaulting focus to this form on startup.
            Focus(); 

            // Give focus to appropriate text box.
            textSelectOnLoad.Select(); 

            Plugin.Trigger(this, "FormLogOn_Loaded", isSimpleSwitch);
        }

        /// <summary>
        /// Move focus to the password field when a user is selected from the list.
        /// </summary>
        void listUser_MouseUp(object sender, MouseEventArgs e) => passwordTextBox.Focus();
        
        /// <summary>
        /// Fills the User list with non-hidden, non-CEMT user names.
        /// Only shows non-hidden CEMT users if Show CEMT users is checked.
        /// </summary>
        void LoadUsers()
        {
            userListBox.BeginUpdate();
            userListBox.Items.Clear();

            var userNameList = Userods.GetUserNamesNoCache(CEMTUsersCheckBox.Checked);
            foreach (var userName in userNameList)
            {
                userListBox.Items.Add(userName);
                if (userNameAutoSelect != null && userNameAutoSelect.Trim().ToLower() == userName.Trim().ToLower())
                {
                    userListBox.SelectedIndex = userListBox.Items.Count - 1;
                }
            }

            // It is possible there are no users in the list if all users are CEMT users.
            if (userListBox.SelectedIndex == -1 && userListBox.Items.Count > 0)
            {
                userListBox.SelectedIndex = 0;
            }

            userListBox.EndUpdate();
        }

        /// <summary>
        /// Reload the list of users when the CEMT checkbox state changes.
        /// </summary>
        void CEMTUsersCheckBox_CheckedChanged(object sender, EventArgs e) => LoadUsers();
        
        /// <summary>
        /// Validates wether the username and password combination that was enterered is valid
        /// and checks whether the password is valid according to the active password policy.
        /// If everything is OK the current user is updated and the form will be closed.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            bool    isEcw       = Programs.UsingEcwTightOrFullMode();
            string  userName    = "";
            User  userCur     = null;

            if (Preferences.GetBool(PrefName.UserNameManualEntry))
            {
                // Check the user name using ToLower and Trim because Open Dental is case insensitive and does not allow white-space in regards to user names.
                userName = userListBox.Items.Cast<string>().FirstOrDefault(x => x.Trim().ToLower() == userTextBox.Text.Trim().ToLower());
            }
            else
            {
                userName = userListBox.SelectedItem?.ToString();
            }

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show(this, Translation.Language.lang_login_failed);
                return;
            }

            var passwordTyped = Plugin.Filter(this, "FormLogOn_PasswordHash", passwordTextBox.Text, userName);

            // ECW requires hash, but non-ecw requires actual password
            if (isEcw) passwordTyped = Authentication.HashPasswordMD5(passwordTyped, true);

            try
            {
                userCur = Userods.CheckUserAndPassword(userName, passwordTyped, isEcw);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }
            

            // successful login.
            if (isSimpleSwitch)
            {
                CurUserSimpleSwitch = userCur;
            }
            else
            {
                // Not a temporary login.
                Security.CurUser = userCur; // Need to set for SecurityL.ChangePassword and calls.
                if (Preferences.GetBool(PrefName.PasswordsMustBeStrong) && Preferences.GetBool(PrefName.PasswordsWeakChangeToStrong))
                {
                    // Check whether the password is strong enough.
                    if (Userods.IsPasswordStrong(passwordTyped) != "")
                    {
                        MessageBox.Show(this, Translation.Language.lang_you_must_change_password_to_stronger_password);
                        if (!SecurityL.ChangePassword(true, doRefreshSecurityCache))
                        {
                            return;
                        }

                        // Indicate to calling method that they should manually refresh the Security cache.
                        RefreshSecurityCache = true; 
                    }
                }

                Security.IsUserLoggedIn = true;

                // Jason approved always storing the cleartext password that the user typed in 
                // since this is necessary for Reporting Servers over middle tier and was already happening when a user logged in over middle tier.
                Security.PasswordTyped = passwordTyped;
                SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, string.Format(Translation.Language.lang_user_has_logged_on, Security.CurUser.UserName));
            }
            Plugin.Trigger(this, "FormLogOn_OK");

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Triggers the 'FormLogOn_Cancel' plugin action and closes the dialog.
        /// </summary>
        void cancelButton_Click(object sender, EventArgs e)
        {
            Plugin.Trigger(this, "FormLogOn_Cancel");

            DialogResult = DialogResult.Cancel;
        }

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
