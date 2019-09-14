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
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAmountEdit : FormBase
    {
        readonly string text;

        public decimal Amount;

        public FormAmountEdit(string text)
        {
            InitializeComponent();

            this.text = text;
        }

        void FormAmountEdit_Load(object sender, EventArgs e)
        {
            amountLabel.Text = text;
            amountTextBox.Text = Amount.ToString();
            amountTextBox.SelectionStart = 0;
            amountTextBox.SelectionLength = amountTextBox.Text.Length;
        }

        void AmountTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (decimal.TryParse(amountTextBox.Text, out var result))
            {
                Amount = result;
            }
            amountTextBox.Text = Amount.ToString("N2");
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}