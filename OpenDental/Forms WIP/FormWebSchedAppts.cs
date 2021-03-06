using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormWebSchedAppts:ODForm {

		///<summary>Form showing new appointments made using web sched.</summary>
		public FormWebSchedAppts() {
			InitializeComponent();
		}

		public FormWebSchedAppts(bool isNewPat,bool isRecall,bool isASAP):this() {
			checkWebSchedNewPat.Checked=isNewPat;
			checkWebSchedRecall.Checked=isRecall;
			checkASAP.Checked=isASAP;
		}

		///<summary></summary>
		private void FormWebSchedAppts_Load(object sender,System.EventArgs e) {
			//Set the initial date
			datePicker.SetDateTimeFrom(DateTime.Today);
			datePicker.SetDateTimeTo(DateTime.Today.AddDays(7));
			//Add the appointment confirmation types
			comboBoxMultiConfStatus.Items.Clear();
			long defaultStatus=Preference.GetLong(PreferenceName.WebSchedNewPatConfirmStatus);
			if(!checkWebSchedNewPat.Checked && checkWebSchedRecall.Checked) {
				defaultStatus=Preference.GetLong(PreferenceName.WebSchedRecallConfirmStatus);
			}
			List<Definition> listDefs=Definition.GetByCategory(DefinitionCategory.ApptConfirmed);
			foreach(Definition defCur in listDefs) {
				ODBoxItem<long> defItem=new ODBoxItem<long>(defCur.Description,defCur.Id);
				int idx=comboBoxMultiConfStatus.Items.Add(defItem);
				if((checkWebSchedNewPat.Checked || checkWebSchedRecall.Checked) && defCur.Id==defaultStatus) {
					comboBoxMultiConfStatus.SetSelected(idx,true);
				}
			}
			FillGrid();
		}

		///<summary></summary>
		private void FillGrid() {
			if(!checkWebSchedNewPat.Checked && !checkWebSchedRecall.Checked && !checkASAP.Checked) {
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				return;
			}
			DataTable table=GetAppointments();
			gridMain.BeginUpdate();
			//Columns
			gridMain.Columns.Clear();
			ODGridColumn col;

				col=new ODGridColumn(Lan.g(this,"Clinic"),100);
				gridMain.Columns.Add(col);
			
			col=new ODGridColumn(Lan.g(this,"Date Time Created"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Appt Date Time"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient Name"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient DOB"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Confirmation Status"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Appt Note"),300);
			gridMain.Columns.Add(col);
			//Rows
			gridMain.Rows.Clear();
			DataColumnCollection columns=table.Columns;
			foreach(DataRow row in table.Rows) {
				ODGridCell[] cellsArray=new ODGridCell[gridMain.Columns.Count];
				ODGridRow newRow=new ODGridRow();				

					newRow.Cells.Add(row["ClinicDesc"].ToString());
				
				newRow.Cells.Add(PIn.Date(row["DateTimeCreated"].ToString()).ToString("d"));
				newRow.Cells.Add(row["AptDateTime"].ToString());
				newRow.Cells.Add(row["PatName"].ToString());
				newRow.Cells.Add(PIn.Date(row["Birthdate"].ToString()).ToString("d"));
				newRow.Cells.Add(Defs.GetDef(DefinitionCategory.ApptConfirmed,PIn.Long(row["Confirmed"].ToString())).Description);
				newRow.Cells.Add(row["Note"].ToString());
				newRow.Tag=row["AptNum"].ToString();
				gridMain.Rows.Add(newRow);
			}
			gridMain.EndUpdate();
		}

		///<summary>Get the list of websched appointments using the RpAppointments query.</summary>
		private DataTable GetAppointments() {
            List<long> listProvNums = new List<long>();/*Providers.GetProvidersForWebSchedNewPatAppt().Select(x => x.Id).ToList();*/
			List<long> listStatus=comboBoxMultiConfStatus.ListSelectedIndices.Select(x => ((ODBoxItem<long>)comboBoxMultiConfStatus.Items[x]).Tag).ToList();
			return RpAppointments.GetAppointmentTable(
				datePicker.GetDateTimeFrom(),
				datePicker.GetDateTimeTo(),
				listProvNums,
				comboBoxClinicMulti.ListSelectedClinicNums,
				true,
				checkWebSchedRecall.Checked,
				checkWebSchedNewPat.Checked,
				checkASAP.Checked,
				RpAppointments.SortAndFilterBy.SecurityLogDate,
				new List<ApptStatus>(),
				listStatus,
				nameof(FormWebSchedAppts));
		}

		///<summary></summary>
		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		///<summary></summary>
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OpenEditAppointmentWindow(gridMain.Rows[e.Row]);
		}

		private void mainGridMenuItemPatChart_Click(object sender,EventArgs e) {
			List<ODGridRow> listSelected=gridMain.SelectedGridRows;
			if(listSelected.Count==1) {
				long aptNum=PIn.Long(listSelected[0].Tag.ToString());
				Appointment apt=Appointments.GetOneApt(aptNum);
				GotoModule.GotoChart(apt.PatNum);
				DialogResult=DialogResult.OK;
			}
		}

		private void mainGridMenuItemApptEdit_Click(object sender,EventArgs e) {
			List<ODGridRow> listSelected=gridMain.SelectedGridRows;
			if(listSelected.Count==1) {
				OpenEditAppointmentWindow(listSelected.First());
			}
		}

		private void OpenEditAppointmentWindow(ODGridRow row) {
			long aptNum=PIn.Long(row.Tag.ToString());
			FormApptEdit formAE=new FormApptEdit(aptNum);
			formAE.ShowDialog();
			if(formAE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}
		
		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}