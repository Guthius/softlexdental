using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAutoNoteEdit : FormBase
    {
        List<AutoNoteControl> autoNoteControlsList;
        int textSelectionStart;

        /// <summary>
        /// Gets or sets a value indicating whether we're creating a new autonote.
        /// </summary>
        public bool IsNew { get; set; }

        public AutoNote AutoNoteCur;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAutoNoteEdit"/> class.
        /// </summary>
        public FormAutoNoteEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAutoNoteEdit_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit, true))
            {
                addButton.Enabled = false;
                deleteButton.Enabled = false;
                acceptButton.Enabled = false;
                textMain.ReadOnly = true;
                nameTextBox.ReadOnly = true;
            }
            else
            {
                promptsGrid.CellDoubleClick += new EventHandler<ODGridClickEventArgs>(promptsGrid_CellDoubleClick);
            }

            nameTextBox.Text = AutoNoteCur.AutoNoteName;
            textMain.Text = AutoNoteCur.MainText;

            LoadAutoNotePrompts();
        }

        /// <summary>
        /// Loads all autonote prompts and populates the grid.
        /// </summary>
        void LoadAutoNotePrompts()
        {
            AutoNoteControls.RefreshCache();
            autoNoteControlsList = AutoNoteControls.GetDeepCopy(false);

            promptsGrid.BeginUpdate();
            promptsGrid.Columns.Clear();
            promptsGrid.Columns.Add(new ODGridColumn("", 100));
            promptsGrid.Rows.Clear();

            for (int i = 0; i < autoNoteControlsList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(autoNoteControlsList[i].Descript);
                promptsGrid.Rows.Add(row);
            }

            promptsGrid.EndUpdate();
        }

        /// <summary>
        /// Show the form to add a new autonote prompt.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var formAutoNoteControlEdit = new FormAutoNoteControlEdit())
            {
                var control = new AutoNoteControl
                {
                    ControlType = "Text"
                };

                formAutoNoteControlEdit.ControlCur = control;
                formAutoNoteControlEdit.IsNew = true;

                if (formAutoNoteControlEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAutoNotePrompts();
                }
            }
        }

        /// <summary>
        /// Open the for to edit a autonote prompt when the user double clicks on one in the grid.
        /// </summary>
        void promptsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formAutoNoteControlEdit = new FormAutoNoteControlEdit())
            {
                formAutoNoteControlEdit.ControlCur = autoNoteControlsList[e.Row];
                if (formAutoNoteControlEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAutoNotePrompts();
                }
            }
        }

        /// <summary>
        /// Insert the selected prompt into the note.
        /// </summary>
        void insertButton_Click(object sender, EventArgs e)
        {
            if (promptsGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show("Please select a prompt first.", Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            string fieldStr = autoNoteControlsList[promptsGrid.GetSelectedIndex()].Descript;
            if (textSelectionStart < textMain.Text.Length - 1)
            {
                textMain.Text = textMain.Text.Substring(0, textSelectionStart)
                    + "[Prompt:\"" + fieldStr + "\"]"
                    + textMain.Text.Substring(textSelectionStart);
            }
            else
            {
                textMain.Text += "[Prompt:\"" + fieldStr + "\"]";
            }

            // TODO: Can't we just set textMain.SelectedText ??

            textMain.Select(textSelectionStart + fieldStr.Length + 11, 0);
            textMain.Focus();
        }

        /// <summary>
        /// Remember the selection start position when the autonote textbox loses focus,
        /// we need this when inserting a autonote prompt.
        /// </summary>
        void textMain_Leave(object sender, EventArgs e) => textSelectionStart = textMain.SelectionStart;

        /// <summary>
        /// Delete the autonote and close the form.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "Delete this autonote?", 
                    Text, 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;
            
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            AutoNotes.Delete(AutoNoteCur.AutoNoteNum);
            DataValid.SetInvalid(InvalidType.AutoNotes);
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Save the autonote and close the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            AutoNoteCur.AutoNoteName = nameTextBox.Text;
            AutoNoteCur.MainText = textMain.Text;

            if (IsNew)
            {
                AutoNotes.Insert(AutoNoteCur);
            }
            else
            {
                AutoNotes.Update(AutoNoteCur);
            }

            DataValid.SetInvalid(InvalidType.AutoNotes);
            DialogResult = DialogResult.OK;
        }
    }
}