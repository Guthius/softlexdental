using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public partial class FormShowFeatures : ODForm {

		///<summary></summary>
		public FormShowFeatures()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
		}

		private void FormShowFeatures_Load(object sender, System.EventArgs e) {
		}

		private void checkEnableClinics_Click(object sender,EventArgs e) {
		}

        private void checkEhr_Click(object sender, EventArgs e)
        {
            if (checkEhr.Checked && !File.Exists(Path.Combine(Application.StartupPath, "EHR.dll")))
            {
                checkEhr.Checked = false;
                MsgBox.Show(this, "EHR.dll could not be found.");
                return;
            }
            MsgBox.Show(this, "You will need to restart the program for the change to take effect.");
        }

		private void checkRestart_Click(object sender,EventArgs e) {
			PreferenceCheckBox checkBox=(PreferenceCheckBox)sender;
			if(checkBox.Checked!=(checkBox.Inverted?!Preference.GetBool(checkBox.Preference):Preference.GetBool(checkBox.Preference))) {
				MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(AutoSave()) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}

			//We should use ToolBut invalidation to redraw toolbars that could've been just enabled and stop forcing customers restarting.
			//DataValid.SetInvalid(InvalidType.ToolBut);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
