//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PhoneGraphCrud {
		///<summary>Gets one PhoneGraph object from the database using the primary key.  Returns null if not found.</summary>
		public static PhoneGraph SelectOne(long phoneGraphNum) {
			string command="SELECT * FROM phonegraph "
				+"WHERE PhoneGraphNum = "+POut.Long(phoneGraphNum);
			List<PhoneGraph> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PhoneGraph object from the database using a query.</summary>
		public static PhoneGraph SelectOne(string command) {
			
			List<PhoneGraph> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PhoneGraph objects from the database using a query.</summary>
		public static List<PhoneGraph> SelectMany(string command) {
			
			List<PhoneGraph> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PhoneGraph> TableToList(DataTable table) {
			List<PhoneGraph> retVal=new List<PhoneGraph>();
			PhoneGraph phoneGraph;
			foreach(DataRow row in table.Rows) {
				phoneGraph=new PhoneGraph();
				phoneGraph.PhoneGraphNum= PIn.Long  (row["PhoneGraphNum"].ToString());
				phoneGraph.EmployeeNum  = PIn.Long  (row["EmployeeNum"].ToString());
				phoneGraph.IsGraphed    = PIn.Bool  (row["IsGraphed"].ToString());
				phoneGraph.DateEntry    = PIn.Date  (row["DateEntry"].ToString());
				retVal.Add(phoneGraph);
			}
			return retVal;
		}

		///<summary>Converts a list of PhoneGraph into a DataTable.</summary>
		public static DataTable ListToTable(List<PhoneGraph> listPhoneGraphs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PhoneGraph";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PhoneGraphNum");
			table.Columns.Add("EmployeeNum");
			table.Columns.Add("IsGraphed");
			table.Columns.Add("DateEntry");
			foreach(PhoneGraph phoneGraph in listPhoneGraphs) {
				table.Rows.Add(new object[] {
					POut.Long  (phoneGraph.PhoneGraphNum),
					POut.Long  (phoneGraph.EmployeeNum),
					POut.Bool  (phoneGraph.IsGraphed),
					POut.DateT (phoneGraph.DateEntry,false),
				});
			}
			return table;
		}

		///<summary>Inserts one PhoneGraph into the database.  Returns the new priKey.</summary>
		public static long Insert(PhoneGraph phoneGraph) {
			return Insert(phoneGraph,false);
		}

		///<summary>Inserts one PhoneGraph into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PhoneGraph phoneGraph,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				phoneGraph.PhoneGraphNum=ReplicationServers.GetKey("phonegraph","PhoneGraphNum");
			}
			string command="INSERT INTO phonegraph (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PhoneGraphNum,";
			}
			command+="EmployeeNum,IsGraphed,DateEntry) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(phoneGraph.PhoneGraphNum)+",";
			}
			command+=
				     POut.Long  (phoneGraph.EmployeeNum)+","
				+    POut.Bool  (phoneGraph.IsGraphed)+","
				+    POut.Date  (phoneGraph.DateEntry)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneGraph.PhoneGraphNum=Db.NonQ(command,true,"PhoneGraphNum","phoneGraph");
			}
			return phoneGraph.PhoneGraphNum;
		}

		///<summary>Inserts one PhoneGraph into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneGraph phoneGraph) {
			return InsertNoCache(phoneGraph,false);
		}

		///<summary>Inserts one PhoneGraph into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneGraph phoneGraph,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO phonegraph (";
			if(!useExistingPK && isRandomKeys) {
				phoneGraph.PhoneGraphNum=ReplicationServers.GetKeyNoCache("phonegraph","PhoneGraphNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PhoneGraphNum,";
			}
			command+="EmployeeNum,IsGraphed,DateEntry) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(phoneGraph.PhoneGraphNum)+",";
			}
			command+=
				     POut.Long  (phoneGraph.EmployeeNum)+","
				+    POut.Bool  (phoneGraph.IsGraphed)+","
				+    POut.Date  (phoneGraph.DateEntry)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneGraph.PhoneGraphNum=Db.NonQ(command,true,"PhoneGraphNum","phoneGraph");
			}
			return phoneGraph.PhoneGraphNum;
		}

		///<summary>Updates one PhoneGraph in the database.</summary>
		public static void Update(PhoneGraph phoneGraph) {
			string command="UPDATE phonegraph SET "
				+"EmployeeNum  =  "+POut.Long  (phoneGraph.EmployeeNum)+", "
				+"IsGraphed    =  "+POut.Bool  (phoneGraph.IsGraphed)+", "
				+"DateEntry    =  "+POut.Date  (phoneGraph.DateEntry)+" "
				+"WHERE PhoneGraphNum = "+POut.Long(phoneGraph.PhoneGraphNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PhoneGraph in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PhoneGraph phoneGraph,PhoneGraph oldPhoneGraph) {
			string command="";
			if(phoneGraph.EmployeeNum != oldPhoneGraph.EmployeeNum) {
				if(command!="") { command+=",";}
				command+="EmployeeNum = "+POut.Long(phoneGraph.EmployeeNum)+"";
			}
			if(phoneGraph.IsGraphed != oldPhoneGraph.IsGraphed) {
				if(command!="") { command+=",";}
				command+="IsGraphed = "+POut.Bool(phoneGraph.IsGraphed)+"";
			}
			if(phoneGraph.DateEntry.Date != oldPhoneGraph.DateEntry.Date) {
				if(command!="") { command+=",";}
				command+="DateEntry = "+POut.Date(phoneGraph.DateEntry)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE phonegraph SET "+command
				+" WHERE PhoneGraphNum = "+POut.Long(phoneGraph.PhoneGraphNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(PhoneGraph,PhoneGraph) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PhoneGraph phoneGraph,PhoneGraph oldPhoneGraph) {
			if(phoneGraph.EmployeeNum != oldPhoneGraph.EmployeeNum) {
				return true;
			}
			if(phoneGraph.IsGraphed != oldPhoneGraph.IsGraphed) {
				return true;
			}
			if(phoneGraph.DateEntry.Date != oldPhoneGraph.DateEntry.Date) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PhoneGraph from the database.</summary>
		public static void Delete(long phoneGraphNum) {
			string command="DELETE FROM phonegraph "
				+"WHERE PhoneGraphNum = "+POut.Long(phoneGraphNum);
			Db.NonQ(command);
		}

	}
}