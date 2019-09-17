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
using System.ComponentModel;
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
            dateStartTextBox.Text = payPeriod.DateStart.ToShortDateString();
            dateEndTextBox.Text = payPeriod.DateEnd.ToShortDateString();

            if (payPeriod.DatePaycheck.HasValue)
            {
                datePaycheckTextBox.Text = payPeriod.DatePaycheck.Value.ToShortDateString();
            }
        }

        private void ValidateDateTime(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (DateTime.TryParse(textBox.Text, out var dateTime))
                {
                    textBox.Text = dateTime.ToShortDateString();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void DatePaycheckTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (DateTime.TryParse(datePaycheckTextBox.Text, out var paycheckDateTime))
            {
                datePaycheckTextBox.Text = paycheckDateTime.ToShortDateString();
            }
            else
            {
                datePaycheckTextBox.Text = "";
            }
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
            var datePaycheckText = datePaycheckTextBox.Text.Trim();

            if (dateStartText.Length == 0 || dateEndText.Length == 0)
            {
                MessageBox.Show(
                     Translation.Language.StartAndEndDatesRequired,
                     Translation.Language.PayPeriod,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!DateTime.TryParse(dateStartText, out var dateStart) ||
                !DateTime.TryParse(dateEndText, out var dateEnd))
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.PayPeriod, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (DateTime.TryParse(datePaycheckText, out var datePaycheck))
            {
                if (datePaycheck <= dateEnd)
                {
                    MessageBox.Show(
                        Translation.Language.PaycheckDateCannotBeOnOrBeforeEndDate,
                        Translation.Language.PayPeriod, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
                payPeriod.DatePaycheck = datePaycheck;
            }
            else
            {
                payPeriod.DatePaycheck = null;
            }

            payPeriod.DateStart = dateStart;
            payPeriod.DateEnd = dateEnd;

            CacheManager.Invalidate<PayPeriod>();

            if (PayPeriod.AreAnyOverlapping(PayPeriod.All(), new List<PayPeriod>() { payPeriod }))
            {
                MessageBox.Show(
                    Translation.Language.PayPeriodIsOverlapping,
                    Translation.Language.PayPeriod,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
