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

namespace OpenDental {
///<summary></summary>
	public partial class FormReactivationSetup : ODForm {

		///<summary></summary>
		public FormReactivationSetup(){
			InitializeComponent();
			Lan.F(this);
		}

		public void FormReactivationSetup_Load(object sender, System.EventArgs e) {
			FillManualReactivation();
		}

		///<summary>Called on load to initially load the Reactivation window with values from the database.  Calls FillGrid at the end.</summary>
		private void FillManualReactivation() {
			FillStatusComboBoxes();
			FillGrid();
		}

		private void FillGrid(){
			string availableFields="[NameFL], [NameF], [ClinicName], [ClinicPhone], [PracticeName], [PracticePhone], [OfficePhone]";
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.AddColumn(Lan.g("TableReactivationMsgs","Mode"),61);
			gridMain.AddColumn("",300);
			gridMain.AddColumn(Lan.g("TableReactivationMsgs","Message"),500);
			gridMain.Rows.Clear();
			#region 1st Reminder
			//ReactivationEmailSubject
			gridMain.AddRow(PreferenceName.ReactivationEmailSubject,Lan.g(this,"E-mail"), Lan.g(this,"Subject line")
				,Preference.GetString(PreferenceName.ReactivationEmailSubject)
			);
			//ReactivationEmailMessage
			gridMain.AddRow(PreferenceName.ReactivationEmailMessage,Lan.g(this,"E-mail"),Lan.g(this,"Available variables")+": "+availableFields
				,Preference.GetString(PreferenceName.ReactivationEmailMessage)
			);
			//ReactivationEmailFamMsg
			gridMain.AddRow(PreferenceName.ReactivationEmailFamMsg,Lan.g(this,"E-mail")
				,Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show.")
				,Preference.GetString(PreferenceName.ReactivationEmailFamMsg)
			);
			//ReactivationPostcardMessage
			gridMain.AddRow(PreferenceName.ReactivationPostcardMessage,Lan.g(this,"Postcard"),Lan.g(this,"Available variables")+": "+availableFields
				,Preference.GetString(PreferenceName.ReactivationPostcardMessage)
			);
			//ReactivationPostcardFamMsg
			gridMain.AddRow(PreferenceName.ReactivationPostcardFamMsg,Lan.g(this,"Postcard")
				,Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show.")
				,Preference.GetString(PreferenceName.ReactivationPostcardFamMsg)
			);
			#endregion
			gridMain.EndUpdate();
		}

		private void FillStatusComboBoxes() {
			List<Definition> listDefs=(Definition.GetByCategory(DefinitionCategory.RecallUnschedStatus));
			comboStatusMailedReactivation.SetItems(listDefs,(def) => new ODBoxItem<long>(def.Description,def.Id));
			comboStatusEmailedReactivation.SetItems(listDefs,(def) => new ODBoxItem<long>(def.Description,def.Id));
			comboStatusTextedReactivation.SetItems(listDefs,(def) => new ODBoxItem<long>(def.Description,def.Id));
			comboStatusEmailTextReactivation.SetItems(listDefs,(def) => new ODBoxItem<long>(def.Description,def.Id));
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PreferenceName prefName=gridMain.SelectedTag<PreferenceName>();
			FormRecallMessageEdit FormR = new FormRecallMessageEdit(prefName);
			FormR.MessageVal=Preference.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Preference.Update(prefName,FormR.MessageVal)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}		

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textMaxReminders.errorProvider1.GetError(textMaxReminders)!="") 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textPostcardsPerSheet.Text!="1"
				&& textPostcardsPerSheet.Text!="3"
				&& textPostcardsPerSheet.Text!="4") 
			{
				MsgBox.Show(this,"The value in postcards per sheet must be 1, 3, or 4");
				return;
			}
			if(comboStatusMailedReactivation.SelectedIndex==-1
				|| comboStatusEmailedReactivation.SelectedIndex==-1
				|| comboStatusTextedReactivation.SelectedIndex==-1
				|| comboStatusEmailTextReactivation.SelectedIndex==-1) 
			{
				MsgBox.Show(this,"All status options on the left must be set.");
				return;
			}
			//End of Validation
			bool didChange=textPostcardsPerSheet.Save();
			if(didChange) {
				if(textPostcardsPerSheet.Text=="1") {
					MsgBox.Show(this,"If using 1 postcard per sheet, you must adjust the position, and also the preview will not work");
				}
			}
			if(AutoSave() || didChange) { //Only set prefs invalid if we made a change
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}
