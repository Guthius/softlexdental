//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class AutoNoteCrud {
		///<summary>Gets one AutoNote object from the database using the primary key.  Returns null if not found.</summary>
		public static AutoNote SelectOne(long autoNoteNum) {
			string command="SELECT * FROM autonote "
				+"WHERE AutoNoteNum = "+POut.Long(autoNoteNum);
			List<AutoNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AutoNote object from the database using a query.</summary>
		public static AutoNote SelectOne(string command) {
			
			List<AutoNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AutoNote objects from the database using a query.</summary>
		public static List<AutoNote> SelectMany(string command) {
			
			List<AutoNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<AutoNote> TableToList(DataTable table) {
			List<AutoNote> retVal=new List<AutoNote>();
			AutoNote autoNote;
			foreach(DataRow row in table.Rows) {
				autoNote=new AutoNote();
				autoNote.AutoNoteNum = PIn.Long  (row["AutoNoteNum"].ToString());
				autoNote.AutoNoteName= PIn.String(row["AutoNoteName"].ToString());
				autoNote.MainText    = PIn.String(row["MainText"].ToString());
				autoNote.Category    = PIn.Long  (row["Category"].ToString());
				retVal.Add(autoNote);
			}
			return retVal;
		}

		///<summary>Converts a list of AutoNote into a DataTable.</summary>
		public static DataTable ListToTable(List<AutoNote> listAutoNotes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="AutoNote";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AutoNoteNum");
			table.Columns.Add("AutoNoteName");
			table.Columns.Add("MainText");
			table.Columns.Add("Category");
			foreach(AutoNote autoNote in listAutoNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (autoNote.AutoNoteNum),
					            autoNote.AutoNoteName,
					            autoNote.MainText,
					POut.Long  (autoNote.Category),
				});
			}
			return table;
		}

		///<summary>Inserts one AutoNote into the database.  Returns the new priKey.</summary>
		public static long Insert(AutoNote autoNote) {
			return Insert(autoNote,false);
		}

		///<summary>Inserts one AutoNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(AutoNote autoNote,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				autoNote.AutoNoteNum=ReplicationServers.GetKey("autonote","AutoNoteNum");
			}
			string command="INSERT INTO autonote (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="AutoNoteNum,";
			}
			command+="AutoNoteName,MainText,Category) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(autoNote.AutoNoteNum)+",";
			}
			command+=
				 "'"+POut.String(autoNote.AutoNoteName)+"',"
				+    DbHelper.ParamChar+"paramMainText,"
				+    POut.Long  (autoNote.Category)+")";
			if(autoNote.MainText==null) {
				autoNote.MainText="";
			}
			OdSqlParameter paramMainText=new OdSqlParameter("paramMainText",OdDbType.Text,POut.StringParam(autoNote.MainText));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramMainText);
			}
			else {
				autoNote.AutoNoteNum=Db.NonQ(command,true,"AutoNoteNum","autoNote",paramMainText);
			}
			return autoNote.AutoNoteNum;
		}

		///<summary>Inserts one AutoNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AutoNote autoNote) {
			return InsertNoCache(autoNote,false);
		}

		///<summary>Inserts one AutoNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AutoNote autoNote,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO autonote (";
			if(!useExistingPK && isRandomKeys) {
				autoNote.AutoNoteNum=ReplicationServers.GetKeyNoCache("autonote","AutoNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AutoNoteNum,";
			}
			command+="AutoNoteName,MainText,Category) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(autoNote.AutoNoteNum)+",";
			}
			command+=
				 "'"+POut.String(autoNote.AutoNoteName)+"',"
				+    DbHelper.ParamChar+"paramMainText,"
				+    POut.Long  (autoNote.Category)+")";
			if(autoNote.MainText==null) {
				autoNote.MainText="";
			}
			OdSqlParameter paramMainText=new OdSqlParameter("paramMainText",OdDbType.Text,POut.StringParam(autoNote.MainText));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramMainText);
			}
			else {
				autoNote.AutoNoteNum=Db.NonQ(command,true,"AutoNoteNum","autoNote",paramMainText);
			}
			return autoNote.AutoNoteNum;
		}

		///<summary>Updates one AutoNote in the database.</summary>
		public static void Update(AutoNote autoNote) {
			string command="UPDATE autonote SET "
				+"AutoNoteName= '"+POut.String(autoNote.AutoNoteName)+"', "
				+"MainText    =  "+DbHelper.ParamChar+"paramMainText, "
				+"Category    =  "+POut.Long  (autoNote.Category)+" "
				+"WHERE AutoNoteNum = "+POut.Long(autoNote.AutoNoteNum);
			if(autoNote.MainText==null) {
				autoNote.MainText="";
			}
			OdSqlParameter paramMainText=new OdSqlParameter("paramMainText",OdDbType.Text,POut.StringParam(autoNote.MainText));
			Db.NonQ(command,paramMainText);
		}

		///<summary>Updates one AutoNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(AutoNote autoNote,AutoNote oldAutoNote) {
			string command="";
			if(autoNote.AutoNoteName != oldAutoNote.AutoNoteName) {
				if(command!="") { command+=",";}
				command+="AutoNoteName = '"+POut.String(autoNote.AutoNoteName)+"'";
			}
			if(autoNote.MainText != oldAutoNote.MainText) {
				if(command!="") { command+=",";}
				command+="MainText = "+DbHelper.ParamChar+"paramMainText";
			}
			if(autoNote.Category != oldAutoNote.Category) {
				if(command!="") { command+=",";}
				command+="Category = "+POut.Long(autoNote.Category)+"";
			}
			if(command=="") {
				return false;
			}
			if(autoNote.MainText==null) {
				autoNote.MainText="";
			}
			OdSqlParameter paramMainText=new OdSqlParameter("paramMainText",OdDbType.Text,POut.StringParam(autoNote.MainText));
			command="UPDATE autonote SET "+command
				+" WHERE AutoNoteNum = "+POut.Long(autoNote.AutoNoteNum);
			Db.NonQ(command,paramMainText);
			return true;
		}

		///<summary>Returns true if Update(AutoNote,AutoNote) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(AutoNote autoNote,AutoNote oldAutoNote) {
			if(autoNote.AutoNoteName != oldAutoNote.AutoNoteName) {
				return true;
			}
			if(autoNote.MainText != oldAutoNote.MainText) {
				return true;
			}
			if(autoNote.Category != oldAutoNote.Category) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one AutoNote from the database.</summary>
		public static void Delete(long autoNoteNum) {
			string command="DELETE FROM autonote "
				+"WHERE AutoNoteNum = "+POut.Long(autoNoteNum);
			Db.NonQ(command);
		}

	}
}