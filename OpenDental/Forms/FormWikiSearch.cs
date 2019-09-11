using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiSearch : FormBase
    {
        List<string> pageTitleList;

        /// <summary>
        /// Gets the title of the selected page.
        /// </summary>
        public string SelectedPageTitle
        {
            get
            {
                if (wikiPagesGrid.SelectedIndices.Length > 0)
                {
                    return pageTitleList[wikiPagesGrid.SelectedIndices[0]];
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Raised when the search form wants to navigate to a page.
        /// </summary>
        public event EventHandler<WikiPageEventArgs> Navigate;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiSearch"/> class.
        /// </summary>
        public FormWikiSearch() => InitializeComponent();

        /// <summary>
        /// Invokes the <see cref="Navigate"/> event.
        /// </summary>
        /// <param name="pageTitle">The title of the page to navigate to.</param>
        protected virtual void OnNavigate(string pageTitle) => Navigate?.Invoke(this, new WikiPageEventArgs(pageTitle));
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiSearch_Load(object sender, EventArgs e)
        {
            SetFilterControlsAndAction(() => LoadPageList(), (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds, searchTextBox);

            LoadPageList();
        }

        /// <summary>
        /// Loads the page with the specified title.
        /// </summary>
        /// <param name="pageTitle">The title of the page to load.</param>
        void LoadPage(string pageTitle)
        {
            restoreButton.Enabled = false;

            wikiWebBrowser.AllowNavigation = true;
            try
            {
                if (archivedOnlyCheckBox.Checked)
                {
                    wikiWebBrowser.DocumentText = MarkupEdit.TranslateToXhtml(WikiPages.GetByTitle(pageTitle, isDeleted: true).PageContent, true);
                    restoreButton.Enabled = true;
                }
                else
                {
                    wikiWebBrowser.DocumentText = MarkupEdit.TranslateToXhtml(WikiPages.GetByTitle(pageTitle).PageContent, true);
                }
            }
            catch (Exception ex)
            {
                wikiWebBrowser.DocumentText = "";

                MessageBox.Show(
                    Translation.Language.WikiPageIsBroken + " " + ex.Message,
                    Translation.Language.WikiSearch, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Populates the grid.
        /// </summary>
        void LoadPageList()
        {
            wikiPagesGrid.BeginUpdate();
            wikiPagesGrid.Columns.Clear();
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnTitle, 70));
            wikiPagesGrid.Rows.Clear();

            if (archivedOnlyCheckBox.Checked)
            {
                pageTitleList = WikiPages.GetForSearch(searchTextBox.Text, ignoreContentCheckBox.Checked, isDeleted: true);
            }
            else
            {
                pageTitleList = WikiPages.GetForSearch(searchTextBox.Text, ignoreContentCheckBox.Checked);
            }

            for (int i = 0; i < pageTitleList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(pageTitleList[i]);
                wikiPagesGrid.Rows.Add(row);
            }

            wikiPagesGrid.EndUpdate();
            wikiWebBrowser.DocumentText = "";
        }

        /// <summary>
        /// Load a page when the user clicks on one in the grid.
        /// </summary>
        void wikiPagesGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            LoadPage(pageTitleList[e.Row]);
            wikiPagesGrid.Focus();
        }

        /// <summary>
        /// Click the accept button automatically when the user double clicks on a page in the grid.
        /// </summary>
        void wikiPagesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            wikiPagesGrid.SetSelected(e.Row, true);
            acceptButton_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Reload the list of pages when the state of the 'Ignore Content' checkbox changes.
        /// </summary>
        void checkIgnoreContent_CheckedChanged(object sender, EventArgs e) => LoadPageList();
        
        /// <summary>
        /// Reload the list of pages when the state of the 'Archived Only' checkbox changes.
        /// If 'Archived Only' is checked we also disable the accept button.
        /// </summary>
        void checkArchivedOnly_CheckedChanged(object sender, EventArgs e)
        {
            acceptButton.Enabled = !archivedOnlyCheckBox.Checked;
            LoadPageList();
        }

        /// <summary>
        /// After navigation disallow navigation, this will disable links within the page.
        /// </summary>
        void wikiWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e) => wikiWebBrowser.AllowNavigation = false;

        /// <summary>
        /// Attempt to restore the selected page.
        /// </summary>
        void restoreButton_Click(object sender, EventArgs e)
        {
            if (wikiPagesGrid.GetSelectedIndex() == -1) return;
            
            if (WikiPages.GetByTitle(SelectedPageTitle) != null)
            {
                MessageBox.Show(
                    Translation.Language.WikiSelectedPageAlreadyRestored,
                    Translation.Language.WikiSearch);
                return;
            }

            var wikiPageRestored = WikiPages.GetByTitle(pageTitleList[wikiPagesGrid.SelectedIndices[0]], isDeleted: true);
            if (wikiPageRestored == null)
            {
                MessageBox.Show(
                    Translation.Language.WikiSelectedPageAlreadyRestored,
                    Translation.Language.WikiSearch);
                return;
            }

            WikiPages.WikiPageRestore(wikiPageRestored, Security.CurUser.Id);

            // TODO: Should we navigate to the restored page?

            Close();
        }

        /// <summary>
        /// Navigates to the selected page (if it's not a archived page) and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (archivedOnlyCheckBox.Checked) return;
            
            if (!string.IsNullOrEmpty(SelectedPageTitle))
            {
                OnNavigate(SelectedPageTitle);
            }

            Close();
        }
    }

    //
    // TODO: This class needs to be moved to a more appropriate location...
    //
    public class WikiPageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the title of the page.
        /// </summary>
        public string PageTitle { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiPageEventArgs"/> class.
        /// </summary>
        /// <param name="pageTitle"></param>
        public WikiPageEventArgs(string pageTitle)
        {
            PageTitle = pageTitle ?? "";
        }
    }
}