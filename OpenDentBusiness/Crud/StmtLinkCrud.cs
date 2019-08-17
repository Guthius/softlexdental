//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class StmtLinkCrud {
		///<summary>Gets one StmtLink object from the database using the primary key.  Returns null if not found.</summary>
		public static StmtLink SelectOne(long stmtLinkNum) {
			string command="SELECT * FROM stmtlink "
				+"WHERE StmtLinkNum = "+POut.Long(stmtLinkNum);
			List<StmtLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one StmtLink object from the database using a query.</summary>
		public static StmtLink SelectOne(string command) {
			
			List<StmtLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of StmtLink objects from the database using a query.</summary>
		public static List<StmtLink> SelectMany(string command) {
			
			List<StmtLink> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<StmtLink> TableToList(DataTable table) {
			List<StmtLink> retVal=new List<StmtLink>();
			StmtLink stmtLink;
			foreach(DataRow row in table.Rows) {
				stmtLink=new StmtLink();
				stmtLink.StmtLinkNum = PIn.Long  (row["StmtLinkNum"].ToString());
				stmtLink.StatementNum= PIn.Long  (row["StatementNum"].ToString());
				stmtLink.StmtLinkType= (OpenDentBusiness.StmtLinkTypes)PIn.Int(row["StmtLinkType"].ToString());
				stmtLink.FKey        = PIn.Long  (row["FKey"].ToString());
				retVal.Add(stmtLink);
			}
			return retVal;
		}

		///<summary>Converts a list of StmtLink into a DataTable.</summary>
		public static DataTable ListToTable(List<StmtLink> listStmtLinks,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="StmtLink";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("StmtLinkNum");
			table.Columns.Add("StatementNum");
			table.Columns.Add("StmtLinkType");
			table.Columns.Add("FKey");
			foreach(StmtLink stmtLink in listStmtLinks) {
				table.Rows.Add(new object[] {
					POut.Long  (stmtLink.StmtLinkNum),
					POut.Long  (stmtLink.StatementNum),
					POut.Int   ((int)stmtLink.StmtLinkType),
					POut.Long  (stmtLink.FKey),
				});
			}
			return table;
		}

		///<summary>Inserts one StmtLink into the database.  Returns the new priKey.</summary>
		public static long Insert(StmtLink stmtLink) {
			return Insert(stmtLink,false);
		}

		///<summary>Inserts one StmtLink into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(StmtLink stmtLink,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				stmtLink.StmtLinkNum=ReplicationServers.GetKey("stmtlink","StmtLinkNum");
			}
			string command="INSERT INTO stmtlink (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="StmtLinkNum,";
			}
			command+="StatementNum,StmtLinkType,FKey) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(stmtLink.StmtLinkNum)+",";
			}
			command+=
				     POut.Long  (stmtLink.StatementNum)+","
				+    POut.Int   ((int)stmtLink.StmtLinkType)+","
				+    POut.Long  (stmtLink.FKey)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				stmtLink.StmtLinkNum=Db.NonQ(command,true,"StmtLinkNum","stmtLink");
			}
			return stmtLink.StmtLinkNum;
		}

		///<summary>Inserts one StmtLink into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(StmtLink stmtLink) {
			return InsertNoCache(stmtLink,false);
		}

		///<summary>Inserts one StmtLink into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(StmtLink stmtLink,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO stmtlink (";
			if(!useExistingPK && isRandomKeys) {
				stmtLink.StmtLinkNum=ReplicationServers.GetKeyNoCache("stmtlink","StmtLinkNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="StmtLinkNum,";
			}
			command+="StatementNum,StmtLinkType,FKey) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(stmtLink.StmtLinkNum)+",";
			}
			command+=
				     POut.Long  (stmtLink.StatementNum)+","
				+    POut.Int   ((int)stmtLink.StmtLinkType)+","
				+    POut.Long  (stmtLink.FKey)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				stmtLink.StmtLinkNum=Db.NonQ(command,true,"StmtLinkNum","stmtLink");
			}
			return stmtLink.StmtLinkNum;
		}

		///<summary>Updates one StmtLink in the database.</summary>
		public static void Update(StmtLink stmtLink) {
			string command="UPDATE stmtlink SET "
				+"StatementNum=  "+POut.Long  (stmtLink.StatementNum)+", "
				+"StmtLinkType=  "+POut.Int   ((int)stmtLink.StmtLinkType)+", "
				+"FKey        =  "+POut.Long  (stmtLink.FKey)+" "
				+"WHERE StmtLinkNum = "+POut.Long(stmtLink.StmtLinkNum);
			Db.NonQ(command);
		}

		///<summary>Updates one StmtLink in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(StmtLink stmtLink,StmtLink oldStmtLink) {
			string command="";
			if(stmtLink.StatementNum != oldStmtLink.StatementNum) {
				if(command!="") { command+=",";}
				command+="StatementNum = "+POut.Long(stmtLink.StatementNum)+"";
			}
			if(stmtLink.StmtLinkType != oldStmtLink.StmtLinkType) {
				if(command!="") { command+=",";}
				command+="StmtLinkType = "+POut.Int   ((int)stmtLink.StmtLinkType)+"";
			}
			if(stmtLink.FKey != oldStmtLink.FKey) {
				if(command!="") { command+=",";}
				command+="FKey = "+POut.Long(stmtLink.FKey)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE stmtlink SET "+command
				+" WHERE StmtLinkNum = "+POut.Long(stmtLink.StmtLinkNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(StmtLink,StmtLink) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(StmtLink stmtLink,StmtLink oldStmtLink) {
			if(stmtLink.StatementNum != oldStmtLink.StatementNum) {
				return true;
			}
			if(stmtLink.StmtLinkType != oldStmtLink.StmtLinkType) {
				return true;
			}
			if(stmtLink.FKey != oldStmtLink.FKey) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one StmtLink from the database.</summary>
		public static void Delete(long stmtLinkNum) {
			string command="DELETE FROM stmtlink "
				+"WHERE StmtLinkNum = "+POut.Long(stmtLinkNum);
			Db.NonQ(command);
		}

	}
}