using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRxAlertEdit:ODForm {
		private RxAlert RxAlertCur;
		private RxDef RxDefCur;

		public FormRxAlertEdit(RxAlert rxAlertCur,RxDef rxDefCur) {
			InitializeComponent();
			
			RxAlertCur=rxAlertCur;
			RxDefCur=rxDefCur;
		}

		private void FormRxAlertEdit_Load(object sender,EventArgs e) {
			textRxName.Text=RxDefCur.Drug;
			if(RxAlertCur.DiseaseDefNum>0) {
				labelName.Text=Lan.g(this,"If the patient already has this Problem");
				textName.Text=DiseaseDef.GetName(RxAlertCur.DiseaseDefNum);
			}
			if(RxAlertCur.AllergyDefNum>0) {
				labelName.Text=Lan.g(this,"If the patient already has this Allergy");
				textName.Text= Allergy.GetById(RxAlertCur.AllergyDefNum).Description;
			}
			if(RxAlertCur.MedicationNum>0) {
				labelName.Text=Lan.g(this,"If the patient is already taking this medication");
				textName.Text=Medication.GetById(RxAlertCur.MedicationNum).Description;
			}
			textMessage.Text=RxAlertCur.NotificationMsg;
			checkIsHighSignificance.Checked=RxAlertCur.IsHighSignificance;
		}

		private void butOK_Click(object sender,EventArgs e) {
			RxAlertCur.NotificationMsg=PIn.String(textMessage.Text);
			RxAlertCur.IsHighSignificance=checkIsHighSignificance.Checked;
			RxAlerts.Update(RxAlertCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			RxAlerts.Delete(RxAlertCur);
			DialogResult=DialogResult.OK;
		}
	}
}