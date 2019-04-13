using System.Windows.Forms;

namespace OpenDental
{
    partial class FormAbout
    {
        private Label labelVersion;
        private Button acceptButton;
        private Label labelCopyright;
        private Label labelMySQLCopyright;
        private Label label4;
        private Button licensesButton;
        private Label label9;
        private Label labelName;
        private Label labelService;
        private Label labelMySqlVersion;
        private Label labelServComment;
        private Label label2;
        private GroupBox connectionGroupBox;
        private PictureBox pictureOpenDental;
        private Label labelMachineName;
        private Button diagnosticsButton;

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
            this.labelVersion = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelMySQLCopyright = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.licensesButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelService = new System.Windows.Forms.Label();
            this.labelMySqlVersion = new System.Windows.Forms.Label();
            this.labelServComment = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelMachineName = new System.Windows.Forms.Label();
            this.pictureOpenDental = new System.Windows.Forms.PictureBox();
            this.diagnosticsButton = new System.Windows.Forms.Button();
            this.connectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).BeginInit();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.Location = new System.Drawing.Point(219, 19);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(402, 20);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "Version: v1.0.0.0";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // labelCopyright
            // 
            this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCopyright.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelCopyright.Location = new System.Drawing.Point(13, 271);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(608, 20);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "This program Copyright 2003-2007, Jordan S. Sparks, D.M.D.";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMySQLCopyright
            // 
            this.labelMySQLCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMySQLCopyright.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMySQLCopyright.Location = new System.Drawing.Point(13, 311);
            this.labelMySQLCopyright.Name = "labelMySQLCopyright";
            this.labelMySQLCopyright.Size = new System.Drawing.Size(608, 20);
            this.labelMySQLCopyright.TabIndex = 6;
            this.labelMySQLCopyright.Text = "MySQL - Copyright 1995-2007, www.mysql.com";
            this.labelMySQLCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(13, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(608, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "All parts of this program are licensed under the GPL, www.opensource.org/licenses" +
    "/gpl-license.php";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // licensesButton
            // 
            this.licensesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.licensesButton.Location = new System.Drawing.Point(512, 117);
            this.licensesButton.Name = "licensesButton";
            this.licensesButton.Size = new System.Drawing.Size(110, 30);
            this.licensesButton.TabIndex = 50;
            this.licensesButton.Text = "View Licenses";
            this.licensesButton.Click += new System.EventHandler(this.licensesButton_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label9.Location = new System.Drawing.Point(13, 291);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(608, 20);
            this.label9.TabIndex = 51;
            this.label9.Text = "All CDT codes are Copyrighted by the ADA.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.Location = new System.Drawing.Point(161, 44);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(326, 20);
            this.labelName.TabIndex = 52;
            this.labelName.Text = "-";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelService
            // 
            this.labelService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelService.Location = new System.Drawing.Point(161, 69);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(326, 20);
            this.labelService.TabIndex = 53;
            this.labelService.Text = "-";
            this.labelService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMySqlVersion
            // 
            this.labelMySqlVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMySqlVersion.Location = new System.Drawing.Point(161, 94);
            this.labelMySqlVersion.Name = "labelMySqlVersion";
            this.labelMySqlVersion.Size = new System.Drawing.Size(326, 20);
            this.labelMySqlVersion.TabIndex = 54;
            this.labelMySqlVersion.Text = "-";
            this.labelMySqlVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelServComment
            // 
            this.labelServComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelServComment.Location = new System.Drawing.Point(161, 119);
            this.labelServComment.Name = "labelServComment";
            this.labelServComment.Size = new System.Drawing.Size(326, 20);
            this.labelServComment.TabIndex = 55;
            this.labelServComment.Text = "-";
            this.labelServComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.connectionGroupBox.Controls.Add(this.label1);
            this.connectionGroupBox.Controls.Add(this.label3);
            this.connectionGroupBox.Controls.Add(this.label5);
            this.connectionGroupBox.Controls.Add(this.label6);
            this.connectionGroupBox.Controls.Add(this.label7);
            this.connectionGroupBox.Controls.Add(this.labelMachineName);
            this.connectionGroupBox.Controls.Add(this.labelService);
            this.connectionGroupBox.Controls.Add(this.labelName);
            this.connectionGroupBox.Controls.Add(this.labelServComment);
            this.connectionGroupBox.Controls.Add(this.labelMySqlVersion);
            this.connectionGroupBox.Location = new System.Drawing.Point(13, 75);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(493, 150);
            this.connectionGroupBox.TabIndex = 57;
            this.connectionGroupBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 20);
            this.label1.TabIndex = 92;
            this.label1.Text = "Client Machine Name: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 20);
            this.label3.TabIndex = 89;
            this.label3.Text = "Service Name: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 20);
            this.label5.TabIndex = 88;
            this.label5.Text = "Server Name: ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 20);
            this.label6.TabIndex = 91;
            this.label6.Text = "Service Comment: ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 20);
            this.label7.TabIndex = 90;
            this.label7.Text = "Service Version: ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMachineName
            // 
            this.labelMachineName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMachineName.Location = new System.Drawing.Point(161, 19);
            this.labelMachineName.Name = "labelMachineName";
            this.labelMachineName.Size = new System.Drawing.Size(326, 20);
            this.labelMachineName.TabIndex = 87;
            this.labelMachineName.Text = "-";
            this.labelMachineName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.diagnosticsButton.Click += new System.EventHandler(this.diagnosticsButton_Click);
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
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelMySQLCopyright);
            this.Controls.Add(this.labelCopyright);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 380);
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.connectionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Label label1;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}