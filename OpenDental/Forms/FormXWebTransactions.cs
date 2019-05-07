using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.Shared.XWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormXWebTransactions : FormBase
    {
        DataTable transactionsTable;
        List<Clinic> clinicsList;

        public FormXWebTransactions() => InitializeComponent();

        void FormXWebTransactions_Load(object sender, EventArgs e)
        {
            if (Preferences.HasClinicsEnabled)
            {
                LoadClinics();
            }
            else
            {
                clinicComboBox.Visible = false;
                clinicLabel.Visible = false;
            }

            dateFromTextBox.Text = DateTime.Today.ToShortDateString();
            dateToTextBox.Text = DateTime.Today.ToShortDateString();
            LoadTransactions();
        }

        void LoadClinics()
        {
            clinicsList = Clinics.GetForUserod(Security.CurUser);
            clinicComboBox.Items.Add("All");
            clinicComboBox.SelectedIndex = 0;

            int offset = 1;
            if (!Security.CurUser.ClinicIsRestricted)
            {
                clinicComboBox.Items.Add("Unassigned");
                offset++;
            }

            clinicsList.ForEach(x => clinicComboBox.Items.Add(x.Abbr));
            clinicComboBox.SelectedIndex = clinicsList.FindIndex(x => x.ClinicNum == Clinics.ClinicNum) + offset;
            if (clinicComboBox.SelectedIndex - offset < 0)
            {
                clinicComboBox.SelectedIndex = 0;
            }
        }

        void LoadTransactions()
        {
            var clinicNumsList = new List<long>();
            if (Preferences.HasClinicsEnabled && clinicComboBox.SelectedIndex != 0) //Not 'All' selected
            {
                if (Security.CurUser.ClinicIsRestricted)
                {
                    clinicNumsList.Add(clinicsList[clinicComboBox.SelectedIndex - 1].ClinicNum);
                }
                else
                {
                    if (clinicComboBox.SelectedIndex == 1)
                    {
                        clinicNumsList.Add(0);
                    }
                    else if (clinicComboBox.SelectedIndex > 1)
                    {
                        clinicNumsList.Add(clinicsList[clinicComboBox.SelectedIndex - 2].ClinicNum);
                    }
                }
            }

            transactionsTable = 
                XWebResponses.GetApprovedTransactions(
                    clinicNumsList,
                    PIn.Date(dateFromTextBox.Text),
                    PIn.Date(dateToTextBox.Text));

            transactionsGrid.BeginUpdate();
            transactionsGrid.Columns.Clear();
            transactionsGrid.Columns.Add(new ODGridColumn("Patient", 120));
            transactionsGrid.Columns.Add(new ODGridColumn("Amount", 60, HorizontalAlignment.Right));
            transactionsGrid.Columns.Add(new ODGridColumn("Date", 80));
            transactionsGrid.Columns.Add(new ODGridColumn("Tran Type", 80));
            transactionsGrid.Columns.Add(new ODGridColumn("Card Number", 140));
            transactionsGrid.Columns.Add(new ODGridColumn("Expiration", 70));
            if (Preferences.HasClinicsEnabled)
            {
                transactionsGrid.Columns.Add(new ODGridColumn("Clinic", 100));
            }
            transactionsGrid.Columns.Add(new ODGridColumn("Transaction ID", 110));
            transactionsGrid.Rows.Clear();

            for (int i = 0; i < transactionsTable.Rows.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(transactionsTable.Rows[i]["Patient"].ToString());
                row.Cells.Add(PIn.Double(transactionsTable.Rows[i]["Amount"].ToString()).ToString("f"));
                row.Cells.Add(PIn.Date(transactionsTable.Rows[i]["DateTUpdate"].ToString()).ToShortDateString());
                XWebTransactionStatus tranStatus = (XWebTransactionStatus)PIn.Int(transactionsTable.Rows[i]["TransactionStatus"].ToString());
                string tranStatusStr;
                switch (tranStatus)
                {
                    case XWebTransactionStatus.DtgPaymentApproved:
                    case XWebTransactionStatus.HpfCompletePaymentApproved:
                    case XWebTransactionStatus.HpfCompletePaymentApprovedPartial:
                        tranStatusStr = "Sale";
                        break;
                    case XWebTransactionStatus.DtgPaymentReturned:
                        tranStatusStr = "Return";
                        break;
                    case XWebTransactionStatus.DtgPaymentVoided:
                        tranStatusStr = "Void";
                        break;
                    default:
                        tranStatusStr = tranStatus.ToString();
                        break;
                }
                row.Cells.Add(tranStatusStr);
                row.Cells.Add(transactionsTable.Rows[i]["MaskedAcctNum"].ToString());
                string expiration = transactionsTable.Rows[i]["ExpDate"].ToString();
                if (expiration.Length > 2)
                {
                    expiration = expiration.Substring(0, 2) + "/" + expiration.Substring(2);
                }
                row.Cells.Add(expiration);
                if (Preferences.HasClinicsEnabled)
                {
                    row.Cells.Add(transactionsTable.Rows[i]["Clinic"].ToString());
                }
                row.Cells.Add(transactionsTable.Rows[i]["TransactionID"].ToString());
                transactionsGrid.Rows.Add(row);
            }
            transactionsGrid.EndUpdate();
        }

        void RefreshButton_Click(object sender, EventArgs e)
        {
            if (dateFromTextBox.Text == "" ||
                dateToTextBox.Text == "" ||
                dateFromTextBox.errorProvider1.GetError(dateFromTextBox) != "" ||
                dateToTextBox.errorProvider1.GetError(dateToTextBox) != "")
            {
                MessageBox.Show(
                    "Please fix data entry errors first.",
                    "XWeb Transactions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            LoadTransactions();
        }

        void TransactionsGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                transactionsGrid.SetSelected(false);
            }
        }

        void TransactionsGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && transactionsGrid.SelectedIndices.Length > 0)
            {
                if (transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["doesPaymentExist"].ToString() == "1")
                {
                    openPaymentMenuItem.Visible = true;
                }
                else
                {
                    openPaymentMenuItem.Visible = false;
                }

                switch ((XWebTransactionStatus)PIn.Int(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["TransactionStatus"].ToString()))
                {
                    case XWebTransactionStatus.DtgPaymentApproved:
                    case XWebTransactionStatus.HpfCompletePaymentApproved:
                    case XWebTransactionStatus.HpfCompletePaymentApprovedPartial:
                    case XWebTransactionStatus.DtgPaymentReturned:
                        voidPaymentMenuItem.Visible = true;
                        processReturnMenuItem.Visible = true;
                        break;
                    case XWebTransactionStatus.DtgPaymentVoided:
                    default:
                        voidPaymentMenuItem.Visible = false;
                        processReturnMenuItem.Visible = false;
                        break;
                }
            }
        }

        void TransactionsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (e.Row < 0 || !Security.IsAuthorized(Permissions.AccountModule))
            {
                return;
            }
            long patNum = PIn.Long(transactionsTable.Rows[e.Row]["PatNum"].ToString());
            GotoModule.GotoAccount(patNum);
        }

        void GoToMenuItem_Click(object sender, EventArgs e)
        {
            if (transactionsGrid.SelectedIndices.Length < 1 || !Security.IsAuthorized(Permissions.AccountModule))
            {
                return;
            }

            long patNum = PIn.Long(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["PatNum"].ToString());

            GotoModule.GotoAccount(patNum);
        }

        void OpenPaymentMenuItem_Click(object sender, EventArgs e)
        {
            if (transactionsGrid.SelectedIndices.Length < 1) return;
            
            var payment = Payments.GetPayment(PIn.Long(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["PaymentNum"].ToString()));
            if (payment == null)
            {
                MessageBox.Show(
                    "This payment no longer exists.",
                    "XWeb Transactions", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Exclamation);

                return;
            }

            var patient = Patients.GetPat(payment.PatNum);
            var family = Patients.GetFamily(patient.PatNum);

            using (var formPayment = new FormPayment(patient, family, payment, false))
            {
                formPayment.ShowDialog(this);
            }

            LoadTransactions();
        }

        void VoidPaymentMenuItem_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.PaymentCreate)) return;

            if (transactionsGrid.SelectedIndices.Length < 1) return;

            var result =
                MessageBox.Show(
                    "Void this payment?",
                    "XWeb Transactions",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            long patNum = PIn.Long(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["PatNum"].ToString());
            long xWebResponseNum = PIn.Long(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["XWebResponseNum"].ToString());

            string paymentNote = 
                "Void XWeb payment made from within Open Dental\r\n" +
                "Amount: " + PIn.Double(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["Amount"].ToString()).ToString("f") + "\r\n" +
                "Transaction ID: " + transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["TransactionID"].ToString() + "\r\n" +
                "Card Number: " + transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["MaskedAcctNum"].ToString() + "\r\n" +
                "Processed: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

            try
            {
                Cursor = Cursors.WaitCursor;

                XWebs.VoidPayment(patNum, paymentNote, xWebResponseNum);

                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Void successful",
                    "XWeb Transactions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadTransactions();
            }
            catch (ODException ex)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    ex.Message,
                    "XWeb Transactions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        void ProcessReturnMenuItem_Click(object sender, EventArgs e)
        {
            if (transactionsGrid.SelectedIndices.Length < 1) return;
            
            long patNum = PIn.Long(transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["PatNum"].ToString());
            string alias = transactionsTable.Rows[transactionsGrid.SelectedIndices[0]]["Alias"].ToString();

            var creditCardList = 
                CreditCards.GetCardsByToken(
                    alias,
                    new List<CreditCardSource> {
                        CreditCardSource.XWeb,
                        CreditCardSource.XWebPortalLogin
                    });

            if (creditCardList.Count == 0)
            {
                MessageBox.Show(
                    "This credit card is no longer stored in the database. Return cannot be processed.",
                    "XWeb Transactions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (creditCardList.Count > 1)
            {
                MessageBox.Show(
                    "There is more than one card in the database with this token. Return cannot be processed due to the risk of charging the incorrect card.",
                    "XWeb Transactions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            using (var formXWeb = new FormXWeb(patNum, creditCardList.FirstOrDefault(), XWebTransactionType.CreditReturnTransaction, createPayment: true))
            {
                formXWeb.LockCardInfo = true;
                if (formXWeb.ShowDialog() == DialogResult.OK)
                {
                    LoadTransactions();
                }
            }
        }

        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}