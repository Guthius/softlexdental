//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProgramCrud {
		///<summary>Gets one Program object from the database using the primary key.  Returns null if not found.</summary>
		public static Program SelectOne(long programNum) {
			string command="SELECT * FROM program "
				+"WHERE ProgramNum = "+POut.Long(programNum);
			List<Program> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Program object from the database using a query.</summary>
		public static Program SelectOne(string command) {
			
			List<Program> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Program objects from the database using a query.</summary>
		public static List<Program> SelectMany(string command) {
			
			List<Program> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Program> TableToList(DataTable table) {
			List<Program> retVal=new List<Program>();
			Program program;
			foreach(DataRow row in table.Rows) {
				program=new Program();
				program.ProgramNum   = PIn.Long  (row["ProgramNum"].ToString());
				program.ProgName     = PIn.String(row["ProgName"].ToString());
				program.ProgDesc     = PIn.String(row["ProgDesc"].ToString());
				program.Enabled      = PIn.Bool  (row["Enabled"].ToString());
				program.Path         = PIn.String(row["Path"].ToString());
				program.CommandLine  = PIn.String(row["CommandLine"].ToString());
				program.Note         = PIn.String(row["Note"].ToString());
				program.PluginDllName= PIn.String(row["PluginDllName"].ToString());
				program.ButtonImage  = PIn.String(row["ButtonImage"].ToString());
				program.FileTemplate = PIn.String(row["FileTemplate"].ToString());
				program.FilePath     = PIn.String(row["FilePath"].ToString());
				retVal.Add(program);
			}
			return retVal;
		}

		///<summary>Converts a list of Program into a DataTable.</summary>
		public static DataTable ListToTable(List<Program> listPrograms,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Program";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProgramNum");
			table.Columns.Add("ProgName");
			table.Columns.Add("ProgDesc");
			table.Columns.Add("Enabled");
			table.Columns.Add("Path");
			table.Columns.Add("CommandLine");
			table.Columns.Add("Note");
			table.Columns.Add("PluginDllName");
			table.Columns.Add("ButtonImage");
			table.Columns.Add("FileTemplate");
			table.Columns.Add("FilePath");
			foreach(Program program in listPrograms) {
				table.Rows.Add(new object[] {
					POut.Long  (program.ProgramNum),
					            program.ProgName,
					            program.ProgDesc,
					POut.Bool  (program.Enabled),
					            program.Path,
					            program.CommandLine,
					            program.Note,
					            program.PluginDllName,
					            program.ButtonImage,
					            program.FileTemplate,
					            program.FilePath,
				});
			}
			return table;
		}

		///<summary>Inserts one Program into the database.  Returns the new priKey.</summary>
		public static long Insert(Program program) {
			return Insert(program,false);
		}

		///<summary>Inserts one Program into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Program program,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				program.ProgramNum=ReplicationServers.GetKey("program","ProgramNum");
			}
			string command="INSERT INTO program (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ProgramNum,";
			}
			command+="ProgName,ProgDesc,Enabled,Path,CommandLine,Note,PluginDllName,ButtonImage,FileTemplate,FilePath) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(program.ProgramNum)+",";
			}
			command+=
				 "'"+POut.String(program.ProgName)+"',"
				+"'"+POut.String(program.ProgDesc)+"',"
				+    POut.Bool  (program.Enabled)+","
				+"'"+POut.String(program.Path)+"',"
				+"'"+POut.String(program.CommandLine)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(program.PluginDllName)+"',"
				+    DbHelper.ParamChar+"paramButtonImage,"
				+    DbHelper.ParamChar+"paramFileTemplate,"
				+"'"+POut.String(program.FilePath)+"')";
			if(program.Note==null) {
				program.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(program.Note));
			if(program.ButtonImage==null) {
				program.ButtonImage="";
			}
			OdSqlParameter paramButtonImage=new OdSqlParameter("paramButtonImage",OdDbType.Text,POut.StringParam(program.ButtonImage));
			if(program.FileTemplate==null) {
				program.FileTemplate="";
			}
			OdSqlParameter paramFileTemplate=new OdSqlParameter("paramFileTemplate",OdDbType.Text,POut.StringParam(program.FileTemplate));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNote,paramButtonImage,paramFileTemplate);
			}
			else {
				program.ProgramNum=Db.NonQ(command,true,"ProgramNum","program",paramNote,paramButtonImage,paramFileTemplate);
			}
			return program.ProgramNum;
		}

		///<summary>Inserts one Program into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Program program) {
			return InsertNoCache(program,false);
		}

		///<summary>Inserts one Program into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Program program,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO program (";
			if(!useExistingPK && isRandomKeys) {
				program.ProgramNum=ReplicationServers.GetKeyNoCache("program","ProgramNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProgramNum,";
			}
			command+="ProgName,ProgDesc,Enabled,Path,CommandLine,Note,PluginDllName,ButtonImage,FileTemplate,FilePath) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(program.ProgramNum)+",";
			}
			command+=
				 "'"+POut.String(program.ProgName)+"',"
				+"'"+POut.String(program.ProgDesc)+"',"
				+    POut.Bool  (program.Enabled)+","
				+"'"+POut.String(program.Path)+"',"
				+"'"+POut.String(program.CommandLine)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(program.PluginDllName)+"',"
				+    DbHelper.ParamChar+"paramButtonImage,"
				+    DbHelper.ParamChar+"paramFileTemplate,"
				+"'"+POut.String(program.FilePath)+"')";
			if(program.Note==null) {
				program.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(program.Note));
			if(program.ButtonImage==null) {
				program.ButtonImage="";
			}
			OdSqlParameter paramButtonImage=new OdSqlParameter("paramButtonImage",OdDbType.Text,POut.StringParam(program.ButtonImage));
			if(program.FileTemplate==null) {
				program.FileTemplate="";
			}
			OdSqlParameter paramFileTemplate=new OdSqlParameter("paramFileTemplate",OdDbType.Text,POut.StringParam(program.FileTemplate));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote,paramButtonImage,paramFileTemplate);
			}
			else {
				program.ProgramNum=Db.NonQ(command,true,"ProgramNum","program",paramNote,paramButtonImage,paramFileTemplate);
			}
			return program.ProgramNum;
		}

		///<summary>Updates one Program in the database.</summary>
		public static void Update(Program program) {
			string command="UPDATE program SET "
				+"ProgName     = '"+POut.String(program.ProgName)+"', "
				+"ProgDesc     = '"+POut.String(program.ProgDesc)+"', "
				+"Enabled      =  "+POut.Bool  (program.Enabled)+", "
				+"Path         = '"+POut.String(program.Path)+"', "
				+"CommandLine  = '"+POut.String(program.CommandLine)+"', "
				+"Note         =  "+DbHelper.ParamChar+"paramNote, "
				+"PluginDllName= '"+POut.String(program.PluginDllName)+"', "
				+"ButtonImage  =  "+DbHelper.ParamChar+"paramButtonImage, "
				+"FileTemplate =  "+DbHelper.ParamChar+"paramFileTemplate, "
				+"FilePath     = '"+POut.String(program.FilePath)+"' "
				+"WHERE ProgramNum = "+POut.Long(program.ProgramNum);
			if(program.Note==null) {
				program.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(program.Note));
			if(program.ButtonImage==null) {
				program.ButtonImage="";
			}
			OdSqlParameter paramButtonImage=new OdSqlParameter("paramButtonImage",OdDbType.Text,POut.StringParam(program.ButtonImage));
			if(program.FileTemplate==null) {
				program.FileTemplate="";
			}
			OdSqlParameter paramFileTemplate=new OdSqlParameter("paramFileTemplate",OdDbType.Text,POut.StringParam(program.FileTemplate));
			Db.NonQ(command,paramNote,paramButtonImage,paramFileTemplate);
		}

		///<summary>Updates one Program in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Program program,Program oldProgram) {
			string command="";
			if(program.ProgName != oldProgram.ProgName) {
				if(command!="") { command+=",";}
				command+="ProgName = '"+POut.String(program.ProgName)+"'";
			}
			if(program.ProgDesc != oldProgram.ProgDesc) {
				if(command!="") { command+=",";}
				command+="ProgDesc = '"+POut.String(program.ProgDesc)+"'";
			}
			if(program.Enabled != oldProgram.Enabled) {
				if(command!="") { command+=",";}
				command+="Enabled = "+POut.Bool(program.Enabled)+"";
			}
			if(program.Path != oldProgram.Path) {
				if(command!="") { command+=",";}
				command+="Path = '"+POut.String(program.Path)+"'";
			}
			if(program.CommandLine != oldProgram.CommandLine) {
				if(command!="") { command+=",";}
				command+="CommandLine = '"+POut.String(program.CommandLine)+"'";
			}
			if(program.Note != oldProgram.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(program.PluginDllName != oldProgram.PluginDllName) {
				if(command!="") { command+=",";}
				command+="PluginDllName = '"+POut.String(program.PluginDllName)+"'";
			}
			if(program.ButtonImage != oldProgram.ButtonImage) {
				if(command!="") { command+=",";}
				command+="ButtonImage = "+DbHelper.ParamChar+"paramButtonImage";
			}
			if(program.FileTemplate != oldProgram.FileTemplate) {
				if(command!="") { command+=",";}
				command+="FileTemplate = "+DbHelper.ParamChar+"paramFileTemplate";
			}
			if(program.FilePath != oldProgram.FilePath) {
				if(command!="") { command+=",";}
				command+="FilePath = '"+POut.String(program.FilePath)+"'";
			}
			if(command=="") {
				return false;
			}
			if(program.Note==null) {
				program.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(program.Note));
			if(program.ButtonImage==null) {
				program.ButtonImage="";
			}
			OdSqlParameter paramButtonImage=new OdSqlParameter("paramButtonImage",OdDbType.Text,POut.StringParam(program.ButtonImage));
			if(program.FileTemplate==null) {
				program.FileTemplate="";
			}
			OdSqlParameter paramFileTemplate=new OdSqlParameter("paramFileTemplate",OdDbType.Text,POut.StringParam(program.FileTemplate));
			command="UPDATE program SET "+command
				+" WHERE ProgramNum = "+POut.Long(program.ProgramNum);
			Db.NonQ(command,paramNote,paramButtonImage,paramFileTemplate);
			return true;
		}

		///<summary>Returns true if Update(Program,Program) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Program program,Program oldProgram) {
			if(program.ProgName != oldProgram.ProgName) {
				return true;
			}
			if(program.ProgDesc != oldProgram.ProgDesc) {
				return true;
			}
			if(program.Enabled != oldProgram.Enabled) {
				return true;
			}
			if(program.Path != oldProgram.Path) {
				return true;
			}
			if(program.CommandLine != oldProgram.CommandLine) {
				return true;
			}
			if(program.Note != oldProgram.Note) {
				return true;
			}
			if(program.PluginDllName != oldProgram.PluginDllName) {
				return true;
			}
			if(program.ButtonImage != oldProgram.ButtonImage) {
				return true;
			}
			if(program.FileTemplate != oldProgram.FileTemplate) {
				return true;
			}
			if(program.FilePath != oldProgram.FilePath) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Program from the database.</summary>
		public static void Delete(long programNum) {
			string command="DELETE FROM program "
				+"WHERE ProgramNum = "+POut.Long(programNum);
			Db.NonQ(command);
		}

	}
}