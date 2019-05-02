//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RxNormCrud {
		///<summary>Gets one RxNorm object from the database using the primary key.  Returns null if not found.</summary>
		public static RxNorm SelectOne(long rxNormNum) {
			string command="SELECT * FROM rxnorm "
				+"WHERE RxNormNum = "+POut.Long(rxNormNum);
			List<RxNorm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RxNorm object from the database using a query.</summary>
		public static RxNorm SelectOne(string command) {
			
			List<RxNorm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RxNorm objects from the database using a query.</summary>
		public static List<RxNorm> SelectMany(string command) {
			
			List<RxNorm> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RxNorm> TableToList(DataTable table) {
			List<RxNorm> retVal=new List<RxNorm>();
			RxNorm rxNorm;
			foreach(DataRow row in table.Rows) {
				rxNorm=new RxNorm();
				rxNorm.RxNormNum  = PIn.Long  (row["RxNormNum"].ToString());
				rxNorm.RxCui      = PIn.String(row["RxCui"].ToString());
				rxNorm.MmslCode   = PIn.String(row["MmslCode"].ToString());
				rxNorm.Description= PIn.String(row["Description"].ToString());
				retVal.Add(rxNorm);
			}
			return retVal;
		}

		///<summary>Converts a list of RxNorm into a DataTable.</summary>
		public static DataTable ListToTable(List<RxNorm> listRxNorms,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RxNorm";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RxNormNum");
			table.Columns.Add("RxCui");
			table.Columns.Add("MmslCode");
			table.Columns.Add("Description");
			foreach(RxNorm rxNorm in listRxNorms) {
				table.Rows.Add(new object[] {
					POut.Long  (rxNorm.RxNormNum),
					            rxNorm.RxCui,
					            rxNorm.MmslCode,
					            rxNorm.Description,
				});
			}
			return table;
		}

		///<summary>Inserts one RxNorm into the database.  Returns the new priKey.</summary>
		public static long Insert(RxNorm rxNorm) {
			return Insert(rxNorm,false);
		}

		///<summary>Inserts one RxNorm into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RxNorm rxNorm,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				rxNorm.RxNormNum=ReplicationServers.GetKey("rxnorm","RxNormNum");
			}
			string command="INSERT INTO rxnorm (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="RxNormNum,";
			}
			command+="RxCui,MmslCode,Description) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(rxNorm.RxNormNum)+",";
			}
			command+=
				 "'"+POut.String(rxNorm.RxCui)+"',"
				+"'"+POut.String(rxNorm.MmslCode)+"',"
				+    DbHelper.ParamChar+"paramDescription)";
			if(rxNorm.Description==null) {
				rxNorm.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(rxNorm.Description));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramDescription);
			}
			else {
				rxNorm.RxNormNum=Db.NonQ(command,true,"RxNormNum","rxNorm",paramDescription);
			}
			return rxNorm.RxNormNum;
		}

		///<summary>Inserts one RxNorm into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RxNorm rxNorm) {
			return InsertNoCache(rxNorm,false);
		}

		///<summary>Inserts one RxNorm into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RxNorm rxNorm,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO rxnorm (";
			if(!useExistingPK && isRandomKeys) {
				rxNorm.RxNormNum=ReplicationServers.GetKeyNoCache("rxnorm","RxNormNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RxNormNum,";
			}
			command+="RxCui,MmslCode,Description) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(rxNorm.RxNormNum)+",";
			}
			command+=
				 "'"+POut.String(rxNorm.RxCui)+"',"
				+"'"+POut.String(rxNorm.MmslCode)+"',"
				+    DbHelper.ParamChar+"paramDescription)";
			if(rxNorm.Description==null) {
				rxNorm.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(rxNorm.Description));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription);
			}
			else {
				rxNorm.RxNormNum=Db.NonQ(command,true,"RxNormNum","rxNorm",paramDescription);
			}
			return rxNorm.RxNormNum;
		}

		///<summary>Updates one RxNorm in the database.</summary>
		public static void Update(RxNorm rxNorm) {
			string command="UPDATE rxnorm SET "
				+"RxCui      = '"+POut.String(rxNorm.RxCui)+"', "
				+"MmslCode   = '"+POut.String(rxNorm.MmslCode)+"', "
				+"Description=  "+DbHelper.ParamChar+"paramDescription "
				+"WHERE RxNormNum = "+POut.Long(rxNorm.RxNormNum);
			if(rxNorm.Description==null) {
				rxNorm.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(rxNorm.Description));
			Db.NonQ(command,paramDescription);
		}

		///<summary>Updates one RxNorm in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RxNorm rxNorm,RxNorm oldRxNorm) {
			string command="";
			if(rxNorm.RxCui != oldRxNorm.RxCui) {
				if(command!="") { command+=",";}
				command+="RxCui = '"+POut.String(rxNorm.RxCui)+"'";
			}
			if(rxNorm.MmslCode != oldRxNorm.MmslCode) {
				if(command!="") { command+=",";}
				command+="MmslCode = '"+POut.String(rxNorm.MmslCode)+"'";
			}
			if(rxNorm.Description != oldRxNorm.Description) {
				if(command!="") { command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(command=="") {
				return false;
			}
			if(rxNorm.Description==null) {
				rxNorm.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(rxNorm.Description));
			command="UPDATE rxnorm SET "+command
				+" WHERE RxNormNum = "+POut.Long(rxNorm.RxNormNum);
			Db.NonQ(command,paramDescription);
			return true;
		}

		///<summary>Returns true if Update(RxNorm,RxNorm) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(RxNorm rxNorm,RxNorm oldRxNorm) {
			if(rxNorm.RxCui != oldRxNorm.RxCui) {
				return true;
			}
			if(rxNorm.MmslCode != oldRxNorm.MmslCode) {
				return true;
			}
			if(rxNorm.Description != oldRxNorm.Description) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one RxNorm from the database.</summary>
		public static void Delete(long rxNormNum) {
			string command="DELETE FROM rxnorm "
				+"WHERE RxNormNum = "+POut.Long(rxNormNum);
			Db.NonQ(command);
		}

	}
}