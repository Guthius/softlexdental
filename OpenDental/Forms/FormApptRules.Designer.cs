namespace OpenDental
{
    partial class FormApptRules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptRules));
            this.infoLabel = new System.Windows.Forms.Label();
            this.rulesGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(758, 60);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            // 
            // rulesGrid
            // 
            this.rulesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rulesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.rulesGrid.EditableEnterMovesDown = false;
            this.rulesGrid.HasAddButton = false;
            this.rulesGrid.HasDropDowns = false;
            this.rulesGrid.HasMultilineHeaders = false;
            this.rulesGrid.HScrollVisible = false;
            this.rulesGrid.Location = new System.Drawing.Point(13, 79);
            this.rulesGrid.Name = "rulesGrid";
            this.rulesGrid.ScrollValue = 0;
            this.rulesGrid.Size = new System.Drawing.Size(758, 413);
            this.rulesGrid.TabIndex = 2;
            this.rulesGrid.Title = "Appointment Rules";
            this.rulesGrid.TitleVisible = true;
            this.rulesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.RulesGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(13, 498);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(661, 498);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FormApptRules
            // 
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.rulesGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptRules";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Rules";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptRules_FormClosing);
            this.Load += new System.EventHandler(this.FormApptRules_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid rulesGrid;
        private System.Windows.Forms.Label infoLabel;
    }
}