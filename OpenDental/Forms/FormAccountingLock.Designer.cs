namespace OpenDental
{
    partial class FormAccountingLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountingLock));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.textDate = new ODR.ValidDate();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(311, 238);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(195, 238);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(408, 130);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // textDate
            // 
            this.textDate.Location = new System.Drawing.Point(120, 149);
            this.textDate.Name = "textDate";
            this.textDate.Size = new System.Drawing.Size(100, 23);
            this.textDate.TabIndex = 3;
            // 
            // FormAccountingLock
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 281);
            this.Controls.Add(this.textDate);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountingLock";
            this.ShowInTaskbar = false;
            this.Text = "Lock Accounting";
            this.Load += new System.EventHandler(this.FormAccountingLock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label infoLabel;
        private ODR.ValidDate textDate;
    }
}