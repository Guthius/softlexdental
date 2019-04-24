namespace OpenDental
{
    partial class ContrFamily
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.menuInsurance = new System.Windows.Forms.ContextMenu();
            this.menuPlansForFam = new System.Windows.Forms.MenuItem();
            this.menuDiscount = new System.Windows.Forms.ContextMenu();
            this.menuItemRemoveDiscount = new System.Windows.Forms.MenuItem();
            this.gridSuperFam = new OpenDental.UI.ODGrid();
            this.gridRecall = new OpenDental.UI.ODGrid();
            this.gridFamily = new OpenDental.UI.ODGrid();
            this.gridPat = new OpenDental.UI.ODGrid();
            this.gridIns = new OpenDental.UI.ODGrid();
            this.superClonesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.gridPatientClones = new OpenDental.UI.ODGrid();
            this.patientPictureBox = new OpenDental.UI.ODPictureBox();
            this.ToolBarMain = new OpenDental.UI.ODToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.superClonesSplitContainer)).BeginInit();
            this.superClonesSplitContainer.Panel1.SuspendLayout();
            this.superClonesSplitContainer.Panel2.SuspendLayout();
            this.superClonesSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuInsurance
            // 
            this.menuInsurance.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPlansForFam});
            // 
            // menuPlansForFam
            // 
            this.menuPlansForFam.Index = 0;
            this.menuPlansForFam.Text = "Plans for Family";
            this.menuPlansForFam.Click += new System.EventHandler(this.menuPlansForFam_Click);
            // 
            // menuDiscount
            // 
            this.menuDiscount.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRemoveDiscount});
            // 
            // menuItemRemoveDiscount
            // 
            this.menuItemRemoveDiscount.Index = 0;
            this.menuItemRemoveDiscount.Text = "Drop Discount Plan";
            this.menuItemRemoveDiscount.Click += new System.EventHandler(this.menuItemRemoveDiscount_Click);
            // 
            // gridSuperFam
            // 
            this.gridSuperFam.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridSuperFam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSuperFam.EditableEnterMovesDown = false;
            this.gridSuperFam.HasAddButton = false;
            this.gridSuperFam.HasDropDowns = false;
            this.gridSuperFam.HasMultilineHeaders = false;
            this.gridSuperFam.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridSuperFam.HeaderHeight = 15;
            this.gridSuperFam.HScrollVisible = false;
            this.gridSuperFam.Location = new System.Drawing.Point(0, 0);
            this.gridSuperFam.Name = "gridSuperFam";
            this.gridSuperFam.ScrollValue = 0;
            this.gridSuperFam.Size = new System.Drawing.Size(320, 250);
            this.gridSuperFam.TabIndex = 33;
            this.gridSuperFam.Title = "Super Family";
            this.gridSuperFam.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridSuperFam.TitleHeight = 18;
            this.gridSuperFam.TranslationName = "TableSuper";
            this.gridSuperFam.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridSuperFam_CellDoubleClick);
            this.gridSuperFam.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridSuperFam_CellClick);
            // 
            // gridRecall
            // 
            this.gridRecall.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridRecall.EditableEnterMovesDown = false;
            this.gridRecall.HasAddButton = false;
            this.gridRecall.HasDropDowns = false;
            this.gridRecall.HasMultilineHeaders = false;
            this.gridRecall.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridRecall.HeaderHeight = 15;
            this.gridRecall.HScrollVisible = true;
            this.gridRecall.Location = new System.Drawing.Point(595, 36);
            this.gridRecall.Name = "gridRecall";
            this.gridRecall.ScrollValue = 0;
            this.gridRecall.SelectionMode = OpenDental.UI.GridSelectionMode.None;
            this.gridRecall.Size = new System.Drawing.Size(525, 100);
            this.gridRecall.TabIndex = 32;
            this.gridRecall.Title = "Recall";
            this.gridRecall.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridRecall.TitleHeight = 18;
            this.gridRecall.TranslationName = "TableRecall";
            this.gridRecall.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridRecall_CellDoubleClick);
            this.gridRecall.DoubleClick += new System.EventHandler(this.gridRecall_DoubleClick);
            // 
            // gridFamily
            // 
            this.gridFamily.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridFamily.EditableEnterMovesDown = false;
            this.gridFamily.HasAddButton = false;
            this.gridFamily.HasDropDowns = false;
            this.gridFamily.HasMultilineHeaders = false;
            this.gridFamily.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridFamily.HeaderHeight = 15;
            this.gridFamily.HScrollVisible = false;
            this.gridFamily.Location = new System.Drawing.Point(109, 36);
            this.gridFamily.Name = "gridFamily";
            this.gridFamily.ScrollValue = 0;
            this.gridFamily.SelectedRowColor = System.Drawing.Color.DarkSalmon;
            this.gridFamily.Size = new System.Drawing.Size(480, 100);
            this.gridFamily.TabIndex = 31;
            this.gridFamily.Title = "Family Members";
            this.gridFamily.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridFamily.TitleHeight = 18;
            this.gridFamily.TranslationName = "TableFamily";
            this.gridFamily.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridFamily_CellDoubleClick);
            this.gridFamily.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridFamily_CellClick);
            // 
            // gridPat
            // 
            this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridPat.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridPat.EditableEnterMovesDown = false;
            this.gridPat.HasAddButton = false;
            this.gridPat.HasDropDowns = false;
            this.gridPat.HasMultilineHeaders = false;
            this.gridPat.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridPat.HeaderHeight = 15;
            this.gridPat.HScrollVisible = false;
            this.gridPat.Location = new System.Drawing.Point(3, 142);
            this.gridPat.Name = "gridPat";
            this.gridPat.ScrollValue = 0;
            this.gridPat.SelectionMode = OpenDental.UI.GridSelectionMode.None;
            this.gridPat.Size = new System.Drawing.Size(250, 513);
            this.gridPat.TabIndex = 30;
            this.gridPat.Title = "Patient Information";
            this.gridPat.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridPat.TitleHeight = 18;
            this.gridPat.TranslationName = "TablePatient";
            this.gridPat.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridPat_CellDoubleClick);
            this.gridPat.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridPat_CellClick);
            // 
            // gridIns
            // 
            this.gridIns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIns.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridIns.EditableEnterMovesDown = false;
            this.gridIns.HasAddButton = false;
            this.gridIns.HasDropDowns = false;
            this.gridIns.HasMultilineHeaders = false;
            this.gridIns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridIns.HeaderHeight = 15;
            this.gridIns.HScrollVisible = true;
            this.gridIns.Location = new System.Drawing.Point(259, 142);
            this.gridIns.Name = "gridIns";
            this.gridIns.ScrollValue = 0;
            this.gridIns.SelectionMode = OpenDental.UI.GridSelectionMode.None;
            this.gridIns.Size = new System.Drawing.Size(898, 510);
            this.gridIns.TabIndex = 29;
            this.gridIns.Title = "Insurance Plans";
            this.gridIns.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridIns.TitleHeight = 18;
            this.gridIns.TranslationName = "TableCoverage";
            this.gridIns.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridIns_CellDoubleClick);
            // 
            // superClonesSplitContainer
            // 
            this.superClonesSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.superClonesSplitContainer.Location = new System.Drawing.Point(259, 142);
            this.superClonesSplitContainer.Name = "superClonesSplitContainer";
            this.superClonesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // superClonesSplitContainer.Panel1
            // 
            this.superClonesSplitContainer.Panel1.Controls.Add(this.gridSuperFam);
            // 
            // superClonesSplitContainer.Panel2
            // 
            this.superClonesSplitContainer.Panel2.Controls.Add(this.gridPatientClones);
            this.superClonesSplitContainer.Size = new System.Drawing.Size(320, 510);
            this.superClonesSplitContainer.SplitterDistance = 250;
            this.superClonesSplitContainer.TabIndex = 34;
            // 
            // gridPatientClones
            // 
            this.gridPatientClones.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridPatientClones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPatientClones.EditableEnterMovesDown = false;
            this.gridPatientClones.HasAddButton = false;
            this.gridPatientClones.HasDropDowns = false;
            this.gridPatientClones.HasMultilineHeaders = false;
            this.gridPatientClones.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridPatientClones.HeaderHeight = 15;
            this.gridPatientClones.HScrollVisible = false;
            this.gridPatientClones.Location = new System.Drawing.Point(0, 0);
            this.gridPatientClones.Name = "gridPatientClones";
            this.gridPatientClones.ScrollValue = 0;
            this.gridPatientClones.Size = new System.Drawing.Size(320, 256);
            this.gridPatientClones.TabIndex = 34;
            this.gridPatientClones.Title = "Patient Clones";
            this.gridPatientClones.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridPatientClones.TitleHeight = 18;
            this.gridPatientClones.TranslationName = "TablePatientClones";
            this.gridPatientClones.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridPatientClone_CellClick);
            // 
            // patientPictureBox
            // 
            this.patientPictureBox.Location = new System.Drawing.Point(3, 36);
            this.patientPictureBox.Name = "patientPictureBox";
            this.patientPictureBox.Size = new System.Drawing.Size(100, 100);
            this.patientPictureBox.TabIndex = 28;
            this.patientPictureBox.Text = "picturePat";
            this.patientPictureBox.TextNullImage = "Patient Picture Unavailable";
            // 
            // ToolBarMain
            // 
            this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
            this.ToolBarMain.Name = "ToolBarMain";
            this.ToolBarMain.Size = new System.Drawing.Size(1160, 30);
            this.ToolBarMain.TabIndex = 19;
            this.ToolBarMain.ButtonClick += new System.EventHandler<OpenDental.UI.ODToolBarButtonClickEventArgs>(this.ToolBarMain_ButtonClick);
            // 
            // ContrFamily
            // 
            this.Controls.Add(this.superClonesSplitContainer);
            this.Controls.Add(this.gridRecall);
            this.Controls.Add(this.gridFamily);
            this.Controls.Add(this.gridPat);
            this.Controls.Add(this.gridIns);
            this.Controls.Add(this.patientPictureBox);
            this.Controls.Add(this.ToolBarMain);
            this.Name = "ContrFamily";
            this.Size = new System.Drawing.Size(1160, 655);
            this.superClonesSplitContainer.Panel1.ResumeLayout(false);
            this.superClonesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superClonesSplitContainer)).EndInit();
            this.superClonesSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private OpenDental.UI.ODToolBar ToolBarMain;
        private OpenDental.UI.ODPictureBox patientPictureBox;
        private OpenDental.UI.ODGrid gridIns;

        private OpenDental.UI.ODGrid gridPat;
        private OpenDental.UI.ODGrid gridFamily;
        private OpenDental.UI.ODGrid gridRecall;
        private OpenDental.UI.ODGrid gridSuperFam;

        private System.Windows.Forms.ContextMenu menuDiscount;
        private System.Windows.Forms.MenuItem menuItemRemoveDiscount;
        private System.Windows.Forms.SplitContainer superClonesSplitContainer;
        private OpenDental.UI.ODGrid gridPatientClones;

        private System.Windows.Forms.ContextMenu menuInsurance;
        private System.Windows.Forms.MenuItem menuPlansForFam;
    }
}
