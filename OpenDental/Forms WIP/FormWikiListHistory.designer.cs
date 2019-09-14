namespace OpenDental
{
    partial class FormWikiListHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListHistory));
            this.gridMain = new OpenDental.UI.ODGrid();
            this.revertButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.oldRevisionGrid = new OpenDental.UI.ODGrid();
            this.currentRevisionGrid = new OpenDental.UI.ODGrid();
            this.historySplitContainer = new System.Windows.Forms.SplitContainer();
            this.revisionsSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.historySplitContainer)).BeginInit();
            this.historySplitContainer.Panel1.SuspendLayout();
            this.historySplitContainer.Panel2.SuspendLayout();
            this.historySplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.revisionsSplitContainer)).BeginInit();
            this.revisionsSplitContainer.Panel1.SuspendLayout();
            this.revisionsSplitContainer.Panel2.SuspendLayout();
            this.revisionsSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridMain
            // 
            this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.EditableEnterMovesDown = false;
            this.gridMain.HasAddButton = false;
            this.gridMain.HasDropDowns = false;
            this.gridMain.HasMultilineHeaders = false;
            this.gridMain.HScrollVisible = false;
            this.gridMain.Location = new System.Drawing.Point(0, 0);
            this.gridMain.Name = "gridMain";
            this.gridMain.ScrollValue = 0;
            this.gridMain.Size = new System.Drawing.Size(263, 609);
            this.gridMain.TabIndex = 0;
            this.gridMain.Title = "Wiki List History";
            this.gridMain.Click += new System.EventHandler(this.gridMain_Click);
            // 
            // revertButton
            // 
            this.revertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revertButton.Location = new System.Drawing.Point(861, 19);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(110, 30);
            this.revertButton.TabIndex = 3;
            this.revertButton.Text = "Revert";
            this.revertButton.Click += new System.EventHandler(this.revertButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(861, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Close";
            // 
            // oldRevisionGrid
            // 
            this.oldRevisionGrid.AllowSortingByColumn = true;
            this.oldRevisionGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.oldRevisionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oldRevisionGrid.EditableAcceptsCR = true;
            this.oldRevisionGrid.EditableEnterMovesDown = false;
            this.oldRevisionGrid.HasAddButton = false;
            this.oldRevisionGrid.HasDropDowns = false;
            this.oldRevisionGrid.HasMultilineHeaders = false;
            this.oldRevisionGrid.HScrollVisible = true;
            this.oldRevisionGrid.Location = new System.Drawing.Point(0, 0);
            this.oldRevisionGrid.Name = "oldRevisionGrid";
            this.oldRevisionGrid.ScrollValue = 0;
            this.oldRevisionGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Cell;
            this.oldRevisionGrid.Size = new System.Drawing.Size(283, 609);
            this.oldRevisionGrid.TabIndex = 1;
            this.oldRevisionGrid.Title = "Old Revision";
            // 
            // currentRevisionGrid
            // 
            this.currentRevisionGrid.AllowSortingByColumn = true;
            this.currentRevisionGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.currentRevisionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRevisionGrid.EditableAcceptsCR = true;
            this.currentRevisionGrid.EditableEnterMovesDown = false;
            this.currentRevisionGrid.HasAddButton = false;
            this.currentRevisionGrid.HasDropDowns = false;
            this.currentRevisionGrid.HasMultilineHeaders = false;
            this.currentRevisionGrid.HScrollVisible = true;
            this.currentRevisionGrid.Location = new System.Drawing.Point(0, 0);
            this.currentRevisionGrid.Name = "currentRevisionGrid";
            this.currentRevisionGrid.ScrollValue = 0;
            this.currentRevisionGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Cell;
            this.currentRevisionGrid.Size = new System.Drawing.Size(288, 609);
            this.currentRevisionGrid.TabIndex = 2;
            this.currentRevisionGrid.Title = "Current Revision";
            // 
            // historySplitContainer
            // 
            this.historySplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historySplitContainer.Location = new System.Drawing.Point(13, 19);
            this.historySplitContainer.Name = "historySplitContainer";
            // 
            // historySplitContainer.Panel1
            // 
            this.historySplitContainer.Panel1.Controls.Add(this.gridMain);
            // 
            // historySplitContainer.Panel2
            // 
            this.historySplitContainer.Panel2.Controls.Add(this.revisionsSplitContainer);
            this.historySplitContainer.Size = new System.Drawing.Size(842, 609);
            this.historySplitContainer.SplitterDistance = 263;
            this.historySplitContainer.TabIndex = 5;
            // 
            // revisionsSplitContainer
            // 
            this.revisionsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revisionsSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.revisionsSplitContainer.Name = "revisionsSplitContainer";
            // 
            // revisionsSplitContainer.Panel1
            // 
            this.revisionsSplitContainer.Panel1.Controls.Add(this.oldRevisionGrid);
            // 
            // revisionsSplitContainer.Panel2
            // 
            this.revisionsSplitContainer.Panel2.Controls.Add(this.currentRevisionGrid);
            this.revisionsSplitContainer.Size = new System.Drawing.Size(575, 609);
            this.revisionsSplitContainer.SplitterDistance = 283;
            this.revisionsSplitContainer.TabIndex = 0;
            // 
            // FormWikiListHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.historySplitContainer);
            this.Controls.Add(this.revertButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(990, 676);
            this.Name = "FormWikiListHistory";
            this.Text = "Wiki List History";
            this.Load += new System.EventHandler(this.FormWikiListHistory_Load);
            this.historySplitContainer.Panel1.ResumeLayout(false);
            this.historySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historySplitContainer)).EndInit();
            this.historySplitContainer.ResumeLayout(false);
            this.revisionsSplitContainer.Panel1.ResumeLayout(false);
            this.revisionsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.revisionsSplitContainer)).EndInit();
            this.revisionsSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid gridMain;
        private System.Windows.Forms.Button revertButton;
        private UI.ODGrid oldRevisionGrid;
        private UI.ODGrid currentRevisionGrid;
        private System.Windows.Forms.SplitContainer historySplitContainer;
        private System.Windows.Forms.SplitContainer revisionsSplitContainer;
    }
}