namespace OpenDental
{
    partial class FormSetupWizard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetupWizard));
            this.gridImageList = new System.Windows.Forms.ImageList(this.components);
            this.infoLabel = new System.Windows.Forms.Label();
            this.wizardsGrid = new OpenDental.UI.ODGrid();
            this.allButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gridImageList
            // 
            this.gridImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("gridImageList.ImageStream")));
            this.gridImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.gridImageList.Images.SetKeyName(0, "iButton_Blue.png");
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(438, 60);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Double click a row to set it up individually. \r\nDouble click a category to set up" +
    " all for that category.\r\nClick \"Set Up All\" to go through the entire setup seque" +
    "nce.";
            // 
            // wizardsGrid
            // 
            this.wizardsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wizardsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.wizardsGrid.EditableEnterMovesDown = false;
            this.wizardsGrid.HasAddButton = false;
            this.wizardsGrid.HasDropDowns = true;
            this.wizardsGrid.HasMultilineHeaders = false;
            this.wizardsGrid.HScrollVisible = false;
            this.wizardsGrid.Location = new System.Drawing.Point(13, 79);
            this.wizardsGrid.Name = "wizardsGrid";
            this.wizardsGrid.ScrollValue = 0;
            this.wizardsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.wizardsGrid.Size = new System.Drawing.Size(438, 413);
            this.wizardsGrid.TabIndex = 1;
            this.wizardsGrid.Title = "Setup";
            this.wizardsGrid.TitleVisible = true;
            this.wizardsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.WizardsGrid_CellDoubleClick);
            this.wizardsGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.WizardsGrid_CellClick);
            // 
            // allButton
            // 
            this.allButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.allButton.Location = new System.Drawing.Point(13, 498);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(110, 30);
            this.allButton.TabIndex = 2;
            this.allButton.Text = "Set Up All";
            this.allButton.Click += new System.EventHandler(this.AllButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 498);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Close";
            // 
            // FormSetupWizard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 541);
            this.Controls.Add(this.allButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.wizardsGrid);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetupWizard";
            this.ShowInTaskbar = false;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.FormSetupWizard_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid wizardsGrid;
        private System.Windows.Forms.ImageList gridImageList;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button allButton;
    }
}