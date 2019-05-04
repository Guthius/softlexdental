using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormTimePick : FormBase
    {
        int hour = 0, minute = 0;
        readonly bool enableDatePicker;
        readonly bool useAmPm = false; // TODO: This is hardcoded for now, but we should make this toggleable in future...

        public DateTime SelectedDateTime
        {
            get => 
                new DateTime(
                    dateTimePicker.Value.Year, 
                    dateTimePicker.Value.Month, 
                    dateTimePicker.Value.Day, 
                    hour, minute, 0);
            set
            {
                dateTimePicker.Value = value.Date;

                hour = value.Hour;
                minute = value.Hour;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormTimePick"/> class.
        /// </summary>
        /// <param name="enableDatePicker">A value indicating whether to enable date selection.</param>
        public FormTimePick(bool enableDatePicker)
        {
            InitializeComponent();

            this.enableDatePicker = enableDatePicker;

            SelectedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormTimePick_Load(object sender, EventArgs e)
        {
            // If the date picker is not enabled, we hide it and move the time picker to its location.
            if (!enableDatePicker)
            {
                dateGroupBox.Visible = false;
                timeGroupBox.Location = dateGroupBox.Location;

                Size = new Size(
                    Width, 
                    Height - dateGroupBox.Height);
            }

            if (useAmPm)
            {
                amRadioButton.Visible = 
                    pmRadioButton.Visible = true;

                if (hour >= 12)
                {
                    pmRadioButton.Checked = true;
                }
                else
                {
                    amRadioButton.Checked = true;
                }
            }

            hourComboBox.Text = (useAmPm && hour > 12 ? hour - 12 : hour).ToString();
            minuteComboBox.Text = minute.ToString();
        }

        /// <summary>
        /// Only allows digits to be entered in the time comboboxes.
        /// </summary>
        void HourComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Validates the hour to ensure it remains within the acceptable range (0-23 or 1-12).
        /// </summary>
        void HourComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(hourComboBox.Text, out int hour))
            {
                if (useAmPm)
                {
                    if (hour < 1) hour = 1;
                    else if (hour > 12)
                    {
                        hour = 12;
                    }
                }
                else 
                {
                    if (hour < 0) hour = 0;
                    else if (hour > 23)
                    {
                        hour = 23;
                    }
                }

                hourComboBox.Text = hour.ToString();
            }
            else
            {
                hourComboBox.Text = this.hour.ToString();
            }
        }

        /// <summary>
        /// Validates the minute to ensure it remains within the acceptable range (0-59).
        /// </summary>
        private void MinuteComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(minuteComboBox.Text, out int minute))
            {
                if (minute < 0) minute = 0;
                else if (minute > 59)
                {
                    minute = 59;
                }
                minuteComboBox.Text = minute.ToString();
            }
        }

        /// <summary>
        /// Validates the entered date and time and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            try
            {
                int hour = int.Parse(hourComboBox.Text);
                if (useAmPm)
                {
                    if (hour < 1 || hour > 12)
                    {
                        throw new Exception("Hour must be between 1-12");
                    }

                    if (amRadioButton.Checked && hour == 12) hour = 0;
                    if (pmRadioButton.Checked)
                    {
                        if (hour < 12)
                        {
                            hour += 12;
                        }
                    }
                }
                else
                {
                    if (hour < 0 || hour > 23)
                    {
                        throw new Exception("Hour must be between 0-23");
                    }
                }

                int minute = int.Parse(minuteComboBox.Text);
                if (minute < 0 || minute > 59)
                {
                    throw new Exception("Minute must be between 0-59");
                }

                this.hour = hour;
                this.minute = minute;

                DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show(
                    Translation.Language.EnterOrSelectAValidDate,
                    Translation.Language.PickTime, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}