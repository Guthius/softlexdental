//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EncounterCrud {
		///<summary>Gets one Encounter object from the database using the primary key.  Returns null if not found.</summary>
		public static Encounter SelectOne(long encounterNum) {
			string command="SELECT * FROM encounter "
				+"WHERE EncounterNum = "+POut.Long(encounterNum);
			List<Encounter> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Encounter object from the database using a query.</summary>
		public static Encounter SelectOne(string command) {
			
			List<Encounter> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Encounter objects from the database using a query.</summary>
		public static List<Encounter> SelectMany(string command) {
			
			List<Encounter> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Encounter> TableToList(DataTable table) {
			List<Encounter> retVal=new List<Encounter>();
			Encounter encounter;
			foreach(DataRow row in table.Rows) {
				encounter=new Encounter();
				encounter.EncounterNum = PIn.Long  (row["EncounterNum"].ToString());
				encounter.PatNum       = PIn.Long  (row["PatNum"].ToString());
				encounter.ProvNum      = PIn.Long  (row["ProvNum"].ToString());
				encounter.CodeValue    = PIn.String(row["CodeValue"].ToString());
				encounter.CodeSystem   = PIn.String(row["CodeSystem"].ToString());
				encounter.Note         = PIn.String(row["Note"].ToString());
				encounter.DateEncounter= PIn.Date  (row["DateEncounter"].ToString());
				retVal.Add(encounter);
			}
			return retVal;
		}

		///<summary>Converts a list of Encounter into a DataTable.</summary>
		public static DataTable ListToTable(List<Encounter> listEncounters,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Encounter";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EncounterNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("CodeValue");
			table.Columns.Add("CodeSystem");
			table.Columns.Add("Note");
			table.Columns.Add("DateEncounter");
			foreach(Encounter encounter in listEncounters) {
				table.Rows.Add(new object[] {
					POut.Long  (encounter.EncounterNum),
					POut.Long  (encounter.PatNum),
					POut.Long  (encounter.ProvNum),
					            encounter.CodeValue,
					            encounter.CodeSystem,
					            encounter.Note,
					POut.DateT (encounter.DateEncounter,false),
				});
			}
			return table;
		}

		///<summary>Inserts one Encounter into the database.  Returns the new priKey.</summary>
		public static long Insert(Encounter encounter) {
			return Insert(encounter,false);
		}

		///<summary>Inserts one Encounter into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Encounter encounter,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				encounter.EncounterNum=ReplicationServers.GetKey("encounter","EncounterNum");
			}
			string command="INSERT INTO encounter (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EncounterNum,";
			}
			command+="PatNum,ProvNum,CodeValue,CodeSystem,Note,DateEncounter) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(encounter.EncounterNum)+",";
			}
			command+=
				     POut.Long  (encounter.PatNum)+","
				+    POut.Long  (encounter.ProvNum)+","
				+"'"+POut.String(encounter.CodeValue)+"',"
				+"'"+POut.String(encounter.CodeSystem)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Date  (encounter.DateEncounter)+")";
			if(encounter.Note==null) {
				encounter.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(encounter.Note));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				encounter.EncounterNum=Db.NonQ(command,true,"EncounterNum","encounter",paramNote);
			}
			return encounter.EncounterNum;
		}

		///<summary>Inserts one Encounter into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Encounter encounter) {
			return InsertNoCache(encounter,false);
		}

		///<summary>Inserts one Encounter into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Encounter encounter,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO encounter (";
			if(!useExistingPK && isRandomKeys) {
				encounter.EncounterNum=ReplicationServers.GetKeyNoCache("encounter","EncounterNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EncounterNum,";
			}
			command+="PatNum,ProvNum,CodeValue,CodeSystem,Note,DateEncounter) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(encounter.EncounterNum)+",";
			}
			command+=
				     POut.Long  (encounter.PatNum)+","
				+    POut.Long  (encounter.ProvNum)+","
				+"'"+POut.String(encounter.CodeValue)+"',"
				+"'"+POut.String(encounter.CodeSystem)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Date  (encounter.DateEncounter)+")";
			if(encounter.Note==null) {
				encounter.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(encounter.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				encounter.EncounterNum=Db.NonQ(command,true,"EncounterNum","encounter",paramNote);
			}
			return encounter.EncounterNum;
		}

		///<summary>Updates one Encounter in the database.</summary>
		public static void Update(Encounter encounter) {
			string command="UPDATE encounter SET "
				+"PatNum       =  "+POut.Long  (encounter.PatNum)+", "
				+"ProvNum      =  "+POut.Long  (encounter.ProvNum)+", "
				+"CodeValue    = '"+POut.String(encounter.CodeValue)+"', "
				+"CodeSystem   = '"+POut.String(encounter.CodeSystem)+"', "
				+"Note         =  "+DbHelper.ParamChar+"paramNote, "
				+"DateEncounter=  "+POut.Date  (encounter.DateEncounter)+" "
				+"WHERE EncounterNum = "+POut.Long(encounter.EncounterNum);
			if(encounter.Note==null) {
				encounter.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(encounter.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one Encounter in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Encounter encounter,Encounter oldEncounter) {
			string command="";
			if(encounter.PatNum != oldEncounter.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(encounter.PatNum)+"";
			}
			if(encounter.ProvNum != oldEncounter.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(encounter.ProvNum)+"";
			}
			if(encounter.CodeValue != oldEncounter.CodeValue) {
				if(command!="") { command+=",";}
				command+="CodeValue = '"+POut.String(encounter.CodeValue)+"'";
			}
			if(encounter.CodeSystem != oldEncounter.CodeSystem) {
				if(command!="") { command+=",";}
				command+="CodeSystem = '"+POut.String(encounter.CodeSystem)+"'";
			}
			if(encounter.Note != oldEncounter.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(encounter.DateEncounter.Date != oldEncounter.DateEncounter.Date) {
				if(command!="") { command+=",";}
				command+="DateEncounter = "+POut.Date(encounter.DateEncounter)+"";
			}
			if(command=="") {
				return false;
			}
			if(encounter.Note==null) {
				encounter.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(encounter.Note));
			command="UPDATE encounter SET "+command
				+" WHERE EncounterNum = "+POut.Long(encounter.EncounterNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(Encounter,Encounter) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Encounter encounter,Encounter oldEncounter) {
			if(encounter.PatNum != oldEncounter.PatNum) {
				return true;
			}
			if(encounter.ProvNum != oldEncounter.ProvNum) {
				return true;
			}
			if(encounter.CodeValue != oldEncounter.CodeValue) {
				return true;
			}
			if(encounter.CodeSystem != oldEncounter.CodeSystem) {
				return true;
			}
			if(encounter.Note != oldEncounter.Note) {
				return true;
			}
			if(encounter.DateEncounter.Date != oldEncounter.DateEncounter.Date) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Encounter from the database.</summary>
		public static void Delete(long encounterNum) {
			string command="DELETE FROM encounter "
				+"WHERE EncounterNum = "+POut.Long(encounterNum);
			Db.NonQ(command);
		}

	}
}