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
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormClockEventEdit : FormBase
    {
        private readonly ClockEvent clockEvent;

        private class ClockEventStatusItem
        {
            public ClockEventStatus Status { get; }

            public string Description { get; set; }

            public ClockEventStatusItem(ClockEventStatus clockEventStatus, string description)
            {
                Status = clockEventStatus;
                Description = description;
            }

            public override string ToString() => Description ?? Status.ToString();
        }

        private DateTime StartDate
        {
            get
            {
                if (DateTime.TryParse(date1DisplayedTextBox.Text, out var date1))
                {
                    return date1;
                }

                return DateTime.Parse(date1EnteredTextBox.Text);
            }
        }

        private DateTime? EndDate
        {
            get
            {
                if (DateTime.TryParse(date2DisplayedTextBox.Text, out var date2))
                {
                    return date2;
                }

                if (DateTime.TryParse(date2EnteredTextBox.Text, out var date2Entered))
                {
                    return date2Entered;
                }

                return null;
            }
        }

        private TimeSpan Adjustment
        {
            get
            {
                if (TimeSpan.TryParse(adjustTextBox.Text, out var adjust))
                {
                    return adjust;
                }

                if (TimeSpan.TryParse(adjustAutoTextBox.Text, out var adjustAuto))
                {
                    return adjustAuto;
                }

                return TimeSpan.Zero;
            }
        }

        private TimeSpan Overtime
        {
            get
            {
                if (TimeSpan.TryParse(overtimeTextBox.Text, out var overtime))
                {
                    return overtime;
                }

                if (TimeSpan.TryParse(overtimeAutoTextBox.Text, out var overtimeAuto))
                {
                    return overtimeAuto;
                }

                return TimeSpan.Zero;
            }
        }

        private TimeSpan Rate2
        {
            get
            {
                if (TimeSpan.TryParse(rate2TextBox.Text, out var rate2))
                {
                    return rate2;
                }

                if (TimeSpan.TryParse(rate2AutoTextBox.Text, out var rate2Auto))
                {
                    return rate2Auto;
                }

                return TimeSpan.Zero;
            }
        }

        public FormClockEventEdit(ClockEvent clockEvent)
        {
            InitializeComponent();

            this.clockEvent = clockEvent;
        }

        private void LoadClinics()
        {
            if (Preferences.HasClinicsEnabled)
            {
                var clinics = Clinics.GetForUserod(Security.CurUser);
                if (!Security.CurUser.ClinicRestricted || Security.CurUser.ClinicId == 0)
                {
                    clinics.Insert(0, new Clinic
                    {
                        Abbr = "Headquarters"
                    });
                }

                foreach (var clinic in clinics)
                {
                    clinicComboBox.Items.Add(clinic);
                    if (clinic.ClinicNum == clockEvent.ClinicId)
                    {
                        clinicComboBox.SelectedItem = clinic;
                    }
                }

                if (clinicComboBox.SelectedItem == null && 
                    clinicComboBox.Items.Count > 0)
                {
                    clinicComboBox.SelectedIndex = 0;
                }

                // TODO: What if a user tries to edit a clock event from a clinic they don't have access to??
            }
            else
            {
                clinicLabel.Visible = false;
                clinicComboBox.Visible = false;
            }
        }

        private void CalculateAutoFields()
        {
            var clockedTime = TimeSpan.Zero;

            var endDate = EndDate;
            if (endDate.HasValue)
            {
                clockedTime = endDate.Value - StartDate;

                clockedTimeTextBox.Text = clockedTime.ToString();
            }

            if (clockedTime > TimeSpan.Zero)
            {
                regularTimeTextBox.Text = (clockedTime + Adjustment - Overtime).ToString();

                var totalTime = clockedTime + Adjustment;

                totalHoursTextBox.Text = totalTime.ToString();
                rate1TextBox.Text = (totalTime - Rate2).ToString();
            }
            else
            {
                clockedTimeTextBox.Text = "";
                regularTimeTextBox.Text = "";
                totalHoursTextBox.Text = "";
                rate1TextBox.Text = "";
            }
        }

        private void FormClockEventEdit_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.TimecardDeleteEntry, clockEvent.Date1Entered, true))
            {
                deleteButton.Enabled = false;
                clearButton.Enabled = false;
                date1NowButton.Enabled = false;
                date2NowButton.Enabled = false;
            }

            if (clockEvent.Status == ClockEventStatus.Break)
            {
                timeSpansGroupBox.Enabled = false;
                rate2GroupBox.Enabled = false;
            }

            LoadClinics();

            date1EnteredTextBox.Text = clockEvent.Date1Entered.ToString();
            date1DisplayedTextBox.Text = clockEvent.Date1Displayed.ToString();

            if (clockEvent.Date2Entered.HasValue)
                date2EnteredTextBox.Text = clockEvent.Date2Entered.Value.ToString();

            if (clockEvent.Date2Displayed.HasValue)
                date2DisplayedTextBox.Text = clockEvent.Date2Displayed.Value.ToString();

            var allowBreak = Preference.GetBool(PreferenceName.ClockEventAllowBreak);

            statusComboBox.Items.Clear();
            foreach (ClockEventStatus clockEventStatus in Enum.GetValues(typeof(ClockEventStatus)))
            {
                ClockEventStatusItem item = null;

                if (!allowBreak)
                {
                    if (clockEventStatus == ClockEventStatus.Break) continue;

                    if (clockEventStatus == ClockEventStatus.Lunch)
                    {
                        item = new ClockEventStatusItem(clockEventStatus, "Break");
                    }
                }
                else
                {
                    item = new ClockEventStatusItem(clockEventStatus, clockEventStatus.ToString()); // TODO: Translate...
                }

                if (item != null)
                {
                    statusComboBox.Items.Add(item);
                    if (item.Status == clockEvent.Status)
                    {
                        statusComboBox.SelectedItem = item;
                    }
                }
            }

            // Users were complaining that their employees were altering breaks / "lunch" clock 
            // events which was causing problems. We will disable listStatus for any user that does
            // not have the ability to edit all time cards (even if it is their own time card).
            // This is so that the user is forced to use the buttons within the Manage module which
            // is more predictable.

            if (!Security.IsAuthorized(Permissions.TimecardsEditAll, true)) statusComboBox.Enabled = false;

            adjustTextBox.Text = ClockEvents.Format(clockEvent.AdjustAuto);
            adjustTextBox.Text = clockEvent.Adjust.HasValue ? ClockEvents.Format(clockEvent.Adjust.Value) : "";

            overtimeAutoTextBox.Text = ClockEvents.Format(clockEvent.OvertimeAuto);
            overtimeTextBox.Text = clockEvent.Overtime.HasValue ? ClockEvents.Format(clockEvent.Overtime.Value) : "";

            rate2AutoTextBox.Text = ClockEvents.Format(clockEvent.Rate2Auto);
            rate2TextBox.Text = clockEvent.Rate2.HasValue ? ClockEvents.Format(clockEvent.Rate2.Value) : "";

            CalculateAutoFields();

            noteTextBox.Text = clockEvent.Note;
        }

        private void Date1DisplayedTextBox_Validating(object sender, CancelEventArgs e)
        {
            var date1DisplayedString = date1DisplayedTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(date1DisplayedString))
            {
                if (DateTime.TryParse(date1DisplayedString, out var date1Displayed))
                {
                    date1DisplayedTextBox.Text = date1Displayed.ToString();
                }
                else
                {
                    date1DisplayedTextBox.Text = date1EnteredTextBox.Text;
                }
            }

            CalculateAutoFields();
        }

        private void Date1NowButton_Click(object sender, EventArgs e) => date1DisplayedTextBox.Text = DateTime.Now.ToString();

        private void Date2DisplayedTextBox_Validating(object sender, CancelEventArgs e)
        {
            var date2DisplayedString = date2DisplayedTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(date2DisplayedString))
            {
                if (DateTime.TryParse(date2DisplayedString, out var date2Displayed))
                {
                    date2DisplayedTextBox.Text = date2Displayed.ToString();
                }
                else
                {
                    date2DisplayedTextBox.Text = "";
                }
            }

            CalculateAutoFields();
        }

        private void Date2NowButton_Click(object sender, EventArgs e)
        {
            date2DisplayedTextBox.Text = DateTime.Now.ToString();

            if (date2EnteredTextBox.Text == "")
            {
                date2EnteredTextBox.Text = MiscData.GetNowDateTime().ToString();
            }

            CalculateAutoFields();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            date2DisplayedTextBox.Text = "";
            date2EnteredTextBox.Text = "";

            CalculateAutoFields();
        }

        private void TimeSpan_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox TextBox)
            {
                if (TimeSpan.TryParse(TextBox.Text, out _))
                {
                    CalculateAutoFields();
                }
            }
        }

        private void TimeSpan_Validating(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (TimeSpan.TryParse(textBox.Text, out var timeSpan))
                {
                    textBox.Text = timeSpan.ToString();
                }
                else
                {
                    textBox.Text = "";
                }

                CalculateAutoFields();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "Delete this clock event?",
                    "Clock Event", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            ClockEvent.Delete(clockEvent.Id);

            Employee.UpdateClockStatus(clockEvent.EmployeeId);

            SecurityLogs.MakeLogEntry(Permissions.TimecardDeleteEntry, 0, "Original entry: " + clockEvent.Date1Entered.ToString());

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(date1DisplayedTextBox.Text, out var date1Displayed))
            {
                MessageBox.Show(
                    "Please enter a valid start date and time.",
                    "Clock Event", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                return;
            }
            else if (date1Displayed.Date > DateTime.Today)
            {
                MessageBox.Show(
                    "Start date and time cannot be in the future.",
                    "Clock Event",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var date2Displayed = DateTime.MinValue;
            if (!string.IsNullOrEmpty(date2DisplayedTextBox.Text))
            {
                if (!DateTime.TryParse(date2DisplayedTextBox.Text, out date2Displayed))
                {
                    MessageBox.Show(
                        "Please enter a valid end date and time.",
                        "Clock Event",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (date2Displayed.Date > DateTime.Today)
                {
                    MessageBox.Show(
                        "End date cannot be a future date.",
                        "Clock Event",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
                else if (date2Displayed < date1Displayed)
                {
                    MessageBox.Show(
                        "End date and time cannot be earlier than start date and time.",
                        "Clock Event",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            if (!string.IsNullOrEmpty(date2EnteredTextBox.Text) && date2Displayed == DateTime.MinValue)
            {
                MessageBox.Show(
                    "A end date and time must be entered, or use the Clear button.",
                    "Clock Event",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (clockEvent.Status != ClockEventStatus.Break)
            {
                if (string.IsNullOrEmpty(regularTimeTextBox.Text) && string.IsNullOrEmpty(date2EnteredTextBox.Text))
                {
                    if (Adjustment > TimeSpan.Zero || Overtime > TimeSpan.Zero)
                    {
                        MessageBox.Show(
                            "Cannot enter overtime or adjustments while clocked in.",
                            "Clock Event",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Overtime and adjustments cannot exceed the total time.",
                        "Clock Event",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            if (date2DisplayedTextBox.Text != "" && date2EnteredTextBox.Text == "")
            {
                clockEvent.Date2Entered = MiscData.GetNowDateTime();
            }

            if (Preferences.HasClinicsEnabled)
            {
                if (clinicComboBox.SelectedItem is Clinic clinic)
                {
                    clockEvent.ClinicId = clinic.ClinicNum;
                }
                else
                {
                    clockEvent.ClinicId = null;
                }
            }

            clockEvent.Date1Displayed = date1Displayed;
            clockEvent.Date2Displayed = date2Displayed;
            clockEvent.Overtime = clockEvent.OvertimeAuto == Overtime ? null : (TimeSpan?)Overtime;
            clockEvent.Adjust = clockEvent.AdjustAuto == Adjustment ? null : (TimeSpan?)Adjustment;
            clockEvent.Rate2 = clockEvent.Rate2Auto == Rate2 ? null : (TimeSpan?)Rate2;
            clockEvent.Note = noteTextBox.Text;

            if (statusComboBox.SelectedItem is ClockEventStatusItem statusItem)
            {
                clockEvent.Status = statusItem.Status;
            }

            ClockEvents.Update(clockEvent);

            Employee.UpdateClockStatus(clockEvent.EmployeeId);

            DialogResult = DialogResult.OK;
        }
    }
}
