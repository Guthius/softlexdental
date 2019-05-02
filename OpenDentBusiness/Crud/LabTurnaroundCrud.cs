//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LabTurnaroundCrud {
		///<summary>Gets one LabTurnaround object from the database using the primary key.  Returns null if not found.</summary>
		public static LabTurnaround SelectOne(long labTurnaroundNum) {
			string command="SELECT * FROM labturnaround "
				+"WHERE LabTurnaroundNum = "+POut.Long(labTurnaroundNum);
			List<LabTurnaround> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LabTurnaround object from the database using a query.</summary>
		public static LabTurnaround SelectOne(string command) {
			
			List<LabTurnaround> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LabTurnaround objects from the database using a query.</summary>
		public static List<LabTurnaround> SelectMany(string command) {
			
			List<LabTurnaround> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<LabTurnaround> TableToList(DataTable table) {
			List<LabTurnaround> retVal=new List<LabTurnaround>();
			LabTurnaround labTurnaround;
			foreach(DataRow row in table.Rows) {
				labTurnaround=new LabTurnaround();
				labTurnaround.LabTurnaroundNum= PIn.Long  (row["LabTurnaroundNum"].ToString());
				labTurnaround.LaboratoryNum   = PIn.Long  (row["LaboratoryNum"].ToString());
				labTurnaround.Description     = PIn.String(row["Description"].ToString());
				labTurnaround.DaysPublished   = PIn.Int   (row["DaysPublished"].ToString());
				labTurnaround.DaysActual      = PIn.Int   (row["DaysActual"].ToString());
				retVal.Add(labTurnaround);
			}
			return retVal;
		}

		///<summary>Converts a list of LabTurnaround into a DataTable.</summary>
		public static DataTable ListToTable(List<LabTurnaround> listLabTurnarounds,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="LabTurnaround";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LabTurnaroundNum");
			table.Columns.Add("LaboratoryNum");
			table.Columns.Add("Description");
			table.Columns.Add("DaysPublished");
			table.Columns.Add("DaysActual");
			foreach(LabTurnaround labTurnaround in listLabTurnarounds) {
				table.Rows.Add(new object[] {
					POut.Long  (labTurnaround.LabTurnaroundNum),
					POut.Long  (labTurnaround.LaboratoryNum),
					            labTurnaround.Description,
					POut.Int   (labTurnaround.DaysPublished),
					POut.Int   (labTurnaround.DaysActual),
				});
			}
			return table;
		}

		///<summary>Inserts one LabTurnaround into the database.  Returns the new priKey.</summary>
		public static long Insert(LabTurnaround labTurnaround) {
			return Insert(labTurnaround,false);
		}

		///<summary>Inserts one LabTurnaround into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(LabTurnaround labTurnaround,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				labTurnaround.LabTurnaroundNum=ReplicationServers.GetKey("labturnaround","LabTurnaroundNum");
			}
			string command="INSERT INTO labturnaround (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="LabTurnaroundNum,";
			}
			command+="LaboratoryNum,Description,DaysPublished,DaysActual) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(labTurnaround.LabTurnaroundNum)+",";
			}
			command+=
				     POut.Long  (labTurnaround.LaboratoryNum)+","
				+"'"+POut.String(labTurnaround.Description)+"',"
				+    POut.Int   (labTurnaround.DaysPublished)+","
				+    POut.Int   (labTurnaround.DaysActual)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				labTurnaround.LabTurnaroundNum=Db.NonQ(command,true,"LabTurnaroundNum","labTurnaround");
			}
			return labTurnaround.LabTurnaroundNum;
		}

		///<summary>Inserts one LabTurnaround into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabTurnaround labTurnaround) {
			return InsertNoCache(labTurnaround,false);
		}

		///<summary>Inserts one LabTurnaround into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabTurnaround labTurnaround,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO labturnaround (";
			if(!useExistingPK && isRandomKeys) {
				labTurnaround.LabTurnaroundNum=ReplicationServers.GetKeyNoCache("labturnaround","LabTurnaroundNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="LabTurnaroundNum,";
			}
			command+="LaboratoryNum,Description,DaysPublished,DaysActual) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(labTurnaround.LabTurnaroundNum)+",";
			}
			command+=
				     POut.Long  (labTurnaround.LaboratoryNum)+","
				+"'"+POut.String(labTurnaround.Description)+"',"
				+    POut.Int   (labTurnaround.DaysPublished)+","
				+    POut.Int   (labTurnaround.DaysActual)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				labTurnaround.LabTurnaroundNum=Db.NonQ(command,true,"LabTurnaroundNum","labTurnaround");
			}
			return labTurnaround.LabTurnaroundNum;
		}

		///<summary>Updates one LabTurnaround in the database.</summary>
		public static void Update(LabTurnaround labTurnaround) {
			string command="UPDATE labturnaround SET "
				+"LaboratoryNum   =  "+POut.Long  (labTurnaround.LaboratoryNum)+", "
				+"Description     = '"+POut.String(labTurnaround.Description)+"', "
				+"DaysPublished   =  "+POut.Int   (labTurnaround.DaysPublished)+", "
				+"DaysActual      =  "+POut.Int   (labTurnaround.DaysActual)+" "
				+"WHERE LabTurnaroundNum = "+POut.Long(labTurnaround.LabTurnaroundNum);
			Db.NonQ(command);
		}

		///<summary>Updates one LabTurnaround in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(LabTurnaround labTurnaround,LabTurnaround oldLabTurnaround) {
			string command="";
			if(labTurnaround.LaboratoryNum != oldLabTurnaround.LaboratoryNum) {
				if(command!="") { command+=",";}
				command+="LaboratoryNum = "+POut.Long(labTurnaround.LaboratoryNum)+"";
			}
			if(labTurnaround.Description != oldLabTurnaround.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(labTurnaround.Description)+"'";
			}
			if(labTurnaround.DaysPublished != oldLabTurnaround.DaysPublished) {
				if(command!="") { command+=",";}
				command+="DaysPublished = "+POut.Int(labTurnaround.DaysPublished)+"";
			}
			if(labTurnaround.DaysActual != oldLabTurnaround.DaysActual) {
				if(command!="") { command+=",";}
				command+="DaysActual = "+POut.Int(labTurnaround.DaysActual)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE labturnaround SET "+command
				+" WHERE LabTurnaroundNum = "+POut.Long(labTurnaround.LabTurnaroundNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(LabTurnaround,LabTurnaround) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(LabTurnaround labTurnaround,LabTurnaround oldLabTurnaround) {
			if(labTurnaround.LaboratoryNum != oldLabTurnaround.LaboratoryNum) {
				return true;
			}
			if(labTurnaround.Description != oldLabTurnaround.Description) {
				return true;
			}
			if(labTurnaround.DaysPublished != oldLabTurnaround.DaysPublished) {
				return true;
			}
			if(labTurnaround.DaysActual != oldLabTurnaround.DaysActual) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one LabTurnaround from the database.</summary>
		public static void Delete(long labTurnaroundNum) {
			string command="DELETE FROM labturnaround "
				+"WHERE LabTurnaroundNum = "+POut.Long(labTurnaroundNum);
			Db.NonQ(command);
		}

	}
}