using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormProvAdditional:ODForm {
		/// <summary>This is a list of providerclinic rows that were given to this form, containing any modifications that were made while in FormProvAdditional.</summary>
		public List<ProviderClinic> ListProviderClinicOut=new List<ProviderClinic>();
		private Provider _provCur;
		private List<ProviderClinic> _listProvClinic;

		public FormProvAdditional(List<ProviderClinic> listProvClinic,Provider provCur) {
			InitializeComponent();
			
			_listProvClinic=listProvClinic.Select(x => x.Copy()).ToList();
			_provCur=provCur;
		}

		private void FormProvAdditional_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridProvProperties.BeginUpdate();
			gridProvProperties.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProviderProperties","Clinic"),120);
			gridProvProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderProperties","DEA Num"),120,isEditable: true);
			gridProvProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderProperties","State License Num"),120, isEditable: true);
			gridProvProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderProperties","State Rx ID"),120, isEditable: true);
			gridProvProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderProperties","State Where Licensed"),120, isEditable: true);
			gridProvProperties.Columns.Add(col);
			gridProvProperties.Rows.Clear();
			ODGridRow row;
			ProviderClinic provClinicDefault=_listProvClinic.Find(x => x.ClinicNum==0);
			//Didn't have an HQ row
			if(provClinicDefault==null) {//Doesn't exist in list
				provClinicDefault=ProviderClinics.GetOne(_provCur.ProvNum,0);
				if(provClinicDefault==null) {//Doesn't exist in database
					provClinicDefault=new ProviderClinic {
						ProvNum=_provCur.ProvNum,
						ClinicNum=0,
						DEANum=_provCur.DEANum,
						StateLicense=_provCur.StateLicense,
						StateRxID=_provCur.StateRxID,
						StateWhereLicensed=_provCur.StateWhereLicensed,
					};
				}
				_listProvClinic.Add(provClinicDefault);//If not in list, add to list.
			}
			row=new ODGridRow();
			row.Cells.Add("Default");
			row.Cells.Add(provClinicDefault.DEANum);
			row.Cells.Add(provClinicDefault.StateLicense);
			row.Cells.Add(provClinicDefault.StateRxID);
			row.Cells.Add(provClinicDefault.StateWhereLicensed);
			row.Tag=provClinicDefault;
			gridProvProperties.Rows.Add(row);
			if(Preferences.HasClinicsEnabled) {
				foreach(Clinic clinicCur in Clinics.GetForUserod(Security.CurrentUser)) {
					row=new ODGridRow();
					ProviderClinic provClinic=_listProvClinic.Find(x => x.ClinicNum == clinicCur.ClinicNum);
					//Doesn't exist in Db, create a new one
					if(provClinic==null) {
						provClinic=ProviderClinics.GetOne(_provCur.ProvNum,clinicCur.ClinicNum);
						if(provClinic==null) {
							provClinic=new ProviderClinic {
								ProvNum=_provCur.ProvNum,
								ClinicNum=clinicCur.ClinicNum,
								DEANum=_provCur.DEANum,
								StateLicense=_provCur.StateLicense,
								StateRxID=_provCur.StateRxID,
								StateWhereLicensed=_provCur.StateWhereLicensed,
							};
						}
						_listProvClinic.Add(provClinic);
					}
					row.Cells.Add(clinicCur.Abbr);
					row.Cells.Add(provClinic.DEANum);
					row.Cells.Add(provClinic.StateLicense);
					row.Cells.Add(provClinic.StateRxID);
					row.Cells.Add(provClinic.StateWhereLicensed);
					row.Tag=provClinic;
					gridProvProperties.Rows.Add(row);
				}
			}
			gridProvProperties.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void gridProvProperties_CellLeave(object sender,ODGridClickEventArgs e) {
			ODGridRow selectedRow=gridProvProperties.SelectedGridRows.First();
			if(selectedRow==null) {
				return;
			}
			ProviderClinic provClin=(ProviderClinic)selectedRow.Tag;
			string strNewValue=PIn.String(selectedRow.Cells[e.Column].Text);
			if(e.Column==1) {
				provClin.DEANum=strNewValue;
			}
			else if(e.Column==2) {
				provClin.StateLicense=strNewValue;
			}
			else if(e.Column==3) {
				provClin.StateRxID=strNewValue;
			}
			else if(e.Column==4) {
				provClin.StateWhereLicensed=strNewValue;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			ListProviderClinicOut=new List<ProviderClinic>();
			foreach(ODGridRow row in gridProvProperties.Rows) {
				ListProviderClinicOut.Add((ProviderClinic)row.Tag);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}