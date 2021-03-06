﻿namespace OpenDental {
	partial class FormRpProcNotBilledIns {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpProcNotBilledIns));
			this.butClose = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.checkMedical = new System.Windows.Forms.CheckBox();
			this.imageListCalendar = new System.Windows.Forms.ImageList(this.components);
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butNewClaims = new OpenDental.UI.Button();
			this.contextMenuGrid=new System.Windows.Forms.ContextMenu();
			this.menuItemGoToAccount=new System.Windows.Forms.MenuItem();
			this.checkAutoGroupProcs = new System.Windows.Forms.CheckBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butSelectAll = new OpenDental.UI.Button();
			this.checkShowProcsNoIns = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkShowProcsInProcess = new System.Windows.Forms.CheckBox();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(917, 662);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 4;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(25, 662);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkMedical
			// 
			this.checkMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.Location = new System.Drawing.Point(678, 12);
			this.checkMedical.Name = "checkMedical";
			this.checkMedical.Size = new System.Drawing.Size(227, 17);
			this.checkMedical.TabIndex = 11;
			this.checkMedical.Text = "Include Medical Procedures";
			this.checkMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.UseVisualStyleBackColor = true;
			this.checkMedical.Visible = false;
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(461, 36);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(55, 16);
			this.labelClinic.TabIndex = 68;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.ArraySelectedIndices = new int[0];
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(517, 35);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 67;
			this.comboBoxMultiClinics.Visible = false;
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
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(25, 71);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
			this.gridMain.Size = new System.Drawing.Size(967, 588);
			this.gridMain.TabIndex = 69;
			this.gridMain.Title = "Procedures Not Billed";
			// 
			// butNewClaims
			// 
			this.butNewClaims.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewClaims.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butNewClaims.Autosize = true;
			this.butNewClaims.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butNewClaims.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butNewClaims.CornerRadius = 4F;
			this.butNewClaims.Location = new System.Drawing.Point(513, 662);
			this.butNewClaims.Name = "butNewClaims";
			this.butNewClaims.Size = new System.Drawing.Size(100, 26);
			this.butNewClaims.TabIndex = 71;
			this.butNewClaims.Text = "New Claims";
			this.butNewClaims.UseVisualStyleBackColor = true;
			this.butNewClaims.Click += new System.EventHandler(this.butNewClaims_Click);
			// 
			// contextMenuGrid
			// 
			this.contextMenuGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGoToAccount});
			// 
			// menuItemGoToAccount
			// 
			this.menuItemGoToAccount.Index = 0;
			this.menuItemGoToAccount.Text = "Go To Account";
			this.menuItemGoToAccount.Click += new System.EventHandler(this.menuItemGridGoToAccount_Click);
			// 
			// checkAutoGroupProcs
			// 
			this.checkAutoGroupProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.Location = new System.Drawing.Point(678, 33);
			this.checkAutoGroupProcs.Name = "checkAutoGroupProcs";
			this.checkAutoGroupProcs.Size = new System.Drawing.Size(227, 17);
			this.checkAutoGroupProcs.TabIndex = 72;
			this.checkAutoGroupProcs.Text = "Automatically Group Procedures";
			this.checkAutoGroupProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.UseVisualStyleBackColor = true;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(942, 38);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(50, 26);
			this.butRefresh.TabIndex = 73;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSelectAll
			// 
			this.butSelectAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butSelectAll.Autosize = true;
			this.butSelectAll.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butSelectAll.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butSelectAll.CornerRadius = 4F;
			this.butSelectAll.Location = new System.Drawing.Point(407, 662);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(100, 26);
			this.butSelectAll.TabIndex = 74;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.UseVisualStyleBackColor = true;
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// checkShowProcsNoIns
			// 
			this.checkShowProcsNoIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsNoIns.Location = new System.Drawing.Point(33, 12);
			this.checkShowProcsNoIns.Name = "checkShowProcsNoIns";
			this.checkShowProcsNoIns.Size = new System.Drawing.Size(371, 17);
			this.checkShowProcsNoIns.TabIndex = 75;
			this.checkShowProcsNoIns.Text = "Show Procedures Completed Before Insurance Added";
			this.checkShowProcsNoIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsNoIns.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkShowProcsInProcess);
			this.groupBox2.Controls.Add(this.checkShowProcsNoIns);
			this.groupBox2.Controls.Add(this.dateRangePicker);
			this.groupBox2.Controls.Add(this.checkMedical);
			this.groupBox2.Controls.Add(this.comboBoxMultiClinics);
			this.groupBox2.Controls.Add(this.checkAutoGroupProcs);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Location = new System.Drawing.Point(25, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(911, 61);
			this.groupBox2.TabIndex = 77;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filters";
			// 
			// checkShowProcsInProcess
			// 
			this.checkShowProcsInProcess.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsInProcess.Location = new System.Drawing.Point(448, 12);
			this.checkShowProcsInProcess.Name = "checkShowProcsInProcess";
			this.checkShowProcsInProcess.Size = new System.Drawing.Size(230, 17);
			this.checkShowProcsInProcess.TabIndex = 76;
			this.checkShowProcsInProcess.Text = "Show Procedures In Process";
			this.checkShowProcsInProcess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsInProcess.UseVisualStyleBackColor = true;
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2018, 1, 9, 0, 0, 0, 0);
			this.dateRangePicker.EnableWeekButtons = false;
			this.dateRangePicker.Location = new System.Drawing.Point(1, 34);
			this.dateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(453, 22);
			this.dateRangePicker.TabIndex = 0;
			// 
			// FormRpProcNotBilledIns
			// 
			this.AcceptButton = this.butPrint;
			this.ClientSize = new System.Drawing.Size(1019, 696);
			this.Controls.Add(this.butSelectAll);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butNewClaims);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1035, 734);
			this.Name = "FormRpProcNotBilledIns";
			this.Text = "Procedures Not Billed to Insurance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRpProcNotBilledIns_FormClosing);
			this.Load += new System.EventHandler(this.FormProcNotAttach_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butPrint;
		private UI.Button butNewClaims;
		private System.Windows.Forms.CheckBox checkMedical;
		private System.Windows.Forms.ContextMenu contextMenuGrid;
		private System.Windows.Forms.MenuItem menuItemGoToAccount;
		private System.Windows.Forms.Label labelClinic;
		private UI.ComboBoxMulti comboBoxMultiClinics;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.CheckBox checkAutoGroupProcs;
		private UI.Button butRefresh;
		private System.Windows.Forms.ImageList imageListCalendar;
		private UI.Button butSelectAll;
		private System.Windows.Forms.CheckBox checkShowProcsNoIns;
		private System.Windows.Forms.GroupBox groupBox2;
		private UI.ODDateRangePicker dateRangePicker;
		private System.Windows.Forms.CheckBox checkShowProcsInProcess;

	}
}
