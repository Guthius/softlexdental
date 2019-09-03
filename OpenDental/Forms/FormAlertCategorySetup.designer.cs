namespace OpenDental
{
    partial class FormAlertCategorySetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlertCategorySetup));
            this.duplicateButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.internalGrid = new OpenDental.UI.ODGrid();
            this.customGrid = new OpenDental.UI.ODGrid();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.duplicateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.duplicateButton.Location = new System.Drawing.Point(545, 518);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(110, 30);
            this.duplicateButton.TabIndex = 3;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.duplicateButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.duplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copyButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowRight;
            this.copyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copyButton.Location = new System.Drawing.Point(337, 220);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(110, 30);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.copyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // internalGrid
            // 
            this.internalGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.internalGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.internalGrid.EditableEnterMovesDown = false;
            this.internalGrid.HasAddButton = false;
            this.internalGrid.HasDropDowns = false;
            this.internalGrid.HasMultilineHeaders = false;
            this.internalGrid.HScrollVisible = false;
            this.internalGrid.Location = new System.Drawing.Point(13, 19);
            this.internalGrid.Name = "internalGrid";
            this.internalGrid.ScrollValue = 0;
            this.internalGrid.Size = new System.Drawing.Size(318, 493);
            this.internalGrid.TabIndex = 0;
            this.internalGrid.Title = "Internal";
            this.internalGrid.TitleVisible = true;
            this.internalGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.InternalGrid_CellDoubleClick);
            // 
            // customGrid
            // 
            this.customGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.customGrid.EditableEnterMovesDown = false;
            this.customGrid.HasAddButton = false;
            this.customGrid.HasDropDowns = false;
            this.customGrid.HasMultilineHeaders = false;
            this.customGrid.HScrollVisible = false;
            this.customGrid.Location = new System.Drawing.Point(453, 19);
            this.customGrid.Name = "customGrid";
            this.customGrid.ScrollValue = 0;
            this.customGrid.Size = new System.Drawing.Size(318, 493);
            this.customGrid.TabIndex = 2;
            this.customGrid.Title = "Custom";
            this.customGrid.TitleVisible = true;
            this.customGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.CustomGrid_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(661, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Close";
            // 
            // FormAlertCategorySetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.internalGrid);
            this.Controls.Add(this.customGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlertCategorySetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alert Category Setup";
            this.Load += new System.EventHandler(this.FormAlertCategorySetup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button duplicateButton;
        private System.Windows.Forms.Button copyButton;
        private UI.ODGrid internalGrid;
        private UI.ODGrid customGrid;
        private System.Windows.Forms.Button cancelButton;
    }
}