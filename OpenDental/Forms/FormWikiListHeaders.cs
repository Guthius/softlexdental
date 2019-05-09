using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiListHeaders : FormBase
    {
        string                      wikiListName;
        List<WikiListHeaderWidth>   columnHeadersList;
        int                         columnHeaderIndex   = -1;
        int                         pickListIndex       = -1;
        List<string>                pickList            = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiListHeaders"/> class.
        /// </summary>
        /// <param name="wikiListName">The name of the wiki list.</param>
        public FormWikiListHeaders(string wikiListName)
        {
            InitializeComponent();

            this.wikiListName = wikiListName;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiListHeaders_Load(object sender, EventArgs e)
        {
            columnHeadersList = 
                WikiListHeaderWidths.GetForList(wikiListName)
                    .Select(tableHeader => tableHeader.Copy())
                        .ToList();

            FillGrid();
        }

        /// <summary>
        /// Each row of data becomes a column in the grid. This pattern is only used in a few locations.
        /// </summary>
        void FillGrid()
        {
            columnHeadersGrid.BeginUpdate();
            columnHeadersGrid.Columns.Clear();
            columnHeadersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnColumnName, 100, isEditable: true));
            columnHeadersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnWidth, 0, isEditable: true));

            columnHeadersGrid.Rows.Clear();
            for (int i = 0; i < columnHeadersList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(columnHeadersList[i].ColName);
                row.Cells.Add(columnHeadersList[i].ColWidth.ToString());
                columnHeadersGrid.Rows.Add(row);
            }

            columnHeadersGrid.EndUpdate();
        }

        /// <summary>
        /// Populate the picklist grid with the values from the picklist of the selected column.
        /// </summary>
        void FillGridPickList()
        {
            pickListGrid.BeginUpdate();
            pickListGrid.Columns.Clear();
            pickListGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnInputText, 100, isEditable: true));
            pickListGrid.Rows.Clear();

            if (columnHeadersGrid.SelectedCell.Y != -1)
            {
                for (int i = 0; i < pickList.Count; i++)
                {
                    var row = new ODGridRow();
                    row.Cells.Add(pickList[i]);
                    pickListGrid.Rows.Add(row);
                }
            }

            pickListGrid.EndUpdate();
            if (pickListIndex > -1 && pickList.Count > pickListIndex)
            {
                pickListGrid.SetSelected(pickListIndex, true);
            }
        }

        /// <summary>
        /// When the user selects a column from the grid, populate the picklist grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void columnHeadersGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (columnHeaderIndex > 0)
            {
                columnHeadersList[columnHeaderIndex].PickList = string.Join("\r\n", pickList);
            }

            columnHeaderIndex = e.Row;

            pickList = columnHeadersList[e.Row].PickList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            FillGridPickList();
        }

        /// <summary>
        /// Used to store the currently selected cell in the secondary grid.  
        /// SelectedCell and Selected don't behave correctly when you click away or the cell is editable.
        /// </summary>
        void pickListGrid_CellEnter(object sender, ODGridClickEventArgs e)
        {
            pickListIndex = e.Row;
        }

        /// <summary>
        /// Update the value of a item from the picklist.
        /// </summary>
        void pickListGrid_CellLeave(object sender, ODGridClickEventArgs e)
        {
            var addedListItem = pickListGrid.Rows[e.Row].Cells[0].Text;
            if (e.Row < pickList.Count)
            {
                pickList[e.Row] = addedListItem;
            }
        }

        /// <summary>
        /// Adds a new item to the picklist of the selected header.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var inputBox = new InputBox(Translation.Language.WIkiNewPickListOption))
            {
                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    pickList.Add(inputBox.textResult.Text);
                    FillGridPickList();
                }
            }
        }

        /// <summary>
        /// Removes the selected item from the picklist.
        /// </summary>
        void removeButton_Click(object sender, EventArgs e)
        {
            if (pickListIndex > -1)
            {
                pickList.RemoveAt(pickListIndex);
                pickListIndex--;

                FillGridPickList();
            }
        }

        /// <summary>
        /// Validates and saves the headers and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            // Save the picklist to the currently selected column.
            if (columnHeaderIndex > 0) columnHeadersList[columnHeaderIndex].PickList = string.Join("\r\n", pickList);

            // Set primary key to correct name. Prevents exceptions from occuring when user tries to rename PK.
            columnHeadersGrid.Rows[0].Cells[0].Text = wikiListName + "Num";

            // Validate the column names.
            for (int i = 0; i < columnHeadersGrid.Rows.Count; i++)
            {
                if (Regex.IsMatch(columnHeadersGrid.Rows[i].Cells[0].Text, @"^\d"))
                {
                    MessageBox.Show(
                        Translation.Language.WikiColumnCannotStartWithNumbers,
                        Translation.Language.WikiEditWikiListHeaders, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
                if (Regex.IsMatch(columnHeadersGrid.Rows[i].Cells[0].Text, @"\s"))
                {
                    MessageBox.Show(
                        Translation.Language.WikiColumnCannotContainSpaces,
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
                if (Regex.IsMatch(columnHeadersGrid.Rows[i].Cells[0].Text, @"\W"))
                {
                    MessageBox.Show(
                        Translation.Language.WikiColumnCannotContainSpecialCharacters,
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            // Check for reserved words.
            for (int i = 0; i < columnHeadersGrid.Rows.Count; i++)
            {
                var columnHeader = columnHeadersGrid.Rows[i].Cells[0].Text;
                if (DbHelper.isMySQLReservedWord(columnHeader))
                {
                    MessageBox.Show(
                        string.Format(Translation.Language.WikiColumnName0IsReservedWord, columnHeader),
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            // Check for duplicate columns.
            var columnHeaderNames = new List<string>();
            for (int i = 0; i < columnHeadersGrid.Rows.Count; i++)
            {
                var columnHeader = columnHeadersGrid.Rows[i].Cells[0].Text;
                if (columnHeaderNames.Contains(columnHeader))
                {
                    MessageBox.Show(
                        Translation.Language.WikiDuplicateColumnNameDetected + ": " + columnHeader,
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
                columnHeaderNames.Add(columnHeadersGrid.Rows[i].Cells[0].Text);
            }

            // Validate column widths.
            for (int i = 0; i < columnHeadersGrid.Rows.Count; i++)
            {
                if (Regex.IsMatch(columnHeadersGrid.Rows[i].Cells[1].Text, @"\D"))
                {
                    MessageBox.Show(
                        Translation.Language.WikiColumnWidthsMustOnlyContainPositiveIntegers,
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if (columnHeadersGrid.Rows[i].Cells[1].Text.Contains("-") || 
                    columnHeadersGrid.Rows[i].Cells[1].Text.Contains(".") ||
                    columnHeadersGrid.Rows[i].Cells[1].Text.Contains(","))
                {
                    MessageBox.Show(
                        Translation.Language.WikiColumnWidthsMustOnlyContainPositiveIntegers,
                        Translation.Language.WikiEditWikiListHeaders,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            for (int i = 0; i < columnHeadersList.Count; i++)
            {
                columnHeadersList[i].ColName = PIn.String(columnHeadersGrid.Rows[i].Cells[0].Text);
                columnHeadersList[i].ColWidth = PIn.Int(columnHeadersGrid.Rows[i].Cells[1].Text);
            }

            // Save data to database.
            try
            {
                WikiListHeaderWidths.UpdateNamesAndWidths(wikiListName, columnHeadersList);
            }
            catch (Exception ex) // Will throw exception if table schema has changed since the window was opened.
            {
                MessageBox.Show(
                    ex.Message,
                    Translation.Language.WikiEditWikiListHeaders,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
            }

            DataValid.SetInvalid(InvalidType.Wiki);
            DialogResult = DialogResult.OK;
        }
    }
}