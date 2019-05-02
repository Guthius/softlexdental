//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ClinicPrefCrud {
		///<summary>Gets one ClinicPref object from the database using the primary key.  Returns null if not found.</summary>
		public static ClinicPref SelectOne(long clinicPrefNum) {
			string command="SELECT * FROM clinicpref "
				+"WHERE ClinicPrefNum = "+POut.Long(clinicPrefNum);
			List<ClinicPref> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ClinicPref object from the database using a query.</summary>
		public static ClinicPref SelectOne(string command) {
			
			List<ClinicPref> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ClinicPref objects from the database using a query.</summary>
		public static List<ClinicPref> SelectMany(string command) {
			
			List<ClinicPref> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ClinicPref> TableToList(DataTable table) {
			List<ClinicPref> retVal=new List<ClinicPref>();
			ClinicPref clinicPref;
			foreach(DataRow row in table.Rows) {
				clinicPref=new ClinicPref();
				clinicPref.ClinicPrefNum= PIn.Long  (row["ClinicPrefNum"].ToString());
				clinicPref.ClinicNum    = PIn.Long  (row["ClinicNum"].ToString());
				string prefName=row["PrefName"].ToString();
				if(prefName=="") {
					clinicPref.PrefName   =(PrefName)0;
				}
				else try{
					clinicPref.PrefName   =(PrefName)Enum.Parse(typeof(PrefName),prefName);
				}
				catch{
					clinicPref.PrefName   =(PrefName)0;
				}
				clinicPref.ValueString  = PIn.String(row["ValueString"].ToString());
				retVal.Add(clinicPref);
			}
			return retVal;
		}

		///<summary>Converts a list of ClinicPref into a DataTable.</summary>
		public static DataTable ListToTable(List<ClinicPref> listClinicPrefs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ClinicPref";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ClinicPrefNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("PrefName");
			table.Columns.Add("ValueString");
			foreach(ClinicPref clinicPref in listClinicPrefs) {
				table.Rows.Add(new object[] {
					POut.Long  (clinicPref.ClinicPrefNum),
					POut.Long  (clinicPref.ClinicNum),
					POut.Int   ((int)clinicPref.PrefName),
					            clinicPref.ValueString,
				});
			}
			return table;
		}

		///<summary>Inserts one ClinicPref into the database.  Returns the new priKey.</summary>
		public static long Insert(ClinicPref clinicPref) {
			return Insert(clinicPref,false);
		}

		///<summary>Inserts one ClinicPref into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ClinicPref clinicPref,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				clinicPref.ClinicPrefNum=ReplicationServers.GetKey("clinicpref","ClinicPrefNum");
			}
			string command="INSERT INTO clinicpref (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ClinicPrefNum,";
			}
			command+="ClinicNum,PrefName,ValueString) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(clinicPref.ClinicPrefNum)+",";
			}
			command+=
				     POut.Long  (clinicPref.ClinicNum)+","
				+"'"+POut.String(clinicPref.PrefName.ToString())+"',"
				+    DbHelper.ParamChar+"paramValueString)";
			if(clinicPref.ValueString==null) {
				clinicPref.ValueString="";
			}
			OdSqlParameter paramValueString=new OdSqlParameter("paramValueString",OdDbType.Text,POut.StringParam(clinicPref.ValueString));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramValueString);
			}
			else {
				clinicPref.ClinicPrefNum=Db.NonQ(command,true,"ClinicPrefNum","clinicPref",paramValueString);
			}
			return clinicPref.ClinicPrefNum;
		}

		///<summary>Inserts one ClinicPref into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClinicPref clinicPref) {
			return InsertNoCache(clinicPref,false);
		}

		///<summary>Inserts one ClinicPref into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClinicPref clinicPref,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO clinicpref (";
			if(!useExistingPK && isRandomKeys) {
				clinicPref.ClinicPrefNum=ReplicationServers.GetKeyNoCache("clinicpref","ClinicPrefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ClinicPrefNum,";
			}
			command+="ClinicNum,PrefName,ValueString) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(clinicPref.ClinicPrefNum)+",";
			}
			command+=
				     POut.Long  (clinicPref.ClinicNum)+","
				+"'"+POut.String(clinicPref.PrefName.ToString())+"',"
				+    DbHelper.ParamChar+"paramValueString)";
			if(clinicPref.ValueString==null) {
				clinicPref.ValueString="";
			}
			OdSqlParameter paramValueString=new OdSqlParameter("paramValueString",OdDbType.Text,POut.StringParam(clinicPref.ValueString));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramValueString);
			}
			else {
				clinicPref.ClinicPrefNum=Db.NonQ(command,true,"ClinicPrefNum","clinicPref",paramValueString);
			}
			return clinicPref.ClinicPrefNum;
		}

		///<summary>Updates one ClinicPref in the database.</summary>
		public static void Update(ClinicPref clinicPref) {
			string command="UPDATE clinicpref SET "
				+"ClinicNum    =  "+POut.Long  (clinicPref.ClinicNum)+", "
				+"PrefName     = '"+POut.String(clinicPref.PrefName.ToString())+"', "
				+"ValueString  =  "+DbHelper.ParamChar+"paramValueString "
				+"WHERE ClinicPrefNum = "+POut.Long(clinicPref.ClinicPrefNum);
			if(clinicPref.ValueString==null) {
				clinicPref.ValueString="";
			}
			OdSqlParameter paramValueString=new OdSqlParameter("paramValueString",OdDbType.Text,POut.StringParam(clinicPref.ValueString));
			Db.NonQ(command,paramValueString);
		}

		///<summary>Updates one ClinicPref in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ClinicPref clinicPref,ClinicPref oldClinicPref) {
			string command="";
			if(clinicPref.ClinicNum != oldClinicPref.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(clinicPref.ClinicNum)+"";
			}
			if(clinicPref.PrefName != oldClinicPref.PrefName) {
				if(command!="") { command+=",";}
				command+="PrefName = '"+POut.String(clinicPref.PrefName.ToString())+"'";
			}
			if(clinicPref.ValueString != oldClinicPref.ValueString) {
				if(command!="") { command+=",";}
				command+="ValueString = "+DbHelper.ParamChar+"paramValueString";
			}
			if(command=="") {
				return false;
			}
			if(clinicPref.ValueString==null) {
				clinicPref.ValueString="";
			}
			OdSqlParameter paramValueString=new OdSqlParameter("paramValueString",OdDbType.Text,POut.StringParam(clinicPref.ValueString));
			command="UPDATE clinicpref SET "+command
				+" WHERE ClinicPrefNum = "+POut.Long(clinicPref.ClinicPrefNum);
			Db.NonQ(command,paramValueString);
			return true;
		}

		///<summary>Returns true if Update(ClinicPref,ClinicPref) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ClinicPref clinicPref,ClinicPref oldClinicPref) {
			if(clinicPref.ClinicNum != oldClinicPref.ClinicNum) {
				return true;
			}
			if(clinicPref.PrefName != oldClinicPref.PrefName) {
				return true;
			}
			if(clinicPref.ValueString != oldClinicPref.ValueString) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ClinicPref from the database.</summary>
		public static void Delete(long clinicPrefNum) {
			string command="DELETE FROM clinicpref "
				+"WHERE ClinicPrefNum = "+POut.Long(clinicPrefNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ClinicPref> listNew,List<ClinicPref> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ClinicPref> listIns    =new List<ClinicPref>();
			List<ClinicPref> listUpdNew =new List<ClinicPref>();
			List<ClinicPref> listUpdDB  =new List<ClinicPref>();
			List<ClinicPref> listDel    =new List<ClinicPref>();
			listNew.Sort((ClinicPref x,ClinicPref y) => { return x.ClinicPrefNum.CompareTo(y.ClinicPrefNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ClinicPref x,ClinicPref y) => { return x.ClinicPrefNum.CompareTo(y.ClinicPrefNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ClinicPref fieldNew;
			ClinicPref fieldDB;
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
				else if(fieldNew.ClinicPrefNum<fieldDB.ClinicPrefNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ClinicPrefNum>fieldDB.ClinicPrefNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ClinicPrefNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}