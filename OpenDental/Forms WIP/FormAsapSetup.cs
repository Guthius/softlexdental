using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAsapSetup:ODForm {
		///<summary>Returns the ClinicNum of the selected clinic. Returns 0 if 'Default' is selected or if clinics are not enabled.</summary>
		private long _selectedClinicNum {
			get {
				if(!Preferences.HasClinicsEnabled) {
					return 0;
				}
				return ((ComboClinicItem)comboClinic.SelectedItem)?.ClinicNum??-1;
			}
		}

		public FormAsapSetup() {
			InitializeComponent();
			
		}

		private void FormAsapSetup_Load(object sender,EventArgs e) {
			if(Preferences.HasClinicsEnabled) {
				FillClinics();
			}
			else {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				checkUseDefaults.Visible=false;
			}
			FillPrefs();
		}

		private void FillClinics() {
			comboClinic.Items.Clear();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurrentUser);
			if(!Security.CurrentUser.ClinicRestricted) {
				comboClinic.Items.Add(new ComboClinicItem(Lan.g(this,"Default"),0));
				comboClinic.SelectedIndex=0;
			}
			for(int i=0;i<listClinics.Count;i++) {
				int addedIdx=comboClinic.Items.Add(new ComboClinicItem(listClinics[i].Abbr,listClinics[i].ClinicNum));
				if(listClinics[i].ClinicNum==Clinics.ClinicNum) {
					comboClinic.SelectedIndex=addedIdx;
				}
			}
		}

		private void FillPrefs() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,""),250);//Random tidbits regarding the template
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Template"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			checkUseDefaults.Checked=true;
			string baseVars=Lan.g(this,"Available variables:")+" [NameF], [Date], [Time], [OfficeName], [OfficePhone]";
			ODGridRow row;
			row=BuildRowForTemplate(PreferenceName.ASAPTextTemplate,"Text manual",baseVars);
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PreferenceName.WebSchedAsapTextTemplate,"Web Sched Text",baseVars+", [AsapURL]");
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PreferenceName.WebSchedAsapEmailTemplate,"Web Sched Email Body",baseVars+", [AsapURL]");
			gridMain.Rows.Add(row);
			row=BuildRowForTemplate(PreferenceName.WebSchedAsapEmailSubj,"Web Sched Email Subject",baseVars);
			gridMain.Rows.Add(row);
			gridMain.EndUpdate();
			if(_selectedClinicNum==0) {
				textWebSchedPerDay.Text=Preference.GetString(PreferenceName.WebSchedAsapTextLimit);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(PreferenceName.WebSchedAsapTextLimit,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					textWebSchedPerDay.Text=Preference.GetString(PreferenceName.WebSchedAsapTextLimit);
				}
				else {
					textWebSchedPerDay.Text=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
		}

		///<summary>Creates a row for the passed in template type. Unchecks checkUseDefaults if a clinic-level template is in use.</summary>
		private ODGridRow BuildRowForTemplate(PreferenceName prefName,string templateName,string availableVars) {
			string templateText;
			bool doShowDefault=false;
			if(_selectedClinicNum==0) {
				templateText=Preference.GetString(prefName);
				checkUseDefaults.Checked=false;
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || clinicPref.ValueString==null) {
					templateText=Preference.GetString(prefName);
					doShowDefault=true;
				}
				else {
					templateText=clinicPref.ValueString;
					checkUseDefaults.Checked=false;
				}
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(Lan.g(this,templateName)+(doShowDefault ? " "+Lan.g(this,"(Default)") : ""));
			row.Cells.Add(availableVars);
			row.Cells.Add(templateText);
			row.Tag=prefName;
			return row;
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			ComboClinicItem selectedItem=(ComboClinicItem)comboClinic.SelectedItem;
			if(selectedItem.ClinicNum==0) {//'Default' is selected.
				checkUseDefaults.Visible=false;
			}
			else {
				checkUseDefaults.Visible=true;
			}
			FillPrefs();
		}

		private void checkUseDefaults_Click(object sender,EventArgs e) {
			List<PreferenceName> listPrefs=new List<PreferenceName> {
				PreferenceName.ASAPTextTemplate,
				PreferenceName.WebSchedAsapTextTemplate,
				PreferenceName.WebSchedAsapEmailTemplate,
				PreferenceName.WebSchedAsapEmailSubj,
				PreferenceName.WebSchedAsapTextLimit,
			};
			if(checkUseDefaults.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete custom templates for this clinic and switch to using defaults? This cannot be undone.")) {
					ClinicPrefs.DeletePrefs(_selectedClinicNum,listPrefs);
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
				else {
					checkUseDefaults.Checked=false;
				}
			}
			else {//Was checked, now user is unchecking it.
				bool wasChanged=false;
				foreach(PreferenceName pref in listPrefs) {
					if(ClinicPrefs.Upsert(pref,_selectedClinicNum,Preference.GetString(pref))) {
						wasChanged=true;
					}
				}
				if(wasChanged) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PreferenceName prefName=(PreferenceName)gridMain.Rows[e.Row].Tag;
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			if(_selectedClinicNum==0) {
				FormR.MessageVal=Preference.GetString(prefName);
			}
			else {
				ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,_selectedClinicNum);
				if(clinicPref==null || string.IsNullOrEmpty(clinicPref.ValueString)) {
					FormR.MessageVal=Preference.GetString(prefName);
				}
				else {
					FormR.MessageVal=clinicPref.ValueString;
				}
			}
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(_selectedClinicNum==0) {
				if(Preference.Update(prefName,FormR.MessageVal)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(prefName,_selectedClinicNum,FormR.MessageVal)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
			FillPrefs();
		}

		private void textWebSchedPerDay_Leave(object sender,EventArgs e) {
			if(!textWebSchedPerDay.IsValid) {
				return;
			}
			if(_selectedClinicNum==0) {
				if(Preference.Update(PreferenceName.WebSchedAsapTextLimit,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				if(ClinicPrefs.Upsert(PreferenceName.WebSchedAsapTextLimit,_selectedClinicNum,textWebSchedPerDay.Text)) {
					DataValid.SetInvalid(InvalidType.ClinicPrefs);
				}
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private class ComboClinicItem {
			public string DisplayName;
			public long ClinicNum;
			public ComboClinicItem(string displayName,long clinicNum) {
				DisplayName=displayName;
				ClinicNum=clinicNum;
			}
			public override string ToString() {
				return DisplayName;
			}
		}

	}
}