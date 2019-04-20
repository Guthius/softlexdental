//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcApptColorCrud {
		///<summary>Gets one ProcApptColor object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcApptColor SelectOne(long procApptColorNum) {
			string command="SELECT * FROM procapptcolor "
				+"WHERE ProcApptColorNum = "+POut.Long(procApptColorNum);
			List<ProcApptColor> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcApptColor object from the database using a query.</summary>
		public static ProcApptColor SelectOne(string command) {
			
			List<ProcApptColor> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcApptColor objects from the database using a query.</summary>
		public static List<ProcApptColor> SelectMany(string command) {
			
			List<ProcApptColor> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcApptColor> TableToList(DataTable table) {
			List<ProcApptColor> retVal=new List<ProcApptColor>();
			ProcApptColor procApptColor;
			foreach(DataRow row in table.Rows) {
				procApptColor=new ProcApptColor();
				procApptColor.ProcApptColorNum= PIn.Long  (row["ProcApptColorNum"].ToString());
				procApptColor.CodeRange       = PIn.String(row["CodeRange"].ToString());
				procApptColor.ShowPreviousDate= PIn.Bool  (row["ShowPreviousDate"].ToString());
				procApptColor.ColorText       = Color.FromArgb(PIn.Int(row["ColorText"].ToString()));
				retVal.Add(procApptColor);
			}
			return retVal;
		}

		///<summary>Converts a list of ProcApptColor into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcApptColor> listProcApptColors,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProcApptColor";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProcApptColorNum");
			table.Columns.Add("CodeRange");
			table.Columns.Add("ShowPreviousDate");
			table.Columns.Add("ColorText");
			foreach(ProcApptColor procApptColor in listProcApptColors) {
				table.Rows.Add(new object[] {
					POut.Long  (procApptColor.ProcApptColorNum),
					            procApptColor.CodeRange,
					POut.Bool  (procApptColor.ShowPreviousDate),
					POut.Int   (procApptColor.ColorText.ToArgb()),
				});
			}
			return table;
		}

		///<summary>Inserts one ProcApptColor into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcApptColor procApptColor) {
			return Insert(procApptColor,false);
		}

		///<summary>Inserts one ProcApptColor into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcApptColor procApptColor,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				procApptColor.ProcApptColorNum=ReplicationServers.GetKey("procapptcolor","ProcApptColorNum");
			}
			string command="INSERT INTO procapptcolor (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcApptColorNum,";
			}
			command+="CodeRange,ShowPreviousDate,ColorText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procApptColor.ProcApptColorNum)+",";
			}
			command+=
				 "'"+POut.String(procApptColor.CodeRange)+"',"
				+    POut.Bool  (procApptColor.ShowPreviousDate)+","
				+    POut.Int   (procApptColor.ColorText.ToArgb())+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procApptColor.ProcApptColorNum=Db.NonQ(command,true,"ProcApptColorNum","procApptColor");
			}
			return procApptColor.ProcApptColorNum;
		}

		///<summary>Inserts one ProcApptColor into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcApptColor procApptColor) {
			return InsertNoCache(procApptColor,false);
		}

		///<summary>Inserts one ProcApptColor into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcApptColor procApptColor,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO procapptcolor (";
			if(!useExistingPK && isRandomKeys) {
				procApptColor.ProcApptColorNum=ReplicationServers.GetKeyNoCache("procapptcolor","ProcApptColorNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcApptColorNum,";
			}
			command+="CodeRange,ShowPreviousDate,ColorText) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procApptColor.ProcApptColorNum)+",";
			}
			command+=
				 "'"+POut.String(procApptColor.CodeRange)+"',"
				+    POut.Bool  (procApptColor.ShowPreviousDate)+","
				+    POut.Int   (procApptColor.ColorText.ToArgb())+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				procApptColor.ProcApptColorNum=Db.NonQ(command,true,"ProcApptColorNum","procApptColor");
			}
			return procApptColor.ProcApptColorNum;
		}

		///<summary>Updates one ProcApptColor in the database.</summary>
		public static void Update(ProcApptColor procApptColor) {
			string command="UPDATE procapptcolor SET "
				+"CodeRange       = '"+POut.String(procApptColor.CodeRange)+"', "
				+"ShowPreviousDate=  "+POut.Bool  (procApptColor.ShowPreviousDate)+", "
				+"ColorText       =  "+POut.Int   (procApptColor.ColorText.ToArgb())+" "
				+"WHERE ProcApptColorNum = "+POut.Long(procApptColor.ProcApptColorNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcApptColor in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcApptColor procApptColor,ProcApptColor oldProcApptColor) {
			string command="";
			if(procApptColor.CodeRange != oldProcApptColor.CodeRange) {
				if(command!="") { command+=",";}
				command+="CodeRange = '"+POut.String(procApptColor.CodeRange)+"'";
			}
			if(procApptColor.ShowPreviousDate != oldProcApptColor.ShowPreviousDate) {
				if(command!="") { command+=",";}
				command+="ShowPreviousDate = "+POut.Bool(procApptColor.ShowPreviousDate)+"";
			}
			if(procApptColor.ColorText != oldProcApptColor.ColorText) {
				if(command!="") { command+=",";}
				command+="ColorText = "+POut.Int(procApptColor.ColorText.ToArgb())+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE procapptcolor SET "+command
				+" WHERE ProcApptColorNum = "+POut.Long(procApptColor.ProcApptColorNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ProcApptColor,ProcApptColor) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProcApptColor procApptColor,ProcApptColor oldProcApptColor) {
			if(procApptColor.CodeRange != oldProcApptColor.CodeRange) {
				return true;
			}
			if(procApptColor.ShowPreviousDate != oldProcApptColor.ShowPreviousDate) {
				return true;
			}
			if(procApptColor.ColorText != oldProcApptColor.ColorText) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProcApptColor from the database.</summary>
		public static void Delete(long procApptColorNum) {
			string command="DELETE FROM procapptcolor "
				+"WHERE ProcApptColorNum = "+POut.Long(procApptColorNum);
			Db.NonQ(command);
		}

	}
}