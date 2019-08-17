//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrQuarterlyKeyCrud {
		///<summary>Gets one EhrQuarterlyKey object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrQuarterlyKey SelectOne(long ehrQuarterlyKeyNum) {
			string command="SELECT * FROM ehrquarterlykey "
				+"WHERE EhrQuarterlyKeyNum = "+POut.Long(ehrQuarterlyKeyNum);
			List<EhrQuarterlyKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrQuarterlyKey object from the database using a query.</summary>
		public static EhrQuarterlyKey SelectOne(string command) {
			
			List<EhrQuarterlyKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrQuarterlyKey objects from the database using a query.</summary>
		public static List<EhrQuarterlyKey> SelectMany(string command) {
			
			List<EhrQuarterlyKey> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrQuarterlyKey> TableToList(DataTable table) {
			List<EhrQuarterlyKey> retVal=new List<EhrQuarterlyKey>();
			EhrQuarterlyKey ehrQuarterlyKey;
			foreach(DataRow row in table.Rows) {
				ehrQuarterlyKey=new EhrQuarterlyKey();
				ehrQuarterlyKey.EhrQuarterlyKeyNum= PIn.Long  (row["EhrQuarterlyKeyNum"].ToString());
				ehrQuarterlyKey.YearValue         = PIn.Int   (row["YearValue"].ToString());
				ehrQuarterlyKey.QuarterValue      = PIn.Int   (row["QuarterValue"].ToString());
				ehrQuarterlyKey.PracticeName      = PIn.String(row["PracticeName"].ToString());
				ehrQuarterlyKey.KeyValue          = PIn.String(row["KeyValue"].ToString());
				ehrQuarterlyKey.PatNum            = PIn.Long  (row["PatNum"].ToString());
				ehrQuarterlyKey.Notes             = PIn.String(row["Notes"].ToString());
				retVal.Add(ehrQuarterlyKey);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrQuarterlyKey into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrQuarterlyKey> listEhrQuarterlyKeys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrQuarterlyKey";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrQuarterlyKeyNum");
			table.Columns.Add("YearValue");
			table.Columns.Add("QuarterValue");
			table.Columns.Add("PracticeName");
			table.Columns.Add("KeyValue");
			table.Columns.Add("PatNum");
			table.Columns.Add("Notes");
			foreach(EhrQuarterlyKey ehrQuarterlyKey in listEhrQuarterlyKeys) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrQuarterlyKey.EhrQuarterlyKeyNum),
					POut.Int   (ehrQuarterlyKey.YearValue),
					POut.Int   (ehrQuarterlyKey.QuarterValue),
					            ehrQuarterlyKey.PracticeName,
					            ehrQuarterlyKey.KeyValue,
					POut.Long  (ehrQuarterlyKey.PatNum),
					            ehrQuarterlyKey.Notes,
				});
			}
			return table;
		}

		///<summary>Inserts one EhrQuarterlyKey into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrQuarterlyKey ehrQuarterlyKey) {
			return Insert(ehrQuarterlyKey,false);
		}

		///<summary>Inserts one EhrQuarterlyKey into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrQuarterlyKey ehrQuarterlyKey,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				ehrQuarterlyKey.EhrQuarterlyKeyNum=ReplicationServers.GetKey("ehrquarterlykey","EhrQuarterlyKeyNum");
			}
			string command="INSERT INTO ehrquarterlykey (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EhrQuarterlyKeyNum,";
			}
			command+="YearValue,QuarterValue,PracticeName,KeyValue,PatNum,Notes) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(ehrQuarterlyKey.EhrQuarterlyKeyNum)+",";
			}
			command+=
				     POut.Int   (ehrQuarterlyKey.YearValue)+","
				+    POut.Int   (ehrQuarterlyKey.QuarterValue)+","
				+"'"+POut.String(ehrQuarterlyKey.PracticeName)+"',"
				+"'"+POut.String(ehrQuarterlyKey.KeyValue)+"',"
				+    POut.Long  (ehrQuarterlyKey.PatNum)+","
				+    DbHelper.ParamChar+"paramNotes)";
			if(ehrQuarterlyKey.Notes==null) {
				ehrQuarterlyKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrQuarterlyKey.Notes));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				ehrQuarterlyKey.EhrQuarterlyKeyNum=Db.NonQ(command,true,"EhrQuarterlyKeyNum","ehrQuarterlyKey",paramNotes);
			}
			return ehrQuarterlyKey.EhrQuarterlyKeyNum;
		}

		///<summary>Inserts one EhrQuarterlyKey into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrQuarterlyKey ehrQuarterlyKey) {
			return InsertNoCache(ehrQuarterlyKey,false);
		}

		///<summary>Inserts one EhrQuarterlyKey into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrQuarterlyKey ehrQuarterlyKey,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO ehrquarterlykey (";
			if(!useExistingPK && isRandomKeys) {
				ehrQuarterlyKey.EhrQuarterlyKeyNum=ReplicationServers.GetKeyNoCache("ehrquarterlykey","EhrQuarterlyKeyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrQuarterlyKeyNum,";
			}
			command+="YearValue,QuarterValue,PracticeName,KeyValue,PatNum,Notes) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrQuarterlyKey.EhrQuarterlyKeyNum)+",";
			}
			command+=
				     POut.Int   (ehrQuarterlyKey.YearValue)+","
				+    POut.Int   (ehrQuarterlyKey.QuarterValue)+","
				+"'"+POut.String(ehrQuarterlyKey.PracticeName)+"',"
				+"'"+POut.String(ehrQuarterlyKey.KeyValue)+"',"
				+    POut.Long  (ehrQuarterlyKey.PatNum)+","
				+    DbHelper.ParamChar+"paramNotes)";
			if(ehrQuarterlyKey.Notes==null) {
				ehrQuarterlyKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrQuarterlyKey.Notes));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				ehrQuarterlyKey.EhrQuarterlyKeyNum=Db.NonQ(command,true,"EhrQuarterlyKeyNum","ehrQuarterlyKey",paramNotes);
			}
			return ehrQuarterlyKey.EhrQuarterlyKeyNum;
		}

		///<summary>Updates one EhrQuarterlyKey in the database.</summary>
		public static void Update(EhrQuarterlyKey ehrQuarterlyKey) {
			string command="UPDATE ehrquarterlykey SET "
				+"YearValue         =  "+POut.Int   (ehrQuarterlyKey.YearValue)+", "
				+"QuarterValue      =  "+POut.Int   (ehrQuarterlyKey.QuarterValue)+", "
				+"PracticeName      = '"+POut.String(ehrQuarterlyKey.PracticeName)+"', "
				+"KeyValue          = '"+POut.String(ehrQuarterlyKey.KeyValue)+"', "
				+"PatNum            =  "+POut.Long  (ehrQuarterlyKey.PatNum)+", "
				+"Notes             =  "+DbHelper.ParamChar+"paramNotes "
				+"WHERE EhrQuarterlyKeyNum = "+POut.Long(ehrQuarterlyKey.EhrQuarterlyKeyNum);
			if(ehrQuarterlyKey.Notes==null) {
				ehrQuarterlyKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrQuarterlyKey.Notes));
			Db.NonQ(command,paramNotes);
		}

		///<summary>Updates one EhrQuarterlyKey in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrQuarterlyKey ehrQuarterlyKey,EhrQuarterlyKey oldEhrQuarterlyKey) {
			string command="";
			if(ehrQuarterlyKey.YearValue != oldEhrQuarterlyKey.YearValue) {
				if(command!="") { command+=",";}
				command+="YearValue = "+POut.Int(ehrQuarterlyKey.YearValue)+"";
			}
			if(ehrQuarterlyKey.QuarterValue != oldEhrQuarterlyKey.QuarterValue) {
				if(command!="") { command+=",";}
				command+="QuarterValue = "+POut.Int(ehrQuarterlyKey.QuarterValue)+"";
			}
			if(ehrQuarterlyKey.PracticeName != oldEhrQuarterlyKey.PracticeName) {
				if(command!="") { command+=",";}
				command+="PracticeName = '"+POut.String(ehrQuarterlyKey.PracticeName)+"'";
			}
			if(ehrQuarterlyKey.KeyValue != oldEhrQuarterlyKey.KeyValue) {
				if(command!="") { command+=",";}
				command+="KeyValue = '"+POut.String(ehrQuarterlyKey.KeyValue)+"'";
			}
			if(ehrQuarterlyKey.PatNum != oldEhrQuarterlyKey.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(ehrQuarterlyKey.PatNum)+"";
			}
			if(ehrQuarterlyKey.Notes != oldEhrQuarterlyKey.Notes) {
				if(command!="") { command+=",";}
				command+="Notes = "+DbHelper.ParamChar+"paramNotes";
			}
			if(command=="") {
				return false;
			}
			if(ehrQuarterlyKey.Notes==null) {
				ehrQuarterlyKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrQuarterlyKey.Notes));
			command="UPDATE ehrquarterlykey SET "+command
				+" WHERE EhrQuarterlyKeyNum = "+POut.Long(ehrQuarterlyKey.EhrQuarterlyKeyNum);
			Db.NonQ(command,paramNotes);
			return true;
		}

		///<summary>Returns true if Update(EhrQuarterlyKey,EhrQuarterlyKey) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrQuarterlyKey ehrQuarterlyKey,EhrQuarterlyKey oldEhrQuarterlyKey) {
			if(ehrQuarterlyKey.YearValue != oldEhrQuarterlyKey.YearValue) {
				return true;
			}
			if(ehrQuarterlyKey.QuarterValue != oldEhrQuarterlyKey.QuarterValue) {
				return true;
			}
			if(ehrQuarterlyKey.PracticeName != oldEhrQuarterlyKey.PracticeName) {
				return true;
			}
			if(ehrQuarterlyKey.KeyValue != oldEhrQuarterlyKey.KeyValue) {
				return true;
			}
			if(ehrQuarterlyKey.PatNum != oldEhrQuarterlyKey.PatNum) {
				return true;
			}
			if(ehrQuarterlyKey.Notes != oldEhrQuarterlyKey.Notes) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrQuarterlyKey from the database.</summary>
		public static void Delete(long ehrQuarterlyKeyNum) {
			string command="DELETE FROM ehrquarterlykey "
				+"WHERE EhrQuarterlyKeyNum = "+POut.Long(ehrQuarterlyKeyNum);
			Db.NonQ(command);
		}

	}
}