using System;
using OpenDentBusiness;
using CodeBase;
using System.Globalization;
using System.IO;
using System.Drawing;

namespace OpenDental {
	///<summary>Launches a splash screen with a progress bar that will listen specifically for ODEvents with the passed in name.
	///Returns an ODProgressWindow that has a Close method that should be invoked whenever the progress window should close.
	///eventName should be set to the name of the ODEvents that this specific progress window should be processing.</summary>
	public partial class FormSplash : FormProgressBase {

		///<summary>Do not instatiate this class.  It is not meant for public use.  Use ODProgress.ShowSplash() instead.
		///Launches a splash screen with a progress bar that will display status updates for SplashProgressEvent events of type SplashScreenProgress.
		///Set hasProgress to true in order to show the actual progress bar and label to the user.  Hidden by default.</summary>
		public FormSplash(bool hasProgress=false) : base(ODEventType.SplashScreenProgress,typeof(SplashProgressEvent)) {
			InitializeComponent();
			Lan.F(this);
			if(!hasProgress) {
				progressBar.Hide();
				labelProgress.Hide();
				this.ClientSize=new Size(500,300);
			}
		}

		private void FormSplash_Load(object sender,EventArgs e) {
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				BackgroundImage=Properties.Resources.splashCanada;
			}
			if(File.Exists(Directory.GetCurrentDirectory()+@"\Splash.jpg")) {
				BackgroundImage=new Bitmap(Directory.GetCurrentDirectory()+@"\Splash.jpg");
			}
			if(Plugins.PluginsAreLoaded) {
				Plugins.HookAddCode(this,"FormSplash.FormSplash_Load_end");
			}
		}

		public sealed override void UpdateProgress(string status,ProgressBarHelper progHelper,bool hasProgHelper) {
			labelProgress.Text=status+"... ("+progHelper.PercentValue+")";
			if(hasProgHelper) {
				if(progHelper.BlockMax!=0) {
					progressBar.Maximum=progHelper.BlockMax;
				}
				if(progHelper.BlockValue!=0) {
					//When the progress bar draws itself it gradually fills in the bar. This causes the progress bar to be much further behind the percent
					//label when the program loads quickly. A trick to get around this is to set the value and then set the value to a lower value.
					progressBar.Value=progHelper.BlockValue;
					progressBar.Value=Math.Max(progHelper.BlockValue-1,0);
					progressBar.Value=progHelper.BlockValue;
				}
			}
		}
  }
}