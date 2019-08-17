//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProviderErxCrud {
		///<summary>Gets one ProviderErx object from the database using the primary key.  Returns null if not found.</summary>
		public static ProviderErx SelectOne(long providerErxNum) {
			string command="SELECT * FROM providererx "
				+"WHERE ProviderErxNum = "+POut.Long(providerErxNum);
			List<ProviderErx> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProviderErx object from the database using a query.</summary>
		public static ProviderErx SelectOne(string command) {
			
			List<ProviderErx> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProviderErx objects from the database using a query.</summary>
		public static List<ProviderErx> SelectMany(string command) {
			
			List<ProviderErx> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProviderErx> TableToList(DataTable table) {
			List<ProviderErx> retVal=new List<ProviderErx>();
			ProviderErx providerErx;
			foreach(DataRow row in table.Rows) {
				providerErx=new ProviderErx();
				providerErx.ProviderErxNum    = PIn.Long  (row["ProviderErxNum"].ToString());
				providerErx.PatNum            = PIn.Long  (row["PatNum"].ToString());
				providerErx.NationalProviderID= PIn.String(row["NationalProviderID"].ToString());
				providerErx.IsEnabled         = (OpenDentBusiness.ErxStatus)PIn.Int(row["IsEnabled"].ToString());
				providerErx.IsIdentifyProofed = PIn.Bool  (row["IsIdentifyProofed"].ToString());
				providerErx.IsSentToHq        = PIn.Bool  (row["IsSentToHq"].ToString());
				providerErx.IsEpcs            = PIn.Bool  (row["IsEpcs"].ToString());
				providerErx.ErxType           = (OpenDentBusiness.ErxOption)PIn.Int(row["ErxType"].ToString());
				providerErx.UserId            = PIn.String(row["UserId"].ToString());
				providerErx.AccountId         = PIn.String(row["AccountId"].ToString());
				providerErx.RegistrationKeyNum= PIn.Long  (row["RegistrationKeyNum"].ToString());
				retVal.Add(providerErx);
			}
			return retVal;
		}

		///<summary>Converts a list of ProviderErx into a DataTable.</summary>
		public static DataTable ListToTable(List<ProviderErx> listProviderErxs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProviderErx";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProviderErxNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("NationalProviderID");
			table.Columns.Add("IsEnabled");
			table.Columns.Add("IsIdentifyProofed");
			table.Columns.Add("IsSentToHq");
			table.Columns.Add("IsEpcs");
			table.Columns.Add("ErxType");
			table.Columns.Add("UserId");
			table.Columns.Add("AccountId");
			table.Columns.Add("RegistrationKeyNum");
			foreach(ProviderErx providerErx in listProviderErxs) {
				table.Rows.Add(new object[] {
					POut.Long  (providerErx.ProviderErxNum),
					POut.Long  (providerErx.PatNum),
					            providerErx.NationalProviderID,
					POut.Int   ((int)providerErx.IsEnabled),
					POut.Bool  (providerErx.IsIdentifyProofed),
					POut.Bool  (providerErx.IsSentToHq),
					POut.Bool  (providerErx.IsEpcs),
					POut.Int   ((int)providerErx.ErxType),
					            providerErx.UserId,
					            providerErx.AccountId,
					POut.Long  (providerErx.RegistrationKeyNum),
				});
			}
			return table;
		}

		///<summary>Inserts one ProviderErx into the database.  Returns the new priKey.</summary>
		public static long Insert(ProviderErx providerErx) {
			return Insert(providerErx,false);
		}

		///<summary>Inserts one ProviderErx into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProviderErx providerErx,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				providerErx.ProviderErxNum=ReplicationServers.GetKey("providererx","ProviderErxNum");
			}
			string command="INSERT INTO providererx (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ProviderErxNum,";
			}
			command+="PatNum,NationalProviderID,IsEnabled,IsIdentifyProofed,IsSentToHq,IsEpcs,ErxType,UserId,AccountId,RegistrationKeyNum) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(providerErx.ProviderErxNum)+",";
			}
			command+=
				     POut.Long  (providerErx.PatNum)+","
				+"'"+POut.String(providerErx.NationalProviderID)+"',"
				+    POut.Int   ((int)providerErx.IsEnabled)+","
				+    POut.Bool  (providerErx.IsIdentifyProofed)+","
				+    POut.Bool  (providerErx.IsSentToHq)+","
				+    POut.Bool  (providerErx.IsEpcs)+","
				+    POut.Int   ((int)providerErx.ErxType)+","
				+"'"+POut.String(providerErx.UserId)+"',"
				+"'"+POut.String(providerErx.AccountId)+"',"
				+    POut.Long  (providerErx.RegistrationKeyNum)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				providerErx.ProviderErxNum=Db.NonQ(command,true,"ProviderErxNum","providerErx");
			}
			return providerErx.ProviderErxNum;
		}

		///<summary>Inserts one ProviderErx into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProviderErx providerErx) {
			return InsertNoCache(providerErx,false);
		}

		///<summary>Inserts one ProviderErx into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProviderErx providerErx,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO providererx (";
			if(!useExistingPK && isRandomKeys) {
				providerErx.ProviderErxNum=ReplicationServers.GetKeyNoCache("providererx","ProviderErxNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProviderErxNum,";
			}
			command+="PatNum,NationalProviderID,IsEnabled,IsIdentifyProofed,IsSentToHq,IsEpcs,ErxType,UserId,AccountId,RegistrationKeyNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(providerErx.ProviderErxNum)+",";
			}
			command+=
				     POut.Long  (providerErx.PatNum)+","
				+"'"+POut.String(providerErx.NationalProviderID)+"',"
				+    POut.Int   ((int)providerErx.IsEnabled)+","
				+    POut.Bool  (providerErx.IsIdentifyProofed)+","
				+    POut.Bool  (providerErx.IsSentToHq)+","
				+    POut.Bool  (providerErx.IsEpcs)+","
				+    POut.Int   ((int)providerErx.ErxType)+","
				+"'"+POut.String(providerErx.UserId)+"',"
				+"'"+POut.String(providerErx.AccountId)+"',"
				+    POut.Long  (providerErx.RegistrationKeyNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				providerErx.ProviderErxNum=Db.NonQ(command,true,"ProviderErxNum","providerErx");
			}
			return providerErx.ProviderErxNum;
		}

		///<summary>Updates one ProviderErx in the database.</summary>
		public static void Update(ProviderErx providerErx) {
			string command="UPDATE providererx SET "
				+"PatNum            =  "+POut.Long  (providerErx.PatNum)+", "
				+"NationalProviderID= '"+POut.String(providerErx.NationalProviderID)+"', "
				+"IsEnabled         =  "+POut.Int   ((int)providerErx.IsEnabled)+", "
				+"IsIdentifyProofed =  "+POut.Bool  (providerErx.IsIdentifyProofed)+", "
				+"IsSentToHq        =  "+POut.Bool  (providerErx.IsSentToHq)+", "
				+"IsEpcs            =  "+POut.Bool  (providerErx.IsEpcs)+", "
				+"ErxType           =  "+POut.Int   ((int)providerErx.ErxType)+", "
				+"UserId            = '"+POut.String(providerErx.UserId)+"', "
				+"AccountId         = '"+POut.String(providerErx.AccountId)+"', "
				+"RegistrationKeyNum=  "+POut.Long  (providerErx.RegistrationKeyNum)+" "
				+"WHERE ProviderErxNum = "+POut.Long(providerErx.ProviderErxNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProviderErx in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProviderErx providerErx,ProviderErx oldProviderErx) {
			string command="";
			if(providerErx.PatNum != oldProviderErx.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(providerErx.PatNum)+"";
			}
			if(providerErx.NationalProviderID != oldProviderErx.NationalProviderID) {
				if(command!="") { command+=",";}
				command+="NationalProviderID = '"+POut.String(providerErx.NationalProviderID)+"'";
			}
			if(providerErx.IsEnabled != oldProviderErx.IsEnabled) {
				if(command!="") { command+=",";}
				command+="IsEnabled = "+POut.Int   ((int)providerErx.IsEnabled)+"";
			}
			if(providerErx.IsIdentifyProofed != oldProviderErx.IsIdentifyProofed) {
				if(command!="") { command+=",";}
				command+="IsIdentifyProofed = "+POut.Bool(providerErx.IsIdentifyProofed)+"";
			}
			if(providerErx.IsSentToHq != oldProviderErx.IsSentToHq) {
				if(command!="") { command+=",";}
				command+="IsSentToHq = "+POut.Bool(providerErx.IsSentToHq)+"";
			}
			if(providerErx.IsEpcs != oldProviderErx.IsEpcs) {
				if(command!="") { command+=",";}
				command+="IsEpcs = "+POut.Bool(providerErx.IsEpcs)+"";
			}
			if(providerErx.ErxType != oldProviderErx.ErxType) {
				if(command!="") { command+=",";}
				command+="ErxType = "+POut.Int   ((int)providerErx.ErxType)+"";
			}
			if(providerErx.UserId != oldProviderErx.UserId) {
				if(command!="") { command+=",";}
				command+="UserId = '"+POut.String(providerErx.UserId)+"'";
			}
			if(providerErx.AccountId != oldProviderErx.AccountId) {
				if(command!="") { command+=",";}
				command+="AccountId = '"+POut.String(providerErx.AccountId)+"'";
			}
			if(providerErx.RegistrationKeyNum != oldProviderErx.RegistrationKeyNum) {
				if(command!="") { command+=",";}
				command+="RegistrationKeyNum = "+POut.Long(providerErx.RegistrationKeyNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE providererx SET "+command
				+" WHERE ProviderErxNum = "+POut.Long(providerErx.ProviderErxNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ProviderErx,ProviderErx) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProviderErx providerErx,ProviderErx oldProviderErx) {
			if(providerErx.PatNum != oldProviderErx.PatNum) {
				return true;
			}
			if(providerErx.NationalProviderID != oldProviderErx.NationalProviderID) {
				return true;
			}
			if(providerErx.IsEnabled != oldProviderErx.IsEnabled) {
				return true;
			}
			if(providerErx.IsIdentifyProofed != oldProviderErx.IsIdentifyProofed) {
				return true;
			}
			if(providerErx.IsSentToHq != oldProviderErx.IsSentToHq) {
				return true;
			}
			if(providerErx.IsEpcs != oldProviderErx.IsEpcs) {
				return true;
			}
			if(providerErx.ErxType != oldProviderErx.ErxType) {
				return true;
			}
			if(providerErx.UserId != oldProviderErx.UserId) {
				return true;
			}
			if(providerErx.AccountId != oldProviderErx.AccountId) {
				return true;
			}
			if(providerErx.RegistrationKeyNum != oldProviderErx.RegistrationKeyNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProviderErx from the database.</summary>
		public static void Delete(long providerErxNum) {
			string command="DELETE FROM providererx "
				+"WHERE ProviderErxNum = "+POut.Long(providerErxNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ProviderErx> listNew,List<ProviderErx> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ProviderErx> listIns    =new List<ProviderErx>();
			List<ProviderErx> listUpdNew =new List<ProviderErx>();
			List<ProviderErx> listUpdDB  =new List<ProviderErx>();
			List<ProviderErx> listDel    =new List<ProviderErx>();
			listNew.Sort((ProviderErx x,ProviderErx y) => { return x.ProviderErxNum.CompareTo(y.ProviderErxNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ProviderErx x,ProviderErx y) => { return x.ProviderErxNum.CompareTo(y.ProviderErxNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ProviderErx fieldNew;
			ProviderErx fieldDB;
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
				else if(fieldNew.ProviderErxNum<fieldDB.ProviderErxNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ProviderErxNum>fieldDB.ProviderErxNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ProviderErxNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}