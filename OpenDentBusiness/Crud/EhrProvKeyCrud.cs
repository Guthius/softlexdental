//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrProvKeyCrud {
		///<summary>Gets one EhrProvKey object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrProvKey SelectOne(long ehrProvKeyNum) {
			string command="SELECT * FROM ehrprovkey "
				+"WHERE EhrProvKeyNum = "+POut.Long(ehrProvKeyNum);
			List<EhrProvKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrProvKey object from the database using a query.</summary>
		public static EhrProvKey SelectOne(string command) {
			
			List<EhrProvKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrProvKey objects from the database using a query.</summary>
		public static List<EhrProvKey> SelectMany(string command) {
			
			List<EhrProvKey> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrProvKey> TableToList(DataTable table) {
			List<EhrProvKey> retVal=new List<EhrProvKey>();
			EhrProvKey ehrProvKey;
			foreach(DataRow row in table.Rows) {
				ehrProvKey=new EhrProvKey();
				ehrProvKey.EhrProvKeyNum= PIn.Long  (row["EhrProvKeyNum"].ToString());
				ehrProvKey.PatNum       = PIn.Long  (row["PatNum"].ToString());
				ehrProvKey.LName        = PIn.String(row["LName"].ToString());
				ehrProvKey.FName        = PIn.String(row["FName"].ToString());
				ehrProvKey.ProvKey      = PIn.String(row["ProvKey"].ToString());
				ehrProvKey.FullTimeEquiv= PIn.Float (row["FullTimeEquiv"].ToString());
				ehrProvKey.Notes        = PIn.String(row["Notes"].ToString());
				ehrProvKey.YearValue    = PIn.Int   (row["YearValue"].ToString());
				retVal.Add(ehrProvKey);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrProvKey into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrProvKey> listEhrProvKeys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrProvKey";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrProvKeyNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("LName");
			table.Columns.Add("FName");
			table.Columns.Add("ProvKey");
			table.Columns.Add("FullTimeEquiv");
			table.Columns.Add("Notes");
			table.Columns.Add("YearValue");
			foreach(EhrProvKey ehrProvKey in listEhrProvKeys) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrProvKey.EhrProvKeyNum),
					POut.Long  (ehrProvKey.PatNum),
					            ehrProvKey.LName,
					            ehrProvKey.FName,
					            ehrProvKey.ProvKey,
					POut.Float (ehrProvKey.FullTimeEquiv),
					            ehrProvKey.Notes,
					POut.Int   (ehrProvKey.YearValue),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrProvKey into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrProvKey ehrProvKey) {
			return Insert(ehrProvKey,false);
		}

		///<summary>Inserts one EhrProvKey into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrProvKey ehrProvKey,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				ehrProvKey.EhrProvKeyNum=ReplicationServers.GetKey("ehrprovkey","EhrProvKeyNum");
			}
			string command="INSERT INTO ehrprovkey (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EhrProvKeyNum,";
			}
			command+="PatNum,LName,FName,ProvKey,FullTimeEquiv,Notes,YearValue) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(ehrProvKey.EhrProvKeyNum)+",";
			}
			command+=
				     POut.Long  (ehrProvKey.PatNum)+","
				+"'"+POut.String(ehrProvKey.LName)+"',"
				+"'"+POut.String(ehrProvKey.FName)+"',"
				+"'"+POut.String(ehrProvKey.ProvKey)+"',"
				+    POut.Float (ehrProvKey.FullTimeEquiv)+","
				+    DbHelper.ParamChar+"paramNotes,"
				+    POut.Int   (ehrProvKey.YearValue)+")";
			if(ehrProvKey.Notes==null) {
				ehrProvKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrProvKey.Notes));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				ehrProvKey.EhrProvKeyNum=Db.NonQ(command,true,"EhrProvKeyNum","ehrProvKey",paramNotes);
			}
			return ehrProvKey.EhrProvKeyNum;
		}

		///<summary>Inserts one EhrProvKey into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrProvKey ehrProvKey) {
			return InsertNoCache(ehrProvKey,false);
		}

		///<summary>Inserts one EhrProvKey into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrProvKey ehrProvKey,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO ehrprovkey (";
			if(!useExistingPK && isRandomKeys) {
				ehrProvKey.EhrProvKeyNum=ReplicationServers.GetKeyNoCache("ehrprovkey","EhrProvKeyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrProvKeyNum,";
			}
			command+="PatNum,LName,FName,ProvKey,FullTimeEquiv,Notes,YearValue) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrProvKey.EhrProvKeyNum)+",";
			}
			command+=
				     POut.Long  (ehrProvKey.PatNum)+","
				+"'"+POut.String(ehrProvKey.LName)+"',"
				+"'"+POut.String(ehrProvKey.FName)+"',"
				+"'"+POut.String(ehrProvKey.ProvKey)+"',"
				+    POut.Float (ehrProvKey.FullTimeEquiv)+","
				+    DbHelper.ParamChar+"paramNotes,"
				+    POut.Int   (ehrProvKey.YearValue)+")";
			if(ehrProvKey.Notes==null) {
				ehrProvKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrProvKey.Notes));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNotes);
			}
			else {
				ehrProvKey.EhrProvKeyNum=Db.NonQ(command,true,"EhrProvKeyNum","ehrProvKey",paramNotes);
			}
			return ehrProvKey.EhrProvKeyNum;
		}

		///<summary>Updates one EhrProvKey in the database.</summary>
		public static void Update(EhrProvKey ehrProvKey) {
			string command="UPDATE ehrprovkey SET "
				+"PatNum       =  "+POut.Long  (ehrProvKey.PatNum)+", "
				+"LName        = '"+POut.String(ehrProvKey.LName)+"', "
				+"FName        = '"+POut.String(ehrProvKey.FName)+"', "
				+"ProvKey      = '"+POut.String(ehrProvKey.ProvKey)+"', "
				+"FullTimeEquiv=  "+POut.Float (ehrProvKey.FullTimeEquiv)+", "
				+"Notes        =  "+DbHelper.ParamChar+"paramNotes, "
				+"YearValue    =  "+POut.Int   (ehrProvKey.YearValue)+" "
				+"WHERE EhrProvKeyNum = "+POut.Long(ehrProvKey.EhrProvKeyNum);
			if(ehrProvKey.Notes==null) {
				ehrProvKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrProvKey.Notes));
			Db.NonQ(command,paramNotes);
		}

		///<summary>Updates one EhrProvKey in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrProvKey ehrProvKey,EhrProvKey oldEhrProvKey) {
			string command="";
			if(ehrProvKey.PatNum != oldEhrProvKey.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(ehrProvKey.PatNum)+"";
			}
			if(ehrProvKey.LName != oldEhrProvKey.LName) {
				if(command!="") { command+=",";}
				command+="LName = '"+POut.String(ehrProvKey.LName)+"'";
			}
			if(ehrProvKey.FName != oldEhrProvKey.FName) {
				if(command!="") { command+=",";}
				command+="FName = '"+POut.String(ehrProvKey.FName)+"'";
			}
			if(ehrProvKey.ProvKey != oldEhrProvKey.ProvKey) {
				if(command!="") { command+=",";}
				command+="ProvKey = '"+POut.String(ehrProvKey.ProvKey)+"'";
			}
			if(ehrProvKey.FullTimeEquiv != oldEhrProvKey.FullTimeEquiv) {
				if(command!="") { command+=",";}
				command+="FullTimeEquiv = "+POut.Float(ehrProvKey.FullTimeEquiv)+"";
			}
			if(ehrProvKey.Notes != oldEhrProvKey.Notes) {
				if(command!="") { command+=",";}
				command+="Notes = "+DbHelper.ParamChar+"paramNotes";
			}
			if(ehrProvKey.YearValue != oldEhrProvKey.YearValue) {
				if(command!="") { command+=",";}
				command+="YearValue = "+POut.Int(ehrProvKey.YearValue)+"";
			}
			if(command=="") {
				return false;
			}
			if(ehrProvKey.Notes==null) {
				ehrProvKey.Notes="";
			}
			OdSqlParameter paramNotes=new OdSqlParameter("paramNotes",OdDbType.Text,POut.StringParam(ehrProvKey.Notes));
			command="UPDATE ehrprovkey SET "+command
				+" WHERE EhrProvKeyNum = "+POut.Long(ehrProvKey.EhrProvKeyNum);
			Db.NonQ(command,paramNotes);
			return true;
		}

		///<summary>Returns true if Update(EhrProvKey,EhrProvKey) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrProvKey ehrProvKey,EhrProvKey oldEhrProvKey) {
			if(ehrProvKey.PatNum != oldEhrProvKey.PatNum) {
				return true;
			}
			if(ehrProvKey.LName != oldEhrProvKey.LName) {
				return true;
			}
			if(ehrProvKey.FName != oldEhrProvKey.FName) {
				return true;
			}
			if(ehrProvKey.ProvKey != oldEhrProvKey.ProvKey) {
				return true;
			}
			if(ehrProvKey.FullTimeEquiv != oldEhrProvKey.FullTimeEquiv) {
				return true;
			}
			if(ehrProvKey.Notes != oldEhrProvKey.Notes) {
				return true;
			}
			if(ehrProvKey.YearValue != oldEhrProvKey.YearValue) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrProvKey from the database.</summary>
		public static void Delete(long ehrProvKeyNum) {
			string command="DELETE FROM ehrprovkey "
				+"WHERE EhrProvKeyNum = "+POut.Long(ehrProvKeyNum);
			Db.NonQ(command);
		}

	}
}