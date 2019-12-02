using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;


namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizOperatory:SetupWizardControl {
		private List<Operatory> _listOps = Operatory.All().ToList();
		private int _blink;
		public UserControlSetupWizOperatory() {
			InitializeComponent();
		}

		private void UserControlSetupWizOperatory_Load(object sender,EventArgs e) {
			FillGrid();
			if(Operatory.All(true).Count()==0) {
				MsgBox.Show("FormSetupWizard","You have no valid operatories. Please click the Add button to add an operatory.");
				timer1.Start();
			}
		}

		private void FillGrid() {
			Color needsAttnCol = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;

				col = new ODGridColumn(Lan.g("FormSetupWizard","OpName"),110);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","Abbrev"),110);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","Clinic"),110);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","ProvDentist"),110);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","ProvHygienist"),110);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","IsHygiene"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col = new ODGridColumn(Lan.g("FormSetupWizard","IsHidden"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);

			//col = new ODGridColumn("Clinic",120);
			//gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			bool IsAllComplete = true;
			if(_listOps.Count == 0) {
				IsAllComplete=false;
			}
			foreach(Operatory opCur in _listOps) {
				row = new ODGridRow();
				row.Cells.Add(opCur.Description);
				if(string.IsNullOrEmpty(opCur.Description)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(opCur.Abbr);
				if(string.IsNullOrEmpty(opCur.Abbr)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					IsAllComplete=false;
				}

					row.Cells.Add(Clinic.GetById(opCur.ClinicId).Abbr);
				
				//not a required field
				row.Cells.Add(Providers.GetAbbr(opCur.ProvDentistId.GetValueOrDefault()));
				//not a required field
				row.Cells.Add(Providers.GetAbbr(opCur.ProvHygienistId.GetValueOrDefault()));
				//not a required field
				row.Cells.Add(opCur.IsHygiene ? "X" : "");
				//not a required field
				row.Cells.Add(opCur.IsHidden ? "X" : "");
				//not a required field
				//row = new ODGridRow();
				//row.Cells.Add(opCur.OpName);
				//if(string.IsNullOrEmpty(opCur.OpName)) {
				//	row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
				//	IsAllComplete=false;
				//}
				row.Tag=opCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(IsAllComplete) {
				IsDone=true;
			}
			else {
				IsDone=false;
			}
		}

		private void timer1_Tick(object sender,EventArgs e) {
			if(_blink > 5) {
				pictureAdd.Visible=true;
				timer1.Stop();
				return;
			}
			pictureAdd.Visible=!pictureAdd.Visible;
			_blink++;
		}

        private void butAdd_Click(object sender, EventArgs e)
        {
            var operatory = new Operatory();

            using (var formOperatoryEdit = new FormOperatoryEdit(operatory))
            {
                if (formOperatoryEdit.ShowDialog() == DialogResult.OK)
                {
                    _listOps.Add(operatory);

                    FillGrid();

                    CacheManager.Invalidate<Operatory>();
                }
            }
        }

        private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            Operatory opCur = (Operatory)gridMain.Rows[e.Row].Tag;
            FormOperatoryEdit FormOE = new FormOperatoryEdit(opCur);

            FormOE.ShowDialog();
            if (FormOE.DialogResult == DialogResult.OK)
            {
                FillGrid();

                DataValid.SetInvalid(InvalidType.Operatories);
            }
        }

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormOperatories().ShowDialog();
			FillGrid();
		}
	}
}
