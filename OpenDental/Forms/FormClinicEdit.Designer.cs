namespace OpenDental
{
    partial class FormClinicEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinicEdit));
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.medLabAccountNumberTextBox = new System.Windows.Forms.TextBox();
            this.medLabAccountNumberLabel = new System.Windows.Forms.Label();
            this.abbrTextBox = new System.Windows.Forms.TextBox();
            this.abbrLabel = new System.Windows.Forms.Label();
            this.excludeFromInsVerifyListCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.PhysicalAddress = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textCity = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textState = new System.Windows.Forms.TextBox();
            this.textAddress2 = new System.Windows.Forms.TextBox();
            this.textZip = new System.Windows.Forms.TextBox();
            this.BillingAddress = new System.Windows.Forms.TabPage();
            this.label25 = new System.Windows.Forms.Label();
            this.useBillingAddressOnClaimsCheckBox = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBillingZip = new System.Windows.Forms.TextBox();
            this.textBillingAddress = new System.Windows.Forms.TextBox();
            this.textBillingST = new System.Windows.Forms.TextBox();
            this.textBillingAddress2 = new System.Windows.Forms.TextBox();
            this.textBillingCity = new System.Windows.Forms.TextBox();
            this.PayToAddress = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textPayToZip = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textPayToST = new System.Windows.Forms.TextBox();
            this.textPayToAddress = new System.Windows.Forms.TextBox();
            this.textPayToCity = new System.Windows.Forms.TextBox();
            this.textPayToAddress2 = new System.Windows.Forms.TextBox();
            this.tabSpecialty = new System.Windows.Forms.TabPage();
            this.removeButton = new System.Windows.Forms.Button();
            this.gridSpecialty = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.regionComboBox = new System.Windows.Forms.ComboBox();
            this.regionLabel = new System.Windows.Forms.Label();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.medicalOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultProviderNoneButton = new System.Windows.Forms.Button();
            this.defaultProviderPickButton = new System.Windows.Forms.Button();
            this.defaultProviderComboBox = new System.Windows.Forms.ComboBox();
            this.defaultProviderLabel = new System.Windows.Forms.Label();
            this.faxTextBox = new OpenDental.ValidPhone();
            this.faxLabel = new System.Windows.Forms.Label();
            this.insBillingProviderDefaultGroupBox = new System.Windows.Forms.GroupBox();
            this.insBillingProviderPickButton = new System.Windows.Forms.Button();
            this.insBillingProviderComboBox = new System.Windows.Forms.ComboBox();
            this.insBillingProviderSpecificRadioButton = new System.Windows.Forms.RadioButton();
            this.insBillingProviderTreatRadioButton = new System.Windows.Forms.RadioButton();
            this.emailLabel = new System.Windows.Forms.Label();
            this.placeOfServiceComboBox = new System.Windows.Forms.ComboBox();
            this.placeOfServiceLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.bankNumberTextBox = new System.Windows.Forms.TextBox();
            this.bankNumberLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new OpenDental.ValidPhone();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.emailPickButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.emailNoneButton = new System.Windows.Forms.Button();
            this.schedulingNoteTextBox = new System.Windows.Forms.TextBox();
            this.schedulingNoteLabel = new System.Windows.Forms.Label();
            this.procedureCodeRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.PhysicalAddress.SuspendLayout();
            this.BillingAddress.SuspendLayout();
            this.PayToAddress.SuspendLayout();
            this.tabSpecialty.SuspendLayout();
            this.insBillingProviderDefaultGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(248, 488);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(76, 19);
            this.hiddenCheckBox.TabIndex = 30;
            this.hiddenCheckBox.Text = "Is Hidden";
            // 
            // medLabAccountNumberTextBox
            // 
            this.medLabAccountNumberTextBox.Location = new System.Drawing.Point(249, 393);
            this.medLabAccountNumberTextBox.MaxLength = 255;
            this.medLabAccountNumberTextBox.Name = "medLabAccountNumberTextBox";
            this.medLabAccountNumberTextBox.Size = new System.Drawing.Size(220, 23);
            this.medLabAccountNumberTextBox.TabIndex = 27;
            this.medLabAccountNumberTextBox.Visible = false;
            // 
            // medLabAccountNumberLabel
            // 
            this.medLabAccountNumberLabel.AutoSize = true;
            this.medLabAccountNumberLabel.Location = new System.Drawing.Point(98, 396);
            this.medLabAccountNumberLabel.Name = "medLabAccountNumberLabel";
            this.medLabAccountNumberLabel.Size = new System.Drawing.Size(145, 15);
            this.medLabAccountNumberLabel.TabIndex = 26;
            this.medLabAccountNumberLabel.Text = "MedLab Account Number";
            this.medLabAccountNumberLabel.Visible = false;
            // 
            // abbrTextBox
            // 
            this.abbrTextBox.Location = new System.Drawing.Point(249, 48);
            this.abbrTextBox.Name = "abbrTextBox";
            this.abbrTextBox.Size = new System.Drawing.Size(160, 23);
            this.abbrTextBox.TabIndex = 1;
            // 
            // abbrLabel
            // 
            this.abbrLabel.AutoSize = true;
            this.abbrLabel.Location = new System.Drawing.Point(168, 51);
            this.abbrLabel.Name = "abbrLabel";
            this.abbrLabel.Size = new System.Drawing.Size(75, 15);
            this.abbrLabel.TabIndex = 0;
            this.abbrLabel.Text = "Abbreviation";
            // 
            // excludeFromInsVerifyListCheckBox
            // 
            this.excludeFromInsVerifyListCheckBox.AutoSize = true;
            this.excludeFromInsVerifyListCheckBox.Location = new System.Drawing.Point(249, 193);
            this.excludeFromInsVerifyListCheckBox.Name = "excludeFromInsVerifyListCheckBox";
            this.excludeFromInsVerifyListCheckBox.Size = new System.Drawing.Size(214, 19);
            this.excludeFromInsVerifyListCheckBox.TabIndex = 12;
            this.excludeFromInsVerifyListCheckBox.Text = "Hide from insurance verification list";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.PhysicalAddress);
            this.tabControl1.Controls.Add(this.BillingAddress);
            this.tabControl1.Controls.Add(this.PayToAddress);
            this.tabControl1.Controls.Add(this.tabSpecialty);
            this.tabControl1.Location = new System.Drawing.Point(651, 19);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 240);
            this.tabControl1.TabIndex = 31;
            this.tabControl1.TabStop = false;
            // 
            // PhysicalAddress
            // 
            this.PhysicalAddress.BackColor = System.Drawing.Color.Transparent;
            this.PhysicalAddress.Controls.Add(this.label24);
            this.PhysicalAddress.Controls.Add(this.textAddress);
            this.PhysicalAddress.Controls.Add(this.label3);
            this.PhysicalAddress.Controls.Add(this.label4);
            this.PhysicalAddress.Controls.Add(this.textCity);
            this.PhysicalAddress.Controls.Add(this.label11);
            this.PhysicalAddress.Controls.Add(this.textState);
            this.PhysicalAddress.Controls.Add(this.textAddress2);
            this.PhysicalAddress.Controls.Add(this.textZip);
            this.PhysicalAddress.Location = new System.Drawing.Point(4, 24);
            this.PhysicalAddress.Name = "PhysicalAddress";
            this.PhysicalAddress.Padding = new System.Windows.Forms.Padding(3);
            this.PhysicalAddress.Size = new System.Drawing.Size(432, 212);
            this.PhysicalAddress.TabIndex = 0;
            this.PhysicalAddress.Text = "Physical Treating Address";
            this.PhysicalAddress.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(48, 140);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(56, 15);
            this.label24.TabIndex = 6;
            this.label24.Text = "State, Zip";
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(110, 50);
            this.textAddress.MaxLength = 255;
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(300, 23);
            this.textAddress.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Address 2";
            // 
            // textCity
            // 
            this.textCity.Location = new System.Drawing.Point(110, 108);
            this.textCity.MaxLength = 255;
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(300, 23);
            this.textCity.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(76, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 15);
            this.label11.TabIndex = 4;
            this.label11.Text = "City";
            // 
            // textState
            // 
            this.textState.Location = new System.Drawing.Point(110, 137);
            this.textState.MaxLength = 255;
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(140, 23);
            this.textState.TabIndex = 7;
            // 
            // textAddress2
            // 
            this.textAddress2.Location = new System.Drawing.Point(110, 79);
            this.textAddress2.MaxLength = 255;
            this.textAddress2.Name = "textAddress2";
            this.textAddress2.Size = new System.Drawing.Size(300, 23);
            this.textAddress2.TabIndex = 3;
            // 
            // textZip
            // 
            this.textZip.Location = new System.Drawing.Point(256, 137);
            this.textZip.MaxLength = 255;
            this.textZip.Name = "textZip";
            this.textZip.Size = new System.Drawing.Size(80, 23);
            this.textZip.TabIndex = 8;
            // 
            // BillingAddress
            // 
            this.BillingAddress.BackColor = System.Drawing.Color.Transparent;
            this.BillingAddress.Controls.Add(this.label25);
            this.BillingAddress.Controls.Add(this.useBillingAddressOnClaimsCheckBox);
            this.BillingAddress.Controls.Add(this.label18);
            this.BillingAddress.Controls.Add(this.label20);
            this.BillingAddress.Controls.Add(this.label16);
            this.BillingAddress.Controls.Add(this.label19);
            this.BillingAddress.Controls.Add(this.textBillingZip);
            this.BillingAddress.Controls.Add(this.textBillingAddress);
            this.BillingAddress.Controls.Add(this.textBillingST);
            this.BillingAddress.Controls.Add(this.textBillingAddress2);
            this.BillingAddress.Controls.Add(this.textBillingCity);
            this.BillingAddress.Location = new System.Drawing.Point(4, 24);
            this.BillingAddress.Name = "BillingAddress";
            this.BillingAddress.Padding = new System.Windows.Forms.Padding(3);
            this.BillingAddress.Size = new System.Drawing.Size(432, 212);
            this.BillingAddress.TabIndex = 1;
            this.BillingAddress.Text = "Billing Address";
            this.BillingAddress.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(48, 140);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(56, 15);
            this.label25.TabIndex = 7;
            this.label25.Text = "State, Zip";
            // 
            // useBillingAddressOnClaimsCheckBox
            // 
            this.useBillingAddressOnClaimsCheckBox.AutoSize = true;
            this.useBillingAddressOnClaimsCheckBox.Location = new System.Drawing.Point(110, 166);
            this.useBillingAddressOnClaimsCheckBox.Name = "useBillingAddressOnClaimsCheckBox";
            this.useBillingAddressOnClaimsCheckBox.Size = new System.Drawing.Size(101, 19);
            this.useBillingAddressOnClaimsCheckBox.TabIndex = 10;
            this.useBillingAddressOnClaimsCheckBox.Text = "Use on Claims";
            this.useBillingAddressOnClaimsCheckBox.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Location = new System.Drawing.Point(6, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(420, 35);
            this.label18.TabIndex = 0;
            this.label18.Text = "Optional, for E-Claims.  Cannot be a PO Box.";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(76, 111);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(28, 15);
            this.label20.TabIndex = 5;
            this.label20.Text = "City";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(46, 82);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 15);
            this.label16.TabIndex = 3;
            this.label16.Text = "Address 2";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(55, 53);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(49, 15);
            this.label19.TabIndex = 1;
            this.label19.Text = "Address";
            // 
            // textBillingZip
            // 
            this.textBillingZip.Location = new System.Drawing.Point(256, 137);
            this.textBillingZip.Name = "textBillingZip";
            this.textBillingZip.Size = new System.Drawing.Size(80, 23);
            this.textBillingZip.TabIndex = 9;
            // 
            // textBillingAddress
            // 
            this.textBillingAddress.Location = new System.Drawing.Point(110, 50);
            this.textBillingAddress.Name = "textBillingAddress";
            this.textBillingAddress.Size = new System.Drawing.Size(300, 23);
            this.textBillingAddress.TabIndex = 2;
            // 
            // textBillingST
            // 
            this.textBillingST.Location = new System.Drawing.Point(110, 137);
            this.textBillingST.Name = "textBillingST";
            this.textBillingST.Size = new System.Drawing.Size(140, 23);
            this.textBillingST.TabIndex = 8;
            // 
            // textBillingAddress2
            // 
            this.textBillingAddress2.Location = new System.Drawing.Point(110, 79);
            this.textBillingAddress2.Name = "textBillingAddress2";
            this.textBillingAddress2.Size = new System.Drawing.Size(300, 23);
            this.textBillingAddress2.TabIndex = 4;
            // 
            // textBillingCity
            // 
            this.textBillingCity.Location = new System.Drawing.Point(110, 108);
            this.textBillingCity.Name = "textBillingCity";
            this.textBillingCity.Size = new System.Drawing.Size(300, 23);
            this.textBillingCity.TabIndex = 6;
            // 
            // PayToAddress
            // 
            this.PayToAddress.BackColor = System.Drawing.Color.Transparent;
            this.PayToAddress.Controls.Add(this.label21);
            this.PayToAddress.Controls.Add(this.label17);
            this.PayToAddress.Controls.Add(this.label13);
            this.PayToAddress.Controls.Add(this.label15);
            this.PayToAddress.Controls.Add(this.textPayToZip);
            this.PayToAddress.Controls.Add(this.label14);
            this.PayToAddress.Controls.Add(this.textPayToST);
            this.PayToAddress.Controls.Add(this.textPayToAddress);
            this.PayToAddress.Controls.Add(this.textPayToCity);
            this.PayToAddress.Controls.Add(this.textPayToAddress2);
            this.PayToAddress.Location = new System.Drawing.Point(4, 24);
            this.PayToAddress.Name = "PayToAddress";
            this.PayToAddress.Size = new System.Drawing.Size(432, 212);
            this.PayToAddress.TabIndex = 2;
            this.PayToAddress.Text = "Pay To Address";
            this.PayToAddress.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(48, 140);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 15);
            this.label21.TabIndex = 7;
            this.label21.Text = "State, Zip";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(6, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(420, 35);
            this.label17.TabIndex = 0;
            this.label17.Text = "Optional for claims.  Can be a PO Box.  \r\nSent in addition to treating or billing" +
    " address.";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(46, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 15);
            this.label13.TabIndex = 3;
            this.label13.Text = "Address 2";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(76, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 15);
            this.label15.TabIndex = 5;
            this.label15.Text = "City";
            // 
            // textPayToZip
            // 
            this.textPayToZip.Location = new System.Drawing.Point(256, 137);
            this.textPayToZip.Name = "textPayToZip";
            this.textPayToZip.Size = new System.Drawing.Size(80, 23);
            this.textPayToZip.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(55, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 15);
            this.label14.TabIndex = 1;
            this.label14.Text = "Address";
            // 
            // textPayToST
            // 
            this.textPayToST.Location = new System.Drawing.Point(110, 137);
            this.textPayToST.Name = "textPayToST";
            this.textPayToST.Size = new System.Drawing.Size(140, 23);
            this.textPayToST.TabIndex = 8;
            // 
            // textPayToAddress
            // 
            this.textPayToAddress.Location = new System.Drawing.Point(110, 50);
            this.textPayToAddress.Name = "textPayToAddress";
            this.textPayToAddress.Size = new System.Drawing.Size(300, 23);
            this.textPayToAddress.TabIndex = 2;
            // 
            // textPayToCity
            // 
            this.textPayToCity.Location = new System.Drawing.Point(110, 108);
            this.textPayToCity.Name = "textPayToCity";
            this.textPayToCity.Size = new System.Drawing.Size(300, 23);
            this.textPayToCity.TabIndex = 6;
            // 
            // textPayToAddress2
            // 
            this.textPayToAddress2.Location = new System.Drawing.Point(110, 79);
            this.textPayToAddress2.Name = "textPayToAddress2";
            this.textPayToAddress2.Size = new System.Drawing.Size(300, 23);
            this.textPayToAddress2.TabIndex = 4;
            // 
            // tabSpecialty
            // 
            this.tabSpecialty.Controls.Add(this.removeButton);
            this.tabSpecialty.Controls.Add(this.gridSpecialty);
            this.tabSpecialty.Controls.Add(this.addButton);
            this.tabSpecialty.Location = new System.Drawing.Point(4, 24);
            this.tabSpecialty.Name = "tabSpecialty";
            this.tabSpecialty.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpecialty.Size = new System.Drawing.Size(432, 212);
            this.tabSpecialty.TabIndex = 3;
            this.tabSpecialty.Text = "Specialty";
            this.tabSpecialty.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(356, 42);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(70, 30);
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // gridSpecialty
            // 
            this.gridSpecialty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSpecialty.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridSpecialty.EditableEnterMovesDown = false;
            this.gridSpecialty.HasAddButton = false;
            this.gridSpecialty.HasDropDowns = false;
            this.gridSpecialty.HasMultilineHeaders = false;
            this.gridSpecialty.HScrollVisible = false;
            this.gridSpecialty.Location = new System.Drawing.Point(6, 6);
            this.gridSpecialty.Name = "gridSpecialty";
            this.gridSpecialty.ScrollValue = 0;
            this.gridSpecialty.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.gridSpecialty.Size = new System.Drawing.Size(344, 200);
            this.gridSpecialty.TabIndex = 0;
            this.gridSpecialty.Title = "Clinic Specialty";
            this.gridSpecialty.TitleVisible = true;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(356, 6);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(70, 30);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // regionComboBox
            // 
            this.regionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(249, 164);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(160, 23);
            this.regionComboBox.TabIndex = 11;
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(199, 167);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(44, 15);
            this.regionLabel.TabIndex = 10;
            this.regionLabel.Text = "Region";
            // 
            // idTextBox
            // 
            this.idTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.idTextBox.Location = new System.Drawing.Point(249, 19);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.ReadOnly = true;
            this.idTextBox.Size = new System.Drawing.Size(60, 23);
            this.idTextBox.TabIndex = 36;
            this.idTextBox.TabStop = false;
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(225, 22);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(18, 15);
            this.idLabel.TabIndex = 35;
            this.idLabel.Text = "ID";
            // 
            // medicalOnlyCheckBox
            // 
            this.medicalOnlyCheckBox.AutoSize = true;
            this.medicalOnlyCheckBox.Location = new System.Drawing.Point(315, 21);
            this.medicalOnlyCheckBox.Name = "medicalOnlyCheckBox";
            this.medicalOnlyCheckBox.Size = new System.Drawing.Size(112, 19);
            this.medicalOnlyCheckBox.TabIndex = 37;
            this.medicalOnlyCheckBox.Text = "Is Medical Clinic";
            // 
            // defaultProviderNoneButton
            // 
            this.defaultProviderNoneButton.Enabled = false;
            this.defaultProviderNoneButton.Location = new System.Drawing.Point(500, 334);
            this.defaultProviderNoneButton.Name = "defaultProviderNoneButton";
            this.defaultProviderNoneButton.Size = new System.Drawing.Size(50, 25);
            this.defaultProviderNoneButton.TabIndex = 23;
            this.defaultProviderNoneButton.Text = "None";
            this.defaultProviderNoneButton.Click += new System.EventHandler(this.DefaultProviderNoneButton_Click);
            // 
            // defaultProviderPickButton
            // 
            this.defaultProviderPickButton.Location = new System.Drawing.Point(470, 334);
            this.defaultProviderPickButton.Name = "defaultProviderPickButton";
            this.defaultProviderPickButton.Size = new System.Drawing.Size(30, 25);
            this.defaultProviderPickButton.TabIndex = 22;
            this.defaultProviderPickButton.Text = "...";
            this.defaultProviderPickButton.Click += new System.EventHandler(this.DefaultProviderPickButton_Click);
            // 
            // defaultProviderComboBox
            // 
            this.defaultProviderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultProviderComboBox.Location = new System.Drawing.Point(249, 335);
            this.defaultProviderComboBox.Name = "defaultProviderComboBox";
            this.defaultProviderComboBox.Size = new System.Drawing.Size(220, 23);
            this.defaultProviderComboBox.TabIndex = 21;
            this.defaultProviderComboBox.SelectedIndexChanged += new System.EventHandler(this.DefaultProviderComboBox_SelectedIndexChanged);
            // 
            // defaultProviderLabel
            // 
            this.defaultProviderLabel.AutoSize = true;
            this.defaultProviderLabel.Location = new System.Drawing.Point(151, 339);
            this.defaultProviderLabel.Name = "defaultProviderLabel";
            this.defaultProviderLabel.Size = new System.Drawing.Size(92, 15);
            this.defaultProviderLabel.TabIndex = 20;
            this.defaultProviderLabel.Text = "Default Provider";
            // 
            // faxTextBox
            // 
            this.faxTextBox.Location = new System.Drawing.Point(249, 135);
            this.faxTextBox.MaxLength = 255;
            this.faxTextBox.Name = "faxTextBox";
            this.faxTextBox.Size = new System.Drawing.Size(160, 23);
            this.faxTextBox.TabIndex = 8;
            // 
            // faxLabel
            // 
            this.faxLabel.AutoSize = true;
            this.faxLabel.Location = new System.Drawing.Point(218, 138);
            this.faxLabel.Name = "faxLabel";
            this.faxLabel.Size = new System.Drawing.Size(25, 15);
            this.faxLabel.TabIndex = 7;
            this.faxLabel.Text = "Fax";
            // 
            // insBillingProviderDefaultGroupBox
            // 
            this.insBillingProviderDefaultGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.insBillingProviderDefaultGroupBox.Controls.Add(this.insBillingProviderPickButton);
            this.insBillingProviderDefaultGroupBox.Controls.Add(this.insBillingProviderComboBox);
            this.insBillingProviderDefaultGroupBox.Controls.Add(this.insBillingProviderSpecificRadioButton);
            this.insBillingProviderDefaultGroupBox.Controls.Add(this.insBillingProviderTreatRadioButton);
            this.insBillingProviderDefaultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.insBillingProviderDefaultGroupBox.Location = new System.Drawing.Point(651, 282);
            this.insBillingProviderDefaultGroupBox.Name = "insBillingProviderDefaultGroupBox";
            this.insBillingProviderDefaultGroupBox.Size = new System.Drawing.Size(440, 110);
            this.insBillingProviderDefaultGroupBox.TabIndex = 32;
            this.insBillingProviderDefaultGroupBox.TabStop = false;
            this.insBillingProviderDefaultGroupBox.Text = "Default Insurance Billing Provider";
            // 
            // insBillingProviderPickButton
            // 
            this.insBillingProviderPickButton.Enabled = false;
            this.insBillingProviderPickButton.Location = new System.Drawing.Point(237, 57);
            this.insBillingProviderPickButton.Name = "insBillingProviderPickButton";
            this.insBillingProviderPickButton.Size = new System.Drawing.Size(30, 25);
            this.insBillingProviderPickButton.TabIndex = 4;
            this.insBillingProviderPickButton.Text = "...";
            this.insBillingProviderPickButton.Click += new System.EventHandler(this.InsBillingProviderPickButton_Click);
            // 
            // insBillingProviderComboBox
            // 
            this.insBillingProviderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.insBillingProviderComboBox.Enabled = false;
            this.insBillingProviderComboBox.Location = new System.Drawing.Point(16, 58);
            this.insBillingProviderComboBox.Name = "insBillingProviderComboBox";
            this.insBillingProviderComboBox.Size = new System.Drawing.Size(220, 23);
            this.insBillingProviderComboBox.TabIndex = 3;
            // 
            // insBillingProviderSpecificRadioButton
            // 
            this.insBillingProviderSpecificRadioButton.AutoSize = true;
            this.insBillingProviderSpecificRadioButton.Location = new System.Drawing.Point(136, 33);
            this.insBillingProviderSpecificRadioButton.Name = "insBillingProviderSpecificRadioButton";
            this.insBillingProviderSpecificRadioButton.Size = new System.Drawing.Size(116, 19);
            this.insBillingProviderSpecificRadioButton.TabIndex = 2;
            this.insBillingProviderSpecificRadioButton.Text = "Selected Provider";
            this.insBillingProviderSpecificRadioButton.CheckedChanged += new System.EventHandler(this.InsBillingProviderSpecificRadioButton_CheckedChanged);
            // 
            // insBillingProviderTreatRadioButton
            // 
            this.insBillingProviderTreatRadioButton.AutoSize = true;
            this.insBillingProviderTreatRadioButton.Checked = true;
            this.insBillingProviderTreatRadioButton.Location = new System.Drawing.Point(16, 33);
            this.insBillingProviderTreatRadioButton.Name = "insBillingProviderTreatRadioButton";
            this.insBillingProviderTreatRadioButton.Size = new System.Drawing.Size(114, 19);
            this.insBillingProviderTreatRadioButton.TabIndex = 1;
            this.insBillingProviderTreatRadioButton.TabStop = true;
            this.insBillingProviderTreatRadioButton.Text = "Treating Provider";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(162, 263);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(81, 15);
            this.emailLabel.TabIndex = 14;
            this.emailLabel.Text = "Email Address";
            // 
            // placeOfServiceComboBox
            // 
            this.placeOfServiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.placeOfServiceComboBox.Location = new System.Drawing.Point(249, 364);
            this.placeOfServiceComboBox.MaxDropDownItems = 30;
            this.placeOfServiceComboBox.Name = "placeOfServiceComboBox";
            this.placeOfServiceComboBox.Size = new System.Drawing.Size(220, 23);
            this.placeOfServiceComboBox.TabIndex = 25;
            // 
            // placeOfServiceLabel
            // 
            this.placeOfServiceLabel.AutoSize = true;
            this.placeOfServiceLabel.Location = new System.Drawing.Point(56, 367);
            this.placeOfServiceLabel.Name = "placeOfServiceLabel";
            this.placeOfServiceLabel.Size = new System.Drawing.Size(187, 15);
            this.placeOfServiceLabel.TabIndex = 24;
            this.placeOfServiceLabel.Text = "Default Procedure Place of Service";
            // 
            // emailTextBox
            // 
            this.emailTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.emailTextBox.Location = new System.Drawing.Point(249, 260);
            this.emailTextBox.MaxLength = 255;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.ReadOnly = true;
            this.emailTextBox.Size = new System.Drawing.Size(300, 23);
            this.emailTextBox.TabIndex = 15;
            // 
            // bankNumberTextBox
            // 
            this.bankNumberTextBox.Location = new System.Drawing.Point(249, 289);
            this.bankNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.bankNumberTextBox.MaxLength = 255;
            this.bankNumberTextBox.Name = "bankNumberTextBox";
            this.bankNumberTextBox.Size = new System.Drawing.Size(300, 23);
            this.bankNumberTextBox.TabIndex = 19;
            // 
            // bankNumberLabel
            // 
            this.bankNumberLabel.AutoSize = true;
            this.bankNumberLabel.Location = new System.Drawing.Point(115, 292);
            this.bankNumberLabel.Name = "bankNumberLabel";
            this.bankNumberLabel.Size = new System.Drawing.Size(128, 15);
            this.bankNumberLabel.TabIndex = 18;
            this.bankNumberLabel.Text = "Bank Account Number";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(249, 106);
            this.phoneTextBox.MaxLength = 255;
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(160, 23);
            this.phoneTextBox.TabIndex = 5;
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(202, 109);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(41, 15);
            this.phoneLabel.TabIndex = 4;
            this.phoneLabel.Text = "Phone";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(249, 77);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(300, 23);
            this.descriptionTextBox.TabIndex = 3;
            // 
            // emailPickButton
            // 
            this.emailPickButton.Location = new System.Drawing.Point(550, 259);
            this.emailPickButton.Name = "emailPickButton";
            this.emailPickButton.Size = new System.Drawing.Size(30, 25);
            this.emailPickButton.TabIndex = 16;
            this.emailPickButton.Text = "...";
            this.emailPickButton.Click += new System.EventHandler(this.EmailPickButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(865, 538);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 33;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(981, 538);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 34;
            this.cancelButton.Text = "&Cancel";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(176, 80);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description";
            // 
            // emailNoneButton
            // 
            this.emailNoneButton.Enabled = false;
            this.emailNoneButton.Location = new System.Drawing.Point(580, 259);
            this.emailNoneButton.Name = "emailNoneButton";
            this.emailNoneButton.Size = new System.Drawing.Size(50, 25);
            this.emailNoneButton.TabIndex = 17;
            this.emailNoneButton.Text = "None";
            this.emailNoneButton.UseVisualStyleBackColor = true;
            this.emailNoneButton.Click += new System.EventHandler(this.EmailNoneButton_Click);
            // 
            // schedulingNoteTextBox
            // 
            this.schedulingNoteTextBox.Location = new System.Drawing.Point(249, 422);
            this.schedulingNoteTextBox.MaxLength = 255;
            this.schedulingNoteTextBox.Multiline = true;
            this.schedulingNoteTextBox.Name = "schedulingNoteTextBox";
            this.schedulingNoteTextBox.Size = new System.Drawing.Size(300, 60);
            this.schedulingNoteTextBox.TabIndex = 29;
            // 
            // schedulingNoteLabel
            // 
            this.schedulingNoteLabel.AutoSize = true;
            this.schedulingNoteLabel.Location = new System.Drawing.Point(148, 425);
            this.schedulingNoteLabel.Name = "schedulingNoteLabel";
            this.schedulingNoteLabel.Size = new System.Drawing.Size(95, 15);
            this.schedulingNoteLabel.TabIndex = 28;
            this.schedulingNoteLabel.Text = "Scheduling Note";
            // 
            // procedureCodeRequiredCheckBox
            // 
            this.procedureCodeRequiredCheckBox.AutoSize = true;
            this.procedureCodeRequiredCheckBox.Enabled = false;
            this.procedureCodeRequiredCheckBox.Location = new System.Drawing.Point(249, 218);
            this.procedureCodeRequiredCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.procedureCodeRequiredCheckBox.Name = "procedureCodeRequiredCheckBox";
            this.procedureCodeRequiredCheckBox.Size = new System.Drawing.Size(271, 19);
            this.procedureCodeRequiredCheckBox.TabIndex = 13;
            this.procedureCodeRequiredCheckBox.Text = "Procedure code required on Rx from this clinic";
            // 
            // FormClinicEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1104, 581);
            this.Controls.Add(this.procedureCodeRequiredCheckBox);
            this.Controls.Add(this.schedulingNoteTextBox);
            this.Controls.Add(this.schedulingNoteLabel);
            this.Controls.Add(this.emailNoneButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.medLabAccountNumberTextBox);
            this.Controls.Add(this.medLabAccountNumberLabel);
            this.Controls.Add(this.abbrTextBox);
            this.Controls.Add(this.abbrLabel);
            this.Controls.Add(this.excludeFromInsVerifyListCheckBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.regionComboBox);
            this.Controls.Add(this.regionLabel);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.medicalOnlyCheckBox);
            this.Controls.Add(this.defaultProviderNoneButton);
            this.Controls.Add(this.defaultProviderPickButton);
            this.Controls.Add(this.defaultProviderComboBox);
            this.Controls.Add(this.defaultProviderLabel);
            this.Controls.Add(this.faxTextBox);
            this.Controls.Add(this.faxLabel);
            this.Controls.Add(this.insBillingProviderDefaultGroupBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.placeOfServiceComboBox);
            this.Controls.Add(this.placeOfServiceLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.bankNumberTextBox);
            this.Controls.Add(this.bankNumberLabel);
            this.Controls.Add(this.phoneTextBox);
            this.Controls.Add(this.phoneLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.emailPickButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.descriptionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormClinicEdit";
            this.ShowInTaskbar = false;
            this.Text = "Edit Clinic";
            this.Load += new System.EventHandler(this.FormClinicEdit_Load);
            this.tabControl1.ResumeLayout(false);
            this.PhysicalAddress.ResumeLayout(false);
            this.PhysicalAddress.PerformLayout();
            this.BillingAddress.ResumeLayout(false);
            this.BillingAddress.PerformLayout();
            this.PayToAddress.ResumeLayout(false);
            this.PayToAddress.PerformLayout();
            this.tabSpecialty.ResumeLayout(false);
            this.insBillingProviderDefaultGroupBox.ResumeLayout(false);
            this.insBillingProviderDefaultGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private ValidPhone phoneTextBox;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.TextBox bankNumberTextBox;
        private System.Windows.Forms.Label bankNumberLabel;
        private System.Windows.Forms.Label placeOfServiceLabel;
        private System.Windows.Forms.ComboBox placeOfServiceComboBox;
        private System.Windows.Forms.GroupBox insBillingProviderDefaultGroupBox;
        private System.Windows.Forms.ComboBox insBillingProviderComboBox;
        private System.Windows.Forms.RadioButton insBillingProviderSpecificRadioButton;
        private System.Windows.Forms.RadioButton insBillingProviderTreatRadioButton;
        private ValidPhone faxTextBox;
        private System.Windows.Forms.Label faxLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label defaultProviderLabel;
        private System.Windows.Forms.ComboBox defaultProviderComboBox;
        private System.Windows.Forms.Button defaultProviderPickButton;
        private System.Windows.Forms.Button emailPickButton;
        private System.Windows.Forms.Button insBillingProviderPickButton;
        private System.Windows.Forms.Button defaultProviderNoneButton;
        private System.Windows.Forms.CheckBox medicalOnlyCheckBox;
        private System.Windows.Forms.TextBox textCity;
        private System.Windows.Forms.TextBox textState;
        private System.Windows.Forms.TextBox textZip;
        private System.Windows.Forms.TextBox textAddress2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textPayToZip;
        private System.Windows.Forms.TextBox textPayToST;
        private System.Windows.Forms.TextBox textPayToCity;
        private System.Windows.Forms.TextBox textPayToAddress2;
        private System.Windows.Forms.TextBox textPayToAddress;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBillingZip;
        private System.Windows.Forms.TextBox textBillingST;
        private System.Windows.Forms.TextBox textBillingCity;
        private System.Windows.Forms.TextBox textBillingAddress2;
        private System.Windows.Forms.TextBox textBillingAddress;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.CheckBox useBillingAddressOnClaimsCheckBox;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.ComboBox regionComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage PhysicalAddress;
        private System.Windows.Forms.TabPage BillingAddress;
        private System.Windows.Forms.TabPage PayToAddress;
        private System.Windows.Forms.CheckBox excludeFromInsVerifyListCheckBox;
        private System.Windows.Forms.TextBox abbrTextBox;
        private System.Windows.Forms.Label abbrLabel;
        private System.Windows.Forms.TextBox medLabAccountNumberTextBox;
        private System.Windows.Forms.Label medLabAccountNumberLabel;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.Button emailNoneButton;
        private System.Windows.Forms.TabPage tabSpecialty;
        private System.Windows.Forms.Button addButton;
        private UI.ODGrid gridSpecialty;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TextBox schedulingNoteTextBox;
        private System.Windows.Forms.Label schedulingNoteLabel;
        private System.Windows.Forms.CheckBox procedureCodeRequiredCheckBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label21;
    }
}
