namespace OpenDental{
	partial class FormEhrMeasureEvents {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrMeasureEvents));
			this.labelType = new System.Windows.Forms.Label();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.labelEndDate = new System.Windows.Forms.Label();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.butAuditTrail = new OpenDental.UI.Button();
			this.textDateStart = new OpenDental.ValidDate();
			this.textDateEnd = new OpenDental.ValidDate();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelType
			// 
			this.labelType.Location = new System.Drawing.Point(-9, 11);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(64, 18);
			this.labelType.TabIndex = 33;
			this.labelType.Text = "Type";
			this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.FormattingEnabled = true;
			this.comboType.Location = new System.Drawing.Point(61, 11);
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(156, 21);
			this.comboType.TabIndex = 6;
			this.comboType.SelectionChangeCommitted += new System.EventHandler(this.comboType_SelectionChangeCommitted);
			// 
			// labelEndDate
			// 
			this.labelEndDate.Location = new System.Drawing.Point(424, 12);
			this.labelEndDate.Name = "labelEndDate";
			this.labelEndDate.Size = new System.Drawing.Size(88, 18);
			this.labelEndDate.TabIndex = 31;
			this.labelEndDate.Text = "End Date";
			this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(232, 12);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(80, 18);
			this.labelStartDate.TabIndex = 29;
			this.labelStartDate.Text = "Start Date";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAuditTrail
			// 
			this.butAuditTrail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAuditTrail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAuditTrail.Autosize = true;
			this.butAuditTrail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAuditTrail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAuditTrail.CornerRadius = 4F;
			this.butAuditTrail.Location = new System.Drawing.Point(355, 405);
			this.butAuditTrail.Name = "butAuditTrail";
			this.butAuditTrail.Size = new System.Drawing.Size(75, 24);
			this.butAuditTrail.TabIndex = 4;
			this.butAuditTrail.Text = "Audit Trail";
			this.butAuditTrail.Click += new System.EventHandler(this.butAuditTrail_Click);
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(318, 12);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(100, 20);
			this.textDateStart.TabIndex = 1;
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(518, 12);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(100, 20);
			this.textDateEnd.TabIndex = 2;
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
			this.gridMain.Location = new System.Drawing.Point(12, 38);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(746, 361);
			this.gridMain.TabIndex = 7;
			this.gridMain.Title = "Measure Events";
			this.gridMain.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(683, 12);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 3;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(683, 405);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 5;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormEhrMeasureEvents
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 441);
			this.Controls.Add(this.butAuditTrail);
			this.Controls.Add(this.textDateStart);
			this.Controls.Add(this.textDateEnd);
			this.Controls.Add(this.labelEndDate);
			this.Controls.Add(this.labelType);
			this.Controls.Add(this.labelStartDate);
			this.Controls.Add(this.comboType);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(740, 435);
			this.Name = "FormEhrMeasureEvents";
			this.Text = "EHR Measure Events";
			this.Load += new System.EventHandler(this.FormEhrMeasureEvents_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butRefresh;
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.ComboBox comboType;
		private System.Windows.Forms.Label labelEndDate;
		private System.Windows.Forms.Label labelStartDate;
		private UI.ODGrid gridMain;
		private ValidDate textDateStart;
		private ValidDate textDateEnd;
		private UI.Button butAuditTrail;
	}
}