using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiRename : FormBase
    {
        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        public string PageTitle
        {
            get => textPageTitle.Text.Trim();
            set
            {
                textPageTitle.Text = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiRename"/> class.
        /// </summary>
        public FormWikiRename() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiRename_Load(object sender, EventArgs e)
        {
            Text = 
                !string.IsNullOrEmpty(PageTitle) ? 
                    Translation.Language.WikiPageTitle + " - " + PageTitle :
                    Translation.Language.WikiPageTitle;

            if (PageTitle.ToLower() == "home")
            {
                MessageBox.Show(
                    Translation.Language.WikiCannotRenameTheDefaultHomepage,
                    Translation.Language.WikiPageTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Validates if the specified title is valid and if so closes the form.
        /// The actual save should be handled by whoever called this form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PageTitle))
            {
                MessageBox.Show(
                    Translation.Language.WikiPageTitleCannotBeEmpty,
                    Translation.Language.WikiPageTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!WikiPages.IsWikiPageTitleValid(textPageTitle.Text, out string errorMsg))
            {
                MessageBox.Show(
                    errorMsg,
                    Translation.Language.WikiPageTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var wikiPage = WikiPages.GetByTitle(textPageTitle.Text);
            if (wikiPage != null)
            {
                MessageBox.Show(
                    Translation.Language.WikiPageTitleAlreadyExists,
                    Translation.Language.WikiPageTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}