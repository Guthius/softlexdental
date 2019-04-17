using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans834Preview:ODForm {

		private ODThread _odThread=null;
		private X834 _x834;
		private List<Patient> _listPatients=null;
		private int _patNumCol;
		private int _sortedByColumnIdx;
		private bool _isSortAscending;

		public FormEtrans834Preview(X834 x834) {
			InitializeComponent();
			Lan.F(this);
			_x834=x834;
		}

		private void FormEtrans834Preview_Load(object sender,EventArgs e) {
			FillGridInsPlans();
		}

		///<summary>Shows current status to user in the progress label.  Useful for when processing for a few seconds or more.</summary>
		private void ShowStatus(string message) {
			labelProgress.Text=message;
			Application.DoEvents();
		}

		void FillGridInsPlans() {
			_sortedByColumnIdx=gridInsPlans.SortedByColumnIdx;
			_isSortAscending=gridInsPlans.SortedIsAscending;
			gridInsPlans.BeginUpdate();
			if(gridInsPlans.Columns.Count==0) {
				gridInsPlans.Columns.Clear();
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Name",200,sortingStrategy: UI.ODGridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Birthdate",74, textAlignment: HorizontalAlignment.Center, sortingStrategy: UI.ODGridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SSN",66, textAlignment: HorizontalAlignment.Center));
				_patNumCol=gridInsPlans.Columns.Count;
				gridInsPlans.Columns.Add(new UI.ODGridColumn("PatNum",68, textAlignment: HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Begin",84, textAlignment: HorizontalAlignment.Center, sortingStrategy: UI.ODGridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Term",84, textAlignment: HorizontalAlignment.Center, sortingStrategy: UI.ODGridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Relation",70, textAlignment: HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SubscriberID",96));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("GroupNum",100));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Payer",0));
				_sortedByColumnIdx=0;//Sort by Patient Last Name by default.
				_isSortAscending=true;//Start with A and progress to Z.
			}
			gridInsPlans.EndUpdate();
			Application.DoEvents();//To show empty grid while the window is loading.
			if(_odThread!=null) {
				_odThread.QuitSync(0);
			}
			_odThread=new ODThread(WorkerPreview834);
			_odThread.Start();
		}

		private void WorkerPreview834(ODThread odThread) {
			Load834_Safe();
			odThread.QuitAsync();
		}

		///<summary>Call this from external thread. Invokes to main thread to avoid cross-thread collision.</summary>
		private void Load834_Safe() {
			try {
				this.BeginInvoke(() => Load834_Unsafe());
			}
			//most likely because form is no longer available to invoke to
			catch { }
		}

		private void Load834_Unsafe() {
			Cursor=Cursors.WaitCursor;
			ShowStatus("Loading patient information");
			const int previewLimitCount=40;
			gridInsPlans.BeginUpdate();
			gridInsPlans.Rows.Clear();
			gridInsPlans.EndUpdate();
			Application.DoEvents();
			if(_listPatients==null) {
				_listPatients=Patients.GetAllPatients();//Testing this on an average sized database took about 1 second to run on a dev machine.
				_listPatients.Sort();
			}
			int rowCount=0;
			for(int i=0;i<_x834.ListTransactions.Count;i++) {
				Hx834_Tran tran=_x834.ListTransactions[i];
				for(int k=0;k<tran.ListMembers.Count;k++) {
					rowCount++;
				}
			}
			for(int i=0;i<_x834.ListTransactions.Count;i++) {
				Hx834_Tran tran=_x834.ListTransactions[i];
				for(int j=0;j<tran.ListMembers.Count;j++) {
					Hx834_Member member=tran.ListMembers[j];
					ShowStatus("Loading "+(gridInsPlans.Rows.Count+1).ToString().PadLeft(6)+"/"+rowCount.ToString().PadLeft(6)
						+"  Patient "+member.Pat.GetNameLF());
					if(gridInsPlans.Rows.Count < previewLimitCount) {
						gridInsPlans.BeginUpdate();
					}
					if(member.ListHealthCoverage.Count==0) {
						UI.ODGridRow row=new UI.ODGridRow();
						gridInsPlans.Rows.Add(row);
						FillGridRow(row,member,null);
					}
					else {//There is at least one insurance plan.
						for(int a=0;a<member.ListHealthCoverage.Count;a++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[a];
							UI.ODGridRow row=new UI.ODGridRow();
							gridInsPlans.Rows.Add(row);
							FillGridRow(row,null,healthCoverage);
						}
					}
					if(gridInsPlans.Rows.Count < previewLimitCount) {
						gridInsPlans.EndUpdate();//Also invalidates grid.
						Application.DoEvents();
					}
				}
			}
			gridInsPlans.BeginUpdate();
			gridInsPlans.SortForced(_sortedByColumnIdx,_isSortAscending);
			gridInsPlans.EndUpdate();//Also invalidates grid.
			ShowStatus("");
			Cursor=Cursors.Default;
			Application.DoEvents();
		}

		///<summary>The healthCoverage variable can be null.</summary>
		private void FillGridRow(UI.ODGridRow row,Hx834_Member member,Hx834_HealthCoverage healthCoverage) {
			row.Cells.Clear();
			if(healthCoverage==null) {
				row.Tag=member;
			}
			else {
				row.Tag=healthCoverage;
				member=healthCoverage.Member;
			}
			row.Cells.Add(member.Pat.GetNameLF());//Name
			if(member.Pat.Birthdate.Year > 1880) {
				row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
			}
			else {
				row.Cells.Add("");//Birthdate
			}
			row.Cells.Add(member.Pat.SSN);//SSN
			List <Patient> listPatientMatches=Patients.GetPatientsByNameAndBirthday(member.Pat,_listPatients);
			if(member.Pat.PatNum==0 && listPatientMatches.Count==1) {
				member.Pat.PatNum=listPatientMatches[0].PatNum;
			}
			if(member.Pat.PatNum==0 && listPatientMatches.Count==0) {
				row.Cells.Add("");//PatNum
			}
			else if(member.Pat.PatNum==0 && listPatientMatches.Count > 1) {
				row.Cells.Add("Multiple");//PatNum
			}
			else {//Either the patient was matched perfectly or the user chose the correct patient already.
				row.Cells.Add(member.Pat.PatNum.ToString());//PatNum
			}
			if(healthCoverage!=null && healthCoverage.DateEffective.Year > 1880) {
				row.Cells.Add(healthCoverage.DateEffective.ToShortDateString());//Date Begin
			}
			else {
				row.Cells.Add("");//Date Begin
			}
			if(healthCoverage!=null && healthCoverage.DateTerm.Year > 1880) {
				row.Cells.Add(healthCoverage.DateTerm.ToShortDateString());//Date Term
			}
			else {
				row.Cells.Add("");//Date Term
			}
			row.Cells.Add(member.PlanRelat.ToString());//Relation
			row.Cells.Add(member.SubscriberId);//SubscriberID
			row.Cells.Add(member.GroupNum);//GroupNum
			row.Cells.Add(member.Tran.Payer.Name);//Payer
		}

		private void gridInsPlans_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			Hx834_Member member=null;
			Hx834_HealthCoverage healthCoverage=null;
			if(gridInsPlans.Rows[e.Row].Tag is Hx834_Member) {
				member=(Hx834_Member)gridInsPlans.Rows[e.Row].Tag;
			}
			else {
				healthCoverage=(Hx834_HealthCoverage)gridInsPlans.Rows[e.Row].Tag;
				member=healthCoverage.Member;
			}
			FormPatientSelect FormPS=new FormPatientSelect(member.Pat);
			if(FormPS.ShowDialog()==DialogResult.OK) {
				member.Pat.PatNum=FormPS.SelectedPatNum;
				gridInsPlans.BeginUpdate();
				//Refresh all rows for this member to show the newly selected PatNum.
				//There will be multiple rows if there are multiple insurance plans for the member.
				for(int i=0;i<gridInsPlans.Rows.Count;i++) {
					Hx834_Member memberRefresh=null;
					if(gridInsPlans.Rows[i].Tag is Hx834_Member) {
						memberRefresh=(Hx834_Member)gridInsPlans.Rows[i].Tag;
					}
					else {
						memberRefresh=((Hx834_HealthCoverage)gridInsPlans.Rows[i].Tag).Member;
					}
					if(memberRefresh==member) {
						FillGridRow(gridInsPlans.Rows[e.Row],member,healthCoverage);
					}
				}
				gridInsPlans.EndUpdate();
			}
		}

		///<summary>Tries to import the 834. Will return false if the user cancelled out of importing.</summary>
		private bool TryImport834() {
			if(checkDropExistingIns.Checked 
				&& !MsgBox.Show(MsgBoxButtons.YesNo,"Insurance plans for patients will be automatically replaced with the new plans. Continue?"))
			{
				return false;
			}
			if(!MsgBox.Show(this,true,"Importing insurance plans is a database intensive operation and can take 10 minutes or more to run.  "
				+"It is best to import insurance plans after hours or during another time period when database usage is otherwise low.\r\n"
				+"Click OK to import insurance plans now, or click Cancel."))
			{
				return false;
			}
			checkIsPatientCreate.Enabled=false;
			checkDropExistingIns.Enabled=false;
			gridInsPlans.Enabled=false;
			butOK.Enabled=false;
			butCancel.Enabled=false;
			Cursor=Cursors.WaitCursor;
			//Save the prefs for both checkboxes.
			AutoSave();
			//Create all of our count variables.
			int createdPatsCount,updatedPatsCount,skippedPatsCount,createdCarrierCount,createdInsPlanCount,updatedInsPlanCount,createdInsSubCount,
				updatedInsSubCount,createdPatPlanCount,droppedPatPlanCount,updatedPatPlanCount;
			StringBuilder sbErrorMessages;
			EtransL.ImportInsurancePlans(_x834,_listPatients,checkIsPatientCreate.Checked,checkDropExistingIns.Checked,out createdPatsCount,
				out updatedPatsCount,out skippedPatsCount,out createdCarrierCount,out createdInsPlanCount,out updatedInsPlanCount,out createdInsSubCount,
				out updatedInsSubCount,out createdPatPlanCount,out droppedPatPlanCount,out updatedPatPlanCount,out sbErrorMessages,(rowIndex,pat) => {
				ShowStatus("Progress "+(rowIndex).ToString().PadLeft(6)+"/"+gridInsPlans.Rows.Count.ToString().PadLeft(6)
					+"  Importing plans for patient "+pat.GetNameLF());
			});
			Cursor=Cursors.Default;
			#region summary output
			string msg=Lan.g(this,"Done.");
			if(createdPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients created:")+" "+createdPatsCount;
			}
			if(updatedPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients updated:")+" "+updatedPatsCount;
			}
			if(skippedPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients skipped:")+" "+skippedPatsCount;
				msg+=sbErrorMessages.ToString();
			}
			if(createdCarrierCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of carriers created:")+" "+createdCarrierCount;
				msg+=sbErrorMessages.ToString();
			}
			if(createdInsPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of insurance plans created:")+" "+createdInsPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(updatedInsPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of insurance plan updates:")+" "+updatedInsPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(createdInsSubCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of insurance subscriptions created:")+" "+createdInsSubCount;
				msg+=sbErrorMessages.ToString();
			}
			if(updatedInsSubCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of insurance subscriptions updated:")+" "+updatedInsSubCount;
				msg+=sbErrorMessages.ToString();
			}
			if(createdPatPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients added to insurance:")+" "+createdPatPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(droppedPatPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients dropped from insurance:")+" "+droppedPatPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(updatedPatPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients insurance information updated:")+" "+updatedPatPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			#endregion summary output
			MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(msg);
			msgBox.ShowDialog();
			return true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!TryImport834()){
				return;
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}

}