namespace OpenDental
{
    partial class FormSuppliers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSuppliers));
            this.closeButton = new System.Windows.Forms.Button();
            this.suppliersGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(821, 598);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // suppliersGrid
            // 
            this.suppliersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suppliersGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.suppliersGrid.EditableEnterMovesDown = false;
            this.suppliersGrid.HasAddButton = false;
            this.suppliersGrid.HasDropDowns = false;
            this.suppliersGrid.HasMultilineHeaders = false;
            this.suppliersGrid.HScrollVisible = false;
            this.suppliersGrid.Location = new System.Drawing.Point(13, 19);
            this.suppliersGrid.Name = "suppliersGrid";
            this.suppliersGrid.ScrollValue = 0;
            this.suppliersGrid.Size = new System.Drawing.Size(918, 566);
            this.suppliersGrid.TabIndex = 0;
            this.suppliersGrid.Title = "Suppliers";
            this.suppliersGrid.TitleVisible = true;
            this.suppliersGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.SuppliersGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(13, 598);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // FormSuppliers
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(944, 641);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.suppliersGrid);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(960, 680);
            this.Name = "FormSuppliers";
            this.ShowInTaskbar = false;
            this.Text = "Suppliers";
            this.Load += new System.EventHandler(this.FormSuppliers_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid suppliersGrid;
        private System.Windows.Forms.Button addButton;
    }
}