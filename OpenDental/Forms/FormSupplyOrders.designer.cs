namespace OpenDental
{
    partial class FormSupplyOrders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplyOrders));
            this.supplierComboBox = new System.Windows.Forms.ComboBox();
            this.supplierLabel = new System.Windows.Forms.Label();
            this.orderItemsGrid = new OpenDental.UI.ODGrid();
            this.ordersGrid = new OpenDental.UI.ODGrid();
            this.butPrint = new System.Windows.Forms.Button();
            this.addOrderItemButton = new System.Windows.Forms.Button();
            this.newOrderButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // supplierComboBox
            // 
            this.supplierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.supplierComboBox.FormattingEnabled = true;
            this.supplierComboBox.Location = new System.Drawing.Point(500, 24);
            this.supplierComboBox.Name = "supplierComboBox";
            this.supplierComboBox.Size = new System.Drawing.Size(160, 23);
            this.supplierComboBox.TabIndex = 13;
            this.supplierComboBox.SelectedIndexChanged += new System.EventHandler(this.SupplierComboBox_SelectedIndexChanged);
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(444, 27);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(50, 15);
            this.supplierLabel.TabIndex = 14;
            this.supplierLabel.Text = "Supplier";
            this.supplierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // orderItemsGrid
            // 
            this.orderItemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orderItemsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.orderItemsGrid.EditableEnterMovesDown = false;
            this.orderItemsGrid.HasAddButton = false;
            this.orderItemsGrid.HasDropDowns = false;
            this.orderItemsGrid.HasMultilineHeaders = false;
            this.orderItemsGrid.HScrollVisible = false;
            this.orderItemsGrid.Location = new System.Drawing.Point(13, 231);
            this.orderItemsGrid.Name = "orderItemsGrid";
            this.orderItemsGrid.ScrollValue = 0;
            this.orderItemsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
            this.orderItemsGrid.Size = new System.Drawing.Size(858, 361);
            this.orderItemsGrid.TabIndex = 17;
            this.orderItemsGrid.Title = "Supplies on One Order";
            this.orderItemsGrid.TitleVisible = true;
            this.orderItemsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OrderItemsGrid_CellDoubleClick);
            this.orderItemsGrid.CellLeave += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OrderItemsGrid_CellLeave);
            // 
            // ordersGrid
            // 
            this.ordersGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.ordersGrid.EditableEnterMovesDown = false;
            this.ordersGrid.HasAddButton = false;
            this.ordersGrid.HasDropDowns = false;
            this.ordersGrid.HasMultilineHeaders = false;
            this.ordersGrid.HScrollVisible = false;
            this.ordersGrid.Location = new System.Drawing.Point(13, 55);
            this.ordersGrid.Name = "ordersGrid";
            this.ordersGrid.ScrollValue = 0;
            this.ordersGrid.Size = new System.Drawing.Size(647, 170);
            this.ordersGrid.TabIndex = 15;
            this.ordersGrid.Title = "Order History";
            this.ordersGrid.TitleVisible = true;
            this.ordersGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OrdersGrid_CellDoubleClick);
            this.ordersGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OrdersGrid_CellClick);
            // 
            // butPrint
            // 
            this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butPrint.Image = global::OpenDental.Properties.Resources.IconPrinter;
            this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.butPrint.Location = new System.Drawing.Point(13, 598);
            this.butPrint.Name = "butPrint";
            this.butPrint.Size = new System.Drawing.Size(110, 30);
            this.butPrint.TabIndex = 26;
            this.butPrint.Text = "Print";
            this.butPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.butPrint.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // addOrderItemButton
            // 
            this.addOrderItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addOrderItemButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addOrderItemButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addOrderItemButton.Location = new System.Drawing.Point(761, 195);
            this.addOrderItemButton.Name = "addOrderItemButton";
            this.addOrderItemButton.Size = new System.Drawing.Size(110, 30);
            this.addOrderItemButton.TabIndex = 25;
            this.addOrderItemButton.Text = "Add";
            this.addOrderItemButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addOrderItemButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addOrderItemButton.Click += new System.EventHandler(this.AddOrderItemButton_Click);
            // 
            // newOrderButton
            // 
            this.newOrderButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.newOrderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.newOrderButton.Location = new System.Drawing.Point(13, 19);
            this.newOrderButton.Name = "newOrderButton";
            this.newOrderButton.Size = new System.Drawing.Size(110, 30);
            this.newOrderButton.TabIndex = 16;
            this.newOrderButton.Text = "New Order";
            this.newOrderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newOrderButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.newOrderButton.Click += new System.EventHandler(this.NewOrderButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(645, 598);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(761, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormSupplyOrders
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 641);
            this.Controls.Add(this.butPrint);
            this.Controls.Add(this.addOrderItemButton);
            this.Controls.Add(this.orderItemsGrid);
            this.Controls.Add(this.newOrderButton);
            this.Controls.Add(this.ordersGrid);
            this.Controls.Add(this.supplierComboBox);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 680);
            this.Name = "FormSupplyOrders";
            this.ShowInTaskbar = false;
            this.Text = "Supply Orders";
            this.Load += new System.EventHandler(this.FormSupplyOrders_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button newOrderButton;
        private UI.ODGrid ordersGrid;
        private System.Windows.Forms.ComboBox supplierComboBox;
        private System.Windows.Forms.Label supplierLabel;
        private UI.ODGrid orderItemsGrid;
        private System.Windows.Forms.Button addOrderItemButton;
        private System.Windows.Forms.Button butPrint;
    }
}