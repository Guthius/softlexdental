using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiDrafts : FormBase
    {
        List<WikiPage> wikiPageList;

        public FormWiki OwnerForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiDrafts"/> class.
        /// </summary>
        public FormWikiDrafts() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiDrafts_Load(object sender, EventArgs e)
        {
            LoadPageList();
            LoadPage(wikiPageList[wikiPagesGrid.SelectedIndices[0]]);

            wikiPagesGrid.SetSelected(wikiPagesGrid.Rows.Count - 1, true);

            Text = Translation.Language.WikiDrafts + " - " + OwnerForm.WikiPageCur.PageTitle;
        }

        /// <summary>
        /// Loads the specified wiki page.
        /// </summary>
        /// <param name="wikiPage">The wiki page to load.</param>
        void LoadPage(WikiPage wikiPage)
        {
            try
            {
                textContent.Text = WikiPages.GetWikiPageContentWithWikiPageTitles(wikiPage.PageContent);
                wikiWebBrowser.AllowNavigation = true;
                wikiWebBrowser.DocumentText = MarkupEdit.TranslateToXhtml(textContent.Text, false, hasWikiPageTitles: true);
            }
            catch (Exception ex)
            {
                wikiWebBrowser.DocumentText = "";

                MessageBox.Show(
                    Translation.Language.WikiPageIsBroken + " " + ex.Message,
                    Translation.Language.WikiDrafts);
            }
        }

        /// <summary>
        /// Loads the list of wiki pages and populates the grid.
        /// </summary>
        void LoadPageList()
        {
            wikiPagesGrid.BeginUpdate();
            wikiPagesGrid.Columns.Clear();
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.lang_user, 70));
            wikiPagesGrid.Columns.Add(new ODGridColumn(Translation.Language.lang_last_saved, 80));
            wikiPagesGrid.Rows.Clear();

            wikiPageList = WikiPages.GetDraftsByTitle(OwnerForm.WikiPageCur.PageTitle);
            for (int i = 0; i < wikiPageList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(User.GetName(wikiPageList[i].UserNum));
                row.Cells.Add(wikiPageList[i].DateTimeSaved.ToString());
                wikiPagesGrid.Rows.Add(row);
            }

            wikiPagesGrid.EndUpdate();
        }

        /// <summary>
        /// When the user selects a draft from the list, display it.
        /// </summary>
        void wikiPagesGrid_Click(object sender, EventArgs e)
        {
            if (wikiPagesGrid.SelectedIndices.Length < 1) return;

            LoadPage(wikiPageList[wikiPagesGrid.SelectedIndices[0]]);

            wikiPagesGrid.Focus();
        }

        /// <summary>
        /// Click the edit button when the user clicks on a row in the grid.
        /// </summary>
        void wikiPagesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) => editButton_Click(sender, EventArgs.Empty);
        
        /// <summary>
        /// After the webbrowser has navigated we disallow navigation, by doing so we disable links in pages.
        /// </summary>
        void wikiWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            wikiWebBrowser.AllowNavigation = false;
        }

        /// <summary>
        /// Edit the selected draft when the user clicks the edit button.
        /// </summary>
        void editButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = wikiPagesGrid.GetSelectedIndex();
            if (selectedIndex < 0)
            {
                return;
            }

            using (var formWikiEdit = new FormWikiEdit())
            {
                formWikiEdit.WikiPageCur = wikiPageList[selectedIndex];
                formWikiEdit.OwnerForm = this.OwnerForm;
                formWikiEdit.ShowDialog();

                if (formWikiEdit.HasSaved)
                {
                    Close();
                    return;
                }
            }

            LoadPageList();
            LoadPage(wikiPageList[selectedIndex]);

            wikiPagesGrid.SetSelected(selectedIndex, true);
            wikiPagesGrid.Focus();
        }
        
        /// <summary>
        /// Delete the currently selected draft.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = wikiPagesGrid.GetSelectedIndex();
            if (selectedIndex < 0)
            {
                return;
            }

            // Ask the user for confirmation before deleting the page.
            var result =
                MessageBox.Show(
                    Translation.Language.WikiDeleteDraft,
                    Translation.Language.WikiDrafts,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;
            
            // Delete the page.
            try
            {
                WikiPages.DeleteDraft(wikiPageList[selectedIndex]);
            }
            catch (Exception ex)
            {
                // Should never happen because we are only ever editing drafts here.
                MessageBox.Show(
                    ex.Message,
                    Translation.Language.WikiDrafts,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Reload the list of drafts.
            LoadPageList();
            if (selectedIndex >= wikiPageList.Count)
            {
                selectedIndex = wikiPageList.Count - 1;
            }

            // If there are no drafts left close the form.
            if (wikiPageList.Count < 1)
            {
                Close();
                return;
            }

            LoadPage(wikiPageList[selectedIndex]);

            wikiPagesGrid.SetSelected(selectedIndex, true);
            wikiPagesGrid.Focus();
        }
    }
}
