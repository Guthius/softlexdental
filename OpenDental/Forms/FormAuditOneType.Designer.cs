namespace OpenDental
{
    partial class FormAuditOneType
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAuditOneType));
            this.logGrid = new OpenDental.UI.ODGrid();
            this.includeArchivedCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // logGrid
            // 
            this.logGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.logGrid.EditableEnterMovesDown = false;
            this.logGrid.HasAddButton = false;
            this.logGrid.HasDropDowns = false;
            this.logGrid.HasMultilineHeaders = false;
            this.logGrid.HScrollVisible = false;
            this.logGrid.Location = new System.Drawing.Point(13, 19);
            this.logGrid.Name = "logGrid";
            this.logGrid.ScrollValue = 0;
            this.logGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.logGrid.Size = new System.Drawing.Size(918, 553);
            this.logGrid.TabIndex = 0;
            this.logGrid.Title = "Audit Trail";
            this.logGrid.TitleVisible = true;
            // 
            // includeArchivedCheckBox
            // 
            this.includeArchivedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.includeArchivedCheckBox.AutoSize = true;
            this.includeArchivedCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.includeArchivedCheckBox.Location = new System.Drawing.Point(13, 578);
            this.includeArchivedCheckBox.Name = "includeArchivedCheckBox";
            this.includeArchivedCheckBox.Size = new System.Drawing.Size(121, 20);
            this.includeArchivedCheckBox.TabIndex = 1;
            this.includeArchivedCheckBox.Text = "Include Archived";
            this.includeArchivedCheckBox.UseVisualStyleBackColor = true;
            this.includeArchivedCheckBox.CheckedChanged += new System.EventHandler(this.includeArchivedCheckBox_CheckedChanged);
            // 
            // FormAuditOneType
            // 
            this.ClientSize = new System.Drawing.Size(944, 611);
            this.Controls.Add(this.includeArchivedCheckBox);
            this.Controls.Add(this.logGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAuditOneType";
            this.ShowInTaskbar = false;
            this.Text = "Audit Trail";
            this.Load += new System.EventHandler(this.FormAuditOneType_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.ODGrid logGrid;
        private System.Windows.Forms.CheckBox includeArchivedCheckBox;
    }
}
