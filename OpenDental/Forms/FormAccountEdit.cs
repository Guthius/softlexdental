using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountEdit : FormBase
    {
        readonly Account account;

        public FormAccountEdit(Account account)
        {
            InitializeComponent();

            this.account = account;
        }

        void FormAccountEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = account.Description;
            bankNumberTextBox.Text = account.BankNumber;
            inactiveCheckBox.Checked = account.Inactive;
            colorButton.BackColor = account.Color;

            for (int i = 0; i < Enum.GetNames(typeof(AccountType)).Length; i++)
            {
                typeListBox.Items.Add(Enum.GetNames(typeof(AccountType))[i]);
                if ((int)account.Type == i)
                {
                    typeListBox.SelectedIndex = i;
                }
            }
        }

        void ColorButton_Click(object sender, EventArgs e)
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

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (account.Id == 0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            try
            {
                Account.Delete(account);
            }
            catch (DataException ex)
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

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show(
                    "A description is required.",
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
            account.Type = (AccountType)typeListBox.SelectedIndex;
            account.BankNumber = bankNumberTextBox.Text;
            account.Inactive = inactiveCheckBox.Checked;
            account.Color = colorButton.BackColor;

            if (account.Id == 0)
            {
                Account.Insert(account);
            }
            else
            {
                Account.Update(account);
            }

            DialogResult = DialogResult.OK;
        }
    }
}