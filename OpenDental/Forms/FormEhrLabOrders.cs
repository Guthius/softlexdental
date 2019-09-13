using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Globalization;

namespace OpenDental
{
    public partial class FormEhrLabOrders : FormBase
    {
        public List<EhrLab> erhLabList;

        public Patient PatCur;

        public FormEhrLabOrders() => InitializeComponent();

        void FormEhrLabOrders_Load(object sender, EventArgs e) => LoadOrders();

        void LoadOrders()
        {
            ordersGrid.BeginUpdate();
            ordersGrid.Columns.Clear();
            ordersGrid.Columns.Add(new ODGridColumn("Date Time", 80, HorizontalAlignment.Center, ODGridSortingStrategy.DateParse));
            ordersGrid.Columns.Add(new ODGridColumn("Placer Order Number", 130, HorizontalAlignment.Center));
            ordersGrid.Columns.Add(new ODGridColumn("Filler Order Number", 130, HorizontalAlignment.Center));
            ordersGrid.Columns.Add(new ODGridColumn("Test Performed", 430));
            ordersGrid.Columns.Add(new ODGridColumn("Results In", 80, HorizontalAlignment.Center));
            ordersGrid.Rows.Clear();

            erhLabList = EhrLabs.GetAllForPat(PatCur.PatNum);
            foreach (var erhLab in erhLabList)
            {
                if (DateTime.TryParseExact(erhLab.ResultDateTime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                {
                    var row = new ODGridRow();
                    row.Cells.Add(dateTime.ToShortDateString());
                    row.Cells.Add(erhLab.PlacerOrderNum);
                    row.Cells.Add(erhLab.FillerOrderNum);
                    row.Cells.Add(erhLab.UsiText);
                    row.Cells.Add(erhLab.ListEhrLabResults.Count.ToString());
                    ordersGrid.Rows.Add(row);
                }
            }


            ordersGrid.EndUpdate();
        }

        void OrdersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formEhrLabOrderEdit = new FormEhrLabOrderEdit2014())
            {
                formEhrLabOrderEdit.EhrLabCur = erhLabList[e.Row];
                if (formEhrLabOrderEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadOrders();
                }
            }
        }

        void ImportButton_Click(object sender, EventArgs e)
        {
            string message = string.Empty;

            using (var msgBoxCopyPaste = new MsgBoxCopyPaste("Paste HL7 Lab Message Text Here."))
            {
                if (msgBoxCopyPaste.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                message = msgBoxCopyPaste.Content;
            }

            List<EhrLab> erhLabList;
            try
            {
                erhLabList = EhrLabs.ProcessHl7Message(message);

                if (erhLabList[0].PatNum == PatCur.PatNum)
                {
                    // Only need to check the first lab, nothing to do here. 
                    // Imported lab matches the current patient.
                }
                else // Does not match current patient, redirect to import form which displays patient information and is build for importing.
                {
                    using (var formEhrLabOrderImport = new FormEhrLabOrderImport())
                    {
                        formEhrLabOrderImport.PatCur = PatCur;
                        formEhrLabOrderImport.Hl7LabMessage = message;
                        formEhrLabOrderImport.ShowDialog();

                        LoadOrders();

                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to import lab.\r\n" + exception.Message, 
                    "Lab Order", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            foreach (var erhLab in erhLabList)
            {
                var erhLabTemp = EhrLabs.GetByGUID(erhLab.PlacerOrderUniversalID, erhLab.PlacerOrderNum);
                if (erhLabTemp == null)
                {
                    erhLabTemp = EhrLabs.GetByGUID(erhLab.FillerOrderUniversalID, erhLab.FillerOrderNum);
                }

                if (erhLabTemp != null)
                {
                    //Date validation.
                    //if(tempLab.ResultDateTime.CompareTo(listEhrLabs[i].ResultDateTime)<=0) {//string compare dates will return 1+ if tempLab Date is greater.
                    //	MsgBox.Show(this,"This lab already exists in the database and has a more recent timestamp.");
                    //	continue;
                    //}
                    //TODO: The code above works, but ignores more recent lab results. Although the lab order my be unchanged there may be updated lab results.
                    //It would be better to check for updated results, unfortunately results have no unique identifiers.
                }

                var provider = Providers.GetProv(Security.CurrentUser.ProviderId.GetValueOrDefault());
                if (Security.CurrentUser.ProviderId != 0 && EhrProvKeys.GetKeysByFLName(provider.LName, provider.FName).Count > 0)
                {
                    // The user who is currently logged in is a provider and has a valid EHR key.
                    erhLab.IsCpoe = true;
                }

                erhLabTemp = EhrLabs.SaveToDB(erhLab);//SAVE

                foreach (var erhLabResult in erhLabTemp.ListEhrLabResults) //EHR TRIGGER
                {
                    if (CDSPermissions.GetForUser(Security.CurrentUser.Id).ShowCDS && CDSPermissions.GetForUser(Security.CurrentUser.Id).LabTestCDS)
                    {
                        using (var formCDSIntervention = new FormCDSIntervention())
                        {
                            formCDSIntervention.ListCDSI = EhrTriggers.TriggerMatch(erhLabResult, PatCur);
                            formCDSIntervention.ShowIfRequired(false);
                        }
                    }
                }
            }
            LoadOrders();
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formEhrLabOrderEdit2014 = new FormEhrLabOrderEdit2014())
            {
                formEhrLabOrderEdit2014.EhrLabCur.PatNum = PatCur.PatNum;
                formEhrLabOrderEdit2014.IsNew = true;
                formEhrLabOrderEdit2014.ShowDialog();
                if (formEhrLabOrderEdit2014.DialogResult != DialogResult.OK)
                {
                    return;
                }

                var newMeasureEvent = new EhrMeasureEvent
                {
                    DateTEvent = DateTime.Now,
                    EventType = EhrMeasureEventType.CPOE_LabOrdered, //default
                    PatNum = formEhrLabOrderEdit2014.EhrLabCur.PatNum,
                    MoreInfo = "",
                    FKey = formEhrLabOrderEdit2014.EhrLabCur.EhrLabNum
                };

                var loinc = Loincs.GetByCode(formEhrLabOrderEdit2014.EhrLabCur.UsiID);
                if (loinc != null && loinc.ClassType == "RAD")
                {
                    newMeasureEvent.EventType = EhrMeasureEventType.CPOE_RadOrdered;
                }

                EhrMeasureEvents.Insert(newMeasureEvent);
                EhrLabs.SaveToDB(formEhrLabOrderEdit2014.EhrLabCur);

                foreach (var erhLabResult in formEhrLabOrderEdit2014.EhrLabCur.ListEhrLabResults)
                {
                    if (CDSPermissions.GetForUser(Security.CurrentUser.Id).ShowCDS && CDSPermissions.GetForUser(Security.CurrentUser.Id).LabTestCDS)
                    {
                        using (var formCDSIntervention = new FormCDSIntervention())
                        {
                            formCDSIntervention.ListCDSI = EhrTriggers.TriggerMatch(erhLabResult, PatCur);
                            formCDSIntervention.ShowIfRequired(false);
                        }
                    }
                }

                LoadOrders();
            }
        }
    }
}