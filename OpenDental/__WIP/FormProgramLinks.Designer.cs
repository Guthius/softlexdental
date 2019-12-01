namespace OpenDental
{
    partial class FormProgramLinks
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinks));
            this.closeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.programsGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(341, 498);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 38;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(13, 498);
            this.addButton.Name = "addButton";
            this.addButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 41;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(438, 41);
            this.infoLabel.TabIndex = 43;
            this.infoLabel.Text = "Double click on one of the programs in the list below to change its settings";
            // 
            // programsGrid
            // 
            this.programsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.programsGrid.EditableEnterMovesDown = false;
            this.programsGrid.HasAddButton = false;
            this.programsGrid.HasDropDowns = false;
            this.programsGrid.HasMultilineHeaders = false;
            this.programsGrid.HScrollVisible = false;
            this.programsGrid.Location = new System.Drawing.Point(13, 60);
            this.programsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.programsGrid.Name = "programsGrid";
            this.programsGrid.ScrollValue = 0;
            this.programsGrid.Size = new System.Drawing.Size(438, 425);
            this.programsGrid.TabIndex = 45;
            this.programsGrid.Title = "Programs";
            this.programsGrid.TitleVisible = true;
            this.programsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.ProgramsGrid_CellDoubleClick);
            // 
            // FormProgramLinks
            // 
            this.ClientSize = new System.Drawing.Size(464, 541);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.programsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(405, 195);
            this.Name = "FormProgramLinks";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Links";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinks_Closing);
            this.Load += new System.EventHandler(this.FormProgramLinks_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label infoLabel;
        private UI.ODGrid programsGrid;
    }
}
