using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class MobileAppDevices
    {
        ///<summary>Gets all MobileAppDevices from the database.</summary>
        public static List<MobileAppDevice> GetAll()
        {
            string command = "SELECT * FROM mobileappdevice";
            return Crud.MobileAppDeviceCrud.SelectMany(command);
        }

        ///<summary>Syncs the two lists in the database.</summary>
        public static void Sync(List<MobileAppDevice> listDevicesNew, List<MobileAppDevice> listDevicesDb)
        {
            Crud.MobileAppDeviceCrud.Sync(listDevicesNew, listDevicesDb);
        }
    }
}