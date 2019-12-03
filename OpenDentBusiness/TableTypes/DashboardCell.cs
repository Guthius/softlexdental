using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Each DashboardLayout can include multiple DashboardCell(s). 
    /// DashboardLayout and DashboardCell work in conjunction to form the dashboard layout.
    /// </summary>
    public class DashboardCell
    {
        public long DashboardCellNum;

        ///<summary>FK to dashboardlayout.DashboardLayoutNum. This foreign key object will include the 0 based DashboardTabOrder, which is used to place this DashboardCell.</summary>
        public long DashboardLayoutNum;

        ///<summary>The row to which this DashboardCell belongs. 0 based.</summary>
        public int CellRow;

        ///<summary>The column to which this DashboardCell belongs. 0 based.</summary>
        public int CellColumn;

        ///<summary>Determines what type of control will be docked in this cell.</summary>
        public DashboardCellType CellType;

        ///<summary>Typically a serialized string that the control will accept in order to change view attributes.</summary>
        public string CellSettings;

        ///<summary>Not used yet. Timestamp at which the cached data behind this cell was last retrieved.</summary>
        public DateTime LastQueryTime;

        ///<summary>Not used yet. Cached data behind this cell.</summary>
        public string LastQueryData;

        ///<summary>Not used yet. Frequency at which the cached data behind this cell should be retrieved.</summary>
        public int RefreshRateSeconds;
    }

    /// <summary>
    /// Represents each type of graph which is available in graphic reports. 
    /// Will be stored as string in DashboardCell unless specified otherwise.
    /// </summary>
    public enum DashboardCellType
    {
        NotDefined,
        ProductionGraph,
        IncomeGraph,
        AccountsReceivableGraph,
        NewPatientsGraph,
        BrokenApptGraph,
    }
}
