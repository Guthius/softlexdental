//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcCodeNoteCrud {
		///<summary>Gets one ProcCodeNote object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcCodeNote SelectOne(long procCodeNoteNum) {
			string command="SELECT * FROM proccodenote "
				+"WHERE ProcCodeNoteNum = "+POut.Long(procCodeNoteNum);
			List<ProcCodeNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcCodeNote object from the database using a query.</summary>
		public static ProcCodeNote SelectOne(string command) {
			
			List<ProcCodeNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcCodeNote objects from the database using a query.</summary>
		public static List<ProcCodeNote> SelectMany(string command) {
			
			List<ProcCodeNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcCodeNote> TableToList(DataTable table) {
			List<ProcCodeNote> retVal=new List<ProcCodeNote>();
			ProcCodeNote procCodeNote;
			foreach(DataRow row in table.Rows) {
				procCodeNote=new ProcCodeNote();
				procCodeNote.ProcCodeNoteNum= PIn.Long  (row["ProcCodeNoteNum"].ToString());
				procCodeNote.CodeNum        = PIn.Long  (row["CodeNum"].ToString());
				procCodeNote.ProvNum        = PIn.Long  (row["ProvNum"].ToString());
				procCodeNote.Note           = PIn.String(row["Note"].ToString());
				procCodeNote.ProcTime       = PIn.String(row["ProcTime"].ToString());
				procCodeNote.ProcStatus     = (OpenDentBusiness.ProcStat)PIn.Int(row["ProcStatus"].ToString());
				retVal.Add(procCodeNote);
			}
			return retVal;
		}

		///<summary>Converts a list of ProcCodeNote into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcCodeNote> listProcCodeNotes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProcCodeNote";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProcCodeNoteNum");
			table.Columns.Add("CodeNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("Note");
			table.Columns.Add("ProcTime");
			table.Columns.Add("ProcStatus");
			foreach(ProcCodeNote procCodeNote in listProcCodeNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (procCodeNote.ProcCodeNoteNum),
					POut.Long  (procCodeNote.CodeNum),
					POut.Long  (procCodeNote.ProvNum),
					            procCodeNote.Note,
					            procCodeNote.ProcTime,
					POut.Int   ((int)procCodeNote.ProcStatus),
				});
			}
			return table;
		}

		///<summary>Inserts one ProcCodeNote into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcCodeNote procCodeNote) {
			return Insert(procCodeNote,false);
		}

		///<summary>Inserts one ProcCodeNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcCodeNote procCodeNote,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				procCodeNote.ProcCodeNoteNum=ReplicationServers.GetKey("proccodenote","ProcCodeNoteNum");
			}
			string command="INSERT INTO proccodenote (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ProcCodeNoteNum,";
			}
			command+="CodeNum,ProvNum,Note,ProcTime,ProcStatus) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(procCodeNote.ProcCodeNoteNum)+",";
			}
			command+=
				     POut.Long  (procCodeNote.CodeNum)+","
				+    POut.Long  (procCodeNote.ProvNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(procCodeNote.ProcTime)+"',"
				+    POut.Int   ((int)procCodeNote.ProcStatus)+")";
			if(procCodeNote.Note==null) {
				procCodeNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(procCodeNote.Note));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				procCodeNote.ProcCodeNoteNum=Db.NonQ(command,true,"ProcCodeNoteNum","procCodeNote",paramNote);
			}
			return procCodeNote.ProcCodeNoteNum;
		}

		///<summary>Inserts one ProcCodeNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcCodeNote procCodeNote) {
			return InsertNoCache(procCodeNote,false);
		}

		///<summary>Inserts one ProcCodeNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcCodeNote procCodeNote,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO proccodenote (";
			if(!useExistingPK && isRandomKeys) {
				procCodeNote.ProcCodeNoteNum=ReplicationServers.GetKeyNoCache("proccodenote","ProcCodeNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcCodeNoteNum,";
			}
			command+="CodeNum,ProvNum,Note,ProcTime,ProcStatus) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procCodeNote.ProcCodeNoteNum)+",";
			}
			command+=
				     POut.Long  (procCodeNote.CodeNum)+","
				+    POut.Long  (procCodeNote.ProvNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(procCodeNote.ProcTime)+"',"
				+    POut.Int   ((int)procCodeNote.ProcStatus)+")";
			if(procCodeNote.Note==null) {
				procCodeNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(procCodeNote.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				procCodeNote.ProcCodeNoteNum=Db.NonQ(command,true,"ProcCodeNoteNum","procCodeNote",paramNote);
			}
			return procCodeNote.ProcCodeNoteNum;
		}

		///<summary>Updates one ProcCodeNote in the database.</summary>
		public static void Update(ProcCodeNote procCodeNote) {
			string command="UPDATE proccodenote SET "
				+"CodeNum        =  "+POut.Long  (procCodeNote.CodeNum)+", "
				+"ProvNum        =  "+POut.Long  (procCodeNote.ProvNum)+", "
				+"Note           =  "+DbHelper.ParamChar+"paramNote, "
				+"ProcTime       = '"+POut.String(procCodeNote.ProcTime)+"', "
				+"ProcStatus     =  "+POut.Int   ((int)procCodeNote.ProcStatus)+" "
				+"WHERE ProcCodeNoteNum = "+POut.Long(procCodeNote.ProcCodeNoteNum);
			if(procCodeNote.Note==null) {
				procCodeNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(procCodeNote.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one ProcCodeNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcCodeNote procCodeNote,ProcCodeNote oldProcCodeNote) {
			string command="";
			if(procCodeNote.CodeNum != oldProcCodeNote.CodeNum) {
				if(command!="") { command+=",";}
				command+="CodeNum = "+POut.Long(procCodeNote.CodeNum)+"";
			}
			if(procCodeNote.ProvNum != oldProcCodeNote.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(procCodeNote.ProvNum)+"";
			}
			if(procCodeNote.Note != oldProcCodeNote.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(procCodeNote.ProcTime != oldProcCodeNote.ProcTime) {
				if(command!="") { command+=",";}
				command+="ProcTime = '"+POut.String(procCodeNote.ProcTime)+"'";
			}
			if(procCodeNote.ProcStatus != oldProcCodeNote.ProcStatus) {
				if(command!="") { command+=",";}
				command+="ProcStatus = "+POut.Int   ((int)procCodeNote.ProcStatus)+"";
			}
			if(command=="") {
				return false;
			}
			if(procCodeNote.Note==null) {
				procCodeNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(procCodeNote.Note));
			command="UPDATE proccodenote SET "+command
				+" WHERE ProcCodeNoteNum = "+POut.Long(procCodeNote.ProcCodeNoteNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(ProcCodeNote,ProcCodeNote) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProcCodeNote procCodeNote,ProcCodeNote oldProcCodeNote) {
			if(procCodeNote.CodeNum != oldProcCodeNote.CodeNum) {
				return true;
			}
			if(procCodeNote.ProvNum != oldProcCodeNote.ProvNum) {
				return true;
			}
			if(procCodeNote.Note != oldProcCodeNote.Note) {
				return true;
			}
			if(procCodeNote.ProcTime != oldProcCodeNote.ProcTime) {
				return true;
			}
			if(procCodeNote.ProcStatus != oldProcCodeNote.ProcStatus) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProcCodeNote from the database.</summary>
		public static void Delete(long procCodeNoteNum) {
			string command="DELETE FROM proccodenote "
				+"WHERE ProcCodeNoteNum = "+POut.Long(procCodeNoteNum);
			Db.NonQ(command);
		}

	}
}