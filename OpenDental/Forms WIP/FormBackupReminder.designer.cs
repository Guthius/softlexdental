namespace OpenDental
{
    partial class FormBackupReminder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackupReminder));
            this.acceptButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.checkNoBackups = new System.Windows.Forms.CheckBox();
            this.checkA1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkA2 = new System.Windows.Forms.CheckBox();
            this.checkA4 = new System.Windows.Forms.CheckBox();
            this.checkA3 = new System.Windows.Forms.CheckBox();
            this.checkB2 = new System.Windows.Forms.CheckBox();
            this.checkB1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkNoProof = new System.Windows.Forms.CheckBox();
            this.checkNoStrategy = new System.Windows.Forms.CheckBox();
            this.checkC2 = new System.Windows.Forms.CheckBox();
            this.checkC1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(511, 498);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(608, 80);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // checkNoBackups
            // 
            this.checkNoBackups.AutoSize = true;
            this.checkNoBackups.Location = new System.Drawing.Point(53, 239);
            this.checkNoBackups.Name = "checkNoBackups";
            this.checkNoBackups.Size = new System.Drawing.Size(89, 19);
            this.checkNoBackups.TabIndex = 6;
            this.checkNoBackups.Text = "No backups";
            this.checkNoBackups.UseVisualStyleBackColor = true;
            // 
            // checkA1
            // 
            this.checkA1.AutoSize = true;
            this.checkA1.Location = new System.Drawing.Point(53, 139);
            this.checkA1.Name = "checkA1";
            this.checkA1.Size = new System.Drawing.Size(61, 19);
            this.checkA1.TabIndex = 2;
            this.checkA1.Text = "Online";
            this.checkA1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 116);
            this.label3.Margin = new System.Windows.Forms.Padding(40, 20, 3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(309, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Do you make backups every single day?  Backup method:";
            // 
            // checkA2
            // 
            this.checkA2.AutoSize = true;
            this.checkA2.Location = new System.Drawing.Point(53, 164);
            this.checkA2.Name = "checkA2";
            this.checkA2.Size = new System.Drawing.Size(236, 19);
            this.checkA2.TabIndex = 3;
            this.checkA2.Text = "Removable (external HD, USB drive, etc)";
            this.checkA2.UseVisualStyleBackColor = true;
            // 
            // checkA4
            // 
            this.checkA4.AutoSize = true;
            this.checkA4.Location = new System.Drawing.Point(53, 214);
            this.checkA4.Name = "checkA4";
            this.checkA4.Size = new System.Drawing.Size(143, 19);
            this.checkA4.TabIndex = 5;
            this.checkA4.Text = "Other backup method";
            this.checkA4.UseVisualStyleBackColor = true;
            // 
            // checkA3
            // 
            this.checkA3.AutoSize = true;
            this.checkA3.Location = new System.Drawing.Point(53, 189);
            this.checkA3.Name = "checkA3";
            this.checkA3.Size = new System.Drawing.Size(265, 19);
            this.checkA3.TabIndex = 4;
            this.checkA3.Text = "Network (to another computer in your office)";
            this.checkA3.UseVisualStyleBackColor = true;
            // 
            // checkB2
            // 
            this.checkB2.AutoSize = true;
            this.checkB2.Location = new System.Drawing.Point(53, 329);
            this.checkB2.Name = "checkB2";
            this.checkB2.Size = new System.Drawing.Size(202, 19);
            this.checkB2.TabIndex = 9;
            this.checkB2.Text = "Run backup from a second server";
            this.checkB2.UseVisualStyleBackColor = true;
            // 
            // checkB1
            // 
            this.checkB1.AutoSize = true;
            this.checkB1.Location = new System.Drawing.Point(53, 304);
            this.checkB1.Name = "checkB1";
            this.checkB1.Size = new System.Drawing.Size(276, 19);
            this.checkB1.TabIndex = 8;
            this.checkB1.Text = "Restore to home computer at least once a week";
            this.checkB1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 281);
            this.label2.Margin = new System.Windows.Forms.Padding(40, 20, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(324, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "What proof do you have that your recent backups are good?";
            // 
            // checkNoProof
            // 
            this.checkNoProof.AutoSize = true;
            this.checkNoProof.Location = new System.Drawing.Point(53, 354);
            this.checkNoProof.Name = "checkNoProof";
            this.checkNoProof.Size = new System.Drawing.Size(74, 19);
            this.checkNoProof.TabIndex = 10;
            this.checkNoProof.Text = "No proof";
            this.checkNoProof.UseVisualStyleBackColor = true;
            // 
            // checkNoStrategy
            // 
            this.checkNoStrategy.AutoSize = true;
            this.checkNoStrategy.Location = new System.Drawing.Point(53, 469);
            this.checkNoStrategy.Name = "checkNoStrategy";
            this.checkNoStrategy.Size = new System.Drawing.Size(87, 19);
            this.checkNoStrategy.TabIndex = 14;
            this.checkNoStrategy.Text = "No strategy";
            this.checkNoStrategy.UseVisualStyleBackColor = true;
            // 
            // checkC2
            // 
            this.checkC2.AutoSize = true;
            this.checkC2.Location = new System.Drawing.Point(53, 444);
            this.checkC2.Name = "checkC2";
            this.checkC2.Size = new System.Drawing.Size(183, 19);
            this.checkC2.TabIndex = 13;
            this.checkC2.Text = "Saved hardcopy paper reports";
            this.checkC2.UseVisualStyleBackColor = true;
            // 
            // checkC1
            // 
            this.checkC1.AutoSize = true;
            this.checkC1.Location = new System.Drawing.Point(53, 419);
            this.checkC1.Name = "checkC1";
            this.checkC1.Size = new System.Drawing.Size(331, 19);
            this.checkC1.TabIndex = 12;
            this.checkC1.Text = "Completely separate archives stored offsite (DVD, HD, etc)";
            this.checkC1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 396);
            this.label4.Margin = new System.Windows.Forms.Padding(40, 20, 3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(428, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "What secondary long-term mechanism do you use to ensure minimal data loss?";
            // 
            // FormBackupReminder
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(634, 541);
            this.ControlBox = false;
            this.Controls.Add(this.checkNoStrategy);
            this.Controls.Add(this.checkC2);
            this.Controls.Add(this.checkC1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkNoProof);
            this.Controls.Add(this.checkB2);
            this.Controls.Add(this.checkB1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkA4);
            this.Controls.Add(this.checkA3);
            this.Controls.Add(this.checkA2);
            this.Controls.Add(this.checkA1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkNoBackups);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBackupReminder";
            this.ShowInTaskbar = false;
            this.Text = "Backup Reminder";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBackupReminder_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.CheckBox checkNoBackups;
        private System.Windows.Forms.CheckBox checkA1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkA2;
        private System.Windows.Forms.CheckBox checkA4;
        private System.Windows.Forms.CheckBox checkA3;
        private System.Windows.Forms.CheckBox checkB2;
        private System.Windows.Forms.CheckBox checkB1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkNoProof;
        private System.Windows.Forms.CheckBox checkNoStrategy;
        private System.Windows.Forms.CheckBox checkC2;
        private System.Windows.Forms.CheckBox checkC1;
        private System.Windows.Forms.Label label4;
    }
}
