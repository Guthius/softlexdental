//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EvaluationDefCrud {
		///<summary>Gets one EvaluationDef object from the database using the primary key.  Returns null if not found.</summary>
		public static EvaluationDef SelectOne(long evaluationDefNum) {
			string command="SELECT * FROM evaluationdef "
				+"WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum);
			List<EvaluationDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EvaluationDef object from the database using a query.</summary>
		public static EvaluationDef SelectOne(string command) {
			
			List<EvaluationDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EvaluationDef objects from the database using a query.</summary>
		public static List<EvaluationDef> SelectMany(string command) {
			
			List<EvaluationDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EvaluationDef> TableToList(DataTable table) {
			List<EvaluationDef> retVal=new List<EvaluationDef>();
			EvaluationDef evaluationDef;
			foreach(DataRow row in table.Rows) {
				evaluationDef=new EvaluationDef();
				evaluationDef.EvaluationDefNum= PIn.Long  (row["EvaluationDefNum"].ToString());
				evaluationDef.SchoolCourseNum = PIn.Long  (row["SchoolCourseNum"].ToString());
				evaluationDef.EvalTitle       = PIn.String(row["EvalTitle"].ToString());
				evaluationDef.GradingScaleNum = PIn.Long  (row["GradingScaleNum"].ToString());
				retVal.Add(evaluationDef);
			}
			return retVal;
		}

		///<summary>Converts a list of EvaluationDef into a DataTable.</summary>
		public static DataTable ListToTable(List<EvaluationDef> listEvaluationDefs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EvaluationDef";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EvaluationDefNum");
			table.Columns.Add("SchoolCourseNum");
			table.Columns.Add("EvalTitle");
			table.Columns.Add("GradingScaleNum");
			foreach(EvaluationDef evaluationDef in listEvaluationDefs) {
				table.Rows.Add(new object[] {
					POut.Long  (evaluationDef.EvaluationDefNum),
					POut.Long  (evaluationDef.SchoolCourseNum),
					            evaluationDef.EvalTitle,
					POut.Long  (evaluationDef.GradingScaleNum),
				});
			}
			return table;
		}

		///<summary>Inserts one EvaluationDef into the database.  Returns the new priKey.</summary>
		public static long Insert(EvaluationDef evaluationDef) {
			return Insert(evaluationDef,false);
		}

		///<summary>Inserts one EvaluationDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EvaluationDef evaluationDef,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				evaluationDef.EvaluationDefNum=ReplicationServers.GetKey("evaluationdef","EvaluationDefNum");
			}
			string command="INSERT INTO evaluationdef (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EvaluationDefNum,";
			}
			command+="SchoolCourseNum,EvalTitle,GradingScaleNum) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(evaluationDef.EvaluationDefNum)+",";
			}
			command+=
				     POut.Long  (evaluationDef.SchoolCourseNum)+","
				+"'"+POut.String(evaluationDef.EvalTitle)+"',"
				+    POut.Long  (evaluationDef.GradingScaleNum)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				evaluationDef.EvaluationDefNum=Db.NonQ(command,true,"EvaluationDefNum","evaluationDef");
			}
			return evaluationDef.EvaluationDefNum;
		}

		///<summary>Inserts one EvaluationDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationDef evaluationDef) {
			return InsertNoCache(evaluationDef,false);
		}

		///<summary>Inserts one EvaluationDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationDef evaluationDef,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO evaluationdef (";
			if(!useExistingPK && isRandomKeys) {
				evaluationDef.EvaluationDefNum=ReplicationServers.GetKeyNoCache("evaluationdef","EvaluationDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EvaluationDefNum,";
			}
			command+="SchoolCourseNum,EvalTitle,GradingScaleNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(evaluationDef.EvaluationDefNum)+",";
			}
			command+=
				     POut.Long  (evaluationDef.SchoolCourseNum)+","
				+"'"+POut.String(evaluationDef.EvalTitle)+"',"
				+    POut.Long  (evaluationDef.GradingScaleNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				evaluationDef.EvaluationDefNum=Db.NonQ(command,true,"EvaluationDefNum","evaluationDef");
			}
			return evaluationDef.EvaluationDefNum;
		}

		///<summary>Updates one EvaluationDef in the database.</summary>
		public static void Update(EvaluationDef evaluationDef) {
			string command="UPDATE evaluationdef SET "
				+"SchoolCourseNum =  "+POut.Long  (evaluationDef.SchoolCourseNum)+", "
				+"EvalTitle       = '"+POut.String(evaluationDef.EvalTitle)+"', "
				+"GradingScaleNum =  "+POut.Long  (evaluationDef.GradingScaleNum)+" "
				+"WHERE EvaluationDefNum = "+POut.Long(evaluationDef.EvaluationDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EvaluationDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EvaluationDef evaluationDef,EvaluationDef oldEvaluationDef) {
			string command="";
			if(evaluationDef.SchoolCourseNum != oldEvaluationDef.SchoolCourseNum) {
				if(command!="") { command+=",";}
				command+="SchoolCourseNum = "+POut.Long(evaluationDef.SchoolCourseNum)+"";
			}
			if(evaluationDef.EvalTitle != oldEvaluationDef.EvalTitle) {
				if(command!="") { command+=",";}
				command+="EvalTitle = '"+POut.String(evaluationDef.EvalTitle)+"'";
			}
			if(evaluationDef.GradingScaleNum != oldEvaluationDef.GradingScaleNum) {
				if(command!="") { command+=",";}
				command+="GradingScaleNum = "+POut.Long(evaluationDef.GradingScaleNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE evaluationdef SET "+command
				+" WHERE EvaluationDefNum = "+POut.Long(evaluationDef.EvaluationDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EvaluationDef,EvaluationDef) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EvaluationDef evaluationDef,EvaluationDef oldEvaluationDef) {
			if(evaluationDef.SchoolCourseNum != oldEvaluationDef.SchoolCourseNum) {
				return true;
			}
			if(evaluationDef.EvalTitle != oldEvaluationDef.EvalTitle) {
				return true;
			}
			if(evaluationDef.GradingScaleNum != oldEvaluationDef.GradingScaleNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EvaluationDef from the database.</summary>
		public static void Delete(long evaluationDefNum) {
			string command="DELETE FROM evaluationdef "
				+"WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum);
			Db.NonQ(command);
		}

	}
}