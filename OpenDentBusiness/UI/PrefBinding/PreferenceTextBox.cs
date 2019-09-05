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
using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class PreferenceTextBox : TextBox, IPreferenceBinding
    {
        private PreferenceName preference = PreferenceName.NotApplicable;

        /// <summary>
        /// Gets or sets the bound preference.
        /// </summary>
        public PreferenceName Preference
        {
            get => preference; 
            set
            {
                preference = value;

                if (!DesignMode)
                {
                    Text = OpenDentBusiness.Preference.GetString(preference);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the preference should be autosaved.
        /// </summary>
        public bool DoAutoSave { get; set; }

        /// <summary>
        /// Returns a value indicating whether the preference was updated.
        /// </summary>
        public bool Save() => OpenDentBusiness.Preference.Update(preference, Text);
    }
}
