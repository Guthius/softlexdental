using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormDashboardWidgetSetup:ODForm {
		///<summary>The currently logged user's first user group.</summary>
		private long _userGroupNum;
		private List<UserGroup> _listUserGroups;
		private List<GroupPermission> _listGroupPermissions;
		private List<GroupPermission> _listGroupPermissionsOld;
		///<summary>The index of the Allowed column in gridCustom.</summary>
		private int _colAllowed;

		public FormDashboardWidgetSetup() {
			InitializeComponent();
			
			_userGroupNum=UserGroup.GetByUser(Security.CurUser.UserNum).FirstOrDefault().Id;
		}
		
		private void FormDashboardSetup_Load(object sender,EventArgs e) {
			InitializeOnStartup();
			FillGridInternal();
			FillGridCustom();
			SetFilterControlsAndAction(() => FillGridCustom(),comboUserGroup);
		}

		private void InitializeOnStartup() {
            _listUserGroups = UserGroup.All();
			comboUserGroup.SetItems(_listUserGroups,x => x.Description,x => x.Id==_userGroupNum);
			if(comboUserGroup.SelectedIndex==-1) {
				comboUserGroup.SelectedIndex=0;
			}
			_listGroupPermissions=GroupPermissions.GetForUserGroups(_listUserGroups.Select(x => x.Id).ToList(),Permissions.DashboardWidget);
			_listGroupPermissionsOld=_listGroupPermissions.Select(x => x.Copy()).ToList();
		}

		private void FillGridInternal() {
			FillGrid(gridInternal,SheetsInternal.GetSheetDef(SheetInternalType.PatientDashboard));
		}

		private void FillGridCustom() {
			SheetDefs.RefreshCache();
			SheetFieldDefs.RefreshCache();
			FillGrid(gridCustom,SheetDefs.GetCustomForType(SheetTypeEnum.PatientDashboardWidget).ToArray());
		}

		private void FillGrid(ODGrid grid,params SheetDef[] arrDashboardSheetDefs) {
			List<SheetDef> listSelectedDashboards=grid.SelectedTags<SheetDef>();
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new ODGridColumn("Name",0,HorizontalAlignment.Left));
			if(grid==gridCustom) {
				grid.Columns.Add(new ODGridColumn("Allowed",50,HorizontalAlignment.Center));
				_colAllowed=gridCustom.Columns.Count-1;//Dynamically determines the 'Allowed' column index in case we add others later.
			}
			grid.Rows.Clear();
			foreach(SheetDef sheetDefWidget in arrDashboardSheetDefs) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(sheetDefWidget.Description);
				if(grid==gridCustom) {
					bool isAllowed=IsUserGroupAllowed(sheetDefWidget,comboUserGroup.SelectedTag<UserGroup>());
					row.Cells.Add(isAllowed ? "X":"");
				}
				row.Tag=sheetDefWidget;
				grid.Rows.Add(row);
			}
			grid.EndUpdate();
			for(int i=0;i<grid.Rows.Count;i++) {
				if(((SheetDef)grid.Rows[i].Tag).SheetDefNum.In(listSelectedDashboards.Select(x => x.SheetDefNum))) {
					grid.SetSelected(i,true);
				}
			}
		}

		///<summary>Determines if there is a GroupPermission for any of the arrayUserGroups that matches the sheetDashboard.</summary>
		private bool IsUserGroupAllowed(SheetDef sheetDefWidget,UserGroup userGroup) {
			if(_listGroupPermissions.Any(x => x.UserGroupNum==userGroup.Id && x.FKey==sheetDefWidget.SheetDefNum)) {
				return true;
			}
			return false;
		}

		private bool ToggleDashboardPermission(UserGroup userGroup,SheetDef sheetDefWidget) {
			GroupPermission selectedGroupPermission=_listGroupPermissions
				.FirstOrDefault(x => x.UserGroupNum==userGroup.Id && x.FKey==sheetDefWidget.SheetDefNum);
			if(selectedGroupPermission==null) {
				GroupPermission groupPermission=new GroupPermission() {
					NewerDate=DateTime.MinValue,
					NewerDays=0,
					PermType=Permissions.DashboardWidget,
					UserGroupNum=userGroup.Id,
					FKey=sheetDefWidget.SheetDefNum,
				};
				_listGroupPermissions.Add(groupPermission);
				return true;
			}
			else {
				_listGroupPermissions.Remove(selectedGroupPermission);//Clear permission to this dashboard for this userGroup.
				return false;
			}
		}

		private void SetDashboardPermission(UserGroup userGroup,params SheetDef[] arraySheetDefWidgets) {
			foreach(SheetDef sheetDefWidget in arraySheetDefWidgets) {
				GroupPermission selectedGroupPermission=_listGroupPermissions
					.FirstOrDefault(x => x.UserGroupNum==userGroup.Id && x.FKey==sheetDefWidget.SheetDefNum);
				if(selectedGroupPermission==null) {
					GroupPermission groupPermission=new GroupPermission() {
						NewerDate=DateTime.MinValue,
						NewerDays=0,
						PermType=Permissions.DashboardWidget,
						UserGroupNum=userGroup.Id,
						FKey=sheetDefWidget.SheetDefNum,
					};
					_listGroupPermissions.Add(groupPermission);
				}
			}
		}

		///<summary>Opens a SheetDefEdit window and returns the SheetDef.SheetDefNum.  Returns -1 if user cancels or deletes the Dashboard.</summary>
		private long EditWidget(SheetDef sheetDefWidget=null) {
			if(sheetDefWidget==null) {
				sheetDefWidget=new SheetDef() {
					SheetType=SheetTypeEnum.PatientDashboardWidget,
					FontName="Microsoft Sans Serif",
					FontSize=9,
					Height=800,
					Width=400,
				};
				FormSheetDef FormSD=new FormSheetDef();
				FormSD.SheetDefCur=sheetDefWidget;
				FormSD.ShowDialog();
				if(FormSD.DialogResult!=DialogResult.OK) {
					return -1;
				}
				sheetDefWidget.SheetFieldDefs=new List<SheetFieldDef>();
				sheetDefWidget.IsNew=true;
			}
			SheetDefs.GetFieldsAndParameters(sheetDefWidget);
			FormSheetDefEdit FormS=new FormSheetDefEdit(sheetDefWidget);
			if(FormS.ShowDialog()==DialogResult.OK) {
				DataValid.SetInvalid(InvalidType.Sheets);
				return sheetDefWidget.SheetDefNum;
			}
			return -1;
		}

		private void gridInternal_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridInternal.SelectedGridRows.Count==0) {
				return;
			}
			FormSheetDefEdit FormS=new FormSheetDefEdit(gridInternal.SelectedTag<SheetDef>());
			FormS.IsInternal=true;
			FormS.ShowDialog();
		}

		private void gridCustom_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridCustom.SelectedCell.X==-1 || gridCustom.SelectedCell.Y==-1) {//Invalid cell.
				return;
			}
			if(gridCustom.SelectedCell.X==_colAllowed) {//Do not open the edit window when double clicking the 'Allowed' column.
				return;
			}
			EditWidget(gridCustom.SelectedTag<SheetDef>());
			FillGridCustom();
		}

		private void gridCustom_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridCustom.SelectedCell.X==-1 || gridCustom.SelectedCell.Y==-1) {//Invalid cell.
				return;
			}
			if(gridCustom.SelectedCell.X!=_colAllowed) {//Not setting/unsetting permission.
				return;
			}
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			bool isAllowed=ToggleDashboardPermission(comboUserGroup.SelectedTag<UserGroup>(),gridCustom.SelectedTag<SheetDef>());
			gridCustom.BeginUpdate();
			gridCustom.SelectedGridRows[0].Cells[_colAllowed].Text=(isAllowed ? "X":"");
			gridCustom.EndUpdate();
		}

		private void gridCustom_TitleAddClick(object sender,EventArgs e) {
			long sheetDefWidgetNum=EditWidget();//Adding a new Dashboard Widget.
			if(sheetDefWidgetNum==-1) {
				return;
			}
			FillGridCustom();
			for(int i=0;i<gridCustom.Rows.Count;i++) {
				if(((SheetDef)gridCustom.Rows[i].Tag).SheetDefNum==sheetDefWidgetNum) {
					gridCustom.SetSelected(i,true);
				}
			}
		}

		private void butSetAll_Click(object sender,EventArgs e) {
			SetDashboardPermission(comboUserGroup.SelectedTag<UserGroup>(),gridCustom.GetTags<SheetDef>().ToArray());
			FillGridCustom();
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(gridInternal.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an internal sheet first.");
				return;
			}
			SheetDef sheetdef=gridInternal.SelectedTag<SheetDef>().Copy();
			sheetdef.IsNew=true;
			SheetDefs.InsertOrUpdate(sheetdef);
			FillGridCustom();
			gridInternal.SetSelected(false);//Clear selection.
			gridCustom.SetSelectedStrict(new int[] { gridCustom.Rows.Count-1 });
		}

		private void butDuplicate_Click(object sender,EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a sheet from the custom list first.");
				return;
			}
			SheetDef sheetdef=gridCustom.SelectedTag<SheetDef>().Copy();
			sheetdef.Description=sheetdef.Description+"2";
			SheetDefs.GetFieldsAndParameters(sheetdef);
			sheetdef.IsNew=true;
			SheetDefs.InsertOrUpdate(sheetdef);
			FillGridCustom();
			gridInternal.SetSelected(false);//Clear selection.
			gridCustom.SetSelectedStrict(new int[] { gridCustom.Rows.Count-1 });
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(GroupPermissions.Sync(_listGroupPermissions,_listGroupPermissionsOld)) {
				DataValid.SetInvalid(InvalidType.Security);
			}
			DialogResult=DialogResult.OK;
		}

    private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}