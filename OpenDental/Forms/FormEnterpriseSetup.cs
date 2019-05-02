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
			Lan.F(this);
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
			checkAgingMonthly.Checked=Preferences.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily);
			checkUseOpHygProv.Checked=Preferences.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly);
			checkApptsRequireProcs.Checked=Preferences.GetBool(PrefName.ApptsRequireProc);
			checkBillingShowProgress.Checked=Preferences.GetBool(PrefName.BillingShowSendProgress);
			checkBillShowTransSinceZero.Checked=Preferences.GetBool(PrefName.BillingShowTransSinceBalZero);
			checkReceiveReportsService.Checked=Preferences.GetBool(PrefName.ClaimReportReceivedByService);
			checkSuperFamCloneCreate.Checked=Preferences.GetBool(PrefName.CloneCreateSuperFamily);
			checkEnterpriseApptList.Checked=Preferences.GetBool(PrefName.EnterpriseApptList);
			checkPasswordsMustBeStrong.Checked=Preferences.GetBool(PrefName.PasswordsMustBeStrong);
			checkPasswordsStrongIncludeSpecial.Checked=Preferences.GetBool(PrefName.PasswordsStrongIncludeSpecial);
			checkPasswordForceWeakToStrong.Checked=Preferences.GetBool(PrefName.PasswordsWeakChangeToStrong);
			checkHidePaysplits.Checked=Preferences.GetBool(PrefName.PaymentWindowDefaultHideSplits);
			checkPaymentsPromptForPayType.Checked=Preferences.GetBool(PrefName.PaymentsPromptForPayType);
			checkLockIncludesAdmin.Checked=Preferences.GetBool(PrefName.SecurityLockIncludesAdmin);
			checkPatClone.Checked=Preferences.GetBool(PrefName.ShowFeaturePatientClone);
			checkSuperFam.Checked=Preferences.GetBool(PrefName.ShowFeatureSuperfamilies);
			checkUserNameManualEntry.Checked=Preferences.GetBool(PrefName.UserNameManualEntry);
			textBillingElectBatchMax.Text=Preferences.GetInt(PrefName.BillingElectBatchMax).ToString();
			textClaimIdentifier.Text=Preferences.GetString(PrefName.ClaimIdPrefix);
			//Reports, copied from FormReportSetup.
			checkUseReportServer.Checked=(Preferences.GetString(PrefName.ReportingServerCompName)!="" || Preferences.GetString(PrefName.ReportingServerURI)!="");
			textServerName.Text=Preferences.GetString(PrefName.ReportingServerCompName);
			comboDatabase.Text=Preferences.GetString(PrefName.ReportingServerDbName);
			textMysqlUser.Text=Preferences.GetString(PrefName.ReportingServerMySqlUser);
			string decryptedPass;
            Encryption.TryDecrypt(Preferences.GetString(PrefName.ReportingServerMySqlPassHash),out decryptedPass);
			textMysqlPass.Text=decryptedPass;
			textMysqlPass.PasswordChar='*';
			textMiddleTierURI.Text=Preferences.GetString(PrefName.ReportingServerURI);
			FillComboDatabases();
			SetReportServerUIEnabled();
			//Claim report receive interval.
			_claimReportReceiveInterval=Preferences.GetInt(PrefName.ClaimReportReceiveInterval);
			if(_claimReportReceiveInterval==0) {
				radioTime.Checked=true;
				DateTime fullDateTime=Preferences.GetDateTime(PrefName.ClaimReportReceiveTime);
				textReportCheckTime.Text=fullDateTime.ToShortTimeString();
			}
			else {
				textReportCheckInterval.Text=POut.Int(_claimReportReceiveInterval);
				radioInterval.Checked=true;
			}
			long sigInterval=Preferences.GetLong(PrefName.ProcessSigsIntervalInSecs);
			textSigInterval.Text=(sigInterval==0 ? "" : sigInterval.ToString());
			textDaysLock.Text=Preferences.GetInt(PrefName.SecurityLockDays).ToString();
			textDateLock.Text=Preferences.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			textLogOffAfterMinutes.Text=Preferences.GetInt(PrefName.SecurityLogOffAfterMinutes).ToString();
			long signalInactive=Preferences.GetLong(PrefName.SignalInactiveMinutes);
			textInactiveSignal.Text=(signalInactive==0 ? "" : signalInactive.ToString());
			textClaimSnapshotRunTime.Text=Preferences.GetDateTime(PrefName.ClaimSnapshotRunTime).ToShortTimeString();
			for(int i=0;i<Enum.GetNames(typeof(AutoSplitPreference)).Length;i++) {
				comboAutoSplitPref.Items.Add(Lans.g(this,Enum.GetNames(typeof(AutoSplitPreference))[i]));
			}
			comboAutoSplitPref.SelectedIndex=Preferences.GetInt(PrefName.AutoSplitLogic);
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(Preferences.GetString(PrefName.ClaimSnapshotTriggerType),true);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(Lan.g("enumPayPlanVersions",version.GetDescription()));
			}
			comboPayPlansVersion.SelectedIndex=Preferences.GetInt(PrefName.PayPlansVersion)-1;
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(Lan.g(this,prompt.GetDescription()));
			}
			comboPaymentClinicSetting.SelectedIndex=Preferences.GetInt(PrefName.PaymentClinicSetting);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=Preferences.GetInt(PrefName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=Preferences.GetInt(PrefName.RigorousAdjustments);
		}

		///<summary>Load values from database for hidden preferences if they exist.  If a pref doesn't exist then the corresponding UI is hidden.</summary>
		private void FillHiddenPrefs() {
			FillOptionalPrefBool(checkAgingEnterprise,PrefName.AgingIsEnterprise);
			FillOptionalPrefBool(checkAgingShowPayplanPayments,PrefName.AgingReportShowAgePatPayplanPayments);
			FillOptionalPrefBool(checkClaimSnapshotEnabled,PrefName.ClaimSnapshotEnabled);
			FillOptionalPrefBool(checkDBMDisableOptimize,PrefName.DatabaseMaintenanceDisableOptimize);
			FillOptionalPrefBool(checkDBMSkipCheckTable,PrefName.DatabaseMaintenanceSkipCheckTable);
			validDateAgingServiceTimeDue.Text=Preferences.GetDateTime(PrefName.AgingServiceTimeDue).ToShortTimeString();
			checkEnableClinics.Checked=Preferences.HasClinicsEnabled;
			string updateStreamline=GetHiddenPrefString(PrefName.UpdateStreamLinePassword);
			if(updateStreamline!=null) {
				checkUpdateStreamlinePassword.Checked=(updateStreamline=="abracadabra");
			}
			else {
				checkUpdateStreamlinePassword.Visible=false;
			}
		}

		///<summary>Returns the ValueString of a pref or null if that pref is not found in the database.</summary>
		private string GetHiddenPrefString(PrefName pref) {
			try {
				Pref hiddenPref=Prefs.GetOne(pref);
				return hiddenPref.ValueString;
			}
			catch {

				return null;
			}
		}

		///<summary>Helper method for setting UI for boolean preferences.  Some of the preferences calling this may not exist in the database.</summary>
		private void FillOptionalPrefBool(CheckBox checkPref,PrefName pref) {
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

		private void FillComboDatabases() {
			comboDatabase.Items.Clear();
			comboDatabase.Items.AddRange(GetDatabases());
		}

		///<summary>Taken from FormReportSetup.</summary>
		private string[] GetDatabases() {
			if(textServerName.Text=="") {
				return new string[0];
			}
			try {
				//use the one table that we know exists
				if(textMysqlUser.Text=="") {
					DataConnection.SetDb(textServerName.Text,"mysql","root",textMysqlPass.Text);
				}
				else {
                    DataConnection.SetDb(textServerName.Text,"mysql",textMysqlUser.Text,textMysqlPass.Text);
				}
				string command="SHOW DATABASES";
				//if this next step fails, table will simply have 0 rows
				DataTable table= DataConnection.GetTable(command,false);
				string[] dbNames=new string[table.Rows.Count];
				for(int i=0;i<table.Rows.Count;i++) {
					dbNames[i]=table.Rows[i][0].ToString();
				}
				return dbNames;
			}
			catch(Exception) {
				return new string[0];
			}
		}

		#endregion
		#region Update preference helpers

		private void UpdatePreferenceChanges() {
			bool hasChanges=false;
			if(Prefs.UpdateBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked)
				| Prefs.UpdateBool(PrefName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked)
				| Prefs.UpdateBool(PrefName.ApptsRequireProc,checkApptsRequireProcs.Checked)
				| Prefs.UpdateBool(PrefName.BillingShowSendProgress,checkBillingShowProgress.Checked)
				| Prefs.UpdateBool(PrefName.BillingShowTransSinceBalZero,checkBillShowTransSinceZero.Checked)
				| Prefs.UpdateBool(PrefName.ClaimReportReceivedByService,checkReceiveReportsService.Checked)
				| Prefs.UpdateBool(PrefName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked)
				| Prefs.UpdateBool(PrefName.EnterpriseApptList,checkEnterpriseApptList.Checked)
				| Prefs.UpdateBool(PrefName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked)
				| Prefs.UpdateBool(PrefName.PasswordsStrongIncludeSpecial,checkPasswordsStrongIncludeSpecial.Checked)
				| Prefs.UpdateBool(PrefName.PasswordsWeakChangeToStrong,checkPasswordForceWeakToStrong.Checked)
				| Prefs.UpdateBool(PrefName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked)
				| Prefs.UpdateBool(PrefName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked)
				| Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkLockIncludesAdmin.Checked)
				| Prefs.UpdateBool(PrefName.ShowFeaturePatientClone,checkPatClone.Checked)
				| Prefs.UpdateBool(PrefName.ShowFeatureSuperfamilies,checkSuperFam.Checked)
				| Prefs.UpdateBool(PrefName.UserNameManualEntry,checkUserNameManualEntry.Checked)
				| Prefs.UpdateInt(PrefName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text))
				| Prefs.UpdateString(PrefName.ClaimIdPrefix,textClaimIdentifier.Text)
				| Prefs.UpdateInt(PrefName.ClaimReportReceiveInterval,PIn.Int(textReportCheckInterval.Text))
				| Prefs.UpdateDateT(PrefName.ClaimReportReceiveTime,PIn.DateT(textReportCheckTime.Text))
				| Prefs.UpdateLong(PrefName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))
				//SecurityLockDate and SecurityLockDays are handled in FormSecurityLock
				//| Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(PIn.Date(textDateLock.Text),false))
				//| Prefs.UpdateInt(PrefName.SecurityLockDays,PIn.Int(textDaysLock.Text))
				| Prefs.UpdateInt(PrefName.SecurityLogOffAfterMinutes,PIn.Int(textLogOffAfterMinutes.Text))
				| Prefs.UpdateLong(PrefName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))
				| Prefs.UpdateInt(PrefName.AutoSplitLogic,comboAutoSplitPref.SelectedIndex)
				| Prefs.UpdateInt(PrefName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1)
				| Prefs.UpdateInt(PrefName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex)
			)
			{
				hasChanges=true;
			}
			int prefRigorousAccounting=Preferences.GetInt(PrefName.RigorousAccounting);
			//Copied logging for RigorousAccounting and RigorousAdjustments from FormModuleSetup.
			if(Prefs.UpdateInt(PrefName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				hasChanges=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=Preferences.GetInt(PrefName.RigorousAdjustments);
			if(Prefs.UpdateInt(PrefName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
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
				if(Prefs.UpdateString(PrefName.ReportingServerCompName,"")
					| Prefs.UpdateString(PrefName.ReportingServerDbName,"")
					| Prefs.UpdateString(PrefName.ReportingServerMySqlUser,"")
					| Prefs.UpdateString(PrefName.ReportingServerMySqlPassHash,"")
					| Prefs.UpdateString(PrefName.ReportingServerURI,"")
				)
				{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					string encryptedPass;
                    Encryption.TryEncrypt(textMysqlPass.Text,out encryptedPass);
					if(Prefs.UpdateString(PrefName.ReportingServerCompName,textServerName.Text)
						| Prefs.UpdateString(PrefName.ReportingServerDbName,comboDatabase.Text)
						| Prefs.UpdateString(PrefName.ReportingServerMySqlUser,textMysqlUser.Text)
						| Prefs.UpdateString(PrefName.ReportingServerMySqlPassHash,encryptedPass)
						| Prefs.UpdateString(PrefName.ReportingServerURI,"")
					)
					{
						changed=true;
					}
				}
				else {
					if(Prefs.UpdateString(PrefName.ReportingServerCompName,"")
						|Prefs.UpdateString(PrefName.ReportingServerDbName,"")
						|Prefs.UpdateString(PrefName.ReportingServerMySqlUser,"")
						|Prefs.UpdateString(PrefName.ReportingServerMySqlPassHash,"")
						|Prefs.UpdateString(PrefName.ReportingServerURI,textMiddleTierURI.Text)
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
			return Prefs.UpdateDateT(PrefName.ClaimSnapshotRunTime,claimSnapshotRunTime);
		}

		private bool UpdateClaimSnapshotTrigger() {
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					return Prefs.UpdateString(PrefName.ClaimSnapshotTriggerType,trigger.ToString());
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
			if(Preferences.GetInt(PrefName.SecurityLockDays)>0) {
				textDaysLock.Text=Preferences.GetInt(PrefName.SecurityLockDays).ToString();
			}
			else {
				textDaysLock.Text="";
			}
			if(Preferences.GetDate(PrefName.SecurityLockDate).Year>1880) {
				textDateLock.Text=Preferences.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}
			else {
				textDateLock.Text="";
			}
			checkLockIncludesAdmin.Checked=Preferences.GetBool(PrefName.SecurityLockIncludesAdmin);
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