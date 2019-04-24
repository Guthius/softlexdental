namespace OpenDental
{
    partial class FormWikiIncomingLinks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiIncomingLinks));
            this.wikiPagesGrid = new OpenDental.UI.ODGrid();
            this.wikiWebBrowser = new System.Windows.Forms.WebBrowser();
            this.cancelButton = new System.Windows.Forms.Button();
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
            this.wikiPagesGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.wikiPagesGrid.HeaderHeight = 15;
            this.wikiPagesGrid.HScrollVisible = false;
            this.wikiPagesGrid.Location = new System.Drawing.Point(13, 19);
            this.wikiPagesGrid.Name = "wikiPagesGrid";
            this.wikiPagesGrid.ScrollValue = 0;
            this.wikiPagesGrid.Size = new System.Drawing.Size(250, 573);
            this.wikiPagesGrid.TabIndex = 6;
            this.wikiPagesGrid.Title = "Incoming Links";
            this.wikiPagesGrid.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.wikiPagesGrid_CellDoubleClick);
            this.wikiPagesGrid.Click += new System.EventHandler(this.wikiPagesGrid_Click);
            // 
            // wikiWebBrowser
            // 
            this.wikiWebBrowser.AllowNavigation = false;
            this.wikiWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wikiWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.wikiWebBrowser.Location = new System.Drawing.Point(269, 19);
            this.wikiWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wikiWebBrowser.Name = "wikiWebBrowser";
            this.wikiWebBrowser.Size = new System.Drawing.Size(702, 573);
            this.wikiWebBrowser.TabIndex = 0;
            this.wikiWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wikiWebBrowser_Navigated);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(861, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Close";
            // 
            // FormWikiIncomingLinks
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.wikiPagesGrid);
            this.Controls.Add(this.wikiWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiIncomingLinks";
            this.Text = "Incoming Links";
            this.Load += new System.EventHandler(this.FormWiki_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wikiWebBrowser;
        private UI.ODGrid wikiPagesGrid;
        private System.Windows.Forms.Button cancelButton;

    }
}