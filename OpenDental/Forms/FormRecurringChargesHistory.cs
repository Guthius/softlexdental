using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRecurringChargesHistory:ODForm {
		///<summary>The list of charges that have most recently been fetched from the database.</summary>
		private List<RecurringCharge> _listRecurringCharges;
		///<summary>Dictionary of patient names. Key is the PatNum.</summary>
		private Dictionary<long,string> _dictPatNames=new Dictionary<long,string>();
		///<summary>The date range that was selected the last time the charges were fetched from the database.</summary>
		private DateRange _previousDateRange;
		///<summary>The clinic nums that were selected the last time the charges were fetched from the database.</summary>
		private List<long> _listPreviousClinicNums;

		public FormRecurringChargesHistory() {
			InitializeComponent();
			
			gridMain.ContextMenu=contextMenu;
		}

		private void FormRecurringChargesHistory_Load(object sender,EventArgs e) {
			datePicker.SetDateTimeFrom(DateTime.Today.AddMonths(-1));
			datePicker.SetDateTimeTo(DateTime.Today);
			if(!Preferences.HasClinicsEnabled) {
				labelClinics.Visible=false;
				butPickClinic.Visible=false;
			}
			comboStatuses.FillWithEnum<RecurringChargeStatus>();
			comboStatuses.SetSelected(true);
			comboAutomated.Items.AddRange(new[] {
				Lans.g(this,"Automated and Manual"),
				Lans.g(this,"Automated Only"),
				Lans.g(this,"Manual Only"),
			});
			comboAutomated.SelectedIndex=0;
			RefreshRecurringCharges();
			FillGrid();
		}

		///<summary>Gets recurring charges from the database.</summary>
		private void RefreshRecurringCharges() {
			Cursor=Cursors.WaitCursor;
			List<SQLWhere> listWheres=new List<SQLWhere> {
				SQLWhere.CreateBetween(nameof(RecurringCharge.DateTimeCharge),datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo(),true)
			};
			if(Preferences.HasClinicsEnabled) {
				listWheres.Add(SQLWhere.CreateIn(nameof(RecurringCharge.ClinicNum),comboClinics.ListSelectedClinicNums));
			}
			_listRecurringCharges=RecurringCharges.GetMany(listWheres);
			if(_listRecurringCharges.Any(x => !_dictPatNames.ContainsKey(x.PatNum))) {
				Dictionary<long,string> dictPatNames=Patients.GetPatientNames(_listRecurringCharges.Select(x => x.PatNum).ToList());
				dictPatNames.ForEach(x => _dictPatNames[x.Key]=x.Value);
			}
			_previousDateRange=new DateRange(datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo());
			_listPreviousClinicNums=comboClinics.ListSelectedClinicNums;
			Cursor=Cursors.Default;
		}

		///<summary>Will refresh charges from the database if necessary.</summary>
		private void FillGrid() {
			if(!_previousDateRange.IsInRange(datePicker.GetDateTimeFrom()) || !_previousDateRange.IsInRange(datePicker.GetDateTimeTo())
				|| comboClinics.ListSelectedClinicNums.Any(x => !_listPreviousClinicNums.Contains(x))) 
			{
				RefreshRecurringCharges();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"PatNum"),55, sortingStrategy: ODGridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),185));
			if(Preferences.HasClinicsEnabled) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Clinic"),65));
			}
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Date Charge"),135,HorizontalAlignment.Center,
				ODGridSortingStrategy.DateParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Charge Status"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"User"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Family Bal"),Preferences.HasClinicsEnabled ? 70 : 85,HorizontalAlignment.Right,
				ODGridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"PayPlan Due"),Preferences.HasClinicsEnabled ? 80 : 90,HorizontalAlignment.Right,
				ODGridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Total Due"),Preferences.HasClinicsEnabled ? 65 : 80,HorizontalAlignment.Right,
				ODGridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Repeat Amt"),Preferences.HasClinicsEnabled ? 75 : 90,HorizontalAlignment.Right,
				ODGridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Charge Amt"),Preferences.HasClinicsEnabled ? 85 : 95,HorizontalAlignment.Right,
				ODGridSortingStrategy.AmountParse));
			if(gridMain.WidthAllColumns > gridMain.Width) {
				gridMain.HScrollVisible=true;
			}
			gridMain.Rows.Clear();
			foreach(RecurringCharge charge in _listRecurringCharges.OrderBy(x => x.DateTimeCharge)) {
				bool isAutomated=(charge.UserNum==0);
				if(!datePicker.IsInDateRange(charge.DateTimeCharge) 
					|| (Preferences.HasClinicsEnabled && !charge.ClinicNum.In(comboClinics.ListSelectedClinicNums))
					|| !charge.ChargeStatus.In(comboStatuses.SelectedTags<RecurringChargeStatus>())
					|| (comboAutomated.SelectedIndex==1 && !isAutomated) || (comboAutomated.SelectedIndex==2 && isAutomated))
				{
					continue;
				}
				ODGridRow row=new ODGridRow();
				row.Cells.Add(charge.PatNum.ToString());
				string patName;
				if(!_dictPatNames.TryGetValue(charge.PatNum,out patName)) {
					patName=Lans.g(this,"UNKNOWN");
				}
				row.Cells.Add(patName);
				if(Preferences.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetFirstOrDefault(x => x.ClinicNum==charge.ClinicNum)?.Description??"");
				}
				row.Cells.Add(charge.DateTimeCharge.ToString());
				row.Cells.Add(Lans.g(this,charge.ChargeStatus.GetDescription()));
				row.Cells.Add(Userods.GetFirstOrDefault(x => x.Id==charge.UserNum)?.UserName??"");
				row.Cells.Add(charge.FamBal.ToString("c"));
				row.Cells.Add(charge.PayPlanDue.ToString("c"));
				row.Cells.Add(charge.TotalDue.ToString("c"));
				row.Cells.Add(charge.RepeatAmt.ToString("c"));
				row.Cells.Add(charge.ChargeAmt.ToString("c"));
				row.Tag=charge;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FilterChanged(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.IsMultiSelect=true;
			FormC.ListClinics=Clinics.GetForUserod(Security.CurUser,true,Lan.g(this,"Unassigned"));
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			comboClinics.ListSelectedClinicNums=FormC.ListSelectedClinicNums;
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshRecurringCharges();
			FillGrid();
		}
		
		private void contextMenu_Popup(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null) {
				return;
			}
			menuItemOpenPayment.Visible=(charge.PayNum!=0);
			menuItemViewError.Visible=(charge.ChargeStatus==RecurringChargeStatus.ChargeFailed);
			menuItemDeletePending.Visible=(charge.ChargeStatus==RecurringChargeStatus.NotYetCharged);
		}
		
		private void menuItemGoTo_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			GotoModule.GotoAccount(charge.PatNum);
		}

		private void menuItemOpenPayment_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || charge.PayNum==0) {
				return;
			}
			Payment pay=Payments.GetPayment(charge.PayNum);
			if(pay==null) {//The payment has been deleted
				MsgBox.Show(this,"This payment no longer exists.");
				return;
			}
			Patient pat=Patients.GetPat(pay.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormPayment FormP=new FormPayment(pat,fam,pay,false);
			FormP.ShowDialog();
		}

		private void menuItemViewError_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null) {
				return;
			}
			new MsgBoxCopyPaste(charge.ErrorMsg).Show();
		}

		private void menuItemDeletePending_Click(object sender,EventArgs e) {
			RecurringCharge charge=gridMain.SelectedTag<RecurringCharge>();
			if(charge==null || !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this pending recurring charge?"
				+"\r\n\r\nAnother user or service may be processing this card right now.")) 
			{
				return;
			}
			RecurringCharge chargeDb=RecurringCharges.GetOne(charge.RecurringChargeNum);
			if(chargeDb==null || chargeDb.ChargeStatus!=RecurringChargeStatus.NotYetCharged) {
				MsgBox.Show(this,"This recurring charge is no longer pending. Unable to delete.");
				return;
			}
			RecurringCharges.Delete(charge.RecurringChargeNum);
			RefreshRecurringCharges();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}