//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SopCrud {
		///<summary>Gets one Sop object from the database using the primary key.  Returns null if not found.</summary>
		public static Sop SelectOne(long sopNum) {
			string command="SELECT * FROM sop "
				+"WHERE SopNum = "+POut.Long(sopNum);
			List<Sop> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Sop object from the database using a query.</summary>
		public static Sop SelectOne(string command) {
			
			List<Sop> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Sop objects from the database using a query.</summary>
		public static List<Sop> SelectMany(string command) {
			
			List<Sop> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Sop> TableToList(DataTable table) {
			List<Sop> retVal=new List<Sop>();
			Sop sop;
			foreach(DataRow row in table.Rows) {
				sop=new Sop();
				sop.SopNum     = PIn.Long  (row["SopNum"].ToString());
				sop.SopCode    = PIn.String(row["SopCode"].ToString());
				sop.Description= PIn.String(row["Description"].ToString());
				retVal.Add(sop);
			}
			return retVal;
		}

		///<summary>Converts a list of Sop into a DataTable.</summary>
		public static DataTable ListToTable(List<Sop> listSops,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Sop";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SopNum");
			table.Columns.Add("SopCode");
			table.Columns.Add("Description");
			foreach(Sop sop in listSops) {
				table.Rows.Add(new object[] {
					POut.Long  (sop.SopNum),
					            sop.SopCode,
					            sop.Description,
				});
			}
			return table;
		}

		///<summary>Inserts one Sop into the database.  Returns the new priKey.</summary>
		public static long Insert(Sop sop) {
			return Insert(sop,false);
		}

		///<summary>Inserts one Sop into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Sop sop,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				sop.SopNum=ReplicationServers.GetKey("sop","SopNum");
			}
			string command="INSERT INTO sop (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="SopNum,";
			}
			command+="SopCode,Description) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(sop.SopNum)+",";
			}
			command+=
				 "'"+POut.String(sop.SopCode)+"',"
				+"'"+POut.String(sop.Description)+"')";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sop.SopNum=Db.NonQ(command,true,"SopNum","sop");
			}
			return sop.SopNum;
		}

		///<summary>Inserts one Sop into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Sop sop) {
			return InsertNoCache(sop,false);
		}

		///<summary>Inserts one Sop into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Sop sop,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO sop (";
			if(!useExistingPK && isRandomKeys) {
				sop.SopNum=ReplicationServers.GetKeyNoCache("sop","SopNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SopNum,";
			}
			command+="SopCode,Description) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sop.SopNum)+",";
			}
			command+=
				 "'"+POut.String(sop.SopCode)+"',"
				+"'"+POut.String(sop.Description)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				sop.SopNum=Db.NonQ(command,true,"SopNum","sop");
			}
			return sop.SopNum;
		}

		///<summary>Updates one Sop in the database.</summary>
		public static void Update(Sop sop) {
			string command="UPDATE sop SET "
				+"SopCode    = '"+POut.String(sop.SopCode)+"', "
				+"Description= '"+POut.String(sop.Description)+"' "
				+"WHERE SopNum = "+POut.Long(sop.SopNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Sop in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Sop sop,Sop oldSop) {
			string command="";
			if(sop.SopCode != oldSop.SopCode) {
				if(command!="") { command+=",";}
				command+="SopCode = '"+POut.String(sop.SopCode)+"'";
			}
			if(sop.Description != oldSop.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(sop.Description)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE sop SET "+command
				+" WHERE SopNum = "+POut.Long(sop.SopNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Sop,Sop) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Sop sop,Sop oldSop) {
			if(sop.SopCode != oldSop.SopCode) {
				return true;
			}
			if(sop.Description != oldSop.Description) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Sop from the database.</summary>
		public static void Delete(long sopNum) {
			string command="DELETE FROM sop "
				+"WHERE SopNum = "+POut.Long(sopNum);
			Db.NonQ(command);
		}

	}
}