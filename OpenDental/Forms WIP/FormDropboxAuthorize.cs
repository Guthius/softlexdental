using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormDropboxAuthorize:ODForm {
		// TODO: Fix me
		public ProgramProperty ProgramPropertyAccessToken;

		public FormDropboxAuthorize() {
			InitializeComponent();
			
		}

		private void FormDropboxAuthorize_Load(object sender,EventArgs e) {
			//browserOAuth.Dock=DockStyle.Fill;
			//try {
			//	string url=WebSerializer.DeserializePrimitiveOrThrow<string>(
			//			WebServiceMainHQProxy.GetWebServiceMainHQInstance().BuildOAuthUrl(Preference.GetString(PreferenceName.RegistrationKey),OAuthApplicationNames.Dropbox.ToString()));
			//	browserOAuth.Navigate(new Uri(url));
			//}
			//catch(Exception ex) {
			//	MessageBox.Show(Lan.g(this,"Error:")+"  "+ex.Message);
			//}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//try {				
			//	string accessTokenFinal=WebSerializer.DeserializePrimitiveOrThrow<string>(
			//		WebServiceMainHQProxy.GetWebServiceMainHQInstance().GetDropboxAccessToken(WebSerializer.SerializePrimitive<string>(textAccessToken.Text)));
			//	ProgramPropertyAccessToken.PropertyValue=accessTokenFinal;
			//	ProgramProperties.Update(ProgramPropertyAccessToken);
			//	DataValid.SetInvalid(InvalidType.Programs);
			//}
			//catch(Exception ex) {
			//	MessageBox.Show(Lan.g(this,"Error:")+"  "+ex.Message);
			//	return;
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}