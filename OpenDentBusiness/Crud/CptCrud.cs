//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CptCrud {
		///<summary>Gets one Cpt object from the database using the primary key.  Returns null if not found.</summary>
		public static Cpt SelectOne(long cptNum) {
			string command="SELECT * FROM cpt "
				+"WHERE CptNum = "+POut.Long(cptNum);
			List<Cpt> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Cpt object from the database using a query.</summary>
		public static Cpt SelectOne(string command) {
			
			List<Cpt> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Cpt objects from the database using a query.</summary>
		public static List<Cpt> SelectMany(string command) {
			
			List<Cpt> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Cpt> TableToList(DataTable table) {
			List<Cpt> retVal=new List<Cpt>();
			Cpt cpt;
			foreach(DataRow row in table.Rows) {
				cpt=new Cpt();
				cpt.CptNum     = PIn.Long  (row["CptNum"].ToString());
				cpt.CptCode    = PIn.String(row["CptCode"].ToString());
				cpt.Description= PIn.String(row["Description"].ToString());
				cpt.VersionIDs = PIn.String(row["VersionIDs"].ToString());
				retVal.Add(cpt);
			}
			return retVal;
		}

		///<summary>Converts a list of Cpt into a DataTable.</summary>
		public static DataTable ListToTable(List<Cpt> listCpts,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Cpt";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CptNum");
			table.Columns.Add("CptCode");
			table.Columns.Add("Description");
			table.Columns.Add("VersionIDs");
			foreach(Cpt cpt in listCpts) {
				table.Rows.Add(new object[] {
					POut.Long  (cpt.CptNum),
					            cpt.CptCode,
					            cpt.Description,
					            cpt.VersionIDs,
				});
			}
			return table;
		}

		///<summary>Inserts one Cpt into the database.  Returns the new priKey.</summary>
		public static long Insert(Cpt cpt) {
			return Insert(cpt,false);
		}

		///<summary>Inserts one Cpt into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Cpt cpt,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				cpt.CptNum=ReplicationServers.GetKey("cpt","CptNum");
			}
			string command="INSERT INTO cpt (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="CptNum,";
			}
			command+="CptCode,Description,VersionIDs) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(cpt.CptNum)+",";
			}
			command+=
				 "'"+POut.String(cpt.CptCode)+"',"
				+"'"+POut.String(cpt.Description)+"',"
				+"'"+POut.String(cpt.VersionIDs)+"')";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				cpt.CptNum=Db.NonQ(command,true,"CptNum","cpt");
			}
			return cpt.CptNum;
		}

		///<summary>Inserts one Cpt into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cpt cpt) {
			return InsertNoCache(cpt,false);
		}

		///<summary>Inserts one Cpt into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Cpt cpt,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO cpt (";
			if(!useExistingPK && isRandomKeys) {
				cpt.CptNum=ReplicationServers.GetKeyNoCache("cpt","CptNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CptNum,";
			}
			command+="CptCode,Description,VersionIDs) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(cpt.CptNum)+",";
			}
			command+=
				 "'"+POut.String(cpt.CptCode)+"',"
				+"'"+POut.String(cpt.Description)+"',"
				+"'"+POut.String(cpt.VersionIDs)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				cpt.CptNum=Db.NonQ(command,true,"CptNum","cpt");
			}
			return cpt.CptNum;
		}

		///<summary>Updates one Cpt in the database.</summary>
		public static void Update(Cpt cpt) {
			string command="UPDATE cpt SET "
				+"CptCode    = '"+POut.String(cpt.CptCode)+"', "
				+"Description= '"+POut.String(cpt.Description)+"', "
				+"VersionIDs = '"+POut.String(cpt.VersionIDs)+"' "
				+"WHERE CptNum = "+POut.Long(cpt.CptNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Cpt in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Cpt cpt,Cpt oldCpt) {
			string command="";
			if(cpt.CptCode != oldCpt.CptCode) {
				if(command!="") { command+=",";}
				command+="CptCode = '"+POut.String(cpt.CptCode)+"'";
			}
			if(cpt.Description != oldCpt.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(cpt.Description)+"'";
			}
			if(cpt.VersionIDs != oldCpt.VersionIDs) {
				if(command!="") { command+=",";}
				command+="VersionIDs = '"+POut.String(cpt.VersionIDs)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE cpt SET "+command
				+" WHERE CptNum = "+POut.Long(cpt.CptNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Cpt,Cpt) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Cpt cpt,Cpt oldCpt) {
			if(cpt.CptCode != oldCpt.CptCode) {
				return true;
			}
			if(cpt.Description != oldCpt.Description) {
				return true;
			}
			if(cpt.VersionIDs != oldCpt.VersionIDs) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Cpt from the database.</summary>
		public static void Delete(long cptNum) {
			string command="DELETE FROM cpt "
				+"WHERE CptNum = "+POut.Long(cptNum);
			Db.NonQ(command);
		}

	}
}