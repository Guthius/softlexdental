//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness.Crud{
	public class TsiTransLogCrud {
		///<summary>Gets one TsiTransLog object from the database using the primary key.  Returns null if not found.</summary>
		public static TsiTransLog SelectOne(long tsiTransLogNum) {
			string command="SELECT * FROM tsitranslog "
				+"WHERE TsiTransLogNum = "+POut.Long(tsiTransLogNum);
			List<TsiTransLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one TsiTransLog object from the database using a query.</summary>
		public static TsiTransLog SelectOne(string command) {
			
			List<TsiTransLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of TsiTransLog objects from the database using a query.</summary>
		public static List<TsiTransLog> SelectMany(string command) {
			
			List<TsiTransLog> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<TsiTransLog> TableToList(DataTable table) {
			List<TsiTransLog> retVal=new List<TsiTransLog>();
			TsiTransLog tsiTransLog;
			foreach(DataRow row in table.Rows) {
				tsiTransLog=new TsiTransLog();
				tsiTransLog.TsiTransLogNum= PIn.Long  (row["TsiTransLogNum"].ToString());
				tsiTransLog.PatNum        = PIn.Long  (row["PatNum"].ToString());
				tsiTransLog.UserNum       = PIn.Long  (row["UserNum"].ToString());
				tsiTransLog.TransType     = (OpenDentBusiness.TsiTransType)PIn.Int(row["TransType"].ToString());
				tsiTransLog.TransDateTime = PIn.DateT (row["TransDateTime"].ToString());
				tsiTransLog.DemandType    = (OpenDentBusiness.TsiDemandType)PIn.Int(row["DemandType"].ToString());
				tsiTransLog.ServiceCode   = (OpenDentBusiness.TsiServiceCode)PIn.Int(row["ServiceCode"].ToString());
				tsiTransLog.ClientId      = PIn.String(row["ClientId"].ToString());
				tsiTransLog.TransAmt      = PIn.Double(row["TransAmt"].ToString());
				tsiTransLog.AccountBalance= PIn.Double(row["AccountBalance"].ToString());
				tsiTransLog.FKeyType      = (OpenDentBusiness.TsiFKeyType)PIn.Int(row["FKeyType"].ToString());
				tsiTransLog.FKey          = PIn.Long  (row["FKey"].ToString());
				tsiTransLog.RawMsgText    = PIn.String(row["RawMsgText"].ToString());
				tsiTransLog.TransJson     = PIn.String(row["TransJson"].ToString());
				tsiTransLog.ClinicNum     = PIn.Long  (row["ClinicNum"].ToString());
				tsiTransLog.AggTransLogNum= PIn.Long  (row["AggTransLogNum"].ToString());
				retVal.Add(tsiTransLog);
			}
			return retVal;
		}

		///<summary>Converts a list of TsiTransLog into a DataTable.</summary>
		public static DataTable ListToTable(List<TsiTransLog> listTsiTransLogs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="TsiTransLog";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("TsiTransLogNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("TransType");
			table.Columns.Add("TransDateTime");
			table.Columns.Add("DemandType");
			table.Columns.Add("ServiceCode");
			table.Columns.Add("ClientId");
			table.Columns.Add("TransAmt");
			table.Columns.Add("AccountBalance");
			table.Columns.Add("FKeyType");
			table.Columns.Add("FKey");
			table.Columns.Add("RawMsgText");
			table.Columns.Add("TransJson");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("AggTransLogNum");
			foreach(TsiTransLog tsiTransLog in listTsiTransLogs) {
				table.Rows.Add(new object[] {
					POut.Long  (tsiTransLog.TsiTransLogNum),
					POut.Long  (tsiTransLog.PatNum),
					POut.Long  (tsiTransLog.UserNum),
					POut.Int   ((int)tsiTransLog.TransType),
					POut.DateT (tsiTransLog.TransDateTime,false),
					POut.Int   ((int)tsiTransLog.DemandType),
					POut.Int   ((int)tsiTransLog.ServiceCode),
					            tsiTransLog.ClientId,
					POut.Double(tsiTransLog.TransAmt),
					POut.Double(tsiTransLog.AccountBalance),
					POut.Int   ((int)tsiTransLog.FKeyType),
					POut.Long  (tsiTransLog.FKey),
					            tsiTransLog.RawMsgText,
					            tsiTransLog.TransJson,
					POut.Long  (tsiTransLog.ClinicNum),
					POut.Long  (tsiTransLog.AggTransLogNum),
				});
			}
			return table;
		}

		///<summary>Inserts one TsiTransLog into the database.  Returns the new priKey.</summary>
		public static long Insert(TsiTransLog tsiTransLog) {
			return Insert(tsiTransLog,false);
		}

		///<summary>Inserts one TsiTransLog into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(TsiTransLog tsiTransLog,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				tsiTransLog.TsiTransLogNum=ReplicationServers.GetKey("tsitranslog","TsiTransLogNum");
			}
			string command="INSERT INTO tsitranslog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="TsiTransLogNum,";
			}
			command+="PatNum,UserNum,TransType,TransDateTime,DemandType,ServiceCode,ClientId,TransAmt,AccountBalance,FKeyType,FKey,RawMsgText,TransJson,ClinicNum,AggTransLogNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(tsiTransLog.TsiTransLogNum)+",";
			}
			command+=
				     POut.Long  (tsiTransLog.PatNum)+","
				+    POut.Long  (tsiTransLog.UserNum)+","
				+    POut.Int   ((int)tsiTransLog.TransType)+","
				+    DbHelper.Now()+","
				+    POut.Int   ((int)tsiTransLog.DemandType)+","
				+    POut.Int   ((int)tsiTransLog.ServiceCode)+","
				+"'"+POut.String(tsiTransLog.ClientId)+"',"
				+"'"+POut.Double(tsiTransLog.TransAmt)+"',"
				+"'"+POut.Double(tsiTransLog.AccountBalance)+"',"
				+    POut.Int   ((int)tsiTransLog.FKeyType)+","
				+    POut.Long  (tsiTransLog.FKey)+","
				+"'"+POut.String(tsiTransLog.RawMsgText)+"',"
				+    DbHelper.ParamChar+"paramTransJson,"
				+    POut.Long  (tsiTransLog.ClinicNum)+","
				+    POut.Long  (tsiTransLog.AggTransLogNum)+")";
			if(tsiTransLog.TransJson==null) {
				tsiTransLog.TransJson="";
			}
			OdSqlParameter paramTransJson=new OdSqlParameter("paramTransJson",OdDbType.Text,POut.StringParam(tsiTransLog.TransJson));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramTransJson);
			}
			else {
				tsiTransLog.TsiTransLogNum=Db.NonQ(command,true,"TsiTransLogNum","tsiTransLog",paramTransJson);
			}
			return tsiTransLog.TsiTransLogNum;
		}

		///<summary>Inserts many TsiTransLogs into the database.</summary>
		public static void InsertMany(List<TsiTransLog> listTsiTransLogs) {
			InsertMany(listTsiTransLogs,false);
		}

		///<summary>Inserts many TsiTransLogs into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<TsiTransLog> listTsiTransLogs,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				foreach(TsiTransLog tsiTransLog in listTsiTransLogs) {
					Insert(tsiTransLog);
				}
			}
			else {
				StringBuilder sbCommands=null;
				int index=0;
				while(index < listTsiTransLogs.Count) {
					TsiTransLog tsiTransLog=listTsiTransLogs[index];
					StringBuilder sbRow=new StringBuilder("(");
					bool hasComma=false;
					if(sbCommands==null) {
						sbCommands=new StringBuilder();
						sbCommands.Append("INSERT INTO tsitranslog (");
						if(useExistingPK) {
							sbCommands.Append("TsiTransLogNum,");
						}
						sbCommands.Append("PatNum,UserNum,TransType,TransDateTime,DemandType,ServiceCode,ClientId,TransAmt,AccountBalance,FKeyType,FKey,RawMsgText,TransJson,ClinicNum,AggTransLogNum) VALUES ");
					}
					else {
						hasComma=true;
					}
					if(useExistingPK) {
						sbRow.Append(POut.Long(tsiTransLog.TsiTransLogNum)); sbRow.Append(",");
					}
					sbRow.Append(POut.Long(tsiTransLog.PatNum)); sbRow.Append(",");
					sbRow.Append(POut.Long(tsiTransLog.UserNum)); sbRow.Append(",");
					sbRow.Append(POut.Int((int)tsiTransLog.TransType)); sbRow.Append(",");
					sbRow.Append(DbHelper.Now()); sbRow.Append(",");
					sbRow.Append(POut.Int((int)tsiTransLog.DemandType)); sbRow.Append(",");
					sbRow.Append(POut.Int((int)tsiTransLog.ServiceCode)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(tsiTransLog.ClientId)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.Double(tsiTransLog.TransAmt)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.Double(tsiTransLog.AccountBalance)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Int((int)tsiTransLog.FKeyType)); sbRow.Append(",");
					sbRow.Append(POut.Long(tsiTransLog.FKey)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(tsiTransLog.RawMsgText)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(tsiTransLog.TransJson)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Long(tsiTransLog.ClinicNum)); sbRow.Append(",");
					sbRow.Append(POut.Long(tsiTransLog.AggTransLogNum)); sbRow.Append(")");
					if(sbCommands.Length+sbRow.Length+1 > ODTable.MaxAllowedPacketCount) {
						Db.NonQ(sbCommands.ToString());
						sbCommands=null;
					}
					else {
						if(hasComma) {
							sbCommands.Append(",");
						}
						sbCommands.Append(sbRow.ToString());
						if(index==listTsiTransLogs.Count-1) {
							Db.NonQ(sbCommands.ToString());
						}
						index++;
					}
				}
			}
		}

		///<summary>Inserts one TsiTransLog into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TsiTransLog tsiTransLog) {
			return InsertNoCache(tsiTransLog,false);
		}

		///<summary>Inserts one TsiTransLog into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TsiTransLog tsiTransLog,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO tsitranslog (";
			if(!useExistingPK && isRandomKeys) {
				tsiTransLog.TsiTransLogNum=ReplicationServers.GetKeyNoCache("tsitranslog","TsiTransLogNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="TsiTransLogNum,";
			}
			command+="PatNum,UserNum,TransType,TransDateTime,DemandType,ServiceCode,ClientId,TransAmt,AccountBalance,FKeyType,FKey,RawMsgText,TransJson,ClinicNum,AggTransLogNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(tsiTransLog.TsiTransLogNum)+",";
			}
			command+=
				     POut.Long  (tsiTransLog.PatNum)+","
				+    POut.Long  (tsiTransLog.UserNum)+","
				+    POut.Int   ((int)tsiTransLog.TransType)+","
				+    DbHelper.Now()+","
				+    POut.Int   ((int)tsiTransLog.DemandType)+","
				+    POut.Int   ((int)tsiTransLog.ServiceCode)+","
				+"'"+POut.String(tsiTransLog.ClientId)+"',"
				+"'"+POut.Double(tsiTransLog.TransAmt)+"',"
				+"'"+POut.Double(tsiTransLog.AccountBalance)+"',"
				+    POut.Int   ((int)tsiTransLog.FKeyType)+","
				+    POut.Long  (tsiTransLog.FKey)+","
				+"'"+POut.String(tsiTransLog.RawMsgText)+"',"
				+    DbHelper.ParamChar+"paramTransJson,"
				+    POut.Long  (tsiTransLog.ClinicNum)+","
				+    POut.Long  (tsiTransLog.AggTransLogNum)+")";
			if(tsiTransLog.TransJson==null) {
				tsiTransLog.TransJson="";
			}
			OdSqlParameter paramTransJson=new OdSqlParameter("paramTransJson",OdDbType.Text,POut.StringParam(tsiTransLog.TransJson));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramTransJson);
			}
			else {
				tsiTransLog.TsiTransLogNum=Db.NonQ(command,true,"TsiTransLogNum","tsiTransLog",paramTransJson);
			}
			return tsiTransLog.TsiTransLogNum;
		}

		///<summary>Updates one TsiTransLog in the database.</summary>
		public static void Update(TsiTransLog tsiTransLog) {
			string command="UPDATE tsitranslog SET "
				+"PatNum        =  "+POut.Long  (tsiTransLog.PatNum)+", "
				+"UserNum       =  "+POut.Long  (tsiTransLog.UserNum)+", "
				+"TransType     =  "+POut.Int   ((int)tsiTransLog.TransType)+", "
				//TransDateTime not allowed to change
				+"DemandType    =  "+POut.Int   ((int)tsiTransLog.DemandType)+", "
				+"ServiceCode   =  "+POut.Int   ((int)tsiTransLog.ServiceCode)+", "
				+"ClientId      = '"+POut.String(tsiTransLog.ClientId)+"', "
				+"TransAmt      = '"+POut.Double(tsiTransLog.TransAmt)+"', "
				+"AccountBalance= '"+POut.Double(tsiTransLog.AccountBalance)+"', "
				+"FKeyType      =  "+POut.Int   ((int)tsiTransLog.FKeyType)+", "
				+"FKey          =  "+POut.Long  (tsiTransLog.FKey)+", "
				+"RawMsgText    = '"+POut.String(tsiTransLog.RawMsgText)+"', "
				+"TransJson     =  "+DbHelper.ParamChar+"paramTransJson, "
				+"ClinicNum     =  "+POut.Long  (tsiTransLog.ClinicNum)+", "
				+"AggTransLogNum=  "+POut.Long  (tsiTransLog.AggTransLogNum)+" "
				+"WHERE TsiTransLogNum = "+POut.Long(tsiTransLog.TsiTransLogNum);
			if(tsiTransLog.TransJson==null) {
				tsiTransLog.TransJson="";
			}
			OdSqlParameter paramTransJson=new OdSqlParameter("paramTransJson",OdDbType.Text,POut.StringParam(tsiTransLog.TransJson));
			Db.NonQ(command,paramTransJson);
		}

		///<summary>Updates one TsiTransLog in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(TsiTransLog tsiTransLog,TsiTransLog oldTsiTransLog) {
			string command="";
			if(tsiTransLog.PatNum != oldTsiTransLog.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(tsiTransLog.PatNum)+"";
			}
			if(tsiTransLog.UserNum != oldTsiTransLog.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(tsiTransLog.UserNum)+"";
			}
			if(tsiTransLog.TransType != oldTsiTransLog.TransType) {
				if(command!="") { command+=",";}
				command+="TransType = "+POut.Int   ((int)tsiTransLog.TransType)+"";
			}
			//TransDateTime not allowed to change
			if(tsiTransLog.DemandType != oldTsiTransLog.DemandType) {
				if(command!="") { command+=",";}
				command+="DemandType = "+POut.Int   ((int)tsiTransLog.DemandType)+"";
			}
			if(tsiTransLog.ServiceCode != oldTsiTransLog.ServiceCode) {
				if(command!="") { command+=",";}
				command+="ServiceCode = "+POut.Int   ((int)tsiTransLog.ServiceCode)+"";
			}
			if(tsiTransLog.ClientId != oldTsiTransLog.ClientId) {
				if(command!="") { command+=",";}
				command+="ClientId = '"+POut.String(tsiTransLog.ClientId)+"'";
			}
			if(tsiTransLog.TransAmt != oldTsiTransLog.TransAmt) {
				if(command!="") { command+=",";}
				command+="TransAmt = '"+POut.Double(tsiTransLog.TransAmt)+"'";
			}
			if(tsiTransLog.AccountBalance != oldTsiTransLog.AccountBalance) {
				if(command!="") { command+=",";}
				command+="AccountBalance = '"+POut.Double(tsiTransLog.AccountBalance)+"'";
			}
			if(tsiTransLog.FKeyType != oldTsiTransLog.FKeyType) {
				if(command!="") { command+=",";}
				command+="FKeyType = "+POut.Int   ((int)tsiTransLog.FKeyType)+"";
			}
			if(tsiTransLog.FKey != oldTsiTransLog.FKey) {
				if(command!="") { command+=",";}
				command+="FKey = "+POut.Long(tsiTransLog.FKey)+"";
			}
			if(tsiTransLog.RawMsgText != oldTsiTransLog.RawMsgText) {
				if(command!="") { command+=",";}
				command+="RawMsgText = '"+POut.String(tsiTransLog.RawMsgText)+"'";
			}
			if(tsiTransLog.TransJson != oldTsiTransLog.TransJson) {
				if(command!="") { command+=",";}
				command+="TransJson = "+DbHelper.ParamChar+"paramTransJson";
			}
			if(tsiTransLog.ClinicNum != oldTsiTransLog.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(tsiTransLog.ClinicNum)+"";
			}
			if(tsiTransLog.AggTransLogNum != oldTsiTransLog.AggTransLogNum) {
				if(command!="") { command+=",";}
				command+="AggTransLogNum = "+POut.Long(tsiTransLog.AggTransLogNum)+"";
			}
			if(command=="") {
				return false;
			}
			if(tsiTransLog.TransJson==null) {
				tsiTransLog.TransJson="";
			}
			OdSqlParameter paramTransJson=new OdSqlParameter("paramTransJson",OdDbType.Text,POut.StringParam(tsiTransLog.TransJson));
			command="UPDATE tsitranslog SET "+command
				+" WHERE TsiTransLogNum = "+POut.Long(tsiTransLog.TsiTransLogNum);
			Db.NonQ(command,paramTransJson);
			return true;
		}

		///<summary>Returns true if Update(TsiTransLog,TsiTransLog) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(TsiTransLog tsiTransLog,TsiTransLog oldTsiTransLog) {
			if(tsiTransLog.PatNum != oldTsiTransLog.PatNum) {
				return true;
			}
			if(tsiTransLog.UserNum != oldTsiTransLog.UserNum) {
				return true;
			}
			if(tsiTransLog.TransType != oldTsiTransLog.TransType) {
				return true;
			}
			//TransDateTime not allowed to change
			if(tsiTransLog.DemandType != oldTsiTransLog.DemandType) {
				return true;
			}
			if(tsiTransLog.ServiceCode != oldTsiTransLog.ServiceCode) {
				return true;
			}
			if(tsiTransLog.ClientId != oldTsiTransLog.ClientId) {
				return true;
			}
			if(tsiTransLog.TransAmt != oldTsiTransLog.TransAmt) {
				return true;
			}
			if(tsiTransLog.AccountBalance != oldTsiTransLog.AccountBalance) {
				return true;
			}
			if(tsiTransLog.FKeyType != oldTsiTransLog.FKeyType) {
				return true;
			}
			if(tsiTransLog.FKey != oldTsiTransLog.FKey) {
				return true;
			}
			if(tsiTransLog.RawMsgText != oldTsiTransLog.RawMsgText) {
				return true;
			}
			if(tsiTransLog.TransJson != oldTsiTransLog.TransJson) {
				return true;
			}
			if(tsiTransLog.ClinicNum != oldTsiTransLog.ClinicNum) {
				return true;
			}
			if(tsiTransLog.AggTransLogNum != oldTsiTransLog.AggTransLogNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one TsiTransLog from the database.</summary>
		public static void Delete(long tsiTransLogNum) {
			string command="DELETE FROM tsitranslog "
				+"WHERE TsiTransLogNum = "+POut.Long(tsiTransLogNum);
			Db.NonQ(command);
		}

	}
}