namespace OpenDental
{
    partial class FormTimeCardRuleEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeCardRuleEdit));
            this.minClockinLabel = new System.Windows.Forms.Label();
            this.overtimeExemptCheckBox = new System.Windows.Forms.CheckBox();
            this.minClockinTextBox = new System.Windows.Forms.TextBox();
            this.overHoursTextBox = new System.Windows.Forms.TextBox();
            this.overHoursLabel = new System.Windows.Forms.Label();
            this.differentialHoursGroupBox = new System.Windows.Forms.GroupBox();
            this.timeBeforeLabel = new System.Windows.Forms.Label();
            this.timeBeforeButton = new System.Windows.Forms.Button();
            this.timeAfterTextBox = new System.Windows.Forms.TextBox();
            this.timeAfterLabel = new System.Windows.Forms.Label();
            this.timeAfterButton = new System.Windows.Forms.Button();
            this.timeBeforeTextBox = new System.Windows.Forms.TextBox();
            this.employeesListBox = new System.Windows.Forms.ListBox();
            this.employeeLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.overHoursSuffixLabel = new System.Windows.Forms.Label();
            this.differentialHoursGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // minClockinLabel
            // 
            this.minClockinLabel.AutoSize = true;
            this.minClockinLabel.Location = new System.Drawing.Point(90, 172);
            this.minClockinLabel.Name = "minClockinLabel";
            this.minClockinLabel.Size = new System.Drawing.Size(119, 15);
            this.minClockinLabel.TabIndex = 5;
            this.minClockinLabel.Text = "Earliest Clock in Time";
            this.minClockinLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // overtimeExemptCheckBox
            // 
            this.overtimeExemptCheckBox.AutoSize = true;
            this.overtimeExemptCheckBox.Location = new System.Drawing.Point(215, 144);
            this.overtimeExemptCheckBox.Name = "overtimeExemptCheckBox";
            this.overtimeExemptCheckBox.Size = new System.Drawing.Size(127, 19);
            this.overtimeExemptCheckBox.TabIndex = 4;
            this.overtimeExemptCheckBox.Text = "Is overtime exempt";
            this.overtimeExemptCheckBox.UseVisualStyleBackColor = true;
            // 
            // minClockinTextBox
            // 
            this.minClockinTextBox.Location = new System.Drawing.Point(215, 169);
            this.minClockinTextBox.Name = "minClockinTextBox";
            this.minClockinTextBox.Size = new System.Drawing.Size(62, 23);
            this.minClockinTextBox.TabIndex = 6;
            this.minClockinTextBox.Text = "6:00";
            // 
            // overHoursTextBox
            // 
            this.overHoursTextBox.Location = new System.Drawing.Point(215, 19);
            this.overHoursTextBox.Name = "overHoursTextBox";
            this.overHoursTextBox.Size = new System.Drawing.Size(62, 23);
            this.overHoursTextBox.TabIndex = 1;
            this.overHoursTextBox.Text = "8:00";
            this.overHoursTextBox.TextChanged += new System.EventHandler(this.OverHoursTextBox_TextChanged);
            // 
            // overHoursLabel
            // 
            this.overHoursLabel.AutoSize = true;
            this.overHoursLabel.Location = new System.Drawing.Point(117, 22);
            this.overHoursLabel.Name = "overHoursLabel";
            this.overHoursLabel.Size = new System.Drawing.Size(92, 15);
            this.overHoursLabel.TabIndex = 0;
            this.overHoursLabel.Text = "Overtime if over";
            this.overHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // differentialHoursGroupBox
            // 
            this.differentialHoursGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.differentialHoursGroupBox.Controls.Add(this.timeBeforeLabel);
            this.differentialHoursGroupBox.Controls.Add(this.timeBeforeButton);
            this.differentialHoursGroupBox.Controls.Add(this.timeAfterTextBox);
            this.differentialHoursGroupBox.Controls.Add(this.timeAfterLabel);
            this.differentialHoursGroupBox.Controls.Add(this.timeAfterButton);
            this.differentialHoursGroupBox.Controls.Add(this.timeBeforeTextBox);
            this.differentialHoursGroupBox.Location = new System.Drawing.Point(13, 48);
            this.differentialHoursGroupBox.Name = "differentialHoursGroupBox";
            this.differentialHoursGroupBox.Size = new System.Drawing.Size(378, 90);
            this.differentialHoursGroupBox.TabIndex = 3;
            this.differentialHoursGroupBox.TabStop = false;
            this.differentialHoursGroupBox.Text = "Or differential hours";
            // 
            // timeBeforeLabel
            // 
            this.timeBeforeLabel.AutoSize = true;
            this.timeBeforeLabel.Location = new System.Drawing.Point(92, 25);
            this.timeBeforeLabel.Name = "timeBeforeLabel";
            this.timeBeforeLabel.Size = new System.Drawing.Size(104, 15);
            this.timeBeforeLabel.TabIndex = 0;
            this.timeBeforeLabel.Text = "Before time of day";
            this.timeBeforeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeBeforeButton
            // 
            this.timeBeforeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.timeBeforeButton.Location = new System.Drawing.Point(270, 20);
            this.timeBeforeButton.Name = "timeBeforeButton";
            this.timeBeforeButton.Size = new System.Drawing.Size(60, 25);
            this.timeBeforeButton.TabIndex = 2;
            this.timeBeforeButton.Text = "6 AM";
            this.timeBeforeButton.Click += new System.EventHandler(this.TimeBeforeButton_Click);
            // 
            // timeAfterTextBox
            // 
            this.timeAfterTextBox.Location = new System.Drawing.Point(202, 49);
            this.timeAfterTextBox.Name = "timeAfterTextBox";
            this.timeAfterTextBox.Size = new System.Drawing.Size(62, 23);
            this.timeAfterTextBox.TabIndex = 4;
            this.timeAfterTextBox.Text = "17:00";
            this.timeAfterTextBox.TextChanged += new System.EventHandler(this.TimeAfterTextBox_TextChanged);
            // 
            // timeAfterLabel
            // 
            this.timeAfterLabel.AutoSize = true;
            this.timeAfterLabel.Location = new System.Drawing.Point(100, 52);
            this.timeAfterLabel.Name = "timeAfterLabel";
            this.timeAfterLabel.Size = new System.Drawing.Size(96, 15);
            this.timeAfterLabel.TabIndex = 3;
            this.timeAfterLabel.Text = "After time of day";
            this.timeAfterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeAfterButton
            // 
            this.timeAfterButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.timeAfterButton.Location = new System.Drawing.Point(270, 48);
            this.timeAfterButton.Name = "timeAfterButton";
            this.timeAfterButton.Size = new System.Drawing.Size(60, 25);
            this.timeAfterButton.TabIndex = 5;
            this.timeAfterButton.Text = "5 PM";
            this.timeAfterButton.Click += new System.EventHandler(this.TimeAfterButton_Click);
            // 
            // timeBeforeTextBox
            // 
            this.timeBeforeTextBox.Location = new System.Drawing.Point(202, 22);
            this.timeBeforeTextBox.Name = "timeBeforeTextBox";
            this.timeBeforeTextBox.Size = new System.Drawing.Size(62, 23);
            this.timeBeforeTextBox.TabIndex = 1;
            this.timeBeforeTextBox.Text = "6:00";
            this.timeBeforeTextBox.TextChanged += new System.EventHandler(this.TimeBeforeTextBox_TextChanged);
            // 
            // employeesListBox
            // 
            this.employeesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.employeesListBox.FormattingEnabled = true;
            this.employeesListBox.IntegralHeight = false;
            this.employeesListBox.ItemHeight = 15;
            this.employeesListBox.Location = new System.Drawing.Point(13, 252);
            this.employeesListBox.Name = "employeesListBox";
            this.employeesListBox.Size = new System.Drawing.Size(378, 280);
            this.employeesListBox.TabIndex = 10;
            // 
            // employeeLabel
            // 
            this.employeeLabel.AutoSize = true;
            this.employeeLabel.Location = new System.Drawing.Point(13, 234);
            this.employeeLabel.Name = "employeeLabel";
            this.employeeLabel.Size = new System.Drawing.Size(64, 15);
            this.employeeLabel.TabIndex = 7;
            this.employeeLabel.Text = "Employees";
            this.employeeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 538);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(165, 538);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 12;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(281, 538);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "&Cancel";
            // 
            // overHoursSuffixLabel
            // 
            this.overHoursSuffixLabel.AutoSize = true;
            this.overHoursSuffixLabel.Location = new System.Drawing.Point(283, 22);
            this.overHoursSuffixLabel.Name = "overHoursSuffixLabel";
            this.overHoursSuffixLabel.Size = new System.Drawing.Size(79, 15);
            this.overHoursSuffixLabel.TabIndex = 2;
            this.overHoursSuffixLabel.Text = "hours per day";
            this.overHoursSuffixLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormTimeCardRuleEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 581);
            this.Controls.Add(this.overHoursSuffixLabel);
            this.Controls.Add(this.minClockinLabel);
            this.Controls.Add(this.overtimeExemptCheckBox);
            this.Controls.Add(this.minClockinTextBox);
            this.Controls.Add(this.overHoursTextBox);
            this.Controls.Add(this.overHoursLabel);
            this.Controls.Add(this.differentialHoursGroupBox);
            this.Controls.Add(this.employeesListBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.employeeLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(333, 350);
            this.Name = "FormTimeCardRuleEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Time Card Rule";
            this.Load += new System.EventHandler(this.FormTimeCardRuleEdit_Load);
            this.differentialHoursGroupBox.ResumeLayout(false);
            this.differentialHoursGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label employeeLabel;
        private System.Windows.Forms.Label overHoursLabel;
        private System.Windows.Forms.TextBox overHoursTextBox;
        private System.Windows.Forms.Label timeAfterLabel;
        private System.Windows.Forms.TextBox timeAfterTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.ListBox employeesListBox;
        private System.Windows.Forms.Button timeAfterButton;
        private System.Windows.Forms.Button timeBeforeButton;
        private System.Windows.Forms.Label timeBeforeLabel;
        private System.Windows.Forms.TextBox timeBeforeTextBox;
        private System.Windows.Forms.GroupBox differentialHoursGroupBox;
        private System.Windows.Forms.CheckBox overtimeExemptCheckBox;
        private System.Windows.Forms.Label minClockinLabel;
        private System.Windows.Forms.TextBox minClockinTextBox;
        private System.Windows.Forms.Label overHoursSuffixLabel;
    }
}
