namespace OpenDental
{
    partial class FormUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdate));
            this.versionLabel = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.checkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.previousVersionsButton = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(13, 16);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(458, 20);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Using Version ";
            // 
            // logTextBox
            // 
            this.logTextBox.AcceptsReturn = true;
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.logTextBox.Location = new System.Drawing.Point(13, 75);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(458, 220);
            this.logTextBox.TabIndex = 4;
            // 
            // checkButton
            // 
            this.checkButton.Enabled = false;
            this.checkButton.Location = new System.Drawing.Point(13, 39);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(160, 30);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "Check for Updates";
            this.checkButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(361, 318);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // previousVersionsButton
            // 
            this.previousVersionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previousVersionsButton.Location = new System.Drawing.Point(311, 39);
            this.previousVersionsButton.Name = "previousVersionsButton";
            this.previousVersionsButton.Size = new System.Drawing.Size(160, 30);
            this.previousVersionsButton.TabIndex = 3;
            this.previousVersionsButton.Text = "Show Previous Versions";
            this.previousVersionsButton.Click += new System.EventHandler(this.PreviousVersionsButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setupButton.Image = global::OpenDental.Properties.Resources.IconCog;
            this.setupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.setupButton.Location = new System.Drawing.Point(13, 318);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(110, 30);
            this.setupButton.TabIndex = 6;
            this.setupButton.Text = "&Setup";
            this.setupButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.setupButton.Visible = false;
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // FormUpdate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.previousVersionsButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.versionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdate";
            this.ShowInTaskbar = false;
            this.Text = "Update";
            this.Load += new System.EventHandler(this.FormUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button previousVersionsButton;
        private System.Windows.Forms.Button setupButton;
    }
}