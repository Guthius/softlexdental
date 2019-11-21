using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormBackupReminder : FormBase
    {
        public FormBackupReminder() => InitializeComponent();

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!checkA1.Checked && !checkA2.Checked && !checkA3.Checked && !checkA4.Checked)
            {
                MessageBox.Show(
                    "You are not allowed to continue using this program unless you are making daily backups.", 
                    "Backup", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (!checkB1.Checked && !checkB2.Checked)
            {
                MessageBox.Show(
                    "You are not allowed to continue using this program unless you have proof that your backups are good.",
                    "Backup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (!checkC1.Checked && !checkC2.Checked)
            {
                MessageBox.Show(
                    "You are not allowed to continue using this program unless you have a long-term strategy.",
                    "Backup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void FormBackupReminder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                var result =
                    MessageBox.Show(
                        "The program will now close.", 
                        "Backup", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Information);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
