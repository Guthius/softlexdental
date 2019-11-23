namespace OpenDental
{
    partial class FormSupplyInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplyInventory));
            this.suppliesGrid = new OpenDental.UI.ODGrid();
            this.addNeededButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.equipmentButton = new System.Windows.Forms.Button();
            this.printButton = new System.Windows.Forms.Button();
            this.ordersButton = new System.Windows.Forms.Button();
            this.suppliesButton = new System.Windows.Forms.Button();
            this.suppliersButton = new System.Windows.Forms.Button();
            this.categoriesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.suppliesGrid.Location = new System.Drawing.Point(13, 55);
            this.suppliesGrid.Name = "suppliesGrid";
            this.suppliesGrid.ScrollValue = 0;
            this.suppliesGrid.Size = new System.Drawing.Size(558, 557);
            this.suppliesGrid.TabIndex = 0;
            this.suppliesGrid.Title = "Supplies Needed";
            this.suppliesGrid.TitleVisible = true;
            this.suppliesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.SuppliesGrid_CellDoubleClick);
            // 
            // addNeededButton
            // 
            this.addNeededButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addNeededButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addNeededButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addNeededButton.Location = new System.Drawing.Point(13, 618);
            this.addNeededButton.Name = "addNeededButton";
            this.addNeededButton.Size = new System.Drawing.Size(110, 30);
            this.addNeededButton.TabIndex = 1;
            this.addNeededButton.Text = "Add";
            this.addNeededButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addNeededButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addNeededButton.Click += new System.EventHandler(this.AddNeededButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(461, 618);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // equipmentButton
            // 
            this.equipmentButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.equipmentButton.Location = new System.Drawing.Point(331, 19);
            this.equipmentButton.Name = "equipmentButton";
            this.equipmentButton.Size = new System.Drawing.Size(100, 30);
            this.equipmentButton.TabIndex = 7;
            this.equipmentButton.Text = "Equipment";
            this.equipmentButton.Click += new System.EventHandler(this.EquipmentButton_Click);
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = global::OpenDental.Properties.Resources.IconPrinter;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printButton.Location = new System.Drawing.Point(156, 618);
            this.printButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(110, 30);
            this.printButton.TabIndex = 2;
            this.printButton.Text = "Print";
            this.printButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // ordersButton
            // 
            this.ordersButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ordersButton.Location = new System.Drawing.Point(437, 19);
            this.ordersButton.Name = "ordersButton";
            this.ordersButton.Size = new System.Drawing.Size(100, 30);
            this.ordersButton.TabIndex = 8;
            this.ordersButton.Text = "Orders";
            this.ordersButton.Click += new System.EventHandler(this.OrdersButton_Click);
            // 
            // suppliesButton
            // 
            this.suppliesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.suppliesButton.Location = new System.Drawing.Point(225, 19);
            this.suppliesButton.Name = "suppliesButton";
            this.suppliesButton.Size = new System.Drawing.Size(100, 30);
            this.suppliesButton.TabIndex = 6;
            this.suppliesButton.Text = "Supplies";
            this.suppliesButton.Click += new System.EventHandler(this.SuppliesButton_Click);
            // 
            // suppliersButton
            // 
            this.suppliersButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.suppliersButton.Location = new System.Drawing.Point(119, 19);
            this.suppliersButton.Name = "suppliersButton";
            this.suppliersButton.Size = new System.Drawing.Size(100, 30);
            this.suppliersButton.TabIndex = 5;
            this.suppliersButton.Text = "Suppliers";
            this.suppliersButton.Click += new System.EventHandler(this.SuppliersButton_Click);
            // 
            // categoriesButton
            // 
            this.categoriesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.categoriesButton.Location = new System.Drawing.Point(13, 19);
            this.categoriesButton.Name = "categoriesButton";
            this.categoriesButton.Size = new System.Drawing.Size(100, 30);
            this.categoriesButton.TabIndex = 4;
            this.categoriesButton.Text = "Categories";
            this.categoriesButton.Click += new System.EventHandler(this.CategoriesButton_Click);
            // 
            // FormSupplyInventory
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(584, 661);
            this.Controls.Add(this.categoriesButton);
            this.Controls.Add(this.suppliersButton);
            this.Controls.Add(this.suppliesGrid);
            this.Controls.Add(this.addNeededButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.equipmentButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.ordersButton);
            this.Controls.Add(this.suppliesButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(510, 500);
            this.Name = "FormSupplyInventory";
            this.ShowInTaskbar = false;
            this.Text = "Supply Inventory";
            this.Load += new System.EventHandler(this.FormInventory_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button addNeededButton;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button suppliesButton;
        private System.Windows.Forms.Button ordersButton;
        private System.Windows.Forms.Button equipmentButton;
        private UI.ODGrid suppliesGrid;
        private System.Windows.Forms.Button suppliersButton;
        private System.Windows.Forms.Button categoriesButton;
    }
}