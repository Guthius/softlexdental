namespace OpenDental
{
    partial class FormWikiHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiHistory));
            this.labelNotAuthorized = new System.Windows.Forms.Label();
            this.revertButton = new System.Windows.Forms.Button();
            this.webBrowserWiki = new System.Windows.Forms.WebBrowser();
            this.wikiPagesGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelNotAuthorized
            // 
            this.labelNotAuthorized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNotAuthorized.Location = new System.Drawing.Point(961, 52);
            this.labelNotAuthorized.Name = "labelNotAuthorized";
            this.labelNotAuthorized.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.labelNotAuthorized.Size = new System.Drawing.Size(110, 140);
            this.labelNotAuthorized.TabIndex = 3;
            this.labelNotAuthorized.Text = "This wiki page is locked and cannot be edited without the WikiAdmin permission.";
            this.labelNotAuthorized.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // revertButton
            // 
            this.revertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revertButton.Location = new System.Drawing.Point(961, 19);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(110, 30);
            this.revertButton.TabIndex = 2;
            this.revertButton.Text = "Revert";
            this.revertButton.Click += new System.EventHandler(this.revertButton_Click);
            // 
            // webBrowserWiki
            // 
            this.webBrowserWiki.AllowWebBrowserDrop = false;
            this.webBrowserWiki.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowserWiki.IsWebBrowserContextMenuEnabled = false;
            this.webBrowserWiki.Location = new System.Drawing.Point(269, 19);
            this.webBrowserWiki.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserWiki.Name = "webBrowserWiki";
            this.webBrowserWiki.Size = new System.Drawing.Size(686, 609);
            this.webBrowserWiki.TabIndex = 0;
            this.webBrowserWiki.WebBrowserShortcutsEnabled = false;
            this.webBrowserWiki.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserWiki_Navigated);
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
            this.wikiPagesGrid.Title = "Page History";
            this.wikiPagesGrid.Click += new System.EventHandler(this.gridMain_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(961, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Close";
            // 
            // FormWikiHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1084, 641);
            this.Controls.Add(this.webBrowserWiki);
            this.Controls.Add(this.labelNotAuthorized);
            this.Controls.Add(this.revertButton);
            this.Controls.Add(this.wikiPagesGrid);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiHistory";
            this.Text = "Wiki History";
            this.Load += new System.EventHandler(this.FormWikiHistory_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid wikiPagesGrid;
        private System.Windows.Forms.WebBrowser webBrowserWiki;
        private System.Windows.Forms.Button revertButton;
        private System.Windows.Forms.Label labelNotAuthorized;
    }
}