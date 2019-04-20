using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary>Insert, Update, Delete are all managed by DashboardLayouts. The 2 classes are tightly coupled and should not be modified separately.</summary>
    public class DashboardCells
    {
        ///<summary></summary>
        public static List<DashboardCell> GetAll()
        {
            string command = "SELECT * FROM dashboardcell";
            return Crud.DashboardCellCrud.SelectMany(command);
        }
    }
}