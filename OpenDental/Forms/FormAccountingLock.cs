using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAccountingLock : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAccountingLock"/> class.
        /// </summary>
        public FormAccountingLock() => InitializeComponent();
        

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAccountingLock_Load(object sender, EventArgs e)
        {
            if (Preferences.GetDate(PrefName.AccountingLockDate).Year > 1880)
            {
                textDate.Text = Preferences.GetDate(PrefName.AccountingLockDate).ToShortDateString();
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