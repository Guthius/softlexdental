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
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPayPeriodEdit : FormBase
    {
        private readonly PayPeriod payPeriod;

        public FormPayPeriodEdit(PayPeriod payPeriod)
        {
            InitializeComponent();

            this.payPeriod = payPeriod;
        }

        private void FormPayPeriodEdit_Load(object sender, EventArgs e)
        {
            if (payPeriod.DateStart.Year > 1880)
                dateStartTextBox.Text = payPeriod.DateStart.ToShortDateString();

            if (payPeriod.DateEnd.Year > 1880)
                dateEndTextBox.Text = payPeriod.DateEnd.ToShortDateString();

            if (payPeriod.DatePaycheck.Year > 1880)
                datePaycheckTextBox.Text = payPeriod.DatePaycheck.ToShortDateString();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (payPeriod.IsNew)
            {
                DialogResult = DialogResult.Cancel;

                return;
            }

            PayPeriod.Delete(payPeriod);

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var dateStartText = dateStartTextBox.Text.Trim();
            var dateEndText = dateEndTextBox.Text.Trim();

            if (dateStartText.Length == 0 || dateEndText.Length == 0)
            {
                MessageBox.Show(
                    "Start and end dates are required.",
                    "Pay Period",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!DateTime.TryParse(dateStartText, out var dateStart) ||
                !DateTime.TryParse(dateEndText, out var dateEnd) ||
                !DateTime.TryParse(datePaycheckTextBox.Text, out var datePaycheck))
            {
                MessageBox.Show(
                    "Please fix data entry errors first.",
                    "Pay Period", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            payPeriod.DateStart = dateStart;
            payPeriod.DateEnd = dateEnd;
            payPeriod.DatePaycheck = datePaycheck;

            CacheManager.Invalidate<PayPeriod>();

            if (PayPeriod.AreAnyOverlapping(PayPeriod.All(), new List<PayPeriod>() { payPeriod }))
            {
                MessageBox.Show(
                    "This pay period overlaps with existing pay periods. Please fix this pay period first.",
                    "Pay Period",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
