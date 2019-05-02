//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CustReferenceCrud {
		///<summary>Gets one CustReference object from the database using the primary key.  Returns null if not found.</summary>
		public static CustReference SelectOne(long custReferenceNum) {
			string command="SELECT * FROM custreference "
				+"WHERE CustReferenceNum = "+POut.Long(custReferenceNum);
			List<CustReference> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one CustReference object from the database using a query.</summary>
		public static CustReference SelectOne(string command) {
			
			List<CustReference> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of CustReference objects from the database using a query.</summary>
		public static List<CustReference> SelectMany(string command) {
			
			List<CustReference> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<CustReference> TableToList(DataTable table) {
			List<CustReference> retVal=new List<CustReference>();
			CustReference custReference;
			foreach(DataRow row in table.Rows) {
				custReference=new CustReference();
				custReference.CustReferenceNum= PIn.Long  (row["CustReferenceNum"].ToString());
				custReference.PatNum          = PIn.Long  (row["PatNum"].ToString());
				custReference.DateMostRecent  = PIn.Date  (row["DateMostRecent"].ToString());
				custReference.Note            = PIn.String(row["Note"].ToString());
				custReference.IsBadRef        = PIn.Bool  (row["IsBadRef"].ToString());
				retVal.Add(custReference);
			}
			return retVal;
		}

		///<summary>Converts a list of CustReference into a DataTable.</summary>
		public static DataTable ListToTable(List<CustReference> listCustReferences,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="CustReference";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CustReferenceNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("DateMostRecent");
			table.Columns.Add("Note");
			table.Columns.Add("IsBadRef");
			foreach(CustReference custReference in listCustReferences) {
				table.Rows.Add(new object[] {
					POut.Long  (custReference.CustReferenceNum),
					POut.Long  (custReference.PatNum),
					POut.DateT (custReference.DateMostRecent,false),
					            custReference.Note,
					POut.Bool  (custReference.IsBadRef),
				});
			}
			return table;
		}

		///<summary>Inserts one CustReference into the database.  Returns the new priKey.</summary>
		public static long Insert(CustReference custReference) {
			return Insert(custReference,false);
		}

		///<summary>Inserts one CustReference into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(CustReference custReference,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				custReference.CustReferenceNum=ReplicationServers.GetKey("custreference","CustReferenceNum");
			}
			string command="INSERT INTO custreference (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="CustReferenceNum,";
			}
			command+="PatNum,DateMostRecent,Note,IsBadRef) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(custReference.CustReferenceNum)+",";
			}
			command+=
				     POut.Long  (custReference.PatNum)+","
				+    POut.Date  (custReference.DateMostRecent)+","
				+"'"+POut.String(custReference.Note)+"',"
				+    POut.Bool  (custReference.IsBadRef)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				custReference.CustReferenceNum=Db.NonQ(command,true,"CustReferenceNum","custReference");
			}
			return custReference.CustReferenceNum;
		}

		///<summary>Inserts one CustReference into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CustReference custReference) {
			return InsertNoCache(custReference,false);
		}

		///<summary>Inserts one CustReference into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CustReference custReference,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO custreference (";
			if(!useExistingPK && isRandomKeys) {
				custReference.CustReferenceNum=ReplicationServers.GetKeyNoCache("custreference","CustReferenceNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CustReferenceNum,";
			}
			command+="PatNum,DateMostRecent,Note,IsBadRef) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(custReference.CustReferenceNum)+",";
			}
			command+=
				     POut.Long  (custReference.PatNum)+","
				+    POut.Date  (custReference.DateMostRecent)+","
				+"'"+POut.String(custReference.Note)+"',"
				+    POut.Bool  (custReference.IsBadRef)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				custReference.CustReferenceNum=Db.NonQ(command,true,"CustReferenceNum","custReference");
			}
			return custReference.CustReferenceNum;
		}

		///<summary>Updates one CustReference in the database.</summary>
		public static void Update(CustReference custReference) {
			string command="UPDATE custreference SET "
				+"PatNum          =  "+POut.Long  (custReference.PatNum)+", "
				+"DateMostRecent  =  "+POut.Date  (custReference.DateMostRecent)+", "
				+"Note            = '"+POut.String(custReference.Note)+"', "
				+"IsBadRef        =  "+POut.Bool  (custReference.IsBadRef)+" "
				+"WHERE CustReferenceNum = "+POut.Long(custReference.CustReferenceNum);
			Db.NonQ(command);
		}

		///<summary>Updates one CustReference in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(CustReference custReference,CustReference oldCustReference) {
			string command="";
			if(custReference.PatNum != oldCustReference.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(custReference.PatNum)+"";
			}
			if(custReference.DateMostRecent.Date != oldCustReference.DateMostRecent.Date) {
				if(command!="") { command+=",";}
				command+="DateMostRecent = "+POut.Date(custReference.DateMostRecent)+"";
			}
			if(custReference.Note != oldCustReference.Note) {
				if(command!="") { command+=",";}
				command+="Note = '"+POut.String(custReference.Note)+"'";
			}
			if(custReference.IsBadRef != oldCustReference.IsBadRef) {
				if(command!="") { command+=",";}
				command+="IsBadRef = "+POut.Bool(custReference.IsBadRef)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE custreference SET "+command
				+" WHERE CustReferenceNum = "+POut.Long(custReference.CustReferenceNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(CustReference,CustReference) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(CustReference custReference,CustReference oldCustReference) {
			if(custReference.PatNum != oldCustReference.PatNum) {
				return true;
			}
			if(custReference.DateMostRecent.Date != oldCustReference.DateMostRecent.Date) {
				return true;
			}
			if(custReference.Note != oldCustReference.Note) {
				return true;
			}
			if(custReference.IsBadRef != oldCustReference.IsBadRef) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one CustReference from the database.</summary>
		public static void Delete(long custReferenceNum) {
			string command="DELETE FROM custreference "
				+"WHERE CustReferenceNum = "+POut.Long(custReferenceNum);
			Db.NonQ(command);
		}

	}
}