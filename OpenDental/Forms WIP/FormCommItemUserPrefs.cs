using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormCommItemUserPrefs:ODForm {
		///<summary>Helper variable that gets set to Security.CurUser.UserNum on load.</summary>
		private long _userNumCur;
		private UserPreference _userOdPrefClearNote;
		private UserPreference _userOdPrefEndDate;
		private UserPreference _userOdPrefUpdateDateTimeNewPat;

		public FormCommItemUserPrefs() {
			InitializeComponent();
			
		}

		private void FormCommItemUserPrefs_Load(object sender,EventArgs e) {
			if(Security.CurrentUser==null || Security.CurrentUser.Id < 1) {
				MsgBox.Show(this,"Invalid user currently logged in.  No user preferences can be saved.");
				DialogResult=DialogResult.Abort;
				return;
			}
			_userNumCur=Security.CurrentUser.Id;
			//Add the user name of the user currently logged in to the title of this window much like we do for FormOpenDental.
			this.Text+=" {"+Security.CurrentUser.UserName+"}";
			_userOdPrefClearNote=UserPreference.GetByKey(_userNumCur,UserPreferenceName.CommlogPersistClearNote);
			_userOdPrefEndDate= UserPreference.GetByKey(_userNumCur,UserPreferenceName.CommlogPersistClearEndDate);
			_userOdPrefUpdateDateTimeNewPat= UserPreference.GetByKey(_userNumCur,UserPreferenceName.CommlogPersistUpdateDateTimeWithNewPatient);
			checkCommlogPersistClearNote.Checked=(_userOdPrefClearNote==null) ? true : PIn.Bool(_userOdPrefClearNote.Value);
			checkCommlogPersistClearEndDate.Checked=(_userOdPrefEndDate==null) ? true : PIn.Bool(_userOdPrefEndDate.Value);
			checkCommlogPersistUpdateDateTimeWithNewPatient.Checked=(_userOdPrefUpdateDateTimeNewPat==null) ? true : PIn.Bool(_userOdPrefUpdateDateTimeNewPat.Value);
		}

		///<summary>Helper method to update or insert the passed in UserOdPref utilizing the specified valueString and keyType.
		///If the user pref passed in it null then a new user pref will be inserted.  Otherwise the user pref is updated.</summary>
		//private void UpsertUserOdPref(UserPreference userOdPref,UserPreferenceName keyType,string valueString) {
		//	//if(userOdPref==null) {
		//	//	UserPreference userOdPrefTemp=new UserPreference();
		//	//	userOdPrefTemp.Fkey=0;
		//	//	userOdPrefTemp.FkeyType=keyType;
		//	//	userOdPrefTemp.UserId=_userNumCur;
		//	//	userOdPrefTemp.Value=valueString;
		//	//	UserOdPrefs.Insert(userOdPrefTemp);
		//	//}
		//	//else {
		//	//	userOdPref.FkeyType=keyType;
		//	//	userOdPref.Value=valueString;
		//	//	UserOdPrefs.Update(userOdPref);
		//	//}
		//}

		private void butOK_Click(object sender,EventArgs e) {
			//UpsertUserOdPref(_userOdPrefClearNote
			//	,UserPreferenceName.CommlogPersistClearNote
			//	,POut.Bool(checkCommlogPersistClearNote.Checked));
			//UpsertUserOdPref(_userOdPrefEndDate
			//	,UserPreferenceName.CommlogPersistClearEndDate
			//	,POut.Bool(checkCommlogPersistClearEndDate.Checked));
			//UpsertUserOdPref(_userOdPrefUpdateDateTimeNewPat
			//	,UserPreferenceName.CommlogPersistUpdateDateTimeWithNewPatient
			//	,POut.Bool(checkCommlogPersistUpdateDateTimeWithNewPatient.Checked));
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}