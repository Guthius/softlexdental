//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcMultiVisitCrud {
		///<summary>Gets one ProcMultiVisit object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcMultiVisit SelectOne(long procMultiVisitNum) {
			string command="SELECT * FROM procmultivisit "
				+"WHERE ProcMultiVisitNum = "+POut.Long(procMultiVisitNum);
			List<ProcMultiVisit> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcMultiVisit object from the database using a query.</summary>
		public static ProcMultiVisit SelectOne(string command) {
			
			List<ProcMultiVisit> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcMultiVisit objects from the database using a query.</summary>
		public static List<ProcMultiVisit> SelectMany(string command) {
			
			List<ProcMultiVisit> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcMultiVisit> TableToList(DataTable table) {
			List<ProcMultiVisit> retVal=new List<ProcMultiVisit>();
			ProcMultiVisit procMultiVisit;
			foreach(DataRow row in table.Rows) {
				procMultiVisit=new ProcMultiVisit();
				procMultiVisit.ProcMultiVisitNum     = PIn.Long  (row["ProcMultiVisitNum"].ToString());
				procMultiVisit.GroupProcMultiVisitNum= PIn.Long  (row["GroupProcMultiVisitNum"].ToString());
				procMultiVisit.ProcNum               = PIn.Long  (row["ProcNum"].ToString());
				procMultiVisit.ProcStatus            = (OpenDentBusiness.ProcStat)PIn.Int(row["ProcStatus"].ToString());
				procMultiVisit.IsInProcess           = PIn.Bool  (row["IsInProcess"].ToString());
				retVal.Add(procMultiVisit);
			}
			return retVal;
		}

		///<summary>Converts a list of ProcMultiVisit into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcMultiVisit> listProcMultiVisits,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProcMultiVisit";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProcMultiVisitNum");
			table.Columns.Add("GroupProcMultiVisitNum");
			table.Columns.Add("ProcNum");
			table.Columns.Add("ProcStatus");
			table.Columns.Add("IsInProcess");
			foreach(ProcMultiVisit procMultiVisit in listProcMultiVisits) {
				table.Rows.Add(new object[] {
					POut.Long  (procMultiVisit.ProcMultiVisitNum),
					POut.Long  (procMultiVisit.GroupProcMultiVisitNum),
					POut.Long  (procMultiVisit.ProcNum),
					POut.Int   ((int)procMultiVisit.ProcStatus),
					POut.Bool  (procMultiVisit.IsInProcess),
				});
			}
			return table;
		}

		///<summary>Inserts one ProcMultiVisit into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcMultiVisit procMultiVisit) {
			return Insert(procMultiVisit,false);
		}

		///<summary>Inserts one ProcMultiVisit into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcMultiVisit procMultiVisit,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				procMultiVisit.ProcMultiVisitNum=ReplicationServers.GetKey("procmultivisit","ProcMultiVisitNum");
			}
			string command="INSERT INTO procmultivisit (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ProcMultiVisitNum,";
			}
			command+="GroupProcMultiVisitNum,ProcNum,ProcStatus,IsInProcess) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(procMultiVisit.ProcMultiVisitNum)+",";
			}
			command+=
				     POut.Long  (procMultiVisit.GroupProcMultiVisitNum)+","
				+    POut.Long  (procMultiVisit.ProcNum)+","
				+    POut.Int   ((int)procMultiVisit.ProcStatus)+","
				+    POut.Bool  (procMultiVisit.IsInProcess)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procMultiVisit.ProcMultiVisitNum=Db.NonQ(command,true,"ProcMultiVisitNum","procMultiVisit");
			}
			return procMultiVisit.ProcMultiVisitNum;
		}

		///<summary>Inserts one ProcMultiVisit into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcMultiVisit procMultiVisit) {
			return InsertNoCache(procMultiVisit,false);
		}

		///<summary>Inserts one ProcMultiVisit into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcMultiVisit procMultiVisit,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO procmultivisit (";
			if(!useExistingPK && isRandomKeys) {
				procMultiVisit.ProcMultiVisitNum=ReplicationServers.GetKeyNoCache("procmultivisit","ProcMultiVisitNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcMultiVisitNum,";
			}
			command+="GroupProcMultiVisitNum,ProcNum,ProcStatus,IsInProcess) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procMultiVisit.ProcMultiVisitNum)+",";
			}
			command+=
				     POut.Long  (procMultiVisit.GroupProcMultiVisitNum)+","
				+    POut.Long  (procMultiVisit.ProcNum)+","
				+    POut.Int   ((int)procMultiVisit.ProcStatus)+","
				+    POut.Bool  (procMultiVisit.IsInProcess)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				procMultiVisit.ProcMultiVisitNum=Db.NonQ(command,true,"ProcMultiVisitNum","procMultiVisit");
			}
			return procMultiVisit.ProcMultiVisitNum;
		}

		///<summary>Updates one ProcMultiVisit in the database.</summary>
		public static void Update(ProcMultiVisit procMultiVisit) {
			string command="UPDATE procmultivisit SET "
				+"GroupProcMultiVisitNum=  "+POut.Long  (procMultiVisit.GroupProcMultiVisitNum)+", "
				+"ProcNum               =  "+POut.Long  (procMultiVisit.ProcNum)+", "
				+"ProcStatus            =  "+POut.Int   ((int)procMultiVisit.ProcStatus)+", "
				+"IsInProcess           =  "+POut.Bool  (procMultiVisit.IsInProcess)+" "
				+"WHERE ProcMultiVisitNum = "+POut.Long(procMultiVisit.ProcMultiVisitNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcMultiVisit in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcMultiVisit procMultiVisit,ProcMultiVisit oldProcMultiVisit) {
			string command="";
			if(procMultiVisit.GroupProcMultiVisitNum != oldProcMultiVisit.GroupProcMultiVisitNum) {
				if(command!="") { command+=",";}
				command+="GroupProcMultiVisitNum = "+POut.Long(procMultiVisit.GroupProcMultiVisitNum)+"";
			}
			if(procMultiVisit.ProcNum != oldProcMultiVisit.ProcNum) {
				if(command!="") { command+=",";}
				command+="ProcNum = "+POut.Long(procMultiVisit.ProcNum)+"";
			}
			if(procMultiVisit.ProcStatus != oldProcMultiVisit.ProcStatus) {
				if(command!="") { command+=",";}
				command+="ProcStatus = "+POut.Int   ((int)procMultiVisit.ProcStatus)+"";
			}
			if(procMultiVisit.IsInProcess != oldProcMultiVisit.IsInProcess) {
				if(command!="") { command+=",";}
				command+="IsInProcess = "+POut.Bool(procMultiVisit.IsInProcess)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE procmultivisit SET "+command
				+" WHERE ProcMultiVisitNum = "+POut.Long(procMultiVisit.ProcMultiVisitNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ProcMultiVisit,ProcMultiVisit) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProcMultiVisit procMultiVisit,ProcMultiVisit oldProcMultiVisit) {
			if(procMultiVisit.GroupProcMultiVisitNum != oldProcMultiVisit.GroupProcMultiVisitNum) {
				return true;
			}
			if(procMultiVisit.ProcNum != oldProcMultiVisit.ProcNum) {
				return true;
			}
			if(procMultiVisit.ProcStatus != oldProcMultiVisit.ProcStatus) {
				return true;
			}
			if(procMultiVisit.IsInProcess != oldProcMultiVisit.IsInProcess) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProcMultiVisit from the database.</summary>
		public static void Delete(long procMultiVisitNum) {
			string command="DELETE FROM procmultivisit "
				+"WHERE ProcMultiVisitNum = "+POut.Long(procMultiVisitNum);
			Db.NonQ(command);
		}

	}
}