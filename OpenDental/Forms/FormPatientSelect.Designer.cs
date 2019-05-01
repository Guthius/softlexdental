namespace OpenDental
{
    partial class FormPatientSelect
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientSelect));
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.addManyButton = new System.Windows.Forms.Button();
            this.addPatientButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.checkRefresh = new System.Windows.Forms.CheckBox();
            this.butGetAll = new System.Windows.Forms.Button();
            this.invoiceNumberTextBox = new System.Windows.Forms.TextBox();
            this.invoiceNumberLabel = new System.Windows.Forms.Label();
            this.butSearch = new System.Windows.Forms.Button();
            this.checkShowMerged = new System.Windows.Forms.CheckBox();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.countryLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.subscriberIdTextBox = new System.Windows.Forms.TextBox();
            this.subscriberIdLabel = new System.Windows.Forms.Label();
            this.siteComboBox = new System.Windows.Forms.ComboBox();
            this.siteLabel = new System.Windows.Forms.Label();
            this.billingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.birthdateTextBox = new System.Windows.Forms.TextBox();
            this.birthdateLabel = new System.Windows.Forms.Label();
            this.checkShowArchived = new System.Windows.Forms.CheckBox();
            this.textChartNumber = new System.Windows.Forms.TextBox();
            this.ssnTextBox = new System.Windows.Forms.TextBox();
            this.ssnLabel = new System.Windows.Forms.Label();
            this.billingTypeLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.patNumTextBox = new System.Windows.Forms.TextBox();
            this.patNumLabel = new System.Windows.Forms.Label();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.stateLabel = new System.Windows.Forms.Label();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.cityLabel = new System.Windows.Forms.Label();
            this.checkGuarantors = new System.Windows.Forms.CheckBox();
            this.checkHideInactive = new System.Windows.Forms.CheckBox();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new OpenDental.ValidPhone();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.gridMain = new OpenDental.UI.ODGrid();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lastNameTextBox.Location = new System.Drawing.Point(148, 19);
            this.lastNameTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(226, 23);
            this.lastNameTextBox.TabIndex = 1;
            this.lastNameTextBox.TextChanged += new System.EventHandler(this.textLName_TextChanged);
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(79, 22);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(63, 15);
            this.lastNameLabel.TabIndex = 0;
            this.lastNameLabel.Text = "Last Name";
            this.lastNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // addManyButton
            // 
            this.addManyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addManyButton.Location = new System.Drawing.Point(179, 614);
            this.addManyButton.Name = "addManyButton";
            this.addManyButton.Size = new System.Drawing.Size(160, 30);
            this.addManyButton.TabIndex = 3;
            this.addManyButton.Text = "Add Many";
            this.addManyButton.Click += new System.EventHandler(this.addManyButton_Click);
            // 
            // addPatientButton
            // 
            this.addPatientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addPatientButton.Location = new System.Drawing.Point(13, 614);
            this.addPatientButton.Name = "addPatientButton";
            this.addPatientButton.Size = new System.Drawing.Size(160, 30);
            this.addPatientButton.TabIndex = 2;
            this.addPatientButton.Text = "&Add Patient";
            this.addPatientButton.Click += new System.EventHandler(this.addPatientButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(846, 619);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(962, 619);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(110, 30);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "&Cancel";
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.checkRefresh);
            this.searchGroupBox.Controls.Add(this.butGetAll);
            this.searchGroupBox.Controls.Add(this.invoiceNumberTextBox);
            this.searchGroupBox.Controls.Add(this.invoiceNumberLabel);
            this.searchGroupBox.Controls.Add(this.butSearch);
            this.searchGroupBox.Controls.Add(this.checkShowMerged);
            this.searchGroupBox.Controls.Add(this.clinicComboBox);
            this.searchGroupBox.Controls.Add(this.clinicLabel);
            this.searchGroupBox.Controls.Add(this.countryTextBox);
            this.searchGroupBox.Controls.Add(this.countryLabel);
            this.searchGroupBox.Controls.Add(this.emailTextBox);
            this.searchGroupBox.Controls.Add(this.emailLabel);
            this.searchGroupBox.Controls.Add(this.subscriberIdTextBox);
            this.searchGroupBox.Controls.Add(this.subscriberIdLabel);
            this.searchGroupBox.Controls.Add(this.siteComboBox);
            this.searchGroupBox.Controls.Add(this.siteLabel);
            this.searchGroupBox.Controls.Add(this.billingTypeComboBox);
            this.searchGroupBox.Controls.Add(this.birthdateTextBox);
            this.searchGroupBox.Controls.Add(this.birthdateLabel);
            this.searchGroupBox.Controls.Add(this.checkShowArchived);
            this.searchGroupBox.Controls.Add(this.textChartNumber);
            this.searchGroupBox.Controls.Add(this.ssnTextBox);
            this.searchGroupBox.Controls.Add(this.ssnLabel);
            this.searchGroupBox.Controls.Add(this.billingTypeLabel);
            this.searchGroupBox.Controls.Add(this.label10);
            this.searchGroupBox.Controls.Add(this.patNumTextBox);
            this.searchGroupBox.Controls.Add(this.patNumLabel);
            this.searchGroupBox.Controls.Add(this.stateTextBox);
            this.searchGroupBox.Controls.Add(this.stateLabel);
            this.searchGroupBox.Controls.Add(this.cityTextBox);
            this.searchGroupBox.Controls.Add(this.cityLabel);
            this.searchGroupBox.Controls.Add(this.checkGuarantors);
            this.searchGroupBox.Controls.Add(this.checkHideInactive);
            this.searchGroupBox.Controls.Add(this.addressTextBox);
            this.searchGroupBox.Controls.Add(this.addressLabel);
            this.searchGroupBox.Controls.Add(this.phoneTextBox);
            this.searchGroupBox.Controls.Add(this.phoneLabel);
            this.searchGroupBox.Controls.Add(this.firstNameTextBox);
            this.searchGroupBox.Controls.Add(this.firstNameLabel);
            this.searchGroupBox.Controls.Add(this.lastNameTextBox);
            this.searchGroupBox.Controls.Add(this.lastNameLabel);
            this.searchGroupBox.Location = new System.Drawing.Point(692, 19);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(380, 570);
            this.searchGroupBox.TabIndex = 1;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search Criteria";
            // 
            // checkRefresh
            // 
            this.checkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkRefresh.AutoSize = true;
            this.checkRefresh.Location = new System.Drawing.Point(148, 545);
            this.checkRefresh.Name = "checkRefresh";
            this.checkRefresh.Size = new System.Drawing.Size(133, 19);
            this.checkRefresh.TabIndex = 42;
            this.checkRefresh.Text = "Refresh while typing";
            this.checkRefresh.UseVisualStyleBackColor = true;
            this.checkRefresh.Click += new System.EventHandler(this.checkRefresh_Click);
            // 
            // butGetAll
            // 
            this.butGetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butGetAll.Location = new System.Drawing.Point(264, 509);
            this.butGetAll.Name = "butGetAll";
            this.butGetAll.Size = new System.Drawing.Size(110, 30);
            this.butGetAll.TabIndex = 41;
            this.butGetAll.Text = "Get All";
            this.butGetAll.Click += new System.EventHandler(this.butGetAll_Click);
            // 
            // invoiceNumberTextBox
            // 
            this.invoiceNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.invoiceNumberTextBox.Location = new System.Drawing.Point(148, 283);
            this.invoiceNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.invoiceNumberTextBox.Name = "invoiceNumberTextBox";
            this.invoiceNumberTextBox.Size = new System.Drawing.Size(226, 23);
            this.invoiceNumberTextBox.TabIndex = 25;
            this.invoiceNumberTextBox.TextChanged += new System.EventHandler(this.textInvoiceNumber_TextChanged);
            // 
            // invoiceNumberLabel
            // 
            this.invoiceNumberLabel.AutoSize = true;
            this.invoiceNumberLabel.Location = new System.Drawing.Point(50, 286);
            this.invoiceNumberLabel.Name = "invoiceNumberLabel";
            this.invoiceNumberLabel.Size = new System.Drawing.Size(92, 15);
            this.invoiceNumberLabel.TabIndex = 24;
            this.invoiceNumberLabel.Text = "Invoice Number";
            this.invoiceNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butSearch
            // 
            this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearch.Location = new System.Drawing.Point(148, 509);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(110, 30);
            this.butSearch.TabIndex = 40;
            this.butSearch.Text = "&Search";
            this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
            // 
            // checkShowMerged
            // 
            this.checkShowMerged.AutoSize = true;
            this.checkShowMerged.Location = new System.Drawing.Point(148, 464);
            this.checkShowMerged.Name = "checkShowMerged";
            this.checkShowMerged.Size = new System.Drawing.Size(144, 19);
            this.checkShowMerged.TabIndex = 39;
            this.checkShowMerged.Text = "Show Merged Patients";
            this.checkShowMerged.CheckedChanged += new System.EventHandler(this.checkShowMerged_CheckedChanged);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(148, 371);
            this.clinicComboBox.MaxDropDownItems = 40;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(226, 23);
            this.clinicComboBox.TabIndex = 35;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(105, 374);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 34;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // countryTextBox
            // 
            this.countryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.countryTextBox.Location = new System.Drawing.Point(148, 305);
            this.countryTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.countryTextBox.Name = "countryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(226, 23);
            this.countryTextBox.TabIndex = 27;
            this.countryTextBox.TextChanged += new System.EventHandler(this.textCountry_TextChanged);
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(92, 308);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(50, 15);
            this.countryLabel.TabIndex = 26;
            this.countryLabel.Text = "Country";
            this.countryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailTextBox.Location = new System.Drawing.Point(148, 261);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(226, 23);
            this.emailTextBox.TabIndex = 23;
            this.emailTextBox.TextChanged += new System.EventHandler(this.textEmail_TextChanged);
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(101, 264);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(41, 15);
            this.emailLabel.TabIndex = 22;
            this.emailLabel.Text = "E-mail";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // subscriberIdTextBox
            // 
            this.subscriberIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subscriberIdTextBox.Location = new System.Drawing.Point(148, 239);
            this.subscriberIdTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.subscriberIdTextBox.Name = "subscriberIdTextBox";
            this.subscriberIdTextBox.Size = new System.Drawing.Size(226, 23);
            this.subscriberIdTextBox.TabIndex = 21;
            this.subscriberIdTextBox.TextChanged += new System.EventHandler(this.textSubscriberID_TextChanged);
            // 
            // subscriberIdLabel
            // 
            this.subscriberIdLabel.AutoSize = true;
            this.subscriberIdLabel.Location = new System.Drawing.Point(66, 242);
            this.subscriberIdLabel.Name = "subscriberIdLabel";
            this.subscriberIdLabel.Size = new System.Drawing.Size(76, 15);
            this.subscriberIdLabel.TabIndex = 20;
            this.subscriberIdLabel.Text = "Subscriber ID";
            this.subscriberIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // siteComboBox
            // 
            this.siteComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.siteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.siteComboBox.Location = new System.Drawing.Point(148, 349);
            this.siteComboBox.MaxDropDownItems = 40;
            this.siteComboBox.Name = "siteComboBox";
            this.siteComboBox.Size = new System.Drawing.Size(226, 23);
            this.siteComboBox.TabIndex = 33;
            this.siteComboBox.SelectionChangeCommitted += new System.EventHandler(this.comboSite_SelectionChangeCommitted);
            // 
            // siteLabel
            // 
            this.siteLabel.AutoSize = true;
            this.siteLabel.Location = new System.Drawing.Point(116, 352);
            this.siteLabel.Name = "siteLabel";
            this.siteLabel.Size = new System.Drawing.Size(26, 15);
            this.siteLabel.TabIndex = 32;
            this.siteLabel.Text = "Site";
            this.siteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // billingTypeComboBox
            // 
            this.billingTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.billingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.billingTypeComboBox.FormattingEnabled = true;
            this.billingTypeComboBox.Location = new System.Drawing.Point(148, 327);
            this.billingTypeComboBox.Name = "billingTypeComboBox";
            this.billingTypeComboBox.Size = new System.Drawing.Size(226, 23);
            this.billingTypeComboBox.TabIndex = 31;
            this.billingTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.comboBillingType_SelectionChangeCommitted);
            // 
            // birthdateTextBox
            // 
            this.birthdateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.birthdateTextBox.Location = new System.Drawing.Point(148, 217);
            this.birthdateTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.birthdateTextBox.Name = "birthdateTextBox";
            this.birthdateTextBox.Size = new System.Drawing.Size(226, 23);
            this.birthdateTextBox.TabIndex = 19;
            this.birthdateTextBox.TextChanged += new System.EventHandler(this.textBirthdate_TextChanged);
            // 
            // birthdateLabel
            // 
            this.birthdateLabel.AutoSize = true;
            this.birthdateLabel.Location = new System.Drawing.Point(87, 220);
            this.birthdateLabel.Name = "birthdateLabel";
            this.birthdateLabel.Size = new System.Drawing.Size(55, 15);
            this.birthdateLabel.TabIndex = 18;
            this.birthdateLabel.Text = "Birthdate";
            this.birthdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkShowArchived
            // 
            this.checkShowArchived.AutoSize = true;
            this.checkShowArchived.Location = new System.Drawing.Point(148, 442);
            this.checkShowArchived.Name = "checkShowArchived";
            this.checkShowArchived.Size = new System.Drawing.Size(204, 19);
            this.checkShowArchived.TabIndex = 38;
            this.checkShowArchived.Text = "Show Archived/Deceased/Hidden";
            this.checkShowArchived.CheckedChanged += new System.EventHandler(this.checkShowArchived_CheckedChanged);
            // 
            // textChartNumber
            // 
            this.textChartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textChartNumber.Location = new System.Drawing.Point(148, 195);
            this.textChartNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textChartNumber.Name = "textChartNumber";
            this.textChartNumber.Size = new System.Drawing.Size(226, 23);
            this.textChartNumber.TabIndex = 17;
            this.textChartNumber.TextChanged += new System.EventHandler(this.textChartNumber_TextChanged);
            // 
            // ssnTextBox
            // 
            this.ssnTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ssnTextBox.Location = new System.Drawing.Point(148, 151);
            this.ssnTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ssnTextBox.Name = "ssnTextBox";
            this.ssnTextBox.Size = new System.Drawing.Size(226, 23);
            this.ssnTextBox.TabIndex = 13;
            this.ssnTextBox.TextChanged += new System.EventHandler(this.textSSN_TextChanged);
            // 
            // ssnLabel
            // 
            this.ssnLabel.AutoSize = true;
            this.ssnLabel.Location = new System.Drawing.Point(114, 154);
            this.ssnLabel.Name = "ssnLabel";
            this.ssnLabel.Size = new System.Drawing.Size(28, 15);
            this.ssnLabel.TabIndex = 12;
            this.ssnLabel.Text = "SSN";
            this.ssnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // billingTypeLabel
            // 
            this.billingTypeLabel.AutoSize = true;
            this.billingTypeLabel.Location = new System.Drawing.Point(74, 330);
            this.billingTypeLabel.Name = "billingTypeLabel";
            this.billingTypeLabel.Size = new System.Drawing.Size(68, 15);
            this.billingTypeLabel.TabIndex = 30;
            this.billingTypeLabel.Text = "Billing Type";
            this.billingTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 15);
            this.label10.TabIndex = 16;
            this.label10.Text = "Chart Number";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // patNumTextBox
            // 
            this.patNumTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patNumTextBox.Location = new System.Drawing.Point(148, 173);
            this.patNumTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.patNumTextBox.Name = "patNumTextBox";
            this.patNumTextBox.Size = new System.Drawing.Size(226, 23);
            this.patNumTextBox.TabIndex = 15;
            this.patNumTextBox.TextChanged += new System.EventHandler(this.textPatNum_TextChanged);
            // 
            // patNumLabel
            // 
            this.patNumLabel.AutoSize = true;
            this.patNumLabel.Location = new System.Drawing.Point(51, 176);
            this.patNumLabel.Name = "patNumLabel";
            this.patNumLabel.Size = new System.Drawing.Size(91, 15);
            this.patNumLabel.TabIndex = 14;
            this.patNumLabel.Text = "Patient Number";
            this.patNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stateTextBox
            // 
            this.stateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateTextBox.Location = new System.Drawing.Point(148, 129);
            this.stateTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(226, 23);
            this.stateTextBox.TabIndex = 11;
            this.stateTextBox.TextChanged += new System.EventHandler(this.textState_TextChanged);
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(109, 132);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(33, 15);
            this.stateLabel.TabIndex = 10;
            this.stateLabel.Text = "State";
            this.stateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cityTextBox
            // 
            this.cityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cityTextBox.Location = new System.Drawing.Point(148, 107);
            this.cityTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(226, 23);
            this.cityTextBox.TabIndex = 9;
            this.cityTextBox.TextChanged += new System.EventHandler(this.textCity_TextChanged);
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(114, 110);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(28, 15);
            this.cityLabel.TabIndex = 8;
            this.cityLabel.Text = "City";
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkGuarantors
            // 
            this.checkGuarantors.AutoSize = true;
            this.checkGuarantors.Location = new System.Drawing.Point(148, 400);
            this.checkGuarantors.Name = "checkGuarantors";
            this.checkGuarantors.Size = new System.Drawing.Size(112, 19);
            this.checkGuarantors.TabIndex = 36;
            this.checkGuarantors.Text = "Guarantors Only";
            this.checkGuarantors.CheckedChanged += new System.EventHandler(this.checkGuarantors_CheckedChanged);
            // 
            // checkHideInactive
            // 
            this.checkHideInactive.AutoSize = true;
            this.checkHideInactive.Location = new System.Drawing.Point(148, 421);
            this.checkHideInactive.Name = "checkHideInactive";
            this.checkHideInactive.Size = new System.Drawing.Size(140, 19);
            this.checkHideInactive.TabIndex = 37;
            this.checkHideInactive.Text = "Hide Inactive Patients";
            this.checkHideInactive.CheckedChanged += new System.EventHandler(this.checkHideInactive_CheckedChanged);
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(148, 85);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(226, 23);
            this.addressTextBox.TabIndex = 7;
            this.addressTextBox.TextChanged += new System.EventHandler(this.textAddress_TextChanged);
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(93, 88);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(49, 15);
            this.addressLabel.TabIndex = 6;
            this.addressLabel.Text = "Address";
            this.addressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneTextBox.Location = new System.Drawing.Point(148, 63);
            this.phoneTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(226, 23);
            this.phoneTextBox.TabIndex = 5;
            this.phoneTextBox.TextChanged += new System.EventHandler(this.textHmPhone_TextChanged);
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(71, 66);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(71, 15);
            this.phoneLabel.TabIndex = 4;
            this.phoneLabel.Text = "Phone (any)";
            this.phoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firstNameTextBox.Location = new System.Drawing.Point(148, 41);
            this.firstNameTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(226, 23);
            this.firstNameTextBox.TabIndex = 3;
            this.firstNameTextBox.TextChanged += new System.EventHandler(this.textFName_TextChanged);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(78, 44);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(64, 15);
            this.firstNameLabel.TabIndex = 2;
            this.firstNameLabel.Text = "First Name";
            this.firstNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridMain.EditableEnterMovesDown = false;
            this.gridMain.HasAddButton = false;
            this.gridMain.HasDropDowns = false;
            this.gridMain.HasMultilineHeaders = false;
            this.gridMain.HScrollVisible = true;
            this.gridMain.Location = new System.Drawing.Point(13, 19);
            this.gridMain.Name = "gridMain";
            this.gridMain.ScrollValue = 0;
            this.gridMain.Size = new System.Drawing.Size(673, 589);
            this.gridMain.TabIndex = 0;
            this.gridMain.Title = "Select Patient";
            this.gridMain.TitleVisible = true;
            this.gridMain.WrapText = false;
            this.gridMain.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
            this.gridMain.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridMain_CellClick);
            // 
            // FormPatientSelect
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.Controls.Add(this.addManyButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.addPatientButton);
            this.Controls.Add(this.gridMain);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPatientSelect";
            this.ShowInTaskbar = false;
            this.Text = "Select Patient";
            this.Load += new System.EventHandler(this.FormSelectPatient_Load);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button addPatientButton;
        private System.Windows.Forms.GroupBox searchGroupBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.TextBox addressTextBox;
        private OpenDental.ValidPhone phoneTextBox;
        private System.Windows.Forms.CheckBox checkHideInactive;
        private System.Windows.Forms.CheckBox checkGuarantors;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.TextBox stateTextBox;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.TextBox patNumTextBox;
        private System.Windows.Forms.Label patNumLabel;
        private System.Windows.Forms.TextBox textChartNumber;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label billingTypeLabel;
        private System.Windows.Forms.TextBox ssnTextBox;
        private System.Windows.Forms.Label ssnLabel;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.CheckBox checkShowMerged;
        private System.Windows.Forms.TextBox invoiceNumberTextBox;
        private System.Windows.Forms.Label invoiceNumberLabel;
        private System.Windows.Forms.CheckBox checkShowArchived;
        private System.Windows.Forms.TextBox birthdateTextBox;
        private System.Windows.Forms.Label birthdateLabel;
        private System.Windows.Forms.ComboBox billingTypeComboBox;
        private System.Windows.Forms.Button butGetAll;
        private System.Windows.Forms.CheckBox checkRefresh;
        private System.Windows.Forms.Button addManyButton;
        private System.Windows.Forms.ComboBox siteComboBox;
        private System.Windows.Forms.Label siteLabel;
        private System.Windows.Forms.TextBox selectedTxtBox;
        private System.Windows.Forms.TextBox subscriberIdTextBox;
        private System.Windows.Forms.Label subscriberIdLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox countryTextBox;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
        private OpenDental.UI.ODGrid gridMain;
    }
}