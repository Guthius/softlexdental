using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEhrEduResourcesPat : FormBase
    {
        List<EduResource> eduResourceList = new List<EduResource>();
        List<EhrMeasureEvent> eduMeasureEventList = new List<EhrMeasureEvent>();

        public Patient patCur;

        public FormEhrEduResourcesPat() => InitializeComponent();

        void FormEhrEduResourcesPat_Load(object sender, EventArgs e)
        {
            FillGridEdu();
            FillGridProvided();
        }

        void FillGridEdu()
        {
            eduResourceList = EduResources.GenerateForPatient(patCur.PatNum);

            resourcesGrid.BeginUpdate();
            resourcesGrid.Columns.Clear();
            resourcesGrid.Columns.Add(new ODGridColumn("Criteria", 300));
            resourcesGrid.Columns.Add(new ODGridColumn("Link", 100));

            resourcesGrid.Rows.Clear();
            foreach (var eduResource in eduResourceList)
            {
                var row = new ODGridRow();
                if (eduResource.DiseaseDefNum != 0)
                {
                    row.Cells.Add("Problem: " + DiseaseDef.GetById(eduResource.DiseaseDefNum).Name);
                    //row.Cells.Add("ICD9: "+DiseaseDefs.GetItem(eduResCur.DiseaseDefNum).ICD9Code);
                }
                else if (eduResource.MedicationNum != 0)
                {
                    row.Cells.Add("Medication: " + Medication.GetDescription(eduResource.MedicationNum));
                }
                else if (eduResource.SmokingSnoMed != "")
                {
                    Snomed sCur = Snomeds.GetByCode(eduResource.SmokingSnoMed);
                    string criteriaCur = "Tobacco Use Assessment: ";
                    if (sCur != null)
                    {
                        criteriaCur += sCur.Description;
                    }
                    row.Cells.Add(criteriaCur);
                }
                else
                {
                    row.Cells.Add("Lab Results: " + eduResource.LabResultName);
                }
                row.Cells.Add(eduResource.ResourceUrl);
                resourcesGrid.Rows.Add(row);
            }
            resourcesGrid.EndUpdate();
        }


        void FillGridProvided()
        {
            eduMeasureEventList = EhrMeasureEvents.RefreshByType(patCur.PatNum, EhrMeasureEventType.EducationProvided);

            providedGrid.BeginUpdate();
            providedGrid.Columns.Clear();
            providedGrid.Columns.Add(new ODGridColumn("DateTime", 140));
            providedGrid.Columns.Add(new ODGridColumn("Details", 600));

            providedGrid.Rows.Clear();
            for (int i = 0; i < eduMeasureEventList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(eduMeasureEventList[i].DateTEvent.ToString());
                row.Cells.Add(eduMeasureEventList[i].MoreInfo);

                providedGrid.Rows.Add(row);
            }

            providedGrid.EndUpdate();
        }

        void ResourcesGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (e.Col != 1) return;

            bool didPrint;
            try
            {
                using (var formEhrEduBrowser = new FormEhrEduBrowser(eduResourceList[e.Row].ResourceUrl))
                {
                    formEhrEduBrowser.ShowDialog();

                    didPrint = formEhrEduBrowser.DidPrint;
                }
            }
            catch
            {
                MessageBox.Show(
                    "Link not found.",
                    "Educational Resource", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (didPrint)
            {
                EhrMeasureEvents.Insert(new EhrMeasureEvent
                {
                    DateTEvent = DateTime.Now,
                    EventType = EhrMeasureEventType.EducationProvided,
                    PatNum = patCur.PatNum,
                    MoreInfo = eduResourceList[e.Row].ResourceUrl
                });

                FillGridProvided();
            }
        }

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (providedGrid.SelectedIndices.Length < 1)
            {
                MessageBox.Show(
                    "Please select at least one record to delete.", 
                    "", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            for (int i = 0; i < providedGrid.SelectedIndices.Length; i++)
            {
                EhrMeasureEvents.Delete(eduMeasureEventList[providedGrid.SelectedIndices[i]].EhrMeasureEventNum);
            }

            FillGridProvided();
        }
    }
}