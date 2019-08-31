/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
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
        readonly bool unattachedMode;
        readonly long patientId;
        List<Adjustment> adjustmentList;
        List<Adjustment> adjustmentListFiltered;

        public Adjustment SelectedAdjustment;

        public FormAdjustmentPicker(long patientId, bool unattachedMode = false, List<Adjustment> adjustmentList = null)
        {
            InitializeComponent();

            this.patientId = patientId;
            this.unattachedMode = unattachedMode;
            this.adjustmentList = adjustmentList;
        }

        void LoadAdjustments()
        {
            adjustmentListFiltered = adjustmentList;
            if (unattachedCheckBox.Checked)
            {
                adjustmentListFiltered = adjustmentList.FindAll(x => x.ProcNum == 0);
            }

            adjustmentGrid.BeginUpdate();
            adjustmentGrid.Columns.Clear();
            adjustmentGrid.Columns.Add(new ODGridColumn("Date", 90));
            adjustmentGrid.Columns.Add(new ODGridColumn("PatNum", 100));
            adjustmentGrid.Columns.Add(new ODGridColumn("Type", 120));
            adjustmentGrid.Columns.Add(new ODGridColumn("Amount", 70));
            adjustmentGrid.Columns.Add(new ODGridColumn("Has Proc", 0, HorizontalAlignment.Center));
            adjustmentGrid.Rows.Clear();

            foreach (var adjustment in adjustmentListFiltered)
            {
                var row = new ODGridRow();

                row.Cells.Add(adjustment.AdjDate.ToShortDateString());
                row.Cells.Add(adjustment.PatNum.ToString());
                row.Cells.Add(Defs.GetName(DefinitionCategory.AdjTypes, adjustment.AdjType));
                row.Cells.Add(adjustment.AdjAmt.ToString("F"));
                row.Cells.Add(adjustment.ProcNum != 0 ? "X" : "");
                row.Tag = adjustment;

                adjustmentGrid.Rows.Add(row);
            }
            adjustmentGrid.EndUpdate();
        }

        void FormAdjustmentPicker_Load(object sender, EventArgs e)
        {
            if (unattachedMode)
            {
                unattachedCheckBox.Checked = true;
                unattachedCheckBox.Enabled = false;
            }

            if (adjustmentList == null)
            {
                adjustmentList = Adjustments.Refresh(patientId).ToList();
            }

            LoadAdjustments();
        }

        void AdjustmentGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) => AcceptButton_Click(this, EventArgs.Empty);
        
        void UnattachedCheckBox_Click(object sender, EventArgs e) => LoadAdjustments();
        
        void AcceptButton_Click(object sender, EventArgs e)
        {
            SelectedAdjustment = adjustmentListFiltered[adjustmentGrid.GetSelectedIndex()];

            DialogResult = DialogResult.OK;
        }
    }
}