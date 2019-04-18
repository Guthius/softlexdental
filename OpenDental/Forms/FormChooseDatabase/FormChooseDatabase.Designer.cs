namespace OpenDental
{
    partial class FormChooseDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseDatabase));
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.computerNameInfoLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.databaseComboBox = new System.Windows.Forms.ComboBox();
            this.computerNameComboBox = new System.Windows.Forms.ComboBox();
            this.computerNameLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.checkNoShow = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.settingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGroupBox.Controls.Add(this.computerNameInfoLabel);
            this.settingsGroupBox.Controls.Add(this.userTextBox);
            this.settingsGroupBox.Controls.Add(this.databaseComboBox);
            this.settingsGroupBox.Controls.Add(this.computerNameComboBox);
            this.settingsGroupBox.Controls.Add(this.computerNameLabel);
            this.settingsGroupBox.Controls.Add(this.passwordTextBox);
            this.settingsGroupBox.Controls.Add(this.passwordLabel);
            this.settingsGroupBox.Controls.Add(this.userLabel);
            this.settingsGroupBox.Controls.Add(this.databaseLabel);
            this.settingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.settingsGroupBox.Location = new System.Drawing.Point(13, 19);
            this.settingsGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.settingsGroupBox.Size = new System.Drawing.Size(418, 300);
            this.settingsGroupBox.TabIndex = 31;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Connection Settings";
            // 
            // computerNameInfoLabel
            // 
            this.computerNameInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.computerNameInfoLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.computerNameInfoLabel.Location = new System.Drawing.Point(3, 70);
            this.computerNameInfoLabel.Name = "computerNameInfoLabel";
            this.computerNameInfoLabel.Size = new System.Drawing.Size(409, 60);
            this.computerNameInfoLabel.TabIndex = 7;
            this.computerNameInfoLabel.Text = "The name of the computer where the MySQL server and database are located.  If you" +
    " are running this program on a single computer only, then the computer name may " +
    "be localhost.";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(6, 148);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(280, 23);
            this.userTextBox.TabIndex = 3;
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.DropDownHeight = 390;
            this.databaseComboBox.IntegralHeight = false;
            this.databaseComboBox.Location = new System.Drawing.Point(6, 250);
            this.databaseComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.databaseComboBox.MaxDropDownItems = 100;
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(280, 23);
            this.databaseComboBox.TabIndex = 2;
            this.databaseComboBox.DropDown += new System.EventHandler(this.databaseComboBox_DropDown);
            // 
            // computerNameComboBox
            // 
            this.computerNameComboBox.DropDownHeight = 390;
            this.computerNameComboBox.IntegralHeight = false;
            this.computerNameComboBox.Location = new System.Drawing.Point(6, 44);
            this.computerNameComboBox.MaxDropDownItems = 100;
            this.computerNameComboBox.Name = "computerNameComboBox";
            this.computerNameComboBox.Size = new System.Drawing.Size(280, 23);
            this.computerNameComboBox.TabIndex = 1;
            // 
            // computerNameLabel
            // 
            this.computerNameLabel.AutoSize = true;
            this.computerNameLabel.Location = new System.Drawing.Point(3, 26);
            this.computerNameLabel.Name = "computerNameLabel";
            this.computerNameLabel.Size = new System.Drawing.Size(74, 15);
            this.computerNameLabel.TabIndex = 0;
            this.computerNameLabel.Text = "Server Name";
            this.computerNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(6, 199);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(280, 23);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(3, 181);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(98, 15);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "MySQL Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(3, 130);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(71, 15);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "MySQL User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(3, 232);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(327, 15);
            this.databaseLabel.TabIndex = 6;
            this.databaseLabel.Text = "Database (usually opendental unless you changed the name)";
            this.databaseLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkNoShow
            // 
            this.checkNoShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkNoShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkNoShow.Location = new System.Drawing.Point(13, 388);
            this.checkNoShow.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.checkNoShow.Name = "checkNoShow";
            this.checkNoShow.Size = new System.Drawing.Size(418, 20);
            this.checkNoShow.TabIndex = 5;
            this.checkNoShow.Text = "Do not show this window on startup";
            this.checkNoShow.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(321, 340);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 36;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(205, 340);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 35;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // FormChooseDatabase
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 421);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkNoShow);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseDatabase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Database";
            this.Load += new System.EventHandler(this.ChooseDatabaseView_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.ComboBox databaseComboBox;
        private System.Windows.Forms.CheckBox checkNoShow;
        private System.Windows.Forms.ComboBox computerNameComboBox;
        private System.Windows.Forms.Label computerNameLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label computerNameInfoLabel;
    }
}