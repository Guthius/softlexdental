namespace OpenDental{
	partial class FormRadOrderList {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRadOrderList));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.menuRightClick = new System.Windows.Forms.ContextMenu();
			this.menuItemSeeChart = new System.Windows.Forms.MenuItem();
			this.menuItemSeeFamily = new System.Windows.Forms.MenuItem();
			this.butSelected = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 42);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
			this.gridMain.Size = new System.Drawing.Size(852, 642);
			this.gridMain.TabIndex = 143;
			this.gridMain.Title = "Non-CPOE Radiology Orders";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(869, 30);
			this.label1.TabIndex = 145;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// menuRightClick
			// 
			this.menuRightClick.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSeeChart,
            this.menuItemSeeFamily});
			// 
			// menuItemSeeChart
			// 
			this.menuItemSeeChart.Index = 0;
			this.menuItemSeeChart.Text = "See Chart";
			this.menuItemSeeChart.Click += new System.EventHandler(this.butGotoChart_Click);
			// 
			// menuItemSeeFamily
			// 
			this.menuItemSeeFamily.Index = 1;
			this.menuItemSeeFamily.Text = "See Family";
			this.menuItemSeeFamily.Click += new System.EventHandler(this.butGotoFamily_Click);
			// 
			// butApprove
			// 
			this.butSelected.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelected.Autosize = true;
			this.butSelected.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butSelected.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butSelected.CornerRadius = 4F;
			this.butSelected.Location = new System.Drawing.Point(13, 24);
			this.butSelected.Name = "butApprove";
			this.butSelected.Size = new System.Drawing.Size(75, 24);
			this.butSelected.TabIndex = 146;
			this.butSelected.Text = "Selected";
			this.butSelected.Click += new System.EventHandler(this.butSelected_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(13, 59);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(75, 24);
			this.butAll.TabIndex = 144;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butSelected);
			this.groupBox1.Controls.Add(this.butAll);
			this.groupBox1.Location = new System.Drawing.Point(870, 302);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(99, 98);
			this.groupBox1.TabIndex = 147;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Approve";
			// 
			// FormRadOrderList
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormRadOrderList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Radiology Order List";
			this.Load += new System.EventHandler(this.FormProcRadLists_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMain;
		private UI.Button butAll;
		private System.Windows.Forms.Label label1;
		private UI.Button butSelected;
		private System.Windows.Forms.ContextMenu menuRightClick;
		private System.Windows.Forms.MenuItem menuItemSeeFamily;
		private System.Windows.Forms.MenuItem menuItemSeeChart;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}