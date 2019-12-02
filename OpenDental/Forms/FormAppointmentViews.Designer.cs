namespace OpenDental
{
    partial class FormAppointmentViews
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppointmentViews));
            this.cancelButton = new System.Windows.Forms.Button();
            this.viewsLabel = new System.Windows.Forms.Label();
            this.appointmentViewsListBox = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.timeIncrementGroupBox = new System.Windows.Forms.GroupBox();
            this.increment5RadioButton = new System.Windows.Forms.RadioButton();
            this.increment15RadioButton = new System.Windows.Forms.RadioButton();
            this.increment10RadioButton = new System.Windows.Forms.RadioButton();
            this.checkTwoRows = new System.Windows.Forms.CheckBox();
            this.procedureColorsButton = new System.Windows.Forms.Button();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.timeIncrementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(361, 378);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Close";
            // 
            // viewsLabel
            // 
            this.viewsLabel.AutoSize = true;
            this.viewsLabel.Location = new System.Drawing.Point(10, 62);
            this.viewsLabel.Name = "viewsLabel";
            this.viewsLabel.Size = new System.Drawing.Size(37, 15);
            this.viewsLabel.TabIndex = 2;
            this.viewsLabel.Text = "Views";
            this.viewsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // appointmentViewsListBox
            // 
            this.appointmentViewsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentViewsListBox.IntegralHeight = false;
            this.appointmentViewsListBox.ItemHeight = 15;
            this.appointmentViewsListBox.Location = new System.Drawing.Point(13, 80);
            this.appointmentViewsListBox.Name = "appointmentViewsListBox";
            this.appointmentViewsListBox.Size = new System.Drawing.Size(266, 292);
            this.appointmentViewsListBox.TabIndex = 3;
            this.appointmentViewsListBox.SelectedIndexChanged += new System.EventHandler(this.AppointmentViewsListBox_SelectedIndexChanged);
            this.appointmentViewsListBox.DoubleClick += new System.EventHandler(this.AppointmentViewsListBox_DoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.Location = new System.Drawing.Point(13, 378);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(40, 30);
            this.addButton.TabIndex = 4;
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // timeIncrementGroupBox
            // 
            this.timeIncrementGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeIncrementGroupBox.Controls.Add(this.increment5RadioButton);
            this.timeIncrementGroupBox.Controls.Add(this.increment15RadioButton);
            this.timeIncrementGroupBox.Controls.Add(this.increment10RadioButton);
            this.timeIncrementGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeIncrementGroupBox.Location = new System.Drawing.Point(285, 78);
            this.timeIncrementGroupBox.Name = "timeIncrementGroupBox";
            this.timeIncrementGroupBox.Size = new System.Drawing.Size(186, 101);
            this.timeIncrementGroupBox.TabIndex = 6;
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
            this.procedureColorsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.procedureColorsButton.Location = new System.Drawing.Point(285, 185);
            this.procedureColorsButton.Name = "procedureColorsButton";
            this.procedureColorsButton.Size = new System.Drawing.Size(150, 30);
            this.procedureColorsButton.TabIndex = 7;
            this.procedureColorsButton.Text = "Procedure Colors";
            this.procedureColorsButton.Click += new System.EventHandler(this.ProcedureColorsButton_Click);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(100, 19);
            this.clinicComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(179, 23);
            this.clinicComboBox.TabIndex = 1;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(57, 22);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 0;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.Location = new System.Drawing.Point(59, 378);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.deleteButton.Size = new System.Drawing.Size(40, 30);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAppointmentViews
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 421);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.procedureColorsButton);
            this.Controls.Add(this.timeIncrementGroupBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.appointmentViewsListBox);
            this.Controls.Add(this.viewsLabel);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAppointmentViews";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Views";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAppointmentViews_FormClosing);
            this.Load += new System.EventHandler(this.FormAppointmentViews_Load);
            this.timeIncrementGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label viewsLabel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ListBox appointmentViewsListBox;
        private System.Windows.Forms.GroupBox timeIncrementGroupBox;
        private System.Windows.Forms.RadioButton increment10RadioButton;
        private System.Windows.Forms.RadioButton increment15RadioButton;
        private System.Windows.Forms.CheckBox checkTwoRows;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton increment5RadioButton;
        private System.Windows.Forms.Button procedureColorsButton;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.Button deleteButton;
    }
}