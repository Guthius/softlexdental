using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;


namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizFeatures:SetupWizardControl {
		public UserControlSetupWizFeatures() {
			InitializeComponent();
		}

		private void UserControlSetupWizFeatures_Load(object sender,EventArgs e) {
			RefreshControls();
		}

		private void RefreshControls() {
			checkCapitation.Checked=!Preference.GetBool(PreferenceName.EasyHideCapitation);
			checkMedicaid.Checked=!Preference.GetBool(PreferenceName.EasyHideMedicaid);
			checkInsurance.Checked=!Preference.GetBool(PreferenceName.EasyHideInsurance);
			checkClinical.Checked=!Preference.GetBool(PreferenceName.EasyHideClinical);
			checkNoClinics.Checked=Preferences.HasClinicsEnabled;
			checkMedicalIns.Checked=Preference.GetBool(PreferenceName.ShowFeatureMedicalInsurance);
			checkEhr.Checked=Preference.GetBool(PreferenceName.ShowFeatureEhr);
			IsDone=true;
		}

		private void labelInfo_MouseClick(object sender,MouseEventArgs e) {
			panelInfo.Controls.OfType<Label>().ToList().ForEach(x => x.ImageIndex=0);
			//foreach(Control item in panelInfo.Controls) {
			//	if(item.GetType() == typeof(Label)) {
			//		((Label)item).ImageIndex = 0;
			//	}
			//}
			((Label)sender).ImageIndex = 1;
			labelExplanation.Text = (string)((Label)sender).Tag;
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormShowFeatures().ShowDialog();
			RefreshControls();
		}

        protected override void OnControlDone(EventArgs e)
        {
            if (
                Preference.Update(PreferenceName.EasyHideCapitation, !checkCapitation.Checked)
                | Preference.Update(PreferenceName.EasyHideMedicaid, !checkMedicaid.Checked)
                | Preference.Update(PreferenceName.EasyHideInsurance, !checkInsurance.Checked)
                | Preference.Update(PreferenceName.EasyHideClinical, !checkClinical.Checked)
                | Preference.Update(PreferenceName.EasyNoClinics, !checkNoClinics.Checked)
                | Preference.Update(PreferenceName.ShowFeatureMedicalInsurance, checkMedicalIns.Checked)
                | Preference.Update(PreferenceName.ShowFeatureEhr, checkEhr.Checked)
            )
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }
        }
	}
}
