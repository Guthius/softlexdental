namespace OpenDental
{
    partial class FormAllergyDefEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAllergyDefEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.extraGroupBox = new System.Windows.Forms.GroupBox();
            this.allergenGroupBox = new System.Windows.Forms.GroupBox();
            this.uniiTextBox = new System.Windows.Forms.TextBox();
            this.uniiNoneButton = new System.Windows.Forms.Button();
            this.medicationNoneButton = new System.Windows.Forms.Button();
            this.uniiBrowseButton = new System.Windows.Forms.Button();
            this.medicationTextBox = new System.Windows.Forms.TextBox();
            this.uniiLabel = new System.Windows.Forms.Label();
            this.medicationBrowseButton = new System.Windows.Forms.Button();
            this.medicationLabel = new System.Windows.Forms.Label();
            this.allergyTypeComboBox = new System.Windows.Forms.ComboBox();
            this.allergyTypeLabel = new System.Windows.Forms.Label();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.extraGroupBox.SuspendLayout();
            this.allergenGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(441, 288);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // extraGroupBox
            // 
            this.extraGroupBox.Controls.Add(this.allergenGroupBox);
            this.extraGroupBox.Controls.Add(this.allergyTypeComboBox);
            this.extraGroupBox.Controls.Add(this.allergyTypeLabel);
            this.extraGroupBox.Location = new System.Drawing.Point(13, 80);
            this.extraGroupBox.Name = "extraGroupBox";
            this.extraGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.extraGroupBox.Size = new System.Drawing.Size(545, 180);
            this.extraGroupBox.TabIndex = 3;
            this.extraGroupBox.TabStop = false;
            this.extraGroupBox.Text = "Only used in EHR for CCDs.  Most offices can ignore this section";
            // 
            // allergenGroupBox
            // 
            this.allergenGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allergenGroupBox.Controls.Add(this.uniiTextBox);
            this.allergenGroupBox.Controls.Add(this.uniiNoneButton);
            this.allergenGroupBox.Controls.Add(this.medicationNoneButton);
            this.allergenGroupBox.Controls.Add(this.uniiBrowseButton);
            this.allergenGroupBox.Controls.Add(this.medicationTextBox);
            this.allergenGroupBox.Controls.Add(this.uniiLabel);
            this.allergenGroupBox.Controls.Add(this.medicationBrowseButton);
            this.allergenGroupBox.Controls.Add(this.medicationLabel);
            this.allergenGroupBox.Location = new System.Drawing.Point(6, 67);
            this.allergenGroupBox.Name = "allergenGroupBox";
            this.allergenGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.allergenGroupBox.Size = new System.Drawing.Size(533, 107);
            this.allergenGroupBox.TabIndex = 2;
            this.allergenGroupBox.TabStop = false;
            this.allergenGroupBox.Text = "Allergen (only one)";
            // 
            // uniiTextBox
            // 
            this.uniiTextBox.Location = new System.Drawing.Point(131, 29);
            this.uniiTextBox.Name = "uniiTextBox";
            this.uniiTextBox.Size = new System.Drawing.Size(276, 23);
            this.uniiTextBox.TabIndex = 1;
            // 
            // uniiNoneButton
            // 
            this.uniiNoneButton.Enabled = false;
            this.uniiNoneButton.Location = new System.Drawing.Point(449, 28);
            this.uniiNoneButton.Name = "uniiNoneButton";
            this.uniiNoneButton.Size = new System.Drawing.Size(60, 25);
            this.uniiNoneButton.TabIndex = 3;
            this.uniiNoneButton.Text = "None";
            this.uniiNoneButton.Click += new System.EventHandler(this.UniiNoneButton_Click);
            // 
            // medicationNoneButton
            // 
            this.medicationNoneButton.Location = new System.Drawing.Point(449, 59);
            this.medicationNoneButton.Name = "medicationNoneButton";
            this.medicationNoneButton.Size = new System.Drawing.Size(60, 25);
            this.medicationNoneButton.TabIndex = 7;
            this.medicationNoneButton.Text = "None";
            this.medicationNoneButton.Click += new System.EventHandler(this.MedicationNoneButton_Click);
            // 
            // uniiBrowseButton
            // 
            this.uniiBrowseButton.Enabled = false;
            this.uniiBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("uniiBrowseButton.Image")));
            this.uniiBrowseButton.Location = new System.Drawing.Point(413, 28);
            this.uniiBrowseButton.Name = "uniiBrowseButton";
            this.uniiBrowseButton.Size = new System.Drawing.Size(30, 25);
            this.uniiBrowseButton.TabIndex = 2;
            this.uniiBrowseButton.Click += new System.EventHandler(this.UniiBrowseButton_Click);
            // 
            // medicationTextBox
            // 
            this.medicationTextBox.Location = new System.Drawing.Point(131, 60);
            this.medicationTextBox.Name = "medicationTextBox";
            this.medicationTextBox.ReadOnly = true;
            this.medicationTextBox.Size = new System.Drawing.Size(276, 23);
            this.medicationTextBox.TabIndex = 5;
            // 
            // uniiLabel
            // 
            this.uniiLabel.AutoSize = true;
            this.uniiLabel.Location = new System.Drawing.Point(95, 32);
            this.uniiLabel.Name = "uniiLabel";
            this.uniiLabel.Size = new System.Drawing.Size(30, 15);
            this.uniiLabel.TabIndex = 0;
            this.uniiLabel.Text = "UNII";
            this.uniiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // medicationBrowseButton
            // 
            this.medicationBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("medicationBrowseButton.Image")));
            this.medicationBrowseButton.Location = new System.Drawing.Point(413, 59);
            this.medicationBrowseButton.Name = "medicationBrowseButton";
            this.medicationBrowseButton.Size = new System.Drawing.Size(30, 25);
            this.medicationBrowseButton.TabIndex = 6;
            this.medicationBrowseButton.Click += new System.EventHandler(this.MedicationBrowseButton_Click);
            // 
            // medicationLabel
            // 
            this.medicationLabel.AutoSize = true;
            this.medicationLabel.Location = new System.Drawing.Point(58, 64);
            this.medicationLabel.Name = "medicationLabel";
            this.medicationLabel.Size = new System.Drawing.Size(67, 15);
            this.medicationLabel.TabIndex = 4;
            this.medicationLabel.Text = "Medication";
            this.medicationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // allergyTypeComboBox
            // 
            this.allergyTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.allergyTypeComboBox.FormattingEnabled = true;
            this.allergyTypeComboBox.Location = new System.Drawing.Point(137, 29);
            this.allergyTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.allergyTypeComboBox.Name = "allergyTypeComboBox";
            this.allergyTypeComboBox.Size = new System.Drawing.Size(276, 23);
            this.allergyTypeComboBox.TabIndex = 1;
            // 
            // allergyTypeLabel
            // 
            this.allergyTypeLabel.AutoSize = true;
            this.allergyTypeLabel.Location = new System.Drawing.Point(59, 32);
            this.allergyTypeLabel.Name = "allergyTypeLabel";
            this.allergyTypeLabel.Size = new System.Drawing.Size(72, 15);
            this.allergyTypeLabel.TabIndex = 0;
            this.allergyTypeLabel.Text = "Allergy Type";
            this.allergyTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(150, 48);
            this.hiddenCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(65, 19);
            this.hiddenCheckBox.TabIndex = 2;
            this.hiddenCheckBox.Text = "Hidden";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(150, 19);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(276, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(77, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(325, 288);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 288);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAllergyDefEdit
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(564, 331);
            this.Controls.Add(this.extraGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.deleteButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergyDefEdit";
            this.ShowInTaskbar = false;
            this.Text = "Allergy Definition";
            this.Load += new System.EventHandler(this.FormAllergyEdit_Load);
            this.extraGroupBox.ResumeLayout(false);
            this.extraGroupBox.PerformLayout();
            this.allergenGroupBox.ResumeLayout(false);
            this.allergenGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label medicationLabel;
        private System.Windows.Forms.ComboBox allergyTypeComboBox;
        private System.Windows.Forms.Label allergyTypeLabel;
        private System.Windows.Forms.TextBox medicationTextBox;
        private System.Windows.Forms.Button medicationBrowseButton;
        private System.Windows.Forms.Button medicationNoneButton;
        private System.Windows.Forms.GroupBox extraGroupBox;
        private System.Windows.Forms.Label uniiLabel;
        private System.Windows.Forms.TextBox uniiTextBox;
        private System.Windows.Forms.Button uniiBrowseButton;
        private System.Windows.Forms.Button uniiNoneButton;
        private System.Windows.Forms.GroupBox allergenGroupBox;
    }
}