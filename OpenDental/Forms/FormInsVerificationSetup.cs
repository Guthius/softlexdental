using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsVerificationSetup:ODForm {
		private bool _hasChanged;

		public FormInsVerificationSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormInsVerificationSetup_Load(object sender,EventArgs e) {
			textInsBenefitEligibilityDays.Text=POut.Int(Preference.GetInt(PreferenceName.InsVerifyBenefitEligibilityDays));
			textPatientEnrollmentDays.Text=POut.Int(Preference.GetInt(PreferenceName.InsVerifyPatientEnrollmentDays));
			textScheduledAppointmentDays.Text=POut.Int(Preference.GetInt(PreferenceName.InsVerifyAppointmentScheduledDays));
			textPastDueDays.Text=POut.Int(Preference.GetInt(PreferenceName.InsVerifyDaysFromPastDueAppt));
			checkInsVerifyUseCurrentUser.Checked=Preference.GetBool(PreferenceName.InsVerifyDefaultToCurrentUser);
			checkInsVerifyExcludePatVerify.Checked=Preference.GetBool(PreferenceName.InsVerifyExcludePatVerify);
			checkFutureDateBenefitYear.Checked=Preference.GetBool(PreferenceName.InsVerifyFutureDateBenefitYear);
			if(!Preference.GetBool(PreferenceName.ShowFeaturePatientClone)) {
				checkExcludePatientClones.Visible=false;
			}
			else {
				checkExcludePatientClones.Checked=Preference.GetBool(PreferenceName.InsVerifyExcludePatientClones);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textInsBenefitEligibilityDays.errorProvider1.GetError(textInsBenefitEligibilityDays)!="") {
				MsgBox.Show(this,"The number entered for insurance benefit eligibility was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textPatientEnrollmentDays.errorProvider1.GetError(textPatientEnrollmentDays)!="") {
				MsgBox.Show(this,"The number entered for patient enrollment was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textScheduledAppointmentDays.errorProvider1.GetError(textScheduledAppointmentDays)!="") {
				MsgBox.Show(this,"The number entered for scheduled appointments was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textPastDueDays.errorProvider1.GetError(textPastDueDays)!="") {
				MsgBox.Show(this,"The number entered for appointment days past due was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			int insBenefitEligibilityDays=PIn.Int(textInsBenefitEligibilityDays.Text);
			int patientEnrollmentDays=PIn.Int(textPatientEnrollmentDays.Text);
			int scheduledAppointmentDays=PIn.Int(textScheduledAppointmentDays.Text);
			int pastDueDays=PIn.Int(textPastDueDays.Text);
			if(Preference.Update(PreferenceName.InsVerifyBenefitEligibilityDays,insBenefitEligibilityDays)
				| Preference.Update(PreferenceName.InsVerifyPatientEnrollmentDays,patientEnrollmentDays)
				| Preference.Update(PreferenceName.InsVerifyAppointmentScheduledDays,scheduledAppointmentDays)
				| Preference.Update(PreferenceName.InsVerifyDaysFromPastDueAppt,pastDueDays)
				| Preference.Update(PreferenceName.InsVerifyExcludePatVerify,checkInsVerifyExcludePatVerify.Checked)
				| Preference.Update(PreferenceName.InsVerifyExcludePatientClones,checkExcludePatientClones.Checked)
				| Preference.Update(PreferenceName.InsVerifyFutureDateBenefitYear,checkFutureDateBenefitYear.Checked)
				| Preference.Update(PreferenceName.InsVerifyDefaultToCurrentUser,checkInsVerifyUseCurrentUser.Checked)) 
			{
				_hasChanged=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormInsVerificationSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}