using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldStatic:ODForm {
		///<summary>This is the object we are editing.</summary>
		public SheetFieldDef SheetFieldDefCur;
		///<summary>We need access to a few other fields of the sheetDef.</summary>
		public SheetDef SheetDefCur;
		public bool IsReadOnly;
		public bool IsNew;
		private int textSelectionStart;

		public FormSheetFieldStatic() {
			InitializeComponent();
			
		}

		private void FormSheetFieldStatic_Load(object sender,EventArgs e) {
			textYPos.MaxVal=SheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
			if(IsReadOnly){
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			if(SheetDefCur.SheetType!=SheetTypeEnum.Statement) {
				checkPmtOpt.Visible=false;
			}
			if(SheetDefCur.SheetType==SheetTypeEnum.PatientLetter) {
				butExamSheet.Visible=true;
			}
			if(SheetDefs.IsDashboardType(SheetDefCur)) {
				comboGrowthBehavior.Enabled=false;
			}
			else {
				butExamSheet.Visible=false;
			}
			checkIncludeInMobile.Visible=SheetDefs.IsWebFormAllowed(SheetDefCur.SheetType);
			//Show/hide in mobile editor depending on if TabOrderMobile has been previously set. This is how we will selectively include only desireable StaticText fields.
			checkIncludeInMobile.Checked=SheetDefCur.HasMobileLayout;
			checkIsLocked.Checked=IsNew ? true : SheetFieldDefCur.IsLocked;
			textFieldValue.Text=SheetFieldDefCur.FieldValue;
			InstalledFontCollection fColl=new InstalledFontCollection();
			for(int i=0;i<fColl.Families.Length;i++){
				comboFontName.Items.Add(fColl.Families[i].Name);
			}
			comboFontName.Text=SheetFieldDefCur.FontName;
			numFontSize.Value=(decimal)SheetFieldDefCur.FontSize;
			checkFontIsBold.Checked=SheetFieldDefCur.FontIsBold;
			SheetUtil.FillComboGrowthBehavior(comboGrowthBehavior,SheetFieldDefCur.GrowthBehavior);
			for(int i=0;i<Enum.GetNames(typeof(System.Windows.Forms.HorizontalAlignment)).Length;i++) {
				comboTextAlign.Items.Add(Enum.GetNames(typeof(System.Windows.Forms.HorizontalAlignment))[i]);
				if((int)SheetFieldDefCur.TextAlign==i) {
					comboTextAlign.SelectedIndex=i;
				}
			}
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			textHeight.Text=SheetFieldDefCur.Height.ToString();
			checkPmtOpt.Checked=SheetFieldDefCur.IsPaymentOption;
			butColor.BackColor=SheetFieldDefCur.ItemColor;
			FillFields();
		}

		private void FillFields(){
			string[] fieldArray=new string[] {
				"activeAllergies", 
				"activeProblems", 
				"address",
				"age",
				"apptsAllFuture",
				"apptModNote",
				"balTotal",
				"bal_0_30",
				"bal_31_60",
				"bal_61_90",
				"balOver90",
				"balInsEst",
				"balTotalMinusInsEst",
				"BillingType",
				"Birthdate",
				"carrierName",
				"carrier2Name",
				"carrierAddress",
				"carrier2Address",
				"carrierCityStZip",
				"carrier2CityStZip",
				"ChartNumber",
				"cityStateZip",
				"clinicPatDescription",
				"clinicPatAddress",
				"clinicPatCityStZip",
				"clinicPatPhone",
				"clinicCurDescription",
				"clinicCurAddress",
				"clinicCurCityStZip",
				"clinicCurPhone",
				"currentMedications",
				"DateFirstVisit",
				"dateLastAppt",
				"dateLastBW",
				"dateLastExam",
				"dateLastPanoFMX",
				"dateLastPerio",
				"dateLastProphy",
				"dateOfLastSavedTP",
				"dateRecallDue",
				"dateTimeLastAppt",
				"dateTodayLong",
				"dateToday",
				"dueForBWYN",
				"dueForPanoYN",
				"Email",
				"famFinNote",
				"famFinUrgNote",
				"famPopups",
				"famRecallDue",
				"gender",
				"genderHeShe",
				"genderheshe",
				"genderHimHer",
				"genderhimher",
				"genderHimselfHerself",
				"genderhimselfherself",
				"genderHisHer",
				"genderhisher",
				"genderHisHers",
				"genderhishers",
				"guarantorHmPhone",
				"guarantorNameF",
				"guarantorNameFL",
				"guarantorNameL",
				"guarantorNamePref",
				"guarantorNameLF",
				"guarantorWirelessPhone",
				"guarantorWkPhone",
				"HmPhone",
				"insAnnualMax",
				"insDeductible",
				"insDeductibleUsed",
				"insEmployer",
				"insFeeSchedule",
				"insFreqBW",
				"insFreqExams",
				"insFreqPanoFMX",
				"insPending",
				"insPercentages",
				"insPlanGroupName",
				"insPlanGroupNumber",
				"insPlanNote",
				"insRemaining",
				"insSubBirthDate",
				"insSubNote",
				"insType",
				"insUsed",
				"ins2AnnualMax",
				"ins2Deductible",
				"ins2DeductibleUsed",
				"ins2Employer",
				"ins2FreqBW",
				"ins2FreqExams",
				"ins2FreqPanoFMX",
				"ins2Pending",
				"ins2Percentages",
				"ins2PlanGroupName",
				"ins2PlanGroupNumber",
				"ins2Remaining",
				"ins2Used",
				"medicalSummary",
				"MedUrgNote",
				"nameF",
				"nameFL",
				"nameFLFormal",
				"nameL",
				"nameLF",
				"nameMI",
				"namePref",
				"nextSchedApptDate",
				"nextSchedApptDateT",
				"nextSchedApptsFam",
				"patientPortalCredentials",
				"PatNum",
				"plannedAppointmentInfo",
				"practiceTitle",
				"premedicateYN",
				"priProvNameFormal",
				"recallInterval",
				"recallScheduledYN",
				"referredFrom",
				"referredTo",
				"salutation",
				"serviceNote",
				"siteDescription",
				"SSN",
				"subscriberID",
				"subscriberNameFL",
				"subscriber2NameFL",
				"timeNow",
				"tpResponsPartyAddress",
				"tpResponsPartyCityStZip",
				"tpResponsPartyNameFL",
				"treatmentNote",
				"treatmentPlanProcs",
				"treatmentPlanProcsPriority",
				"WirelessPhone",
				"WkPhone"
			};
			listFields.Items.Clear();
			for(int i=0;i<fieldArray.Length;i++){
				listFields.Items.Add(fieldArray[i]);
			}
		}

		private void listFields_MouseClick(object sender,MouseEventArgs e) {
			string fieldStr="";
			for(int i=0;i<listFields.Items.Count;i++) {
				if(listFields.GetItemRectangle(i).Contains(e.Location)) {
					fieldStr=listFields.Items[i].ToString();
				}
			}
			if(fieldStr=="") {
				return;
			}
			if(textSelectionStart < textFieldValue.Text.Length-1) {
				textFieldValue.Text=textFieldValue.Text.Substring(0,textSelectionStart)
					+"["+fieldStr+"]"
					+textFieldValue.Text.Substring(textSelectionStart);
			}
			else{//otherwise, just tack it on the end
				textFieldValue.Text+="["+fieldStr+"]";
			}
			textFieldValue.Select(textSelectionStart+fieldStr.Length+2,0);
			textFieldValue.Focus();
			//if(!textFieldValue.Focused){
			//	textFieldValue.Text+="["+fieldStr+"]";
			//	return;
			//}
			//MessageBox.Show(textFieldValue.SelectionStart.ToString());
		}

		private void textFieldValue_Leave(object sender,EventArgs e) {
			textSelectionStart=textFieldValue.SelectionStart;
		}

		/// <summary>This method is tied to any event that could change text size, such as font size, text, or the Bold checkbox.</summary>
		private void UpdateTextSizeLabels(object sender,EventArgs e) {
			float fontSize=(float)numFontSize.Value;
			FontStyle curFontStyle=FontStyle.Regular;
			if(checkFontIsBold.Checked) {
				curFontStyle=FontStyle.Bold;
			}
			Size sizeText;
			using(Font font=new Font(comboFontName.Text,fontSize,curFontStyle)){
				//If we measure using a graphics object, it will report the size of the text if we drew it with a graphics object.
				//This correctly reports the text size for how we are drawing the text.
				sizeText=TextRenderer.MeasureText(textFieldValue.Text,font);
			}
			labelTextW.Text=Lan.g(this,"TextW:")+" "+sizeText.Width.ToString();
			labelTextH.Text=Lan.g(this,"TextH:")+" "+sizeText.Height.ToString();
		}

		private void butExamSheet_Click(object sender,EventArgs e) {
			FormSheetFieldExam FormE=new FormSheetFieldExam();
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(textSelectionStart < textFieldValue.Text.Length-1) {//if cursor is not at the end of the text in textFieldValue, insert into text beginning at cursor
				textFieldValue.Text=textFieldValue.Text.Substring(0,textSelectionStart)
				+"["+FormE.ExamFieldSelected+"]"
				+textFieldValue.Text.Substring(textSelectionStart);
			}
			else {//otherwise, just tack it on the end
				textFieldValue.Text+="["+FormE.ExamFieldSelected+"]";
			}
			textFieldValue.Select(textSelectionStart+FormE.ExamFieldSelected.Length+2,0);
			textFieldValue.Focus();
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			SheetFieldDefCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textXPos.errorProvider1.GetError(textXPos)!=""
				|| textYPos.errorProvider1.GetError(textYPos)!=""
				|| textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textFieldValue.Text==""){
				MsgBox.Show(this,"Please set a field value first.");
				return;
			}
			if(comboFontName.Text==""){
				//not going to bother testing for validity unless it will cause a crash.
				MsgBox.Show(this,"Please select a font name first.");
				return;
			}
			float fontSize=(float)numFontSize.Value;
			if(fontSize<2){
				MsgBox.Show(this,"Font size is invalid.");
				return;
			}
			SheetFieldDefCur.FieldValue=textFieldValue.Text;
			SheetFieldDefCur.FontName=comboFontName.Text;
			SheetFieldDefCur.FontSize=fontSize;
			SheetFieldDefCur.FontIsBold=checkFontIsBold.Checked;
			SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
			SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
			SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
			SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
			SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.SelectedTag<GrowthBehaviorEnum>();
			SheetFieldDefCur.TextAlign=(System.Windows.Forms.HorizontalAlignment)comboTextAlign.SelectedIndex;
			SheetFieldDefCur.IsPaymentOption=checkPmtOpt.Checked;
			SheetFieldDefCur.ItemColor=butColor.BackColor;
			SheetFieldDefCur.IsLocked=checkIsLocked.Checked;
			if(!checkIncludeInMobile.Checked || !SheetDefs.IsWebFormAllowed(SheetDefCur.SheetType)) { //Hide from mobile layout.
				SheetFieldDefCur.TabOrderMobile=0;
			}
			else if(SheetFieldDefCur.TabOrderMobile<=0) { //Had previously been hidden from mobile layout so show and set to top. User can re-order using the mobile editor.
				SheetFieldDefCur.TabOrderMobile=1;
			}
			//don't save to database here.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}