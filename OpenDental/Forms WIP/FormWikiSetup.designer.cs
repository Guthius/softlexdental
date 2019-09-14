namespace OpenDental
{
    partial class FormWikiSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiSetup));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textMaster = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.detectLinksCheckBox = new System.Windows.Forms.CheckBox();
            this.createPageFromLinksCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(745, 598);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(861, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // textMaster
            // 
            this.textMaster.AcceptsTab = true;
            this.textMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMaster.Font = new System.Drawing.Font("Courier New", 9.5F);
            this.textMaster.Location = new System.Drawing.Point(13, 34);
            this.textMaster.Multiline = true;
            this.textMaster.Name = "textMaster";
            this.textMaster.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMaster.Size = new System.Drawing.Size(958, 542);
            this.textMaster.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Master Page";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // detectLinksCheckBox
            // 
            this.detectLinksCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detectLinksCheckBox.AutoSize = true;
            this.detectLinksCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.detectLinksCheckBox.Location = new System.Drawing.Point(13, 582);
            this.detectLinksCheckBox.Name = "detectLinksCheckBox";
            this.detectLinksCheckBox.Size = new System.Drawing.Size(234, 20);
            this.detectLinksCheckBox.TabIndex = 2;
            this.detectLinksCheckBox.Text = "Detect wiki links in textboxes and grids";
            // 
            // createPageFromLinksCheckBox
            // 
            this.createPageFromLinksCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createPageFromLinksCheckBox.AutoSize = true;
            this.createPageFromLinksCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.createPageFromLinksCheckBox.Location = new System.Drawing.Point(13, 608);
            this.createPageFromLinksCheckBox.Name = "createPageFromLinksCheckBox";
            this.createPageFromLinksCheckBox.Size = new System.Drawing.Size(201, 20);
            this.createPageFromLinksCheckBox.TabIndex = 3;
            this.createPageFromLinksCheckBox.Text = "Allow new wiki pages from links";
            // 
            // FormWikiSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.createPageFromLinksCheckBox);
            this.Controls.Add(this.detectLinksCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textMaster);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiSetup";
            this.Text = "Wiki Setup";
            this.Load += new System.EventHandler(this.FormWikiSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textMaster;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox detectLinksCheckBox;
        private System.Windows.Forms.CheckBox createPageFromLinksCheckBox;
    }
}