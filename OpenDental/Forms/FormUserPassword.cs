using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Summary description for FormBasicTemplate.
    /// </summary>
    public class FormUserPassword : FormBase
    {
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label newPasswordLabel;
        private System.Windows.Forms.TextBox newPasswordTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox currentPasswordTextBox;
        private System.Windows.Forms.Label currentPasswordLabel;
        private System.Windows.Forms.CheckBox showCheckBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private bool IsCreate;
        private bool _isPasswordReset;
        public bool IsInSecurityWindow;
        public bool PasswordIsStrong;
        public string PasswordTyped;
        public PasswordContainer LoginDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserPassword"/> class.
        /// </summary>
        /// <param name="isCreate">Set true if creating rather than changing a password.</param>
        public FormUserPassword(bool isCreate, string username, bool isPasswordReset = false)
        {
            InitializeComponent();

            IsCreate = isCreate;
            userNameTextBox.Text = username;
            _isPasswordReset = isPasswordReset;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.newPasswordLabel = new System.Windows.Forms.Label();
            this.newPasswordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.currentPasswordTextBox = new System.Windows.Forms.TextBox();
            this.currentPasswordLabel = new System.Windows.Forms.Label();
            this.showCheckBox = new System.Windows.Forms.CheckBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newPasswordLabel
            // 
            this.newPasswordLabel.Location = new System.Drawing.Point(13, 76);
            this.newPasswordLabel.Name = "newPasswordLabel";
            this.newPasswordLabel.Size = new System.Drawing.Size(149, 18);
            this.newPasswordLabel.TabIndex = 4;
            this.newPasswordLabel.Text = "New Password";
            this.newPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // newPasswordTextBox
            // 
            this.newPasswordTextBox.Location = new System.Drawing.Point(168, 75);
            this.newPasswordTextBox.Name = "newPasswordTextBox";
            this.newPasswordTextBox.PasswordChar = '*';
            this.newPasswordTextBox.Size = new System.Drawing.Size(203, 23);
            this.newPasswordTextBox.TabIndex = 5;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(168, 19);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.ReadOnly = true;
            this.userNameTextBox.Size = new System.Drawing.Size(203, 23);
            this.userNameTextBox.TabIndex = 1;
            // 
            // userLabel
            // 
            this.userLabel.Location = new System.Drawing.Point(13, 20);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(149, 18);
            this.userLabel.TabIndex = 0;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // currentPasswordTextBox
            // 
            this.currentPasswordTextBox.Location = new System.Drawing.Point(168, 47);
            this.currentPasswordTextBox.Name = "currentPasswordTextBox";
            this.currentPasswordTextBox.PasswordChar = '*';
            this.currentPasswordTextBox.Size = new System.Drawing.Size(203, 23);
            this.currentPasswordTextBox.TabIndex = 3;
            // 
            // currentPasswordLabel
            // 
            this.currentPasswordLabel.Location = new System.Drawing.Point(13, 48);
            this.currentPasswordLabel.Name = "currentPasswordLabel";
            this.currentPasswordLabel.Size = new System.Drawing.Size(149, 18);
            this.currentPasswordLabel.TabIndex = 2;
            this.currentPasswordLabel.Text = "Current Password";
            this.currentPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // showCheckBox
            // 
            this.showCheckBox.AutoSize = true;
            this.showCheckBox.Location = new System.Drawing.Point(168, 104);
            this.showCheckBox.Name = "showCheckBox";
            this.showCheckBox.Size = new System.Drawing.Size(55, 19);
            this.showCheckBox.TabIndex = 6;
            this.showCheckBox.Text = "Show";
            this.showCheckBox.UseVisualStyleBackColor = true;
            this.showCheckBox.CheckedChanged += new System.EventHandler(this.showCheckBox_CheckedChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(145, 158);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(261, 158);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormUserPassword
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 201);
            this.Controls.Add(this.showCheckBox);
            this.Controls.Add(this.currentPasswordTextBox);
            this.Controls.Add(this.currentPasswordLabel);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.newPasswordTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.newPasswordLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserPassword";
            this.ShowInTaskbar = false;
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.FormUserPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        void FormUserPassword_Load(object sender, EventArgs e)
        {
            if (IsCreate)
            {
                Text = Translation.Language.lang_create_password;
            }

            if (IsInSecurityWindow)
            {
                currentPasswordLabel.Visible = false;
                currentPasswordTextBox.Visible = false;
            }

            if (_isPasswordReset)
            {
                currentPasswordLabel.Text = Translation.Language.lang_new_password;
                newPasswordLabel.Text = Translation.Language.lang_re_enter_password;
                cancelButton.Visible = false;
                acceptButton.Location = cancelButton.Location;

                ControlBox = false;
            }
        }

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

        void acceptButton_Click(object sender, System.EventArgs e)
        {
            if (_isPasswordReset)
            {
                if (newPasswordTextBox.Text != currentPasswordTextBox.Text || string.IsNullOrWhiteSpace(newPasswordTextBox.Text))
                {
                    MsgBox.Show(this, Translation.Language.lang_password_must_match_and_not_be_empty);
                    return;
                }
            }
            else if (!IsInSecurityWindow && !Authentication.CheckPassword(Security.CurUser, currentPasswordTextBox.Text))
            {
                MsgBox.Show(this, Translation.Language.lang_current_password_incorrect);
                return;
            }

            string explanation = Userods.IsPasswordStrong(newPasswordTextBox.Text);
            if (PrefC.GetBool(PrefName.PasswordsMustBeStrong))
            {
                if (explanation != "")
                {
                    MessageBox.Show(explanation);
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

            PasswordTyped = newPasswordTextBox.Text; // update the stored typed password for middle tier refresh
            DialogResult = DialogResult.OK;
        }
    }
}
