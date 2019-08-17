using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormDrugManufacturerSetup:ODForm {
		private List<Manufacturer> _listDrugManufacturers;

		public FormDrugManufacturerSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDrugManufacturerSetup_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
            CacheManager.Invalidate<Manufacturer>();
			_listDrugManufacturers=Manufacturer.All();
			listMain.Items.Clear();
			for(int i=0;i<_listDrugManufacturers.Count;i++) {
				listMain.Items.Add(_listDrugManufacturers[i].Code + " - " + _listDrugManufacturers[i].Name);
			}
		}

		private void listMain_DoubleClick(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				return;
			}
			FormDrugManufacturerEdit FormD=new FormDrugManufacturerEdit();
			FormD.DrugManufacturerCur=_listDrugManufacturers[listMain.SelectedIndex];
			FormD.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormDrugManufacturerEdit FormD=new FormDrugManufacturerEdit();
			FormD.DrugManufacturerCur=new Manufacturer();
			FormD.IsNew=true;
			FormD.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}