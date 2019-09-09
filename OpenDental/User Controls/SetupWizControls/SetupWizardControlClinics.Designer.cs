namespace OpenDental.User_Controls.SetupWizard
{
    partial class SetupWizardControlClinics
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.advancedLabel = new System.Windows.Forms.Label();
            this.clinicsGrid = new OpenDental.UI.ODGrid();
            this.helpLabel = new System.Windows.Forms.Label();
            this.advancedButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(43, 40);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(500, 30);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Here are your currently set up clinics. Click \'Add\' to add more.\r\nEach clinic mus" +
    "t have a description, abbreviation, phone number, and address.";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(43, 477);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // advancedLabel
            // 
            this.advancedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedLabel.Location = new System.Drawing.Point(159, 480);
            this.advancedLabel.Name = "advancedLabel";
            this.advancedLabel.Size = new System.Drawing.Size(632, 25);
            this.advancedLabel.TabIndex = 4;
            this.advancedLabel.Text = "Further modifications to this list can be made by going to Lists -> Clinics, or c" +
    "licking \"Advanced\".\r\n";
            this.advancedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clinicsGrid
            // 
            this.clinicsGrid.AllowSortingByColumn = true;
            this.clinicsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.clinicsGrid.EditableEnterMovesDown = false;
            this.clinicsGrid.HasAddButton = false;
            this.clinicsGrid.HasDropDowns = false;
            this.clinicsGrid.HasMultilineHeaders = false;
            this.clinicsGrid.HScrollVisible = true;
            this.clinicsGrid.Location = new System.Drawing.Point(43, 73);
            this.clinicsGrid.Name = "clinicsGrid";
            this.clinicsGrid.ScrollValue = 0;
            this.clinicsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.clinicsGrid.Size = new System.Drawing.Size(864, 398);
            this.clinicsGrid.TabIndex = 2;
            this.clinicsGrid.Title = "Clinics";
            this.clinicsGrid.TitleVisible = true;
            this.clinicsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.ClinicsGrid_CellDoubleClick);
            // 
            // helpLabel
            // 
            this.helpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpLabel.Location = new System.Drawing.Point(607, 40);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(300, 30);
            this.helpLabel.TabIndex = 1;
            this.helpLabel.Text = "Items that need attention are highlighted in red.\r\nDouble click a row to edit the" +
    " specific clinic.";
            this.helpLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // advancedButton
            // 
            this.advancedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedButton.Image = global::OpenDental.Properties.Resources.IconCog;
            this.advancedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.advancedButton.Location = new System.Drawing.Point(797, 480);
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.Size = new System.Drawing.Size(110, 30);
            this.advancedButton.TabIndex = 5;
            this.advancedButton.Text = "Advanced";
            this.advancedButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.advancedButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.advancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
            // 
            // SetupWizardControlClinics
            // 
            this.Controls.Add(this.advancedButton);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.clinicsGrid);
            this.Controls.Add(this.advancedLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.infoLabel);
            this.Name = "SetupWizardControlClinics";
            this.Size = new System.Drawing.Size(950, 550);
            this.Load += new System.EventHandler(this.SetupWizardControlClinics_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label advancedLabel;
        private OpenDental.UI.ODGrid clinicsGrid;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.Button advancedButton;
    }
}