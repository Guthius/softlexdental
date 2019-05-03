using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountEdit : FormBase
    {
        public bool IsNew;

        Account account;
        Account accountOld;

        public FormAccountEdit(Account account)
        {
            InitializeComponent();

            this.account = account;
        }

        void FormAccountEdit_Load(object sender, EventArgs e)
        {
            accountOld = account.Clone();

            descriptionTextBox.Text = account.Description;
            bankNumberTextBox.Text = account.BankNumber;
            inactiveCheckBox.Checked = account.Inactive;
            colorButton.BackColor = account.AccountColor;

            for (int i = 0; i < Enum.GetNames(typeof(AccountType)).Length; i++)
            {
                typeListBox.Items.Add(Enum.GetNames(typeof(AccountType))[i]);
                if ((int)account.AcctType == i)
                {
                    typeListBox.SelectedIndex = i;
                }
            }
        }

        void colorButton_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = colorButton.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = colorDialog.Color;
                }
            }
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            try
            {
                Accounts.Delete(account);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Edit Account", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        void acceptButton_Click(object sender, EventArgs e)
        {
            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show(
                    "Description is required.",
                    "Edit Account",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (account.Description != descriptionTextBox.Text)
            {
                var result =
                    MessageBox.Show(
                        "This will update the Splits column for all Transactions attached to this account that have a date after the Accounting Lock Date. Are you sure you want to continue?",
                        "Edit Account", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            account.Description = descriptionTextBox.Text;
            account.AcctType = (AccountType)typeListBox.SelectedIndex;
            account.BankNumber = bankNumberTextBox.Text;
            account.Inactive = inactiveCheckBox.Checked;
            account.AccountColor = colorButton.BackColor;

            if (IsNew)
            {
                Accounts.Insert(account);
            }
            else
            {
                Accounts.Update(account, accountOld);
            }

            DialogResult = DialogResult.OK;
        }
    }
}