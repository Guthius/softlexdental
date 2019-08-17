using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailMasterTemplate:ODForm {

		public FormEmailMasterTemplate() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailSetupMasterTemplate_Load(object sender,EventArgs e) {
			textMaster.Text=Preference.GetString(PreferenceName.EmailMasterTemplate);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Preference.Update(PreferenceName.EmailMasterTemplate,textMaster.Text)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
