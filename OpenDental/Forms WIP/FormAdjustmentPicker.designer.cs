namespace OpenDental
{
    partial class FormAdjustmentPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjustmentPicker));
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.unattachedCheckBox = new System.Windows.Forms.CheckBox();
            this.adjustmentGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(265, 478);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(110, 30);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "&OK";
            this.butOK.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(381, 478);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(110, 30);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "&Cancel";
            // 
            // unattachedCheckBox
            // 
            this.unattachedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unattachedCheckBox.AutoSize = true;
            this.unattachedCheckBox.Location = new System.Drawing.Point(13, 485);
            this.unattachedCheckBox.Name = "unattachedCheckBox";
            this.unattachedCheckBox.Size = new System.Drawing.Size(147, 19);
            this.unattachedCheckBox.TabIndex = 3;
            this.unattachedCheckBox.Text = "Show Unattached Only";
            this.unattachedCheckBox.UseVisualStyleBackColor = true;
            this.unattachedCheckBox.Click += new System.EventHandler(this.UnattachedCheckBox_Click);
            // 
            // adjustmentGrid
            // 
            this.adjustmentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adjustmentGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.adjustmentGrid.EditableEnterMovesDown = false;
            this.adjustmentGrid.HasAddButton = false;
            this.adjustmentGrid.HasDropDowns = false;
            this.adjustmentGrid.HasMultilineHeaders = false;
            this.adjustmentGrid.HScrollVisible = false;
            this.adjustmentGrid.Location = new System.Drawing.Point(13, 19);
            this.adjustmentGrid.Name = "adjustmentGrid";
            this.adjustmentGrid.ScrollValue = 0;
            this.adjustmentGrid.Size = new System.Drawing.Size(478, 453);
            this.adjustmentGrid.TabIndex = 2;
            this.adjustmentGrid.Title = "Adjustments";
            this.adjustmentGrid.TitleVisible = true;
            this.adjustmentGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AdjustmentGrid_CellDoubleClick);
            // 
            // FormAdjustmentPicker
            // 
            this.AcceptButton = this.butOK;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(504, 521);
            this.Controls.Add(this.unattachedCheckBox);
            this.Controls.Add(this.adjustmentGrid);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 560);
            this.Name = "FormAdjustmentPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjustment Picker";
            this.Load += new System.EventHandler(this.FormAdjustmentPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox unattachedCheckBox;
        private UI.ODGrid adjustmentGrid;
    }
}