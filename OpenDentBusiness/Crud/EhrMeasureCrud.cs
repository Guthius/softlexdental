//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrMeasureCrud {
		///<summary>Gets one EhrMeasure object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrMeasure SelectOne(long ehrMeasureNum) {
			string command="SELECT * FROM ehrmeasure "
				+"WHERE EhrMeasureNum = "+POut.Long(ehrMeasureNum);
			List<EhrMeasure> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrMeasure object from the database using a query.</summary>
		public static EhrMeasure SelectOne(string command) {
			
			List<EhrMeasure> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrMeasure objects from the database using a query.</summary>
		public static List<EhrMeasure> SelectMany(string command) {
			
			List<EhrMeasure> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrMeasure> TableToList(DataTable table) {
			List<EhrMeasure> retVal=new List<EhrMeasure>();
			EhrMeasure ehrMeasure;
			foreach(DataRow row in table.Rows) {
				ehrMeasure=new EhrMeasure();
				ehrMeasure.EhrMeasureNum= PIn.Long  (row["EhrMeasureNum"].ToString());
				ehrMeasure.MeasureType  = (OpenDentBusiness.EhrMeasureType)PIn.Int(row["MeasureType"].ToString());
				ehrMeasure.Numerator    = PIn.Int   (row["Numerator"].ToString());
				ehrMeasure.Denominator  = PIn.Int   (row["Denominator"].ToString());
				retVal.Add(ehrMeasure);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrMeasure into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrMeasure> listEhrMeasures,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrMeasure";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrMeasureNum");
			table.Columns.Add("MeasureType");
			table.Columns.Add("Numerator");
			table.Columns.Add("Denominator");
			foreach(EhrMeasure ehrMeasure in listEhrMeasures) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrMeasure.EhrMeasureNum),
					POut.Int   ((int)ehrMeasure.MeasureType),
					POut.Int   (ehrMeasure.Numerator),
					POut.Int   (ehrMeasure.Denominator),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrMeasure into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrMeasure ehrMeasure) {
			return Insert(ehrMeasure,false);
		}

		///<summary>Inserts one EhrMeasure into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrMeasure ehrMeasure,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrMeasure.EhrMeasureNum=ReplicationServers.GetKey("ehrmeasure","EhrMeasureNum");
			}
			string command="INSERT INTO ehrmeasure (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrMeasureNum,";
			}
			command+="MeasureType,Numerator,Denominator) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrMeasure.EhrMeasureNum)+",";
			}
			command+=
				     POut.Int   ((int)ehrMeasure.MeasureType)+","
				+    POut.Int   (ehrMeasure.Numerator)+","
				+    POut.Int   (ehrMeasure.Denominator)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrMeasure.EhrMeasureNum=Db.NonQ(command,true,"EhrMeasureNum","ehrMeasure");
			}
			return ehrMeasure.EhrMeasureNum;
		}

		///<summary>Inserts one EhrMeasure into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrMeasure ehrMeasure) {
			return InsertNoCache(ehrMeasure,false);
		}

		///<summary>Inserts one EhrMeasure into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrMeasure ehrMeasure,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrmeasure (";
			if(!useExistingPK && isRandomKeys) {
				ehrMeasure.EhrMeasureNum=ReplicationServers.GetKeyNoCache("ehrmeasure","EhrMeasureNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrMeasureNum,";
			}
			command+="MeasureType,Numerator,Denominator) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrMeasure.EhrMeasureNum)+",";
			}
			command+=
				     POut.Int   ((int)ehrMeasure.MeasureType)+","
				+    POut.Int   (ehrMeasure.Numerator)+","
				+    POut.Int   (ehrMeasure.Denominator)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrMeasure.EhrMeasureNum=Db.NonQ(command,true,"EhrMeasureNum","ehrMeasure");
			}
			return ehrMeasure.EhrMeasureNum;
		}

		///<summary>Updates one EhrMeasure in the database.</summary>
		public static void Update(EhrMeasure ehrMeasure) {
			string command="UPDATE ehrmeasure SET "
				+"MeasureType  =  "+POut.Int   ((int)ehrMeasure.MeasureType)+", "
				+"Numerator    =  "+POut.Int   (ehrMeasure.Numerator)+", "
				+"Denominator  =  "+POut.Int   (ehrMeasure.Denominator)+" "
				+"WHERE EhrMeasureNum = "+POut.Long(ehrMeasure.EhrMeasureNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrMeasure in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrMeasure ehrMeasure,EhrMeasure oldEhrMeasure) {
			string command="";
			if(ehrMeasure.MeasureType != oldEhrMeasure.MeasureType) {
				if(command!="") { command+=",";}
				command+="MeasureType = "+POut.Int   ((int)ehrMeasure.MeasureType)+"";
			}
			if(ehrMeasure.Numerator != oldEhrMeasure.Numerator) {
				if(command!="") { command+=",";}
				command+="Numerator = "+POut.Int(ehrMeasure.Numerator)+"";
			}
			if(ehrMeasure.Denominator != oldEhrMeasure.Denominator) {
				if(command!="") { command+=",";}
				command+="Denominator = "+POut.Int(ehrMeasure.Denominator)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE ehrmeasure SET "+command
				+" WHERE EhrMeasureNum = "+POut.Long(ehrMeasure.EhrMeasureNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EhrMeasure,EhrMeasure) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrMeasure ehrMeasure,EhrMeasure oldEhrMeasure) {
			if(ehrMeasure.MeasureType != oldEhrMeasure.MeasureType) {
				return true;
			}
			if(ehrMeasure.Numerator != oldEhrMeasure.Numerator) {
				return true;
			}
			if(ehrMeasure.Denominator != oldEhrMeasure.Denominator) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrMeasure from the database.</summary>
		public static void Delete(long ehrMeasureNum) {
			string command="DELETE FROM ehrmeasure "
				+"WHERE EhrMeasureNum = "+POut.Long(ehrMeasureNum);
			Db.NonQ(command);
		}

	}
}