using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailMessageEdit : FormBase
    {
        bool templatesChanged;
        private EmailMessage _emailMessage;
        private bool _isDeleteAllowed = true;
        public bool HasEmailChanged;
        private List<EmailMessage> _listAllEmailMessages = null;
        private List<EmailAutograph> _listEmailAutographs;
        private List<EmailTemplate> _listEmailTemplates;

        ///<summary>isDeleteAllowed defines whether the email is able to be deleted when a patient is attached. 
        ///emailAddress corresponds to the account in Email Setup that will be used to send the email.
        ///Currently, emails that are "Deleted" from the inbox are actually just hidden if they have a patient attached.</summary>
        public FormEmailMessageEdit(EmailMessage emailMessage, EmailAddress emailAddress = null, bool isDeleteAllowed = true, params List<EmailMessage>[] listAllEmailMessages)
        {
            InitializeComponent();// Required for Windows Form Designer support

            _isDeleteAllowed = isDeleteAllowed;
            _emailMessage = emailMessage;


            emailPreview.EmailAddressPreview = emailAddress;
            List<List<EmailMessage>> listAllHistoric = listAllEmailMessages.ToList().FindAll(x => x != null);
            if (listAllHistoric.Count > 0)
            {
                _listAllEmailMessages = listAllEmailMessages.SelectMany(x => x).Where(x => x != null).ToList();
            }
        }

        void FormEmailMessageEdit_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup, true))
            {
                templateAddButton.Enabled = false;
                templateDeleteButton.Enabled = false;
            }

            Cursor = Cursors.WaitCursor;

            RefreshAll();

            // TODO: Set the default autograph...

            SetDefaultAutograph();

            EmailSaveEvent.Fired += EmailSaveEvent_Fired;

            Cursor = Cursors.Default;
        }

        private void EmailSaveEvent_Fired(ODEventArgs e)
        {
            if (e.EventType != ODEventType.EmailSave)
            {
                return;
            }

            SaveMessage();
        }

        private void RefreshAll()
        {
            emailPreview.LoadEmailMessage(_emailMessage, _listAllEmailMessages);

            if (!emailPreview.IsComposing)
            {
                draftSplitContainer.Panel1Collapsed = true;

                sendButton.Visible = false;//not allowed to send again.
                saveButton.Visible = false;//not allowed to save changes.
                                        //When opening an email from FormEmailInbox, the email status will change to read automatically,
                                        //and changing the text on the cancel button helps convey that to the user.
                cancelButton.Text = "Close";
                editTextButton.Visible = false;
                editHtmlButton.Visible = false;
            }
            LoadTemplates();
            FillAutographs();


            //For all email received types, we disable most of the controls and put the form into a mostly read-only state.
            if (_emailMessage.Status == EmailMessageStatus.Received ||
                _emailMessage.Status == EmailMessageStatus.Read)
            {
                sendButton.Visible = true;
                sendButton.Text = "Reply";
            }
        }

        #region Templates

        void LoadTemplates()
        {
            foreach (var emailTemplate in EmailTemplate.All())
            {
                templatesListBox.Items.Add(emailTemplate);
            }
        }

        void TemplatesListBox_DoubleClick(object sender, EventArgs e) => TemplateEditButton_Click(this, EventArgs.Empty);

        void TemplateAddButton_Click(object sender, EventArgs e)
        {
            using (var formEmailTemplateEdit = new FormEmailTemplateEdit())
            {
                formEmailTemplateEdit.Template = new EmailTemplate();
                if (formEmailTemplateEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                CacheManager.Invalidate<EmailTemplate>();

                templatesChanged = true;

                templatesListBox.Items.Add(formEmailTemplateEdit.Template);
                templatesListBox.SelectedItem = formEmailTemplateEdit.Template;
            }
        }

        void TemplateEditButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            if (templatesListBox.SelectedItem is EmailTemplate emailTemplate)
            {
                using (var formEmailTemplateEdit = new FormEmailTemplateEdit())
                {
                    formEmailTemplateEdit.Template = emailTemplate;
                    if (formEmailTemplateEdit.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    CacheManager.Invalidate<EmailTemplate>();

                    templatesChanged = true;

                    LoadTemplates();
                }
            }
        }

        void TemplateDeleteButton_Click(object sender, EventArgs e)
        {
            if (templatesListBox.SelectedItem is EmailTemplate emailTemplate)
            {
                var result =
                    MessageBox.Show(
                        "Delete e-mail template?", 
                        Translation.Language.Mail, 
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    EmailTemplate.Delete(emailTemplate.Id);

                    CacheManager.Invalidate<EmailTemplate>();

                    templatesChanged = true;

                    LoadTemplates();
                }
            }
            else
            {
                MessageBox.Show(
                    "Please select an item first.",
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        void TemplateInsertButton_Click(object sender, EventArgs e)
        {
            if (templatesListBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (emailPreview.Body != "" || emailPreview.Subject != "" || emailPreview.HasAttachments)
            {
                var result =
                    MessageBox.Show(
                        "Replace existing e-mail text with text from the template? Existing attachments will not be deleted.",
                        Translation.Language.Mail,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.No) return;
            }

            if (templatesListBox.SelectedItem is EmailTemplate emailTemplate)
            {
                var emailAttachmentList = EmailAttachment.GetByTemplate(emailTemplate.Id).Select(emailAttachment => new EmailAttachment
                {
                    Description = emailAttachment.Description,
                    FileName = emailAttachment.FileName
                });

                emailPreview.LoadTemplate(emailTemplate.Subject, emailTemplate.Body, emailAttachmentList);
            }
        }

        #endregion




        #region Autographs

        /// <summary>Fills the autographs picklist.</summary>
        private void FillAutographs()
        {
            listAutographs.Items.Clear();
            _listEmailAutographs = EmailAutograph.All();
            for (int i = 0; i < _listEmailAutographs.Count; i++)
            {
                listAutographs.Items.Add(_listEmailAutographs[i].Description);
            }
        }

        ///<summary>Sets the default autograph that shows in the message body. 
        ///The default autograph is determined to be the first autograph with an email that matches the email address of the sender.</summary>
        private void SetDefaultAutograph()
        {
            if (!emailPreview.IsComposing || !_emailMessage.IsNew)
            {
                return;
            }
            EmailAddress emailOutgoing = null;

            string emailUserName = EmailMessage.GetAddressSimple(emailOutgoing.SmtpUsername);
            string emailSender = EmailMessage.GetAddressSimple(emailOutgoing.Sender);
            string autographEmail;
            for (int i = 0; i < _listEmailAutographs.Count; i++)
            {
                // TODO: Fix...
                //autographEmail = EmailMessage.GetAddressSimple(_listEmailAutographs[i].EmailAddress.Trim());
                ////Use Contains() because an autograph can theoretically have multiple email addresses associated with it.
                //if (
                //    (!string.IsNullOrWhiteSpace(emailUserName) && autographEmail.Contains(emailUserName)) || 
                //    (!string.IsNullOrWhiteSpace(emailSender) && autographEmail.Contains(emailSender)))
                //{
                //    InsertAutograph(_listEmailAutographs[i]);
                //    break;
                //}
            }
        }

        ///<summary>Currently just appends an autograph to the bottom of the email message.  When the functionality to reply to emails is implemented, 
        ///this will need to be modified so that it inserts the autograph text at the bottom of the new message being composed, but above the message
        ///history.</summary>
        private void InsertAutograph(EmailAutograph emailAutograph)
        {
            emailPreview.Body += "\r\n\r\n" + emailAutograph.Autograph;
        }

        private void listAutographs_DoubleClick(object sender, EventArgs e)
        { //edit an autograph
            if (listAutographs.SelectedIndex == -1)
            {
                return;
            }
            FormEmailAutographEdit FormEAE = new FormEmailAutographEdit(_listEmailAutographs[listAutographs.SelectedIndex]);
            FormEAE.ShowDialog();
            if (FormEAE.DialogResult == DialogResult.OK)
            {

                CacheManager.Invalidate<EmailAutograph>();
                FillAutographs();
            }
        }

        void AddAutographButton_Click(object sender, EventArgs e)
        {
            var emailAutograph = new EmailAutograph();

            using (var formEmailAutographEdit = new FormEmailAutographEdit(emailAutograph))
            {
                if (formEmailAutographEdit.ShowDialog() == DialogResult.OK)
                {
                    CacheManager.Invalidate<EmailAutograph>();

                    FillAutographs();
                }
            }
        }

        void InsertAutographButton_Click(object sender, EventArgs e)
        {
            if (listAutographs.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an autograph before inserting.");

                return;
            }

            InsertAutograph(_listEmailAutographs[listAutographs.SelectedIndex]);
        }

        void DeleteAutographButton_Click(object sender, EventArgs e)
        {
            if (listAutographs.SelectedItem is EmailAutograph emailAutograph)
            {
                var result =
                    MessageBox.Show(
                        "Delete autograph?",
                        Translation.Language.Mail,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                EmailAutograph.Delete(emailAutograph.Id);

                CacheManager.Invalidate<EmailAutograph>();

                listAutographs.Items.Remove(emailAutograph);
            }
            else
            {
                MessageBox.Show(
                    "Please select an item first.",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

        }

        void EditAutographButton_Click(object sender, EventArgs e)
        {
            if (listAutographs.SelectedItem is EmailAutograph emailAutograph)
            {
                using (var formEmailAutographEdit = new FormEmailAutographEdit(emailAutograph))
                {
                    if (formEmailAutographEdit.ShowDialog() == DialogResult.OK)
                    {
                        CacheManager.Invalidate<EmailAutograph>();

                        listAutographs.Invalidate(
                            listAutographs.GetItemRectangle(
                                listAutographs.SelectedIndex));
                    }
                }
            }
        }

        #endregion

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_emailMessage.IsNew)
            {
                DialogResult = DialogResult.Cancel;

                Close();
            }
            else
            {
                if (_emailMessage.PatientId.HasValue && !_isDeleteAllowed)
                {
                    var result =
                        MessageBox.Show(
                            "Hide this email from the inbox?", 
                            "Mail", 
                            MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question, 
                            MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        _emailMessage.Flags |= EmailMessageFlags.Hidden;
                        EmailMessage.Update(_emailMessage);

                        HasEmailChanged = true;

                        DialogResult = DialogResult.OK;

                        Close();
                    }
                }
                else
                {
                    var result = 
                        MessageBox.Show(
                            "Delete this email?", 
                            "Mail", 
                            MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question, 
                            MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        EmailMessage.Delete(_emailMessage);

                        HasEmailChanged = true;

                        DialogResult = DialogResult.OK;

                        Close();
                    }
                }
            }
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            SaveMessage();

            DialogResult = DialogResult.OK;

            Close();
        }

        void SaveMessage()
        {
            _emailMessage.Body = emailPreview.Body;

            emailPreview.SaveMsg(_emailMessage);
            if (_emailMessage.IsNew)
            {
                EmailMessage.Insert(_emailMessage);
            }
            else
            {
                EmailMessage.Update(_emailMessage);
            }
            HasEmailChanged = true;
        }

        private void butEditHtml_Click(object sender, EventArgs e)
        {
        }

        ///<summary>Gets the outgoing email account (EmailAddress object) from emailPreview. Prompts user when there are problems matching the textbox 
        ///displaying the sending address with an account in Email Setup.
        ///Returns null if failed to match, or if matched to multiple and user canceled out of selection window.</summary>
        private EmailAddress GetOutgoingEmailForSending()
        {
            EmailAddress emailAddress = null;
            //FromAddressMatchResult result = emailPreview.TryGetFromEmailAddress(out emailAddress);
            //switch (result)
            //{
            //    case FromAddressMatchResult.Failed:
            //        MessageBox.Show(Lan.g(this, "No email account found in Email Setup for") + ": " + emailPreview.FromAddress);
            //        break;
            //    case FromAddressMatchResult.Success:
            //        //emailAddress set succesfully
            //        break;
            //    case FromAddressMatchResult.Multi:
            //        if (MessageBox.Show(Lan.g(this, "Multiple email accounts matching") + " " + emailPreview.FromAddress + "\r\n"
            //            + Lan.g(this, "Send using") + ":\r\n"
            //            + Lan.g(this, "Username") + ": " + emailAddress.SmtpUsername + "\r\n"
            //            + Lan.g(this, "Sending Address") + ": " + emailAddress.GetFrom() + "?", "Email Address", MessageBoxButtons.YesNo)
            //            == DialogResult.No)
            //        {
            //            emailAddress = emailPreview.PickEmailAccount();
            //        }
            //        else
            //        {
            //            //emailAddress set to first matched emailAddress in email setup (isChooseFirstOnDuplicate).
            //        }
            //        break;
            //}
            return emailAddress;
        }

        void SendButton_Click(object sender, EventArgs e)
        {
            // If this was a message we received, create a reply...
            if (_emailMessage.Status == EmailMessageStatus.Received || _emailMessage.Status == EmailMessageStatus.Read)
            {
                if (!Security.IsAuthorized(Permissions.EmailSend)) return;

                using (var formEmailMessageEdit = new FormEmailMessageEdit(EmailMessage.CreateReply(_emailMessage, emailPreview.EmailAddressPreview), emailPreview.EmailAddressPreview, true, _listAllEmailMessages))
                {
                    if (formEmailMessageEdit.ShowDialog() == DialogResult.OK)
                    {
                        HasEmailChanged = true;

                        SaveMessage();

                        DialogResult = DialogResult.OK;

                        Close();
                    }
                }
                return;
            }

            var fromAddress = emailPreview.FromAddress;
            if (fromAddress.Length == 0)
            {
                MessageBox.Show(
                    "Please enter a sender address.",
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (emailPreview.ToAddress == "" && emailPreview.CcAddress == "" && emailPreview.BccAddress == "")
            {
                MessageBox.Show(
                    "Please enter at least one recipient.",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            //if (EhrCCD.HasCcdEmailAttachment(_emailMessage))
            //{
            //    MsgBox.Show(this, "The email has a summary of care attachment which may contain sensitive patient data.  Use the Direct Message button instead.");
            //    return;
            //}

            var emailAddress = GetOutgoingEmailForSending();
            if (emailAddress == null)
            {
                return;
            }

            if (emailAddress.SmtpServer == "")
            {
                MessageBox.Show(
                    "The email address in email setup must have an SMTP server.",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            Cursor = Cursors.WaitCursor;

            SaveMessage();

            try
            {
                EmailMessage.Send(emailAddress, _emailMessage);

                _emailMessage.Status = EmailMessageStatus.Sent;
                EmailMessage.Update(_emailMessage);

                MessageBox.Show(
                    "Sent",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Default;

                string message = "Failed to send email.r\n\r\nError message from the email client was:\r\n  " + exception.Message;

                using (var msgBoxCopyPaste = new MsgBoxCopyPaste(message))
                {
                    msgBoxCopyPaste.ShowDialog();
                }

                return;
            }

            Cursor = Cursors.Default;

            DialogResult = DialogResult.OK;

            Close();
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            if (!emailPreview.IsComposing) //Use clicked the 'Close' button.  This is a 'read' email, so only changeable property is HideInFlags.
            {
                SaveMessage();

                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }

        void FormEmailMessageEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (templatesChanged)
            {
                DataValid.SetInvalid(InvalidType.Email);
            }

            EmailSaveEvent.Fired -= EmailSaveEvent_Fired;
            if (HasEmailChanged)
            {
                Signalods.SetInvalid(InvalidType.EmailMessages);
                return;
            }
        }
    }
}