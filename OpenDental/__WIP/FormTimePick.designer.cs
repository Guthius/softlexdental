namespace OpenDental
{
    partial class FormTimePick
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimePick));
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.pmRadioButton = new System.Windows.Forms.RadioButton();
            this.amRadioButton = new System.Windows.Forms.RadioButton();
            this.splitLabel = new System.Windows.Forms.Label();
            this.minuteComboBox = new System.Windows.Forms.ComboBox();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.dateGroupBox = new System.Windows.Forms.GroupBox();
            this.timeGroupBox = new System.Windows.Forms.GroupBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dateGroupBox.SuspendLayout();
            this.timeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.Location = new System.Drawing.Point(6, 19);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(246, 23);
            this.dateTimePicker.TabIndex = 0;
            // 
            // pmRadioButton
            // 
            this.pmRadioButton.AutoSize = true;
            this.pmRadioButton.Location = new System.Drawing.Point(197, 21);
            this.pmRadioButton.Name = "pmRadioButton";
            this.pmRadioButton.Size = new System.Drawing.Size(43, 19);
            this.pmRadioButton.TabIndex = 4;
            this.pmRadioButton.Text = "PM";
            this.pmRadioButton.UseVisualStyleBackColor = true;
            this.pmRadioButton.Visible = false;
            // 
            // amRadioButton
            // 
            this.amRadioButton.AutoSize = true;
            this.amRadioButton.Checked = true;
            this.amRadioButton.Location = new System.Drawing.Point(147, 21);
            this.amRadioButton.Name = "amRadioButton";
            this.amRadioButton.Size = new System.Drawing.Size(44, 19);
            this.amRadioButton.TabIndex = 3;
            this.amRadioButton.TabStop = true;
            this.amRadioButton.Text = "AM";
            this.amRadioButton.UseVisualStyleBackColor = true;
            this.amRadioButton.Visible = false;
            // 
            // splitLabel
            // 
            this.splitLabel.AutoSize = true;
            this.splitLabel.Location = new System.Drawing.Point(65, 22);
            this.splitLabel.Name = "splitLabel";
            this.splitLabel.Size = new System.Drawing.Size(10, 15);
            this.splitLabel.TabIndex = 1;
            this.splitLabel.Text = ":";
            this.splitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // minuteComboBox
            // 
            this.minuteComboBox.FormattingEnabled = true;
            this.minuteComboBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.minuteComboBox.Location = new System.Drawing.Point(80, 19);
            this.minuteComboBox.MaxDropDownItems = 48;
            this.minuteComboBox.Name = "minuteComboBox";
            this.minuteComboBox.Size = new System.Drawing.Size(54, 23);
            this.minuteComboBox.TabIndex = 2;
            this.minuteComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HourComboBox_KeyPress);
            this.minuteComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.MinuteComboBox_Validating);
            // 
            // hourComboBox
            // 
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.hourComboBox.Location = new System.Drawing.Point(6, 19);
            this.hourComboBox.MaxDropDownItems = 48;
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(54, 23);
            this.hourComboBox.TabIndex = 0;
            this.hourComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HourComboBox_KeyPress);
            this.hourComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.HourComboBox_Validating);
            // 
            // dateGroupBox
            // 
            this.dateGroupBox.Controls.Add(this.dateTimePicker);
            this.dateGroupBox.Location = new System.Drawing.Point(13, 19);
            this.dateGroupBox.Name = "dateGroupBox";
            this.dateGroupBox.Size = new System.Drawing.Size(258, 50);
            this.dateGroupBox.TabIndex = 0;
            this.dateGroupBox.TabStop = false;
            this.dateGroupBox.Text = "Pick Date";
            // 
            // timeGroupBox
            // 
            this.timeGroupBox.Controls.Add(this.hourComboBox);
            this.timeGroupBox.Controls.Add(this.pmRadioButton);
            this.timeGroupBox.Controls.Add(this.minuteComboBox);
            this.timeGroupBox.Controls.Add(this.amRadioButton);
            this.timeGroupBox.Controls.Add(this.splitLabel);
            this.timeGroupBox.Location = new System.Drawing.Point(13, 75);
            this.timeGroupBox.Name = "timeGroupBox";
            this.timeGroupBox.Size = new System.Drawing.Size(258, 50);
            this.timeGroupBox.TabIndex = 1;
            this.timeGroupBox.TabStop = false;
            this.timeGroupBox.Text = "Pick Time";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(45, 148);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(161, 148);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormTimePick
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 191);
            this.ControlBox = false;
            this.Controls.Add(this.timeGroupBox);
            this.Controls.Add(this.dateGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTimePick";
            this.ShowInTaskbar = false;
            this.Text = "Pick Time";
            this.Load += new System.EventHandler(this.FormTimePick_Load);
            this.dateGroupBox.ResumeLayout(false);
            this.timeGroupBox.ResumeLayout(false);
            this.timeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox hourComboBox;
        private System.Windows.Forms.ComboBox minuteComboBox;
        private System.Windows.Forms.Label splitLabel;
        private System.Windows.Forms.RadioButton amRadioButton;
        private System.Windows.Forms.RadioButton pmRadioButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.GroupBox dateGroupBox;
        private System.Windows.Forms.GroupBox timeGroupBox;
    }
}