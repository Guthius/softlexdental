//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LabResultCrud {
		///<summary>Gets one LabResult object from the database using the primary key.  Returns null if not found.</summary>
		public static LabResult SelectOne(long labResultNum) {
			string command="SELECT * FROM labresult "
				+"WHERE LabResultNum = "+POut.Long(labResultNum);
			List<LabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LabResult object from the database using a query.</summary>
		public static LabResult SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LabResult objects from the database using a query.</summary>
		public static List<LabResult> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabResult> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<LabResult> TableToList(DataTable table) {
			List<LabResult> retVal=new List<LabResult>();
			LabResult labResult;
			foreach(DataRow row in table.Rows) {
				labResult=new LabResult();
				labResult.LabResultNum= PIn.Long  (row["LabResultNum"].ToString());
				labResult.LabPanelNum = PIn.Long  (row["LabPanelNum"].ToString());
				labResult.DateTimeTest= PIn.DateT (row["DateTimeTest"].ToString());
				labResult.TestName    = PIn.String(row["TestName"].ToString());
				labResult.DateTStamp  = PIn.DateT (row["DateTStamp"].ToString());
				labResult.TestID      = PIn.String(row["TestID"].ToString());
				labResult.ObsValue    = PIn.String(row["ObsValue"].ToString());
				labResult.ObsUnits    = PIn.String(row["ObsUnits"].ToString());
				labResult.ObsRange    = PIn.String(row["ObsRange"].ToString());
				labResult.AbnormalFlag= (OpenDentBusiness.LabAbnormalFlag)PIn.Int(row["AbnormalFlag"].ToString());
				retVal.Add(labResult);
			}
			return retVal;
		}

		///<summary>Converts a list of LabResult into a DataTable.</summary>
		public static DataTable ListToTable(List<LabResult> listLabResults,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="LabResult";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LabResultNum");
			table.Columns.Add("LabPanelNum");
			table.Columns.Add("DateTimeTest");
			table.Columns.Add("TestName");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("TestID");
			table.Columns.Add("ObsValue");
			table.Columns.Add("ObsUnits");
			table.Columns.Add("ObsRange");
			table.Columns.Add("AbnormalFlag");
			foreach(LabResult labResult in listLabResults) {
				table.Rows.Add(new object[] {
					POut.Long  (labResult.LabResultNum),
					POut.Long  (labResult.LabPanelNum),
					POut.DateT (labResult.DateTimeTest,false),
					            labResult.TestName,
					POut.DateT (labResult.DateTStamp,false),
					            labResult.TestID,
					            labResult.ObsValue,
					            labResult.ObsUnits,
					            labResult.ObsRange,
					POut.Int   ((int)labResult.AbnormalFlag),
				});
			}
			return table;
		}

		///<summary>Inserts one LabResult into the database.  Returns the new priKey.</summary>
		public static long Insert(LabResult labResult) {
			return Insert(labResult,false);
		}

		///<summary>Inserts one LabResult into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(LabResult labResult,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				labResult.LabResultNum=ReplicationServers.GetKey("labresult","LabResultNum");
			}
			string command="INSERT INTO labresult (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LabResultNum,";
			}
			command+="LabPanelNum,DateTimeTest,TestName,TestID,ObsValue,ObsUnits,ObsRange,AbnormalFlag) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(labResult.LabResultNum)+",";
			}
			command+=
				     POut.Long  (labResult.LabPanelNum)+","
				+    POut.DateT (labResult.DateTimeTest)+","
				+"'"+POut.String(labResult.TestName)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labResult.TestID)+"',"
				+"'"+POut.String(labResult.ObsValue)+"',"
				+"'"+POut.String(labResult.ObsUnits)+"',"
				+"'"+POut.String(labResult.ObsRange)+"',"
				+    POut.Int   ((int)labResult.AbnormalFlag)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				labResult.LabResultNum=Db.NonQ(command,true,"LabResultNum","labResult");
			}
			return labResult.LabResultNum;
		}

		///<summary>Inserts one LabResult into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabResult labResult) {
			return InsertNoCache(labResult,false);
		}

		///<summary>Inserts one LabResult into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(LabResult labResult,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO labresult (";
			if(!useExistingPK && isRandomKeys) {
				labResult.LabResultNum=ReplicationServers.GetKeyNoCache("labresult","LabResultNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="LabResultNum,";
			}
			command+="LabPanelNum,DateTimeTest,TestName,TestID,ObsValue,ObsUnits,ObsRange,AbnormalFlag) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(labResult.LabResultNum)+",";
			}
			command+=
				     POut.Long  (labResult.LabPanelNum)+","
				+    POut.DateT (labResult.DateTimeTest)+","
				+"'"+POut.String(labResult.TestName)+"',"
				//DateTStamp can only be set by MySQL
				+"'"+POut.String(labResult.TestID)+"',"
				+"'"+POut.String(labResult.ObsValue)+"',"
				+"'"+POut.String(labResult.ObsUnits)+"',"
				+"'"+POut.String(labResult.ObsRange)+"',"
				+    POut.Int   ((int)labResult.AbnormalFlag)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				labResult.LabResultNum=Db.NonQ(command,true,"LabResultNum","labResult");
			}
			return labResult.LabResultNum;
		}

		///<summary>Updates one LabResult in the database.</summary>
		public static void Update(LabResult labResult) {
			string command="UPDATE labresult SET "
				+"LabPanelNum =  "+POut.Long  (labResult.LabPanelNum)+", "
				+"DateTimeTest=  "+POut.DateT (labResult.DateTimeTest)+", "
				+"TestName    = '"+POut.String(labResult.TestName)+"', "
				//DateTStamp can only be set by MySQL
				+"TestID      = '"+POut.String(labResult.TestID)+"', "
				+"ObsValue    = '"+POut.String(labResult.ObsValue)+"', "
				+"ObsUnits    = '"+POut.String(labResult.ObsUnits)+"', "
				+"ObsRange    = '"+POut.String(labResult.ObsRange)+"', "
				+"AbnormalFlag=  "+POut.Int   ((int)labResult.AbnormalFlag)+" "
				+"WHERE LabResultNum = "+POut.Long(labResult.LabResultNum);
			Db.NonQ(command);
		}

		///<summary>Updates one LabResult in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(LabResult labResult,LabResult oldLabResult) {
			string command="";
			if(labResult.LabPanelNum != oldLabResult.LabPanelNum) {
				if(command!="") { command+=",";}
				command+="LabPanelNum = "+POut.Long(labResult.LabPanelNum)+"";
			}
			if(labResult.DateTimeTest != oldLabResult.DateTimeTest) {
				if(command!="") { command+=",";}
				command+="DateTimeTest = "+POut.DateT(labResult.DateTimeTest)+"";
			}
			if(labResult.TestName != oldLabResult.TestName) {
				if(command!="") { command+=",";}
				command+="TestName = '"+POut.String(labResult.TestName)+"'";
			}
			//DateTStamp can only be set by MySQL
			if(labResult.TestID != oldLabResult.TestID) {
				if(command!="") { command+=",";}
				command+="TestID = '"+POut.String(labResult.TestID)+"'";
			}
			if(labResult.ObsValue != oldLabResult.ObsValue) {
				if(command!="") { command+=",";}
				command+="ObsValue = '"+POut.String(labResult.ObsValue)+"'";
			}
			if(labResult.ObsUnits != oldLabResult.ObsUnits) {
				if(command!="") { command+=",";}
				command+="ObsUnits = '"+POut.String(labResult.ObsUnits)+"'";
			}
			if(labResult.ObsRange != oldLabResult.ObsRange) {
				if(command!="") { command+=",";}
				command+="ObsRange = '"+POut.String(labResult.ObsRange)+"'";
			}
			if(labResult.AbnormalFlag != oldLabResult.AbnormalFlag) {
				if(command!="") { command+=",";}
				command+="AbnormalFlag = "+POut.Int   ((int)labResult.AbnormalFlag)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE labresult SET "+command
				+" WHERE LabResultNum = "+POut.Long(labResult.LabResultNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(LabResult,LabResult) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(LabResult labResult,LabResult oldLabResult) {
			if(labResult.LabPanelNum != oldLabResult.LabPanelNum) {
				return true;
			}
			if(labResult.DateTimeTest != oldLabResult.DateTimeTest) {
				return true;
			}
			if(labResult.TestName != oldLabResult.TestName) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(labResult.TestID != oldLabResult.TestID) {
				return true;
			}
			if(labResult.ObsValue != oldLabResult.ObsValue) {
				return true;
			}
			if(labResult.ObsUnits != oldLabResult.ObsUnits) {
				return true;
			}
			if(labResult.ObsRange != oldLabResult.ObsRange) {
				return true;
			}
			if(labResult.AbnormalFlag != oldLabResult.AbnormalFlag) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one LabResult from the database.</summary>
		public static void Delete(long labResultNum) {
			string command="DELETE FROM labresult "
				+"WHERE LabResultNum = "+POut.Long(labResultNum);
			Db.NonQ(command);
		}

	}
}