using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailSetupEHR:ODForm {
		public FormEmailSetupEHR() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailSetupEHR_Load(object sender,EventArgs e) {
			textPOPserver.Text=Preference.GetString(PreferenceName.EHREmailPOPserver);
			textUsername.Text=Preference.GetString(PreferenceName.EHREmailFromAddress);
			textPassword.Text=Preference.GetString(PreferenceName.EHREmailPassword);
			textPort.Text=Preference.GetString(PreferenceName.EHREmailPort);
		}

		private void butOK_Click(object sender,EventArgs e) {
			Preference.Update(PreferenceName.EHREmailPOPserver,textPOPserver.Text);
			Preference.Update(PreferenceName.EHREmailFromAddress,textUsername.Text);
			Preference.Update(PreferenceName.EHREmailPassword,textPassword.Text);
			try{
				Preference.Update(PreferenceName.EHREmailPort,PIn.Long(textPort.Text));
			}
			catch{
				MsgBox.Show(this,"invalid port number.");
			}
			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}