//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JournalEntryCrud {
		///<summary>Gets one JournalEntry object from the database using the primary key.  Returns null if not found.</summary>
		public static JournalEntry SelectOne(long journalEntryNum) {
			string command="SELECT * FROM journalentry "
				+"WHERE JournalEntryNum = "+POut.Long(journalEntryNum);
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JournalEntry object from the database using a query.</summary>
		public static JournalEntry SelectOne(string command) {
			
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JournalEntry objects from the database using a query.</summary>
		public static List<JournalEntry> SelectMany(string command) {
			
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JournalEntry> TableToList(DataTable table) {
			List<JournalEntry> retVal=new List<JournalEntry>();
			JournalEntry journalEntry;
			foreach(DataRow row in table.Rows) {
				journalEntry=new JournalEntry();
				journalEntry.JournalEntryNum= PIn.Long  (row["JournalEntryNum"].ToString());
				journalEntry.TransactionNum = PIn.Long  (row["TransactionNum"].ToString());
				journalEntry.AccountNum     = PIn.Long  (row["AccountNum"].ToString());
				journalEntry.DateDisplayed  = PIn.Date  (row["DateDisplayed"].ToString());
				journalEntry.DebitAmt       = PIn.Double(row["DebitAmt"].ToString());
				journalEntry.CreditAmt      = PIn.Double(row["CreditAmt"].ToString());
				journalEntry.Memo           = PIn.String(row["Memo"].ToString());
				journalEntry.Splits         = PIn.String(row["Splits"].ToString());
				journalEntry.CheckNumber    = PIn.String(row["CheckNumber"].ToString());
				journalEntry.ReconcileNum   = PIn.Long  (row["ReconcileNum"].ToString());
				journalEntry.SecUserNumEntry= PIn.Long  (row["SecUserNumEntry"].ToString());
				journalEntry.SecDateTEntry  = PIn.DateT (row["SecDateTEntry"].ToString());
				journalEntry.SecUserNumEdit = PIn.Long  (row["SecUserNumEdit"].ToString());
				journalEntry.SecDateTEdit   = PIn.DateT (row["SecDateTEdit"].ToString());
				retVal.Add(journalEntry);
			}
			return retVal;
		}

		///<summary>Converts a list of JournalEntry into a DataTable.</summary>
		public static DataTable ListToTable(List<JournalEntry> listJournalEntrys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="JournalEntry";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("JournalEntryNum");
			table.Columns.Add("TransactionNum");
			table.Columns.Add("AccountNum");
			table.Columns.Add("DateDisplayed");
			table.Columns.Add("DebitAmt");
			table.Columns.Add("CreditAmt");
			table.Columns.Add("Memo");
			table.Columns.Add("Splits");
			table.Columns.Add("CheckNumber");
			table.Columns.Add("ReconcileNum");
			table.Columns.Add("SecUserNumEntry");
			table.Columns.Add("SecDateTEntry");
			table.Columns.Add("SecUserNumEdit");
			table.Columns.Add("SecDateTEdit");
			foreach(JournalEntry journalEntry in listJournalEntrys) {
				table.Rows.Add(new object[] {
					POut.Long  (journalEntry.JournalEntryNum),
					POut.Long  (journalEntry.TransactionNum),
					POut.Long  (journalEntry.AccountNum),
					POut.DateT (journalEntry.DateDisplayed,false),
					POut.Double(journalEntry.DebitAmt),
					POut.Double(journalEntry.CreditAmt),
					            journalEntry.Memo,
					            journalEntry.Splits,
					            journalEntry.CheckNumber,
					POut.Long  (journalEntry.ReconcileNum),
					POut.Long  (journalEntry.SecUserNumEntry),
					POut.DateT (journalEntry.SecDateTEntry,false),
					POut.Long  (journalEntry.SecUserNumEdit),
					POut.DateT (journalEntry.SecDateTEdit,false),
				});
			}
			return table;
		}

		///<summary>Inserts one JournalEntry into the database.  Returns the new priKey.</summary>
		public static long Insert(JournalEntry journalEntry) {
			return Insert(journalEntry,false);
		}

		///<summary>Inserts one JournalEntry into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JournalEntry journalEntry,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				journalEntry.JournalEntryNum=ReplicationServers.GetKey("journalentry","JournalEntryNum");
			}
			string command="INSERT INTO journalentry (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="JournalEntryNum,";
			}
			command+="TransactionNum,AccountNum,DateDisplayed,DebitAmt,CreditAmt,Memo,Splits,CheckNumber,ReconcileNum,SecUserNumEntry,SecDateTEntry,SecUserNumEdit) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(journalEntry.JournalEntryNum)+",";
			}
			command+=
				     POut.Long  (journalEntry.TransactionNum)+","
				+    POut.Long  (journalEntry.AccountNum)+","
				+    POut.Date  (journalEntry.DateDisplayed)+","
				+"'"+POut.Double(journalEntry.DebitAmt)+"',"
				+"'"+POut.Double(journalEntry.CreditAmt)+"',"
				+    DbHelper.ParamChar+"paramMemo,"
				+    DbHelper.ParamChar+"paramSplits,"
				+"'"+POut.String(journalEntry.CheckNumber)+"',"
				+    POut.Long  (journalEntry.ReconcileNum)+","
				+    POut.Long  (journalEntry.SecUserNumEntry)+","
				+    DbHelper.Now()+","
				+    POut.Long  (journalEntry.SecUserNumEdit)+")";
				//SecDateTEdit can only be set by MySQL
			if(journalEntry.Memo==null) {
				journalEntry.Memo="";
			}
			OdSqlParameter paramMemo=new OdSqlParameter("paramMemo",OdDbType.Text,POut.StringParam(journalEntry.Memo));
			if(journalEntry.Splits==null) {
				journalEntry.Splits="";
			}
			OdSqlParameter paramSplits=new OdSqlParameter("paramSplits",OdDbType.Text,POut.StringParam(journalEntry.Splits));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramMemo,paramSplits);
			}
			else {
				journalEntry.JournalEntryNum=Db.NonQ(command,true,"JournalEntryNum","journalEntry",paramMemo,paramSplits);
			}
			return journalEntry.JournalEntryNum;
		}

		///<summary>Inserts one JournalEntry into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JournalEntry journalEntry) {
			return InsertNoCache(journalEntry,false);
		}

		///<summary>Inserts one JournalEntry into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JournalEntry journalEntry,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO journalentry (";
			if(!useExistingPK && isRandomKeys) {
				journalEntry.JournalEntryNum=ReplicationServers.GetKeyNoCache("journalentry","JournalEntryNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JournalEntryNum,";
			}
			command+="TransactionNum,AccountNum,DateDisplayed,DebitAmt,CreditAmt,Memo,Splits,CheckNumber,ReconcileNum,SecUserNumEntry,SecDateTEntry,SecUserNumEdit) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(journalEntry.JournalEntryNum)+",";
			}
			command+=
				     POut.Long  (journalEntry.TransactionNum)+","
				+    POut.Long  (journalEntry.AccountNum)+","
				+    POut.Date  (journalEntry.DateDisplayed)+","
				+"'"+POut.Double(journalEntry.DebitAmt)+"',"
				+"'"+POut.Double(journalEntry.CreditAmt)+"',"
				+    DbHelper.ParamChar+"paramMemo,"
				+    DbHelper.ParamChar+"paramSplits,"
				+"'"+POut.String(journalEntry.CheckNumber)+"',"
				+    POut.Long  (journalEntry.ReconcileNum)+","
				+    POut.Long  (journalEntry.SecUserNumEntry)+","
				+    DbHelper.Now()+","
				+    POut.Long  (journalEntry.SecUserNumEdit)+")";
				//SecDateTEdit can only be set by MySQL
			if(journalEntry.Memo==null) {
				journalEntry.Memo="";
			}
			OdSqlParameter paramMemo=new OdSqlParameter("paramMemo",OdDbType.Text,POut.StringParam(journalEntry.Memo));
			if(journalEntry.Splits==null) {
				journalEntry.Splits="";
			}
			OdSqlParameter paramSplits=new OdSqlParameter("paramSplits",OdDbType.Text,POut.StringParam(journalEntry.Splits));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramMemo,paramSplits);
			}
			else {
				journalEntry.JournalEntryNum=Db.NonQ(command,true,"JournalEntryNum","journalEntry",paramMemo,paramSplits);
			}
			return journalEntry.JournalEntryNum;
		}

		///<summary>Updates one JournalEntry in the database.</summary>
		public static void Update(JournalEntry journalEntry) {
			string command="UPDATE journalentry SET "
				+"TransactionNum =  "+POut.Long  (journalEntry.TransactionNum)+", "
				+"AccountNum     =  "+POut.Long  (journalEntry.AccountNum)+", "
				+"DateDisplayed  =  "+POut.Date  (journalEntry.DateDisplayed)+", "
				+"DebitAmt       = '"+POut.Double(journalEntry.DebitAmt)+"', "
				+"CreditAmt      = '"+POut.Double(journalEntry.CreditAmt)+"', "
				+"Memo           =  "+DbHelper.ParamChar+"paramMemo, "
				+"Splits         =  "+DbHelper.ParamChar+"paramSplits, "
				+"CheckNumber    = '"+POut.String(journalEntry.CheckNumber)+"', "
				+"ReconcileNum   =  "+POut.Long  (journalEntry.ReconcileNum)+", "
				//SecUserNumEntry excluded from update
				//SecDateTEntry not allowed to change
				+"SecUserNumEdit =  "+POut.Long  (journalEntry.SecUserNumEdit)+" "
				//SecDateTEdit can only be set by MySQL
				+"WHERE JournalEntryNum = "+POut.Long(journalEntry.JournalEntryNum);
			if(journalEntry.Memo==null) {
				journalEntry.Memo="";
			}
			OdSqlParameter paramMemo=new OdSqlParameter("paramMemo",OdDbType.Text,POut.StringParam(journalEntry.Memo));
			if(journalEntry.Splits==null) {
				journalEntry.Splits="";
			}
			OdSqlParameter paramSplits=new OdSqlParameter("paramSplits",OdDbType.Text,POut.StringParam(journalEntry.Splits));
			Db.NonQ(command,paramMemo,paramSplits);
		}

		///<summary>Updates one JournalEntry in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JournalEntry journalEntry,JournalEntry oldJournalEntry) {
			string command="";
			if(journalEntry.TransactionNum != oldJournalEntry.TransactionNum) {
				if(command!="") { command+=",";}
				command+="TransactionNum = "+POut.Long(journalEntry.TransactionNum)+"";
			}
			if(journalEntry.AccountNum != oldJournalEntry.AccountNum) {
				if(command!="") { command+=",";}
				command+="AccountNum = "+POut.Long(journalEntry.AccountNum)+"";
			}
			if(journalEntry.DateDisplayed.Date != oldJournalEntry.DateDisplayed.Date) {
				if(command!="") { command+=",";}
				command+="DateDisplayed = "+POut.Date(journalEntry.DateDisplayed)+"";
			}
			if(journalEntry.DebitAmt != oldJournalEntry.DebitAmt) {
				if(command!="") { command+=",";}
				command+="DebitAmt = '"+POut.Double(journalEntry.DebitAmt)+"'";
			}
			if(journalEntry.CreditAmt != oldJournalEntry.CreditAmt) {
				if(command!="") { command+=",";}
				command+="CreditAmt = '"+POut.Double(journalEntry.CreditAmt)+"'";
			}
			if(journalEntry.Memo != oldJournalEntry.Memo) {
				if(command!="") { command+=",";}
				command+="Memo = "+DbHelper.ParamChar+"paramMemo";
			}
			if(journalEntry.Splits != oldJournalEntry.Splits) {
				if(command!="") { command+=",";}
				command+="Splits = "+DbHelper.ParamChar+"paramSplits";
			}
			if(journalEntry.CheckNumber != oldJournalEntry.CheckNumber) {
				if(command!="") { command+=",";}
				command+="CheckNumber = '"+POut.String(journalEntry.CheckNumber)+"'";
			}
			if(journalEntry.ReconcileNum != oldJournalEntry.ReconcileNum) {
				if(command!="") { command+=",";}
				command+="ReconcileNum = "+POut.Long(journalEntry.ReconcileNum)+"";
			}
			//SecUserNumEntry excluded from update
			//SecDateTEntry not allowed to change
			if(journalEntry.SecUserNumEdit != oldJournalEntry.SecUserNumEdit) {
				if(command!="") { command+=",";}
				command+="SecUserNumEdit = "+POut.Long(journalEntry.SecUserNumEdit)+"";
			}
			//SecDateTEdit can only be set by MySQL
			if(command=="") {
				return false;
			}
			if(journalEntry.Memo==null) {
				journalEntry.Memo="";
			}
			OdSqlParameter paramMemo=new OdSqlParameter("paramMemo",OdDbType.Text,POut.StringParam(journalEntry.Memo));
			if(journalEntry.Splits==null) {
				journalEntry.Splits="";
			}
			OdSqlParameter paramSplits=new OdSqlParameter("paramSplits",OdDbType.Text,POut.StringParam(journalEntry.Splits));
			command="UPDATE journalentry SET "+command
				+" WHERE JournalEntryNum = "+POut.Long(journalEntry.JournalEntryNum);
			Db.NonQ(command,paramMemo,paramSplits);
			return true;
		}

		///<summary>Returns true if Update(JournalEntry,JournalEntry) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(JournalEntry journalEntry,JournalEntry oldJournalEntry) {
			if(journalEntry.TransactionNum != oldJournalEntry.TransactionNum) {
				return true;
			}
			if(journalEntry.AccountNum != oldJournalEntry.AccountNum) {
				return true;
			}
			if(journalEntry.DateDisplayed.Date != oldJournalEntry.DateDisplayed.Date) {
				return true;
			}
			if(journalEntry.DebitAmt != oldJournalEntry.DebitAmt) {
				return true;
			}
			if(journalEntry.CreditAmt != oldJournalEntry.CreditAmt) {
				return true;
			}
			if(journalEntry.Memo != oldJournalEntry.Memo) {
				return true;
			}
			if(journalEntry.Splits != oldJournalEntry.Splits) {
				return true;
			}
			if(journalEntry.CheckNumber != oldJournalEntry.CheckNumber) {
				return true;
			}
			if(journalEntry.ReconcileNum != oldJournalEntry.ReconcileNum) {
				return true;
			}
			//SecUserNumEntry excluded from update
			//SecDateTEntry not allowed to change
			if(journalEntry.SecUserNumEdit != oldJournalEntry.SecUserNumEdit) {
				return true;
			}
			//SecDateTEdit can only be set by MySQL
			return false;
		}

		///<summary>Deletes one JournalEntry from the database.</summary>
		public static void Delete(long journalEntryNum) {
			string command="DELETE FROM journalentry "
				+"WHERE JournalEntryNum = "+POut.Long(journalEntryNum);
			Db.NonQ(command);
		}

	}
}