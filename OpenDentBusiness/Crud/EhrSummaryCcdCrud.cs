//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrSummaryCcdCrud {
		///<summary>Gets one EhrSummaryCcd object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrSummaryCcd SelectOne(long ehrSummaryCcdNum) {
			string command="SELECT * FROM ehrsummaryccd "
				+"WHERE EhrSummaryCcdNum = "+POut.Long(ehrSummaryCcdNum);
			List<EhrSummaryCcd> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrSummaryCcd object from the database using a query.</summary>
		public static EhrSummaryCcd SelectOne(string command) {
			
			List<EhrSummaryCcd> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrSummaryCcd objects from the database using a query.</summary>
		public static List<EhrSummaryCcd> SelectMany(string command) {
			
			List<EhrSummaryCcd> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrSummaryCcd> TableToList(DataTable table) {
			List<EhrSummaryCcd> retVal=new List<EhrSummaryCcd>();
			EhrSummaryCcd ehrSummaryCcd;
			foreach(DataRow row in table.Rows) {
				ehrSummaryCcd=new EhrSummaryCcd();
				ehrSummaryCcd.EhrSummaryCcdNum= PIn.Long  (row["EhrSummaryCcdNum"].ToString());
				ehrSummaryCcd.PatNum          = PIn.Long  (row["PatNum"].ToString());
				ehrSummaryCcd.DateSummary     = PIn.Date  (row["DateSummary"].ToString());
				ehrSummaryCcd.ContentSummary  = PIn.String(row["ContentSummary"].ToString());
				ehrSummaryCcd.EmailAttachNum  = PIn.Long  (row["EmailAttachNum"].ToString());
				retVal.Add(ehrSummaryCcd);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrSummaryCcd into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrSummaryCcd> listEhrSummaryCcds,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrSummaryCcd";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrSummaryCcdNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("DateSummary");
			table.Columns.Add("ContentSummary");
			table.Columns.Add("EmailAttachNum");
			foreach(EhrSummaryCcd ehrSummaryCcd in listEhrSummaryCcds) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrSummaryCcd.EhrSummaryCcdNum),
					POut.Long  (ehrSummaryCcd.PatNum),
					POut.DateT (ehrSummaryCcd.DateSummary,false),
					            ehrSummaryCcd.ContentSummary,
					POut.Long  (ehrSummaryCcd.EmailAttachNum),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrSummaryCcd into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrSummaryCcd ehrSummaryCcd) {
			return Insert(ehrSummaryCcd,false);
		}

		///<summary>Inserts one EhrSummaryCcd into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrSummaryCcd ehrSummaryCcd,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrSummaryCcd.EhrSummaryCcdNum=ReplicationServers.GetKey("ehrsummaryccd","EhrSummaryCcdNum");
			}
			string command="INSERT INTO ehrsummaryccd (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrSummaryCcdNum,";
			}
			command+="PatNum,DateSummary,ContentSummary,EmailAttachNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrSummaryCcd.EhrSummaryCcdNum)+",";
			}
			command+=
				     POut.Long  (ehrSummaryCcd.PatNum)+","
				+    POut.Date  (ehrSummaryCcd.DateSummary)+","
				+    DbHelper.ParamChar+"paramContentSummary,"
				+    POut.Long  (ehrSummaryCcd.EmailAttachNum)+")";
			if(ehrSummaryCcd.ContentSummary==null) {
				ehrSummaryCcd.ContentSummary="";
			}
			OdSqlParameter paramContentSummary=new OdSqlParameter("paramContentSummary",OdDbType.Text,POut.StringParam(ehrSummaryCcd.ContentSummary));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramContentSummary);
			}
			else {
				ehrSummaryCcd.EhrSummaryCcdNum=Db.NonQ(command,true,"EhrSummaryCcdNum","ehrSummaryCcd",paramContentSummary);
			}
			return ehrSummaryCcd.EhrSummaryCcdNum;
		}

		///<summary>Inserts one EhrSummaryCcd into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrSummaryCcd ehrSummaryCcd) {
			return InsertNoCache(ehrSummaryCcd,false);
		}

		///<summary>Inserts one EhrSummaryCcd into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrSummaryCcd ehrSummaryCcd,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrsummaryccd (";
			if(!useExistingPK && isRandomKeys) {
				ehrSummaryCcd.EhrSummaryCcdNum=ReplicationServers.GetKeyNoCache("ehrsummaryccd","EhrSummaryCcdNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrSummaryCcdNum,";
			}
			command+="PatNum,DateSummary,ContentSummary,EmailAttachNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrSummaryCcd.EhrSummaryCcdNum)+",";
			}
			command+=
				     POut.Long  (ehrSummaryCcd.PatNum)+","
				+    POut.Date  (ehrSummaryCcd.DateSummary)+","
				+    DbHelper.ParamChar+"paramContentSummary,"
				+    POut.Long  (ehrSummaryCcd.EmailAttachNum)+")";
			if(ehrSummaryCcd.ContentSummary==null) {
				ehrSummaryCcd.ContentSummary="";
			}
			OdSqlParameter paramContentSummary=new OdSqlParameter("paramContentSummary",OdDbType.Text,POut.StringParam(ehrSummaryCcd.ContentSummary));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramContentSummary);
			}
			else {
				ehrSummaryCcd.EhrSummaryCcdNum=Db.NonQ(command,true,"EhrSummaryCcdNum","ehrSummaryCcd",paramContentSummary);
			}
			return ehrSummaryCcd.EhrSummaryCcdNum;
		}

		///<summary>Updates one EhrSummaryCcd in the database.</summary>
		public static void Update(EhrSummaryCcd ehrSummaryCcd) {
			string command="UPDATE ehrsummaryccd SET "
				+"PatNum          =  "+POut.Long  (ehrSummaryCcd.PatNum)+", "
				+"DateSummary     =  "+POut.Date  (ehrSummaryCcd.DateSummary)+", "
				+"ContentSummary  =  "+DbHelper.ParamChar+"paramContentSummary, "
				+"EmailAttachNum  =  "+POut.Long  (ehrSummaryCcd.EmailAttachNum)+" "
				+"WHERE EhrSummaryCcdNum = "+POut.Long(ehrSummaryCcd.EhrSummaryCcdNum);
			if(ehrSummaryCcd.ContentSummary==null) {
				ehrSummaryCcd.ContentSummary="";
			}
			OdSqlParameter paramContentSummary=new OdSqlParameter("paramContentSummary",OdDbType.Text,POut.StringParam(ehrSummaryCcd.ContentSummary));
			Db.NonQ(command,paramContentSummary);
		}

		///<summary>Updates one EhrSummaryCcd in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrSummaryCcd ehrSummaryCcd,EhrSummaryCcd oldEhrSummaryCcd) {
			string command="";
			if(ehrSummaryCcd.PatNum != oldEhrSummaryCcd.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(ehrSummaryCcd.PatNum)+"";
			}
			if(ehrSummaryCcd.DateSummary.Date != oldEhrSummaryCcd.DateSummary.Date) {
				if(command!="") { command+=",";}
				command+="DateSummary = "+POut.Date(ehrSummaryCcd.DateSummary)+"";
			}
			if(ehrSummaryCcd.ContentSummary != oldEhrSummaryCcd.ContentSummary) {
				if(command!="") { command+=",";}
				command+="ContentSummary = "+DbHelper.ParamChar+"paramContentSummary";
			}
			if(ehrSummaryCcd.EmailAttachNum != oldEhrSummaryCcd.EmailAttachNum) {
				if(command!="") { command+=",";}
				command+="EmailAttachNum = "+POut.Long(ehrSummaryCcd.EmailAttachNum)+"";
			}
			if(command=="") {
				return false;
			}
			if(ehrSummaryCcd.ContentSummary==null) {
				ehrSummaryCcd.ContentSummary="";
			}
			OdSqlParameter paramContentSummary=new OdSqlParameter("paramContentSummary",OdDbType.Text,POut.StringParam(ehrSummaryCcd.ContentSummary));
			command="UPDATE ehrsummaryccd SET "+command
				+" WHERE EhrSummaryCcdNum = "+POut.Long(ehrSummaryCcd.EhrSummaryCcdNum);
			Db.NonQ(command,paramContentSummary);
			return true;
		}

		///<summary>Returns true if Update(EhrSummaryCcd,EhrSummaryCcd) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrSummaryCcd ehrSummaryCcd,EhrSummaryCcd oldEhrSummaryCcd) {
			if(ehrSummaryCcd.PatNum != oldEhrSummaryCcd.PatNum) {
				return true;
			}
			if(ehrSummaryCcd.DateSummary.Date != oldEhrSummaryCcd.DateSummary.Date) {
				return true;
			}
			if(ehrSummaryCcd.ContentSummary != oldEhrSummaryCcd.ContentSummary) {
				return true;
			}
			if(ehrSummaryCcd.EmailAttachNum != oldEhrSummaryCcd.EmailAttachNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrSummaryCcd from the database.</summary>
		public static void Delete(long ehrSummaryCcdNum) {
			string command="DELETE FROM ehrsummaryccd "
				+"WHERE EhrSummaryCcdNum = "+POut.Long(ehrSummaryCcdNum);
			Db.NonQ(command);
		}

	}
}