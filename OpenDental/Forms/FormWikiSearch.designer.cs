namespace OpenDental
{
    partial class FormWikiSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiSearch));
            this.wikiPagesGrid = new OpenDental.UI.ODGrid();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.ignoreContentCheckBox = new System.Windows.Forms.CheckBox();
            this.archivedOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.wikiWebBrowser = new System.Windows.Forms.WebBrowser();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.restoreButton = new System.Windows.Forms.Button();
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
            this.wikiPagesGrid.Location = new System.Drawing.Point(12, 48);
            this.wikiPagesGrid.Name = "wikiPagesGrid";
            this.wikiPagesGrid.ScrollValue = 0;
            this.wikiPagesGrid.Size = new System.Drawing.Size(250, 580);
            this.wikiPagesGrid.TabIndex = 2;
            this.wikiPagesGrid.Title = "Wiki Pages";
            this.wikiPagesGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.wikiPagesGrid.TitleHeight = 18;
            this.wikiPagesGrid.TranslationName = "TableWikiSearchPages";
            this.wikiPagesGrid.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.wikiPagesGrid_CellDoubleClick);
            this.wikiPagesGrid.CellClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.wikiPagesGrid_CellClick);
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
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(100, 19);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(162, 23);
            this.searchTextBox.TabIndex = 1;
            // 
            // ignoreContentCheckBox
            // 
            this.ignoreContentCheckBox.AutoSize = true;
            this.ignoreContentCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ignoreContentCheckBox.Location = new System.Drawing.Point(268, 20);
            this.ignoreContentCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.ignoreContentCheckBox.Name = "ignoreContentCheckBox";
            this.ignoreContentCheckBox.Size = new System.Drawing.Size(112, 20);
            this.ignoreContentCheckBox.TabIndex = 3;
            this.ignoreContentCheckBox.Text = "Ignore Content";
            this.ignoreContentCheckBox.CheckedChanged += new System.EventHandler(this.checkIgnoreContent_CheckedChanged);
            // 
            // archivedOnlyCheckBox
            // 
            this.archivedOnlyCheckBox.AutoSize = true;
            this.archivedOnlyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.archivedOnlyCheckBox.Location = new System.Drawing.Point(413, 20);
            this.archivedOnlyCheckBox.Name = "archivedOnlyCheckBox";
            this.archivedOnlyCheckBox.Size = new System.Drawing.Size(107, 20);
            this.archivedOnlyCheckBox.TabIndex = 4;
            this.archivedOnlyCheckBox.Text = "Archived Only";
            this.archivedOnlyCheckBox.CheckedChanged += new System.EventHandler(this.checkArchivedOnly_CheckedChanged);
            // 
            // wikiWebBrowser
            // 
            this.wikiWebBrowser.AllowNavigation = false;
            this.wikiWebBrowser.AllowWebBrowserDrop = false;
            this.wikiWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wikiWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.wikiWebBrowser.Location = new System.Drawing.Point(268, 49);
            this.wikiWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wikiWebBrowser.Name = "wikiWebBrowser";
            this.wikiWebBrowser.Size = new System.Drawing.Size(587, 580);
            this.wikiWebBrowser.TabIndex = 5;
            this.wikiWebBrowser.WebBrowserShortcutsEnabled = false;
            this.wikiWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wikiWebBrowser_Navigated);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(861, 562);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 7;
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
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Close";
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreButton.Enabled = false;
            this.restoreButton.Location = new System.Drawing.Point(861, 48);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(110, 30);
            this.restoreButton.TabIndex = 6;
            this.restoreButton.Text = "Restore";
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // FormWikiSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.restoreButton);
            this.Controls.Add(this.archivedOnlyCheckBox);
            this.Controls.Add(this.ignoreContentCheckBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.wikiWebBrowser);
            this.Controls.Add(this.wikiPagesGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiSearch";
            this.Text = "Wiki Search";
            this.Load += new System.EventHandler(this.FormWikiSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.WebBrowser wikiWebBrowser;
        private UI.ODGrid wikiPagesGrid;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.CheckBox ignoreContentCheckBox;
        private System.Windows.Forms.CheckBox archivedOnlyCheckBox;
        private System.Windows.Forms.Button restoreButton;
    }
}