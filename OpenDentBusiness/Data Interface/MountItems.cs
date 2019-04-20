using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using CodeBase;

namespace OpenDentBusiness
{
    public class MountItems
    {
        public static long Insert(MountItem mountItem)
        {
            return Crud.MountItemCrud.Insert(mountItem);
        }

        public static void Delete(MountItem mountItem)
        {
            string command = "DELETE FROM mountitem WHERE MountItemNum='" + POut.Long(mountItem.MountItemNum) + "'";
            Db.NonQ(command);
        }

        ///<summary>Returns the list of mount items associated with the given mount key.</summary>
        public static List<MountItem> GetItemsForMount(long mountNum)
        {
            string command = "SELECT * FROM mountitem WHERE MountNum='" + POut.Long(mountNum) + "' ORDER BY OrdinalPos";
            return Crud.MountItemCrud.SelectMany(command);
        }
    }
}