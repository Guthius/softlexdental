//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class AutomationCrud {
		///<summary>Gets one Automation object from the database using the primary key.  Returns null if not found.</summary>
		public static Automation SelectOne(long automationNum) {
			string command="SELECT * FROM automation "
				+"WHERE AutomationNum = "+POut.Long(automationNum);
			List<Automation> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Automation object from the database using a query.</summary>
		public static Automation SelectOne(string command) {
			
			List<Automation> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Automation objects from the database using a query.</summary>
		public static List<Automation> SelectMany(string command) {
			
			List<Automation> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Automation> TableToList(DataTable table) {
			List<Automation> retVal=new List<Automation>();
			Automation automation;
			foreach(DataRow row in table.Rows) {
				automation=new Automation();
				automation.AutomationNum     = PIn.Long  (row["AutomationNum"].ToString());
				automation.Description       = PIn.String(row["Description"].ToString());
				automation.Autotrigger       = (OpenDentBusiness.AutomationTrigger)PIn.Int(row["Autotrigger"].ToString());
				automation.ProcCodes         = PIn.String(row["ProcCodes"].ToString());
				automation.AutoAction        = (OpenDentBusiness.AutomationAction)PIn.Int(row["AutoAction"].ToString());
				automation.SheetDefNum       = PIn.Long  (row["SheetDefNum"].ToString());
				automation.CommType          = PIn.Long  (row["CommType"].ToString());
				automation.MessageContent    = PIn.String(row["MessageContent"].ToString());
				automation.AptStatus         = (OpenDentBusiness.ApptStatus)PIn.Int(row["AptStatus"].ToString());
				automation.AppointmentTypeNum= PIn.Long  (row["AppointmentTypeNum"].ToString());
				automation.PatStatus         = (OpenDentBusiness.PatientStatus)PIn.Int(row["PatStatus"].ToString());
				retVal.Add(automation);
			}
			return retVal;
		}

		///<summary>Converts a list of Automation into a DataTable.</summary>
		public static DataTable ListToTable(List<Automation> listAutomations,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Automation";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AutomationNum");
			table.Columns.Add("Description");
			table.Columns.Add("Autotrigger");
			table.Columns.Add("ProcCodes");
			table.Columns.Add("AutoAction");
			table.Columns.Add("SheetDefNum");
			table.Columns.Add("CommType");
			table.Columns.Add("MessageContent");
			table.Columns.Add("AptStatus");
			table.Columns.Add("AppointmentTypeNum");
			table.Columns.Add("PatStatus");
			foreach(Automation automation in listAutomations) {
				table.Rows.Add(new object[] {
					POut.Long  (automation.AutomationNum),
					            automation.Description,
					POut.Int   ((int)automation.Autotrigger),
					            automation.ProcCodes,
					POut.Int   ((int)automation.AutoAction),
					POut.Long  (automation.SheetDefNum),
					POut.Long  (automation.CommType),
					            automation.MessageContent,
					POut.Int   ((int)automation.AptStatus),
					POut.Long  (automation.AppointmentTypeNum),
					POut.Int   ((int)automation.PatStatus),
				});
			}
			return table;
		}

		///<summary>Inserts one Automation into the database.  Returns the new priKey.</summary>
		public static long Insert(Automation automation) {
			return Insert(automation,false);
		}

		///<summary>Inserts one Automation into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Automation automation,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				automation.AutomationNum=ReplicationServers.GetKey("automation","AutomationNum");
			}
			string command="INSERT INTO automation (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="AutomationNum,";
			}
			command+="Description,Autotrigger,ProcCodes,AutoAction,SheetDefNum,CommType,MessageContent,AptStatus,AppointmentTypeNum,PatStatus) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(automation.AutomationNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDescription,"
				+    POut.Int   ((int)automation.Autotrigger)+","
				+    DbHelper.ParamChar+"paramProcCodes,"
				+    POut.Int   ((int)automation.AutoAction)+","
				+    POut.Long  (automation.SheetDefNum)+","
				+    POut.Long  (automation.CommType)+","
				+    DbHelper.ParamChar+"paramMessageContent,"
				+    POut.Int   ((int)automation.AptStatus)+","
				+    POut.Long  (automation.AppointmentTypeNum)+","
				+    POut.Int   ((int)automation.PatStatus)+")";
			if(automation.Description==null) {
				automation.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(automation.Description));
			if(automation.ProcCodes==null) {
				automation.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(automation.ProcCodes));
			if(automation.MessageContent==null) {
				automation.MessageContent="";
			}
			OdSqlParameter paramMessageContent=new OdSqlParameter("paramMessageContent",OdDbType.Text,POut.StringParam(automation.MessageContent));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramDescription,paramProcCodes,paramMessageContent);
			}
			else {
				automation.AutomationNum=Db.NonQ(command,true,"AutomationNum","automation",paramDescription,paramProcCodes,paramMessageContent);
			}
			return automation.AutomationNum;
		}

		///<summary>Inserts one Automation into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Automation automation) {
			return InsertNoCache(automation,false);
		}

		///<summary>Inserts one Automation into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Automation automation,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO automation (";
			if(!useExistingPK && isRandomKeys) {
				automation.AutomationNum=ReplicationServers.GetKeyNoCache("automation","AutomationNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AutomationNum,";
			}
			command+="Description,Autotrigger,ProcCodes,AutoAction,SheetDefNum,CommType,MessageContent,AptStatus,AppointmentTypeNum,PatStatus) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(automation.AutomationNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDescription,"
				+    POut.Int   ((int)automation.Autotrigger)+","
				+    DbHelper.ParamChar+"paramProcCodes,"
				+    POut.Int   ((int)automation.AutoAction)+","
				+    POut.Long  (automation.SheetDefNum)+","
				+    POut.Long  (automation.CommType)+","
				+    DbHelper.ParamChar+"paramMessageContent,"
				+    POut.Int   ((int)automation.AptStatus)+","
				+    POut.Long  (automation.AppointmentTypeNum)+","
				+    POut.Int   ((int)automation.PatStatus)+")";
			if(automation.Description==null) {
				automation.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(automation.Description));
			if(automation.ProcCodes==null) {
				automation.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(automation.ProcCodes));
			if(automation.MessageContent==null) {
				automation.MessageContent="";
			}
			OdSqlParameter paramMessageContent=new OdSqlParameter("paramMessageContent",OdDbType.Text,POut.StringParam(automation.MessageContent));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription,paramProcCodes,paramMessageContent);
			}
			else {
				automation.AutomationNum=Db.NonQ(command,true,"AutomationNum","automation",paramDescription,paramProcCodes,paramMessageContent);
			}
			return automation.AutomationNum;
		}

		///<summary>Updates one Automation in the database.</summary>
		public static void Update(Automation automation) {
			string command="UPDATE automation SET "
				+"Description       =  "+DbHelper.ParamChar+"paramDescription, "
				+"Autotrigger       =  "+POut.Int   ((int)automation.Autotrigger)+", "
				+"ProcCodes         =  "+DbHelper.ParamChar+"paramProcCodes, "
				+"AutoAction        =  "+POut.Int   ((int)automation.AutoAction)+", "
				+"SheetDefNum       =  "+POut.Long  (automation.SheetDefNum)+", "
				+"CommType          =  "+POut.Long  (automation.CommType)+", "
				+"MessageContent    =  "+DbHelper.ParamChar+"paramMessageContent, "
				+"AptStatus         =  "+POut.Int   ((int)automation.AptStatus)+", "
				+"AppointmentTypeNum=  "+POut.Long  (automation.AppointmentTypeNum)+", "
				+"PatStatus         =  "+POut.Int   ((int)automation.PatStatus)+" "
				+"WHERE AutomationNum = "+POut.Long(automation.AutomationNum);
			if(automation.Description==null) {
				automation.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(automation.Description));
			if(automation.ProcCodes==null) {
				automation.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(automation.ProcCodes));
			if(automation.MessageContent==null) {
				automation.MessageContent="";
			}
			OdSqlParameter paramMessageContent=new OdSqlParameter("paramMessageContent",OdDbType.Text,POut.StringParam(automation.MessageContent));
			Db.NonQ(command,paramDescription,paramProcCodes,paramMessageContent);
		}

		///<summary>Updates one Automation in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Automation automation,Automation oldAutomation) {
			string command="";
			if(automation.Description != oldAutomation.Description) {
				if(command!="") { command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(automation.Autotrigger != oldAutomation.Autotrigger) {
				if(command!="") { command+=",";}
				command+="Autotrigger = "+POut.Int   ((int)automation.Autotrigger)+"";
			}
			if(automation.ProcCodes != oldAutomation.ProcCodes) {
				if(command!="") { command+=",";}
				command+="ProcCodes = "+DbHelper.ParamChar+"paramProcCodes";
			}
			if(automation.AutoAction != oldAutomation.AutoAction) {
				if(command!="") { command+=",";}
				command+="AutoAction = "+POut.Int   ((int)automation.AutoAction)+"";
			}
			if(automation.SheetDefNum != oldAutomation.SheetDefNum) {
				if(command!="") { command+=",";}
				command+="SheetDefNum = "+POut.Long(automation.SheetDefNum)+"";
			}
			if(automation.CommType != oldAutomation.CommType) {
				if(command!="") { command+=",";}
				command+="CommType = "+POut.Long(automation.CommType)+"";
			}
			if(automation.MessageContent != oldAutomation.MessageContent) {
				if(command!="") { command+=",";}
				command+="MessageContent = "+DbHelper.ParamChar+"paramMessageContent";
			}
			if(automation.AptStatus != oldAutomation.AptStatus) {
				if(command!="") { command+=",";}
				command+="AptStatus = "+POut.Int   ((int)automation.AptStatus)+"";
			}
			if(automation.AppointmentTypeNum != oldAutomation.AppointmentTypeNum) {
				if(command!="") { command+=",";}
				command+="AppointmentTypeNum = "+POut.Long(automation.AppointmentTypeNum)+"";
			}
			if(automation.PatStatus != oldAutomation.PatStatus) {
				if(command!="") { command+=",";}
				command+="PatStatus = "+POut.Int   ((int)automation.PatStatus)+"";
			}
			if(command=="") {
				return false;
			}
			if(automation.Description==null) {
				automation.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(automation.Description));
			if(automation.ProcCodes==null) {
				automation.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(automation.ProcCodes));
			if(automation.MessageContent==null) {
				automation.MessageContent="";
			}
			OdSqlParameter paramMessageContent=new OdSqlParameter("paramMessageContent",OdDbType.Text,POut.StringParam(automation.MessageContent));
			command="UPDATE automation SET "+command
				+" WHERE AutomationNum = "+POut.Long(automation.AutomationNum);
			Db.NonQ(command,paramDescription,paramProcCodes,paramMessageContent);
			return true;
		}

		///<summary>Returns true if Update(Automation,Automation) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Automation automation,Automation oldAutomation) {
			if(automation.Description != oldAutomation.Description) {
				return true;
			}
			if(automation.Autotrigger != oldAutomation.Autotrigger) {
				return true;
			}
			if(automation.ProcCodes != oldAutomation.ProcCodes) {
				return true;
			}
			if(automation.AutoAction != oldAutomation.AutoAction) {
				return true;
			}
			if(automation.SheetDefNum != oldAutomation.SheetDefNum) {
				return true;
			}
			if(automation.CommType != oldAutomation.CommType) {
				return true;
			}
			if(automation.MessageContent != oldAutomation.MessageContent) {
				return true;
			}
			if(automation.AptStatus != oldAutomation.AptStatus) {
				return true;
			}
			if(automation.AppointmentTypeNum != oldAutomation.AppointmentTypeNum) {
				return true;
			}
			if(automation.PatStatus != oldAutomation.PatStatus) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Automation from the database.</summary>
		public static void Delete(long automationNum) {
			string command="DELETE FROM automation "
				+"WHERE AutomationNum = "+POut.Long(automationNum);
			Db.NonQ(command);
		}

	}
}