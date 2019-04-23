using OpenDentBusiness;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Summary description for FormBasicTemplate.
    /// </summary>
    public partial class FormRegistrationKey : FormBaseDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormRegistrationKey"/> class.
        /// </summary>
		public FormRegistrationKey() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormRegistrationKey_Load(object sender, EventArgs e)
        {
            string key = PrefC.GetString(PrefName.RegistrationKey);
            if (key != null && key.Length == 16)
            {
                key = key.Substring(0, 4) + "-" + key.Substring(4, 4) + "-" + key.Substring(8, 4) + "-" + key.Substring(12, 4);
            }

            keyTextBox.Text = key;
            agreementRichTextBox.Rtf = Properties.Resources.CDT_Content_End_User_License;
            agreeCheckBox.Checked = false;
        }

        /// <summary>
        /// Only allow characters that are accepted to be entered into the key textbox.
        /// </summary>
        void keyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            else
            {
                if (char.IsLetter(e.KeyChar))
                {
                    e.KeyChar = char.ToUpper(e.KeyChar);
                }
            }
        }

        /// <summary>
        /// Auto inserts dashes whenever needed.
        /// </summary>
        void keyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var cursorPos = keyTextBox.SelectionStart;
            if (Regex.IsMatch(keyTextBox.Text, @"^[A-Z0-9]{5}$"))
            {
                keyTextBox.Text = keyTextBox.Text.Substring(0, 4) + "-" + keyTextBox.Text.Substring(4);
                keyTextBox.SelectionStart = cursorPos + 1;
            }
            else if (Regex.IsMatch(keyTextBox.Text, @"^[A-Z0-9]{4}-[A-Z0-9]{5}$"))
            {
                keyTextBox.Text = keyTextBox.Text.Substring(0, 9) + "-" + keyTextBox.Text.Substring(9);
                keyTextBox.SelectionStart = cursorPos + 1;
            }
            else if (Regex.IsMatch(keyTextBox.Text, @"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{5}$"))
            {
                keyTextBox.Text = keyTextBox.Text.Substring(0, 14) + "-" + keyTextBox.Text.Substring(14);
                keyTextBox.SelectionStart = cursorPos + 1;
            }
        }

        /// <summary>
        /// Check whether the accept button should be enabled.
        /// </summary>
        void keyTextBox_TextChanged(object sender, EventArgs e) => acceptButton.Enabled = agreeCheckBox.Checked || keyTextBox.Text.Length > 0;
        
        /// <summary>
        /// Check whether the accept button should be enabled.
        /// </summary>
        void agreeCheckBox_CheckedChanged(object sender, EventArgs e) => acceptButton.Enabled = agreeCheckBox.Checked || keyTextBox.Text.Length > 0;

        /// <summary>
        /// Checks whether the specified registration key is valid.
        /// </summary>
        /// <param name="registrationKey">The registration key.</param>
        /// <returns>True if the key is correctly formatted; otherwise, false.</returns>
        static bool IsKeyFormatCorrect(string registrationKey)
        {
            if (registrationKey != null &&
                !Regex.IsMatch(registrationKey, @"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$") &&
                !Regex.IsMatch(registrationKey, @"^[A-Z0-9]{16}$"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the specified registration key and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            // Check whether the specified registration key is in the correct format.
            var registrationKey = keyTextBox.Text.Trim();
            if (registrationKey.Length == 0 || !IsKeyFormatCorrect(registrationKey))
            {
                MessageBox.Show(
                    "Invalid registration key format.",
                    "Registration Key",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            // Check whether the specified registration key is valid.
            if (!License.ValidateKey(registrationKey))
            {
                MessageBox.Show(
                    "Invalid registration key.",
                    "Registration Key",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Prefs.UpdateString(PrefName.RegistrationKey, registrationKey);
            DialogResult = DialogResult.OK;
        }
    }
}