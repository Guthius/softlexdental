using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPatientStatusTool:ODForm {
		
		///<summary>True when radioInactiveWith is checked. When true, we will be changing patient statuses from inactive to active.</summary>
		private bool _isConvertToPatient;
		
		public FormPatientStatusTool() {
			InitializeComponent();
			
		}

		private void FormPatientStatusTool_Load(object sender,EventArgs e) {
			//Auto set the date picker to two years in the past.
			odDatePickerSince.SetDateTime(DateTime.Today.AddYears(-2));
			//Fill listbox
			listOptions.Items.Add(Lan.g(this,"Planned Procedures"));
			listOptions.Items.Add(Lan.g(this,"Completed Procedures"));
			listOptions.Items.Add(Lan.g(this,"Appointments"));
			//Fill and enable ComboBoxClinic if clinics are enabled
			if(Preferences.HasClinicsEnabled) {
				comboClinic.Visible=true;
				labelClinic.Visible=true;
				comboClinic.SelectedIndex=0;//Default to All
			}
		}

		private void FillGrid() {
			//Sets bools for which filters to add
			bool includeTPProc=listOptions.SelectedIndices.Contains(0);
			bool includeCompletedProc=listOptions.SelectedIndices.Contains(1);
			bool includeAppointments=listOptions.SelectedIndices.Contains(2);
			//Gets clinic selection
			List<long> listClinicNums=new List<long>();
			if(Preferences.HasClinicsEnabled) {
				if(comboClinic.IsAllSelected) {
					listClinicNums=comboClinic.ListAvailableClinicNums;
				}
				else {
					listClinicNums.Add(comboClinic.SelectedClinicNum);
				}
			}
			List<Patient> listPatients=Patients.GetPatsToChangeStatus(_isConvertToPatient,odDatePickerSince.GetDateTime()
				,includeTPProc,includeCompletedProc,includeAppointments,listClinicNums);
			gridMain.BeginUpdate();
			if(gridMain.Columns.Count==0) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"PatNum"),75, sortingStrategy: ODGridSortingStrategy.AmountParse));
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"PatStatusCur"),100));
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"PatStatusNew"),100));
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"First Name"),125));
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Last Name"),125));
				gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Birthdate"),75, sortingStrategy: ODGridSortingStrategy.DateParse));
				if(Preferences.HasClinicsEnabled) {
					gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Clinic"),75));
				}
			}
			gridMain.Rows.Clear();
			//Mimics FormPatientEdit
			string patientStatus=Lan.g("enumPatientStatus","Patient");
			string inactiveStatus=Lan.g("enumPatientStatus","Inactive");
			ODGridRow row;
			foreach(Patient pat in listPatients) {
				row=new ODGridRow();
				row.Cells.Add(POut.Long(pat.PatNum));
				row.Cells.Add(pat.PatStatus==PatientStatus.Patient ? patientStatus : inactiveStatus);
				row.Cells.Add(pat.PatStatus==PatientStatus.Patient ? inactiveStatus : patientStatus);
				row.Cells.Add(pat.FName);
				row.Cells.Add(pat.LName);
				row.Cells.Add(pat.Birthdate.ToShortDateString()=="01/01/0001"?"": pat.Birthdate.ToShortDateString());
				if(Preferences.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(pat.ClinicNum));
				}
				row.Tag=pat;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butDeselectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butCreateList_Click(object sender,EventArgs e) {
			if(listOptions.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select an option from the list.");
				return;
			}
			if(!odDatePickerSince.IsValid) {
				MsgBox.Show(this,"Please select a valid from and to date.");
				return;
			}
			_isConvertToPatient=radioInactiveWith.Checked;
			FillGrid();
		}

		private void butRun_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please make a selection first");
				return;
			}
			string patientStatus=Lan.g("enumPatientStatus","Patient");
			string inactiveStatus=Lan.g("enumPatientStatus","Inactive");
			string msgText=Lan.g(this,"This will change the status for selected patients from")+" "
				+(_isConvertToPatient? inactiveStatus : patientStatus)+" "
				+Lans.g(this,"to")+" "
				+(_isConvertToPatient ? patientStatus : inactiveStatus)+".\r\n"+
				Lan.g(this,"Do you wish to continue?");
			if(MessageBox.Show(msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
				return;//The user chose not to change the statuses.
			}
			StringBuilder builder=new StringBuilder();
			List<long> listPatNums=new List<long>();
			foreach(int index in gridMain.SelectedIndices) {
				Patient patOld=(Patient)gridMain.Rows[index].Tag;
				Patient patCur=patOld.Copy();
				listPatNums.Add(patCur.PatNum);
				patCur.PatStatus=(_isConvertToPatient?PatientStatus.Patient:PatientStatus.Inactive);
				Patients.UpdateRecalls(patCur,patOld,"Patient Status Tool");
				Patients.Update(patCur,patOld);
				builder.AppendLine(
					Lans.g(this,"Patient")+" "+POut.Long(patCur.PatNum)+": "+patCur.GetNameLF()+" "
					+Lans.g(this,"patient status changed from")+" "+(_isConvertToPatient ? inactiveStatus : patientStatus)+" "
					+Lans.g(this,"to")+" "+(_isConvertToPatient ? patientStatus : inactiveStatus)
				);//Like "Patient 123: John Doe patient status changed from X to Y"
			}
			MsgBoxCopyPaste msg=new MsgBoxCopyPaste(builder.ToString());
			msg.Text=Lans.g(this,"Done");
			msg.ShowDialog();
			SecurityLog.WriteMany(listPatNums,Permissions.SecurityAdmin,Lans.g(this,"Patient status changed from")+" "
				+(_isConvertToPatient ? inactiveStatus : patientStatus)+" "
				+Lans.g(this,"to")+" "+(_isConvertToPatient ? patientStatus : inactiveStatus)
				+Lans.g(this," by the Patient Status Setter tool."));
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}