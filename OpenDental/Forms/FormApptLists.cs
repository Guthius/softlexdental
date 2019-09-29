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
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptLists : FormBase
    {
        public ApptListSelection SelectionResult { get; private set; }

        public FormApptLists() => InitializeComponent();

        private void RecallsButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Recall);

        private void ConfirmationsButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Confirm);

        private void PlannedTrackerButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Planned);

        private void UnscheduledButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Unsched);

        private void AsapButton_Click(object sender, EventArgs e) => Close(ApptListSelection.ASAP);

        private void RadiologyButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Radiology);

        private void InsuranceVerifyButton_Click(object sender, EventArgs e) => Close(ApptListSelection.InsVerify);

        private void Close(ApptListSelection apptListSelection)
        {
            SelectionResult = apptListSelection;
            DialogResult = DialogResult.OK;
        }
    }

    /// <summary>
    /// Used in FormApptLists as the selection result.
    /// </summary>
    public enum ApptListSelection
    {
        Recall,
        Confirm,
        Planned,
        Unsched,
        ASAP,
        Radiology,
        InsVerify
    }
}
