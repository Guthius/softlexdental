namespace OpenDental
{
    partial class FormPayConnectSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayConnectSetup));
            this.websiteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboPaymentType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.comboClinic = new System.Windows.Forms.ComboBox();
            this.labelClinic = new System.Windows.Forms.Label();
            this.paySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.portalPaymentsGroupBox = new System.Windows.Forms.GroupBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.tokenLabel = new System.Windows.Forms.Label();
            this.tokenTextBox = new System.Windows.Forms.TextBox();
            this.portalPayEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.checkPreventSavingNewCC = new System.Windows.Forms.CheckBox();
            this.comboDefaultProcessing = new System.Windows.Forms.ComboBox();
            this.labelDefaultProcMethod = new System.Windows.Forms.Label();
            this.checkForceRecurring = new System.Windows.Forms.CheckBox();
            this.checkTerminal = new System.Windows.Forms.CheckBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.labelClinicEnable = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.paySettingsGroupBox.SuspendLayout();
            this.portalPaymentsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // websiteLinkLabel
            // 
            this.websiteLinkLabel.AutoSize = true;
            this.websiteLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(29, 28);
            this.websiteLinkLabel.Location = new System.Drawing.Point(13, 16);
            this.websiteLinkLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.websiteLinkLabel.Name = "websiteLinkLabel";
            this.websiteLinkLabel.Size = new System.Drawing.Size(301, 21);
            this.websiteLinkLabel.TabIndex = 0;
            this.websiteLinkLabel.TabStop = true;
            this.websiteLinkLabel.Text = "The PayConnect website is at www.dentalxchange.com";
            this.websiteLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.websiteLinkLabel.UseCompatibleTextRendering = true;
            this.websiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLinkLabel_LinkClicked);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(193, 53);
            this.enabledCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(165, 19);
            this.enabledCheckBox.TabIndex = 1;
            this.enabledCheckBox.Text = "Enabled (affects all clinics)";
            this.enabledCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Payment Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboPaymentType
            // 
            this.comboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPaymentType.FormattingEnabled = true;
            this.comboPaymentType.Location = new System.Drawing.Point(180, 22);
            this.comboPaymentType.MaxDropDownItems = 25;
            this.comboPaymentType.Name = "comboPaymentType";
            this.comboPaymentType.Size = new System.Drawing.Size(175, 23);
            this.comboPaymentType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textUsername
            // 
            this.textUsername.Location = new System.Drawing.Point(180, 80);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(175, 23);
            this.textUsername.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(180, 109);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(175, 23);
            this.textPassword.TabIndex = 7;
            this.textPassword.UseSystemPasswordChar = true;
            this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
            // 
            // comboClinic
            // 
            this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClinic.Location = new System.Drawing.Point(193, 119);
            this.comboClinic.MaxDropDownItems = 30;
            this.comboClinic.Name = "comboClinic";
            this.comboClinic.Size = new System.Drawing.Size(175, 23);
            this.comboClinic.TabIndex = 4;
            this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
            // 
            // labelClinic
            // 
            this.labelClinic.AutoSize = true;
            this.labelClinic.Location = new System.Drawing.Point(150, 122);
            this.labelClinic.Name = "labelClinic";
            this.labelClinic.Size = new System.Drawing.Size(37, 15);
            this.labelClinic.TabIndex = 3;
            this.labelClinic.Text = "Clinic";
            this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // paySettingsGroupBox
            // 
            this.paySettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paySettingsGroupBox.Controls.Add(this.portalPaymentsGroupBox);
            this.paySettingsGroupBox.Controls.Add(this.checkPreventSavingNewCC);
            this.paySettingsGroupBox.Controls.Add(this.comboDefaultProcessing);
            this.paySettingsGroupBox.Controls.Add(this.labelDefaultProcMethod);
            this.paySettingsGroupBox.Controls.Add(this.checkForceRecurring);
            this.paySettingsGroupBox.Controls.Add(this.checkTerminal);
            this.paySettingsGroupBox.Controls.Add(this.textPassword);
            this.paySettingsGroupBox.Controls.Add(this.label3);
            this.paySettingsGroupBox.Controls.Add(this.textUsername);
            this.paySettingsGroupBox.Controls.Add(this.label2);
            this.paySettingsGroupBox.Controls.Add(this.comboPaymentType);
            this.paySettingsGroupBox.Controls.Add(this.label1);
            this.paySettingsGroupBox.Location = new System.Drawing.Point(13, 150);
            this.paySettingsGroupBox.Name = "paySettingsGroupBox";
            this.paySettingsGroupBox.Size = new System.Drawing.Size(502, 320);
            this.paySettingsGroupBox.TabIndex = 5;
            this.paySettingsGroupBox.TabStop = false;
            this.paySettingsGroupBox.Text = "Clinic Payment Settings";
            // 
            // portalPaymentsGroupBox
            // 
            this.portalPaymentsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portalPaymentsGroupBox.Controls.Add(this.generateButton);
            this.portalPaymentsGroupBox.Controls.Add(this.tokenLabel);
            this.portalPaymentsGroupBox.Controls.Add(this.tokenTextBox);
            this.portalPaymentsGroupBox.Controls.Add(this.portalPayEnabledCheckBox);
            this.portalPaymentsGroupBox.Location = new System.Drawing.Point(180, 220);
            this.portalPaymentsGroupBox.Name = "portalPaymentsGroupBox";
            this.portalPaymentsGroupBox.Size = new System.Drawing.Size(316, 90);
            this.portalPaymentsGroupBox.TabIndex = 11;
            this.portalPaymentsGroupBox.TabStop = false;
            this.portalPaymentsGroupBox.Text = "Patient Portal Payments";
            this.portalPaymentsGroupBox.Visible = false;
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.Location = new System.Drawing.Point(230, 46);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(80, 25);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Generate";
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // tokenLabel
            // 
            this.tokenLabel.AutoSize = true;
            this.tokenLabel.Location = new System.Drawing.Point(45, 50);
            this.tokenLabel.Name = "tokenLabel";
            this.tokenLabel.Size = new System.Drawing.Size(39, 15);
            this.tokenLabel.TabIndex = 1;
            this.tokenLabel.Text = "Token";
            this.tokenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.Location = new System.Drawing.Point(90, 47);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.ReadOnly = true;
            this.tokenTextBox.Size = new System.Drawing.Size(134, 23);
            this.tokenTextBox.TabIndex = 2;
            // 
            // portalPayEnabledCheckBox
            // 
            this.portalPayEnabledCheckBox.AutoSize = true;
            this.portalPayEnabledCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.portalPayEnabledCheckBox.Location = new System.Drawing.Point(90, 22);
            this.portalPayEnabledCheckBox.Name = "portalPayEnabledCheckBox";
            this.portalPayEnabledCheckBox.Size = new System.Drawing.Size(68, 19);
            this.portalPayEnabledCheckBox.TabIndex = 0;
            this.portalPayEnabledCheckBox.Text = "Enabled";
            this.portalPayEnabledCheckBox.Click += new System.EventHandler(this.checkPatientPortalPayEnabled_Click);
            // 
            // checkPreventSavingNewCC
            // 
            this.checkPreventSavingNewCC.AutoSize = true;
            this.checkPreventSavingNewCC.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPreventSavingNewCC.Location = new System.Drawing.Point(180, 188);
            this.checkPreventSavingNewCC.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.checkPreventSavingNewCC.Name = "checkPreventSavingNewCC";
            this.checkPreventSavingNewCC.Size = new System.Drawing.Size(159, 19);
            this.checkPreventSavingNewCC.TabIndex = 10;
            this.checkPreventSavingNewCC.Text = "Prevent saving new cards";
            // 
            // comboDefaultProcessing
            // 
            this.comboDefaultProcessing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDefaultProcessing.FormattingEnabled = true;
            this.comboDefaultProcessing.Location = new System.Drawing.Point(180, 51);
            this.comboDefaultProcessing.MaxDropDownItems = 25;
            this.comboDefaultProcessing.Name = "comboDefaultProcessing";
            this.comboDefaultProcessing.Size = new System.Drawing.Size(175, 23);
            this.comboDefaultProcessing.TabIndex = 3;
            // 
            // labelDefaultProcMethod
            // 
            this.labelDefaultProcMethod.AutoSize = true;
            this.labelDefaultProcMethod.Location = new System.Drawing.Point(24, 54);
            this.labelDefaultProcMethod.Name = "labelDefaultProcMethod";
            this.labelDefaultProcMethod.Size = new System.Drawing.Size(150, 15);
            this.labelDefaultProcMethod.TabIndex = 2;
            this.labelDefaultProcMethod.Text = "Default Processing Method";
            this.labelDefaultProcMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkForceRecurring
            // 
            this.checkForceRecurring.AutoSize = true;
            this.checkForceRecurring.Location = new System.Drawing.Point(180, 163);
            this.checkForceRecurring.Name = "checkForceRecurring";
            this.checkForceRecurring.Size = new System.Drawing.Size(277, 19);
            this.checkForceRecurring.TabIndex = 9;
            this.checkForceRecurring.Text = "Recurring charge list force duplicates by default";
            this.checkForceRecurring.UseVisualStyleBackColor = true;
            // 
            // checkTerminal
            // 
            this.checkTerminal.AutoSize = true;
            this.checkTerminal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkTerminal.Location = new System.Drawing.Point(180, 138);
            this.checkTerminal.Name = "checkTerminal";
            this.checkTerminal.Size = new System.Drawing.Size(168, 19);
            this.checkTerminal.TabIndex = 8;
            this.checkTerminal.Text = "Enable terminal processing";
            this.checkTerminal.CheckedChanged += new System.EventHandler(this.checkTerminal_CheckedChanged);
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.downloadButton.Location = new System.Drawing.Point(13, 498);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(130, 30);
            this.downloadButton.TabIndex = 6;
            this.downloadButton.Text = "Download Driver";
            this.downloadButton.Visible = false;
            this.downloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // labelClinicEnable
            // 
            this.labelClinicEnable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelClinicEnable.Location = new System.Drawing.Point(190, 82);
            this.labelClinicEnable.Name = "labelClinicEnable";
            this.labelClinicEnable.Size = new System.Drawing.Size(325, 34);
            this.labelClinicEnable.TabIndex = 2;
            this.labelClinicEnable.Text = "To enable PayConnect for a clinic, set the username and password for that clinic." +
    "";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(289, 498);
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
            this.cancelButton.Location = new System.Drawing.Point(405, 498);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormPayConnectSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(528, 541);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.labelClinicEnable);
            this.Controls.Add(this.paySettingsGroupBox);
            this.Controls.Add(this.comboClinic);
            this.Controls.Add(this.labelClinic);
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.websiteLinkLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(393, 340);
            this.Name = "FormPayConnectSetup";
            this.ShowInTaskbar = false;
            this.Text = "PayConnect Setup";
            this.Load += new System.EventHandler(this.FormPayConnectSetup_Load);
            this.paySettingsGroupBox.ResumeLayout(false);
            this.paySettingsGroupBox.PerformLayout();
            this.portalPaymentsGroupBox.ResumeLayout(false);
            this.portalPaymentsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.LinkLabel websiteLinkLabel;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboPaymentType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.ComboBox comboClinic;
        private System.Windows.Forms.Label labelClinic;
        private System.Windows.Forms.GroupBox paySettingsGroupBox;
        private System.Windows.Forms.Label labelClinicEnable;
        private System.Windows.Forms.CheckBox checkTerminal;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.CheckBox checkForceRecurring;
        private System.Windows.Forms.ComboBox comboDefaultProcessing;
        private System.Windows.Forms.Label labelDefaultProcMethod;
        private System.Windows.Forms.CheckBox checkPreventSavingNewCC;
        private System.Windows.Forms.GroupBox portalPaymentsGroupBox;
        private System.Windows.Forms.CheckBox portalPayEnabledCheckBox;
        private System.Windows.Forms.Label tokenLabel;
        private System.Windows.Forms.TextBox tokenTextBox;
        private System.Windows.Forms.Button generateButton;
    }
}
