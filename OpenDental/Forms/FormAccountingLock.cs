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
