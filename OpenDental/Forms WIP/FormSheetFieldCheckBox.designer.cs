namespace OpenDental{
	partial class FormSheetFieldCheckBox {
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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldCheckBox));
			this.label2 = new System.Windows.Forms.Label();
			this.listBoxFields = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.checkRequired = new System.Windows.Forms.CheckBox();
			this.groupRadioMisc = new System.Windows.Forms.GroupBox();
			this.comboRadioGroupNameMisc = new System.Windows.Forms.ComboBox();
			this.textUiLabelMobileMisc = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobileMisc = new System.Windows.Forms.Label();
			this.textUiLabelMobileRadioButtonMisc = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobileRadioButtonMisc = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.listRadio = new System.Windows.Forms.ListBox();
			this.groupRadio = new System.Windows.Forms.GroupBox();
			this.labelAlsoActs = new System.Windows.Forms.Label();
			this.textUiLabelMobile = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobile = new System.Windows.Forms.Label();
			this.labelTabOrder = new System.Windows.Forms.Label();
			this.listMedical = new System.Windows.Forms.ListBox();
			this.labelMedical = new System.Windows.Forms.Label();
			this.radioYes = new System.Windows.Forms.RadioButton();
			this.radioNo = new System.Windows.Forms.RadioButton();
			this.labelRequired = new System.Windows.Forms.Label();
			this.textReportableName = new System.Windows.Forms.TextBox();
			this.labelReportableName = new System.Windows.Forms.Label();
			this.labelMiscInstructions = new System.Windows.Forms.Label();
			this.labelYesNo = new System.Windows.Forms.Label();
			this.textTabOrder = new OpenDental.ValidNum();
			this.butDelete = new OpenDental.UI.Button();
			this.textHeight = new OpenDental.ValidNum();
			this.textWidth = new OpenDental.ValidNum();
			this.textYPos = new OpenDental.ValidNum();
			this.textXPos = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textUiLabelMobileCheckBoxNonMisc = new System.Windows.Forms.TextBox();
			this.labelUiLabelMobileCheckBoxNonMisc = new System.Windows.Forms.Label();
			this.butAddAllergy = new OpenDental.UI.Button();
			this.butAddProblem = new OpenDental.UI.Button();
			this.butAddProc = new OpenDental.UI.Button();
			this.groupRadioMisc.SuspendLayout();
			this.groupRadio.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 16);
			this.label2.TabIndex = 86;
			this.label2.Text = "Field Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFields
			// 
			this.listBoxFields.FormattingEnabled = true;
			this.listBoxFields.Location = new System.Drawing.Point(15, 37);
			this.listBoxFields.Name = "listFields";
			this.listBoxFields.Size = new System.Drawing.Size(142, 511);
			this.listBoxFields.TabIndex = 11;
			this.listBoxFields.SelectedIndexChanged += new System.EventHandler(this.listFields_SelectedIndexChanged);
			this.listBoxFields.DoubleClick += new System.EventHandler(this.listFields_DoubleClick);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(385, 11);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 16);
			this.label5.TabIndex = 90;
			this.label5.Text = "X Pos";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(531, 11);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(61, 16);
			this.label6.TabIndex = 92;
			this.label6.Text = "Y Pos";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(385, 38);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(71, 16);
			this.label7.TabIndex = 94;
			this.label7.Text = "Width";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Location = new System.Drawing.Point(531, 41);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(61, 16);
			this.label8.TabIndex = 96;
			this.label8.Text = "Height";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(30, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 16);
			this.label3.TabIndex = 103;
			this.label3.Text = "Group Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(36, 15);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(280, 33);
			this.label9.TabIndex = 106;
			this.label9.Text = "Use the same Field Name (misc) and the same Group Name for each radio button in a" +
    " group.";
			// 
			// checkRequired
			// 
			this.checkRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRequired.Location = new System.Drawing.Point(359, 205);
			this.checkRequired.Name = "checkRequired";
			this.checkRequired.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkRequired.Size = new System.Drawing.Size(97, 17);
			this.checkRequired.TabIndex = 5;
			this.checkRequired.Text = "Required";
			this.checkRequired.UseVisualStyleBackColor = true;
			// 
			// groupRadioMisc
			// 
			this.groupRadioMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupRadioMisc.Controls.Add(this.comboRadioGroupNameMisc);
			this.groupRadioMisc.Controls.Add(this.textUiLabelMobileMisc);
			this.groupRadioMisc.Controls.Add(this.labelUiLabelMobileMisc);
			this.groupRadioMisc.Controls.Add(this.textUiLabelMobileRadioButtonMisc);
			this.groupRadioMisc.Controls.Add(this.labelUiLabelMobileRadioButtonMisc);
			this.groupRadioMisc.Controls.Add(this.label9);
			this.groupRadioMisc.Controls.Add(this.label3);
			this.groupRadioMisc.Location = new System.Drawing.Point(337, 63);
			this.groupRadioMisc.Name = "groupRadioMisc";
			this.groupRadioMisc.Size = new System.Drawing.Size(322, 129);
			this.groupRadioMisc.TabIndex = 4;
			this.groupRadioMisc.TabStop = false;
			this.groupRadioMisc.Text = "Radio Button";
			this.groupRadioMisc.Visible = false;
			// 
			// comboRadioGroupNameMisc
			// 
			this.comboRadioGroupNameMisc.FormattingEnabled = true;
			this.comboRadioGroupNameMisc.Location = new System.Drawing.Point(119, 49);
			this.comboRadioGroupNameMisc.Name = "comboRadioGroupNameMisc";
			this.comboRadioGroupNameMisc.Size = new System.Drawing.Size(197, 21);
			this.comboRadioGroupNameMisc.TabIndex = 121;
			this.comboRadioGroupNameMisc.SelectedIndexChanged += new System.EventHandler(this.comboRadioGroupNameMisc_SelectedIndexChanged);
			// 
			// textUiLabelMobileMisc
			// 
			this.textUiLabelMobileMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUiLabelMobileMisc.Location = new System.Drawing.Point(119, 76);
			this.textUiLabelMobileMisc.Name = "textUiLabelMobileMisc";
			this.textUiLabelMobileMisc.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobileMisc.TabIndex = 1;
			// 
			// labelUiLabelMobileMisc
			// 
			this.labelUiLabelMobileMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelUiLabelMobileMisc.Location = new System.Drawing.Point(6, 77);
			this.labelUiLabelMobileMisc.Name = "labelUiLabelMobileMisc";
			this.labelUiLabelMobileMisc.Size = new System.Drawing.Size(113, 16);
			this.labelUiLabelMobileMisc.TabIndex = 120;
			this.labelUiLabelMobileMisc.Text = "Mobile Group Caption";
			this.labelUiLabelMobileMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUiLabelMobileRadioButtonMisc
			// 
			this.textUiLabelMobileRadioButtonMisc.Location = new System.Drawing.Point(119, 102);
			this.textUiLabelMobileRadioButtonMisc.Name = "textUiLabelMobileRadioButtonMisc";
			this.textUiLabelMobileRadioButtonMisc.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobileRadioButtonMisc.TabIndex = 2;
			// 
			// labelUiLabelMobileRadioButtonMisc
			// 
			this.labelUiLabelMobileRadioButtonMisc.Location = new System.Drawing.Point(5, 103);
			this.labelUiLabelMobileRadioButtonMisc.Name = "labelUiLabelMobileRadioButtonMisc";
			this.labelUiLabelMobileRadioButtonMisc.Size = new System.Drawing.Size(114, 16);
			this.labelUiLabelMobileRadioButtonMisc.TabIndex = 108;
			this.labelUiLabelMobileRadioButtonMisc.Text = "Mobile Item Caption";
			this.labelUiLabelMobileRadioButtonMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(283, 31);
			this.label1.TabIndex = 87;
			this.label1.Text = "Use the same Field Name for each radio button in a group.  But set a different Ra" +
    "dio Button Value for each.";
			// 
			// listRadio
			// 
			this.listRadio.FormattingEnabled = true;
			this.listRadio.Location = new System.Drawing.Point(119, 90);
			this.listRadio.Name = "listRadio";
			this.listRadio.Size = new System.Drawing.Size(197, 95);
			this.listRadio.TabIndex = 0;
			this.listRadio.Click += new System.EventHandler(this.listRadio_Click);
			// 
			// groupRadio
			// 
			this.groupRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupRadio.Controls.Add(this.labelAlsoActs);
			this.groupRadio.Controls.Add(this.textUiLabelMobile);
			this.groupRadio.Controls.Add(this.labelUiLabelMobile);
			this.groupRadio.Controls.Add(this.listRadio);
			this.groupRadio.Controls.Add(this.label1);
			this.groupRadio.Location = new System.Drawing.Point(337, 316);
			this.groupRadio.Name = "groupRadio";
			this.groupRadio.Size = new System.Drawing.Size(322, 191);
			this.groupRadio.TabIndex = 8;
			this.groupRadio.TabStop = false;
			this.groupRadio.Text = "Radio Button Value";
			this.groupRadio.Visible = false;
			// 
			// labelAlsoActs
			// 
			this.labelAlsoActs.Location = new System.Drawing.Point(11, 90);
			this.labelAlsoActs.Name = "labelAlsoActs";
			this.labelAlsoActs.Size = new System.Drawing.Size(108, 41);
			this.labelAlsoActs.TabIndex = 123;
			this.labelAlsoActs.Text = "Also acts as the\r\nMobile Item Caption";
			// 
			// textUiLabelMobile
			// 
			this.textUiLabelMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUiLabelMobile.Location = new System.Drawing.Point(119, 19);
			this.textUiLabelMobile.Name = "textUiLabelMobile";
			this.textUiLabelMobile.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobile.TabIndex = 121;
			// 
			// labelUiLabelMobile
			// 
			this.labelUiLabelMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelUiLabelMobile.Location = new System.Drawing.Point(6, 20);
			this.labelUiLabelMobile.Name = "labelUiLabelMobile";
			this.labelUiLabelMobile.Size = new System.Drawing.Size(113, 16);
			this.labelUiLabelMobile.TabIndex = 122;
			this.labelUiLabelMobile.Text = "Mobile Group Caption";
			this.labelUiLabelMobile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTabOrder
			// 
			this.labelTabOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTabOrder.Location = new System.Drawing.Point(385, 614);
			this.labelTabOrder.Name = "labelTabOrder";
			this.labelTabOrder.Size = new System.Drawing.Size(71, 16);
			this.labelTabOrder.TabIndex = 108;
			this.labelTabOrder.Text = "Tab Order";
			this.labelTabOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listMedical
			// 
			this.listMedical.FormattingEnabled = true;
			this.listMedical.Location = new System.Drawing.Point(186, 37);
			this.listMedical.Name = "listMedical";
			this.listMedical.Size = new System.Drawing.Size(142, 511);
			this.listMedical.TabIndex = 12;
			this.listMedical.Visible = false;
			this.listMedical.DoubleClick += new System.EventHandler(this.listMedical_DoubleClick);
			// 
			// labelMedical
			// 
			this.labelMedical.Location = new System.Drawing.Point(183, 18);
			this.labelMedical.Name = "labelMedical";
			this.labelMedical.Size = new System.Drawing.Size(108, 16);
			this.labelMedical.TabIndex = 111;
			this.labelMedical.Text = "labelMedical";
			this.labelMedical.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelMedical.Visible = false;
			// 
			// radioYes
			// 
			this.radioYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioYes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioYes.Location = new System.Drawing.Point(395, 262);
			this.radioYes.Name = "radioYes";
			this.radioYes.Size = new System.Drawing.Size(61, 17);
			this.radioYes.TabIndex = 6;
			this.radioYes.TabStop = true;
			this.radioYes.Text = "Yes";
			this.radioYes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioYes.UseVisualStyleBackColor = true;
			this.radioYes.Visible = false;
			this.radioYes.Click += new System.EventHandler(this.radioYes_Click);
			// 
			// radioNo
			// 
			this.radioNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioNo.Location = new System.Drawing.Point(395, 280);
			this.radioNo.Name = "radioNo";
			this.radioNo.Size = new System.Drawing.Size(61, 17);
			this.radioNo.TabIndex = 7;
			this.radioNo.TabStop = true;
			this.radioNo.Text = "No";
			this.radioNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioNo.UseVisualStyleBackColor = true;
			this.radioNo.Visible = false;
			this.radioNo.Click += new System.EventHandler(this.radioNo_Click);
			// 
			// labelRequired
			// 
			this.labelRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRequired.Location = new System.Drawing.Point(465, 195);
			this.labelRequired.Name = "labelRequired";
			this.labelRequired.Size = new System.Drawing.Size(188, 40);
			this.labelRequired.TabIndex = 116;
			this.labelRequired.Text = "Radio buttons in a radio button group must all be marked required or all be marke" +
    "d not required.";
			// 
			// textReportableName
			// 
			this.textReportableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textReportableName.Location = new System.Drawing.Point(456, 587);
			this.textReportableName.Name = "textReportableName";
			this.textReportableName.Size = new System.Drawing.Size(197, 20);
			this.textReportableName.TabIndex = 9;
			// 
			// labelReportableName
			// 
			this.labelReportableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelReportableName.Location = new System.Drawing.Point(334, 588);
			this.labelReportableName.Name = "labelReportableName";
			this.labelReportableName.Size = new System.Drawing.Size(122, 16);
			this.labelReportableName.TabIndex = 108;
			this.labelReportableName.Text = "Reportable Name";
			this.labelReportableName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMiscInstructions
			// 
			this.labelMiscInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMiscInstructions.Location = new System.Drawing.Point(370, 550);
			this.labelMiscInstructions.Name = "labelMiscInstructions";
			this.labelMiscInstructions.Size = new System.Drawing.Size(289, 32);
			this.labelMiscInstructions.TabIndex = 117;
			this.labelMiscInstructions.Text = "To make misc radio buttons reportable, set a different Reportable Name for each b" +
    "utton in the group.";
			this.labelMiscInstructions.Visible = false;
			// 
			// labelYesNo
			// 
			this.labelYesNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelYesNo.Location = new System.Drawing.Point(462, 245);
			this.labelYesNo.Name = "labelYesNo";
			this.labelYesNo.Size = new System.Drawing.Size(197, 68);
			this.labelYesNo.TabIndex = 118;
			this.labelYesNo.Text = "First create a checkbox with the \"Yes\" value, then create a second checkbox for t" +
    "he same category with the \"No\" value.  The two checkboxes will act as a radio bu" +
    "tton group.";
			this.labelYesNo.Visible = false;
			// 
			// textTabOrder
			// 
			this.textTabOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTabOrder.DoAutoSave = false;
			this.textTabOrder.Location = new System.Drawing.Point(456, 613);
			this.textTabOrder.MaxVal = 2000;
			this.textTabOrder.MinVal = -100;
			this.textTabOrder.Name = "textTabOrder";
			this.textTabOrder.Size = new System.Drawing.Size(69, 20);
			this.textTabOrder.TabIndex = 10;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(16, 639);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 13;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textHeight
			// 
			this.textHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textHeight.DoAutoSave = false;
			this.textHeight.Location = new System.Drawing.Point(592, 37);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = 1;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 3;
			// 
			// textWidth
			// 
			this.textWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textWidth.DoAutoSave = false;
			this.textWidth.Location = new System.Drawing.Point(456, 37);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 2;
			// 
			// textYPos
			// 
			this.textYPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textYPos.DoAutoSave = false;
			this.textYPos.Location = new System.Drawing.Point(592, 9);
			this.textYPos.MaxVal = 2000;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 1;
			// 
			// textXPos
			// 
			this.textXPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textXPos.DoAutoSave = false;
			this.textXPos.Location = new System.Drawing.Point(456, 9);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 0;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(503, 639);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 14;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(584, 639);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 15;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textUiLabelMobileCheckBoxNonMisc
			// 
			this.textUiLabelMobileCheckBoxNonMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUiLabelMobileCheckBoxNonMisc.Location = new System.Drawing.Point(456, 513);
			this.textUiLabelMobileCheckBoxNonMisc.Name = "textUiLabelMobileCheckBoxNonMisc";
			this.textUiLabelMobileCheckBoxNonMisc.Size = new System.Drawing.Size(197, 20);
			this.textUiLabelMobileCheckBoxNonMisc.TabIndex = 119;
			// 
			// labelUiLabelMobileCheckBoxNonMisc
			// 
			this.labelUiLabelMobileCheckBoxNonMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelUiLabelMobileCheckBoxNonMisc.Location = new System.Drawing.Point(334, 514);
			this.labelUiLabelMobileCheckBoxNonMisc.Name = "labelUiLabelMobileCheckBoxNonMisc";
			this.labelUiLabelMobileCheckBoxNonMisc.Size = new System.Drawing.Size(122, 16);
			this.labelUiLabelMobileCheckBoxNonMisc.TabIndex = 120;
			this.labelUiLabelMobileCheckBoxNonMisc.Text = "Mobile Item Caption";
			this.labelUiLabelMobileCheckBoxNonMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAddAllergy
			// 
			this.butAddAllergy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddAllergy.Autosize = true;
			this.butAddAllergy.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddAllergy.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddAllergy.CornerRadius = 4F;
			this.butAddAllergy.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAllergy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAllergy.Location = new System.Drawing.Point(186, 550);
			this.butAddAllergy.Name = "butAddAllergy";
			this.butAddAllergy.Size = new System.Drawing.Size(105, 24);
			this.butAddAllergy.TabIndex = 121;
			this.butAddAllergy.Text = "Add Allergy";
			this.butAddAllergy.Visible = false;
			this.butAddAllergy.Click += new System.EventHandler(this.butAddAllergy_Click);
			// 
			// butAddProblem
			// 
			this.butAddProblem.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProblem.Autosize = true;
			this.butAddProblem.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddProblem.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddProblem.CornerRadius = 4F;
			this.butAddProblem.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProblem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProblem.Location = new System.Drawing.Point(186, 577);
			this.butAddProblem.Name = "butAddProblem";
			this.butAddProblem.Size = new System.Drawing.Size(105, 24);
			this.butAddProblem.TabIndex = 122;
			this.butAddProblem.Text = "Add Problem";
			this.butAddProblem.Visible = false;
			this.butAddProblem.Click += new System.EventHandler(this.butAddProblem_Click);
			// 
			// butAddProc
			// 
			this.butAddProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProc.Autosize = false;
			this.butAddProc.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddProc.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddProc.CornerRadius = 4F;
			this.butAddProc.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProc.Location = new System.Drawing.Point(15, 554);
			this.butAddProc.Name = "butAddProc";
			this.butAddProc.Size = new System.Drawing.Size(124, 24);
			this.butAddProc.TabIndex = 123;
			this.butAddProc.Text = "Add Procedure";
			this.butAddProc.Visible = false;
			this.butAddProc.Click += new System.EventHandler(this.butAddProc_Click);
			// 
			// FormSheetFieldCheckBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(675, 677);
			this.Controls.Add(this.butAddProc);
			this.Controls.Add(this.butAddProblem);
			this.Controls.Add(this.butAddAllergy);
			this.Controls.Add(this.textUiLabelMobileCheckBoxNonMisc);
			this.Controls.Add(this.labelUiLabelMobileCheckBoxNonMisc);
			this.Controls.Add(this.labelYesNo);
			this.Controls.Add(this.listMedical);
			this.Controls.Add(this.labelMiscInstructions);
			this.Controls.Add(this.textReportableName);
			this.Controls.Add(this.labelReportableName);
			this.Controls.Add(this.labelRequired);
			this.Controls.Add(this.radioNo);
			this.Controls.Add(this.radioYes);
			this.Controls.Add(this.labelMedical);
			this.Controls.Add(this.textTabOrder);
			this.Controls.Add(this.labelTabOrder);
			this.Controls.Add(this.checkRequired);
			this.Controls.Add(this.groupRadioMisc);
			this.Controls.Add(this.groupRadio);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listBoxFields);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(691, 667);
			this.Name = "FormSheetFieldCheckBox";
			this.Text = "Edit CheckBox";
			this.Load += new System.EventHandler(this.FormSheetFieldCheckBox_Load);
			this.Shown += new System.EventHandler(this.FormSheetFieldCheckBox_Shown);
			this.groupRadioMisc.ResumeLayout(false);
			this.groupRadioMisc.PerformLayout();
			this.groupRadio.ResumeLayout(false);
			this.groupRadio.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listBoxFields;
		private System.Windows.Forms.Label label5;
		private ValidNum textXPos;
		private ValidNum textYPos;
		private System.Windows.Forms.Label label6;
		private ValidNum textWidth;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private ValidNum textHeight;
		private UI.Button butDelete;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkRequired;
		private System.Windows.Forms.GroupBox groupRadioMisc;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listRadio;
		private System.Windows.Forms.GroupBox groupRadio;
		private ValidNum textTabOrder;
		private System.Windows.Forms.Label labelTabOrder;
		private System.Windows.Forms.ListBox listMedical;
		private System.Windows.Forms.Label labelMedical;
		private System.Windows.Forms.RadioButton radioYes;
		private System.Windows.Forms.RadioButton radioNo;
		private System.Windows.Forms.Label labelRequired;
		private System.Windows.Forms.TextBox textReportableName;
		private System.Windows.Forms.Label labelReportableName;
		private System.Windows.Forms.Label labelMiscInstructions;
		private System.Windows.Forms.Label labelYesNo;
		private System.Windows.Forms.TextBox textUiLabelMobileMisc;
		private System.Windows.Forms.Label labelUiLabelMobileMisc;
		private System.Windows.Forms.TextBox textUiLabelMobileRadioButtonMisc;
		private System.Windows.Forms.Label labelUiLabelMobileRadioButtonMisc;
		private System.Windows.Forms.TextBox textUiLabelMobile;
		private System.Windows.Forms.Label labelUiLabelMobile;
		private System.Windows.Forms.Label labelAlsoActs;
		private System.Windows.Forms.ComboBox comboRadioGroupNameMisc;
		private System.Windows.Forms.TextBox textUiLabelMobileCheckBoxNonMisc;
		private System.Windows.Forms.Label labelUiLabelMobileCheckBoxNonMisc;
		private UI.Button butAddAllergy;
		private UI.Button butAddProblem;
		private UI.Button butAddProc;
	}
}