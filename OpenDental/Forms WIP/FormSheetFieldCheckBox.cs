using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormSheetFieldCheckBox:ODForm {
		///<summary>This is the object we are editing.</summary>
		public SheetFieldDef SheetFieldDefCur { get; private set; }
		///<summary>We need access to a few other fields of the sheetDef.</summary>
		private SheetDef _sheetDefCur;
		private List<string> radioButtonValues;
		private List<Allergy> _listAllergies;
		///<summary>True if the sheet type is MedicalHistory.</summary>
		private bool _isMedHistSheet {
			get { return _sheetDefCur.SheetType==SheetTypeEnum.MedicalHistory; }
		}
		private List<DiseaseDef> _listDiseaseDefs;
		private string _selectedFieldName {
			get {
				if(!_hasSelectedFieldName) {
					return "";
				}
				return listBoxFields.SelectedTag<SheetFieldDef>().FieldName;
			}
		}
		private bool _hasSelectedFieldName {
			get { return listBoxFields.SelectedIndex>=0; }
		}
		private string _selectedMedicalItemName {
			get {
				if(!_hasSelectedMedicalItem) {
					return "";
				}
				if(listMedical.HasSelectedTag<Allergy>()) {
					return listMedical.SelectedTag<Allergy>().Description;
				}
				if(listMedical.HasSelectedTag<DiseaseDef>()) {
					return listMedical.SelectedTag<DiseaseDef>().Name;
				}
				return "";
			}
		}
		private bool _hasSelectedMedicalItem {
			get { return listMedical.SelectedIndex>=0; }
		}

		public FormSheetFieldCheckBox(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly) {
			InitializeComponent();
			
			SheetFieldDefCur=sheetFieldDef;
			_sheetDefCur=sheetDef;
			if(isReadOnly) {
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void FormSheetFieldCheckBox_Load(object sender,EventArgs e) {
            _listDiseaseDefs = DiseaseDef.All();
			textYPos.MaxVal=_sheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
			labelReportableName.Visible=false;
			textReportableName.Visible=false;
			if(SheetFieldDefCur.FieldName.StartsWith("misc")) {
				labelReportableName.Visible=true;
				textReportableName.Visible=true;
				textReportableName.Text=SheetFieldDefCur.ReportableName;
			}
			textUiLabelMobileMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobileMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			textUiLabelMobileRadioButtonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobileRadioButtonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			textUiLabelMobile.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobile.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			textUiLabelMobileCheckBoxNonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			labelUiLabelMobileCheckBoxNonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			labelAlsoActs.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
			//Not allowed to change sheettype or fieldtype once created.  So get all avail fields for this sheettype.
			//These names will not include the ':' for allergies and problems.
			List<SheetFieldDef> listSheetFieldDefs=SheetFieldsAvailable.GetList(_sheetDefCur.SheetType,OutInCheck.Check);
			//If an existing SheetFieldDefCur is not found in the list, add it so we maintain current selection.
			if(SheetFieldDefCur.FieldName.In("FluorideProc","AssessmentProc") || SheetFieldDefCur.FieldName.StartsWith("Proc:")){
				//Couldn't find the current sheetfielddef.  Add it to the list.
				//Checkboxes associated to procedure codes will never be present in SheetFieldsAvailable.GetList(...).
				//Previously this list would contain AssessmentProc and FluorideProc.
				//All other checkboxes associated to proc codes will not exists in list either.
				listSheetFieldDefs.Add(SheetFieldDefCur);
			}
			listBoxFields.SetItems(
				listSheetFieldDefs,
				(item) => item.FieldName,
				(item) => SheetFieldDefCur.FieldName.StartsWith(item.FieldName));
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			textHeight.Text=SheetFieldDefCur.Height.ToString();
			_sheetDefCur.SheetFieldDefs
				.Where(x => !string.IsNullOrEmpty(x.RadioButtonGroup))
				.GroupBy(x => x.RadioButtonGroup)
				.Select(x => x.Key)
				.ForEach(x => { comboRadioGroupNameMisc.Items.Add(x); });
			comboRadioGroupNameMisc.Text=SheetFieldDefCur.RadioButtonGroup;
			checkRequired.Checked=SheetFieldDefCur.IsRequired;
			textTabOrder.Text=SheetFieldDefCur.TabOrder.ToString();
			textUiLabelMobileMisc.Text=SheetFieldDefCur.UiLabelMobile;
			textUiLabelMobileRadioButtonMisc.Text=SheetFieldDefCur.UiLabelMobileRadioButton;
			textUiLabelMobile.Text=SheetFieldDefCur.UiLabelMobile;
			textUiLabelMobileCheckBoxNonMisc.Text=SheetFieldDefCur.UiLabelMobile;
			if(_isMedHistSheet) {
				radioYes.Checked=true;
				if(_selectedFieldName=="allergy") {
					//Will be of format allergy:Aspirin
					FillListMedical(MedicalListType.allergy);
				}
				else if(_selectedFieldName=="problem") {
					//Will be of format problem:Bleeding
					FillListMedical(MedicalListType.problem);
				}
				if(SheetFieldDefCur.RadioButtonValue=="N") {
					radioNo.Checked=true;
					radioYes.Checked=false;
				}
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.Screening) {
				butAddProc.Visible=true;
			}
		}

		private void FormSheetFieldCheckBox_Shown(object sender,EventArgs e) {
			//show allergy/problem message box here so the user can see the list of allergies/problems before deciding to add one
			if(_selectedFieldName=="allergy") {
				//if nothing is selected we didn't find it, prompt to add
				if(listMedical.SelectedIndex<=-1) {
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Allergy does not exist in list. Would you like to add the allergy?")){
						AddAllergy(SheetFieldDefCur);
					}
				}
			}
			else if(_selectedFieldName=="problem") {
				//if nothing is selected we didn't find it, prompt to add
				if(listMedical.SelectedIndex<=-1) {
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Problem does not exist in problems list. Would you like to add the problem?")){
						AddProblem(SheetFieldDefCur);
					}
				}
			}
		}

		///<summary>Fills listMedical with the corresponding list type.  This saves on load time by only filling necessary lists.
		///Attempts to seelct the cooresponding allergy/problem. Will select nothing if it does not exist. </summary>
		private void FillListMedical(MedicalListType medListType) {
			string medSelection=SheetFieldDefCur.FieldName.Remove(0,Math.Min(SheetFieldDefCur.FieldName.Length,8));
			switch(medListType) {
				case MedicalListType.allergy:
					if(_listAllergies==null) {
                        _listAllergies = Allergy.All(false);
					}
					listMedical.SetItems(_listAllergies,(item) => item.Description,(item) => item.Description==medSelection);
					break;
				case MedicalListType.problem:
					listMedical.SetItems(_listDiseaseDefs,(item) => item.Name,(item) => item.Name==medSelection);
					break;
			}
		}
		
		private void comboRadioGroupNameMisc_SelectedIndexChanged(object sender,EventArgs e) {
			var selected=_sheetDefCur.SheetFieldDefs.FirstOrDefault(x => !string.IsNullOrEmpty(x.RadioButtonGroup) && x.RadioButtonGroup==comboRadioGroupNameMisc.Text);
			if(selected==null) {
				return;
			}
			textUiLabelMobileMisc.Text=selected.UiLabelMobile;
		}

		private void listFields_SelectedIndexChanged(object sender,EventArgs e) {
			labelMiscInstructions.Visible=false;
			labelReportableName.Visible=false;
			textReportableName.Visible=false;
			groupRadio.Visible=false;
			groupRadioMisc.Visible=false;
			labelUiLabelMobileCheckBoxNonMisc.Visible=false;
			textUiLabelMobileCheckBoxNonMisc.Visible=false;
			labelRequired.Visible=false;
			checkRequired.Visible=false;
			labelMedical.Visible=false;
			listMedical.Visible=false;
			radioYes.Visible=false;
			radioNo.Visible=false;
			labelYesNo.Visible=false;
			butAddAllergy.Visible=false;
			butAddProblem.Visible=false;
			if(!_hasSelectedFieldName) {
				return;
			}
			if(_isMedHistSheet) {
				labelRequired.Visible=true;
				checkRequired.Visible=true;
				switch(_selectedFieldName) {
					case "allergy":
						labelMedical.Visible=true;
						listMedical.Visible=true;
						radioYes.Visible=true;
						radioNo.Visible=true;
						labelYesNo.Visible=true;
						labelMedical.Text="Allergies";
						FillListMedical(MedicalListType.allergy);
						butAddAllergy.Visible=true;
						break;
					case "problem":
						labelMedical.Visible=true;
						listMedical.Visible=true;
						radioYes.Visible=true;
						radioNo.Visible=true;
						labelYesNo.Visible=true;
						labelMedical.Text="Problems";
						FillListMedical(MedicalListType.problem);
						butAddProblem.Location=butAddAllergy.Location;
						butAddProblem.Visible=true;
						break;
				}
			}
			if(_selectedFieldName=="misc") {
				labelMiscInstructions.Visible=true;
				labelReportableName.Visible=true;
				textReportableName.Visible=true;
				textReportableName.Text=SheetFieldDefCur.ReportableName;//will either be "" or saved ReportableName.
				groupRadioMisc.Visible=true;
				labelRequired.Visible=true;
				checkRequired.Visible=true;
			}
			else if(_isMedHistSheet) {
				return;
			}
			else {
				textReportableName.Text="";
				radioButtonValues=SheetFieldsAvailable.GetRadio(_selectedFieldName);
				if(radioButtonValues.Count==0) { //Rare, currently only addressAndHmPhoneIsSameEntireFamily.
					labelUiLabelMobileCheckBoxNonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
					textUiLabelMobileCheckBoxNonMisc.Visible=SheetDefs.IsWebFormAllowed(_sheetDefCur.SheetType);
					return;
				}
				groupRadio.Visible=true;
				labelRequired.Visible=true;
				checkRequired.Visible=true;
				listRadio.Items.Clear();
				for(int i=0;i<radioButtonValues.Count;i++) {
					listRadio.Items.Add(radioButtonValues[i]);
					if(SheetFieldDefCur.RadioButtonValue==radioButtonValues[i]) {
						listRadio.SelectedIndex=i;
					}
				}				
				//Set the mobile group caption.
				var sheetFieldGroup=_sheetDefCur.SheetFieldDefs.FirstOrDefault(x =>
					x.FieldType==SheetFieldType.CheckBox &&
					!string.IsNullOrEmpty(x.FieldName) &&
					x.FieldName==_selectedFieldName);
				textUiLabelMobile.Text=sheetFieldGroup==null ? "" : sheetFieldGroup.UiLabelMobile;
			}
		}

		private void listRadio_Click(object sender,EventArgs e) {
			if(listRadio.SelectedIndex==-1){
				return;
			}
			SheetFieldDefCur.RadioButtonValue=radioButtonValues[listRadio.SelectedIndex];
		}

		private void listFields_DoubleClick(object sender,EventArgs e) {
			SaveAndClose();
		}

		private void listMedical_DoubleClick(object sender,EventArgs e) {
			SaveAndClose();
		}

		private void butAddAllergy_Click(object sender,EventArgs e) {
			AddAllergy(SheetFieldDefCur);
		}

		private void AddAllergy(SheetFieldDef SheetFieldDefCur) {
			FormAllergyDefEdit formADE=new FormAllergyDefEdit();
			formADE.AllergyDefCur=new Allergy();
			formADE.AllergyDefCur.Description=SheetFieldDefCur?.FieldName.Replace("allergy:","")??"";
			formADE.ShowDialog();
			if(formADE.DialogResult!=DialogResult.OK) {
				return;
			}
			_listAllergies.Add(formADE.AllergyDefCur);
			FillListMedical(MedicalListType.allergy);
		}

		private void butAddProblem_Click(object sender,EventArgs e) {
			AddProblem(SheetFieldDefCur);
		}

		private void AddProblem(SheetFieldDef SheetFieldDefCur) {
			if(!Security.IsAuthorized(Permissions.ProblemEdit)) {
				return;
			}
			DiseaseDef def=new DiseaseDef() {
				ICD9Code="",
				ICD10Code="",
				SnomedCode="",
				SortOrder=(int)DiseaseDef.GetCount(),
				Name=SheetFieldDefCur?.FieldName.Replace("problem:","")??""
			};
			FormDiseaseDefEdit formDDE=new FormDiseaseDefEdit(def,false);
			formDDE.IsNew=true;
			formDDE.ShowDialog();
			if(formDDE.DialogResult!=DialogResult.OK) {
				return;
			}
			DiseaseDef.Insert(formDDE.DiseaseDefCur);
			DataValid.SetInvalid(InvalidType.Diseases);
            _listDiseaseDefs = DiseaseDef.All();
			SecurityLog.Write(Permissions.ProblemEdit,0,formDDE.SecurityLogMsgText);
			FillListMedical(MedicalListType.problem);
		}

		private void radioYes_Click(object sender,EventArgs e) {
			if(radioYes.Checked) {
				radioNo.Checked=false;
			}
			else {
				radioNo.Checked=true;
			}
		}

		private void radioNo_Click(object sender,EventArgs e) {
			if(radioNo.Checked) {
				radioYes.Checked=false;
			}
			else {
				radioYes.Checked=true;
			}
		}

		private void butAddProc_Click(object sender,EventArgs e) {
			List<ODGridColumn> listGridCols=new List<ODGridColumn>() {
				new ODGridColumn(Lan.g(this,"Code"),70),
				new ODGridColumn(Lan.g(this,"Abbreviation"),90,HorizontalAlignment.Center),
				new ODGridColumn(Lan.g(this,"Description"),0,HorizontalAlignment.Right)
			};
			List<ProcedureCode> listMouthProcCodes=ProcedureCodes.GetProcCodesByTreatmentArea(false,TreatmentArea.Mouth,TreatmentArea.None)
				.OrderBy(x => x.ProcCode).ToList();
			List<ODGridRow> listGridRows=new List<ODGridRow>();
			listMouthProcCodes.ForEach(x => {
				ODGridRow row=new ODGridRow (x.ProcCode,x.AbbrDesc,x.Descript);
				row.Tag=x;
				listGridRows.Add(row);
			});
			FormGridSelection formGridSelect=new FormGridSelection(listGridCols,listGridRows,"Add Procedure","Procedures");
			if(formGridSelect.ShowDialog()!=DialogResult.OK) {
				return;
			}
			foreach(object tag in formGridSelect.ListSelectedTags) {
				string fieldName="Proc:"+((ProcedureCode)tag).ProcCode;
				listBoxFields.Items.Add(new ODBoxItem<SheetFieldDef>(fieldName
					,new SheetFieldDef(SheetFieldType.CheckBox,fieldName,"",0,"",false,0,0,0,0,GrowthBehaviorEnum.None,"")));
				listBoxFields.SetSelected(listBoxFields.Items.Count-1,true);
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			SheetFieldDefCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			SaveAndClose();
		}

		private void SaveAndClose(){
			if(textXPos.errorProvider1.GetError(textXPos)!=""
				|| textYPos.errorProvider1.GetError(textYPos)!=""
				|| textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!=""
				|| textTabOrder.errorProvider1.GetError(textTabOrder)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(!_hasSelectedFieldName) {
				MsgBox.Show(this,"Please select a field name first.");
				return;
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.ExamSheet) {
				if(textReportableName.Text.Contains(";") || textReportableName.Text.Contains(":")) {
					MsgBox.Show(this,"Reportable name for Exam Sheet fields may not contain a ':' or a ';'.");
					return;
				}
				if(comboRadioGroupNameMisc.Text.Contains(";") ||comboRadioGroupNameMisc.Text.Contains(":")) {
					MsgBox.Show(this,"Radio button group name for Exam Sheet fields may not contain a ':' or a ';'.");
					return;
				}
			}
			string fieldName=_selectedFieldName;
			string radioButtonValue="";
			string radioItemValue="";
			#region Medical History Sheet
			if(_isMedHistSheet && fieldName!="misc") {
				if(listMedical.Visible) {
					if(!_hasSelectedMedicalItem) {
						switch(fieldName) {
							case "allergy":
								MsgBox.Show(this,"Please select an allergy first.");
								return;
							case "problem":
								MsgBox.Show(this,"Please select a problem first.");
								return;
						}
					}
					fieldName+=":"+_selectedMedicalItemName;
				}
				if(radioNo.Checked || fieldName.StartsWith("checkMed")) {
					radioButtonValue="N";
					radioItemValue="No";
				}
				else {
					radioButtonValue="Y";
					radioItemValue="Yes";
				}
			}
			#endregion
			if(groupRadio.Visible && listRadio.SelectedIndex<0) {
				MsgBox.Show(this,"Please select a Radio Button Value first.");
				return;
			}
			SheetFieldDefCur.FieldName=fieldName;
			SheetFieldDefCur.ReportableName=textReportableName.Text;//always safe even if not a misc field or if textReportableName is blank.
			SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
			SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
			SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
			SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
			//We will set these below where applicable.
			SheetFieldDefCur.RadioButtonGroup="";
			SheetFieldDefCur.UiLabelMobile="";
			SheetFieldDefCur.UiLabelMobileRadioButton="";
			SheetFieldDefCur.RadioButtonValue=radioButtonValue;
			Action<string> updateGroupCaptionForFieldName=new Action<string>((caption) => {
				SheetFieldDefCur.UiLabelMobile=caption;
				_sheetDefCur.SheetFieldDefs
					.Where(x =>
						x.FieldType==SheetFieldType.CheckBox &&
						!string.IsNullOrEmpty(x.FieldName) &&
						x.FieldName==SheetFieldDefCur.FieldName)
					.ForEach(x => {
						x.UiLabelMobile=caption;
					});
			});
			if(groupRadio.Visible) {
				//All items with this group name get this UiLabelMobile.
				updateGroupCaptionForFieldName(textUiLabelMobile.Text);
				if(listRadio.SelectedIndex>=0) {
					SheetFieldDefCur.RadioButtonValue=radioButtonValues[listRadio.SelectedIndex];
				}
			}
			else if(groupRadioMisc.Visible){
				SheetFieldDefCur.RadioButtonGroup=comboRadioGroupNameMisc.Text;
				SheetFieldDefCur.UiLabelMobile=textUiLabelMobileMisc.Text;
				SheetFieldDefCur.UiLabelMobileRadioButton=textUiLabelMobileRadioButtonMisc.Text;
				//All items with this group name get this UiLabelMobile.
				_sheetDefCur.SheetFieldDefs
					.Where(x =>
						x.FieldType==SheetFieldType.CheckBox &&
						!string.IsNullOrEmpty(x.RadioButtonGroup) &&
						x.RadioButtonGroup==SheetFieldDefCur.RadioButtonGroup)
					.ForEach(x => x.UiLabelMobile=SheetFieldDefCur.UiLabelMobile);

			}
			else if(_isMedHistSheet) {
				//All items with this group name get this UiLabelMobile.
				updateGroupCaptionForFieldName(_selectedMedicalItemName);
				SheetFieldDefCur.UiLabelMobileRadioButton=radioItemValue;
			}
			else if(labelUiLabelMobileCheckBoxNonMisc.Visible) { 
				//All items with this group name get this UiLabelMobile.
				updateGroupCaptionForFieldName(textUiLabelMobileCheckBoxNonMisc.Text);
				SheetFieldDefCur.UiLabelMobileRadioButton=string.IsNullOrEmpty(radioItemValue) ? textUiLabelMobileCheckBoxNonMisc.Text : radioItemValue;
			}			
			SheetFieldDefCur.IsRequired=checkRequired.Checked;
			SheetFieldDefCur.TabOrder=PIn.Int(textTabOrder.Text);
			//don't save to database here.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}









		private enum MedicalListType {
			allergy,
			checkMed,
			problem
		}
	}
}