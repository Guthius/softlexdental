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
using CodeBase;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormGlobalSecurity : FormBase
    {
        public FormGlobalSecurity() => InitializeComponent();

        private void FormGlobalSecurity_Load(object sender, EventArgs e)
        {
            autoLogoffTextBox.Text = Preference.GetInt(PreferenceName.SecurityLogOffAfterMinutes).ToString();
            passwordsMustBeStrongCheckBox.Checked = Preference.GetBool(PreferenceName.PasswordsMustBeStrong);
            passwordsRequireSpecialCharacterCheckBox.Checked = Preference.GetBool(PreferenceName.PasswordsStrongIncludeSpecial);
            forceChangeWeakPasswordsCheckBox.Checked = Preference.GetBool(PreferenceName.PasswordsWeakChangeToStrong);
            timecardSecurityEnabledCheckBox.Checked = Preference.GetBool(PreferenceName.TimecardSecurityEnabled);
            cannotEditOwnTimecardCheckBox.Checked = Preference.GetBool(PreferenceName.TimecardUsersDontEditOwnCard);
            cannotEditOwnTimecardCheckBox.Enabled = timecardSecurityEnabledCheckBox.Checked;
            domainEnabledCheckBox.Checked = Preference.GetBool(PreferenceName.DomainLoginEnabled);
            domainPathTextBox.ReadOnly = !domainEnabledCheckBox.Checked;
            domainPathTextBox.Text = Preference.GetString(PreferenceName.DomainLoginPath);
            logOffWithWindowsCheckBox.Checked = Preference.GetBool(PreferenceName.SecurityLogOffWithWindows);

            if (Preference.GetDate(PreferenceName.BackupReminderLastDateRun).ToShortDateString() == DateTime.MaxValue.AddMonths(-1).ToShortDateString())
            {
                disableBackupReminderCheckBox.Checked = true;
            }

            var userGroups = UserGroup.All().Where(userGroup => !UserGroupPermission.HasPermission(userGroup.Id, Permissions.SecurityAdmin));
            foreach (var userGroup in userGroups)
            {
                int idx = userGroupComboBox.Items.Add(new ODBoxItem<UserGroup>(userGroup.Description, userGroup));
                if (Preference.GetLong(PreferenceName.DefaultUserGroup) == userGroup.Id)
                {
                    userGroupComboBox.SelectedIndex = idx;
                }
            }
        }

        private void TimecardSecurityEnabledCheckBox_Click(object sender, EventArgs e) => 
            cannotEditOwnTimecardCheckBox.Enabled = timecardSecurityEnabledCheckBox.Checked;

        private void PasswordsMustBeStrongCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!passwordsMustBeStrongCheckBox.Checked)
            {
                var result =
                    MessageBox.Show(
                        "Warning. If this box is unchecked, the strong password flag on all users will be reset. " +
                        "If strong passwords are again turned on later, then each user will have to edit their password " +
                        "in order to cause the strong password flag to be set again.",
                        "Global Security Settings", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    passwordsMustBeStrongCheckBox.Checked = true;

                    return;
                }
            }
        }

        private void AutoLogoffTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(autoLogoffTextBox.Text, out var autoLogoffMinutes) || autoLogoffMinutes < 0)
            {
                autoLogoffMinutes = 0;
            }
            autoLogoffTextBox.Text = autoLogoffMinutes.ToString();
        }

        private void DomainEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            domainPathTextBox.ReadOnly = !domainEnabledCheckBox.Checked;

            if (domainEnabledCheckBox.Checked && string.IsNullOrWhiteSpace(domainPathTextBox.Text))
            {
                var result =
                    MessageBox.Show(
                        "Would you like to use your current domain as the domain login path?",
                        "Global Security Settings", 
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var directoryEntry = new DirectoryEntry("LDAP://RootDSE");

                        string defaultNamingContext = directoryEntry.Properties["defaultNamingContext"].Value.ToString();

                        domainPathTextBox.Text = "LDAP://" + defaultNamingContext;
                    }
                    catch (Exception exception)
                    {
                        FormFriendlyException.Show(
                            "Unable to bind to the current domain.",
                            exception);
                    }
                }
            }
        }

        private void DomainPathTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (domainEnabledCheckBox.Checked)
            {
                if (string.IsNullOrWhiteSpace(domainPathTextBox.Text))
                {
                    MessageBox.Show(
                        "Warning. Domain login is enabled, but no path has been entered. " +
                        "If you do not provide a domain path, you will not be able to assign domain logins to users.",
                        "Global Security Settings",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        var directoryEntry = new DirectoryEntry(domainPathTextBox.Text);

                        using (var directorySearcher = new DirectorySearcher(directoryEntry))
                        {
                            // Just do a generic search to verify the object might have users on it.
                            var searchResultCollection = directorySearcher.FindAll(); 
                        }
                    }
                    catch (Exception exception)
                    {
                        FormFriendlyException.Show(
                            "An error occurred while attempting to access the provided domain login path.",
                            exception);
                    }
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(autoLogoffTextBox.Text, out var autoLogoffMinutes) || autoLogoffMinutes < 0)
            {
                MessageBox.Show(
                    "Log off after minutes is invalid.",
                    "Global Security Settings", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            long userGroupId = 0;
            if (userGroupComboBox.SelectedItem is UserGroup userGroup)
            {
                userGroupId = userGroup.Id;
            }

            bool invalidatePreferences =
                Preference.Update(PreferenceName.TimecardSecurityEnabled, timecardSecurityEnabledCheckBox.Checked) |
                Preference.Update(PreferenceName.TimecardUsersDontEditOwnCard, cannotEditOwnTimecardCheckBox.Checked) |
                Preference.Update(PreferenceName.SecurityLogOffWithWindows, logOffWithWindowsCheckBox.Checked) |
                Preference.Update(PreferenceName.PasswordsStrongIncludeSpecial, passwordsRequireSpecialCharacterCheckBox.Checked) |
                Preference.Update(PreferenceName.PasswordsWeakChangeToStrong, forceChangeWeakPasswordsCheckBox.Checked) |
                Preference.Update(PreferenceName.SecurityLogOffAfterMinutes, autoLogoffMinutes) |
                Preference.Update(PreferenceName.DomainLoginPath, domainPathTextBox.Text) |
                Preference.Update(PreferenceName.DomainLoginPath, domainPathTextBox.Text) |
                Preference.Update(PreferenceName.DomainLoginPath, domainPathTextBox.Text) |
                Preference.Update(PreferenceName.DomainLoginEnabled, domainEnabledCheckBox.Checked) |
                Preference.Update(PreferenceName.DefaultUserGroup, userGroupId) |
                Preference.Update(PreferenceName.PasswordsMustBeStrong, passwordsMustBeStrongCheckBox.Checked) |
                Preference.Update(PreferenceName.BackupReminderLastDateRun, disableBackupReminderCheckBox.Checked ? DateTime.MaxValue : DateTime.Today);

            if (!passwordsMustBeStrongCheckBox.Checked)
            {
                // TODO: User.ResetStrongPasswordFlags();
            }

            if (invalidatePreferences) CacheManager.Invalidate<Preference>();

            DialogResult = DialogResult.OK;
        }
    }
}
