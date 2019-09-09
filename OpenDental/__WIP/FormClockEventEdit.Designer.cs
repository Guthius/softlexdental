namespace OpenDental
{
    partial class FormClockEventEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClockEventEdit));
            this.date1EnteredTextBox = new System.Windows.Forms.TextBox();
            this.date1EnteredLabel = new System.Windows.Forms.Label();
            this.date1DisplayedLabel = new System.Windows.Forms.Label();
            this.date1DisplayedTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.noteLabel = new System.Windows.Forms.Label();
            this.dateStartGroupBox = new System.Windows.Forms.GroupBox();
            this.date1NowButton = new System.Windows.Forms.Button();
            this.dateEndGroupBox = new System.Windows.Forms.GroupBox();
            this.date2EnteredTextBox = new System.Windows.Forms.TextBox();
            this.date2EnteredLabel = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.date2DisplayedLabel = new System.Windows.Forms.Label();
            this.date2NowButton = new System.Windows.Forms.Button();
            this.date2DisplayedTextBox = new System.Windows.Forms.TextBox();
            this.overtimeTextBox = new System.Windows.Forms.TextBox();
            this.overtimeLabel = new System.Windows.Forms.Label();
            this.clockedTimeTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.regularTimeTextBox = new System.Windows.Forms.TextBox();
            this.regularTimeLabel = new System.Windows.Forms.Label();
            this.timeSpansGroupBox = new System.Windows.Forms.GroupBox();
            this.overtimeAutoTextBox = new System.Windows.Forms.TextBox();
            this.adjustTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.adjustLabel = new System.Windows.Forms.Label();
            this.adjustAutoTextBox = new System.Windows.Forms.TextBox();
            this.rate2AutoTextBox = new System.Windows.Forms.TextBox();
            this.rate2Label = new System.Windows.Forms.Label();
            this.rate2TextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.totalHoursLabel = new System.Windows.Forms.Label();
            this.totalHoursTextBox = new System.Windows.Forms.TextBox();
            this.rate1TextBox = new System.Windows.Forms.TextBox();
            this.rate1Label = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.rate2GroupBox = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.dateStartGroupBox.SuspendLayout();
            this.dateEndGroupBox.SuspendLayout();
            this.timeSpansGroupBox.SuspendLayout();
            this.rate2GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // date1EnteredTextBox
            // 
            this.date1EnteredTextBox.Location = new System.Drawing.Point(110, 24);
            this.date1EnteredTextBox.Name = "date1EnteredTextBox";
            this.date1EnteredTextBox.ReadOnly = true;
            this.date1EnteredTextBox.Size = new System.Drawing.Size(156, 23);
            this.date1EnteredTextBox.TabIndex = 1;
            // 
            // date1EnteredLabel
            // 
            this.date1EnteredLabel.AutoSize = true;
            this.date1EnteredLabel.Location = new System.Drawing.Point(54, 27);
            this.date1EnteredLabel.Name = "date1EnteredLabel";
            this.date1EnteredLabel.Size = new System.Drawing.Size(47, 15);
            this.date1EnteredLabel.TabIndex = 0;
            this.date1EnteredLabel.Text = "Entered";
            this.date1EnteredLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // date1DisplayedLabel
            // 
            this.date1DisplayedLabel.AutoSize = true;
            this.date1DisplayedLabel.Location = new System.Drawing.Point(43, 56);
            this.date1DisplayedLabel.Name = "date1DisplayedLabel";
            this.date1DisplayedLabel.Size = new System.Drawing.Size(58, 15);
            this.date1DisplayedLabel.TabIndex = 2;
            this.date1DisplayedLabel.Text = "Displayed";
            this.date1DisplayedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // date1DisplayedTextBox
            // 
            this.date1DisplayedTextBox.Location = new System.Drawing.Point(110, 53);
            this.date1DisplayedTextBox.Name = "date1DisplayedTextBox";
            this.date1DisplayedTextBox.Size = new System.Drawing.Size(156, 23);
            this.date1DisplayedTextBox.TabIndex = 3;
            this.date1DisplayedTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Date1DisplayedTextBox_Validating);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(78, 22);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 15);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(101, 368);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(33, 15);
            this.noteLabel.TabIndex = 8;
            this.noteLabel.Text = "Note";
            // 
            // dateStartGroupBox
            // 
            this.dateStartGroupBox.Controls.Add(this.date1EnteredTextBox);
            this.dateStartGroupBox.Controls.Add(this.date1EnteredLabel);
            this.dateStartGroupBox.Controls.Add(this.date1NowButton);
            this.dateStartGroupBox.Controls.Add(this.date1DisplayedLabel);
            this.dateStartGroupBox.Controls.Add(this.date1DisplayedTextBox);
            this.dateStartGroupBox.Location = new System.Drawing.Point(13, 63);
            this.dateStartGroupBox.Name = "dateStartGroupBox";
            this.dateStartGroupBox.Size = new System.Drawing.Size(300, 120);
            this.dateStartGroupBox.TabIndex = 4;
            this.dateStartGroupBox.TabStop = false;
            this.dateStartGroupBox.Text = "Start Date and Time";
            // 
            // date1NowButton
            // 
            this.date1NowButton.Image = ((System.Drawing.Image)(resources.GetObject("date1NowButton.Image")));
            this.date1NowButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.date1NowButton.Location = new System.Drawing.Point(110, 82);
            this.date1NowButton.Name = "date1NowButton";
            this.date1NowButton.Size = new System.Drawing.Size(80, 30);
            this.date1NowButton.TabIndex = 4;
            this.date1NowButton.Text = "Now";
            this.date1NowButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.date1NowButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.date1NowButton.Click += new System.EventHandler(this.Date1NowButton_Click);
            // 
            // dateEndGroupBox
            // 
            this.dateEndGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateEndGroupBox.Controls.Add(this.date2EnteredTextBox);
            this.dateEndGroupBox.Controls.Add(this.date2EnteredLabel);
            this.dateEndGroupBox.Controls.Add(this.clearButton);
            this.dateEndGroupBox.Controls.Add(this.date2DisplayedLabel);
            this.dateEndGroupBox.Controls.Add(this.date2NowButton);
            this.dateEndGroupBox.Controls.Add(this.date2DisplayedTextBox);
            this.dateEndGroupBox.Location = new System.Drawing.Point(319, 63);
            this.dateEndGroupBox.Name = "dateEndGroupBox";
            this.dateEndGroupBox.Size = new System.Drawing.Size(302, 120);
            this.dateEndGroupBox.TabIndex = 5;
            this.dateEndGroupBox.TabStop = false;
            this.dateEndGroupBox.Text = "End Date and Time";
            // 
            // date2EnteredTextBox
            // 
            this.date2EnteredTextBox.Location = new System.Drawing.Point(110, 24);
            this.date2EnteredTextBox.Name = "date2EnteredTextBox";
            this.date2EnteredTextBox.ReadOnly = true;
            this.date2EnteredTextBox.Size = new System.Drawing.Size(156, 23);
            this.date2EnteredTextBox.TabIndex = 1;
            // 
            // date2EnteredLabel
            // 
            this.date2EnteredLabel.AutoSize = true;
            this.date2EnteredLabel.Location = new System.Drawing.Point(57, 27);
            this.date2EnteredLabel.Name = "date2EnteredLabel";
            this.date2EnteredLabel.Size = new System.Drawing.Size(47, 15);
            this.date2EnteredLabel.TabIndex = 0;
            this.date2EnteredLabel.Text = "Entered";
            this.date2EnteredLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // clearButton
            // 
            this.clearButton.Image = global::OpenDental.Properties.Resources.IconEraser;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearButton.Location = new System.Drawing.Point(196, 82);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(80, 30);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "Clear";
            this.clearButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // date2DisplayedLabel
            // 
            this.date2DisplayedLabel.AutoSize = true;
            this.date2DisplayedLabel.Location = new System.Drawing.Point(46, 56);
            this.date2DisplayedLabel.Name = "date2DisplayedLabel";
            this.date2DisplayedLabel.Size = new System.Drawing.Size(58, 15);
            this.date2DisplayedLabel.TabIndex = 2;
            this.date2DisplayedLabel.Text = "Displayed";
            this.date2DisplayedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // date2NowButton
            // 
            this.date2NowButton.Image = ((System.Drawing.Image)(resources.GetObject("date2NowButton.Image")));
            this.date2NowButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.date2NowButton.Location = new System.Drawing.Point(110, 82);
            this.date2NowButton.Name = "date2NowButton";
            this.date2NowButton.Size = new System.Drawing.Size(80, 30);
            this.date2NowButton.TabIndex = 4;
            this.date2NowButton.Text = "Now";
            this.date2NowButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.date2NowButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.date2NowButton.Click += new System.EventHandler(this.Date2NowButton_Click);
            // 
            // date2DisplayedTextBox
            // 
            this.date2DisplayedTextBox.Location = new System.Drawing.Point(110, 53);
            this.date2DisplayedTextBox.Name = "date2DisplayedTextBox";
            this.date2DisplayedTextBox.Size = new System.Drawing.Size(156, 23);
            this.date2DisplayedTextBox.TabIndex = 3;
            this.date2DisplayedTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Date2DisplayedTextBox_Validating);
            // 
            // overtimeTextBox
            // 
            this.overtimeTextBox.Location = new System.Drawing.Point(196, 95);
            this.overtimeTextBox.Name = "overtimeTextBox";
            this.overtimeTextBox.Size = new System.Drawing.Size(70, 23);
            this.overtimeTextBox.TabIndex = 9;
            this.overtimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.overtimeTextBox.TextChanged += new System.EventHandler(this.TimeSpan_TextChanged);
            this.overtimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TimeSpan_Validating);
            // 
            // overtimeLabel
            // 
            this.overtimeLabel.AutoSize = true;
            this.overtimeLabel.Location = new System.Drawing.Point(50, 98);
            this.overtimeLabel.Name = "overtimeLabel";
            this.overtimeLabel.Size = new System.Drawing.Size(64, 15);
            this.overtimeLabel.TabIndex = 7;
            this.overtimeLabel.Text = "- Overtime";
            this.overtimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // clockedTimeTextBox
            // 
            this.clockedTimeTextBox.Location = new System.Drawing.Point(120, 37);
            this.clockedTimeTextBox.Name = "clockedTimeTextBox";
            this.clockedTimeTextBox.ReadOnly = true;
            this.clockedTimeTextBox.Size = new System.Drawing.Size(70, 23);
            this.clockedTimeTextBox.TabIndex = 3;
            this.clockedTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Clocked Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // regularTimeTextBox
            // 
            this.regularTimeTextBox.Location = new System.Drawing.Point(120, 124);
            this.regularTimeTextBox.Name = "regularTimeTextBox";
            this.regularTimeTextBox.ReadOnly = true;
            this.regularTimeTextBox.Size = new System.Drawing.Size(70, 23);
            this.regularTimeTextBox.TabIndex = 11;
            this.regularTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // regularTimeLabel
            // 
            this.regularTimeLabel.AutoSize = true;
            this.regularTimeLabel.Location = new System.Drawing.Point(38, 127);
            this.regularTimeLabel.Name = "regularTimeLabel";
            this.regularTimeLabel.Size = new System.Drawing.Size(76, 15);
            this.regularTimeLabel.TabIndex = 10;
            this.regularTimeLabel.Text = "Regular Time";
            this.regularTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeSpansGroupBox
            // 
            this.timeSpansGroupBox.Controls.Add(this.overtimeAutoTextBox);
            this.timeSpansGroupBox.Controls.Add(this.adjustTextBox);
            this.timeSpansGroupBox.Controls.Add(this.label12);
            this.timeSpansGroupBox.Controls.Add(this.label11);
            this.timeSpansGroupBox.Controls.Add(this.adjustLabel);
            this.timeSpansGroupBox.Controls.Add(this.adjustAutoTextBox);
            this.timeSpansGroupBox.Controls.Add(this.label8);
            this.timeSpansGroupBox.Controls.Add(this.regularTimeTextBox);
            this.timeSpansGroupBox.Controls.Add(this.overtimeLabel);
            this.timeSpansGroupBox.Controls.Add(this.regularTimeLabel);
            this.timeSpansGroupBox.Controls.Add(this.overtimeTextBox);
            this.timeSpansGroupBox.Controls.Add(this.clockedTimeTextBox);
            this.timeSpansGroupBox.Location = new System.Drawing.Point(13, 189);
            this.timeSpansGroupBox.Name = "timeSpansGroupBox";
            this.timeSpansGroupBox.Size = new System.Drawing.Size(290, 170);
            this.timeSpansGroupBox.TabIndex = 6;
            this.timeSpansGroupBox.TabStop = false;
            this.timeSpansGroupBox.Text = "Time Spans";
            // 
            // overtimeAutoTextBox
            // 
            this.overtimeAutoTextBox.Location = new System.Drawing.Point(120, 95);
            this.overtimeAutoTextBox.Name = "overtimeAutoTextBox";
            this.overtimeAutoTextBox.ReadOnly = true;
            this.overtimeAutoTextBox.Size = new System.Drawing.Size(70, 23);
            this.overtimeAutoTextBox.TabIndex = 8;
            this.overtimeAutoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // adjustTextBox
            // 
            this.adjustTextBox.Location = new System.Drawing.Point(196, 66);
            this.adjustTextBox.Name = "adjustTextBox";
            this.adjustTextBox.Size = new System.Drawing.Size(70, 23);
            this.adjustTextBox.TabIndex = 6;
            this.adjustTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.adjustTextBox.TextChanged += new System.EventHandler(this.TimeSpan_TextChanged);
            this.adjustTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TimeSpan_Validating);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(193, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 15);
            this.label12.TabIndex = 1;
            this.label12.Text = "Override";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(117, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Calculated";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // adjustLabel
            // 
            this.adjustLabel.AutoSize = true;
            this.adjustLabel.Location = new System.Drawing.Point(34, 69);
            this.adjustLabel.Name = "adjustLabel";
            this.adjustLabel.Size = new System.Drawing.Size(80, 15);
            this.adjustLabel.TabIndex = 4;
            this.adjustLabel.Text = "+ Adjustment";
            this.adjustLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // adjustAutoTextBox
            // 
            this.adjustAutoTextBox.Location = new System.Drawing.Point(120, 66);
            this.adjustAutoTextBox.Name = "adjustAutoTextBox";
            this.adjustAutoTextBox.ReadOnly = true;
            this.adjustAutoTextBox.Size = new System.Drawing.Size(70, 23);
            this.adjustAutoTextBox.TabIndex = 5;
            this.adjustAutoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rate2AutoTextBox
            // 
            this.rate2AutoTextBox.Location = new System.Drawing.Point(150, 66);
            this.rate2AutoTextBox.Name = "rate2AutoTextBox";
            this.rate2AutoTextBox.ReadOnly = true;
            this.rate2AutoTextBox.Size = new System.Drawing.Size(70, 23);
            this.rate2AutoTextBox.TabIndex = 5;
            this.rate2AutoTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rate2Label
            // 
            this.rate2Label.AutoSize = true;
            this.rate2Label.Location = new System.Drawing.Point(68, 69);
            this.rate2Label.Name = "rate2Label";
            this.rate2Label.Size = new System.Drawing.Size(76, 15);
            this.rate2Label.TabIndex = 4;
            this.rate2Label.Text = "- Rate 2 Time";
            this.rate2Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rate2TextBox
            // 
            this.rate2TextBox.Location = new System.Drawing.Point(226, 66);
            this.rate2TextBox.Name = "rate2TextBox";
            this.rate2TextBox.Size = new System.Drawing.Size(70, 23);
            this.rate2TextBox.TabIndex = 6;
            this.rate2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.rate2TextBox.TextChanged += new System.EventHandler(this.TimeSpan_TextChanged);
            this.rate2TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TimeSpan_Validating);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 468);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(395, 468);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 11;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(511, 468);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "&Cancel";
            // 
            // totalHoursLabel
            // 
            this.totalHoursLabel.AutoSize = true;
            this.totalHoursLabel.Location = new System.Drawing.Point(83, 40);
            this.totalHoursLabel.Name = "totalHoursLabel";
            this.totalHoursLabel.Size = new System.Drawing.Size(61, 15);
            this.totalHoursLabel.TabIndex = 2;
            this.totalHoursLabel.Text = "Total Time";
            this.totalHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalHoursTextBox
            // 
            this.totalHoursTextBox.Location = new System.Drawing.Point(150, 37);
            this.totalHoursTextBox.Name = "totalHoursTextBox";
            this.totalHoursTextBox.ReadOnly = true;
            this.totalHoursTextBox.Size = new System.Drawing.Size(70, 23);
            this.totalHoursTextBox.TabIndex = 3;
            this.totalHoursTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rate1TextBox
            // 
            this.rate1TextBox.Location = new System.Drawing.Point(150, 95);
            this.rate1TextBox.Name = "rate1TextBox";
            this.rate1TextBox.ReadOnly = true;
            this.rate1TextBox.Size = new System.Drawing.Size(70, 23);
            this.rate1TextBox.TabIndex = 8;
            this.rate1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rate1Label
            // 
            this.rate1Label.AutoSize = true;
            this.rate1Label.Location = new System.Drawing.Point(76, 98);
            this.rate1Label.Name = "rate1Label";
            this.rate1Label.Size = new System.Drawing.Size(68, 15);
            this.rate1Label.TabIndex = 7;
            this.rate1Label.Text = "Rate 1 Time";
            this.rate1Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(223, 19);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 15);
            this.label17.TabIndex = 1;
            this.label17.Text = "Override";
            this.label17.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // rate2GroupBox
            // 
            this.rate2GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rate2GroupBox.Controls.Add(this.label18);
            this.rate2GroupBox.Controls.Add(this.label17);
            this.rate2GroupBox.Controls.Add(this.rate2TextBox);
            this.rate2GroupBox.Controls.Add(this.rate2Label);
            this.rate2GroupBox.Controls.Add(this.rate1TextBox);
            this.rate2GroupBox.Controls.Add(this.rate2AutoTextBox);
            this.rate2GroupBox.Controls.Add(this.rate1Label);
            this.rate2GroupBox.Controls.Add(this.totalHoursTextBox);
            this.rate2GroupBox.Controls.Add(this.totalHoursLabel);
            this.rate2GroupBox.Location = new System.Drawing.Point(309, 189);
            this.rate2GroupBox.Name = "rate2GroupBox";
            this.rate2GroupBox.Size = new System.Drawing.Size(312, 170);
            this.rate2GroupBox.TabIndex = 7;
            this.rate2GroupBox.TabStop = false;
            this.rate2GroupBox.Text = "Rate 2";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(147, 19);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 15);
            this.label18.TabIndex = 0;
            this.label18.Text = "Calculated";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsReturn = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.noteTextBox.Location = new System.Drawing.Point(140, 365);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.noteTextBox.Size = new System.Drawing.Size(481, 80);
            this.noteTextBox.TabIndex = 9;
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(297, 22);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 2;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.FormattingEnabled = true;
            this.clinicComboBox.Location = new System.Drawing.Point(340, 19);
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(160, 23);
            this.clinicComboBox.TabIndex = 3;
            // 
            // statusComboBox
            // 
            this.statusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(123, 19);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(121, 23);
            this.statusComboBox.TabIndex = 1;
            // 
            // FormClockEventEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(634, 511);
            this.Controls.Add(this.statusComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.rate2GroupBox);
            this.Controls.Add(this.timeSpansGroupBox);
            this.Controls.Add(this.dateEndGroupBox);
            this.Controls.Add(this.dateStartGroupBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormClockEventEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clock Event";
            this.Load += new System.EventHandler(this.FormClockEventEdit_Load);
            this.dateStartGroupBox.ResumeLayout(false);
            this.dateStartGroupBox.PerformLayout();
            this.dateEndGroupBox.ResumeLayout(false);
            this.dateEndGroupBox.PerformLayout();
            this.timeSpansGroupBox.ResumeLayout(false);
            this.timeSpansGroupBox.PerformLayout();
            this.rate2GroupBox.ResumeLayout(false);
            this.rate2GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox date1EnteredTextBox;
        private System.Windows.Forms.Label date1EnteredLabel;
        private System.Windows.Forms.Label date1DisplayedLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.TextBox date1DisplayedTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.GroupBox dateStartGroupBox;
        private System.Windows.Forms.GroupBox dateEndGroupBox;
        private System.Windows.Forms.TextBox date2EnteredTextBox;
        private System.Windows.Forms.Label date2EnteredLabel;
        private System.Windows.Forms.Label date2DisplayedLabel;
        private System.Windows.Forms.TextBox date2DisplayedTextBox;
        private System.Windows.Forms.Button date2NowButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button date1NowButton;
        private System.Windows.Forms.TextBox overtimeTextBox;
        private System.Windows.Forms.Label overtimeLabel;
        private System.Windows.Forms.TextBox clockedTimeTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox regularTimeTextBox;
        private System.Windows.Forms.Label regularTimeLabel;
        private System.Windows.Forms.GroupBox timeSpansGroupBox;
        private System.Windows.Forms.TextBox overtimeAutoTextBox;
        private System.Windows.Forms.TextBox adjustTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label adjustLabel;
        private System.Windows.Forms.TextBox adjustAutoTextBox;
        private System.Windows.Forms.TextBox rate2AutoTextBox;
        private System.Windows.Forms.Label rate2Label;
        private System.Windows.Forms.TextBox rate2TextBox;
        private System.Windows.Forms.Label totalHoursLabel;
        private System.Windows.Forms.TextBox totalHoursTextBox;
        private System.Windows.Forms.TextBox rate1TextBox;
        private System.Windows.Forms.Label rate1Label;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox rate2GroupBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox noteTextBox;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.ComboBox statusComboBox;
    }
}
