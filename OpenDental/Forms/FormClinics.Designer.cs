namespace OpenDental
{
    partial class FormClinics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinics));
            this.clinicOrderGroupBox = new System.Windows.Forms.GroupBox();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.orderAlphabeticalCheckBox = new System.Windows.Forms.CheckBox();
            this.movePatientsGroupBox = new System.Windows.Forms.GroupBox();
            this.movePatientsButton = new System.Windows.Forms.Button();
            this.pickClinicButton = new System.Windows.Forms.Button();
            this.moveToTextBox = new System.Windows.Forms.TextBox();
            this.moveToLabel = new System.Windows.Forms.Label();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.clinicsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.selectNoneButton = new System.Windows.Forms.Button();
            this.clinicOrderGroupBox.SuspendLayout();
            this.movePatientsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // clinicOrderGroupBox
            // 
            this.clinicOrderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicOrderGroupBox.Controls.Add(this.upButton);
            this.clinicOrderGroupBox.Controls.Add(this.downButton);
            this.clinicOrderGroupBox.Controls.Add(this.orderAlphabeticalCheckBox);
            this.clinicOrderGroupBox.Location = new System.Drawing.Point(661, 150);
            this.clinicOrderGroupBox.Name = "clinicOrderGroupBox";
            this.clinicOrderGroupBox.Size = new System.Drawing.Size(270, 100);
            this.clinicOrderGroupBox.TabIndex = 2;
            this.clinicOrderGroupBox.TabStop = false;
            this.clinicOrderGroupBox.Text = "Clinic Order";
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::OpenDental.Properties.Resources.IconArrowUp;
            this.upButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upButton.Location = new System.Drawing.Point(10, 22);
            this.upButton.Name = "upButton";
            this.upButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.upButton.Size = new System.Drawing.Size(110, 30);
            this.upButton.TabIndex = 0;
            this.upButton.Text = "&Up";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::OpenDental.Properties.Resources.IconArrowDown;
            this.downButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downButton.Location = new System.Drawing.Point(126, 22);
            this.downButton.Name = "downButton";
            this.downButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.downButton.Size = new System.Drawing.Size(110, 30);
            this.downButton.TabIndex = 1;
            this.downButton.Text = "&Down";
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // orderAlphabeticalCheckBox
            // 
            this.orderAlphabeticalCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.orderAlphabeticalCheckBox.AutoSize = true;
            this.orderAlphabeticalCheckBox.Location = new System.Drawing.Point(10, 58);
            this.orderAlphabeticalCheckBox.Name = "orderAlphabeticalCheckBox";
            this.orderAlphabeticalCheckBox.Size = new System.Drawing.Size(125, 19);
            this.orderAlphabeticalCheckBox.TabIndex = 2;
            this.orderAlphabeticalCheckBox.Text = "Order Alphabetical";
            this.orderAlphabeticalCheckBox.UseVisualStyleBackColor = true;
            this.orderAlphabeticalCheckBox.Click += new System.EventHandler(this.OrderAlphabeticalCheckBox_Click);
            // 
            // movePatientsGroupBox
            // 
            this.movePatientsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.movePatientsGroupBox.Controls.Add(this.movePatientsButton);
            this.movePatientsGroupBox.Controls.Add(this.pickClinicButton);
            this.movePatientsGroupBox.Controls.Add(this.moveToTextBox);
            this.movePatientsGroupBox.Controls.Add(this.moveToLabel);
            this.movePatientsGroupBox.Location = new System.Drawing.Point(661, 44);
            this.movePatientsGroupBox.Name = "movePatientsGroupBox";
            this.movePatientsGroupBox.Size = new System.Drawing.Size(270, 100);
            this.movePatientsGroupBox.TabIndex = 1;
            this.movePatientsGroupBox.TabStop = false;
            this.movePatientsGroupBox.Text = "Move Patients";
            // 
            // movePatientsButton
            // 
            this.movePatientsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.movePatientsButton.Location = new System.Drawing.Point(154, 57);
            this.movePatientsButton.Name = "movePatientsButton";
            this.movePatientsButton.Size = new System.Drawing.Size(110, 30);
            this.movePatientsButton.TabIndex = 3;
            this.movePatientsButton.Text = "&Move";
            this.movePatientsButton.UseVisualStyleBackColor = true;
            this.movePatientsButton.Click += new System.EventHandler(this.MovePatientsButton_Click);
            // 
            // pickClinicButton
            // 
            this.pickClinicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pickClinicButton.Location = new System.Drawing.Point(234, 27);
            this.pickClinicButton.Name = "pickClinicButton";
            this.pickClinicButton.Size = new System.Drawing.Size(30, 25);
            this.pickClinicButton.TabIndex = 2;
            this.pickClinicButton.Text = "...";
            this.pickClinicButton.Click += new System.EventHandler(this.PickClinicButton_Click);
            // 
            // moveToTextBox
            // 
            this.moveToTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTextBox.Location = new System.Drawing.Point(90, 28);
            this.moveToTextBox.MaxLength = 15;
            this.moveToTextBox.Name = "moveToTextBox";
            this.moveToTextBox.ReadOnly = true;
            this.moveToTextBox.Size = new System.Drawing.Size(138, 23);
            this.moveToTextBox.TabIndex = 1;
            // 
            // moveToLabel
            // 
            this.moveToLabel.AutoSize = true;
            this.moveToLabel.Location = new System.Drawing.Point(32, 32);
            this.moveToLabel.Name = "moveToLabel";
            this.moveToLabel.Size = new System.Drawing.Size(52, 15);
            this.moveToLabel.TabIndex = 0;
            this.moveToLabel.Text = "To Clinic";
            this.moveToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showHiddenCheckBox.Checked = true;
            this.showHiddenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(558, 19);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(97, 19);
            this.showHiddenCheckBox.TabIndex = 8;
            this.showHiddenCheckBox.Text = "Show Hidden";
            this.showHiddenCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showHiddenCheckBox.UseVisualStyleBackColor = true;
            this.showHiddenCheckBox.CheckedChanged += new System.EventHandler(this.ShowHiddenCheckBox_CheckedChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(821, 532);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // clinicsGrid
            // 
            this.clinicsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.clinicsGrid.EditableEnterMovesDown = false;
            this.clinicsGrid.HasAddButton = false;
            this.clinicsGrid.HasDropDowns = false;
            this.clinicsGrid.HasMultilineHeaders = false;
            this.clinicsGrid.HScrollVisible = false;
            this.clinicsGrid.Location = new System.Drawing.Point(13, 44);
            this.clinicsGrid.Name = "clinicsGrid";
            this.clinicsGrid.ScrollValue = 0;
            this.clinicsGrid.Size = new System.Drawing.Size(642, 554);
            this.clinicsGrid.TabIndex = 0;
            this.clinicsGrid.Title = "Clinics";
            this.clinicsGrid.TitleVisible = true;
            this.clinicsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.ClinicsGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(821, 256);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 40);
            this.addButton.Name = "addButton";
            this.addButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(821, 568);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAllButton.Location = new System.Drawing.Point(821, 423);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(110, 30);
            this.selectAllButton.TabIndex = 4;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Visible = false;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // selectNoneButton
            // 
            this.selectNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectNoneButton.Location = new System.Drawing.Point(821, 459);
            this.selectNoneButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 40);
            this.selectNoneButton.Name = "selectNoneButton";
            this.selectNoneButton.Size = new System.Drawing.Size(110, 30);
            this.selectNoneButton.TabIndex = 5;
            this.selectNoneButton.Text = "Select None";
            this.selectNoneButton.UseVisualStyleBackColor = true;
            this.selectNoneButton.Visible = false;
            this.selectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // FormClinics
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(944, 611);
            this.Controls.Add(this.selectNoneButton);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.clinicOrderGroupBox);
            this.Controls.Add(this.movePatientsGroupBox);
            this.Controls.Add(this.showHiddenCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.clinicsGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(960, 650);
            this.Name = "FormClinics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinics";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClinics_Closing);
            this.Load += new System.EventHandler(this.FormClinics_Load);
            this.clinicOrderGroupBox.ResumeLayout(false);
            this.clinicOrderGroupBox.PerformLayout();
            this.movePatientsGroupBox.ResumeLayout(false);
            this.movePatientsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button closeButton;
        private UI.ODGrid clinicsGrid;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.GroupBox movePatientsGroupBox;
        private System.Windows.Forms.Button movePatientsButton;
        private System.Windows.Forms.Button pickClinicButton;
        private System.Windows.Forms.TextBox moveToTextBox;
        private System.Windows.Forms.Label moveToLabel;
        private System.Windows.Forms.GroupBox clinicOrderGroupBox;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button selectNoneButton;
        private System.Windows.Forms.CheckBox showHiddenCheckBox;
        private System.Windows.Forms.CheckBox orderAlphabeticalCheckBox;
    }
}
