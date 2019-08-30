/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountingLock : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAccountingLock"/> class.
        /// </summary>
        public FormAccountingLock() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAccountingLock_Load(object sender, EventArgs e)
        {
            if (Preference.GetDate(PreferenceName.AccountingLockDate).Year > 1880)
            {
                dateTextBox.Text = Preference.GetDate(PreferenceName.AccountingLockDate).ToShortDateString();
            }
        }

        /// <summary>
        /// Check whether the user has entered a valid date.
        /// </summary>
        void DateTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (DateTime.TryParse(dateTextBox.Text, out var dateTime))
            {
                dateTextBox.Text = dateTime.ToShortDateString();
            }
            else
            {
                dateTextBox.Text = "";
            }
        }

        /// <summary>
        /// Updates the lock date and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(dateTextBox.Text, out var dateTime))
            {
                MessageBox.Show(
                    "Please enter a valid date.",
                    "Lock Accounting",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                dateTextBox.Focus();

                return;
            }

            if (Preference.Update(PreferenceName.AccountingLockDate, dateTime))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            DialogResult = DialogResult.OK;
        }
    }
}