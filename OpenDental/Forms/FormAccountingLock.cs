using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountingLock : FormBase
    {
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAccountingLock_Load(object sender, EventArgs e)
        {
            if (PrefC.GetDate(PrefName.AccountingLockDate).Year > 1880)
            {
                textDate.Text = PrefC.GetDate(PrefName.AccountingLockDate).ToShortDateString();
            }
        }

        /// <summary>
        /// Updates the lock date and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, System.EventArgs e)
        {
            if (textDate.errorProvider1.GetError(textDate) != "")
            {
                MessageBox.Show(
                    "Please fix error first.",
                    "Lock Accounting", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                textDate.Focus();

                return;
            }

            if (Prefs.UpdateString(PrefName.AccountingLockDate, POut.Date(PIn.Date(textDate.Text), false)))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            DialogResult = DialogResult.OK;
        }
    }
}