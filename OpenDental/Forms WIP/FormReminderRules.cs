﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormReminderRules:ODForm {
		public List<ReminderRule> listReminders;
		public FormReminderRules() {
			InitializeComponent();
			
		}

		private void FormReminderRules_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Reminder Criterion",200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Message",200);
			gridMain.Columns.Add(col);
			listReminders=ReminderRules.SelectAll();
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listReminders.Count;i++) {
				row=new ODGridRow();
				switch(listReminders[i].ReminderCriterion) {
					case EhrCriterion.Problem:
						DiseaseDef def=DiseaseDef.GetById(listReminders[i].CriterionFK);
						row.Cells.Add("Problem ="+def.ICD9Code+" "+def.Name);
						break;
					case EhrCriterion.Medication:
						Medication tempMed = Medication.GetById(listReminders[i].CriterionFK);
						if(!tempMed.GenericId.HasValue) {//handle generic medication names.
							row.Cells.Add("Medication = "+tempMed.Description);
						}
						else {
							row.Cells.Add("Medication = "+tempMed.Description+" / "+Medication.GetGenericName(tempMed.GenericId.Value));
						}
						break;
					case EhrCriterion.Allergy:
						row.Cells.Add("Allergy = "+ Allergy.GetById(listReminders[i].CriterionFK).Description);
						break;
					case EhrCriterion.Age:
						row.Cells.Add("Age "+listReminders[i].CriterionValue);
						break;
					case EhrCriterion.Gender:
						row.Cells.Add("Gender is "+listReminders[i].CriterionValue);
						break;
					case EhrCriterion.LabResult:
						row.Cells.Add("LabResult "+listReminders[i].CriterionValue);
						break;
					//case EhrCriterion.ICD9:
					//  row.Cells.Add("ICD9 "+ICD9s.GetDescription(listReminders[i].CriterionFK));
					//  break;
				}
				row.Cells.Add(listReminders[i].Message);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormReminderRuleEdit FormRRE=new FormReminderRuleEdit();
			FormRRE.RuleCur = listReminders[e.Row];
			FormRRE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormReminderRuleEdit FormRRE=new FormReminderRuleEdit();
			FormRRE.IsNew=true;
			FormRRE.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		
	}
}
