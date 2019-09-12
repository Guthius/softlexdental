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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormTimeCardSetup : FormBase
    {
        private bool hasChanged;
        private List<PayPeriod> payPeriods;

        public FormTimeCardSetup() => InitializeComponent();

        private void FormPayPeriods_Load(object sender, EventArgs e)
        {
            useDecimalCheckBox.Checked = Preference.GetBool(PreferenceName.TimeCardsUseDecimalInsteadOfColon);
            adjOverBreaksCheckBox.Checked = Preference.GetBool(PreferenceName.TimeCardsMakesAdjustmentsForOverBreaks);
            showSecondsCheckBox.Checked = Preference.GetBool(PreferenceName.TimeCardShowSeconds);

            CacheManager.Invalidate<Employee>();

            LoadPayPeriods();
            FillRules();

            adpCompanyCodeTextBox.Text = Preference.GetString(PreferenceName.ADPCompanyCode);
        }

        private void LoadPayPeriods()
        {
            CacheManager.Invalidate<PayPeriod>();

            payPeriodsGrid.BeginUpdate();
            payPeriodsGrid.Columns.Clear();
            payPeriodsGrid.Columns.Add(new ODGridColumn("Start Date", 80));
            payPeriodsGrid.Columns.Add(new ODGridColumn("End Date", 80));
            payPeriodsGrid.Columns.Add(new ODGridColumn("Paycheck Date", 100));
            payPeriodsGrid.Rows.Clear();

            payPeriods = PayPeriod.All().OrderBy(x => x.DateStart).ToList();
            foreach (var payPeriod in payPeriods)
            {
                if (hideOlderCheckBox.Checked && payPeriod.DateStart < DateTimeOD.Today.AddMonths(-6))
                {
                    continue;
                }

                var row = new ODGridRow();
                row.Cells.Add(payPeriod.DateStart.ToShortDateString());
                row.Cells.Add(payPeriod.DateEnd.ToShortDateString());
                row.Cells.Add(payPeriod.DatePaycheck.Year < 1880 ? "" : payPeriod.DatePaycheck.ToShortDateString());
                row.Tag = payPeriod;

                if (payPeriod.DateStart <= DateTimeOD.Today && payPeriod.DateEnd >= DateTimeOD.Today)
                {
                    row.BackColor = Color.LightCyan;
                }

                payPeriodsGrid.Rows.Add(row);
            }

            payPeriodsGrid.EndUpdate();
        }

        private void FillRules()
        {
            CacheManager.Invalidate<TimeCardRule>();

            var sortedTimeCardRules = 
                TimeCardRule.All()
                    .OrderBy(timeCardRule => timeCardRule.IsOvertimeExempt)
                    .ThenBy(timeCardRule => timeCardRule.EmployeeId.HasValue);

            rulesGrid.BeginUpdate();
            rulesGrid.Columns.Clear();
            rulesGrid.Columns.Add(new ODGridColumn("Employee", 150));
            rulesGrid.Columns.Add(new ODGridColumn("OT before x Time", 105, sortingStrategy: ODGridSortingStrategy.TimeParse));
            rulesGrid.Columns.Add(new ODGridColumn("OT after x Time", 100, sortingStrategy: ODGridSortingStrategy.TimeParse));
            rulesGrid.Columns.Add(new ODGridColumn("OT after x Hours", 110, sortingStrategy: ODGridSortingStrategy.TimeParse));
            rulesGrid.Columns.Add(new ODGridColumn("Min Clock In Time", 105, sortingStrategy: ODGridSortingStrategy.TimeParse));
            rulesGrid.Columns.Add(new ODGridColumn("Is OT Exempt", 100, textAlignment: HorizontalAlignment.Center));
            rulesGrid.Rows.Clear();

            foreach (var timeCardRule in sortedTimeCardRules)
            {
                var row = new ODGridRow();
                if (!timeCardRule.EmployeeId.HasValue)
                {
                    row.Cells.Add("All Employees");
                }
                else
                {
                    var employee = Employee.GetById(timeCardRule.EmployeeId.Value);

                    row.Cells.Add(employee.ToString());
                }

                row.Cells.Add(timeCardRule.TimeStart.ToStringHmm());
                row.Cells.Add(timeCardRule.TimeEnd.ToStringHmm());
                row.Cells.Add(timeCardRule.Hours.ToStringHmm());
                row.Cells.Add(timeCardRule.ClockInTime.ToStringHmm());
                row.Cells.Add(timeCardRule.IsOvertimeExempt ? "X" : "");
                row.Tag = timeCardRule;

                rulesGrid.Rows.Add(row);
            }

            rulesGrid.EndUpdate();
        }

        /// <summary>
        /// <para>Makes sure that the pay periods that the user has selected are safe to delete.</para>
        /// <para>MA pay period cannot be deleted in bulk if:</para>
        /// a) It is in the past OR 
        /// b) There are clockevents tied to it and there are no other pay periods for the date of the clockevent.
        /// </summary>
        private bool IsSafeToDelete(List<PayPeriod> selectedPayPeriods, out List<PayPeriod> results)
        {
            results = new List<PayPeriod>();

            if (selectedPayPeriods.Where(x => x.DateEnd < DateTimeOD.Today).Count() > 0)
            {
                MessageBox.Show(
                    "You may not delete past pay periods from here. Delete them individually by double clicking them instead.");

                return false;
            }


            var clockEvents = ClockEvent.GetAllForPeriod(selectedPayPeriods.Min(x => x.DateStart), selectedPayPeriods.Max(x => x.DateEnd));
            foreach (var payPeriod in selectedPayPeriods)
            {
                var clockEventsForPayPeriod = clockEvents.Where(x => x.Date1Displayed >= payPeriod.DateStart && x.Date2Displayed <= payPeriod.DateEnd).ToList();
                if (clockEventsForPayPeriod.Count == 0)
                {
                    results.Add(payPeriod);

                    continue;
                }

                // There are clock events for this period. now are there other periods that are *not* in the selected list?
                foreach (var clockEvent in clockEventsForPayPeriod)
                {
                    if (payPeriods.Where(x => x.DateStart <= clockEvent.Date1Displayed && x.DateEnd >= clockEvent.Date1Displayed && !selectedPayPeriods.Contains(x)).Count() < 1)
                    {
                        MessageBox.Show(
                            "You may not delete all pay periods where a clock event exists.");

                        results.Clear();

                        return false;
                    }

                    results.Add(payPeriod);
                }
            }

            return true;
        }

        private void PayPeriodsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var payPeriod = (PayPeriod)payPeriodsGrid.Rows[e.Row].Tag;

            using (var formPayPeriodEdit = new FormPayPeriodEdit(payPeriod))
            {
                if (formPayPeriodEdit.ShowDialog() == DialogResult.OK)
                {
                    PayPeriod.Update(payPeriod);

                    LoadPayPeriods();

                    hasChanged = true;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var payPeriod = new PayPeriod
            {
                DateStart =
                    PayPeriod.GetCount() == 0 ?
                        DateTime.Today :
                        PayPeriod.GetLast().DateEnd.AddDays(1)
            };

            payPeriod.DateEnd = payPeriod.DateStart.AddDays(13);
            payPeriod.DatePaycheck = payPeriod.DateEnd.AddDays(4);

            using (var formPayPeriodEdit = new FormPayPeriodEdit(payPeriod))
            {
                if (formPayPeriodEdit.ShowDialog() == DialogResult.OK)
                {
                    PayPeriod.Insert(payPeriod);

                    LoadPayPeriods();

                    hasChanged = true;
                }
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            using (var formPayPeriodManager = new FormPayPeriodManager())
            {
                if (formPayPeriodManager.ShowDialog() == DialogResult.OK)
                {
                    LoadPayPeriods();
                }
            }
        }

        private void UseDecimalCheckBox_Click(object sender, EventArgs e)
        {
            if (Preference.Update(PreferenceName.TimeCardsUseDecimalInsteadOfColon, useDecimalCheckBox.Checked))
            {
                hasChanged = true;
            }
        }

        private void AdjOverBreaksCheckBox_Click(object sender, EventArgs e)
        {
            if (Preference.Update(PreferenceName.TimeCardsMakesAdjustmentsForOverBreaks, adjOverBreaksCheckBox.Checked))
            {
                hasChanged = true;
            }
        }

        private void ShowSecondsCheckBox_Click(object sender, EventArgs e)
        {
            if (Preference.Update(PreferenceName.TimeCardShowSeconds, showSecondsCheckBox.Checked))
            {
                hasChanged = true;
            }
        }

        private void AddRuleButton_Click(object sender, EventArgs e)
        {
            using (var formTimeCardRuleEdit = new FormTimeCardRuleEdit(new TimeCardRule()))
            {
                formTimeCardRuleEdit.ShowDialog(this);

                FillRules();

                hasChanged = true;
            }
        }

        private void DeleteRulesButton_Click(object sender, EventArgs e)
        {
            if (rulesGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select one or more Rules to delete.", 
                    "Time Card Setup", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var result =
                MessageBox.Show(
                    "Are you sure you want to delete all selected Rules?",
                    "Time Card Setup", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);


            if (result == DialogResult.No) return;

            var timeCardRuleIds = rulesGrid.SelectedTags<TimeCardRule>().Select(x => x.Id).ToList();

            TimeCardRule.DeleteMany(timeCardRuleIds);

            CacheManager.Invalidate<TimeCardRule>();

            FillRules();
        }

        private void RulesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formTimeCardRuleEdit = new FormTimeCardRuleEdit((TimeCardRule)rulesGrid.Rows[e.Row].Tag))
            {
                formTimeCardRuleEdit.ShowDialog(this);

                FillRules();

                hasChanged = true;
            }
        }

        private void HideOlderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadPayPeriods();
            if (hideOlderCheckBox.Checked)
            {
                payPeriodsGrid.ScrollToEnd();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (payPeriodsGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select one or more Pay Periods to delete.",
                    "Time Card Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var result =
                MessageBox.Show(
                    "Are you sure you want to delete all selected pay periods?",
                    "Time Card Setup", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;
            
            var selectedPayPeriods = new List<PayPeriod>();
            for (int i = 0; i < payPeriodsGrid.SelectedIndices.Length; i++)
            {
                selectedPayPeriods.Add((PayPeriod)payPeriodsGrid.Rows[payPeriodsGrid.SelectedIndices[i]].Tag);
            }

            if (!IsSafeToDelete(selectedPayPeriods, out var payPeriodsToDelete) || 
                payPeriodsToDelete == null || 
                payPeriodsToDelete.Count == 0)
                return;

            foreach (var payPeriod in selectedPayPeriods)
            {
                PayPeriod.Delete(payPeriod);
            }

            LoadPayPeriods();
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void FormPayPeriods_FormClosing(object sender, FormClosingEventArgs e)
        {
            var errors = TimeCardRule.ValidateOvertimeRules();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(
                    "Fix the following errors:\r\n" + errors,
                    "Time Card Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                e.Cancel = true;
            }

            var adpCompanyCode = adpCompanyCodeTextBox.Text;
            if (adpCompanyCode.Length > 0 && !Regex.IsMatch(adpCompanyCode, "^[a-zA-Z0-9]{2,3}$"))
            {
                MessageBox.Show(
                    "ADP Company Code must be two or three alpha-numeric characters.\r\nFix or clear before continuing.",
                    "Time Card Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                e.Cancel = true;
            }

            if (Preference.Update(PreferenceName.ADPCompanyCode, adpCompanyCode)) hasChanged = true;

            if (hasChanged)
            {
                CacheManager.Invalidate<Employee>();
                CacheManager.Invalidate<Preference>();
                CacheManager.Invalidate<TimeCardRule>();
            }
        }
    }
}
