namespace OpenDental
{
    partial class FormEhrEduResourcesPat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrEduResourcesPat));
            this.closeButton = new System.Windows.Forms.Button();
            this.resourcesGrid = new OpenDental.UI.ODGrid();
            this.resourcesLabel = new System.Windows.Forms.Label();
            this.providedGrid = new OpenDental.UI.ODGrid();
            this.providedLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.resourcesSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesSplitContainer)).BeginInit();
            this.resourcesSplitContainer.Panel1.SuspendLayout();
            this.resourcesSplitContainer.Panel2.SuspendLayout();
            this.resourcesSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(861, 568);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // resourcesGrid
            // 
            this.resourcesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.resourcesGrid.EditableEnterMovesDown = false;
            this.resourcesGrid.HasAddButton = false;
            this.resourcesGrid.HasDropDowns = false;
            this.resourcesGrid.HasMultilineHeaders = false;
            this.resourcesGrid.HScrollVisible = false;
            this.resourcesGrid.Location = new System.Drawing.Point(3, 53);
            this.resourcesGrid.Name = "resourcesGrid";
            this.resourcesGrid.ScrollValue = 0;
            this.resourcesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.None;
            this.resourcesGrid.Size = new System.Drawing.Size(952, 264);
            this.resourcesGrid.TabIndex = 1;
            this.resourcesGrid.Title = "Educational Resources";
            this.resourcesGrid.TitleVisible = true;
            this.resourcesGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.ResourcesGrid_CellClick);
            // 
            // resourcesLabel
            // 
            this.resourcesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesLabel.Location = new System.Drawing.Point(3, 0);
            this.resourcesLabel.Name = "resourcesLabel";
            this.resourcesLabel.Size = new System.Drawing.Size(952, 50);
            this.resourcesLabel.TabIndex = 2;
            this.resourcesLabel.Text = resources.GetString("resourcesLabel.Text");
            // 
            // providedGrid
            // 
            this.providedGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.providedGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.providedGrid.EditableEnterMovesDown = false;
            this.providedGrid.HasAddButton = false;
            this.providedGrid.HasDropDowns = false;
            this.providedGrid.HasMultilineHeaders = false;
            this.providedGrid.HScrollVisible = false;
            this.providedGrid.Location = new System.Drawing.Point(3, 23);
            this.providedGrid.Name = "providedGrid";
            this.providedGrid.ScrollValue = 0;
            this.providedGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.providedGrid.Size = new System.Drawing.Size(952, 193);
            this.providedGrid.TabIndex = 3;
            this.providedGrid.Title = "Education Provided";
            this.providedGrid.TitleVisible = true;
            // 
            // providedLabel
            // 
            this.providedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.providedLabel.Location = new System.Drawing.Point(3, 0);
            this.providedLabel.Name = "providedLabel";
            this.providedLabel.Size = new System.Drawing.Size(952, 20);
            this.providedLabel.TabIndex = 4;
            this.providedLabel.Text = "This is a historical record of education resources provided to this patient.  Del" +
    "ete any entries that are inaccurate.";
            this.providedLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(13, 568);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // resourcesSplitContainer
            // 
            this.resourcesSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesSplitContainer.Location = new System.Drawing.Point(13, 19);
            this.resourcesSplitContainer.Name = "resourcesSplitContainer";
            this.resourcesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // resourcesSplitContainer.Panel1
            // 
            this.resourcesSplitContainer.Panel1.Controls.Add(this.resourcesGrid);
            this.resourcesSplitContainer.Panel1.Controls.Add(this.resourcesLabel);
            // 
            // resourcesSplitContainer.Panel2
            // 
            this.resourcesSplitContainer.Panel2.Controls.Add(this.providedGrid);
            this.resourcesSplitContainer.Panel2.Controls.Add(this.providedLabel);
            this.resourcesSplitContainer.Size = new System.Drawing.Size(958, 543);
            this.resourcesSplitContainer.SplitterDistance = 320;
            this.resourcesSplitContainer.TabIndex = 7;
            // 
            // FormEhrEduResourcesPat
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.resourcesSplitContainer);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEhrEduResourcesPat";
            this.Text = "Educational Resources";
            this.Load += new System.EventHandler(this.FormEhrEduResourcesPat_Load);
            this.resourcesSplitContainer.Panel1.ResumeLayout(false);
            this.resourcesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resourcesSplitContainer)).EndInit();
            this.resourcesSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid resourcesGrid;
        private System.Windows.Forms.Label resourcesLabel;
        private OpenDental.UI.ODGrid providedGrid;
        private System.Windows.Forms.Label providedLabel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.SplitContainer resourcesSplitContainer;
    }
}