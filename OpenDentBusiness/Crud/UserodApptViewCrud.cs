//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class UserodApptViewCrud {
		///<summary>Gets one UserodApptView object from the database using the primary key.  Returns null if not found.</summary>
		public static UserodApptView SelectOne(long userodApptViewNum) {
			string command="SELECT * FROM userodapptview "
				+"WHERE UserodApptViewNum = "+POut.Long(userodApptViewNum);
			List<UserodApptView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one UserodApptView object from the database using a query.</summary>
		public static UserodApptView SelectOne(string command) {
			
			List<UserodApptView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of UserodApptView objects from the database using a query.</summary>
		public static List<UserodApptView> SelectMany(string command) {
			
			List<UserodApptView> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<UserodApptView> TableToList(DataTable table) {
			List<UserodApptView> retVal=new List<UserodApptView>();
			UserodApptView userodApptView;
			foreach(DataRow row in table.Rows) {
				userodApptView=new UserodApptView();
				userodApptView.UserodApptViewNum= PIn.Long  (row["UserodApptViewNum"].ToString());
				userodApptView.UserNum          = PIn.Long  (row["UserNum"].ToString());
				userodApptView.ClinicNum        = PIn.Long  (row["ClinicNum"].ToString());
				userodApptView.ApptViewNum      = PIn.Long  (row["ApptViewNum"].ToString());
				retVal.Add(userodApptView);
			}
			return retVal;
		}

		///<summary>Converts a list of UserodApptView into a DataTable.</summary>
		public static DataTable ListToTable(List<UserodApptView> listUserodApptViews,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="UserodApptView";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("UserodApptViewNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("ApptViewNum");
			foreach(UserodApptView userodApptView in listUserodApptViews) {
				table.Rows.Add(new object[] {
					POut.Long  (userodApptView.UserodApptViewNum),
					POut.Long  (userodApptView.UserNum),
					POut.Long  (userodApptView.ClinicNum),
					POut.Long  (userodApptView.ApptViewNum),
				});
			}
			return table;
		}

		///<summary>Inserts one UserodApptView into the database.  Returns the new priKey.</summary>
		public static long Insert(UserodApptView userodApptView) {
			return Insert(userodApptView,false);
		}

		///<summary>Inserts one UserodApptView into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(UserodApptView userodApptView,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				userodApptView.UserodApptViewNum=ReplicationServers.GetKey("userodapptview","UserodApptViewNum");
			}
			string command="INSERT INTO userodapptview (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UserodApptViewNum,";
			}
			command+="UserNum,ClinicNum,ApptViewNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(userodApptView.UserodApptViewNum)+",";
			}
			command+=
				     POut.Long  (userodApptView.UserNum)+","
				+    POut.Long  (userodApptView.ClinicNum)+","
				+    POut.Long  (userodApptView.ApptViewNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				userodApptView.UserodApptViewNum=Db.NonQ(command,true,"UserodApptViewNum","userodApptView");
			}
			return userodApptView.UserodApptViewNum;
		}

		///<summary>Inserts one UserodApptView into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UserodApptView userodApptView) {
			return InsertNoCache(userodApptView,false);
		}

		///<summary>Inserts one UserodApptView into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UserodApptView userodApptView,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO userodapptview (";
			if(!useExistingPK && isRandomKeys) {
				userodApptView.UserodApptViewNum=ReplicationServers.GetKeyNoCache("userodapptview","UserodApptViewNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="UserodApptViewNum,";
			}
			command+="UserNum,ClinicNum,ApptViewNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(userodApptView.UserodApptViewNum)+",";
			}
			command+=
				     POut.Long  (userodApptView.UserNum)+","
				+    POut.Long  (userodApptView.ClinicNum)+","
				+    POut.Long  (userodApptView.ApptViewNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				userodApptView.UserodApptViewNum=Db.NonQ(command,true,"UserodApptViewNum","userodApptView");
			}
			return userodApptView.UserodApptViewNum;
		}

		///<summary>Updates one UserodApptView in the database.</summary>
		public static void Update(UserodApptView userodApptView) {
			string command="UPDATE userodapptview SET "
				+"UserNum          =  "+POut.Long  (userodApptView.UserNum)+", "
				+"ClinicNum        =  "+POut.Long  (userodApptView.ClinicNum)+", "
				+"ApptViewNum      =  "+POut.Long  (userodApptView.ApptViewNum)+" "
				+"WHERE UserodApptViewNum = "+POut.Long(userodApptView.UserodApptViewNum);
			Db.NonQ(command);
		}

		///<summary>Updates one UserodApptView in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(UserodApptView userodApptView,UserodApptView oldUserodApptView) {
			string command="";
			if(userodApptView.UserNum != oldUserodApptView.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(userodApptView.UserNum)+"";
			}
			if(userodApptView.ClinicNum != oldUserodApptView.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(userodApptView.ClinicNum)+"";
			}
			if(userodApptView.ApptViewNum != oldUserodApptView.ApptViewNum) {
				if(command!="") { command+=",";}
				command+="ApptViewNum = "+POut.Long(userodApptView.ApptViewNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE userodapptview SET "+command
				+" WHERE UserodApptViewNum = "+POut.Long(userodApptView.UserodApptViewNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(UserodApptView,UserodApptView) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(UserodApptView userodApptView,UserodApptView oldUserodApptView) {
			if(userodApptView.UserNum != oldUserodApptView.UserNum) {
				return true;
			}
			if(userodApptView.ClinicNum != oldUserodApptView.ClinicNum) {
				return true;
			}
			if(userodApptView.ApptViewNum != oldUserodApptView.ApptViewNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one UserodApptView from the database.</summary>
		public static void Delete(long userodApptViewNum) {
			string command="DELETE FROM userodapptview "
				+"WHERE UserodApptViewNum = "+POut.Long(userodApptViewNum);
			Db.NonQ(command);
		}

	}
}