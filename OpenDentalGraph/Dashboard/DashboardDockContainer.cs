using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDentalGraph
{
    /// <summary>
    /// Provides a method to create a DashboardDockContainer. 
    /// All controls that want to be available for docking in DashboardCellCtrl should implement this interface.
    /// </summary>
    public interface IDashboardDockContainer
    {
        DashboardDockContainer CreateDashboardDockContainer(ODTable dbItem);

        DashboardCellType GetCellType();

        string GetCellSettings();
    }

    /// <summary>
    /// Helper class used by DashboardCellCtrl. Holds all necessary input needed for docking to DashboardCellCtrl.
    /// </summary>
    public class DashboardDockContainer
    {
        public Control Control { get; }

        public EventHandler OnEditClick { get; }

        public EventHandler OnEditOk { get; }

        public EventHandler OnEditCancel { get; }

        public EventHandler OnDropComplete { get; }

        public EventHandler OnRefreshCache { get; }

        public IODGraphPrinter Printer { get; }

        public ODTable DbItem { get; }

        public DashboardDockContainer(
            Control c,
            IODGraphPrinter printer = null,
            EventHandler onEditClick = null,
            EventHandler onEditOk = null,
            EventHandler onEditCancel = null,
            EventHandler onDropComplete = null,
            EventHandler onRefreshCache = null,
            ODTable dbItem = null)
        {
            Control = c;
            Printer = printer;
            OnEditClick = onEditClick;
            OnEditOk = onEditOk;
            OnEditCancel = onEditCancel;
            OnDropComplete = onDropComplete;
            OnRefreshCache = onRefreshCache;
            DbItem = dbItem;
        }
    }
}
