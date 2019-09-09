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
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormTimeCardRuleEdit : FormBase
    {
        private readonly TimeCardRule timeCardRule;

        public FormTimeCardRuleEdit(TimeCardRule timeCardRule)
        {
            InitializeComponent();

            this.timeCardRule = timeCardRule;

            if (this.timeCardRule.IsNew)
            {
                employeesListBox.SelectionMode = SelectionMode.MultiExtended;
            }
        }

        private void FormTimeCardRuleEdit_Load(object sender, EventArgs e)
        {
            var employees = 
                Preferences.HasClinicsEnabled ? 
                    Employee.GetEmpsForClinic(Clinics.ClinicNum) : 
                    Employee.GetForTimeCard();

            employeesListBox.Items.Add("All Employees");
            if (timeCardRule.EmployeeId == null)
            {
                employeesListBox.SelectedIndex = 0;
            }

            foreach (var employee in employees)
            {
                int index = employeesListBox.Items.Add(employee);
                if (employee.Id == timeCardRule.EmployeeId)
                {
                    employeesListBox.SelectedItem = employee;
                }
            }

            overHoursTextBox.Text = timeCardRule.Hours.ToString();
            timeAfterTextBox.Text = timeCardRule.TimeEnd.ToString();
            timeBeforeTextBox.Text = timeCardRule.TimeStart.ToString();
            overtimeExemptCheckBox.Checked = timeCardRule.IsOvertimeExempt;
            minClockinTextBox.Text = timeCardRule.ClockInTime.ToString();
        }

        private void TimeAfterButton_Click(object sender, EventArgs e) => 
            timeAfterTextBox.Text = new DateTime(2010, 1, 1, 17, 0, 0).ToShortTimeString();

        private void TimeBeforeButton_Click(object sender, EventArgs e) =>
            timeBeforeTextBox.Text = new DateTime(2010, 1, 1, 6, 0, 0).ToShortTimeString();

        private void OverHoursTextBox_TextChanged(object sender, EventArgs e)
        {
            if (overHoursTextBox.Text != "")
            {
                timeAfterTextBox.Text = "";
                timeBeforeTextBox.Text = "";
            }
        }

        private void TimeBeforeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (timeBeforeTextBox.Text != "")
            {
                overHoursTextBox.Text = "";
            }
        }

        private void TimeAfterTextBox_TextChanged(object sender, EventArgs e)
        {
            if (timeAfterTextBox.Text != "")
            {
                overHoursTextBox.Text = "";
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (timeCardRule.IsNew)
            {
                DialogResult = DialogResult.Cancel;

                return;
            }

            var result =
                MessageBox.Show(
                    "Are you sure you want to delete this time card rule?",
                    "Time Card Rule", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            TimeCardRule.Delete(timeCardRule.Id);

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!employeesListBox.GetSelected(0) && employeesListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show(
                    "Please select an employee.",
                    "Time Card Rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (!TimeSpan.TryParse(overHoursTextBox.Text, out var overHoursPerDay) ||
                overHoursPerDay == TimeSpan.Zero ||
                overHoursPerDay.Days > 0)
            {
                MessageBox.Show(
                    "Over hours per day invalid.",
                    "Time Card Rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!TimeSpan.TryParse(timeAfterTextBox.Text, out var afterTimeOfDay) ||
                afterTimeOfDay == TimeSpan.Zero ||
                afterTimeOfDay.Days > 0)
            {
                MessageBox.Show(
                    "After time of day invalid.",
                    "Time Card Rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!TimeSpan.TryParse(timeAfterTextBox.Text, out var beforeTimeOfDay) ||
                beforeTimeOfDay == TimeSpan.Zero ||
                beforeTimeOfDay.Days > 0)
            {
                MessageBox.Show(
                    "Before time of day invalid.",
                    "Time Card Rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string minClockInText = minClockinTextBox.Text.Trim();
            var minClockIn = TimeSpan.Zero;

            if (minClockInText.Length > 0)
            {
                if (!TimeSpan.TryParse(minClockInText, out minClockIn))
                {
                    MessageBox.Show(
                        "Earliest clock in time invalid.",
                        "Time Card Rule",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            if (!overtimeExemptCheckBox.Checked && overHoursPerDay == TimeSpan.Zero && afterTimeOfDay == TimeSpan.Zero && beforeTimeOfDay == TimeSpan.Zero && minClockIn == TimeSpan.Zero)
            {
                MessageBox.Show(
                    "Either is overtime exempt, over hours, after or before time of day or Clock In Min must be entered.",
                    "Time Card Rule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            timeCardRule.EmployeeId = null;
            timeCardRule.Hours = overHoursPerDay;
            timeCardRule.TimeEnd = afterTimeOfDay;
            timeCardRule.TimeStart = beforeTimeOfDay;
            timeCardRule.IsOvertimeExempt = overtimeExemptCheckBox.Checked;
            timeCardRule.ClockInTime = minClockIn;

            if (timeCardRule.IsNew)
            {
                if (!employeesListBox.GetSelected(0))
                {
                    timeCardRule.EmployeeId = ((Employee)employeesListBox.SelectedItems[0]).Id;

                    TimeCardRule.Insert(timeCardRule);

                    foreach (Employee employee in employeesListBox.SelectedItems)
                    {
                        if (employee.Id == timeCardRule.EmployeeId) continue;

                        TimeCardRule.Insert(new TimeCardRule
                        {
                            EmployeeId = employee.Id,
                            Hours = overHoursPerDay,
                            TimeEnd = afterTimeOfDay,
                            TimeStart = beforeTimeOfDay,
                            IsOvertimeExempt = overtimeExemptCheckBox.Checked,
                            ClockInTime = minClockIn
                        });
                    }
                }
            }
            else
            {
                if (!employeesListBox.GetSelected(0))
                {
                    if (employeesListBox.SelectedItems[0] is Employee employee)
                    {
                        timeCardRule.EmployeeId = employee.Id;
                    }
                }

                TimeCardRule.Update(timeCardRule);
            }

            CacheManager.Invalidate<TimeCardRule>();

            DialogResult = DialogResult.OK;
        }
    }
}
