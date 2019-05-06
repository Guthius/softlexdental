namespace OpenDental
{
    partial class FormEquipmentEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEquipmentEdit));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.serialNumberTextBox = new System.Windows.Forms.TextBox();
            this.serialNumberLabel = new System.Windows.Forms.Label();
            this.modelYearTextBox = new System.Windows.Forms.TextBox();
            this.modelYearLabel = new System.Windows.Forms.Label();
            this.datePurchasedLabel = new System.Windows.Forms.Label();
            this.dateSoldLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.purchaseCostLabel = new System.Windows.Forms.Label();
            this.marketValueLabel = new System.Windows.Forms.Label();
            this.marketValueTextBox = new OpenDental.ValidDouble();
            this.dateSoldTextBox = new OpenDental.ValidDate();
            this.datePurchasedTextBox = new OpenDental.ValidDate();
            this.purchaseCostTextBox = new OpenDental.ValidDouble();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dateEntryTextBox = new System.Windows.Forms.TextBox();
            this.dateEntryLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusTextBox = new OpenDental.ODtextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(87, 51);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(160, 48);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(411, 23);
            this.descriptionTextBox.TabIndex = 3;
            // 
            // serialNumberTextBox
            // 
            this.serialNumberTextBox.Location = new System.Drawing.Point(160, 77);
            this.serialNumberTextBox.Name = "serialNumberTextBox";
            this.serialNumberTextBox.Size = new System.Drawing.Size(144, 23);
            this.serialNumberTextBox.TabIndex = 5;
            // 
            // serialNumberLabel
            // 
            this.serialNumberLabel.AutoSize = true;
            this.serialNumberLabel.Location = new System.Drawing.Point(72, 80);
            this.serialNumberLabel.Name = "serialNumberLabel";
            this.serialNumberLabel.Size = new System.Drawing.Size(82, 15);
            this.serialNumberLabel.TabIndex = 4;
            this.serialNumberLabel.Text = "Serial Number";
            this.serialNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // modelYearTextBox
            // 
            this.modelYearTextBox.Location = new System.Drawing.Point(160, 106);
            this.modelYearTextBox.MaxLength = 2;
            this.modelYearTextBox.Name = "modelYearTextBox";
            this.modelYearTextBox.Size = new System.Drawing.Size(42, 23);
            this.modelYearTextBox.TabIndex = 8;
            // 
            // modelYearLabel
            // 
            this.modelYearLabel.AutoSize = true;
            this.modelYearLabel.Location = new System.Drawing.Point(56, 109);
            this.modelYearLabel.Name = "modelYearLabel";
            this.modelYearLabel.Size = new System.Drawing.Size(98, 15);
            this.modelYearLabel.TabIndex = 7;
            this.modelYearLabel.Text = "Model Yr (2 digit)";
            this.modelYearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePurchasedLabel
            // 
            this.datePurchasedLabel.AutoSize = true;
            this.datePurchasedLabel.Location = new System.Drawing.Point(65, 138);
            this.datePurchasedLabel.Name = "datePurchasedLabel";
            this.datePurchasedLabel.Size = new System.Drawing.Size(89, 15);
            this.datePurchasedLabel.TabIndex = 9;
            this.datePurchasedLabel.Text = "Date Purchased";
            this.datePurchasedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateSoldLabel
            // 
            this.dateSoldLabel.AutoSize = true;
            this.dateSoldLabel.Location = new System.Drawing.Point(97, 167);
            this.dateSoldLabel.Name = "dateSoldLabel";
            this.dateSoldLabel.Size = new System.Drawing.Size(57, 15);
            this.dateSoldLabel.TabIndex = 11;
            this.dateSoldLabel.Text = "Date Sold";
            this.dateSoldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // locationTextBox
            // 
            this.locationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.locationTextBox.Location = new System.Drawing.Point(160, 251);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(411, 23);
            this.locationTextBox.TabIndex = 18;
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(101, 254);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(53, 15);
            this.locationLabel.TabIndex = 17;
            this.locationLabel.Text = "Location";
            this.locationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // purchaseCostLabel
            // 
            this.purchaseCostLabel.AutoSize = true;
            this.purchaseCostLabel.Location = new System.Drawing.Point(72, 196);
            this.purchaseCostLabel.Name = "purchaseCostLabel";
            this.purchaseCostLabel.Size = new System.Drawing.Size(82, 15);
            this.purchaseCostLabel.TabIndex = 13;
            this.purchaseCostLabel.Text = "Purchase Cost";
            this.purchaseCostLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // marketValueLabel
            // 
            this.marketValueLabel.AutoSize = true;
            this.marketValueLabel.Location = new System.Drawing.Point(24, 225);
            this.marketValueLabel.Name = "marketValueLabel";
            this.marketValueLabel.Size = new System.Drawing.Size(130, 15);
            this.marketValueLabel.TabIndex = 15;
            this.marketValueLabel.Text = "Estimated Market Value";
            this.marketValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // marketValueTextBox
            // 
            this.marketValueTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.marketValueTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.marketValueTextBox.Location = new System.Drawing.Point(160, 222);
            this.marketValueTextBox.MaxVal = 100000000D;
            this.marketValueTextBox.MinVal = -100000000D;
            this.marketValueTextBox.Name = "marketValueTextBox";
            this.marketValueTextBox.Size = new System.Drawing.Size(100, 23);
            this.marketValueTextBox.TabIndex = 16;
            // 
            // dateSoldTextBox
            // 
            this.dateSoldTextBox.Location = new System.Drawing.Point(160, 164);
            this.dateSoldTextBox.Name = "dateSoldTextBox";
            this.dateSoldTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateSoldTextBox.TabIndex = 12;
            // 
            // datePurchasedTextBox
            // 
            this.datePurchasedTextBox.Location = new System.Drawing.Point(160, 135);
            this.datePurchasedTextBox.Name = "datePurchasedTextBox";
            this.datePurchasedTextBox.Size = new System.Drawing.Size(100, 23);
            this.datePurchasedTextBox.TabIndex = 10;
            // 
            // purchaseCostTextBox
            // 
            this.purchaseCostTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.purchaseCostTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.purchaseCostTextBox.Location = new System.Drawing.Point(160, 193);
            this.purchaseCostTextBox.MaxVal = 100000000D;
            this.purchaseCostTextBox.MinVal = -100000000D;
            this.purchaseCostTextBox.Name = "purchaseCostTextBox";
            this.purchaseCostTextBox.Size = new System.Drawing.Size(100, 23);
            this.purchaseCostTextBox.TabIndex = 14;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(12, 399);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 21;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(346, 399);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 22;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(462, 399);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 23;
            this.cancelButton.Text = "&Cancel";
            // 
            // dateEntryTextBox
            // 
            this.dateEntryTextBox.Location = new System.Drawing.Point(160, 19);
            this.dateEntryTextBox.Name = "dateEntryTextBox";
            this.dateEntryTextBox.ReadOnly = true;
            this.dateEntryTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateEntryTextBox.TabIndex = 1;
            this.dateEntryTextBox.TabStop = false;
            // 
            // dateEntryLabel
            // 
            this.dateEntryLabel.AutoSize = true;
            this.dateEntryLabel.Location = new System.Drawing.Point(93, 22);
            this.dateEntryLabel.Name = "dateEntryLabel";
            this.dateEntryLabel.Size = new System.Drawing.Size(61, 15);
            this.dateEntryLabel.TabIndex = 0;
            this.dateEntryLabel.Text = "Date Entry";
            this.dateEntryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(115, 283);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 15);
            this.statusLabel.TabIndex = 19;
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusTextBox
            // 
            this.statusTextBox.AcceptsTab = true;
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.statusTextBox.DetectLinksEnabled = false;
            this.statusTextBox.DetectUrls = false;
            this.statusTextBox.Location = new System.Drawing.Point(160, 280);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Equipment;
            this.statusTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.statusTextBox.Size = new System.Drawing.Size(411, 80);
            this.statusTextBox.TabIndex = 20;
            this.statusTextBox.Text = "";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(310, 76);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(70, 25);
            this.generateButton.TabIndex = 6;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // FormEquipmentEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.dateEntryTextBox);
            this.Controls.Add(this.dateEntryLabel);
            this.Controls.Add(this.marketValueTextBox);
            this.Controls.Add(this.dateSoldTextBox);
            this.Controls.Add(this.datePurchasedTextBox);
            this.Controls.Add(this.purchaseCostTextBox);
            this.Controls.Add(this.marketValueLabel);
            this.Controls.Add(this.purchaseCostLabel);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.dateSoldLabel);
            this.Controls.Add(this.datePurchasedLabel);
            this.Controls.Add(this.modelYearTextBox);
            this.Controls.Add(this.modelYearLabel);
            this.Controls.Add(this.serialNumberTextBox);
            this.Controls.Add(this.serialNumberLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEquipmentEdit";
            this.ShowInTaskbar = false;
            this.Text = "Equipment";
            this.Load += new System.EventHandler(this.FormEquipmentEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox serialNumberTextBox;
        private System.Windows.Forms.Label serialNumberLabel;
        private System.Windows.Forms.TextBox modelYearTextBox;
        private System.Windows.Forms.Label modelYearLabel;
        private System.Windows.Forms.Label datePurchasedLabel;
        private System.Windows.Forms.Label dateSoldLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Label purchaseCostLabel;
        private System.Windows.Forms.Label marketValueLabel;
        private ValidDouble purchaseCostTextBox;
        private ValidDate datePurchasedTextBox;
        private ValidDate dateSoldTextBox;
        private ValidDouble marketValueTextBox;
        private System.Windows.Forms.TextBox dateEntryTextBox;
        private System.Windows.Forms.Label dateEntryLabel;
        private System.Windows.Forms.Label statusLabel;
        private ODtextBox statusTextBox;
        private System.Windows.Forms.Button generateButton;
    }
}