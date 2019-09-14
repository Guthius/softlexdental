namespace OpenDental
{
    partial class FormPreviousVersions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreviousVersions));
            this.closeButton = new System.Windows.Forms.Button();
            this.historyGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(241, 468);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // historyGrid
            // 
            this.historyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.historyGrid.EditableEnterMovesDown = false;
            this.historyGrid.HasAddButton = false;
            this.historyGrid.HasDropDowns = false;
            this.historyGrid.HasMultilineHeaders = false;
            this.historyGrid.HScrollVisible = false;
            this.historyGrid.Location = new System.Drawing.Point(13, 19);
            this.historyGrid.Name = "historyGrid";
            this.historyGrid.ScrollValue = 0;
            this.historyGrid.Size = new System.Drawing.Size(338, 443);
            this.historyGrid.TabIndex = 0;
            this.historyGrid.Title = null;
            this.historyGrid.TitleVisible = true;
            // 
            // FormPreviousVersions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(364, 511);
            this.Controls.Add(this.historyGrid);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreviousVersions";
            this.ShowInTaskbar = false;
            this.Text = "Previous Versions";
            this.Load += new System.EventHandler(this.FormPreviousVersions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid historyGrid;
    }
}