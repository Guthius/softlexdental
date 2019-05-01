using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiListItemEdit : FormBase
    {
        DataTable itemDataTable;

        public long ItemNum;
        public bool IsNew;

        /// <summary>
        /// Name of the wiki list.
        /// </summary>
        public string WikiListCurName;

        /// <summary>
        /// A list of all possible column headers for the current wiki list. 
        /// Each header contains additional information (e.g. PickList) that can be useful.
        /// </summary>
        public List<WikiListHeaderWidth> ListColumnHeaders;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiListItemEdit"/> class.
        /// </summary>
        public FormWikiListItemEdit() => InitializeComponent();

        /// <summary>
        /// Populates the grid.
        /// </summary>
        void FillGrid()
        {
            itemDataGrid.BeginUpdate();
            itemDataGrid.Columns.Clear();
            itemDataGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnColumn, 200));
            itemDataGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnValue, 400, isEditable: true));
            itemDataGrid.Rows.Clear();

            for (int i = 1; i < itemDataTable.Columns.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(itemDataTable.Columns[i].ColumnName);
                row.Cells.Add(itemDataTable.Rows[0][i].ToString());

                if (i == 0)
                {
                    row.ColorBackG = Color.Gray;
                }

                itemDataGrid.Rows.Add(row);
            }

            itemDataGrid.EndUpdate();
            itemDataGrid.Title = Translation.Language.WikiEditListItem;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiListEdit_Load(object sender, EventArgs e)
        {
            itemDataTable = WikiLists.GetItem(WikiListCurName, ItemNum);

            Text = 
                Translation.Language.WikiEditWikiListItem  + " - " + 
                itemDataTable.Columns[0] + " " + 
                itemDataTable.Rows[0][0].ToString();

            FillGrid();
        }

        /// <summary>
        /// When entering a cell with a picklist associated with it, populate the picklist combobox
        /// and display it so the user can select a value from the list.
        /// </summary>
        void itemDataGrid_CellEnter(object sender, ODGridClickEventArgs e)
        {
            if (ListColumnHeaders == null || ListColumnHeaders.Count <= e.Row + 1 || string.IsNullOrEmpty(ListColumnHeaders[e.Row + 1].PickList))
            {
                pickListComboBox.Visible = false;
                return;
            }
            pickListComboBox.Items.Clear();

            var currentValue = itemDataTable.Rows[0][e.Row + 1].ToString();

            var pickListOptions = ListColumnHeaders[e.Row + 1].PickList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string option in pickListOptions)
            {
                pickListComboBox.Items.Add(option);
                if (option.Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    pickListComboBox.SelectedItem = option;
                }
            }

            // Determine the correct position for the combobox.
            pickListComboBox.Location =
                new Point(
                    itemDataGrid.Rows[e.Row].RowLoc + itemDataGrid.Location.Y + ODGrid.HeaderHeight + itemDataGrid.TitleHeight + 1,
                    itemDataGrid.Columns[0].Width + itemDataGrid.Location.X + 1);

            // Set the correct size of the combobox and display it.
            pickListComboBox.Width = itemDataGrid.Columns[1].Width + 1;
            pickListComboBox.Height = itemDataGrid.Rows[e.Row].RowHeight - 1;
            pickListComboBox.Visible = true;
            pickListComboBox.Focus();
            pickListComboBox.DroppedDown = true;
            pickListComboBox.Tag = e.Row + 1;
        }

        /// <summary>
        /// Cleans up the specified value so it can be safely used.
        /// </summary>
        /// <param name="value">The value to clean up.</param>
        /// <returns>The cleaned up value.</returns>
        string Clean(string value) => value.Replace("\r", "").Replace("\n", "\r\n");
        
        /// <summary>
        /// Commit the value of a item column back to the data table when the cell loses focus.
        /// </summary>
        void itemDataGrid_CellLeave(object sender, ODGridClickEventArgs e)
        {
            for (int i = 0; i < itemDataGrid.Rows.Count; i++)
            {
                itemDataTable.Rows[0].BeginEdit();
                itemDataTable.Rows[0][i + 1] = Clean(itemDataGrid.Rows[i].Cells[1].Text);
                itemDataTable.Rows[0].EndEdit();
            }
        }

        /// <summary>
        /// Deletes the item and closes the form.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.WikiListSetup)) return;

            var result =
                MessageBox.Show(
                    Translation.Language.WikiDeleteItemAndReferences,
                    Translation.Language.WikiEditWikiListItem,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            WikiLists.DeleteItem(WikiListCurName, ItemNum);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// When the user selects a value from the pick list commit it to the data table and refresh the grid.
        /// </summary>
        void pickListComboBox_Leave(object sender, EventArgs e)
        {
            pickListComboBox.Visible = false;
            
            if (pickListComboBox.Tag != null && pickListComboBox.SelectedItem != null)
            {
                itemDataTable.Rows[0][(int)pickListComboBox.Tag] = pickListComboBox.SelectedItem;

                pickListComboBox.Tag = null;
                pickListComboBox.SelectedItem = null;

                FillGrid();
            }
        }

        /// <summary>
        /// Creates the item and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            WikiLists.UpdateItem(WikiListCurName, itemDataTable);

            DialogResult = DialogResult.OK;
        }
    }
}