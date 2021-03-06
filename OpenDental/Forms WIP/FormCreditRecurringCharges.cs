using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormCreditRecurringCharges : ODForm
    {
        private int pagesPrinted;
        private int headingPrintH;
        private bool headingPrinted;
        ///<summary>List of clinics for which the current user is authorized.  If user is not restricted, list will also contain a dummy clinic with
        ///ClinicNum=0 for HQ.  List will be empty if the current user is not allowed to access any clinics or clinics are not enabled.</summary>
        private List<Clinic> _listUserClinics;
        ///<summary>True if we are programmatically selecting indexes in listClinics.  This allows us to use the SelectedIndexChanged event handler and
        ///disable the event when we are setting the selected indexes programmatically.</summary>
        private bool _isSelecting;
        ///<summary>The object that charges the recurring charges.</summary>
        RecurringChargerator _charger;

        ///<summary>Only works for XCharge,PayConnect, and PaySimple so far.</summary>
        public FormCreditRecurringCharges()
        {
            InitializeComponent();
        }

        private void FormRecurringCharges_Load(object sender, EventArgs e)
        {
            // TODO: Implement me

            //if (Programs.HasMultipleCreditCardProgramsEnabled())
            //{
            //    gridMain.HScrollVisible = true;
            //}
            ////if(!Preferences.IsODHQ) {
            ////	checkHideBold.Checked=true;
            ////	checkHideBold.Visible=false;
            ////}
            //Program progCur = null;
            //if (Programs.IsEnabled(ProgramName.PaySimple))
            //{
            //    progCur = Programs.GetCur(ProgramName.PaySimple);
            //    labelUpdated.Visible = false;
            //    checkForceDuplicates.Checked = false;
            //    checkForceDuplicates.Visible = false;//PaySimple always rejects identical transactions made within 5 minutes of eachother.
            //}
            ////if(Programs.IsEnabled(ProgramName.PayConnect)) {
            ////	progCur=Programs.GetCur(ProgramName.PayConnect);
            ////	labelUpdated.Visible=false;
            ////	checkForceDuplicates.Visible=true;
            ////	checkForceDuplicates.Checked=PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(progCur.ProgramNum,
            ////		PayConnect.ProgramProperties.PayConnectForceRecurringCharge,Clinics.ClinicNum));
            ////}
            //if (Programs.IsEnabled(ProgramName.Xcharge))
            //{
            //    progCur = Programs.GetCur(ProgramName.Xcharge);
            //    labelUpdated.Visible = true;
            //    checkForceDuplicates.Visible = true;
            //    string xPath = Programs.GetProgramPath(progCur);
            //    checkForceDuplicates.Checked = PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(progCur.ProgramNum,
            //        XCharge.ProgramProperties.XChargeForceRecurringCharge, Clinics.ClinicId));
            //    if (!File.Exists(xPath))
            //    {//program path is invalid
            //     //if user has setup permission and they want to edit the program path, show the X-Charge setup window
            //        if (Security.IsAuthorized(Permissions.Setup)
            //            && MsgBox.Show(this, MsgBoxButtons.YesNo, "The X-Charge path is not valid.  Would you like to edit the path?"))
            //        {
            //            FormXchargeSetup FormX = new FormXchargeSetup();
            //            FormX.ShowDialog();
            //            if (FormX.DialogResult == DialogResult.OK)
            //            {
            //                //The user could have correctly enabled the X-Charge bridge, we need to update our local _programCur and _xPath variable2
            //                progCur = Programs.GetCur(ProgramName.Xcharge);
            //                xPath = Programs.GetProgramPath(progCur);
            //            }
            //        }
            //        //if the program path still does not exist, whether or not they attempted to edit the program link, tell them to edit and close the form
            //        if (!File.Exists(xPath))
            //        {
            //            MsgBox.Show(this, "The X-Charge program path is not valid.  Edit the program link in order to use the CC Recurring Charges feature.");
            //            Close();
            //            return;
            //        }
            //    }
            //}
            //if (progCur == null)
            //{
            //    MsgBox.Show(this, "The PayConnect, PaySimple, or X-Charge program link must be enabled in order to use the CC Recurring Charges feature.");
            //    Close();
            //    return;
            //}
            //_isSelecting = true;
            //_listUserClinics = new List<Clinic>();

            //if (!Security.CurrentUser.ClinicRestricted)
            //{
            //    _listUserClinics.Add(new Clinic() { Description = Lan.g(this, "Unassigned") });
            //}
            //Clinic.GetByUser(Security.CurrentUser).ForEach(x => _listUserClinics.Add(x));
            //for (int i = 0; i < _listUserClinics.Count; i++)
            //{
            //    listClinics.Items.Add(_listUserClinics[i].Description);
            //    listClinics.SetSelected(i, true);
            //}
            ////checkAllClin.Checked=true;//checked true by default in designer so we don't trigger the event to select all and fill grid

            //_charger = new RecurringChargerator(true);
            //_charger.SingleCardFinished = new Action(() =>
            //{
            //    this.Invoke(() =>
            //    {
            //        labelCharged.Text = Lans.g(this, "Charged=") + _charger.Success;
            //        labelFailed.Text = Lans.g(this, "Failed=") + _charger.Failed;
            //        labelUpdated.Text = Lans.g(this, "Updated=") + _charger.Updated;
            //    });
            //});
            //GeneralProgramEvent.Fired += StopRecurringCharges;//This is so we'll be alerted in case of a shutdown.
            //labelCharged.Text = Lan.g(this, "Charged=") + "0";
            //labelFailed.Text = Lan.g(this, "Failed=") + "0";
            //FillGrid(true);
            //gridMain.SetSelected(true);
            //labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
        }

        ///<summary>The DataTable used to fill the grid will only be refreshed from the db if isFromDb is true.  Otherwise the grid will be refilled using
        ///the existing table.  Only get from the db on load or if the Refresh button is pressed, not when the user is selecting the clinic(s).</summary>
        private void FillGrid(bool isFromDb = false)
        {
            Cursor = Cursors.WaitCursor;
            if (isFromDb)
            {
                _charger.FillCharges(_listUserClinics);
            }
            List<long> listSelectedClinicNums = listClinics.SelectedIndices.OfType<int>().Select(x => _listUserClinics[x].Id).ToList();
            List<RecurringChargeData> listChargesCur;

            listChargesCur = _charger.ListRecurringChargeData.Where(x => listSelectedClinicNums.Contains(x.RecurringCharge.ClinicNum)).ToList();

            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "PatNum"), 55));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Name"), 190));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Clinic"), 65));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Date"), 80, textAlignment: HorizontalAlignment.Right));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Family Bal"), 70, textAlignment: HorizontalAlignment.Right));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "PayPlan Due"), 75, textAlignment: HorizontalAlignment.Right));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Total Due"), 65, textAlignment: HorizontalAlignment.Right));
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Repeat Amt"), 75, textAlignment: HorizontalAlignment.Right));//RptChrgAmt
            gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring", "Charge Amt"), 85, textAlignment: HorizontalAlignment.Right));

            // TODO: Fix me
            //if (Programs.HasMultipleCreditCardProgramsEnabled())
            //{
            //    if (Programs.IsEnabled(ProgramName.Xcharge))
            //    {
            //        gridMain.Columns.Add(new ODGridColumn("X-Charge", 70, textAlignment: HorizontalAlignment.Center));
            //    }
            //    if (Programs.IsEnabled(ProgramName.PayConnect))
            //    {
            //        gridMain.Columns.Add(new ODGridColumn("PayConnect", 85, textAlignment: HorizontalAlignment.Center));
            //    }
            //    if (Programs.IsEnabled(ProgramName.PaySimple))
            //    {
            //        gridMain.Columns.Add(new ODGridColumn("PaySimple", 80, textAlignment: HorizontalAlignment.Center));
            //    }
            //}

            gridMain.Rows.Clear();
            ODGridRow row;
            foreach (RecurringChargeData chargeCur in listChargesCur)
            {
                row = new ODGridRow();
                double famBalTotal = chargeCur.RecurringCharge.FamBal;//pat bal+payplan due, but if pat bal<0 and payplan due>0 then just payplan due
                double payPlanDue = chargeCur.RecurringCharge.PayPlanDue;
                double chargeAmt = chargeCur.RecurringCharge.ChargeAmt;
                double rptChargeAmt = chargeCur.RecurringCharge.RepeatAmt;//includes repeat charge (from procs if ODHQ) and attached payplan
                row.Cells.Add(chargeCur.RecurringCharge.PatNum.ToString());
                row.Cells.Add(chargeCur.PatName);

                Clinic clinicCur = _listUserClinics.FirstOrDefault(x => x.Id == chargeCur.RecurringCharge.ClinicNum);
                row.Cells.Add(clinicCur != null ? clinicCur.Description : "");//get description from cache if clinics are enabled

                int billingDay = 0;
                if (Preference.GetBool(PreferenceName.BillingUseBillingCycleDay))
                {
                    billingDay = chargeCur.BillingCycleDay;
                }
                else
                {
                    billingDay = chargeCur.DateStart.Day;
                }
                DateTime startBillingCycle = DateTimeOD.GetMostRecentValidDate(DateTime.Today.Year, DateTime.Today.Month, billingDay);
                if (startBillingCycle > DateTime.Today)
                {
                    startBillingCycle = startBillingCycle.AddMonths(-1);//Won't give a date with incorrect day.  AddMonths will give the end of the month if needed.
                }
                DateTime dateExcludeIfBefore = PIn.Date(textDate.Text);//If entry is invalid, all charges will be included because this will be MinDate.
                if (startBillingCycle < dateExcludeIfBefore)
                {
                    continue;//Don't show row in grid
                }
                row.Cells.Add(startBillingCycle.ToShortDateString());
                row.Cells.Add(famBalTotal.ToString("c"));
                if (payPlanDue != 0)
                {
                    row.Cells.Add(payPlanDue.ToString("c"));
                    //negative family balance does not subtract from payplan amount due and negative payplan amount due does not subtract from family balance due
                    double totalBal = (Math.Max(famBalTotal, 0));
                    if (Preference.GetInt(PreferenceName.PayPlansVersion) == 1)
                    {//in PP v2, the PP amt due is included in the pat balance
                        totalBal += Math.Max(payPlanDue, 0);
                    }
                    else if (Preference.GetInt(PreferenceName.PayPlansVersion) == 2)
                    {
                        totalBal = Math.Max(totalBal, payPlanDue);//At minimum, the Total Due should be the Pay Plan Due amount.
                    }
                    row.Cells.Add(totalBal.ToString("c"));
                }
                else
                {
                    row.Cells.Add("");
                    row.Cells.Add(famBalTotal.ToString("c"));
                }
                row.Cells.Add(rptChargeAmt.ToString("c"));
                row.Cells.Add(chargeAmt.ToString("c"));
                if (!checkHideBold.Checked)
                {
                    double diff = (Math.Max(famBalTotal, 0) + Math.Max(payPlanDue, 0)) - rptChargeAmt;
                    if (diff.IsZero())
                    {
                        //don't bold anything
                    }
                    else if (diff > 0)
                    {
                        row.Cells[6].Bold = true;//"Repeating Amt"
                        row.Cells[7].Bold = true;//"Charge Amt"
                    }
                    else if (diff < 0)
                    {
                        row.Cells[5].Bold = true;//"Total Due"
                        row.Cells[7].Bold = true;//"Charge Amt"
                    }
                }

                // TODO: Fix me
                //if (Programs.HasMultipleCreditCardProgramsEnabled())
                //{
                //    
                //    if (Programs.IsEnabled(ProgramName.Xcharge))
                //    {
                //        row.Cells.Add(!string.IsNullOrEmpty(chargeCur.XChargeToken) ? "X" : "");
                //    }
                //    if (Programs.IsEnabled(ProgramName.PayConnect))
                //    {
                //        row.Cells.Add(!string.IsNullOrEmpty(chargeCur.PayConnectToken) ? "X" : "");
                //    }
                //    if (Programs.IsEnabled(ProgramName.PaySimple))
                //    {
                //        row.Cells.Add(!string.IsNullOrEmpty(chargeCur.PaySimpleToken) ? "X" : "");
                //    }
                //}


                row.Tag = chargeCur;
                gridMain.Rows.Add(row);
            }
            gridMain.EndUpdate();
            labelTotal.Text = Lan.g(this, "Total=") + gridMain.Rows.Count.ToString();
            labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
            Cursor = Cursors.Default;
        }

        private void listClinics_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if the selected indices have not changed, don't refill the grid
            if (_isSelecting)
            {
                return;
            }
            RefreshRecurringCharges(true, false);
            checkAllClin.Checked = (listClinics.SelectedIndices.Count == listClinics.Items.Count);
        }

        private void checkAllClin_Click(object sender, EventArgs e)
        {
            if (!checkAllClin.Checked)
            {
                return;
            }
            _isSelecting = true;
            for (int i = 0; i < listClinics.Items.Count; i++)
            {
                listClinics.SetSelected(i, true);
            }
            _isSelecting = false;
            RefreshRecurringCharges(true, false);
        }

        private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
        {
            labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
        }

        private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ModuleAccount))
            {
                return;
            }
            if (e.Row < 0)
            {
                MsgBox.Show(this, "Must select at least one recurring charge.");
                return;
            }
            long patNum = ((RecurringChargeData)gridMain.Rows[e.Row].Tag).RecurringCharge.PatNum;
            GotoModule.GotoAccount(patNum);
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
            RefreshRecurringCharges(false, true);
        }

        private void RefreshRecurringCharges(bool isSelectAll, bool isFromDb)
        {
            Cursor = Cursors.WaitCursor;
            List<long> listSelectedCCNums = gridMain.SelectedTags<RecurringChargeData>()
                .Select(x => x.RecurringCharge.CreditCardNum).ToList();
            FillGrid(isFromDb);
            labelCharged.Text = Lan.g(this, "Charged=") + "0";
            labelFailed.Text = Lan.g(this, "Failed=") + "0";
            labelUpdated.Text = Lan.g(this, "Updated=") + "0";
            if (isSelectAll)
            {
                gridMain.SetSelected(true);
            }
            else
            {
                for (int i = 0; i < gridMain.Rows.Count; i++)
                {
                    long creditCardNum = ((RecurringChargeData)gridMain.Rows[i].Tag).RecurringCharge.CreditCardNum;
                    if (listSelectedCCNums.Contains(creditCardNum))
                    {
                        gridMain.SetSelected(i, true);
                    }
                }
            }
            labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
            Cursor = Cursors.Default;
        }

        private void textDate_TextChanged(object sender, EventArgs e)
        {
            RefreshRecurringCharges(false, false);
        }

        private void butToday_Click(object sender, EventArgs e)
        {
            textDate.Text = DateTime.Today.ToShortDateString();
        }

        private void checkHideBold_Click(object sender, EventArgs e)
        {
            RefreshRecurringCharges(false, false);
        }

        private void butPrintList_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;
            headingPrinted = false;
            PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage, Lan.g(this, "CreditCard recurring charges list printed"), PrintoutOrientation.Landscape);
        }

        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            //new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
            Graphics g = e.Graphics;
            string text;
            Font headingFont = new Font("Arial", 13, FontStyle.Bold);
            Font subHeadingFont = new Font("Arial", 10, FontStyle.Bold);
            int yPos = bounds.Top;
            int center = bounds.X + bounds.Width / 2;
            #region printHeading
            if (!headingPrinted)
            {
                text = Lan.g(this, "Recurring Charges");
                g.DrawString(text, headingFont, Brushes.Black, center - g.MeasureString(text, headingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, headingFont).Height;
                yPos += 20;
                headingPrinted = true;
                headingPrintH = yPos;
            }
            #endregion
            yPos = gridMain.PrintPage(g, pagesPrinted, bounds, headingPrintH);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            g.Dispose();
        }

        private void butAll_Click(object sender, EventArgs e)
        {
            gridMain.SetSelected(true);
            labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
        }

        private void butNone_Click(object sender, EventArgs e)
        {
            gridMain.SetSelected(false);
            labelSelected.Text = Lan.g(this, "Selected=") + gridMain.SelectedIndices.Length.ToString();
        }

        private void butHistory_Click(object sender, EventArgs e)
        {
            FormRecurringChargesHistory FormRCH = new FormRecurringChargesHistory();
            FormRCH.Show();
        }

        ///<summary>Will process payments for all authorized charges for each CC stored and marked for recurring charges.
        ///X-Charge or PayConnect or PaySimple must be enabled.
        ///Program validation done on load and if properties are not valid the form will close and exit.</summary>
        private void butSend_Click(object sender, EventArgs e)
        {
            if (_charger.IsCharging)
            {
                return;
            }
            RefreshRecurringCharges(false, true);
            if (gridMain.SelectedIndices.Length < 1)
            {
                MsgBox.Show(this, "Must select at least one recurring charge.");
                return;
            }
            if (!Security.IsAuthorized(Permissions.PaymentCreate))
            {
                return;
            }
            _charger.IsCharging = true;//Doing this on the main thread in case another click event gets here before the thread can start
                                       //Not checking DatePay for FutureTransactionDate pref violation because these should always have a date <= today
            Cursor = Cursors.WaitCursor;
            List<RecurringChargeData> listCharges = gridMain.SelectedTags<RecurringChargeData>();
            bool doForceDuplicates = checkForceDuplicates.Checked;
            ODThread chargeCreationThread = new ODThread(o =>
            {
                _charger.SendCharges(listCharges, doForceDuplicates);
                if (IsDisposed)
                {
                    return;
                }
                try
                {
                    this.Invoke(() =>
                    {
                        FillGrid(true);
                        Cursor = Cursors.Default;
                        MsgBox.Show(this, "Done charging cards.\r\nIf there are any patients remaining in list, print the list and handle each one manually.");
                    });
                }
                catch (ObjectDisposedException)
                {
                    //This likely occurred if the user clicked Close before the charges were done processing. Since we don't need to display the grid, we can
                    //do nothing.
                }
            });
            chargeCreationThread.Name = "RecurringChargeProcessor";
            chargeCreationThread.Start();
        }

        private void StopRecurringCharges(ODEventArgs args)
        {
            if (args.EventType == ODEventType.Shutdown)
            {
                _charger.StopCharges(true);
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormCreditRecurringCharges_FormClosing(object sender, FormClosingEventArgs e)
        {
            _charger?.DeleteNotYetCharged();
            _charger?.StopCharges();//This will still allow the current card to finish.
            GeneralProgramEvent.Fired -= StopRecurringCharges;
        }
    }
}
