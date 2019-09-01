/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDentBusiness;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailTemplateEdit : FormBase
    {
        readonly List<EmailAttachment> emailAttachmentListRemoved = new List<EmailAttachment>();
        List<EmailAttachment> emailAttachmentList;

        public EmailTemplate Template { get; set; }

        public FormEmailTemplateEdit() => InitializeComponent();

        void LoadEmailAttachments()
        {
            attachmentsListView.BeginUpdate();
            attachmentsListView.Clear();

            foreach (var emailAttachment in emailAttachmentList)
            {
                var listViewItem = attachmentsListView.Items.Add(emailAttachment.Description);

                listViewItem.Tag = emailAttachment;
            }
        }

        void FormEmailTemplateEdit_Load(object sender, EventArgs e)
        {
            subjectTextBox.Text = Template.Subject;
            descriptionTextBox.Text = Template.Description;

            emailAttachmentList = 
                Template.IsNew ? 
                    new List<EmailAttachment>() : 
                    EmailAttachment.GetByTemplate(Template.Id);

            bodyTextBox.Text = Template.Body;

            LoadEmailAttachments();
        }

        void SubjectFieldsButton_Click(object sender, EventArgs e)
        {
            using (var formMessageReplacements = new FormMessageReplacements(
                MessageReplaceType.Appointment |
                MessageReplaceType.Office |
                MessageReplaceType.Patient |
                MessageReplaceType.User |
                MessageReplaceType.Misc))
            {
                formMessageReplacements.IsSelectionMode = true;
                if (formMessageReplacements.ShowDialog() == DialogResult.OK)
                {
                    subjectTextBox.SelectedText = formMessageReplacements.Replacement;
                }
            }
        }

        void BodyFieldsButton_Click(object sender, EventArgs e)
        {
            using (var formMessageReplacements = new FormMessageReplacements(
                MessageReplaceType.Appointment |
                MessageReplaceType.Office |
                MessageReplaceType.Patient |
                MessageReplaceType.User |
                MessageReplaceType.Misc))
            {
                formMessageReplacements.IsSelectionMode = true;
                if (formMessageReplacements.ShowDialog() == DialogResult.OK)
                {
                    bodyTextBox.SelectedText = formMessageReplacements.Replacement;
                }
            }
        }

        void AttachmentsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitTestInfo = attachmentsListView.HitTest(e.Location);

            if (hitTestInfo.Item != null)
            {
                hitTestInfo.Item.Selected = true;

                OpenMenuItem_Click(this, EventArgs.Empty);
            }
        }

        void AttachmentsListView_MouseUp(object sender, MouseEventArgs e)
        {
            var hitTestInfo = attachmentsListView.HitTest(e.Location);

            if (hitTestInfo.Item != null)
            {
                hitTestInfo.Item.Selected = true;

                attachmentsContextMenu.Show(attachmentsListView, e.Location);
            }
        }

        void AttachmentButton_Click(object sender, EventArgs e)
        {
            emailAttachmentList.AddRange(EmailAttachL.PickAttachments(null));

            LoadEmailAttachments();
        }

        void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (attachmentsListView.SelectedItems.Count > 0 && 
                attachmentsListView.SelectedItems[0].Tag is EmailAttachment emailAttachment)
            {
                Storage.Default.OpenFile(
                    Storage.Default.CombinePath(
                        EmailAttachment.GetAttachmentPath(),
                        emailAttachment.FileName));
            }
        }

        void RenameMenuItem_Click(object sender, EventArgs e)
        {
            if (attachmentsListView.SelectedItems.Count > 0 &&
                attachmentsListView.SelectedItems[0].Tag is EmailAttachment emailAttachment)
            {
                using (var inputBox = new InputBox("Filename"))
                {
                    inputBox.textResult.Text = emailAttachment.Description;
                    if (inputBox.ShowDialog() == DialogResult.OK)
                    {
                        emailAttachment.Description = inputBox.textResult.Text;

                        LoadEmailAttachments();
                    }
                }
            }
        }

        void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            if (attachmentsListView.SelectedItems.Count > 0 &&
                attachmentsListView.SelectedItems[0].Tag is EmailAttachment emailAttachment)
            {
                emailAttachmentList.Remove(emailAttachment);
                emailAttachmentListRemoved.Add(emailAttachment);

                LoadEmailAttachments();
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (subjectTextBox.Text == "" && bodyTextBox.Text == "")
            {
                MessageBox.Show(
                    Translation.Language.TemplateSubjectAndBodyCannotBeLeftBlank, 
                    Translation.Language.Template,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.Template,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Template.Subject = subjectTextBox.Text;
            Template.Body = bodyTextBox.Text;
            Template.Description = description;

            if (Template.IsNew)
            {
                EmailTemplate.Insert(Template);
            }
            else
            {
                EmailTemplate.Update(Template);
            }

            foreach (var emailAttachment in emailAttachmentListRemoved) EmailAttachment.Delete(emailAttachment.Id);
            foreach (var emailAttachment in emailAttachmentList)
            {
                emailAttachment.EmailTemplateId = Template.Id;
                if (emailAttachment.IsNew)
                {
                    EmailAttachment.Insert(emailAttachment);
                }
                else
                {
                    EmailAttachment.Update(emailAttachment);
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}