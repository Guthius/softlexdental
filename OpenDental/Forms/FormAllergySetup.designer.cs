namespace OpenDental{
	partial class FormAllergySetup {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAllergySetup));
            this.butOK = new System.Windows.Forms.Button();
            this.checkShowHidden = new System.Windows.Forms.CheckBox();
            this.gridMain = new OpenDental.UI.ODGrid();
            this.butAdd = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(273, 282);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(110, 30);
            this.butOK.TabIndex = 6;
            this.butOK.Text = "&OK";
            this.butOK.Visible = false;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // checkShowHidden
            // 
            this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkShowHidden.Location = new System.Drawing.Point(273, 72);
            this.checkShowHidden.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.checkShowHidden.Name = "checkShowHidden";
            this.checkShowHidden.Size = new System.Drawing.Size(110, 24);
            this.checkShowHidden.TabIndex = 5;
            this.checkShowHidden.TabStop = false;
            this.checkShowHidden.Text = "Show Hidden";
            this.checkShowHidden.UseVisualStyleBackColor = true;
            this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridMain.EditableEnterMovesDown = false;
            this.gridMain.HasAddButton = false;
            this.gridMain.HasDropDowns = false;
            this.gridMain.HasMultilineHeaders = false;
            this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridMain.HeaderHeight = 15;
            this.gridMain.HScrollVisible = false;
            this.gridMain.Location = new System.Drawing.Point(13, 19);
            this.gridMain.Name = "gridMain";
            this.gridMain.ScrollValue = 0;
            this.gridMain.Size = new System.Drawing.Size(254, 329);
            this.gridMain.TabIndex = 4;
            this.gridMain.Title = "Allergies";
            this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridMain.TitleHeight = 24;
            this.gridMain.TranslationName = "TableAllergies";
            this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
            // 
            // butAdd
            // 
            this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
            this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butAdd.Location = new System.Drawing.Point(273, 19);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(110, 30);
            this.butAdd.TabIndex = 3;
            this.butAdd.Text = "&Add";
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Location = new System.Drawing.Point(273, 318);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(110, 30);
            this.butClose.TabIndex = 2;
            this.butClose.Text = "&Close";
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // FormAllergySetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(396, 361);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.checkShowHidden);
            this.Controls.Add(this.gridMain);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.butClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergySetup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Allergy Setup";
            this.Load += new System.EventHandler(this.FormAllergySetup_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button butAdd;
		private System.Windows.Forms.Button butClose;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.CheckBox checkShowHidden;
		private System.Windows.Forms.Button butOK;
	}
}