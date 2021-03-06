using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	public partial class FormCanadaPaymentReconciliation:ODForm {

		List<Carrier> carriers=new List<Carrier>();
		private List<Provider> _listProviders;

		public FormCanadaPaymentReconciliation() {
			InitializeComponent();
			
		}

		private void FormCanadaPaymentReconciliation_Load(object sender,EventArgs e) {
			carriers=Carriers.GetWhere(x => x.CDAnetVersion!="02" &&//This transaction does not exist in version 02.
				(x.CanadianSupportedTypes & CanSupTransTypes.RequestForPaymentReconciliation_06)==CanSupTransTypes.RequestForPaymentReconciliation_06);
			foreach(Carrier carrier in carriers) {
				listCarriers.Items.Add(carrier.CarrierName);
			}
			long defaultProvNum=Preference.GetLong(PreferenceName.PracticeDefaultProv);
			_listProviders=Provider.All().ToList();
			for(int i=0;i<_listProviders.Count;i++) {
				if(_listProviders[i].IsCDAnet) {
					listBillingProvider.Items.Add(_listProviders[i].Abbr);
					listTreatingProvider.Items.Add(_listProviders[i].Abbr);
					if(_listProviders[i].Id==defaultProvNum) {
						listBillingProvider.SelectedIndex=i;
						textBillingOfficeNumber.Text=_listProviders[i].CanadianOfficeNumber;
						listTreatingProvider.SelectedIndex=i;
						textTreatingOfficeNumber.Text=_listProviders[i].CanadianOfficeNumber;
					}
				}
			}
			textDateReconciliation.Text=DateTime.Today.ToShortDateString();
		}

		private void listBillingProvider_Click(object sender,EventArgs e) {
			textBillingOfficeNumber.Text=_listProviders[listBillingProvider.SelectedIndex].CanadianOfficeNumber;
		}

		private void listTreatingProvider_Click(object sender,EventArgs e) {
			textTreatingOfficeNumber.Text=_listProviders[listTreatingProvider.SelectedIndex].CanadianOfficeNumber;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listCarriers.SelectedIndex<0) {
				MsgBox.Show(this,"You must first choose a carrier.");
				return;
			}
			if(listBillingProvider.SelectedIndex<0) {
				MsgBox.Show(this,"You must first choose a billing provider.");
				return;
			}
			if(listTreatingProvider.SelectedIndex<0) {
				MsgBox.Show(this,"You must first choose a treating provider.");
				return;
			}
			DateTime reconciliationDate;
			try {
				reconciliationDate=DateTime.Parse(textDateReconciliation.Text).Date;
			}
			catch {
				MsgBox.Show(this,"Reconciliation date invalid.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			try {
				Carrier carrier=carriers[listCarriers.SelectedIndex];
				Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(carrier);
				Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicId); 
				CanadianOutput.GetPaymentReconciliations(clearinghouseClin,carrier,_listProviders[listTreatingProvider.SelectedIndex],
					_listProviders[listBillingProvider.SelectedIndex],reconciliationDate,Clinics.ClinicId,false,FormCCDPrint.PrintCCD);
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Done.");
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Request failed: ")+ex.Message);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}