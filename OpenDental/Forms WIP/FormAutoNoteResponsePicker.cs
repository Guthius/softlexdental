using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAutoNoteResponsePicker : FormBase
    {
        /// <summary>
        /// This will have the Response text and the chosen AutoNote in the format "Response text : {AutoNoteName}".
        /// </summary>
        public string AutoNoteResponseText;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAutoNoteResponsePicker"/> class.
        /// </summary>
        public FormAutoNoteResponsePicker() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAutoNoteResponsePicker_Load(object sender, EventArgs e) => LoadAutoNotes();

        /// <summary>
        /// Loads all autonotes and populates the grid.
        /// </summary>
        void LoadAutoNotes()
        {
            AutoNotes.RefreshCache();

            autoNotesGrid.BeginUpdate();
            autoNotesGrid.Columns.Clear();
            autoNotesGrid.Columns.Add(new ODGridColumn("", 100));
            autoNotesGrid.Rows.Clear();

            var listAutoNotes = AutoNotes.GetDeepCopy();
            foreach (var autoNote in listAutoNotes)
            {
                var row = new ODGridRow();
                row.Cells.Add(autoNote.AutoNoteName);
                row.Tag = autoNote;
                autoNotesGrid.Rows.Add(row);
            }

            autoNotesGrid.EndUpdate();
        }

        /// <summary>
        /// Sets the AutoNoteResponseText with the selected AutoNote in the format "Auto Note Response Text : {AutoNoteName}".
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(responseTextBox.Text))
            {
                MessageBox.Show(
                    "Please enter a response text.",
                    "Auto Note Response Picker", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (autoNotesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an AutoNote.",
                    "Auto Note Response Picker",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var autoNoteSelected = autoNotesGrid.SelectedTag<AutoNote>();
            if (autoNoteSelected == null)
            {
                MessageBox.Show(
                    "Invalid AutoNote selected. Please select a new one.", 
                    "Auto Note Response Picker",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                autoNotesGrid.SetSelected(false);

                return;
            }

            // The AutoNoteResponseText should be in format "Auto Note Response Text : {AutoNoteName}"
            // This format is needed so the text processing logic can parse through it correctly.
            // If this format changes, we need to change the logic in FormAutoNoteCompose.PromptForAutoNotes()
            // If this format changes, you will also need to modify FormAutoNoteCompose.GetAutoNoteName() and FormAutoNoteCompose.GetAutoNoteResponseText
            AutoNoteResponseText = responseTextBox.Text + " : {" + autoNoteSelected.AutoNoteName + "}";
            DialogResult = DialogResult.OK;
        }
    }
}