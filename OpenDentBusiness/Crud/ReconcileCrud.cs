//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ReconcileCrud {
		///<summary>Gets one Reconcile object from the database using the primary key.  Returns null if not found.</summary>
		public static Reconcile SelectOne(long reconcileNum) {
			string command="SELECT * FROM reconcile "
				+"WHERE ReconcileNum = "+POut.Long(reconcileNum);
			List<Reconcile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Reconcile object from the database using a query.</summary>
		public static Reconcile SelectOne(string command) {
			
			List<Reconcile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Reconcile objects from the database using a query.</summary>
		public static List<Reconcile> SelectMany(string command) {
			
			List<Reconcile> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Reconcile> TableToList(DataTable table) {
			List<Reconcile> retVal=new List<Reconcile>();
			Reconcile reconcile;
			foreach(DataRow row in table.Rows) {
				reconcile=new Reconcile();
				reconcile.ReconcileNum = PIn.Long  (row["ReconcileNum"].ToString());
				reconcile.AccountNum   = PIn.Long  (row["AccountNum"].ToString());
				reconcile.StartingBal  = PIn.Double(row["StartingBal"].ToString());
				reconcile.EndingBal    = PIn.Double(row["EndingBal"].ToString());
				reconcile.DateReconcile= PIn.Date  (row["DateReconcile"].ToString());
				reconcile.IsLocked     = PIn.Bool  (row["IsLocked"].ToString());
				retVal.Add(reconcile);
			}
			return retVal;
		}

		///<summary>Converts a list of Reconcile into a DataTable.</summary>
		public static DataTable ListToTable(List<Reconcile> listReconciles,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Reconcile";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ReconcileNum");
			table.Columns.Add("AccountNum");
			table.Columns.Add("StartingBal");
			table.Columns.Add("EndingBal");
			table.Columns.Add("DateReconcile");
			table.Columns.Add("IsLocked");
			foreach(Reconcile reconcile in listReconciles) {
				table.Rows.Add(new object[] {
					POut.Long  (reconcile.ReconcileNum),
					POut.Long  (reconcile.AccountNum),
					POut.Double(reconcile.StartingBal),
					POut.Double(reconcile.EndingBal),
					POut.DateT (reconcile.DateReconcile,false),
					POut.Bool  (reconcile.IsLocked),
				});
			}
			return table;
		}

		///<summary>Inserts one Reconcile into the database.  Returns the new priKey.</summary>
		public static long Insert(Reconcile reconcile) {
			return Insert(reconcile,false);
		}

		///<summary>Inserts one Reconcile into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Reconcile reconcile,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				reconcile.ReconcileNum=ReplicationServers.GetKey("reconcile","ReconcileNum");
			}
			string command="INSERT INTO reconcile (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ReconcileNum,";
			}
			command+="AccountNum,StartingBal,EndingBal,DateReconcile,IsLocked) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(reconcile.ReconcileNum)+",";
			}
			command+=
				     POut.Long  (reconcile.AccountNum)+","
				+"'"+POut.Double(reconcile.StartingBal)+"',"
				+"'"+POut.Double(reconcile.EndingBal)+"',"
				+    POut.Date  (reconcile.DateReconcile)+","
				+    POut.Bool  (reconcile.IsLocked)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				reconcile.ReconcileNum=Db.NonQ(command,true,"ReconcileNum","reconcile");
			}
			return reconcile.ReconcileNum;
		}

		///<summary>Inserts one Reconcile into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Reconcile reconcile) {
			return InsertNoCache(reconcile,false);
		}

		///<summary>Inserts one Reconcile into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Reconcile reconcile,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO reconcile (";
			if(!useExistingPK && isRandomKeys) {
				reconcile.ReconcileNum=ReplicationServers.GetKeyNoCache("reconcile","ReconcileNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ReconcileNum,";
			}
			command+="AccountNum,StartingBal,EndingBal,DateReconcile,IsLocked) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(reconcile.ReconcileNum)+",";
			}
			command+=
				     POut.Long  (reconcile.AccountNum)+","
				+"'"+POut.Double(reconcile.StartingBal)+"',"
				+"'"+POut.Double(reconcile.EndingBal)+"',"
				+    POut.Date  (reconcile.DateReconcile)+","
				+    POut.Bool  (reconcile.IsLocked)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				reconcile.ReconcileNum=Db.NonQ(command,true,"ReconcileNum","reconcile");
			}
			return reconcile.ReconcileNum;
		}

		///<summary>Updates one Reconcile in the database.</summary>
		public static void Update(Reconcile reconcile) {
			string command="UPDATE reconcile SET "
				+"AccountNum   =  "+POut.Long  (reconcile.AccountNum)+", "
				+"StartingBal  = '"+POut.Double(reconcile.StartingBal)+"', "
				+"EndingBal    = '"+POut.Double(reconcile.EndingBal)+"', "
				+"DateReconcile=  "+POut.Date  (reconcile.DateReconcile)+", "
				+"IsLocked     =  "+POut.Bool  (reconcile.IsLocked)+" "
				+"WHERE ReconcileNum = "+POut.Long(reconcile.ReconcileNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Reconcile in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Reconcile reconcile,Reconcile oldReconcile) {
			string command="";
			if(reconcile.AccountNum != oldReconcile.AccountNum) {
				if(command!="") { command+=",";}
				command+="AccountNum = "+POut.Long(reconcile.AccountNum)+"";
			}
			if(reconcile.StartingBal != oldReconcile.StartingBal) {
				if(command!="") { command+=",";}
				command+="StartingBal = '"+POut.Double(reconcile.StartingBal)+"'";
			}
			if(reconcile.EndingBal != oldReconcile.EndingBal) {
				if(command!="") { command+=",";}
				command+="EndingBal = '"+POut.Double(reconcile.EndingBal)+"'";
			}
			if(reconcile.DateReconcile.Date != oldReconcile.DateReconcile.Date) {
				if(command!="") { command+=",";}
				command+="DateReconcile = "+POut.Date(reconcile.DateReconcile)+"";
			}
			if(reconcile.IsLocked != oldReconcile.IsLocked) {
				if(command!="") { command+=",";}
				command+="IsLocked = "+POut.Bool(reconcile.IsLocked)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE reconcile SET "+command
				+" WHERE ReconcileNum = "+POut.Long(reconcile.ReconcileNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Reconcile,Reconcile) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Reconcile reconcile,Reconcile oldReconcile) {
			if(reconcile.AccountNum != oldReconcile.AccountNum) {
				return true;
			}
			if(reconcile.StartingBal != oldReconcile.StartingBal) {
				return true;
			}
			if(reconcile.EndingBal != oldReconcile.EndingBal) {
				return true;
			}
			if(reconcile.DateReconcile.Date != oldReconcile.DateReconcile.Date) {
				return true;
			}
			if(reconcile.IsLocked != oldReconcile.IsLocked) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Reconcile from the database.</summary>
		public static void Delete(long reconcileNum) {
			string command="DELETE FROM reconcile "
				+"WHERE ReconcileNum = "+POut.Long(reconcileNum);
			Db.NonQ(command);
		}

	}
}