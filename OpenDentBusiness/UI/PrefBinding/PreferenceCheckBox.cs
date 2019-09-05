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
    public partial class PreferenceCheckBox : CheckBox, IPreferenceBinding
    {
        private PreferenceName preference = PreferenceName.NotApplicable;
        private bool invertValue;

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
                    Checked =
                        invertValue ? 
                            !OpenDentBusiness.Preference.GetBool(value) : 
                             OpenDentBusiness.Preference.GetBool(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the preference should be autosaved.
        /// </summary>
        public bool DoAutoSave { get; set; }

        /// <summary>
        /// For all those "EasyHide" prefs etc. where we store the opposite value of what we mean.
        /// </summary>
        public bool Inverted
        {
            get => invertValue;
            set
            {
                invertValue = value;
                if (!DesignMode)
                {
                    Checked = 
                        invertValue ? 
                            !OpenDentBusiness.Preference.GetBool(preference) : 
                             OpenDentBusiness.Preference.GetBool(preference);
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether the preference was updated.
        /// </summary>
        public bool Save() => OpenDentBusiness.Preference.Update(preference, Inverted ? !Checked : Checked);
    }
}
