namespace OpenDental
{
    partial class FormApptViews
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptViews));
            this.cancelButton = new System.Windows.Forms.Button();
            this.viewsLabel = new System.Windows.Forms.Label();
            this.viewsListBox = new System.Windows.Forms.ListBox();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.timeIncrementGroupBox = new System.Windows.Forms.GroupBox();
            this.increment5RadioButton = new System.Windows.Forms.RadioButton();
            this.increment15RadioButton = new System.Windows.Forms.RadioButton();
            this.increment10RadioButton = new System.Windows.Forms.RadioButton();
            this.checkTwoRows = new System.Windows.Forms.CheckBox();
            this.procedureColorsButton = new System.Windows.Forms.Button();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.timeIncrementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(411, 498);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Close";
            // 
            // viewsLabel
            // 
            this.viewsLabel.AutoSize = true;
            this.viewsLabel.Location = new System.Drawing.Point(107, 60);
            this.viewsLabel.Name = "viewsLabel";
            this.viewsLabel.Size = new System.Drawing.Size(37, 15);
            this.viewsLabel.TabIndex = 2;
            this.viewsLabel.Text = "Views";
            this.viewsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // viewsListBox
            // 
            this.viewsListBox.IntegralHeight = false;
            this.viewsListBox.ItemHeight = 15;
            this.viewsListBox.Location = new System.Drawing.Point(109, 78);
            this.viewsListBox.Name = "viewsListBox";
            this.viewsListBox.Size = new System.Drawing.Size(220, 349);
            this.viewsListBox.TabIndex = 3;
            this.viewsListBox.DoubleClick += new System.EventHandler(this.viewsListBox_DoubleClick);
            // 
            // downButton
            // 
            this.downButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowDown;
            this.downButton.Location = new System.Drawing.Point(289, 433);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(40, 30);
            this.downButton.TabIndex = 6;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowUp;
            this.upButton.Location = new System.Drawing.Point(243, 433);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(40, 30);
            this.upButton.TabIndex = 5;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(110, 433);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "&Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // timeIncrementGroupBox
            // 
            this.timeIncrementGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeIncrementGroupBox.Controls.Add(this.increment5RadioButton);
            this.timeIncrementGroupBox.Controls.Add(this.increment15RadioButton);
            this.timeIncrementGroupBox.Controls.Add(this.increment10RadioButton);
            this.timeIncrementGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeIncrementGroupBox.Location = new System.Drawing.Point(335, 78);
            this.timeIncrementGroupBox.Name = "timeIncrementGroupBox";
            this.timeIncrementGroupBox.Size = new System.Drawing.Size(186, 101);
            this.timeIncrementGroupBox.TabIndex = 7;
            this.timeIncrementGroupBox.TabStop = false;
            this.timeIncrementGroupBox.Text = "Time Increments";
            // 
            // increment5RadioButton
            // 
            this.increment5RadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.increment5RadioButton.Location = new System.Drawing.Point(28, 23);
            this.increment5RadioButton.Name = "increment5RadioButton";
            this.increment5RadioButton.Size = new System.Drawing.Size(120, 23);
            this.increment5RadioButton.TabIndex = 0;
            this.increment5RadioButton.Text = "5 Min";
            // 
            // increment15RadioButton
            // 
            this.increment15RadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.increment15RadioButton.Location = new System.Drawing.Point(28, 70);
            this.increment15RadioButton.Name = "increment15RadioButton";
            this.increment15RadioButton.Size = new System.Drawing.Size(120, 22);
            this.increment15RadioButton.TabIndex = 2;
            this.increment15RadioButton.Text = "15 Min";
            // 
            // increment10RadioButton
            // 
            this.increment10RadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.increment10RadioButton.Location = new System.Drawing.Point(28, 47);
            this.increment10RadioButton.Name = "increment10RadioButton";
            this.increment10RadioButton.Size = new System.Drawing.Size(120, 22);
            this.increment10RadioButton.TabIndex = 1;
            this.increment10RadioButton.Text = "10 Min";
            // 
            // checkTwoRows
            // 
            this.checkTwoRows.Location = new System.Drawing.Point(0, 0);
            this.checkTwoRows.Name = "checkTwoRows";
            this.checkTwoRows.Size = new System.Drawing.Size(104, 24);
            this.checkTwoRows.TabIndex = 0;
            // 
            // procedureColorsButton
            // 
            this.procedureColorsButton.Location = new System.Drawing.Point(335, 185);
            this.procedureColorsButton.Name = "procedureColorsButton";
            this.procedureColorsButton.Size = new System.Drawing.Size(150, 30);
            this.procedureColorsButton.TabIndex = 8;
            this.procedureColorsButton.Text = "Proc Colors";
            this.procedureColorsButton.Click += new System.EventHandler(this.procedureColorsButton_Click);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(110, 19);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(219, 23);
            this.clinicComboBox.TabIndex = 1;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.clinicComboBox_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(67, 22);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 0;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // FormApptViews
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(534, 541);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.procedureColorsButton);
            this.Controls.Add(this.timeIncrementGroupBox);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.viewsListBox);
            this.Controls.Add(this.viewsLabel);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptViews";
            this.ShowInTaskbar = false;
            this.Text = "Appointment Views";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptViews_FormClosing);
            this.Load += new System.EventHandler(this.FormApptViews_Load);
            this.timeIncrementGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label viewsLabel;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ListBox viewsListBox;
        private System.Windows.Forms.GroupBox timeIncrementGroupBox;
        private System.Windows.Forms.RadioButton increment10RadioButton;
        private System.Windows.Forms.RadioButton increment15RadioButton;
        private System.Windows.Forms.CheckBox checkTwoRows;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton increment5RadioButton;
        private System.Windows.Forms.Button procedureColorsButton;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
    }
}