namespace OpenDental
{
    partial class FormWikiListHeaders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListHeaders));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.columnHeadersGrid = new OpenDental.UI.ODGrid();
            this.pickListGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(621, 422);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(621, 458);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            // 
            // columnHeadersGrid
            // 
            this.columnHeadersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnHeadersGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.columnHeadersGrid.EditableEnterMovesDown = false;
            this.columnHeadersGrid.HasAddButton = false;
            this.columnHeadersGrid.HasDropDowns = false;
            this.columnHeadersGrid.HasMultilineHeaders = false;
            this.columnHeadersGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.columnHeadersGrid.HeaderHeight = 15;
            this.columnHeadersGrid.HScrollVisible = false;
            this.columnHeadersGrid.Location = new System.Drawing.Point(13, 19);
            this.columnHeadersGrid.Name = "columnHeadersGrid";
            this.columnHeadersGrid.ScrollValue = 0;
            this.columnHeadersGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.columnHeadersGrid.Size = new System.Drawing.Size(360, 469);
            this.columnHeadersGrid.TabIndex = 0;
            this.columnHeadersGrid.Title = "Wiki List Headers";
            this.columnHeadersGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.columnHeadersGrid.TitleHeight = 18;
            this.columnHeadersGrid.TranslationName = "TableWikiListHeaders";
            this.columnHeadersGrid.CellClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.columnHeadersGrid_CellClick);
            // 
            // pickListGrid
            // 
            this.pickListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pickListGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.pickListGrid.EditableEnterMovesDown = false;
            this.pickListGrid.HasAddButton = false;
            this.pickListGrid.HasDropDowns = false;
            this.pickListGrid.HasMultilineHeaders = false;
            this.pickListGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.pickListGrid.HeaderHeight = 15;
            this.pickListGrid.HScrollVisible = false;
            this.pickListGrid.Location = new System.Drawing.Point(379, 19);
            this.pickListGrid.Name = "pickListGrid";
            this.pickListGrid.ScrollValue = 0;
            this.pickListGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.pickListGrid.Size = new System.Drawing.Size(236, 469);
            this.pickListGrid.TabIndex = 1;
            this.pickListGrid.Title = "Pick List Options";
            this.pickListGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.pickListGrid.TitleHeight = 18;
            this.pickListGrid.TranslationName = "TableWikiListHeaderPickList";
            this.pickListGrid.CellLeave += new System.EventHandler<UI.ODGridClickEventArgs>(this.pickListGrid_CellLeave);
            this.pickListGrid.CellEnter += new System.EventHandler<UI.ODGridClickEventArgs>(this.pickListGrid_CellEnter);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(621, 19);
            this.addButton.Name = "addButton";
            this.addButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeButton.Location = new System.Drawing.Point(621, 55);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(110, 30);
            this.removeButton.TabIndex = 3;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // FormWikiListHeaders
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(744, 501);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.pickListGrid);
            this.Controls.Add(this.columnHeadersGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiListHeaders";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Wiki List Headers";
            this.Load += new System.EventHandler(this.FormWikiListHeaders_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid columnHeadersGrid;
        private UI.ODGrid pickListGrid;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
    }
}