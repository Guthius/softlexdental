using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormDoseSpotAssignUserId:ODForm {
		private List<User> _listUsersInComboBox;
		private long _selectedUserNum;
		private ProviderErx _providerErxCur;
		Program _programErx;

		public FormDoseSpotAssignUserId(long provErxNum) {
			InitializeComponent();
			//get providerErx from provErxNum that was passed in
			_providerErxCur=ProviderErxs.GetFirstOrDefault(x => x.ProviderErxNum==provErxNum);
			
		}

		private void FormDoseSpotAssignUserId_Load(object sender,EventArgs e) {
			//_programErx=Programs.GetCur(ProgramName.eRx);
			//_listUsersInComboBox=GetListDoseSpotUsers(true,_providerErxCur.NationalProviderID);
			//if(_listUsersInComboBox.Count==0) {//empty list, populate with all users
			//	_listUsersInComboBox=GetListDoseSpotUsers(false);
			//}
			//FillComboBox();
			//textUserId.Text=_providerErxCur.UserId;//UserID passed from Alert
		}

		private List<User> GetListDoseSpotUsers(bool includeProv,string provNpi="") {
			List<User> retVal=new List<User>();
			//List<Provider> listProviders=Providers.GetWhere(x => x.NationalProvID==provNpi,true);
			//List<UserPreference> listUserPrefDoseSpotIds=UserOdPrefs.GetAllByFkeyAndFkeyType(_programErx.ProgramNum,UserPreferenceName.Program);
			//listUserPrefDoseSpotIds=listUserPrefDoseSpotIds.FindAll(x => string.IsNullOrWhiteSpace(x.Value));
			//if(includeProv) {
			//	retVal=Userods.GetWhere(
			//		(x => listProviders.Exists(y => y.ProvNum==x.ProviderId) //Find users that have a link to the NPI that has been passed in
			//			&& !listUserPrefDoseSpotIds.Exists(y => y.UserId==x.Id)) //Also, these users shouldn't already have a DoseSpot User ID.
			//		,true);//Only consider non-hidden users.
			//}
			//else {
			//	retVal=Userods.GetWhere(
			//		(x => !listUserPrefDoseSpotIds.Exists(y => y.UserId==x.Id)) //All users that don't already have a DoseSpot User ID.
			//		,true);//Only consider non-hidden users.
			//}
			return retVal;
		}

		private void FillComboBox() {
			comboDoseUsers.Items.Clear();
			comboDoseUsers.Items.Add(new ODBoxItem<User>(Lan.g(this,"None")));
			comboDoseUsers.SelectedIndex=0;
			foreach(User userCur in _listUsersInComboBox) {
				ODBoxItem<User> boxItemCur=new ODBoxItem<User>(userCur.UserName,userCur);
				comboDoseUsers.Items.Add(boxItemCur);
				if(userCur.Id==_selectedUserNum) {
					comboDoseUsers.SelectedIndex=comboDoseUsers.Items.Count-1;//Select The item that was just added if it is the selected num.
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//if(_selectedUserNum==0) {
			//	MsgBox.Show(this,"Please select a user.");
			//	return;
			//}
			//UserPreference userDosePref=UserOdPrefs.GetByCompositeKey(_selectedUserNum,_programErx.ProgramNum,UserPreferenceName.Program);
			//userDosePref.Value=_providerErxCur.UserId.ToString();
			//if(userDosePref.IsNew) {
			//	userDosePref.Fkey=_programErx.ProgramNum;
			//	UserOdPrefs.Insert(userDosePref);
			//}
			//else { 
			//	UserOdPrefs.Update(userDosePref);
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butUserPick_Click(object sender,EventArgs e) {
			FormUserPick formUP=new FormUserPick();
			formUP.IsSelectionMode=true;
			formUP.Users=_listUsersInComboBox;
			formUP.IsPickAllAllowed=false;
			formUP.ShowDialog();
			if(formUP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedUserNum=formUP.SelectedUserId;
			FillComboBox();
		}

		private void comboDoseUsers_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboDoseUsers.SelectedIndex==0) {
				_selectedUserNum=0;
				return;
			}
			_selectedUserNum=_listUsersInComboBox[comboDoseUsers.SelectedIndex-1].Id;
		}
	}
}