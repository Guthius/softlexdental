namespace OpenDental
{
    partial class FormAppointmentSearchAdvanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppointmentSearchAdvanced));
            this.searchButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resultsGrid = new OpenDental.UI.ODGrid();
            this.blockoutComboBox = new System.Windows.Forms.ComboBox();
            this.moreButton = new System.Windows.Forms.Button();
            this.filtersGroupBox = new System.Windows.Forms.GroupBox();
            this.providersComboBox = new OpenDental.UI.ComboBoxMulti();
            this.providersLabel = new System.Windows.Forms.Label();
            this.timeBeforeLabel = new System.Windows.Forms.Label();
            this.dateToDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.providersBrowseButton = new System.Windows.Forms.Button();
            this.clinicBrowseButton = new System.Windows.Forms.Button();
            this.timeAfterTextBox = new System.Windows.Forms.TextBox();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.timeAfterLabel = new System.Windows.Forms.Label();
            this.appointmentViewLabel = new System.Windows.Forms.Label();
            this.appointmentViewComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.timeBeforeTextBox = new System.Windows.Forms.TextBox();
            this.blockoutLabel = new System.Windows.Forms.Label();
            this.dateToLabel = new System.Windows.Forms.Label();
            this.dateFromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateFromLabel = new System.Windows.Forms.Label();
            this.quickSearchGroupBox = new System.Windows.Forms.GroupBox();
            this.providersHygienistButton = new System.Windows.Forms.Button();
            this.providersDentistButton = new System.Windows.Forms.Button();
            this.filtersGroupBox.SuspendLayout();
            this.quickSearchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(275, 478);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(110, 30);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "&Search";
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(581, 478);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Close";
            // 
            // resultsGrid
            // 
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.resultsGrid.EditableEnterMovesDown = false;
            this.resultsGrid.HasAddButton = false;
            this.resultsGrid.HasDropDowns = false;
            this.resultsGrid.HasMultilineHeaders = false;
            this.resultsGrid.HScrollVisible = false;
            this.resultsGrid.Location = new System.Drawing.Point(13, 19);
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ScrollValue = 0;
            this.resultsGrid.Size = new System.Drawing.Size(372, 417);
            this.resultsGrid.TabIndex = 0;
            this.resultsGrid.Title = "Search Results";
            this.resultsGrid.TitleVisible = true;
            this.resultsGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.ResultsGrid_CellClick);
            // 
            // blockoutComboBox
            // 
            this.blockoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockoutComboBox.FormattingEnabled = true;
            this.blockoutComboBox.Location = new System.Drawing.Point(120, 138);
            this.blockoutComboBox.Name = "blockoutComboBox";
            this.blockoutComboBox.Size = new System.Drawing.Size(130, 23);
            this.blockoutComboBox.TabIndex = 9;
            // 
            // moreButton
            // 
            this.moreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moreButton.Enabled = false;
            this.moreButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowRight;
            this.moreButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.moreButton.Location = new System.Drawing.Point(275, 442);
            this.moreButton.Name = "moreButton";
            this.moreButton.Size = new System.Drawing.Size(110, 30);
            this.moreButton.TabIndex = 3;
            this.moreButton.Text = "More";
            this.moreButton.Click += new System.EventHandler(this.MoreButton_Click);
            // 
            // filtersGroupBox
            // 
            this.filtersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filtersGroupBox.Controls.Add(this.providersComboBox);
            this.filtersGroupBox.Controls.Add(this.providersLabel);
            this.filtersGroupBox.Controls.Add(this.timeBeforeLabel);
            this.filtersGroupBox.Controls.Add(this.dateToDateTimePicker);
            this.filtersGroupBox.Controls.Add(this.providersBrowseButton);
            this.filtersGroupBox.Controls.Add(this.clinicBrowseButton);
            this.filtersGroupBox.Controls.Add(this.timeAfterTextBox);
            this.filtersGroupBox.Controls.Add(this.clinicComboBox);
            this.filtersGroupBox.Controls.Add(this.timeAfterLabel);
            this.filtersGroupBox.Controls.Add(this.appointmentViewLabel);
            this.filtersGroupBox.Controls.Add(this.appointmentViewComboBox);
            this.filtersGroupBox.Controls.Add(this.clinicLabel);
            this.filtersGroupBox.Controls.Add(this.timeBeforeTextBox);
            this.filtersGroupBox.Controls.Add(this.blockoutLabel);
            this.filtersGroupBox.Controls.Add(this.dateToLabel);
            this.filtersGroupBox.Controls.Add(this.dateFromDateTimePicker);
            this.filtersGroupBox.Controls.Add(this.blockoutComboBox);
            this.filtersGroupBox.Controls.Add(this.dateFromLabel);
            this.filtersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filtersGroupBox.Location = new System.Drawing.Point(391, 19);
            this.filtersGroupBox.Name = "filtersGroupBox";
            this.filtersGroupBox.Size = new System.Drawing.Size(300, 300);
            this.filtersGroupBox.TabIndex = 1;
            this.filtersGroupBox.TabStop = false;
            this.filtersGroupBox.Text = "Filters";
            // 
            // providersComboBox
            // 
            this.providersComboBox.ArraySelectedIndices = new int[0];
            this.providersComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.providersComboBox.Items = ((System.Collections.ArrayList)(resources.GetObject("providersComboBox.Items")));
            this.providersComboBox.Location = new System.Drawing.Point(120, 196);
            this.providersComboBox.Name = "providersComboBox";
            this.providersComboBox.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("providersComboBox.SelectedIndices")));
            this.providersComboBox.Size = new System.Drawing.Size(130, 21);
            this.providersComboBox.TabIndex = 14;
            this.providersComboBox.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.ProvidersComboBox_SelectionChangeCommitted);
            // 
            // providersLabel
            // 
            this.providersLabel.AutoSize = true;
            this.providersLabel.Location = new System.Drawing.Point(58, 197);
            this.providersLabel.Name = "providersLabel";
            this.providersLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.providersLabel.Size = new System.Drawing.Size(56, 15);
            this.providersLabel.TabIndex = 13;
            this.providersLabel.Text = "Providers";
            this.providersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeBeforeLabel
            // 
            this.timeBeforeLabel.AutoSize = true;
            this.timeBeforeLabel.Location = new System.Drawing.Point(29, 83);
            this.timeBeforeLabel.Name = "timeBeforeLabel";
            this.timeBeforeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.timeBeforeLabel.Size = new System.Drawing.Size(85, 15);
            this.timeBeforeLabel.TabIndex = 4;
            this.timeBeforeLabel.Text = "Starting before";
            this.timeBeforeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateToDateTimePicker
            // 
            this.dateToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateToDateTimePicker.Location = new System.Drawing.Point(120, 51);
            this.dateToDateTimePicker.Name = "dateToDateTimePicker";
            this.dateToDateTimePicker.Size = new System.Drawing.Size(130, 23);
            this.dateToDateTimePicker.TabIndex = 3;
            // 
            // providersBrowseButton
            // 
            this.providersBrowseButton.Location = new System.Drawing.Point(256, 195);
            this.providersBrowseButton.Name = "providersBrowseButton";
            this.providersBrowseButton.Size = new System.Drawing.Size(30, 25);
            this.providersBrowseButton.TabIndex = 15;
            this.providersBrowseButton.Text = "...";
            this.providersBrowseButton.Click += new System.EventHandler(this.ProvidersBrowseButton_Click);
            // 
            // clinicBrowseButton
            // 
            this.clinicBrowseButton.Location = new System.Drawing.Point(256, 166);
            this.clinicBrowseButton.Name = "clinicBrowseButton";
            this.clinicBrowseButton.Size = new System.Drawing.Size(30, 25);
            this.clinicBrowseButton.TabIndex = 12;
            this.clinicBrowseButton.Text = "...";
            this.clinicBrowseButton.Click += new System.EventHandler(this.ClinicBrowseButton_Click);
            // 
            // timeAfterTextBox
            // 
            this.timeAfterTextBox.Location = new System.Drawing.Point(120, 109);
            this.timeAfterTextBox.Name = "timeAfterTextBox";
            this.timeAfterTextBox.Size = new System.Drawing.Size(60, 23);
            this.timeAfterTextBox.TabIndex = 7;
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.FormattingEnabled = true;
            this.clinicComboBox.Location = new System.Drawing.Point(120, 167);
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(130, 23);
            this.clinicComboBox.TabIndex = 11;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // timeAfterLabel
            // 
            this.timeAfterLabel.AutoSize = true;
            this.timeAfterLabel.Location = new System.Drawing.Point(39, 111);
            this.timeAfterLabel.Name = "timeAfterLabel";
            this.timeAfterLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.timeAfterLabel.Size = new System.Drawing.Size(75, 15);
            this.timeAfterLabel.TabIndex = 6;
            this.timeAfterLabel.Text = "Starting after";
            this.timeAfterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // appointmentViewLabel
            // 
            this.appointmentViewLabel.AutoSize = true;
            this.appointmentViewLabel.Location = new System.Drawing.Point(82, 226);
            this.appointmentViewLabel.Name = "appointmentViewLabel";
            this.appointmentViewLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.appointmentViewLabel.Size = new System.Drawing.Size(32, 15);
            this.appointmentViewLabel.TabIndex = 16;
            this.appointmentViewLabel.Text = "View";
            this.appointmentViewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.appointmentViewLabel.Visible = false;
            // 
            // appointmentViewComboBox
            // 
            this.appointmentViewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.appointmentViewComboBox.FormattingEnabled = true;
            this.appointmentViewComboBox.Location = new System.Drawing.Point(120, 223);
            this.appointmentViewComboBox.Name = "appointmentViewComboBox";
            this.appointmentViewComboBox.Size = new System.Drawing.Size(130, 23);
            this.appointmentViewComboBox.TabIndex = 17;
            this.appointmentViewComboBox.Visible = false;
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(77, 170);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 10;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeBeforeTextBox
            // 
            this.timeBeforeTextBox.Location = new System.Drawing.Point(120, 80);
            this.timeBeforeTextBox.Name = "timeBeforeTextBox";
            this.timeBeforeTextBox.Size = new System.Drawing.Size(60, 23);
            this.timeBeforeTextBox.TabIndex = 5;
            // 
            // blockoutLabel
            // 
            this.blockoutLabel.AutoSize = true;
            this.blockoutLabel.Location = new System.Drawing.Point(60, 141);
            this.blockoutLabel.Name = "blockoutLabel";
            this.blockoutLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.blockoutLabel.Size = new System.Drawing.Size(54, 15);
            this.blockoutLabel.TabIndex = 8;
            this.blockoutLabel.Text = "Blockout";
            this.blockoutLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateToLabel
            // 
            this.dateToLabel.AutoSize = true;
            this.dateToLabel.Location = new System.Drawing.Point(95, 55);
            this.dateToLabel.Name = "dateToLabel";
            this.dateToLabel.Size = new System.Drawing.Size(19, 15);
            this.dateToLabel.TabIndex = 2;
            this.dateToLabel.Text = "To";
            this.dateToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateFromDateTimePicker
            // 
            this.dateFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFromDateTimePicker.Location = new System.Drawing.Point(120, 22);
            this.dateFromDateTimePicker.Name = "dateFromDateTimePicker";
            this.dateFromDateTimePicker.Size = new System.Drawing.Size(130, 23);
            this.dateFromDateTimePicker.TabIndex = 1;
            // 
            // dateFromLabel
            // 
            this.dateFromLabel.AutoSize = true;
            this.dateFromLabel.Location = new System.Drawing.Point(79, 26);
            this.dateFromLabel.Name = "dateFromLabel";
            this.dateFromLabel.Size = new System.Drawing.Size(35, 15);
            this.dateFromLabel.TabIndex = 0;
            this.dateFromLabel.Text = "From";
            this.dateFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // quickSearchGroupBox
            // 
            this.quickSearchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quickSearchGroupBox.Controls.Add(this.providersHygienistButton);
            this.quickSearchGroupBox.Controls.Add(this.providersDentistButton);
            this.quickSearchGroupBox.Location = new System.Drawing.Point(13, 442);
            this.quickSearchGroupBox.Name = "quickSearchGroupBox";
            this.quickSearchGroupBox.Size = new System.Drawing.Size(244, 66);
            this.quickSearchGroupBox.TabIndex = 2;
            this.quickSearchGroupBox.TabStop = false;
            this.quickSearchGroupBox.Text = "Quick Search";
            // 
            // providersHygienistButton
            // 
            this.providersHygienistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.providersHygienistButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.providersHygienistButton.Location = new System.Drawing.Point(124, 24);
            this.providersHygienistButton.Name = "providersHygienistButton";
            this.providersHygienistButton.Size = new System.Drawing.Size(110, 30);
            this.providersHygienistButton.TabIndex = 1;
            this.providersHygienistButton.Text = "Hygienists";
            this.providersHygienistButton.Click += new System.EventHandler(this.ProvidersHygienistButton_Click);
            // 
            // providersDentistButton
            // 
            this.providersDentistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.providersDentistButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.providersDentistButton.Location = new System.Drawing.Point(8, 24);
            this.providersDentistButton.Name = "providersDentistButton";
            this.providersDentistButton.Size = new System.Drawing.Size(110, 30);
            this.providersDentistButton.TabIndex = 0;
            this.providersDentistButton.Text = "Providers";
            this.providersDentistButton.Click += new System.EventHandler(this.ProvidersDentistButton_Click);
            // 
            // FormAppointmentSearchAdvanced
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(704, 521);
            this.Controls.Add(this.quickSearchGroupBox);
            this.Controls.Add(this.filtersGroupBox);
            this.Controls.Add(this.moreButton);
            this.Controls.Add(this.resultsGrid);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "FormAppointmentSearchAdvanced";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Appointment Search";
            this.Load += new System.EventHandler(this.FormAppointmentSearchAdvanced_Load);
            this.filtersGroupBox.ResumeLayout(false);
            this.filtersGroupBox.PerformLayout();
            this.quickSearchGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid resultsGrid;
        private System.Windows.Forms.ComboBox blockoutComboBox;
        private System.Windows.Forms.Button moreButton;
        private System.Windows.Forms.GroupBox filtersGroupBox;
        private System.Windows.Forms.DateTimePicker dateToDateTimePicker;
        private System.Windows.Forms.TextBox timeAfterTextBox;
        private System.Windows.Forms.Label timeAfterLabel;
        private System.Windows.Forms.TextBox timeBeforeTextBox;
        private System.Windows.Forms.Label dateToLabel;
        private System.Windows.Forms.DateTimePicker dateFromDateTimePicker;
        private System.Windows.Forms.Label dateFromLabel;
        private System.Windows.Forms.Label timeBeforeLabel;
        private System.Windows.Forms.Label blockoutLabel;
        private System.Windows.Forms.Button providersBrowseButton;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.ComboBox appointmentViewComboBox;
        private System.Windows.Forms.Label appointmentViewLabel;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Button clinicBrowseButton;
        private System.Windows.Forms.GroupBox quickSearchGroupBox;
        private System.Windows.Forms.Button providersHygienistButton;
        private System.Windows.Forms.Button providersDentistButton;
        private System.Windows.Forms.Label providersLabel;
        private UI.ComboBoxMulti providersComboBox;
    }
}