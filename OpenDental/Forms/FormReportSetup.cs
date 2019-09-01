using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.IO;
using CodeBase;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace OpenDental {
	public partial class FormReportSetup:ODForm {
		private bool changed;
		///<summary>Either the currently logged in user or the user of a group selected in the Security window.</summary>
		private long _userGroupNum;
		private bool _isPermissionMode;
		public bool HasReportPerms; 

		public FormReportSetup(long userGroupNum,bool isPermissionMode) {
			InitializeComponent();
			
			_userGroupNum=userGroupNum;
			_isPermissionMode=isPermissionMode;
		}

		private void FormReportSetup_Load(object sender,EventArgs e) {
			if(!Preferences.HasClinicsEnabled) {
				checkReportPIClinic.Visible=false;
				checkReportPIClinicInfo.Visible=false;
			}
			FillComboReportWriteoff();
			comboReportWriteoff.SelectedIndex=Preference.GetInt(PreferenceName.ReportsPPOwriteoffDefaultToProcDate);
			checkProviderPayrollAllowToday.Checked=Preference.GetBool(PreferenceName.ProviderPayrollAllowToday);
			checkNetProdDetailUseSnapshotToday.Checked=Preference.GetBool(PreferenceName.NetProdDetailUseSnapshotToday);
			checkReportsShowPatNum.Checked=Preference.GetBool(PreferenceName.ReportsShowPatNum);
			checkReportProdWO.Checked=Preference.GetBool(PreferenceName.ReportPandIschedProdSubtractsWO);
			checkReportPIClinicInfo.Checked=Preference.GetBool(PreferenceName.ReportPandIhasClinicInfo);
			checkReportPIClinic.Checked=Preference.GetBool(PreferenceName.ReportPandIhasClinicBreakdown);
			checkReportPrintWrapColumns.Checked=Preference.GetBool(PreferenceName.ReportsWrapColumns);
			checkReportsShowHistory.Checked=Preference.GetBool(PreferenceName.ReportsShowHistory);
			checkReportsIncompleteProcsNoNotes.Checked=Preference.GetBool(PreferenceName.ReportsIncompleteProcsNoNotes);
			checkReportsIncompleteProcsUnsigned.Checked=Preference.GetBool(PreferenceName.ReportsIncompleteProcsUnsigned);
			checkBenefitAssumeGeneral.Checked=Preference.GetBool(PreferenceName.TreatFinderProcsAllGeneral);
			checkOutstandingRpDateTab.Checked=Preference.GetBool(PreferenceName.OutstandingInsReportDateFilterTab);
      FillReportServer();
			userControlReportSetup.InitializeOnStartup(true,_userGroupNum,_isPermissionMode);
			if(_isPermissionMode) {
				tabControl1.SelectedIndex=1;
			}
		}

		private void FillReportServer() {
			checkUseReportServer.Checked=Preference.GetString(PreferenceName.ReportingServerCompName)!="" || Preference.GetString(PreferenceName.ReportingServerURI)!="";
			radioReportServerDirect.Checked=Preference.GetString(PreferenceName.ReportingServerURI)=="";
			radioReportServerMiddleTier.Checked=Preference.GetString(PreferenceName.ReportingServerURI)!="";
			comboServerName.Text=Preference.GetString(PreferenceName.ReportingServerCompName);
			comboDatabase.Text=Preference.GetString(PreferenceName.ReportingServerDbName);
			textMysqlUser.Text=Preference.GetString(PreferenceName.ReportingServerMySqlUser);
			string decryptedPass;
            Encryption.TryDecrypt(Preference.GetString(PreferenceName.ReportingServerMySqlPassHash),out decryptedPass);
			textMysqlPass.Text=decryptedPass;
			textMysqlPass.PasswordChar='*';
			textMiddleTierURI.Text=Preference.GetString(PreferenceName.ReportingServerURI);
			FillComboComputers();
			SetReportServerUIEnabled();
		}

		private void FillComboComputers() {
			comboServerName.Items.Clear();
			comboServerName.Items.AddRange(GetComputerNames());
		}

		private void FillComboReportWriteoff() {
			comboReportWriteoff.Items.Clear();
			foreach(PPOWriteoffDateCalc val in Enum.GetValues(typeof(PPOWriteoffDateCalc))){
				comboReportWriteoff.Items.Add(val.GetDescription());
			}
		}

		private void SetReportServerUIEnabled() {
			if(!checkUseReportServer.Checked) {
				radioReportServerDirect.Enabled=false;
				radioReportServerMiddleTier.Enabled=false;
				groupConnectionSettings.Enabled=false;
				groupMiddleTier.Enabled=false;
			}
			else {
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
		}

		///<summary>Gets a list of all computer names on the network (this is not easy)</summary>
		private string[] GetComputerNames() {
			if(Environment.OSVersion.Platform==PlatformID.Unix) {
				return new string[0];
			}
			try {
				File.Delete(Path.Combine(Application.StartupPath,"tempCompNames.txt"));
				ArrayList retList = new ArrayList();
				//string myAdd=Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();//obsolete
				string myAdd = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName=@"C:\WINDOWS\system32\cmd.exe";//Path for the cmd prompt
				psi.Arguments="/c net view > tempCompNames.txt";//Arguments for the command prompt
				//"/c" tells it to run the following command which is "net view > tempCompNames.txt"
				//"net view" lists all the computers on the network
				//" > tempCompNames.txt" tells dos to put the results in a file called tempCompNames.txt
				psi.WindowStyle=ProcessWindowStyle.Hidden;//Hide the window
				Process.Start(psi);
				StreamReader sr = null;
				string filename = Path.Combine(Application.StartupPath,"tempCompNames.txt");
				Thread.Sleep(200);//sleep for 1/5 second
				if(!File.Exists(filename)) {
					return new string[0];
				}
				try {
					sr=new StreamReader(filename);
				}
				catch(Exception) {
				}
				while(!sr.ReadLine().StartsWith("--")) {
					//The line just before the data looks like: --------------------------
				}
				string line = "";
				retList.Add("localhost");
				while(true) {
					line=sr.ReadLine();
					if(line.StartsWith("The"))//cycle until we reach,"The command completed successfully."
					break;
					line=line.Split(char.Parse(" "))[0];// Split the line after the first space
																	// Normally, in the file it lists it like this
																	// \\MyComputer                 My Computer's Description
																	// Take off the slashes, "\\MyComputer" to "MyComputer"
					retList.Add(line.Substring(2,line.Length-2));
				}
				sr.Close();
				File.Delete(Path.Combine(Application.StartupPath,"tempCompNames.txt"));
				string[] retArray = new string[retList.Count];
				retList.CopyTo(retArray);
				return retArray;
			}
			catch(Exception) {//it will always fail if not WinXP
				return new string[0];
			}
		}

		private void checkReportingServer_CheckChanged(object sender,EventArgs e) {
			SetReportServerUIEnabled();
		}

		private void comboDatabase_DropDown(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			Cursor=Cursors.Default;
		}

		private void tabControl1_SelectedIndexChanged(object sender,EventArgs e) {
			if(tabControl1.SelectedIndex==0) {
				userControlReportSetup.Parent=tabDisplaySettings;
				userControlReportSetup.InitializeOnStartup(false,_userGroupNum,false);//This will change usergroups when they change tabs.  NOT what we want......
			}
			else if(tabControl1.SelectedIndex==1) {
				if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
					tabControl1.SelectedIndex=0;
					return;
				}
				userControlReportSetup.Parent=tabReportPermissions;
				userControlReportSetup.InitializeOnStartup(false,_userGroupNum,true);
			}
		}

		private bool UpdateReportingServer() {
			bool changed=false;
			if(!checkUseReportServer.Checked) {
				if(Preference.Update(PreferenceName.ReportingServerCompName,"")
						| Preference.Update(PreferenceName.ReportingServerDbName,"")
						| Preference.Update(PreferenceName.ReportingServerMySqlUser,"")
						| Preference.Update(PreferenceName.ReportingServerMySqlPassHash,"")
						| Preference.Update(PreferenceName.ReportingServerURI,"")) 
					{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					string encryptedPass;
                    Encryption.TryEncrypt(textMysqlPass.Text,out encryptedPass);
					if(Preference.Update(PreferenceName.ReportingServerCompName,comboServerName.Text)
							| Preference.Update(PreferenceName.ReportingServerDbName,comboDatabase.Text)
							| Preference.Update(PreferenceName.ReportingServerMySqlUser,textMysqlUser.Text)
							| Preference.Update(PreferenceName.ReportingServerMySqlPassHash,encryptedPass)
							| Preference.Update(PreferenceName.ReportingServerURI,"")
					) {
					changed=true;
					}
				}
				else {
					if(Preference.Update(PreferenceName.ReportingServerCompName,"")
							|Preference.Update(PreferenceName.ReportingServerDbName,"")
							|Preference.Update(PreferenceName.ReportingServerMySqlUser,"")
							|Preference.Update(PreferenceName.ReportingServerMySqlPassHash,"")
							|Preference.Update(PreferenceName.ReportingServerURI,textMiddleTierURI.Text)
					) {
					changed=true;
					}
				}
			}

			return changed;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Preference.Update(PreferenceName.ReportsPPOwriteoffDefaultToProcDate,comboReportWriteoff.SelectedIndex)
				| Preference.Update(PreferenceName.ReportsShowPatNum,checkReportsShowPatNum.Checked)
				| Preference.Update(PreferenceName.ReportPandIschedProdSubtractsWO,checkReportProdWO.Checked)
				| Preference.Update(PreferenceName.ReportPandIhasClinicInfo,checkReportPIClinicInfo.Checked)
				| Preference.Update(PreferenceName.ReportPandIhasClinicBreakdown,checkReportPIClinic.Checked)
				| Preference.Update(PreferenceName.ProviderPayrollAllowToday,checkProviderPayrollAllowToday.Checked)
				| Preference.Update(PreferenceName.NetProdDetailUseSnapshotToday,checkNetProdDetailUseSnapshotToday.Checked)
				| Preference.Update(PreferenceName.ReportsWrapColumns,checkReportPrintWrapColumns.Checked)
				| Preference.Update(PreferenceName.ReportsIncompleteProcsNoNotes,checkReportsIncompleteProcsNoNotes.Checked)
				| Preference.Update(PreferenceName.ReportsIncompleteProcsUnsigned,checkReportsIncompleteProcsUnsigned.Checked)
				| Preference.Update(PreferenceName.TreatFinderProcsAllGeneral,checkBenefitAssumeGeneral.Checked)
				| Preference.Update(PreferenceName.ReportsShowHistory,checkReportsShowHistory.Checked)
				| Preference.Update(PreferenceName.OutstandingInsReportDateFilterTab,checkOutstandingRpDateTab.Checked)
				) {
				changed=true;
			}
      if(UpdateReportingServer()) {
				
        changed=true;
      }
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(Security.IsAuthorized(Permissions.SecurityAdmin)) {
				GroupPermissions.Sync(userControlReportSetup.ListGroupPermissionsForReports,userControlReportSetup.ListGroupPermissionsOld);
				if(userControlReportSetup.ListGroupPermissionsForReports.Exists(x => x.UserGroupNum==_userGroupNum)) {
					HasReportPerms=true;
				}
				DataValid.SetInvalid(InvalidType.Security);
			}
			if(DisplayReports.Sync(userControlReportSetup.ListDisplayReportAll)) {
				DataValid.SetInvalid(InvalidType.DisplayReports);
			}
			DialogResult=DialogResult.OK;
		}

    private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}