namespace OpenDental
{
    partial class FormOperatoryEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatoryEdit));
            this.pickHygienistButton = new System.Windows.Forms.Button();
            this.pickProviderButton = new System.Windows.Forms.Button();
            this.pickClinicButton = new System.Windows.Forms.Button();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.prospectiveLabel = new System.Windows.Forms.Label();
            this.hygieneLabel = new System.Windows.Forms.Label();
            this.prospectiveCheckBox = new System.Windows.Forms.CheckBox();
            this.hygieneCheckBox = new System.Windows.Forms.CheckBox();
            this.hygienistComboBox = new System.Windows.Forms.ComboBox();
            this.providerComboBox = new System.Windows.Forms.ComboBox();
            this.hygienistLabel = new System.Windows.Forms.Label();
            this.providerLabel = new System.Windows.Forms.Label();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.abbrevTextBox = new System.Windows.Forms.TextBox();
            this.abbrevLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pickHygienistButton
            // 
            this.pickHygienistButton.Location = new System.Drawing.Point(426, 159);
            this.pickHygienistButton.Name = "pickHygienistButton";
            this.pickHygienistButton.Size = new System.Drawing.Size(30, 25);
            this.pickHygienistButton.TabIndex = 13;
            this.pickHygienistButton.Text = "...";
            this.pickHygienistButton.Click += new System.EventHandler(this.PickHygienistButton_Click);
            // 
            // pickProviderButton
            // 
            this.pickProviderButton.Location = new System.Drawing.Point(426, 130);
            this.pickProviderButton.Name = "pickProviderButton";
            this.pickProviderButton.Size = new System.Drawing.Size(30, 25);
            this.pickProviderButton.TabIndex = 10;
            this.pickProviderButton.Text = "...";
            this.pickProviderButton.Click += new System.EventHandler(this.PickProviderButton_Click);
            // 
            // pickClinicButton
            // 
            this.pickClinicButton.Location = new System.Drawing.Point(426, 101);
            this.pickClinicButton.Name = "pickClinicButton";
            this.pickClinicButton.Size = new System.Drawing.Size(30, 25);
            this.pickClinicButton.TabIndex = 7;
            this.pickClinicButton.Text = "...";
            this.pickClinicButton.Click += new System.EventHandler(this.PickClinicButton_Click);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(160, 102);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(260, 23);
            this.clinicComboBox.TabIndex = 6;
            this.clinicComboBox.SelectedIndexChanged += new System.EventHandler(this.ClinicComboBox_SelectedIndexChanged);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(117, 105);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 5;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // prospectiveLabel
            // 
            this.prospectiveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prospectiveLabel.Location = new System.Drawing.Point(157, 273);
            this.prospectiveLabel.Name = "prospectiveLabel";
            this.prospectiveLabel.Size = new System.Drawing.Size(334, 30);
            this.prospectiveLabel.TabIndex = 17;
            this.prospectiveLabel.Text = "Change status of patients in this operatory to prospective.";
            // 
            // hygieneLabel
            // 
            this.hygieneLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hygieneLabel.Location = new System.Drawing.Point(157, 218);
            this.hygieneLabel.Name = "hygieneLabel";
            this.hygieneLabel.Size = new System.Drawing.Size(334, 30);
            this.hygieneLabel.TabIndex = 15;
            this.hygieneLabel.Text = "The hygienist will be considered the main provider for this op.";
            // 
            // prospectiveCheckBox
            // 
            this.prospectiveCheckBox.AutoSize = true;
            this.prospectiveCheckBox.Location = new System.Drawing.Point(160, 251);
            this.prospectiveCheckBox.Name = "prospectiveCheckBox";
            this.prospectiveCheckBox.Size = new System.Drawing.Size(106, 19);
            this.prospectiveCheckBox.TabIndex = 16;
            this.prospectiveCheckBox.Text = "Set Prospective";
            // 
            // hygieneCheckBox
            // 
            this.hygieneCheckBox.AutoSize = true;
            this.hygieneCheckBox.Location = new System.Drawing.Point(160, 196);
            this.hygieneCheckBox.Name = "hygieneCheckBox";
            this.hygieneCheckBox.Size = new System.Drawing.Size(81, 19);
            this.hygieneCheckBox.TabIndex = 14;
            this.hygieneCheckBox.Text = "Is Hygiene";
            // 
            // hygienistComboBox
            // 
            this.hygienistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hygienistComboBox.Location = new System.Drawing.Point(160, 160);
            this.hygienistComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.hygienistComboBox.MaxDropDownItems = 30;
            this.hygienistComboBox.Name = "hygienistComboBox";
            this.hygienistComboBox.Size = new System.Drawing.Size(260, 23);
            this.hygienistComboBox.TabIndex = 12;
            // 
            // providerComboBox
            // 
            this.providerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.providerComboBox.Location = new System.Drawing.Point(160, 131);
            this.providerComboBox.MaxDropDownItems = 30;
            this.providerComboBox.Name = "providerComboBox";
            this.providerComboBox.Size = new System.Drawing.Size(260, 23);
            this.providerComboBox.TabIndex = 9;
            // 
            // hygienistLabel
            // 
            this.hygienistLabel.AutoSize = true;
            this.hygienistLabel.Location = new System.Drawing.Point(97, 163);
            this.hygienistLabel.Name = "hygienistLabel";
            this.hygienistLabel.Size = new System.Drawing.Size(57, 15);
            this.hygienistLabel.TabIndex = 11;
            this.hygienistLabel.Text = "Hygienist";
            this.hygienistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // providerLabel
            // 
            this.providerLabel.AutoSize = true;
            this.providerLabel.Location = new System.Drawing.Point(103, 134);
            this.providerLabel.Name = "providerLabel";
            this.providerLabel.Size = new System.Drawing.Size(51, 15);
            this.providerLabel.TabIndex = 8;
            this.providerLabel.Text = "Provider";
            this.providerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(160, 77);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(287, 19);
            this.hiddenCheckBox.TabIndex = 4;
            this.hiddenCheckBox.Text = "Is Hidden (because you can\'t delete an operatory)";
            // 
            // abbrevTextBox
            // 
            this.abbrevTextBox.Location = new System.Drawing.Point(160, 48);
            this.abbrevTextBox.MaxLength = 5;
            this.abbrevTextBox.Name = "abbrevTextBox";
            this.abbrevTextBox.Size = new System.Drawing.Size(78, 23);
            this.abbrevTextBox.TabIndex = 3;
            // 
            // abbrevLabel
            // 
            this.abbrevLabel.AutoSize = true;
            this.abbrevLabel.Location = new System.Drawing.Point(40, 51);
            this.abbrevLabel.Name = "abbrevLabel";
            this.abbrevLabel.Size = new System.Drawing.Size(114, 15);
            this.abbrevLabel.TabIndex = 2;
            this.abbrevLabel.Text = "Abbrev (max 5 char)";
            this.abbrevLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(160, 19);
            this.nameTextBox.MaxLength = 255;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(260, 23);
            this.nameTextBox.TabIndex = 1;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(265, 328);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 18;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(381, 328);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "&Cancel";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(59, 22);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(95, 15);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Operatory Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormOperatoryEdit
            // 
            this.ClientSize = new System.Drawing.Size(504, 371);
            this.Controls.Add(this.pickHygienistButton);
            this.Controls.Add(this.pickProviderButton);
            this.Controls.Add(this.pickClinicButton);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.prospectiveLabel);
            this.Controls.Add(this.hygieneLabel);
            this.Controls.Add(this.prospectiveCheckBox);
            this.Controls.Add(this.hygieneCheckBox);
            this.Controls.Add(this.hygienistComboBox);
            this.Controls.Add(this.providerComboBox);
            this.Controls.Add(this.hygienistLabel);
            this.Controls.Add(this.providerLabel);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.abbrevTextBox);
            this.Controls.Add(this.abbrevLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.nameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 410);
            this.Name = "FormOperatoryEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Operatory";
            this.Load += new System.EventHandler(this.FormOperatoryEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label abbrevLabel;
        private System.Windows.Forms.Label hygienistLabel;
        private System.Windows.Forms.Label providerLabel;
        private System.Windows.Forms.Label hygieneLabel;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox abbrevTextBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.ComboBox hygienistComboBox;
        private System.Windows.Forms.ComboBox providerComboBox;
        private System.Windows.Forms.CheckBox hygieneCheckBox;
        private System.Windows.Forms.CheckBox prospectiveCheckBox;
        private System.Windows.Forms.Label prospectiveLabel;
        private System.Windows.Forms.Button pickClinicButton;
        private System.Windows.Forms.Button pickProviderButton;
        private System.Windows.Forms.Button pickHygienistButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label nameLabel;
    }
}
