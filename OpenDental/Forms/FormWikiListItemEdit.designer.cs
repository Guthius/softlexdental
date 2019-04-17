namespace OpenDental
{
    partial class FormWikiListItemEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListItemEdit));
            this.pickListComboBox = new System.Windows.Forms.ComboBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.itemDataGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // pickListComboBox
            // 
            this.pickListComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pickListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pickListComboBox.FormattingEnabled = true;
            this.pickListComboBox.ItemHeight = 15;
            this.pickListComboBox.Location = new System.Drawing.Point(431, 19);
            this.pickListComboBox.Name = "pickListComboBox";
            this.pickListComboBox.Size = new System.Drawing.Size(180, 23);
            this.pickListComboBox.TabIndex = 1;
            this.pickListComboBox.Visible = false;
            this.pickListComboBox.Leave += new System.EventHandler(this.pickListComboBox_Leave);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(385, 638);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(501, 638);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 638);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // itemDataGrid
            // 
            this.itemDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemDataGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.itemDataGrid.EditableAcceptsCR = true;
            this.itemDataGrid.EditableEnterMovesDown = false;
            this.itemDataGrid.HasAddButton = false;
            this.itemDataGrid.HasDropDowns = false;
            this.itemDataGrid.HasMultilineHeaders = false;
            this.itemDataGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.itemDataGrid.HeaderHeight = 15;
            this.itemDataGrid.HScrollVisible = true;
            this.itemDataGrid.Location = new System.Drawing.Point(13, 19);
            this.itemDataGrid.Name = "itemDataGrid";
            this.itemDataGrid.ScrollValue = 0;
            this.itemDataGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.itemDataGrid.Size = new System.Drawing.Size(598, 613);
            this.itemDataGrid.TabIndex = 0;
            this.itemDataGrid.Title = "";
            this.itemDataGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.itemDataGrid.TitleHeight = 18;
            this.itemDataGrid.TranslationName = "";
            this.itemDataGrid.CellLeave += new System.EventHandler<UI.ODGridClickEventArgs>(this.itemDataGrid_CellLeave);
            this.itemDataGrid.CellEnter += new System.EventHandler<UI.ODGridClickEventArgs>(this.itemDataGrid_CellEnter);
            // 
            // FormWikiListItemEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(624, 681);
            this.Controls.Add(this.pickListComboBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.itemDataGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWikiListItemEdit";
            this.Text = "Edit Wiki List Item";
            this.Load += new System.EventHandler(this.FormWikiListEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.ODGrid itemDataGrid;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox pickListComboBox;
    }
}