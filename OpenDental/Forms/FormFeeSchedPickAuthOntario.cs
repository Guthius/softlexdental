using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFeeSchedPickAuthOntario:ODForm {

		public string ODAMemberNumber {
			get {
				return textODAMemberNumber.Text;
			}
		}

		public string ODAMemberPassword {
			get {
				return textODAMemberPassword.Text;
			}
		}

		public FormFeeSchedPickAuthOntario() {
			InitializeComponent();
			
		}

		private void FormFeeSchedPickAuthOntario_Load(object sender,EventArgs e) {
			textODAMemberNumber.Text=Preference.GetString(PreferenceName.CanadaODAMemberNumber);
			textODAMemberPassword.Text=Preference.GetString(PreferenceName.CanadaODAMemberPass);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textODAMemberNumber.Text=="") {
				MsgBox.Show(this,"ODA Member Number cannot be blank.");
				return;
			}
			if(textODAMemberPassword.Text=="") {
				MsgBox.Show(this,"ODA Member Password cannot be blank.");
				return;
			}
			Preference.Update(PreferenceName.CanadaODAMemberNumber,textODAMemberNumber.Text);
			Preference.Update(PreferenceName.CanadaODAMemberPass,textODAMemberPassword.Text);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}