using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiAllPages : FormBase
    {
        List<WikiPage> wikiPagesList;

        /// <summary>
        /// Need a reference to the form where this was launched from so that we can tell it to refresh later.
        /// </summary>
        public FormWikiEdit OwnerForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiAllPages"/> class.
        /// </summary>
        public FormWikiAllPages() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiAllPages_Load(object sender, EventArgs e) => LoadWikiPages();

        /// <summary>
        /// Reload the pages (matching the search criteria).
        /// </summary>
        void textSearch_TextChanged(object sender, EventArgs e) => LoadWikiPages();

        /// <summary>
        /// Loads the specified wiki page.
        /// </summary>
        /// <param name="wikiPage">The wiki page to load.</param>
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
                    "This page is broken and cannot be viewed. " + ex.Message,
                    "All Wiki Pages", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads all wiki pages (matching the specified search criteria).
        /// </summary>
        void LoadWikiPages()
        {
            pagesGrid.BeginUpdate();
            pagesGrid.Columns.Clear();
            pagesGrid.Columns.Add(new ODGridColumn("Title", 70));
            pagesGrid.Rows.Clear();

            wikiPagesList = WikiPages.GetByTitleContains(searchTextBox.Text);
            for (int i = 0; i < wikiPagesList.Count; i++)
            {
                ODGridRow row = new ODGridRow();

                // TODO: Add a indicator for deleted columns...

                row.Cells.Add(wikiPagesList[i].PageTitle);
                pagesGrid.Rows.Add(row);
            }

            pagesGrid.EndUpdate();
        }

        void pagesGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            LoadWikiPage(wikiPagesList[e.Row]);
            pagesGrid.Focus();
        }

        void pagesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            pagesGrid.SetSelected(false);
            pagesGrid.SetSelected(e.Row, true);

            acceptButton_Click(this, EventArgs.Empty);
        }

        void webBrowserWiki_Navigated(object sender, WebBrowserNavigatedEventArgs e) => wikiWebBrowser.AllowNavigation = false;
        
        /// <summary>
        /// Adds a new wikipage.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var formWikiRename = new FormWikiRename())
            {
                if (formWikiRename.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var onWikiSaved = new Action<string>((pageTitleNew) =>
                {
                    var wikiPage = WikiPages.GetByTitle(pageTitleNew);
                    if (wikiPage != null && OwnerForm != null && !OwnerForm.IsDisposed)
                    {
                        OwnerForm.RefreshPage(wikiPage);
                    }
                });

                var formWikiEdit = new FormWikiEdit(onWikiSaved);
                formWikiEdit.WikiPageCur = new WikiPage();
                formWikiEdit.WikiPageCur.IsNew = true;
                formWikiEdit.WikiPageCur.PageTitle = formWikiRename.PageTitle;
                formWikiEdit.WikiPageCur.PageContent = "[[" + OwnerForm.WikiPageCur.PageTitle + "]]\r\n<h1>" + formWikiRename.PageTitle + "</h1>\r\n";
                formWikiEdit.Show();
            }

            Close();
        }

        void bracketsButton_Click(object sender, EventArgs e)
        {
            // TODO: What is this supposed to do??

            if (OwnerForm != null && !OwnerForm.IsDisposed)
            {
                OwnerForm.RefreshPage(null);
            }
            Close();
        }

        void acceptButton_Click(object sender, EventArgs e)
        {
            if (pagesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select a page first.",
                    "All Wiki Pages", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                return;
            }

            if (OwnerForm != null && !OwnerForm.IsDisposed)
            {
                OwnerForm.RefreshPage(wikiPagesList[pagesGrid.GetSelectedIndex()]);
            }
            Close();
        }
    }
}