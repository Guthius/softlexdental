﻿namespace OpenDental{
	partial class FormLayoutSheetDefs {
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
			this.butClose = new OpenDental.UI.Button();
			this.butDuplicate = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.gridOtherLayouts = new OpenDental.UI.ODGrid();
			this.gridCustomLayouts = new OpenDental.UI.ODGrid();
			this.butNew = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(781, 322);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(80, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butDuplicate
			// 
			this.butDuplicate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDuplicate.Autosize = true;
			this.butDuplicate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDuplicate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDuplicate.CornerRadius = 4F;
			this.butDuplicate.Image = global::OpenDental.Properties.Resources.Add;
			this.butDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDuplicate.Location = new System.Drawing.Point(580, 322);
			this.butDuplicate.Name = "butDuplicate";
			this.butDuplicate.Size = new System.Drawing.Size(89, 24);
			this.butDuplicate.TabIndex = 12;
			this.butDuplicate.Text = "Duplicate";
			this.butDuplicate.Click += new System.EventHandler(this.butDuplicate_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.Right;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(400, 152);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 10;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// gridOtherLayouts
			// 
			this.gridOtherLayouts.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOtherLayouts.EditableEnterMovesDown = false;
			this.gridOtherLayouts.HasAddButton = false;
			this.gridOtherLayouts.HasDropDowns = false;
			this.gridOtherLayouts.HasMultilineHeaders = false;
			this.gridOtherLayouts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOtherLayouts.HeaderHeight = 15;
			this.gridOtherLayouts.HScrollVisible = false;
			this.gridOtherLayouts.Location = new System.Drawing.Point(12, 12);
			this.gridOtherLayouts.Name = "gridOtherLayouts";
			this.gridOtherLayouts.ScrollValue = 0;
			this.gridOtherLayouts.Size = new System.Drawing.Size(370, 304);
			this.gridOtherLayouts.TabIndex = 8;
			this.gridOtherLayouts.Title = "Internal and Other User Layouts";
			this.gridOtherLayouts.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridOtherLayouts_CellDoubleClick);
			this.gridOtherLayouts.Click += new System.EventHandler(this.gridOtherLayouts_Click);
			// 
			// gridCustomLayouts
			// 
			this.gridCustomLayouts.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCustomLayouts.EditableEnterMovesDown = false;
			this.gridCustomLayouts.HasAddButton = false;
			this.gridCustomLayouts.HasDropDowns = false;
			this.gridCustomLayouts.HasMultilineHeaders = false;
			this.gridCustomLayouts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCustomLayouts.HeaderHeight = 15;
			this.gridCustomLayouts.HScrollVisible = false;
			this.gridCustomLayouts.Location = new System.Drawing.Point(494, 12);
			this.gridCustomLayouts.Name = "gridCustomLayouts";
			this.gridCustomLayouts.ScrollValue = 0;
			this.gridCustomLayouts.Size = new System.Drawing.Size(367, 304);
			this.gridCustomLayouts.TabIndex = 9;
			this.gridCustomLayouts.Title = "My Custom Layouts";
			this.gridCustomLayouts.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridCustomLayouts_CellDoubleClick);
			this.gridCustomLayouts.Click += new System.EventHandler(this.gridCustomLayouts_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Image = global::OpenDental.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(494, 322);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(80, 24);
			this.butNew.TabIndex = 11;
			this.butNew.Text = "New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// FormLayoutSheetDefs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(873, 352);
			this.ControlBox = false;
			this.Controls.Add(this.butDuplicate);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.gridOtherLayouts);
			this.Controls.Add(this.gridCustomLayouts);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.butClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximumSize = new System.Drawing.Size(889, 769);
			this.Name = "FormLayoutSheetDefs";
			this.Text = "Sheet Dynamic Layouts";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModuleSheetDefs_FormClosing);
			this.Load += new System.EventHandler(this.FormLayoutSheetDefs_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.Button butDuplicate;
		private UI.Button butCopy;
		private UI.ODGrid gridOtherLayouts;
		private UI.ODGrid gridCustomLayouts;
		private UI.Button butNew;
	}
}