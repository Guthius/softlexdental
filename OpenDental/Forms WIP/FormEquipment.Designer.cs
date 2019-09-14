namespace OpenDental
{
    partial class FormEquipment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEquipment));
            this.infoLabel = new System.Windows.Forms.Label();
            this.dateRangeGroupBox = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.dateStartTextBox = new OpenDental.ValidDate();
            this.dateEndTextBox = new OpenDental.ValidDate();
            this.purchasedRadioButton = new System.Windows.Forms.RadioButton();
            this.soldRadioButton = new System.Windows.Forms.RadioButton();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.equipmentGrid = new OpenDental.UI.ODGrid();
            this.closeButton = new System.Windows.Forms.Button();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.dateRangeGroupBox.SuspendLayout();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.infoLabel.Location = new System.Drawing.Point(13, 19);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(200, 60);
            this.infoLabel.TabIndex = 7;
            this.infoLabel.Text = "This list tracks equipment for payment of property taxes.";
            // 
            // dateRangeGroupBox
            // 
            this.dateRangeGroupBox.Controls.Add(this.refreshButton);
            this.dateRangeGroupBox.Controls.Add(this.dateStartTextBox);
            this.dateRangeGroupBox.Controls.Add(this.dateEndTextBox);
            this.dateRangeGroupBox.Location = new System.Drawing.Point(475, 19);
            this.dateRangeGroupBox.Name = "dateRangeGroupBox";
            this.dateRangeGroupBox.Size = new System.Drawing.Size(260, 60);
            this.dateRangeGroupBox.TabIndex = 23;
            this.dateRangeGroupBox.TabStop = false;
            this.dateRangeGroupBox.Text = "Date Range";
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(172, 21);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(80, 25);
            this.refreshButton.TabIndex = 23;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(6, 22);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(77, 23);
            this.dateStartTextBox.TabIndex = 21;
            // 
            // dateEndTextBox
            // 
            this.dateEndTextBox.Location = new System.Drawing.Point(89, 22);
            this.dateEndTextBox.Name = "dateEndTextBox";
            this.dateEndTextBox.Size = new System.Drawing.Size(77, 23);
            this.dateEndTextBox.TabIndex = 22;
            // 
            // purchasedRadioButton
            // 
            this.purchasedRadioButton.AutoSize = true;
            this.purchasedRadioButton.Location = new System.Drawing.Point(748, 20);
            this.purchasedRadioButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.purchasedRadioButton.Name = "purchasedRadioButton";
            this.purchasedRadioButton.Size = new System.Drawing.Size(80, 19);
            this.purchasedRadioButton.TabIndex = 23;
            this.purchasedRadioButton.Text = "Purchased";
            this.purchasedRadioButton.UseVisualStyleBackColor = true;
            this.purchasedRadioButton.Click += new System.EventHandler(this.radioPurchased_Click);
            // 
            // soldRadioButton
            // 
            this.soldRadioButton.AutoSize = true;
            this.soldRadioButton.Location = new System.Drawing.Point(748, 40);
            this.soldRadioButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.soldRadioButton.Name = "soldRadioButton";
            this.soldRadioButton.Size = new System.Drawing.Size(48, 19);
            this.soldRadioButton.TabIndex = 24;
            this.soldRadioButton.Text = "Sold";
            this.soldRadioButton.UseVisualStyleBackColor = true;
            this.soldRadioButton.Click += new System.EventHandler(this.radioSold_Click);
            // 
            // allRadioButton
            // 
            this.allRadioButton.AutoSize = true;
            this.allRadioButton.Checked = true;
            this.allRadioButton.Location = new System.Drawing.Point(748, 60);
            this.allRadioButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.Size = new System.Drawing.Size(39, 19);
            this.allRadioButton.TabIndex = 25;
            this.allRadioButton.TabStop = true;
            this.allRadioButton.Text = "All";
            this.allRadioButton.UseVisualStyleBackColor = true;
            this.allRadioButton.Click += new System.EventHandler(this.radioAll_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(6, 22);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(238, 23);
            this.searchTextBox.TabIndex = 39;
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = global::OpenDental.Properties.Resources.IconPrinter;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printButton.Location = new System.Drawing.Point(156, 538);
            this.printButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(110, 30);
            this.printButton.TabIndex = 8;
            this.printButton.Text = "Print";
            this.printButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(13, 538);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // equipmentGrid
            // 
            this.equipmentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.equipmentGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.equipmentGrid.EditableEnterMovesDown = false;
            this.equipmentGrid.HasAddButton = false;
            this.equipmentGrid.HasDropDowns = false;
            this.equipmentGrid.HasMultilineHeaders = false;
            this.equipmentGrid.HScrollVisible = false;
            this.equipmentGrid.Location = new System.Drawing.Point(13, 85);
            this.equipmentGrid.Name = "equipmentGrid";
            this.equipmentGrid.ScrollValue = 0;
            this.equipmentGrid.Size = new System.Drawing.Size(918, 447);
            this.equipmentGrid.TabIndex = 5;
            this.equipmentGrid.Title = "Equipment";
            this.equipmentGrid.TitleVisible = true;
            this.equipmentGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.EquipmentGrid_CellDoubleClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(821, 538);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Controls.Add(this.searchTextBox);
            this.searchGroupBox.Location = new System.Drawing.Point(219, 19);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(250, 60);
            this.searchGroupBox.TabIndex = 40;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "SN/Desciption/Location";
            // 
            // FormEquipment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(944, 581);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.allRadioButton);
            this.Controls.Add(this.dateRangeGroupBox);
            this.Controls.Add(this.soldRadioButton);
            this.Controls.Add(this.purchasedRadioButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.equipmentGrid);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(920, 500);
            this.Name = "FormEquipment";
            this.ShowInTaskbar = false;
            this.Text = "Equipment";
            this.Load += new System.EventHandler(this.FormEquipment_Load);
            this.dateRangeGroupBox.ResumeLayout(false);
            this.dateRangeGroupBox.PerformLayout();
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid equipmentGrid;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button printButton;
        private ValidDate dateEndTextBox;
        private ValidDate dateStartTextBox;
        private System.Windows.Forms.GroupBox dateRangeGroupBox;
        private System.Windows.Forms.RadioButton purchasedRadioButton;
        private System.Windows.Forms.RadioButton soldRadioButton;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.GroupBox searchGroupBox;
    }
}