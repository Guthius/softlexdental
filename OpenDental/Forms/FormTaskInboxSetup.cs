using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormTaskInboxSetup:ODForm {
		private List<User> UserList;
		private List<User> UserListOld;
		private List<TaskList> TrunkList;

		public FormTaskInboxSetup() {
			InitializeComponent();
			
		}

		private void FormTaskInboxSetup_Load(object sender,EventArgs e) {
			UserList=Userods.GetDeepCopy(true);
			UserListOld=Userods.GetDeepCopy(true);
			TrunkList=TaskLists.RefreshMainTrunk(Security.CurrentUser.Id,TaskType.All);
			listMain.Items.Add(Lan.g(this,"none"));
			for(int i=0;i<TrunkList.Count;i++){
				listMain.Items.Add(TrunkList[i].Descript);
			}
			FillGrid();
		}

		private void FillGrid(){
			//doesn't refresh from db because nothing actually gets saved until we hit the OK button.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableTaskSetup","User"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTaskSetup","Inbox"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<UserList.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(UserList[i].UserName);
				row.Cells.Add(GetDescription(UserList[i].TaskListId));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private string GetDescription(long taskListNum) {
			if(taskListNum==0){
				return "";
			}
			for(int i=0;i<TrunkList.Count;i++){
				if(TrunkList[i].TaskListNum==taskListNum){
					return TrunkList[i].Descript;
				}
			}
			return "";
		}

		private void butSet_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a user first.");
				return;
			}
			if(listMain.SelectedIndex==-1){
				MsgBox.Show(this,"Please select an item from the list first.");
				return;
			}
			if(listMain.SelectedIndex==0){
				UserList[gridMain.GetSelectedIndex()].TaskListId=0;
			}
			else{
				UserList[gridMain.GetSelectedIndex()].TaskListId=TrunkList[listMain.SelectedIndex-1].TaskListNum;
			}
			FillGrid();
			listMain.SelectedIndex=-1;
		}

		private void butOK_Click(object sender,EventArgs e) {
			bool changed=false;
			Dictionary<string,List<User>> dictFailedUserUpdates=new Dictionary<string, List<User>>();
			for(int i=0;i<UserList.Count;i++){
				if(UserList[i].TaskListId!=UserListOld[i].TaskListId){
					try {
						Userods.Update(UserList[i]);
						changed=true;
					}
					catch(Exception ex) {
						if(!dictFailedUserUpdates.ContainsKey(ex.Message)){
							dictFailedUserUpdates.Add(ex.Message,new List<User>());
						}
						dictFailedUserUpdates[ex.Message].Add(UserList[i]);
					}
				}
			}
			if(dictFailedUserUpdates.Count>0) {//Inform user that user inboxes could not be updated.
				StringBuilder sb=new StringBuilder();
				foreach(string exceptionMsgKey in dictFailedUserUpdates.Keys) {
					foreach(User user in dictFailedUserUpdates[exceptionMsgKey]) {
						sb.AppendLine("  "+user.UserName+" - "+exceptionMsgKey);
					}
				}
				MessageBox.Show(this,Lans.g(this,"The following users could not be updated:\r\n")+sb.ToString());
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Security);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}