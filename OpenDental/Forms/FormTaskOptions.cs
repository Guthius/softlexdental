using System;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using CodeBase;

namespace OpenDental {
	public partial class FormTaskOptions:ODForm {
		public bool IsShowFinishedTasks;
		public DateTime DateTimeStartShowFinished;
		public bool IsSortApptDateTime;
		private UserPreference _taskCollapsedPref;

		public FormTaskOptions(bool isShowFinishedTasks, DateTime dateTimeStartShowFinished, bool isAptDateTimeSort) {
			InitializeComponent();
			
			checkShowFinished.Checked=isShowFinishedTasks;
			textStartDate.Text=dateTimeStartShowFinished.ToShortDateString();
			checkTaskSortApptDateTime.Checked=isAptDateTimeSort;
			List<UserPreference> listPrefs=UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id,UserPreferenceName.TaskCollapse);
			_taskCollapsedPref=listPrefs.Count==0? null : listPrefs[0];
			checkCollapsed.Checked=_taskCollapsedPref==null ? false : PIn.Bool(_taskCollapsedPref.Value);
			if(!isShowFinishedTasks) {
				labelStartDate.Enabled=false;
				textStartDate.Enabled=false;
			}
		}

		private void checkShowFinished_Click(object sender,EventArgs e) {
			if(checkShowFinished.Checked) {
				labelStartDate.Enabled=true;
				textStartDate.Enabled=true;
			}
			else {
				labelStartDate.Enabled=false;
				textStartDate.Enabled=false;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!(textStartDate.errorProvider1.GetError(textStartDate)=="")) {
				if(checkShowFinished.Checked) {
					MsgBox.Show(this,"Invalid finished task start date");
					return;
				}
				else {
					//We are not going to be using the textStartDate so not reason to warn the user, just reset it back to the default value.
					textStartDate.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
				}
			}
			if(_taskCollapsedPref==null) {
				_taskCollapsedPref=new UserPreference();
				_taskCollapsedPref.Fkey=0;
				_taskCollapsedPref.FkeyType=UserPreferenceName.TaskCollapse;
				_taskCollapsedPref.UserId=Security.CurrentUser.Id;
				_taskCollapsedPref.Value=POut.Bool(checkCollapsed.Checked);
				UserOdPrefs.Insert(_taskCollapsedPref);
			}
			else { 
				_taskCollapsedPref.Value=POut.Bool(checkCollapsed.Checked);
				UserOdPrefs.Update(_taskCollapsedPref);
			}
			IsShowFinishedTasks=checkShowFinished.Checked;
			DateTimeStartShowFinished=PIn.Date(textStartDate.Text);//Note that this may have not been enabled but we'll pass it back anyway, won't be used.
			IsSortApptDateTime=checkTaskSortApptDateTime.Checked;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}