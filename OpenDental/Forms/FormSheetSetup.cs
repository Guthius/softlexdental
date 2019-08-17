using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetSetup:ODForm {
		private bool changed;

		public FormSheetSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormReportSetup_Load(object sender,EventArgs e) {
			checkPatientFormsShowConsent.Checked=Preference.GetBool(PreferenceName.PatientFormsShowConsent);
		}
		
		private void butOK_Click(object sender,EventArgs e) {
			if(Preference.Update(PreferenceName.PatientFormsShowConsent,checkPatientFormsShowConsent.Checked)
				) {
				changed=true;
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}