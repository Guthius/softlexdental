namespace OpenDental
{
    partial class FormProcButtons
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcButtons));
            this.buttonsLabel = new System.Windows.Forms.Label();
            this.categoriesLabel = new System.Windows.Forms.Label();
            this.categoriesListBox = new System.Windows.Forms.ListBox();
            this.buttonsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListProcButtons = new System.Windows.Forms.ImageList(this.components);
            this.labelEdit = new System.Windows.Forms.Label();
            this.buttonsPanel = new OpenDental.UI.ODButtonPanel();
            this.editButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonsLabel
            // 
            this.buttonsLabel.AutoSize = true;
            this.buttonsLabel.Location = new System.Drawing.Point(276, 16);
            this.buttonsLabel.Name = "buttonsLabel";
            this.buttonsLabel.Size = new System.Drawing.Size(181, 15);
            this.buttonsLabel.TabIndex = 3;
            this.buttonsLabel.Text = "Buttons for the selected category";
            this.buttonsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // categoriesLabel
            // 
            this.categoriesLabel.AutoSize = true;
            this.categoriesLabel.Location = new System.Drawing.Point(13, 16);
            this.categoriesLabel.Name = "categoriesLabel";
            this.categoriesLabel.Size = new System.Drawing.Size(102, 15);
            this.categoriesLabel.TabIndex = 0;
            this.categoriesLabel.Text = "Button Categories";
            this.categoriesLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // categoriesListBox
            // 
            this.categoriesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.categoriesListBox.IntegralHeight = false;
            this.categoriesListBox.ItemHeight = 15;
            this.categoriesListBox.Location = new System.Drawing.Point(13, 34);
            this.categoriesListBox.Name = "categoriesListBox";
            this.categoriesListBox.Size = new System.Drawing.Size(260, 357);
            this.categoriesListBox.TabIndex = 1;
            this.categoriesListBox.Click += new System.EventHandler(this.categoriesListBox_Click);
            // 
            // buttonsListView
            // 
            this.buttonsListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.buttonsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsListView.AutoArrange = false;
            this.buttonsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.buttonsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.buttonsListView.HideSelection = false;
            this.buttonsListView.Location = new System.Drawing.Point(279, 34);
            this.buttonsListView.MultiSelect = false;
            this.buttonsListView.Name = "buttonsListView";
            this.buttonsListView.Size = new System.Drawing.Size(266, 357);
            this.buttonsListView.SmallImageList = this.imageListProcButtons;
            this.buttonsListView.TabIndex = 189;
            this.buttonsListView.UseCompatibleStateImageBehavior = false;
            this.buttonsListView.View = System.Windows.Forms.View.Details;
            this.buttonsListView.DoubleClick += new System.EventHandler(this.buttonsListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 155;
            // 
            // imageListProcButtons
            // 
            this.imageListProcButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProcButtons.ImageStream")));
            this.imageListProcButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListProcButtons.Images.SetKeyName(0, "deposit.gif");
            // 
            // labelEdit
            // 
            this.labelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEdit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelEdit.Location = new System.Drawing.Point(279, 318);
            this.labelEdit.Name = "labelEdit";
            this.labelEdit.Size = new System.Drawing.Size(266, 73);
            this.labelEdit.TabIndex = 5;
            this.labelEdit.Text = "The Quick Buttons category allows custom placement of buttons and labels.  Double" +
    " click anywhere on panel above to add or edit an item.";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsPanel.Location = new System.Drawing.Point(279, 34);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(266, 281);
            this.buttonsPanel.TabIndex = 4;
            this.buttonsPanel.MouseDoubleClickItem += new System.EventHandler<OpenDental.UI.ODButtonPanelItemMouseEventArgs>(this.ButtonsPanel_ItemMouseDoubleClick);
            this.buttonsPanel.MouseDoubleClickRow += new System.EventHandler<OpenDental.UI.ODButtonPanelRowMouseEventArgs>(this.ButtonsPanel_RowMouseDoubleClick);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Image = global::OpenDental.Properties.Resources.IconEdit;
            this.editButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.editButton.Location = new System.Drawing.Point(13, 397);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(150, 30);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "Edit Categories";
            this.editButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowDown;
            this.downButton.Location = new System.Drawing.Point(551, 70);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(40, 30);
            this.downButton.TabIndex = 9;
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowUp;
            this.upButton.Location = new System.Drawing.Point(551, 34);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(40, 30);
            this.upButton.TabIndex = 8;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(279, 397);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "&Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(395, 397);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(481, 458);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FormProcButtons
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(604, 501);
            this.Controls.Add(this.labelEdit);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.buttonsListView);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.categoriesLabel);
            this.Controls.Add(this.categoriesListBox);
            this.Controls.Add(this.buttonsLabel);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProcButtons";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Procedure Buttons";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcButtons_Closing);
            this.Load += new System.EventHandler(this.FormChartProcedureEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Label buttonsLabel;
        private System.Windows.Forms.Label categoriesLabel;
        private System.Windows.Forms.ListBox categoriesListBox;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ListView buttonsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageListProcButtons;
        private OpenDental.UI.ODButtonPanel buttonsPanel;
        private System.Windows.Forms.Label labelEdit;
    }
}