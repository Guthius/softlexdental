using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiLists : FormBase
    {
        List<string> wikiLists;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiLists"/> class.
        /// </summary>
        public FormWikiLists() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiLists_Load(object sender, EventArgs e) => LoadLists();
        
        /// <summary>
        /// Populates the lists listbox.
        /// </summary>
        void LoadLists()
        {
            listBox.Items.Clear();

            wikiLists = WikiLists.GetAllLists();
            foreach (string wikiList in wikiLists)
            {
                listBox.Items.Add(wikiList);
            }
        }

        /// <summary>
        /// Open the edit window when the user double clicks on a list in the listbox.
        /// </summary>
        void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = listBox.IndexFromPoint(e.Location);

            if (index != -1)
            {
                if (listBox.Items[index] is string listName)
                {
                    using (var formWikiListEdit = new FormWikiListEdit())
                    {
                        formWikiListEdit.WikiListCurName = listName;
                        formWikiListEdit.ShowDialog();
                    }

                    LoadLists(); // TODO: Just update the single item instead of reloading everything...
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter a name for a new list.
        /// </summary>
        /// <param name="listName">The name for a new list.</param>
        /// <returns>True if the user gave a name; otherwise, false.</returns>
        bool TryGetNewListName(out string listName)
        {
            listName = string.Empty;

            using (InputBox inputBox = new InputBox(Translation.Language.WikiNewListName))
            {
                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    listName = inputBox.textResult.Text.ToLower().Replace(" ", "");

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a new list.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;
            
            if (!TryGetNewListName(out string listName))
            {
                return;
            }

            if (DbHelper.isMySQLReservedWord(listName)) // TODO: Why is this a issue???
            {
                MessageBox.Show(
                    Translation.Language.WikiListNameIsReservedWord,
                    Translation.Language.WikiLists,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(listName))
            {
                MessageBox.Show(
                    Translation.Language.WikiListNameCannotBeBlank,
                    Translation.Language.WikiLists,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (WikiLists.CheckExists(listName))
            {
                var result =
                    MessageBox.Show(
                        Translation.Language.WikiListAlreadyExistsWithThatName,
                        Translation.Language.WikiLists,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            using (var formWikiListEdit = new FormWikiListEdit())
            {
                formWikiListEdit.WikiListCurName = listName;
                formWikiListEdit.ShowDialog();
            }

            LoadLists();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void cancelButton_Click(object sender, EventArgs e) => Close();
    }
}