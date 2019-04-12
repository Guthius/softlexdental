//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MobileAppDeviceCrud {
		///<summary>Gets one MobileAppDevice object from the database using the primary key.  Returns null if not found.</summary>
		public static MobileAppDevice SelectOne(long mobileAppDeviceNum) {
			string command="SELECT * FROM mobileappdevice "
				+"WHERE MobileAppDeviceNum = "+POut.Long(mobileAppDeviceNum);
			List<MobileAppDevice> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MobileAppDevice object from the database using a query.</summary>
		public static MobileAppDevice SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MobileAppDevice> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MobileAppDevice objects from the database using a query.</summary>
		public static List<MobileAppDevice> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MobileAppDevice> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MobileAppDevice> TableToList(DataTable table) {
			List<MobileAppDevice> retVal=new List<MobileAppDevice>();
			MobileAppDevice mobileAppDevice;
			foreach(DataRow row in table.Rows) {
				mobileAppDevice=new MobileAppDevice();
				mobileAppDevice.MobileAppDeviceNum= PIn.Long  (row["MobileAppDeviceNum"].ToString());
				mobileAppDevice.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				mobileAppDevice.DeviceName        = PIn.String(row["DeviceName"].ToString());
				mobileAppDevice.UniqueID          = PIn.String(row["UniqueID"].ToString());
				mobileAppDevice.IsAllowed         = PIn.Bool  (row["IsAllowed"].ToString());
				mobileAppDevice.LastAttempt       = PIn.DateT (row["LastAttempt"].ToString());
				mobileAppDevice.LastLogin         = PIn.DateT (row["LastLogin"].ToString());
				retVal.Add(mobileAppDevice);
			}
			return retVal;
		}

		///<summary>Converts a list of MobileAppDevice into a DataTable.</summary>
		public static DataTable ListToTable(List<MobileAppDevice> listMobileAppDevices,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="MobileAppDevice";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("MobileAppDeviceNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("DeviceName");
			table.Columns.Add("UniqueID");
			table.Columns.Add("IsAllowed");
			table.Columns.Add("LastAttempt");
			table.Columns.Add("LastLogin");
			foreach(MobileAppDevice mobileAppDevice in listMobileAppDevices) {
				table.Rows.Add(new object[] {
					POut.Long  (mobileAppDevice.MobileAppDeviceNum),
					POut.Long  (mobileAppDevice.ClinicNum),
					            mobileAppDevice.DeviceName,
					            mobileAppDevice.UniqueID,
					POut.Bool  (mobileAppDevice.IsAllowed),
					POut.DateT (mobileAppDevice.LastAttempt,false),
					POut.DateT (mobileAppDevice.LastLogin,false),
				});
			}
			return table;
		}

		///<summary>Inserts one MobileAppDevice into the database.  Returns the new priKey.</summary>
		public static long Insert(MobileAppDevice mobileAppDevice) {
			return Insert(mobileAppDevice,false);
		}

		///<summary>Inserts one MobileAppDevice into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MobileAppDevice mobileAppDevice,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				mobileAppDevice.MobileAppDeviceNum=ReplicationServers.GetKey("mobileappdevice","MobileAppDeviceNum");
			}
			string command="INSERT INTO mobileappdevice (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MobileAppDeviceNum,";
			}
			command+="ClinicNum,DeviceName,UniqueID,IsAllowed,LastAttempt,LastLogin) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mobileAppDevice.MobileAppDeviceNum)+",";
			}
			command+=
				     POut.Long  (mobileAppDevice.ClinicNum)+","
				+"'"+POut.String(mobileAppDevice.DeviceName)+"',"
				+"'"+POut.String(mobileAppDevice.UniqueID)+"',"
				+    POut.Bool  (mobileAppDevice.IsAllowed)+","
				+    POut.DateT (mobileAppDevice.LastAttempt)+","
				+    POut.DateT (mobileAppDevice.LastLogin)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mobileAppDevice.MobileAppDeviceNum=Db.NonQ(command,true,"MobileAppDeviceNum","mobileAppDevice");
			}
			return mobileAppDevice.MobileAppDeviceNum;
		}

		///<summary>Inserts one MobileAppDevice into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MobileAppDevice mobileAppDevice) {
			return InsertNoCache(mobileAppDevice,false);
		}

		///<summary>Inserts one MobileAppDevice into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MobileAppDevice mobileAppDevice,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO mobileappdevice (";
			if(!useExistingPK && isRandomKeys) {
				mobileAppDevice.MobileAppDeviceNum=ReplicationServers.GetKeyNoCache("mobileappdevice","MobileAppDeviceNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MobileAppDeviceNum,";
			}
			command+="ClinicNum,DeviceName,UniqueID,IsAllowed,LastAttempt,LastLogin) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(mobileAppDevice.MobileAppDeviceNum)+",";
			}
			command+=
				     POut.Long  (mobileAppDevice.ClinicNum)+","
				+"'"+POut.String(mobileAppDevice.DeviceName)+"',"
				+"'"+POut.String(mobileAppDevice.UniqueID)+"',"
				+    POut.Bool  (mobileAppDevice.IsAllowed)+","
				+    POut.DateT (mobileAppDevice.LastAttempt)+","
				+    POut.DateT (mobileAppDevice.LastLogin)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				mobileAppDevice.MobileAppDeviceNum=Db.NonQ(command,true,"MobileAppDeviceNum","mobileAppDevice");
			}
			return mobileAppDevice.MobileAppDeviceNum;
		}

		///<summary>Updates one MobileAppDevice in the database.</summary>
		public static void Update(MobileAppDevice mobileAppDevice) {
			string command="UPDATE mobileappdevice SET "
				+"ClinicNum         =  "+POut.Long  (mobileAppDevice.ClinicNum)+", "
				+"DeviceName        = '"+POut.String(mobileAppDevice.DeviceName)+"', "
				+"UniqueID          = '"+POut.String(mobileAppDevice.UniqueID)+"', "
				+"IsAllowed         =  "+POut.Bool  (mobileAppDevice.IsAllowed)+", "
				+"LastAttempt       =  "+POut.DateT (mobileAppDevice.LastAttempt)+", "
				+"LastLogin         =  "+POut.DateT (mobileAppDevice.LastLogin)+" "
				+"WHERE MobileAppDeviceNum = "+POut.Long(mobileAppDevice.MobileAppDeviceNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MobileAppDevice in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MobileAppDevice mobileAppDevice,MobileAppDevice oldMobileAppDevice) {
			string command="";
			if(mobileAppDevice.ClinicNum != oldMobileAppDevice.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(mobileAppDevice.ClinicNum)+"";
			}
			if(mobileAppDevice.DeviceName != oldMobileAppDevice.DeviceName) {
				if(command!="") { command+=",";}
				command+="DeviceName = '"+POut.String(mobileAppDevice.DeviceName)+"'";
			}
			if(mobileAppDevice.UniqueID != oldMobileAppDevice.UniqueID) {
				if(command!="") { command+=",";}
				command+="UniqueID = '"+POut.String(mobileAppDevice.UniqueID)+"'";
			}
			if(mobileAppDevice.IsAllowed != oldMobileAppDevice.IsAllowed) {
				if(command!="") { command+=",";}
				command+="IsAllowed = "+POut.Bool(mobileAppDevice.IsAllowed)+"";
			}
			if(mobileAppDevice.LastAttempt != oldMobileAppDevice.LastAttempt) {
				if(command!="") { command+=",";}
				command+="LastAttempt = "+POut.DateT(mobileAppDevice.LastAttempt)+"";
			}
			if(mobileAppDevice.LastLogin != oldMobileAppDevice.LastLogin) {
				if(command!="") { command+=",";}
				command+="LastLogin = "+POut.DateT(mobileAppDevice.LastLogin)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE mobileappdevice SET "+command
				+" WHERE MobileAppDeviceNum = "+POut.Long(mobileAppDevice.MobileAppDeviceNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(MobileAppDevice,MobileAppDevice) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(MobileAppDevice mobileAppDevice,MobileAppDevice oldMobileAppDevice) {
			if(mobileAppDevice.ClinicNum != oldMobileAppDevice.ClinicNum) {
				return true;
			}
			if(mobileAppDevice.DeviceName != oldMobileAppDevice.DeviceName) {
				return true;
			}
			if(mobileAppDevice.UniqueID != oldMobileAppDevice.UniqueID) {
				return true;
			}
			if(mobileAppDevice.IsAllowed != oldMobileAppDevice.IsAllowed) {
				return true;
			}
			if(mobileAppDevice.LastAttempt != oldMobileAppDevice.LastAttempt) {
				return true;
			}
			if(mobileAppDevice.LastLogin != oldMobileAppDevice.LastLogin) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one MobileAppDevice from the database.</summary>
		public static void Delete(long mobileAppDeviceNum) {
			string command="DELETE FROM mobileappdevice "
				+"WHERE MobileAppDeviceNum = "+POut.Long(mobileAppDeviceNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<MobileAppDevice> listNew,List<MobileAppDevice> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<MobileAppDevice> listIns    =new List<MobileAppDevice>();
			List<MobileAppDevice> listUpdNew =new List<MobileAppDevice>();
			List<MobileAppDevice> listUpdDB  =new List<MobileAppDevice>();
			List<MobileAppDevice> listDel    =new List<MobileAppDevice>();
			listNew.Sort((MobileAppDevice x,MobileAppDevice y) => { return x.MobileAppDeviceNum.CompareTo(y.MobileAppDeviceNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((MobileAppDevice x,MobileAppDevice y) => { return x.MobileAppDeviceNum.CompareTo(y.MobileAppDeviceNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			MobileAppDevice fieldNew;
			MobileAppDevice fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.MobileAppDeviceNum<fieldDB.MobileAppDeviceNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.MobileAppDeviceNum>fieldDB.MobileAppDeviceNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].MobileAppDeviceNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}