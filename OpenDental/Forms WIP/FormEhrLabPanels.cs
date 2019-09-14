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
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEhrLabPanels : FormBase
    {
        private List<LabPanel> panels;

        public Patient Patient { get; set; }

        public bool IsSelectionMode { get; set; }

        public long SelectedLabPanelId { get; set; }

        public FormEhrLabPanels() =>  InitializeComponent();

        private void FormLabPanels_Load(object sender, EventArgs e)
        {
            if (!IsSelectionMode)
            {
                acceptButton.Visible = false;
                cancelButton.Text = "Close";
            }
            FillGrid();
        }

        private void FillGrid()
        {
            panelsGrid.BeginUpdate();
            panelsGrid.Columns.Clear();
            panelsGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateTime, 135));
            panelsGrid.Columns.Add(new ODGridColumn("Service", 200));
            panelsGrid.Rows.Clear();

            panels = LabPanels.Refresh(Patient.PatNum);
            foreach (var labPanel in panels)
            {
                var row = new ODGridRow();

                var labResults = LabResults.GetForPanel(labPanel.LabPanelNum);
                if (labResults.Count == 0)
                {
                    row.Cells.Add(" ");
                }
                else
                {
                    row.Cells.Add(labResults[0].DateTimeTest.ToString());
                }

                row.Cells.Add(labPanel.ServiceName);

                panelsGrid.Rows.Add(row);
            }

            panelsGrid.EndUpdate();
        }

        private void PanelsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formEhrLabPanelEdit = new FormEhrLabPanelEdit())
            {
                formEhrLabPanelEdit.PanelCur = panels[e.Row];
                formEhrLabPanelEdit.ShowDialog();

                FillGrid();
            }

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var formEhrLabPanelEdit = new FormEhrLabPanelEdit())
            {
                formEhrLabPanelEdit.IsNew = true;
                formEhrLabPanelEdit.PanelCur = new LabPanel
                {
                    PatNum = Patient.PatNum,
                    SpecimenSource = ""
                };
                formEhrLabPanelEdit.ShowDialog();

                FillGrid();
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (panelsGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select lab panels first.",
                    "Lab Panels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var selectedPanels = new List<LabPanel>();
            for (int i = 0; i < panelsGrid.SelectedIndices.Length; i++)
            {
                selectedPanels.Add(panels[panelsGrid.SelectedIndices[i]]);
            }

            EhrORU oru = new EhrORU();

            Cursor = Cursors.WaitCursor;
            try
            {
                oru.Initialize(selectedPanels);
            }
            catch (ApplicationException exception)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    exception.Message, 
                    "Lab Panels", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            // string message = oru.GenerateMessage();

            try
            {
                // TODO: Implement me.

                // EmailMessages.SendTestUnsecure("Public Health","oru.txt",outputStr);
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    exception.Message,
                    "Lab Panels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Cursor = Cursors.Default;

            MessageBox.Show(
                "Sent",
                "Lab Panels",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            if (panelsGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select lab panels first.",
                    "Lab Panels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var selectedPanels = new List<LabPanel>();
            for (int i = 0; i < panelsGrid.SelectedIndices.Length; i++)
            {
                selectedPanels.Add(panels[panelsGrid.SelectedIndices[i]]);
            }

            EhrORU oru = new EhrORU();

            Cursor = Cursors.WaitCursor;
            try
            {
                oru.Initialize(selectedPanels);
            }
            catch (ApplicationException ex)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    ex.Message,
                    "Lab Panels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string message = oru.GenerateMessage();

            Cursor = Cursors.Default;

            using (var msgBoxCopyPaste = new MsgBoxCopyPaste(message))
            {
                msgBoxCopyPaste.ShowDialog(this);
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (panelsGrid.SelectedIndices.Length != 1)
            {
                MessageBox.Show(
                    "Please select exactly one lab panel.",
                    "Lab Panels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            SelectedLabPanelId = panels[panelsGrid.SelectedIndices[0]].LabPanelNum;

            DialogResult = DialogResult.OK;
        }
    }
}
