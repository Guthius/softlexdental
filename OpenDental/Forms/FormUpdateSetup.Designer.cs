namespace OpenDental
{
    partial class FormUpdateSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateSetup));
            this.registrationKeyTextBox = new System.Windows.Forms.TextBox();
            this.registrationKeyLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.proxyGroupBox = new System.Windows.Forms.GroupBox();
            this.proxyPasswordTextBox = new System.Windows.Forms.TextBox();
            this.proxyPasswordLabel = new System.Windows.Forms.Label();
            this.proxyUsernameTextBox = new System.Windows.Forms.TextBox();
            this.proxyUsernameLabel = new System.Windows.Forms.Label();
            this.proxyAddresTextBox = new System.Windows.Forms.TextBox();
            this.proxyAddressLabel = new System.Windows.Forms.Label();
            this.registrationKeyButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.proxyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // registrationKeyTextBox
            // 
            this.registrationKeyTextBox.Location = new System.Drawing.Point(150, 218);
            this.registrationKeyTextBox.Name = "registrationKeyTextBox";
            this.registrationKeyTextBox.ReadOnly = true;
            this.registrationKeyTextBox.Size = new System.Drawing.Size(305, 23);
            this.registrationKeyTextBox.TabIndex = 4;
            // 
            // registrationKeyLabel
            // 
            this.registrationKeyLabel.AutoSize = true;
            this.registrationKeyLabel.Location = new System.Drawing.Point(52, 222);
            this.registrationKeyLabel.Name = "registrationKeyLabel";
            this.registrationKeyLabel.Size = new System.Drawing.Size(92, 15);
            this.registrationKeyLabel.TabIndex = 3;
            this.registrationKeyLabel.Text = "Registration Key";
            this.registrationKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(147, 244);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(384, 40);
            this.infoLabel.TabIndex = 6;
            this.infoLabel.Text = "Valid for one office ONLY.  This is tracked.";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(150, 19);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(381, 23);
            this.addressTextBox.TabIndex = 1;
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(64, 22);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(80, 15);
            this.addressLabel.TabIndex = 0;
            this.addressLabel.Text = "Update Server";
            this.addressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // proxyGroupBox
            // 
            this.proxyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyGroupBox.Controls.Add(this.proxyPasswordTextBox);
            this.proxyGroupBox.Controls.Add(this.proxyPasswordLabel);
            this.proxyGroupBox.Controls.Add(this.proxyUsernameTextBox);
            this.proxyGroupBox.Controls.Add(this.proxyUsernameLabel);
            this.proxyGroupBox.Controls.Add(this.proxyAddresTextBox);
            this.proxyGroupBox.Controls.Add(this.proxyAddressLabel);
            this.proxyGroupBox.Location = new System.Drawing.Point(13, 65);
            this.proxyGroupBox.Margin = new System.Windows.Forms.Padding(3, 20, 3, 10);
            this.proxyGroupBox.Name = "proxyGroupBox";
            this.proxyGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.proxyGroupBox.Size = new System.Drawing.Size(518, 140);
            this.proxyGroupBox.TabIndex = 2;
            this.proxyGroupBox.TabStop = false;
            this.proxyGroupBox.Text = "Proxy (most users will ignore this section)";
            // 
            // proxyPasswordTextBox
            // 
            this.proxyPasswordTextBox.Location = new System.Drawing.Point(137, 87);
            this.proxyPasswordTextBox.Name = "proxyPasswordTextBox";
            this.proxyPasswordTextBox.PasswordChar = '*';
            this.proxyPasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.proxyPasswordTextBox.TabIndex = 5;
            // 
            // proxyPasswordLabel
            // 
            this.proxyPasswordLabel.AutoSize = true;
            this.proxyPasswordLabel.Location = new System.Drawing.Point(74, 90);
            this.proxyPasswordLabel.Name = "proxyPasswordLabel";
            this.proxyPasswordLabel.Size = new System.Drawing.Size(57, 15);
            this.proxyPasswordLabel.TabIndex = 4;
            this.proxyPasswordLabel.Text = "Password";
            this.proxyPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // proxyUsernameTextBox
            // 
            this.proxyUsernameTextBox.Location = new System.Drawing.Point(137, 58);
            this.proxyUsernameTextBox.Name = "proxyUsernameTextBox";
            this.proxyUsernameTextBox.Size = new System.Drawing.Size(200, 23);
            this.proxyUsernameTextBox.TabIndex = 3;
            // 
            // proxyUsernameLabel
            // 
            this.proxyUsernameLabel.AutoSize = true;
            this.proxyUsernameLabel.Location = new System.Drawing.Point(71, 61);
            this.proxyUsernameLabel.Name = "proxyUsernameLabel";
            this.proxyUsernameLabel.Size = new System.Drawing.Size(60, 15);
            this.proxyUsernameLabel.TabIndex = 2;
            this.proxyUsernameLabel.Text = "Username";
            this.proxyUsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // proxyAddresTextBox
            // 
            this.proxyAddresTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyAddresTextBox.Location = new System.Drawing.Point(137, 29);
            this.proxyAddresTextBox.Name = "proxyAddresTextBox";
            this.proxyAddresTextBox.Size = new System.Drawing.Size(375, 23);
            this.proxyAddresTextBox.TabIndex = 1;
            // 
            // proxyAddressLabel
            // 
            this.proxyAddressLabel.AutoSize = true;
            this.proxyAddressLabel.Location = new System.Drawing.Point(82, 32);
            this.proxyAddressLabel.Name = "proxyAddressLabel";
            this.proxyAddressLabel.Size = new System.Drawing.Size(49, 15);
            this.proxyAddressLabel.TabIndex = 0;
            this.proxyAddressLabel.Text = "Address";
            this.proxyAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // registrationKeyButton
            // 
            this.registrationKeyButton.Location = new System.Drawing.Point(461, 217);
            this.registrationKeyButton.Name = "registrationKeyButton";
            this.registrationKeyButton.Size = new System.Drawing.Size(70, 25);
            this.registrationKeyButton.TabIndex = 5;
            this.registrationKeyButton.Text = "Change";
            this.registrationKeyButton.Click += new System.EventHandler(this.RegistrationKeyButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(305, 318);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(421, 318);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormUpdateSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(544, 361);
            this.Controls.Add(this.registrationKeyButton);
            this.Controls.Add(this.proxyGroupBox);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.addressLabel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.registrationKeyTextBox);
            this.Controls.Add(this.registrationKeyLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdateSetup";
            this.ShowInTaskbar = false;
            this.Text = "Update Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdateSetup_FormClosing);
            this.Load += new System.EventHandler(this.FormUpdateSetup_Load);
            this.proxyGroupBox.ResumeLayout(false);
            this.proxyGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox registrationKeyTextBox;
        private System.Windows.Forms.Label registrationKeyLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.GroupBox proxyGroupBox;
        private System.Windows.Forms.TextBox proxyPasswordTextBox;
        private System.Windows.Forms.Label proxyPasswordLabel;
        private System.Windows.Forms.TextBox proxyUsernameTextBox;
        private System.Windows.Forms.Label proxyUsernameLabel;
        private System.Windows.Forms.TextBox proxyAddresTextBox;
        private System.Windows.Forms.Label proxyAddressLabel;
        private System.Windows.Forms.Button registrationKeyButton;
    }
}