using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DashboardLayouts
    {
        public static List<DashboardLayout> GetDashboardLayout(string dashboardGroupName = "")
        {
            string command = "SELECT * FROM dashboardlayout";
            List<DashboardLayout> layouts = Crud.DashboardLayoutCrud.SelectMany(command);
            if (!string.IsNullOrEmpty(dashboardGroupName))
            { //Limit to a single group.
                layouts = layouts.FindAll(x => x.DashboardGroupName.ToLower() == dashboardGroupName.ToLower());
            }
            //Fill the non-db Cells field.
            List<DashboardCell> cells = DashboardCells.GetAll();
            foreach (DashboardLayout layout in layouts)
            {
                layout.Cells = cells.FindAll(x => x.DashboardLayoutNum == layout.DashboardLayoutNum);
            }
            return layouts;
        }

        ///<summary>Inserts the given dashboard layouts and cells into the database.</summary>
        public static void SetDashboardLayout(List<DashboardLayout> layouts, string dashboardGroupName)
        {
            //Get all old layouts.
            List<DashboardLayout> layoutsDbAll = GetDashboardLayout();
            //Get all old layouts for this group.
            List<DashboardLayout> layoutsDbGroup = layoutsDbAll.FindAll(x => x.DashboardGroupName.ToLower() == dashboardGroupName.ToLower());
            //Delete all cells from old dashboard group.
            layoutsDbGroup.SelectMany(x => x.Cells).ToList().ForEach(x => Crud.DashboardCellCrud.Delete(x.DashboardCellNum));
            //Delete all layouts from old dashboard group.
            layoutsDbGroup.ForEach(x => Crud.DashboardLayoutCrud.Delete(x.DashboardLayoutNum));
            List<DashboardCell> cellsDb = DashboardCells.GetAll();
            foreach (DashboardLayout layout in layouts)
            {
                layout.DashboardGroupName = dashboardGroupName;
                //Delete old tab if it exists.
                layoutsDbAll
                    .FindAll(x => x.DashboardLayoutNum == layout.DashboardLayoutNum)
                    .ForEach(x => Crud.DashboardLayoutCrud.Delete(x.DashboardLayoutNum));
                //Delete old cells which belonged to this tab if they exist.
                cellsDb
                    .FindAll(x => x.DashboardLayoutNum == layout.DashboardLayoutNum)
                    .ForEach(x => Crud.DashboardCellCrud.Delete(x.DashboardCellNum));
                //Insert new tab.
                long layoutNumNew = Crud.DashboardLayoutCrud.Insert(layout);
                //Insert link cells to new tab and insert.
                layout.Cells.ForEach(x => { x.DashboardLayoutNum = layoutNumNew; Crud.DashboardCellCrud.Insert(x); });
            }
        }
    }
}