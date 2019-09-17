namespace OpenDental
{
    partial class FormSupplies
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplies));
            this.addToOrderButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.showShoppingListCheckBox = new System.Windows.Forms.CheckBox();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.printButton = new System.Windows.Forms.Button();
            this.searchLabel = new System.Windows.Forms.Label();
            this.supplierLabel = new System.Windows.Forms.Label();
            this.supplierComboBox = new System.Windows.Forms.ComboBox();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.addButton = new System.Windows.Forms.Button();
            this.suppliesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // addToOrderButton
            // 
            this.addToOrderButton.Enabled = false;
            this.addToOrderButton.Location = new System.Drawing.Point(279, 19);
            this.addToOrderButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.addToOrderButton.Name = "addToOrderButton";
            this.addToOrderButton.Size = new System.Drawing.Size(110, 30);
            this.addToOrderButton.TabIndex = 14;
            this.addToOrderButton.Text = "Add to Order";
            this.addToOrderButton.Click += new System.EventHandler(this.AddToOrderButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(304, 606);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(395, 15);
            this.infoLabel.TabIndex = 8;
            this.infoLabel.Text = "Make sure to press OK to save all desired changes to the supply inventory.";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showShoppingListCheckBox
            // 
            this.showShoppingListCheckBox.AutoSize = true;
            this.showShoppingListCheckBox.Location = new System.Drawing.Point(136, 19);
            this.showShoppingListCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.showShoppingListCheckBox.Name = "showShoppingListCheckBox";
            this.showShoppingListCheckBox.Size = new System.Drawing.Size(130, 19);
            this.showShoppingListCheckBox.TabIndex = 12;
            this.showShoppingListCheckBox.Text = "Show Shopping List";
            this.showShoppingListCheckBox.UseVisualStyleBackColor = true;
            this.showShoppingListCheckBox.Click += new System.EventHandler(this.ShowShoppingListCheckBox_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(500, 24);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(168, 23);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.upButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowUp;
            this.upButton.Location = new System.Drawing.Point(13, 598);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(40, 30);
            this.upButton.TabIndex = 5;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.downButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowDown;
            this.downButton.Location = new System.Drawing.Point(59, 598);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(40, 30);
            this.downButton.TabIndex = 6;
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(705, 598);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(821, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = global::OpenDental.Properties.Resources.IconPrinter;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printButton.Location = new System.Drawing.Point(112, 598);
            this.printButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(110, 30);
            this.printButton.TabIndex = 7;
            this.printButton.Text = "Print";
            this.printButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(452, 27);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(42, 15);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "Search";
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // supplierLabel
            // 
            this.supplierLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(705, 27);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(50, 15);
            this.supplierLabel.TabIndex = 2;
            this.supplierLabel.Text = "Supplier";
            this.supplierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // supplierComboBox
            // 
            this.supplierComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.supplierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.supplierComboBox.FormattingEnabled = true;
            this.supplierComboBox.Location = new System.Drawing.Point(761, 24);
            this.supplierComboBox.Name = "supplierComboBox";
            this.supplierComboBox.Size = new System.Drawing.Size(170, 23);
            this.supplierComboBox.TabIndex = 3;
            this.supplierComboBox.SelectionChangeCommitted += new System.EventHandler(this.SupplierComboBox_SelectionChangeCommitted);
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(136, 41);
            this.showHiddenCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(97, 19);
            this.showHiddenCheckBox.TabIndex = 13;
            this.showHiddenCheckBox.Text = "Show Hidden";
            this.showHiddenCheckBox.UseVisualStyleBackColor = true;
            this.showHiddenCheckBox.Click += new System.EventHandler(this.ShowHiddenCheckBox_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(13, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 11;
            this.addButton.Text = "Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // suppliesGrid
            // 
            this.suppliesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suppliesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.suppliesGrid.EditableEnterMovesDown = false;
            this.suppliesGrid.HasAddButton = false;
            this.suppliesGrid.HasDropDowns = false;
            this.suppliesGrid.HasMultilineHeaders = false;
            this.suppliesGrid.HScrollVisible = false;
            this.suppliesGrid.Location = new System.Drawing.Point(13, 66);
            this.suppliesGrid.Name = "suppliesGrid";
            this.suppliesGrid.ScrollValue = 0;
            this.suppliesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.suppliesGrid.Size = new System.Drawing.Size(918, 511);
            this.suppliesGrid.TabIndex = 4;
            this.suppliesGrid.Title = "Supplies";
            this.suppliesGrid.TitleVisible = true;
            this.suppliesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.SuppliesGrid_CellDoubleClick);
            // 
            // FormSupplies
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(944, 641);
            this.Controls.Add(this.addToOrderButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.showShoppingListCheckBox);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.supplierComboBox);
            this.Controls.Add(this.showHiddenCheckBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.suppliesGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(960, 680);
            this.Name = "FormSupplies";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supplies";
            this.Load += new System.EventHandler(this.FormSupplies_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.ODGrid suppliesGrid;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.ComboBox supplierComboBox;
        private System.Windows.Forms.CheckBox showHiddenCheckBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.CheckBox showShoppingListCheckBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button addToOrderButton;
    }
}