/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using CodeBase;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAging : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAging"/> class.
        /// </summary>
        public FormAging() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAging_Load(object sender, EventArgs e)
        {
            var dateLastAging = Preference.GetDate(PreferenceName.DateLastAging);
            if (dateLastAging.Year < 1880)
            {
                lastDateTextBox.Text = "";
            }
            else
            {
                lastDateTextBox.Text = dateLastAging.ToShortDateString();
            }

            if (Preference.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily))
            {
                if (dateLastAging < DateTime.Today.AddDays(-15))
                {
                    calculateDateTextBox.Text = dateLastAging.AddMonths(1).ToShortDateString();
                }
                else
                {
                    calculateDateTextBox.Text = dateLastAging.ToShortDateString();
                }
            }
            else
            {
                calculateDateTextBox.Text = DateTime.Today.ToShortDateString();
                if (Preference.GetBool(PreferenceName.AgingIsEnterprise)) // Enterprise aging requires daily not monthly calc
                {
                    calculateDateTextBox.ReadOnly = true;
                    calculateDateTextBox.BackColor = SystemColors.Control;
                }
            }
        }

        /// <summary>
        /// Runs enterprise aging.
        /// </summary>
        /// <param name="calculateDate"></param>
        /// <returns>True if aging ran succesfully; otherwise, false.</returns>
        bool RunAgingEnterprise(DateTime calculateDate)
        {
            var dateLastAging = Preference.GetDate(PreferenceName.DateLastAging);
            if (dateLastAging.Date == calculateDate.Date)
            {
                var result =
                    MessageBox.Show(
                        string.Format(
                            "Aging has already been calculated for {0} and does not normally need to run more than once per day.\r\n\r\nRun anyway?",
                            calculateDate.ToShortDateString()),
                        "Calculate Aging", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information);

                if (result == DialogResult.No) return false;
            }

            // Refresh prefs because AgingBeginDateTime is very time sensitive
            Preference.Refresh();

            var agingBeginDateTime = Preference.GetDateTime(PreferenceName.AgingBeginDateTime);
            if (agingBeginDateTime > DateTime.MinValue)
            {
                MessageBox.Show(
                    string.Format(
                        "You cannot run aging until it has finished the current calculations which began on {0}.\r\n" +
                        "If you believe the current aging process has finished, a user with SecurityAdmin permission " +
                        "can manually clear the date and time by going to Setup | Miscellaneous and pressing the 'Clear' button.",
                        agingBeginDateTime.ToString()),
                    "Calculate Aging", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return false;
            }

            SecurityLogs.MakeLogEntry(Permissions.AgingRan, 0, "Aging Ran - Aging Form");

            Preference.Update(PreferenceName.AgingBeginDateTime, MiscData.GetNowDateTime());
            Signalods.SetInvalid(InvalidType.Prefs); // Signal a cache refresh so other computers will have the updated pref as quickly as possible

            Cursor = Cursors.WaitCursor;
            {
                var result = true;

                ODProgress.ShowAction(
                    () =>
                    {
                        Ledgers.ComputeAging(0, calculateDate);

                        Preference.Update(PreferenceName.DateLastAging, calculateDate);
                    },
                    startingMessage: string.Format("Calculating enterprise aging for all patients as of {0}...", calculateDate.ToShortDateString()),
                    actionException: ex =>
                    {
                        Ledgers.AgingExceptionHandler(ex, this);
                        result = false;
                    }
                );

                Cursor = Cursors.Default;

                Preference.Update(PreferenceName.AgingBeginDateTime, ""); // Clear lock on pref whether aging was successful or not
                Signalods.SetInvalid(InvalidType.Prefs);

                return result;
            }
        }

        /// <summary>
        /// Runs aging and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(calculateDateTextBox.Text, out var calculateDate))
            {
                MessageBox.Show(
                    "Please enter a valid date.",
                    "Calculate Aging",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                calculateDateTextBox.Focus();

                return;
            }

            if (Preference.GetBool(PreferenceName.AgingIsEnterprise))
            {
                // If this is true, calculateDate has to be DateTime.Today and aging calculated daily not monthly.
                if (!RunAgingEnterprise(calculateDate))
                {
                    return;
                }
            }
            else
            {
                bool result = true;

                SecurityLogs.MakeLogEntry(Permissions.AgingRan, 0, "Aging Ran - Aging Form");

                Cursor = Cursors.WaitCursor;
                {
                    ODProgress.ShowAction(() => Ledgers.ComputeAging(0, calculateDate),
                        startingMessage: string.Format("Calculating aging for all patients as of {0}...", calculateDate.ToShortDateString()),
                        actionException: ex =>
                        {
                            Ledgers.AgingExceptionHandler(ex, this);
                            result = false;
                        }
                    );
                }
                Cursor = Cursors.Default;

                if (!result)
                {
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                if (Preference.Update(PreferenceName.DateLastAging, calculateDate))
                {
                    DataValid.SetInvalid(InvalidType.Prefs);
                }
            }

            MessageBox.Show(
                "Aging Complete.",
                "Calculate Aging", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}