using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLoginFailed:ODForm {
		private string _errorMsg;

		///<summary></summary>
		public FormLoginFailed(string errorMessage) {
			InitializeComponent();
			_errorMsg=errorMessage;
		}

		private void FormLoginFailed_Load(object sender,EventArgs e) {
			labelErrMsg.Text=_errorMsg;
			textUser.Text=Security.CurrentUser.UserName;//CurUser verified to not be null in FormOpenDental before loading this form
			textPassword.Focus();
		}

		private void butLogin_Click(object sender,EventArgs e) {
			User userEntered;
			string password;
			try {

				//ecw requires hash, but non-ecw requires actual password
				password=textPassword.Text;

				string username=textUser.Text;
				#if DEBUG
				if(username=="") {
					username="Admin";
					password="pass";
				}
				#endif
				//Set the PasswordTyped property prior to checking the credentials for Middle Tier.
				Security.PasswordTyped=password;
				userEntered= User.CheckUserAndPassword(username,password);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			//successful login.
			Security.CurrentUser=userEntered;
			Security.IsUserLoggedIn=true;

			if(Preference.GetBool(PreferenceName.PasswordsMustBeStrong)
				&& Preference.GetBool(PreferenceName.PasswordsWeakChangeToStrong)
				&& User.IsPasswordStrong(textPassword.Text)!="") //Password is not strong
			{
				MsgBox.Show(this,"You must change your password to a strong password due to the current Security settings.");
				if(!SecurityL.ChangePassword(true)) {//Failed password update.
					return;
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurrentUser.Id+" has logged on.");
			DialogResult=DialogResult.OK;
		}

		private void butExit_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}