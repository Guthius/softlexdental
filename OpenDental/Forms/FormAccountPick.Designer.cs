namespace OpenDental
{
    partial class FormAccountPick
    {
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountPick));
            this.checkInactive = new System.Windows.Forms.CheckBox();
            this.accountsGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkInactive
            // 
            this.checkInactive.AutoSize = true;
            this.checkInactive.Location = new System.Drawing.Point(24, 646);
            this.checkInactive.Name = "checkInactive";
            this.checkInactive.Size = new System.Drawing.Size(162, 19);
            this.checkInactive.TabIndex = 2;
            this.checkInactive.Text = "Include Inactive Accounts";
            this.checkInactive.UseVisualStyleBackColor = true;
            this.checkInactive.Click += new System.EventHandler(this.InactiveCheckBox_Click);
            // 
            // accountsGrid
            // 
            this.accountsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.accountsGrid.EditableEnterMovesDown = false;
            this.accountsGrid.HasAddButton = false;
            this.accountsGrid.HasDropDowns = false;
            this.accountsGrid.HasMultilineHeaders = false;
            this.accountsGrid.HScrollVisible = false;
            this.accountsGrid.Location = new System.Drawing.Point(13, 19);
            this.accountsGrid.Name = "accountsGrid";
            this.accountsGrid.ScrollValue = 0;
            this.accountsGrid.Size = new System.Drawing.Size(458, 533);
            this.accountsGrid.TabIndex = 0;
            this.accountsGrid.Title = "Accounts";
            this.accountsGrid.TitleVisible = true;
            this.accountsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AccountsGrid_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(361, 558);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(245, 558);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormAccountPick
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 601);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.checkInactive);
            this.Controls.Add(this.accountsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountPick";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pick Account";
            this.Load += new System.EventHandler(this.FormAccountPick_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.ODGrid accountsGrid;
        private System.Windows.Forms.CheckBox checkInactive;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
    }
}