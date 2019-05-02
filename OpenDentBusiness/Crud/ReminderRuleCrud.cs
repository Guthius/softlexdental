//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ReminderRuleCrud {
		///<summary>Gets one ReminderRule object from the database using the primary key.  Returns null if not found.</summary>
		public static ReminderRule SelectOne(long reminderRuleNum) {
			string command="SELECT * FROM reminderrule "
				+"WHERE ReminderRuleNum = "+POut.Long(reminderRuleNum);
			List<ReminderRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ReminderRule object from the database using a query.</summary>
		public static ReminderRule SelectOne(string command) {
			
			List<ReminderRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ReminderRule objects from the database using a query.</summary>
		public static List<ReminderRule> SelectMany(string command) {
			
			List<ReminderRule> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ReminderRule> TableToList(DataTable table) {
			List<ReminderRule> retVal=new List<ReminderRule>();
			ReminderRule reminderRule;
			foreach(DataRow row in table.Rows) {
				reminderRule=new ReminderRule();
				reminderRule.ReminderRuleNum  = PIn.Long  (row["ReminderRuleNum"].ToString());
				reminderRule.ReminderCriterion= (OpenDentBusiness.EhrCriterion)PIn.Int(row["ReminderCriterion"].ToString());
				reminderRule.CriterionFK      = PIn.Long  (row["CriterionFK"].ToString());
				reminderRule.CriterionValue   = PIn.String(row["CriterionValue"].ToString());
				reminderRule.Message          = PIn.String(row["Message"].ToString());
				retVal.Add(reminderRule);
			}
			return retVal;
		}

		///<summary>Converts a list of ReminderRule into a DataTable.</summary>
		public static DataTable ListToTable(List<ReminderRule> listReminderRules,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ReminderRule";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ReminderRuleNum");
			table.Columns.Add("ReminderCriterion");
			table.Columns.Add("CriterionFK");
			table.Columns.Add("CriterionValue");
			table.Columns.Add("Message");
			foreach(ReminderRule reminderRule in listReminderRules) {
				table.Rows.Add(new object[] {
					POut.Long  (reminderRule.ReminderRuleNum),
					POut.Int   ((int)reminderRule.ReminderCriterion),
					POut.Long  (reminderRule.CriterionFK),
					            reminderRule.CriterionValue,
					            reminderRule.Message,
				});
			}
			return table;
		}

		///<summary>Inserts one ReminderRule into the database.  Returns the new priKey.</summary>
		public static long Insert(ReminderRule reminderRule) {
			return Insert(reminderRule,false);
		}

		///<summary>Inserts one ReminderRule into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ReminderRule reminderRule,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				reminderRule.ReminderRuleNum=ReplicationServers.GetKey("reminderrule","ReminderRuleNum");
			}
			string command="INSERT INTO reminderrule (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ReminderRuleNum,";
			}
			command+="ReminderCriterion,CriterionFK,CriterionValue,Message) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(reminderRule.ReminderRuleNum)+",";
			}
			command+=
				     POut.Int   ((int)reminderRule.ReminderCriterion)+","
				+    POut.Long  (reminderRule.CriterionFK)+","
				+"'"+POut.String(reminderRule.CriterionValue)+"',"
				+"'"+POut.String(reminderRule.Message)+"')";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				reminderRule.ReminderRuleNum=Db.NonQ(command,true,"ReminderRuleNum","reminderRule");
			}
			return reminderRule.ReminderRuleNum;
		}

		///<summary>Inserts one ReminderRule into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReminderRule reminderRule) {
			return InsertNoCache(reminderRule,false);
		}

		///<summary>Inserts one ReminderRule into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReminderRule reminderRule,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO reminderrule (";
			if(!useExistingPK && isRandomKeys) {
				reminderRule.ReminderRuleNum=ReplicationServers.GetKeyNoCache("reminderrule","ReminderRuleNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ReminderRuleNum,";
			}
			command+="ReminderCriterion,CriterionFK,CriterionValue,Message) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(reminderRule.ReminderRuleNum)+",";
			}
			command+=
				     POut.Int   ((int)reminderRule.ReminderCriterion)+","
				+    POut.Long  (reminderRule.CriterionFK)+","
				+"'"+POut.String(reminderRule.CriterionValue)+"',"
				+"'"+POut.String(reminderRule.Message)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				reminderRule.ReminderRuleNum=Db.NonQ(command,true,"ReminderRuleNum","reminderRule");
			}
			return reminderRule.ReminderRuleNum;
		}

		///<summary>Updates one ReminderRule in the database.</summary>
		public static void Update(ReminderRule reminderRule) {
			string command="UPDATE reminderrule SET "
				+"ReminderCriterion=  "+POut.Int   ((int)reminderRule.ReminderCriterion)+", "
				+"CriterionFK      =  "+POut.Long  (reminderRule.CriterionFK)+", "
				+"CriterionValue   = '"+POut.String(reminderRule.CriterionValue)+"', "
				+"Message          = '"+POut.String(reminderRule.Message)+"' "
				+"WHERE ReminderRuleNum = "+POut.Long(reminderRule.ReminderRuleNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ReminderRule in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ReminderRule reminderRule,ReminderRule oldReminderRule) {
			string command="";
			if(reminderRule.ReminderCriterion != oldReminderRule.ReminderCriterion) {
				if(command!="") { command+=",";}
				command+="ReminderCriterion = "+POut.Int   ((int)reminderRule.ReminderCriterion)+"";
			}
			if(reminderRule.CriterionFK != oldReminderRule.CriterionFK) {
				if(command!="") { command+=",";}
				command+="CriterionFK = "+POut.Long(reminderRule.CriterionFK)+"";
			}
			if(reminderRule.CriterionValue != oldReminderRule.CriterionValue) {
				if(command!="") { command+=",";}
				command+="CriterionValue = '"+POut.String(reminderRule.CriterionValue)+"'";
			}
			if(reminderRule.Message != oldReminderRule.Message) {
				if(command!="") { command+=",";}
				command+="Message = '"+POut.String(reminderRule.Message)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE reminderrule SET "+command
				+" WHERE ReminderRuleNum = "+POut.Long(reminderRule.ReminderRuleNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ReminderRule,ReminderRule) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ReminderRule reminderRule,ReminderRule oldReminderRule) {
			if(reminderRule.ReminderCriterion != oldReminderRule.ReminderCriterion) {
				return true;
			}
			if(reminderRule.CriterionFK != oldReminderRule.CriterionFK) {
				return true;
			}
			if(reminderRule.CriterionValue != oldReminderRule.CriterionValue) {
				return true;
			}
			if(reminderRule.Message != oldReminderRule.Message) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ReminderRule from the database.</summary>
		public static void Delete(long reminderRuleNum) {
			string command="DELETE FROM reminderrule "
				+"WHERE ReminderRuleNum = "+POut.Long(reminderRuleNum);
			Db.NonQ(command);
		}

	}
}