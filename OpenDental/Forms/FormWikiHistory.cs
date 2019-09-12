using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiHistory : FormBase
    {
        public string SelectedPageTitle;

        /// <summary>
        /// True if the page can only be edited by WikiAdmins.
        /// </summary>
        public bool IsLocked = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiHistory"/> class.
        /// </summary>
        public FormWikiHistory() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiHistory_Load(object sender, EventArgs e)
        {
            LoadPageList();
            LoadPage(wikiPagesGrid.Rows[wikiPagesGrid.GetSelectedIndex()].Tag as WikiPageHist);

            Text = Translation.Language.WikiHistory + " - " + SelectedPageTitle;

            //Page is locked and user doesn't have permission
            if (IsLocked && !Security.IsAuthorized(Permissions.WikiAdmin, true))
            {
                revertButton.Enabled = false;
            }
            else
            {
                labelNotAuthorized.Visible = false;
            }
        }

        /// <summary>
        /// Loads the specified page.
        /// </summary>
        /// <param name="wikiPage">The page to load.</param>
        void LoadPage(WikiPageHist wikiPage)
        {
            webBrowserWiki.AllowNavigation = true;
            try
            {
                if (string.IsNullOrEmpty(wikiPage.PageContent))
                {
                    // If this is the first time the user has clicked on this revision, 
                    // get page content from db (the row's tag will have this as well)
                    wikiPage.PageContent = WikiPageHists.GetPageContent(wikiPage.WikiPageNum);
                }

                webBrowserWiki.DocumentText = MarkupEdit.TranslateToXhtml(
                    WikiPages.GetWikiPageContentWithWikiPageTitles(wikiPage.PageContent), 
                    false, hasWikiPageTitles: true);
            }
            catch (Exception ex)
            {
                webBrowserWiki.DocumentText = "";

                MessageBox.Show(
                    Translation.Language.WikiPageIsBroken + " " + ex.Message,
                    Translation.Language.Wiki,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the list of pages and populates the grid.
        /// </summary>
        void LoadPageList()
        {
            wikiPagesGrid.BeginUpdate();
            wikiPagesGrid.Columns.Clear();
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnUser, 70));
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDel, 25));
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSaved, 80));
            
            var listWikiPageHists = WikiPageHists.GetByTitleNoPageContent(SelectedPageTitle);

            var wikiPage = WikiPages.GetByTitle(SelectedPageTitle);
            if (wikiPage != null)
            {
                listWikiPageHists.Add(WikiPages.PageToHist(wikiPage));
            }

            var usersDictionary = 
                Userods.GetUsers(listWikiPageHists.Select(x => x.UserNum).Distinct().ToList()) //gets from cache, very fast
                    .ToDictionary(x => x.Id, x => x.UserName);//create dictionary of key=UserNum, value=UserName for fast lookup

            wikiPagesGrid.Rows.Clear();
            foreach (var wikiPageHist in listWikiPageHists)
            {
                var row = new ODGridRow();
                if (!usersDictionary.TryGetValue(wikiPageHist.UserNum, out string userName))
                {
                    userName = "";
                }

                row.Cells.Add(userName);
                row.Cells.Add((wikiPageHist.IsDeleted ? "X" : ""));
                row.Cells.Add(wikiPageHist.DateTimeSaved.ToString());
                row.Tag = wikiPageHist;

                wikiPagesGrid.Rows.Add(row);
            }

            wikiPagesGrid.EndUpdate();
            wikiPagesGrid.SetSelected(wikiPagesGrid.Rows.Count - 1, true);
            wikiPagesGrid.ScrollToEnd();
        }

        /// <summary>
        /// Load a page when the user clicks on one in the grid.
        /// </summary>
        void gridMain_Click(object sender, EventArgs e)
        {
            if (wikiPagesGrid.GetSelectedIndex() != -1 && 
                wikiPagesGrid.Rows[wikiPagesGrid.GetSelectedIndex()].Tag is WikiPageHist wikiPage)
            {
                LoadPage(wikiPage);
            }

            // TODO: Disable the revert button when the last revision is selected.

            wikiPagesGrid.Focus();
        }

        /// <summary>
        /// After navigation completes disable navigations, this disables links in the webbrowser control.
        /// </summary>
        void webBrowserWiki_Navigated(object sender, WebBrowserNavigatedEventArgs e) => webBrowserWiki.AllowNavigation = false;
        
        /// <summary>
        /// Reverts the page to the current revision.
        /// </summary>
        void revertButton_Click(object sender, EventArgs e)
        {
            if (wikiPagesGrid.GetSelectedIndex() == -1 || 
                wikiPagesGrid.GetSelectedIndex() == wikiPagesGrid.Rows.Count - 1)
                return;

            var result = 
                MessageBox.Show(
                    Translation.Language.WikiRevertPageToSelectedRevision,
                    Translation.Language.WikiHistory, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            if (wikiPagesGrid.Rows[wikiPagesGrid.GetSelectedIndex()].Tag is WikiPageHist wikiPageHist)
            {
                var wikiPage = WikiPageHists.RevertFrom(wikiPageHist);

                wikiPage.UserNum = Security.CurrentUser.Id;

                WikiPages.InsertAndArchive(wikiPage);
                LoadPageList();
            }
        }
    }
}