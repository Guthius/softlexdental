namespace OpenDental
{
    partial class FormEmailAddressEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAddressEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.smtpServerLabel = new System.Windows.Forms.Label();
            this.senderLabel = new System.Windows.Forms.Label();
            this.smtpServerTextBox = new System.Windows.Forms.TextBox();
            this.senderTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.smtpPortTextBox = new System.Windows.Forms.TextBox();
            this.smtpPortLabel = new System.Windows.Forms.Label();
            this.smtpPortHelpLabel = new System.Windows.Forms.Label();
            this.useSslCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.outgoingGroupBox = new System.Windows.Forms.GroupBox();
            this.smtpServerHelpLabel = new System.Windows.Forms.Label();
            this.incomingGroupBox = new System.Windows.Forms.GroupBox();
            this.pop3ServerHelpLabel = new System.Windows.Forms.Label();
            this.pop3ServerTextBox = new System.Windows.Forms.TextBox();
            this.pop3ServerLabel = new System.Windows.Forms.Label();
            this.pop3PortIHelpLabel = new System.Windows.Forms.Label();
            this.pop3PortTextBox = new System.Windows.Forms.TextBox();
            this.pop3PortLabel = new System.Windows.Forms.Label();
            this.usernameHelpLabel = new System.Windows.Forms.Label();
            this.userGroupBox = new System.Windows.Forms.GroupBox();
            this.pickUserButton = new System.Windows.Forms.Button();
            this.userLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.outgoingGroupBox.SuspendLayout();
            this.incomingGroupBox.SuspendLayout();
            this.userGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(461, 538);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(345, 538);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // smtpServerLabel
            // 
            this.smtpServerLabel.AutoSize = true;
            this.smtpServerLabel.Location = new System.Drawing.Point(55, 25);
            this.smtpServerLabel.Name = "smtpServerLabel";
            this.smtpServerLabel.Size = new System.Drawing.Size(126, 15);
            this.smtpServerLabel.TabIndex = 2;
            this.smtpServerLabel.Text = "Outgoing SMTP Server";
            this.smtpServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // senderLabel
            // 
            this.senderLabel.AutoSize = true;
            this.senderLabel.Location = new System.Drawing.Point(45, 153);
            this.senderLabel.Name = "senderLabel";
            this.senderLabel.Size = new System.Drawing.Size(136, 15);
            this.senderLabel.TabIndex = 3;
            this.senderLabel.Text = "E-mail address of sender";
            this.senderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // smtpServerTextBox
            // 
            this.smtpServerTextBox.Location = new System.Drawing.Point(187, 22);
            this.smtpServerTextBox.Name = "smtpServerTextBox";
            this.smtpServerTextBox.Size = new System.Drawing.Size(220, 23);
            this.smtpServerTextBox.TabIndex = 1;
            // 
            // senderTextBox
            // 
            this.senderTextBox.Location = new System.Drawing.Point(187, 150);
            this.senderTextBox.Name = "senderTextBox";
            this.senderTextBox.Size = new System.Drawing.Size(220, 23);
            this.senderTextBox.TabIndex = 3;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(200, 19);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(220, 23);
            this.usernameTextBox.TabIndex = 1;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(134, 22);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(60, 15);
            this.usernameLabel.TabIndex = 0;
            this.usernameLabel.Text = "Username";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(200, 48);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(220, 23);
            this.passwordTextBox.TabIndex = 2;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(137, 51);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 15);
            this.passwordLabel.TabIndex = 0;
            this.passwordLabel.Text = "Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // smtpPortTextBox
            // 
            this.smtpPortTextBox.Location = new System.Drawing.Point(187, 121);
            this.smtpPortTextBox.Name = "smtpPortTextBox";
            this.smtpPortTextBox.Size = new System.Drawing.Size(56, 23);
            this.smtpPortTextBox.TabIndex = 2;
            // 
            // smtpPortLabel
            // 
            this.smtpPortLabel.AutoSize = true;
            this.smtpPortLabel.Location = new System.Drawing.Point(98, 124);
            this.smtpPortLabel.Name = "smtpPortLabel";
            this.smtpPortLabel.Size = new System.Drawing.Size(83, 15);
            this.smtpPortLabel.TabIndex = 22;
            this.smtpPortLabel.Text = "Outgoing Port";
            this.smtpPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // smtpPortHelpLabel
            // 
            this.smtpPortHelpLabel.AutoSize = true;
            this.smtpPortHelpLabel.Location = new System.Drawing.Point(249, 124);
            this.smtpPortHelpLabel.Name = "smtpPortHelpLabel";
            this.smtpPortHelpLabel.Size = new System.Drawing.Size(187, 15);
            this.smtpPortHelpLabel.TabIndex = 0;
            this.smtpPortHelpLabel.Text = "Usually 587.  Sometimes 25 or 465.";
            this.smtpPortHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // useSslCheckBox
            // 
            this.useSslCheckBox.AutoSize = true;
            this.useSslCheckBox.Location = new System.Drawing.Point(200, 77);
            this.useSslCheckBox.Name = "useSslCheckBox";
            this.useSslCheckBox.Size = new System.Drawing.Size(66, 19);
            this.useSslCheckBox.TabIndex = 3;
            this.useSslCheckBox.Text = "Use SSL";
            this.useSslCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.useSslCheckBox.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 538);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // outgoingGroupBox
            // 
            this.outgoingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outgoingGroupBox.Controls.Add(this.smtpServerHelpLabel);
            this.outgoingGroupBox.Controls.Add(this.smtpServerTextBox);
            this.outgoingGroupBox.Controls.Add(this.smtpServerLabel);
            this.outgoingGroupBox.Controls.Add(this.senderLabel);
            this.outgoingGroupBox.Controls.Add(this.smtpPortHelpLabel);
            this.outgoingGroupBox.Controls.Add(this.senderTextBox);
            this.outgoingGroupBox.Controls.Add(this.smtpPortTextBox);
            this.outgoingGroupBox.Controls.Add(this.smtpPortLabel);
            this.outgoingGroupBox.Location = new System.Drawing.Point(13, 102);
            this.outgoingGroupBox.Name = "outgoingGroupBox";
            this.outgoingGroupBox.Size = new System.Drawing.Size(558, 190);
            this.outgoingGroupBox.TabIndex = 4;
            this.outgoingGroupBox.TabStop = false;
            this.outgoingGroupBox.Text = "Outgoing Email Settings (SMTP)";
            // 
            // smtpServerHelpLabel
            // 
            this.smtpServerHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smtpServerHelpLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.smtpServerHelpLabel.Location = new System.Drawing.Point(184, 48);
            this.smtpServerHelpLabel.Name = "smtpServerHelpLabel";
            this.smtpServerHelpLabel.Size = new System.Drawing.Size(370, 70);
            this.smtpServerHelpLabel.TabIndex = 0;
            this.smtpServerHelpLabel.Text = "smtp.comcast.net\r\nmailhost.mycompany.com \r\nmail.mycompany.com\r\nsmtp.gmail.com\r\nor" +
    " similar...";
            // 
            // incomingGroupBox
            // 
            this.incomingGroupBox.Controls.Add(this.pop3ServerHelpLabel);
            this.incomingGroupBox.Controls.Add(this.pop3ServerTextBox);
            this.incomingGroupBox.Controls.Add(this.pop3ServerLabel);
            this.incomingGroupBox.Controls.Add(this.pop3PortIHelpLabel);
            this.incomingGroupBox.Controls.Add(this.pop3PortTextBox);
            this.incomingGroupBox.Controls.Add(this.pop3PortLabel);
            this.incomingGroupBox.Location = new System.Drawing.Point(13, 298);
            this.incomingGroupBox.Name = "incomingGroupBox";
            this.incomingGroupBox.Size = new System.Drawing.Size(628, 140);
            this.incomingGroupBox.TabIndex = 5;
            this.incomingGroupBox.TabStop = false;
            this.incomingGroupBox.Text = "Incoming Email Settings (POP3)";
            // 
            // pop3ServerHelpLabel
            // 
            this.pop3ServerHelpLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pop3ServerHelpLabel.Location = new System.Drawing.Point(184, 50);
            this.pop3ServerHelpLabel.Name = "pop3ServerHelpLabel";
            this.pop3ServerHelpLabel.Size = new System.Drawing.Size(438, 43);
            this.pop3ServerHelpLabel.TabIndex = 0;
            this.pop3ServerHelpLabel.Text = "pop.secureserver.net\r\npop.gmail.com\r\nor similar...";
            // 
            // pop3ServerTextBox
            // 
            this.pop3ServerTextBox.Location = new System.Drawing.Point(187, 22);
            this.pop3ServerTextBox.Name = "pop3ServerTextBox";
            this.pop3ServerTextBox.Size = new System.Drawing.Size(220, 23);
            this.pop3ServerTextBox.TabIndex = 1;
            // 
            // pop3ServerLabel
            // 
            this.pop3ServerLabel.AutoSize = true;
            this.pop3ServerLabel.Location = new System.Drawing.Point(56, 25);
            this.pop3ServerLabel.Name = "pop3ServerLabel";
            this.pop3ServerLabel.Size = new System.Drawing.Size(125, 15);
            this.pop3ServerLabel.TabIndex = 0;
            this.pop3ServerLabel.Text = "Incoming POP3 Server";
            this.pop3ServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pop3PortIHelpLabel
            // 
            this.pop3PortIHelpLabel.AutoSize = true;
            this.pop3PortIHelpLabel.Location = new System.Drawing.Point(249, 99);
            this.pop3PortIHelpLabel.Name = "pop3PortIHelpLabel";
            this.pop3PortIHelpLabel.Size = new System.Drawing.Size(158, 15);
            this.pop3PortIHelpLabel.TabIndex = 0;
            this.pop3PortIHelpLabel.Text = "Usually 110.  Sometimes 995.";
            this.pop3PortIHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pop3PortTextBox
            // 
            this.pop3PortTextBox.Location = new System.Drawing.Point(187, 96);
            this.pop3PortTextBox.Name = "pop3PortTextBox";
            this.pop3PortTextBox.Size = new System.Drawing.Size(56, 23);
            this.pop3PortTextBox.TabIndex = 2;
            // 
            // pop3PortLabel
            // 
            this.pop3PortLabel.AutoSize = true;
            this.pop3PortLabel.Location = new System.Drawing.Point(98, 99);
            this.pop3PortLabel.Name = "pop3PortLabel";
            this.pop3PortLabel.Size = new System.Drawing.Size(83, 15);
            this.pop3PortLabel.TabIndex = 0;
            this.pop3PortLabel.Text = "Incoming Port";
            this.pop3PortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // usernameHelpLabel
            // 
            this.usernameHelpLabel.AutoSize = true;
            this.usernameHelpLabel.Location = new System.Drawing.Point(426, 22);
            this.usernameHelpLabel.Name = "usernameHelpLabel";
            this.usernameHelpLabel.Size = new System.Drawing.Size(107, 15);
            this.usernameHelpLabel.TabIndex = 0;
            this.usernameHelpLabel.Text = "(full email address)";
            this.usernameHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userGroupBox
            // 
            this.userGroupBox.Controls.Add(this.pickUserButton);
            this.userGroupBox.Controls.Add(this.userLabel);
            this.userGroupBox.Controls.Add(this.userTextBox);
            this.userGroupBox.Location = new System.Drawing.Point(13, 444);
            this.userGroupBox.Name = "userGroupBox";
            this.userGroupBox.Size = new System.Drawing.Size(628, 60);
            this.userGroupBox.TabIndex = 10;
            this.userGroupBox.TabStop = false;
            this.userGroupBox.Text = "User";
            // 
            // pickUserButton
            // 
            this.pickUserButton.Location = new System.Drawing.Point(413, 21);
            this.pickUserButton.Name = "pickUserButton";
            this.pickUserButton.Size = new System.Drawing.Size(30, 25);
            this.pickUserButton.TabIndex = 4;
            this.pickUserButton.Text = "...";
            this.pickUserButton.UseVisualStyleBackColor = true;
            this.pickUserButton.Visible = false;
            this.pickUserButton.Click += new System.EventHandler(this.PickUserButton_Click);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(151, 26);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(30, 15);
            this.userLabel.TabIndex = 3;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(187, 22);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.ReadOnly = true;
            this.userTextBox.Size = new System.Drawing.Size(220, 23);
            this.userTextBox.TabIndex = 0;
            // 
            // FormEmailAddressEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 581);
            this.Controls.Add(this.userGroupBox);
            this.Controls.Add(this.usernameHelpLabel);
            this.Controls.Add(this.incomingGroupBox);
            this.Controls.Add(this.outgoingGroupBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.useSslCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEmailAddressEdit";
            this.ShowInTaskbar = false;
            this.Text = "Edit Mail Address";
            this.Load += new System.EventHandler(this.FormEmailAddressEdit_Load);
            this.outgoingGroupBox.ResumeLayout(false);
            this.outgoingGroupBox.PerformLayout();
            this.incomingGroupBox.ResumeLayout(false);
            this.incomingGroupBox.PerformLayout();
            this.userGroupBox.ResumeLayout(false);
            this.userGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label smtpServerLabel;
        private System.Windows.Forms.Label senderLabel;
        private System.Windows.Forms.TextBox smtpServerTextBox;
        private System.Windows.Forms.TextBox senderTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox smtpPortTextBox;
        private System.Windows.Forms.Label smtpPortLabel;
        private System.Windows.Forms.Label smtpPortHelpLabel;
        private System.Windows.Forms.CheckBox useSslCheckBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.GroupBox outgoingGroupBox;
        private System.Windows.Forms.GroupBox incomingGroupBox;
        private System.Windows.Forms.TextBox pop3ServerTextBox;
        private System.Windows.Forms.Label pop3ServerLabel;
        private System.Windows.Forms.Label pop3PortIHelpLabel;
        private System.Windows.Forms.TextBox pop3PortTextBox;
        private System.Windows.Forms.Label pop3PortLabel;
        private System.Windows.Forms.Label usernameHelpLabel;
        private System.Windows.Forms.Label smtpServerHelpLabel;
        private System.Windows.Forms.Label pop3ServerHelpLabel;
        private System.Windows.Forms.GroupBox userGroupBox;
        private System.Windows.Forms.Button pickUserButton;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox userTextBox;
    }
}
