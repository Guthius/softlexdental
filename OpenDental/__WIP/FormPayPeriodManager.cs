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
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPayPeriodManager : FormBase
    {
        private readonly List<PayPeriod> payPeriods = new List<PayPeriod>();

        public FormPayPeriodManager() => InitializeComponent();

        private void FormPayPeriodManager_Load(object sender, EventArgs e)
        {
            var payPeriod = PayPeriod.GetMostRecent();

            startDateDateTimePicker.Value = 
                payPeriod == null ? 
                    DateTime.Today : 
                    payPeriod.DateEnd.AddDays(1);

            var payPeriodInterval = (PayPeriodInterval)Preference.GetInt(PreferenceName.PayPeriodIntervalSetting);
            if (payPeriodInterval == PayPeriodInterval.Weekly)
            {
                intervalWeeklyRadioButton.Checked = true;
                numPayPeriodsTextBox.Text = "52";
            }
            else if (payPeriodInterval == PayPeriodInterval.BiWeekly)
            {
                intervalBiWeeklyRadioButton.Checked = true;
                numPayPeriodsTextBox.Text = "21";
            }
            else if (payPeriodInterval == PayPeriodInterval.Monthly)
            {
                intervalMonhtlyRadioButton.Checked = true;
                numPayPeriodsTextBox.Text = "12";
            }

            int dayOfWeek = Preference.GetInt(PreferenceName.PayPeriodPayDay);
            if (dayOfWeek != 0)
            {
                dayComboBox.SelectedIndex = dayOfWeek;
                numDaysAfterTextBox.Enabled = false;
                excludeWeekendsCheckBox.Enabled = false;
                payBeforeRadioButton.Enabled = false;
                payAfterRadioButton.Enabled = false;
            }
            else
            {
                dayComboBox.SelectedIndex = 0;
                numDaysAfterTextBox.Text = Preference.GetString(PreferenceName.PayPeriodPayAfterNumberOfDays);
                excludeWeekendsCheckBox.Checked = Preference.GetBool(PreferenceName.PayPeriodPayDateExcludesWeekends);
                if (excludeWeekendsCheckBox.Checked)
                {
                    if (Preference.GetBool(PreferenceName.PayPeriodPayDateBeforeWeekend))
                    {
                        payBeforeRadioButton.Checked = true;
                    }
                    else
                    {
                        payAfterRadioButton.Checked = true;
                    }
                }
                if (!excludeWeekendsCheckBox.Checked)
                {
                    payBeforeRadioButton.Checked = false;
                    payBeforeRadioButton.Enabled = false;
                    payAfterRadioButton.Checked = false;
                    payAfterRadioButton.Enabled = false;
                }
                else
                {
                    payBeforeRadioButton.Enabled = true;
                    payAfterRadioButton.Enabled = true;
                }
            }

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            payPeriodGrid.BeginUpdate();
            payPeriodGrid.Columns.Clear();
            payPeriodGrid.Columns.Add(new ODGridColumn("Start Date", 80));
            payPeriodGrid.Columns.Add(new ODGridColumn("End Date", 80));
            payPeriodGrid.Columns.Add(new ODGridColumn("Paycheck Date", 100));
            payPeriodGrid.Rows.Clear();

            foreach (var payPeriod in payPeriods)
            {
                var row = new ODGridRow();
                row.Cells.Add(payPeriod.DateStart.ToShortDateString());
                row.Cells.Add(payPeriod.DateEnd.ToShortDateString());

                if (payPeriod.DatePaycheck.Year < 1880)
                {
                    row.Cells.Add("");
                }
                else
                {
                    row.Cells.Add(payPeriod.DatePaycheck.ToShortDateString());
                }

                payPeriodGrid.Rows.Add(row);
            }

            payPeriodGrid.EndUpdate();
        }

        private void PayPeriodGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formPayPeriodEdit = new FormPayPeriodEdit(payPeriods[e.Row]))
            {
                formPayPeriodEdit.ShowDialog(this);

                PopulateGrid();
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(numPayPeriodsTextBox.Text, out var numPayPeriods) || numPayPeriods <= 0)
            {
                MessageBox.Show(
                    "The number of payment periods to generate must be above 0.",
                    "Pay Period Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                numPayPeriodsTextBox.Focus();

                return;
            }

            int numDaysAfter = 0;
            if (numDaysAfterTextBox.Enabled)
            {
                if (!int.TryParse(numDaysAfterTextBox.Text, out numDaysAfter) || numDaysAfter <= 0)
                {
                    MessageBox.Show(
                        "The number of days after pay period cannot be zero.",
                        "Pay Period Manager",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    numDaysAfterTextBox.Focus();

                    return;
                }
            }

            payPeriods.Clear();

            var payPeriodInterval = PayPeriodInterval.Weekly;
            if (intervalBiWeeklyRadioButton.Checked)
            {
                payPeriodInterval = PayPeriodInterval.BiWeekly;
            }
            else if (intervalMonhtlyRadioButton.Checked)
            {
                payPeriodInterval = PayPeriodInterval.Monthly;
            }

            var currentDate = startDateDateTimePicker.Value.Date;

            for (int i = 0; i < numPayPeriods; i++)
            {
                var payPeriod = new PayPeriod
                {
                    DateStart = currentDate
                };

                switch (payPeriodInterval)
                {
                    case PayPeriodInterval.Weekly:
                        payPeriod.DateEnd = currentDate.AddDays(6);
                        currentDate = currentDate.AddDays(7);
                        break;

                    case PayPeriodInterval.BiWeekly:
                        payPeriod.DateEnd = currentDate.AddDays(13);
                        currentDate = currentDate.AddDays(14);
                        break;

                    case PayPeriodInterval.Monthly:
                        payPeriod.DateEnd = currentDate.AddMonths(1).Subtract(TimeSpan.FromDays(1));
                        currentDate = currentDate.AddMonths(1);
                        break;
                }

                if (dayComboBox.Enabled) payPeriod.DatePaycheck = GetDateOfDay(payPeriod.DateEnd, (DayOfWeek)(dayComboBox.SelectedIndex - 1));
                else
                {
                    payPeriod.DatePaycheck = payPeriod.DateEnd.AddDays(numDaysAfter);

                    if (excludeWeekendsCheckBox.Checked && (payPeriod.DatePaycheck.DayOfWeek == DayOfWeek.Saturday || payPeriod.DatePaycheck.DayOfWeek == DayOfWeek.Sunday))
                    {
                        if (payBeforeRadioButton.Checked)
                        {
                            // Get the date of the friday before the paycheck date. If the friday 
                            // is not within the pay period we use that as the paycheck date. 
                            // If the friday is within the pay period we instead move forward and 
                            // use next monday as the paycheck date instead.

                            var paycheckDate = GetDateOfDay(payPeriod.DatePaycheck, DayOfWeek.Friday, -1);
                            if (paycheckDate <= payPeriod.DateEnd)
                            {
                                paycheckDate = GetDateOfDay(payPeriod.DatePaycheck, DayOfWeek.Monday);
                            }

                            payPeriod.DatePaycheck = paycheckDate;
                        }
                        else
                        {
                            payPeriod.DatePaycheck = GetDateOfDay(payPeriod.DatePaycheck, DayOfWeek.Monday);
                        }
                    }
                }

                payPeriods.Add(payPeriod);
            }

            PopulateGrid();
        }

        /// <summary>
        /// Returns the DateTime of the first instance of <see cref="DayOfWeek"/>, given a specific
        /// start time. It will not include the start date as a result.
        /// </summary>
        private DateTime GetDateOfDay(DateTime startDate, DayOfWeek day, int step = 1)
        {
            do
            {
                startDate = startDate.AddDays(step);
            }
            while (startDate.DayOfWeek != day);

            return startDate;
        }

        private void MonthlyRadioButton_Click(object sender, EventArgs e) => numPayPeriodsTextBox.Text = "12";

        private void BiWeeklyRadioButton_Click(object sender, EventArgs e) => numPayPeriodsTextBox.Text = "21";

        private void WeeklyRadioButton_Click(object sender, EventArgs e) => numPayPeriodsTextBox.Text = "52";

        private void DayComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (dayComboBox.SelectedIndex != 0)
            {
                numDaysAfterTextBox.Text = "0";
                numDaysAfterTextBox.Enabled = false;
                excludeWeekendsCheckBox.Enabled = false;
                excludeWeekendsCheckBox.Checked = false;
                payBeforeRadioButton.Enabled = false;
                payBeforeRadioButton.Checked = false;
                payAfterRadioButton.Enabled = false;
                payAfterRadioButton.Checked = false;
            }
            else
            {
                numDaysAfterTextBox.Text = "0";
                numDaysAfterTextBox.Enabled = true;
                excludeWeekendsCheckBox.Enabled = true;
                payBeforeRadioButton.Enabled = true;
                payAfterRadioButton.Enabled = true;
            }
        }

        private void NumDaysAfterTextBox_TextChanged(object sender, EventArgs e)
        {
            var numDaysAfterText = numDaysAfterTextBox.Text.Trim();

            if (numDaysAfterText.Length > 0 && int.TryParse(numDaysAfterText, out var numDaysAfter) && numDaysAfter > 0)
            {
                dayComboBox.SelectedIndex = 0;
                dayComboBox.Enabled = false;
                excludeWeekendsCheckBox.Enabled = true;
                payBeforeRadioButton.Enabled = true;
                payAfterRadioButton.Enabled = true;

                numDaysAfterTextBox.Text = numDaysAfter.ToString();
            }
            else
            {
                dayComboBox.Enabled = true;

                numDaysAfterTextBox.Text = "0";
            }
        }

        private void ExcludeWeekendsCheckBox_CheckedChanged(object sender, EventArgs e) =>
            payAfterRadioButton.Enabled = payBeforeRadioButton.Enabled = excludeWeekendsCheckBox.Checked;

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (payPeriodGrid.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Pay periods must be generated first.",
                    "Pay Period Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var numDaysAfterText = numDaysAfterTextBox.Text.Trim();
            if (numDaysAfterText.Length == 0 || !int.TryParse(numDaysAfterText, out int numDaysAfter) || numDaysAfter <= 0)
            {
                MessageBox.Show(
                    "You must specify a valid day.",
                    "Pay Period Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            CacheManager.Invalidate<PayPeriod>();

            if (PayPeriod.AreAnyOverlapping(PayPeriod.All(), payPeriods))
            {
                MessageBox.Show(
                    "You have created pay periods that would overlap with existing pay periods. Please fix those pay periods first.",
                    "Pay Period Manager", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                return;
            }

            foreach (var payPeriod in payPeriods) PayPeriod.Insert(payPeriod);

            if (intervalWeeklyRadioButton.Checked)
            {
                Preference.Update(PreferenceName.PayPeriodIntervalSetting, (int)PayPeriodInterval.Weekly);
            }
            else if (intervalBiWeeklyRadioButton.Checked)
            {
                Preference.Update(PreferenceName.PayPeriodIntervalSetting, (int)PayPeriodInterval.BiWeekly);
            }
            else
            {
                Preference.Update(PreferenceName.PayPeriodIntervalSetting, (int)PayPeriodInterval.Monthly);
            }

            Preference.Update(PreferenceName.PayPeriodPayDay, dayComboBox.SelectedIndex);
            Preference.Update(PreferenceName.PayPeriodPayAfterNumberOfDays, numDaysAfter);
            Preference.Update(PreferenceName.PayPeriodPayDateExcludesWeekends, excludeWeekendsCheckBox.Checked);

            if (payBeforeRadioButton.Checked)
            {
                Preference.Update(PreferenceName.PayPeriodPayDateBeforeWeekend, true);
            }
            else if (payAfterRadioButton.Checked)
            {
                Preference.Update(PreferenceName.PayPeriodPayDateBeforeWeekend, false);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
