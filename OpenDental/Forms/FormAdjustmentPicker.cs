/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
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
        private readonly bool unattachedMode;
        private readonly long patientId;
        private List<Adjustment> adjustments;
        private List<Adjustment> adjustmentsFiltered;

        /// <summary>
        /// Gets or sets the selected adjustment.
        /// </summary>
        public Adjustment SelectedAdjustment { get; set; }

        public FormAdjustmentPicker(long patientId, bool unattachedMode = false, List<Adjustment> adjustments = null)
        {
            InitializeComponent();

            this.patientId = patientId;
            this.unattachedMode = unattachedMode;
            this.adjustments = adjustments;
        }

        void LoadAdjustments()
        {
            adjustmentsFiltered = adjustments;
            if (unattachedCheckBox.Checked)
            {
                adjustmentsFiltered = adjustments.FindAll(x => x.ProcNum == 0);
            }

            adjustmentGrid.BeginUpdate();
            adjustmentGrid.Columns.Clear();
            adjustmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDate, 90));
            adjustmentGrid.Columns.Add(new ODGridColumn("PatNum", 100));
            adjustmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnType, 120));
            adjustmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnAmount, 70));
            adjustmentGrid.Columns.Add(new ODGridColumn("Has Proc", 0, HorizontalAlignment.Center));
            adjustmentGrid.Rows.Clear();

            foreach (var adjustment in adjustmentsFiltered)
            {
                var row = new ODGridRow();

                row.Cells.Add(adjustment.AdjDate.ToShortDateString());
                row.Cells.Add(adjustment.PatNum.ToString());
                row.Cells.Add(Defs.GetName(DefinitionCategory.AdjTypes, adjustment.AdjType));
                row.Cells.Add(adjustment.AdjAmt.ToString("f"));
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

            if (adjustments == null)
            {
                adjustments = Adjustments.Refresh(patientId).ToList();
            }

            LoadAdjustments();
        }

        void AdjustmentGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) => AcceptButton_Click(this, EventArgs.Empty);
        
        void UnattachedCheckBox_Click(object sender, EventArgs e) => LoadAdjustments();
        
        void AcceptButton_Click(object sender, EventArgs e)
        {
            SelectedAdjustment = adjustmentsFiltered[adjustmentGrid.GetSelectedIndex()];

            DialogResult = DialogResult.OK;
        }
    }
}
