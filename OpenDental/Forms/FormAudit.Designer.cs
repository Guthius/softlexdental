namespace OpenDental
{
    partial class FormAudit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudit));
            this.textDateEditedFrom = new OpenDental.ValidDate();
            this.textDateEditedTo = new OpenDental.ValidDate();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rowsTextBox = new OpenDental.ValidNum();
            this.rowsLabel = new System.Windows.Forms.Label();
            this.printButton = new System.Windows.Forms.Button();
            this.currentButton = new System.Windows.Forms.Button();
            this.allButton = new System.Windows.Forms.Button();
            this.findButton = new System.Windows.Forms.Button();
            this.patientTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.patientLabel = new System.Windows.Forms.Label();
            this.permissionLabel = new System.Windows.Forms.Label();
            this.permissionComboBox = new System.Windows.Forms.ComboBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.textDateFrom = new OpenDental.ValidDate();
            this.textDateTo = new OpenDental.ValidDate();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // textDateEditedFrom
            // 
            this.textDateEditedFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textDateEditedFrom.Location = new System.Drawing.Point(900, 19);
            this.textDateEditedFrom.Name = "textDateEditedFrom";
            this.textDateEditedFrom.Size = new System.Drawing.Size(80, 23);
            this.textDateEditedFrom.TabIndex = 14;
            // 
            // textDateEditedTo
            // 
            this.textDateEditedTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textDateEditedTo.Location = new System.Drawing.Point(900, 48);
            this.textDateEditedTo.Name = "textDateEditedTo";
            this.textDateEditedTo.Size = new System.Drawing.Size(80, 23);
            this.textDateEditedTo.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(784, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "Previous From Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(848, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "To Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rowsTextBox
            // 
            this.rowsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rowsTextBox.DoAutoSave = false;
            this.rowsTextBox.Location = new System.Drawing.Point(1115, 605);
            this.rowsTextBox.MaxLength = 5;
            this.rowsTextBox.MaxVal = 99999;
            this.rowsTextBox.MinVal = 1;
            this.rowsTextBox.Name = "rowsTextBox";
            this.rowsTextBox.Preference = OpenDentBusiness.PreferenceName.NotApplicable;
            this.rowsTextBox.Size = new System.Drawing.Size(56, 23);
            this.rowsTextBox.TabIndex = 21;
            // 
            // rowsLabel
            // 
            this.rowsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rowsLabel.AutoSize = true;
            this.rowsLabel.Location = new System.Drawing.Point(1044, 608);
            this.rowsLabel.Name = "rowsLabel";
            this.rowsLabel.Size = new System.Drawing.Size(65, 15);
            this.rowsLabel.TabIndex = 20;
            this.rowsLabel.Text = "Limit Rows";
            this.rowsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Image = global::OpenDental.Properties.Resources.Icon32Printer;
            this.printButton.Location = new System.Drawing.Point(1111, 19);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(60, 50);
            this.printButton.TabIndex = 18;
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // currentButton
            // 
            this.currentButton.Location = new System.Drawing.Point(500, 46);
            this.currentButton.Name = "currentButton";
            this.currentButton.Size = new System.Drawing.Size(70, 25);
            this.currentButton.TabIndex = 10;
            this.currentButton.Text = "Current";
            this.currentButton.Click += new System.EventHandler(this.CurrentButton_Click);
            // 
            // allButton
            // 
            this.allButton.Location = new System.Drawing.Point(652, 46);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(70, 25);
            this.allButton.TabIndex = 12;
            this.allButton.Text = "All";
            this.allButton.Click += new System.EventHandler(this.AllButton_Click);
            // 
            // findButton
            // 
            this.findButton.Location = new System.Drawing.Point(576, 46);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(70, 25);
            this.findButton.TabIndex = 11;
            this.findButton.Text = "Find";
            this.findButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // patientTextBox
            // 
            this.patientTextBox.Location = new System.Drawing.Point(500, 19);
            this.patientTextBox.Name = "patientTextBox";
            this.patientTextBox.Size = new System.Drawing.Size(221, 23);
            this.patientTextBox.TabIndex = 9;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(214, 51);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(30, 15);
            this.userLabel.TabIndex = 6;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userComboBox
            // 
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(250, 48);
            this.userComboBox.MaxDropDownItems = 40;
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(170, 23);
            this.userComboBox.TabIndex = 7;
            this.userComboBox.SelectionChangeCommitted += new System.EventHandler(this.UserComboBox_SelectionChangeCommitted);
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Location = new System.Drawing.Point(450, 22);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(44, 15);
            this.patientLabel.TabIndex = 8;
            this.patientLabel.Text = "Patient";
            this.patientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // permissionLabel
            // 
            this.permissionLabel.AutoSize = true;
            this.permissionLabel.Location = new System.Drawing.Point(179, 22);
            this.permissionLabel.Name = "permissionLabel";
            this.permissionLabel.Size = new System.Drawing.Size(65, 15);
            this.permissionLabel.TabIndex = 4;
            this.permissionLabel.Text = "Permission";
            this.permissionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // permissionComboBox
            // 
            this.permissionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.permissionComboBox.FormattingEnabled = true;
            this.permissionComboBox.Location = new System.Drawing.Point(250, 19);
            this.permissionComboBox.MaxDropDownItems = 40;
            this.permissionComboBox.Name = "permissionComboBox";
            this.permissionComboBox.Size = new System.Drawing.Size(170, 23);
            this.permissionComboBox.TabIndex = 5;
            this.permissionComboBox.SelectionChangeCommitted += new System.EventHandler(this.PermissionComboBox_SelectionChangeCommitted);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::OpenDental.Properties.Resources.Icon32Refresh;
            this.refreshButton.Location = new System.Drawing.Point(1045, 19);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(60, 50);
            this.refreshButton.TabIndex = 17;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // textDateFrom
            // 
            this.textDateFrom.Location = new System.Drawing.Point(80, 19);
            this.textDateFrom.Name = "textDateFrom";
            this.textDateFrom.Size = new System.Drawing.Size(80, 23);
            this.textDateFrom.TabIndex = 1;
            // 
            // textDateTo
            // 
            this.textDateTo.Location = new System.Drawing.Point(80, 48);
            this.textDateTo.Name = "textDateTo";
            this.textDateTo.Size = new System.Drawing.Size(80, 23);
            this.textDateTo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "From Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "To Date";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grid
            // 
            this.grid.AllowSortingByColumn = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.grid.EditableEnterMovesDown = false;
            this.grid.HasAddButton = false;
            this.grid.HasDropDowns = false;
            this.grid.HasMultilineHeaders = false;
            this.grid.HScrollVisible = false;
            this.grid.Location = new System.Drawing.Point(13, 77);
            this.grid.Name = "grid";
            this.grid.ScrollValue = 0;
            this.grid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.grid.Size = new System.Drawing.Size(1158, 522);
            this.grid.TabIndex = 19;
            this.grid.Title = "Audit Trail";
            this.grid.TitleVisible = true;
            this.grid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.Grid_CellDoubleClick);
            // 
            // FormAudit
            // 
            this.ClientSize = new System.Drawing.Size(1184, 641);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.textDateEditedFrom);
            this.Controls.Add(this.textDateEditedTo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.rowsTextBox);
            this.Controls.Add(this.rowsLabel);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.currentButton);
            this.Controls.Add(this.allButton);
            this.Controls.Add(this.findButton);
            this.Controls.Add(this.patientTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.userComboBox);
            this.Controls.Add(this.patientLabel);
            this.Controls.Add(this.permissionLabel);
            this.Controls.Add(this.permissionComboBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.textDateFrom);
            this.Controls.Add(this.textDateTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1124, 218);
            this.Name = "FormAudit";
            this.ShowInTaskbar = false;
            this.Text = "Audit Trail";
            this.Load += new System.EventHandler(this.FormAudit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.ValidDate textDateEditedFrom;
        private OpenDental.ValidDate textDateEditedTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private OpenDental.ValidNum rowsTextBox;
        private System.Windows.Forms.Label rowsLabel;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button currentButton;
        private System.Windows.Forms.Button allButton;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.TextBox patientTextBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Label patientLabel;
        private System.Windows.Forms.Label permissionLabel;
        private System.Windows.Forms.ComboBox permissionComboBox;
        private System.Windows.Forms.Button refreshButton;
        private OpenDental.ValidDate textDateFrom;
        private OpenDental.ValidDate textDateTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private OpenDental.UI.ODGrid grid;
    }
}
