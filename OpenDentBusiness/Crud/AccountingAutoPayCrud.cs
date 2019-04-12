//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class AccountingAutoPayCrud {
		///<summary>Gets one AccountingAutoPay object from the database using the primary key.  Returns null if not found.</summary>
		public static AccountingAutoPay SelectOne(long accountingAutoPayNum) {
			string command="SELECT * FROM accountingautopay "
				+"WHERE AccountingAutoPayNum = "+POut.Long(accountingAutoPayNum);
			List<AccountingAutoPay> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AccountingAutoPay object from the database using a query.</summary>
		public static AccountingAutoPay SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AccountingAutoPay> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AccountingAutoPay objects from the database using a query.</summary>
		public static List<AccountingAutoPay> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AccountingAutoPay> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<AccountingAutoPay> TableToList(DataTable table) {
			List<AccountingAutoPay> retVal=new List<AccountingAutoPay>();
			AccountingAutoPay accountingAutoPay;
			foreach(DataRow row in table.Rows) {
				accountingAutoPay=new AccountingAutoPay();
				accountingAutoPay.AccountingAutoPayNum= PIn.Long  (row["AccountingAutoPayNum"].ToString());
				accountingAutoPay.PayType             = PIn.Long  (row["PayType"].ToString());
				accountingAutoPay.PickList            = PIn.String(row["PickList"].ToString());
				retVal.Add(accountingAutoPay);
			}
			return retVal;
		}

		///<summary>Converts a list of AccountingAutoPay into a DataTable.</summary>
		public static DataTable ListToTable(List<AccountingAutoPay> listAccountingAutoPays,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="AccountingAutoPay";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AccountingAutoPayNum");
			table.Columns.Add("PayType");
			table.Columns.Add("PickList");
			foreach(AccountingAutoPay accountingAutoPay in listAccountingAutoPays) {
				table.Rows.Add(new object[] {
					POut.Long  (accountingAutoPay.AccountingAutoPayNum),
					POut.Long  (accountingAutoPay.PayType),
					            accountingAutoPay.PickList,
				});
			}
			return table;
		}

		///<summary>Inserts one AccountingAutoPay into the database.  Returns the new priKey.</summary>
		public static long Insert(AccountingAutoPay accountingAutoPay) {
			return Insert(accountingAutoPay,false);
		}

		///<summary>Inserts one AccountingAutoPay into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(AccountingAutoPay accountingAutoPay,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				accountingAutoPay.AccountingAutoPayNum=ReplicationServers.GetKey("accountingautopay","AccountingAutoPayNum");
			}
			string command="INSERT INTO accountingautopay (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AccountingAutoPayNum,";
			}
			command+="PayType,PickList) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(accountingAutoPay.AccountingAutoPayNum)+",";
			}
			command+=
				     POut.Long  (accountingAutoPay.PayType)+","
				+"'"+POut.String(accountingAutoPay.PickList)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				accountingAutoPay.AccountingAutoPayNum=Db.NonQ(command,true,"AccountingAutoPayNum","accountingAutoPay");
			}
			return accountingAutoPay.AccountingAutoPayNum;
		}

		///<summary>Inserts one AccountingAutoPay into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AccountingAutoPay accountingAutoPay) {
			return InsertNoCache(accountingAutoPay,false);
		}

		///<summary>Inserts one AccountingAutoPay into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AccountingAutoPay accountingAutoPay,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO accountingautopay (";
			if(!useExistingPK && isRandomKeys) {
				accountingAutoPay.AccountingAutoPayNum=ReplicationServers.GetKeyNoCache("accountingautopay","AccountingAutoPayNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AccountingAutoPayNum,";
			}
			command+="PayType,PickList) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(accountingAutoPay.AccountingAutoPayNum)+",";
			}
			command+=
				     POut.Long  (accountingAutoPay.PayType)+","
				+"'"+POut.String(accountingAutoPay.PickList)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				accountingAutoPay.AccountingAutoPayNum=Db.NonQ(command,true,"AccountingAutoPayNum","accountingAutoPay");
			}
			return accountingAutoPay.AccountingAutoPayNum;
		}

		///<summary>Updates one AccountingAutoPay in the database.</summary>
		public static void Update(AccountingAutoPay accountingAutoPay) {
			string command="UPDATE accountingautopay SET "
				+"PayType             =  "+POut.Long  (accountingAutoPay.PayType)+", "
				+"PickList            = '"+POut.String(accountingAutoPay.PickList)+"' "
				+"WHERE AccountingAutoPayNum = "+POut.Long(accountingAutoPay.AccountingAutoPayNum);
			Db.NonQ(command);
		}

		///<summary>Updates one AccountingAutoPay in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(AccountingAutoPay accountingAutoPay,AccountingAutoPay oldAccountingAutoPay) {
			string command="";
			if(accountingAutoPay.PayType != oldAccountingAutoPay.PayType) {
				if(command!="") { command+=",";}
				command+="PayType = "+POut.Long(accountingAutoPay.PayType)+"";
			}
			if(accountingAutoPay.PickList != oldAccountingAutoPay.PickList) {
				if(command!="") { command+=",";}
				command+="PickList = '"+POut.String(accountingAutoPay.PickList)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE accountingautopay SET "+command
				+" WHERE AccountingAutoPayNum = "+POut.Long(accountingAutoPay.AccountingAutoPayNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(AccountingAutoPay,AccountingAutoPay) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(AccountingAutoPay accountingAutoPay,AccountingAutoPay oldAccountingAutoPay) {
			if(accountingAutoPay.PayType != oldAccountingAutoPay.PayType) {
				return true;
			}
			if(accountingAutoPay.PickList != oldAccountingAutoPay.PickList) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one AccountingAutoPay from the database.</summary>
		public static void Delete(long accountingAutoPayNum) {
			string command="DELETE FROM accountingautopay "
				+"WHERE AccountingAutoPayNum = "+POut.Long(accountingAutoPayNum);
			Db.NonQ(command);
		}

	}
}