using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormOrthoSetup:ODForm {
		private bool _hasChanges;
		///<summary>Set to the OrthoAutoProcCodeNum Pref value on load.  Can be changed by the user via this form.</summary>
		private long _orthoAutoProcCodeNum;
		///<summary>Filled upon load.</summary>
		private List<long> _listOrthoPlacementCodeNums= new List<long>();

		public FormOrthoSetup() {
			InitializeComponent();
			
		}

		private void FormOrthoSetup_Load(object sender,EventArgs e) {
			checkPatClone.Checked=Preference.GetBool(PreferenceName.ShowFeaturePatientClone);
			checkApptModuleShowOrthoChartItem.Checked=Preference.GetBool(PreferenceName.ApptModuleShowOrthoChartItem);
			checkOrthoEnabled.Checked=Preference.GetBool(PreferenceName.OrthoEnabled);
			checkOrthoFinancialInfoInChart.Checked=Preference.GetBool(PreferenceName.OrthoCaseInfoInOrthoChart);
			checkOrthoClaimMarkAsOrtho.Checked=Preference.GetBool(PreferenceName.OrthoClaimMarkAsOrtho);
			checkOrthoClaimUseDatePlacement.Checked=Preference.GetBool(PreferenceName.OrthoClaimUseDatePlacement);
			textOrthoMonthsTreat.Text=Preference.GetByte(PreferenceName.OrthoDefaultMonthsTreat).ToString();
			_orthoAutoProcCodeNum=Preference.GetLong(PreferenceName.OrthoAutoProcCodeNum);
			textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			checkConsolidateInsPayment.Checked=Preference.GetBool(PreferenceName.OrthoInsPayConsolidated);
			string strListOrthoNums = Preference.GetString(PreferenceName.OrthoPlacementProcsList);
			if(strListOrthoNums!="") {
				_listOrthoPlacementCodeNums.AddRange(strListOrthoNums.Split(new char[] { ',' }).ToList().Select(x => PIn.Long(x)));
			}
			RefreshListBoxProcs();
		}

		private void RefreshListBoxProcs() {
			listboxOrthoPlacementProcs.Items.Clear();
			foreach(long orthoProcCodeNum in _listOrthoPlacementCodeNums) {
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(orthoProcCodeNum);
				ODBoxItem<ProcedureCode> listBoxItem = new ODBoxItem<ProcedureCode>(procCodeCur.ProcCode,procCodeCur);
				listboxOrthoPlacementProcs.Items.Add(listBoxItem);
			}
		}

		private void butOrthoDisplayFields_Click(object sender,EventArgs e) {
			FormDisplayFieldsOrthoChart FormDFOC = new FormDisplayFieldsOrthoChart();
			FormDFOC.ShowDialog();
		}

		private void butPickOrthoProc_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode=true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_orthoAutoProcCodeNum=FormPC.SelectedCodeNum;
				textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			}
		}

		private void butPlacementProcsEdit_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode = true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_listOrthoPlacementCodeNums.Add(FormPC.SelectedCodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(listboxOrthoPlacementProcs.SelectedIndices.Count == 0) {
				MsgBox.Show(this,"Select an item to delete.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to delete the selected items?")) {
				return;
			}
			foreach(ODBoxItem<ProcedureCode> boxItem in listboxOrthoPlacementProcs.SelectedItems) {
				_listOrthoPlacementCodeNums.Remove(boxItem.Tag.CodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textOrthoMonthsTreat.errorProvider1.GetError(textOrthoMonthsTreat)!="") {
				MsgBox.Show(this,"Default months treatment must be between 0 and 255 months.");
				return;
			}
			if(Preference.GetBool(PreferenceName.ShowFeaturePatientClone)!=checkPatClone.Checked) {
				MsgBox.Show(this,"You will need to restart OpenDental for this change to take effect.");
			}
			if(Preference.Update(PreferenceName.ShowFeaturePatientClone,checkPatClone.Checked)
			| Preference.Update(PreferenceName.ApptModuleShowOrthoChartItem,checkApptModuleShowOrthoChartItem.Checked)
			| Preference.Update(PreferenceName.OrthoEnabled,checkOrthoEnabled.Checked)
			| Preference.Update(PreferenceName.OrthoCaseInfoInOrthoChart,checkOrthoFinancialInfoInChart.Checked)
			| Preference.Update(PreferenceName.OrthoClaimMarkAsOrtho,checkOrthoClaimMarkAsOrtho.Checked)
			| Preference.Update(PreferenceName.OrthoClaimUseDatePlacement,checkOrthoClaimUseDatePlacement.Checked)
			| Preference.Update(PreferenceName.OrthoDefaultMonthsTreat,PIn.Byte(textOrthoMonthsTreat.Text))
			| Preference.Update(PreferenceName.OrthoInsPayConsolidated,checkConsolidateInsPayment.Checked)
			| Preference.Update(PreferenceName.OrthoAutoProcCodeNum,_orthoAutoProcCodeNum)
			| Preference.Update(PreferenceName.OrthoPlacementProcsList,string.Join(",",_listOrthoPlacementCodeNums))
			) {
				_hasChanges=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			_hasChanges=false;
			DialogResult=DialogResult.Cancel;
		}

		private void FormOrthoSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanges) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}