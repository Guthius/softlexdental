namespace OpenDental
{
    partial class FormPayPeriodManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPeriodManager));
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.intervalWeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.intervalMonhtlyRadioButton = new System.Windows.Forms.RadioButton();
            this.payDayGroupBox = new System.Windows.Forms.GroupBox();
            this.orLabel = new System.Windows.Forms.Label();
            this.payBeforeRadioButton = new System.Windows.Forms.RadioButton();
            this.payAfterRadioButton = new System.Windows.Forms.RadioButton();
            this.excludeWeekendsCheckBox = new System.Windows.Forms.CheckBox();
            this.dayComboBox = new System.Windows.Forms.ComboBox();
            this.numDaysAfterTextBox = new System.Windows.Forms.TextBox();
            this.numDaysAfterLabel = new System.Windows.Forms.Label();
            this.dayLabel = new System.Windows.Forms.Label();
            this.intervalBiWeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.numPayPeriodsTextBox = new System.Windows.Forms.TextBox();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.numPayPeriodsLabel = new System.Windows.Forms.Label();
            this.startDateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.payPeriodGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.optionsGroupBox.SuspendLayout();
            this.payDayGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Controls.Add(this.intervalLabel);
            this.optionsGroupBox.Controls.Add(this.intervalWeeklyRadioButton);
            this.optionsGroupBox.Controls.Add(this.intervalMonhtlyRadioButton);
            this.optionsGroupBox.Controls.Add(this.payDayGroupBox);
            this.optionsGroupBox.Controls.Add(this.intervalBiWeeklyRadioButton);
            this.optionsGroupBox.Controls.Add(this.numPayPeriodsTextBox);
            this.optionsGroupBox.Controls.Add(this.startDateLabel);
            this.optionsGroupBox.Controls.Add(this.numPayPeriodsLabel);
            this.optionsGroupBox.Controls.Add(this.startDateDateTimePicker);
            this.optionsGroupBox.Location = new System.Drawing.Point(13, 19);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(362, 373);
            this.optionsGroupBox.TabIndex = 0;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Pay Period Options";
            // 
            // intervalLabel
            // 
            this.intervalLabel.AutoSize = true;
            this.intervalLabel.Location = new System.Drawing.Point(183, 82);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(46, 15);
            this.intervalLabel.TabIndex = 4;
            this.intervalLabel.Text = "Interval";
            this.intervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // intervalWeeklyRadioButton
            // 
            this.intervalWeeklyRadioButton.AutoSize = true;
            this.intervalWeeklyRadioButton.Checked = true;
            this.intervalWeeklyRadioButton.Location = new System.Drawing.Point(235, 80);
            this.intervalWeeklyRadioButton.Name = "intervalWeeklyRadioButton";
            this.intervalWeeklyRadioButton.Size = new System.Drawing.Size(63, 19);
            this.intervalWeeklyRadioButton.TabIndex = 5;
            this.intervalWeeklyRadioButton.TabStop = true;
            this.intervalWeeklyRadioButton.Text = "Weekly";
            this.intervalWeeklyRadioButton.UseVisualStyleBackColor = true;
            this.intervalWeeklyRadioButton.Click += new System.EventHandler(this.WeeklyRadioButton_Click);
            // 
            // intervalMonhtlyRadioButton
            // 
            this.intervalMonhtlyRadioButton.AutoSize = true;
            this.intervalMonhtlyRadioButton.Location = new System.Drawing.Point(235, 130);
            this.intervalMonhtlyRadioButton.Name = "intervalMonhtlyRadioButton";
            this.intervalMonhtlyRadioButton.Size = new System.Drawing.Size(70, 19);
            this.intervalMonhtlyRadioButton.TabIndex = 7;
            this.intervalMonhtlyRadioButton.Text = "Monthly";
            this.intervalMonhtlyRadioButton.UseVisualStyleBackColor = true;
            this.intervalMonhtlyRadioButton.Click += new System.EventHandler(this.MonthlyRadioButton_Click);
            // 
            // payDayGroupBox
            // 
            this.payDayGroupBox.Controls.Add(this.orLabel);
            this.payDayGroupBox.Controls.Add(this.payBeforeRadioButton);
            this.payDayGroupBox.Controls.Add(this.payAfterRadioButton);
            this.payDayGroupBox.Controls.Add(this.excludeWeekendsCheckBox);
            this.payDayGroupBox.Controls.Add(this.dayComboBox);
            this.payDayGroupBox.Controls.Add(this.numDaysAfterTextBox);
            this.payDayGroupBox.Controls.Add(this.numDaysAfterLabel);
            this.payDayGroupBox.Controls.Add(this.dayLabel);
            this.payDayGroupBox.Location = new System.Drawing.Point(6, 155);
            this.payDayGroupBox.Name = "payDayGroupBox";
            this.payDayGroupBox.Size = new System.Drawing.Size(350, 212);
            this.payDayGroupBox.TabIndex = 8;
            this.payDayGroupBox.TabStop = false;
            this.payDayGroupBox.Text = "Pay Day";
            // 
            // orLabel
            // 
            this.orLabel.AutoSize = true;
            this.orLabel.Location = new System.Drawing.Point(201, 48);
            this.orLabel.Name = "orLabel";
            this.orLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.orLabel.Size = new System.Drawing.Size(23, 25);
            this.orLabel.TabIndex = 2;
            this.orLabel.Text = "OR";
            // 
            // payBeforeRadioButton
            // 
            this.payBeforeRadioButton.AutoSize = true;
            this.payBeforeRadioButton.Checked = true;
            this.payBeforeRadioButton.Enabled = false;
            this.payBeforeRadioButton.Location = new System.Drawing.Point(226, 137);
            this.payBeforeRadioButton.Name = "payBeforeRadioButton";
            this.payBeforeRadioButton.Size = new System.Drawing.Size(81, 19);
            this.payBeforeRadioButton.TabIndex = 6;
            this.payBeforeRadioButton.TabStop = true;
            this.payBeforeRadioButton.Text = "Pay Before";
            this.payBeforeRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.payBeforeRadioButton.UseVisualStyleBackColor = true;
            // 
            // payAfterRadioButton
            // 
            this.payAfterRadioButton.AutoSize = true;
            this.payAfterRadioButton.Enabled = false;
            this.payAfterRadioButton.Location = new System.Drawing.Point(226, 162);
            this.payAfterRadioButton.Name = "payAfterRadioButton";
            this.payAfterRadioButton.Size = new System.Drawing.Size(73, 19);
            this.payAfterRadioButton.TabIndex = 7;
            this.payAfterRadioButton.Text = "Pay After";
            this.payAfterRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.payAfterRadioButton.UseVisualStyleBackColor = true;
            // 
            // excludeWeekendsCheckBox
            // 
            this.excludeWeekendsCheckBox.AutoSize = true;
            this.excludeWeekendsCheckBox.Location = new System.Drawing.Point(204, 112);
            this.excludeWeekendsCheckBox.Name = "excludeWeekendsCheckBox";
            this.excludeWeekendsCheckBox.Size = new System.Drawing.Size(122, 19);
            this.excludeWeekendsCheckBox.TabIndex = 5;
            this.excludeWeekendsCheckBox.Text = "Exclude weekends";
            this.excludeWeekendsCheckBox.UseVisualStyleBackColor = true;
            this.excludeWeekendsCheckBox.CheckedChanged += new System.EventHandler(this.ExcludeWeekendsCheckBox_CheckedChanged);
            // 
            // dayComboBox
            // 
            this.dayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayComboBox.FormattingEnabled = true;
            this.dayComboBox.Items.AddRange(new object[] {
            "None",
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.dayComboBox.Location = new System.Drawing.Point(204, 22);
            this.dayComboBox.Name = "dayComboBox";
            this.dayComboBox.Size = new System.Drawing.Size(140, 23);
            this.dayComboBox.TabIndex = 1;
            this.dayComboBox.SelectionChangeCommitted += new System.EventHandler(this.DayComboBox_SelectionChangeCommitted);
            // 
            // numDaysAfterTextBox
            // 
            this.numDaysAfterTextBox.Location = new System.Drawing.Point(204, 76);
            this.numDaysAfterTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numDaysAfterTextBox.Name = "numDaysAfterTextBox";
            this.numDaysAfterTextBox.Size = new System.Drawing.Size(60, 23);
            this.numDaysAfterTextBox.TabIndex = 4;
            this.numDaysAfterTextBox.TextChanged += new System.EventHandler(this.NumDaysAfterTextBox_TextChanged);
            // 
            // numDaysAfterLabel
            // 
            this.numDaysAfterLabel.AutoSize = true;
            this.numDaysAfterLabel.Location = new System.Drawing.Point(20, 79);
            this.numDaysAfterLabel.Name = "numDaysAfterLabel";
            this.numDaysAfterLabel.Size = new System.Drawing.Size(178, 15);
            this.numDaysAfterLabel.TabIndex = 3;
            this.numDaysAfterLabel.Text = "Number of days after pay period";
            this.numDaysAfterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Location = new System.Drawing.Point(127, 25);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(71, 15);
            this.dayLabel.TabIndex = 0;
            this.dayLabel.Text = "Day of week";
            this.dayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // intervalBiWeeklyRadioButton
            // 
            this.intervalBiWeeklyRadioButton.AutoSize = true;
            this.intervalBiWeeklyRadioButton.Location = new System.Drawing.Point(235, 105);
            this.intervalBiWeeklyRadioButton.Name = "intervalBiWeeklyRadioButton";
            this.intervalBiWeeklyRadioButton.Size = new System.Drawing.Size(78, 19);
            this.intervalBiWeeklyRadioButton.TabIndex = 6;
            this.intervalBiWeeklyRadioButton.Text = "Bi-Weekly";
            this.intervalBiWeeklyRadioButton.UseVisualStyleBackColor = true;
            this.intervalBiWeeklyRadioButton.Click += new System.EventHandler(this.BiWeeklyRadioButton_Click);
            this.intervalBiWeeklyRadioButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BiWeeklyRadioButton_Click);
            // 
            // numPayPeriodsTextBox
            // 
            this.numPayPeriodsTextBox.Location = new System.Drawing.Point(235, 51);
            this.numPayPeriodsTextBox.Name = "numPayPeriodsTextBox";
            this.numPayPeriodsTextBox.Size = new System.Drawing.Size(60, 23);
            this.numPayPeriodsTextBox.TabIndex = 3;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(156, 25);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(74, 15);
            this.startDateLabel.TabIndex = 0;
            this.startDateLabel.Text = "Starting date";
            this.startDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPayPeriodsLabel
            // 
            this.numPayPeriodsLabel.AutoSize = true;
            this.numPayPeriodsLabel.Location = new System.Drawing.Point(37, 54);
            this.numPayPeriodsLabel.Name = "numPayPeriodsLabel";
            this.numPayPeriodsLabel.Size = new System.Drawing.Size(192, 15);
            this.numPayPeriodsLabel.TabIndex = 2;
            this.numPayPeriodsLabel.Text = "Number of pay periods to generate";
            this.numPayPeriodsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // startDateDateTimePicker
            // 
            this.startDateDateTimePicker.CustomFormat = "";
            this.startDateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDateDateTimePicker.Location = new System.Drawing.Point(236, 22);
            this.startDateDateTimePicker.Name = "startDateDateTimePicker";
            this.startDateDateTimePicker.Size = new System.Drawing.Size(120, 23);
            this.startDateDateTimePicker.TabIndex = 1;
            // 
            // payPeriodGrid
            // 
            this.payPeriodGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.payPeriodGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.payPeriodGrid.EditableEnterMovesDown = false;
            this.payPeriodGrid.HasAddButton = false;
            this.payPeriodGrid.HasDropDowns = false;
            this.payPeriodGrid.HasMultilineHeaders = false;
            this.payPeriodGrid.HScrollVisible = false;
            this.payPeriodGrid.Location = new System.Drawing.Point(381, 19);
            this.payPeriodGrid.Name = "payPeriodGrid";
            this.payPeriodGrid.ScrollValue = 0;
            this.payPeriodGrid.Size = new System.Drawing.Size(290, 373);
            this.payPeriodGrid.TabIndex = 1;
            this.payPeriodGrid.Title = "Pay Periods";
            this.payPeriodGrid.TitleVisible = true;
            this.payPeriodGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.PayPeriodGrid_CellDoubleClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(445, 398);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.generateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.generateButton.Location = new System.Drawing.Point(265, 398);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(110, 30);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "&Generate";
            this.generateButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generateButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(561, 398);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormPayPeriodManager
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(684, 441);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.payPeriodGrid);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPayPeriodManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pay Period Manager";
            this.Load += new System.EventHandler(this.FormPayPeriodManager_Load);
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.payDayGroupBox.ResumeLayout(false);
            this.payDayGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button generateButton;
        private UI.ODGrid payPeriodGrid;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.DateTimePicker startDateDateTimePicker;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.RadioButton intervalWeeklyRadioButton;
        private System.Windows.Forms.RadioButton intervalMonhtlyRadioButton;
        private System.Windows.Forms.RadioButton intervalBiWeeklyRadioButton;
        private System.Windows.Forms.GroupBox payDayGroupBox;
        private System.Windows.Forms.Label numDaysAfterLabel;
        private System.Windows.Forms.Label dayLabel;
        private System.Windows.Forms.Label numPayPeriodsLabel;
        private System.Windows.Forms.TextBox numDaysAfterTextBox;
        private System.Windows.Forms.TextBox numPayPeriodsTextBox;
        private System.Windows.Forms.ComboBox dayComboBox;
        private System.Windows.Forms.RadioButton payBeforeRadioButton;
        private System.Windows.Forms.RadioButton payAfterRadioButton;
        private System.Windows.Forms.CheckBox excludeWeekendsCheckBox;
        private System.Windows.Forms.Label intervalLabel;
        private System.Windows.Forms.Label orLabel;
    }
}
