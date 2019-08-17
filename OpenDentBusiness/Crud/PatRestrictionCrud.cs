//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatRestrictionCrud {
		///<summary>Gets one PatRestriction object from the database using the primary key.  Returns null if not found.</summary>
		public static PatRestriction SelectOne(long patRestrictionNum) {
			string command="SELECT * FROM patrestriction "
				+"WHERE PatRestrictionNum = "+POut.Long(patRestrictionNum);
			List<PatRestriction> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatRestriction object from the database using a query.</summary>
		public static PatRestriction SelectOne(string command) {
			
			List<PatRestriction> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatRestriction objects from the database using a query.</summary>
		public static List<PatRestriction> SelectMany(string command) {
			
			List<PatRestriction> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatRestriction> TableToList(DataTable table) {
			List<PatRestriction> retVal=new List<PatRestriction>();
			PatRestriction patRestriction;
			foreach(DataRow row in table.Rows) {
				patRestriction=new PatRestriction();
				patRestriction.PatRestrictionNum= PIn.Long  (row["PatRestrictionNum"].ToString());
				patRestriction.PatNum           = PIn.Long  (row["PatNum"].ToString());
				patRestriction.PatRestrictType  = (OpenDentBusiness.PatRestrict)PIn.Int(row["PatRestrictType"].ToString());
				retVal.Add(patRestriction);
			}
			return retVal;
		}

		///<summary>Converts a list of PatRestriction into a DataTable.</summary>
		public static DataTable ListToTable(List<PatRestriction> listPatRestrictions,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PatRestriction";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PatRestrictionNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("PatRestrictType");
			foreach(PatRestriction patRestriction in listPatRestrictions) {
				table.Rows.Add(new object[] {
					POut.Long  (patRestriction.PatRestrictionNum),
					POut.Long  (patRestriction.PatNum),
					POut.Int   ((int)patRestriction.PatRestrictType),
				});
			}
			return table;
		}

		///<summary>Inserts one PatRestriction into the database.  Returns the new priKey.</summary>
		public static long Insert(PatRestriction patRestriction) {
			return Insert(patRestriction,false);
		}

		///<summary>Inserts one PatRestriction into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatRestriction patRestriction,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				patRestriction.PatRestrictionNum=ReplicationServers.GetKey("patrestriction","PatRestrictionNum");
			}
			string command="INSERT INTO patrestriction (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="PatRestrictionNum,";
			}
			command+="PatNum,PatRestrictType) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(patRestriction.PatRestrictionNum)+",";
			}
			command+=
				     POut.Long  (patRestriction.PatNum)+","
				+    POut.Int   ((int)patRestriction.PatRestrictType)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patRestriction.PatRestrictionNum=Db.NonQ(command,true,"PatRestrictionNum","patRestriction");
			}
			return patRestriction.PatRestrictionNum;
		}

		///<summary>Inserts one PatRestriction into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatRestriction patRestriction) {
			return InsertNoCache(patRestriction,false);
		}

		///<summary>Inserts one PatRestriction into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatRestriction patRestriction,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO patrestriction (";
			if(!useExistingPK && isRandomKeys) {
				patRestriction.PatRestrictionNum=ReplicationServers.GetKeyNoCache("patrestriction","PatRestrictionNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatRestrictionNum,";
			}
			command+="PatNum,PatRestrictType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patRestriction.PatRestrictionNum)+",";
			}
			command+=
				     POut.Long  (patRestriction.PatNum)+","
				+    POut.Int   ((int)patRestriction.PatRestrictType)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				patRestriction.PatRestrictionNum=Db.NonQ(command,true,"PatRestrictionNum","patRestriction");
			}
			return patRestriction.PatRestrictionNum;
		}

		///<summary>Updates one PatRestriction in the database.</summary>
		public static void Update(PatRestriction patRestriction) {
			string command="UPDATE patrestriction SET "
				+"PatNum           =  "+POut.Long  (patRestriction.PatNum)+", "
				+"PatRestrictType  =  "+POut.Int   ((int)patRestriction.PatRestrictType)+" "
				+"WHERE PatRestrictionNum = "+POut.Long(patRestriction.PatRestrictionNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PatRestriction in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatRestriction patRestriction,PatRestriction oldPatRestriction) {
			string command="";
			if(patRestriction.PatNum != oldPatRestriction.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(patRestriction.PatNum)+"";
			}
			if(patRestriction.PatRestrictType != oldPatRestriction.PatRestrictType) {
				if(command!="") { command+=",";}
				command+="PatRestrictType = "+POut.Int   ((int)patRestriction.PatRestrictType)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE patrestriction SET "+command
				+" WHERE PatRestrictionNum = "+POut.Long(patRestriction.PatRestrictionNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(PatRestriction,PatRestriction) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PatRestriction patRestriction,PatRestriction oldPatRestriction) {
			if(patRestriction.PatNum != oldPatRestriction.PatNum) {
				return true;
			}
			if(patRestriction.PatRestrictType != oldPatRestriction.PatRestrictType) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PatRestriction from the database.</summary>
		public static void Delete(long patRestrictionNum) {
			string command="DELETE FROM patrestriction "
				+"WHERE PatRestrictionNum = "+POut.Long(patRestrictionNum);
			Db.NonQ(command);
		}

	}
}