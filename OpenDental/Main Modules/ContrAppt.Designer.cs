using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental
{
    partial class ContrAppt
    {
        private System.Windows.Forms.ImageList imageListMain;
        private System.Windows.Forms.MonthCalendar Calendar2;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelDate2;
        private System.Windows.Forms.Panel panelArrows;
        private System.Windows.Forms.Button butBackMonth;
        private System.Windows.Forms.Button butFwdMonth;
        private System.Windows.Forms.Button butBackWeek;
        private System.Windows.Forms.Button butFwdWeek;
        private System.Windows.Forms.Button butToday;
        private System.Windows.Forms.Button butBack;
        private System.Windows.Forms.Button butFwd;
        private System.Windows.Forms.Panel panelSheet;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private OpenDental.ContrApptSheet ContrApptSheet2;
        private System.Windows.Forms.Label labelNoneView;
        private System.Windows.Forms.Panel panelAptInfo;
        private System.Windows.Forms.ListBox listConfirmed;
        private System.Windows.Forms.Panel panelCalendar;
        private System.Windows.Forms.Panel panelOps;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button butComplete;
        private System.Windows.Forms.Button butUnsched;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butBreak;
        private System.Windows.Forms.Button butClearPin;
        private System.Windows.Forms.Label label2;
        private OpenDental.UI.ODToolBar ToolBarMain;
        private System.Windows.Forms.TextBox textLab;
        private System.Windows.Forms.TextBox textProduction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboView;
        private System.Windows.Forms.ContextMenu menuPatient;
        private System.Windows.Forms.Button butMakeAppt;
        private System.Windows.Forms.ContextMenu menuApt;
        private System.Windows.Forms.MenuItem menuItemBreakAppt;
        private System.Windows.Forms.ContextMenu menuBlockout;
        private System.Windows.Forms.Button butSearch;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Button butSearchNext;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBefore;
        private System.Windows.Forms.RadioButton radioBeforeAM;
        private System.Windows.Forms.RadioButton radioBeforePM;
        private System.Windows.Forms.RadioButton radioAfterPM;
        private System.Windows.Forms.RadioButton radioAfterAM;
        private System.Windows.Forms.TextBox textAfter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button butSearchClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butSearchCloseX;
        private System.Windows.Forms.ListBox _listBoxProviders;
        private System.Windows.Forms.ListBox listSearchResults;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateSearch;
        private System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.RadioButton radioDay;
        private System.Windows.Forms.RadioButton radioWeek;
        private System.Windows.Forms.Button butGraph;
        private System.Windows.Forms.Button butProvPick;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butProvHygenist;
        private System.Windows.Forms.Button butProvDentist;
        private System.Windows.Forms.Button butFamRecall;
        private System.Windows.Forms.Button butViewAppts;
        private System.Windows.Forms.Button butMakeRecall;
        private System.Windows.Forms.Button butLab;
        private System.Windows.Forms.Button butMonth;
        private OpenDental.UI.ODGrid gridEmpSched;
        private System.Windows.Forms.Timer timerInfoBubble;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabWaiting;
        private System.Windows.Forms.TabPage tabSched;
        private OpenDental.UI.ODGrid gridWaiting;
        private OpenDental.UI.PinBoard pinBoard;
        private System.Windows.Forms.Timer timerWaitingRoom;
        private System.Windows.Forms.Panel panelMakeButtons;
        private System.Windows.Forms.ContextMenuStrip _menuOp;
        private System.Windows.Forms.TabPage tabProv;
        private OpenDental.UI.ODGrid gridProv;
        private System.Windows.Forms.TabPage tabReminders;
        private OpenDental.UI.ODGrid gridReminders;
        private System.Windows.Forms.ImageList imageListTasks;
        private System.Windows.Forms.ContextMenu menuReminderEdit;
        private System.Windows.Forms.MenuItem menuItemReminderDone;
        private System.Windows.Forms.MenuItem menuItemReminderGoto;
        private System.Windows.Forms.TextBox textProdGoal;
        private System.Windows.Forms.Label labelProdGoal;
        private System.Windows.Forms.Button butAdvSearch;

        private System.ComponentModel.IContainer components;// Required designer variable.

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrAppt));
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.Calendar2 = new System.Windows.Forms.MonthCalendar();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelDate2 = new System.Windows.Forms.Label();
            this.panelArrows = new System.Windows.Forms.Panel();
            this.butBackMonth = new System.Windows.Forms.Button();
            this.butFwdMonth = new System.Windows.Forms.Button();
            this.butBackWeek = new System.Windows.Forms.Button();
            this.butFwdWeek = new System.Windows.Forms.Button();
            this.butToday = new System.Windows.Forms.Button();
            this.butBack = new System.Windows.Forms.Button();
            this.butFwd = new System.Windows.Forms.Button();
            this.panelSheet = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.ContrApptSheet2 = new OpenDental.ContrApptSheet();
            this.labelNoneView = new System.Windows.Forms.Label();
            this.panelAptInfo = new System.Windows.Forms.Panel();
            this.listConfirmed = new System.Windows.Forms.ListBox();
            this.butComplete = new System.Windows.Forms.Button();
            this.butUnsched = new System.Windows.Forms.Button();
            this.butDelete = new System.Windows.Forms.Button();
            this.butBreak = new System.Windows.Forms.Button();
            this.panelCalendar = new System.Windows.Forms.Panel();
            this.butAdvSearch = new System.Windows.Forms.Button();
            this.textProdGoal = new System.Windows.Forms.TextBox();
            this.labelProdGoal = new System.Windows.Forms.Label();
            this.radioWeek = new System.Windows.Forms.RadioButton();
            this.radioDay = new System.Windows.Forms.RadioButton();
            this.butGraph = new System.Windows.Forms.Button();
            this.butMonth = new System.Windows.Forms.Button();
            this.pinBoard = new OpenDental.UI.PinBoard();
            this.butLab = new System.Windows.Forms.Button();
            this.butSearch = new System.Windows.Forms.Button();
            this.textProduction = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textLab = new System.Windows.Forms.TextBox();
            this.comboView = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.butClearPin = new System.Windows.Forms.Button();
            this.panelOps = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuApt = new System.Windows.Forms.ContextMenu();
            this.menuPatient = new System.Windows.Forms.ContextMenu();
            this.menuBlockout = new System.Windows.Forms.ContextMenu();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butProvHygenist = new System.Windows.Forms.Button();
            this.butProvDentist = new System.Windows.Forms.Button();
            this.butProvPick = new System.Windows.Forms.Button();
            this.butRefresh = new System.Windows.Forms.Button();
            this.listSearchResults = new System.Windows.Forms.ListBox();
            this._listBoxProviders = new System.Windows.Forms.ListBox();
            this.butSearchClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textAfter = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.radioBeforePM = new System.Windows.Forms.RadioButton();
            this.radioBeforeAM = new System.Windows.Forms.RadioButton();
            this.textBefore = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioAfterAM = new System.Windows.Forms.RadioButton();
            this.radioAfterPM = new System.Windows.Forms.RadioButton();
            this.dateSearch = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.butSearchCloseX = new System.Windows.Forms.Button();
            this.butSearchNext = new System.Windows.Forms.Button();
            this.timerInfoBubble = new System.Windows.Forms.Timer(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabWaiting = new System.Windows.Forms.TabPage();
            this.gridWaiting = new OpenDental.UI.ODGrid();
            this.tabSched = new System.Windows.Forms.TabPage();
            this.gridEmpSched = new OpenDental.UI.ODGrid();
            this.tabProv = new System.Windows.Forms.TabPage();
            this.gridProv = new OpenDental.UI.ODGrid();
            this.tabReminders = new System.Windows.Forms.TabPage();
            this.gridReminders = new OpenDental.UI.ODGrid();
            this.timerWaitingRoom = new System.Windows.Forms.Timer(this.components);
            this.panelMakeButtons = new System.Windows.Forms.Panel();
            this.butMakeAppt = new System.Windows.Forms.Button();
            this.butFamRecall = new System.Windows.Forms.Button();
            this.butMakeRecall = new System.Windows.Forms.Button();
            this.butViewAppts = new System.Windows.Forms.Button();
            this.imageListTasks = new System.Windows.Forms.ImageList(this.components);
            this.ToolBarMain = new OpenDental.UI.ODToolBar();
            this.menuReminderEdit = new System.Windows.Forms.ContextMenu();
            this.menuItemReminderDone = new System.Windows.Forms.MenuItem();
            this.menuItemReminderGoto = new System.Windows.Forms.MenuItem();
            this._menuOp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelArrows.SuspendLayout();
            this.panelSheet.SuspendLayout();
            this.panelAptInfo.SuspendLayout();
            this.panelCalendar.SuspendLayout();
            this.groupSearch.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabWaiting.SuspendLayout();
            this.tabSched.SuspendLayout();
            this.tabProv.SuspendLayout();
            this.tabReminders.SuspendLayout();
            this.panelMakeButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "Pat.gif");
            this.imageListMain.Images.SetKeyName(1, "print.gif");
            this.imageListMain.Images.SetKeyName(2, "apptLists.gif");
            this.imageListMain.Images.SetKeyName(3, "DT Rapid Call.png");
            // 
            // Calendar2
            // 
            this.Calendar2.Location = new System.Drawing.Point(0, 24);
            this.Calendar2.Name = "Calendar2";
            this.Calendar2.ScrollChange = 1;
            this.Calendar2.TabIndex = 23;
            this.Calendar2.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.Calendar2_DateSelected);
            this.Calendar2.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.Calendar2_DateSelected);
            // 
            // labelDate
            // 
            this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDate.Location = new System.Drawing.Point(2, 4);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(56, 16);
            this.labelDate.TabIndex = 24;
            this.labelDate.Text = "Wed";
            // 
            // labelDate2
            // 
            this.labelDate2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDate2.Location = new System.Drawing.Point(46, 4);
            this.labelDate2.Name = "labelDate2";
            this.labelDate2.Size = new System.Drawing.Size(100, 20);
            this.labelDate2.TabIndex = 25;
            this.labelDate2.Text = "-  Oct 20";
            // 
            // panelArrows
            // 
            this.panelArrows.Controls.Add(this.butBackMonth);
            this.panelArrows.Controls.Add(this.butFwdMonth);
            this.panelArrows.Controls.Add(this.butBackWeek);
            this.panelArrows.Controls.Add(this.butFwdWeek);
            this.panelArrows.Controls.Add(this.butToday);
            this.panelArrows.Controls.Add(this.butBack);
            this.panelArrows.Controls.Add(this.butFwd);
            this.panelArrows.Location = new System.Drawing.Point(1, 189);
            this.panelArrows.Name = "panelArrows";
            this.panelArrows.Size = new System.Drawing.Size(217, 24);
            this.panelArrows.TabIndex = 32;
            // 
            // butBackMonth
            // 
            this.butBackMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butBackMonth.Location = new System.Drawing.Point(1, 0);
            this.butBackMonth.Name = "butBackMonth";
            this.butBackMonth.Size = new System.Drawing.Size(32, 22);
            this.butBackMonth.TabIndex = 57;
            this.butBackMonth.Text = "M-";
            this.butBackMonth.Click += new System.EventHandler(this.butBackMonth_Click);
            // 
            // butFwdMonth
            // 
            this.butFwdMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.butFwdMonth.Location = new System.Drawing.Point(188, 0);
            this.butFwdMonth.Name = "butFwdMonth";
            this.butFwdMonth.Size = new System.Drawing.Size(29, 22);
            this.butFwdMonth.TabIndex = 56;
            this.butFwdMonth.Text = "M+";
            this.butFwdMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butFwdMonth.Click += new System.EventHandler(this.butFwdMonth_Click);
            // 
            // butBackWeek
            // 
            this.butBackWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butBackWeek.Location = new System.Drawing.Point(33, 0);
            this.butBackWeek.Name = "butBackWeek";
            this.butBackWeek.Size = new System.Drawing.Size(33, 22);
            this.butBackWeek.TabIndex = 55;
            this.butBackWeek.Text = "W-";
            this.butBackWeek.Click += new System.EventHandler(this.butBackWeek_Click);
            // 
            // butFwdWeek
            // 
            this.butFwdWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.butFwdWeek.Location = new System.Drawing.Point(158, 0);
            this.butFwdWeek.Name = "butFwdWeek";
            this.butFwdWeek.Size = new System.Drawing.Size(30, 22);
            this.butFwdWeek.TabIndex = 54;
            this.butFwdWeek.Text = "W+";
            this.butFwdWeek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butFwdWeek.Click += new System.EventHandler(this.butFwdWeek_Click);
            // 
            // butToday
            // 
            this.butToday.Location = new System.Drawing.Point(85, 0);
            this.butToday.Name = "butToday";
            this.butToday.Size = new System.Drawing.Size(54, 22);
            this.butToday.TabIndex = 29;
            this.butToday.Text = "Today";
            this.butToday.Click += new System.EventHandler(this.butToday_Click);
            // 
            // butBack
            // 
            this.butBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butBack.Image = global::OpenDental.Properties.Resources.IconBulletArrowLeft;
            this.butBack.Location = new System.Drawing.Point(66, 0);
            this.butBack.Name = "butBack";
            this.butBack.Size = new System.Drawing.Size(19, 22);
            this.butBack.TabIndex = 51;
            this.butBack.Click += new System.EventHandler(this.butBack_Click);
            // 
            // butFwd
            // 
            this.butFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butFwd.Image = global::OpenDental.Properties.Resources.IconBulletArrowRight;
            this.butFwd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.butFwd.Location = new System.Drawing.Point(139, 0);
            this.butFwd.Name = "butFwd";
            this.butFwd.Size = new System.Drawing.Size(19, 22);
            this.butFwd.TabIndex = 53;
            this.butFwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butFwd.Click += new System.EventHandler(this.butFwd_Click);
            // 
            // panelSheet
            // 
            this.panelSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSheet.Controls.Add(this.vScrollBar1);
            this.panelSheet.Controls.Add(this.ContrApptSheet2);
            this.panelSheet.Controls.Add(this.labelNoneView);
            this.panelSheet.Location = new System.Drawing.Point(0, 17);
            this.panelSheet.Name = "panelSheet";
            this.panelSheet.Size = new System.Drawing.Size(235, 726);
            this.panelSheet.TabIndex = 44;
            this.panelSheet.Resize += new System.EventHandler(this.panelSheet_Resize);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(216, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 724);
            this.vScrollBar1.TabIndex = 23;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // ContrApptSheet2
            // 
            this.ContrApptSheet2.Location = new System.Drawing.Point(2, -550);
            this.ContrApptSheet2.Name = "ContrApptSheet2";
            this.ContrApptSheet2.Size = new System.Drawing.Size(60, 1728);
            this.ContrApptSheet2.TabIndex = 22;
            this.ContrApptSheet2.DoubleClick += new System.EventHandler(this.ContrApptSheet2_DoubleClick);
            this.ContrApptSheet2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseDown);
            this.ContrApptSheet2.MouseLeave += new System.EventHandler(this.ContrApptSheet2_MouseLeave);
            this.ContrApptSheet2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseMove);
            this.ContrApptSheet2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseUp);
            // 
            // labelNoneView
            // 
            this.labelNoneView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelNoneView.AutoSize = true;
            this.labelNoneView.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoneView.Location = new System.Drawing.Point(-57, 248);
            this.labelNoneView.Name = "labelNoneView";
            this.labelNoneView.Size = new System.Drawing.Size(324, 66);
            this.labelNoneView.TabIndex = 83;
            this.labelNoneView.Text = "Please select a clinic \r\nor an appointment view.";
            this.labelNoneView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelAptInfo
            // 
            this.panelAptInfo.Controls.Add(this.listConfirmed);
            this.panelAptInfo.Controls.Add(this.butComplete);
            this.panelAptInfo.Controls.Add(this.butUnsched);
            this.panelAptInfo.Controls.Add(this.butDelete);
            this.panelAptInfo.Controls.Add(this.butBreak);
            this.panelAptInfo.Location = new System.Drawing.Point(665, 423);
            this.panelAptInfo.Name = "panelAptInfo";
            this.panelAptInfo.Size = new System.Drawing.Size(107, 116);
            this.panelAptInfo.TabIndex = 45;
            // 
            // listConfirmed
            // 
            this.listConfirmed.IntegralHeight = false;
            this.listConfirmed.Location = new System.Drawing.Point(31, 2);
            this.listConfirmed.Name = "listConfirmed";
            this.listConfirmed.Size = new System.Drawing.Size(73, 111);
            this.listConfirmed.TabIndex = 75;
            this.listConfirmed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listConfirmed_MouseDown);
            // 
            // butComplete
            // 
            this.butComplete.BackColor = System.Drawing.SystemColors.Control;
            this.butComplete.Image = ((System.Drawing.Image)(resources.GetObject("butComplete.Image")));
            this.butComplete.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.butComplete.Location = new System.Drawing.Point(2, 57);
            this.butComplete.Name = "butComplete";
            this.butComplete.Size = new System.Drawing.Size(28, 28);
            this.butComplete.TabIndex = 69;
            this.butComplete.TabStop = false;
            this.butComplete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butComplete.UseVisualStyleBackColor = false;
            this.butComplete.Click += new System.EventHandler(this.butComplete_Click);
            // 
            // butUnsched
            // 
            this.butUnsched.BackColor = System.Drawing.SystemColors.Control;
            this.butUnsched.Image = ((System.Drawing.Image)(resources.GetObject("butUnsched.Image")));
            this.butUnsched.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.butUnsched.Location = new System.Drawing.Point(2, 1);
            this.butUnsched.Name = "butUnsched";
            this.butUnsched.Size = new System.Drawing.Size(28, 28);
            this.butUnsched.TabIndex = 68;
            this.butUnsched.TabStop = false;
            this.butUnsched.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butUnsched.UseVisualStyleBackColor = false;
            this.butUnsched.Click += new System.EventHandler(this.butUnsched_Click);
            // 
            // butDelete
            // 
            this.butDelete.BackColor = System.Drawing.SystemColors.Control;
            this.butDelete.Image = ((System.Drawing.Image)(resources.GetObject("butDelete.Image")));
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.butDelete.Location = new System.Drawing.Point(2, 85);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(28, 28);
            this.butDelete.TabIndex = 66;
            this.butDelete.TabStop = false;
            this.butDelete.UseVisualStyleBackColor = false;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butBreak
            // 
            this.butBreak.BackColor = System.Drawing.SystemColors.Control;
            this.butBreak.Image = ((System.Drawing.Image)(resources.GetObject("butBreak.Image")));
            this.butBreak.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.butBreak.Location = new System.Drawing.Point(2, 29);
            this.butBreak.Name = "butBreak";
            this.butBreak.Size = new System.Drawing.Size(28, 28);
            this.butBreak.TabIndex = 65;
            this.butBreak.TabStop = false;
            this.butBreak.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butBreak.UseVisualStyleBackColor = false;
            this.butBreak.Click += new System.EventHandler(this.butBreak_Click);
            // 
            // panelCalendar
            // 
            this.panelCalendar.Controls.Add(this.butAdvSearch);
            this.panelCalendar.Controls.Add(this.textProdGoal);
            this.panelCalendar.Controls.Add(this.labelProdGoal);
            this.panelCalendar.Controls.Add(this.radioWeek);
            this.panelCalendar.Controls.Add(this.panelArrows);
            this.panelCalendar.Controls.Add(this.radioDay);
            this.panelCalendar.Controls.Add(this.butGraph);
            this.panelCalendar.Controls.Add(this.butMonth);
            this.panelCalendar.Controls.Add(this.pinBoard);
            this.panelCalendar.Controls.Add(this.butLab);
            this.panelCalendar.Controls.Add(this.butSearch);
            this.panelCalendar.Controls.Add(this.textProduction);
            this.panelCalendar.Controls.Add(this.label7);
            this.panelCalendar.Controls.Add(this.textLab);
            this.panelCalendar.Controls.Add(this.comboView);
            this.panelCalendar.Controls.Add(this.label2);
            this.panelCalendar.Controls.Add(this.butClearPin);
            this.panelCalendar.Controls.Add(this.Calendar2);
            this.panelCalendar.Controls.Add(this.labelDate);
            this.panelCalendar.Controls.Add(this.labelDate2);
            this.panelCalendar.Location = new System.Drawing.Point(665, 28);
            this.panelCalendar.Name = "panelCalendar";
            this.panelCalendar.Size = new System.Drawing.Size(219, 394);
            this.panelCalendar.TabIndex = 46;
            // 
            // butAdvSearch
            // 
            this.butAdvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butAdvSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butAdvSearch.Location = new System.Drawing.Point(3, 285);
            this.butAdvSearch.Name = "butAdvSearch";
            this.butAdvSearch.Size = new System.Drawing.Size(63, 24);
            this.butAdvSearch.TabIndex = 90;
            this.butAdvSearch.Text = "Advanced";
            this.butAdvSearch.Click += new System.EventHandler(this.butAdvSearch_Click);
            // 
            // textProdGoal
            // 
            this.textProdGoal.BackColor = System.Drawing.Color.White;
            this.textProdGoal.Location = new System.Drawing.Point(85, 371);
            this.textProdGoal.Name = "textProdGoal";
            this.textProdGoal.ReadOnly = true;
            this.textProdGoal.Size = new System.Drawing.Size(133, 20);
            this.textProdGoal.TabIndex = 93;
            this.textProdGoal.Text = "$100";
            this.textProdGoal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelProdGoal
            // 
            this.labelProdGoal.Location = new System.Drawing.Point(5, 375);
            this.labelProdGoal.Name = "labelProdGoal";
            this.labelProdGoal.Size = new System.Drawing.Size(79, 15);
            this.labelProdGoal.TabIndex = 94;
            this.labelProdGoal.Text = "Daily Goal";
            this.labelProdGoal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioWeek
            // 
            this.radioWeek.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioWeek.Location = new System.Drawing.Point(43, 238);
            this.radioWeek.Name = "radioWeek";
            this.radioWeek.Size = new System.Drawing.Size(68, 16);
            this.radioWeek.TabIndex = 92;
            this.radioWeek.Text = "Week";
            this.radioWeek.Click += new System.EventHandler(this.radioWeek_Click);
            // 
            // radioDay
            // 
            this.radioDay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioDay.Location = new System.Drawing.Point(43, 218);
            this.radioDay.Name = "radioDay";
            this.radioDay.Size = new System.Drawing.Size(68, 16);
            this.radioDay.TabIndex = 91;
            this.radioDay.Text = "Day";
            this.radioDay.Click += new System.EventHandler(this.radioDay_Click);
            // 
            // butGraph
            // 
            this.butGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butGraph.Location = new System.Drawing.Point(3, 309);
            this.butGraph.Name = "butGraph";
            this.butGraph.Size = new System.Drawing.Size(42, 24);
            this.butGraph.TabIndex = 78;
            this.butGraph.TabStop = false;
            this.butGraph.Text = "Emp";
            this.butGraph.Visible = false;
            this.butGraph.Click += new System.EventHandler(this.butGraph_Click);
            // 
            // butMonth
            // 
            this.butMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butMonth.Location = new System.Drawing.Point(152, 1);
            this.butMonth.Name = "butMonth";
            this.butMonth.Size = new System.Drawing.Size(65, 22);
            this.butMonth.TabIndex = 79;
            this.butMonth.Text = "Month";
            this.butMonth.Visible = false;
            this.butMonth.Click += new System.EventHandler(this.butMonth_Click);
            // 
            // pinBoard
            // 
            this.pinBoard.Location = new System.Drawing.Point(119, 213);
            this.pinBoard.Name = "pinBoard";
            this.pinBoard.SelectedIndex = -1;
            this.pinBoard.Size = new System.Drawing.Size(99, 96);
            this.pinBoard.TabIndex = 78;
            this.pinBoard.Text = "pinBoard";
            this.pinBoard.SelectedIndexChanged += new System.EventHandler(this.pinBoard_SelectedIndexChanged);
            this.pinBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseDown);
            this.pinBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseMove);
            this.pinBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseUp);
            // 
            // butLab
            // 
            this.butLab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butLab.Location = new System.Drawing.Point(3, 333);
            this.butLab.Name = "butLab";
            this.butLab.Size = new System.Drawing.Size(79, 21);
            this.butLab.TabIndex = 77;
            this.butLab.Text = "Lab Cases";
            this.butLab.Click += new System.EventHandler(this.butLab_Click);
            // 
            // butSearch
            // 
            this.butSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butSearch.Location = new System.Drawing.Point(67, 285);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(51, 24);
            this.butSearch.TabIndex = 40;
            this.butSearch.Text = "Search";
            this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
            // 
            // textProduction
            // 
            this.textProduction.BackColor = System.Drawing.Color.White;
            this.textProduction.Location = new System.Drawing.Point(85, 353);
            this.textProduction.Name = "textProduction";
            this.textProduction.ReadOnly = true;
            this.textProduction.Size = new System.Drawing.Size(133, 20);
            this.textProduction.TabIndex = 38;
            this.textProduction.Text = "$100";
            this.textProduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 357);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 39;
            this.label7.Text = "Daily Prod";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textLab
            // 
            this.textLab.BackColor = System.Drawing.Color.White;
            this.textLab.Location = new System.Drawing.Point(85, 333);
            this.textLab.Name = "textLab";
            this.textLab.ReadOnly = true;
            this.textLab.Size = new System.Drawing.Size(133, 20);
            this.textLab.TabIndex = 36;
            this.textLab.Text = "All Received";
            this.textLab.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboView
            // 
            this.comboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboView.Location = new System.Drawing.Point(85, 312);
            this.comboView.MaxDropDownItems = 30;
            this.comboView.Name = "comboView";
            this.comboView.Size = new System.Drawing.Size(133, 21);
            this.comboView.TabIndex = 35;
            this.comboView.SelectionChangeCommitted += new System.EventHandler(this.comboView_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "View";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butClearPin
            // 
            this.butClearPin.Image = global::OpenDental.Properties.Resources.IconEraser;
            this.butClearPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butClearPin.Location = new System.Drawing.Point(43, 260);
            this.butClearPin.Name = "butClearPin";
            this.butClearPin.Size = new System.Drawing.Size(75, 24);
            this.butClearPin.TabIndex = 33;
            this.butClearPin.Text = "Clear";
            this.butClearPin.Click += new System.EventHandler(this.butClearPin_Click);
            // 
            // panelOps
            // 
            this.panelOps.Location = new System.Drawing.Point(0, 0);
            this.panelOps.Name = "panelOps";
            this.panelOps.Size = new System.Drawing.Size(676, 17);
            this.panelOps.TabIndex = 48;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            // 
            // menuApt
            // 
            this.menuApt.Popup += new System.EventHandler(this.menuApt_Popup);
            // 
            // groupSearch
            // 
            this.groupSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupSearch.Controls.Add(this.groupBox1);
            this.groupSearch.Controls.Add(this.butProvPick);
            this.groupSearch.Controls.Add(this.butRefresh);
            this.groupSearch.Controls.Add(this.listSearchResults);
            this.groupSearch.Controls.Add(this._listBoxProviders);
            this.groupSearch.Controls.Add(this.butSearchClose);
            this.groupSearch.Controls.Add(this.groupBox2);
            this.groupSearch.Controls.Add(this.label8);
            this.groupSearch.Controls.Add(this.butSearchCloseX);
            this.groupSearch.Controls.Add(this.butSearchNext);
            this.groupSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupSearch.Location = new System.Drawing.Point(380, 340);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Size = new System.Drawing.Size(219, 366);
            this.groupSearch.TabIndex = 74;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "Openings in View";
            this.groupSearch.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butProvHygenist);
            this.groupBox1.Controls.Add(this.butProvDentist);
            this.groupBox1.Location = new System.Drawing.Point(130, 253);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(85, 63);
            this.groupBox1.TabIndex = 89;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search by";
            // 
            // butProvHygenist
            // 
            this.butProvHygenist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butProvHygenist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butProvHygenist.Location = new System.Drawing.Point(6, 37);
            this.butProvHygenist.Name = "butProvHygenist";
            this.butProvHygenist.Size = new System.Drawing.Size(73, 22);
            this.butProvHygenist.TabIndex = 92;
            this.butProvHygenist.Text = "Hygienists";
            this.butProvHygenist.UseVisualStyleBackColor = true;
            this.butProvHygenist.Click += new System.EventHandler(this.butProvHygenist_Click);
            // 
            // butProvDentist
            // 
            this.butProvDentist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butProvDentist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butProvDentist.Location = new System.Drawing.Point(6, 14);
            this.butProvDentist.Name = "butProvDentist";
            this.butProvDentist.Size = new System.Drawing.Size(73, 22);
            this.butProvDentist.TabIndex = 91;
            this.butProvDentist.Text = "Providers";
            this.butProvDentist.UseVisualStyleBackColor = true;
            this.butProvDentist.Click += new System.EventHandler(this.butProvDentist_Click);
            // 
            // butProvPick
            // 
            this.butProvPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butProvPick.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butProvPick.Location = new System.Drawing.Point(6, 340);
            this.butProvPick.Name = "butProvPick";
            this.butProvPick.Size = new System.Drawing.Size(82, 22);
            this.butProvPick.TabIndex = 88;
            this.butProvPick.Text = "Providers...";
            this.butProvPick.UseVisualStyleBackColor = true;
            this.butProvPick.Click += new System.EventHandler(this.butProvPick_Click);
            // 
            // butRefresh
            // 
            this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butRefresh.Location = new System.Drawing.Point(153, 318);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(62, 22);
            this.butRefresh.TabIndex = 88;
            this.butRefresh.Text = "Search";
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
            // 
            // listSearchResults
            // 
            this.listSearchResults.IntegralHeight = false;
            this.listSearchResults.Location = new System.Drawing.Point(6, 32);
            this.listSearchResults.Name = "listSearchResults";
            this.listSearchResults.Size = new System.Drawing.Size(193, 134);
            this.listSearchResults.TabIndex = 87;
            this.listSearchResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSearchResults_MouseDown);
            // 
            // _listBoxProviders
            // 
            this._listBoxProviders.Location = new System.Drawing.Point(6, 269);
            this._listBoxProviders.Name = "_listBoxProviders";
            this._listBoxProviders.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this._listBoxProviders.Size = new System.Drawing.Size(118, 69);
            this._listBoxProviders.TabIndex = 86;
            // 
            // butSearchClose
            // 
            this.butSearchClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butSearchClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butSearchClose.Location = new System.Drawing.Point(153, 342);
            this.butSearchClose.Name = "butSearchClose";
            this.butSearchClose.Size = new System.Drawing.Size(62, 22);
            this.butSearchClose.TabIndex = 85;
            this.butSearchClose.Text = "Close";
            this.butSearchClose.UseVisualStyleBackColor = true;
            this.butSearchClose.Click += new System.EventHandler(this.butSearchClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textAfter);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.radioBeforePM);
            this.groupBox2.Controls.Add(this.radioBeforeAM);
            this.groupBox2.Controls.Add(this.textBefore);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.dateSearch);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(6, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 84);
            this.groupBox2.TabIndex = 84;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date/Time Restrictions";
            // 
            // textAfter
            // 
            this.textAfter.Location = new System.Drawing.Point(57, 60);
            this.textAfter.Name = "textAfter";
            this.textAfter.Size = new System.Drawing.Size(44, 20);
            this.textAfter.TabIndex = 88;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(1, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 16);
            this.label11.TabIndex = 87;
            this.label11.Text = "After";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioBeforePM
            // 
            this.radioBeforePM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioBeforePM.Location = new System.Drawing.Point(151, 41);
            this.radioBeforePM.Name = "radioBeforePM";
            this.radioBeforePM.Size = new System.Drawing.Size(37, 15);
            this.radioBeforePM.TabIndex = 86;
            this.radioBeforePM.Text = "pm";
            // 
            // radioBeforeAM
            // 
            this.radioBeforeAM.Checked = true;
            this.radioBeforeAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioBeforeAM.Location = new System.Drawing.Point(108, 41);
            this.radioBeforeAM.Name = "radioBeforeAM";
            this.radioBeforeAM.Size = new System.Drawing.Size(37, 15);
            this.radioBeforeAM.TabIndex = 85;
            this.radioBeforeAM.TabStop = true;
            this.radioBeforeAM.Text = "am";
            // 
            // textBefore
            // 
            this.textBefore.Location = new System.Drawing.Point(57, 38);
            this.textBefore.Name = "textBefore";
            this.textBefore.Size = new System.Drawing.Size(44, 20);
            this.textBefore.TabIndex = 84;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(1, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 16);
            this.label10.TabIndex = 83;
            this.label10.Text = "Before";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioAfterAM);
            this.panel1.Controls.Add(this.radioAfterPM);
            this.panel1.Location = new System.Drawing.Point(105, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(84, 20);
            this.panel1.TabIndex = 86;
            // 
            // radioAfterAM
            // 
            this.radioAfterAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioAfterAM.Location = new System.Drawing.Point(3, 2);
            this.radioAfterAM.Name = "radioAfterAM";
            this.radioAfterAM.Size = new System.Drawing.Size(37, 15);
            this.radioAfterAM.TabIndex = 89;
            this.radioAfterAM.Text = "am";
            // 
            // radioAfterPM
            // 
            this.radioAfterPM.Checked = true;
            this.radioAfterPM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioAfterPM.Location = new System.Drawing.Point(46, 2);
            this.radioAfterPM.Name = "radioAfterPM";
            this.radioAfterPM.Size = new System.Drawing.Size(36, 15);
            this.radioAfterPM.TabIndex = 90;
            this.radioAfterPM.TabStop = true;
            this.radioAfterPM.Text = "pm";
            // 
            // dateSearch
            // 
            this.dateSearch.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateSearch.Location = new System.Drawing.Point(57, 16);
            this.dateSearch.Name = "dateSearch";
            this.dateSearch.Size = new System.Drawing.Size(130, 20);
            this.dateSearch.TabIndex = 90;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(1, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 16);
            this.label9.TabIndex = 89;
            this.label9.Text = "After";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 16);
            this.label8.TabIndex = 80;
            this.label8.Text = "Providers";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // butSearchCloseX
            // 
            this.butSearchCloseX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butSearchCloseX.ForeColor = System.Drawing.SystemColors.Control;
            this.butSearchCloseX.Image = ((System.Drawing.Image)(resources.GetObject("butSearchCloseX.Image")));
            this.butSearchCloseX.Location = new System.Drawing.Point(185, 7);
            this.butSearchCloseX.Name = "butSearchCloseX";
            this.butSearchCloseX.Size = new System.Drawing.Size(16, 16);
            this.butSearchCloseX.TabIndex = 0;
            this.butSearchCloseX.Click += new System.EventHandler(this.butSearchCloseX_Click);
            // 
            // butSearchNext
            // 
            this.butSearchNext.Image = global::OpenDental.Properties.Resources.IconBulletArrowRight;
            this.butSearchNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.butSearchNext.Location = new System.Drawing.Point(111, 9);
            this.butSearchNext.Name = "butSearchNext";
            this.butSearchNext.Size = new System.Drawing.Size(71, 22);
            this.butSearchNext.TabIndex = 77;
            this.butSearchNext.Text = "More";
            this.butSearchNext.UseVisualStyleBackColor = true;
            this.butSearchNext.Click += new System.EventHandler(this.butSearchMore_Click);
            // 
            // timerInfoBubble
            // 
            this.timerInfoBubble.Interval = 300;
            this.timerInfoBubble.Tick += new System.EventHandler(this.timerInfoBubble_Tick);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl.Controls.Add(this.tabWaiting);
            this.tabControl.Controls.Add(this.tabSched);
            this.tabControl.Controls.Add(this.tabProv);
            this.tabControl.Controls.Add(this.tabReminders);
            this.tabControl.Location = new System.Drawing.Point(665, 542);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(219, 166);
            this.tabControl.TabIndex = 78;
            // 
            // tabWaiting
            // 
            this.tabWaiting.Controls.Add(this.gridWaiting);
            this.tabWaiting.Location = new System.Drawing.Point(4, 22);
            this.tabWaiting.Name = "tabWaiting";
            this.tabWaiting.Padding = new System.Windows.Forms.Padding(3);
            this.tabWaiting.Size = new System.Drawing.Size(211, 140);
            this.tabWaiting.TabIndex = 0;
            this.tabWaiting.Text = "Waiting";
            // 
            // gridWaiting
            // 
            this.gridWaiting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridWaiting.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridWaiting.EditableEnterMovesDown = false;
            this.gridWaiting.HasAddButton = false;
            this.gridWaiting.HasDropDowns = false;
            this.gridWaiting.HasMultilineHeaders = false;
            this.gridWaiting.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridWaiting.HeaderHeight = 15;
            this.gridWaiting.HScrollVisible = false;
            this.gridWaiting.Location = new System.Drawing.Point(0, 0);
            this.gridWaiting.Margin = new System.Windows.Forms.Padding(0);
            this.gridWaiting.Name = "gridWaiting";
            this.gridWaiting.ScrollValue = 0;
            this.gridWaiting.Size = new System.Drawing.Size(211, 140);
            this.gridWaiting.TabIndex = 78;
            this.gridWaiting.Title = "Waiting Room";
            // 
            // tabSched
            // 
            this.tabSched.Controls.Add(this.gridEmpSched);
            this.tabSched.Location = new System.Drawing.Point(4, 22);
            this.tabSched.Name = "tabSched";
            this.tabSched.Padding = new System.Windows.Forms.Padding(3);
            this.tabSched.Size = new System.Drawing.Size(211, 140);
            this.tabSched.TabIndex = 1;
            this.tabSched.Text = "Emp";
            // 
            // gridEmpSched
            // 
            this.gridEmpSched.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridEmpSched.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridEmpSched.EditableEnterMovesDown = false;
            this.gridEmpSched.HasAddButton = false;
            this.gridEmpSched.HasDropDowns = false;
            this.gridEmpSched.HasMultilineHeaders = false;
            this.gridEmpSched.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridEmpSched.HeaderHeight = 15;
            this.gridEmpSched.HScrollVisible = true;
            this.gridEmpSched.Location = new System.Drawing.Point(0, 0);
            this.gridEmpSched.Margin = new System.Windows.Forms.Padding(0);
            this.gridEmpSched.Name = "gridEmpSched";
            this.gridEmpSched.ScrollValue = 0;
            this.gridEmpSched.Size = new System.Drawing.Size(211, 140);
            this.gridEmpSched.TabIndex = 77;
            this.gridEmpSched.Title = "Employee Schedules";
            this.gridEmpSched.DoubleClick += new System.EventHandler(this.gridEmpSched_DoubleClick);
            // 
            // tabProv
            // 
            this.tabProv.Controls.Add(this.gridProv);
            this.tabProv.Location = new System.Drawing.Point(4, 22);
            this.tabProv.Name = "tabProv";
            this.tabProv.Padding = new System.Windows.Forms.Padding(3);
            this.tabProv.Size = new System.Drawing.Size(211, 140);
            this.tabProv.TabIndex = 2;
            this.tabProv.Text = "Prov";
            // 
            // gridProv
            // 
            this.gridProv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridProv.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridProv.EditableEnterMovesDown = false;
            this.gridProv.HasAddButton = false;
            this.gridProv.HasDropDowns = false;
            this.gridProv.HasMultilineHeaders = false;
            this.gridProv.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridProv.HeaderHeight = 15;
            this.gridProv.HScrollVisible = true;
            this.gridProv.Location = new System.Drawing.Point(0, 0);
            this.gridProv.Margin = new System.Windows.Forms.Padding(0);
            this.gridProv.Name = "gridProv";
            this.gridProv.ScrollValue = 0;
            this.gridProv.Size = new System.Drawing.Size(211, 140);
            this.gridProv.TabIndex = 79;
            this.gridProv.Title = "Provider Schedules";
            this.gridProv.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridProv_CellDoubleClick);
            // 
            // tabReminders
            // 
            this.tabReminders.Controls.Add(this.gridReminders);
            this.tabReminders.Location = new System.Drawing.Point(4, 22);
            this.tabReminders.Name = "tabReminders";
            this.tabReminders.Padding = new System.Windows.Forms.Padding(3);
            this.tabReminders.Size = new System.Drawing.Size(211, 140);
            this.tabReminders.TabIndex = 3;
            this.tabReminders.Text = "Reminders";
            // 
            // gridReminders
            // 
            this.gridReminders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridReminders.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridReminders.EditableEnterMovesDown = false;
            this.gridReminders.HasAddButton = false;
            this.gridReminders.HasDropDowns = false;
            this.gridReminders.HasMultilineHeaders = false;
            this.gridReminders.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridReminders.HeaderHeight = 15;
            this.gridReminders.HScrollVisible = false;
            this.gridReminders.Location = new System.Drawing.Point(0, 0);
            this.gridReminders.Name = "gridReminders";
            this.gridReminders.ScrollValue = 0;
            this.gridReminders.Size = new System.Drawing.Size(211, 140);
            this.gridReminders.TabIndex = 0;
            this.gridReminders.Title = "Reminders";
            this.gridReminders.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridReminders_CellDoubleClick);
            this.gridReminders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridReminders_MouseDown);
            // 
            // timerWaitingRoom
            // 
            this.timerWaitingRoom.Enabled = true;
            this.timerWaitingRoom.Interval = 1000;
            this.timerWaitingRoom.Tick += new System.EventHandler(this.timerWaitingRoom_Tick);
            // 
            // panelMakeButtons
            // 
            this.panelMakeButtons.Controls.Add(this.butMakeAppt);
            this.panelMakeButtons.Controls.Add(this.butFamRecall);
            this.panelMakeButtons.Controls.Add(this.butMakeRecall);
            this.panelMakeButtons.Controls.Add(this.butViewAppts);
            this.panelMakeButtons.Location = new System.Drawing.Point(772, 423);
            this.panelMakeButtons.Name = "panelMakeButtons";
            this.panelMakeButtons.Size = new System.Drawing.Size(112, 116);
            this.panelMakeButtons.TabIndex = 82;
            // 
            // butMakeAppt
            // 
            this.butMakeAppt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butMakeAppt.Location = new System.Drawing.Point(5, 5);
            this.butMakeAppt.Name = "butMakeAppt";
            this.butMakeAppt.Size = new System.Drawing.Size(103, 24);
            this.butMakeAppt.TabIndex = 76;
            this.butMakeAppt.TabStop = false;
            this.butMakeAppt.Text = "Make Appt";
            this.butMakeAppt.Click += new System.EventHandler(this.butMakeAppt_Click);
            // 
            // butFamRecall
            // 
            this.butFamRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butFamRecall.Location = new System.Drawing.Point(5, 57);
            this.butFamRecall.Name = "butFamRecall";
            this.butFamRecall.Size = new System.Drawing.Size(103, 24);
            this.butFamRecall.TabIndex = 81;
            this.butFamRecall.TabStop = false;
            this.butFamRecall.Text = "Fam Recall";
            this.butFamRecall.Click += new System.EventHandler(this.butFamRecall_Click);
            // 
            // butMakeRecall
            // 
            this.butMakeRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butMakeRecall.Location = new System.Drawing.Point(5, 31);
            this.butMakeRecall.Name = "butMakeRecall";
            this.butMakeRecall.Size = new System.Drawing.Size(103, 24);
            this.butMakeRecall.TabIndex = 79;
            this.butMakeRecall.TabStop = false;
            this.butMakeRecall.Text = "Make Recall";
            this.butMakeRecall.Click += new System.EventHandler(this.butMakeRecall_Click);
            // 
            // butViewAppts
            // 
            this.butViewAppts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butViewAppts.Location = new System.Drawing.Point(5, 83);
            this.butViewAppts.Name = "butViewAppts";
            this.butViewAppts.Size = new System.Drawing.Size(103, 24);
            this.butViewAppts.TabIndex = 80;
            this.butViewAppts.TabStop = false;
            this.butViewAppts.Text = "View Pat Appts";
            this.butViewAppts.Click += new System.EventHandler(this.butViewAppts_Click);
            // 
            // imageListTasks
            // 
            this.imageListTasks.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTasks.ImageStream")));
            this.imageListTasks.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTasks.Images.SetKeyName(0, "TaskList.gif");
            this.imageListTasks.Images.SetKeyName(1, "checkBoxChecked.gif");
            this.imageListTasks.Images.SetKeyName(2, "checkBoxUnchecked.gif");
            this.imageListTasks.Images.SetKeyName(3, "TaskListHighlight.gif");
            this.imageListTasks.Images.SetKeyName(4, "checkBoxNew.gif");
            // 
            // ToolBarMain
            // 
            this.ToolBarMain.Location = new System.Drawing.Point(680, 2);
            this.ToolBarMain.Name = "ToolBarMain";
            this.ToolBarMain.Size = new System.Drawing.Size(203, 27);
            this.ToolBarMain.TabIndex = 73;
            this.ToolBarMain.ButtonClick += new System.EventHandler<OpenDental.UI.ODToolBarButtonClickEventArgs>(this.ToolBarMain_ButtonClick);
            // 
            // menuReminderEdit
            // 
            this.menuReminderEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemReminderDone,
            this.menuItemReminderGoto});
            this.menuReminderEdit.Popup += new System.EventHandler(this.menuReminderEdit_Popup);
            // 
            // menuItemReminderDone
            // 
            this.menuItemReminderDone.Index = 0;
            this.menuItemReminderDone.Text = "Done (affects all users)";
            this.menuItemReminderDone.Click += new System.EventHandler(this.menuItemReminderDone_Click);
            // 
            // menuItemReminderGoto
            // 
            this.menuItemReminderGoto.Index = 1;
            this.menuItemReminderGoto.Text = "Go To";
            this.menuItemReminderGoto.Click += new System.EventHandler(this.menuItemReminderGoto_Click);
            // 
            // _menuOp
            // 
            this._menuOp.Name = "_menuRightClick";
            this._menuOp.Size = new System.Drawing.Size(61, 4);
            // 
            // ContrAppt
            // 
            this.Controls.Add(this.groupSearch);
            this.Controls.Add(this.panelMakeButtons);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.ToolBarMain);
            this.Controls.Add(this.panelOps);
            this.Controls.Add(this.panelCalendar);
            this.Controls.Add(this.panelAptInfo);
            this.Controls.Add(this.panelSheet);
            this.Name = "ContrAppt";
            this.Size = new System.Drawing.Size(939, 708);
            this.Load += new System.EventHandler(this.ContrAppt_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrAppt_Layout);
            this.Resize += new System.EventHandler(this.ContrAppt_Resize);
            this.panelArrows.ResumeLayout(false);
            this.panelSheet.ResumeLayout(false);
            this.panelSheet.PerformLayout();
            this.panelAptInfo.ResumeLayout(false);
            this.panelCalendar.ResumeLayout(false);
            this.panelCalendar.PerformLayout();
            this.groupSearch.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabWaiting.ResumeLayout(false);
            this.tabSched.ResumeLayout(false);
            this.tabProv.ResumeLayout(false);
            this.tabReminders.ResumeLayout(false);
            this.panelMakeButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

    }
}
