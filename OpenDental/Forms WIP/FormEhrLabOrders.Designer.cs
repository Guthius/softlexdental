namespace OpenDental
{
    partial class FormEhrLabOrders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrLabOrders));
            this.addButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.ordersGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(851, 55);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Location = new System.Drawing.Point(851, 19);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(110, 30);
            this.importButton.TabIndex = 7;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(851, 359);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // ordersGrid
            // 
            this.ordersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.ordersGrid.EditableEnterMovesDown = false;
            this.ordersGrid.HasAddButton = false;
            this.ordersGrid.HasDropDowns = false;
            this.ordersGrid.HasMultilineHeaders = false;
            this.ordersGrid.HScrollVisible = false;
            this.ordersGrid.Location = new System.Drawing.Point(13, 19);
            this.ordersGrid.Name = "ordersGrid";
            this.ordersGrid.ScrollValue = 0;
            this.ordersGrid.Size = new System.Drawing.Size(832, 370);
            this.ordersGrid.TabIndex = 5;
            this.ordersGrid.Title = "Laboratory Orders";
            this.ordersGrid.TitleVisible = true;
            this.ordersGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OrdersGrid_CellDoubleClick);
            // 
            // FormEhrLabOrders
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(974, 402);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.ordersGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEhrLabOrders";
            this.Text = "Lab Orders";
            this.Load += new System.EventHandler(this.FormEhrLabOrders_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button importButton;
        private OpenDental.UI.ODGrid ordersGrid;
        private System.Windows.Forms.Button closeButton;
    }
}