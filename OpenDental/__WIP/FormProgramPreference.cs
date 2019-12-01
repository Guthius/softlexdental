/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness.Bridges;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormProgramPreference : FormBase
    {
        private readonly BridgePreference programPreference;
        private readonly string value;

        /// <summary>
        /// Gets the value of the preference.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProgramPreference"/> class.
        /// </summary>
        /// <param name="programPreference">The preference to edit.</param>
        /// <param name="value">The current value of the preference.</param>
        public FormProgramPreference(BridgePreference programPreference, string value)
        {
            InitializeComponent();

            this.programPreference = programPreference;
            this.value = Value = value ?? "";
        }

        private void FormProgramProperty_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = programPreference.Description;

            switch (programPreference.Type)
            {
                case BridgePreferenceType.Password:
                    {
                        Encryption.TryDecrypt(value, out var password);

                        valueTextBox.Text = password;
                    }
                    break;

                default:
                    valueTextBox.Text = value;
                    break;
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var newValue = valueTextBox.Text;

            switch (programPreference.Type)
            {
                case BridgePreferenceType.Password:
                    {
                        Encryption.TryEncrypt(newValue, out var password);

                        newValue = password;
                    }
                    break;

                default:
                    break;
            }

            Value = newValue;

            DialogResult = DialogResult.OK;
        }
    }
}
