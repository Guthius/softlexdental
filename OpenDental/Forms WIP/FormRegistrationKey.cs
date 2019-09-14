using OpenDentBusiness;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormRegistrationKey : FormBaseDialog
    {
        string key;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormRegistrationKey"/> class.
        /// </summary>
		public FormRegistrationKey() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormRegistrationKey_Load(object sender, EventArgs e)
        {
            key = Preference.GetString(PreferenceName.RegistrationKey);
            keyTextBox.Text = License.FormatKey(key);

            agreementRichTextBox.Rtf = Properties.Resources.CDT_Content_End_User_License;
            agreeCheckBox.Checked = false;
        }

        /// <summary>
        /// Only allow characters that are accepted to be entered into the key textbox.
        /// </summary>
        void KeyTextBox_KeyPress(object sender, KeyPressEventArgs e)
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
        void KeyTextBox_KeyUp(object sender, KeyEventArgs e)
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
        void KeyTextBox_TextChanged(object sender, EventArgs e) => acceptButton.Enabled = agreeCheckBox.Checked || keyTextBox.Text.Length > 0;
        
        /// <summary>
        /// Check whether the accept button should be enabled.
        /// </summary>
        void AgreeCheckBox_CheckedChanged(object sender, EventArgs e) => acceptButton.Enabled = agreeCheckBox.Checked || keyTextBox.Text.Length > 0;

        /// <summary>
        /// Checks whether the specified registration key is valid.
        /// </summary>
        /// <param name="registrationKey">The registration key.</param>
        /// <returns>True if the key is correctly formatted; otherwise, false.</returns>
        static bool IsKeyFormatCorrect(string registrationKey)
        {
            if (registrationKey != null && !Regex.IsMatch(registrationKey, @"^[A-Z0-9]{16}$"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cleans up the specified registration key by stripping out all special characters.
        /// </summary>
        /// <param name="registrationKey">The registration key to clean.</param>
        /// <returns></returns>
        static string CleanupKey(string registrationKey)
        {
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = 0; i < registrationKey.Length; i++)
            {
                if (char.IsLetterOrDigit(registrationKey[i]))
                {
                    keyBuilder.Append(registrationKey[i]);
                }
            }
            return keyBuilder.ToString();
        }

        /// <summary>
        /// Validates the specified registration key and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            // Check whether the specified registration key is in the correct format.
            var registrationKey = CleanupKey(keyTextBox.Text);
            if (registrationKey.Length == 0 || !IsKeyFormatCorrect(registrationKey))
            {
                MessageBox.Show(
                    Translation.Language.InvalidRegistrationKeyFormat,
                    Translation.Language.RegistrationKey,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            // Check whether the specified registration key is valid.
            if (!License.ValidateKey(registrationKey))
            {
                MessageBox.Show(
                    Translation.Language.InvalidRegistrationKey,
                    Translation.Language.RegistrationKey,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Preference.Update(PreferenceName.RegistrationKey, registrationKey);
            DialogResult = DialogResult.OK;
        }
    }
}