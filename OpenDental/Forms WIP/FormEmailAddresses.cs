/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailAddresses : FormBase
    {
        List<EmailAddress> emailAddressList;

        public bool IsSelectionMode;
        public bool IsChanged;
        public long EmailAddressId;

        public FormEmailAddresses() => InitializeComponent();

        void FormEmailAddresses_Load(object sender, EventArgs e)
        {
            disclaimerCheckBox.Checked = Preference.GetBool(PreferenceName.EmailDisclaimerIsOn);

            if (IsSelectionMode)
            {
                checkIntervalLabel.Visible = false;
                checkIntervalTextBox.Visible = false;
                checkIntervalHelpLabel.Visible = false;
                defaultGroupBox.Visible = false;
                addButton.Visible = false;
                disclaimerCheckBox.Visible = false;
            }
            else
            {
                checkIntervalTextBox.Text = Preference.GetInt(PreferenceName.EmailInboxCheckInterval).ToString();
            }

            LoadEmailAddressList();
        }

        void FormEmailAddresses_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChanged)
            {
                DataValid.SetInvalid(InvalidType.Email);
            }
        }

        void EmailAddressGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsSelectionMode)
            {
                EmailAddressId = emailAddressList[emailAddressGrid.GetSelectedIndex()].Id;

                DialogResult = DialogResult.OK;
            }
            else
            {
                using (var formEmailAddressEdit = new FormEmailAddressEdit(emailAddressList[e.Row], true))
                {
                    formEmailAddressEdit.ShowDialog();
                    if (formEmailAddressEdit.DialogResult == DialogResult.OK)
                    {
                        IsChanged = true;

                        LoadEmailAddressList();
                    }
                }
            }
        }

        void LoadEmailAddressList()
        {
            emailAddressList = EmailAddress.All();

            var isSecurityAdmin = Security.IsAuthorized(Permissions.SecurityAdmin, true);
            
            emailAddressGrid.BeginUpdate();
            emailAddressGrid.Columns.Clear();
            emailAddressGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnUserName, 240));
            emailAddressGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSender, 270));
            emailAddressGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnUser, 135));
            emailAddressGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDefault, 50, HorizontalAlignment.Center));
            emailAddressGrid.Rows.Clear();

            var defaultEmailAddressId = Preference.GetLong(PreferenceName.EmailDefaultAddressNum);
            foreach (var emailAddress in emailAddressList)
            {
                // Users with SecurityAdmin authorization see every e-mail address. Users without SecurityAdmin authorization 
                // will only see the e-mail address assigned to them and public e-mail addresses (e.g. e-mail addresses that have 
                // no user assigned).
                if (isSecurityAdmin || !emailAddress.UserId.HasValue || Security.CurrentUser.Id == emailAddress.UserId)
                {
                    var row = new ODGridRow();

                    row.Cells.Add(emailAddress.SmtpUsername);
                    row.Cells.Add(emailAddress.Sender);
                    row.Cells.Add(emailAddress.UserId.HasValue ? User.GetName(emailAddress.UserId.Value) : "");
                    row.Cells.Add((emailAddress.Id == defaultEmailAddressId) ? "X" : "");
                    row.Tag = emailAddress;

                    emailAddressGrid.Rows.Add(row);
                }
            }
            emailAddressGrid.EndUpdate();
        }

        void DefaultButton_Click(object sender, EventArgs e)
        {
            if (emailAddressGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectARowFirst,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (emailAddressGrid.SelectedTag<EmailAddress>().UserId > 0)
            {
                MessageBox.Show(
                    Translation.Language.UserMailAddressCannotBeSetAsDefault,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (Preference.Update(PreferenceName.EmailDefaultAddressNum, emailAddressList[emailAddressGrid.GetSelectedIndex()].Id))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            LoadEmailAddressList();
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formEmailAddressEdit = new FormEmailAddressEdit(new EmailAddress(), true))
            {
                formEmailAddressEdit.ShowDialog();
                if (formEmailAddressEdit.DialogResult == DialogResult.OK)
                {
                    LoadEmailAddressList();

                    IsChanged = true;
                }
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (IsSelectionMode)
            {
                if (emailAddressGrid.GetSelectedIndex() == -1)
                {
                    MessageBox.Show(
                        Translation.Language.PleaseSelectAMailAddress,
                        Translation.Language.Mail,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }
                EmailAddressId = emailAddressList[emailAddressGrid.GetSelectedIndex()].Id;
            }
            else
            {
                if (!int.TryParse(checkIntervalTextBox.Text, out var checkInterval))
                {
                    MessageBox.Show(
                        Translation.Language.InboxCheckIntervalMustBeBetween1And60,
                        Translation.Language.Mail,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    checkIntervalTextBox.Focus();

                    return;
                }

                if (checkInterval < 1 || checkInterval > 60)
                {
                    MessageBox.Show(
                        Translation.Language.InboxCheckIntervalMustBeBetween1And60,
                        Translation.Language.Mail,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    checkIntervalTextBox.Focus();

                    return;
                }

                if (Preference.Update(PreferenceName.EmailInboxCheckInterval, checkInterval) | 
                    Preference.Update(PreferenceName.EmailDisclaimerIsOn, disclaimerCheckBox.Checked))
                {
                    DataValid.SetInvalid(InvalidType.Prefs);
                }  
            }

            DialogResult = DialogResult.OK;
        }
    }
}