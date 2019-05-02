//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class GradingScaleItemCrud {
		///<summary>Gets one GradingScaleItem object from the database using the primary key.  Returns null if not found.</summary>
		public static GradingScaleItem SelectOne(long gradingScaleItemNum) {
			string command="SELECT * FROM gradingscaleitem "
				+"WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItemNum);
			List<GradingScaleItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one GradingScaleItem object from the database using a query.</summary>
		public static GradingScaleItem SelectOne(string command) {
			
			List<GradingScaleItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of GradingScaleItem objects from the database using a query.</summary>
		public static List<GradingScaleItem> SelectMany(string command) {
			
			List<GradingScaleItem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<GradingScaleItem> TableToList(DataTable table) {
			List<GradingScaleItem> retVal=new List<GradingScaleItem>();
			GradingScaleItem gradingScaleItem;
			foreach(DataRow row in table.Rows) {
				gradingScaleItem=new GradingScaleItem();
				gradingScaleItem.GradingScaleItemNum= PIn.Long  (row["GradingScaleItemNum"].ToString());
				gradingScaleItem.GradingScaleNum    = PIn.Long  (row["GradingScaleNum"].ToString());
				gradingScaleItem.GradeShowing       = PIn.String(row["GradeShowing"].ToString());
				gradingScaleItem.GradeNumber        = PIn.Float (row["GradeNumber"].ToString());
				gradingScaleItem.Description        = PIn.String(row["Description"].ToString());
				retVal.Add(gradingScaleItem);
			}
			return retVal;
		}

		///<summary>Converts a list of GradingScaleItem into a DataTable.</summary>
		public static DataTable ListToTable(List<GradingScaleItem> listGradingScaleItems,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="GradingScaleItem";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("GradingScaleItemNum");
			table.Columns.Add("GradingScaleNum");
			table.Columns.Add("GradeShowing");
			table.Columns.Add("GradeNumber");
			table.Columns.Add("Description");
			foreach(GradingScaleItem gradingScaleItem in listGradingScaleItems) {
				table.Rows.Add(new object[] {
					POut.Long  (gradingScaleItem.GradingScaleItemNum),
					POut.Long  (gradingScaleItem.GradingScaleNum),
					            gradingScaleItem.GradeShowing,
					POut.Float (gradingScaleItem.GradeNumber),
					            gradingScaleItem.Description,
				});
			}
			return table;
		}

		///<summary>Inserts one GradingScaleItem into the database.  Returns the new priKey.</summary>
		public static long Insert(GradingScaleItem gradingScaleItem) {
			return Insert(gradingScaleItem,false);
		}

		///<summary>Inserts one GradingScaleItem into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(GradingScaleItem gradingScaleItem,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				gradingScaleItem.GradingScaleItemNum=ReplicationServers.GetKey("gradingscaleitem","GradingScaleItemNum");
			}
			string command="INSERT INTO gradingscaleitem (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="GradingScaleItemNum,";
			}
			command+="GradingScaleNum,GradeShowing,GradeNumber,Description) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(gradingScaleItem.GradingScaleItemNum)+",";
			}
			command+=
				     POut.Long  (gradingScaleItem.GradingScaleNum)+","
				+"'"+POut.String(gradingScaleItem.GradeShowing)+"',"
				+    POut.Float (gradingScaleItem.GradeNumber)+","
				+"'"+POut.String(gradingScaleItem.Description)+"')";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				gradingScaleItem.GradingScaleItemNum=Db.NonQ(command,true,"GradingScaleItemNum","gradingScaleItem");
			}
			return gradingScaleItem.GradingScaleItemNum;
		}

		///<summary>Inserts one GradingScaleItem into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(GradingScaleItem gradingScaleItem) {
			return InsertNoCache(gradingScaleItem,false);
		}

		///<summary>Inserts one GradingScaleItem into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(GradingScaleItem gradingScaleItem,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO gradingscaleitem (";
			if(!useExistingPK && isRandomKeys) {
				gradingScaleItem.GradingScaleItemNum=ReplicationServers.GetKeyNoCache("gradingscaleitem","GradingScaleItemNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="GradingScaleItemNum,";
			}
			command+="GradingScaleNum,GradeShowing,GradeNumber,Description) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(gradingScaleItem.GradingScaleItemNum)+",";
			}
			command+=
				     POut.Long  (gradingScaleItem.GradingScaleNum)+","
				+"'"+POut.String(gradingScaleItem.GradeShowing)+"',"
				+    POut.Float (gradingScaleItem.GradeNumber)+","
				+"'"+POut.String(gradingScaleItem.Description)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				gradingScaleItem.GradingScaleItemNum=Db.NonQ(command,true,"GradingScaleItemNum","gradingScaleItem");
			}
			return gradingScaleItem.GradingScaleItemNum;
		}

		///<summary>Updates one GradingScaleItem in the database.</summary>
		public static void Update(GradingScaleItem gradingScaleItem) {
			string command="UPDATE gradingscaleitem SET "
				+"GradingScaleNum    =  "+POut.Long  (gradingScaleItem.GradingScaleNum)+", "
				+"GradeShowing       = '"+POut.String(gradingScaleItem.GradeShowing)+"', "
				+"GradeNumber        =  "+POut.Float (gradingScaleItem.GradeNumber)+", "
				+"Description        = '"+POut.String(gradingScaleItem.Description)+"' "
				+"WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItem.GradingScaleItemNum);
			Db.NonQ(command);
		}

		///<summary>Updates one GradingScaleItem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(GradingScaleItem gradingScaleItem,GradingScaleItem oldGradingScaleItem) {
			string command="";
			if(gradingScaleItem.GradingScaleNum != oldGradingScaleItem.GradingScaleNum) {
				if(command!="") { command+=",";}
				command+="GradingScaleNum = "+POut.Long(gradingScaleItem.GradingScaleNum)+"";
			}
			if(gradingScaleItem.GradeShowing != oldGradingScaleItem.GradeShowing) {
				if(command!="") { command+=",";}
				command+="GradeShowing = '"+POut.String(gradingScaleItem.GradeShowing)+"'";
			}
			if(gradingScaleItem.GradeNumber != oldGradingScaleItem.GradeNumber) {
				if(command!="") { command+=",";}
				command+="GradeNumber = "+POut.Float(gradingScaleItem.GradeNumber)+"";
			}
			if(gradingScaleItem.Description != oldGradingScaleItem.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(gradingScaleItem.Description)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE gradingscaleitem SET "+command
				+" WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItem.GradingScaleItemNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(GradingScaleItem,GradingScaleItem) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(GradingScaleItem gradingScaleItem,GradingScaleItem oldGradingScaleItem) {
			if(gradingScaleItem.GradingScaleNum != oldGradingScaleItem.GradingScaleNum) {
				return true;
			}
			if(gradingScaleItem.GradeShowing != oldGradingScaleItem.GradeShowing) {
				return true;
			}
			if(gradingScaleItem.GradeNumber != oldGradingScaleItem.GradeNumber) {
				return true;
			}
			if(gradingScaleItem.Description != oldGradingScaleItem.Description) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one GradingScaleItem from the database.</summary>
		public static void Delete(long gradingScaleItemNum) {
			string command="DELETE FROM gradingscaleitem "
				+"WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItemNum);
			Db.NonQ(command);
		}

	}
}