namespace OpenDental{
	partial class FormWiki {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWiki));
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.labelStatus = new System.Windows.Forms.Label();
            this.ToolBarMain = new OpenDental.UI.ODToolBar();
            this.wikiWebBrowser = new System.Windows.Forms.WebBrowser();
            this.menuHomeDropDown = new System.Windows.Forms.ContextMenu();
            this.menuItemHomePageSave = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "Left.gif");
            this.imageListMain.Images.SetKeyName(1, "Right.gif");
            this.imageListMain.Images.SetKeyName(2, "Manage22.gif");
            this.imageListMain.Images.SetKeyName(3, "home.gif");
            this.imageListMain.Images.SetKeyName(4, "editPencil.gif");
            this.imageListMain.Images.SetKeyName(5, "print.gif");
            this.imageListMain.Images.SetKeyName(6, "rename.gif");
            this.imageListMain.Images.SetKeyName(7, "deleteX.gif");
            this.imageListMain.Images.SetKeyName(8, "history.gif");
            this.imageListMain.Images.SetKeyName(9, "incoming.gif");
            this.imageListMain.Images.SetKeyName(10, "Add.gif");
            this.imageListMain.Images.SetKeyName(11, "allpages.gif");
            this.imageListMain.Images.SetKeyName(12, "search.gif");
            this.imageListMain.Images.SetKeyName(13, "WikiLists.png");
            this.imageListMain.Images.SetKeyName(14, "Drafts_2.png");
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.Location = new System.Drawing.Point(-3, 623);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(987, 18);
            this.labelStatus.TabIndex = 73;
            this.labelStatus.Text = "Status Bar";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToolBarMain
            // 
            this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
            this.ToolBarMain.Name = "ToolBarMain";
            this.ToolBarMain.Size = new System.Drawing.Size(984, 25);
            this.ToolBarMain.TabIndex = 72;
            this.ToolBarMain.ButtonClick += new System.EventHandler<UI.ODToolBarButtonClickEventArgs>(this.ToolBarMain_ButtonClick);
            // 
            // wikiWebBrowser
            // 
            this.wikiWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wikiWebBrowser.Location = new System.Drawing.Point(0, 28);
            this.wikiWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wikiWebBrowser.Name = "wikiWebBrowser";
            this.wikiWebBrowser.Size = new System.Drawing.Size(984, 594);
            this.wikiWebBrowser.TabIndex = 0;
            this.wikiWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserWiki_Navigating);
            // 
            // menuHomeDropDown
            // 
            this.menuHomeDropDown.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHomePageSave});
            // 
            // menuItemHomePageSave
            // 
            this.menuItemHomePageSave.Index = 0;
            this.menuItemHomePageSave.Text = "Save As Home Page";
            this.menuItemHomePageSave.Click += new System.EventHandler(this.menuItemHomePageSave_Click);
            // 
            // FormWiki
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.ToolBarMain);
            this.Controls.Add(this.wikiWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormWiki";
            this.Text = "Wiki";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWiki_FormClosing);
            this.Load += new System.EventHandler(this.FormWiki_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser wikiWebBrowser;
		private UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.ContextMenu menuHomeDropDown;
		private System.Windows.Forms.MenuItem menuItemHomePageSave;
	}
}