//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RecurringChargeCrud {
		///<summary>Gets one RecurringCharge object from the database using the primary key.  Returns null if not found.</summary>
		public static RecurringCharge SelectOne(long recurringChargeNum) {
			string command="SELECT * FROM recurringcharge "
				+"WHERE RecurringChargeNum = "+POut.Long(recurringChargeNum);
			List<RecurringCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RecurringCharge object from the database using a query.</summary>
		public static RecurringCharge SelectOne(string command) {
			
			List<RecurringCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RecurringCharge objects from the database using a query.</summary>
		public static List<RecurringCharge> SelectMany(string command) {
			
			List<RecurringCharge> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RecurringCharge> TableToList(DataTable table) {
			List<RecurringCharge> retVal=new List<RecurringCharge>();
			RecurringCharge recurringCharge;
			foreach(DataRow row in table.Rows) {
				recurringCharge=new RecurringCharge();
				recurringCharge.RecurringChargeNum= PIn.Long  (row["RecurringChargeNum"].ToString());
				recurringCharge.PatNum            = PIn.Long  (row["PatNum"].ToString());
				recurringCharge.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				recurringCharge.DateTimeCharge    = PIn.DateT (row["DateTimeCharge"].ToString());
				recurringCharge.ChargeStatus      = (OpenDentBusiness.RecurringChargeStatus)PIn.Int(row["ChargeStatus"].ToString());
				recurringCharge.FamBal            = PIn.Double(row["FamBal"].ToString());
				recurringCharge.PayPlanDue        = PIn.Double(row["PayPlanDue"].ToString());
				recurringCharge.TotalDue          = PIn.Double(row["TotalDue"].ToString());
				recurringCharge.RepeatAmt         = PIn.Double(row["RepeatAmt"].ToString());
				recurringCharge.ChargeAmt         = PIn.Double(row["ChargeAmt"].ToString());
				recurringCharge.UserNum           = PIn.Long  (row["UserNum"].ToString());
				recurringCharge.PayNum            = PIn.Long  (row["PayNum"].ToString());
				recurringCharge.CreditCardNum     = PIn.Long  (row["CreditCardNum"].ToString());
				recurringCharge.ErrorMsg          = PIn.String(row["ErrorMsg"].ToString());
				retVal.Add(recurringCharge);
			}
			return retVal;
		}

		///<summary>Converts a list of RecurringCharge into a DataTable.</summary>
		public static DataTable ListToTable(List<RecurringCharge> listRecurringCharges,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RecurringCharge";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RecurringChargeNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("DateTimeCharge");
			table.Columns.Add("ChargeStatus");
			table.Columns.Add("FamBal");
			table.Columns.Add("PayPlanDue");
			table.Columns.Add("TotalDue");
			table.Columns.Add("RepeatAmt");
			table.Columns.Add("ChargeAmt");
			table.Columns.Add("UserNum");
			table.Columns.Add("PayNum");
			table.Columns.Add("CreditCardNum");
			table.Columns.Add("ErrorMsg");
			foreach(RecurringCharge recurringCharge in listRecurringCharges) {
				table.Rows.Add(new object[] {
					POut.Long  (recurringCharge.RecurringChargeNum),
					POut.Long  (recurringCharge.PatNum),
					POut.Long  (recurringCharge.ClinicNum),
					POut.DateT (recurringCharge.DateTimeCharge,false),
					POut.Int   ((int)recurringCharge.ChargeStatus),
					POut.Double(recurringCharge.FamBal),
					POut.Double(recurringCharge.PayPlanDue),
					POut.Double(recurringCharge.TotalDue),
					POut.Double(recurringCharge.RepeatAmt),
					POut.Double(recurringCharge.ChargeAmt),
					POut.Long  (recurringCharge.UserNum),
					POut.Long  (recurringCharge.PayNum),
					POut.Long  (recurringCharge.CreditCardNum),
					            recurringCharge.ErrorMsg,
				});
			}
			return table;
		}

		///<summary>Inserts one RecurringCharge into the database.  Returns the new priKey.</summary>
		public static long Insert(RecurringCharge recurringCharge) {
			return Insert(recurringCharge,false);
		}

		///<summary>Inserts one RecurringCharge into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RecurringCharge recurringCharge,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				recurringCharge.RecurringChargeNum=ReplicationServers.GetKey("recurringcharge","RecurringChargeNum");
			}
			string command="INSERT INTO recurringcharge (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="RecurringChargeNum,";
			}
			command+="PatNum,ClinicNum,DateTimeCharge,ChargeStatus,FamBal,PayPlanDue,TotalDue,RepeatAmt,ChargeAmt,UserNum,PayNum,CreditCardNum,ErrorMsg) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(recurringCharge.RecurringChargeNum)+",";
			}
			command+=
				     POut.Long  (recurringCharge.PatNum)+","
				+    POut.Long  (recurringCharge.ClinicNum)+","
				+    POut.DateT (recurringCharge.DateTimeCharge)+","
				+    POut.Int   ((int)recurringCharge.ChargeStatus)+","
				+"'"+POut.Double(recurringCharge.FamBal)+"',"
				+"'"+POut.Double(recurringCharge.PayPlanDue)+"',"
				+"'"+POut.Double(recurringCharge.TotalDue)+"',"
				+"'"+POut.Double(recurringCharge.RepeatAmt)+"',"
				+"'"+POut.Double(recurringCharge.ChargeAmt)+"',"
				+    POut.Long  (recurringCharge.UserNum)+","
				+    POut.Long  (recurringCharge.PayNum)+","
				+    POut.Long  (recurringCharge.CreditCardNum)+","
				+    DbHelper.ParamChar+"paramErrorMsg)";
			if(recurringCharge.ErrorMsg==null) {
				recurringCharge.ErrorMsg="";
			}
			OdSqlParameter paramErrorMsg=new OdSqlParameter("paramErrorMsg",OdDbType.Text,POut.StringParam(recurringCharge.ErrorMsg));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramErrorMsg);
			}
			else {
				recurringCharge.RecurringChargeNum=Db.NonQ(command,true,"RecurringChargeNum","recurringCharge",paramErrorMsg);
			}
			return recurringCharge.RecurringChargeNum;
		}

		///<summary>Inserts one RecurringCharge into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RecurringCharge recurringCharge) {
			return InsertNoCache(recurringCharge,false);
		}

		///<summary>Inserts one RecurringCharge into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RecurringCharge recurringCharge,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO recurringcharge (";
			if(!useExistingPK && isRandomKeys) {
				recurringCharge.RecurringChargeNum=ReplicationServers.GetKeyNoCache("recurringcharge","RecurringChargeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RecurringChargeNum,";
			}
			command+="PatNum,ClinicNum,DateTimeCharge,ChargeStatus,FamBal,PayPlanDue,TotalDue,RepeatAmt,ChargeAmt,UserNum,PayNum,CreditCardNum,ErrorMsg) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(recurringCharge.RecurringChargeNum)+",";
			}
			command+=
				     POut.Long  (recurringCharge.PatNum)+","
				+    POut.Long  (recurringCharge.ClinicNum)+","
				+    POut.DateT (recurringCharge.DateTimeCharge)+","
				+    POut.Int   ((int)recurringCharge.ChargeStatus)+","
				+"'"+POut.Double(recurringCharge.FamBal)+"',"
				+"'"+POut.Double(recurringCharge.PayPlanDue)+"',"
				+"'"+POut.Double(recurringCharge.TotalDue)+"',"
				+"'"+POut.Double(recurringCharge.RepeatAmt)+"',"
				+"'"+POut.Double(recurringCharge.ChargeAmt)+"',"
				+    POut.Long  (recurringCharge.UserNum)+","
				+    POut.Long  (recurringCharge.PayNum)+","
				+    POut.Long  (recurringCharge.CreditCardNum)+","
				+    DbHelper.ParamChar+"paramErrorMsg)";
			if(recurringCharge.ErrorMsg==null) {
				recurringCharge.ErrorMsg="";
			}
			OdSqlParameter paramErrorMsg=new OdSqlParameter("paramErrorMsg",OdDbType.Text,POut.StringParam(recurringCharge.ErrorMsg));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramErrorMsg);
			}
			else {
				recurringCharge.RecurringChargeNum=Db.NonQ(command,true,"RecurringChargeNum","recurringCharge",paramErrorMsg);
			}
			return recurringCharge.RecurringChargeNum;
		}

		///<summary>Updates one RecurringCharge in the database.</summary>
		public static void Update(RecurringCharge recurringCharge) {
			string command="UPDATE recurringcharge SET "
				+"PatNum            =  "+POut.Long  (recurringCharge.PatNum)+", "
				+"ClinicNum         =  "+POut.Long  (recurringCharge.ClinicNum)+", "
				+"DateTimeCharge    =  "+POut.DateT (recurringCharge.DateTimeCharge)+", "
				+"ChargeStatus      =  "+POut.Int   ((int)recurringCharge.ChargeStatus)+", "
				+"FamBal            = '"+POut.Double(recurringCharge.FamBal)+"', "
				+"PayPlanDue        = '"+POut.Double(recurringCharge.PayPlanDue)+"', "
				+"TotalDue          = '"+POut.Double(recurringCharge.TotalDue)+"', "
				+"RepeatAmt         = '"+POut.Double(recurringCharge.RepeatAmt)+"', "
				+"ChargeAmt         = '"+POut.Double(recurringCharge.ChargeAmt)+"', "
				+"UserNum           =  "+POut.Long  (recurringCharge.UserNum)+", "
				+"PayNum            =  "+POut.Long  (recurringCharge.PayNum)+", "
				+"CreditCardNum     =  "+POut.Long  (recurringCharge.CreditCardNum)+", "
				+"ErrorMsg          =  "+DbHelper.ParamChar+"paramErrorMsg "
				+"WHERE RecurringChargeNum = "+POut.Long(recurringCharge.RecurringChargeNum);
			if(recurringCharge.ErrorMsg==null) {
				recurringCharge.ErrorMsg="";
			}
			OdSqlParameter paramErrorMsg=new OdSqlParameter("paramErrorMsg",OdDbType.Text,POut.StringParam(recurringCharge.ErrorMsg));
			Db.NonQ(command,paramErrorMsg);
		}

		///<summary>Updates one RecurringCharge in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RecurringCharge recurringCharge,RecurringCharge oldRecurringCharge) {
			string command="";
			if(recurringCharge.PatNum != oldRecurringCharge.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(recurringCharge.PatNum)+"";
			}
			if(recurringCharge.ClinicNum != oldRecurringCharge.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(recurringCharge.ClinicNum)+"";
			}
			if(recurringCharge.DateTimeCharge != oldRecurringCharge.DateTimeCharge) {
				if(command!="") { command+=",";}
				command+="DateTimeCharge = "+POut.DateT(recurringCharge.DateTimeCharge)+"";
			}
			if(recurringCharge.ChargeStatus != oldRecurringCharge.ChargeStatus) {
				if(command!="") { command+=",";}
				command+="ChargeStatus = "+POut.Int   ((int)recurringCharge.ChargeStatus)+"";
			}
			if(recurringCharge.FamBal != oldRecurringCharge.FamBal) {
				if(command!="") { command+=",";}
				command+="FamBal = '"+POut.Double(recurringCharge.FamBal)+"'";
			}
			if(recurringCharge.PayPlanDue != oldRecurringCharge.PayPlanDue) {
				if(command!="") { command+=",";}
				command+="PayPlanDue = '"+POut.Double(recurringCharge.PayPlanDue)+"'";
			}
			if(recurringCharge.TotalDue != oldRecurringCharge.TotalDue) {
				if(command!="") { command+=",";}
				command+="TotalDue = '"+POut.Double(recurringCharge.TotalDue)+"'";
			}
			if(recurringCharge.RepeatAmt != oldRecurringCharge.RepeatAmt) {
				if(command!="") { command+=",";}
				command+="RepeatAmt = '"+POut.Double(recurringCharge.RepeatAmt)+"'";
			}
			if(recurringCharge.ChargeAmt != oldRecurringCharge.ChargeAmt) {
				if(command!="") { command+=",";}
				command+="ChargeAmt = '"+POut.Double(recurringCharge.ChargeAmt)+"'";
			}
			if(recurringCharge.UserNum != oldRecurringCharge.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(recurringCharge.UserNum)+"";
			}
			if(recurringCharge.PayNum != oldRecurringCharge.PayNum) {
				if(command!="") { command+=",";}
				command+="PayNum = "+POut.Long(recurringCharge.PayNum)+"";
			}
			if(recurringCharge.CreditCardNum != oldRecurringCharge.CreditCardNum) {
				if(command!="") { command+=",";}
				command+="CreditCardNum = "+POut.Long(recurringCharge.CreditCardNum)+"";
			}
			if(recurringCharge.ErrorMsg != oldRecurringCharge.ErrorMsg) {
				if(command!="") { command+=",";}
				command+="ErrorMsg = "+DbHelper.ParamChar+"paramErrorMsg";
			}
			if(command=="") {
				return false;
			}
			if(recurringCharge.ErrorMsg==null) {
				recurringCharge.ErrorMsg="";
			}
			OdSqlParameter paramErrorMsg=new OdSqlParameter("paramErrorMsg",OdDbType.Text,POut.StringParam(recurringCharge.ErrorMsg));
			command="UPDATE recurringcharge SET "+command
				+" WHERE RecurringChargeNum = "+POut.Long(recurringCharge.RecurringChargeNum);
			Db.NonQ(command,paramErrorMsg);
			return true;
		}

		///<summary>Returns true if Update(RecurringCharge,RecurringCharge) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(RecurringCharge recurringCharge,RecurringCharge oldRecurringCharge) {
			if(recurringCharge.PatNum != oldRecurringCharge.PatNum) {
				return true;
			}
			if(recurringCharge.ClinicNum != oldRecurringCharge.ClinicNum) {
				return true;
			}
			if(recurringCharge.DateTimeCharge != oldRecurringCharge.DateTimeCharge) {
				return true;
			}
			if(recurringCharge.ChargeStatus != oldRecurringCharge.ChargeStatus) {
				return true;
			}
			if(recurringCharge.FamBal != oldRecurringCharge.FamBal) {
				return true;
			}
			if(recurringCharge.PayPlanDue != oldRecurringCharge.PayPlanDue) {
				return true;
			}
			if(recurringCharge.TotalDue != oldRecurringCharge.TotalDue) {
				return true;
			}
			if(recurringCharge.RepeatAmt != oldRecurringCharge.RepeatAmt) {
				return true;
			}
			if(recurringCharge.ChargeAmt != oldRecurringCharge.ChargeAmt) {
				return true;
			}
			if(recurringCharge.UserNum != oldRecurringCharge.UserNum) {
				return true;
			}
			if(recurringCharge.PayNum != oldRecurringCharge.PayNum) {
				return true;
			}
			if(recurringCharge.CreditCardNum != oldRecurringCharge.CreditCardNum) {
				return true;
			}
			if(recurringCharge.ErrorMsg != oldRecurringCharge.ErrorMsg) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one RecurringCharge from the database.</summary>
		public static void Delete(long recurringChargeNum) {
			string command="DELETE FROM recurringcharge "
				+"WHERE RecurringChargeNum = "+POut.Long(recurringChargeNum);
			Db.NonQ(command);
		}

	}
}