//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class TaskTakenCrud {
		///<summary>Gets one TaskTaken object from the database using the primary key.  Returns null if not found.</summary>
		public static TaskTaken SelectOne(long taskTakenNum) {
			string command="SELECT * FROM tasktaken "
				+"WHERE TaskTakenNum = "+POut.Long(taskTakenNum);
			List<TaskTaken> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one TaskTaken object from the database using a query.</summary>
		public static TaskTaken SelectOne(string command) {
			
			List<TaskTaken> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of TaskTaken objects from the database using a query.</summary>
		public static List<TaskTaken> SelectMany(string command) {
			
			List<TaskTaken> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<TaskTaken> TableToList(DataTable table) {
			List<TaskTaken> retVal=new List<TaskTaken>();
			TaskTaken taskTaken;
			foreach(DataRow row in table.Rows) {
				taskTaken=new TaskTaken();
				taskTaken.TaskTakenNum= PIn.Long  (row["TaskTakenNum"].ToString());
				taskTaken.TaskNum     = PIn.Long  (row["TaskNum"].ToString());
				retVal.Add(taskTaken);
			}
			return retVal;
		}

		///<summary>Converts a list of TaskTaken into a DataTable.</summary>
		public static DataTable ListToTable(List<TaskTaken> listTaskTakens,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="TaskTaken";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("TaskTakenNum");
			table.Columns.Add("TaskNum");
			foreach(TaskTaken taskTaken in listTaskTakens) {
				table.Rows.Add(new object[] {
					POut.Long  (taskTaken.TaskTakenNum),
					POut.Long  (taskTaken.TaskNum),
				});
			}
			return table;
		}

		///<summary>Inserts one TaskTaken into the database.  Returns the new priKey.</summary>
		public static long Insert(TaskTaken taskTaken) {
			return Insert(taskTaken,false);
		}

		///<summary>Inserts one TaskTaken into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(TaskTaken taskTaken,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				taskTaken.TaskTakenNum=ReplicationServers.GetKey("tasktaken","TaskTakenNum");
			}
			string command="INSERT INTO tasktaken (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="TaskTakenNum,";
			}
			command+="TaskNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(taskTaken.TaskTakenNum)+",";
			}
			command+=
				     POut.Long  (taskTaken.TaskNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskTaken.TaskTakenNum=Db.NonQ(command,true,"TaskTakenNum","taskTaken");
			}
			return taskTaken.TaskTakenNum;
		}

		///<summary>Inserts one TaskTaken into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskTaken taskTaken) {
			return InsertNoCache(taskTaken,false);
		}

		///<summary>Inserts one TaskTaken into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskTaken taskTaken,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO tasktaken (";
			if(!useExistingPK && isRandomKeys) {
				taskTaken.TaskTakenNum=ReplicationServers.GetKeyNoCache("tasktaken","TaskTakenNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="TaskTakenNum,";
			}
			command+="TaskNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(taskTaken.TaskTakenNum)+",";
			}
			command+=
				     POut.Long  (taskTaken.TaskNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskTaken.TaskTakenNum=Db.NonQ(command,true,"TaskTakenNum","taskTaken");
			}
			return taskTaken.TaskTakenNum;
		}

		///<summary>Updates one TaskTaken in the database.</summary>
		public static void Update(TaskTaken taskTaken) {
			string command="UPDATE tasktaken SET "
				+"TaskNum     =  "+POut.Long  (taskTaken.TaskNum)+" "
				+"WHERE TaskTakenNum = "+POut.Long(taskTaken.TaskTakenNum);
			Db.NonQ(command);
		}

		///<summary>Updates one TaskTaken in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(TaskTaken taskTaken,TaskTaken oldTaskTaken) {
			string command="";
			if(taskTaken.TaskNum != oldTaskTaken.TaskNum) {
				if(command!="") { command+=",";}
				command+="TaskNum = "+POut.Long(taskTaken.TaskNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE tasktaken SET "+command
				+" WHERE TaskTakenNum = "+POut.Long(taskTaken.TaskTakenNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(TaskTaken,TaskTaken) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(TaskTaken taskTaken,TaskTaken oldTaskTaken) {
			if(taskTaken.TaskNum != oldTaskTaken.TaskNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one TaskTaken from the database.</summary>
		public static void Delete(long taskTakenNum) {
			string command="DELETE FROM tasktaken "
				+"WHERE TaskTakenNum = "+POut.Long(taskTakenNum);
			Db.NonQ(command);
		}

	}
}