namespace OpenDental{
	partial class FormChooseDatabase {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseDatabase));
            this.textConnectionString = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupDirect = new System.Windows.Forms.GroupBox();
            this.textUser = new System.Windows.Forms.TextBox();
            this.comboDatabase = new System.Windows.Forms.ComboBox();
            this.checkNoShow = new System.Windows.Forms.CheckBox();
            this.comboComputerName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.checkDynamicMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.groupDirect.SuspendLayout();
            this.SuspendLayout();
            // 
            // textConnectionString
            // 
            this.textConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textConnectionString.Location = new System.Drawing.Point(12, 400);
            this.textConnectionString.Multiline = true;
            this.textConnectionString.Name = "textConnectionString";
            this.textConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textConnectionString.Size = new System.Drawing.Size(396, 130);
            this.textConnectionString.TabIndex = 34;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 382);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(188, 15);
            this.label8.TabIndex = 39;
            this.label8.Text = "Advanced: (use connection string)";
            // 
            // groupDirect
            // 
            this.groupDirect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDirect.Controls.Add(this.label5);
            this.groupDirect.Controls.Add(this.textUser);
            this.groupDirect.Controls.Add(this.comboDatabase);
            this.groupDirect.Controls.Add(this.checkNoShow);
            this.groupDirect.Controls.Add(this.comboComputerName);
            this.groupDirect.Controls.Add(this.label1);
            this.groupDirect.Controls.Add(this.textPassword);
            this.groupDirect.Controls.Add(this.label2);
            this.groupDirect.Controls.Add(this.label3);
            this.groupDirect.Controls.Add(this.label4);
            this.groupDirect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupDirect.Location = new System.Drawing.Point(12, 52);
            this.groupDirect.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.groupDirect.Name = "groupDirect";
            this.groupDirect.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupDirect.Size = new System.Drawing.Size(396, 320);
            this.groupDirect.TabIndex = 31;
            this.groupDirect.TabStop = false;
            this.groupDirect.Text = "Connection Settings";
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(6, 148);
            this.textUser.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(280, 23);
            this.textUser.TabIndex = 3;
            // 
            // comboDatabase
            // 
            this.comboDatabase.DropDownHeight = 390;
            this.comboDatabase.IntegralHeight = false;
            this.comboDatabase.Location = new System.Drawing.Point(6, 250);
            this.comboDatabase.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.comboDatabase.MaxDropDownItems = 100;
            this.comboDatabase.Name = "comboDatabase";
            this.comboDatabase.Size = new System.Drawing.Size(280, 23);
            this.comboDatabase.TabIndex = 2;
            this.comboDatabase.DropDown += new System.EventHandler(this.comboDatabase_DropDown);
            // 
            // checkNoShow
            // 
            this.checkNoShow.AutoSize = true;
            this.checkNoShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkNoShow.Location = new System.Drawing.Point(6, 286);
            this.checkNoShow.Name = "checkNoShow";
            this.checkNoShow.Size = new System.Drawing.Size(334, 20);
            this.checkNoShow.TabIndex = 5;
            this.checkNoShow.Text = "Do not show this window on startup (this computer only)";
            this.checkNoShow.UseVisualStyleBackColor = true;
            // 
            // comboComputerName
            // 
            this.comboComputerName.DropDownHeight = 390;
            this.comboComputerName.IntegralHeight = false;
            this.comboComputerName.Location = new System.Drawing.Point(6, 44);
            this.comboComputerName.MaxDropDownItems = 100;
            this.comboComputerName.Name = "comboComputerName";
            this.comboComputerName.Size = new System.Drawing.Size(280, 23);
            this.comboComputerName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(6, 199);
            this.textPassword.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(280, 23);
            this.textPassword.TabIndex = 4;
            this.textPassword.UseSystemPasswordChar = true;
            this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
            this.textPassword.Leave += new System.EventHandler(this.textPassword_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "MySQL Password";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "MySQL User";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(327, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Database (usually opendental unless you changed the name)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Autosize = true;
            this.cancelButton.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
            this.cancelButton.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
            this.cancelButton.CornerRadius = 4F;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(298, 579);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 36;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Autosize = true;
            this.acceptButton.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
            this.acceptButton.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
            this.acceptButton.CornerRadius = 4F;
            this.acceptButton.Location = new System.Drawing.Point(182, 579);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 35;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.butOK_Click);
            // 
            // checkDynamicMode
            // 
            this.checkDynamicMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkDynamicMode.Location = new System.Drawing.Point(12, 536);
            this.checkDynamicMode.Name = "checkDynamicMode";
            this.checkDynamicMode.Size = new System.Drawing.Size(374, 18);
            this.checkDynamicMode.TabIndex = 41;
            this.checkDynamicMode.Text = "Dynamic Mode: Automatically downgrades to server version.";
            this.checkDynamicMode.UseVisualStyleBackColor = true;
            this.checkDynamicMode.CheckedChanged += new System.EventHandler(this.checkDynamicMode_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(384, 60);
            this.label5.TabIndex = 7;
            this.label5.Text = "The name of the computer where the MySQL server and database are located.  If you" +
    " are running this program on a single computer only, then the computer name may " +
    "be localhost.";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(396, 40);
            this.infoLabel.TabIndex = 42;
            this.infoLabel.Text = "These values will only be used on this computer.  They have to be set on each com" +
    "puter";
            // 
            // FormChooseDatabase
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(420, 621);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.checkDynamicMode);
            this.Controls.Add(this.textConnectionString);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupDirect);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseDatabase";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Choose Database";
            this.Load += new System.EventHandler(this.ChooseDatabaseView_Load);
            this.groupDirect.ResumeLayout(false);
            this.groupDirect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textConnectionString;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupDirect;
		private System.Windows.Forms.TextBox textUser;
		private System.Windows.Forms.ComboBox comboDatabase;
		private System.Windows.Forms.CheckBox checkNoShow;
		private System.Windows.Forms.ComboBox comboComputerName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private UI.Button cancelButton;
		private UI.Button acceptButton;
		private System.Windows.Forms.CheckBox checkDynamicMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label infoLabel;
    }
}