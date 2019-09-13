using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormCommItem : FormBase
    {
        bool _sigChanged;
        bool _isStartingUp;

        bool userOdPrefClearNote;
        bool userOdPrefEndDate;
        bool userOdPrefUpdateDateTimeNewPat;
        List<Definition> commlogTypeDefsList;

        public Commlog CommlogCur;

        /// <summary>
        /// Gets or sets a value indicating whether the commlog item is new.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is persistent.
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCommItem"/> class.
        /// </summary>
        /// <param name="commlog"></param>
        public FormCommItem(Commlog commlog)
        {
            InitializeComponent();

            CommlogCur = commlog;
        }

        /// <summary>
        /// Refreshes the preferences of the current user.
        /// </summary>
        void RefreshUserPreferences()
        {
            if (Security.CurrentUser == null || Security.CurrentUser.Id < 1) return;
            
            userOdPrefClearNote             = UserPreference.GetBool(Security.CurrentUser.Id, UserPreferenceName.CommlogPersistClearNote);
            userOdPrefEndDate               = UserPreference.GetBool(Security.CurrentUser.Id, UserPreferenceName.CommlogPersistClearEndDate);
            userOdPrefUpdateDateTimeNewPat  = UserPreference.GetBool(Security.CurrentUser.Id, UserPreferenceName.CommlogPersistUpdateDateTimeWithNewPatient);
        }

        /// <summary>
        /// Gets the name of the patient with the specified <paramref name="patNum"/>.
        /// </summary>
        /// <param name="patNum">The patient number.</param>
        /// <returns>The name of the patient.</returns>
        string GetPatientName(long patNum) => Patients.GetLim(patNum).GetNameFL();
        
        /// <summary>
        /// Gets the name of the user with the specified <paramref name="userNum"/>.
        /// </summary>
        /// <param name="userNum">The user number.</param>
        /// <returns>The name of the user. Can by blank.</returns>
        string GetUserodName(long userNum) => User.GetName(userNum);

        /// <summary>
        /// Saves the current commlog to the database.
        /// </summary>
        /// <param name="showMsg">A value indicating whether to show a message when the commlog cannot be saved.</param>
        /// <returns>True if the commlog was saved to the database; otherwise, false.</returns>
        bool Save(bool showMsg)
        {
            // In persistent mode, we don't want to save empty commlogs.
            CommlogCur.Note = noteTextBox.Text.Trim();
            if (IsPersistent && string.IsNullOrWhiteSpace(CommlogCur.Note))
            { 
                return false;
            }

            if (IsNew || IsPersistent)
            {
                CommlogCur.CommlogNum = Commlogs.Insert(CommlogCur);

                SecurityLogs.MakeLogEntry(Permissions.CommlogEdit, CommlogCur.PatNum, "Insert");

                // Post insert persistent user preferences.
                if (IsPersistent)
                {
                    if (userOdPrefClearNote) clearNoteButton_Click(this, EventArgs.Empty);

                    if (userOdPrefEndDate)
                    {
                        endTextBox.Text = "";
                    }

                    ODException.SwallowAnyException(() => FormOpenDental.S_RefreshCurrentModule());
                }
            }
            else
            {
                Commlogs.Update(CommlogCur);

                SecurityLogs.MakeLogEntry(Permissions.CommlogEdit, CommlogCur.PatNum, "");
            }
            return true;
        }

        /// <summary>
        /// Deletes the current commlog.
        /// </summary>
        /// <param name="logText">The log message.</param>
        void Delete(string logText = "Delete")
        {
            Commlogs.Delete(CommlogCur);

            SecurityLogs.MakeLogEntry(Permissions.CommlogEdit, CommlogCur.PatNum, logText);
        }

        public void SetPatNum(long patNum)
        {
            CommlogCur.PatNum = patNum;
            patientTextBox.Text = GetPatientName(CommlogCur.PatNum);
        }

        public void SetUserNum(long userNum)
        {
            CommlogCur.UserNum = Security.CurrentUser.Id;
            userTextBox.Text = GetUserodName(CommlogCur.UserNum);
        }

        void CommItemSaveEvent_Fired(ODEventArgs e)
        {
            if (e.EventType != ODEventType.CommItemSave)
            {
                return;
            }

            // Save the item.
            Save(false);
        }

        void signatureBoxWrapper_SignatureChanged(object sender, EventArgs e)
        {
            SetUserNum(Security.CurrentUser.Id);
        }

        void PatientChangedEvent_Fired(ODEventArgs e)
        {
            if (e.EventType != ODEventType.Patient)
            {
                return;
            }

            if (e.Tag is long patNum)
            {
                SetPatNum(patNum);
                if (IsPersistent && userOdPrefUpdateDateTimeNewPat)
                {
                    startNowButton_Click(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The list of non-hidden CommLogTypes defs.  
        /// This property is mainly used as an example to show how we might use business logic within models.
        /// </summary>
        public List<Definition> ListCommlogTypeDefs
        {
            get
            {
                if (commlogTypeDefsList == null)
                {
                    commlogTypeDefsList = Definition.GetByCategory(DefinitionCategory.CommLogTypes);;
                }
                return commlogTypeDefsList;
            }
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormCommItem_Load(object sender, EventArgs e)
        {
            if (IsPersistent)
            {
                PatientChangedEvent.Fired += PatientChangedEvent_Fired;
            }
            CommItemSaveEvent.Fired += CommItemSaveEvent_Fired;

            _isStartingUp = true;

            endLabel.Visible = false;
            endTextBox.Visible = false;
            startNowButton.Visible = false;
            endNowButton.Visible = false;


            // Check whether the user is allowed to edit commlog items.
            if (!Security.IsAuthorized(Permissions.CommlogEdit, CommlogCur.CommDateTime))
            {
                // The user does not have permissions to create or edit commlogs.
                if (IsNew || IsPersistent)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                deleteButton.Enabled = false;
                acceptButton.Enabled = false;
                editAutoNoteButton.Enabled = false;
            }

            // There will usually be a commtype set before this dialog is opened
            for (int i = 0; i < ListCommlogTypeDefs.Count; i++)
            {
                typeListBox.Items.Add(ListCommlogTypeDefs[i].Description);
                if (ListCommlogTypeDefs[i].Id == CommlogCur.CommType)
                {
                    typeListBox.SelectedIndex = i;
                }
            }

            for (int i = 0; i < Enum.GetNames(typeof(CommItemMode)).Length; i++)
            {
                modeListBox.Items.Add(Lan.g("enumCommItemMode", Enum.GetNames(typeof(CommItemMode))[i]));
            }
            modeListBox.SelectedIndex = (int)CommlogCur.Mode_;

            for (int i = 0; i < Enum.GetNames(typeof(CommSentOrReceived)).Length; i++)
            {
                sendOrReceivedListBox.Items.Add(Lan.g("enumCommSentOrReceived", Enum.GetNames(typeof(CommSentOrReceived))[i]));
            }
            sendOrReceivedListBox.SelectedIndex = (int)CommlogCur.SentOrReceived;


            signatureBoxWrapper.FillSignature(CommlogCur.SigIsTopaz, GetSignatureKey(), CommlogCur.Signature);
            signatureBoxWrapper.BringToFront();

            editAutoNoteButton.Visible = HasAutoNotePrompt;

            if (IsPersistent)
            {
                RefreshUserPreferences();
                userPrefsButton.Visible = true;
                acceptButton.Text = "Create";
                cancelButton.Text = "Close";
                deleteButton.Visible = false;
            }

            if (IsNew && Preference.GetBool(PreferenceName.CommLogAutoSave))
            {
                autoSaveTimer.Start();
            }

            patientTextBox.Text = GetPatientName(CommlogCur.PatNum);
            userTextBox.Text = GetUserodName(CommlogCur.UserNum);
            startTextBox.Text = CommlogCur.CommDateTime.ToShortDateString() + "  " + CommlogCur.CommDateTime.ToShortTimeString();
            if (CommlogCur.DateTimeEnd.Year > 1880)
            {
                endTextBox.Text = CommlogCur.DateTimeEnd.ToShortDateString() + "  " + CommlogCur.DateTimeEnd.ToShortTimeString();
            }

            noteTextBox.Select();
            noteTextBox.Text = CommlogCur.Note;
            noteTextBox.SelectionStart = noteTextBox.Text.Length;

            _isStartingUp = false;
        }

        /// <summary>
        /// Displays the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        void Error(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Communication Item",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Perform cleanup when the form is closing.
        /// </summary>
        void FormCommItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommItemSaveEvent.Fired -= CommItemSaveEvent_Fired;
            if (IsPersistent)
            {
                PatientChangedEvent.Fired -= PatientChangedEvent_Fired;
                return;
            }

            // If the dialog is cancalle and the autosave timer is enabled, we need to delete the autosaved commlog.
            if (DialogResult == DialogResult.Cancel && autoSaveTimer.Enabled && !IsNew)
            {
                try
                {
                    Delete("Autosaved Commlog Deleted");
                }
                catch (Exception ex)
                {
                    Error(ex.Message);
                }
            }

            autoSaveTimer.Stop();
            autoSaveTimer.Enabled = false;
        }

        /// <summary>
        /// Checks whether the note contains a auto note prompt.
        /// </summary>
        bool HasAutoNotePrompt => Regex.IsMatch(noteTextBox.Text, @"\[Prompt:""[a-zA-Z_0-9 ]+""\]");
        
        /// <summary>
        /// Validates the commlog.
        /// </summary>
        /// <param name="showMsg"></param>
        /// <returns></returns>
        bool ValidateLog(bool showMsg)
        {
            var start = startTextBox.Text.Trim();
            if (start.Length == 0)
            {
                if (showMsg)
                {
                    Error("Please enter a date first.");
                }
                return false;
            }

            if (!DateTime.TryParse(start, out var dateTimeStart))
            {
                if (showMsg)
                {
                    Error("Start date and time invalid.");
                }
                return false;
            }

            if (endTextBox.Text != "")
            {
                if (!DateTime.TryParse(endTextBox.Text, out var dateTimeEnd))
                {
                    if (showMsg)
                    {
                        Error("End date and time invalid.");
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clears the current signature.
        /// </summary>
        void ClearSignature_Event(object sender, EventArgs e)
        {
            if (!_isStartingUp && !_sigChanged)
            {
                signatureBoxWrapper.ClearSignature();
            }
        }

        /// <summary>
        /// Generate the encryption key for the signature.
        /// </summary>
        /// <returns></returns>
        string GetSignatureKey()
        {
            var keyData =
                CommlogCur.UserNum.ToString() +
                CommlogCur.CommDateTime.ToString() +
                CommlogCur.Mode_.ToString() +
                CommlogCur.SentOrReceived.ToString();

            if (CommlogCur.Note != null)
            {
                keyData += CommlogCur.Note.ToString();
            }

            return keyData;
        }

        /// <summary>
        /// Creates a autosave of the current commlog.
        /// </summary>
        void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (IsNew)
            {
                CommlogCur.CommlogNum = Commlogs.Insert(CommlogCur);

                SecurityLogs.MakeLogEntry(Permissions.CommlogEdit, CommlogCur.PatNum, "Autosave Insert");

                IsNew = false;
            }
            else Commlogs.Update(CommlogCur);

            // Getting this far means that the commlog was successfully updated so we need to update the UI to reflect that event.
            if (IsNew)
            {
                cancelButton.Enabled = false;
            }

            Text = "Communication Item - Saved: " + DateTime.Now;
        }

        /// <summary>
        /// Open the user preferences form and refresh the preferences.
        /// </summary>
        void userPrefsButton_Click(object sender, EventArgs e)
        {
            using (var formCommItemUserPrefs = new FormCommItemUserPrefs())
            {
                if (formCommItemUserPrefs.ShowDialog() == DialogResult.OK)
                {
                    RefreshUserPreferences();
                }
            }
        }

        /// <summary>
        /// Sets the start date/time textbox the the current date and time.
        /// </summary>
        void startNowButton_Click(object sender, EventArgs e)
        {
            startTextBox.Text = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Sets the end date/time textbox to the current date and time.
        /// </summary>
        void endNowButton_Click(object sender, EventArgs e)
        {
            endTextBox.Text = DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Open the form to edit the current auto note.
        /// </summary>
        void editAutoNoteButton_Click(object sender, EventArgs e)
        {
            if (HasAutoNotePrompt)
            {
                using (var formAutoNoteCompose = new FormAutoNoteCompose())
                {
                    formAutoNoteCompose.MainTextNote = noteTextBox.Text;
                    if (formAutoNoteCompose.ShowDialog() == DialogResult.OK)
                    {
                        noteTextBox.Text = formAutoNoteCompose.CompletedNote;

                        editAutoNoteButton.Visible = HasAutoNotePrompt;
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "No Auto Note available to edit.",
                    "Communication Item", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Open the form to create a auto note.
        /// </summary>
        void autoNoteButton_Click(object sender, EventArgs e)
        {
            using (var formAutoNoteCompose = new FormAutoNoteCompose())
            {
                if (formAutoNoteCompose.ShowDialog() == DialogResult.OK)
                {
                    noteTextBox.AppendText(formAutoNoteCompose.CompletedNote);

                    editAutoNoteButton.Visible = HasAutoNotePrompt;
                }
            }
        }

        /// <summary>
        /// Clears the note textbox.
        /// </summary>
        void clearNoteButton_Click(object sender, EventArgs e) => noteTextBox.Clear();

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <remarks>
        /// Button not enabled if no permission and is invisible for persistent mode.
        /// </remarks>
        void deleteButton_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            var result =
                MessageBox.Show(
                    "Delete?",
                    "Communication Item", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;
            
            try
            {
                Delete();

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Communication Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the item and closes the form.
        /// </summary>
        async void acceptButton_Click(object sender, EventArgs e)
        {
            // Button not enabled if no permission
            if (!Save(true))
            {
                return;
            }

            // Show the user an indicator that the commlog has been saved but do not close the window.
            if (IsPersistent)
            {
                await ShowManualSaveLabel();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Shows the saved manually label for 1.5 seconds.
        /// </summary>
        async System.Threading.Tasks.Task ShowManualSaveLabel()
        {
            savedManuallyLabel.Visible = true;

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.5));

            savedManuallyLabel.Visible = false;
        }
    }
}