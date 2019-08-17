//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrAmendmentCrud {
		///<summary>Gets one EhrAmendment object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrAmendment SelectOne(long ehrAmendmentNum) {
			string command="SELECT * FROM ehramendment "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendmentNum);
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrAmendment object from the database using a query.</summary>
		public static EhrAmendment SelectOne(string command) {
			
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrAmendment objects from the database using a query.</summary>
		public static List<EhrAmendment> SelectMany(string command) {
			
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrAmendment> TableToList(DataTable table) {
			List<EhrAmendment> retVal=new List<EhrAmendment>();
			EhrAmendment ehrAmendment;
			foreach(DataRow row in table.Rows) {
				ehrAmendment=new EhrAmendment();
				ehrAmendment.EhrAmendmentNum= PIn.Long  (row["EhrAmendmentNum"].ToString());
				ehrAmendment.PatNum         = PIn.Long  (row["PatNum"].ToString());
				ehrAmendment.IsAccepted     = (OpenDentBusiness.YN)PIn.Int(row["IsAccepted"].ToString());
				ehrAmendment.Description    = PIn.String(row["Description"].ToString());
				ehrAmendment.Source         = (OpenDentBusiness.AmendmentSource)PIn.Int(row["Source"].ToString());
				ehrAmendment.SourceName     = PIn.String(row["SourceName"].ToString());
				ehrAmendment.FileName       = PIn.String(row["FileName"].ToString());
				ehrAmendment.RawBase64      = PIn.String(row["RawBase64"].ToString());
				ehrAmendment.DateTRequest   = PIn.DateT (row["DateTRequest"].ToString());
				ehrAmendment.DateTAcceptDeny= PIn.DateT (row["DateTAcceptDeny"].ToString());
				ehrAmendment.DateTAppend    = PIn.DateT (row["DateTAppend"].ToString());
				retVal.Add(ehrAmendment);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrAmendment into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrAmendment> listEhrAmendments,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrAmendment";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrAmendmentNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("IsAccepted");
			table.Columns.Add("Description");
			table.Columns.Add("Source");
			table.Columns.Add("SourceName");
			table.Columns.Add("FileName");
			table.Columns.Add("RawBase64");
			table.Columns.Add("DateTRequest");
			table.Columns.Add("DateTAcceptDeny");
			table.Columns.Add("DateTAppend");
			foreach(EhrAmendment ehrAmendment in listEhrAmendments) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrAmendment.EhrAmendmentNum),
					POut.Long  (ehrAmendment.PatNum),
					POut.Int   ((int)ehrAmendment.IsAccepted),
					            ehrAmendment.Description,
					POut.Int   ((int)ehrAmendment.Source),
					            ehrAmendment.SourceName,
					            ehrAmendment.FileName,
					            ehrAmendment.RawBase64,
					POut.DateT (ehrAmendment.DateTRequest,false),
					POut.DateT (ehrAmendment.DateTAcceptDeny,false),
					POut.DateT (ehrAmendment.DateTAppend,false),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrAmendment into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrAmendment ehrAmendment) {
			return Insert(ehrAmendment,false);
		}

		///<summary>Inserts one EhrAmendment into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrAmendment ehrAmendment,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				ehrAmendment.EhrAmendmentNum=ReplicationServers.GetKey("ehramendment","EhrAmendmentNum");
			}
			string command="INSERT INTO ehramendment (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="EhrAmendmentNum,";
			}
			command+="PatNum,IsAccepted,Description,Source,SourceName,FileName,RawBase64,DateTRequest,DateTAcceptDeny,DateTAppend) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(ehrAmendment.EhrAmendmentNum)+",";
			}
			command+=
				     POut.Long  (ehrAmendment.PatNum)+","
				+    POut.Int   ((int)ehrAmendment.IsAccepted)+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    POut.Int   ((int)ehrAmendment.Source)+","
				+    DbHelper.ParamChar+"paramSourceName,"
				+"'"+POut.String(ehrAmendment.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64,"
				+    POut.DateT (ehrAmendment.DateTRequest)+","
				+    POut.DateT (ehrAmendment.DateTAcceptDeny)+","
				+    POut.DateT (ehrAmendment.DateTAppend)+")";
			if(ehrAmendment.Description==null) {
				ehrAmendment.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(ehrAmendment.Description));
			if(ehrAmendment.SourceName==null) {
				ehrAmendment.SourceName="";
			}
			OdSqlParameter paramSourceName=new OdSqlParameter("paramSourceName",OdDbType.Text,POut.StringParam(ehrAmendment.SourceName));
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,POut.StringParam(ehrAmendment.RawBase64));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramDescription,paramSourceName,paramRawBase64);
			}
			else {
				ehrAmendment.EhrAmendmentNum=Db.NonQ(command,true,"EhrAmendmentNum","ehrAmendment",paramDescription,paramSourceName,paramRawBase64);
			}
			return ehrAmendment.EhrAmendmentNum;
		}

		///<summary>Inserts one EhrAmendment into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAmendment ehrAmendment) {
			return InsertNoCache(ehrAmendment,false);
		}

		///<summary>Inserts one EhrAmendment into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAmendment ehrAmendment,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO ehramendment (";
			if(!useExistingPK && isRandomKeys) {
				ehrAmendment.EhrAmendmentNum=ReplicationServers.GetKeyNoCache("ehramendment","EhrAmendmentNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrAmendmentNum,";
			}
			command+="PatNum,IsAccepted,Description,Source,SourceName,FileName,RawBase64,DateTRequest,DateTAcceptDeny,DateTAppend) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrAmendment.EhrAmendmentNum)+",";
			}
			command+=
				     POut.Long  (ehrAmendment.PatNum)+","
				+    POut.Int   ((int)ehrAmendment.IsAccepted)+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    POut.Int   ((int)ehrAmendment.Source)+","
				+    DbHelper.ParamChar+"paramSourceName,"
				+"'"+POut.String(ehrAmendment.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64,"
				+    POut.DateT (ehrAmendment.DateTRequest)+","
				+    POut.DateT (ehrAmendment.DateTAcceptDeny)+","
				+    POut.DateT (ehrAmendment.DateTAppend)+")";
			if(ehrAmendment.Description==null) {
				ehrAmendment.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(ehrAmendment.Description));
			if(ehrAmendment.SourceName==null) {
				ehrAmendment.SourceName="";
			}
			OdSqlParameter paramSourceName=new OdSqlParameter("paramSourceName",OdDbType.Text,POut.StringParam(ehrAmendment.SourceName));
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,POut.StringParam(ehrAmendment.RawBase64));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription,paramSourceName,paramRawBase64);
			}
			else {
				ehrAmendment.EhrAmendmentNum=Db.NonQ(command,true,"EhrAmendmentNum","ehrAmendment",paramDescription,paramSourceName,paramRawBase64);
			}
			return ehrAmendment.EhrAmendmentNum;
		}

		///<summary>Updates one EhrAmendment in the database.</summary>
		public static void Update(EhrAmendment ehrAmendment) {
			string command="UPDATE ehramendment SET "
				+"PatNum         =  "+POut.Long  (ehrAmendment.PatNum)+", "
				+"IsAccepted     =  "+POut.Int   ((int)ehrAmendment.IsAccepted)+", "
				+"Description    =  "+DbHelper.ParamChar+"paramDescription, "
				+"Source         =  "+POut.Int   ((int)ehrAmendment.Source)+", "
				+"SourceName     =  "+DbHelper.ParamChar+"paramSourceName, "
				+"FileName       = '"+POut.String(ehrAmendment.FileName)+"', "
				+"RawBase64      =  "+DbHelper.ParamChar+"paramRawBase64, "
				+"DateTRequest   =  "+POut.DateT (ehrAmendment.DateTRequest)+", "
				+"DateTAcceptDeny=  "+POut.DateT (ehrAmendment.DateTAcceptDeny)+", "
				+"DateTAppend    =  "+POut.DateT (ehrAmendment.DateTAppend)+" "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendment.EhrAmendmentNum);
			if(ehrAmendment.Description==null) {
				ehrAmendment.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(ehrAmendment.Description));
			if(ehrAmendment.SourceName==null) {
				ehrAmendment.SourceName="";
			}
			OdSqlParameter paramSourceName=new OdSqlParameter("paramSourceName",OdDbType.Text,POut.StringParam(ehrAmendment.SourceName));
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,POut.StringParam(ehrAmendment.RawBase64));
			Db.NonQ(command,paramDescription,paramSourceName,paramRawBase64);
		}

		///<summary>Updates one EhrAmendment in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrAmendment ehrAmendment,EhrAmendment oldEhrAmendment) {
			string command="";
			if(ehrAmendment.PatNum != oldEhrAmendment.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(ehrAmendment.PatNum)+"";
			}
			if(ehrAmendment.IsAccepted != oldEhrAmendment.IsAccepted) {
				if(command!="") { command+=",";}
				command+="IsAccepted = "+POut.Int   ((int)ehrAmendment.IsAccepted)+"";
			}
			if(ehrAmendment.Description != oldEhrAmendment.Description) {
				if(command!="") { command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(ehrAmendment.Source != oldEhrAmendment.Source) {
				if(command!="") { command+=",";}
				command+="Source = "+POut.Int   ((int)ehrAmendment.Source)+"";
			}
			if(ehrAmendment.SourceName != oldEhrAmendment.SourceName) {
				if(command!="") { command+=",";}
				command+="SourceName = "+DbHelper.ParamChar+"paramSourceName";
			}
			if(ehrAmendment.FileName != oldEhrAmendment.FileName) {
				if(command!="") { command+=",";}
				command+="FileName = '"+POut.String(ehrAmendment.FileName)+"'";
			}
			if(ehrAmendment.RawBase64 != oldEhrAmendment.RawBase64) {
				if(command!="") { command+=",";}
				command+="RawBase64 = "+DbHelper.ParamChar+"paramRawBase64";
			}
			if(ehrAmendment.DateTRequest != oldEhrAmendment.DateTRequest) {
				if(command!="") { command+=",";}
				command+="DateTRequest = "+POut.DateT(ehrAmendment.DateTRequest)+"";
			}
			if(ehrAmendment.DateTAcceptDeny != oldEhrAmendment.DateTAcceptDeny) {
				if(command!="") { command+=",";}
				command+="DateTAcceptDeny = "+POut.DateT(ehrAmendment.DateTAcceptDeny)+"";
			}
			if(ehrAmendment.DateTAppend != oldEhrAmendment.DateTAppend) {
				if(command!="") { command+=",";}
				command+="DateTAppend = "+POut.DateT(ehrAmendment.DateTAppend)+"";
			}
			if(command=="") {
				return false;
			}
			if(ehrAmendment.Description==null) {
				ehrAmendment.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(ehrAmendment.Description));
			if(ehrAmendment.SourceName==null) {
				ehrAmendment.SourceName="";
			}
			OdSqlParameter paramSourceName=new OdSqlParameter("paramSourceName",OdDbType.Text,POut.StringParam(ehrAmendment.SourceName));
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,POut.StringParam(ehrAmendment.RawBase64));
			command="UPDATE ehramendment SET "+command
				+" WHERE EhrAmendmentNum = "+POut.Long(ehrAmendment.EhrAmendmentNum);
			Db.NonQ(command,paramDescription,paramSourceName,paramRawBase64);
			return true;
		}

		///<summary>Returns true if Update(EhrAmendment,EhrAmendment) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrAmendment ehrAmendment,EhrAmendment oldEhrAmendment) {
			if(ehrAmendment.PatNum != oldEhrAmendment.PatNum) {
				return true;
			}
			if(ehrAmendment.IsAccepted != oldEhrAmendment.IsAccepted) {
				return true;
			}
			if(ehrAmendment.Description != oldEhrAmendment.Description) {
				return true;
			}
			if(ehrAmendment.Source != oldEhrAmendment.Source) {
				return true;
			}
			if(ehrAmendment.SourceName != oldEhrAmendment.SourceName) {
				return true;
			}
			if(ehrAmendment.FileName != oldEhrAmendment.FileName) {
				return true;
			}
			if(ehrAmendment.RawBase64 != oldEhrAmendment.RawBase64) {
				return true;
			}
			if(ehrAmendment.DateTRequest != oldEhrAmendment.DateTRequest) {
				return true;
			}
			if(ehrAmendment.DateTAcceptDeny != oldEhrAmendment.DateTAcceptDeny) {
				return true;
			}
			if(ehrAmendment.DateTAppend != oldEhrAmendment.DateTAppend) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrAmendment from the database.</summary>
		public static void Delete(long ehrAmendmentNum) {
			string command="DELETE FROM ehramendment "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendmentNum);
			Db.NonQ(command);
		}

	}
}