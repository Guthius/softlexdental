namespace OpenDental
{
    partial class FormOperatoryPick
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatoryPick));
            this.operatoriesGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // operatoriesGrid
            // 
            this.operatoriesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operatoriesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.operatoriesGrid.EditableEnterMovesDown = false;
            this.operatoriesGrid.HasAddButton = false;
            this.operatoriesGrid.HasDropDowns = false;
            this.operatoriesGrid.HasMultilineHeaders = false;
            this.operatoriesGrid.HScrollVisible = false;
            this.operatoriesGrid.Location = new System.Drawing.Point(15, 21);
            this.operatoriesGrid.Name = "operatoriesGrid";
            this.operatoriesGrid.ScrollValue = 0;
            this.operatoriesGrid.Size = new System.Drawing.Size(804, 389);
            this.operatoriesGrid.TabIndex = 0;
            this.operatoriesGrid.Title = "Select the Operatory to Keep";
            this.operatoriesGrid.TitleVisible = true;
            this.operatoriesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OperatoriesGrid_CellDoubleClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(593, 416);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(709, 416);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormOperatoryPick
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.operatoriesGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 500);
            this.Name = "FormOperatoryPick";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operatory Pick";
            this.Load += new System.EventHandler(this.FormOperatoryPick_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.ODGrid operatoriesGrid;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
    }
}
