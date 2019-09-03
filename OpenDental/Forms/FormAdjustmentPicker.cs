/**
 * Softlex Dental Project
 * Copyright (C) 2019 Dental Stars SRL
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
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