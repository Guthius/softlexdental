//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ApptFieldCrud {
		///<summary>Gets one ApptField object from the database using the primary key.  Returns null if not found.</summary>
		public static ApptField SelectOne(long apptFieldNum) {
			string command="SELECT * FROM apptfield "
				+"WHERE ApptFieldNum = "+POut.Long(apptFieldNum);
			List<ApptField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ApptField object from the database using a query.</summary>
		public static ApptField SelectOne(string command) {
			
			List<ApptField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ApptField objects from the database using a query.</summary>
		public static List<ApptField> SelectMany(string command) {
			
			List<ApptField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ApptField> TableToList(DataTable table) {
			List<ApptField> retVal=new List<ApptField>();
			ApptField apptField;
			foreach(DataRow row in table.Rows) {
				apptField=new ApptField();
				apptField.ApptFieldNum= PIn.Long  (row["ApptFieldNum"].ToString());
				apptField.AptNum      = PIn.Long  (row["AptNum"].ToString());
				apptField.FieldName   = PIn.String(row["FieldName"].ToString());
				apptField.FieldValue  = PIn.String(row["FieldValue"].ToString());
				retVal.Add(apptField);
			}
			return retVal;
		}

		///<summary>Converts a list of ApptField into a DataTable.</summary>
		public static DataTable ListToTable(List<ApptField> listApptFields,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ApptField";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ApptFieldNum");
			table.Columns.Add("AptNum");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldValue");
			foreach(ApptField apptField in listApptFields) {
				table.Rows.Add(new object[] {
					POut.Long  (apptField.ApptFieldNum),
					POut.Long  (apptField.AptNum),
					            apptField.FieldName,
					            apptField.FieldValue,
				});
			}
			return table;
		}

		///<summary>Inserts one ApptField into the database.  Returns the new priKey.</summary>
		public static long Insert(ApptField apptField) {
			return Insert(apptField,false);
		}

		///<summary>Inserts one ApptField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ApptField apptField,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				apptField.ApptFieldNum=ReplicationServers.GetKey("apptfield","ApptFieldNum");
			}
			string command="INSERT INTO apptfield (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ApptFieldNum,";
			}
			command+="AptNum,FieldName,FieldValue) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(apptField.ApptFieldNum)+",";
			}
			command+=
				     POut.Long  (apptField.AptNum)+","
				+"'"+POut.String(apptField.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue)";
			if(apptField.FieldValue==null) {
				apptField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(apptField.FieldValue));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramFieldValue);
			}
			else {
				apptField.ApptFieldNum=Db.NonQ(command,true,"ApptFieldNum","apptField",paramFieldValue);
			}
			return apptField.ApptFieldNum;
		}

		///<summary>Inserts one ApptField into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptField apptField) {
			return InsertNoCache(apptField,false);
		}

		///<summary>Inserts one ApptField into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptField apptField,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO apptfield (";
			if(!useExistingPK && isRandomKeys) {
				apptField.ApptFieldNum=ReplicationServers.GetKeyNoCache("apptfield","ApptFieldNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ApptFieldNum,";
			}
			command+="AptNum,FieldName,FieldValue) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(apptField.ApptFieldNum)+",";
			}
			command+=
				     POut.Long  (apptField.AptNum)+","
				+"'"+POut.String(apptField.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue)";
			if(apptField.FieldValue==null) {
				apptField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(apptField.FieldValue));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramFieldValue);
			}
			else {
				apptField.ApptFieldNum=Db.NonQ(command,true,"ApptFieldNum","apptField",paramFieldValue);
			}
			return apptField.ApptFieldNum;
		}

		///<summary>Updates one ApptField in the database.</summary>
		public static void Update(ApptField apptField) {
			string command="UPDATE apptfield SET "
				+"AptNum      =  "+POut.Long  (apptField.AptNum)+", "
				+"FieldName   = '"+POut.String(apptField.FieldName)+"', "
				+"FieldValue  =  "+DbHelper.ParamChar+"paramFieldValue "
				+"WHERE ApptFieldNum = "+POut.Long(apptField.ApptFieldNum);
			if(apptField.FieldValue==null) {
				apptField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(apptField.FieldValue));
			Db.NonQ(command,paramFieldValue);
		}

		///<summary>Updates one ApptField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ApptField apptField,ApptField oldApptField) {
			string command="";
			if(apptField.AptNum != oldApptField.AptNum) {
				if(command!="") { command+=",";}
				command+="AptNum = "+POut.Long(apptField.AptNum)+"";
			}
			if(apptField.FieldName != oldApptField.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(apptField.FieldName)+"'";
			}
			if(apptField.FieldValue != oldApptField.FieldValue) {
				if(command!="") { command+=",";}
				command+="FieldValue = "+DbHelper.ParamChar+"paramFieldValue";
			}
			if(command=="") {
				return false;
			}
			if(apptField.FieldValue==null) {
				apptField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(apptField.FieldValue));
			command="UPDATE apptfield SET "+command
				+" WHERE ApptFieldNum = "+POut.Long(apptField.ApptFieldNum);
			Db.NonQ(command,paramFieldValue);
			return true;
		}

		///<summary>Returns true if Update(ApptField,ApptField) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ApptField apptField,ApptField oldApptField) {
			if(apptField.AptNum != oldApptField.AptNum) {
				return true;
			}
			if(apptField.FieldName != oldApptField.FieldName) {
				return true;
			}
			if(apptField.FieldValue != oldApptField.FieldValue) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ApptField from the database.</summary>
		public static void Delete(long apptFieldNum) {
			string command="DELETE FROM apptfield "
				+"WHERE ApptFieldNum = "+POut.Long(apptFieldNum);
			Db.NonQ(command);
		}

	}
}