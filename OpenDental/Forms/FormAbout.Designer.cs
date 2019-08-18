namespace OpenDental
{
    partial class FormAbout
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.versionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.copyrightMySqlLabel = new System.Windows.Forms.Label();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.licensesButton = new System.Windows.Forms.Button();
            this.copyrightAdaLabel = new System.Windows.Forms.Label();
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.serviceNameLabel = new System.Windows.Forms.Label();
            this.serviceVersionLabel = new System.Windows.Forms.Label();
            this.serviceCommentLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.machineNameCaptionLabel = new System.Windows.Forms.Label();
            this.serviceNameCaptionLabel = new System.Windows.Forms.Label();
            this.serverNameCaptionLabel = new System.Windows.Forms.Label();
            this.serviceCommentsCaptionLabel = new System.Windows.Forms.Label();
            this.serviceVersionCaptionLabel = new System.Windows.Forms.Label();
            this.machineNameLabel = new System.Windows.Forms.Label();
            this.pictureOpenDental = new System.Windows.Forms.PictureBox();
            this.diagnosticsButton = new System.Windows.Forms.Button();
            this.connectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).BeginInit();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.Location = new System.Drawing.Point(219, 19);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(402, 20);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Version: v1.0.0.0";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(511, 298);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&Close";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyrightLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.copyrightLabel.Location = new System.Drawing.Point(13, 271);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(608, 20);
            this.copyrightLabel.TabIndex = 3;
            this.copyrightLabel.Text = "This program Copyright 2003-2007, Jordan S. Sparks, D.M.D.";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // copyrightMySqlLabel
            // 
            this.copyrightMySqlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyrightMySqlLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.copyrightMySqlLabel.Location = new System.Drawing.Point(13, 311);
            this.copyrightMySqlLabel.Name = "copyrightMySqlLabel";
            this.copyrightMySqlLabel.Size = new System.Drawing.Size(608, 20);
            this.copyrightMySqlLabel.TabIndex = 6;
            this.copyrightMySqlLabel.Text = "MySQL - Copyright 1995-2007, www.mysql.com";
            this.copyrightMySqlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // licenseLabel
            // 
            this.licenseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.licenseLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.licenseLabel.Location = new System.Drawing.Point(13, 251);
            this.licenseLabel.Name = "licenseLabel";
            this.licenseLabel.Size = new System.Drawing.Size(608, 20);
            this.licenseLabel.TabIndex = 7;
            this.licenseLabel.Text = "All parts of this program are licensed under the GPL, www.opensource.org/licenses" +
    "/gpl-license.php";
            this.licenseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // licensesButton
            // 
            this.licensesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.licensesButton.Location = new System.Drawing.Point(512, 117);
            this.licensesButton.Name = "licensesButton";
            this.licensesButton.Size = new System.Drawing.Size(110, 30);
            this.licensesButton.TabIndex = 50;
            this.licensesButton.Text = "View Licenses";
            this.licensesButton.Click += new System.EventHandler(this.LicensesButton_Click);
            // 
            // copyrightAdaLabel
            // 
            this.copyrightAdaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyrightAdaLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.copyrightAdaLabel.Location = new System.Drawing.Point(13, 291);
            this.copyrightAdaLabel.Name = "copyrightAdaLabel";
            this.copyrightAdaLabel.Size = new System.Drawing.Size(608, 20);
            this.copyrightAdaLabel.TabIndex = 51;
            this.copyrightAdaLabel.Text = "All CDT codes are Copyrighted by the ADA.";
            this.copyrightAdaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverNameLabel.Location = new System.Drawing.Point(161, 44);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(326, 20);
            this.serverNameLabel.TabIndex = 52;
            this.serverNameLabel.Text = "-";
            this.serverNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceNameLabel
            // 
            this.serviceNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceNameLabel.Location = new System.Drawing.Point(161, 69);
            this.serviceNameLabel.Name = "serviceNameLabel";
            this.serviceNameLabel.Size = new System.Drawing.Size(326, 20);
            this.serviceNameLabel.TabIndex = 53;
            this.serviceNameLabel.Text = "-";
            this.serviceNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceVersionLabel
            // 
            this.serviceVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceVersionLabel.Location = new System.Drawing.Point(161, 94);
            this.serviceVersionLabel.Name = "serviceVersionLabel";
            this.serviceVersionLabel.Size = new System.Drawing.Size(326, 20);
            this.serviceVersionLabel.TabIndex = 54;
            this.serviceVersionLabel.Text = "-";
            this.serviceVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceCommentLabel
            // 
            this.serviceCommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceCommentLabel.Location = new System.Drawing.Point(161, 119);
            this.serviceCommentLabel.Name = "serviceCommentLabel";
            this.serviceCommentLabel.Size = new System.Drawing.Size(326, 20);
            this.serviceCommentLabel.TabIndex = 55;
            this.serviceCommentLabel.Text = "-";
            this.serviceCommentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(13, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(608, 2);
            this.label2.TabIndex = 56;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionGroupBox.Controls.Add(this.machineNameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceNameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serverNameCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceCommentsCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.serviceVersionCaptionLabel);
            this.connectionGroupBox.Controls.Add(this.machineNameLabel);
            this.connectionGroupBox.Controls.Add(this.serviceNameLabel);
            this.connectionGroupBox.Controls.Add(this.serverNameLabel);
            this.connectionGroupBox.Controls.Add(this.serviceCommentLabel);
            this.connectionGroupBox.Controls.Add(this.serviceVersionLabel);
            this.connectionGroupBox.Location = new System.Drawing.Point(13, 75);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(493, 150);
            this.connectionGroupBox.TabIndex = 57;
            this.connectionGroupBox.TabStop = false;
            // 
            // machineNameCaptionLabel
            // 
            this.machineNameCaptionLabel.Location = new System.Drawing.Point(6, 19);
            this.machineNameCaptionLabel.Name = "machineNameCaptionLabel";
            this.machineNameCaptionLabel.Size = new System.Drawing.Size(149, 20);
            this.machineNameCaptionLabel.TabIndex = 92;
            this.machineNameCaptionLabel.Text = "Client Machine Name: ";
            this.machineNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceNameCaptionLabel
            // 
            this.serviceNameCaptionLabel.Location = new System.Drawing.Point(6, 69);
            this.serviceNameCaptionLabel.Name = "serviceNameCaptionLabel";
            this.serviceNameCaptionLabel.Size = new System.Drawing.Size(149, 20);
            this.serviceNameCaptionLabel.TabIndex = 89;
            this.serviceNameCaptionLabel.Text = "Service Name: ";
            this.serviceNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serverNameCaptionLabel
            // 
            this.serverNameCaptionLabel.Location = new System.Drawing.Point(6, 44);
            this.serverNameCaptionLabel.Name = "serverNameCaptionLabel";
            this.serverNameCaptionLabel.Size = new System.Drawing.Size(149, 20);
            this.serverNameCaptionLabel.TabIndex = 88;
            this.serverNameCaptionLabel.Text = "Server Name: ";
            this.serverNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceCommentsCaptionLabel
            // 
            this.serviceCommentsCaptionLabel.Location = new System.Drawing.Point(6, 119);
            this.serviceCommentsCaptionLabel.Name = "serviceCommentsCaptionLabel";
            this.serviceCommentsCaptionLabel.Size = new System.Drawing.Size(149, 20);
            this.serviceCommentsCaptionLabel.TabIndex = 91;
            this.serviceCommentsCaptionLabel.Text = "Service Comment: ";
            this.serviceCommentsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceVersionCaptionLabel
            // 
            this.serviceVersionCaptionLabel.Location = new System.Drawing.Point(6, 94);
            this.serviceVersionCaptionLabel.Name = "serviceVersionCaptionLabel";
            this.serviceVersionCaptionLabel.Size = new System.Drawing.Size(149, 20);
            this.serviceVersionCaptionLabel.TabIndex = 90;
            this.serviceVersionCaptionLabel.Text = "Service Version: ";
            this.serviceVersionCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // machineNameLabel
            // 
            this.machineNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.machineNameLabel.Location = new System.Drawing.Point(161, 19);
            this.machineNameLabel.Name = "machineNameLabel";
            this.machineNameLabel.Size = new System.Drawing.Size(326, 20);
            this.machineNameLabel.TabIndex = 87;
            this.machineNameLabel.Text = "-";
            this.machineNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureOpenDental
            // 
            this.pictureOpenDental.Image = ((System.Drawing.Image)(resources.GetObject("pictureOpenDental.Image")));
            this.pictureOpenDental.Location = new System.Drawing.Point(13, 19);
            this.pictureOpenDental.Name = "pictureOpenDental";
            this.pictureOpenDental.Size = new System.Drawing.Size(200, 50);
            this.pictureOpenDental.TabIndex = 58;
            this.pictureOpenDental.TabStop = false;
            // 
            // diagnosticsButton
            // 
            this.diagnosticsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.diagnosticsButton.Location = new System.Drawing.Point(512, 81);
            this.diagnosticsButton.Name = "diagnosticsButton";
            this.diagnosticsButton.Size = new System.Drawing.Size(110, 30);
            this.diagnosticsButton.TabIndex = 59;
            this.diagnosticsButton.Text = "Diagnostics";
            this.diagnosticsButton.Click += new System.EventHandler(this.DiagnosticsButton_Click);
            // 
            // FormAbout
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(634, 341);
            this.Controls.Add(this.diagnosticsButton);
            this.Controls.Add(this.pictureOpenDental);
            this.Controls.Add(this.connectionGroupBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.licensesButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.copyrightAdaLabel);
            this.Controls.Add(this.licenseLabel);
            this.Controls.Add(this.copyrightMySqlLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 380);
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.connectionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label copyrightMySqlLabel;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.Button licensesButton;
        private System.Windows.Forms.Label copyrightAdaLabel;
        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.Label serviceNameLabel;
        private System.Windows.Forms.Label serviceVersionLabel;
        private System.Windows.Forms.Label serviceCommentLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.PictureBox pictureOpenDental;
        private System.Windows.Forms.Label machineNameLabel;
        private System.Windows.Forms.Button diagnosticsButton;
        private System.Windows.Forms.Label machineNameCaptionLabel;
        private System.Windows.Forms.Label serviceNameCaptionLabel;
        private System.Windows.Forms.Label serverNameCaptionLabel;
        private System.Windows.Forms.Label serviceCommentsCaptionLabel;
        private System.Windows.Forms.Label serviceVersionCaptionLabel;
    }
}