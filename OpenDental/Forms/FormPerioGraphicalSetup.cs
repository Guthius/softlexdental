using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPerioGraphicalSetup:ODForm {

		public FormPerioGraphicalSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPerioGraphicalSetup_Load(object sender,EventArgs e) {
			this.butColorCal.BackColor=Preference.GetColor(PreferenceName.PerioColorCAL);
			this.butColorFurc.BackColor=Preference.GetColor(PreferenceName.PerioColorFurcations);
			this.butColorFurcRed.BackColor=Preference.GetColor(PreferenceName.PerioColorFurcationsRed);
			this.butColorGM.BackColor=Preference.GetColor(PreferenceName.PerioColorGM);
			this.butColorMGJ.BackColor=Preference.GetColor(PreferenceName.PerioColorMGJ);	
			this.butColorProbing.BackColor=Preference.GetColor(PreferenceName.PerioColorProbing);
			this.butColorProbingRed.BackColor=Preference.GetColor(PreferenceName.PerioColorProbingRed);
		}

		private void butColorCal_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorCal.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK) {
				this.butColorCal.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorFurc_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorFurc.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK) {
				this.butColorFurc.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorFurcRed_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorFurcRed.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK) {
				this.butColorFurcRed.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorGM_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorGM.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK){
				this.butColorGM.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorMGJ_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorMGJ.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK){
				this.butColorMGJ.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorProbing_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorProbing.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK){
				this.butColorProbing.BackColor=this.colorPicker.Color;
			}
		}

		private void butColorProbingRed_Click(object sender,EventArgs e) {
			this.colorPicker.Color=this.butColorProbingRed.BackColor;
			if(this.colorPicker.ShowDialog()==DialogResult.OK){
				this.butColorProbingRed.BackColor=this.colorPicker.Color;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			Preference.Update(PreferenceName.PerioColorCAL,this.butColorCal.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorFurcations,this.butColorFurc.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorFurcationsRed,this.butColorFurcRed.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorGM,this.butColorGM.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorMGJ,this.butColorMGJ.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorProbing,this.butColorProbing.BackColor.ToArgb());
			Preference.Update(PreferenceName.PerioColorProbingRed,this.butColorProbingRed.BackColor.ToArgb());
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}