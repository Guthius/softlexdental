using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormUserPrefAdditional:ODForm {
		/// <summary>This is a list of providerclinic rows that were given to this form, containing any modifications that were made while in FormProvAdditional.</summary>
		public List<UserPreference> ListUserPrefOut=new List<UserPreference>();
		private User _userCur;
		private List<UserPreference> _listUserPref;

		public FormUserPrefAdditional(List<UserPreference> listUserPref,User userCur) {
			InitializeComponent();
			_listUserPref=listUserPref.Select(x => x.Clone()).ToList();
			_userCur=userCur.Copy();
		}

		private void FormProvAdditional_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridUserProperties.BeginUpdate();
			gridUserProperties.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableUserPrefProperties","Clinic"),120);
			gridUserProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUserPrefProperties","DoseSpot User ID"),120, isEditable: true);
			gridUserProperties.Columns.Add(col);
			gridUserProperties.Rows.Clear();
			ODGridRow row;
			UserPreference userPrefDefault=_listUserPref.Find(x => x.ClinicId==0);
			//Doesn't exist in Db, create one
			if(userPrefDefault==null) {
				userPrefDefault=UserOdPrefs.GetByCompositeKey(_userCur.Id,Programs.GetCur(ProgramName.eRx).ProgramNum,UserPreferenceName.Program,0);
				//Doesn't exist in db, add to list to be synced later
				_listUserPref.Add(userPrefDefault);
			}
			row=new ODGridRow();
			row.Cells.Add("Default");
			row.Cells.Add(userPrefDefault.Value);
			row.Tag=userPrefDefault;
			gridUserProperties.Rows.Add(row);
			foreach(Clinic clinicCur in Clinics.GetForUserod(Security.CurUser)) {
				row=new ODGridRow();
				UserPreference userPrefCur=_listUserPref.Find(x => x.ClinicId==clinicCur.ClinicNum);
				//wasn't in list, check Db and create a new one if needed
				if(userPrefCur==null) {
					userPrefCur=UserOdPrefs.GetByCompositeKey(_userCur.Id,Programs.GetCur(ProgramName.eRx).ProgramNum,UserPreferenceName.Program,clinicCur.ClinicNum);
					//Doesn't exist in db, add to list to be synced later
					_listUserPref.Add(userPrefCur);
				}
				row.Cells.Add(clinicCur.Abbr);		
				row.Cells.Add(userPrefCur.Value);
				row.Tag=userPrefCur;
				gridUserProperties.Rows.Add(row);
			}
			gridUserProperties.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void gridProvProperties_CellLeave(object sender,ODGridClickEventArgs e) {
			string newDoseSpotID=PIn.String(gridUserProperties.Rows[e.Row].Cells[e.Column].Text);
			UserPreference userPref=(UserPreference)gridUserProperties.Rows[e.Row].Tag;
			userPref.Value=newDoseSpotID;
		}

		private void butOK_Click(object sender,EventArgs e) {
			ListUserPrefOut=new List<UserPreference>();
			foreach(ODGridRow row in gridUserProperties.Rows) {
				ListUserPrefOut.Add((UserPreference)row.Tag);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}