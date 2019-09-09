using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class EmailPreviewControl : UserControl
    {
        private EmailMessage _emailMessage = null;

        private List<EmailAttachment> _listEmailAttachDisplayed = null;

        bool _isComposing;

        ///<summary>Used when sending to get Clinic.</summary>
        private Patient _patCur = null;

        ///<summary>If the message is an html email with images, then this list contains the raw image mime parts.  The user must give permission before converting these to images, for security purposes.  Gmail also does this with images, for example.</summary>
        private List<Health.Direct.Common.Mime.MimeEntity> _listImageParts = null;
       
        ///<summary>The list of email addresses in email setup. Primarly used for matching From address to internal EmailAddress.</summary>
        private List<EmailAddress> _listEmailAddresses;
        
        ///<summary>Can be null. Should be set externally before showing this control to the user. Otherwise will attempt to match _emailMessage.FromAddress
        ///against an EmailAddress object found in Email Setup.</summary>
        public EmailAddress EmailAddressPreview = null;

        ///<summary>List of recommended emails to be show on email releated textboxes.
        ///Usually history of all email messages regarding a specific inbox/outbox.
        ///These email messages are "light" such that the do not include body text or raw email data.
        ///These messages must be "light" in order to prevent from bloating memory.</summary>
        private List<string> _listHistoricContacts = new List<string>();

        ///<summary>True if the thread has finished filling _listHistoricContacts.</summary>
        private bool _hasSetHistoricContacts;

        ///<summary>List of all HideInFlags, except None.</summary>
        private List<EmailMessageFlags> _listHideInFlags = new List<EmailMessageFlags>();

        ///<summary>used specifically for checking if the message string in the body text has changed
        private bool _hasMessageTextChanged = false;

        public bool IsComposing { get { return _isComposing; } }

        public string Subject { get { return textSubject.Text; } set { textSubject.Text = value; } }

        public string Body { get { return textBodyText.Text; } set { textBodyText.Text = value; } }

        public string FromAddress { get { return textFromAddress.Text; } }

        public string ToAddress { get { return textToAddress.Text; } set { textToAddress.Text = value; } }

        public string CcAddress { get { return textCcAddress.Text; } set { textCcAddress.Text = value; } }

        public string BccAddress { get { return textBccAddress.Text; } set { textBccAddress.Text = value; } }

        public bool HasAttachments { get { return _emailMessage.Attachments.Count > 0; } }

        public long PatNum
        {
            get
            {
                if (_patCur != null)
                {
                    return _patCur.PatNum;
                }
                return 0;
            }
        }

        public long ClinicNum
        {
            get
            {
                if (_patCur != null)
                {
                    return _patCur.ClinicNum;
                }
                return 0;
            }
        }

        public EmailPreviewControl()
        {
            InitializeComponent();

            gridAttachments.ContextMenu = contextMenuAttachments;
        }

        ///<summary>Loads the given emailMessage into the control.
        ///Set listEmailMessages to messages to be considered for the auto complete contacts pop up.  When null will query.</summary>
        public void LoadEmailMessage(EmailMessage emailMessage, List<EmailMessage> listHistoricEmailMessages = null)
        {
            Cursor = Cursors.WaitCursor;
            _emailMessage = emailMessage;
            _patCur = Patients.GetPat(_emailMessage.PatientId.Value);//we could just as easily pass this in.
            if (_emailMessage.Status == EmailMessageStatus.Unknown)
            {//Composing a message

                _emailMessage.UserId = Security.CurUser.UserNum;//UserNum is also updated when sent. Setting here to display when composing.
            }
            else
            {//sent or received (not composing)
             //For all email received or sent types, we disable most of the controls and put the window into a mostly read-only state.
             //There is no reason a user should ever edit a received message.
             //The user can copy the content and send a new email if needed (to mimic forwarding until we add the forwarding feature).
                _isComposing = false;
                textMsgDateTime.Text = _emailMessage.Date.ToString();
                textMsgDateTime.ForeColor = Color.Black;
                gridAttachments.SetAddButtonEnabled(false);
                textFromAddress.ReadOnly = true;
                textToAddress.ReadOnly = true;
                textCcAddress.ReadOnly = true;
                textBccAddress.ReadOnly = true;
                textSubject.ReadOnly = true;
                textBodyText.ReadOnly = true;

                butAccountPicker.Visible = false;
                textFromAddress.Width = textCcAddress.Width;
            }
            textSentOrReceived.Text = _emailMessage.Status.ToString();
            textFromAddress.Text = _emailMessage.FromAddress;
            textToAddress.Text = _emailMessage.ToAddress;
            textCcAddress.Text = _emailMessage.CcAddress;
            textBccAddress.Text = _emailMessage.BccAddress; //if you send an email to yourself, you'll be able to see everyone in the bcc field.
            textSubject.Text = _emailMessage.Subject;
            textBodyText.Visible = true;
            textBodyText.Text = _emailMessage.Body;

            lableUserName.Visible = true;
            textUserName.Visible = true;
            if (_emailMessage.UserId.HasValue)
            {
                textUserName.Text = (Userods.GetName(_emailMessage.UserId.Value));
            }

            FillAttachments();
            if (IsComposing)
            {
                LoadEmailAddresses(ClinicNum);
     
                SetHistoricContacts(listHistoricEmailMessages);
            }
            textBodyText.Select();
            Cursor = Cursors.Default;
        }

        public void LoadEmailAddresses(long clinicNum)
        {
            //emails to include: 
            //1. Default Practice/Clinic
            //2. Me
            //3. All other email addresses not tied to a user
            _listEmailAddresses = new List<EmailAddress>();
            EmailAddress emailAddressDefault = EmailAddress.GetByClinic(clinicNum);
            EmailAddress emailAddressMe = EmailAddress.GetByUser(Security.CurUser.UserNum);
            if (emailAddressDefault != null)
            {
                _listEmailAddresses.Add(emailAddressDefault);
            }
            if (emailAddressMe != null)
            {
                _listEmailAddresses.Add(emailAddressMe);
            }
            foreach (EmailAddress emailCur in EmailAddress.All())
            {
                if ((emailAddressDefault != null && emailCur.SmtpUsername == emailAddressDefault.SmtpUsername)
                  || (emailAddressMe != null && emailCur.SmtpUsername == emailAddressMe.SmtpUsername))
                {
                    continue;
                }
                _listEmailAddresses.Add(emailCur);
            }
        }


        ///<summary>Returns distinct list of email strings to be recommended to user.
        ///Splits all email address fields into a large list of individual addresses into one large distinct list.
        ///When given list is null, will run query.</summary>
        private void SetHistoricContacts(List<EmailMessage> listEmailMessages)
        {
            if (EmailAddressPreview == null)
            {
                //Only null when failed to match from address. If we do not know the from address then we can't load anything useful.
                return;
            }

            ODThread thread = new ODThread(o =>
            {
                var listHistoricContacts = EmailMessage.GetAddresses(listEmailMessages);

                this.InvokeIfRequired(() =>
                {
                    _listHistoricContacts = listHistoricContacts.ToList();
                    _hasSetHistoricContacts = true;
                });
            });
            thread.Name = "SetHistoricContacts";
            thread.Start();
        }

        #region Attachments

        public void FillAttachments()
        {
            _listEmailAttachDisplayed = new List<EmailAttachment>();

            gridAttachments.BeginUpdate();
            gridAttachments.Rows.Clear();
            gridAttachments.Columns.Clear();
            gridAttachments.Columns.Add(new OpenDental.UI.ODGridColumn("", 0));//No name column, since there is only one column.
            for (int i = 0; i < _emailMessage.Attachments.Count; i++)
            {
                OpenDental.UI.ODGridRow row = new UI.ODGridRow();
                row.Cells.Add(_emailMessage.Attachments[i].Description);
                gridAttachments.Rows.Add(row);
                _listEmailAttachDisplayed.Add(_emailMessage.Attachments[i]);
            }
            gridAttachments.EndUpdate();
            if (gridAttachments.Rows.Count > 0)
            {
                gridAttachments.SetSelected(0, true);
            }
        }

        private void contextMenuAttachments_Popup(object sender, EventArgs e)
        {
            menuItemOpen.Enabled = false;
            menuItemRename.Enabled = false;
            menuItemRemove.Enabled = false;
            if (gridAttachments.SelectedIndices.Length > 0)
            {
                menuItemOpen.Enabled = true;
            }
            if (gridAttachments.SelectedIndices.Length > 0 && _isComposing)
            {
                menuItemRename.Enabled = true;
                menuItemRemove.Enabled = true;
            }
        }

        void OpenMenuItem_Click(object sender, EventArgs e) => OpenFile();

        void RenameMenuItem_Click(object sender, EventArgs e)
        {
            using (var inputBox = new InputBox("Filename"))
            {
                var emailAttachment = _listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
                inputBox.textResult.Text = emailAttachment.Description;

                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    emailAttachment.Description = inputBox.textResult.Text;

                    FillAttachments();
                }
            }
        }

        void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            var emailAttachment = _listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
            _emailMessage.Attachments.Remove(emailAttachment);

            FillAttachments();
        }

        private void gridAttachments_MouseDown(object sender, MouseEventArgs e)
        {
            //A right click also needs to select an items so that the context menu will work properly.
            if (e.Button == MouseButtons.Right)
            {
                int clickedIndex = gridAttachments.PointToRow(e.Y);
                if (clickedIndex != -1)
                {
                    gridAttachments.SetSelected(clickedIndex, true);
                }
            }
        }

        private void gridAttachments_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            var emailAttachment = _listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];

            var attachmentPath = Storage.Default.CombinePath(EmailAttachment.GetAttachmentPath(), emailAttachment.FileName);
            try
            {
                if (EhrCCD.IsCcdEmailAttachment(emailAttachment))
                {
                    string attachmentXml = Storage.Default.ReadAllText(attachmentPath);
                    if (EhrCCD.IsCCD(attachmentXml))
                    {
                        Patient patEmail = null;//Will be null for most email messages.
                                                //if(_emailMessage.Status==EmailMessageStatus.ReadDirect || _emailMessage.Status==EmailMessageStatus.ReceivedDirect) {
                                                //	patEmail=_patCur;//Only allow reconcile if received via Direct.
                                                //}
                        string strAlterateFilPathXslCCD = "";
                        //Try to find a corresponding stylesheet. This will only be used in the event that the default stylesheet cannot be loaded from the EHR dll.
                        for (int i = 0; i < _listEmailAttachDisplayed.Count; i++)
                        {
                            if (Path.GetExtension(_listEmailAttachDisplayed[i].FileName).ToLower() == ".xsl")
                            {
                                strAlterateFilPathXslCCD = Storage.Default.CombinePath(EmailAttachment.GetAttachmentPath(), _listEmailAttachDisplayed[i].FileName);
                                break;
                            }
                        }

                        FormEhrSummaryOfCare.DisplayCCD(attachmentXml, patEmail, strAlterateFilPathXslCCD);
                        return;
                    }
                }
                else if (IsORU_R01message(attachmentPath))
                {
                    using (var formEhrLabOrderImport = new FormEhrLabOrderImport())
                    {
                        formEhrLabOrderImport.Hl7LabMessage = Storage.Default.ReadAllText(attachmentPath);
                        formEhrLabOrderImport.ShowDialog();

                        return;
                    }
                }

                Storage.Default.OpenFile(
                    Storage.Default.CombinePath(
                        EmailAttachment.GetAttachmentPath(), emailAttachment.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void gridAttachmentsAdd_Click(object sender, EventArgs e)
        {
            if (!gridAttachments.GetIsAddButtonEnabled())
            {
                MessageBox.Show(
                    "Attachments cannot be modified on historical email.");
                return;
            }

            _emailMessage.Attachments.AddRange(EmailAttachL.PickAttachments(_patCur));

            FillAttachments();
        }

        ///<summary>Attempts to parse message and detects if it is an ORU_R01 HL7 message.  Returns false if it fails, or does not detect message type.</summary>
        private bool IsORU_R01message(string strFilePathAttach)
        {
            if (Path.GetExtension(strFilePathAttach) != "txt")
            {
                return false;
            }
            try
            {
                string[] ArrayMSHFields = Storage.Default.ReadAllText(strFilePathAttach).Split(new string[] { "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries)[0].Split('|');
                if (ArrayMSHFields[8] != "ORU^R01^ORU_R01")
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion Attachments

        #region Body

        public void LoadTemplate(string subject, string bodyText, IEnumerable<EmailAttachment> attachments)
        {
            var appointmentList = Appointments.GetFutureSchedApts(PatNum);

            Appointment nextAppointment = null;
            if (appointmentList.Count > 0)
            {
                nextAppointment = appointmentList[0]; //next sched appt. If none, null.
            }

            Clinic clinic = Clinics.GetClinic(ClinicNum);
            Subject = ReplaceTemplateFields(subject, _patCur, nextAppointment, clinic); ;
            Body = ReplaceTemplateFields(bodyText, _patCur, nextAppointment, clinic);
            _emailMessage.Attachments.AddRange(attachments);
            _hasMessageTextChanged = true;
            FillAttachments();
        }

        ///<summary></summary>
        public static string ReplaceTemplateFields(string templateText, Patient pat, Appointment aptNext, Clinic clinic)
        {
            //patient information
            templateText = Patients.ReplacePatient(templateText, pat);
            //Guarantor Information
            templateText = Patients.ReplaceGuarantor(templateText, pat);
            //Family Information
            templateText = Family.ReplaceFamily(templateText, pat);
            //Next Scheduled Appointment Information
            templateText = Appointments.ReplaceAppointment(templateText, aptNext); //handles null nextApts.
                                                                                   //Currently Logged in User Information
            templateText = FormMessageReplacements.ReplaceUser(templateText, Security.CurUser);
            //Clinic Information
            templateText = Clinics.ReplaceOffice(templateText, clinic);
            //Misc Information
            templateText = FormMessageReplacements.ReplaceMisc(templateText);
            //Referral Information
            templateText = Referrals.ReplaceRefProvider(templateText, pat);
            //Recall Information
            return Recalls.ReplaceRecall(templateText, pat);
        }

        private void textBodyText_KeyDown(object sender, KeyEventArgs e)
        {
            _hasMessageTextChanged = true;
        }

        #endregion Body

        ///<summary>Saves the UI input values into the emailMessage.  Allowed to save message with invalid fields, so no validation here.</summary>
        public void SaveMsg(EmailMessage emailMessage)
        {
            if (_isComposing)
            {
                emailMessage.FromAddress = textFromAddress.Text;
                emailMessage.ToAddress = textToAddress.Text;
                emailMessage.CcAddress = textCcAddress.Text;
                emailMessage.BccAddress = textBccAddress.Text;
                emailMessage.Subject = textSubject.Text;
                emailMessage.Body = textBodyText.Text;
                emailMessage.Date = DateTime.Now;
                emailMessage.Status = _emailMessage.Status;
            }
        }

        private void butAccountPicker_Click(object sender, EventArgs e)
        {
            PickEmailAccount();
        }

        public EmailAddress PickEmailAccount()
        {
            using (var formEmailAddresses = new FormEmailAddresses())
            {
                formEmailAddresses.IsSelectionMode = true;
                if (formEmailAddresses.ShowDialog() == DialogResult.OK)
                {
                    EmailAddress emailAccountSelected = _listEmailAddresses.Find(x => x.Id == formEmailAddresses.EmailAddressId);
                    if (emailAccountSelected != null)
                    {
                        EmailAddressPreview = emailAccountSelected;
                    }
                    else
                    {
                        MessageBox.Show(
                            "Error selecting email account.", 
                            "Mail", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                        return null;
                    }

                    textFromAddress.Text = EmailAddressPreview.GetFrom();
                    if (!_isComposing)
                    {
                        return null;
                    }
                }
                else
                {
                    EmailAddressPreview = null;
                }
                return EmailAddressPreview;
            }
        }

        private void emailAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (char.IsLetterOrDigit((char)e.KeyCode) || e.KeyCode.In(Keys.Enter, Keys.Up, Keys.Down, Keys.Escape, Keys.Back))
            {
                RecommendedEmailHelper(((ODtextBox)sender), e.KeyCode);
            }
        }

        ///<summary>Creates a list box under given textBox filled with filtered list of recommended emails based on textBox.Text values.
        ///Key is used to navigate list indirectly.</summary>
        private void RecommendedEmailHelper(ODtextBox textBox, Keys key)
        {
            if (_hasSetHistoricContacts && _listHistoricContacts.Count == 0)
            {//No recommendations to show.
                return;
            }
            //The passed in textBox's tag points to the grid of options.
            //The created grid's tag will point to the textBox.
            if (textBox.Tag == null)
            {
                textBox.Tag = new ODGrid();
            }
            ODGrid gridContacts = (ODGrid)textBox.Tag;
            //textBox.Text could contain multiple email addresses.
            //We only want to grab the last few characters as the filter string.
            //email@od.com,email2@od.com,emai => "emai" is the filter.
            //When there is no comma, will just use what is currently in the textbox.
            string emailFilter = textBox.Text.ToLower().Split(',').Last();
            if (emailFilter.Length < 2)
            {//Require at least 2 characters for now.
                gridContacts.Hide();//Even if not showing .Hide() won't harm anything.
                textBox.Tag = null;//Reset tag so that initial logic runs again.
                return;
            }
            #region Key navigation and filtering
            switch (key)
            {
                case Keys.Enter://Select currently highlighted recommendation.
                    if (gridContacts.Rows.Count == 0)
                    {
                        return;
                    }
                    CloseAndSetRecommendedContacts(gridContacts, true);
                    return;
                case Keys.Up://Navigate the recommendations from the textBox indirectly.
                    if (gridContacts.Rows.Count == 0)
                    {
                        return;
                    }
                    //gridContacts is multi select. We are navigating 1 row at a time so clear and set the selected index.
                    int index = Math.Max(gridContacts.GetSelectedIndex() - 1, 0);
                    gridContacts.SetSelected(false);
                    gridContacts.SetSelected(new int[] { index }, true);
                    gridContacts.ScrollToIndex(index);
                    break;
                case Keys.Down://Navigate the recommendations from the textBox indirectly.
                    if (gridContacts.Rows.Count == 0)
                    {
                        return;
                    }
                    //gridContacts is multi select. We are navigating 1 row at a time so clear and set the selected index.
                    index = Math.Min(gridContacts.GetSelectedIndex() + 1, gridContacts.Rows.Count - 1);
                    gridContacts.SetSelected(false);
                    gridContacts.SetSelected(new int[] { index }, true);
                    gridContacts.ScrollToIndex(index);
                    break;
                default:
                    #region Filter recommendations
                    List<string> listFilteredContacts;
                    if (_hasSetHistoricContacts)
                    {
                        listFilteredContacts = _listHistoricContacts.FindAll(x => x.ToLower().Contains(emailFilter.ToLower()));
                    }
                    else
                    {//The thread is still filling historic contacts.
                        listFilteredContacts = new List<string> { Lans.g(this, "Loading contacts...") };
                    }
                    if (listFilteredContacts.Count == 0)
                    {
                        gridContacts.Hide();//No options to show so make sure and hide the list box
                        textBox.Tag = null;//Reset tag.
                        return;
                    }
                    listFilteredContacts.Sort();
                    gridContacts.BeginUpdate();
                    if (gridContacts.Columns.Count == 0)
                    {//First time loading.
                        gridContacts.Columns.Add(new ODGridColumn());
                    }
                    gridContacts.Rows.Clear();
                    foreach (string email in listFilteredContacts)
                    {
                        ODGridRow row = new ODGridRow(email);
                        row.Tag = email;
                        gridContacts.Rows.Add(row);
                    }
                    gridContacts.EndUpdate();
                    gridContacts.SetSelected(0, true);//Force a selection.
                    #endregion
                    break;
            }
            #endregion
            if (gridContacts.Tag != null)
            {//Already initialized
                return;
            }
            //When the text box losses focus, we close/hide the grid.
            //TextBox_LostFocus event fires after the EmailAuto_Click event.
            textBox.Leave += TextBox_LostFocus;
            #region Grid Init
            gridContacts.SelectionMode = GridSelectionMode.Multiple;
            gridContacts.MouseClick += EmailAuto_Click;
            gridContacts.Tag = textBox;
            gridContacts.Parent = this;
            gridContacts.BringToFront();
            Point menuPosition = textBox.Location;
            menuPosition.X += 10;
            menuPosition.Y += textBox.Height - 1;
            gridContacts.Location = menuPosition;
            gridContacts.Width = (int)(textBox.Width * 0.75);
            gridContacts.SetSelected(0, true);
            #endregion
            gridContacts.Show();
        }

        ///<summary>Fires after EmailAuto_Click()</summary>
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            ODtextBox textBox = ((ODtextBox)sender);
            textBox.LostFocus -= TextBox_LostFocus;//Stops EventHandler from firing multiple times.
            if (textBox.Tag == null || this.ActiveControl == (ODGrid)textBox.Tag)
            {//The contacts grid handles its own events.
                return;//Prevent from selecting email addresses twice.
            }
            CloseAndSetRecommendedContacts((ODGrid)textBox.Tag, false);
        }

        ///<summary>Fires before TextBox_LostFocus()</summary>
        private void EmailAuto_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right//Let base ODGrid handle right clicks, do not hide.
                || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)
                || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl)
                || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift)
                || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift))
            {
                //ODGrid selection keys.
                //Set focus back to textbox so user can continue to type and navigate.
                ((ODtextBox)((ODGrid)sender).Tag).Focus();
                return;
            }
            CloseAndSetRecommendedContacts((ODGrid)sender);
        }

        ///<summary>Resets tags to null and hides the given grid.
        ///If isSelectionMade is true, will set textBox.Text to selected item.</summary>
        private void CloseAndSetRecommendedContacts(ODGrid grid, bool isSelectionMade = true)
        {
            ODtextBox textBox = ((ODtextBox)grid?.Tag ?? null);
            if (textBox == null)
            {
                //Done for a bug from TextBox_LostFocus where textBox was null, could be cuase by the form closing and controls being disposed?
                return;
            }
            textBox.Tag = null;
            grid.Hide();
            if (isSelectionMade)
            {
                int index = textBox.Text.LastIndexOf(',');//-1 if not found.
                if (index == -1)
                {//The selected email is the first email being placed in our textbox.
                    textBox.Text = string.Join(",", grid.SelectedGridRows.Select(x => ((string)x.Tag)).ToList());
                }
                else
                {//Adding multiple emails.
                    textBox.Text = textBox.Text.Remove(index + 1, textBox.Text.Length - index - 1);//Remove filter characters
                    textBox.Text += string.Join(",", grid.SelectedGridRows.Select(x => ((string)x.Tag)).ToList());//Replace with selected email
                }
            }
            textBox.Focus();//Ensures that auto complete textbox maintains focus after auto complete.
            textBox.SelectionStart = textBox.Text.Length;//Moves cursor to end of the text in the textbox.
        }

    }

    public enum FromAddressMatchResult
    {
        Failed,
        Success,
        Multi
    }
}