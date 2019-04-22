namespace OpenDental
{
    partial class ContrStaff
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
            this.components = new System.ComponentModel.Container();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.textTime = new System.Windows.Forms.Label();
            this.timerUpdateTime = new System.Windows.Forms.Timer(this.components);
            this.timeClockGroupBox = new System.Windows.Forms.GroupBox();
            this.butViewSched = new System.Windows.Forms.Button();
            this.butManage = new System.Windows.Forms.Button();
            this.butBreaks = new System.Windows.Forms.Button();
            this.employeeGrid = new OpenDental.UI.ODGrid();
            this.labelCurrentTime = new System.Windows.Forms.Label();
            this.butClockOut = new System.Windows.Forms.Button();
            this.butTimeCard = new System.Windows.Forms.Button();
            this.butClockIn = new System.Windows.Forms.Button();
            this.messagingGroupBox = new System.Windows.Forms.GroupBox();
            this.listMessages = new System.Windows.Forms.ListBox();
            this.butSend = new System.Windows.Forms.Button();
            this.butAck = new System.Windows.Forms.Button();
            this.sendingLabel = new System.Windows.Forms.Label();
            this.textDays = new System.Windows.Forms.TextBox();
            this.daysLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.gridMessages = new OpenDental.UI.ODGrid();
            this.checkIncludeAck = new System.Windows.Forms.CheckBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listExtras = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listFrom = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listTo = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.dailyActionsGroupBox = new System.Windows.Forms.GroupBox();
            this.erasButton = new System.Windows.Forms.Button();
            this.importInsPlansButton = new System.Windows.Forms.Button();
            this.emailButton = new System.Windows.Forms.Button();
            this.supplyButton = new System.Windows.Forms.Button();
            this.claimPayButton = new System.Windows.Forms.Button();
            this.billingButton = new System.Windows.Forms.Button();
            this.accountingButton = new System.Windows.Forms.Button();
            this.backupButton = new System.Windows.Forms.Button();
            this.depositsButton = new System.Windows.Forms.Button();
            this.sendClaimsButton = new System.Windows.Forms.Button();
            this.tasksButton = new System.Windows.Forms.Button();
            this.collectionsButton = new System.Windows.Forms.Button();
            this.timeClockGroupBox.SuspendLayout();
            this.messagingGroupBox.SuspendLayout();
            this.dailyActionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // listStatus
            // 
            this.listStatus.ItemHeight = 15;
            this.listStatus.Location = new System.Drawing.Point(367, 217);
            this.listStatus.Name = "listStatus";
            this.listStatus.Size = new System.Drawing.Size(107, 34);
            this.listStatus.TabIndex = 9;
            // 
            // textTime
            // 
            this.textTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTime.Location = new System.Drawing.Point(365, 138);
            this.textTime.Name = "textTime";
            this.textTime.Size = new System.Drawing.Size(109, 21);
            this.textTime.TabIndex = 6;
            this.textTime.Text = "12:00:00 PM";
            this.textTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // timerUpdateTime
            // 
            this.timerUpdateTime.Enabled = true;
            this.timerUpdateTime.Interval = 1000;
            this.timerUpdateTime.Tick += new System.EventHandler(this.timerUpdateTime_Tick);
            // 
            // timeClockGroupBox
            // 
            this.timeClockGroupBox.Controls.Add(this.butViewSched);
            this.timeClockGroupBox.Controls.Add(this.butManage);
            this.timeClockGroupBox.Controls.Add(this.butBreaks);
            this.timeClockGroupBox.Controls.Add(this.employeeGrid);
            this.timeClockGroupBox.Controls.Add(this.labelCurrentTime);
            this.timeClockGroupBox.Controls.Add(this.listStatus);
            this.timeClockGroupBox.Controls.Add(this.butClockOut);
            this.timeClockGroupBox.Controls.Add(this.butTimeCard);
            this.timeClockGroupBox.Controls.Add(this.textTime);
            this.timeClockGroupBox.Controls.Add(this.butClockIn);
            this.timeClockGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeClockGroupBox.Location = new System.Drawing.Point(289, 3);
            this.timeClockGroupBox.Name = "timeClockGroupBox";
            this.timeClockGroupBox.Size = new System.Drawing.Size(530, 270);
            this.timeClockGroupBox.TabIndex = 1;
            this.timeClockGroupBox.TabStop = false;
            this.timeClockGroupBox.Text = "Time Clock";
            // 
            // butViewSched
            // 
            this.butViewSched.Location = new System.Drawing.Point(366, 94);
            this.butViewSched.Name = "butViewSched";
            this.butViewSched.Size = new System.Drawing.Size(108, 25);
            this.butViewSched.TabIndex = 4;
            this.butViewSched.Text = "View Schedule";
            this.butViewSched.Click += new System.EventHandler(this.butViewSched_Click);
            // 
            // butManage
            // 
            this.butManage.Location = new System.Drawing.Point(366, 13);
            this.butManage.Name = "butManage";
            this.butManage.Size = new System.Drawing.Size(108, 25);
            this.butManage.TabIndex = 1;
            this.butManage.Text = "Manage";
            this.butManage.Click += new System.EventHandler(this.butManage_Click);
            // 
            // butBreaks
            // 
            this.butBreaks.Location = new System.Drawing.Point(366, 67);
            this.butBreaks.Name = "butBreaks";
            this.butBreaks.Size = new System.Drawing.Size(108, 25);
            this.butBreaks.TabIndex = 3;
            this.butBreaks.Text = "View Breaks";
            this.butBreaks.Click += new System.EventHandler(this.butBreaks_Click);
            // 
            // employeeGrid
            // 
            this.employeeGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.employeeGrid.EditableEnterMovesDown = false;
            this.employeeGrid.HasAddButton = false;
            this.employeeGrid.HasDropDowns = false;
            this.employeeGrid.HasMultilineHeaders = false;
            this.employeeGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.employeeGrid.HeaderHeight = 15;
            this.employeeGrid.HScrollVisible = false;
            this.employeeGrid.Location = new System.Drawing.Point(6, 22);
            this.employeeGrid.Name = "employeeGrid";
            this.employeeGrid.ScrollValue = 0;
            this.employeeGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.employeeGrid.Size = new System.Drawing.Size(303, 242);
            this.employeeGrid.TabIndex = 0;
            this.employeeGrid.Title = "Employee";
            this.employeeGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.employeeGrid.TitleHeight = 18;
            this.employeeGrid.TranslationName = "TableEmpClock";
            this.employeeGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridEmp_CellDoubleClick);
            this.employeeGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.employeeGrid_CellClick);
            // 
            // labelCurrentTime
            // 
            this.labelCurrentTime.Location = new System.Drawing.Point(376, 121);
            this.labelCurrentTime.Name = "labelCurrentTime";
            this.labelCurrentTime.Size = new System.Drawing.Size(88, 17);
            this.labelCurrentTime.TabIndex = 5;
            this.labelCurrentTime.Text = "Server Time";
            this.labelCurrentTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // butClockOut
            // 
            this.butClockOut.Location = new System.Drawing.Point(366, 189);
            this.butClockOut.Name = "butClockOut";
            this.butClockOut.Size = new System.Drawing.Size(108, 25);
            this.butClockOut.TabIndex = 8;
            this.butClockOut.Text = "Clock Out For:";
            this.butClockOut.Click += new System.EventHandler(this.butClockOut_Click);
            // 
            // butTimeCard
            // 
            this.butTimeCard.Location = new System.Drawing.Point(366, 40);
            this.butTimeCard.Name = "butTimeCard";
            this.butTimeCard.Size = new System.Drawing.Size(108, 25);
            this.butTimeCard.TabIndex = 2;
            this.butTimeCard.Text = "View Time Card";
            this.butTimeCard.Click += new System.EventHandler(this.butTimeCard_Click);
            // 
            // butClockIn
            // 
            this.butClockIn.Location = new System.Drawing.Point(366, 162);
            this.butClockIn.Name = "butClockIn";
            this.butClockIn.Size = new System.Drawing.Size(108, 25);
            this.butClockIn.TabIndex = 7;
            this.butClockIn.Text = "Clock In";
            this.butClockIn.Click += new System.EventHandler(this.butClockIn_Click);
            // 
            // messagingGroupBox
            // 
            this.messagingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.messagingGroupBox.Controls.Add(this.listMessages);
            this.messagingGroupBox.Controls.Add(this.butSend);
            this.messagingGroupBox.Controls.Add(this.butAck);
            this.messagingGroupBox.Controls.Add(this.sendingLabel);
            this.messagingGroupBox.Controls.Add(this.textDays);
            this.messagingGroupBox.Controls.Add(this.daysLabel);
            this.messagingGroupBox.Controls.Add(this.userLabel);
            this.messagingGroupBox.Controls.Add(this.userComboBox);
            this.messagingGroupBox.Controls.Add(this.gridMessages);
            this.messagingGroupBox.Controls.Add(this.checkIncludeAck);
            this.messagingGroupBox.Controls.Add(this.messageLabel);
            this.messagingGroupBox.Controls.Add(this.label5);
            this.messagingGroupBox.Controls.Add(this.listExtras);
            this.messagingGroupBox.Controls.Add(this.label4);
            this.messagingGroupBox.Controls.Add(this.listFrom);
            this.messagingGroupBox.Controls.Add(this.label3);
            this.messagingGroupBox.Controls.Add(this.listTo);
            this.messagingGroupBox.Controls.Add(this.label1);
            this.messagingGroupBox.Controls.Add(this.textMessage);
            this.messagingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.messagingGroupBox.Location = new System.Drawing.Point(3, 279);
            this.messagingGroupBox.Name = "messagingGroupBox";
            this.messagingGroupBox.Size = new System.Drawing.Size(902, 318);
            this.messagingGroupBox.TabIndex = 2;
            this.messagingGroupBox.TabStop = false;
            this.messagingGroupBox.Text = "Messaging";
            // 
            // listMessages
            // 
            this.listMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listMessages.FormattingEnabled = true;
            this.listMessages.ItemHeight = 15;
            this.listMessages.Location = new System.Drawing.Point(249, 51);
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(100, 199);
            this.listMessages.TabIndex = 7;
            this.listMessages.Click += new System.EventHandler(this.listMessages_Click);
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butSend.Location = new System.Drawing.Point(249, 288);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(100, 25);
            this.butSend.TabIndex = 10;
            this.butSend.Text = "Send Message";
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // butAck
            // 
            this.butAck.Location = new System.Drawing.Point(645, 21);
            this.butAck.Name = "butAck";
            this.butAck.Size = new System.Drawing.Size(67, 25);
            this.butAck.TabIndex = 15;
            this.butAck.Text = "Ack";
            this.butAck.Click += new System.EventHandler(this.butAck_Click);
            // 
            // sendingLabel
            // 
            this.sendingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sendingLabel.Location = new System.Drawing.Point(249, 265);
            this.sendingLabel.Name = "sendingLabel";
            this.sendingLabel.Size = new System.Drawing.Size(100, 20);
            this.sendingLabel.TabIndex = 11;
            this.sendingLabel.Text = "Sending";
            this.sendingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sendingLabel.Visible = false;
            // 
            // textDays
            // 
            this.textDays.Location = new System.Drawing.Point(594, 22);
            this.textDays.Name = "textDays";
            this.textDays.Size = new System.Drawing.Size(45, 23);
            this.textDays.TabIndex = 14;
            this.textDays.Visible = false;
            this.textDays.TextChanged += new System.EventHandler(this.textDays_TextChanged);
            // 
            // daysLabel
            // 
            this.daysLabel.AutoSize = true;
            this.daysLabel.Location = new System.Drawing.Point(556, 25);
            this.daysLabel.Name = "daysLabel";
            this.daysLabel.Size = new System.Drawing.Size(32, 15);
            this.daysLabel.TabIndex = 13;
            this.daysLabel.Text = "Days";
            this.daysLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.daysLabel.Visible = false;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(730, 25);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(46, 15);
            this.userLabel.TabIndex = 16;
            this.userLabel.Text = "To User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userComboBox
            // 
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(782, 22);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(114, 23);
            this.userComboBox.TabIndex = 17;
            this.userComboBox.SelectionChangeCommitted += new System.EventHandler(this.userComboBox_SelectionChangeCommitted);
            // 
            // gridMessages
            // 
            this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridMessages.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridMessages.EditableEnterMovesDown = false;
            this.gridMessages.HasAddButton = false;
            this.gridMessages.HasDropDowns = false;
            this.gridMessages.HasMultilineHeaders = false;
            this.gridMessages.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridMessages.HeaderHeight = 15;
            this.gridMessages.HScrollVisible = false;
            this.gridMessages.Location = new System.Drawing.Point(355, 51);
            this.gridMessages.Name = "gridMessages";
            this.gridMessages.ScrollValue = 0;
            this.gridMessages.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.gridMessages.Size = new System.Drawing.Size(541, 261);
            this.gridMessages.TabIndex = 18;
            this.gridMessages.Title = "Message History";
            this.gridMessages.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gridMessages.TitleHeight = 18;
            this.gridMessages.TranslationName = "TableTextMessages";
            // 
            // checkIncludeAck
            // 
            this.checkIncludeAck.AutoSize = true;
            this.checkIncludeAck.Location = new System.Drawing.Point(355, 24);
            this.checkIncludeAck.Name = "checkIncludeAck";
            this.checkIncludeAck.Size = new System.Drawing.Size(147, 19);
            this.checkIncludeAck.TabIndex = 12;
            this.checkIncludeAck.Text = "Include Acknowledged";
            this.checkIncludeAck.UseVisualStyleBackColor = true;
            this.checkIncludeAck.Click += new System.EventHandler(this.checkIncludeAck_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(3, 271);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(53, 15);
            this.messageLabel.TabIndex = 8;
            this.messageLabel.Text = "Message";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(246, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Message (&& Send)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listExtras
            // 
            this.listExtras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listExtras.FormattingEnabled = true;
            this.listExtras.ItemHeight = 15;
            this.listExtras.Location = new System.Drawing.Point(168, 51);
            this.listExtras.Name = "listExtras";
            this.listExtras.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listExtras.Size = new System.Drawing.Size(75, 199);
            this.listExtras.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(165, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Extras";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listFrom
            // 
            this.listFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listFrom.FormattingEnabled = true;
            this.listFrom.ItemHeight = 15;
            this.listFrom.Location = new System.Drawing.Point(87, 51);
            this.listFrom.Name = "listFrom";
            this.listFrom.Size = new System.Drawing.Size(75, 199);
            this.listFrom.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "From";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listTo
            // 
            this.listTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listTo.FormattingEnabled = true;
            this.listTo.ItemHeight = 15;
            this.listTo.Location = new System.Drawing.Point(6, 51);
            this.listTo.Name = "listTo";
            this.listTo.Size = new System.Drawing.Size(75, 199);
            this.listTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "To";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textMessage.Location = new System.Drawing.Point(6, 289);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(237, 23);
            this.textMessage.TabIndex = 9;
            // 
            // dailyActionsGroupBox
            // 
            this.dailyActionsGroupBox.Controls.Add(this.erasButton);
            this.dailyActionsGroupBox.Controls.Add(this.importInsPlansButton);
            this.dailyActionsGroupBox.Controls.Add(this.emailButton);
            this.dailyActionsGroupBox.Controls.Add(this.supplyButton);
            this.dailyActionsGroupBox.Controls.Add(this.claimPayButton);
            this.dailyActionsGroupBox.Controls.Add(this.billingButton);
            this.dailyActionsGroupBox.Controls.Add(this.accountingButton);
            this.dailyActionsGroupBox.Controls.Add(this.backupButton);
            this.dailyActionsGroupBox.Controls.Add(this.depositsButton);
            this.dailyActionsGroupBox.Controls.Add(this.sendClaimsButton);
            this.dailyActionsGroupBox.Controls.Add(this.tasksButton);
            this.dailyActionsGroupBox.Controls.Add(this.collectionsButton);
            this.dailyActionsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dailyActionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.dailyActionsGroupBox.Name = "dailyActionsGroupBox";
            this.dailyActionsGroupBox.Size = new System.Drawing.Size(280, 270);
            this.dailyActionsGroupBox.TabIndex = 0;
            this.dailyActionsGroupBox.TabStop = false;
            this.dailyActionsGroupBox.Text = "Daily";
            // 
            // erasButton
            // 
            this.erasButton.Location = new System.Drawing.Point(142, 166);
            this.erasButton.Name = "erasButton";
            this.erasButton.Size = new System.Drawing.Size(130, 30);
            this.erasButton.TabIndex = 9;
            this.erasButton.Text = "ERAs";
            this.erasButton.Click += new System.EventHandler(this.erasButton_Click);
            // 
            // importInsPlansButton
            // 
            this.importInsPlansButton.Location = new System.Drawing.Point(142, 202);
            this.importInsPlansButton.Name = "importInsPlansButton";
            this.importInsPlansButton.Size = new System.Drawing.Size(130, 30);
            this.importInsPlansButton.TabIndex = 11;
            this.importInsPlansButton.Text = "Import Ins Plans";
            this.importInsPlansButton.Click += new System.EventHandler(this.importInsPlansButton_Click);
            // 
            // emailButton
            // 
            this.emailButton.Location = new System.Drawing.Point(142, 130);
            this.emailButton.Name = "emailButton";
            this.emailButton.Size = new System.Drawing.Size(130, 30);
            this.emailButton.TabIndex = 7;
            this.emailButton.Text = "Emails";
            this.emailButton.Click += new System.EventHandler(this.emailButton_Click);
            // 
            // supplyButton
            // 
            this.supplyButton.Location = new System.Drawing.Point(6, 166);
            this.supplyButton.Name = "supplyButton";
            this.supplyButton.Size = new System.Drawing.Size(130, 30);
            this.supplyButton.TabIndex = 8;
            this.supplyButton.Text = "Supply Inventory";
            this.supplyButton.Click += new System.EventHandler(this.supplyButton_Click);
            // 
            // claimPayButton
            // 
            this.claimPayButton.Location = new System.Drawing.Point(6, 58);
            this.claimPayButton.Name = "claimPayButton";
            this.claimPayButton.Size = new System.Drawing.Size(130, 30);
            this.claimPayButton.TabIndex = 2;
            this.claimPayButton.Text = "Batch Ins";
            this.claimPayButton.Click += new System.EventHandler(this.claimPayButton_Click);
            // 
            // billingButton
            // 
            this.billingButton.Location = new System.Drawing.Point(6, 94);
            this.billingButton.Name = "billingButton";
            this.billingButton.Size = new System.Drawing.Size(130, 30);
            this.billingButton.TabIndex = 4;
            this.billingButton.Text = "Billing";
            this.billingButton.Click += new System.EventHandler(this.billingButton_Click);
            // 
            // accountingButton
            // 
            this.accountingButton.Location = new System.Drawing.Point(142, 94);
            this.accountingButton.Name = "accountingButton";
            this.accountingButton.Size = new System.Drawing.Size(130, 30);
            this.accountingButton.TabIndex = 5;
            this.accountingButton.Text = "Accounting";
            this.accountingButton.Click += new System.EventHandler(this.accountingButton_Click);
            // 
            // backupButton
            // 
            this.backupButton.Location = new System.Drawing.Point(142, 58);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(130, 30);
            this.backupButton.TabIndex = 3;
            this.backupButton.Text = "Backup";
            this.backupButton.Click += new System.EventHandler(this.backupButton_Click);
            // 
            // depositsButton
            // 
            this.depositsButton.Location = new System.Drawing.Point(6, 130);
            this.depositsButton.Name = "depositsButton";
            this.depositsButton.Size = new System.Drawing.Size(130, 30);
            this.depositsButton.TabIndex = 6;
            this.depositsButton.Text = "Deposits";
            this.depositsButton.Click += new System.EventHandler(this.depositsButton_Click);
            // 
            // sendClaimsButton
            // 
            this.sendClaimsButton.Location = new System.Drawing.Point(6, 22);
            this.sendClaimsButton.Name = "sendClaimsButton";
            this.sendClaimsButton.Size = new System.Drawing.Size(130, 30);
            this.sendClaimsButton.TabIndex = 0;
            this.sendClaimsButton.Text = "Send Claims";
            this.sendClaimsButton.Click += new System.EventHandler(this.sendClaimsButton_Click);
            // 
            // tasksButton
            // 
            this.tasksButton.Location = new System.Drawing.Point(142, 22);
            this.tasksButton.Name = "tasksButton";
            this.tasksButton.Size = new System.Drawing.Size(130, 30);
            this.tasksButton.TabIndex = 1;
            this.tasksButton.Text = "Tasks";
            this.tasksButton.Click += new System.EventHandler(this.tasksButton_Click);
            // 
            // collectionsButton
            // 
            this.collectionsButton.Location = new System.Drawing.Point(6, 202);
            this.collectionsButton.Name = "collectionsButton";
            this.collectionsButton.Size = new System.Drawing.Size(130, 30);
            this.collectionsButton.TabIndex = 10;
            this.collectionsButton.Text = "Collections";
            this.collectionsButton.Click += new System.EventHandler(this.collectionsButton_Click);
            // 
            // ContrStaff
            // 
            this.Controls.Add(this.dailyActionsGroupBox);
            this.Controls.Add(this.messagingGroupBox);
            this.Controls.Add(this.timeClockGroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ContrStaff";
            this.Size = new System.Drawing.Size(908, 600);
            this.timeClockGroupBox.ResumeLayout(false);
            this.messagingGroupBox.ResumeLayout(false);
            this.messagingGroupBox.PerformLayout();
            this.dailyActionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button butTimeCard;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.Label textTime;
        private System.Windows.Forms.Timer timerUpdateTime;
        private System.Windows.Forms.Button butClockIn;
        private System.Windows.Forms.Button butClockOut;
        private System.Windows.Forms.GroupBox timeClockGroupBox;
        private System.Windows.Forms.GroupBox messagingGroupBox;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Label labelCurrentTime;
        private System.Windows.Forms.Button sendClaimsButton;
        private System.Windows.Forms.Button tasksButton;
        private System.Windows.Forms.Button backupButton;
        private OpenDental.UI.ODGrid employeeGrid;
        private System.Windows.Forms.GroupBox dailyActionsGroupBox;
        private System.Windows.Forms.Button depositsButton;
        private System.Windows.Forms.Button butBreaks;
        private System.Windows.Forms.Button billingButton;
        private System.Windows.Forms.Button accountingButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.ListBox listMessages;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listExtras;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listTo;
        private System.Windows.Forms.Label label1;
        private OpenDental.UI.ODGrid gridMessages;
        private System.Windows.Forms.CheckBox checkIncludeAck;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Label daysLabel;
        private System.Windows.Forms.TextBox textDays;
        private System.Windows.Forms.Label sendingLabel;
        private System.Windows.Forms.Button butAck;
        private System.Windows.Forms.Button supplyButton;
        private System.Windows.Forms.Button claimPayButton;
        private System.Windows.Forms.Button butManage;
        private System.Windows.Forms.Button emailButton;
        private System.Windows.Forms.Button importInsPlansButton;
        private System.Windows.Forms.Button erasButton;
        private System.Windows.Forms.Button butViewSched;
        private System.Windows.Forms.Button collectionsButton;
    }
}
