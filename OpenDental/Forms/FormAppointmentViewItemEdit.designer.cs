namespace OpenDental
{
    partial class FormAppointmentViewItemEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppointmentViewItemEdit));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textColorLabel = new System.Windows.Forms.Label();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.textColorButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(245, 178);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(361, 178);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // textColorLabel
            // 
            this.textColorLabel.AutoSize = true;
            this.textColorLabel.Location = new System.Drawing.Point(98, 52);
            this.textColorLabel.Name = "textColorLabel";
            this.textColorLabel.Size = new System.Drawing.Size(36, 15);
            this.textColorLabel.TabIndex = 6;
            this.textColorLabel.Text = "Color";
            // 
            // colorPanel
            // 
            this.colorPanel.BackColor = System.Drawing.Color.White;
            this.colorPanel.Location = new System.Drawing.Point(140, 48);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(24, 24);
            this.colorPanel.TabIndex = 7;
            // 
            // textColorButton
            // 
            this.textColorButton.Location = new System.Drawing.Point(170, 47);
            this.textColorButton.Name = "textColorButton";
            this.textColorButton.Size = new System.Drawing.Size(70, 25);
            this.textColorButton.TabIndex = 8;
            this.textColorButton.Text = "Change";
            this.textColorButton.Click += new System.EventHandler(this.TextColorButton_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(67, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(140, 19);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(331, 23);
            this.descriptionTextBox.TabIndex = 5;
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(81, 81);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(53, 15);
            this.locationLabel.TabIndex = 0;
            this.locationLabel.Text = "Location";
            // 
            // locationListBox
            // 
            this.locationListBox.FormattingEnabled = true;
            this.locationListBox.IntegralHeight = false;
            this.locationListBox.ItemHeight = 15;
            this.locationListBox.Items.AddRange(new object[] {
            "Main",
            "Upper Right",
            "Lower Right"});
            this.locationListBox.Location = new System.Drawing.Point(140, 78);
            this.locationListBox.Name = "locationListBox";
            this.locationListBox.Size = new System.Drawing.Size(200, 60);
            this.locationListBox.TabIndex = 1;
            // 
            // FormAppointmentViewItemEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 221);
            this.Controls.Add(this.locationListBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.textColorButton);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.textColorLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAppointmentViewItemEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment View Item Edit";
            this.Load += new System.EventHandler(this.FormApptViewItemEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label textColorLabel;
        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.Button textColorButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.ListBox locationListBox;
    }
}
