namespace OpenDental
{
    partial class FormUpdateInProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateInProgress));
            this.warningLabel = new System.Windows.Forms.Label();
            this.retryButton = new System.Windows.Forms.Button();
            this.overrideButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // warningLabel
            // 
            this.warningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.warningLabel.Location = new System.Drawing.Point(13, 16);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(508, 129);
            this.warningLabel.TabIndex = 2;
            this.warningLabel.Text = "Warning";
            // 
            // retryButton
            // 
            this.retryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.retryButton.Location = new System.Drawing.Point(295, 148);
            this.retryButton.Name = "retryButton";
            this.retryButton.Size = new System.Drawing.Size(110, 30);
            this.retryButton.TabIndex = 0;
            this.retryButton.Text = "Try Again";
            this.retryButton.Click += new System.EventHandler(this.RetryButton_Click);
            // 
            // overrideButton
            // 
            this.overrideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.overrideButton.Location = new System.Drawing.Point(13, 148);
            this.overrideButton.Name = "overrideButton";
            this.overrideButton.Size = new System.Drawing.Size(110, 30);
            this.overrideButton.TabIndex = 3;
            this.overrideButton.Text = "Override";
            this.overrideButton.Visible = false;
            this.overrideButton.Click += new System.EventHandler(this.OverrideButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(411, 148);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            // 
            // FormUpdateInProgress
            // 
            this.AcceptButton = this.retryButton;
            this.ClientSize = new System.Drawing.Size(534, 191);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.retryButton);
            this.Controls.Add(this.overrideButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdateInProgress";
            this.ShowInTaskbar = false;
            this.Text = "Update In Progress";
            this.Load += new System.EventHandler(this.FormUpdateInProgress_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button retryButton;
        private System.Windows.Forms.Button overrideButton;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Button cancelButton;
    }
}