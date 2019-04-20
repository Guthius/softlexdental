//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobPermissionCrud {
		///<summary>Gets one JobPermission object from the database using the primary key.  Returns null if not found.</summary>
		public static JobPermission SelectOne(long jobPermissionNum) {
			string command="SELECT * FROM jobpermission "
				+"WHERE JobPermissionNum = "+POut.Long(jobPermissionNum);
			List<JobPermission> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobPermission object from the database using a query.</summary>
		public static JobPermission SelectOne(string command) {
			
			List<JobPermission> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobPermission objects from the database using a query.</summary>
		public static List<JobPermission> SelectMany(string command) {
			
			List<JobPermission> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobPermission> TableToList(DataTable table) {
			List<JobPermission> retVal=new List<JobPermission>();
			JobPermission jobPermission;
			foreach(DataRow row in table.Rows) {
				jobPermission=new JobPermission();
				jobPermission.JobPermissionNum= PIn.Long  (row["JobPermissionNum"].ToString());
				jobPermission.UserNum         = PIn.Long  (row["UserNum"].ToString());
				string jobPermType=row["JobPermType"].ToString();
				if(jobPermType=="") {
					jobPermission.JobPermType   =(JobPerm)0;
				}
				else try{
					jobPermission.JobPermType   =(JobPerm)Enum.Parse(typeof(JobPerm),jobPermType);
				}
				catch{
					jobPermission.JobPermType   =(JobPerm)0;
				}
				retVal.Add(jobPermission);
			}
			return retVal;
		}

		///<summary>Converts a list of JobPermission into a DataTable.</summary>
		public static DataTable ListToTable(List<JobPermission> listJobPermissions,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="JobPermission";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("JobPermissionNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("JobPermType");
			foreach(JobPermission jobPermission in listJobPermissions) {
				table.Rows.Add(new object[] {
					POut.Long  (jobPermission.JobPermissionNum),
					POut.Long  (jobPermission.UserNum),
					POut.Int   ((int)jobPermission.JobPermType),
				});
			}
			return table;
		}

		///<summary>Inserts one JobPermission into the database.  Returns the new priKey.</summary>
		public static long Insert(JobPermission jobPermission) {
			return Insert(jobPermission,false);
		}

		///<summary>Inserts one JobPermission into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobPermission jobPermission,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				jobPermission.JobPermissionNum=ReplicationServers.GetKey("jobpermission","JobPermissionNum");
			}
			string command="INSERT INTO jobpermission (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobPermissionNum,";
			}
			command+="UserNum,JobPermType) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobPermission.JobPermissionNum)+",";
			}
			command+=
				     POut.Long  (jobPermission.UserNum)+","
				+"'"+POut.String(jobPermission.JobPermType.ToString())+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobPermission.JobPermissionNum=Db.NonQ(command,true,"JobPermissionNum","jobPermission");
			}
			return jobPermission.JobPermissionNum;
		}

		///<summary>Inserts one JobPermission into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobPermission jobPermission) {
			return InsertNoCache(jobPermission,false);
		}

		///<summary>Inserts one JobPermission into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobPermission jobPermission,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO jobpermission (";
			if(!useExistingPK && isRandomKeys) {
				jobPermission.JobPermissionNum=ReplicationServers.GetKeyNoCache("jobpermission","JobPermissionNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobPermissionNum,";
			}
			command+="UserNum,JobPermType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobPermission.JobPermissionNum)+",";
			}
			command+=
				     POut.Long  (jobPermission.UserNum)+","
				+"'"+POut.String(jobPermission.JobPermType.ToString())+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobPermission.JobPermissionNum=Db.NonQ(command,true,"JobPermissionNum","jobPermission");
			}
			return jobPermission.JobPermissionNum;
		}

		///<summary>Updates one JobPermission in the database.</summary>
		public static void Update(JobPermission jobPermission) {
			string command="UPDATE jobpermission SET "
				+"UserNum         =  "+POut.Long  (jobPermission.UserNum)+", "
				+"JobPermType     = '"+POut.String(jobPermission.JobPermType.ToString())+"' "
				+"WHERE JobPermissionNum = "+POut.Long(jobPermission.JobPermissionNum);
			Db.NonQ(command);
		}

		///<summary>Updates one JobPermission in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobPermission jobPermission,JobPermission oldJobPermission) {
			string command="";
			if(jobPermission.UserNum != oldJobPermission.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(jobPermission.UserNum)+"";
			}
			if(jobPermission.JobPermType != oldJobPermission.JobPermType) {
				if(command!="") { command+=",";}
				command+="JobPermType = '"+POut.String(jobPermission.JobPermType.ToString())+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE jobpermission SET "+command
				+" WHERE JobPermissionNum = "+POut.Long(jobPermission.JobPermissionNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(JobPermission,JobPermission) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(JobPermission jobPermission,JobPermission oldJobPermission) {
			if(jobPermission.UserNum != oldJobPermission.UserNum) {
				return true;
			}
			if(jobPermission.JobPermType != oldJobPermission.JobPermType) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one JobPermission from the database.</summary>
		public static void Delete(long jobPermissionNum) {
			string command="DELETE FROM jobpermission "
				+"WHERE JobPermissionNum = "+POut.Long(jobPermissionNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobPermission> listNew,List<JobPermission> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<JobPermission> listIns    =new List<JobPermission>();
			List<JobPermission> listUpdNew =new List<JobPermission>();
			List<JobPermission> listUpdDB  =new List<JobPermission>();
			List<JobPermission> listDel    =new List<JobPermission>();
			listNew.Sort((JobPermission x,JobPermission y) => { return x.JobPermissionNum.CompareTo(y.JobPermissionNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((JobPermission x,JobPermission y) => { return x.JobPermissionNum.CompareTo(y.JobPermissionNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			JobPermission fieldNew;
			JobPermission fieldDB;
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
				else if(fieldNew.JobPermissionNum<fieldDB.JobPermissionNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.JobPermissionNum>fieldDB.JobPermissionNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].JobPermissionNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}