using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental.User_Controls {
	public partial class ContrNewPatHostedURL:UserControl {

		private bool _isExpanded;

		private const int LAUNCHWF_COL=4;//The launch webforms column of the options grid is auto-filled, keep track of its index
		private const int VERIFYTXT_COL=3;//The verify text column of the options grid can only be checked if texting is enabled, keep track of its index

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				_isExpanded=value;
				butExpander.Text=_isExpanded ? "-" : "+";
				Height=_isExpanded ? 175 : 25;
			}
		}

		private bool IsTextingEnabled
        {
            get
            {
                return !Preferences.HasClinicsEnabled && SmsPhones.IsIntegratedTextingEnabled();
            }
        }


		public ContrNewPatHostedURL() {
			InitializeComponent();
			IsExpanded=false;
			AddContextMenu(textWebFormToLaunch);
			AddContextMenu(textSchedulingURL);
			FillControl();
		}

		public string GetPrefValue(PreferenceName prefName) {
			switch(prefName) {
				case PreferenceName.WebSchedNewPatAllowChildren:
					return FromGridCell(0);
				case PreferenceName.WebSchedNewPatVerifyInfo:
					return FromGridCell(1);
				case PreferenceName.WebSchedNewPatDoAuthEmail:
					return FromGridCell(2);
				case PreferenceName.WebSchedNewPatDoAuthText:
					return FromGridCell(3);
				case PreferenceName.WebSchedNewPatWebFormsURL:
					return textWebFormToLaunch.Text;
				default: return "";
			}
		}

        private void FillControl()
        {
            labelClinicName.Text = Lan.g(this, "Headquarters");
            labelEnabled.Text = Lan.g(this, "Disabled");
            FillGrid();
        }

		private void FillGrid() {
			gridOptions.BeginUpdate();
			//Columns
			gridOptions.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Allow Children"),95,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Show Pre-Screen Questions"),180,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Verify Email"),85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Verify Text"),85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Launch WebForm on Complete"),200,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			//Rows
			gridOptions.Rows.Clear();
			ODGridRow row=new ODGridRow();
			//row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PreferenceName.WebSchedNewPatAllowChildren,Signup.ClinicNum)));
			//row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PreferenceName.WebSchedNewPatVerifyInfo,Signup.ClinicNum)));
			//row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(PreferenceName.WebSchedNewPatDoAuthEmail,Signup.ClinicNum)));
			//row.Cells.Add(IsTextingEnabled?ToGridStr(ClinicPrefs.GetBool(PreferenceName.WebSchedNewPatDoAuthText,Signup.ClinicNum)):"");
			//string url="";
			//if(Signup.ClinicNum==0) { //HQ always uses pref.
			//	url=Preference.GetString(PreferenceName.WebSchedNewPatWebFormsURL);
			//}
			//else { //Clinic should not default back to HQ version of URL. This is unlike typical ClinicPref behavior.
			//	ClinicPref pref=ClinicPrefs.GetPref(PreferenceName.WebSchedNewPatWebFormsURL,Signup.ClinicNum);			
			//	if(pref!=null) {
			//		url=pref.ValueString;
			//	}
			//}
			//row.Cells.Add(ToGridStr(!string.IsNullOrWhiteSpace(url)));
			//gridOptions.Rows.Add(row);
			gridOptions.EndUpdate();
			//SetFormToLaunch(url);			
		}

		private string ToGridStr(bool value) {
			return value ? "X" : "";
		}

		private string FromGridCell(int cellIdx) {
			return gridOptions.Rows[0].Cells[cellIdx].Text=="X" ? "1" : "0";
		}

		private static void AddContextMenu(TextBox text) {
			if(text.ContextMenuStrip==null) {
				ContextMenuStrip menu=new ContextMenuStrip();
				ToolStripMenuItem browse = new ToolStripMenuItem("Browse");
        browse.Click += (sender, e) => {
					if(!string.IsNullOrWhiteSpace(text.Text)) {
						System.Diagnostics.Process.Start(text.Text);
					}
				};
        menu.Items.Add(browse);
        ToolStripMenuItem copy = new ToolStripMenuItem("Copy");
        copy.Click += (sender, e) => text.Copy();
        menu.Items.Add(copy);
        text.ContextMenuStrip = menu;
			}
		}

		private void SetFormToLaunch(string formURL) {
			//textWebFormToLaunch.Text=formURL;
			//string extraParams="";
			//if(!string.IsNullOrWhiteSpace(formURL)) {
			//	gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="X";
			//	extraParams+="&WF=Y&ReturnURL="+formURL.Replace("&","%26");//encode &s so they aren't misinterpreted as separate parameters
			//}
			//else {
			//	gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="";
			//}
			//gridOptions.Refresh();
			//textSchedulingURL.Text=Signup.HostedUrl+extraParams;
		}

		private void butExpander_Click(object sender,EventArgs e) {
			IsExpanded=!IsExpanded;
		}

		private void butEdit_Click(object sender,EventArgs e) {
			//FormWebFormSetup formWFS=new FormWebFormSetup(Signup.ClinicNum,true);
			//formWFS.ShowDialog();
			//if(formWFS.DialogResult==DialogResult.OK) {
			//	SetFormToLaunch(formWFS.SheetURLs);
			//}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			try {
				Clipboard.SetText(textSchedulingURL.Text);
			}
			catch(Exception ex) {
                FormFriendlyException.Show(Lan.g(this,"Unable to copy to clipboard."),ex);
			}
		}

		private void gridOptions_CellClick(object sender,ODGridClickEventArgs e) {
			//Cell coordinates are [e.Row][e.Col]
			switch(e.Col) {
				case LAUNCHWF_COL: //This column is not checkable so just return and don't allow anything.
					return;
				case VERIFYTXT_COL:
					if(!IsTextingEnabled) {
						MsgBox.Show(this,"Texting not enabled"+(Preferences.HasClinicsEnabled?" for this clinic":""));
						return;
					}
					break;
				default: break;
			}
			string cellTextCur=gridOptions.Rows[e.Row].Cells[e.Col].Text;
			string cellTextNew=(cellTextCur=="X" ? "" : "X");
			gridOptions.Rows[e.Row].Cells[e.Col].Text=cellTextNew;
			gridOptions.Refresh();
		}
	}
}
