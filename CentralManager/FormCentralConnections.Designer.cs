﻿namespace CentralManager {
	partial class FormCentralConnections {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnections));
			this.LabelText = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.comboConnectionGroups = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupOrdering = new System.Windows.Forms.GroupBox();
			this.butAlphabetize = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupOrdering.SuspendLayout();
			this.SuspendLayout();
			// 
			// LabelText
			// 
			this.LabelText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelText.Location = new System.Drawing.Point(9, 424);
			this.LabelText.Name = "LabelText";
			this.LabelText.Size = new System.Drawing.Size(623, 36);
			this.LabelText.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(366, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 17);
			this.label1.TabIndex = 214;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(438, 7);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(190, 20);
			this.textSearch.TabIndex = 213;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// comboConnectionGroups
			// 
			this.comboConnectionGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConnectionGroups.FormattingEnabled = true;
			this.comboConnectionGroups.Location = new System.Drawing.Point(170, 6);
			this.comboConnectionGroups.MaxDropDownItems = 20;
			this.comboConnectionGroups.Name = "comboConnectionGroups";
			this.comboConnectionGroups.Size = new System.Drawing.Size(190, 21);
			this.comboConnectionGroups.TabIndex = 216;
			this.comboConnectionGroups.SelectionChangeCommitted += new System.EventHandler(this.comboConnectionGroups_SelectionChangeCommitted);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(12, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(155, 17);
			this.label3.TabIndex = 215;
			this.label3.Text = "Connection Groups";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupOrdering
			// 
			this.groupOrdering.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupOrdering.Controls.Add(this.butAlphabetize);
			this.groupOrdering.Controls.Add(this.butUp);
			this.groupOrdering.Controls.Add(this.butDown);
			this.groupOrdering.Location = new System.Drawing.Point(634, 98);
			this.groupOrdering.Name = "groupOrdering";
			this.groupOrdering.Size = new System.Drawing.Size(89, 122);
			this.groupOrdering.TabIndex = 219;
			this.groupOrdering.TabStop = false;
			this.groupOrdering.Text = "Conn. Order";
			// 
			// butAlphabetize
			// 
			this.butAlphabetize.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlphabetize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlphabetize.Autosize = true;
			this.butAlphabetize.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlphabetize.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlphabetize.CornerRadius = 4F;
			this.butAlphabetize.Location = new System.Drawing.Point(6, 90);
			this.butAlphabetize.Name = "butAlphabetize";
			this.butAlphabetize.Size = new System.Drawing.Size(77, 26);
			this.butAlphabetize.TabIndex = 16;
			this.butAlphabetize.Text = "Alphabetize";
			this.butAlphabetize.Click += new System.EventHandler(this.butAlphabetize_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = ((System.Drawing.Image)(resources.GetObject("butUp.Image")));
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(6, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(77, 26);
			this.butUp.TabIndex = 4;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = ((System.Drawing.Image)(resources.GetObject("butDown.Image")));
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(6, 51);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(77, 26);
			this.butDown.TabIndex = 5;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = ((System.Drawing.Image)(resources.GetObject("butAdd.Image")));
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(638, 66);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 26);
			this.butAdd.TabIndex = 218;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
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
			this.gridMain.Location = new System.Drawing.Point(12, 33);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(616, 387);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "Connections";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableConnections";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(642, 419);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(642, 448);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormCentralConnections
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(729, 483);
			this.Controls.Add(this.groupOrdering);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.comboConnectionGroups);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.LabelText);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(738, 521);
			this.Name = "FormCentralConnections";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Load += new System.EventHandler(this.FormCentralConnections_Load);
			this.groupOrdering.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.ODGrid gridMain;
		public System.Windows.Forms.Label LabelText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.ComboBox comboConnectionGroups;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.GroupBox groupOrdering;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butAlphabetize;
	}
}