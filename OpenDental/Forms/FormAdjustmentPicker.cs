using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAdjustmentPicker : FormBase
    {
        bool _isUnattachedMode;
        long _patNum;
        List<Adjustment> _listAdjustments;
        List<Adjustment> _listAdjustmentsFiltered;

        public Adjustment SelectedAdjustment;

        public FormAdjustmentPicker(long patNum, bool isUnattachedMode = false, List<Adjustment> listAdjustments = null)
        {
            InitializeComponent();

            _patNum = patNum;
            _isUnattachedMode = isUnattachedMode;
            _listAdjustments = listAdjustments;
        }

        private void FormAdjustmentPicker_Load(object sender, EventArgs e)
        {
            if (_isUnattachedMode)
            {
                checkUnattached.Checked = true;
                checkUnattached.Enabled = false;
            }
            if (_listAdjustments == null)
            {
                _listAdjustments = Adjustments.Refresh(_patNum).ToList();
            }
            FillGrid();
        }

        void FillGrid()
        {
            _listAdjustmentsFiltered = _listAdjustments;
            if (checkUnattached.Checked)
            {
                _listAdjustmentsFiltered = _listAdjustments.FindAll(x => x.ProcNum == 0);
            }

            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            gridMain.Columns.Add(new ODGridColumn("Date", 90));
            gridMain.Columns.Add(new ODGridColumn("PatNum", 100));
            gridMain.Columns.Add(new ODGridColumn("Type", 120));
            gridMain.Columns.Add(new ODGridColumn("Amount", 70));
            gridMain.Columns.Add(new ODGridColumn("Has Proc", 0, HorizontalAlignment.Center));
            gridMain.Rows.Clear();

            foreach (Adjustment adjCur in _listAdjustmentsFiltered)
            {
                var row = new ODGridRow();

                row.Cells.Add(adjCur.AdjDate.ToShortDateString());
                row.Cells.Add(adjCur.PatNum.ToString());
                row.Cells.Add(Defs.GetName(DefCat.AdjTypes, adjCur.AdjType));
                row.Cells.Add(adjCur.AdjAmt.ToString("F"));
                if (adjCur.ProcNum != 0)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }
                row.Tag = adjCur;

                gridMain.Rows.Add(row);
            }
            gridMain.EndUpdate();
        }

        private void checkUnattached_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            SelectedAdjustment = _listAdjustmentsFiltered[gridMain.GetSelectedIndex()];
            DialogResult = DialogResult.OK;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            SelectedAdjustment = _listAdjustmentsFiltered[gridMain.GetSelectedIndex()];
            DialogResult = DialogResult.OK;
        }
    }
}