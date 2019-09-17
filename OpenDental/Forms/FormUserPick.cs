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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserPick : FormBase
    {
        private readonly bool isMultiSelect;

        public List<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected user.
        /// </summary>
        public long SelectedUserId { get; set; }

        /// <summary>
        /// Gets the ID's of the selected users.
        /// </summary>
        public List<long> SelectedUserIds { get; } = new List<long>();

        /// <summary>
        /// Gets or sets the ID of the suggested user. 
        /// The suggested user will be automatically selected when the dialog is loaded.
        /// </summary>
        public long SuggestedUserId { get; set; }

        /// <summary>
        /// Gets or sets the ID's of the suggested users.
        /// The suggested users will be automatically selected when the dialog is loaded.
        /// </summary>
        public List<long> SuggestedUserIds { get; } = new List<long>();

        /// <summary>
        /// Gets or sets a value indicating whether the dialog is in selection mode.
        /// </summary>
        public bool IsSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the [Show All] button is enabled.
        /// </summary>
        public bool IsShowAllAllowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicatign whether the [None] button is enabled.
        /// </summary>
        public bool IsPickNoneAllowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the [All] button is enabled.
        /// </summary>
        public bool IsPickAllAllowed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserPick"/> class.
        /// </summary>
        /// <param name="isMultiSelect">A value indicating whether selecting mutliple users is allowed.</param>
        public FormUserPick(bool isMultiSelect = false)
        {
            InitializeComponent();

            this.isMultiSelect = isMultiSelect;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        private void FormUserPick_Load(object sender, EventArgs e)
        {
            showButton.Text = Translation.Language.ShowAll;
            if (IsShowAllAllowed && Users != null && Users.Count > 0)
            {
                showButton.Enabled = true;
            }

            allButton.Enabled = IsPickAllAllowed;
            noneButton.Enabled = IsPickNoneAllowed;

            if (isMultiSelect)
            {
                userListBox.SelectionMode = SelectionMode.MultiExtended;
                Text = Translation.Language.PickUsers;
            }

            ShowUsers(Users);
        }

        /// <summary>
        /// Populates the users list box with the users from the specified list.
        /// </summary>
        private void ShowUsers(List<User> users)
        {
            if (Users == null) return;

            users.ForEach(user => userListBox.Items.Add(user));

            if (isMultiSelect)
            {
                foreach (long userId in SuggestedUserIds)
                {
                    int index = users.FindIndex(user => user.Id == userId);

                    userListBox.SetSelected(index, true);
                }
            }
            else
            {
                userListBox.SelectedIndex = users.FindIndex(user => user.Id == SuggestedUserId);
            }
        }

        /// <summary>
        /// Selects a user when the user double clicks on a user in the list.
        /// </summary>
        void UserListBox_DoubleClick(object sender, EventArgs e)
        {
            if (userListBox.SelectedIndex == -1)  return;

            AcceptButton_Click(sender, e);
        }

        /// <summary>
        /// Toggles between showing all users and filtered users.
        /// </summary>
        private void Showbutton_Click(object sender, EventArgs e)
        {
            SelectedUserId = 0;

            if (Text == Translation.Language.ShowAll)
            {
                Text = Translation.Language.ShowFiltered;
                ShowUsers(User.All());
            }
            else
            {
                Text = Translation.Language.ShowAll;
                ShowUsers(Users);
            }
        }

        /// <summary>
        /// Select all users and close the form.
        /// </summary>
        private void AllButton_Click(object sender, EventArgs e)
        {
            SelectedUserId = -1;

            SelectedUserIds.Clear();
            SelectedUserIds.AddRange(Users.Select(user => user.Id));

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the form without selecting a user.
        /// </summary>
        private void NoneButton_Click(object sender, EventArgs e)
        {
            SelectedUserId = -1;
            SelectedUserIds.Clear();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (userListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleasePickAUserFirst,
                    Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (!IsSelectionMode && !Security.IsAuthorized(Permissions.TaskEdit, true))
            {
                if (userListBox.SelectedItem is User user && user.TaskListId.HasValue)
                {
                    MessageBox.Show(
                        Translation.Language.PleaseSelectAUserThatDoesNotHaveAnInbox,
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            foreach (User user in userListBox.SelectedItems)
            {
                SelectedUserIds.Add(user.Id);
            }
            SelectedUserId = SelectedUserIds[0];

            DialogResult = DialogResult.OK;
        }
    }
}