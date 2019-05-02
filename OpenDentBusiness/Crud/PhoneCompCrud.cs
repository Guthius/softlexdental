//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PhoneCompCrud {
		///<summary>Gets one PhoneComp object from the database using the primary key.  Returns null if not found.</summary>
		public static PhoneComp SelectOne(long phoneCompNum) {
			string command="SELECT * FROM phonecomp "
				+"WHERE PhoneCompNum = "+POut.Long(phoneCompNum);
			List<PhoneComp> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PhoneComp object from the database using a query.</summary>
		public static PhoneComp SelectOne(string command) {
			
			List<PhoneComp> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PhoneComp objects from the database using a query.</summary>
		public static List<PhoneComp> SelectMany(string command) {
			
			List<PhoneComp> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PhoneComp> TableToList(DataTable table) {
			List<PhoneComp> retVal=new List<PhoneComp>();
			PhoneComp phoneComp;
			foreach(DataRow row in table.Rows) {
				phoneComp=new PhoneComp();
				phoneComp.PhoneCompNum= PIn.Long  (row["PhoneCompNum"].ToString());
				phoneComp.PhoneExt    = PIn.Int   (row["PhoneExt"].ToString());
				phoneComp.ComputerName= PIn.String(row["ComputerName"].ToString());
				retVal.Add(phoneComp);
			}
			return retVal;
		}

		///<summary>Converts a list of PhoneComp into a DataTable.</summary>
		public static DataTable ListToTable(List<PhoneComp> listPhoneComps,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PhoneComp";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PhoneCompNum");
			table.Columns.Add("PhoneExt");
			table.Columns.Add("ComputerName");
			foreach(PhoneComp phoneComp in listPhoneComps) {
				table.Rows.Add(new object[] {
					POut.Long  (phoneComp.PhoneCompNum),
					POut.Int   (phoneComp.PhoneExt),
					            phoneComp.ComputerName,
				});
			}
			return table;
		}

		///<summary>Inserts one PhoneComp into the database.  Returns the new priKey.</summary>
		public static long Insert(PhoneComp phoneComp) {
			return Insert(phoneComp,false);
		}

		///<summary>Inserts one PhoneComp into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PhoneComp phoneComp,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				phoneComp.PhoneCompNum=ReplicationServers.GetKey("phonecomp","PhoneCompNum");
			}
			string command="INSERT INTO phonecomp (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="PhoneCompNum,";
			}
			command+="PhoneExt,ComputerName) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(phoneComp.PhoneCompNum)+",";
			}
			command+=
				     POut.Int   (phoneComp.PhoneExt)+","
				+"'"+POut.String(phoneComp.ComputerName)+"')";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneComp.PhoneCompNum=Db.NonQ(command,true,"PhoneCompNum","phoneComp");
			}
			return phoneComp.PhoneCompNum;
		}

		///<summary>Inserts one PhoneComp into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneComp phoneComp) {
			return InsertNoCache(phoneComp,false);
		}

		///<summary>Inserts one PhoneComp into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneComp phoneComp,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO phonecomp (";
			if(!useExistingPK && isRandomKeys) {
				phoneComp.PhoneCompNum=ReplicationServers.GetKeyNoCache("phonecomp","PhoneCompNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PhoneCompNum,";
			}
			command+="PhoneExt,ComputerName) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(phoneComp.PhoneCompNum)+",";
			}
			command+=
				     POut.Int   (phoneComp.PhoneExt)+","
				+"'"+POut.String(phoneComp.ComputerName)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneComp.PhoneCompNum=Db.NonQ(command,true,"PhoneCompNum","phoneComp");
			}
			return phoneComp.PhoneCompNum;
		}

		///<summary>Updates one PhoneComp in the database.</summary>
		public static void Update(PhoneComp phoneComp) {
			string command="UPDATE phonecomp SET "
				+"PhoneExt    =  "+POut.Int   (phoneComp.PhoneExt)+", "
				+"ComputerName= '"+POut.String(phoneComp.ComputerName)+"' "
				+"WHERE PhoneCompNum = "+POut.Long(phoneComp.PhoneCompNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PhoneComp in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PhoneComp phoneComp,PhoneComp oldPhoneComp) {
			string command="";
			if(phoneComp.PhoneExt != oldPhoneComp.PhoneExt) {
				if(command!="") { command+=",";}
				command+="PhoneExt = "+POut.Int(phoneComp.PhoneExt)+"";
			}
			if(phoneComp.ComputerName != oldPhoneComp.ComputerName) {
				if(command!="") { command+=",";}
				command+="ComputerName = '"+POut.String(phoneComp.ComputerName)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE phonecomp SET "+command
				+" WHERE PhoneCompNum = "+POut.Long(phoneComp.PhoneCompNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(PhoneComp,PhoneComp) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PhoneComp phoneComp,PhoneComp oldPhoneComp) {
			if(phoneComp.PhoneExt != oldPhoneComp.PhoneExt) {
				return true;
			}
			if(phoneComp.ComputerName != oldPhoneComp.ComputerName) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PhoneComp from the database.</summary>
		public static void Delete(long phoneCompNum) {
			string command="DELETE FROM phonecomp "
				+"WHERE PhoneCompNum = "+POut.Long(phoneCompNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<PhoneComp> listNew,List<PhoneComp> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<PhoneComp> listIns    =new List<PhoneComp>();
			List<PhoneComp> listUpdNew =new List<PhoneComp>();
			List<PhoneComp> listUpdDB  =new List<PhoneComp>();
			List<PhoneComp> listDel    =new List<PhoneComp>();
			listNew.Sort((PhoneComp x,PhoneComp y) => { return x.PhoneCompNum.CompareTo(y.PhoneCompNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((PhoneComp x,PhoneComp y) => { return x.PhoneCompNum.CompareTo(y.PhoneCompNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			PhoneComp fieldNew;
			PhoneComp fieldDB;
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
				else if(fieldNew.PhoneCompNum<fieldDB.PhoneCompNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.PhoneCompNum>fieldDB.PhoneCompNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].PhoneCompNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}