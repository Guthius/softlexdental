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
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailInbox : FormBase
    {
        static Dictionary<long, string> patientNamesDict = Patients.GetAllPatientNames();

        readonly string smtpUsername = "";
        long?           searchPatientId;
        string          searchAddress;
        DateTime        searchDateFrom;
        DateTime        searchDateTo;
        string          searchText;
        bool            searchAttachments;

        /// <summary>
        /// Represents a list of messages to display.
        /// </summary>
        class MessageView
        {
            public delegate void MessageViewInitialze(ODGrid grid);
            public delegate IEnumerable<EmailMessage> MessageViewLoad(EmailAddress emailAddress);
            public delegate void MessageViewRefresh(ODGrid grid, IEnumerable<EmailMessage> emailMessages);
            public delegate void MessageViewFill(ODGrid grid, IEnumerable<EmailMessage> emailMessages);
            public delegate void MessageViewInsert(ODGrid grid, EmailMessage emailMessage);

            ODGrid grid;
            IEnumerable<EmailMessage> emailMessages;

            readonly MessageViewInitialze funcInitialize;
            readonly MessageViewLoad      funcLoad;
            readonly MessageViewRefresh   funcRefresh;
            readonly MessageViewFill      funcFill;
            readonly MessageViewInsert    funcInsert;

            /// <summary>
            /// Gets the mail address associated with the view.
            /// </summary>
            public EmailAddress EmailAddress { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="MessageView"/> class.
            /// </summary>
            /// <param name="emailAddress">The email address the view is bound to.</param>
            /// <param name="refreshFunc">The function for refreshing the messages list.</param>
            /// <param name="fillAction">The action to use to fill the grid.</param>
            /// <param name="createRowAction">The action to use to construct a grid row.</param>
            public MessageView(ODGrid grid, EmailAddress emailAddress, 
                MessageViewInitialze initialize,
                MessageViewLoad load,
                MessageViewRefresh refresh,
                MessageViewFill fill,
                MessageViewInsert insert)
            {
                this.grid = grid;

                funcInitialize  = initialize;
                funcLoad        = load;
                funcRefresh     = refresh;
                funcFill        = fill;
                funcInsert      = insert;

                EmailAddress = emailAddress;
            }

            /// <summary>
            /// Activates the view.
            /// </summary>
            public void Activate()
            {
                funcInitialize?.Invoke(grid);

                if (funcLoad != null && funcFill != null)
                {
                    emailMessages = funcLoad(EmailAddress);

                    funcFill(grid, emailMessages);
                }
            }

            /// <summary>
            /// Refreshes the view.
            /// </summary>
            public void Refresh()
            {
                if (funcLoad != null && funcRefresh != null)
                {
                    emailMessages = funcLoad(EmailAddress);

                    funcRefresh(grid, emailMessages);
                }
            }

            /// <summary>
            /// Adds the specified message to the view.
            /// </summary>
            /// <param name="emailMessage">The message to add to the view.</param>
            public void Add(EmailMessage emailMessage) => funcInsert?.Invoke(grid, emailMessage);
        }

        #region Inbox

        void InboxViewInitialize(ODGrid grid)
        {
            grid.BeginUpdate();
            grid.Rows.Clear();
            
            int colReceivedDatePixCount = 140;
            int colMessageTypePixCount = 120;
            int colFromPixCount = 200;
            int colSigPixCount = 40;
            int colPatientPixCount = 140;
            int variableWidth = grid.Width - 10 - colFromPixCount - colReceivedDatePixCount - colMessageTypePixCount - colSigPixCount - colPatientPixCount;

            grid.Columns.Clear();
            grid.Columns.Add(new ODGridColumn(Translation.Language.ColumnFrom, colFromPixCount, HorizontalAlignment.Left));
            grid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSubject, variableWidth, HorizontalAlignment.Left));
            grid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateReceived, colReceivedDatePixCount, HorizontalAlignment.Left));
            grid.Columns.Add(new ODGridColumn(Translation.Language.ColumnMessageType, colMessageTypePixCount, HorizontalAlignment.Left));
            grid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPatient, colPatientPixCount, HorizontalAlignment.Left));

            grid.EndUpdate();
        }

        IEnumerable<EmailMessage> InboxViewLoad(EmailAddress emailAddress) => EmailMessage.GetByEmailAddress(emailAddress, searchDateFrom, searchDateTo, EmailMessageStatus.Received, EmailMessageStatus.Read);

        void InboxViewFill(ODGrid grid, IEnumerable<EmailMessage> emailMessages)
        {
            grid.BeginUpdate();
            grid.Rows.Clear();

            foreach (var emailMessage in emailMessages)
            {
                InboxViewInsert(grid, emailMessage);
            }

            grid.EndUpdate();
        }

        void InboxViewRefresh(ODGrid grid, IEnumerable<EmailMessage> emailMessages)
        {
            var selectedMessageIds = new List<long>();
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                if (grid.Rows[grid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                {
                    selectedMessageIds.Add(emailMessage.Id);
                }
            }

            int sortColumnIndex = grid.SortedByColumnIdx;
            bool sortAscending = grid.SortedIsAscending;

            if (sortColumnIndex == -1) // Default to sorting by Date Received descending.
            {
                sortColumnIndex = 2;
                sortAscending = false;
            }

            InboxViewFill(grid, emailMessages);
           
            grid.SortForced(sortColumnIndex, sortAscending);

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.Rows[i].Tag is EmailMessage emailMessage)
                {
                    if (selectedMessageIds.Contains(emailMessage.Id))
                    {
                        grid.SetSelected(i, true);
                    }
                }
            }
        }

        void InboxViewInsert(ODGrid grid, EmailMessage emailMessage)
        {
            if (!showHiddenCheckBox.Checked && emailMessage.Flags.HasFlag(EmailMessageFlags.Hidden)) return;

            var row = new ODGridRow
            {
                Tag = emailMessage
            };

            row.Cells.Add(new ODGridCell(emailMessage.FromAddress));
            row.Cells.Add(new ODGridCell(emailMessage.Subject));
            row.Cells.Add(new ODGridCell(emailMessage.Date.ToString()));
            row.Cells.Add(new ODGridCell(GetPatientName(emailMessage.PatientId)));

            if (emailMessage.Status == EmailMessageStatus.Received)
            {
                row.Bold = true;
            }

            grid.Rows.Add(row);
        }

        #endregion

        /// <summary>
        /// Gets the currently selected view.
        /// </summary>
        MessageView CurrentView
        {
            get
            {
                if (messageViewTreeView.SelectedNode != null &&
                    messageViewTreeView.SelectedNode.Tag is MessageView messageView)
                {
                    return messageView;
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEmailInbox"/> class.
        /// </summary>
        /// <param name="smtpUsername"></param>
        public FormEmailInbox(string smtpUsername = null)
        {
            InitializeComponent();

            this.smtpUsername = smtpUsername;
        }

        void FormEmailInbox_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            fromTextBox.Text = DateTime.Now.AddDays(-31).ToShortDateString();
            toTextBox.Text = DateTime.Now.ToShortDateString();

            LoadEmailAddresses();

            if (emailAddressComboBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "No email addresses available for current user. Edit email address info in the File menu or Setup menu.",
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CloseButton_Click(this, EventArgs.Empty);

                return;
            }

            // If a email username is passed in, we will try to set the form to that inbox.
            if (!string.IsNullOrWhiteSpace(smtpUsername))
            {
                foreach (TreeNode treeNode in messageViewTreeView.Nodes)
                {
                    if (treeNode.Tag is EmailAddress emailAddress && emailAddress.SmtpUsername.Contains(smtpUsername))
                    {
                        messageViewTreeView.SelectedNode = treeNode;

                        treeNode.Expand();

                        break;
                    }
                }
            }

            Application.DoEvents();

            ReceiveMessages();

            Cursor = Cursors.Default;
        }

        void EmailAddressComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            messageViewTreeView.BeginUpdate();
            messageViewTreeView.Nodes.Clear();

            if (emailAddressComboBox.SelectedItem is EmailAddress emailAddress)
            {
                var treeNode = messageViewTreeView.Nodes.Add("Inbox");

                treeNode.Tag =
                    new MessageView(messageGrid, emailAddress,
                        InboxViewInitialize,
                        InboxViewLoad,
                        InboxViewRefresh,
                        InboxViewFill,
                        InboxViewInsert);

                messageViewTreeView.SelectedNode = treeNode;
            }

            messageViewTreeView.EndUpdate();
        }

        /// <summary>
        /// Activates a message view after it has been selected from the tree view control.
        /// </summary>
        void MessageViewTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (messageViewTreeView.SelectedNode != null &&
                messageViewTreeView.SelectedNode.Tag is MessageView messageView)
            {
                messageView.Activate();
            }
        }

        /// <summary>
        /// Loads the list of mail addresses and populates the navigation tree view control.
        /// </summary>
        void LoadEmailAddresses()
        {
            var emailAddressList = EmailAddress.All();

            // Exclude the default practie mail address.
            emailAddressList.RemoveAll(x => x.Id == Preference.GetLong(PreferenceName.EmailDefaultAddressNum));

            // Remove all the clinic mail addresses and then add back only the ones the user has access to.
            if (Preferences.HasClinicsEnabled)
            {
                var clinicList = Clinics.GetDeepCopy();
                foreach (var clinic in clinicList)
                {
                    emailAddressList.RemoveAll(emailAddress => emailAddress.Id == clinic.EmailAddressNum);
                }

                var clinicListForUser = Clinics.GetForUserod(Security.CurrentUser);
                foreach (var clinic in clinicListForUser)
                {
                    var emailAddress = EmailAddress.GetByClinic(clinic.EmailAddressNum);
                    if (emailAddress == null || emailAddressList.Any(x => x.Id == emailAddress.Id))
                    {
                        continue;
                    }

                    emailAddressList.Add(emailAddress);
                }
            }

            SynchronizeEmailAddresses(emailAddressList);
        }

        /// <summary>
        /// Synchronizes the email addresses combobox with the specified list of email addresses.
        /// </summary>
        /// <param name="emailAddresses"></param>
        void SynchronizeEmailAddresses(IEnumerable<EmailAddress> emailAddresses)
        {
            var existingAddressesDict = new Dictionary<long, EmailAddress>();

            emailAddressComboBox.BeginUpdate();

            foreach (EmailAddress emailAddress in emailAddressComboBox.Items)
            {
                if (!emailAddresses.Any(e => e.Id == emailAddress.Id))
                {
                    emailAddressComboBox.Items.Remove(emailAddress);
                }
                else
                {
                    existingAddressesDict.Add(emailAddress.Id, emailAddress);
                }
            }

            foreach (var emailAddress in emailAddresses)
            {
                if (!existingAddressesDict.TryGetValue(emailAddress.Id, out var existingEmailAddress))
                {
                    emailAddressComboBox.Items.Add(emailAddress);
                }
                else
                {
                    // We only update the username since this is the only thing that is relevant for us, as this is
                    // the name displayed in the combobox.
                    existingEmailAddress.SmtpUsername = emailAddress.SmtpUsername;
                }
            }

            emailAddressComboBox.EndUpdate();

            // If there is no address selected, select the first one...
            if (emailAddressComboBox.SelectedItem == null)
            {
                if (emailAddressComboBox.Items.Count > 0)
                {
                    emailAddressComboBox.SelectedIndex = 0;
                }
            }
            
            emailAddressComboBox.Invalidate(); // TODO: Is this required?
        }

        /// <summary>
        /// Gets new messages from email inbox, as well as older messages from the db. Also fills the grid.
        /// </summary>
        int ReceiveMessages()
        {
            var messageView = CurrentView;
            if (messageView == null)
            {
                return 0;
            }

            messageView.Refresh();
            if (messageView.EmailAddress.SmtpUsername == "" || messageView.EmailAddress.Pop3Server == "")
            {
                return 0;
            }

            int totalEmailMessages = 0;

            try
            {
                foreach (var emailMessage in EmailMessage.Receive(CurrentView.EmailAddress))
                {
                    messageView.Add(emailMessage);

                    totalEmailMessages++;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Error receiving messages: " + exception.Message,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return totalEmailMessages;
        }

        /// <summary>
        /// Gets the name of the patient with the specified ID.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <returns>The name of the patient; or a empty string if no patient with the specified ID exists.</returns>
        static string GetPatientName(long? patientId)
        {
            if (patientId.HasValue)
            {
                lock (patientNamesDict)
                {
                    if (patientNamesDict.TryGetValue(patientId.Value, out var patientName))
                    {
                        return patientName;
                    }

                    patientNamesDict = Patients.GetAllPatientNames();
                    if (patientNamesDict.TryGetValue(patientId.Value, out patientName))
                    {
                        return patientName;
                    }
                }
            }

            return "";
        }

        bool FilterMessage(EmailMessage message)
        {
            if (searchPatientId.HasValue && message.PatientId != searchPatientId)
            {
                return false;
            }

            if (searchAddress.Length > 0)
            {

            }

            if (searchAttachments)
            {
                if (message.Attachments == null || message.Attachments.Count == 0)
                {
                    return false;
                }
            }

            if (searchText.Length > 0)
            {
                if (message.Subject.IndexOf("", StringComparison.InvariantCultureIgnoreCase) == -1 && message.Body.IndexOf("", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        void FillGridSent()
        {
            var selectedMessageIds = new List<long>();
            for (int i = 0; i < messageGrid.SelectedIndices.Length; i++)
            {
                if (messageGrid.Rows[messageGrid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                {
                    selectedMessageIds.Add(emailMessage.Id);
                }
            }

            int sortByColIdx = messageGrid.SortedByColumnIdx;
            bool isSortAsc = messageGrid.SortedIsAscending;
            if (sortByColIdx == -1)
            {
                sortByColIdx = 2;
                isSortAsc = false;
            }

            //calculate column widths
            int colSentToPixCount = 200;
            int colSentDatePixCount = 140;
            int colMessageTypePixCount = 120;
            int colSigPixCount = 40;
            int colPatientPixCount = 140;
            int variableWidth = messageGrid.Width - 10 - colSentToPixCount - colSentDatePixCount - colMessageTypePixCount - colSigPixCount - colPatientPixCount;

            messageGrid.BeginUpdate();
            messageGrid.Columns.Clear();
            messageGrid.Columns.Add(new ODGridColumn("Sent To", colSentToPixCount, HorizontalAlignment.Left));
            messageGrid.Columns.Add(new ODGridColumn("Subject", variableWidth, HorizontalAlignment.Left));
            messageGrid.Columns.Add(new ODGridColumn("Date Sent", colSentDatePixCount, HorizontalAlignment.Left));
            messageGrid.Columns.Add(new ODGridColumn("MsgType", colMessageTypePixCount, HorizontalAlignment.Left));
            messageGrid.Columns.Add(new ODGridColumn("Patient", colPatientPixCount, HorizontalAlignment.Left));

            List<EmailMessage> listEmailsFiltered = new List<EmailMessage>();

            messageGrid.Rows.Clear();
            foreach (var emailMessage in listEmailsFiltered)
            {
                if (!showHiddenCheckBox.Checked && emailMessage.Flags.HasFlag(EmailMessageFlags.Hidden))
                {
                    continue;
                }

                var row = new ODGridRow();
                row.Cells.Add(emailMessage.ToAddress);
                row.Cells.Add(emailMessage.Subject);
                row.Cells.Add(emailMessage.Date.ToString());
                row.Cells.Add(emailMessage.Status.ToString());
                row.Cells.Add(GetPatientName(emailMessage.PatientId));
                row.Tag = emailMessage;

                messageGrid.Rows.Add(row);

            }

            messageGrid.EndUpdate();
            messageGrid.SortForced(sortByColIdx, isSortAsc);

            for (int i = 0; i < messageGrid.Rows.Count; i++)
            {
                if (messageGrid.Rows[i].Tag is EmailMessage emailMessage)
                {
                    if (selectedMessageIds.Contains(emailMessage.Id))
                    {
                        messageGrid.SetSelected(i, true);
                    }
                }
            }
        }

        void FillSearch()
        {
            //string searchBody = textSearchBody.Text;
            //string searchEmail = textSearchEmail.Text;
            //DateTime searchDateFrom = PIn.Date(textDateFrom.Text); //returns MinVal if empty or invalid.
            //DateTime searchDateTo = PIn.Date(textDateTo.Text); //returns MinVal if empty or invalid.
            //List<EmailMessage> listInboxSearched = new List<EmailMessage>();
            //List<EmailMessage> listSentMessagesSearched = new List<EmailMessage>();
            //if (searchBody != "")
            //{
            //    //We have to run a query here to search the database, since our cache only includes the first 50 characters of the body text for preview.
            //    List<EmailMessage> listEmailsSearched = EmailMessage.Find(_searchPatNum, searchEmail, searchDateFrom, searchDateTo, searchBody, checkSearchAttach.Checked);
            //    
            //    //inbox emails
            //    listInboxSearched = listEmailsSearched.Where(x => x.Status.In(EmailMessageStatus.Read, EmailMessageStatus.Received)).ToList();
            //
            //    //sent messages
            //    listSentMessagesSearched = listEmailsSearched.Where(x => x.Status == EmailMessageStatus.Sent).ToList();
            //}
            //else
            //{
            //    //if not filtering by subject/body, then don't look through the db.
            //    //Filter Inbox Emails
            //    foreach (EmailMessage messageCur in _listInboxEmails)
            //    {
            //        if (_searchPatNum != 0)
            //        {
            //            if (messageCur.PatientId != _searchPatNum)
            //            {
            //                continue;
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(searchEmail))
            //        {
            //            if (!messageCur.FromAddress.Contains(searchEmail) && !messageCur.ToAddress.Contains(searchEmail)
            //              && !messageCur.CcAddress.Contains(searchEmail)
            //              && !messageCur.BccAddress.Contains(searchEmail))
            //            {
            //                continue;
            //            }
            //        }
            //        if (checkSearchAttach.Checked)
            //        {
            //            if (messageCur.Attachments.Count < 1)
            //            {
            //                continue;
            //            }
            //        }
            //        if (searchDateFrom != DateTime.MinValue)
            //        {
            //            if (messageCur.Date < searchDateFrom)
            //            {
            //                continue;
            //            }
            //        }
            //        if (searchDateTo != DateTime.MinValue)
            //        {
            //            if (messageCur.Date > searchDateTo)
            //            {
            //                continue;
            //            }
            //        }
            //        listInboxSearched.Add(messageCur); //only happens if all the criteria are filled.
            //    }
            //    //Filter Sent Emails
            //    foreach (EmailMessage messageCur in _listSentEmails)
            //    {
            //        if (_searchPatNum != 0)
            //        {
            //            if (messageCur.PatientId != _searchPatNum)
            //            {
            //                continue;
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(searchEmail))
            //        {
            //            if (!messageCur.FromAddress.Contains(searchEmail) && !messageCur.ToAddress.Contains(searchEmail)
            //              && !messageCur.CcAddress.Contains(searchEmail)
            //              && !messageCur.BccAddress.Contains(searchEmail))
            //            {
            //                continue;
            //            }
            //        }
            //        if (checkSearchAttach.Checked)
            //        {
            //            if (messageCur.Attachments.Count < 1)
            //            {
            //                continue;
            //            }
            //        }
            //        if (searchDateFrom != DateTime.MinValue)
            //        {
            //            if (messageCur.Date.ToShortDateString() != searchDateFrom.ToShortDateString())
            //            {
            //                continue;
            //            }
            //        }
            //        listSentMessagesSearched.Add(messageCur); //only happens if all the criteria are filled.
            //    }
            //}
            //
            //CurrentView.Refresh();
        }

        void MessageGrid_CellClick(object sender, ODGridClickEventArgs e) => SetButtonsEnabled();

        void MessageGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (e.Row != -1 && messageGrid.Rows[e.Row].Tag is EmailMessage emailMessage)
            {
                emailMessage.Status = EmailMessage.MarkAsRead(emailMessage);

                OpenMessageViewer(emailMessage, CurrentView.EmailAddress, false);
            }
        }

        void MarkNotReadButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            for (int i = 0; i < messageGrid.SelectedIndices.Length; i++)
            {
                if (messageGrid.Rows[messageGrid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                {
                    emailMessage.Status = EmailMessage.MarkAsNotRead(emailMessage);

                    messageGrid.Rows[messageGrid.SelectedIndices[i]].Bold = true; // TODO: Invalidate??
                }
            }

            Cursor = Cursors.Default;
        }

        void MarkReadButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            for (int i = 0; i < messageGrid.SelectedIndices.Length; i++)
            {
                if (messageGrid.Rows[messageGrid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                {
                    emailMessage.Status = EmailMessage.MarkAsRead(emailMessage);

                    messageGrid.Rows[messageGrid.SelectedIndices[i]].Bold = false; // TODO: Invalidate??
                }
            }

            Cursor = Cursors.Default;
        }

        void ChangePatientButton_Click(object sender, EventArgs e)
        {
            if (messageGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectAnEmailMessage,
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            using (var formPatientSelect = new FormPatientSelect())
            {
                if (formPatientSelect.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    for (int i = 0; i < messageGrid.SelectedIndices.Length; i++)
                    {
                        if (messageGrid.Rows[messageGrid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                        {
                            EmailMessage.AssignToPatient(emailMessage, formPatientSelect.SelectedPatNum);
                        }
                    }

                    int messagesMovedCount = messageGrid.SelectedIndices.Length;

                    CurrentView?.Refresh();

                    Signalods.SetInvalid(InvalidType.EmailMessages);

                    Cursor = Cursors.Default;

                    MessageBox.Show(
                        string.Format("Succesfully moved {0} messages.", messagesMovedCount), 
                        Translation.Language.Mail, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
        }

        void RefreshButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            ReceiveMessages();

            Cursor = Cursors.Default;
        }

        void SetButtonsEnabled()
        {
            // TODO: Implement me...
        }

        void PickPatientButton_Click(object sender, EventArgs e)
        {
            using (var formPatientSelect = new FormPatientSelect())
            {
                formPatientSelect.ShowDialog();
                if (formPatientSelect.DialogResult == DialogResult.OK)
                {
                    searchPatientId = formPatientSelect.SelectedPatNum;

                    patientTextBox.Text = GetPatientName(formPatientSelect.SelectedPatNum);
                }
            }
        }

        void SearchButton_Click(object sender, EventArgs e)
        {
            searchText = subjectTextBox.Text.Trim();
            searchAddress = emailAddessTextBox.Text.Trim();
            searchAttachments = showAttachmentsOnlyCheckBox.Checked;

            if (subjectTextBox.Text == "" && fromTextBox.Text == "" && toTextBox.Text == "" && emailAddessTextBox.Text == "" && patientTextBox.Text == "" && !showAttachmentsOnlyCheckBox.Checked)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSpecifySearchCriteriaBeforeSearching, 
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            Cursor = Cursors.WaitCursor;

            clearButton.Enabled = true;

            FillSearch();

            CurrentView?.Refresh();

            Cursor = Cursors.Default;
        }

        void ClearButton_Click(object sender, EventArgs e)
        {
            subjectTextBox.Text = "";
            fromTextBox.Text = "";
            toTextBox.Text = "";
            emailAddessTextBox.Text = "";
            patientTextBox.Text = "";
            showAttachmentsOnlyCheckBox.Checked = false;
            clearButton.Enabled = false;

            CurrentView?.Refresh();
        }

        void ComposeButton_Click(object sender, EventArgs e)
        {
            if (Security.IsAuthorized(Permissions.EmailSend))
            {
                var emailMessage = new EmailMessage
                {
                    FromAddress = CurrentView.EmailAddress.GetFrom()
                };

                OpenMessageViewer(emailMessage, CurrentView.EmailAddress, true);
            }
        }

        void ReplyButton_Click(object sender, EventArgs e) => CreateReply();
        
        void ReplyAllButton_Click(object sender, EventArgs e) => CreateReply(true);
        
        void CreateReply(bool replyAll = false)
        {
            if (!Security.IsAuthorized(Permissions.EmailSend)) return;
            
            if (messageGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.YouMustSelectAnEmailBeforeReplying,
                    Translation.Language.Mail,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (messageGrid.Rows[messageGrid.GetSelectedIndex()].Tag is EmailMessage emailMessage)
            {
                emailMessage = EmailMessage.GetById(emailMessage.Id);
                if (emailMessage == null)
                {
                    return;
                }

                OpenMessageViewer(EmailMessage.CreateReply(emailMessage, CurrentView.EmailAddress, replyAll), CurrentView.EmailAddress, true);
            }
        }

        void ForwardButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.EmailSend)) return;
            
            if (messageGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.YouMustSelectAnEmailToForward,
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (messageGrid.Rows[messageGrid.GetSelectedIndex()].Tag is EmailMessage emailMessage)
            {
                emailMessage = EmailMessage.GetById(emailMessage.Id);
                if (emailMessage == null)
                {
                    return;
                }

                OpenMessageViewer(EmailMessage.CreateForward(emailMessage, CurrentView.EmailAddress), CurrentView.EmailAddress, true);
            }
        }
        
        /// <summary>
        /// Opens the editor for the specified email message and address.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="emailAddress"></param>
        /// <param name="allowDelete"></param>
        void OpenMessageViewer(EmailMessage emailMessage, EmailAddress emailAddress, bool allowDelete)
        {
            var formEmailMessageEdit = new FormEmailMessageEdit(emailMessage, emailAddress, allowDelete);
            formEmailMessageEdit.FormClosed += FormEmailMessage_Closed;
            formEmailMessageEdit.Show();
        }

        /// <summary>
        /// If someone else is sending emails on another workstation, this will update this form to reflect that.
        /// </summary>
        public override void OnProcessSignals(List<Signal> listSignals)
        {
            if (listSignals.Exists(x => x.IType == InvalidType.Email))
            {
                Cursor = Cursors.WaitCursor;

                LoadEmailAddresses();
            }

            if (listSignals.Exists(x => x.IType == InvalidType.EmailMessages))
            {
                Cursor = Cursors.WaitCursor;

                CurrentView?.Refresh();
            }

            Cursor = Cursors.Default;
        }

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (messageGrid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select email to delete or hide.", 
                    Translation.Language.Mail, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var result = 
                MessageBox.Show(
                    "Permanently delete or hide selected email(s)?",
                    Translation.Language.Mail, 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;
            
            Cursor = Cursors.WaitCursor;

            for (int i = 0; i < messageGrid.SelectedIndices.Length; i++)
            {
                if (messageGrid.Rows[messageGrid.SelectedIndices[i]].Tag is EmailMessage emailMessage)
                {
                    // If attached to a patient, simply hide the email message instead of deleting it so that it still shows in other parts of the program.
                    if (emailMessage.PatientId != 0)
                    {
                        emailMessage.Flags |= EmailMessageFlags.Hidden;
                        EmailMessage.Update(emailMessage);

                        continue;
                    }

                    EmailMessage.Delete(emailMessage);
                }
            }

            CurrentView.Refresh();

            Signalods.SetInvalid(InvalidType.EmailMessages);

            Cursor = Cursors.Default;
        }

        void FormEmailMessage_Closed(object sender, EventArgs e)
        {
            if (sender is FormEmailMessageEdit formEmailMessageEdit)
            {
                if (formEmailMessageEdit.HasEmailChanged)
                {
                    Cursor = Cursors.WaitCursor;

                    CurrentView?.Refresh();

                    Cursor = Cursors.Default;
                }
            }
        }

        void CloseButton_Click(object sender, EventArgs e) => Close();

        void SetupButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formEmailAddresses = new FormEmailAddresses())
            {
                formEmailAddresses.ShowDialog();

                LoadEmailAddresses();
            }
        }
    }
}