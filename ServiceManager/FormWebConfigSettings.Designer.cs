namespace ServiceManager {
	partial class FormWebConfigSettings {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebConfigSettings));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.logLevelLabel = new System.Windows.Forms.Label();
            this.passwordLowLabel = new System.Windows.Forms.Label();
            this.userLowLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.serverLabel = new System.Windows.Forms.Label();
            this.logLevelComboBox = new System.Windows.Forms.ComboBox();
            this.passwordLowTextBox = new System.Windows.Forms.TextBox();
            this.userLowTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.settingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(262, 289);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(146, 289);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGroupBox.Controls.Add(this.logLevelLabel);
            this.settingsGroupBox.Controls.Add(this.passwordLowLabel);
            this.settingsGroupBox.Controls.Add(this.userLowLabel);
            this.settingsGroupBox.Controls.Add(this.passwordLabel);
            this.settingsGroupBox.Controls.Add(this.userLabel);
            this.settingsGroupBox.Controls.Add(this.databaseLabel);
            this.settingsGroupBox.Controls.Add(this.serverLabel);
            this.settingsGroupBox.Controls.Add(this.logLevelComboBox);
            this.settingsGroupBox.Controls.Add(this.passwordLowTextBox);
            this.settingsGroupBox.Controls.Add(this.userLowTextBox);
            this.settingsGroupBox.Controls.Add(this.passwordTextBox);
            this.settingsGroupBox.Controls.Add(this.userTextBox);
            this.settingsGroupBox.Controls.Add(this.databaseTextBox);
            this.settingsGroupBox.Controls.Add(this.serverTextBox);
            this.settingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.settingsGroupBox.Size = new System.Drawing.Size(360, 260);
            this.settingsGroupBox.TabIndex = 0;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Connection Settings";
            // 
            // logLevelLabel
            // 
            this.logLevelLabel.AutoSize = true;
            this.logLevelLabel.Location = new System.Drawing.Point(67, 206);
            this.logLevelLabel.Name = "logLevelLabel";
            this.logLevelLabel.Size = new System.Drawing.Size(57, 15);
            this.logLevelLabel.TabIndex = 12;
            this.logLevelLabel.Text = "Log Level";
            this.logLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordLowLabel
            // 
            this.passwordLowLabel.AutoSize = true;
            this.passwordLowLabel.Location = new System.Drawing.Point(45, 177);
            this.passwordLowLabel.Name = "passwordLowLabel";
            this.passwordLowLabel.Size = new System.Drawing.Size(79, 15);
            this.passwordLowLabel.TabIndex = 10;
            this.passwordLowLabel.Text = "PasswordLow";
            this.passwordLowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userLowLabel
            // 
            this.userLowLabel.AutoSize = true;
            this.userLowLabel.Location = new System.Drawing.Point(72, 148);
            this.userLowLabel.Name = "userLowLabel";
            this.userLowLabel.Size = new System.Drawing.Size(52, 15);
            this.userLowLabel.TabIndex = 8;
            this.userLowLabel.Text = "UserLow";
            this.userLowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(67, 119);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 15);
            this.passwordLabel.TabIndex = 6;
            this.passwordLabel.Text = "Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(94, 90);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(30, 15);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(69, 61);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(55, 15);
            this.databaseLabel.TabIndex = 2;
            this.databaseLabel.Text = "Database";
            this.databaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(85, 32);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(39, 15);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server";
            this.serverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logLevelComboBox
            // 
            this.logLevelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logLevelComboBox.FormattingEnabled = true;
            this.logLevelComboBox.Items.AddRange(new object[] {
            "Error",
            "Information",
            "Verbose"});
            this.logLevelComboBox.Location = new System.Drawing.Point(130, 203);
            this.logLevelComboBox.Name = "logLevelComboBox";
            this.logLevelComboBox.Size = new System.Drawing.Size(217, 23);
            this.logLevelComboBox.TabIndex = 13;
            // 
            // passwordLowTextBox
            // 
            this.passwordLowTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordLowTextBox.Location = new System.Drawing.Point(130, 174);
            this.passwordLowTextBox.Name = "passwordLowTextBox";
            this.passwordLowTextBox.Size = new System.Drawing.Size(217, 23);
            this.passwordLowTextBox.TabIndex = 11;
            // 
            // userLowTextBox
            // 
            this.userLowTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userLowTextBox.Location = new System.Drawing.Point(130, 145);
            this.userLowTextBox.Name = "userLowTextBox";
            this.userLowTextBox.Size = new System.Drawing.Size(217, 23);
            this.userLowTextBox.TabIndex = 9;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(130, 116);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(217, 23);
            this.passwordTextBox.TabIndex = 7;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(130, 87);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(217, 23);
            this.userTextBox.TabIndex = 5;
            this.userTextBox.Text = "root";
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseTextBox.Location = new System.Drawing.Point(130, 58);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(217, 23);
            this.databaseTextBox.TabIndex = 3;
            this.databaseTextBox.Text = "opendental";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTextBox.Location = new System.Drawing.Point(130, 29);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(217, 23);
            this.serverTextBox.TabIndex = 1;
            this.serverTextBox.Text = "localhost";
            // 
            // FormWebConfigSettings
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 331);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWebConfigSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OpenDentalWebConfig.xml Settings";
            this.Load += new System.EventHandler(this.FormWebConfigSettings_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.GroupBox settingsGroupBox;
		private System.Windows.Forms.Label logLevelLabel;
		private System.Windows.Forms.Label passwordLowLabel;
		private System.Windows.Forms.Label userLowLabel;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.Label databaseLabel;
		private System.Windows.Forms.Label serverLabel;
		private System.Windows.Forms.ComboBox logLevelComboBox;
		private System.Windows.Forms.TextBox passwordLowTextBox;
		private System.Windows.Forms.TextBox userLowTextBox;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.TextBox databaseTextBox;
		private System.Windows.Forms.TextBox serverTextBox;
	}
}