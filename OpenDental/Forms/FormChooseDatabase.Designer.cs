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
            this.SuspendLayout();
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(13, 125);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(338, 23);
            this.userTextBox.TabIndex = 4;
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseComboBox.DropDownHeight = 390;
            this.databaseComboBox.IntegralHeight = false;
            this.databaseComboBox.Location = new System.Drawing.Point(13, 228);
            this.databaseComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.databaseComboBox.MaxDropDownItems = 100;
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(338, 23);
            this.databaseComboBox.TabIndex = 8;
            this.databaseComboBox.DropDown += new System.EventHandler(this.databaseComboBox_DropDown);
            // 
            // computerNameComboBox
            // 
            this.computerNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.computerNameComboBox.DropDownHeight = 390;
            this.computerNameComboBox.IntegralHeight = false;
            this.computerNameComboBox.Location = new System.Drawing.Point(13, 74);
            this.computerNameComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.computerNameComboBox.MaxDropDownItems = 100;
            this.computerNameComboBox.Name = "computerNameComboBox";
            this.computerNameComboBox.Size = new System.Drawing.Size(338, 23);
            this.computerNameComboBox.TabIndex = 1;
            // 
            // computerNameLabel
            // 
            this.computerNameLabel.AutoSize = true;
            this.computerNameLabel.Location = new System.Drawing.Point(13, 56);
            this.computerNameLabel.Name = "computerNameLabel";
            this.computerNameLabel.Size = new System.Drawing.Size(74, 15);
            this.computerNameLabel.TabIndex = 0;
            this.computerNameLabel.Text = "Server Name";
            this.computerNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(13, 176);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(338, 23);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(13, 158);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(98, 15);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "MySQL Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(13, 107);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(71, 15);
            this.userLabel.TabIndex = 3;
            this.userLabel.Text = "MySQL User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(13, 210);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(55, 15);
            this.databaseLabel.TabIndex = 7;
            this.databaseLabel.Text = "Database";
            this.databaseLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkNoShow
            // 
            this.checkNoShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkNoShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkNoShow.Location = new System.Drawing.Point(13, 328);
            this.checkNoShow.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.checkNoShow.Name = "checkNoShow";
            this.checkNoShow.Size = new System.Drawing.Size(338, 20);
            this.checkNoShow.TabIndex = 11;
            this.checkNoShow.Text = "Do not show this window on startup";
            this.checkNoShow.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(241, 280);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(125, 280);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // FormChooseDatabase
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(364, 361);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.databaseComboBox);
            this.Controls.Add(this.checkNoShow);
            this.Controls.Add(this.computerNameComboBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.computerNameLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.databaseLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HeaderText = "Enter the database connection details below.";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseDatabase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Database";
            this.Load += new System.EventHandler(this.ChooseDatabaseView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
    }
}