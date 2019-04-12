namespace OpenDental{
	partial class FormApptSearchAdvanced {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptSearchAdvanced));
			this.butSearch = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboBlockout = new System.Windows.Forms.ComboBox();
			this.butMore = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.dateSearchTo = new System.Windows.Forms.DateTimePicker();
			this.butProviders = new OpenDental.UI.Button();
			this.butClinicMore = new OpenDental.UI.Button();
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butProvHygenist = new OpenDental.UI.Button();
			this.butProvDentist = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(196, 298);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 24);
			this.butSearch.TabIndex = 3;
			this.butSearch.Text = "&Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(490, 298);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(259, 251);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "Search Results";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableApptSearchResults";
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// comboBlockout
			// 
			this.comboBlockout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBlockout.FormattingEnabled = true;
			this.comboBlockout.Location = new System.Drawing.Point(105, 152);
			this.comboBlockout.Name = "comboBlockout";
			this.comboBlockout.Size = new System.Drawing.Size(130, 21);
			this.comboBlockout.TabIndex = 263;
			// 
			// butMore
			// 
			this.butMore.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMore.Autosize = true;
			this.butMore.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMore.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMore.CornerRadius = 4F;
			this.butMore.Enabled = false;
			this.butMore.Image = ((System.Drawing.Image)(resources.GetObject("butMore.Image")));
			this.butMore.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butMore.Location = new System.Drawing.Point(196, 268);
			this.butMore.Name = "butMore";
			this.butMore.Size = new System.Drawing.Size(75, 24);
			this.butMore.TabIndex = 268;
			this.butMore.Text = "More";
			this.butMore.Click += new System.EventHandler(this.butMore_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.comboBoxMultiProv);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.dateSearchTo);
			this.groupBox2.Controls.Add(this.butProviders);
			this.groupBox2.Controls.Add(this.butClinicMore);
			this.groupBox2.Controls.Add(this.textAfter);
			this.groupBox2.Controls.Add(this.comboBoxClinic);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.labelAptViews);
			this.groupBox2.Controls.Add(this.radioBeforePM);
			this.groupBox2.Controls.Add(this.comboApptView);
			this.groupBox2.Controls.Add(this.radioBeforeAM);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.textBefore);
			this.groupBox2.Controls.Add(this.labelBlockout);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Controls.Add(this.dateSearchFrom);
			this.groupBox2.Controls.Add(this.comboBlockout);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(287, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(278, 251);
			this.groupBox2.TabIndex = 269;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filters";
			// 
			// comboBoxMultiProv
			// 
			this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
			this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
			this.comboBoxMultiProv.Location = new System.Drawing.Point(105, 125);
			this.comboBoxMultiProv.Name = "comboBoxMultiProv";
			this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
			this.comboBoxMultiProv.Size = new System.Drawing.Size(130, 21);
			this.comboBoxMultiProv.TabIndex = 284;
			this.comboBoxMultiProv.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiProv_SelectionChangeCommitted);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 125);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(97, 16);
			this.label2.TabIndex = 283;
			this.label2.Text = "Providers";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 73);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(95, 16);
			this.label1.TabIndex = 92;
			this.label1.Text = "Starting before";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dateSearchTo
			// 
			this.dateSearchTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateSearchTo.Location = new System.Drawing.Point(105, 46);
			this.dateSearchTo.Name = "dateSearchTo";
			this.dateSearchTo.Size = new System.Drawing.Size(130, 20);
			this.dateSearchTo.TabIndex = 91;
			// 
			// butProviders
			// 
			this.butProviders.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProviders.Autosize = true;
			this.butProviders.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProviders.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProviders.CornerRadius = 4F;
			this.butProviders.Location = new System.Drawing.Point(241, 125);
			this.butProviders.Name = "butProviders";
			this.butProviders.Size = new System.Drawing.Size(20, 21);
			this.butProviders.TabIndex = 272;
			this.butProviders.Text = "...";
			this.butProviders.Click += new System.EventHandler(this.butProviders_Click);
			// 
			// butClinicMore
			// 
			this.butClinicMore.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClinicMore.Autosize = true;
			this.butClinicMore.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClinicMore.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClinicMore.CornerRadius = 4F;
			this.butClinicMore.Location = new System.Drawing.Point(241, 179);
			this.butClinicMore.Name = "butClinicMore";
			this.butClinicMore.Size = new System.Drawing.Size(20, 21);
			this.butClinicMore.TabIndex = 280;
			this.butClinicMore.Text = "...";
			this.butClinicMore.Visible = false;
			this.butClinicMore.Click += new System.EventHandler(this.butClinicMore_Click);
			// 
			// textAfter
			// 
			this.textAfter.Location = new System.Drawing.Point(105, 98);
			this.textAfter.Name = "textAfter";
			this.textAfter.Size = new System.Drawing.Size(44, 20);
			this.textAfter.TabIndex = 88;
			// 
			// comboBoxClinic
			// 
			this.comboBoxClinic.DoIncludeUnassigned = true;
			this.comboBoxClinic.FormattingEnabled = true;
			this.comboBoxClinic.Location = new System.Drawing.Point(105, 179);
			this.comboBoxClinic.Name = "comboBoxClinic";
			this.comboBoxClinic.Size = new System.Drawing.Size(130, 21);
			this.comboBoxClinic.TabIndex = 279;
			this.comboBoxClinic.SelectionChangeCommitted += new System.EventHandler(this.comboBoxClinic_SelectionChangeCommitted);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(9, 98);
			this.label11.Name = "label11";
			this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label11.Size = new System.Drawing.Size(95, 16);
			this.label11.TabIndex = 87;
			this.label11.Text = "Starting after";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelAptViews
			// 
			this.labelAptViews.Location = new System.Drawing.Point(1, 208);
			this.labelAptViews.Name = "labelAptViews";
			this.labelAptViews.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelAptViews.Size = new System.Drawing.Size(102, 20);
			this.labelAptViews.TabIndex = 278;
			this.labelAptViews.Text = "Appt View";
			this.labelAptViews.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelAptViews.Visible = false;
			// 
			// radioBeforePM
			// 
			this.radioBeforePM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioBeforePM.Location = new System.Drawing.Point(200, 75);
			this.radioBeforePM.Name = "radioBeforePM";
			this.radioBeforePM.Size = new System.Drawing.Size(37, 16);
			this.radioBeforePM.TabIndex = 86;
			this.radioBeforePM.Text = "pm";
			// 
			// comboApptView
			// 
			this.comboApptView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboApptView.FormattingEnabled = true;
			this.comboApptView.Location = new System.Drawing.Point(105, 206);
			this.comboApptView.Name = "comboApptView";
			this.comboApptView.Size = new System.Drawing.Size(130, 21);
			this.comboApptView.TabIndex = 277;
			this.comboApptView.Visible = false;
			// 
			// radioBeforeAM
			// 
			this.radioBeforeAM.Checked = true;
			this.radioBeforeAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioBeforeAM.Location = new System.Drawing.Point(158, 75);
			this.radioBeforeAM.Name = "radioBeforeAM";
			this.radioBeforeAM.Size = new System.Drawing.Size(37, 16);
			this.radioBeforeAM.TabIndex = 85;
			this.radioBeforeAM.TabStop = true;
			this.radioBeforeAM.Text = "am";
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(7, 181);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelClinic.Size = new System.Drawing.Size(97, 16);
			this.labelClinic.TabIndex = 275;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// textBefore
			// 
			this.textBefore.Location = new System.Drawing.Point(105, 72);
			this.textBefore.Name = "textBefore";
			this.textBefore.Size = new System.Drawing.Size(44, 20);
			this.textBefore.TabIndex = 84;
			// 
			// labelBlockout
			// 
			this.labelBlockout.Location = new System.Drawing.Point(3, 153);
			this.labelBlockout.Name = "labelBlockout";
			this.labelBlockout.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelBlockout.Size = new System.Drawing.Size(99, 19);
			this.labelBlockout.TabIndex = 271;
			this.labelBlockout.Text = "Blockout";
			this.labelBlockout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(30, 48);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72, 16);
			this.label10.TabIndex = 83;
			this.label10.Text = "To";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.radioAfterAM);
			this.panel1.Controls.Add(this.radioAfterPM);
			this.panel1.Location = new System.Drawing.Point(155, 99);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(84, 20);
			this.panel1.TabIndex = 86;
			// 
			// radioAfterAM
			// 
			this.radioAfterAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioAfterAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAfterAM.Location = new System.Drawing.Point(3, 2);
			this.radioAfterAM.Name = "radioAfterAM";
			this.radioAfterAM.Size = new System.Drawing.Size(37, 15);
			this.radioAfterAM.TabIndex = 89;
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
			this.radioAfterPM.TabIndex = 90;
			this.radioAfterPM.TabStop = true;
			this.radioAfterPM.Text = "pm";
			// 
			// dateSearchFrom
			// 
			this.dateSearchFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateSearchFrom.Location = new System.Drawing.Point(105, 20);
			this.dateSearchFrom.Name = "dateSearchFrom";
			this.dateSearchFrom.Size = new System.Drawing.Size(130, 20);
			this.dateSearchFrom.TabIndex = 90;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(9, 22);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 16);
			this.label9.TabIndex = 89;
			this.label9.Text = "From";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.butProvHygenist);
			this.groupBox1.Controls.Add(this.butProvDentist);
			this.groupBox1.Location = new System.Drawing.Point(12, 280);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(165, 48);
			this.groupBox1.TabIndex = 281;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Quick Search";
			// 
			// butProvHygenist
			// 
			this.butProvHygenist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvHygenist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvHygenist.Autosize = true;
			this.butProvHygenist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvHygenist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvHygenist.CornerRadius = 4F;
			this.butProvHygenist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProvHygenist.Location = new System.Drawing.Point(84, 18);
			this.butProvHygenist.Name = "butProvHygenist";
			this.butProvHygenist.Size = new System.Drawing.Size(75, 24);
			this.butProvHygenist.TabIndex = 92;
			this.butProvHygenist.Text = "Hygienists";
			this.butProvHygenist.Click += new System.EventHandler(this.butProvHygenist_Click);
			// 
			// butProvDentist
			// 
			this.butProvDentist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvDentist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvDentist.Autosize = true;
			this.butProvDentist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvDentist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvDentist.CornerRadius = 4F;
			this.butProvDentist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProvDentist.Location = new System.Drawing.Point(6, 18);
			this.butProvDentist.Name = "butProvDentist";
			this.butProvDentist.Size = new System.Drawing.Size(75, 24);
			this.butProvDentist.TabIndex = 91;
			this.butProvDentist.Text = "Providers";
			this.butProvDentist.Click += new System.EventHandler(this.butProvDentist_Click);
			// 
			// FormApptSearchAdvanced
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(577, 334);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butMore);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(593, 373);
			this.Name = "FormApptSearchAdvanced";
			this.Text = "Advanced Appointment Search";
			this.Load += new System.EventHandler(this.FormApptSearchAdvanced_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butSearch;
		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.ComboBox comboBlockout;
		private UI.Button butMore;
		private System.Windows.Forms.GroupBox groupBox2;
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
		private UI.Button butProviders;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboApptView;
		private System.Windows.Forms.Label labelAptViews;
		private UI.ComboBoxClinic comboBoxClinic;
		private UI.Button butClinicMore;
		private System.Windows.Forms.GroupBox groupBox1;
		private UI.Button butProvHygenist;
		private UI.Button butProvDentist;
		private System.Windows.Forms.Label label2;
		private UI.ComboBoxMulti comboBoxMultiProv;
	}
}