﻿namespace OpenDental {
	partial class FormPaySplitEdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySplitEdit));
			this.labelRemainder = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.listPatient = new System.Windows.Forms.ListBox();
			this.labelAmount = new System.Windows.Forms.Label();
			this.checkPayPlan = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.checkPatOtherFam = new System.Windows.Forms.CheckBox();
			this.groupPatient = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.textDatePay = new OpenDental.ValidDate();
			this.butDelete = new OpenDental.UI.Button();
			this.textAmount = new OpenDental.ValidDouble();
			this.butRemainder = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.ButCancel = new OpenDental.UI.Button();
			this.comboUnearnedTypes = new System.Windows.Forms.ComboBox();
			this.comboProvider = new System.Windows.Forms.ComboBox();
			this.butPickProv = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelPrePayWarning = new System.Windows.Forms.Label();
			this.textPrePaidElsewhere = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.textPrePaidRemain = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.textPrePaidHere = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.textPrePayAmt = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.textPrePayType = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.textPrePayDate = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.butDetachPrepay = new OpenDental.UI.Button();
			this.butAttachPrepay = new OpenDental.UI.Button();
			this.butEditAnyway = new OpenDental.UI.Button();
			this.labelEditAnyway = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabProcedure = new System.Windows.Forms.TabPage();
			this.textProcWriteoff = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textProcTooth = new System.Windows.Forms.TextBox();
			this.labelProcTooth = new System.Windows.Forms.Label();
			this.textProcProv = new System.Windows.Forms.TextBox();
			this.textProcDescription = new System.Windows.Forms.TextBox();
			this.textProcDate = new System.Windows.Forms.TextBox();
			this.labelProcRemain = new System.Windows.Forms.Label();
			this.textProcPaidHere = new System.Windows.Forms.TextBox();
			this.textProcPrevPaid = new System.Windows.Forms.TextBox();
			this.textProcAdj = new System.Windows.Forms.TextBox();
			this.textProcInsEst = new System.Windows.Forms.TextBox();
			this.textProcInsPaid = new System.Windows.Forms.TextBox();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butDetachProc = new OpenDental.UI.Button();
			this.butAttachProc = new OpenDental.UI.Button();
			this.tabAdjustment = new System.Windows.Forms.TabPage();
			this.textAdjProv = new System.Windows.Forms.TextBox();
			this.textAdjDate = new System.Windows.Forms.TextBox();
			this.labelAdjRemaining = new System.Windows.Forms.Label();
			this.textAdjPaidHere = new System.Windows.Forms.TextBox();
			this.textAdjPrevUsed = new System.Windows.Forms.TextBox();
			this.textAdjAmt = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.butDetachAdjust = new OpenDental.UI.Button();
			this.butAttachAdjust = new OpenDental.UI.Button();
			this.groupPatient.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabProcedure.SuspendLayout();
			this.tabAdjustment.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelRemainder
			// 
			this.labelRemainder.Location = new System.Drawing.Point(5, 336);
			this.labelRemainder.Name = "labelRemainder";
			this.labelRemainder.Size = new System.Drawing.Size(119, 88);
			this.labelRemainder.TabIndex = 5;
			this.labelRemainder.Text = "The Remainder button will calculate the value needed to make the splits balance.";
			this.labelRemainder.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(33, 169);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(95, 16);
			this.label5.TabIndex = 10;
			this.label5.Text = "Provider";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listPatient
			// 
			this.listPatient.Location = new System.Drawing.Point(11, 34);
			this.listPatient.Name = "listPatient";
			this.listPatient.Size = new System.Drawing.Size(192, 108);
			this.listPatient.TabIndex = 3;
			this.listPatient.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPatient_MouseDown);
			// 
			// labelAmount
			// 
			this.labelAmount.Location = new System.Drawing.Point(23, 96);
			this.labelAmount.Name = "labelAmount";
			this.labelAmount.Size = new System.Drawing.Size(104, 16);
			this.labelAmount.TabIndex = 15;
			this.labelAmount.Text = "Amount";
			this.labelAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkPayPlan
			// 
			this.checkPayPlan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlan.Location = new System.Drawing.Point(257, 549);
			this.checkPayPlan.Name = "checkPayPlan";
			this.checkPayPlan.Size = new System.Drawing.Size(198, 18);
			this.checkPayPlan.TabIndex = 20;
			this.checkPayPlan.Text = "Attached to Payment Plan";
			this.checkPayPlan.Click += new System.EventHandler(this.checkPayPlan_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 16);
			this.label1.TabIndex = 23;
			this.label1.Text = "Payment Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(11, 33);
			this.textPatient.Name = "textPatient";
			this.textPatient.Size = new System.Drawing.Size(238, 20);
			this.textPatient.TabIndex = 111;
			// 
			// checkPatOtherFam
			// 
			this.checkPatOtherFam.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatOtherFam.Location = new System.Drawing.Point(11, 15);
			this.checkPatOtherFam.Name = "checkPatOtherFam";
			this.checkPatOtherFam.Size = new System.Drawing.Size(192, 17);
			this.checkPatOtherFam.TabIndex = 110;
			this.checkPatOtherFam.TabStop = false;
			this.checkPatOtherFam.Text = "Is from another family";
			this.checkPatOtherFam.Click += new System.EventHandler(this.checkPatOtherFam_Click);
			// 
			// groupPatient
			// 
			this.groupPatient.Controls.Add(this.listPatient);
			this.groupPatient.Controls.Add(this.textPatient);
			this.groupPatient.Controls.Add(this.checkPatOtherFam);
			this.groupPatient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupPatient.Location = new System.Drawing.Point(480, 3);
			this.groupPatient.Name = "groupPatient";
			this.groupPatient.Size = new System.Drawing.Size(265, 157);
			this.groupPatient.TabIndex = 112;
			this.groupPatient.TabStop = false;
			this.groupPatient.Text = "Patient";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(1, 24);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(127, 16);
			this.label15.TabIndex = 115;
			this.label15.Text = "Entry Date";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(3, 121);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(124, 16);
			this.label17.TabIndex = 116;
			this.label17.Text = "Unearned Type";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(129, 22);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(92, 20);
			this.textDateEntry.TabIndex = 114;
			// 
			// textDatePay
			// 
			this.textDatePay.Location = new System.Drawing.Point(129, 46);
			this.textDatePay.Name = "textDatePay";
			this.textDatePay.ReadOnly = true;
			this.textDatePay.Size = new System.Drawing.Size(92, 20);
			this.textDatePay.TabIndex = 22;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 555);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(85, 24);
			this.butDelete.TabIndex = 21;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(129, 94);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(77, 20);
			this.textAmount.TabIndex = 1;
			this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// butRemainder
			// 
			this.butRemainder.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemainder.Autosize = true;
			this.butRemainder.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemainder.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemainder.CornerRadius = 4F;
			this.butRemainder.Location = new System.Drawing.Point(5, 304);
			this.butRemainder.Name = "butRemainder";
			this.butRemainder.Size = new System.Drawing.Size(92, 24);
			this.butRemainder.TabIndex = 7;
			this.butRemainder.Text = "&Remainder";
			this.butRemainder.Visible = false;
			this.butRemainder.Click += new System.EventHandler(this.butRemainder_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(589, 555);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// ButCancel
			// 
			this.ButCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.ButCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButCancel.Autosize = true;
			this.ButCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.ButCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.ButCancel.CornerRadius = 4F;
			this.ButCancel.Location = new System.Drawing.Point(670, 555);
			this.ButCancel.Name = "ButCancel";
			this.ButCancel.Size = new System.Drawing.Size(75, 24);
			this.ButCancel.TabIndex = 6;
			this.ButCancel.Text = "&Cancel";
			this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
			// 
			// comboUnearnedTypes
			// 
			this.comboUnearnedTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnearnedTypes.FormattingEnabled = true;
			this.comboUnearnedTypes.Location = new System.Drawing.Point(129, 118);
			this.comboUnearnedTypes.Name = "comboUnearnedTypes";
			this.comboUnearnedTypes.Size = new System.Drawing.Size(165, 21);
			this.comboUnearnedTypes.TabIndex = 117;
			this.comboUnearnedTypes.SelectionChangeCommitted += new System.EventHandler(this.comboUnearnedTypes_SelectionChangeCommitted);
			// 
			// comboProvider
			// 
			this.comboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider.FormattingEnabled = true;
			this.comboProvider.Location = new System.Drawing.Point(129, 168);
			this.comboProvider.Name = "comboProvider";
			this.comboProvider.Size = new System.Drawing.Size(145, 21);
			this.comboProvider.TabIndex = 118;
			this.comboProvider.SelectionChangeCommitted += new System.EventHandler(this.comboProvider_SelectionChangeCommitted);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 4F;
			this.butPickProv.Location = new System.Drawing.Point(276, 168);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 20);
			this.butPickProv.TabIndex = 158;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(129, 143);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(165, 21);
			this.comboClinic.TabIndex = 160;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(3, 146);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(124, 16);
			this.labelClinic.TabIndex = 159;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelPrePayWarning);
			this.groupBox1.Controls.Add(this.textPrePaidElsewhere);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.textPrePaidRemain);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.textPrePaidHere);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.textPrePayAmt);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.textPrePayType);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.textPrePayDate);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.butDetachPrepay);
			this.groupBox1.Controls.Add(this.butAttachPrepay);
			this.groupBox1.Location = new System.Drawing.Point(129, 436);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(616, 101);
			this.groupBox1.TabIndex = 161;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Payment Split";
			// 
			// labelPrePayWarning
			// 
			this.labelPrePayWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPrePayWarning.ForeColor = System.Drawing.Color.Firebrick;
			this.labelPrePayWarning.Location = new System.Drawing.Point(182, 20);
			this.labelPrePayWarning.Name = "labelPrePayWarning";
			this.labelPrePayWarning.Size = new System.Drawing.Size(228, 23);
			this.labelPrePayWarning.TabIndex = 57;
			this.labelPrePayWarning.Text = "ERROR: PAYMENT SPLIT DELETED!";
			this.labelPrePayWarning.Visible = false;
			// 
			// textPrePaidElsewhere
			// 
			this.textPrePaidElsewhere.Location = new System.Drawing.Point(514, 51);
			this.textPrePaidElsewhere.Name = "textPrePaidElsewhere";
			this.textPrePaidElsewhere.ReadOnly = true;
			this.textPrePaidElsewhere.Size = new System.Drawing.Size(76, 20);
			this.textPrePaidElsewhere.TabIndex = 56;
			this.textPrePaidElsewhere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(318, 53);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(192, 16);
			this.label21.TabIndex = 55;
			this.label21.Text = "Allocated Elsewhere";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePaidRemain
			// 
			this.textPrePaidRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textPrePaidRemain.Location = new System.Drawing.Point(515, 72);
			this.textPrePaidRemain.Name = "textPrePaidRemain";
			this.textPrePaidRemain.Size = new System.Drawing.Size(73, 18);
			this.textPrePaidRemain.TabIndex = 52;
			this.textPrePaidRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(318, 73);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(192, 16);
			this.label22.TabIndex = 51;
			this.label22.Text = "Remaining";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePaidHere
			// 
			this.textPrePaidHere.Location = new System.Drawing.Point(514, 33);
			this.textPrePaidHere.Name = "textPrePaidHere";
			this.textPrePaidHere.ReadOnly = true;
			this.textPrePaidHere.Size = new System.Drawing.Size(76, 20);
			this.textPrePaidHere.TabIndex = 52;
			this.textPrePaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(318, 35);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(192, 16);
			this.label20.TabIndex = 51;
			this.label20.Text = "Used Here";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePayAmt
			// 
			this.textPrePayAmt.Location = new System.Drawing.Point(514, 13);
			this.textPrePayAmt.Name = "textPrePayAmt";
			this.textPrePayAmt.ReadOnly = true;
			this.textPrePayAmt.Size = new System.Drawing.Size(76, 20);
			this.textPrePayAmt.TabIndex = 52;
			this.textPrePayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(318, 15);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(192, 16);
			this.label19.TabIndex = 51;
			this.label19.Text = "Prepayment Amount";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePayType
			// 
			this.textPrePayType.Location = new System.Drawing.Point(116, 70);
			this.textPrePayType.Name = "textPrePayType";
			this.textPrePayType.ReadOnly = true;
			this.textPrePayType.Size = new System.Drawing.Size(155, 20);
			this.textPrePayType.TabIndex = 54;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(9, 72);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(104, 16);
			this.label18.TabIndex = 53;
			this.label18.Text = "Payment Type";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPrePayDate
			// 
			this.textPrePayDate.Location = new System.Drawing.Point(116, 49);
			this.textPrePayDate.Name = "textPrePayDate";
			this.textPrePayDate.ReadOnly = true;
			this.textPrePayDate.Size = new System.Drawing.Size(76, 20);
			this.textPrePayDate.TabIndex = 52;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(9, 51);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 16);
			this.label14.TabIndex = 51;
			this.label14.Text = "Date";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachPrepay
			// 
			this.butDetachPrepay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachPrepay.Autosize = true;
			this.butDetachPrepay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachPrepay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachPrepay.CornerRadius = 4F;
			this.butDetachPrepay.Location = new System.Drawing.Point(100, 19);
			this.butDetachPrepay.Name = "butDetachPrepay";
			this.butDetachPrepay.Size = new System.Drawing.Size(75, 24);
			this.butDetachPrepay.TabIndex = 52;
			this.butDetachPrepay.Text = "Detach";
			this.butDetachPrepay.Click += new System.EventHandler(this.butDetachPrepay_Click);
			// 
			// butAttachPrepay
			// 
			this.butAttachPrepay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachPrepay.Autosize = true;
			this.butAttachPrepay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachPrepay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachPrepay.CornerRadius = 4F;
			this.butAttachPrepay.Location = new System.Drawing.Point(12, 19);
			this.butAttachPrepay.Name = "butAttachPrepay";
			this.butAttachPrepay.Size = new System.Drawing.Size(75, 24);
			this.butAttachPrepay.TabIndex = 51;
			this.butAttachPrepay.Text = "Attach";
			this.butAttachPrepay.Click += new System.EventHandler(this.butAttachPrepay_Click);
			// 
			// butEditAnyway
			// 
			this.butEditAnyway.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditAnyway.Autosize = true;
			this.butEditAnyway.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAnyway.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAnyway.CornerRadius = 4F;
			this.butEditAnyway.Location = new System.Drawing.Point(670, 178);
			this.butEditAnyway.Name = "butEditAnyway";
			this.butEditAnyway.Size = new System.Drawing.Size(75, 24);
			this.butEditAnyway.TabIndex = 163;
			this.butEditAnyway.Text = "Edit Anyway";
			this.butEditAnyway.Visible = false;
			this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
			// 
			// labelEditAnyway
			// 
			this.labelEditAnyway.Location = new System.Drawing.Point(414, 176);
			this.labelEditAnyway.Name = "labelEditAnyway";
			this.labelEditAnyway.Size = new System.Drawing.Size(250, 28);
			this.labelEditAnyway.TabIndex = 164;
			this.labelEditAnyway.Text = "This paysplit is attached to a \r\nprocedure or adjustment and should not be edited" +
    ".";
			this.labelEditAnyway.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelEditAnyway.Visible = false;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabProcedure);
			this.tabControl.Controls.Add(this.tabAdjustment);
			this.tabControl.Location = new System.Drawing.Point(124, 204);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(627, 226);
			this.tabControl.TabIndex = 165;
			// 
			// tabProcedure
			// 
			this.tabProcedure.Controls.Add(this.textProcWriteoff);
			this.tabProcedure.Controls.Add(this.label16);
			this.tabProcedure.Controls.Add(this.textProcTooth);
			this.tabProcedure.Controls.Add(this.labelProcTooth);
			this.tabProcedure.Controls.Add(this.textProcProv);
			this.tabProcedure.Controls.Add(this.textProcDescription);
			this.tabProcedure.Controls.Add(this.textProcDate);
			this.tabProcedure.Controls.Add(this.labelProcRemain);
			this.tabProcedure.Controls.Add(this.textProcPaidHere);
			this.tabProcedure.Controls.Add(this.textProcPrevPaid);
			this.tabProcedure.Controls.Add(this.textProcAdj);
			this.tabProcedure.Controls.Add(this.textProcInsEst);
			this.tabProcedure.Controls.Add(this.textProcInsPaid);
			this.tabProcedure.Controls.Add(this.textProcFee);
			this.tabProcedure.Controls.Add(this.label13);
			this.tabProcedure.Controls.Add(this.label12);
			this.tabProcedure.Controls.Add(this.label11);
			this.tabProcedure.Controls.Add(this.label10);
			this.tabProcedure.Controls.Add(this.label9);
			this.tabProcedure.Controls.Add(this.label8);
			this.tabProcedure.Controls.Add(this.label6);
			this.tabProcedure.Controls.Add(this.label4);
			this.tabProcedure.Controls.Add(this.label3);
			this.tabProcedure.Controls.Add(this.label2);
			this.tabProcedure.Controls.Add(this.butDetachProc);
			this.tabProcedure.Controls.Add(this.butAttachProc);
			this.tabProcedure.Location = new System.Drawing.Point(4, 22);
			this.tabProcedure.Name = "tabProcedure";
			this.tabProcedure.Padding = new System.Windows.Forms.Padding(3);
			this.tabProcedure.Size = new System.Drawing.Size(619, 200);
			this.tabProcedure.TabIndex = 0;
			this.tabProcedure.Text = "Procedure";
			// 
			// textProcWriteoff
			// 
			this.textProcWriteoff.Location = new System.Drawing.Point(513, 39);
			this.textProcWriteoff.Name = "textProcWriteoff";
			this.textProcWriteoff.ReadOnly = true;
			this.textProcWriteoff.Size = new System.Drawing.Size(76, 20);
			this.textProcWriteoff.TabIndex = 76;
			this.textProcWriteoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(405, 41);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(104, 16);
			this.label16.TabIndex = 75;
			this.label16.Text = "Writeoffs";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcTooth
			// 
			this.textProcTooth.Location = new System.Drawing.Point(115, 119);
			this.textProcTooth.Name = "textProcTooth";
			this.textProcTooth.ReadOnly = true;
			this.textProcTooth.Size = new System.Drawing.Size(43, 20);
			this.textProcTooth.TabIndex = 74;
			// 
			// labelProcTooth
			// 
			this.labelProcTooth.Location = new System.Drawing.Point(9, 122);
			this.labelProcTooth.Name = "labelProcTooth";
			this.labelProcTooth.Size = new System.Drawing.Size(104, 16);
			this.labelProcTooth.TabIndex = 73;
			this.labelProcTooth.Text = "Tooth";
			this.labelProcTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProcProv
			// 
			this.textProcProv.Location = new System.Drawing.Point(115, 99);
			this.textProcProv.Name = "textProcProv";
			this.textProcProv.ReadOnly = true;
			this.textProcProv.Size = new System.Drawing.Size(76, 20);
			this.textProcProv.TabIndex = 72;
			// 
			// textProcDescription
			// 
			this.textProcDescription.Location = new System.Drawing.Point(115, 139);
			this.textProcDescription.Name = "textProcDescription";
			this.textProcDescription.ReadOnly = true;
			this.textProcDescription.Size = new System.Drawing.Size(241, 20);
			this.textProcDescription.TabIndex = 71;
			// 
			// textProcDate
			// 
			this.textProcDate.Location = new System.Drawing.Point(115, 79);
			this.textProcDate.Name = "textProcDate";
			this.textProcDate.ReadOnly = true;
			this.textProcDate.Size = new System.Drawing.Size(76, 20);
			this.textProcDate.TabIndex = 70;
			// 
			// labelProcRemain
			// 
			this.labelProcRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProcRemain.Location = new System.Drawing.Point(514, 166);
			this.labelProcRemain.Name = "labelProcRemain";
			this.labelProcRemain.Size = new System.Drawing.Size(73, 18);
			this.labelProcRemain.TabIndex = 69;
			this.labelProcRemain.Text = "$0.00";
			this.labelProcRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcPaidHere
			// 
			this.textProcPaidHere.Location = new System.Drawing.Point(513, 139);
			this.textProcPaidHere.Name = "textProcPaidHere";
			this.textProcPaidHere.ReadOnly = true;
			this.textProcPaidHere.Size = new System.Drawing.Size(76, 20);
			this.textProcPaidHere.TabIndex = 68;
			this.textProcPaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcPrevPaid
			// 
			this.textProcPrevPaid.Location = new System.Drawing.Point(513, 119);
			this.textProcPrevPaid.Name = "textProcPrevPaid";
			this.textProcPrevPaid.ReadOnly = true;
			this.textProcPrevPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcPrevPaid.TabIndex = 67;
			this.textProcPrevPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcAdj
			// 
			this.textProcAdj.Location = new System.Drawing.Point(513, 99);
			this.textProcAdj.Name = "textProcAdj";
			this.textProcAdj.ReadOnly = true;
			this.textProcAdj.Size = new System.Drawing.Size(76, 20);
			this.textProcAdj.TabIndex = 66;
			this.textProcAdj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsEst
			// 
			this.textProcInsEst.Location = new System.Drawing.Point(513, 79);
			this.textProcInsEst.Name = "textProcInsEst";
			this.textProcInsEst.ReadOnly = true;
			this.textProcInsEst.Size = new System.Drawing.Size(76, 20);
			this.textProcInsEst.TabIndex = 65;
			this.textProcInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsPaid
			// 
			this.textProcInsPaid.Location = new System.Drawing.Point(513, 59);
			this.textProcInsPaid.Name = "textProcInsPaid";
			this.textProcInsPaid.ReadOnly = true;
			this.textProcInsPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcInsPaid.TabIndex = 64;
			this.textProcInsPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(513, 19);
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.ReadOnly = true;
			this.textProcFee.Size = new System.Drawing.Size(76, 20);
			this.textProcFee.TabIndex = 63;
			this.textProcFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(405, 141);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 16);
			this.label13.TabIndex = 62;
			this.label13.Text = "This Payment Split";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(405, 167);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 16);
			this.label12.TabIndex = 61;
			this.label12.Text = "Remaining";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(382, 121);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(127, 16);
			this.label11.TabIndex = 60;
			this.label11.Text = "Patient Paid";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(405, 101);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 16);
			this.label10.TabIndex = 59;
			this.label10.Text = "Adjustments";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(405, 81);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 58;
			this.label9.Text = "Ins Est";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(405, 61);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 57;
			this.label8.Text = "Ins Paid";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(405, 21);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 16);
			this.label6.TabIndex = 56;
			this.label6.Text = "Fee";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 55;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(9, 142);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 54;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 53;
			this.label2.Text = "Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachProc
			// 
			this.butDetachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachProc.Autosize = true;
			this.butDetachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachProc.CornerRadius = 4F;
			this.butDetachProc.Location = new System.Drawing.Point(99, 45);
			this.butDetachProc.Name = "butDetachProc";
			this.butDetachProc.Size = new System.Drawing.Size(75, 24);
			this.butDetachProc.TabIndex = 52;
			this.butDetachProc.Text = "Detach";
			this.butDetachProc.Click += new System.EventHandler(this.butDetachProc_Click);
			// 
			// butAttachProc
			// 
			this.butAttachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachProc.Autosize = true;
			this.butAttachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachProc.CornerRadius = 4F;
			this.butAttachProc.Location = new System.Drawing.Point(12, 45);
			this.butAttachProc.Name = "butAttachProc";
			this.butAttachProc.Size = new System.Drawing.Size(75, 24);
			this.butAttachProc.TabIndex = 51;
			this.butAttachProc.Text = "Attach";
			this.butAttachProc.Click += new System.EventHandler(this.butAttachProc_Click);
			// 
			// tabAdjustment
			// 
			this.tabAdjustment.Controls.Add(this.textAdjProv);
			this.tabAdjustment.Controls.Add(this.textAdjDate);
			this.tabAdjustment.Controls.Add(this.labelAdjRemaining);
			this.tabAdjustment.Controls.Add(this.textAdjPaidHere);
			this.tabAdjustment.Controls.Add(this.textAdjPrevUsed);
			this.tabAdjustment.Controls.Add(this.textAdjAmt);
			this.tabAdjustment.Controls.Add(this.label26);
			this.tabAdjustment.Controls.Add(this.label27);
			this.tabAdjustment.Controls.Add(this.label28);
			this.tabAdjustment.Controls.Add(this.label29);
			this.tabAdjustment.Controls.Add(this.label33);
			this.tabAdjustment.Controls.Add(this.label35);
			this.tabAdjustment.Controls.Add(this.butDetachAdjust);
			this.tabAdjustment.Controls.Add(this.butAttachAdjust);
			this.tabAdjustment.Location = new System.Drawing.Point(4, 22);
			this.tabAdjustment.Name = "tabAdjustment";
			this.tabAdjustment.Padding = new System.Windows.Forms.Padding(3);
			this.tabAdjustment.Size = new System.Drawing.Size(619, 200);
			this.tabAdjustment.TabIndex = 1;
			this.tabAdjustment.Text = "Adjustment";
			// 
			// textAdjProv
			// 
			this.textAdjProv.Location = new System.Drawing.Point(115, 99);
			this.textAdjProv.Name = "textAdjProv";
			this.textAdjProv.ReadOnly = true;
			this.textAdjProv.Size = new System.Drawing.Size(76, 20);
			this.textAdjProv.TabIndex = 58;
			// 
			// textAdjDate
			// 
			this.textAdjDate.Location = new System.Drawing.Point(115, 79);
			this.textAdjDate.Name = "textAdjDate";
			this.textAdjDate.ReadOnly = true;
			this.textAdjDate.Size = new System.Drawing.Size(76, 20);
			this.textAdjDate.TabIndex = 57;
			// 
			// labelAdjRemaining
			// 
			this.labelAdjRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAdjRemaining.Location = new System.Drawing.Point(514, 126);
			this.labelAdjRemaining.Name = "labelAdjRemaining";
			this.labelAdjRemaining.Size = new System.Drawing.Size(73, 18);
			this.labelAdjRemaining.TabIndex = 56;
			this.labelAdjRemaining.Text = "$0.00";
			this.labelAdjRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAdjPaidHere
			// 
			this.textAdjPaidHere.Location = new System.Drawing.Point(513, 99);
			this.textAdjPaidHere.Name = "textAdjPaidHere";
			this.textAdjPaidHere.ReadOnly = true;
			this.textAdjPaidHere.Size = new System.Drawing.Size(76, 20);
			this.textAdjPaidHere.TabIndex = 55;
			this.textAdjPaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textAdjPrevUsed
			// 
			this.textAdjPrevUsed.Location = new System.Drawing.Point(513, 79);
			this.textAdjPrevUsed.Name = "textAdjPrevUsed";
			this.textAdjPrevUsed.ReadOnly = true;
			this.textAdjPrevUsed.Size = new System.Drawing.Size(76, 20);
			this.textAdjPrevUsed.TabIndex = 54;
			this.textAdjPrevUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textAdjAmt
			// 
			this.textAdjAmt.Location = new System.Drawing.Point(513, 59);
			this.textAdjAmt.Name = "textAdjAmt";
			this.textAdjAmt.ReadOnly = true;
			this.textAdjAmt.Size = new System.Drawing.Size(76, 20);
			this.textAdjAmt.TabIndex = 53;
			this.textAdjAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(405, 101);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(104, 16);
			this.label26.TabIndex = 52;
			this.label26.Text = "This Payment Split";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(405, 127);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(104, 16);
			this.label27.TabIndex = 51;
			this.label27.Text = "Remaining";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(382, 81);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(127, 16);
			this.label28.TabIndex = 50;
			this.label28.Text = "Paid Previously";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(405, 61);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(104, 16);
			this.label29.TabIndex = 49;
			this.label29.Text = "Adjust Amount";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(9, 102);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(104, 16);
			this.label33.TabIndex = 48;
			this.label33.Text = "Provider";
			this.label33.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(8, 81);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(104, 16);
			this.label35.TabIndex = 47;
			this.label35.Text = "Date";
			this.label35.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachAdjust
			// 
			this.butDetachAdjust.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachAdjust.Autosize = true;
			this.butDetachAdjust.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachAdjust.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachAdjust.CornerRadius = 4F;
			this.butDetachAdjust.Location = new System.Drawing.Point(99, 45);
			this.butDetachAdjust.Name = "butDetachAdjust";
			this.butDetachAdjust.Size = new System.Drawing.Size(75, 24);
			this.butDetachAdjust.TabIndex = 46;
			this.butDetachAdjust.Text = "Detach";
			this.butDetachAdjust.Click += new System.EventHandler(this.butDetachAdjust_Click);
			// 
			// butAttachAdjust
			// 
			this.butAttachAdjust.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachAdjust.Autosize = true;
			this.butAttachAdjust.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachAdjust.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachAdjust.CornerRadius = 4F;
			this.butAttachAdjust.Location = new System.Drawing.Point(12, 45);
			this.butAttachAdjust.Name = "butAttachAdjust";
			this.butAttachAdjust.Size = new System.Drawing.Size(75, 24);
			this.butAttachAdjust.TabIndex = 45;
			this.butAttachAdjust.Text = "Attach";
			this.butAttachAdjust.Click += new System.EventHandler(this.butAttachAdjust_Click);
			// 
			// FormPaySplitEdit
			// 
			this.ClientSize = new System.Drawing.Size(801, 592);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.butEditAnyway);
			this.Controls.Add(this.labelEditAnyway);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.comboProvider);
			this.Controls.Add(this.comboUnearnedTypes);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.groupPatient);
			this.Controls.Add(this.textDatePay);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.butRemainder);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.ButCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkPayPlan);
			this.Controls.Add(this.labelAmount);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.labelRemainder);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(0, 400);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(766, 630);
			this.Name = "FormPaySplitEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Payment Split";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPaySplitEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormPaySplitEdit_Load);
			this.groupPatient.ResumeLayout(false);
			this.groupPatient.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabProcedure.ResumeLayout(false);
			this.tabProcedure.PerformLayout();
			this.tabAdjustment.ResumeLayout(false);
			this.tabAdjustment.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button ButCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butRemainder;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butPickProv;
		private UI.Button butDetachPrepay;
		private UI.Button butAttachPrepay;
		private UI.Button butEditAnyway;
		private System.Windows.Forms.CheckBox checkPayPlan;
		private System.Windows.Forms.CheckBox checkPatOtherFam;
		private System.Windows.Forms.ComboBox comboUnearnedTypes;
		private System.Windows.Forms.ComboBox comboProvider;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupPatient;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.Label labelEditAnyway;
		private System.Windows.Forms.Label labelPrePayWarning;
		private System.Windows.Forms.Label textPrePaidRemain;
		private System.Windows.Forms.Label labelAmount;
		private System.Windows.Forms.Label labelRemainder;
		private System.Windows.Forms.ListBox listPatient;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.TextBox textPrePayType;
		private System.Windows.Forms.TextBox textPrePayDate;
		private System.Windows.Forms.TextBox textPrePaidHere;
		private System.Windows.Forms.TextBox textPrePayAmt;
		private System.Windows.Forms.TextBox textPrePaidElsewhere;
		private OpenDental.ValidDate textDatePay;
		private OpenDental.ValidDate textDateEntry;
		private OpenDental.ValidDouble textAmount;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabProcedure;
		private System.Windows.Forms.TabPage tabAdjustment;
		private UI.Button butAttachAdjust;
		private UI.Button butDetachAdjust;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox textAdjAmt;
		private System.Windows.Forms.TextBox textAdjPrevUsed;
		private System.Windows.Forms.TextBox textAdjPaidHere;
		private System.Windows.Forms.Label labelAdjRemaining;
		private System.Windows.Forms.TextBox textAdjDate;
		private System.Windows.Forms.TextBox textAdjProv;
		private UI.Button butAttachProc;
		private UI.Button butDetachProc;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textProcFee;
		private System.Windows.Forms.TextBox textProcInsPaid;
		private System.Windows.Forms.TextBox textProcInsEst;
		private System.Windows.Forms.TextBox textProcAdj;
		private System.Windows.Forms.TextBox textProcPrevPaid;
		private System.Windows.Forms.TextBox textProcPaidHere;
		private System.Windows.Forms.Label labelProcRemain;
		private System.Windows.Forms.TextBox textProcDate;
		private System.Windows.Forms.TextBox textProcDescription;
		private System.Windows.Forms.TextBox textProcProv;
		private System.Windows.Forms.Label labelProcTooth;
		private System.Windows.Forms.TextBox textProcTooth;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TextBox textProcWriteoff;
	}
}
