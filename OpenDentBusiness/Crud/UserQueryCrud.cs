//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class UserQueryCrud {
		///<summary>Gets one UserQuery object from the database using the primary key.  Returns null if not found.</summary>
		public static UserQuery SelectOne(long queryNum) {
			string command="SELECT * FROM userquery "
				+"WHERE QueryNum = "+POut.Long(queryNum);
			List<UserQuery> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one UserQuery object from the database using a query.</summary>
		public static UserQuery SelectOne(string command) {
			
			List<UserQuery> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of UserQuery objects from the database using a query.</summary>
		public static List<UserQuery> SelectMany(string command) {
			
			List<UserQuery> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<UserQuery> TableToList(DataTable table) {
			List<UserQuery> retVal=new List<UserQuery>();
			UserQuery userQuery;
			foreach(DataRow row in table.Rows) {
				userQuery=new UserQuery();
				userQuery.QueryNum     = PIn.Long  (row["QueryNum"].ToString());
				userQuery.Description  = PIn.String(row["Description"].ToString());
				userQuery.FileName     = PIn.String(row["FileName"].ToString());
				userQuery.QueryText    = PIn.String(row["QueryText"].ToString());
				userQuery.IsReleased   = PIn.Bool  (row["IsReleased"].ToString());
				userQuery.IsPromptSetup= PIn.Bool  (row["IsPromptSetup"].ToString());
				retVal.Add(userQuery);
			}
			return retVal;
		}

		///<summary>Converts a list of UserQuery into a DataTable.</summary>
		public static DataTable ListToTable(List<UserQuery> listUserQuerys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="UserQuery";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("QueryNum");
			table.Columns.Add("Description");
			table.Columns.Add("FileName");
			table.Columns.Add("QueryText");
			table.Columns.Add("IsReleased");
			table.Columns.Add("IsPromptSetup");
			foreach(UserQuery userQuery in listUserQuerys) {
				table.Rows.Add(new object[] {
					POut.Long  (userQuery.QueryNum),
					            userQuery.Description,
					            userQuery.FileName,
					            userQuery.QueryText,
					POut.Bool  (userQuery.IsReleased),
					POut.Bool  (userQuery.IsPromptSetup),
				});
			}
			return table;
		}

		///<summary>Inserts one UserQuery into the database.  Returns the new priKey.</summary>
		public static long Insert(UserQuery userQuery) {
			return Insert(userQuery,false);
		}

		///<summary>Inserts one UserQuery into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(UserQuery userQuery,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				userQuery.QueryNum=ReplicationServers.GetKey("userquery","QueryNum");
			}
			string command="INSERT INTO userquery (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="QueryNum,";
			}
			command+="Description,FileName,QueryText,IsReleased,IsPromptSetup) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(userQuery.QueryNum)+",";
			}
			command+=
				 "'"+POut.String(userQuery.Description)+"',"
				+"'"+POut.String(userQuery.FileName)+"',"
				+    DbHelper.ParamChar+"paramQueryText,"
				+    POut.Bool  (userQuery.IsReleased)+","
				+    POut.Bool  (userQuery.IsPromptSetup)+")";
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,POut.StringParam(userQuery.QueryText));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramQueryText);
			}
			else {
				userQuery.QueryNum=Db.NonQ(command,true,"QueryNum","userQuery",paramQueryText);
			}
			return userQuery.QueryNum;
		}

		///<summary>Inserts one UserQuery into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UserQuery userQuery) {
			return InsertNoCache(userQuery,false);
		}

		///<summary>Inserts one UserQuery into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UserQuery userQuery,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO userquery (";
			if(!useExistingPK && isRandomKeys) {
				userQuery.QueryNum=ReplicationServers.GetKeyNoCache("userquery","QueryNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="QueryNum,";
			}
			command+="Description,FileName,QueryText,IsReleased,IsPromptSetup) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(userQuery.QueryNum)+",";
			}
			command+=
				 "'"+POut.String(userQuery.Description)+"',"
				+"'"+POut.String(userQuery.FileName)+"',"
				+    DbHelper.ParamChar+"paramQueryText,"
				+    POut.Bool  (userQuery.IsReleased)+","
				+    POut.Bool  (userQuery.IsPromptSetup)+")";
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,POut.StringParam(userQuery.QueryText));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramQueryText);
			}
			else {
				userQuery.QueryNum=Db.NonQ(command,true,"QueryNum","userQuery",paramQueryText);
			}
			return userQuery.QueryNum;
		}

		///<summary>Updates one UserQuery in the database.</summary>
		public static void Update(UserQuery userQuery) {
			string command="UPDATE userquery SET "
				+"Description  = '"+POut.String(userQuery.Description)+"', "
				+"FileName     = '"+POut.String(userQuery.FileName)+"', "
				+"QueryText    =  "+DbHelper.ParamChar+"paramQueryText, "
				+"IsReleased   =  "+POut.Bool  (userQuery.IsReleased)+", "
				+"IsPromptSetup=  "+POut.Bool  (userQuery.IsPromptSetup)+" "
				+"WHERE QueryNum = "+POut.Long(userQuery.QueryNum);
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,POut.StringParam(userQuery.QueryText));
			Db.NonQ(command,paramQueryText);
		}

		///<summary>Updates one UserQuery in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(UserQuery userQuery,UserQuery oldUserQuery) {
			string command="";
			if(userQuery.Description != oldUserQuery.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(userQuery.Description)+"'";
			}
			if(userQuery.FileName != oldUserQuery.FileName) {
				if(command!="") { command+=",";}
				command+="FileName = '"+POut.String(userQuery.FileName)+"'";
			}
			if(userQuery.QueryText != oldUserQuery.QueryText) {
				if(command!="") { command+=",";}
				command+="QueryText = "+DbHelper.ParamChar+"paramQueryText";
			}
			if(userQuery.IsReleased != oldUserQuery.IsReleased) {
				if(command!="") { command+=",";}
				command+="IsReleased = "+POut.Bool(userQuery.IsReleased)+"";
			}
			if(userQuery.IsPromptSetup != oldUserQuery.IsPromptSetup) {
				if(command!="") { command+=",";}
				command+="IsPromptSetup = "+POut.Bool(userQuery.IsPromptSetup)+"";
			}
			if(command=="") {
				return false;
			}
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,POut.StringParam(userQuery.QueryText));
			command="UPDATE userquery SET "+command
				+" WHERE QueryNum = "+POut.Long(userQuery.QueryNum);
			Db.NonQ(command,paramQueryText);
			return true;
		}

		///<summary>Returns true if Update(UserQuery,UserQuery) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(UserQuery userQuery,UserQuery oldUserQuery) {
			if(userQuery.Description != oldUserQuery.Description) {
				return true;
			}
			if(userQuery.FileName != oldUserQuery.FileName) {
				return true;
			}
			if(userQuery.QueryText != oldUserQuery.QueryText) {
				return true;
			}
			if(userQuery.IsReleased != oldUserQuery.IsReleased) {
				return true;
			}
			if(userQuery.IsPromptSetup != oldUserQuery.IsPromptSetup) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one UserQuery from the database.</summary>
		public static void Delete(long queryNum) {
			string command="DELETE FROM userquery "
				+"WHERE QueryNum = "+POut.Long(queryNum);
			Db.NonQ(command);
		}

	}
}