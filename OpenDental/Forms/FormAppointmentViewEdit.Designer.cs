namespace OpenDental
{
    partial class FormAppointmentViewEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppointmentViewEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.operatoriesListBox = new System.Windows.Forms.ListBox();
            this.providersListBox = new System.Windows.Forms.ListBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveLeftButton = new System.Windows.Forms.Button();
            this.moveRightButton = new System.Windows.Forms.Button();
            this.rowsPerIncrementLabel = new System.Windows.Forms.Label();
            this.rowsPerIncrementTextBox = new System.Windows.Forms.TextBox();
            this.onlyShowScheduledProvidersCheckBox = new System.Windows.Forms.CheckBox();
            this.timeBeforeTextBox = new System.Windows.Forms.TextBox();
            this.displayFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.timeAfterLabel = new System.Windows.Forms.Label();
            this.timeAfterTextBox = new System.Windows.Forms.TextBox();
            this.timeBeforeLabel = new System.Windows.Forms.Label();
            this.fieldsGroupBox = new System.Windows.Forms.GroupBox();
            this.upperRightLabel = new System.Windows.Forms.Label();
            this.upperRightStackBehaviourComboBox = new System.Windows.Forms.ComboBox();
            this.lowerRightStackBehaviourComboBox = new System.Windows.Forms.ComboBox();
            this.fieldsAvailableTreeView = new System.Windows.Forms.TreeView();
            this.fieldsUpperRightListBox = new System.Windows.Forms.ListBox();
            this.fieldsLowerRightListBox = new System.Windows.Forms.ListBox();
            this.mainLabel = new System.Windows.Forms.Label();
            this.lowerRightLabel = new System.Windows.Forms.Label();
            this.fieldsMainListBox = new System.Windows.Forms.ListBox();
            this.moveLabel = new System.Windows.Forms.Label();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.scrollStartTimeTextBox = new System.Windows.Forms.TextBox();
            this.scrollStartTimeLabel = new System.Windows.Forms.Label();
            this.scrollStartDynamicCheckBox = new System.Windows.Forms.CheckBox();
            this.hideAppointmentBubblesCheckBox = new System.Windows.Forms.CheckBox();
            this.operatoriesGroupBox = new System.Windows.Forms.GroupBox();
            this.providersGroupBox = new System.Windows.Forms.GroupBox();
            this.displayFilterGroupBox.SuspendLayout();
            this.fieldsGroupBox.SuspendLayout();
            this.operatoriesGroupBox.SuspendLayout();
            this.providersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(776, 658);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(660, 658);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 15;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // operatoriesListBox
            // 
            this.operatoriesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operatoriesListBox.IntegralHeight = false;
            this.operatoriesListBox.ItemHeight = 15;
            this.operatoriesListBox.Location = new System.Drawing.Point(3, 19);
            this.operatoriesListBox.Name = "operatoriesListBox";
            this.operatoriesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.operatoriesListBox.Size = new System.Drawing.Size(194, 178);
            this.operatoriesListBox.TabIndex = 0;
            // 
            // providersListBox
            // 
            this.providersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.providersListBox.IntegralHeight = false;
            this.providersListBox.ItemHeight = 15;
            this.providersListBox.Location = new System.Drawing.Point(3, 19);
            this.providersListBox.Name = "providersListBox";
            this.providersListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.providersListBox.Size = new System.Drawing.Size(194, 178);
            this.providersListBox.TabIndex = 0;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(167, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(240, 19);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(250, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // moveDownButton
            // 
            this.moveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveDownButton.Image = global::OpenDental.Properties.Resources.IconArrowDown;
            this.moveDownButton.Location = new System.Drawing.Point(621, 404);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(40, 30);
            this.moveDownButton.TabIndex = 13;
            this.moveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveUpButton.Image = global::OpenDental.Properties.Resources.IconArrowUp;
            this.moveUpButton.Location = new System.Drawing.Point(575, 404);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(40, 30);
            this.moveUpButton.TabIndex = 12;
            this.moveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // moveLeftButton
            // 
            this.moveLeftButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.moveLeftButton.Image = global::OpenDental.Properties.Resources.IconArrowLeft;
            this.moveLeftButton.Location = new System.Drawing.Point(232, 191);
            this.moveLeftButton.Name = "moveLeftButton";
            this.moveLeftButton.Size = new System.Drawing.Size(30, 25);
            this.moveLeftButton.TabIndex = 2;
            this.moveLeftButton.Click += new System.EventHandler(this.MoveLeftButton_Click);
            // 
            // moveRightButton
            // 
            this.moveRightButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.moveRightButton.Image = global::OpenDental.Properties.Resources.IconArrowRight;
            this.moveRightButton.Location = new System.Drawing.Point(232, 160);
            this.moveRightButton.Name = "moveRightButton";
            this.moveRightButton.Size = new System.Drawing.Size(30, 25);
            this.moveRightButton.TabIndex = 1;
            this.moveRightButton.Click += new System.EventHandler(this.MoveRightButton_Click);
            // 
            // rowsPerIncrementLabel
            // 
            this.rowsPerIncrementLabel.AutoSize = true;
            this.rowsPerIncrementLabel.Location = new System.Drawing.Point(38, 51);
            this.rowsPerIncrementLabel.Name = "rowsPerIncrementLabel";
            this.rowsPerIncrementLabel.Size = new System.Drawing.Size(196, 15);
            this.rowsPerIncrementLabel.TabIndex = 2;
            this.rowsPerIncrementLabel.Text = "Rows per time increment (usually 1)";
            // 
            // rowsPerIncrementTextBox
            // 
            this.rowsPerIncrementTextBox.Location = new System.Drawing.Point(240, 48);
            this.rowsPerIncrementTextBox.Name = "rowsPerIncrementTextBox";
            this.rowsPerIncrementTextBox.Size = new System.Drawing.Size(60, 23);
            this.rowsPerIncrementTextBox.TabIndex = 3;
            this.rowsPerIncrementTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RowsPerIncrementTextBox_Validating);
            // 
            // onlyShowScheduledProvidersCheckBox
            // 
            this.onlyShowScheduledProvidersCheckBox.AutoSize = true;
            this.onlyShowScheduledProvidersCheckBox.Location = new System.Drawing.Point(6, 22);
            this.onlyShowScheduledProvidersCheckBox.Name = "onlyShowScheduledProvidersCheckBox";
            this.onlyShowScheduledProvidersCheckBox.Size = new System.Drawing.Size(271, 19);
            this.onlyShowScheduledProvidersCheckBox.TabIndex = 0;
            this.onlyShowScheduledProvidersCheckBox.Text = "Only show operatories for scheduled providers";
            this.onlyShowScheduledProvidersCheckBox.UseVisualStyleBackColor = true;
            this.onlyShowScheduledProvidersCheckBox.Click += new System.EventHandler(this.OnlyShowScheduledProvidersCheckBox_Click);
            // 
            // timeBeforeTextBox
            // 
            this.timeBeforeTextBox.Location = new System.Drawing.Point(160, 60);
            this.timeBeforeTextBox.Name = "timeBeforeTextBox";
            this.timeBeforeTextBox.Size = new System.Drawing.Size(56, 23);
            this.timeBeforeTextBox.TabIndex = 2;
            // 
            // displayFilterGroupBox
            // 
            this.displayFilterGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.displayFilterGroupBox.Controls.Add(this.timeAfterLabel);
            this.displayFilterGroupBox.Controls.Add(this.timeAfterTextBox);
            this.displayFilterGroupBox.Controls.Add(this.timeBeforeLabel);
            this.displayFilterGroupBox.Controls.Add(this.timeBeforeTextBox);
            this.displayFilterGroupBox.Controls.Add(this.onlyShowScheduledProvidersCheckBox);
            this.displayFilterGroupBox.Location = new System.Drawing.Point(586, 19);
            this.displayFilterGroupBox.Name = "displayFilterGroupBox";
            this.displayFilterGroupBox.Size = new System.Drawing.Size(300, 140);
            this.displayFilterGroupBox.TabIndex = 10;
            this.displayFilterGroupBox.TabStop = false;
            this.displayFilterGroupBox.Text = "Display Filter";
            // 
            // timeAfterLabel
            // 
            this.timeAfterLabel.AutoSize = true;
            this.timeAfterLabel.Location = new System.Drawing.Point(58, 92);
            this.timeAfterLabel.Name = "timeAfterLabel";
            this.timeAfterLabel.Size = new System.Drawing.Size(96, 15);
            this.timeAfterLabel.TabIndex = 3;
            this.timeAfterLabel.Text = "Only if after time";
            // 
            // timeAfterTextBox
            // 
            this.timeAfterTextBox.Location = new System.Drawing.Point(160, 89);
            this.timeAfterTextBox.Name = "timeAfterTextBox";
            this.timeAfterTextBox.Size = new System.Drawing.Size(56, 23);
            this.timeAfterTextBox.TabIndex = 4;
            // 
            // timeBeforeLabel
            // 
            this.timeBeforeLabel.AutoSize = true;
            this.timeBeforeLabel.Location = new System.Drawing.Point(48, 63);
            this.timeBeforeLabel.Name = "timeBeforeLabel";
            this.timeBeforeLabel.Size = new System.Drawing.Size(106, 15);
            this.timeBeforeLabel.TabIndex = 1;
            this.timeBeforeLabel.Text = "Only if before time";
            // 
            // fieldsGroupBox
            // 
            this.fieldsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsGroupBox.Controls.Add(this.upperRightLabel);
            this.fieldsGroupBox.Controls.Add(this.upperRightStackBehaviourComboBox);
            this.fieldsGroupBox.Controls.Add(this.lowerRightStackBehaviourComboBox);
            this.fieldsGroupBox.Controls.Add(this.fieldsAvailableTreeView);
            this.fieldsGroupBox.Controls.Add(this.fieldsUpperRightListBox);
            this.fieldsGroupBox.Controls.Add(this.fieldsLowerRightListBox);
            this.fieldsGroupBox.Controls.Add(this.mainLabel);
            this.fieldsGroupBox.Controls.Add(this.lowerRightLabel);
            this.fieldsGroupBox.Controls.Add(this.fieldsMainListBox);
            this.fieldsGroupBox.Controls.Add(this.moveUpButton);
            this.fieldsGroupBox.Controls.Add(this.moveLabel);
            this.fieldsGroupBox.Controls.Add(this.moveRightButton);
            this.fieldsGroupBox.Controls.Add(this.moveDownButton);
            this.fieldsGroupBox.Controls.Add(this.moveLeftButton);
            this.fieldsGroupBox.Location = new System.Drawing.Point(219, 192);
            this.fieldsGroupBox.Name = "fieldsGroupBox";
            this.fieldsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.fieldsGroupBox.Size = new System.Drawing.Size(667, 440);
            this.fieldsGroupBox.TabIndex = 13;
            this.fieldsGroupBox.TabStop = false;
            this.fieldsGroupBox.Text = "Rows Displayed (double click to edit or to move to another corner)";
            // 
            // upperRightLabel
            // 
            this.upperRightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upperRightLabel.AutoSize = true;
            this.upperRightLabel.Location = new System.Drawing.Point(478, 26);
            this.upperRightLabel.Name = "upperRightLabel";
            this.upperRightLabel.Size = new System.Drawing.Size(70, 15);
            this.upperRightLabel.TabIndex = 5;
            this.upperRightLabel.Text = "Upper Right";
            // 
            // upperRightStackBehaviourComboBox
            // 
            this.upperRightStackBehaviourComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upperRightStackBehaviourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.upperRightStackBehaviourComboBox.FormattingEnabled = true;
            this.upperRightStackBehaviourComboBox.Items.AddRange(new object[] {
            "Vertical",
            "Horizontal"});
            this.upperRightStackBehaviourComboBox.Location = new System.Drawing.Point(481, 44);
            this.upperRightStackBehaviourComboBox.Name = "upperRightStackBehaviourComboBox";
            this.upperRightStackBehaviourComboBox.Size = new System.Drawing.Size(180, 23);
            this.upperRightStackBehaviourComboBox.TabIndex = 6;
            // 
            // lowerRightStackBehaviourComboBox
            // 
            this.lowerRightStackBehaviourComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lowerRightStackBehaviourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lowerRightStackBehaviourComboBox.FormattingEnabled = true;
            this.lowerRightStackBehaviourComboBox.Items.AddRange(new object[] {
            "Vertical",
            "Horizontal"});
            this.lowerRightStackBehaviourComboBox.Location = new System.Drawing.Point(481, 238);
            this.lowerRightStackBehaviourComboBox.Name = "lowerRightStackBehaviourComboBox";
            this.lowerRightStackBehaviourComboBox.Size = new System.Drawing.Size(180, 23);
            this.lowerRightStackBehaviourComboBox.TabIndex = 9;
            // 
            // fieldsAvailableTreeView
            // 
            this.fieldsAvailableTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fieldsAvailableTreeView.Location = new System.Drawing.Point(6, 44);
            this.fieldsAvailableTreeView.Name = "fieldsAvailableTreeView";
            this.fieldsAvailableTreeView.Size = new System.Drawing.Size(220, 343);
            this.fieldsAvailableTreeView.TabIndex = 0;
            this.fieldsAvailableTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FieldsAvailableTreeView_MouseDoubleClick);
            // 
            // fieldsUpperRightListBox
            // 
            this.fieldsUpperRightListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsUpperRightListBox.FormattingEnabled = true;
            this.fieldsUpperRightListBox.IntegralHeight = false;
            this.fieldsUpperRightListBox.ItemHeight = 15;
            this.fieldsUpperRightListBox.Location = new System.Drawing.Point(481, 73);
            this.fieldsUpperRightListBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.fieldsUpperRightListBox.Name = "fieldsUpperRightListBox";
            this.fieldsUpperRightListBox.Size = new System.Drawing.Size(180, 120);
            this.fieldsUpperRightListBox.TabIndex = 7;
            this.fieldsUpperRightListBox.SelectedIndexChanged += new System.EventHandler(this.FieldsListBox_SelectedIndexChanged);
            this.fieldsUpperRightListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FieldsListBox_MouseDoubleClick);
            // 
            // fieldsLowerRightListBox
            // 
            this.fieldsLowerRightListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsLowerRightListBox.FormattingEnabled = true;
            this.fieldsLowerRightListBox.IntegralHeight = false;
            this.fieldsLowerRightListBox.ItemHeight = 15;
            this.fieldsLowerRightListBox.Location = new System.Drawing.Point(481, 267);
            this.fieldsLowerRightListBox.Name = "fieldsLowerRightListBox";
            this.fieldsLowerRightListBox.Size = new System.Drawing.Size(180, 120);
            this.fieldsLowerRightListBox.TabIndex = 10;
            this.fieldsLowerRightListBox.SelectedIndexChanged += new System.EventHandler(this.FieldsListBox_SelectedIndexChanged);
            this.fieldsLowerRightListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FieldsListBox_MouseDoubleClick);
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.mainLabel.Location = new System.Drawing.Point(265, 26);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(34, 15);
            this.mainLabel.TabIndex = 3;
            this.mainLabel.Text = "Main";
            // 
            // lowerRightLabel
            // 
            this.lowerRightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lowerRightLabel.AutoSize = true;
            this.lowerRightLabel.Location = new System.Drawing.Point(481, 220);
            this.lowerRightLabel.Name = "lowerRightLabel";
            this.lowerRightLabel.Size = new System.Drawing.Size(70, 15);
            this.lowerRightLabel.TabIndex = 8;
            this.lowerRightLabel.Text = "Lower Right";
            // 
            // fieldsMainListBox
            // 
            this.fieldsMainListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsMainListBox.FormattingEnabled = true;
            this.fieldsMainListBox.IntegralHeight = false;
            this.fieldsMainListBox.ItemHeight = 15;
            this.fieldsMainListBox.Location = new System.Drawing.Point(268, 44);
            this.fieldsMainListBox.Name = "fieldsMainListBox";
            this.fieldsMainListBox.Size = new System.Drawing.Size(207, 343);
            this.fieldsMainListBox.TabIndex = 4;
            this.fieldsMainListBox.SelectedIndexChanged += new System.EventHandler(this.FieldsListBox_SelectedIndexChanged);
            this.fieldsMainListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FieldsListBox_MouseDoubleClick);
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(388, 412);
            this.moveLabel.Name = "moveLabel";
            this.moveLabel.Size = new System.Drawing.Size(181, 15);
            this.moveLabel.TabIndex = 11;
            this.moveLabel.Text = "Move any item within its own list\r\n";
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(240, 156);
            this.clinicComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(160, 23);
            this.clinicComboBox.TabIndex = 9;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(146, 159);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(88, 15);
            this.clinicLabel.TabIndex = 8;
            this.clinicLabel.Text = "Assigned Clinic";
            // 
            // scrollStartTimeTextBox
            // 
            this.scrollStartTimeTextBox.Location = new System.Drawing.Point(240, 77);
            this.scrollStartTimeTextBox.Name = "scrollStartTimeTextBox";
            this.scrollStartTimeTextBox.Size = new System.Drawing.Size(60, 23);
            this.scrollStartTimeTextBox.TabIndex = 5;
            // 
            // scrollStartTimeLabel
            // 
            this.scrollStartTimeLabel.AutoSize = true;
            this.scrollStartTimeLabel.Location = new System.Drawing.Point(106, 80);
            this.scrollStartTimeLabel.Name = "scrollStartTimeLabel";
            this.scrollStartTimeLabel.Size = new System.Drawing.Size(128, 15);
            this.scrollStartTimeLabel.TabIndex = 4;
            this.scrollStartTimeLabel.Text = "View start time on load";
            // 
            // scrollStartDynamicCheckBox
            // 
            this.scrollStartDynamicCheckBox.AutoSize = true;
            this.scrollStartDynamicCheckBox.Location = new System.Drawing.Point(240, 106);
            this.scrollStartDynamicCheckBox.Name = "scrollStartDynamicCheckBox";
            this.scrollStartDynamicCheckBox.Size = new System.Drawing.Size(227, 19);
            this.scrollStartDynamicCheckBox.TabIndex = 6;
            this.scrollStartDynamicCheckBox.Text = "Dynamic start time based on schedule";
            this.scrollStartDynamicCheckBox.UseVisualStyleBackColor = true;
            // 
            // hideAppointmentBubblesCheckBox
            // 
            this.hideAppointmentBubblesCheckBox.AutoSize = true;
            this.hideAppointmentBubblesCheckBox.Location = new System.Drawing.Point(240, 131);
            this.hideAppointmentBubblesCheckBox.Name = "hideAppointmentBubblesCheckBox";
            this.hideAppointmentBubblesCheckBox.Size = new System.Drawing.Size(168, 19);
            this.hideAppointmentBubblesCheckBox.TabIndex = 7;
            this.hideAppointmentBubblesCheckBox.Text = "Hide appointment bubbles";
            this.hideAppointmentBubblesCheckBox.UseVisualStyleBackColor = true;
            // 
            // operatoriesGroupBox
            // 
            this.operatoriesGroupBox.Controls.Add(this.operatoriesListBox);
            this.operatoriesGroupBox.Location = new System.Drawing.Point(13, 192);
            this.operatoriesGroupBox.Name = "operatoriesGroupBox";
            this.operatoriesGroupBox.Size = new System.Drawing.Size(200, 200);
            this.operatoriesGroupBox.TabIndex = 11;
            this.operatoriesGroupBox.TabStop = false;
            this.operatoriesGroupBox.Text = "View Operatories";
            // 
            // providersGroupBox
            // 
            this.providersGroupBox.Controls.Add(this.providersListBox);
            this.providersGroupBox.Location = new System.Drawing.Point(13, 398);
            this.providersGroupBox.Name = "providersGroupBox";
            this.providersGroupBox.Size = new System.Drawing.Size(200, 200);
            this.providersGroupBox.TabIndex = 12;
            this.providersGroupBox.TabStop = false;
            this.providersGroupBox.Text = "View Provider Bars";
            // 
            // FormAppointmentViewEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(899, 701);
            this.Controls.Add(this.providersGroupBox);
            this.Controls.Add(this.operatoriesGroupBox);
            this.Controls.Add(this.hideAppointmentBubblesCheckBox);
            this.Controls.Add(this.scrollStartDynamicCheckBox);
            this.Controls.Add(this.scrollStartTimeLabel);
            this.Controls.Add(this.scrollStartTimeTextBox);
            this.Controls.Add(this.displayFilterGroupBox);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.fieldsGroupBox);
            this.Controls.Add(this.rowsPerIncrementTextBox);
            this.Controls.Add(this.rowsPerIncrementLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAppointmentViewEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment View Edit";
            this.Load += new System.EventHandler(this.FormAppointmentViewEdit_Load);
            this.displayFilterGroupBox.ResumeLayout(false);
            this.displayFilterGroupBox.PerformLayout();
            this.fieldsGroupBox.ResumeLayout(false);
            this.fieldsGroupBox.PerformLayout();
            this.operatoriesGroupBox.ResumeLayout(false);
            this.providersGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.ListBox operatoriesListBox;
        private System.Windows.Forms.ListBox providersListBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveLeftButton;
        private System.Windows.Forms.Button moveRightButton;
        private System.Windows.Forms.Label rowsPerIncrementLabel;
        private System.Windows.Forms.TextBox rowsPerIncrementTextBox;
        private System.Windows.Forms.CheckBox onlyShowScheduledProvidersCheckBox;
        private System.Windows.Forms.TextBox timeBeforeTextBox;
        private System.Windows.Forms.GroupBox displayFilterGroupBox;
        private System.Windows.Forms.Label timeBeforeLabel;
        private System.Windows.Forms.Label timeAfterLabel;
        private System.Windows.Forms.TextBox timeAfterTextBox;
        private System.Windows.Forms.GroupBox fieldsGroupBox;
        private System.Windows.Forms.Label moveLabel;
        private System.Windows.Forms.Label lowerRightLabel;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.TextBox scrollStartTimeTextBox;
        private System.Windows.Forms.Label scrollStartTimeLabel;
        private System.Windows.Forms.CheckBox scrollStartDynamicCheckBox;
        private System.Windows.Forms.CheckBox hideAppointmentBubblesCheckBox;
        private System.Windows.Forms.TreeView fieldsAvailableTreeView;
        private System.Windows.Forms.ListBox fieldsMainListBox;
        private System.Windows.Forms.ListBox fieldsUpperRightListBox;
        private System.Windows.Forms.ListBox fieldsLowerRightListBox;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.Label upperRightLabel;
        private System.Windows.Forms.ComboBox upperRightStackBehaviourComboBox;
        private System.Windows.Forms.ComboBox lowerRightStackBehaviourComboBox;
        private System.Windows.Forms.GroupBox operatoriesGroupBox;
        private System.Windows.Forms.GroupBox providersGroupBox;
    }
}
