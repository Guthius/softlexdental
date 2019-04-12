using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MobileAppDevices{

		#region Get Methods

		///<summary>Gets all MobileAppDevices from the database.</summary>
		public static List<MobileAppDevice> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MobileAppDevice>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM mobileappdevice";
			return Crud.MobileAppDeviceCrud.SelectMany(command);
		}

		#endregion Get Methods

		#region Modification Methods
		#region Update

		///<summary>Syncs the two lists in the database.</summary>
		public static void Sync(List<MobileAppDevice> listDevicesNew,List<MobileAppDevice> listDevicesDb) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listDevicesNew,listDevicesDb);
				return;
			}
			Crud.MobileAppDeviceCrud.Sync(listDevicesNew,listDevicesDb);
		}

		#endregion Update
		#endregion Modification Methods

	}
}