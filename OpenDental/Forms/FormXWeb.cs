using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.Shared.XWeb;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormXWeb : FormBase
    {
        readonly long patNum;
        CreditCard creditCard;
        XWebTransactionType transactionType;
        readonly bool createPayment;

        public bool LockCardInfo;
        public XWebResponse ResponseResult;

        public FormXWeb(long patNum, CreditCard creditCard, XWebTransactionType transactionType, bool createPayment)
        {
            InitializeComponent();

            this.patNum = patNum;
            this.creditCard = creditCard;
            this.transactionType = transactionType;
            this.createPayment = createPayment;
        }

        void FormXWeb_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.PaymentCreate))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            if (transactionType == XWebTransactionType.CreditReturnTransaction)
            {
                radioReturn.Checked = true;
            }

            if (creditCard != null)
            {
                textCardNumber.Text = creditCard.CCNumberMasked;
                textExpDate.Text = creditCard.CCExpiration.ToString("MMy");
                textZipCode.Text = creditCard.Zip;
            }

            if (LockCardInfo)
            {
                textCardNumber.ReadOnly = true;
                textCardNumber.BackColor = SystemColors.Control;
                textExpDate.ReadOnly = true;
                textExpDate.BackColor = SystemColors.Control;
                textNameOnCard.ReadOnly = true;
                textNameOnCard.BackColor = SystemColors.Control;
                textSecurityCode.ReadOnly = true;
                textSecurityCode.BackColor = SystemColors.Control;
                textZipCode.ReadOnly = true;
                textZipCode.BackColor = SystemColors.Control;
                textAmount.Focus();
            }
        }

        /// <summary>
        /// Returns true if all data is entered correctly, false otherwise.
        /// </summary>
        bool VerifyData()
        {
            int expYear = 0;
            int expMonth = 0;

            if (textCardNumber.Text.Trim().Length < 5)
            {
                MessageBox.Show("Invalid Card Number.");
                return false;
            }

            try
            {//PIn.Int will throw an exception if not a valid format
                if (Regex.IsMatch(textExpDate.Text, @"^\d\d[/\- ]\d\d$"))
                {//08/07 or 08-07 or 08 07
                    expYear = PIn.Int("20" + textExpDate.Text.Substring(3, 2));
                    expMonth = PIn.Int(textExpDate.Text.Substring(0, 2));
                }
                else if (Regex.IsMatch(textExpDate.Text, @"^\d{4}$"))
                {//0807
                    expYear = PIn.Int("20" + textExpDate.Text.Substring(2, 2));
                    expMonth = PIn.Int(textExpDate.Text.Substring(0, 2));
                }
                else
                {
                    MessageBox.Show("Expiration format invalid.");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Expiration format invalid.");
                return false;
            }

            if (creditCard == null)
            {
                //using a new CC and the card number entered contains something other than digits
                if (textCardNumber.Text.Any(x => !char.IsDigit(x)))
                {
                    MessageBox.Show("Invalid card number.");
                    return false;
                }
            }
            else if (creditCard.XChargeToken == "" && Regex.IsMatch(textCardNumber.Text, @"X+[0-9]{4}"))//using a stored CC
            {
                MessageBox.Show("There is no saved XWeb token for this credit card.  The card number and expiration must be re-entered.");
                return false;
            }

            if (!Regex.IsMatch(textAmount.Text, "^[0-9]+$") && !Regex.IsMatch(textAmount.Text, "^[0-9]*\\.[0-9]+$"))
            {
                MessageBox.Show("Invalid amount.");
                return false;
            }

            if (transactionType == XWebTransactionType.CreditVoidTransaction && textRefNumber.Text == "")
            {
                MessageBox.Show("Ref Number required.");
                return false;
            }

            if (textPayNote.Text == "")
            {
                MessageBox.Show("Payment note required.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Processes the selected XWeb transaction. Returns true if the payment was successful, false otherwise.
        /// </summary>
        bool ProcessSelectedTransaction()
        {
            double amount = PIn.Double(textAmount.Text);
            try
            {
                Cursor = Cursors.WaitCursor;

                if (transactionType == XWebTransactionType.CreditReturnTransaction)
                {
                    ResponseResult = 
                        XWebs.ReturnPayment(
                            creditCard.PatNum, 
                            textPayNote.Text, 
                            amount, 
                            creditCard.CreditCardNum, 
                            createPayment);
                }
            }
            catch (ODException ex)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(ex.Message);

                return false;
            }

            Cursor = Cursors.Default;

            return true;
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            transactionType = 
                radioReturn.Checked ? 
                    XWebTransactionType.CreditReturnTransaction : 
                    XWebTransactionType.Undefined;

            if (!VerifyData())
            {
                return;
            }
            if (ProcessSelectedTransaction())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}