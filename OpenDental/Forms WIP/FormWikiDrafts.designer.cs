namespace OpenDental {
	partial class FormWikiDrafts {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiDrafts));
            this.wikiPagesGrid = new OpenDental.UI.ODGrid();
            this.editButton = new System.Windows.Forms.Button();
            this.textContent = new OpenDental.TextBoxWiki();
            this.wikiWebBrowser = new System.Windows.Forms.WebBrowser();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wikiPagesGrid
            // 
            this.wikiPagesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.wikiPagesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.wikiPagesGrid.EditableEnterMovesDown = false;
            this.wikiPagesGrid.HasAddButton = false;
            this.wikiPagesGrid.HasDropDowns = false;
            this.wikiPagesGrid.HasMultilineHeaders = false;
            this.wikiPagesGrid.HScrollVisible = false;
            this.wikiPagesGrid.Location = new System.Drawing.Point(13, 19);
            this.wikiPagesGrid.Name = "wikiPagesGrid";
            this.wikiPagesGrid.ScrollValue = 0;
            this.wikiPagesGrid.Size = new System.Drawing.Size(250, 609);
            this.wikiPagesGrid.TabIndex = 0;
            this.wikiPagesGrid.Title = "Drafts";
            this.wikiPagesGrid.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.wikiPagesGrid_CellDoubleClick);
            this.wikiPagesGrid.Click += new System.EventHandler(this.wikiPagesGrid_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editButton.Location = new System.Drawing.Point(981, 19);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(110, 30);
            this.editButton.TabIndex = 4;
            this.editButton.Text = "&Edit";
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // textContent
            // 
            this.textContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textContent.ContextMenuStripWiki = null;
            this.textContent.Location = new System.Drawing.Point(269, 19);
            this.textContent.Name = "textContent";
            this.textContent.ReadOnly = true;
            this.textContent.SelectedText = "";
            this.textContent.SelectionLength = 0;
            this.textContent.SelectionStart = 0;
            this.textContent.Size = new System.Drawing.Size(347, 609);
            this.textContent.TabIndex = 2;
            // 
            // wikiWebBrowser
            // 
            this.wikiWebBrowser.AllowWebBrowserDrop = false;
            this.wikiWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wikiWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.wikiWebBrowser.Location = new System.Drawing.Point(647, 19);
            this.wikiWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wikiWebBrowser.Name = "wikiWebBrowser";
            this.wikiWebBrowser.Size = new System.Drawing.Size(328, 609);
            this.wikiWebBrowser.TabIndex = 3;
            this.wikiWebBrowser.WebBrowserShortcutsEnabled = false;
            this.wikiWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wikiWebBrowser_Navigated);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(981, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Close";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(981, 55);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // FormWikiDrafts
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1104, 641);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.textContent);
            this.Controls.Add(this.wikiWebBrowser);
            this.Controls.Add(this.wikiPagesGrid);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "FormWikiDrafts";
            this.ShowInTaskbar = false;
            this.Text = "Wiki Drafts";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormWikiDrafts_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private UI.ODGrid wikiPagesGrid;
		private System.Windows.Forms.WebBrowser wikiWebBrowser;
		private TextBoxWiki textContent;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button deleteButton;
	}
}