using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace OpenDental
{
    public partial class FormWikiListAdvancedSearch : FormBase
    {
        List<WikiListHeaderWidth> columnHeaders;

        /// <summary>
        /// Gets the indices of the selected columns.
        /// </summary>
        public int[] SelectedColumnIndices => gridMain.SelectedIndices;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiListAdvancedSearch"/> class.
        /// </summary>
        /// <param name="columnHeaders"></param>
        public FormWikiListAdvancedSearch(List<WikiListHeaderWidth> columnHeaders)
        {
            InitializeComponent();
            this.columnHeaders = columnHeaders;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiListAdvancedSearch_Load(object sender, EventArgs e) => FillGrid();

        /// <summary>
        /// Populates the grid with the current Wiki's column headers.
        /// </summary>
        void FillGrid()
        {
            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            gridMain.Columns.Add(new ODGridColumn("", 80));
            gridMain.Rows.Clear();

            foreach (var columnHeader in columnHeaders)
            {
                gridMain.Rows.Add(new ODGridRow(columnHeader.ColName));
            }

            gridMain.EndUpdate();
        }
    }
}