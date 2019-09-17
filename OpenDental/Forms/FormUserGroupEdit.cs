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
    public partial class FormUserGroupEdit : FormBase
    {
        private readonly UserGroup userGroup;

        public FormUserGroupEdit(UserGroup userGroup)
        {
            InitializeComponent();

            this.userGroup = userGroup;
        }

        private void FormUserGroupEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = userGroup.Description;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (userGroup.IsNew)
            {
                DialogResult = DialogResult.Cancel;

                return;
            }

            if (Preference.GetLong(PreferenceName.DefaultUserGroup) == userGroup.Id)
            {
                MessageBox.Show(
                    Translation.Language.CannotDeleteDefaultUserGroup,
                    Translation.Language.UserGroup,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                UserGroup.Delete(userGroup);

                CacheManager.Invalidate<UserGroup>();

                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                   exception.Message,
                   Translation.Language.UserGroup,
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.UserGroup, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            userGroup.Description = description;
            try
            {
                if (userGroup.IsNew)
                {
                    UserGroup.Insert(userGroup);
                }
                else
                {
                    UserGroup.Update(userGroup);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Translation.Language.UserGroup,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
