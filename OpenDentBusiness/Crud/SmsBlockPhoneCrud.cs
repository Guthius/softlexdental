//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SmsBlockPhoneCrud {
		///<summary>Gets one SmsBlockPhone object from the database using the primary key.  Returns null if not found.</summary>
		public static SmsBlockPhone SelectOne(long smsBlockPhoneNum) {
			string command="SELECT * FROM smsblockphone "
				+"WHERE SmsBlockPhoneNum = "+POut.Long(smsBlockPhoneNum);
			List<SmsBlockPhone> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SmsBlockPhone object from the database using a query.</summary>
		public static SmsBlockPhone SelectOne(string command) {
			
			List<SmsBlockPhone> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SmsBlockPhone objects from the database using a query.</summary>
		public static List<SmsBlockPhone> SelectMany(string command) {
			
			List<SmsBlockPhone> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SmsBlockPhone> TableToList(DataTable table) {
			List<SmsBlockPhone> retVal=new List<SmsBlockPhone>();
			SmsBlockPhone smsBlockPhone;
			foreach(DataRow row in table.Rows) {
				smsBlockPhone=new SmsBlockPhone();
				smsBlockPhone.SmsBlockPhoneNum   = PIn.Long  (row["SmsBlockPhoneNum"].ToString());
				smsBlockPhone.BlockWirelessNumber= PIn.String(row["BlockWirelessNumber"].ToString());
				retVal.Add(smsBlockPhone);
			}
			return retVal;
		}

		///<summary>Converts a list of SmsBlockPhone into a DataTable.</summary>
		public static DataTable ListToTable(List<SmsBlockPhone> listSmsBlockPhones,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SmsBlockPhone";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SmsBlockPhoneNum");
			table.Columns.Add("BlockWirelessNumber");
			foreach(SmsBlockPhone smsBlockPhone in listSmsBlockPhones) {
				table.Rows.Add(new object[] {
					POut.Long  (smsBlockPhone.SmsBlockPhoneNum),
					            smsBlockPhone.BlockWirelessNumber,
				});
			}
			return table;
		}

		///<summary>Inserts one SmsBlockPhone into the database.  Returns the new priKey.</summary>
		public static long Insert(SmsBlockPhone smsBlockPhone) {
			return Insert(smsBlockPhone,false);
		}

		///<summary>Inserts one SmsBlockPhone into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SmsBlockPhone smsBlockPhone,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				smsBlockPhone.SmsBlockPhoneNum=ReplicationServers.GetKey("smsblockphone","SmsBlockPhoneNum");
			}
			string command="INSERT INTO smsblockphone (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SmsBlockPhoneNum,";
			}
			command+="BlockWirelessNumber) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(smsBlockPhone.SmsBlockPhoneNum)+",";
			}
			command+=
				 "'"+POut.String(smsBlockPhone.BlockWirelessNumber)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				smsBlockPhone.SmsBlockPhoneNum=Db.NonQ(command,true,"SmsBlockPhoneNum","smsBlockPhone");
			}
			return smsBlockPhone.SmsBlockPhoneNum;
		}

		///<summary>Inserts one SmsBlockPhone into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsBlockPhone smsBlockPhone) {
			return InsertNoCache(smsBlockPhone,false);
		}

		///<summary>Inserts one SmsBlockPhone into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsBlockPhone smsBlockPhone,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO smsblockphone (";
			if(!useExistingPK && isRandomKeys) {
				smsBlockPhone.SmsBlockPhoneNum=ReplicationServers.GetKeyNoCache("smsblockphone","SmsBlockPhoneNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SmsBlockPhoneNum,";
			}
			command+="BlockWirelessNumber) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(smsBlockPhone.SmsBlockPhoneNum)+",";
			}
			command+=
				 "'"+POut.String(smsBlockPhone.BlockWirelessNumber)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				smsBlockPhone.SmsBlockPhoneNum=Db.NonQ(command,true,"SmsBlockPhoneNum","smsBlockPhone");
			}
			return smsBlockPhone.SmsBlockPhoneNum;
		}

		///<summary>Updates one SmsBlockPhone in the database.</summary>
		public static void Update(SmsBlockPhone smsBlockPhone) {
			string command="UPDATE smsblockphone SET "
				+"BlockWirelessNumber= '"+POut.String(smsBlockPhone.BlockWirelessNumber)+"' "
				+"WHERE SmsBlockPhoneNum = "+POut.Long(smsBlockPhone.SmsBlockPhoneNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SmsBlockPhone in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SmsBlockPhone smsBlockPhone,SmsBlockPhone oldSmsBlockPhone) {
			string command="";
			if(smsBlockPhone.BlockWirelessNumber != oldSmsBlockPhone.BlockWirelessNumber) {
				if(command!="") { command+=",";}
				command+="BlockWirelessNumber = '"+POut.String(smsBlockPhone.BlockWirelessNumber)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE smsblockphone SET "+command
				+" WHERE SmsBlockPhoneNum = "+POut.Long(smsBlockPhone.SmsBlockPhoneNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(SmsBlockPhone,SmsBlockPhone) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SmsBlockPhone smsBlockPhone,SmsBlockPhone oldSmsBlockPhone) {
			if(smsBlockPhone.BlockWirelessNumber != oldSmsBlockPhone.BlockWirelessNumber) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SmsBlockPhone from the database.</summary>
		public static void Delete(long smsBlockPhoneNum) {
			string command="DELETE FROM smsblockphone "
				+"WHERE SmsBlockPhoneNum = "+POut.Long(smsBlockPhoneNum);
			Db.NonQ(command);
		}

	}
}