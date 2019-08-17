//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatientLinkCrud {
		///<summary>Gets one PatientLink object from the database using the primary key.  Returns null if not found.</summary>
		public static PatientLink SelectOne(long patientLinkNum) {
			string command="SELECT * FROM patientlink "
				+"WHERE PatientLinkNum = "+POut.Long(patientLinkNum);
			List<PatientLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatientLink object from the database using a query.</summary>
		public static PatientLink SelectOne(string command) {
			
			List<PatientLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatientLink objects from the database using a query.</summary>
		public static List<PatientLink> SelectMany(string command) {
			
			List<PatientLink> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatientLink> TableToList(DataTable table) {
			List<PatientLink> retVal=new List<PatientLink>();
			PatientLink patientLink;
			foreach(DataRow row in table.Rows) {
				patientLink=new PatientLink();
				patientLink.PatientLinkNum= PIn.Long  (row["PatientLinkNum"].ToString());
				patientLink.PatNumFrom    = PIn.Long  (row["PatNumFrom"].ToString());
				patientLink.PatNumTo      = PIn.Long  (row["PatNumTo"].ToString());
				patientLink.LinkType      = (OpenDentBusiness.PatientLinkType)PIn.Int(row["LinkType"].ToString());
				patientLink.DateTimeLink  = PIn.DateT (row["DateTimeLink"].ToString());
				retVal.Add(patientLink);
			}
			return retVal;
		}

		///<summary>Converts a list of PatientLink into a DataTable.</summary>
		public static DataTable ListToTable(List<PatientLink> listPatientLinks,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PatientLink";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PatientLinkNum");
			table.Columns.Add("PatNumFrom");
			table.Columns.Add("PatNumTo");
			table.Columns.Add("LinkType");
			table.Columns.Add("DateTimeLink");
			foreach(PatientLink patientLink in listPatientLinks) {
				table.Rows.Add(new object[] {
					POut.Long  (patientLink.PatientLinkNum),
					POut.Long  (patientLink.PatNumFrom),
					POut.Long  (patientLink.PatNumTo),
					POut.Int   ((int)patientLink.LinkType),
					POut.DateT (patientLink.DateTimeLink,false),
				});
			}
			return table;
		}

		///<summary>Inserts one PatientLink into the database.  Returns the new priKey.</summary>
		public static long Insert(PatientLink patientLink) {
			return Insert(patientLink,false);
		}

		///<summary>Inserts one PatientLink into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatientLink patientLink,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				patientLink.PatientLinkNum=ReplicationServers.GetKey("patientlink","PatientLinkNum");
			}
			string command="INSERT INTO patientlink (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="PatientLinkNum,";
			}
			command+="PatNumFrom,PatNumTo,LinkType,DateTimeLink) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(patientLink.PatientLinkNum)+",";
			}
			command+=
				     POut.Long  (patientLink.PatNumFrom)+","
				+    POut.Long  (patientLink.PatNumTo)+","
				+    POut.Int   ((int)patientLink.LinkType)+","
				+    DbHelper.Now()+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patientLink.PatientLinkNum=Db.NonQ(command,true,"PatientLinkNum","patientLink");
			}
			return patientLink.PatientLinkNum;
		}

		///<summary>Inserts one PatientLink into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientLink patientLink) {
			return InsertNoCache(patientLink,false);
		}

		///<summary>Inserts one PatientLink into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientLink patientLink,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO patientlink (";
			if(!useExistingPK && isRandomKeys) {
				patientLink.PatientLinkNum=ReplicationServers.GetKeyNoCache("patientlink","PatientLinkNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatientLinkNum,";
			}
			command+="PatNumFrom,PatNumTo,LinkType,DateTimeLink) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patientLink.PatientLinkNum)+",";
			}
			command+=
				     POut.Long  (patientLink.PatNumFrom)+","
				+    POut.Long  (patientLink.PatNumTo)+","
				+    POut.Int   ((int)patientLink.LinkType)+","
				+    DbHelper.Now()+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				patientLink.PatientLinkNum=Db.NonQ(command,true,"PatientLinkNum","patientLink");
			}
			return patientLink.PatientLinkNum;
		}

		///<summary>Updates one PatientLink in the database.</summary>
		public static void Update(PatientLink patientLink) {
			string command="UPDATE patientlink SET "
				+"PatNumFrom    =  "+POut.Long  (patientLink.PatNumFrom)+", "
				+"PatNumTo      =  "+POut.Long  (patientLink.PatNumTo)+", "
				+"LinkType      =  "+POut.Int   ((int)patientLink.LinkType)+" "
				//DateTimeLink not allowed to change
				+"WHERE PatientLinkNum = "+POut.Long(patientLink.PatientLinkNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PatientLink in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatientLink patientLink,PatientLink oldPatientLink) {
			string command="";
			if(patientLink.PatNumFrom != oldPatientLink.PatNumFrom) {
				if(command!="") { command+=",";}
				command+="PatNumFrom = "+POut.Long(patientLink.PatNumFrom)+"";
			}
			if(patientLink.PatNumTo != oldPatientLink.PatNumTo) {
				if(command!="") { command+=",";}
				command+="PatNumTo = "+POut.Long(patientLink.PatNumTo)+"";
			}
			if(patientLink.LinkType != oldPatientLink.LinkType) {
				if(command!="") { command+=",";}
				command+="LinkType = "+POut.Int   ((int)patientLink.LinkType)+"";
			}
			//DateTimeLink not allowed to change
			if(command=="") {
				return false;
			}
			command="UPDATE patientlink SET "+command
				+" WHERE PatientLinkNum = "+POut.Long(patientLink.PatientLinkNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(PatientLink,PatientLink) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PatientLink patientLink,PatientLink oldPatientLink) {
			if(patientLink.PatNumFrom != oldPatientLink.PatNumFrom) {
				return true;
			}
			if(patientLink.PatNumTo != oldPatientLink.PatNumTo) {
				return true;
			}
			if(patientLink.LinkType != oldPatientLink.LinkType) {
				return true;
			}
			//DateTimeLink not allowed to change
			return false;
		}

		///<summary>Deletes one PatientLink from the database.</summary>
		public static void Delete(long patientLinkNum) {
			string command="DELETE FROM patientlink "
				+"WHERE PatientLinkNum = "+POut.Long(patientLinkNum);
			Db.NonQ(command);
		}

	}
}