namespace OpenDental{
	partial class FormTaskHist {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskHist));
			this.butOK = new OpenDental.UI.Button();
			this.gridTaskHist = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(749, 365);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "Close";
			this.butOK.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridTaskHist
			// 
			this.gridTaskHist.HasMultilineHeaders = false;
			this.gridTaskHist.HScrollVisible = false;
			this.gridTaskHist.Location = new System.Drawing.Point(12, 12);
			this.gridTaskHist.Name = "gridTaskHist";
			this.gridTaskHist.ScrollValue = 0;
			this.gridTaskHist.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
			this.gridTaskHist.Size = new System.Drawing.Size(731, 377);
			this.gridTaskHist.TabIndex = 5;
			this.gridTaskHist.Title = "History";
			// 
			// FormTaskHist
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(836, 416);
			this.Controls.Add(this.gridTaskHist);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormTaskHist";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Task History";
			this.Load += new System.EventHandler(this.FormTaskHist_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private UI.ODGrid gridTaskHist;
	}
}