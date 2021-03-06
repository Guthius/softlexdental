using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	///<summary></summary>
	public partial class FormRpBrokenAppointments:ODForm {

		private List<Clinic> _listClinics;
		private List<Definition> _listPosAdjTypes=new List<Definition>();
		private List<BrokenApptProcedure> _listBrokenProcOptions=new List<BrokenApptProcedure>();
		private List<Provider> _listProviders;


		///<summary></summary>
		public FormRpBrokenAppointments() {
			InitializeComponent();
			
		}

		private void FormRpBrokenAppointments_Load(object sender,EventArgs e) {

			_listProviders=Provider.GetForReporting().ToList();
			dateStart.SelectionStart=DateTime.Today;
			dateEnd.SelectionStart=DateTime.Today;
			for(int i=0;i<_listProviders.Count;i++) {
				listProvs.Items.Add(_listProviders[i].GetLongDesc());
			}

				_listClinics=Clinic.GetByUser(Security.CurrentUser).ToList();
				if(!Security.CurrentUser.ClinicRestricted) {
					listClinics.Items.Add(Lan.g(this,"Unassigned"));
					listClinics.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClinics.Items.Add(_listClinics[i].Abbr);
					if(Clinics.ClinicId==0) {
						listClinics.SetSelected(curIndex,true);
						checkAllClinics.Checked=true;
					}
					if(_listClinics[i].Id==Clinics.ClinicId) {
						listClinics.SelectedIndices.Clear();
						listClinics.SetSelected(curIndex,true);
					}
				}
			
			int value=Preference.GetInt(PreferenceName.BrokenApptProcedure);
			if(value==(int)BrokenApptProcedure.None) {//
				radioProcs.Visible=false;
			}
			if(value>0){
				radioProcs.Checked=true;
			}
			else if(Preference.GetBool(PreferenceName.BrokenApptAdjustment)) {
				radioAdj.Checked=true;
			}
			else {
				radioAptStatus.Checked=true;
			}
		}

		private void checkAllProvs_Click(object sender,EventArgs e) {
			if(checkAllProvs.Checked) {
				listProvs.SelectedIndices.Clear();
			}
		}

		private void checkAllClinics_Click(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				for(int i=0;i<listClinics.Items.Count;i++) {
					listClinics.SetSelected(i,true);
				}
			}
			else {
				listClinics.SelectedIndices.Clear();
			}
		}

		private void listProvs_Click(object sender,EventArgs e) {
			if(listProvs.SelectedIndices.Count>0) {
				checkAllProvs.Checked=false;
			}
		}

		private void listClinics_Click(object sender,EventArgs e) {
			if(listClinics.SelectedIndices.Count>0) {
				checkAllClinics.Checked=false;
			}
		}

		private void radioProcs_CheckedChanged(object sender,EventArgs e) {
			if(radioProcs.Checked) {
				listOptions.Items.Clear();
				listOptions.SelectionMode=SelectionMode.One;
				int index=0;
				_listBrokenProcOptions.Clear();
				BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)Preference.GetInt(PreferenceName.BrokenApptProcedure);
				switch(brokenApptCodeDB) {
					case BrokenApptProcedure.None:
					case BrokenApptProcedure.Missed:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Missed);
						index=listOptions.Items.Add(Lans.g(this,brokenApptCodeDB.ToString())+": (D9986)");
						labelDescr.Text=Lan.g(this,"Broken appointments based on ADA code D9986");
					break;
					case BrokenApptProcedure.Cancelled:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Cancelled);
						index=listOptions.Items.Add(Lans.g(this,brokenApptCodeDB.ToString())+": (D9987)");
						labelDescr.Text=Lan.g(this,"Broken appointments based on ADA code D9987");
					break;
					case BrokenApptProcedure.Both:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Missed);
						_listBrokenProcOptions.Add(BrokenApptProcedure.Cancelled);
						_listBrokenProcOptions.Add(BrokenApptProcedure.Both);
						listOptions.Items.Add(Lans.g(this,BrokenApptProcedure.Missed.ToString())+": (D9986)");
						listOptions.Items.Add(Lans.g(this,BrokenApptProcedure.Cancelled.ToString())+": (D9987)");
						index=listOptions.Items.Add(Lans.g(this,brokenApptCodeDB.ToString()));
						labelDescr.Text=Lan.g(this,"Broken appointments based on ADA code D9986 or D9987");
					break;
				}
				listOptions.SetSelected(index,true);
				listOptions.Visible=true;
			}
		}

		private void radioAdj_CheckedChanged(object sender,EventArgs e) {
			if(radioAdj.Checked) {
				labelDescr.Text=Lan.g(this,"Broken appointments based on broken appointment adjustments");
				listOptions.Items.Clear();
				_listPosAdjTypes.Clear();
				listOptions.SelectionMode=SelectionMode.MultiSimple;
				_listPosAdjTypes=Defs.GetPositiveAdjTypes();
				long brokenApptAdjDefNum=Preference.GetLong(PreferenceName.BrokenAppointmentAdjustmentType);
				for(int i=0; i<_listPosAdjTypes.Count;i++) {
					listOptions.Items.Add(_listPosAdjTypes[i].Description);
					if(_listPosAdjTypes[i].Id==brokenApptAdjDefNum) {
						listOptions.SelectedIndices.Add(i);
					}
				}
				listOptions.Visible=true;
			}
			else {
				listOptions.Visible=false;
			}
		}

		private void radioAptStatus_CheckedChanged(object sender,EventArgs e) {
			if(radioAptStatus.Checked) {
				labelDescr.Text=Lan.g(this,"Broken appointments based on appointment status");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!checkAllProvs.Checked && listProvs.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}

				if(!checkAllClinics.Checked && listClinics.SelectedIndices.Count==0) {
					MsgBox.Show(this,"At least one clinic must be selected.");
					return;
				}
			
			if(radioAdj.Checked && listOptions.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one adjustment type must be selected.");
				return;
			}
			if(radioProcs.Checked && listOptions.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one procedure code option must be selected.");
				return;
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
				if(Security.CurrentUser.ClinicRestricted) {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]].Id);//we know that the list is a 1:1 to _listClinics
					}
				else {
					if(listClinics.SelectedIndices[i]==0) {
						listClinicNums.Add(0);
					}
					else {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]-1].Id);//Minus 1 from the selected index
					}
				}
			}
			List<long> listProvNums=new List<long>();
			if(checkAllProvs.Checked) {
				for(int i = 0;i<_listProviders.Count;i++) {
					listProvNums.Add(_listProviders[i].Id);
				}
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
				listProvNums.Add(_listProviders[listProvs.SelectedIndices[i]].Id);
				}
			}
			List<long> listAdjDefNums=new List<long>();
			if(radioAdj.Checked) {
				for(int i=0;i<listOptions.SelectedIndices.Count;i++) {
					listAdjDefNums.Add(_listPosAdjTypes[listOptions.SelectedIndices[i]].Id);
				}
			}
			BrokenApptProcedure brokenApptSelection=BrokenApptProcedure.None;
			if(radioProcs.Checked) {
				brokenApptSelection=_listBrokenProcOptions[listOptions.SelectedIndex];
			}
			ReportComplex report=new ReportComplex(true,false);
			DataTable table = new DataTable();
			table=RpBrokenAppointments.GetBrokenApptTable(dateStart.SelectionStart,dateEnd.SelectionStart,listProvNums,listClinicNums,listAdjDefNums,brokenApptSelection
				,checkAllClinics.Checked,radioProcs.Checked,radioAptStatus.Checked,radioAdj.Checked,true);
			string subtitleProvs="";
			string subtitleClinics="";
			if(checkAllProvs.Checked) {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						subtitleProvs+=", ";
					}
					subtitleProvs+=_listProviders[listProvs.SelectedIndices[i]].Abbr;
				}
			}

				if(checkAllClinics.Checked) {
					subtitleClinics=Lan.g(this,"All Clinics");
				}
				else {
					for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurrentUser.ClinicRestricted) {
							subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]].Abbr;
						}
						else {
							if(listClinics.SelectedIndices[i]==0) {
								subtitleClinics+=Lan.g(this,"Unassigned");
							}
							else {
								subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]-1].Abbr;//Minus 1 from the selected index
							}
						}
					}
				}
			
			Font font=new Font("Tahoma",10);
			Font fontBold=new Font("Tahoma",10,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",11,FontStyle.Bold);
			report.ReportName=Lan.g(this,"Broken Appointments");
			report.AddTitle("Title",Lan.g(this,"Broken Appointments"),fontTitle);
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				string codes="";
				switch(brokenApptSelection) {
					case BrokenApptProcedure.None:
					case BrokenApptProcedure.Missed:
						codes="D9986";
					break;
					case BrokenApptProcedure.Cancelled:
						codes="D9987";
					break;
					case BrokenApptProcedure.Both:
						codes="D9986 or D9987";
					break;
				}
				report.AddSubTitle("Report Description",Lan.g(this,"By ADA Code "+codes),fontSubTitle);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				report.AddSubTitle("Report Description",Lan.g(this,"By Broken Appointment Adjustment"),fontSubTitle);
			}
			else {//Report looking at appointments with a status of 'Broken'
				report.AddSubTitle("Report Description",Lan.g(this,"By Appointment Status"),fontSubTitle);
			}
			report.AddSubTitle("Providers",subtitleProvs,fontSubTitle);
			report.AddSubTitle("Clinics",subtitleClinics,fontSubTitle);
			QueryObject query;

				query=report.AddQuery(table,Lan.g(this,"Date")+": "+DateTime.Today.ToString("d"),"ClinicDesc",SplitByKind.Value,0,true);

			//Add columns to report
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986 or D9987
				query.AddColumn(Lan.g(this,"Date"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Provider"),180,FieldValueType.String,font);
				if(brokenApptSelection==BrokenApptProcedure.Both) {
					query.AddColumn(Lan.g(this,"Code"),75,FieldValueType.String,font);
				}
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Fee"),200,FieldValueType.Number,font);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointment Fees")+":",Lan.g(this,"Fee"),"ProcFee",SummaryOperation.Sum,fontBold,0,10);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",Lan.g(this,"Fee"),"ProcFee",SummaryOperation.Count,fontBold,0,10);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				query.AddColumn(Lan.g(this,"Date"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Provider"),100,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Amount"),80,FieldValueType.Number,font);
				query.AddColumn(Lan.g(this,"Note"),300,FieldValueType.String,font);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointment Adjustment Amount")+":",
					Lan.g(this,"Amount"),"AdjAmt",SummaryOperation.Sum,fontBold,0,10);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",
					Lan.g(this,"Amount"),"AdjAmt",SummaryOperation.Count,fontBold,0,10);
			}
			else {//Report looking at appointments with a status of 'Broken'
				query.AddColumn(Lan.g(this,"AptDate"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Doctor"),165,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Hygienist"),165,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"IsHyg"),50,FieldValueType.Boolean,font);
				query.GetColumnDetail(Lan.g(this,"IsHyg")).ContentAlignment = ContentAlignment.MiddleCenter;
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",Lan.g(this,"IsHyg"),"AptDateTime",SummaryOperation.Count,fontBold,0,10);
			}
			query.ContentAlignment=ContentAlignment.MiddleRight;
			report.AddPageNum(font);
			//execute query
			if(!report.SubmitQueries()) {
				return;
			}
			//display report
			FormReportComplex FormR=new FormReportComplex(report);
			//FormR.MyReport=report;
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

	}
}