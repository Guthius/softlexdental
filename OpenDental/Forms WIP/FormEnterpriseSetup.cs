using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEnterpriseSetup:ODForm {
		private int _claimReportReceiveInterval;

		public FormEnterpriseSetup() {
			InitializeComponent();
			
		}

		private void FormEnterpriseSetup_Load(object sender,EventArgs e) {
			FillStandardPrefs();
			try {
				FillHiddenPrefs();
			}
			catch {
				//Suppress unhandled exceptions from hidden preferences, since they are read only.
			}
		}

		/// <summary>Sets UI for preferences that we know for sure will exist.</summary>
		private void FillStandardPrefs() {
			checkAgingMonthly.Checked=Preference.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily);
			checkUseOpHygProv.Checked=Preference.GetBool(PreferenceName.ApptSecondaryProviderConsiderOpOnly);
			checkApptsRequireProcs.Checked=Preference.GetBool(PreferenceName.ApptsRequireProc);
			checkBillingShowProgress.Checked=Preference.GetBool(PreferenceName.BillingShowSendProgress);
			checkBillShowTransSinceZero.Checked=Preference.GetBool(PreferenceName.BillingShowTransSinceBalZero);
			checkReceiveReportsService.Checked=Preference.GetBool(PreferenceName.ClaimReportReceivedByService);
			checkSuperFamCloneCreate.Checked=Preference.GetBool(PreferenceName.CloneCreateSuperFamily);
			checkEnterpriseApptList.Checked=Preference.GetBool(PreferenceName.EnterpriseApptList);
			checkPasswordsMustBeStrong.Checked=Preference.GetBool(PreferenceName.PasswordsMustBeStrong);
			checkPasswordsStrongIncludeSpecial.Checked=Preference.GetBool(PreferenceName.PasswordsStrongIncludeSpecial);
			checkPasswordForceWeakToStrong.Checked=Preference.GetBool(PreferenceName.PasswordsWeakChangeToStrong);
			checkHidePaysplits.Checked=Preference.GetBool(PreferenceName.PaymentWindowDefaultHideSplits);
			checkPaymentsPromptForPayType.Checked=Preference.GetBool(PreferenceName.PaymentsPromptForPayType);
			checkLockIncludesAdmin.Checked=Preference.GetBool(PreferenceName.SecurityLockIncludesAdmin);
			checkPatClone.Checked=Preference.GetBool(PreferenceName.ShowFeaturePatientClone);
			checkSuperFam.Checked=Preference.GetBool(PreferenceName.ShowFeatureSuperfamilies);
			checkUserNameManualEntry.Checked=Preference.GetBool(PreferenceName.UserNameManualEntry);
			textBillingElectBatchMax.Text=Preference.GetInt(PreferenceName.BillingElectBatchMax).ToString();
			textClaimIdentifier.Text=Preference.GetString(PreferenceName.ClaimIdPrefix);
			//Reports, copied from FormReportSetup.
			checkUseReportServer.Checked=(Preference.GetString(PreferenceName.ReportingServerCompName)!="" || Preference.GetString(PreferenceName.ReportingServerURI)!="");
			textServerName.Text=Preference.GetString(PreferenceName.ReportingServerCompName);
			comboDatabase.Text=Preference.GetString(PreferenceName.ReportingServerDbName);
			textMysqlUser.Text=Preference.GetString(PreferenceName.ReportingServerMySqlUser);
			string decryptedPass;
            Encryption.TryDecrypt(Preference.GetString(PreferenceName.ReportingServerMySqlPassHash),out decryptedPass);
			textMysqlPass.Text=decryptedPass;
			textMysqlPass.PasswordChar='*';
			textMiddleTierURI.Text=Preference.GetString(PreferenceName.ReportingServerURI);
			SetReportServerUIEnabled();
			//Claim report receive interval.
			_claimReportReceiveInterval=Preference.GetInt(PreferenceName.ClaimReportReceiveInterval);
			if(_claimReportReceiveInterval==0) {
				radioTime.Checked=true;
				DateTime fullDateTime=Preference.GetDateTime(PreferenceName.ClaimReportReceiveTime);
				textReportCheckTime.Text=fullDateTime.ToShortTimeString();
			}
			else {
				textReportCheckInterval.Text=POut.Int(_claimReportReceiveInterval);
				radioInterval.Checked=true;
			}
			long sigInterval=Preference.GetLong(PreferenceName.ProcessSigsIntervalInSecs);
			textSigInterval.Text=(sigInterval==0 ? "" : sigInterval.ToString());
			textDaysLock.Text=Preference.GetInt(PreferenceName.SecurityLockDays).ToString();
			textDateLock.Text=Preference.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			textLogOffAfterMinutes.Text=Preference.GetInt(PreferenceName.SecurityLogOffAfterMinutes).ToString();
			long signalInactive=Preference.GetLong(PreferenceName.SignalInactiveMinutes);
			textInactiveSignal.Text=(signalInactive==0 ? "" : signalInactive.ToString());
			textClaimSnapshotRunTime.Text=Preference.GetDateTime(PreferenceName.ClaimSnapshotRunTime).ToShortTimeString();
			for(int i=0;i<Enum.GetNames(typeof(AutoSplitPreference)).Length;i++) {
				comboAutoSplitPref.Items.Add(Lans.g(this,Enum.GetNames(typeof(AutoSplitPreference))[i]));
			}
			comboAutoSplitPref.SelectedIndex=Preference.GetInt(PreferenceName.AutoSplitLogic);
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(Preference.GetString(PreferenceName.ClaimSnapshotTriggerType),true);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(Lan.g("enumPayPlanVersions",version.GetDescription()));
			}
			comboPayPlansVersion.SelectedIndex=Preference.GetInt(PreferenceName.PayPlansVersion)-1;
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(Lan.g(this,prompt.GetDescription()));
			}
			comboPaymentClinicSetting.SelectedIndex=Preference.GetInt(PreferenceName.PaymentClinicSetting);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=Preference.GetInt(PreferenceName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=Preference.GetInt(PreferenceName.RigorousAdjustments);
		}

		///<summary>Load values from database for hidden preferences if they exist.  If a pref doesn't exist then the corresponding UI is hidden.</summary>
		private void FillHiddenPrefs() {
			FillOptionalPrefBool(checkAgingEnterprise,PreferenceName.AgingIsEnterprise);
			FillOptionalPrefBool(checkAgingShowPayplanPayments,PreferenceName.AgingReportShowAgePatPayplanPayments);
			FillOptionalPrefBool(checkClaimSnapshotEnabled,PreferenceName.ClaimSnapshotEnabled);
			FillOptionalPrefBool(checkDBMDisableOptimize,PreferenceName.DatabaseMaintenanceDisableOptimize);
			FillOptionalPrefBool(checkDBMSkipCheckTable,PreferenceName.DatabaseMaintenanceSkipCheckTable);
			validDateAgingServiceTimeDue.Text=Preference.GetDateTime(PreferenceName.AgingServiceTimeDue).ToShortTimeString();
			checkEnableClinics.Checked=Preferences.HasClinicsEnabled;
			string updateStreamline=GetHiddenPrefString(PreferenceName.UpdateStreamLinePassword);
			if(updateStreamline!=null) {
				checkUpdateStreamlinePassword.Checked=(updateStreamline=="abracadabra");
			}
			else {
				checkUpdateStreamlinePassword.Visible=false;
			}
		}

		///<summary>Returns the ValueString of a pref or null if that pref is not found in the database.</summary>
		private string GetHiddenPrefString(PreferenceName pref) {
			try {
				Preference hiddenPref= Preference.GetByName(pref);
				return hiddenPref.Value;
			}
			catch {

				return null;
			}
		}

		///<summary>Helper method for setting UI for boolean preferences.  Some of the preferences calling this may not exist in the database.</summary>
		private void FillOptionalPrefBool(CheckBox checkPref,PreferenceName pref) {
			string valueString=GetHiddenPrefString(pref);
			if(valueString==null) {
				checkPref.Visible=false;
				return;
			}
			checkPref.Checked=PIn.Bool(valueString);
		}

		#region Report helper functions

		private void SetReportServerUIEnabled() {
			if(checkUseReportServer.Checked) {
				radioReportServerDirect.Enabled=true;
				radioReportServerMiddleTier.Enabled=true;
				if(radioReportServerDirect.Checked) {
					groupConnectionSettings.Enabled=true;
					groupMiddleTier.Enabled=false;
				}
				else {
					groupConnectionSettings.Enabled=false;
					groupMiddleTier.Enabled=true;
				}
			}
			else {
				radioReportServerDirect.Enabled=false;
				radioReportServerMiddleTier.Enabled=false;
				groupConnectionSettings.Enabled=false;
				groupMiddleTier.Enabled=false;
			}
		}

		#endregion
		#region Update preference helpers

		private void UpdatePreferenceChanges() {
			bool hasChanges=false;
			if(Preference.Update(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked)
				| Preference.Update(PreferenceName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked)
				| Preference.Update(PreferenceName.ApptsRequireProc,checkApptsRequireProcs.Checked)
				| Preference.Update(PreferenceName.BillingShowSendProgress,checkBillingShowProgress.Checked)
				| Preference.Update(PreferenceName.BillingShowTransSinceBalZero,checkBillShowTransSinceZero.Checked)
				| Preference.Update(PreferenceName.ClaimReportReceivedByService,checkReceiveReportsService.Checked)
				| Preference.Update(PreferenceName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked)
				| Preference.Update(PreferenceName.EnterpriseApptList,checkEnterpriseApptList.Checked)
				| Preference.Update(PreferenceName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked)
				| Preference.Update(PreferenceName.PasswordsStrongIncludeSpecial,checkPasswordsStrongIncludeSpecial.Checked)
				| Preference.Update(PreferenceName.PasswordsWeakChangeToStrong,checkPasswordForceWeakToStrong.Checked)
				| Preference.Update(PreferenceName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked)
				| Preference.Update(PreferenceName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked)
				| Preference.Update(PreferenceName.SecurityLockIncludesAdmin,checkLockIncludesAdmin.Checked)
				| Preference.Update(PreferenceName.ShowFeaturePatientClone,checkPatClone.Checked)
				| Preference.Update(PreferenceName.ShowFeatureSuperfamilies,checkSuperFam.Checked)
				| Preference.Update(PreferenceName.UserNameManualEntry,checkUserNameManualEntry.Checked)
				| Preference.Update(PreferenceName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text))
				| Preference.Update(PreferenceName.ClaimIdPrefix,textClaimIdentifier.Text)
				| Preference.Update(PreferenceName.ClaimReportReceiveInterval,PIn.Int(textReportCheckInterval.Text))
				| Preference.Update(PreferenceName.ClaimReportReceiveTime,PIn.DateT(textReportCheckTime.Text))
				| Preference.Update(PreferenceName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))
				//SecurityLockDate and SecurityLockDays are handled in FormSecurityLock
				//| Preference.Update(PrefName.SecurityLockDate,POut.Date(PIn.Date(textDateLock.Text),false))
				//| Prefs.UpdateInt(PrefName.SecurityLockDays,PIn.Int(textDaysLock.Text))
				| Preference.Update(PreferenceName.SecurityLogOffAfterMinutes,PIn.Int(textLogOffAfterMinutes.Text))
				| Preference.Update(PreferenceName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))
				| Preference.Update(PreferenceName.AutoSplitLogic,comboAutoSplitPref.SelectedIndex)
				| Preference.Update(PreferenceName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1)
				| Preference.Update(PreferenceName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex)
			)
			{
				hasChanges=true;
			}
			int prefRigorousAccounting=Preference.GetInt(PreferenceName.RigorousAccounting);
			//Copied logging for RigorousAccounting and RigorousAdjustments from FormModuleSetup.
			if(Preference.Update(PreferenceName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				hasChanges=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=Preference.GetInt(PreferenceName.RigorousAdjustments);
			if(Preference.Update(PreferenceName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				hasChanges=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
			hasChanges|=UpdateReportingServer();
			hasChanges|=UpdateClaimSnapshotRuntime();
			hasChanges|=UpdateClaimSnapshotTrigger();
			if(hasChanges) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		///<summary>Copied from FormReportSetup.</summary>
		private bool UpdateReportingServer() {
			bool changed=false;
			if(!checkUseReportServer.Checked) {
				if(Preference.Update(PreferenceName.ReportingServerCompName,"")
					| Preference.Update(PreferenceName.ReportingServerDbName,"")
					| Preference.Update(PreferenceName.ReportingServerMySqlUser,"")
					| Preference.Update(PreferenceName.ReportingServerMySqlPassHash,"")
					| Preference.Update(PreferenceName.ReportingServerURI,"")
				)
				{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					string encryptedPass;
                    Encryption.TryEncrypt(textMysqlPass.Text,out encryptedPass);
					if(Preference.Update(PreferenceName.ReportingServerCompName,textServerName.Text)
						| Preference.Update(PreferenceName.ReportingServerDbName,comboDatabase.Text)
						| Preference.Update(PreferenceName.ReportingServerMySqlUser,textMysqlUser.Text)
						| Preference.Update(PreferenceName.ReportingServerMySqlPassHash,encryptedPass)
						| Preference.Update(PreferenceName.ReportingServerURI,"")
					)
					{
						changed=true;
					}
				}
				else {
					if(Preference.Update(PreferenceName.ReportingServerCompName,"")
						|Preference.Update(PreferenceName.ReportingServerDbName,"")
						|Preference.Update(PreferenceName.ReportingServerMySqlUser,"")
						|Preference.Update(PreferenceName.ReportingServerMySqlPassHash,"")
						|Preference.Update(PreferenceName.ReportingServerURI,textMiddleTierURI.Text)
					)
					{
						changed=true;
					}
				}
			}
			return changed;
		}

		private bool UpdateClaimSnapshotRuntime() {
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime);//This already gets checked in the validate method.
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			return Preference.Update(PreferenceName.ClaimSnapshotRunTime,claimSnapshotRunTime);
		}

		private bool UpdateClaimSnapshotTrigger() {
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					return Preference.Update(PreferenceName.ClaimSnapshotTriggerType,trigger.ToString());
				}
			}
			return false;
		}

		///<summary>Checks preferences that take user entry for errors, returns true if all entries are valid.</summary>
		private bool ValidateEntries() {
			string errorMsg="";
			//SecurityLogOffAfterMinutes
			if(textLogOffAfterMinutes.Text!="") {
				try {
					int logOffMinutes = Int32.Parse(textLogOffAfterMinutes.Text);
					if(logOffMinutes<0) {//Automatic log off must be a positive numerical value.
						throw new Exception();
					}
				}
				catch {

					errorMsg+="Log off after minutes is invalid. Must be a positive number.\r\n";
				}
			}
			//ClaimReportReceiveInterval
			int reportCheckIntervalMinuteCount=0;
			reportCheckIntervalMinuteCount=PIn.Int(textReportCheckInterval.Text,false);
			if(textReportCheckInterval.Enabled && (reportCheckIntervalMinuteCount<5 || reportCheckIntervalMinuteCount>60)) {
				errorMsg+="Report check interval must be between 5 and 60 inclusive.\r\n";
			}
			//ClaimReportReceiveTime
			if(radioTime.Checked && (textReportCheckTime.Text=="" || !textReportCheckTime.IsEntryValid)) {
				errorMsg+="Please enter a time to receive reports.";
			}
			//ClaimSnapshotRuntime
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime)) {
				errorMsg+="Service Snapshot Run Time must be a valid time value.\r\n";
			}
			//ProcessSigsIntervalInSecs
			if(!textSigInterval.IsValid) {
				errorMsg+="Signal interval must be a valid number or blank.\r\n";
			}
			//SignalInactiveMinutes
			if(!textInactiveSignal.IsValid) {
				errorMsg+="Disable signal interval must be a valid number or blank.\r\n";
			}
			if(errorMsg!="") {
				MsgBox.Show(this,"Please fix the following errors:\r\n"+errorMsg);
				return false;
			}
			return true;

		}

		#endregion

		private void checkUseReportServer_CheckedChanged(object sender,EventArgs e) {
			SetReportServerUIEnabled();
		}

		private void radioInterval_CheckedChanged(object sender,EventArgs e) {
			//Copied from FormClearingHouses
			if(radioInterval.Checked) {
				labelReportheckUnits.Enabled=true;
				textReportCheckInterval.Enabled=true;
				textReportCheckTime.Text="";
				textReportCheckTime.Enabled=false;
				textReportCheckTime.ClearError();
			}
			else {
				labelReportheckUnits.Enabled=false;
				textReportCheckInterval.Text="";
				textReportCheckInterval.Enabled=false;
				textReportCheckTime.Enabled=true;
			}
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			//Copied from FormModuleSetup.
			FormMessageReplacements form=new FormMessageReplacements(MessageReplaceType.Patient);
			form.IsSelectionMode=true;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			textClaimIdentifier.Focus();
			int cursorIndex=textClaimIdentifier.SelectionStart;
			textClaimIdentifier.Text=textClaimIdentifier.Text.Insert(cursorIndex,form.Replacement);
			textClaimIdentifier.SelectionStart=cursorIndex+form.Replacement.Length;
		}

		private void butChange_Click(object sender,EventArgs e) {
			//Copied from FormGlobalSecurity.
			FormSecurityLock FormS=new FormSecurityLock();
			FormS.ShowDialog();//prefs are set invalid within that form if needed.
			if(Preference.GetInt(PreferenceName.SecurityLockDays)>0) {
				textDaysLock.Text=Preference.GetInt(PreferenceName.SecurityLockDays).ToString();
			}
			else {
				textDaysLock.Text="";
			}
			if(Preference.GetDate(PreferenceName.SecurityLockDate).Year>1880) {
				textDateLock.Text=Preference.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			}
			else {
				textDateLock.Text="";
			}
			checkLockIncludesAdmin.Checked=Preference.GetBool(PreferenceName.SecurityLockIncludesAdmin);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!ValidateEntries()) {
				return;
			}
			UpdatePreferenceChanges();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}