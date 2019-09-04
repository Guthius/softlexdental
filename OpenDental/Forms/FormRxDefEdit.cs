using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormRxDefEdit : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textDrug;
		private System.Windows.Forms.TextBox textNotes;
		private System.Windows.Forms.TextBox textRefills;
		private System.Windows.Forms.TextBox textDisp;
		private System.Windows.Forms.TextBox textSig;
		private System.ComponentModel.Container components = null;
		private Label label2;
		private ListBox listAlerts;
		private OpenDental.UI.Button butDelete;
		private Label label7;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private RxDef RxDefCur;
		private CheckBox checkControlled;
		private UI.Button butAddAllergy;
		private Label labelRxNorm;
		private UI.Button butRxNormSelect;
		private TextBox textRxCui;
		private UI.Button butAddProblem;
		private UI.Button butAddMedication;
		private CheckBox checkProcRequired;
		private ODtextBox textPatInstruction;
		private Label labelPatInstruct;
		private List<RxAlert> RxAlertList;

		///<summary>Must have already saved it to db so that we have a RxDefNum to work with.</summary>
		public FormRxDefEdit(RxDef rxDefCur){
			InitializeComponent();// Required for Windows Form Designer support
			
			RxDefCur=rxDefCur.Copy();
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRxDefEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textDrug = new System.Windows.Forms.TextBox();
			this.textNotes = new System.Windows.Forms.TextBox();
			this.textRefills = new System.Windows.Forms.TextBox();
			this.textDisp = new System.Windows.Forms.TextBox();
			this.textSig = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.listAlerts = new System.Windows.Forms.ListBox();
			this.butDelete = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.checkControlled = new System.Windows.Forms.CheckBox();
			this.butAddAllergy = new OpenDental.UI.Button();
			this.labelRxNorm = new System.Windows.Forms.Label();
			this.butRxNormSelect = new OpenDental.UI.Button();
			this.textRxCui = new System.Windows.Forms.TextBox();
			this.butAddProblem = new OpenDental.UI.Button();
			this.butAddMedication = new OpenDental.UI.Button();
			this.checkProcRequired = new System.Windows.Forms.CheckBox();
			this.textPatInstruction = new OpenDental.ODtextBox();
			this.labelPatInstruct = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(52, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "Drug";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(20, 197);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(114, 37);
			this.label3.TabIndex = 2;
			this.label3.Text = "Notes (will not show on printout)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(46, 154);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 14);
			this.label4.TabIndex = 3;
			this.label4.Text = "Refills";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(46, 133);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 14);
			this.label5.TabIndex = 4;
			this.label5.Text = "Disp";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(56, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(78, 14);
			this.label6.TabIndex = 5;
			this.label6.Text = "Sig";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDrug
			// 
			this.textDrug.Location = new System.Drawing.Point(134, 24);
			this.textDrug.Name = "textDrug";
			this.textDrug.Size = new System.Drawing.Size(254, 20);
			this.textDrug.TabIndex = 0;
			// 
			// textNotes
			// 
			this.textNotes.AcceptsReturn = true;
			this.textNotes.Location = new System.Drawing.Point(134, 193);
			this.textNotes.Multiline = true;
			this.textNotes.Name = "textNotes";
			this.textNotes.Size = new System.Drawing.Size(386, 92);
			this.textNotes.TabIndex = 7;
			// 
			// textRefills
			// 
			this.textRefills.Location = new System.Drawing.Point(134, 150);
			this.textRefills.Name = "textRefills";
			this.textRefills.Size = new System.Drawing.Size(114, 20);
			this.textRefills.TabIndex = 5;
			// 
			// textDisp
			// 
			this.textDisp.Location = new System.Drawing.Point(134, 129);
			this.textDisp.Name = "textDisp";
			this.textDisp.Size = new System.Drawing.Size(114, 20);
			this.textDisp.TabIndex = 4;
			// 
			// textSig
			// 
			this.textSig.AcceptsReturn = true;
			this.textSig.Location = new System.Drawing.Point(134, 84);
			this.textSig.Multiline = true;
			this.textSig.Name = "textSig";
			this.textSig.Size = new System.Drawing.Size(254, 44);
			this.textSig.TabIndex = 3;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(542, 511);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 13;
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(542, 551);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 14;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(50, 441);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 14);
			this.label2.TabIndex = 7;
			this.label2.Text = "Alerts";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listAlerts
			// 
			this.listAlerts.FormattingEnabled = true;
			this.listAlerts.Location = new System.Drawing.Point(134, 440);
			this.listAlerts.Name = "listAlerts";
			this.listAlerts.Size = new System.Drawing.Size(120, 95);
			this.listAlerts.TabIndex = 9;
			this.listAlerts.DoubleClick += new System.EventHandler(this.listAlerts_DoubleClick);
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
			this.butDelete.Location = new System.Drawing.Point(36, 551);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 15;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(132, 551);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(260, 26);
			this.label7.TabIndex = 17;
			this.label7.Text = "This does not damage any patient records";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkControlled
			// 
			this.checkControlled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkControlled.Location = new System.Drawing.Point(3, 45);
			this.checkControlled.Name = "checkControlled";
			this.checkControlled.Size = new System.Drawing.Size(145, 20);
			this.checkControlled.TabIndex = 1;
			this.checkControlled.Text = "Controlled Substance";
			this.checkControlled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkControlled.UseVisualStyleBackColor = true;
			// 
			// butAddAllergy
			// 
			this.butAddAllergy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddAllergy.Autosize = false;
			this.butAddAllergy.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddAllergy.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddAllergy.CornerRadius = 4F;
			this.butAddAllergy.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAllergy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAllergy.Location = new System.Drawing.Point(260, 511);
			this.butAddAllergy.Name = "butAddAllergy";
			this.butAddAllergy.Size = new System.Drawing.Size(117, 24);
			this.butAddAllergy.TabIndex = 12;
			this.butAddAllergy.Text = "Add Allergy";
			this.butAddAllergy.Click += new System.EventHandler(this.butAddAllergy_Click);
			// 
			// labelRxNorm
			// 
			this.labelRxNorm.Location = new System.Drawing.Point(46, 174);
			this.labelRxNorm.Name = "labelRxNorm";
			this.labelRxNorm.Size = new System.Drawing.Size(88, 14);
			this.labelRxNorm.TabIndex = 21;
			this.labelRxNorm.Text = "RxNorm";
			this.labelRxNorm.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butRxNormSelect
			// 
			this.butRxNormSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRxNormSelect.Autosize = true;
			this.butRxNormSelect.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butRxNormSelect.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butRxNormSelect.CornerRadius = 4F;
			this.butRxNormSelect.Location = new System.Drawing.Point(498, 170);
			this.butRxNormSelect.Name = "butRxNormSelect";
			this.butRxNormSelect.Size = new System.Drawing.Size(22, 22);
			this.butRxNormSelect.TabIndex = 260;
			this.butRxNormSelect.Text = "...";
			this.butRxNormSelect.Click += new System.EventHandler(this.butRxNormSelect_Click);
			// 
			// textRxCui
			// 
			this.textRxCui.Location = new System.Drawing.Point(134, 171);
			this.textRxCui.Name = "textRxCui";
			this.textRxCui.ReadOnly = true;
			this.textRxCui.Size = new System.Drawing.Size(358, 20);
			this.textRxCui.TabIndex = 6;
			// 
			// butAddProblem
			// 
			this.butAddProblem.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProblem.Autosize = false;
			this.butAddProblem.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddProblem.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddProblem.CornerRadius = 4F;
			this.butAddProblem.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProblem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProblem.Location = new System.Drawing.Point(260, 440);
			this.butAddProblem.Name = "butAddProblem";
			this.butAddProblem.Size = new System.Drawing.Size(117, 24);
			this.butAddProblem.TabIndex = 10;
			this.butAddProblem.Text = "Add Problem";
			this.butAddProblem.Click += new System.EventHandler(this.butAddProblem_Click);
			// 
			// butAddMedication
			// 
			this.butAddMedication.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddMedication.Autosize = false;
			this.butAddMedication.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
			this.butAddMedication.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
			this.butAddMedication.CornerRadius = 4F;
			this.butAddMedication.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddMedication.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddMedication.Location = new System.Drawing.Point(260, 475);
			this.butAddMedication.Name = "butAddMedication";
			this.butAddMedication.Size = new System.Drawing.Size(117, 24);
			this.butAddMedication.TabIndex = 11;
			this.butAddMedication.Text = "Add Medication";
			this.butAddMedication.Click += new System.EventHandler(this.butAddMedication_Click);
			// 
			// checkProcRequired
			// 
			this.checkProcRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcRequired.Enabled = false;
			this.checkProcRequired.Location = new System.Drawing.Point(3, 63);
			this.checkProcRequired.Name = "checkProcRequired";
			this.checkProcRequired.Size = new System.Drawing.Size(145, 20);
			this.checkProcRequired.TabIndex = 2;
			this.checkProcRequired.Text = "Is Proc Required";
			this.checkProcRequired.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcRequired.UseVisualStyleBackColor = true;
			// 
			// textPatInstruction
			// 
			this.textPatInstruction.AcceptsTab = true;
			this.textPatInstruction.BackColor = System.Drawing.SystemColors.Window;
			this.textPatInstruction.DetectLinksEnabled = false;
			this.textPatInstruction.DetectUrls = false;
			this.textPatInstruction.Location = new System.Drawing.Point(134, 291);
			this.textPatInstruction.Name = "textPatInstruction";
			this.textPatInstruction.QuickPasteType = OpenDentBusiness.QuickPasteType.Rx;
			this.textPatInstruction.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textPatInstruction.Size = new System.Drawing.Size(386, 143);
			this.textPatInstruction.TabIndex = 8;
			this.textPatInstruction.Text = "";
			// 
			// labelPatInstruct
			// 
			this.labelPatInstruct.Location = new System.Drawing.Point(7, 295);
			this.labelPatInstruct.Name = "labelPatInstruct";
			this.labelPatInstruct.Size = new System.Drawing.Size(126, 14);
			this.labelPatInstruct.TabIndex = 264;
			this.labelPatInstruct.Text = "Patient Instructions";
			this.labelPatInstruct.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormRxDefEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(634, 587);
			this.Controls.Add(this.textPatInstruction);
			this.Controls.Add(this.labelPatInstruct);
			this.Controls.Add(this.checkProcRequired);
			this.Controls.Add(this.butAddMedication);
			this.Controls.Add(this.butAddProblem);
			this.Controls.Add(this.textRxCui);
			this.Controls.Add(this.butRxNormSelect);
			this.Controls.Add(this.labelRxNorm);
			this.Controls.Add(this.butAddAllergy);
			this.Controls.Add(this.checkControlled);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.listAlerts);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textSig);
			this.Controls.Add(this.textDisp);
			this.Controls.Add(this.textRefills);
			this.Controls.Add(this.textNotes);
			this.Controls.Add(this.textDrug);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(556, 483);
			this.Name = "FormRxDefEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Rx Template";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRxDefEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormRxDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRxDefEdit_Load(object sender, System.EventArgs e) {
			textDrug.Text=RxDefCur.Drug;
			textSig.Text=RxDefCur.Sig;
			textDisp.Text=RxDefCur.Disp;
			textRefills.Text=RxDefCur.Refills;
			textNotes.Text=RxDefCur.Notes;
			textPatInstruction.Text=RxDefCur.PatientInstruction;
			checkControlled.Checked=RxDefCur.IsControlled;
			if(Preference.GetBool(PreferenceName.RxHasProc)) {
				checkProcRequired.Enabled=true;
				checkProcRequired.Checked=RxDefCur.IsProcRequired;
			}
			FillAlerts();
			FillRxCui();
		}

		private void FillRxCui() {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("US")) {//Not United States
				labelRxNorm.Visible=false;
				textRxCui.Visible=false;
				butRxNormSelect.Visible=false;
				return;
			}
			if(string.IsNullOrEmpty(RxDefCur.RxCui)) {
				textRxCui.Text="";
			}
			else {
				textRxCui.Text=RxDefCur.RxCui.ToString()+" - "+RxNorms.GetDescByRxCui(RxDefCur.RxCui);
			}
		}

		private void FillAlerts(){
			RxAlertList=RxAlerts.Refresh(RxDefCur.RxDefNum);
			listAlerts.Items.Clear();
			for(int i=0;i<RxAlertList.Count;i++) {
				if(RxAlertList[i].DiseaseDefNum>0) {
					listAlerts.Items.Add(DiseaseDef.GetName(RxAlertList[i].DiseaseDefNum));
				}
				if(RxAlertList[i].AllergyDefNum>0) {
					AllergyDef allergyDef=AllergyDefs.GetOne(RxAlertList[i].AllergyDefNum);
					if(allergyDef!=null) {
						listAlerts.Items.Add(allergyDef.Description);
					}
				}
				if(RxAlertList[i].MedicationNum>0) {
                    Medication.Refresh();
					Medication med= Medication.GetById(RxAlertList[i].MedicationNum);
					if(med!=null) {
						listAlerts.Items.Add(med.Description);
					}
				}
			}
		}

		private void listAlerts_DoubleClick(object sender,EventArgs e) {
			if(listAlerts.SelectedIndex<0) {
				MsgBox.Show(this,"Select at least one Alert.");
				return;
			}
			FormRxAlertEdit FormRAE=new FormRxAlertEdit(RxAlertList[listAlerts.SelectedIndex],RxDefCur);
			FormRAE.ShowDialog();
			FillAlerts();
		}

		private void butAddProblem_Click(object sender,EventArgs e) {
			FormDiseaseDefs FormD=new FormDiseaseDefs();
			FormD.IsSelectionMode=true;
			FormD.IsMultiSelect=true;
			FormD.ShowDialog();
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			for(int i=0;i<FormD.ListSelectedDiseaseDefs.Count;i++) {
				RxAlert alert=new RxAlert();
				alert.DiseaseDefNum=FormD.ListSelectedDiseaseDefs[i].Id;
				alert.RxDefNum=RxDefCur.RxDefNum;
				RxAlerts.Insert(alert);
			}
			FillAlerts();
		}

		private void butAddMedication_Click(object sender,EventArgs e) {
			FormMedications FormMED=new FormMedications();
			FormMED.IsSelectionMode=true;
			FormMED.ShowDialog();
			if(FormMED.DialogResult!=DialogResult.OK) {
				return;
			}
			RxAlert alert=new RxAlert();
			alert.MedicationNum=FormMED.SelectedMedicationNum;
			alert.RxDefNum=RxDefCur.RxDefNum;
			RxAlerts.Insert(alert);
			FillAlerts();
		}

		private void butAddAllergy_Click(object sender,EventArgs e) {
			FormAllergySetup FormAS=new FormAllergySetup();
			FormAS.IsSelectionMode=true;
			FormAS.ShowDialog();
			if(FormAS.DialogResult!=DialogResult.OK) {
				return;
			}
			RxAlert alert=new RxAlert();
			alert.AllergyDefNum=FormAS.SelectedAllergyDefNum;
			alert.RxDefNum=RxDefCur.RxDefNum;
			RxAlerts.Insert(alert);
			FillAlerts();
		}

		private void butRxNormSelect_Click(object sender,EventArgs e) {
			FormRxNorms FormRN=new FormRxNorms();
			FormRN.IsSelectionMode=true;
			FormRN.InitSearchCodeOrDescript=textDrug.Text;
			FormRN.ShowDialog();
			if(FormRN.DialogResult!=DialogResult.OK) {
				return;
			}
			RxDefCur.RxCui=FormRN.SelectedRxNorm.RxCui;
			FillRxCui();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete this prescription template?")){
				return;
			}
			RxDefs.Delete(RxDefCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//RxCui is set when butRxNormSelect is clicked.
			if(CultureInfo.CurrentCulture.Name.EndsWith("US") && string.IsNullOrEmpty(RxDefCur.RxCui)) {//United States
				if(!MsgBox.Show(this,true,"Warning: RxNorm was not picked.  "
					+"RxNorm uniquely identifies drugs in the United States and helps you keep your medications organized.  "
					+"RxNorm is used to send information to and from eRx if you are using or plan to use eRx.\r\n"
					+"Click OK to continue without an RxNorm, or click Cancel to stay in this window."))
				{
					return;
				}
			}
			RxDefCur.Drug=textDrug.Text;
			RxDefCur.Sig=textSig.Text;
			RxDefCur.Disp=textDisp.Text;
			RxDefCur.Refills=textRefills.Text;
			RxDefCur.Notes=textNotes.Text;
			RxDefCur.IsControlled=checkControlled.Checked;
			RxDefCur.IsProcRequired=checkProcRequired.Checked;
			RxDefCur.PatientInstruction=textPatInstruction.Text;
			RxDefs.Update(RxDefCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormRxDefEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;//close as normal
			}
			if(IsNew){
				RxDefs.Delete(RxDefCur);
			}
		}
	}
}
