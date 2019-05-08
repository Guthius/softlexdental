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
            this.checkUnattached = new System.Windows.Forms.CheckBox();
            this.gridMain = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(265, 478);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(110, 30);
            this.butOK.TabIndex = 2;
            this.butOK.Text = "&OK";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(381, 478);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(110, 30);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "&Cancel";
            // 
            // checkUnattached
            // 
            this.checkUnattached.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkUnattached.AutoSize = true;
            this.checkUnattached.Location = new System.Drawing.Point(13, 478);
            this.checkUnattached.Name = "checkUnattached";
            this.checkUnattached.Size = new System.Drawing.Size(147, 19);
            this.checkUnattached.TabIndex = 1;
            this.checkUnattached.Text = "Show Unattached Only";
            this.checkUnattached.UseVisualStyleBackColor = true;
            this.checkUnattached.Click += new System.EventHandler(this.checkUnattached_Click);
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridMain.EditableEnterMovesDown = false;
            this.gridMain.HasAddButton = false;
            this.gridMain.HasDropDowns = false;
            this.gridMain.HasMultilineHeaders = false;
            this.gridMain.HScrollVisible = false;
            this.gridMain.Location = new System.Drawing.Point(13, 19);
            this.gridMain.Name = "gridMain";
            this.gridMain.ScrollValue = 0;
            this.gridMain.Size = new System.Drawing.Size(478, 453);
            this.gridMain.TabIndex = 0;
            this.gridMain.Title = "Adjustments";
            this.gridMain.TitleVisible = true;
            this.gridMain.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
            // 
            // FormAdjustmentPicker
            // 
            this.AcceptButton = this.butOK;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(504, 521);
            this.Controls.Add(this.checkUnattached);
            this.Controls.Add(this.gridMain);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.CheckBox checkUnattached;
        private UI.ODGrid gridMain;
    }
}