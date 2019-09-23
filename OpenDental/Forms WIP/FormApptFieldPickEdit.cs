using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptFieldPickEdit:ODForm {
		public bool IsNew;
		private AppointmentField Field;

		public FormApptFieldPickEdit(AppointmentField field) {
			InitializeComponent();
			
			Field=field;
		}

		private void FormApptFieldPickEdit_Load(object sender,EventArgs e) {
			labelName.Text=Field.FieldName;
			string value="";
			value=ApptFieldDefs.GetPickListByFieldName(Field.FieldName);
			string[] valueArray=value.Split(new string[] { "\r\n" },StringSplitOptions.None);
			foreach(string s in valueArray) {
				listBoxPick.Items.Add(s);
			}
			if(!IsNew) {
				listBoxPick.SelectedItem=Field.Value;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listBoxPick.SelectedItems.Count==0) {
				MsgBox.Show(this,"Please select an item in the list first.");
				return;
			}
			Field.Value=listBoxPick.SelectedItem.ToString();
			if(Field.Value=="") {//If blank, then delete
				if(IsNew) {
					DialogResult=DialogResult.Cancel;
					return;
				}
				ApptFields.Delete(Field.Id);
				DialogResult=DialogResult.OK;
				return;
			}
			if(IsNew) {
				ApptFields.Insert(Field);
			}
			else {
				ApptFields.Update(Field);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}