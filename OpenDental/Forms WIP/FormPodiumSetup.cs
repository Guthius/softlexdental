using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormPodiumSetup:ODForm {
		private Program _progCur;
		private ProgramPreference _apiToken;
		private ProgramPreference _compName;
		///<summary>Dictionary used to store changes for each clinic to be updated or inserted when saving to DB.</summary>
		private Dictionary<long,ProgramPreference> _dictLocationIDs=new Dictionary<long, ProgramPreference>();
		private ProgramPreference _useService;
		private ProgramPreference _disableAdvertising;
		private ProgramPreference _apptSetCompleteMins;
		private ProgramPreference _apptTimeArrivedMins;
		private ProgramPreference _apptTimeDismissedMins;
		private ProgramPreference _newPatTriggerType;
		private ProgramPreference _existingPatTriggerType;
		private ProgramPreference _showCommlogsInChartAndAccount;
		private ReviewInvitationTrigger _existingPatTriggerEnum;
		private ReviewInvitationTrigger _newPatTriggerEnum;
		private bool _hasProgramPropertyChanged;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		private List<ProgramPreference> _listProgramProperties;
		///<summary>Can be 0 for "Headquarters" or non clinic users.</summary>
		private long _clinicNumCur;

		public FormPodiumSetup() {
			InitializeComponent();
			
		}

		private void FormPodiumSetup_Load(object sender,EventArgs e) {
			if(Preferences.HasClinicsEnabled) {//Using clinics
				_listUserClinicNums=new List<long>();
				comboClinic.Items.Clear();
				if(Security.CurrentUser.ClinicRestricted) {
					if(checkEnabled.Checked) {
						checkEnabled.Enabled=false;
						_clinicNumCur=0;
					}
				}
				else {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					//this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
					_listUserClinicNums.Add(0);
					comboClinic.SelectedIndex=0;
					_clinicNumCur=0;
				}
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurrentUser);
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Abbr);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(Clinics.ClinicNum==listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurrentUser.ClinicRestricted) {
							comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
						}
						_clinicNumCur=_listUserClinicNums[comboClinic.SelectedIndex];
					}
				}
			}
			else {//clinics are not enabled, use ClinicNum 0 to indicate 'Headquarters' or practice level program properties
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
				_clinicNumCur=0;
			}
			_progCur=Programs.GetCur(ProgramName.Podium);
			if(_progCur==null) {
				MsgBox.Show(this,"The Podium bridge is missing from the database.");//should never happen
				DialogResult=DialogResult.Cancel;
				return;
			}
			try {
				//long clinicNum=0;
				//if(comboClinic.SelectedIndex>0) {//0 is always "All" so only check for greater than 0.
				//	clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
				//}
				//_listProgramProperties=ProgramProperties.GetListForProgramAndClinicWithDefault(_progCur.ProgramNum,clinicNum);
				//_useService=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.UseService);
				//_disableAdvertising=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.DisableAdvertising);
				//_apptSetCompleteMins=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ApptSetCompletedMinutes);
				//_apptTimeArrivedMins=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ApptTimeArrivedMinutes);
				//_apptTimeDismissedMins=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ApptTimeDismissedMinutes);
				//_compName=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ComputerNameOrIP);
				//_apiToken=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.APIToken);
				//List<ProgramProperty> listLocationIDs=ProgramProperties.GetForProgram(_progCur.ProgramNum).FindAll(x => x.Key==Podium.PropertyDescs.LocationID);
				//_dictLocationIDs.Clear();
				//foreach(ProgramProperty ppCur in listLocationIDs) {//If clinics is off, this will only grab the program property with a 0 clinic num (_listUserClinicNums will only have 0).
				//	if(_dictLocationIDs.ContainsKey(ppCur.ClinicId) || !_listUserClinicNums.Contains(ppCur.ClinicId)) {
				//		continue;
				//	}
				//	_dictLocationIDs.Add(ppCur.ClinicId,ppCur);
				//}
				//_newPatTriggerType=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.NewPatientTriggerType);
				//_existingPatTriggerType=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ExistingPatientTriggerType);
				//_showCommlogsInChartAndAccount=_listProgramProperties.FirstOrDefault(x => x.Key==Podium.PropertyDescs.ShowCommlogsInChartAndAccount);
			}
			catch(Exception) {
				MsgBox.Show(this,"You are missing a program property for Podium.  Please contact support to resolve this issue.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			FillForm();
			SetAdvertising();
		}

		///<summary>Handles both visibility and checking of checkHideButtons.</summary>
		private void SetAdvertising() {
			checkHideButtons.Visible=true;
			ProgramPreference prop=ProgramProperties.GetForProgram(_progCur.Id).FirstOrDefault(x => x.Key=="Disable Advertising");
			if(checkEnabled.Checked || prop==null) {
				checkHideButtons.Visible=false;
			}
			if(prop!=null) {
				checkHideButtons.Checked=(prop.Value=="1");
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			SaveClinicCurProgramPropertiesToDict();
			_clinicNumCur=_listUserClinicNums[comboClinic.SelectedIndex];
			//This will either display the HQ value, or the clinic specific value.
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
				textLocationID.Text=_dictLocationIDs[_clinicNumCur].Value;
			}
			else {
				textLocationID.Text=_dictLocationIDs[0].Value;//Default to showing the HQ value when filling info for a clinic with no program property.
			}
		}
		
		///<summary>Updates the in memory dictionary with any changes made to the current locationID for each clinic before showing the next one.</summary>
		private void SaveClinicCurProgramPropertiesToDict() {
			//First check if Headquarters (default) is selected.
			if(_clinicNumCur==0) {
				//Headquarters is selected so only update the location ID (might have changed) on all other location ID properties that match the "old" location ID of HQ.
				if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
					//Get the location ID so that we correctly update all program properties with a matching location ID.
					string locationIdOld=_dictLocationIDs[_clinicNumCur].Value;
					foreach(KeyValuePair<long,ProgramPreference> item in _dictLocationIDs) {
						ProgramPreference ppCur=item.Value;
						if(ppCur.Value==locationIdOld) {
							ppCur.Value=textLocationID.Text;
						}
					}
				}
				return;//No other clinic specific changes could have been made, we need to return.
			}
			//Update or Insert clinic specific properties into memory
			ProgramPreference ppLocationID=new ProgramPreference();
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
				ppLocationID=_dictLocationIDs[_clinicNumCur];//Override the database's property with what is in memory.
			}
			else {//Get default programproperty from db.
				ppLocationID=ProgramProperties.GetListForProgramAndClinicWithDefault(_progCur.Id,_clinicNumCur)
					.FirstOrDefault(x => x.Key==Podium.PropertyDescs.LocationID);
			}
			if(ppLocationID.ClinicId==0) {//No program property for current clinic, since _clinicNumCur!=0
				//ProgramProperty ppLocationIDNew=ppLocationID.Copy();
				//ppLocationIDNew.ProgramPropertyNum=0;
				//ppLocationIDNew.ClinicId=_clinicNumCur;
				//ppLocationIDNew.Value=textLocationID.Text;
				//if(!_dictLocationIDs.ContainsKey(_clinicNumCur)) {//Should always happen
				//	_dictLocationIDs.Add(_clinicNumCur,ppLocationIDNew);
				//}
				//return;
			}
			//At this point we know that the clinicnum isn't 0 and the database has a property for that clinicnum.
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {//Should always happen
				ppLocationID.Value=textLocationID.Text;
				_dictLocationIDs[_clinicNumCur]=ppLocationID;
			}
			else {
				_dictLocationIDs.Add(_clinicNumCur,ppLocationID);//Should never happen.
			}
		}

		private void FillForm() {
			try {
				checkUseService.Checked=PIn.Bool(_useService.Value);
				checkShowCommlogsInChart.Checked=PIn.Bool(_showCommlogsInChartAndAccount.Value);
				checkEnabled.Checked=_progCur.Enabled;
				checkHideButtons.Checked=PIn.Bool(_disableAdvertising.Value);
				textApptSetComplete.Text=_apptSetCompleteMins.Value;
				textApptTimeArrived.Text=_apptTimeArrivedMins.Value;
				textApptTimeDismissed.Text=_apptTimeDismissedMins.Value;
				textCompNameOrIP.Text=_compName.Value;
				textAPIToken.Text=_apiToken.Value;
				if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
					textLocationID.Text=_dictLocationIDs[_clinicNumCur].Value;
				}
				else {
					textLocationID.Text=_dictLocationIDs[0].Value;//Default to showing the HQ value when filling info for a clinic with no program property.
				}
				_existingPatTriggerEnum=PIn.Enum<ReviewInvitationTrigger>(_existingPatTriggerType.Value);
				_newPatTriggerEnum=PIn.Enum<ReviewInvitationTrigger>(_newPatTriggerType.Value);
				switch(_existingPatTriggerEnum) {
					case ReviewInvitationTrigger.AppointmentCompleted:
						radioSetCompleteExistingPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeArrived:
						radioTimeArrivedExistingPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeDismissed:
						radioTimeDismissedExistingPat.Checked=true;
						break;
				}
				switch(_newPatTriggerEnum) {
					case ReviewInvitationTrigger.AppointmentCompleted:
						radioSetCompleteNewPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeArrived:
						radioTimeArrivedNewPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeDismissed:
						radioTimeDismissedNewPat.Checked=true;
						break;
				}
			}
			catch(Exception) {
				MsgBox.Show(this,"You are missing a program property from the database.  Please call support to resolve this issue.");
				DialogResult=DialogResult.Cancel;
				return;
			}
		}

		private void RadioButton_CheckChanged(object sender,EventArgs e) {
			if(sender.GetType()!=typeof(RadioButton)) {
				return;
			}
			RadioButton radioButtonCur=(RadioButton)sender;
			if(radioButtonCur.Checked) {
				switch(radioButtonCur.Name) {
					case "radioSetCompleteExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentCompleted;
						break;
					case "radioTimeArrivedExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeArrived;
						break;
					case "radioTimeDismissedExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeDismissed;
						break;
					case "radioSetCompleteNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentCompleted;
						break;
					case "radioTimeArrivedNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeArrived;
						break;
					case "radioTimeDismissedNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeDismissed;
						break;
					default:
						throw new Exception("Unknown Radio Button Name");
				}
			}
		}

		private void SaveProgram() {
			SaveClinicCurProgramPropertiesToDict();
			_progCur.Enabled=checkEnabled.Checked;
			UpdateProgramProperty(_useService,POut.Bool(checkUseService.Checked));
			UpdateProgramProperty(_showCommlogsInChartAndAccount,POut.Bool(checkShowCommlogsInChart.Checked));
			UpdateProgramProperty(_disableAdvertising,POut.Bool(checkHideButtons.Checked));
			UpdateProgramProperty(_apptSetCompleteMins,textApptSetComplete.Text);
			UpdateProgramProperty(_apptTimeArrivedMins,textApptTimeArrived.Text);
			UpdateProgramProperty(_apptTimeDismissedMins,textApptTimeDismissed.Text);
			UpdateProgramProperty(_compName,textCompNameOrIP.Text);
			UpdateProgramProperty(_apiToken,textAPIToken.Text);
			UpdateProgramProperty(_newPatTriggerType,POut.Int((int)_newPatTriggerEnum));
			UpdateProgramProperty(_existingPatTriggerType,POut.Int((int)_existingPatTriggerEnum));
			UpsertProgramPropertiesForClinics();
			Program.Update(_progCur);
		}

		private void UpdateProgramProperty(ProgramPreference ppFromDb,string newpropertyValue) {
			//if(ppFromDb.Value==newpropertyValue) {
			//	return;
			//}
			//ppFromDb.Value=newpropertyValue;
			//ProgramProperties.Update(ppFromDb);
			//_hasProgramPropertyChanged=true;
		}

		private void UpsertProgramPropertiesForClinics() {
			//List<ProgramProperty> listLocationIDsFromDb=ProgramProperties.GetForProgram(_progCur.ProgramNum).FindAll(x => x.Key==Podium.PropertyDescs.LocationID);
			//List<ProgramProperty> listLocationIDsCur=_dictLocationIDs.Values.ToList();
			//foreach(ProgramProperty ppCur in listLocationIDsCur) {
			//	if(listLocationIDsFromDb.Exists(x => x.ProgramPropertyNum == ppCur.ProgramPropertyNum)) {
			//		UpdateProgramProperty(listLocationIDsFromDb[listLocationIDsFromDb.FindIndex(x => x.ProgramPropertyNum == ppCur.ProgramPropertyNum)],ppCur.Value);//ppCur.PropertyValue will match textLocationID.Text
			//	}
			//	else {
			//		ProgramProperties.Insert(ppCur);//Program property for that clinicnum didn't exist, so insert it into the db.
			//		_hasProgramPropertyChanged=true;
			//	}
			//}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textApptSetComplete.errorProvider1.GetError(textApptSetComplete)!=""
				|| textApptTimeArrived.errorProvider1.GetError(textApptTimeArrived)!=""
				|| textApptTimeDismissed.errorProvider1.GetError(textApptTimeDismissed)!="") 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			SaveProgram();
			if(_hasProgramPropertyChanged) {
				DataValid.SetInvalid(InvalidType.Programs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}