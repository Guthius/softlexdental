using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormConfirmationSetup:ODForm {
		private List<Definition> _listApptConfirmedDefs;

		public FormConfirmationSetup() {
			InitializeComponent();
			Lan.F(this);
		}


		public void FormConfirmationSetup_Load(object sender,System.EventArgs e) {
			FillTabManualConfirmation();
            Plugin.Trigger(this, "FormConfirmationSetup_Loaded");
		}

		//===============================================================================================
		#region Confirmations

		///<summary>Called on load to initially load confirmation with values from the database.  Calls FillGrid at the end.</summary>
		private void FillTabManualConfirmation() {
			_listApptConfirmedDefs=Definition.GetByCategory(DefinitionCategory.ApptConfirmed);
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboStatusEmailedConfirm.Items.Add(_listApptConfirmedDefs[i].Description);
				if(_listApptConfirmedDefs[i].Id==Preference.GetLong(PreferenceName.ConfirmStatusEmailed)) {
					comboStatusEmailedConfirm.SelectedIndex=i;
				}
			}
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboStatusTextMessagedConfirm.Items.Add(_listApptConfirmedDefs[i].Description);
				if(_listApptConfirmedDefs[i].Id==Preference.GetLong(PreferenceName.ConfirmStatusTextMessaged)) {
					comboStatusTextMessagedConfirm.SelectedIndex=i;
				}
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableConfirmMsgs","Mode"),61);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmMsgs","Message"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			#region Confirmation
			//Confirmation---------------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Confirmation message.  Use [date]  and [time] where you want those values to be inserted"));
			row.Cells.Add(Preference.GetString(PreferenceName.ConfirmPostcardMessage));
			row.Tag=PreferenceName.ConfirmPostcardMessage;
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Confirmation subject line."));
			row.Cells.Add(Preference.GetString(PreferenceName.ConfirmEmailSubject));
			row.Tag=PreferenceName.ConfirmEmailSubject;
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Confirmation message. Available variables: [NameF], [NameFL], [date], [time]."));
			row.Cells.Add(Preference.GetString(PreferenceName.ConfirmEmailMessage));
			row.Tag=PreferenceName.ConfirmEmailMessage;
			gridMain.Rows.Add(row);
			#endregion
			#region Text Messaging
			//Text Messaging----------------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add(Lan.g(this,"Text"));
			row.Cells.Add(Lan.g(this,"Confirmation message. Available variables: [NameF], [NameFL], [date], [time]."));
			row.Cells.Add(Preference.GetString(PreferenceName.ConfirmTextMessage));
			row.Tag=PreferenceName.ConfirmTextMessage;
			gridMain.Rows.Add(row);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PreferenceName prefName=(PreferenceName)gridMain.Rows[e.Row].Tag;
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			FormR.MessageVal=Preference.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			Preference.Update(prefName,FormR.MessageVal);
			//Preference.Refresh();//above line handles it.
			FillGrid();
		}

		#endregion Confirmations
		//===============================================================================================


		private void butSetup_Click(object sender,EventArgs e) {
			//FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.eConfirmRemind);
			//FormESS.Show();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(comboStatusEmailedConfirm.SelectedIndex==-1) {
				Preference.Update(PreferenceName.ConfirmStatusEmailed,0);
			}
			else {
				Preference.Update(PreferenceName.ConfirmStatusEmailed,_listApptConfirmedDefs[comboStatusEmailedConfirm.SelectedIndex].Id);
			}
			if(comboStatusTextMessagedConfirm.SelectedIndex==-1) {
				Preference.Update(PreferenceName.ConfirmStatusTextMessaged,0);
			}
			else {
				Preference.Update(PreferenceName.ConfirmStatusTextMessaged,_listApptConfirmedDefs[comboStatusTextMessagedConfirm.SelectedIndex].Id);
			}
			//If we want to take the time to check every Update and see if something changed 
			//then we could move this to a FormClosing event later.
			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}