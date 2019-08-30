/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using System;
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

        void AmountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
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