namespace OpenDental
{
    partial class FormGlobalSecurity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGlobalSecurity));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.domainPathLabel = new System.Windows.Forms.Label();
            this.domainPathTextBox = new System.Windows.Forms.TextBox();
            this.domainEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.forceChangeWeakPasswordsCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordsRequireSpecialCharacterCheckBox = new System.Windows.Forms.CheckBox();
            this.disableBackupReminderCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLogoffTextBox = new System.Windows.Forms.TextBox();
            this.logOffWithWindowsCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordsMustBeStrongCheckBox = new System.Windows.Forms.CheckBox();
            this.cannotEditOwnTimecardCheckBox = new System.Windows.Forms.CheckBox();
            this.timecardSecurityEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.domainGroupBox = new System.Windows.Forms.GroupBox();
            this.userGroupComboBox = new System.Windows.Forms.ComboBox();
            this.userGroupGroupBox = new System.Windows.Forms.GroupBox();
            this.autoLogoffGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.domainGroupBox.SuspendLayout();
            this.userGroupGroupBox.SuspendLayout();
            this.autoLogoffGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(485, 338);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(601, 338);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            // 
            // domainPathLabel
            // 
            this.domainPathLabel.AutoSize = true;
            this.domainPathLabel.Location = new System.Drawing.Point(6, 50);
            this.domainPathLabel.Name = "domainPathLabel";
            this.domainPathLabel.Size = new System.Drawing.Size(76, 15);
            this.domainPathLabel.TabIndex = 1;
            this.domainPathLabel.Text = "Domain Path";
            this.domainPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // domainPathTextBox
            // 
            this.domainPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainPathTextBox.Location = new System.Drawing.Point(6, 68);
            this.domainPathTextBox.Name = "domainPathTextBox";
            this.domainPathTextBox.ReadOnly = true;
            this.domainPathTextBox.Size = new System.Drawing.Size(360, 23);
            this.domainPathTextBox.TabIndex = 2;
            this.domainPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DomainPathTextBox_Validating);
            // 
            // domainEnabledCheckBox
            // 
            this.domainEnabledCheckBox.AutoSize = true;
            this.domainEnabledCheckBox.Location = new System.Drawing.Point(6, 22);
            this.domainEnabledCheckBox.Name = "domainEnabledCheckBox";
            this.domainEnabledCheckBox.Size = new System.Drawing.Size(146, 19);
            this.domainEnabledCheckBox.TabIndex = 0;
            this.domainEnabledCheckBox.Text = "Domain Login Enabled";
            this.domainEnabledCheckBox.CheckedChanged += new System.EventHandler(this.DomainEnabledCheckBox_CheckedChanged);
            // 
            // forceChangeWeakPasswordsCheckBox
            // 
            this.forceChangeWeakPasswordsCheckBox.AutoSize = true;
            this.forceChangeWeakPasswordsCheckBox.Location = new System.Drawing.Point(6, 172);
            this.forceChangeWeakPasswordsCheckBox.Name = "forceChangeWeakPasswordsCheckBox";
            this.forceChangeWeakPasswordsCheckBox.Size = new System.Drawing.Size(218, 19);
            this.forceChangeWeakPasswordsCheckBox.TabIndex = 6;
            this.forceChangeWeakPasswordsCheckBox.Text = "Force password change if not strong";
            // 
            // passwordsRequireSpecialCharacterCheckBox
            // 
            this.passwordsRequireSpecialCharacterCheckBox.AutoSize = true;
            this.passwordsRequireSpecialCharacterCheckBox.Location = new System.Drawing.Point(6, 147);
            this.passwordsRequireSpecialCharacterCheckBox.Name = "passwordsRequireSpecialCharacterCheckBox";
            this.passwordsRequireSpecialCharacterCheckBox.Size = new System.Drawing.Size(250, 19);
            this.passwordsRequireSpecialCharacterCheckBox.TabIndex = 5;
            this.passwordsRequireSpecialCharacterCheckBox.Text = "Strong passwords require special character";
            // 
            // disableBackupReminderCheckBox
            // 
            this.disableBackupReminderCheckBox.AutoSize = true;
            this.disableBackupReminderCheckBox.Location = new System.Drawing.Point(6, 72);
            this.disableBackupReminderCheckBox.Name = "disableBackupReminderCheckBox";
            this.disableBackupReminderCheckBox.Size = new System.Drawing.Size(205, 19);
            this.disableBackupReminderCheckBox.TabIndex = 2;
            this.disableBackupReminderCheckBox.Text = "Disable monthly backup reminder";
            // 
            // autoLogoffTextBox
            // 
            this.autoLogoffTextBox.Location = new System.Drawing.Point(6, 22);
            this.autoLogoffTextBox.Name = "autoLogoffTextBox";
            this.autoLogoffTextBox.Size = new System.Drawing.Size(40, 23);
            this.autoLogoffTextBox.TabIndex = 0;
            this.autoLogoffTextBox.Text = "0";
            this.autoLogoffTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.AutoLogoffTextBox_Validating);
            // 
            // logOffWithWindowsCheckBox
            // 
            this.logOffWithWindowsCheckBox.AutoSize = true;
            this.logOffWithWindowsCheckBox.Location = new System.Drawing.Point(6, 97);
            this.logOffWithWindowsCheckBox.Name = "logOffWithWindowsCheckBox";
            this.logOffWithWindowsCheckBox.Size = new System.Drawing.Size(191, 19);
            this.logOffWithWindowsCheckBox.TabIndex = 3;
            this.logOffWithWindowsCheckBox.Text = "Log off when Windows logs off";
            // 
            // passwordsMustBeStrongCheckBox
            // 
            this.passwordsMustBeStrongCheckBox.AutoSize = true;
            this.passwordsMustBeStrongCheckBox.Location = new System.Drawing.Point(6, 122);
            this.passwordsMustBeStrongCheckBox.Name = "passwordsMustBeStrongCheckBox";
            this.passwordsMustBeStrongCheckBox.Size = new System.Drawing.Size(164, 19);
            this.passwordsMustBeStrongCheckBox.TabIndex = 4;
            this.passwordsMustBeStrongCheckBox.Text = "Passwords must be strong";
            this.passwordsMustBeStrongCheckBox.CheckedChanged += new System.EventHandler(this.PasswordsMustBeStrongCheckBox_CheckedChanged);
            // 
            // cannotEditOwnTimecardCheckBox
            // 
            this.cannotEditOwnTimecardCheckBox.AutoSize = true;
            this.cannotEditOwnTimecardCheckBox.Enabled = false;
            this.cannotEditOwnTimecardCheckBox.Location = new System.Drawing.Point(21, 47);
            this.cannotEditOwnTimecardCheckBox.Name = "cannotEditOwnTimecardCheckBox";
            this.cannotEditOwnTimecardCheckBox.Size = new System.Drawing.Size(223, 19);
            this.cannotEditOwnTimecardCheckBox.TabIndex = 1;
            this.cannotEditOwnTimecardCheckBox.Text = "Users cannot edit their own time card";
            // 
            // timecardSecurityEnabledCheckBox
            // 
            this.timecardSecurityEnabledCheckBox.AutoSize = true;
            this.timecardSecurityEnabledCheckBox.Location = new System.Drawing.Point(6, 22);
            this.timecardSecurityEnabledCheckBox.Name = "timecardSecurityEnabledCheckBox";
            this.timecardSecurityEnabledCheckBox.Size = new System.Drawing.Size(167, 19);
            this.timecardSecurityEnabledCheckBox.TabIndex = 0;
            this.timecardSecurityEnabledCheckBox.Text = "Time card security enabled";
            this.timecardSecurityEnabledCheckBox.Click += new System.EventHandler(this.TimecardSecurityEnabledCheckBox_Click);
            // 
            // domainGroupBox
            // 
            this.domainGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainGroupBox.Controls.Add(this.domainEnabledCheckBox);
            this.domainGroupBox.Controls.Add(this.domainPathTextBox);
            this.domainGroupBox.Controls.Add(this.domainPathLabel);
            this.domainGroupBox.Location = new System.Drawing.Point(339, 85);
            this.domainGroupBox.Name = "domainGroupBox";
            this.domainGroupBox.Size = new System.Drawing.Size(372, 108);
            this.domainGroupBox.TabIndex = 5;
            this.domainGroupBox.TabStop = false;
            this.domainGroupBox.Text = "Domain Login";
            // 
            // userGroupComboBox
            // 
            this.userGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userGroupComboBox.Location = new System.Drawing.Point(6, 22);
            this.userGroupComboBox.MaxDropDownItems = 30;
            this.userGroupComboBox.Name = "userGroupComboBox";
            this.userGroupComboBox.Size = new System.Drawing.Size(308, 23);
            this.userGroupComboBox.TabIndex = 0;
            // 
            // userGroupGroupBox
            // 
            this.userGroupGroupBox.Controls.Add(this.userGroupComboBox);
            this.userGroupGroupBox.Location = new System.Drawing.Point(13, 235);
            this.userGroupGroupBox.Name = "userGroupGroupBox";
            this.userGroupGroupBox.Size = new System.Drawing.Size(320, 64);
            this.userGroupGroupBox.TabIndex = 3;
            this.userGroupGroupBox.TabStop = false;
            this.userGroupGroupBox.Text = "Default User Group";
            // 
            // autoLogoffGroupBox
            // 
            this.autoLogoffGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoLogoffGroupBox.Controls.Add(this.autoLogoffTextBox);
            this.autoLogoffGroupBox.Location = new System.Drawing.Point(339, 19);
            this.autoLogoffGroupBox.Name = "autoLogoffGroupBox";
            this.autoLogoffGroupBox.Size = new System.Drawing.Size(372, 60);
            this.autoLogoffGroupBox.TabIndex = 4;
            this.autoLogoffGroupBox.TabStop = false;
            this.autoLogoffGroupBox.Text = "Automatic logoff time in minutes (0 to disable)";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.timecardSecurityEnabledCheckBox);
            this.generalGroupBox.Controls.Add(this.cannotEditOwnTimecardCheckBox);
            this.generalGroupBox.Controls.Add(this.passwordsMustBeStrongCheckBox);
            this.generalGroupBox.Controls.Add(this.logOffWithWindowsCheckBox);
            this.generalGroupBox.Controls.Add(this.disableBackupReminderCheckBox);
            this.generalGroupBox.Controls.Add(this.forceChangeWeakPasswordsCheckBox);
            this.generalGroupBox.Controls.Add(this.passwordsRequireSpecialCharacterCheckBox);
            this.generalGroupBox.Location = new System.Drawing.Point(13, 19);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(320, 210);
            this.generalGroupBox.TabIndex = 2;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General Settings";
            // 
            // FormGlobalSecurity
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(724, 381);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.autoLogoffGroupBox);
            this.Controls.Add(this.userGroupGroupBox);
            this.Controls.Add(this.domainGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGlobalSecurity";
            this.ShowInTaskbar = false;
            this.Text = "Global Security Settings";
            this.Load += new System.EventHandler(this.FormGlobalSecurity_Load);
            this.domainGroupBox.ResumeLayout(false);
            this.domainGroupBox.PerformLayout();
            this.userGroupGroupBox.ResumeLayout(false);
            this.autoLogoffGroupBox.ResumeLayout(false);
            this.autoLogoffGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label domainPathLabel;
        private System.Windows.Forms.TextBox domainPathTextBox;
        private System.Windows.Forms.CheckBox domainEnabledCheckBox;
        private System.Windows.Forms.CheckBox forceChangeWeakPasswordsCheckBox;
        private System.Windows.Forms.CheckBox passwordsRequireSpecialCharacterCheckBox;
        private System.Windows.Forms.CheckBox disableBackupReminderCheckBox;
        private System.Windows.Forms.TextBox autoLogoffTextBox;
        private System.Windows.Forms.CheckBox logOffWithWindowsCheckBox;
        private System.Windows.Forms.CheckBox passwordsMustBeStrongCheckBox;
        private System.Windows.Forms.CheckBox cannotEditOwnTimecardCheckBox;
        private System.Windows.Forms.CheckBox timecardSecurityEnabledCheckBox;
        private System.Windows.Forms.GroupBox domainGroupBox;
        private System.Windows.Forms.ComboBox userGroupComboBox;
        private System.Windows.Forms.GroupBox userGroupGroupBox;
        private System.Windows.Forms.GroupBox autoLogoffGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
    }
}
