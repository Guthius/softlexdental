namespace OpenDental
{
    partial class FormWikiListEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListEdit));
            this.listGroupBox = new System.Windows.Forms.GroupBox();
            this.listHistoryButton = new System.Windows.Forms.Button();
            this.listRenameButton = new System.Windows.Forms.Button();
            this.rowsGroupBox = new System.Windows.Forms.GroupBox();
            this.rowAddButton = new System.Windows.Forms.Button();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.columnsGroupBox = new System.Windows.Forms.GroupBox();
            this.columnDeleteButton = new System.Windows.Forms.Button();
            this.columnEditButton = new System.Windows.Forms.Button();
            this.columnAddButton = new System.Windows.Forms.Button();
            this.columnRightButton = new System.Windows.Forms.Button();
            this.columnLeftButton = new System.Windows.Forms.Button();
            this.itemsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new System.Windows.Forms.Button();
            this.advSearchButton = new System.Windows.Forms.Button();
            this.advSearchClearButton = new System.Windows.Forms.Button();
            this.highlightRadioButton = new System.Windows.Forms.RadioButton();
            this.filterRadioButton = new System.Windows.Forms.RadioButton();
            this.listGroupBox.SuspendLayout();
            this.rowsGroupBox.SuspendLayout();
            this.columnsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // listGroupBox
            // 
            this.listGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listGroupBox.Controls.Add(this.listHistoryButton);
            this.listGroupBox.Controls.Add(this.listRenameButton);
            this.listGroupBox.Location = new System.Drawing.Point(721, 48);
            this.listGroupBox.Name = "listGroupBox";
            this.listGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.listGroupBox.Size = new System.Drawing.Size(110, 106);
            this.listGroupBox.TabIndex = 7;
            this.listGroupBox.TabStop = false;
            this.listGroupBox.Text = "List";
            // 
            // listHistoryButton
            // 
            this.listHistoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listHistoryButton.Location = new System.Drawing.Point(6, 62);
            this.listHistoryButton.Name = "listHistoryButton";
            this.listHistoryButton.Size = new System.Drawing.Size(98, 30);
            this.listHistoryButton.TabIndex = 1;
            this.listHistoryButton.Text = "History";
            this.listHistoryButton.Click += new System.EventHandler(this.listHistoryButton_Click);
            // 
            // listRenameButton
            // 
            this.listRenameButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRenameButton.Location = new System.Drawing.Point(6, 26);
            this.listRenameButton.Name = "listRenameButton";
            this.listRenameButton.Size = new System.Drawing.Size(98, 30);
            this.listRenameButton.TabIndex = 0;
            this.listRenameButton.Text = "Rename";
            this.listRenameButton.Click += new System.EventHandler(this.listRenameButton_Click);
            // 
            // rowsGroupBox
            // 
            this.rowsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rowsGroupBox.Controls.Add(this.rowAddButton);
            this.rowsGroupBox.Location = new System.Drawing.Point(721, 350);
            this.rowsGroupBox.Name = "rowsGroupBox";
            this.rowsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rowsGroupBox.Size = new System.Drawing.Size(110, 70);
            this.rowsGroupBox.TabIndex = 9;
            this.rowsGroupBox.TabStop = false;
            this.rowsGroupBox.Text = "Rows";
            // 
            // rowAddButton
            // 
            this.rowAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rowAddButton.Location = new System.Drawing.Point(6, 28);
            this.rowAddButton.Name = "rowAddButton";
            this.rowAddButton.Size = new System.Drawing.Size(98, 30);
            this.rowAddButton.TabIndex = 0;
            this.rowAddButton.Text = "Add";
            this.rowAddButton.Click += new System.EventHandler(this.rowAddButton_Click);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(72, 22);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(42, 15);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "Search";
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(120, 19);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(218, 23);
            this.searchTextBox.TabIndex = 1;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 598);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // columnsGroupBox
            // 
            this.columnsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.columnsGroupBox.Controls.Add(this.columnDeleteButton);
            this.columnsGroupBox.Controls.Add(this.columnEditButton);
            this.columnsGroupBox.Controls.Add(this.columnAddButton);
            this.columnsGroupBox.Controls.Add(this.columnRightButton);
            this.columnsGroupBox.Controls.Add(this.columnLeftButton);
            this.columnsGroupBox.Location = new System.Drawing.Point(721, 164);
            this.columnsGroupBox.Name = "columnsGroupBox";
            this.columnsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.columnsGroupBox.Size = new System.Drawing.Size(110, 180);
            this.columnsGroupBox.TabIndex = 8;
            this.columnsGroupBox.TabStop = false;
            this.columnsGroupBox.Text = "Columns";
            // 
            // columnDeleteButton
            // 
            this.columnDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnDeleteButton.Location = new System.Drawing.Point(6, 137);
            this.columnDeleteButton.Name = "columnDeleteButton";
            this.columnDeleteButton.Size = new System.Drawing.Size(98, 30);
            this.columnDeleteButton.TabIndex = 4;
            this.columnDeleteButton.Text = "Delete";
            this.columnDeleteButton.Click += new System.EventHandler(this.columnDeleteButton_Click);
            // 
            // columnEditButton
            // 
            this.columnEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnEditButton.Location = new System.Drawing.Point(6, 65);
            this.columnEditButton.Name = "columnEditButton";
            this.columnEditButton.Size = new System.Drawing.Size(98, 30);
            this.columnEditButton.TabIndex = 2;
            this.columnEditButton.Text = "Edit";
            this.columnEditButton.Click += new System.EventHandler(this.columnEditButton_Click);
            // 
            // columnAddButton
            // 
            this.columnAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnAddButton.Location = new System.Drawing.Point(6, 101);
            this.columnAddButton.Name = "columnAddButton";
            this.columnAddButton.Size = new System.Drawing.Size(98, 30);
            this.columnAddButton.TabIndex = 3;
            this.columnAddButton.Text = "Add";
            this.columnAddButton.Click += new System.EventHandler(this.columnAddButton_Click);
            // 
            // columnRightButton
            // 
            this.columnRightButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.columnRightButton.Location = new System.Drawing.Point(59, 29);
            this.columnRightButton.Name = "columnRightButton";
            this.columnRightButton.Size = new System.Drawing.Size(45, 30);
            this.columnRightButton.TabIndex = 1;
            this.columnRightButton.Text = "R";
            this.columnRightButton.Click += new System.EventHandler(this.columnRightButton_Click);
            // 
            // columnLeftButton
            // 
            this.columnLeftButton.Location = new System.Drawing.Point(6, 29);
            this.columnLeftButton.Name = "columnLeftButton";
            this.columnLeftButton.Size = new System.Drawing.Size(45, 30);
            this.columnLeftButton.TabIndex = 0;
            this.columnLeftButton.Text = "L";
            this.columnLeftButton.Click += new System.EventHandler(this.columnLeftButton_Click);
            // 
            // itemsGrid
            // 
            this.itemsGrid.AllowSortingByColumn = true;
            this.itemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.itemsGrid.EditableAcceptsCR = true;
            this.itemsGrid.EditableEnterMovesDown = false;
            this.itemsGrid.HasAddButton = false;
            this.itemsGrid.HasAutoWrappedHeaders = true;
            this.itemsGrid.HasDropDowns = false;
            this.itemsGrid.HasMultilineHeaders = true;
            this.itemsGrid.HScrollVisible = true;
            this.itemsGrid.Location = new System.Drawing.Point(13, 48);
            this.itemsGrid.Name = "itemsGrid";
            this.itemsGrid.ScrollValue = 0;
            this.itemsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.itemsGrid.Size = new System.Drawing.Size(702, 544);
            this.itemsGrid.TabIndex = 6;
            this.itemsGrid.Title = "";
            this.itemsGrid.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(721, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Close";
            // 
            // advSearchButton
            // 
            this.advSearchButton.Location = new System.Drawing.Point(339, 18);
            this.advSearchButton.Name = "advSearchButton";
            this.advSearchButton.Size = new System.Drawing.Size(60, 25);
            this.advSearchButton.TabIndex = 2;
            this.advSearchButton.Text = "Adv. Search";
            this.advSearchButton.UseVisualStyleBackColor = true;
            this.advSearchButton.Click += new System.EventHandler(this.advSearchButton_Click);
            // 
            // advSearchClearButton
            // 
            this.advSearchClearButton.Location = new System.Drawing.Point(399, 18);
            this.advSearchClearButton.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.advSearchClearButton.Name = "advSearchClearButton";
            this.advSearchClearButton.Size = new System.Drawing.Size(60, 25);
            this.advSearchClearButton.TabIndex = 3;
            this.advSearchClearButton.Text = "Clear";
            this.advSearchClearButton.UseVisualStyleBackColor = true;
            this.advSearchClearButton.Click += new System.EventHandler(this.advSearchClearButton_Click);
            // 
            // highlightRadioButton
            // 
            this.highlightRadioButton.AutoSize = true;
            this.highlightRadioButton.Location = new System.Drawing.Point(512, 21);
            this.highlightRadioButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.highlightRadioButton.Name = "highlightRadioButton";
            this.highlightRadioButton.Size = new System.Drawing.Size(75, 19);
            this.highlightRadioButton.TabIndex = 4;
            this.highlightRadioButton.TabStop = true;
            this.highlightRadioButton.Text = "Highlight";
            this.highlightRadioButton.UseVisualStyleBackColor = true;
            this.highlightRadioButton.CheckedChanged += new System.EventHandler(this.highlightRadioButton_CheckedChanged);
            // 
            // filterRadioButton
            // 
            this.filterRadioButton.AutoSize = true;
            this.filterRadioButton.Location = new System.Drawing.Point(620, 21);
            this.filterRadioButton.Name = "filterRadioButton";
            this.filterRadioButton.Size = new System.Drawing.Size(51, 19);
            this.filterRadioButton.TabIndex = 5;
            this.filterRadioButton.TabStop = true;
            this.filterRadioButton.Text = "Filter";
            this.filterRadioButton.UseVisualStyleBackColor = true;
            this.filterRadioButton.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // FormWikiListEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(844, 641);
            this.Controls.Add(this.filterRadioButton);
            this.Controls.Add(this.highlightRadioButton);
            this.Controls.Add(this.advSearchClearButton);
            this.Controls.Add(this.advSearchButton);
            this.Controls.Add(this.listGroupBox);
            this.Controls.Add(this.rowsGroupBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.columnsGroupBox);
            this.Controls.Add(this.itemsGrid);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(860, 540);
            this.Name = "FormWikiListEdit";
            this.Text = "Edit Wiki List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormWikiListEdit_Load);
            this.listGroupBox.ResumeLayout(false);
            this.rowsGroupBox.ResumeLayout(false);
            this.columnsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid itemsGrid;
        private System.Windows.Forms.GroupBox columnsGroupBox;
        private System.Windows.Forms.Button columnEditButton;
        private System.Windows.Forms.Button columnRightButton;
        private System.Windows.Forms.Button columnLeftButton;
        private System.Windows.Forms.Button rowAddButton;
        private System.Windows.Forms.Button columnAddButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.GroupBox rowsGroupBox;
        private System.Windows.Forms.GroupBox listGroupBox;
        private System.Windows.Forms.Button listRenameButton;
        private System.Windows.Forms.Button listHistoryButton;
        private System.Windows.Forms.Button advSearchButton;
        private System.Windows.Forms.Button advSearchClearButton;
        private System.Windows.Forms.RadioButton highlightRadioButton;
        private System.Windows.Forms.RadioButton filterRadioButton;
        private System.Windows.Forms.Button columnDeleteButton;
    }
}