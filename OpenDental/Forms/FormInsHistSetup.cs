using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormInsHistSetup:ODForm {
		private Patient _patCur;
		///<summary></summary>
		private Dictionary<PreferenceName,Procedure> _dictInsHistProcs;
		private InsSub _insSubCur;
		///<summary></summary>
		private List<ClaimProc> _listClaimProcsForInsHistProcs;
		private const string NO_INSHIST="No History";
		private const string NO_INSHISTSET="Not Set";

		//1. Find claiprocs for current plan
		//2. Find EO and C procs for claimprocs on current plan
		//3. Only have procs on this insplan that are EO and C
		//4. Fill in date info based on EO or C proc list
		//5. On OK click, fetch all claimprocs for EO and C procs
			//5a. Date filled, no matching EO or C proc - User entered date themselves, make new proc with claimproc status inshist
			//5b. Date filled, matching EO or C proc and user changed the date.- 
				//Modify proc and claimproc for that category ONLY if the proc has a status of EO to be the date specified
				//If the proc has a status of C and the new date is greater than the the current ProcDate, create a new EO procedure and InsHist ClaimProc with the new date entered.
			//5c. No date, no proc - Do nothing
			//5d. No date, matching EO or C proc - User erased the date, delete proc/claimproc ONLY if proc has a status of EO.


		public FormInsHistSetup(long patNum,InsSub insSub) {
			InitializeComponent();
			
			_patCur=Patients.GetPat(patNum);
			_insSubCur=insSub;
			_dictInsHistProcs=Procedures.GetDictInsHistProcs(patNum,insSub.InsSubNum,out _listClaimProcsForInsHistProcs);
		}

		private void FormInsHistSetup_Load(object sender,EventArgs e) {
			FillDates();
		}

		/// <summary>Returns the text box control corresponding to the given procType</summary>
		private void FillDates() {
			var listInsHistPref= Preference.GetInsHistPrefs();
			string text=NO_INSHIST;
			foreach(PreferenceName prefName in Preference.GetInsHistPrefNames()) {
				Procedure proc=_dictInsHistProcs[prefName];
				ClaimProc claimProc=null;
				if(proc!=null) {
					claimProc=_listClaimProcsForInsHistProcs.Find(x => x.InsSubNum==_insSubCur.InsSubNum && x.Status.In(ClaimProcStatus.InsHist,ClaimProcStatus.Received)
						&& x.ProcNum==proc.ProcNum);
				}
				text=((claimProc!=null && proc!=null && proc.ProcDate.Year>1880) ? proc.ProcDate.ToShortDateString() : NO_INSHIST);
				bool isPrefSet=listInsHistPref.Where(x => x.Key==prefName.ToString() && !string.IsNullOrWhiteSpace(x.Value)).Count() > 0;
				TextBox textBoxCur=GetControlForPrefName(prefName);
				if(!isPrefSet) {
					text=NO_INSHISTSET;
					textBoxCur.Enabled=false;
				}
				textBoxCur.Text=text;
			}
		}
		
		///<summary>Returns the text box control corresponding to the given procType</summary>
		private TextBox GetControlForPrefName(PreferenceName prefName) {
			switch(prefName) {
				case PreferenceName.InsHistExamCodes:
					return textDateExam;
				case PreferenceName.InsHistProphyCodes:
					return textDateProphy;
				case PreferenceName.InsHistBWCodes:
					return textDateBW;
				case PreferenceName.InsHistPanoCodes:
					return textDateFmxPano;
				case PreferenceName.InsHistPerioURCodes:
					return textDatePerioScalingUR;
				case PreferenceName.InsHistPerioULCodes:
					return textDatePerioScalingUL;
				case PreferenceName.InsHistPerioLRCodes:
					return textDatePerioScalingLR;
				case PreferenceName.InsHistPerioLLCodes:
					return textDatePerioScalingLL;
				case PreferenceName.InsHistPerioMaintCodes:
					return textDatePerioMaint;
				case PreferenceName.InsHistDebridementCodes:
					return textDateDebridgement;
				default:
					return null;
			}
		}

		///<summary></summary>
		private bool IsValid() {
			DateTime dateEntry;
			foreach(PreferenceName prefName in Preference.GetInsHistPrefNames()) {
				TextBox textBox=GetControlForPrefName(prefName);
				//Continue if no date is entered or the date entered is valid.
				if(!textBox.Enabled || string.IsNullOrEmpty(textBox.Text) || textBox.Text.Trim()==NO_INSHIST) {
					continue;
				}
				if(!DateTime.TryParse(textBox.Text,out dateEntry)) {
					//Invalid date entered.
					MsgBox.Show(this,"Invalid date");
					return false;
				}
			}
			return true;
		}

		private void TextBoxValidating(object sender,System.ComponentModel.CancelEventArgs e) {
			if(sender.GetType()!=typeof(TextBox)) {
				return;
			}
			TextBox textBox=(TextBox)sender;
			//If its disabled, empty or the default text return.
			if(!textBox.Enabled || string.IsNullOrEmpty(textBox.Text) || textBox.Text.Trim()==NO_INSHIST) {
				return;
			}
			bool allNums=true;
			for(int i=0;i<textBox.Text.Length;i++) {
				if(!Char.IsNumber(textBox.Text,i)) {
					allNums=false;
				}
			}
			if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName=="en") {
				if(allNums) {
					if(textBox.Text.Length==6) {
						textBox.Text=textBox.Text.Substring(0,2)+"/"+textBox.Text.Substring(2,2)+"/"+textBox.Text.Substring(4,2);
					}
					else if(textBox.Text.Length==8) {
						textBox.Text=textBox.Text.Substring(0,2)+"/"+textBox.Text.Substring(2,2)+"/"+textBox.Text.Substring(4,4);
					}
				}
			}
			try {
				textBox.Text=DateTime.Parse(textBox.Text).ToString("d");//will throw exception if invalid
			}
			catch {
				//We don't want a full exception, just a popup.  OK_Click will block them from putting invalid data in the db.

				MsgBox.Show(this,"Invalid date.");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!IsValid()) {
				return;
			}
			List<Procedure> listProcsForDelete=new List<Procedure>();
			foreach(PreferenceName prefName in Preference.GetInsHistPrefNames()) {
				TextBox textBox=GetControlForPrefName(prefName);
				if(!textBox.Enabled || textBox.Text.Trim()==NO_INSHIST) {
					continue;
				}
				Procedure proc=_dictInsHistProcs[prefName];
				if(string.IsNullOrWhiteSpace(textBox.Text)) {
					if(proc!=null && proc.ProcStatus==ProcStat.EO) {//Only delete EO procedures
						listProcsForDelete.Add(proc);//Delete proc if user deleted procedure date from textbox.
					}
					continue;
				}
				DateTime dateEntered=PIn.Date(textBox.Text);
				List<ClaimProc> listClaimProcsForProc=new List<ClaimProc>();
				if(proc!=null) {
					//Get all of the claimprocs for this procedure.
					listClaimProcsForProc=_listClaimProcsForInsHistProcs.FindAll(x => x.ProcNum==proc.ProcNum);
				}
				Procedures.InsertOrUpdateInsHistProcedure(_patCur,prefName,dateEntered,_insSubCur.PlanNum,_insSubCur.InsSubNum,proc,listClaimProcsForProc);
			}
			if(listProcsForDelete.Count>0
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting the last procedure date for a category will delete the Existing Other procedure with that date for this patient.  Continue?"))
			{
				return;
			}
			foreach(Procedure proc in listProcsForDelete) {
				try {
					Procedures.Delete(proc.ProcNum);
				}
				catch{
					//Tried deleting the procedure. Do nothing. 
					
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}