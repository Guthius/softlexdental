using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiIncomingLinks : FormBase
    {
        string          wikiPageTitle;
        List<WikiPage>  wikiPagesList;

        /// <summary>
        /// Gets the currently selected page.
        /// </summary>
        public WikiPage SelectedPage
        {
            get
            {
                if (wikiPagesGrid.SelectedIndices.Length > 0)
                {
                    return wikiPagesList[wikiPagesGrid.SelectedIndices[0]];
                }
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiIncomingLinks"/> class.
        /// </summary>
        /// <param name="wikiPageTitle"</param>
        public FormWikiIncomingLinks(string wikiPageTitle)
        {
            InitializeComponent();

            this.wikiPageTitle = wikiPageTitle;
        }
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWiki_Load(object sender, EventArgs e)
        {
            Text = string.Format(Translation.Language.WikiIncomingLinksTo0, wikiPageTitle);

            LoadWikiPageList();

            if (wikiPagesList.Count == 0)
            {
                MessageBox.Show(
                    Translation.Language.WikiPageHasNoIncomingLinks,
                    Translation.Language.WikiIncomingLinks, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                Close();
            }
        }

        /// <summary>
        /// Loadsthe specified page.
        /// </summary>
        /// <param name="wikiPage">The page to load.</param>
        void LoadWikiPage(WikiPage wikiPage)
        {
            wikiWebBrowser.AllowNavigation = true;
            try
            {
                wikiWebBrowser.DocumentText = MarkupEdit.TranslateToXhtml(wikiPage.PageContent, false);
            }
            catch (Exception ex)
            {
                wikiWebBrowser.DocumentText = "";

                MessageBox.Show(
                    Translation.Language.WikiPageIsBroken + " " + ex.Message,
                    Translation.Language.WikiIncomingLinks, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the list of wiki pages and populates the grid.
        /// </summary>
        void LoadWikiPageList()
        {
            wikiPagesGrid.BeginUpdate();
            wikiPagesGrid.Columns.Clear();
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPageTitle, 70));

            wikiPagesGrid.Rows.Clear();

            wikiPagesList = WikiPages.GetIncomingLinks(wikiPageTitle);
            for (int i = 0; i < wikiPagesList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(wikiPagesList[i].PageTitle);
                wikiPagesGrid.Rows.Add(row);
            }
            wikiPagesGrid.EndUpdate();
        }

        /// <summary>
        /// When the user clicks on a page in the grid, load the page preview.
        /// </summary>
        void wikiPagesGrid_Click(object sender, EventArgs e)
        {
            if (wikiPagesGrid.SelectedIndices.Length < 1)
            {
                return;
            }
            
            LoadWikiPage(wikiPagesList[wikiPagesGrid.SelectedIndices[0]]);
            wikiPagesGrid.Focus();
        }

        /// <summary>
        /// Close the form when the user double clicks on a page in the grid.
        /// </summary>
        void wikiPagesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (SelectedPage != null)
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Disable further navigation after the preview browser completes navigation.
        /// </summary>
        void wikiWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e) => wikiWebBrowser.AllowNavigation = false;
    }
}