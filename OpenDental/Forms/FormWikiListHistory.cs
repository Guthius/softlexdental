using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.IO;
using System.Xml;

namespace OpenDental
{
    public partial class FormWikiListHistory : FormBase
    {
        ///<summary>Set from outside this form to load all appropriate data into the form during Form_Load().</summary>
        public string ListNameCur;

        /// <summary>
        /// Gets a value indicating whether the list has been reverted to a old revision.
        /// </summary>
        public bool IsReverted { get; private set; }



        private List<WikiListHist> _listWikiListHists;
        private DataTable _tableCur;
        private DataTable _tableOld;


        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiListHistory"/> class.
        /// </summary>
        public FormWikiListHistory() => InitializeComponent();
        

        private void FormWikiListHistory_Load(object sender, EventArgs e)
        {
            FillGridMain();
            if (gridMain.Rows.Count > 0)
            {
                gridMain.SetSelected(gridMain.Rows.Count - 1, true);
            }
            Text = "Wiki List History - " + ListNameCur;
            oldRevisionGrid.Title = "Old Revision";
            currentRevisionGrid.Title = "Current Revision";
            FillGridOld();
            FillGridCur();
        }

        private void FillGridMain()
        {
            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            gridMain.Columns.Add(new ODGridColumn(Lan.g(this, "User"), 70));
            gridMain.Columns.Add(new ODGridColumn(Lan.g(this, "Saved"), 80));
            gridMain.Rows.Clear();

            _listWikiListHists = WikiListHists.GetByName(ListNameCur);
            for (int i = 0; i < _listWikiListHists.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(Userods.GetName(_listWikiListHists[i].UserNum));
                row.Cells.Add(_listWikiListHists[i].DateTimeSaved.ToString());
                gridMain.Rows.Add(row);
            }

            gridMain.EndUpdate();
        }

        /// <summary></summary>
        private void FillGridOld()
        {
            List<WikiListHeaderWidth> colHeaderWidths = new List<WikiListHeaderWidth>();

            _tableOld = new DataTable();
            if (gridMain.GetSelectedIndex() > -1)
            {
                colHeaderWidths = WikiListHeaderWidths.GetFromListHist(_listWikiListHists[gridMain.GetSelectedIndex()]);
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(_listWikiListHists[gridMain.GetSelectedIndex()].ListContent)))
                {
                    try
                    {
                        _tableOld.ReadXml(xmlReader);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            "Corruption detected in the Old Revision table. Partial data will be displayed. Please call us for support.",
                            "Wiki List History",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }

            oldRevisionGrid.BeginUpdate();
            oldRevisionGrid.Columns.Clear();
            for (int c = 0; c < _tableOld.Columns.Count; c++)
            {
                int colWidth = 100;
                foreach (WikiListHeaderWidth colHead in colHeaderWidths)
                {
                    if (colHead.ColName == _tableOld.Columns[c].ColumnName)
                    {
                        colWidth = colHead.ColWidth;
                        break;
                    }
                }
                var col = new ODGridColumn(_tableOld.Columns[c].ColumnName, colWidth);
                oldRevisionGrid.Columns.Add(col);
            }

            oldRevisionGrid.Rows.Clear();
            for (int i = 0; i < _tableOld.Rows.Count; i++)
            {
                var row = new ODGridRow();
                for (int c = 0; c < _tableOld.Columns.Count; c++)
                {
                    row.Cells.Add(_tableOld.Rows[i][c].ToString());
                }
                oldRevisionGrid.Rows.Add(row);
                oldRevisionGrid.Rows[i].Tag = i;
            }
            oldRevisionGrid.EndUpdate();
        }


        private void FillGridCur()
        {
            List<WikiListHeaderWidth> listColHeaderWidths = WikiListHeaderWidths.GetForList(ListNameCur);
            _tableCur = WikiLists.GetByName(ListNameCur);
            currentRevisionGrid.BeginUpdate();
            currentRevisionGrid.Columns.Clear();
            ODGridColumn col;
            for (int c = 0; c < _tableCur.Columns.Count; c++)
            {
                int colWidth = 100;//100 = default value in case something is malformed in the database.
                foreach (WikiListHeaderWidth colHead in listColHeaderWidths)
                {
                    if (colHead.ColName == _tableCur.Columns[c].ColumnName)
                    {
                        colWidth = colHead.ColWidth;
                        break;
                    }
                }
                col = new ODGridColumn(_tableCur.Columns[c].ColumnName, colWidth);
                currentRevisionGrid.Columns.Add(col);
            }
            currentRevisionGrid.Rows.Clear();
            ODGridRow row;
            for (int i = 0; i < _tableCur.Rows.Count; i++)
            {
                row = new ODGridRow();
                for (int c = 0; c < _tableCur.Columns.Count; c++)
                {
                    row.Cells.Add(_tableCur.Rows[i][c].ToString());
                }
                currentRevisionGrid.Rows.Add(row);
                currentRevisionGrid.Rows[i].Tag = i;
            }
            currentRevisionGrid.EndUpdate();
        }

        private void gridMain_Click(object sender, EventArgs e)
        {
            if (gridMain.SelectedIndices.Length < 1)
            {
                return;
            }
            FillGridOld();
            gridMain.Focus();
        }




        void revertButton_Click(object sender, EventArgs e)
        {
            if (gridMain.GetSelectedIndex() == -1) return;

            var result =
                MessageBox.Show(
                    "Revert list to currently selected revision?",
                    "Wiki List History", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                WikiListHists.RevertFrom(_listWikiListHists[gridMain.GetSelectedIndex()], Security.CurUser.UserNum);
            }
            catch 
            {
                MessageBox.Show(
                    "There was an error when trying to revert changes. Please call us for support.",
                    "Wiki List History",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            FillGridMain();
            gridMain.SetSelected(false);
            gridMain.SetSelected(gridMain.Rows.Count - 1, true);
            gridMain.ScrollToEnd();

            FillGridOld();
            FillGridCur();

            IsReverted = true;
        }
    }
}