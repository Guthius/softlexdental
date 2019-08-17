//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EvaluationCriterionCrud {
		///<summary>Gets one EvaluationCriterion object from the database using the primary key.  Returns null if not found.</summary>
		public static EvaluationCriterion SelectOne(long evaluationCriterionNum) {
			string command="SELECT * FROM evaluationcriterion "
				+"WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterionNum);
			List<EvaluationCriterion> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EvaluationCriterion object from the database using a query.</summary>
		public static EvaluationCriterion SelectOne(string command) {
			
			List<EvaluationCriterion> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EvaluationCriterion objects from the database using a query.</summary>
		public static List<EvaluationCriterion> SelectMany(string command) {
			
			List<EvaluationCriterion> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EvaluationCriterion> TableToList(DataTable table) {
			List<EvaluationCriterion> retVal=new List<EvaluationCriterion>();
			EvaluationCriterion evaluationCriterion;
			foreach(DataRow row in table.Rows) {
				evaluationCriterion=new EvaluationCriterion();
				evaluationCriterion.EvaluationCriterionNum= PIn.Long  (row["EvaluationCriterionNum"].ToString());
				evaluationCriterion.EvaluationNum         = PIn.Long  (row["EvaluationNum"].ToString());
				evaluationCriterion.CriterionDescript     = PIn.String(row["CriterionDescript"].ToString());
				evaluationCriterion.IsCategoryName        = PIn.Bool  (row["IsCategoryName"].ToString());
				evaluationCriterion.GradingScaleNum       = PIn.Long  (row["GradingScaleNum"].ToString());
				evaluationCriterion.GradeShowing          = PIn.String(row["GradeShowing"].ToString());
				evaluationCriterion.GradeNumber           = PIn.Float (row["GradeNumber"].ToString());
				evaluationCriterion.Notes                 = PIn.String(row["Notes"].ToString());
				evaluationCriterion.ItemOrder             = PIn.Int   (row["ItemOrder"].ToString());
				evaluationCriterion.MaxPointsPoss         = PIn.Float (row["MaxPointsPoss"].ToString());
				retVal.Add(evaluationCriterion);
			}
			return retVal;
		}

		///<summary>Converts a list of EvaluationCriterion into a DataTable.</summary>
		public static DataTable ListToTable(List<EvaluationCriterion> listEvaluationCriterions,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EvaluationCriterion";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EvaluationCriterionNum");
			table.Columns.Add("EvaluationNum");
			table.Columns.Add("CriterionDescript");
			table.Columns.Add("IsCategoryName");
			table.Columns.Add("GradingScaleNum");
			table.Columns.Add("GradeShowing");
			table.Columns.Add("GradeNumber");
			table.Columns.Add("Notes");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("MaxPointsPoss");
			foreach(EvaluationCriterion evaluationCriterion in listEvaluationCriterions) {
				table.Rows.Add(new object[] {
					POut.Long  (evaluationCriterion.EvaluationCriterionNum),
					POut.Long  (evaluationCriterion.EvaluationNum),
					            evaluationCriterion.CriterionDescript,
					POut.Bool  (evaluationCriterion.IsCategoryName),
					POut.Long  (evaluationCriterion.GradingScaleNum),
					            evaluationCriterion.GradeShowing,
					POut.Float (evaluationCriterion.GradeNumber),
					            evaluationCriterion.Notes,
					POut.Int   (evaluationCriterion.ItemOrder),
					POut.Float (evaluationCriterion.MaxPointsPoss),
				});
			}
			return table;
		}

		///<summary>Inserts one EvaluationCriterion into the database.  Returns the new priKey.</summary>
		public static long Insert(EvaluationCriterion evaluationCriterion) {
			return Insert(evaluationCriterion,false);
		}

		///<summary>Inserts one EvaluationCriterion into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EvaluationCriterion evaluationCriterion,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				evaluationCriterion.EvaluationCriterionNum=ReplicationServers.GetKey("evaluationcriterion","EvaluationCriterionNum");
			}
			string command="INSERT INTO evaluationcriterion (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EvaluationCriterionNum,";
			}
			command+="EvaluationNum,CriterionDescript,IsCategoryName,GradingScaleNum,GradeShowing,GradeNumber,Notes,ItemOrder,MaxPointsPoss) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(evaluationCriterion.EvaluationCriterionNum)+",";
			}
			command+=
				     POut.Long  (evaluationCriterion.EvaluationNum)+","
				+"'"+POut.String(evaluationCriterion.CriterionDescript)+"',"
				+    POut.Bool  (evaluationCriterion.IsCategoryName)+","
				+    POut.Long  (evaluationCriterion.GradingScaleNum)+","
				+"'"+POut.String(evaluationCriterion.GradeShowing)+"',"
				+    POut.Float (evaluationCriterion.GradeNumber)+","
				+    DbHelper.ParamChar+"paramNotes,"
				+    POut.Int   (evaluationCriterion.ItemOrder)+","
				+    POut.Float (evaluationCriterion.MaxPointsPoss)+")";
			if(evaluationCriterion.Notes==null) {
				evaluationCriterion.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(evaluationCriterion.Notes));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				evaluationCriterion.EvaluationCriterionNum=Db.NonQ(command,true,"EvaluationCriterionNum","evaluationCriterion",paramNotes);
			}
			return evaluationCriterion.EvaluationCriterionNum;
		}

		///<summary>Inserts one EvaluationCriterion into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationCriterion evaluationCriterion) {
			return InsertNoCache(evaluationCriterion,false);
		}

		///<summary>Inserts one EvaluationCriterion into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationCriterion evaluationCriterion,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO evaluationcriterion (";
			if(!useExistingPK && isRandomKeys) {
				evaluationCriterion.EvaluationCriterionNum=ReplicationServers.GetKeyNoCache("evaluationcriterion","EvaluationCriterionNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EvaluationCriterionNum,";
			}
			command+="EvaluationNum,CriterionDescript,IsCategoryName,GradingScaleNum,GradeShowing,GradeNumber,Notes,ItemOrder,MaxPointsPoss) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(evaluationCriterion.EvaluationCriterionNum)+",";
			}
			command+=
				     POut.Long  (evaluationCriterion.EvaluationNum)+","
				+"'"+POut.String(evaluationCriterion.CriterionDescript)+"',"
				+    POut.Bool  (evaluationCriterion.IsCategoryName)+","
				+    POut.Long  (evaluationCriterion.GradingScaleNum)+","
				+"'"+POut.String(evaluationCriterion.GradeShowing)+"',"
				+    POut.Float (evaluationCriterion.GradeNumber)+","
				+    DbHelper.ParamChar+"paramNotes,"
				+    POut.Int   (evaluationCriterion.ItemOrder)+","
				+    POut.Float (evaluationCriterion.MaxPointsPoss)+")";
			if(evaluationCriterion.Notes==null) {
				evaluationCriterion.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(evaluationCriterion.Notes));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				evaluationCriterion.EvaluationCriterionNum=Db.NonQ(command,true,"EvaluationCriterionNum","evaluationCriterion",paramNotes);
			}
			return evaluationCriterion.EvaluationCriterionNum;
		}

		///<summary>Updates one EvaluationCriterion in the database.</summary>
		public static void Update(EvaluationCriterion evaluationCriterion) {
			string command="UPDATE evaluationcriterion SET "
				+"EvaluationNum         =  "+POut.Long  (evaluationCriterion.EvaluationNum)+", "
				+"CriterionDescript     = '"+POut.String(evaluationCriterion.CriterionDescript)+"', "
				+"IsCategoryName        =  "+POut.Bool  (evaluationCriterion.IsCategoryName)+", "
				+"GradingScaleNum       =  "+POut.Long  (evaluationCriterion.GradingScaleNum)+", "
				+"GradeShowing          = '"+POut.String(evaluationCriterion.GradeShowing)+"', "
				+"GradeNumber           =  "+POut.Float (evaluationCriterion.GradeNumber)+", "
				+"Notes                 =  "+DbHelper.ParamChar+"paramNotes, "
				+"ItemOrder             =  "+POut.Int   (evaluationCriterion.ItemOrder)+", "
				+"MaxPointsPoss         =  "+POut.Float (evaluationCriterion.MaxPointsPoss)+" "
				+"WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterion.EvaluationCriterionNum);
			if(evaluationCriterion.Notes==null) {
				evaluationCriterion.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(evaluationCriterion.Notes));
			Db.NonQ(command,paramNotes);
		}

		///<summary>Updates one EvaluationCriterion in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EvaluationCriterion evaluationCriterion,EvaluationCriterion oldEvaluationCriterion) {
			string command="";
			if(evaluationCriterion.EvaluationNum != oldEvaluationCriterion.EvaluationNum) {
				if(command!="") { command+=",";}
				command+="EvaluationNum = "+POut.Long(evaluationCriterion.EvaluationNum)+"";
			}
			if(evaluationCriterion.CriterionDescript != oldEvaluationCriterion.CriterionDescript) {
				if(command!="") { command+=",";}
				command+="CriterionDescript = '"+POut.String(evaluationCriterion.CriterionDescript)+"'";
			}
			if(evaluationCriterion.IsCategoryName != oldEvaluationCriterion.IsCategoryName) {
				if(command!="") { command+=",";}
				command+="IsCategoryName = "+POut.Bool(evaluationCriterion.IsCategoryName)+"";
			}
			if(evaluationCriterion.GradingScaleNum != oldEvaluationCriterion.GradingScaleNum) {
				if(command!="") { command+=",";}
				command+="GradingScaleNum = "+POut.Long(evaluationCriterion.GradingScaleNum)+"";
			}
			if(evaluationCriterion.GradeShowing != oldEvaluationCriterion.GradeShowing) {
				if(command!="") { command+=",";}
				command+="GradeShowing = '"+POut.String(evaluationCriterion.GradeShowing)+"'";
			}
			if(evaluationCriterion.GradeNumber != oldEvaluationCriterion.GradeNumber) {
				if(command!="") { command+=",";}
				command+="GradeNumber = "+POut.Float(evaluationCriterion.GradeNumber)+"";
			}
			if(evaluationCriterion.Notes != oldEvaluationCriterion.Notes) {
				if(command!="") { command+=",";}
				command+="Notes = "+DbHelper.ParamChar+"paramNotes";
			}
			if(evaluationCriterion.ItemOrder != oldEvaluationCriterion.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(evaluationCriterion.ItemOrder)+"";
			}
			if(evaluationCriterion.MaxPointsPoss != oldEvaluationCriterion.MaxPointsPoss) {
				if(command!="") { command+=",";}
				command+="MaxPointsPoss = "+POut.Float(evaluationCriterion.MaxPointsPoss)+"";
			}
			if(command=="") {
				return false;
			}
			if(evaluationCriterion.Notes==null) {
				evaluationCriterion.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(evaluationCriterion.Notes));
			command="UPDATE evaluationcriterion SET "+command
				+" WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterion.EvaluationCriterionNum);
			Db.NonQ(command,paramNotes);
			return true;
		}

		///<summary>Returns true if Update(EvaluationCriterion,EvaluationCriterion) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EvaluationCriterion evaluationCriterion,EvaluationCriterion oldEvaluationCriterion) {
			if(evaluationCriterion.EvaluationNum != oldEvaluationCriterion.EvaluationNum) {
				return true;
			}
			if(evaluationCriterion.CriterionDescript != oldEvaluationCriterion.CriterionDescript) {
				return true;
			}
			if(evaluationCriterion.IsCategoryName != oldEvaluationCriterion.IsCategoryName) {
				return true;
			}
			if(evaluationCriterion.GradingScaleNum != oldEvaluationCriterion.GradingScaleNum) {
				return true;
			}
			if(evaluationCriterion.GradeShowing != oldEvaluationCriterion.GradeShowing) {
				return true;
			}
			if(evaluationCriterion.GradeNumber != oldEvaluationCriterion.GradeNumber) {
				return true;
			}
			if(evaluationCriterion.Notes != oldEvaluationCriterion.Notes) {
				return true;
			}
			if(evaluationCriterion.ItemOrder != oldEvaluationCriterion.ItemOrder) {
				return true;
			}
			if(evaluationCriterion.MaxPointsPoss != oldEvaluationCriterion.MaxPointsPoss) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EvaluationCriterion from the database.</summary>
		public static void Delete(long evaluationCriterionNum) {
			string command="DELETE FROM evaluationcriterion "
				+"WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterionNum);
			Db.NonQ(command);
		}

	}
}