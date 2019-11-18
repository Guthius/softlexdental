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
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserSetting : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserSetting"/> class.
        /// </summary>
        public FormUserSetting() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserSetting_Load(object sender, EventArgs e)
        {
            suppressMessageCheckBox.Checked = UserPreference.GetBool(Security.CurrentUser.Id, UserPreferenceName.SuppressLogOffMessage);
        }

        /// <summary>
        /// Saves the preferences and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            UserPreference.Update(Security.CurrentUser.Id, UserPreferenceName.SuppressLogOffMessage, suppressMessageCheckBox.Checked);

            DialogResult = DialogResult.OK;
        }
    }
}
