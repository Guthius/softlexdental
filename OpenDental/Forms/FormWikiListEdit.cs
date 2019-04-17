using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiListEdit : FormBase
    {
        /// <summary>
        /// Name of the wiki list being manipulated. This does not include the "wikilist" prefix. i.e. "networkdevices" not "wikilistnetworkdevices"
        /// </summary>
        public string WikiListCurName;
        public bool IsNew;

        DataTable listDataTable;


        private WikiListHist _wikiListOld;
        private bool _isEdited;
        private int[] _arraySearchColIdxs;
        
        /// <summary>
        /// A list of all possible column headers for the current wiki list. 
        /// Each header contains additional information (e.g. PickList) that can be useful.
        /// </summary>
        List<WikiListHeaderWidth> _listColumnHeaders = new List<WikiListHeaderWidth>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiListEdit"/> class.
        /// </summary>
        public FormWikiListEdit() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiListEdit_Load(object sender, EventArgs e)
        {
            SetFilterControlsAndAction(() => FillGrid(), (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds, searchTextBox);

            // Check whether it is is a existing list or not.
            if (!WikiLists.CheckExists(WikiListCurName))
            {
                IsNew = true;

                WikiLists.CreateNewWikiList(WikiListCurName);
            }

            // Get the wiki list.
            listDataTable = WikiLists.GetByName(WikiListCurName);


            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            _wikiListOld = WikiListHists.GenerateFromName(WikiListCurName, Security.CurUser.UserNum);
            if (_wikiListOld == null)
            {
                _wikiListOld = new WikiListHist();
            }

            highlightRadioButton.Checked = true;
            filterRadioButton.Checked = false;
            FillGrid();
            ActiveControl = searchTextBox;//start in search box.
        }

        /// <summary>
        /// Fills the grid with the contents of the corresponding wiki list table in the database.
        /// After filling the grid, FilterGrid() will get invoked to apply any advanced search options.
        /// </summary>
        private void FillGrid()
        {
            if (listDataTable.Rows.Count > 0)
            {
                if (_listColumnHeaders.Count != listDataTable.Columns.Count)
                {
                    WikiListHeaderWidths.RefreshCache();
                    listDataTable = WikiLists.GetByName(WikiListCurName);
                    _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);

                    if (_listColumnHeaders.Count != listDataTable.Columns.Count)
                    {
                        MessageBox.Show(
                            "Unable to open the wiki list.",
                            "Edit Wiki List", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                        return;
                    }
                }
            }

            itemsGrid.BeginUpdate();
            itemsGrid.Columns.Clear();

            for (int c = 0; c < _listColumnHeaders.Count; c++)
            {
                var col = new ODGridColumn(_listColumnHeaders[c].ColName, _listColumnHeaders[c].ColWidth + 20, false);
                itemsGrid.Columns.Add(col);
            }


            itemsGrid.Rows.Clear();
            for (int i = 0; i < listDataTable.Rows.Count; i++)
            {
                var row = new ODGridRow();
                for (int c = 0; c < listDataTable.Columns.Count; c++)
                {
                    row.Cells.Add(listDataTable.Rows[i][c].ToString());
                }
                itemsGrid.Rows.Add(row);
                itemsGrid.Rows[i].Tag = i;
            }
            itemsGrid.EndUpdate();
            itemsGrid.Title = WikiListCurName;

            FilterGrid();
        }

        ///<summary>Visually filters gridMain.  Tag is preserved so that double clicking and editing can still work.</summary>
        private void FilterGrid()
        {
            filterRadioButton.Enabled = true;
            highlightRadioButton.Enabled = true;
            searchLabel.Text = Lan.g(this, "Search");
            searchLabel.ForeColor = Color.Black;
            List<string> searchTerms = searchTextBox.Text.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();
            #region Advanced Search
            if (_arraySearchColIdxs != null && _arraySearchColIdxs.Length > 0)
            {//adv search has been used, search specific columns selected
                searchLabel.Text = Lan.g(this, "Advanced Search");
                searchLabel.ForeColor = Color.Red;
                filterRadioButton.Checked = false;
                filterRadioButton.Enabled = false;
                highlightRadioButton.Checked = false;
                highlightRadioButton.Enabled = false;
                if (searchTextBox.Text == "")
                {
                    return;
                }
                itemsGrid.BeginUpdate();
                itemsGrid.Rows.Clear();
                bool hasSearchText = false;
                for (int i = 0; i < listDataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < _arraySearchColIdxs.Length; j++)
                    { //loop through the selected columns only (very short loop)
                      //Search through the corresponding cells for any partial matches (split by spaces).
                        foreach (string searchWord in searchTerms)
                        {
                            if (listDataTable.Rows[i].ItemArray[_arraySearchColIdxs[j]].ToString().ToUpper().Contains(searchWord.ToUpper()))
                            {//if the cell contains the searched text
                                hasSearchText = true;
                                break;
                            }
                        }
                        if (hasSearchText)
                        {
                            ODGridRow row = new ODGridRow();
                            for (int k = 0; k < listDataTable.Columns.Count; k++)
                            {
                                row.Cells.Add(listDataTable.Rows[i][k].ToString());
                            }
                            row.Tag = i;
                            itemsGrid.Rows.Add(row);
                            hasSearchText = false;
                            break;
                        }
                    }//end j
                }//end i
            }
            #endregion
            #region Highlight
            else if (highlightRadioButton.Checked)
            {//highlight radiobutton checked
                if (searchTextBox.Text == "")
                {
                    return;
                }
                bool isScrollSet = false;
                for (int i = 0; i < listDataTable.Rows.Count; i++)
                {
                    itemsGrid.Rows[i].ColorBackG = Color.White;
                    List<string> listCellVals = listDataTable.Rows[i].ItemArray.Select(x => x.ToString().ToUpper()).ToList();
                    foreach (string searchWord in searchTerms)
                    {
                        //If any of the cell values contains the current search word, color the row yellow and move on.
                        if (listCellVals.Any(x => x.Contains(searchWord.ToUpper())))
                        {
                            itemsGrid.Rows[i].ColorBackG = Color.Yellow;
                            if (!isScrollSet)
                            {//scroll to the first match in the list.
                                itemsGrid.ScrollToIndex(i);
                                isScrollSet = true;
                            }
                            break;
                        }
                    }
                }//end i
            }
            #endregion
            #region Filter
            else
            {//filter radiobutton checked
                if (searchTextBox.Text == "")
                {
                    return;
                }
                itemsGrid.BeginUpdate();
                itemsGrid.Rows.Clear();
                ODGridRow row;
                for (int i = 0; i < listDataTable.Rows.Count; i++)
                {
                    List<string> listCellVals = listDataTable.Rows[i].ItemArray.Select(x => x.ToString().ToUpper()).ToList();
                    foreach (string searchWord in searchTerms)
                    {
                        if (listCellVals.Any(x => x.Contains(searchWord.ToUpper())))
                        {
                            row = new ODGridRow();
                            for (int j = 0; j < listDataTable.Columns.Count; j++)
                            {
                                row.Cells.Add(listDataTable.Rows[i][j].ToString());
                            }
                            row.Tag = i;
                            itemsGrid.Rows.Add(row);
                            break;
                        }
                    }
                }//end i
                #endregion
            }
            itemsGrid.EndUpdate();
        }


        /// <summary>
        /// Open the dialog to edit a item when the user double clicks on a item in the grid.
        /// </summary>
        void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formWikiListItemEdit = new FormWikiListItemEdit())
            {
                formWikiListItemEdit.WikiListCurName = WikiListCurName;
                formWikiListItemEdit.ItemNum = PIn.Long(listDataTable.Rows[(int)itemsGrid.Rows[e.Row].Tag][0].ToString());
                formWikiListItemEdit.ListColumnHeaders = _listColumnHeaders;

                if (formWikiListItemEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }


            SetIsEdited();

            // Reload the table data.
            listDataTable = WikiLists.GetByName(WikiListCurName);
            FillGrid();
        }


       
        ///<summary>Set the _isEdited bool to true and saves a copy of the list. This only happens once. This prevents spamming of updates.</summary>
        private void SetIsEdited()
        {
            if (_isEdited || IsNew)
            {//Dont save a wikiListHist entry if this is a new list, or we have already saved an entry prior to a previous edit.
                return;
            }
            _wikiListOld.WikiListHistNum = WikiListHists.Insert(_wikiListOld);
            _isEdited = true;
        }







        void advSearchButton_Click(object sender, EventArgs e)
        {
            var columnHeaderWidths = WikiListHeaderWidths.GetForList(WikiListCurName);

            using (var formWikiListAdvancedSearch = new FormWikiListAdvancedSearch(columnHeaderWidths))
            {
                if (formWikiListAdvancedSearch.ShowDialog() == DialogResult.OK)
                {
                    _arraySearchColIdxs = formWikiListAdvancedSearch.SelectedColumnIndices;

                    FillGrid();
                }
            }

            ActiveControl = searchTextBox;
        }

        /// <summary>
        /// Clears the advanced search options.
        /// </summary>
        void advSearchClearButton_Click(object sender, EventArgs e)
        {
            _arraySearchColIdxs = new int[0];
            highlightRadioButton.Checked = true;
            searchTextBox.Clear();
            ActiveControl = searchTextBox;
            FillGrid();
        }

        /// <summary>
        /// Enable or disable highlighting of search results in the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void highlightRadioButton_CheckedChanged(object sender, EventArgs e) => FillGrid();

        /// <summary>
        /// Enable or disable filtering of the items in the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void filterRadioButton_CheckedChanged(object sender, EventArgs e) => FillGrid();

        void listRenameButton_Click(object sender, EventArgs e)
        {
            //Logic copied from FormWikiLists.butAdd_Click()---------------------
            InputBox inputListName = new InputBox("New List Name");
            inputListName.ShowDialog();
            if (inputListName.DialogResult != DialogResult.OK)
            {
                return;
            }


            //Format input as it would be saved in the database--------------------------------------------
            inputListName.textResult.Text = inputListName.textResult.Text.ToLower().Replace(" ", "");
            //Validate list name---------------------------------------------------------------------------

            if (DbHelper.isMySQLReservedWord(inputListName.textResult.Text))
            {
                //Can become an issue when retrieving column header names.
                MsgBox.Show(this, "List name is a reserved word in MySQL.");
                return;
            }

            if (inputListName.textResult.Text == "")
            {
                MsgBox.Show(this, "List name cannot be blank.");
                return;
            }
            if (WikiLists.CheckExists(inputListName.textResult.Text))
            {
                MsgBox.Show(this, "List name already exists.");
                return;
            }
            try
            {
                WikiLists.Rename(WikiListCurName, inputListName.textResult.Text);
                SetIsEdited();
                WikiListHists.Rename(WikiListCurName, inputListName.textResult.Text);
                WikiListCurName = inputListName.textResult.Text;
                listDataTable = WikiLists.GetByName(WikiListCurName);
                FillGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        void listHistoryButton_Click(object sender, EventArgs e)
        {
            using (var formWikiListhistory = new FormWikiListHistory())
            {
                formWikiListhistory.ListNameCur = WikiListCurName;
                formWikiListhistory.ShowDialog();

                if (!formWikiListhistory.IsReverted)
                {
                    return;
                }
            }

            //Reversion has already saved a copy of the current revision.
            _wikiListOld = WikiListHists.GenerateFromName(WikiListCurName, Security.CurUser.UserNum);
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
            _isEdited = false;
            IsNew = false;
        }

        void columnLeftButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;

            if (itemsGrid.SelectedCell.X == -1)
            {
                return;
            }

            SetIsEdited();
            Point pointNewSelectedCell = itemsGrid.SelectedCell;
            pointNewSelectedCell.X = Math.Max(1, pointNewSelectedCell.X - 1);
            WikiLists.ShiftColumnLeft(WikiListCurName, listDataTable.Columns[itemsGrid.SelectedCell.X].ColumnName);
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
            itemsGrid.SetSelected(pointNewSelectedCell);
        }

        void columnRightButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;

            if (itemsGrid.SelectedCell.X == -1)
            {
                return;
            }

            SetIsEdited();
            Point pointNewSelectedCell = itemsGrid.SelectedCell;
            pointNewSelectedCell.X = Math.Min(itemsGrid.Columns.Count - 1, pointNewSelectedCell.X + 1);
            WikiLists.ShiftColumnRight(WikiListCurName, listDataTable.Columns[itemsGrid.SelectedCell.X].ColumnName);
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
            itemsGrid.SetSelected(pointNewSelectedCell);
        }

        void columnEditButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;

            FormWikiListHeaders FormWLH = new FormWikiListHeaders(WikiListCurName);
            FormWLH.ShowDialog();
            if (FormWLH.DialogResult != DialogResult.OK)
            {
                return;
            }
            SetIsEdited();
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
        }

        void columnAddButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;
            
            SetIsEdited();
            WikiLists.AddColumn(WikiListCurName);
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
        }

        void columnDeleteButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup))
            {//gives a message box if no permission
                return;
            }
            if (itemsGrid.SelectedCell.X == -1)
            {
                MsgBox.Show(this, "Select cell in column to be deleted first.");
                return;
            }
            if (!WikiLists.CheckColumnEmpty(WikiListCurName, listDataTable.Columns[itemsGrid.SelectedCell.X].ColumnName))
            {
                MsgBox.Show(this, "Column cannot be deleted because it contains data.");
                return;
            }
            SetIsEdited();
            WikiLists.DeleteColumn(WikiListCurName, listDataTable.Columns[itemsGrid.SelectedCell.X].ColumnName);
            listDataTable = WikiLists.GetByName(WikiListCurName);
            _listColumnHeaders = WikiListHeaderWidths.GetForList(WikiListCurName);
            FillGrid();
        }

        void rowAddButton_Click(object sender, EventArgs e)
        {
            using (var formWikiListItemEdit = new FormWikiListItemEdit())
            {
                formWikiListItemEdit.WikiListCurName = WikiListCurName;
                formWikiListItemEdit.ItemNum = WikiLists.AddItem(WikiListCurName);
                formWikiListItemEdit.ListColumnHeaders = _listColumnHeaders;

                if (formWikiListItemEdit.ShowDialog(this) != DialogResult.OK)
                {
                    WikiLists.DeleteItem(formWikiListItemEdit.WikiListCurName, formWikiListItemEdit.ItemNum); // TODO: delete new item because dialog was not OK'ed. Wtf?
                    return;
                }

                long itemNum = formWikiListItemEdit.ItemNum;//capture itemNum to prevent marshall-by-reference warning

                SetIsEdited();

                listDataTable = WikiLists.GetByName(WikiListCurName);

                FillGrid();
                for (int i = 0; i < itemsGrid.Rows.Count; i++)
                {
                    if (itemsGrid.Rows[i].Cells[0].Text == itemNum.ToString())
                    {
                        itemsGrid.Rows[i].ColorBackG = Color.FromArgb(255, 255, 128);
                        itemsGrid.ScrollToIndex(i);
                    }
                }
            }
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;
            
            if (itemsGrid.Rows.Count > 0)
            {
                MessageBox.Show(
                    "Cannot delete a non-empty list.  Remove all items first and try again.",
                    "Edit Wiki List",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var result =
                MessageBox.Show(
                    "Delete this entire list and all references to it?",
                    "Edit Wiki List", 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            SetIsEdited();

            WikiLists.DeleteList(WikiListCurName);

            DialogResult = DialogResult.OK;
        }
    }
}