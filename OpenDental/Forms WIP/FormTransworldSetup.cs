using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTransworldSetup:ODForm {
		private Program _progCur;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		///<summary>Dictionary is ClinicNum key with the list of Transworld program properties for the clinic and will be filled with all prog props in
		///the db for Transworld on load.  If a clinic is synced with HQ, i.e. all props for the clinic match those for HQ, the props for that clinic will
		///be deleted from the dict and on form close the sync will delete them from the db and the form will display the HQ 'clinic' details.  Any edit
		///to a clinic synced with HQ will break the sync.</summary>
		private Dictionary<long,List<ProgramPreference>> _dictClinicListProgProps;
		///<summary>If the user changes clinics, this will hold the previous ClinicNum used to save the form details to the local list of props before
		///loading the new clinic props.</summary>
		private long _selectedClinicNum;

		public FormTransworldSetup() {
			InitializeComponent();
			
		}

		private void FormTransworldSetup_Load(object sender,EventArgs e) {
			_progCur=Programs.GetCur(ProgramName.Transworld);
			if(_progCur==null) {
				MsgBox.Show(this,"The Transworld entry is missing from the database.  Please contact support.");//should never happen
				return;
			}
			checkEnabled.Checked=_progCur.Enabled;

				groupClinicSettings.Text=Lan.g(this,"Transworld Clinic Settings");
				_listUserClinicNums=new List<long>();
				//if Transworld is enabled and the user is restricted to a clinic, don't allow the user to disable for all clinics
				if(Security.CurrentUser.ClinicRestricted) {
					if(checkEnabled.Checked) {
						checkEnabled.Enabled=false;
					}
				}
				else {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					//this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
					_listUserClinicNums.Add(0);
					comboClinic.SelectedIndex=0;
				}
				List<Clinic> listClinics=Clinic.GetByUser(Security.CurrentUser).ToList();
				foreach(Clinic clinicCur in listClinics) {
					comboClinic.Items.Add(clinicCur.Abbr);
					_listUserClinicNums.Add(clinicCur.Id);
					if(Clinics.ClinicId==clinicCur.Id) {//set selected index to the currently selected clinic in FormOpenDental
						_selectedClinicNum=clinicCur.Id;
						comboClinic.SelectedIndex=comboClinic.Items.Count-1;
					}
				}
			
			//_dictClinicListProgProps=ProgramProperties.GetForProgram(_progCur.ProgramNum)//get list of all props for the program
			//	.GroupBy(x => x.ClinicId)//group each clinic
			//	.ToDictionary(x => x.Key,x => x.ToList());//turn list into a dictionary of key=ClinicNum, value=List<ProgramProperty> for the clinic
			//DateTime dateTSend=Preference.GetDateTime(PreferenceName.TransworldServiceTimeDue);
			//if(dateTSend!=DateTime.MinValue) {
			//	textUpdatesTimeOfDay.Text=dateTSend.ToShortTimeString();
			//}
			comboSendFrequencyUnits.Items.AddRange(Enum.GetNames(typeof(FrequencyUnit)));
			string[] sendFreqStrs=Preference.GetString(PreferenceName.TransworldServiceSendFrequency).Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
			if(sendFreqStrs.Length==2) {
				int sendFreq=PIn.Int(sendFreqStrs[0],false);
				FrequencyUnit sendFreqUnit;
				if(sendFreq>0 && Enum.TryParse(sendFreqStrs[1],out sendFreqUnit)) {
					numericSendFrequency.Value=sendFreq;
					comboSendFrequencyUnits.SelectedIndex=(int)sendFreqUnit;
				}
			}
			else {//if not set, default to repeat once a day
				comboSendFrequencyUnits.SelectedIndex=(int)FrequencyUnit.Days;
				numericSendFrequency.Value=1;
			}
			FillComboBoxes();
			FillFields();
		}

		/// <summary>Fill the combo boxes with items. Some will have their indicies set later in FillFields() </summary>
		private void FillComboBoxes() {
			foreach(Definition defCur in Definition.GetByCategory(DefinitionCategory.BillingTypes).Where(x => x.Value.ToLower()!="c")) {
				comboPaidInFullBillType.Items.Add(new ODBoxItem<Definition>(defCur.Description,defCur.Copy()));
				if(defCur.Id==Preference.GetLong(PreferenceName.TransworldPaidInFullBillingType)) {
					comboPaidInFullBillType.SelectedIndex=comboPaidInFullBillType.Items.Count-1;
				}
			}
			ODBoxItem<Definition> noneDef=new ODBoxItem<Definition>("None",new Definition());
			comboPosAdjType.Items.Add(noneDef);
			comboPosAdjType.SelectedIndex=0;
			foreach(Definition defCur in Definition.GetByCategory(DefinitionCategory.AdjTypes).Where(x => x.Value.Contains("+"))) {
				comboPosAdjType.Items.Add(new ODBoxItem<Definition>(defCur.Description,defCur));
			}
			comboNegAdjType.Items.Add(noneDef);
			comboNegAdjType.SelectedIndex=0;
			foreach(Definition defCur in Definition.GetByCategory(DefinitionCategory.AdjTypes).Where(x => x.Value.Contains("-"))) {
				comboNegAdjType.Items.Add(new ODBoxItem<Definition>(defCur.Description,defCur));
			}
		}

		///<summary>Details displayed in form may be for HQ clinic and not the currently selected clinic if the current clinic is synced with HQ.  If the
		///values are modified and the currently selected clinic is not the HQ clinic but the HQ details are being displayed, the HQ clinic details will
		///remain unchanged and the currently selected clinic will no longer be synced with HQ and will have a set of props added to the dict.</summary>
		private void FillFields() {
			long clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			
			List<ProgramProperty> listPropsCurClinic;

			if(!_dictClinicListProgProps.TryGetValue(clinicNum,out listPropsCurClinic)) {
				listPropsCurClinic=_dictClinicListProgProps[0];//dictionary guaranteed to have ClinicNum 0 in it
			}
			foreach(ProgramPreference propCur in listPropsCurClinic) {
				switch(propCur.Key) {
					case "SftpServerAddress":
						textSftpAddress.Text=propCur.Value;
						continue;
					case "SftpServerPort":
						textSftpPort.Text=propCur.Value;
						continue;
					case "SftpUsername":
						textSftpUsername.Text=propCur.Value;
						continue;
					case "SftpPassword":
						textSftpPassword.Text=propCur.Value;
						continue;
					case "ClientIdAccelerator":
						textClientIdAccelerator.Text=propCur.Value;
						continue;
					case "ClientIdCollection":
						textClientIdCollection.Text=propCur.Value;
						continue;
					case "IsThankYouLetterEnabled":
						checkThankYouLetter.Checked=PIn.Bool(propCur.Value);
						continue;
					case "SelectedServices":
						checkAccelService.Checked=propCur.Value.Contains(((int)TsiDemandType.Accelerator).ToString());
						checkPRService.Checked=propCur.Value.Contains(((int)TsiDemandType.ProfitRecovery).ToString());
						checkCollService.Checked=propCur.Value.Contains(((int)TsiDemandType.Collection).ToString());
						continue;
					case "SyncExcludePosAdjType":
							comboPosAdjType.SetSelectedItem<Definition>(x => x.Id==PIn.Long(propCur.Value),"");
						continue;
					case "SyncExcludeNegAdjType":
							comboNegAdjType.SetSelectedItem<Definition>(x => x.Id==PIn.Long(propCur.Value),"");
						continue;
				}
			}
			SetAdvertising();
		}
		
		///<summary>Handles both visibility and checking of checkHideButtons.</summary>
		private void SetAdvertising() {
			ProgramPreference prop=_dictClinicListProgProps[0].FirstOrDefault(x => x.Key=="Disable Advertising");//dict guaranteed to contain key 0
			checkHideButtons.Visible=(prop!=null && !checkEnabled.Checked);//show check box if disable prop exists and program is not enabled
			checkHideButtons.Checked=(prop?.Value=="1");//check box checked if disable prop exists and is set to "1" for HQ
		}

		private void checkEnabled_Click(object sender,EventArgs e) {
			SetAdvertising();
		}
		
		///<summary>Saves form data to the dict and then removes any clinics from the dict that exactly match the HQ clinic details.  If editing a clinic
		///other than HQ and there are no props for that clinic and the form values are different than HQ, a new list is added to the dict.</summary>
		private void SyncWithHQ() {
			List<ProgramPreference> listHqProps=_dictClinicListProgProps[0];//dict guaranteed to contain ClinicNum 0
			List<ProgramPreference> listPropsCur;
			if(!_dictClinicListProgProps.TryGetValue(_selectedClinicNum,out listPropsCur)) {
				//if there isn't a list of props for the clinic, create a new list for comparison and possibly for inserting for the clinic
				listPropsCur=listHqProps.Select(x => new ProgramPreference() {
					ProgramId=x.ProgramId,
					ClinicId=_selectedClinicNum,
					Key=x.Key
				}).ToList();
			}
			//these are the props that will be synced with HQ and used to determine whether a clinic's props should be deleted or used instead of the HQ props
			//the disable advert and disable advert HQ props are only for the HQ clinic and will be ignored for syncing a clinic
			string[] listSyncedProps=new[] { "SftpServerAddress","SftpServerPort","SftpUsername","SftpPassword","ClientIdAccelerator","ClientIdCollection",
				"IsThankYouLetterEnabled","SelectedServices","SyncExcludePosAdjType","SyncExcludeNegAdjType" };
			foreach(ProgramPreference propCur in listPropsCur) {//update the currently selected props with the current form values
				switch(propCur.Key) {
					case "SftpServerAddress":
						propCur.Value=textSftpAddress.Text;
						continue;
					case "SftpServerPort":
						propCur.Value=textSftpPort.Text;
						continue;
					case "SftpUsername":
						propCur.Value=textSftpUsername.Text;
						continue;
					case "SftpPassword":
						propCur.Value=textSftpPassword.Text;
						continue;
					case "ClientIdAccelerator":
						propCur.Value=textClientIdAccelerator.Text;
						continue;
					case "ClientIdCollection":
						propCur.Value=textClientIdCollection.Text;
						continue;
					case "IsThankYouLetterEnabled":
						propCur.Value=POut.Bool(checkThankYouLetter.Checked);
						continue;
					case "Disable Advertising":
						_dictClinicListProgProps.Values.SelectMany(x => x.Where(y => y.Key=="Disable Advertising")).ToList()
							.ForEach(y => y.Value=POut.Bool(checkHideButtons.Checked));
						propCur.Value=POut.Bool(checkHideButtons.Checked);//in case list is for a new clinic and not in dict
						continue;
					case "Disable Advertising HQ":
						//false if prop is null or if the value is anything but "1"
						bool isAdvertDisabledHQ=(listHqProps.FirstOrDefault(x => x.Key=="Disable Advertising HQ")?.Value=="1");
						_dictClinicListProgProps.Values.SelectMany(x => x.Where(y => y.Key=="Disable Advertising HQ")).ToList()
							.ForEach(x => x.Value=POut.Bool(isAdvertDisabledHQ));//in case list is for a new clinic and not in dict
						propCur.Value=POut.Bool(isAdvertDisabledHQ);
						continue;
					case "SelectedServices":
						List<int> selectedServices=new List<int>();
						if(checkAccelService.Checked) {
							selectedServices.Add((int)TsiDemandType.Accelerator);
						}
						if(checkPRService.Checked) {
							selectedServices.Add((int)TsiDemandType.ProfitRecovery);
						}
						if(checkCollService.Checked) {
							selectedServices.Add((int)TsiDemandType.Collection);
						}
						propCur.Value=string.Join(",",selectedServices);
						continue;
					case "SyncExcludePosAdjType":
							propCur.Value=comboPosAdjType.SelectedTag<Definition>().Id.ToString();
						continue;
					case "SyncExcludeNegAdjType":
							propCur.Value=comboNegAdjType.SelectedTag<Definition>().Id.ToString();
						continue;
				}
			}
			if(_selectedClinicNum==0) {//if HQ selected
				_dictClinicListProgProps.ToList()//go through all clinic properties
					.RemoveAll(x => x.Key>0 //remove the non-HQ clinic props
						&& x.Value.All(y => !listSyncedProps.Contains(y.Key)//the prop is an HQ only prop, i.e. the clinic prop is ignored
							|| listHqProps.Any(z => z.Key==y.Key && z.Value==y.Value)));//have matching HQ prop desc and value
			}
			else {
				if(listPropsCur.All(x => !listSyncedProps.Contains(x.Key)//if all props for the non-HQ clinic are HQ only props, i.e. the clinic prop is ignored
					|| listHqProps.Any(y => y.Key==x.Key && y.Value==x.Value)))//have matching HQ prop desc and value
				{
					//remove non-HQ clinic props, they are synced with HQ
					_dictClinicListProgProps.Remove(_selectedClinicNum);//does not throw exception if dict doesn't contain the key! (according to MSDN)
				}
				else if(!_dictClinicListProgProps.ContainsKey(_selectedClinicNum)) {//if the clinic is not in the dict (otherwise values are already updated)
					//add non-HQ clinic to the dict if any prop for the non-HQ clinic is different than the HQ clinic and it's not already in the dict
					_dictClinicListProgProps.Add(_selectedClinicNum,listPropsCur);
				}
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex<0
				|| comboClinic.SelectedIndex>=_listUserClinicNums.Count
				|| _listUserClinicNums[comboClinic.SelectedIndex]==_selectedClinicNum)//didn't change the selected clinic
			{
				return;
			}
			if(!string.IsNullOrEmpty(textSftpPort.errorProvider1.GetError(textSftpPort))) {
				MsgBox.Show(this,"Please enter a valid integer for the Sftp Server Port.");
				comboClinic.SelectedIndex=_listUserClinicNums.IndexOf(_selectedClinicNum);
				return;
			}
			//save form values to dict before filling fields with selected clinic data
			SyncWithHQ();
			_selectedClinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			FillFields();
		}

		private void comboSendFrequencyUnits_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboSendFrequencyUnits.SelectedIndex<0 || comboSendFrequencyUnits.SelectedIndex>Enum.GetNames(typeof(FrequencyUnit)).Length) {
				return;
			}			
			if(comboSendFrequencyUnits.SelectedIndex==(int)FrequencyUnit.Days) {
				numericSendFrequency.Value=Math.Min(numericSendFrequency.Value,30);
				numericSendFrequency.Maximum=30;
			}
			else if(comboSendFrequencyUnits.SelectedIndex==(int)FrequencyUnit.Hours) {
				numericSendFrequency.Value=Math.Min(numericSendFrequency.Value,24);
				numericSendFrequency.Maximum=24;
			}
			else if(comboSendFrequencyUnits.SelectedIndex==(int)FrequencyUnit.Minutes) {
				numericSendFrequency.Value=Math.Min(numericSendFrequency.Value,60);
				numericSendFrequency.Maximum=60;
			}
		}

		private void checkAccelService_CheckedChanged(object sender,EventArgs e) {
			textClientIdAccelerator.Enabled=checkAccelService.Checked;
			labelClientIdAccelerator.Enabled=checkAccelService.Checked;
		}

		private void checkPRService_CheckedChanged(object sender,EventArgs e) {
			textClientIdCollection.Enabled=(checkPRService.Checked || checkCollService.Checked);
			labelClientIdCollection.Enabled=(checkPRService.Checked || checkCollService.Checked);
		}

		private void checkCollService_CheckedChanged(object sender,EventArgs e) {
			textClientIdCollection.Enabled=(checkPRService.Checked || checkCollService.Checked);
			labelClientIdCollection.Enabled=(checkPRService.Checked || checkCollService.Checked);
		}

		private void butOK_Click(object sender,EventArgs e) {
			//if(!string.IsNullOrEmpty(textSftpPort.errorProvider1.GetError(textSftpPort))) {
			//	MsgBox.Show(this,"Please enter a valid integer for the Sftp Server Port.");
			//	return;
			//}
			//int sendFreq=(int)numericSendFrequency.Value;
			//DateTime accountUpdatesRuntime=DateTime.MinValue;
			//if(!string.IsNullOrWhiteSpace(textUpdatesTimeOfDay.Text) && !DateTime.TryParse(textUpdatesTimeOfDay.Text,out accountUpdatesRuntime)) {
			//	MsgBox.Show(this,"Account Updates Run Time must be blank or a valid time of day.");
			//	return;
			//}
			//if(comboSendFrequencyUnits.SelectedIndex<0 || comboSendFrequencyUnits.SelectedIndex>=Enum.GetNames(typeof(FrequencyUnit)).Length) {
			//	//shouldn't be possible, but just in case
			//	MsgBox.Show(this,"Please select a valid unit of measurement for the Account Activity Updates repeat frequency.");
			//	return;
			//}
			//if(numericSendFrequency.Value<1 || numericSendFrequency.Value>new[] { 30,24,60 }[comboSendFrequencyUnits.SelectedIndex]) {
			//	//shouldn't be possible, but just in case
			//	MsgBox.Show(this,"Please enter a valid value for the Account Activity Updates repeat frequency.");
			//	return;
			//}
			//long billTypePaidInFullDefNum=comboPaidInFullBillType.SelectedTag<Definition>()?.Id??0;
			//if(billTypePaidInFullDefNum==0 && checkEnabled.Checked) {
			//	MsgBox.Show(this,"Please select a Paid in Full Billing Type.");
			//	return;
			//}
			//SyncWithHQ();//will remove any clinic from the dict if all props exactly match the HQ props, or add clinic props if different
			//if(_progCur.Enabled!=checkEnabled.Checked) {//only update the program if the IsEnabled flag has changed
			//	_progCur.Enabled=checkEnabled.Checked;
			//	Programs.Update(_progCur);
			//}
			//ProgramProperties.Sync(_dictClinicListProgProps.Where(x => _listUserClinicNums.Contains(x.Key)).SelectMany(x => x.Value).ToList(),_progCur.ProgramNum,_listUserClinicNums);
			//DataValid.SetInvalid(InvalidType.Programs);
			//string updateFreq=numericSendFrequency.Value+" "+(FrequencyUnit)comboSendFrequencyUnits.SelectedIndex;
			//bool hasChanged=false;
			//if(Preference.Update(PreferenceName.TransworldServiceTimeDue,accountUpdatesRuntime==DateTime.MinValue?"":POut.Time(accountUpdatesRuntime.TimeOfDay,false))
			//	| Preference.Update(PreferenceName.TransworldServiceSendFrequency,updateFreq))
			//{
			//	Preference.Update(PreferenceName.TransworldDateTimeLastUpdated,DateTime.MinValue);
			//	hasChanged=true;
			//}
			//if(Preference.Update(PreferenceName.TransworldPaidInFullBillingType,billTypePaidInFullDefNum) | hasChanged) {
			//	DataValid.SetInvalid(InvalidType.Prefs);
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}