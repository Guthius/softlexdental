using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ToothGridDefs
    {
        public static List<ToothGridDef> Refresh(long patNum)
        {
            string command = "SELECT * FROM toothgriddef WHERE toothgriddefnum = " + POut.Long(patNum);
            return Crud.ToothGridDefCrud.SelectMany(command);
        }
    }
}