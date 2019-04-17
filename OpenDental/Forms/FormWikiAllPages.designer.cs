namespace OpenDental
{
    partial class FormWikiAllPages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiAllPages));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.wikiWebBrowser = new System.Windows.Forms.WebBrowser();
            this.pagesGrid = new OpenDental.UI.ODGrid();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.bracketsButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(861, 562);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(861, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            // 
            // wikiWebBrowser
            // 
            this.wikiWebBrowser.AllowWebBrowserDrop = false;
            this.wikiWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wikiWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.wikiWebBrowser.Location = new System.Drawing.Point(269, 48);
            this.wikiWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wikiWebBrowser.Name = "wikiWebBrowser";
            this.wikiWebBrowser.Size = new System.Drawing.Size(586, 580);
            this.wikiWebBrowser.TabIndex = 3;
            this.wikiWebBrowser.WebBrowserShortcutsEnabled = false;
            this.wikiWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserWiki_Navigated);
            // 
            // pagesGrid
            // 
            this.pagesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pagesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.pagesGrid.EditableEnterMovesDown = false;
            this.pagesGrid.HasAddButton = false;
            this.pagesGrid.HasDropDowns = false;
            this.pagesGrid.HasMultilineHeaders = false;
            this.pagesGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.pagesGrid.HeaderHeight = 15;
            this.pagesGrid.HScrollVisible = false;
            this.pagesGrid.Location = new System.Drawing.Point(13, 48);
            this.pagesGrid.Name = "pagesGrid";
            this.pagesGrid.ScrollValue = 0;
            this.pagesGrid.Size = new System.Drawing.Size(250, 580);
            this.pagesGrid.TabIndex = 2;
            this.pagesGrid.Title = "All Wiki Pages";
            this.pagesGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.pagesGrid.TitleHeight = 18;
            this.pagesGrid.TranslationName = "TableWikiHistory";
            this.pagesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.pagesGrid_CellDoubleClick);
            this.pagesGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.pagesGrid_CellClick);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(100, 19);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(163, 23);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(52, 22);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(42, 15);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "Search";
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bracketsButton
            // 
            this.bracketsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bracketsButton.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bracketsButton.Location = new System.Drawing.Point(861, 84);
            this.bracketsButton.Name = "bracketsButton";
            this.bracketsButton.Size = new System.Drawing.Size(110, 30);
            this.bracketsButton.TabIndex = 5;
            this.bracketsButton.Text = "[[  ]]";
            this.bracketsButton.Click += new System.EventHandler(this.bracketsButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(861, 48);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // FormWikiAllPages
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.bracketsButton);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.wikiWebBrowser);
            this.Controls.Add(this.pagesGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiAllPages";
            this.Text = "All Wiki Pages";
            this.Load += new System.EventHandler(this.FormWikiAllPages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.WebBrowser wikiWebBrowser;
        private UI.ODGrid pagesGrid;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button bracketsButton;
        private System.Windows.Forms.Button addButton;
    }
}