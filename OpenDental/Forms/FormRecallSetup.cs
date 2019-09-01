using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace OpenDental{
///<summary></summary>
	public class FormRecallSetup : ODForm {
		//Designer Variables
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPostcardsPerSheet;
		private System.Windows.Forms.CheckBox checkReturnAdd;
		private GroupBox groupBox2;
		private ValidDouble textDown;
		private Label label12;
		private ValidDouble textRight;
		private Label label13;
		private CheckBox checkGroupFamilies;
		private Label label14;
		private Label label15;
		private GroupBox groupBox3;
		private Label label25;
		private ComboBox comboStatusMailedRecall;
		private ComboBox comboStatusEmailedRecall;
		private Label label26;
		private ListBox listTypes;
		private Label label1;
		private ValidNumber textDaysPast;
		private ValidNumber textDaysFuture;
		private GroupBox groupBox1;
		private ValidNum textDaysSecondReminder;
		private ValidNum textDaysFirstReminder;
		private Label label2;
		private Label label3;
		private OpenDental.UI.ODGrid gridMain;
		private System.ComponentModel.Container components = null;
		private ValidNumber textMaxReminders;//""= infinite, 0=disabled; Must Be ValidNumber not ValidNum.
		private Label label4;
		private GroupBox groupBox4;
		private RadioButton radioUseEmailFalse;
		private RadioButton radioUseEmailTrue;
		private RadioButton radioExcludeFutureNo;
		private RadioButton radioExcludeFutureYes;
		private ComboBox comboStatusEmailTextRecall;
		private Label label5;
		private ComboBox comboStatusTextedRecall;
		private Label label6;
		private List<RecallType> listRecallCache;

		///<summary></summary>
		public FormRecallSetup(){
			InitializeComponent();
			
			//Lan.C(this, new System.Windows.Forms.Control[] {
				//textBox1,
				//textBox6
			//});
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecallSetup));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.radioExcludeFutureYes = new System.Windows.Forms.RadioButton();
			this.label8 = new System.Windows.Forms.Label();
			this.radioExcludeFutureNo = new System.Windows.Forms.RadioButton();
			this.textPostcardsPerSheet = new System.Windows.Forms.TextBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioUseEmailFalse = new System.Windows.Forms.RadioButton();
			this.radioUseEmailTrue = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textDown = new OpenDental.ValidDouble();
			this.label12 = new System.Windows.Forms.Label();
			this.textRight = new OpenDental.ValidDouble();
			this.label13 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textDaysFuture = new OpenDental.ValidNumber();
			this.textDaysPast = new OpenDental.ValidNumber();
			this.checkGroupFamilies = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.comboStatusMailedRecall = new System.Windows.Forms.ComboBox();
			this.label26 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textMaxReminders = new OpenDental.ValidNumber();
			this.label4 = new System.Windows.Forms.Label();
			this.textDaysSecondReminder = new OpenDental.ValidNum();
			this.textDaysFirstReminder = new OpenDental.ValidNum();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboStatusEmailedRecall = new System.Windows.Forms.ComboBox();
			this.listTypes = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkReturnAdd = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.comboStatusEmailTextRecall = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboStatusTextedRecall = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(16, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(872, 456);
			this.gridMain.TabIndex = 67;
			this.gridMain.Title = "Messages";
			this.gridMain.CellDoubleClick += new System.EventHandler<UI.ODGridClickEventArgs>(this.gridMain_CellDoubleClick);
			// 
			// radioExcludeFutureYes
			// 
			this.radioExcludeFutureYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.radioExcludeFutureYes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureYes.Location = new System.Drawing.Point(414, 577);
			this.radioExcludeFutureYes.Name = "radioExcludeFutureYes";
			this.radioExcludeFutureYes.Size = new System.Drawing.Size(217, 18);
			this.radioExcludeFutureYes.TabIndex = 72;
			this.radioExcludeFutureYes.Text = "Exclude from list if any future appt";
			this.radioExcludeFutureYes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureYes.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(28, 567);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(176, 16);
			this.label8.TabIndex = 19;
			this.label8.Text = "Postcards per sheet (1,3,or 4)";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// radioExcludeFutureNo
			// 
			this.radioExcludeFutureNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.radioExcludeFutureNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureNo.Location = new System.Drawing.Point(414, 559);
			this.radioExcludeFutureNo.Name = "radioExcludeFutureNo";
			this.radioExcludeFutureNo.Size = new System.Drawing.Size(217, 18);
			this.radioExcludeFutureNo.TabIndex = 71;
			this.radioExcludeFutureNo.Text = "Exclude from list if recall scheduled";
			this.radioExcludeFutureNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureNo.UseVisualStyleBackColor = true;
			// 
			// textPostcardsPerSheet
			// 
			this.textPostcardsPerSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textPostcardsPerSheet.Location = new System.Drawing.Point(204, 564);
			this.textPostcardsPerSheet.Name = "textPostcardsPerSheet";
			this.textPostcardsPerSheet.Size = new System.Drawing.Size(34, 20);
			this.textPostcardsPerSheet.TabIndex = 18;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.radioUseEmailFalse);
			this.groupBox4.Controls.Add(this.radioUseEmailTrue);
			this.groupBox4.Location = new System.Drawing.Point(697, 552);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(191, 57);
			this.groupBox4.TabIndex = 70;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Use e-mail if";
			// 
			// radioUseEmailFalse
			// 
			this.radioUseEmailFalse.Location = new System.Drawing.Point(7, 34);
			this.radioUseEmailFalse.Name = "radioUseEmailFalse";
			this.radioUseEmailFalse.Size = new System.Drawing.Size(181, 18);
			this.radioUseEmailFalse.TabIndex = 1;
			this.radioUseEmailFalse.Text = "E-mail is preferred recall method";
			this.radioUseEmailFalse.UseVisualStyleBackColor = true;
			// 
			// radioUseEmailTrue
			// 
			this.radioUseEmailTrue.Location = new System.Drawing.Point(7, 17);
			this.radioUseEmailTrue.Name = "radioUseEmailTrue";
			this.radioUseEmailTrue.Size = new System.Drawing.Size(181, 18);
			this.radioUseEmailTrue.TabIndex = 0;
			this.radioUseEmailTrue.Text = "Has e-mail address";
			this.radioUseEmailTrue.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.textDown);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.textRight);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Location = new System.Drawing.Point(697, 479);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(191, 67);
			this.groupBox2.TabIndex = 48;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Adjust Postcard Position in Inches";
			// 
			// textDown
			// 
			this.textDown.Location = new System.Drawing.Point(110, 43);
			this.textDown.MaxVal = 100000000D;
			this.textDown.MinVal = -100000000D;
			this.textDown.Name = "textDown";
			this.textDown.Size = new System.Drawing.Size(73, 20);
			this.textDown.TabIndex = 6;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(48, 42);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(60, 20);
			this.label12.TabIndex = 5;
			this.label12.Text = "Down";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRight
			// 
			this.textRight.Location = new System.Drawing.Point(110, 18);
			this.textRight.MaxVal = 100000000D;
			this.textRight.MinVal = -100000000D;
			this.textRight.Name = "textRight";
			this.textRight.Size = new System.Drawing.Size(73, 20);
			this.textRight.TabIndex = 4;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(48, 17);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(60, 20);
			this.label13.TabIndex = 4;
			this.label13.Text = "Right";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.textDaysFuture);
			this.groupBox3.Controls.Add(this.textDaysPast);
			this.groupBox3.Controls.Add(this.checkGroupFamilies);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Location = new System.Drawing.Point(425, 479);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(253, 78);
			this.groupBox3.TabIndex = 54;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Recall List Default View";
			// 
			// textDaysFuture
			// 
			this.textDaysFuture.Location = new System.Drawing.Point(192, 54);
			this.textDaysFuture.MaxVal = 10000;
			this.textDaysFuture.MinVal = 0;
			this.textDaysFuture.Name = "textDaysFuture";
			this.textDaysFuture.Size = new System.Drawing.Size(53, 20);
			this.textDaysFuture.TabIndex = 66;
			// 
			// textDaysPast
			// 
			this.textDaysPast.Location = new System.Drawing.Point(192, 32);
			this.textDaysPast.MaxVal = 10000;
			this.textDaysPast.MinVal = 0;
			this.textDaysPast.Name = "textDaysPast";
			this.textDaysPast.Size = new System.Drawing.Size(53, 20);
			this.textDaysPast.TabIndex = 65;
			// 
			// checkGroupFamilies
			// 
			this.checkGroupFamilies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.Location = new System.Drawing.Point(85, 15);
			this.checkGroupFamilies.Name = "checkGroupFamilies";
			this.checkGroupFamilies.Size = new System.Drawing.Size(121, 18);
			this.checkGroupFamilies.TabIndex = 49;
			this.checkGroupFamilies.Text = "Group Families";
			this.checkGroupFamilies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 32);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(184, 20);
			this.label14.TabIndex = 50;
			this.label14.Text = "Days Past (e.g. 1095, blank, etc)";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(9, 53);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(181, 20);
			this.label15.TabIndex = 52;
			this.label15.Text = "Days Future (e.g. 7)";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label25
			// 
			this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label25.Location = new System.Drawing.Point(45, 476);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(157, 16);
			this.label25.TabIndex = 57;
			this.label25.Text = "Status for mailed recall";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusMailedRecall
			// 
			this.comboStatusMailedRecall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusMailedRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusMailedRecall.FormattingEnabled = true;
			this.comboStatusMailedRecall.Location = new System.Drawing.Point(204, 472);
			this.comboStatusMailedRecall.MaxDropDownItems = 20;
			this.comboStatusMailedRecall.Name = "comboStatusMailedRecall";
			this.comboStatusMailedRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusMailedRecall.TabIndex = 58;
			// 
			// label26
			// 
			this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label26.Location = new System.Drawing.Point(45, 499);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(157, 16);
			this.label26.TabIndex = 59;
			this.label26.Text = "Status for e-mailed recall";
			this.label26.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textMaxReminders);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textDaysSecondReminder);
			this.groupBox1.Controls.Add(this.textDaysFirstReminder);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(425, 597);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(253, 86);
			this.groupBox1.TabIndex = 65;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Also show in list if # of days since";
			// 
			// textMaxReminders
			// 
			this.textMaxReminders.Location = new System.Drawing.Point(192, 60);
			this.textMaxReminders.MaxVal = 10000;
			this.textMaxReminders.MinVal = 0;
			this.textMaxReminders.Name = "textMaxReminders";
			this.textMaxReminders.Size = new System.Drawing.Size(53, 20);
			this.textMaxReminders.TabIndex = 68;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(44, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(146, 20);
			this.label4.TabIndex = 67;
			this.label4.Text = "Max # Reminders (e.g. 4)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDaysSecondReminder
			// 
			this.textDaysSecondReminder.Location = new System.Drawing.Point(192, 38);
			this.textDaysSecondReminder.MaxVal = 10000;
			this.textDaysSecondReminder.MinVal = 0;
			this.textDaysSecondReminder.Name = "textDaysSecondReminder";
			this.textDaysSecondReminder.Size = new System.Drawing.Size(53, 20);
			this.textDaysSecondReminder.TabIndex = 66;
			// 
			// textDaysFirstReminder
			// 
			this.textDaysFirstReminder.Location = new System.Drawing.Point(192, 16);
			this.textDaysFirstReminder.MaxVal = 10000;
			this.textDaysFirstReminder.MinVal = 0;
			this.textDaysFirstReminder.Name = "textDaysFirstReminder";
			this.textDaysFirstReminder.Size = new System.Drawing.Size(53, 20);
			this.textDaysFirstReminder.TabIndex = 65;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(89, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 20);
			this.label2.TabIndex = 50;
			this.label2.Text = "Initial Reminder";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(44, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 20);
			this.label3.TabIndex = 52;
			this.label3.Text = "Second (or more) Reminder";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatusEmailedRecall
			// 
			this.comboStatusEmailedRecall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEmailedRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusEmailedRecall.FormattingEnabled = true;
			this.comboStatusEmailedRecall.Location = new System.Drawing.Point(204, 495);
			this.comboStatusEmailedRecall.MaxDropDownItems = 20;
			this.comboStatusEmailedRecall.Name = "comboStatusEmailedRecall";
			this.comboStatusEmailedRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusEmailedRecall.TabIndex = 60;
			// 
			// listTypes
			// 
			this.listTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.listTypes.FormattingEnabled = true;
			this.listTypes.Location = new System.Drawing.Point(204, 604);
			this.listTypes.Name = "listTypes";
			this.listTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listTypes.Size = new System.Drawing.Size(120, 82);
			this.listTypes.TabIndex = 64;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(47, 604);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(157, 65);
			this.label1.TabIndex = 63;
			this.label1.Text = "Types to show in recall list (typically just prophy, perio, and user-added types)" +
    "";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkReturnAdd
			// 
			this.checkReturnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkReturnAdd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReturnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReturnAdd.Location = new System.Drawing.Point(70, 585);
			this.checkReturnAdd.Name = "checkReturnAdd";
			this.checkReturnAdd.Size = new System.Drawing.Size(147, 19);
			this.checkReturnAdd.TabIndex = 43;
			this.checkReturnAdd.Text = "Show return address";
			this.checkReturnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(732, 659);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(813, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// comboStatusEmailTextRecall
			// 
			this.comboStatusEmailTextRecall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusEmailTextRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusEmailTextRecall.FormattingEnabled = true;
			this.comboStatusEmailTextRecall.Location = new System.Drawing.Point(204, 541);
			this.comboStatusEmailTextRecall.MaxDropDownItems = 20;
			this.comboStatusEmailTextRecall.Name = "comboStatusEmailTextRecall";
			this.comboStatusEmailTextRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusEmailTextRecall.TabIndex = 78;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(2, 545);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(200, 16);
			this.label5.TabIndex = 77;
			this.label5.Text = "Status for e-mailed and texted recall";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusTextedRecall
			// 
			this.comboStatusTextedRecall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatusTextedRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusTextedRecall.FormattingEnabled = true;
			this.comboStatusTextedRecall.Location = new System.Drawing.Point(204, 518);
			this.comboStatusTextedRecall.MaxDropDownItems = 20;
			this.comboStatusTextedRecall.Name = "comboStatusTextedRecall";
			this.comboStatusTextedRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusTextedRecall.TabIndex = 76;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(45, 522);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(157, 16);
			this.label6.TabIndex = 75;
			this.label6.Text = "Status for texted recall";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormRecallSetup
			// 
			this.ClientSize = new System.Drawing.Size(900, 695);
			this.Controls.Add(this.comboStatusEmailTextRecall);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboStatusTextedRecall);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.radioExcludeFutureYes);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.radioExcludeFutureNo);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.textPostcardsPerSheet);
			this.Controls.Add(this.checkReturnAdd);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listTypes);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.comboStatusEmailedRecall);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.comboStatusMailedRecall);
			this.Controls.Add(this.label25);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(990, 734);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(830, 360);
			this.Name = "FormRecallSetup";
			this.ShowInTaskbar = false;
			this.Text = "Setup Recall";
			this.Load += new System.EventHandler(this.FormRecallSetup_Load);
			this.groupBox4.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public void FormRecallSetup_Load(object sender, System.EventArgs e) {
			FillManualRecall();
		}

		///<summary>Called on load to initially load the recall window with values from the database.  Calls FillGrid at the end.</summary>
		private void FillManualRecall() {
			checkGroupFamilies.Checked = Preference.GetBool(PreferenceName.RecallGroupByFamily);
			textPostcardsPerSheet.Text=Preference.GetLong(PreferenceName.RecallPostcardsPerSheet).ToString();
			checkReturnAdd.Checked=Preference.GetBool(PreferenceName.RecallCardsShowReturnAdd);
			checkGroupFamilies.Checked=Preference.GetBool(PreferenceName.RecallGroupByFamily);
			textDaysPast.Text=Preference.GetLongHideNegOne(PreferenceName.RecallDaysPast);
			textDaysFuture.Text=Preference.GetLongHideNegOne(PreferenceName.RecallDaysFuture);
			if(Preference.GetBool(PreferenceName.RecallExcludeIfAnyFutureAppt)) {
				radioExcludeFutureYes.Checked=true;
			}
			else {
				radioExcludeFutureNo.Checked=true;
			}
			textRight.Text=Preference.GetDouble(PreferenceName.RecallAdjustRight).ToString();
			textDown.Text=Preference.GetDouble(PreferenceName.RecallAdjustDown).ToString();
			List<Definition> listUnschedStatusDefs=Definition.GetByCategory(DefinitionCategory.RecallUnschedStatus);
			comboStatusMailedRecall.SetItems(listUnschedStatusDefs,(status) => status.Description,
				(status) => status.Id==Preference.GetLong(PreferenceName.RecallStatusMailed));
			comboStatusEmailedRecall.SetItems(listUnschedStatusDefs,(status) => status.Description,
				(status) => status.Id==Preference.GetLong(PreferenceName.RecallStatusEmailed));
			comboStatusTextedRecall.SetItems(listUnschedStatusDefs,(status) => status.Description,
				(status) => status.Id==Preference.GetLong(PreferenceName.RecallStatusTexted));
			comboStatusEmailTextRecall.SetItems(listUnschedStatusDefs,(status) => status.Description,
				(status) => status.Id==Preference.GetLong(PreferenceName.RecallStatusEmailedTexted));
			List<long> recalltypes=new List<long>();
			string[] typearray=Preference.GetString(PreferenceName.RecallTypesShowingInList).Split(',');
			if(typearray.Length>0) {
				for(int i=0;i<typearray.Length;i++) {
					recalltypes.Add(PIn.Long(typearray[i]));
				}
			}
			listRecallCache=RecallTypes.GetWhere(x => x.Description!="Child Prophy");
			for(int i=0;i<listRecallCache.Count;i++) {
				listTypes.Items.Add(listRecallCache[i].Description);
				if(recalltypes.Contains(listRecallCache[i].RecallTypeNum)) {
					listTypes.SetSelected(i,true);
				}
			}
			textDaysFirstReminder.Text=Preference.GetLongHideNegOne(PreferenceName.RecallShowIfDaysFirstReminder,useZero:true);
			textDaysSecondReminder.Text=Preference.GetLongHideNegOne(PreferenceName.RecallShowIfDaysSecondReminder,useZero:true);
			textMaxReminders.Text=Preference.GetLongHideNegOne(PreferenceName.RecallMaxNumberReminders);
			if(Preference.GetBool(PreferenceName.RecallUseEmailIfHasEmailAddress)) {
				radioUseEmailTrue.Checked=true;
			}
			else {
				radioUseEmailFalse.Checked=true;
			}
			FillGrid();
		}

		private void FillGrid(){
			string availableFields="[NameF], [DueDate], [ClinicName], [ClinicPhone], [PracticeName], [PracticePhone], [OfficePhone]";
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Remind#"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Mode"),61);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Message"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			#region 1st Reminder
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailSubject));//old
			row.Tag="RecallEmailSubject";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameFL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailMessage));
			row.Tag="RecallEmailMessage";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailFamMsg));
			row.Tag="RecallEmailFamMsg";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameFL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardMessage));//old
			row.Tag="RecallPostcardMessage";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardFamMsg));//old
			row.Tag="RecallPostcardFamMsg";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedSubject));
			row.Tag="WebSchedSubject";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Email body.  Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessage));
			row.Tag="WebSchedMessage";
			gridMain.Rows.Add(row);
			AddRow(Lans.g(this,"All"),Lans.g(this,"WebSched Email Aggregate"),Lans.g(this,"Subject Line.  Available variables")+": [NameF], "
				+"[ClinicName], [PracticeName], [OfficeName]",PreferenceName.WebSchedAggregatedEmailSubject);
			AddRow(Lans.g(this,"All"),Lans.g(this,"WebSched Email Aggregate"),Lans.g(this,"Email body.  Available variables")+": [FamilyListURLs], [NameF],"
				+" [ClinicName], [ClinicPhone], [PracticeName], [PracticePhone], [OfficeName], [OfficePhone]",PreferenceName.WebSchedAggregatedEmailBody);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"WebSched Text"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessageText));
			row.Tag=PreferenceName.WebSchedMessageText.ToString();
			gridMain.Rows.Add(row);
			AddRow(Lans.g(this,"All"),Lans.g(this,"WebSched Text Aggregate"),Lans.g(this,"Available variables")+": [FamilyListURLs], [NameF], [ClinicName],"
				+" [ClinicPhone], [PracticeName], [PracticePhone], [OfficeName], [OfficePhone]",PreferenceName.WebSchedAggregatedTextMessage);
			#endregion
			#region 2nd Reminder
			//2---------------------------------------------------------------------------------------------
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailSubject2));
			row.Tag="RecallEmailSubject2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables")+": "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailMessage2));
			row.Tag="RecallEmailMessage2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailFamMsg2));
			row.Tag="RecallEmailFamMsg2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameFL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardMessage2));
			row.Tag="RecallPostcardMessage2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardFamMsg2));
			row.Tag="RecallPostcardFamMsg2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedSubject2));
			row.Tag="WebSchedSubject2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Email body.  Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessage2));
			row.Tag="WebSchedMessage2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"WebSched Text"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessageText2));
			row.Tag=PreferenceName.WebSchedMessageText2;
			gridMain.Rows.Add(row);
			#endregion
			#region 3rd Reminder
			//3---------------------------------------------------------------------------------------------
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailSubject3));
			row.Tag="RecallEmailSubject3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables")+": "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailMessage3));
			row.Tag="RecallEmailMessage3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallEmailFamMsg3));
			row.Tag="RecallEmailFamMsg3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameFL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardMessage3));
			row.Tag="RecallPostcardMessage3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(Preference.GetString(PreferenceName.RecallPostcardFamMsg3));
			row.Tag="RecallPostcardFamMsg3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedSubject3));
			row.Tag="WebSchedSubject3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"WebSched Email"));
			row.Cells.Add(Lan.g(this,"Email body.  Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessage3));
			row.Tag="WebSchedMessage3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"WebSched Text"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [URL], "+availableFields);
			row.Cells.Add(Preference.GetString(PreferenceName.WebSchedMessageText3));
			row.Tag=PreferenceName.WebSchedMessageText3.ToString();
			gridMain.Rows.Add(row);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PreferenceName prefName=(PreferenceName)Enum.Parse(typeof(PreferenceName),gridMain.Rows[e.Row].Tag.ToString());
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			FormR.MessageVal=Preference.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			Preference.Update(prefName,FormR.MessageVal);
			//Preference.Refresh();//above line handles it.
			FillGrid();
		}		

		private void AddRow(string reminderNum,string title,string availableVariables,PreferenceName pref) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(reminderNum);
			row.Cells.Add(title);
			row.Cells.Add(availableVariables);
			row.Cells.Add(Preference.GetString(pref));
			row.Tag=pref.ToString();
			gridMain.Rows.Add(row);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textRight.errorProvider1.GetError(textRight)!=""
				|| textDown.errorProvider1.GetError(textDown)!=""
				|| textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textDaysFuture.errorProvider1.GetError(textDaysFuture)!=""
				|| textDaysFirstReminder.errorProvider1.GetError(textDaysFirstReminder)!=""
				|| textDaysSecondReminder.errorProvider1.GetError(textDaysSecondReminder)!=""
				|| textMaxReminders.errorProvider1.GetError(textMaxReminders)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			//We changed the text box to a ValidNum, which prevents it from ever being blank, so this message will never fire.
			//if(textDaysFirstReminder.Text=="") {
			//	if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Initial Reminder box should not be blank, or the recall list will be blank.")) {
			//		return;
			//	}
			//}
			if(textPostcardsPerSheet.Text!="1"
				&& textPostcardsPerSheet.Text!="3"
				&& textPostcardsPerSheet.Text!="4") {
				MsgBox.Show(this,"The value in postcards per sheet must be 1, 3, or 4");
				return;
			}
			if(comboStatusMailedRecall.SelectedIndex==-1
				|| comboStatusEmailedRecall.SelectedIndex==-1
				|| comboStatusTextedRecall.SelectedIndex==-1
				|| comboStatusEmailTextRecall.SelectedIndex==-1) 
			{
				MsgBox.Show(this,"All status options on the left must be set.");
				return;
			}
			//End of Validation
			if(Preference.Update(PreferenceName.RecallPostcardsPerSheet,textPostcardsPerSheet.Text)) {
				if(textPostcardsPerSheet.Text=="1") {
					MsgBox.Show(this,"If using 1 postcard per sheet, you must adjust the position, and also the preview will not work");
				}
			}
			Preference.Update(PreferenceName.RecallCardsShowReturnAdd,checkReturnAdd.Checked);
			Preference.Update(PreferenceName.RecallGroupByFamily,checkGroupFamilies.Checked);
            Preference.UpdateLongAsNegOne(PreferenceName.RecallDaysPast,textDaysPast.Text);
            Preference.UpdateLongAsNegOne(PreferenceName.RecallDaysFuture,textDaysFuture.Text);
			Preference.Update(PreferenceName.RecallExcludeIfAnyFutureAppt,radioExcludeFutureYes.Checked);
			Preference.Update(PreferenceName.RecallAdjustRight,PIn.Double(textRight.Text));
			Preference.Update(PreferenceName.RecallAdjustDown,PIn.Double(textDown.Text));
			//combo boxes These have already been checked for -1
			Preference.Update(PreferenceName.RecallStatusEmailed,comboStatusEmailedRecall.SelectedTag<Definition>().Id);
			Preference.Update(PreferenceName.RecallStatusMailed,comboStatusMailedRecall.SelectedTag<Definition>().Id);
			Preference.Update(PreferenceName.RecallStatusTexted,comboStatusTextedRecall.SelectedTag<Definition>().Id);
			Preference.Update(PreferenceName.RecallStatusEmailedTexted,comboStatusEmailTextRecall.SelectedTag<Definition>().Id);
			string recalltypes = string.Join(",",listTypes.SelectedIndices.OfType<int>().Select(x => listRecallCache[x].RecallTypeNum));
			Preference.Update(PreferenceName.RecallTypesShowingInList,recalltypes);
            Preference.UpdateLongAsNegOne(PreferenceName.RecallShowIfDaysFirstReminder,textDaysFirstReminder.Text);
            Preference.UpdateLongAsNegOne(PreferenceName.RecallShowIfDaysSecondReminder,textDaysSecondReminder.Text);
            Preference.UpdateLongAsNegOne(PreferenceName.RecallMaxNumberReminders,textMaxReminders.Text);
			Preference.Update(PreferenceName.RecallUseEmailIfHasEmailAddress,radioUseEmailTrue.Checked);
			//If we want to take the time to check every Update and see if something changed 
			//then we could move this to a FormClosing event later.
			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}
