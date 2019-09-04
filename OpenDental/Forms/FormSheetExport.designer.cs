namespace OpenDental{
	partial class FormSheetExport {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetExport));
			this.butExport = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.gridCustomSheet = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Location = new System.Drawing.Point(405, 307);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 24);
			this.butExport.TabIndex = 2;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(405, 602);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridCustomSheet
			// 
			this.gridCustomSheet.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCustomSheet.HasAddButton = false;
			this.gridCustomSheet.HasDropDowns = false;
			this.gridCustomSheet.HasMultilineHeaders = false;
			this.gridCustomSheet.HScrollVisible = false;
			this.gridCustomSheet.Location = new System.Drawing.Point(12, 43);
			this.gridCustomSheet.Name = "gridCustomSheet";
			this.gridCustomSheet.ScrollValue = 0;
			this.gridCustomSheet.Size = new System.Drawing.Size(376, 583);
			this.gridCustomSheet.TabIndex = 1;
			this.gridCustomSheet.Title = "Custom Sheet";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(376, 31);
			this.label1.TabIndex = 29;
			this.label1.Text = "Select a sheet to export to an xml file, then click the Export button.";
			// 
			// FormSheetExport
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(492, 638);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridCustomSheet);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetExport";
			this.Text = "Sheet Export";
			this.Load += new System.EventHandler(this.FormSheetExport_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butExport;
		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridCustomSheet;
		private System.Windows.Forms.Label label1;
	}
}