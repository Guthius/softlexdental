﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using System.Globalization;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizPractice:SetupWizardControl {
		///<summary>Deep copy of the short providers cache.  Refilled every time FillControls() is invoked.</summary>
		private List<Provider> _listProviders;

		public UserControlSetupWizPractice() {
			InitializeComponent();
		}

		private void UserControlSetupWizPractice_Load(object sender,EventArgs e) {
			FillControls();
		}

		private void FillControls() {
            _listProviders = Provider.All().ToList();
			textPracticeTitle.Text=Preference.GetString(PreferenceName.PracticeTitle);
			textAddress.Text=Preference.GetString(PreferenceName.PracticeAddress);
			textAddress2.Text=Preference.GetString(PreferenceName.PracticeAddress2);
			textCity.Text=Preference.GetString(PreferenceName.PracticeCity);
			textST.Text=Preference.GetString(PreferenceName.PracticeST);
			textZip.Text=Preference.GetString(PreferenceName.PracticeZip);
			textPhone.Text=TelephoneNumbers.ReFormat(Preference.GetString(PreferenceName.PracticePhone));
			textFax.Text=TelephoneNumbers.ReFormat(Preference.GetString(PreferenceName.PracticeFax));
			listProvider.Items.Clear();
			for(int i = 0;i<_listProviders.Count;i++) {
				listProvider.Items.Add(_listProviders[i].GetLongDesc());
				if(_listProviders[i].Id==Preference.GetLong(PreferenceName.PracticeDefaultProv)) {
					listProvider.SelectedIndex=i;
				}
			}
			CheckIsDone();
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormPractice().ShowDialog();
			FillControls();
		}

		private void CheckIsDone() {
			IsDone=true;
			Error = Lan.g("FormSetupWizard","The following fields need to be corrected: ");
			string phone = textPhone.Text;//Auto formatting turned off on purpose.
			if(!TelephoneNumbers.IsNumberValidTenDigit(ref phone)) {
				IsDone=false;
				Error+="\r\n "+Lan.g("FormSetupWizard","-Practice Phone is invalid.  Must contain exactly ten digits.");
			}
			string fax = textFax.Text;//Auto formatting turned off on purpose.
			if(!TelephoneNumbers.IsNumberValidTenDigit(ref fax)) {
				IsDone=false;
				Error+="\r\n "+Lan.g("FormSetupWizard","-Practice Fax is invalid.  Must contain exactly ten digits.");
			}
			if(listProvider.SelectedIndex==-1//practice really needs a default prov
				&& _listProviders.Count > 0) 
			{
				listProvider.SelectedIndex=0;
			}
			if(_listProviders.Count > 0
				&& _listProviders[listProvider.SelectedIndex].FeeScheduleId==0)//Default provider must have a fee schedule set.
			{
				//listProvider.BackColor = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
				IsDone=false;
				Error+="\r\n "+Lan.g("FormSetupWizard","-Practice Provider must have a default fee schedule.");
			}
		}

		private void Control_Validated(object sender,EventArgs e) {
			CheckIsDone();
		}

        protected override void OnControlDone(EventArgs e)
        {
            base.OnControlDone(e);

            string phone = textPhone.Text;
            if (Application.CurrentCulture.Name == "en-US"
                || CultureInfo.CurrentCulture.Name.EndsWith("CA")) //Canadian. en-CA or fr-CA)
            {
                phone = phone.Replace("(", "");
                phone = phone.Replace(")", "");
                phone = phone.Replace(" ", "");
                phone = phone.Replace("-", "");
            }
            string fax = textFax.Text;
            if (Application.CurrentCulture.Name == "en-US"
                || CultureInfo.CurrentCulture.Name.EndsWith("CA")) //Canadian. en-CA or fr-CA)
            {
                fax = fax.Replace("(", "");
                fax = fax.Replace(")", "");
                fax = fax.Replace("-", "");
                if (fax.Length != 0 && fax.Length != 10)
                {
                    textFax.BackColor = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
                    IsDone = false;
                }
            }
            bool changed = false;
            if (Preference.Update(PreferenceName.PracticeTitle, textPracticeTitle.Text)
                | Preference.Update(PreferenceName.PracticeAddress, textAddress.Text)
                | Preference.Update(PreferenceName.PracticeAddress2, textAddress2.Text)
                | Preference.Update(PreferenceName.PracticeCity, textCity.Text)
                | Preference.Update(PreferenceName.PracticeST, textST.Text)
                | Preference.Update(PreferenceName.PracticeZip, textZip.Text)
                | Preference.Update(PreferenceName.PracticePhone, phone)
                | Preference.Update(PreferenceName.PracticeFax, fax))
            {
                changed = true;
            }
            if (listProvider.SelectedIndex != -1)
            {
                if (Preference.Update(PreferenceName.PracticeDefaultProv, _listProviders[listProvider.SelectedIndex].Id))
                {
                    changed = true;
                }
            }
            if (changed)
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }
            //FormEServicesSetup.UploadPreference(PrefName.PracticeTitle);
        }
	}
}
