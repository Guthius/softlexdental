namespace OpenDental
{
    partial class FormAging
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAging));
            this.lastDateTextBox = new OpenDental.ValidDate();
            this.lastDateLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.calculateDateTextBox = new OpenDental.ValidDate();
            this.calculateDateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lastDateTextBox
            // 
            this.lastDateTextBox.Location = new System.Drawing.Point(170, 89);
            this.lastDateTextBox.Name = "lastDateTextBox";
            this.lastDateTextBox.ReadOnly = true;
            this.lastDateTextBox.Size = new System.Drawing.Size(94, 23);
            this.lastDateTextBox.TabIndex = 2;
            this.lastDateTextBox.TabStop = false;
            // 
            // lastDateLabel
            // 
            this.lastDateLabel.AutoSize = true;
            this.lastDateLabel.Location = new System.Drawing.Point(77, 92);
            this.lastDateLabel.Name = "lastDateLabel";
            this.lastDateLabel.Size = new System.Drawing.Size(87, 15);
            this.lastDateLabel.TabIndex = 1;
            this.lastDateLabel.Text = "Last Calculated";
            this.lastDateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(311, 198);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(195, 198);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.BackColor = System.Drawing.SystemColors.Control;
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(408, 70);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "If you use monthly billing instead of daily, then this is where you change the ag" +
    "ing date every month.  Otherwise, it\'s not necessary to manually run aging.  It\'" +
    "s all handled automatically.";
            // 
            // calculateDateTextBox
            // 
            this.calculateDateTextBox.Location = new System.Drawing.Point(170, 118);
            this.calculateDateTextBox.Name = "calculateDateTextBox";
            this.calculateDateTextBox.Size = new System.Drawing.Size(94, 23);
            this.calculateDateTextBox.TabIndex = 4;
            // 
            // calculateDateLabel
            // 
            this.calculateDateLabel.AutoSize = true;
            this.calculateDateLabel.Location = new System.Drawing.Point(80, 121);
            this.calculateDateLabel.Name = "calculateDateLabel";
            this.calculateDateLabel.Size = new System.Drawing.Size(84, 15);
            this.calculateDateLabel.TabIndex = 3;
            this.calculateDateLabel.Text = "Calculate as of";
            this.calculateDateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormAging
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 241);
            this.Controls.Add(this.calculateDateTextBox);
            this.Controls.Add(this.calculateDateLabel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.lastDateTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.lastDateLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAging";
            this.ShowInTaskbar = false;
            this.Text = "Calculate Aging";
            this.Load += new System.EventHandler(this.FormAging_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label lastDateLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label infoLabel;
        private OpenDental.ValidDate lastDateTextBox;
        private OpenDental.ValidDate calculateDateTextBox;
        private System.Windows.Forms.Label calculateDateLabel;
    }
}