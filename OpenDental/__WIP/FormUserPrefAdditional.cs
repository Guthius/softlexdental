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
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserPrefAdditional : FormBase
    {
        private readonly User user;
        private readonly List<UserPreference> userPreferences;

        /// <summary>
        /// This is a list of providerclinic rows that were given to this form, containing any 
        /// modifications that were made while in FormProvAdditional.
        /// </summary>
        public List<UserPreference> ListUserPrefOut = new List<UserPreference>();

        public FormUserPrefAdditional(List<UserPreference> userPreferences, User user)
        {
            InitializeComponent();

            this.userPreferences = new List<UserPreference>(userPreferences);
            this.user = user;
        }

        private void FormProvAdditional_Load(object sender, EventArgs e) => FillGrid();

        private void FillGrid()
        {
            Cursor = Cursors.WaitCursor;
            userPropertiesGrid.BeginUpdate();
            userPropertiesGrid.Columns.Clear();
            userPropertiesGrid.Columns.Add(new ODGridColumn("Clinic", 120));
            userPropertiesGrid.Columns.Add(new ODGridColumn("DoseSpot User ID", 120, isEditable: true));
            userPropertiesGrid.Rows.Clear();


            var userPrefDefault = userPreferences.Find(
                userPreference => !userPreference.ClinicId.HasValue);

            if (userPrefDefault == null)
            {
                userPrefDefault = UserPreference.GetByKey(user.Id, UserPreferenceName.DoseSpotUserId);

                userPreferences.Add(userPrefDefault);
            }

            var row = new ODGridRow();
            row.Cells.Add("Default");
            row.Cells.Add(userPrefDefault.Value);
            row.Tag = userPrefDefault;
            userPropertiesGrid.Rows.Add(row);

            foreach (var clinic in Clinic.GetByUser(Security.CurrentUser))
            {
                row = new ODGridRow();
                var userPreference = userPreferences.Find(x => x.ClinicId == clinic.Id);
                if (userPreference == null)
                {
                    userPrefDefault = UserPreference.GetByKey(clinic.Id, user.Id, UserPreferenceName.DoseSpotUserId);

                    userPreferences.Add(userPreference);
                }

                row.Cells.Add(clinic.Abbr);
                row.Cells.Add(userPreference.Value);
                row.Tag = userPreference;

                userPropertiesGrid.Rows.Add(row);
            }

            userPropertiesGrid.EndUpdate();

            Cursor = Cursors.Default;
        }

        private void UserPropertiesGrid_CellLeave(object sender, ODGridClickEventArgs e)
        {
            if (userPropertiesGrid.Rows[e.Row].Tag is UserPreference userPreference)
            {
                userPreference.Value = userPropertiesGrid.Rows[e.Row].Cells[e.Column].Text;
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            ListUserPrefOut = new List<UserPreference>();

            foreach (ODGridRow row in userPropertiesGrid.Rows)
            {
                ListUserPrefOut.Add((UserPreference)row.Tag);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
