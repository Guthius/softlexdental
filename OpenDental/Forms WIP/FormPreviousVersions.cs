using OpenDental.UI;
using OpenDentBusiness;
using System;

namespace OpenDental
{
    public partial class FormPreviousVersions : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormPreviousVersions"/> class.
        /// </summary>
        public FormPreviousVersions() => InitializeComponent();
     
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormPreviousVersions_Load(object sender, EventArgs e) => LoadUpdateHistory();
        
        /// <summary>
        /// Loads the update history and populates the grid.
        /// </summary>
        void LoadUpdateHistory()
        {
            historyGrid.BeginUpdate();
            historyGrid.Columns.Clear();
            historyGrid.Columns.Add(new ODGridColumn("Version", 117));
            historyGrid.Columns.Add(new ODGridColumn("Date", 117));
            historyGrid.Rows.Clear();

            var updateHistoryList = UpdateHistories.GetAll();
            foreach (var updateHistory in updateHistoryList)
            {
                var row = new ODGridRow();

                row.Cells.Add(updateHistory.ProgramVersion);
                row.Cells.Add(updateHistory.DateTimeUpdated.ToString());

                historyGrid.Rows.Add(row);
            }
            historyGrid.EndUpdate();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}