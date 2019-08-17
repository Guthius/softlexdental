//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LanguageCrud {
		///<summary>Gets one Language object from the database using the primary key.  Returns null if not found.</summary>
		public static Language SelectOne(long languageNum) {
			string command="SELECT * FROM language "
				+"WHERE LanguageNum = "+POut.Long(languageNum);
			List<Language> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Language object from the database using a query.</summary>
		public static Language SelectOne(string command) {
			
			List<Language> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Language objects from the database using a query.</summary>
		public static List<Language> SelectMany(string command) {
			
			List<Language> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Language> TableToList(DataTable table) {
			List<Language> retVal=new List<Language>();
			Language language;
			foreach(DataRow row in table.Rows) {
				language=new Language();
				language.LanguageNum    = PIn.Long  (row["LanguageNum"].ToString());
				language.EnglishComments= PIn.String(row["EnglishComments"].ToString());
				language.ClassType      = PIn.String(row["ClassType"].ToString());
				language.English        = PIn.String(row["English"].ToString());
				language.IsObsolete     = PIn.Bool  (row["IsObsolete"].ToString());
				retVal.Add(language);
			}
			return retVal;
		}

		///<summary>Converts a list of Language into a DataTable.</summary>
		public static DataTable ListToTable(List<Language> listLanguages,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Language";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LanguageNum");
			table.Columns.Add("EnglishComments");
			table.Columns.Add("ClassType");
			table.Columns.Add("English");
			table.Columns.Add("IsObsolete");
			foreach(Language language in listLanguages) {
				table.Rows.Add(new object[] {
					POut.Long  (language.LanguageNum),
					            language.EnglishComments,
					            language.ClassType,
					            language.English,
					POut.Bool  (language.IsObsolete),
				});
			}
			return table;
		}

		///<summary>Inserts one Language into the database.  Returns the new priKey.</summary>
		public static long Insert(Language language) {
			return Insert(language,false);
		}

		///<summary>Inserts one Language into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Language language,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				language.LanguageNum=ReplicationServers.GetKey("language","LanguageNum");
			}
			string command="INSERT INTO language (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="LanguageNum,";
			}
			command+="EnglishComments,ClassType,English,IsObsolete) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(language.LanguageNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramEnglishComments,"
				+    DbHelper.ParamChar+"paramClassType,"
				+    DbHelper.ParamChar+"paramEnglish,"
				+    POut.Bool  (language.IsObsolete)+")";
			if(language.EnglishComments==null) {
				language.EnglishComments="";
			}
			OdSqlParameter paramEnglishComments=new OdSqlParameter("paramEnglishComments",OdDbType.Text,POut.StringParam(language.EnglishComments));
			if(language.ClassType==null) {
				language.ClassType="";
			}
			OdSqlParameter paramClassType=new OdSqlParameter("paramClassType",OdDbType.Text,POut.StringParam(language.ClassType));
			if(language.English==null) {
				language.English="";
			}
			OdSqlParameter paramEnglish=new OdSqlParameter("paramEnglish",OdDbType.Text,POut.StringParam(language.English));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramEnglishComments,paramClassType,paramEnglish);
			}
			else {
				language.LanguageNum=Db.NonQ(command,true,"LanguageNum","language",paramEnglishComments,paramClassType,paramEnglish);
			}
			return language.LanguageNum;
		}

		///<summary>Inserts one Language into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Language language) {
			return InsertNoCache(language,false);
		}

		///<summary>Inserts one Language into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Language language,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO language (";
			if(!useExistingPK && isRandomKeys) {
				language.LanguageNum=ReplicationServers.GetKeyNoCache("language","LanguageNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="LanguageNum,";
			}
			command+="EnglishComments,ClassType,English,IsObsolete) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(language.LanguageNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramEnglishComments,"
				+    DbHelper.ParamChar+"paramClassType,"
				+    DbHelper.ParamChar+"paramEnglish,"
				+    POut.Bool  (language.IsObsolete)+")";
			if(language.EnglishComments==null) {
				language.EnglishComments="";
			}
			OdSqlParameter paramEnglishComments=new OdSqlParameter("paramEnglishComments",OdDbType.Text,POut.StringParam(language.EnglishComments));
			if(language.ClassType==null) {
				language.ClassType="";
			}
			OdSqlParameter paramClassType=new OdSqlParameter("paramClassType",OdDbType.Text,POut.StringParam(language.ClassType));
			if(language.English==null) {
				language.English="";
			}
			OdSqlParameter paramEnglish=new OdSqlParameter("paramEnglish",OdDbType.Text,POut.StringParam(language.English));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramEnglishComments,paramClassType,paramEnglish);
			}
			else {
				language.LanguageNum=Db.NonQ(command,true,"LanguageNum","language",paramEnglishComments,paramClassType,paramEnglish);
			}
			return language.LanguageNum;
		}

		///<summary>Updates one Language in the database.</summary>
		public static void Update(Language language) {
			string command="UPDATE language SET "
				+"EnglishComments=  "+DbHelper.ParamChar+"paramEnglishComments, "
				+"ClassType      =  "+DbHelper.ParamChar+"paramClassType, "
				+"English        =  "+DbHelper.ParamChar+"paramEnglish, "
				+"IsObsolete     =  "+POut.Bool  (language.IsObsolete)+" "
				+"WHERE LanguageNum = "+POut.Long(language.LanguageNum);
			if(language.EnglishComments==null) {
				language.EnglishComments="";
			}
			OdSqlParameter paramEnglishComments=new OdSqlParameter("paramEnglishComments",OdDbType.Text,POut.StringParam(language.EnglishComments));
			if(language.ClassType==null) {
				language.ClassType="";
			}
			OdSqlParameter paramClassType=new OdSqlParameter("paramClassType",OdDbType.Text,POut.StringParam(language.ClassType));
			if(language.English==null) {
				language.English="";
			}
			OdSqlParameter paramEnglish=new OdSqlParameter("paramEnglish",OdDbType.Text,POut.StringParam(language.English));
			Db.NonQ(command,paramEnglishComments,paramClassType,paramEnglish);
		}

		///<summary>Updates one Language in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Language language,Language oldLanguage) {
			string command="";
			if(language.EnglishComments != oldLanguage.EnglishComments) {
				if(command!="") { command+=",";}
				command+="EnglishComments = "+DbHelper.ParamChar+"paramEnglishComments";
			}
			if(language.ClassType != oldLanguage.ClassType) {
				if(command!="") { command+=",";}
				command+="ClassType = "+DbHelper.ParamChar+"paramClassType";
			}
			if(language.English != oldLanguage.English) {
				if(command!="") { command+=",";}
				command+="English = "+DbHelper.ParamChar+"paramEnglish";
			}
			if(language.IsObsolete != oldLanguage.IsObsolete) {
				if(command!="") { command+=",";}
				command+="IsObsolete = "+POut.Bool(language.IsObsolete)+"";
			}
			if(command=="") {
				return false;
			}
			if(language.EnglishComments==null) {
				language.EnglishComments="";
			}
			OdSqlParameter paramEnglishComments=new OdSqlParameter("paramEnglishComments",OdDbType.Text,POut.StringParam(language.EnglishComments));
			if(language.ClassType==null) {
				language.ClassType="";
			}
			OdSqlParameter paramClassType=new OdSqlParameter("paramClassType",OdDbType.Text,POut.StringParam(language.ClassType));
			if(language.English==null) {
				language.English="";
			}
			OdSqlParameter paramEnglish=new OdSqlParameter("paramEnglish",OdDbType.Text,POut.StringParam(language.English));
			command="UPDATE language SET "+command
				+" WHERE LanguageNum = "+POut.Long(language.LanguageNum);
			Db.NonQ(command,paramEnglishComments,paramClassType,paramEnglish);
			return true;
		}

		///<summary>Returns true if Update(Language,Language) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Language language,Language oldLanguage) {
			if(language.EnglishComments != oldLanguage.EnglishComments) {
				return true;
			}
			if(language.ClassType != oldLanguage.ClassType) {
				return true;
			}
			if(language.English != oldLanguage.English) {
				return true;
			}
			if(language.IsObsolete != oldLanguage.IsObsolete) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Language from the database.</summary>
		public static void Delete(long languageNum) {
			string command="DELETE FROM language "
				+"WHERE LanguageNum = "+POut.Long(languageNum);
			Db.NonQ(command);
		}

	}
}