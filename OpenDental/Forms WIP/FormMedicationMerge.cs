using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMedicationMerge:ODForm {
		private Medication _medFrom;
		private Medication _medInto;

		public FormMedicationMerge() {
			InitializeComponent();
			
		}

		private void CheckUIState() {
			butMerge.Enabled=(textMedNumFrom.Text.Trim()!="" && textMedNumInto.Text.Trim()!="");
		}
		
		private void butChangeMedInto_Click(object sender,EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult==DialogResult.OK) {
				_medInto= Medication.GetById(FormM.SelectedMedicationNum);
				textGenNumInto.Text=_medInto.GenericId.Value.ToString();
				textMedNameInto.Text=_medInto.Description;
				textMedNumInto.Text=POut.Long(_medInto.Id);
				textRxInto.Text=_medInto.RxCui;
			}
			CheckUIState();
		}

		private void butChangeMedFrom_Click(object sender,EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult==DialogResult.OK) {
				_medFrom= Medication.GetById(FormM.SelectedMedicationNum);
				textGenNumFrom.Text=_medFrom.GenericId.Value.ToString();
				textMedNameFrom.Text=_medFrom.Description;
				textMedNumFrom.Text=POut.Long(_medFrom.Id);
				textRxFrom.Text=_medFrom.RxCui;
			}
			CheckUIState();
		}

		private void butMerge_Click(object sender,EventArgs e) {
			string differentFields="";
			string msgText="";
			if(textMedNumInto.Text==textMedNumFrom.Text) {
				//do not attempt a merge if the same medication was selected twice, or if one of the fields is blank.
				MsgBox.Show(this,"You must select two different medications to merge.");
				return;
			}
			if(_medFrom.Id==_medFrom.GenericId && _medInto.Id!=_medInto.GenericId) {
				msgText=Lan.g(this,"You may not merge a generic medication into a brand")+".  "+
					Lan.g(this,"Select the generic version of the medication to merge into instead")+".";
				MessageBox.Show(msgText);
				return;
			}
			if(textMedNameFrom.Text!=textMedNameInto.Text) {
				differentFields+="\r\n"+Lan.g(this,"Medication Name");
			}
			if(textGenNumFrom.Text!=textGenNumInto.Text) {
				differentFields+="\r\n"+Lan.g(this,"GenericNum");
			}
			if(textRxFrom.Text!=textRxInto.Text) {
				differentFields+="\r\n"+Lan.g(this,"RxCui");
			}
			long numPats= Medication.CountPats(_medFrom.Id);
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure?  The results are permanent and cannot be undone.")) {
				return;
			}
			msgText="";
			if(differentFields!="") {
				msgText=Lan.g(this,"The following medication fields do not match")+": "+differentFields+"\r\n";
			}
			msgText+=Lan.g(this,"This change is irreversible")+".  "+Lan.g(this,"This medication is assigned to")+" "+numPats+" "
				+Lan.g(this,"patients")+".  "+Lan.g(this,"Continue anyways?");
			if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			long rowsChanged= Medication.Merge(_medFrom.Id,_medInto.Id);
			string logText=Lan.g(this,"Medications merged")+": "+_medFrom.Description+" "+Lan.g(this,"merged into")+" "+_medInto.Description+".\r\n"
			+Lan.g(this,"Rows changed")+": "+POut.Long(rowsChanged);
			SecurityLogs.MakeLogEntry(Permissions.MedicationMerge,0,logText);
			textRxFrom.Clear();
			textMedNumFrom.Clear();
			textMedNameFrom.Clear();
			textGenNumFrom.Clear();
			MsgBox.Show(this,"Done.");
			DataValid.SetInvalid(InvalidType.Medications);
			CheckUIState();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}