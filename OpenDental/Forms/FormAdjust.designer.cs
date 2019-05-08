namespace OpenDental
{
    partial class FormAdjust
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjust));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelAdditions = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textAdjDate = new OpenDental.ValidDate();
            this.labelSubtractions = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.textAmount = new OpenDental.ValidDouble();
            this.listTypePos = new System.Windows.Forms.ListBox();
            this.listTypeNeg = new System.Windows.Forms.ListBox();
            this.textProcDate = new OpenDental.ValidDate();
            this.label7 = new System.Windows.Forms.Label();
            this.textDateEntry = new OpenDental.ValidDate();
            this.label8 = new System.Windows.Forms.Label();
            this.butPickProv = new System.Windows.Forms.Button();
            this.comboProv = new System.Windows.Forms.ComboBox();
            this.comboClinic = new System.Windows.Forms.ComboBox();
            this.labelClinic = new System.Windows.Forms.Label();
            this.textNote = new OpenDental.ODtextBox();
            this.procedureGroupBox = new System.Windows.Forms.GroupBox();
            this.labelProcDisabled = new System.Windows.Forms.Label();
            this.butEditAnyway = new System.Windows.Forms.Button();
            this.textProcWriteoff = new System.Windows.Forms.TextBox();
            this.labelEditAnyway = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textProcTooth = new System.Windows.Forms.TextBox();
            this.labelProcTooth = new System.Windows.Forms.Label();
            this.textProcProv = new System.Windows.Forms.TextBox();
            this.textProcDescription = new System.Windows.Forms.TextBox();
            this.textProcDate2 = new System.Windows.Forms.TextBox();
            this.labelProcRemain = new System.Windows.Forms.Label();
            this.textProcAdjCur = new System.Windows.Forms.TextBox();
            this.textProcPatPaid = new System.Windows.Forms.TextBox();
            this.textProcAdj = new System.Windows.Forms.TextBox();
            this.textProcInsEst = new System.Windows.Forms.TextBox();
            this.textProcInsPaid = new System.Windows.Forms.TextBox();
            this.textProcFee = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.butDetachProc = new System.Windows.Forms.Button();
            this.butAttachProc = new System.Windows.Forms.Button();
            this.procedureGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Adjustment Date";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 441);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Note";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Amount";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAdditions
            // 
            this.labelAdditions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdditions.AutoSize = true;
            this.labelAdditions.Location = new System.Drawing.Point(352, 16);
            this.labelAdditions.Name = "labelAdditions";
            this.labelAdditions.Size = new System.Drawing.Size(58, 15);
            this.labelAdditions.TabIndex = 13;
            this.labelAdditions.Text = "Additions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Provider";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(535, 548);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 21;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.butOK_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(651, 548);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 22;
            this.cancelButton.Text = "&Cancel";
            // 
            // textAdjDate
            // 
            this.textAdjDate.Location = new System.Drawing.Point(130, 63);
            this.textAdjDate.Name = "textAdjDate";
            this.textAdjDate.Size = new System.Drawing.Size(100, 23);
            this.textAdjDate.TabIndex = 3;
            // 
            // labelSubtractions
            // 
            this.labelSubtractions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSubtractions.AutoSize = true;
            this.labelSubtractions.Location = new System.Drawing.Point(558, 16);
            this.labelSubtractions.Name = "labelSubtractions";
            this.labelSubtractions.Size = new System.Drawing.Size(73, 15);
            this.labelSubtractions.TabIndex = 15;
            this.labelSubtractions.Text = "Subtractions";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 548);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 20;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // textAmount
            // 
            this.textAmount.Location = new System.Drawing.Point(130, 121);
            this.textAmount.MaxVal = 100000000D;
            this.textAmount.MinVal = -100000000D;
            this.textAmount.Name = "textAmount";
            this.textAmount.Size = new System.Drawing.Size(80, 23);
            this.textAmount.TabIndex = 7;
            this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
            // 
            // listTypePos
            // 
            this.listTypePos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listTypePos.IntegralHeight = false;
            this.listTypePos.ItemHeight = 15;
            this.listTypePos.Location = new System.Drawing.Point(355, 34);
            this.listTypePos.Name = "listTypePos";
            this.listTypePos.Size = new System.Drawing.Size(200, 160);
            this.listTypePos.TabIndex = 14;
            this.listTypePos.SelectedIndexChanged += new System.EventHandler(this.listTypePos_SelectedIndexChanged);
            // 
            // listTypeNeg
            // 
            this.listTypeNeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listTypeNeg.IntegralHeight = false;
            this.listTypeNeg.ItemHeight = 15;
            this.listTypeNeg.Location = new System.Drawing.Point(561, 34);
            this.listTypeNeg.Name = "listTypeNeg";
            this.listTypeNeg.Size = new System.Drawing.Size(200, 160);
            this.listTypeNeg.TabIndex = 16;
            this.listTypeNeg.SelectedIndexChanged += new System.EventHandler(this.listTypeNeg_SelectedIndexChanged);
            // 
            // textProcDate
            // 
            this.textProcDate.Location = new System.Drawing.Point(130, 92);
            this.textProcDate.Name = "textProcDate";
            this.textProcDate.Size = new System.Drawing.Size(100, 23);
            this.textProcDate.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Procedure Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textDateEntry
            // 
            this.textDateEntry.Location = new System.Drawing.Point(130, 34);
            this.textDateEntry.Name = "textDateEntry";
            this.textDateEntry.ReadOnly = true;
            this.textDateEntry.Size = new System.Drawing.Size(100, 23);
            this.textDateEntry.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Entry Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // butPickProv
            // 
            this.butPickProv.Location = new System.Drawing.Point(300, 149);
            this.butPickProv.Name = "butPickProv";
            this.butPickProv.Size = new System.Drawing.Size(30, 25);
            this.butPickProv.TabIndex = 10;
            this.butPickProv.Text = "...";
            this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
            // 
            // comboProv
            // 
            this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProv.Location = new System.Drawing.Point(130, 150);
            this.comboProv.MaxDropDownItems = 30;
            this.comboProv.Name = "comboProv";
            this.comboProv.Size = new System.Drawing.Size(169, 23);
            this.comboProv.TabIndex = 9;
            this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
            // 
            // comboClinic
            // 
            this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClinic.Location = new System.Drawing.Point(130, 179);
            this.comboClinic.MaxDropDownItems = 30;
            this.comboClinic.Name = "comboClinic";
            this.comboClinic.Size = new System.Drawing.Size(200, 23);
            this.comboClinic.TabIndex = 12;
            this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
            // 
            // labelClinic
            // 
            this.labelClinic.AutoSize = true;
            this.labelClinic.Location = new System.Drawing.Point(87, 182);
            this.labelClinic.Name = "labelClinic";
            this.labelClinic.Size = new System.Drawing.Size(37, 15);
            this.labelClinic.TabIndex = 11;
            this.labelClinic.Text = "Clinic";
            this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textNote
            // 
            this.textNote.AcceptsTab = true;
            this.textNote.BackColor = System.Drawing.SystemColors.Window;
            this.textNote.DetectLinksEnabled = false;
            this.textNote.DetectUrls = false;
            this.textNote.Location = new System.Drawing.Point(130, 438);
            this.textNote.Name = "textNote";
            this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Adjustment;
            this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textNote.Size = new System.Drawing.Size(400, 80);
            this.textNote.TabIndex = 19;
            this.textNote.Text = "";
            // 
            // procedureGroupBox
            // 
            this.procedureGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.procedureGroupBox.Controls.Add(this.labelProcDisabled);
            this.procedureGroupBox.Controls.Add(this.butEditAnyway);
            this.procedureGroupBox.Controls.Add(this.textProcWriteoff);
            this.procedureGroupBox.Controls.Add(this.labelEditAnyway);
            this.procedureGroupBox.Controls.Add(this.label16);
            this.procedureGroupBox.Controls.Add(this.textProcTooth);
            this.procedureGroupBox.Controls.Add(this.labelProcTooth);
            this.procedureGroupBox.Controls.Add(this.textProcProv);
            this.procedureGroupBox.Controls.Add(this.textProcDescription);
            this.procedureGroupBox.Controls.Add(this.textProcDate2);
            this.procedureGroupBox.Controls.Add(this.labelProcRemain);
            this.procedureGroupBox.Controls.Add(this.textProcAdjCur);
            this.procedureGroupBox.Controls.Add(this.textProcPatPaid);
            this.procedureGroupBox.Controls.Add(this.textProcAdj);
            this.procedureGroupBox.Controls.Add(this.textProcInsEst);
            this.procedureGroupBox.Controls.Add(this.textProcInsPaid);
            this.procedureGroupBox.Controls.Add(this.textProcFee);
            this.procedureGroupBox.Controls.Add(this.label13);
            this.procedureGroupBox.Controls.Add(this.label12);
            this.procedureGroupBox.Controls.Add(this.label11);
            this.procedureGroupBox.Controls.Add(this.label10);
            this.procedureGroupBox.Controls.Add(this.label9);
            this.procedureGroupBox.Controls.Add(this.label14);
            this.procedureGroupBox.Controls.Add(this.label15);
            this.procedureGroupBox.Controls.Add(this.label17);
            this.procedureGroupBox.Controls.Add(this.label18);
            this.procedureGroupBox.Controls.Add(this.label19);
            this.procedureGroupBox.Controls.Add(this.butDetachProc);
            this.procedureGroupBox.Controls.Add(this.butAttachProc);
            this.procedureGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.procedureGroupBox.Location = new System.Drawing.Point(130, 208);
            this.procedureGroupBox.Name = "procedureGroupBox";
            this.procedureGroupBox.Size = new System.Drawing.Size(631, 224);
            this.procedureGroupBox.TabIndex = 17;
            this.procedureGroupBox.TabStop = false;
            this.procedureGroupBox.Text = "Procedure";
            // 
            // labelProcDisabled
            // 
            this.labelProcDisabled.ForeColor = System.Drawing.Color.Firebrick;
            this.labelProcDisabled.Location = new System.Drawing.Point(206, 19);
            this.labelProcDisabled.Name = "labelProcDisabled";
            this.labelProcDisabled.Size = new System.Drawing.Size(270, 50);
            this.labelProcDisabled.TabIndex = 2;
            this.labelProcDisabled.Text = "There is at least one payment for this adjustment.  You cannot attach this adjust" +
    "ment to a procedure until the payment(s) are deleted.";
            this.labelProcDisabled.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelProcDisabled.Visible = false;
            // 
            // butEditAnyway
            // 
            this.butEditAnyway.Image = global::OpenDental.Properties.Resources.IconEdit;
            this.butEditAnyway.Location = new System.Drawing.Point(385, 141);
            this.butEditAnyway.Name = "butEditAnyway";
            this.butEditAnyway.Size = new System.Drawing.Size(30, 25);
            this.butEditAnyway.TabIndex = 11;
            this.butEditAnyway.Visible = false;
            this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
            // 
            // textProcWriteoff
            // 
            this.textProcWriteoff.Location = new System.Drawing.Point(545, 46);
            this.textProcWriteoff.Name = "textProcWriteoff";
            this.textProcWriteoff.ReadOnly = true;
            this.textProcWriteoff.Size = new System.Drawing.Size(80, 23);
            this.textProcWriteoff.TabIndex = 16;
            this.textProcWriteoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelEditAnyway
            // 
            this.labelEditAnyway.AutoSize = true;
            this.labelEditAnyway.Location = new System.Drawing.Point(9, 200);
            this.labelEditAnyway.Name = "labelEditAnyway";
            this.labelEditAnyway.Size = new System.Drawing.Size(367, 15);
            this.labelEditAnyway.TabIndex = 12;
            this.labelEditAnyway.Text = "This adjustment is attached to a procedure and should not be edited";
            this.labelEditAnyway.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelEditAnyway.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(484, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 15);
            this.label16.TabIndex = 15;
            this.label16.Text = "Writeoffs";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textProcTooth
            // 
            this.textProcTooth.Location = new System.Drawing.Point(124, 118);
            this.textProcTooth.Name = "textProcTooth";
            this.textProcTooth.ReadOnly = true;
            this.textProcTooth.Size = new System.Drawing.Size(50, 23);
            this.textProcTooth.TabIndex = 8;
            // 
            // labelProcTooth
            // 
            this.labelProcTooth.AutoSize = true;
            this.labelProcTooth.Location = new System.Drawing.Point(80, 121);
            this.labelProcTooth.Name = "labelProcTooth";
            this.labelProcTooth.Size = new System.Drawing.Size(38, 15);
            this.labelProcTooth.TabIndex = 7;
            this.labelProcTooth.Text = "Tooth";
            this.labelProcTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textProcProv
            // 
            this.textProcProv.Location = new System.Drawing.Point(124, 94);
            this.textProcProv.Name = "textProcProv";
            this.textProcProv.ReadOnly = true;
            this.textProcProv.Size = new System.Drawing.Size(100, 23);
            this.textProcProv.TabIndex = 6;
            // 
            // textProcDescription
            // 
            this.textProcDescription.Location = new System.Drawing.Point(124, 142);
            this.textProcDescription.Name = "textProcDescription";
            this.textProcDescription.ReadOnly = true;
            this.textProcDescription.Size = new System.Drawing.Size(260, 23);
            this.textProcDescription.TabIndex = 10;
            // 
            // textProcDate2
            // 
            this.textProcDate2.Location = new System.Drawing.Point(124, 70);
            this.textProcDate2.Name = "textProcDate2";
            this.textProcDate2.ReadOnly = true;
            this.textProcDate2.Size = new System.Drawing.Size(100, 23);
            this.textProcDate2.TabIndex = 4;
            // 
            // labelProcRemain
            // 
            this.labelProcRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcRemain.Location = new System.Drawing.Point(545, 192);
            this.labelProcRemain.Name = "labelProcRemain";
            this.labelProcRemain.Size = new System.Drawing.Size(80, 23);
            this.labelProcRemain.TabIndex = 28;
            this.labelProcRemain.Text = "$0.00";
            this.labelProcRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textProcAdjCur
            // 
            this.textProcAdjCur.Location = new System.Drawing.Point(545, 166);
            this.textProcAdjCur.Name = "textProcAdjCur";
            this.textProcAdjCur.ReadOnly = true;
            this.textProcAdjCur.Size = new System.Drawing.Size(80, 23);
            this.textProcAdjCur.TabIndex = 26;
            this.textProcAdjCur.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textProcPatPaid
            // 
            this.textProcPatPaid.Location = new System.Drawing.Point(545, 142);
            this.textProcPatPaid.Name = "textProcPatPaid";
            this.textProcPatPaid.ReadOnly = true;
            this.textProcPatPaid.Size = new System.Drawing.Size(80, 23);
            this.textProcPatPaid.TabIndex = 24;
            this.textProcPatPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textProcAdj
            // 
            this.textProcAdj.Location = new System.Drawing.Point(545, 118);
            this.textProcAdj.Name = "textProcAdj";
            this.textProcAdj.ReadOnly = true;
            this.textProcAdj.Size = new System.Drawing.Size(80, 23);
            this.textProcAdj.TabIndex = 22;
            this.textProcAdj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textProcInsEst
            // 
            this.textProcInsEst.Location = new System.Drawing.Point(545, 94);
            this.textProcInsEst.Name = "textProcInsEst";
            this.textProcInsEst.ReadOnly = true;
            this.textProcInsEst.Size = new System.Drawing.Size(80, 23);
            this.textProcInsEst.TabIndex = 20;
            this.textProcInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textProcInsPaid
            // 
            this.textProcInsPaid.Location = new System.Drawing.Point(545, 70);
            this.textProcInsPaid.Name = "textProcInsPaid";
            this.textProcInsPaid.ReadOnly = true;
            this.textProcInsPaid.Size = new System.Drawing.Size(80, 23);
            this.textProcInsPaid.TabIndex = 18;
            this.textProcInsPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textProcFee
            // 
            this.textProcFee.Location = new System.Drawing.Point(545, 22);
            this.textProcFee.Name = "textProcFee";
            this.textProcFee.ReadOnly = true;
            this.textProcFee.Size = new System.Drawing.Size(80, 23);
            this.textProcFee.TabIndex = 14;
            this.textProcFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(445, 169);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 15);
            this.label13.TabIndex = 25;
            this.label13.Text = "This Adjustment";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(475, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 15);
            this.label12.TabIndex = 27;
            this.label12.Text = "Remaining";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(469, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 15);
            this.label11.TabIndex = 23;
            this.label11.Text = "Patient Paid";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(465, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 15);
            this.label10.TabIndex = 21;
            this.label10.Text = "Adjustments";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(499, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 15);
            this.label9.TabIndex = 19;
            this.label9.Text = "Ins Est";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(491, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 15);
            this.label14.TabIndex = 17;
            this.label14.Text = "Ins Paid";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(514, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 15);
            this.label15.TabIndex = 13;
            this.label15.Text = "Fee";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(67, 97);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 15);
            this.label17.TabIndex = 5;
            this.label17.Text = "Provider";
            this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(51, 145);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 15);
            this.label18.TabIndex = 9;
            this.label18.Text = "Description";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(87, 73);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 15);
            this.label19.TabIndex = 3;
            this.label19.Text = "Date";
            this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // butDetachProc
            // 
            this.butDetachProc.Location = new System.Drawing.Point(92, 22);
            this.butDetachProc.Name = "butDetachProc";
            this.butDetachProc.Size = new System.Drawing.Size(80, 25);
            this.butDetachProc.TabIndex = 1;
            this.butDetachProc.Text = "Detach";
            this.butDetachProc.Click += new System.EventHandler(this.butDetachProc_Click);
            // 
            // butAttachProc
            // 
            this.butAttachProc.Location = new System.Drawing.Point(6, 22);
            this.butAttachProc.Name = "butAttachProc";
            this.butAttachProc.Size = new System.Drawing.Size(80, 25);
            this.butAttachProc.TabIndex = 0;
            this.butAttachProc.Text = "Attach";
            this.butAttachProc.Click += new System.EventHandler(this.butAttachProc_Click);
            // 
            // FormAdjust
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(774, 591);
            this.Controls.Add(this.procedureGroupBox);
            this.Controls.Add(this.butPickProv);
            this.Controls.Add(this.comboProv);
            this.Controls.Add(this.comboClinic);
            this.Controls.Add(this.labelClinic);
            this.Controls.Add(this.textDateEntry);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textProcDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textNote);
            this.Controls.Add(this.listTypeNeg);
            this.Controls.Add(this.listTypePos);
            this.Controls.Add(this.textAmount);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.labelSubtractions);
            this.Controls.Add(this.textAdjDate);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelAdditions);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAdjust";
            this.ShowInTaskbar = false;
            this.Text = "Edit Adjustment";
            this.Load += new System.EventHandler(this.FormAdjust_Load);
            this.procedureGroupBox.ResumeLayout(false);
            this.procedureGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelAdditions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelSubtractions;
        private System.Windows.Forms.Button deleteButton;
        private OpenDental.ValidDouble textAmount;
        private System.Windows.Forms.ListBox listTypePos;
        private System.Windows.Forms.ListBox listTypeNeg;
        private OpenDental.ODtextBox textNote;
        private OpenDental.ValidDate textProcDate;
        private System.Windows.Forms.Label label7;
        private OpenDental.ValidDate textAdjDate;
        private OpenDental.ValidDate textDateEntry;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button butPickProv;
        private System.Windows.Forms.ComboBox comboProv;
        private System.Windows.Forms.ComboBox comboClinic;
        private System.Windows.Forms.Label labelClinic;
        private System.Windows.Forms.GroupBox procedureGroupBox;
        private System.Windows.Forms.TextBox textProcWriteoff;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textProcTooth;
        private System.Windows.Forms.Label labelProcTooth;
        private System.Windows.Forms.TextBox textProcProv;
        private System.Windows.Forms.TextBox textProcDescription;
        private System.Windows.Forms.TextBox textProcDate2;
        private System.Windows.Forms.Label labelProcRemain;
        private System.Windows.Forms.TextBox textProcAdjCur;
        private System.Windows.Forms.TextBox textProcPatPaid;
        private System.Windows.Forms.TextBox textProcAdj;
        private System.Windows.Forms.TextBox textProcInsEst;
        private System.Windows.Forms.TextBox textProcInsPaid;
        private System.Windows.Forms.TextBox textProcFee;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button butDetachProc;
        private System.Windows.Forms.Button butAttachProc;
        private System.Windows.Forms.Button butEditAnyway;
        private System.Windows.Forms.Label labelEditAnyway;
        private System.Windows.Forms.Label labelProcDisabled;
    }
}