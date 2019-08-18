using OpenDentBusiness;
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
            amountTextBox.Text = POut.Decimal(Amount);
            amountTextBox.SelectionStart = 0;
            amountTextBox.SelectionLength = amountTextBox.Text.Length;
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            Amount = PIn.Decimal(amountTextBox.Text);

            DialogResult = DialogResult.OK;
        }
    }
}