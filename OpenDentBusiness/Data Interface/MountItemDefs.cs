using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using OpenDentBusiness;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class MountItemDefs
    {

        ///<summary>No need to surround with try/catch, because all deletions are allowed.</summary>
        public static void Delete(long mountItemDefNum)
        {

            string command = "DELETE FROM mountitemdef WHERE MountItemDefNum=" + POut.Long(mountItemDefNum);
            Db.NonQ(command);
        }




    }

}