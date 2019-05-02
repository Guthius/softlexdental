//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class DunningCrud {
		///<summary>Gets one Dunning object from the database using the primary key.  Returns null if not found.</summary>
		public static Dunning SelectOne(long dunningNum) {
			string command="SELECT * FROM dunning "
				+"WHERE DunningNum = "+POut.Long(dunningNum);
			List<Dunning> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Dunning object from the database using a query.</summary>
		public static Dunning SelectOne(string command) {
			
			List<Dunning> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Dunning objects from the database using a query.</summary>
		public static List<Dunning> SelectMany(string command) {
			
			List<Dunning> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Dunning> TableToList(DataTable table) {
			List<Dunning> retVal=new List<Dunning>();
			Dunning dunning;
			foreach(DataRow row in table.Rows) {
				dunning=new Dunning();
				dunning.DunningNum   = PIn.Long  (row["DunningNum"].ToString());
				dunning.DunMessage   = PIn.String(row["DunMessage"].ToString());
				dunning.BillingType  = PIn.Long  (row["BillingType"].ToString());
				dunning.AgeAccount   = PIn.Byte  (row["AgeAccount"].ToString());
				dunning.InsIsPending = (OpenDentBusiness.YN)PIn.Int(row["InsIsPending"].ToString());
				dunning.MessageBold  = PIn.String(row["MessageBold"].ToString());
				dunning.EmailSubject = PIn.String(row["EmailSubject"].ToString());
				dunning.EmailBody    = PIn.String(row["EmailBody"].ToString());
				dunning.DaysInAdvance= PIn.Int   (row["DaysInAdvance"].ToString());
				dunning.ClinicNum    = PIn.Long  (row["ClinicNum"].ToString());
				dunning.IsSuperFamily= PIn.Bool  (row["IsSuperFamily"].ToString());
				retVal.Add(dunning);
			}
			return retVal;
		}

		///<summary>Converts a list of Dunning into a DataTable.</summary>
		public static DataTable ListToTable(List<Dunning> listDunnings,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Dunning";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("DunningNum");
			table.Columns.Add("DunMessage");
			table.Columns.Add("BillingType");
			table.Columns.Add("AgeAccount");
			table.Columns.Add("InsIsPending");
			table.Columns.Add("MessageBold");
			table.Columns.Add("EmailSubject");
			table.Columns.Add("EmailBody");
			table.Columns.Add("DaysInAdvance");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("IsSuperFamily");
			foreach(Dunning dunning in listDunnings) {
				table.Rows.Add(new object[] {
					POut.Long  (dunning.DunningNum),
					            dunning.DunMessage,
					POut.Long  (dunning.BillingType),
					POut.Byte  (dunning.AgeAccount),
					POut.Int   ((int)dunning.InsIsPending),
					            dunning.MessageBold,
					            dunning.EmailSubject,
					            dunning.EmailBody,
					POut.Int   (dunning.DaysInAdvance),
					POut.Long  (dunning.ClinicNum),
					POut.Bool  (dunning.IsSuperFamily),
				});
			}
			return table;
		}

		///<summary>Inserts one Dunning into the database.  Returns the new priKey.</summary>
		public static long Insert(Dunning dunning) {
			return Insert(dunning,false);
		}

		///<summary>Inserts one Dunning into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Dunning dunning,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				dunning.DunningNum=ReplicationServers.GetKey("dunning","DunningNum");
			}
			string command="INSERT INTO dunning (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="DunningNum,";
			}
			command+="DunMessage,BillingType,AgeAccount,InsIsPending,MessageBold,EmailSubject,EmailBody,DaysInAdvance,ClinicNum,IsSuperFamily) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(dunning.DunningNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDunMessage,"
				+    POut.Long  (dunning.BillingType)+","
				+    POut.Byte  (dunning.AgeAccount)+","
				+    POut.Int   ((int)dunning.InsIsPending)+","
				+    DbHelper.ParamChar+"paramMessageBold,"
				+"'"+POut.String(dunning.EmailSubject)+"',"
				+    DbHelper.ParamChar+"paramEmailBody,"
				+    POut.Int   (dunning.DaysInAdvance)+","
				+    POut.Long  (dunning.ClinicNum)+","
				+    POut.Bool  (dunning.IsSuperFamily)+")";
			if(dunning.DunMessage==null) {
				dunning.DunMessage="";
			}
			OdSqlParameter paramDunMessage=new OdSqlParameter("paramDunMessage",OdDbType.Text,POut.StringParam(dunning.DunMessage));
			if(dunning.MessageBold==null) {
				dunning.MessageBold="";
			}
			OdSqlParameter paramMessageBold=new OdSqlParameter("paramMessageBold",OdDbType.Text,POut.StringParam(dunning.MessageBold));
			if(dunning.EmailBody==null) {
				dunning.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,POut.StringParam(dunning.EmailBody));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramDunMessage,paramMessageBold,paramEmailBody);
			}
			else {
				dunning.DunningNum=Db.NonQ(command,true,"DunningNum","dunning",paramDunMessage,paramMessageBold,paramEmailBody);
			}
			return dunning.DunningNum;
		}

		///<summary>Inserts one Dunning into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Dunning dunning) {
			return InsertNoCache(dunning,false);
		}

		///<summary>Inserts one Dunning into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Dunning dunning,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO dunning (";
			if(!useExistingPK && isRandomKeys) {
				dunning.DunningNum=ReplicationServers.GetKeyNoCache("dunning","DunningNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="DunningNum,";
			}
			command+="DunMessage,BillingType,AgeAccount,InsIsPending,MessageBold,EmailSubject,EmailBody,DaysInAdvance,ClinicNum,IsSuperFamily) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(dunning.DunningNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDunMessage,"
				+    POut.Long  (dunning.BillingType)+","
				+    POut.Byte  (dunning.AgeAccount)+","
				+    POut.Int   ((int)dunning.InsIsPending)+","
				+    DbHelper.ParamChar+"paramMessageBold,"
				+"'"+POut.String(dunning.EmailSubject)+"',"
				+    DbHelper.ParamChar+"paramEmailBody,"
				+    POut.Int   (dunning.DaysInAdvance)+","
				+    POut.Long  (dunning.ClinicNum)+","
				+    POut.Bool  (dunning.IsSuperFamily)+")";
			if(dunning.DunMessage==null) {
				dunning.DunMessage="";
			}
			OdSqlParameter paramDunMessage=new OdSqlParameter("paramDunMessage",OdDbType.Text,POut.StringParam(dunning.DunMessage));
			if(dunning.MessageBold==null) {
				dunning.MessageBold="";
			}
			OdSqlParameter paramMessageBold=new OdSqlParameter("paramMessageBold",OdDbType.Text,POut.StringParam(dunning.MessageBold));
			if(dunning.EmailBody==null) {
				dunning.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,POut.StringParam(dunning.EmailBody));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDunMessage,paramMessageBold,paramEmailBody);
			}
			else {
				dunning.DunningNum=Db.NonQ(command,true,"DunningNum","dunning",paramDunMessage,paramMessageBold,paramEmailBody);
			}
			return dunning.DunningNum;
		}

		///<summary>Updates one Dunning in the database.</summary>
		public static void Update(Dunning dunning) {
			string command="UPDATE dunning SET "
				+"DunMessage   =  "+DbHelper.ParamChar+"paramDunMessage, "
				+"BillingType  =  "+POut.Long  (dunning.BillingType)+", "
				+"AgeAccount   =  "+POut.Byte  (dunning.AgeAccount)+", "
				+"InsIsPending =  "+POut.Int   ((int)dunning.InsIsPending)+", "
				+"MessageBold  =  "+DbHelper.ParamChar+"paramMessageBold, "
				+"EmailSubject = '"+POut.String(dunning.EmailSubject)+"', "
				+"EmailBody    =  "+DbHelper.ParamChar+"paramEmailBody, "
				+"DaysInAdvance=  "+POut.Int   (dunning.DaysInAdvance)+", "
				+"ClinicNum    =  "+POut.Long  (dunning.ClinicNum)+", "
				+"IsSuperFamily=  "+POut.Bool  (dunning.IsSuperFamily)+" "
				+"WHERE DunningNum = "+POut.Long(dunning.DunningNum);
			if(dunning.DunMessage==null) {
				dunning.DunMessage="";
			}
			OdSqlParameter paramDunMessage=new OdSqlParameter("paramDunMessage",OdDbType.Text,POut.StringParam(dunning.DunMessage));
			if(dunning.MessageBold==null) {
				dunning.MessageBold="";
			}
			OdSqlParameter paramMessageBold=new OdSqlParameter("paramMessageBold",OdDbType.Text,POut.StringParam(dunning.MessageBold));
			if(dunning.EmailBody==null) {
				dunning.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,POut.StringParam(dunning.EmailBody));
			Db.NonQ(command,paramDunMessage,paramMessageBold,paramEmailBody);
		}

		///<summary>Updates one Dunning in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Dunning dunning,Dunning oldDunning) {
			string command="";
			if(dunning.DunMessage != oldDunning.DunMessage) {
				if(command!="") { command+=",";}
				command+="DunMessage = "+DbHelper.ParamChar+"paramDunMessage";
			}
			if(dunning.BillingType != oldDunning.BillingType) {
				if(command!="") { command+=",";}
				command+="BillingType = "+POut.Long(dunning.BillingType)+"";
			}
			if(dunning.AgeAccount != oldDunning.AgeAccount) {
				if(command!="") { command+=",";}
				command+="AgeAccount = "+POut.Byte(dunning.AgeAccount)+"";
			}
			if(dunning.InsIsPending != oldDunning.InsIsPending) {
				if(command!="") { command+=",";}
				command+="InsIsPending = "+POut.Int   ((int)dunning.InsIsPending)+"";
			}
			if(dunning.MessageBold != oldDunning.MessageBold) {
				if(command!="") { command+=",";}
				command+="MessageBold = "+DbHelper.ParamChar+"paramMessageBold";
			}
			if(dunning.EmailSubject != oldDunning.EmailSubject) {
				if(command!="") { command+=",";}
				command+="EmailSubject = '"+POut.String(dunning.EmailSubject)+"'";
			}
			if(dunning.EmailBody != oldDunning.EmailBody) {
				if(command!="") { command+=",";}
				command+="EmailBody = "+DbHelper.ParamChar+"paramEmailBody";
			}
			if(dunning.DaysInAdvance != oldDunning.DaysInAdvance) {
				if(command!="") { command+=",";}
				command+="DaysInAdvance = "+POut.Int(dunning.DaysInAdvance)+"";
			}
			if(dunning.ClinicNum != oldDunning.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(dunning.ClinicNum)+"";
			}
			if(dunning.IsSuperFamily != oldDunning.IsSuperFamily) {
				if(command!="") { command+=",";}
				command+="IsSuperFamily = "+POut.Bool(dunning.IsSuperFamily)+"";
			}
			if(command=="") {
				return false;
			}
			if(dunning.DunMessage==null) {
				dunning.DunMessage="";
			}
			OdSqlParameter paramDunMessage=new OdSqlParameter("paramDunMessage",OdDbType.Text,POut.StringParam(dunning.DunMessage));
			if(dunning.MessageBold==null) {
				dunning.MessageBold="";
			}
			OdSqlParameter paramMessageBold=new OdSqlParameter("paramMessageBold",OdDbType.Text,POut.StringParam(dunning.MessageBold));
			if(dunning.EmailBody==null) {
				dunning.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,POut.StringParam(dunning.EmailBody));
			command="UPDATE dunning SET "+command
				+" WHERE DunningNum = "+POut.Long(dunning.DunningNum);
			Db.NonQ(command,paramDunMessage,paramMessageBold,paramEmailBody);
			return true;
		}

		///<summary>Returns true if Update(Dunning,Dunning) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Dunning dunning,Dunning oldDunning) {
			if(dunning.DunMessage != oldDunning.DunMessage) {
				return true;
			}
			if(dunning.BillingType != oldDunning.BillingType) {
				return true;
			}
			if(dunning.AgeAccount != oldDunning.AgeAccount) {
				return true;
			}
			if(dunning.InsIsPending != oldDunning.InsIsPending) {
				return true;
			}
			if(dunning.MessageBold != oldDunning.MessageBold) {
				return true;
			}
			if(dunning.EmailSubject != oldDunning.EmailSubject) {
				return true;
			}
			if(dunning.EmailBody != oldDunning.EmailBody) {
				return true;
			}
			if(dunning.DaysInAdvance != oldDunning.DaysInAdvance) {
				return true;
			}
			if(dunning.ClinicNum != oldDunning.ClinicNum) {
				return true;
			}
			if(dunning.IsSuperFamily != oldDunning.IsSuperFamily) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Dunning from the database.</summary>
		public static void Delete(long dunningNum) {
			string command="DELETE FROM dunning "
				+"WHERE DunningNum = "+POut.Long(dunningNum);
			Db.NonQ(command);
		}

	}
}