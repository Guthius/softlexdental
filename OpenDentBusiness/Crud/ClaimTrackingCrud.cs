//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ClaimTrackingCrud {
		///<summary>Gets one ClaimTracking object from the database using the primary key.  Returns null if not found.</summary>
		public static ClaimTracking SelectOne(long claimTrackingNum) {
			string command="SELECT * FROM claimtracking "
				+"WHERE ClaimTrackingNum = "+POut.Long(claimTrackingNum);
			List<ClaimTracking> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ClaimTracking object from the database using a query.</summary>
		public static ClaimTracking SelectOne(string command) {
			
			List<ClaimTracking> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ClaimTracking objects from the database using a query.</summary>
		public static List<ClaimTracking> SelectMany(string command) {
			
			List<ClaimTracking> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ClaimTracking> TableToList(DataTable table) {
			List<ClaimTracking> retVal=new List<ClaimTracking>();
			ClaimTracking claimTracking;
			foreach(DataRow row in table.Rows) {
				claimTracking=new ClaimTracking();
				claimTracking.ClaimTrackingNum   = PIn.Long  (row["ClaimTrackingNum"].ToString());
				claimTracking.ClaimNum           = PIn.Long  (row["ClaimNum"].ToString());
				string trackingType=row["TrackingType"].ToString();
				if(trackingType=="") {
					claimTracking.TrackingType     =(ClaimTrackingType)0;
				}
				else try{
					claimTracking.TrackingType     =(ClaimTrackingType)Enum.Parse(typeof(ClaimTrackingType),trackingType);
				}
				catch{
					claimTracking.TrackingType     =(ClaimTrackingType)0;
				}
				claimTracking.UserNum            = PIn.Long  (row["UserNum"].ToString());
				claimTracking.DateTimeEntry      = PIn.DateT (row["DateTimeEntry"].ToString());
				claimTracking.Note               = PIn.String(row["Note"].ToString());
				claimTracking.TrackingDefNum     = PIn.Long  (row["TrackingDefNum"].ToString());
				claimTracking.TrackingErrorDefNum= PIn.Long  (row["TrackingErrorDefNum"].ToString());
				retVal.Add(claimTracking);
			}
			return retVal;
		}

		///<summary>Converts a list of ClaimTracking into a DataTable.</summary>
		public static DataTable ListToTable(List<ClaimTracking> listClaimTrackings,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ClaimTracking";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ClaimTrackingNum");
			table.Columns.Add("ClaimNum");
			table.Columns.Add("TrackingType");
			table.Columns.Add("UserNum");
			table.Columns.Add("DateTimeEntry");
			table.Columns.Add("Note");
			table.Columns.Add("TrackingDefNum");
			table.Columns.Add("TrackingErrorDefNum");
			foreach(ClaimTracking claimTracking in listClaimTrackings) {
				table.Rows.Add(new object[] {
					POut.Long  (claimTracking.ClaimTrackingNum),
					POut.Long  (claimTracking.ClaimNum),
					POut.Int   ((int)claimTracking.TrackingType),
					POut.Long  (claimTracking.UserNum),
					POut.DateT (claimTracking.DateTimeEntry,false),
					            claimTracking.Note,
					POut.Long  (claimTracking.TrackingDefNum),
					POut.Long  (claimTracking.TrackingErrorDefNum),
				});
			}
			return table;
		}

		///<summary>Inserts one ClaimTracking into the database.  Returns the new priKey.</summary>
		public static long Insert(ClaimTracking claimTracking) {
			return Insert(claimTracking,false);
		}

		///<summary>Inserts one ClaimTracking into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ClaimTracking claimTracking,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				claimTracking.ClaimTrackingNum=ReplicationServers.GetKey("claimtracking","ClaimTrackingNum");
			}
			string command="INSERT INTO claimtracking (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClaimTrackingNum,";
			}
			command+="ClaimNum,TrackingType,UserNum,Note,TrackingDefNum,TrackingErrorDefNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(claimTracking.ClaimTrackingNum)+",";
			}
			command+=
				     POut.Long  (claimTracking.ClaimNum)+","
				+"'"+POut.String(claimTracking.TrackingType.ToString())+"',"
				+    POut.Long  (claimTracking.UserNum)+","
				//DateTimeEntry can only be set by MySQL
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Long  (claimTracking.TrackingDefNum)+","
				+    POut.Long  (claimTracking.TrackingErrorDefNum)+")";
			if(claimTracking.Note==null) {
				claimTracking.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(claimTracking.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				claimTracking.ClaimTrackingNum=Db.NonQ(command,true,"ClaimTrackingNum","claimTracking",paramNote);
			}
			return claimTracking.ClaimTrackingNum;
		}

		///<summary>Inserts one ClaimTracking into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClaimTracking claimTracking) {
			return InsertNoCache(claimTracking,false);
		}

		///<summary>Inserts one ClaimTracking into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClaimTracking claimTracking,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO claimtracking (";
			if(!useExistingPK && isRandomKeys) {
				claimTracking.ClaimTrackingNum=ReplicationServers.GetKeyNoCache("claimtracking","ClaimTrackingNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ClaimTrackingNum,";
			}
			command+="ClaimNum,TrackingType,UserNum,Note,TrackingDefNum,TrackingErrorDefNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(claimTracking.ClaimTrackingNum)+",";
			}
			command+=
				     POut.Long  (claimTracking.ClaimNum)+","
				+"'"+POut.String(claimTracking.TrackingType.ToString())+"',"
				+    POut.Long  (claimTracking.UserNum)+","
				//DateTimeEntry can only be set by MySQL
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Long  (claimTracking.TrackingDefNum)+","
				+    POut.Long  (claimTracking.TrackingErrorDefNum)+")";
			if(claimTracking.Note==null) {
				claimTracking.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(claimTracking.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				claimTracking.ClaimTrackingNum=Db.NonQ(command,true,"ClaimTrackingNum","claimTracking",paramNote);
			}
			return claimTracking.ClaimTrackingNum;
		}

		///<summary>Updates one ClaimTracking in the database.</summary>
		public static void Update(ClaimTracking claimTracking) {
			string command="UPDATE claimtracking SET "
				+"ClaimNum           =  "+POut.Long  (claimTracking.ClaimNum)+", "
				+"TrackingType       = '"+POut.String(claimTracking.TrackingType.ToString())+"', "
				+"UserNum            =  "+POut.Long  (claimTracking.UserNum)+", "
				//DateTimeEntry can only be set by MySQL
				+"Note               =  "+DbHelper.ParamChar+"paramNote, "
				+"TrackingDefNum     =  "+POut.Long  (claimTracking.TrackingDefNum)+", "
				+"TrackingErrorDefNum=  "+POut.Long  (claimTracking.TrackingErrorDefNum)+" "
				+"WHERE ClaimTrackingNum = "+POut.Long(claimTracking.ClaimTrackingNum);
			if(claimTracking.Note==null) {
				claimTracking.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(claimTracking.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one ClaimTracking in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ClaimTracking claimTracking,ClaimTracking oldClaimTracking) {
			string command="";
			if(claimTracking.ClaimNum != oldClaimTracking.ClaimNum) {
				if(command!="") { command+=",";}
				command+="ClaimNum = "+POut.Long(claimTracking.ClaimNum)+"";
			}
			if(claimTracking.TrackingType != oldClaimTracking.TrackingType) {
				if(command!="") { command+=",";}
				command+="TrackingType = '"+POut.String(claimTracking.TrackingType.ToString())+"'";
			}
			if(claimTracking.UserNum != oldClaimTracking.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(claimTracking.UserNum)+"";
			}
			//DateTimeEntry can only be set by MySQL
			if(claimTracking.Note != oldClaimTracking.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(claimTracking.TrackingDefNum != oldClaimTracking.TrackingDefNum) {
				if(command!="") { command+=",";}
				command+="TrackingDefNum = "+POut.Long(claimTracking.TrackingDefNum)+"";
			}
			if(claimTracking.TrackingErrorDefNum != oldClaimTracking.TrackingErrorDefNum) {
				if(command!="") { command+=",";}
				command+="TrackingErrorDefNum = "+POut.Long(claimTracking.TrackingErrorDefNum)+"";
			}
			if(command=="") {
				return false;
			}
			if(claimTracking.Note==null) {
				claimTracking.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(claimTracking.Note));
			command="UPDATE claimtracking SET "+command
				+" WHERE ClaimTrackingNum = "+POut.Long(claimTracking.ClaimTrackingNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(ClaimTracking,ClaimTracking) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ClaimTracking claimTracking,ClaimTracking oldClaimTracking) {
			if(claimTracking.ClaimNum != oldClaimTracking.ClaimNum) {
				return true;
			}
			if(claimTracking.TrackingType != oldClaimTracking.TrackingType) {
				return true;
			}
			if(claimTracking.UserNum != oldClaimTracking.UserNum) {
				return true;
			}
			//DateTimeEntry can only be set by MySQL
			if(claimTracking.Note != oldClaimTracking.Note) {
				return true;
			}
			if(claimTracking.TrackingDefNum != oldClaimTracking.TrackingDefNum) {
				return true;
			}
			if(claimTracking.TrackingErrorDefNum != oldClaimTracking.TrackingErrorDefNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ClaimTracking from the database.</summary>
		public static void Delete(long claimTrackingNum) {
			string command="DELETE FROM claimtracking "
				+"WHERE ClaimTrackingNum = "+POut.Long(claimTrackingNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ClaimTracking> listNew,List<ClaimTracking> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ClaimTracking> listIns    =new List<ClaimTracking>();
			List<ClaimTracking> listUpdNew =new List<ClaimTracking>();
			List<ClaimTracking> listUpdDB  =new List<ClaimTracking>();
			List<ClaimTracking> listDel    =new List<ClaimTracking>();
			listNew.Sort((ClaimTracking x,ClaimTracking y) => { return x.ClaimTrackingNum.CompareTo(y.ClaimTrackingNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ClaimTracking x,ClaimTracking y) => { return x.ClaimTrackingNum.CompareTo(y.ClaimTrackingNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ClaimTracking fieldNew;
			ClaimTracking fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.ClaimTrackingNum<fieldDB.ClaimTrackingNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ClaimTrackingNum>fieldDB.ClaimTrackingNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].ClaimTrackingNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}