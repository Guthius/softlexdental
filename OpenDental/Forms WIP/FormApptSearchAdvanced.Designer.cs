namespace OpenDental
{
    partial class FormApptSearchAdvanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptSearchAdvanced));
            this.searchButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resultsGrid = new OpenDental.UI.ODGrid();
            this.comboBlockout = new System.Windows.Forms.ComboBox();
            this.moreButton = new System.Windows.Forms.Button();
            this.filtersGroupBox = new System.Windows.Forms.GroupBox();
            this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateSearchTo = new System.Windows.Forms.DateTimePicker();
            this.browseProviderButton = new System.Windows.Forms.Button();
            this.browseClinicButton = new System.Windows.Forms.Button();
            this.textAfter = new System.Windows.Forms.TextBox();
            this.comboBoxClinic = new OpenDental.UI.ComboBoxClinic();
            this.label11 = new System.Windows.Forms.Label();
            this.labelAptViews = new System.Windows.Forms.Label();
            this.radioBeforePM = new System.Windows.Forms.RadioButton();
            this.comboApptView = new System.Windows.Forms.ComboBox();
            this.radioBeforeAM = new System.Windows.Forms.RadioButton();
            this.labelClinic = new System.Windows.Forms.Label();
            this.textBefore = new System.Windows.Forms.TextBox();
            this.labelBlockout = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioAfterAM = new System.Windows.Forms.RadioButton();
            this.radioAfterPM = new System.Windows.Forms.RadioButton();
            this.dateSearchFrom = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.quickSearchGroupBox = new System.Windows.Forms.GroupBox();
            this.butProvHygenist = new System.Windows.Forms.Button();
            this.butProvDentist = new System.Windows.Forms.Button();
            this.filtersGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // comboBlockout
            // 
            this.comboBlockout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBlockout.FormattingEnabled = true;
            this.comboBlockout.Location = new System.Drawing.Point(120, 165);
            this.comboBlockout.Name = "comboBlockout";
            this.comboBlockout.Size = new System.Drawing.Size(130, 23);
            this.comboBlockout.TabIndex = 15;
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
            this.filtersGroupBox.Controls.Add(this.comboBoxMultiProv);
            this.filtersGroupBox.Controls.Add(this.label2);
            this.filtersGroupBox.Controls.Add(this.label1);
            this.filtersGroupBox.Controls.Add(this.dateSearchTo);
            this.filtersGroupBox.Controls.Add(this.browseProviderButton);
            this.filtersGroupBox.Controls.Add(this.browseClinicButton);
            this.filtersGroupBox.Controls.Add(this.textAfter);
            this.filtersGroupBox.Controls.Add(this.comboBoxClinic);
            this.filtersGroupBox.Controls.Add(this.label11);
            this.filtersGroupBox.Controls.Add(this.labelAptViews);
            this.filtersGroupBox.Controls.Add(this.radioBeforePM);
            this.filtersGroupBox.Controls.Add(this.comboApptView);
            this.filtersGroupBox.Controls.Add(this.radioBeforeAM);
            this.filtersGroupBox.Controls.Add(this.labelClinic);
            this.filtersGroupBox.Controls.Add(this.textBefore);
            this.filtersGroupBox.Controls.Add(this.labelBlockout);
            this.filtersGroupBox.Controls.Add(this.label10);
            this.filtersGroupBox.Controls.Add(this.panel1);
            this.filtersGroupBox.Controls.Add(this.dateSearchFrom);
            this.filtersGroupBox.Controls.Add(this.comboBlockout);
            this.filtersGroupBox.Controls.Add(this.label9);
            this.filtersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filtersGroupBox.Location = new System.Drawing.Point(391, 19);
            this.filtersGroupBox.Name = "filtersGroupBox";
            this.filtersGroupBox.Size = new System.Drawing.Size(300, 300);
            this.filtersGroupBox.TabIndex = 1;
            this.filtersGroupBox.TabStop = false;
            this.filtersGroupBox.Text = "Filters";
            // 
            // comboBoxMultiProv
            // 
            this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
            this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
            this.comboBoxMultiProv.Location = new System.Drawing.Point(120, 138);
            this.comboBoxMultiProv.Name = "comboBoxMultiProv";
            this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
            this.comboBoxMultiProv.Size = new System.Drawing.Size(130, 21);
            this.comboBoxMultiProv.TabIndex = 12;
            this.comboBoxMultiProv.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiProv_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 139);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Providers";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 83);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Starting before";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateSearchTo
            // 
            this.dateSearchTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateSearchTo.Location = new System.Drawing.Point(120, 51);
            this.dateSearchTo.Name = "dateSearchTo";
            this.dateSearchTo.Size = new System.Drawing.Size(130, 23);
            this.dateSearchTo.TabIndex = 3;
            // 
            // browseProviderButton
            // 
            this.browseProviderButton.Location = new System.Drawing.Point(256, 137);
            this.browseProviderButton.Name = "browseProviderButton";
            this.browseProviderButton.Size = new System.Drawing.Size(30, 25);
            this.browseProviderButton.TabIndex = 13;
            this.browseProviderButton.Text = "...";
            this.browseProviderButton.Click += new System.EventHandler(this.butProviders_Click);
            // 
            // browseClinicButton
            // 
            this.browseClinicButton.Location = new System.Drawing.Point(256, 193);
            this.browseClinicButton.Name = "browseClinicButton";
            this.browseClinicButton.Size = new System.Drawing.Size(30, 25);
            this.browseClinicButton.TabIndex = 18;
            this.browseClinicButton.Text = "...";
            this.browseClinicButton.Visible = false;
            this.browseClinicButton.Click += new System.EventHandler(this.BrowseClinicButton_Click);
            // 
            // textAfter
            // 
            this.textAfter.Location = new System.Drawing.Point(120, 109);
            this.textAfter.Name = "textAfter";
            this.textAfter.Size = new System.Drawing.Size(44, 23);
            this.textAfter.TabIndex = 9;
            // 
            // comboBoxClinic
            // 
            this.comboBoxClinic.DoIncludeUnassigned = true;
            this.comboBoxClinic.FormattingEnabled = true;
            this.comboBoxClinic.Location = new System.Drawing.Point(120, 194);
            this.comboBoxClinic.Name = "comboBoxClinic";
            this.comboBoxClinic.Size = new System.Drawing.Size(130, 23);
            this.comboBoxClinic.TabIndex = 17;
            this.comboBoxClinic.SelectionChangeCommitted += new System.EventHandler(this.comboBoxClinic_SelectionChangeCommitted);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(39, 111);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(75, 15);
            this.label11.TabIndex = 8;
            this.label11.Text = "Starting after";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAptViews
            // 
            this.labelAptViews.AutoSize = true;
            this.labelAptViews.Location = new System.Drawing.Point(53, 226);
            this.labelAptViews.Name = "labelAptViews";
            this.labelAptViews.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAptViews.Size = new System.Drawing.Size(61, 15);
            this.labelAptViews.TabIndex = 19;
            this.labelAptViews.Text = "Appt View";
            this.labelAptViews.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAptViews.Visible = false;
            // 
            // radioBeforePM
            // 
            this.radioBeforePM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioBeforePM.Location = new System.Drawing.Point(215, 83);
            this.radioBeforePM.Name = "radioBeforePM";
            this.radioBeforePM.Size = new System.Drawing.Size(37, 16);
            this.radioBeforePM.TabIndex = 7;
            this.radioBeforePM.Text = "pm";
            // 
            // comboApptView
            // 
            this.comboApptView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboApptView.FormattingEnabled = true;
            this.comboApptView.Location = new System.Drawing.Point(120, 223);
            this.comboApptView.Name = "comboApptView";
            this.comboApptView.Size = new System.Drawing.Size(130, 23);
            this.comboApptView.TabIndex = 20;
            this.comboApptView.Visible = false;
            // 
            // radioBeforeAM
            // 
            this.radioBeforeAM.Checked = true;
            this.radioBeforeAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioBeforeAM.Location = new System.Drawing.Point(173, 83);
            this.radioBeforeAM.Name = "radioBeforeAM";
            this.radioBeforeAM.Size = new System.Drawing.Size(37, 16);
            this.radioBeforeAM.TabIndex = 6;
            this.radioBeforeAM.TabStop = true;
            this.radioBeforeAM.Text = "am";
            // 
            // labelClinic
            // 
            this.labelClinic.AutoSize = true;
            this.labelClinic.Location = new System.Drawing.Point(77, 197);
            this.labelClinic.Name = "labelClinic";
            this.labelClinic.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelClinic.Size = new System.Drawing.Size(37, 15);
            this.labelClinic.TabIndex = 16;
            this.labelClinic.Text = "Clinic";
            this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelClinic.Visible = false;
            // 
            // textBefore
            // 
            this.textBefore.Location = new System.Drawing.Point(120, 80);
            this.textBefore.Name = "textBefore";
            this.textBefore.Size = new System.Drawing.Size(44, 23);
            this.textBefore.TabIndex = 5;
            // 
            // labelBlockout
            // 
            this.labelBlockout.AutoSize = true;
            this.labelBlockout.Location = new System.Drawing.Point(60, 168);
            this.labelBlockout.Name = "labelBlockout";
            this.labelBlockout.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelBlockout.Size = new System.Drawing.Size(54, 15);
            this.labelBlockout.TabIndex = 14;
            this.labelBlockout.Text = "Blockout";
            this.labelBlockout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(95, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 15);
            this.label10.TabIndex = 2;
            this.label10.Text = "To";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioAfterAM);
            this.panel1.Controls.Add(this.radioAfterPM);
            this.panel1.Location = new System.Drawing.Point(170, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(84, 20);
            this.panel1.TabIndex = 10;
            // 
            // radioAfterAM
            // 
            this.radioAfterAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioAfterAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioAfterAM.Location = new System.Drawing.Point(3, 2);
            this.radioAfterAM.Name = "radioAfterAM";
            this.radioAfterAM.Size = new System.Drawing.Size(37, 15);
            this.radioAfterAM.TabIndex = 0;
            this.radioAfterAM.Text = "am";
            // 
            // radioAfterPM
            // 
            this.radioAfterPM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioAfterPM.Checked = true;
            this.radioAfterPM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioAfterPM.Location = new System.Drawing.Point(46, 3);
            this.radioAfterPM.Name = "radioAfterPM";
            this.radioAfterPM.Size = new System.Drawing.Size(36, 15);
            this.radioAfterPM.TabIndex = 1;
            this.radioAfterPM.TabStop = true;
            this.radioAfterPM.Text = "pm";
            // 
            // dateSearchFrom
            // 
            this.dateSearchFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateSearchFrom.Location = new System.Drawing.Point(120, 22);
            this.dateSearchFrom.Name = "dateSearchFrom";
            this.dateSearchFrom.Size = new System.Drawing.Size(130, 23);
            this.dateSearchFrom.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(79, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "From";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // quickSearchGroupBox
            // 
            this.quickSearchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quickSearchGroupBox.Controls.Add(this.butProvHygenist);
            this.quickSearchGroupBox.Controls.Add(this.butProvDentist);
            this.quickSearchGroupBox.Location = new System.Drawing.Point(13, 442);
            this.quickSearchGroupBox.Name = "quickSearchGroupBox";
            this.quickSearchGroupBox.Size = new System.Drawing.Size(244, 66);
            this.quickSearchGroupBox.TabIndex = 2;
            this.quickSearchGroupBox.TabStop = false;
            this.quickSearchGroupBox.Text = "Quick Search";
            // 
            // butProvHygenist
            // 
            this.butProvHygenist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butProvHygenist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butProvHygenist.Location = new System.Drawing.Point(124, 24);
            this.butProvHygenist.Name = "butProvHygenist";
            this.butProvHygenist.Size = new System.Drawing.Size(110, 30);
            this.butProvHygenist.TabIndex = 1;
            this.butProvHygenist.Text = "Hygienists";
            this.butProvHygenist.Click += new System.EventHandler(this.butProvHygenist_Click);
            // 
            // butProvDentist
            // 
            this.butProvDentist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butProvDentist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butProvDentist.Location = new System.Drawing.Point(8, 24);
            this.butProvDentist.Name = "butProvDentist";
            this.butProvDentist.Size = new System.Drawing.Size(110, 30);
            this.butProvDentist.TabIndex = 0;
            this.butProvDentist.Text = "Providers";
            this.butProvDentist.Click += new System.EventHandler(this.butProvDentist_Click);
            // 
            // FormApptSearchAdvanced
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
            this.Name = "FormApptSearchAdvanced";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Appointment Search";
            this.Load += new System.EventHandler(this.FormApptSearchAdvanced_Load);
            this.filtersGroupBox.ResumeLayout(false);
            this.filtersGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.quickSearchGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid resultsGrid;
        private System.Windows.Forms.ComboBox comboBlockout;
        private System.Windows.Forms.Button moreButton;
        private System.Windows.Forms.GroupBox filtersGroupBox;
        private System.Windows.Forms.DateTimePicker dateSearchTo;
        private System.Windows.Forms.TextBox textAfter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton radioBeforePM;
        private System.Windows.Forms.RadioButton radioBeforeAM;
        private System.Windows.Forms.TextBox textBefore;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioAfterAM;
        private System.Windows.Forms.RadioButton radioAfterPM;
        private System.Windows.Forms.DateTimePicker dateSearchFrom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelBlockout;
        private System.Windows.Forms.Button browseProviderButton;
        private System.Windows.Forms.Label labelClinic;
        private System.Windows.Forms.ComboBox comboApptView;
        private System.Windows.Forms.Label labelAptViews;
        private UI.ComboBoxClinic comboBoxClinic;
        private System.Windows.Forms.Button browseClinicButton;
        private System.Windows.Forms.GroupBox quickSearchGroupBox;
        private System.Windows.Forms.Button butProvHygenist;
        private System.Windows.Forms.Button butProvDentist;
        private System.Windows.Forms.Label label2;
        private UI.ComboBoxMulti comboBoxMultiProv;
    }
}