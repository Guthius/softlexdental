using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental
{
    public partial class FormAdjustSelect : FormBase
    {
        readonly List<AccountEntry> accountEntryList = new List<AccountEntry>();
        List<PaySplit> paySplitList;

        public Adjustment SelectedAdj { get; set; }

        public PaySplit PaySplitCur;
        public List<PaySplit> ListSplitsForPayment;
        public long PatNumCur;
        public double PaySplitCurAmt;
        public long PayNumCur;

        public FormAdjustSelect() => InitializeComponent();

        void FormAdjustSelect_Load(object sender, EventArgs e)
        {
            // Get all unallocated adjustments for current pat.
            var adjustmentList = Adjustments.GetAdjustForPats(new List<long>() { PatNumCur }).FindAll(x => x.ProcNum == 0);
            foreach (var adjustment in adjustmentList)
            {
                accountEntryList.Add(new AccountEntry(adjustment));
            }

            paySplitList = PaySplits.GetForAdjustments(adjustmentList.Select(x => x.Id).ToList());
            foreach (var accountEntry in accountEntryList)
            {
                // Figure out how much each adjustment has left, not counting this payment.
                accountEntry.AmountStart -= (decimal)Adjustments.GetAmtAllocated(accountEntry.PriKey, PayNumCur, paySplitList.FindAll(x => x.AdjNum == accountEntry.PriKey));

                // Reduce adjustments based on current payment's splits as well (this is in-memory list, could be new, could be modified) but not the current split
                accountEntry.AmountStart -= (decimal)Adjustments.GetAmtAllocated(accountEntry.PriKey, 0, ListSplitsForPayment.FindAll(x => x.AdjNum == accountEntry.PriKey && x != PaySplitCur));
            }

            LoadAdjustments();
        }

        void LoadAdjustments()
        {
            adjustmentGrid.BeginUpdate();
            
            adjustmentGrid.Columns.Clear();
            adjustmentGrid.Columns.Add(new ODGridColumn("Date", 70));
            adjustmentGrid.Columns.Add(new ODGridColumn("Prov", 55));
            if (Preferences.HasClinicsEnabled)
            {
                adjustmentGrid.Columns.Add(new ODGridColumn("Clinic", 55));
            }
            adjustmentGrid.Columns.Add(new ODGridColumn("Amt Orig", 60, HorizontalAlignment.Right));
            adjustmentGrid.Columns.Add(new ODGridColumn("Amt End", 60, HorizontalAlignment.Right));
            adjustmentGrid.Rows.Clear();

            foreach (var accountEntry in accountEntryList)
            {
                var row = new ODGridRow();
                row.Cells.Add(((Adjustment)accountEntry.Tag).AdjDate.ToShortDateString());
                row.Cells.Add(Providers.GetAbbr(((Adjustment)accountEntry.Tag).ProvNum));
                if (Preferences.HasClinicsEnabled)
                {
                    row.Cells.Add(Clinics.GetAbbr(((Adjustment)accountEntry.Tag).ClinicNum));
                }
                row.Cells.Add(accountEntry.AmountOriginal.ToString("F"));//Amt Orig
                row.Cells.Add(accountEntry.AmountStart.ToString("F"));//Amt Available
                row.Tag = accountEntry;
                adjustmentGrid.Rows.Add(row);
            }

            adjustmentGrid.EndUpdate();
        }

        void AdjustmentGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (adjustmentGrid.SelectedGridRows[0].Tag is AccountEntry accountEntry)
            {
                amtOriginialLabel.Text = accountEntry.AmountOriginal.ToString("F");                         // Adjustment's start original - Negative or positive it doesn't matter.
                labelAmtUsed.Text = (accountEntry.AmountOriginal - accountEntry.AmountStart).ToString("F"); // Amount of Adjustment that's been used elsewhere
                labelAmtAvail.Text = accountEntry.AmountStart.ToString("F");                                // Amount of Adjustment that's left available
                labelCurSplitAmt.Text = (-PaySplitCurAmt).ToString("F");                                    // Amount of current PaySplit (We can only access this window from current PaySplitEdit window right now)
                labelAmtEnd.Text = ((double)accountEntry.AmountStart - PaySplitCurAmt).ToString("F");       // Amount of Adjustment after everything.
            }
        }

        void AdjustmentGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (adjustmentGrid.SelectedGridRows[0].Tag is AccountEntry accountEntry)
            {
                if (accountEntry.Tag is Adjustment adjustment)
                {
                    SelectedAdj = adjustment;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (adjustmentGrid.SelectedIndices.Length < 1)
            {
                MessageBox.Show(
                    "Please select an adjustment first or press Cancel.",
                    "Select Adjustment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (adjustmentGrid.SelectedGridRows[0].Tag is AccountEntry accountEntry)
            {
                if (accountEntry.Tag is Adjustment adjustment)
                {
                    SelectedAdj = adjustment;

                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}