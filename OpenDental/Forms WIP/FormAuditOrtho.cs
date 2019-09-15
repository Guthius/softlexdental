using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormAuditOrtho:ODForm {
		///<summary>Should be passed in from calling function.</summary>
		public SortedDictionary<DateTime,List<SecurityLog>> DictDateOrthoLogs;
		public List<SecurityLog> PatientFieldLogs;

		public FormAuditOrtho() {
			InitializeComponent();
			
			DictDateOrthoLogs=new SortedDictionary<DateTime,List<SecurityLog>>();
			PatientFieldLogs=new List<SecurityLog>();
		}

		private void FormAuditOrtho_Load(object sender,EventArgs e) {
			FillGridDates();
			FillGridMain();
		}

		private void FillGridDates() {
			gridHist.BeginUpdate();
			gridHist.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableOrthoAudit","Date"),70);
			gridHist.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOrthoAudit","Entries"),50,HorizontalAlignment.Center);
			gridHist.Columns.Add(col);
			gridHist.Rows.Clear();
			ODGridRow row;
			foreach(DateTime dt in DictDateOrthoLogs.Keys) {//must use foreach to enumerate through keys in the dictionary
				row=new ODGridRow();
				row.Cells.Add(dt.ToShortDateString());
				row.Cells.Add(DictDateOrthoLogs[dt].Count.ToString());
				row.Tag=dt;
				gridHist.Rows.Add(row);
			}
			gridHist.EndUpdate();
			gridHist.ScrollToEnd();
			gridHist.SetSelected(true);
		}

		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableOrthoAudit","Date Time"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOrthoAudit","User"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOrthoAudit","Permission"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOrthoAudit","Log Text"),569);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			User user;
			//First Selected Ortho Chart Logs
			foreach(int iDate in gridHist.SelectedIndices) {
				DateTime dateRow=(DateTime)gridHist.Rows[iDate].Tag;
				if(!DictDateOrthoLogs.ContainsKey(dateRow)){
					continue;
				}
				for(int i=0;i<DictDateOrthoLogs[dateRow].Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(DictDateOrthoLogs[dateRow][i].LogDate.ToShortDateString()+" "+DictDateOrthoLogs[dateRow][i].LogDate.ToShortTimeString());
					user=User.GetById(DictDateOrthoLogs[dateRow][i].UserId);
					if(user==null) {//Will be null for audit trails made by outside entities that do not require users to be logged in.  E.g. Web Sched.
						row.Cells.Add("unknown");
					}
					else {
						row.Cells.Add(user.UserName);
					}
					row.Cells.Add(DictDateOrthoLogs[dateRow][i].EventName.ToString());
					row.Cells.Add(DictDateOrthoLogs[dateRow][i].LogMessage);
					gridMain.Rows.Add(row);
				}
			}
			//Then any applicable patient field logs.
			for(int i=0;i<PatientFieldLogs.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(PatientFieldLogs[i].LogDate.ToShortDateString()+" "+PatientFieldLogs[i].LogDate.ToShortTimeString());
				row.Cells.Add(User.GetById(PatientFieldLogs[i].UserId).UserName);
				row.Cells.Add(PatientFieldLogs[i].EventName.ToString());
				row.Cells.Add(PatientFieldLogs[i].LogMessage);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}

		private void gridHist_CellClick(object sender,ODGridClickEventArgs e) {
			FillGridMain();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}