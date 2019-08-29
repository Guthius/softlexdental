/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using CodeBase;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailAddressEdit : FormBase
    {
        readonly EmailAddress emailAddress;

        public FormEmailAddressEdit(EmailAddress emailAddress, bool isOpenedFromEmailSetup = false)
        {
            InitializeComponent();

            this.emailAddress = emailAddress;

            var defaultEmailAddressId = Preference.GetLong(PreferenceName.EmailDefaultAddressNum);

            if (isOpenedFromEmailSetup && Security.IsAuthorized(Permissions.SecurityAdmin, true) && (emailAddress.IsNew || this.emailAddress.Id != defaultEmailAddressId))
            {
                pickUserButton.Visible = true;
            }
        }

        void FormEmailAddressEdit_Load(object sender, EventArgs e)
        {
            if (emailAddress != null)
            {
                smtpServerTextBox.Text = emailAddress.SmtpServer;
                usernameTextBox.Text = emailAddress.SmtpUsername;

                if (!string.IsNullOrEmpty(emailAddress.SmtpPassword))
                {
                    passwordTextBox.Text = MiscUtils.Decrypt(emailAddress.SmtpPassword);
                }

                smtpPortTextBox.Text = emailAddress.SmtpPort.ToString();
                useSslCheckBox.Checked = emailAddress.UseSsl;
                senderTextBox.Text = emailAddress.Sender;
                pop3ServerTextBox.Text = emailAddress.Pop3Server;
                pop3PortTextBox.Text = emailAddress.Pop3Port.ToString();

                var defaultEmailAddressId = Preference.GetLong(PreferenceName.EmailDefaultAddressNum);

                if (emailAddress.IsNew || emailAddress.Id != defaultEmailAddressId)
                {
                    if (emailAddress.UserId.HasValue)
                    {
                        var user = Userods.GetUser(emailAddress.UserId.Value);

                        userTextBox.Tag = user;
                        userTextBox.Text = user?.UserName;
                    }
                }
                else
                {
                    userGroupBox.Visible = false;
                }
            }
        }

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (emailAddress.IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            if (emailAddress.Id == Preference.GetLong(PreferenceName.EmailDefaultAddressNum))
            {
                MessageBox.Show(
                    Translation.Language.CannotDeleteDefaultMailAddress,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var clinic = Clinics.GetFirstOrDefault(x => x.EmailAddressNum == emailAddress.Id); // TODO: Add a Clinic.GetByEmailAddress method...
            if (clinic != null)
            {
                MessageBox.Show(
                    string.Format(Translation.Language.CannotDeleteMailAddressInUseByClinic, clinic.Description),
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);


                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.Mail, 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            EmailAddress.Delete(emailAddress.Id);

            DialogResult = DialogResult.OK;
        }

        void PickUserButton_Click(object sender, EventArgs e)
        {
            using (var formUserPick = new FormUserPick())
            {
                formUserPick.SuggestedUserNum = ((User)userTextBox.Tag)?.UserNum ?? 0;

                if (formUserPick.ShowDialog() == DialogResult.OK)
                {
                    var user = Userods.GetUser(formUserPick.SelectedUserNum);
                    if (user.UserNum == (((User)userTextBox.Tag)?.UserNum ?? 0))
                    {
                        return;
                    }

                    var emailAddress = EmailAddress.GetByUser(user.UserNum);
                    if (emailAddress != null)
                    {
                        MessageBox.Show(
                            string.Format(Translation.Language.MailAddressAlreadyExistsForUser, user.UserName),
                            Translation.Language.Mail,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    userTextBox.Tag = user;
                    userTextBox.Text = user.UserName;
                }
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(smtpPortTextBox.Text, out var port))
            {
                MessageBox.Show(
                    Translation.Language.InvalidOutgoingPortNumber,
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (!int.TryParse(pop3PortTextBox.Text, out var incomingPort))
            {
                MessageBox.Show(
                    Translation.Language.InvalidIncomingPortNumber,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }


            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterAValidMailAddress,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            emailAddress.UserId = ((User)userTextBox.Tag)?.UserNum;
            emailAddress.SmtpServer = smtpServerTextBox.Text.Trim();
            emailAddress.SmtpUsername = usernameTextBox.Text;
            emailAddress.SmtpPassword = MiscUtils.Encrypt(passwordTextBox.Text);
            emailAddress.SmtpPort = port;
            emailAddress.UseSsl = useSslCheckBox.Checked;
            emailAddress.Sender = senderTextBox.Text.Trim();
            emailAddress.Pop3Server = pop3ServerTextBox.Text.Trim();
            emailAddress.Pop3Port = incomingPort;

            if (emailAddress.Id == 0)
            {
                EmailAddress.Insert(emailAddress);
            }
            else
            {
                EmailAddress.Update(emailAddress);
            }

            DialogResult = DialogResult.OK;
        }
    }
}