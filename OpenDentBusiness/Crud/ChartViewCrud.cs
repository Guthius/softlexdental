//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ChartViewCrud {
		///<summary>Gets one ChartView object from the database using the primary key.  Returns null if not found.</summary>
		public static ChartView SelectOne(long chartViewNum) {
			string command="SELECT * FROM chartview "
				+"WHERE ChartViewNum = "+POut.Long(chartViewNum);
			List<ChartView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ChartView object from the database using a query.</summary>
		public static ChartView SelectOne(string command) {
			
			List<ChartView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ChartView objects from the database using a query.</summary>
		public static List<ChartView> SelectMany(string command) {
			
			List<ChartView> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ChartView> TableToList(DataTable table) {
			List<ChartView> retVal=new List<ChartView>();
			ChartView chartView;
			foreach(DataRow row in table.Rows) {
				chartView=new ChartView();
				chartView.ChartViewNum     = PIn.Long  (row["ChartViewNum"].ToString());
				chartView.Description      = PIn.String(row["Description"].ToString());
				chartView.ItemOrder        = PIn.Int   (row["ItemOrder"].ToString());
				chartView.ProcStatuses     = (OpenDentBusiness.ChartViewProcStat)PIn.Int(row["ProcStatuses"].ToString());
				chartView.ObjectTypes      = (OpenDentBusiness.ChartViewObjs)PIn.Int(row["ObjectTypes"].ToString());
				chartView.ShowProcNotes    = PIn.Bool  (row["ShowProcNotes"].ToString());
				chartView.IsAudit          = PIn.Bool  (row["IsAudit"].ToString());
				chartView.SelectedTeethOnly= PIn.Bool  (row["SelectedTeethOnly"].ToString());
				chartView.OrionStatusFlags = (OpenDentBusiness.OrionStatus)PIn.Int(row["OrionStatusFlags"].ToString());
				chartView.DatesShowing     = (OpenDentBusiness.ChartViewDates)PIn.Int(row["DatesShowing"].ToString());
				chartView.IsTpCharting     = PIn.Bool  (row["IsTpCharting"].ToString());
				retVal.Add(chartView);
			}
			return retVal;
		}

		///<summary>Converts a list of ChartView into a DataTable.</summary>
		public static DataTable ListToTable(List<ChartView> listChartViews,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ChartView";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ChartViewNum");
			table.Columns.Add("Description");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("ProcStatuses");
			table.Columns.Add("ObjectTypes");
			table.Columns.Add("ShowProcNotes");
			table.Columns.Add("IsAudit");
			table.Columns.Add("SelectedTeethOnly");
			table.Columns.Add("OrionStatusFlags");
			table.Columns.Add("DatesShowing");
			table.Columns.Add("IsTpCharting");
			foreach(ChartView chartView in listChartViews) {
				table.Rows.Add(new object[] {
					POut.Long  (chartView.ChartViewNum),
					            chartView.Description,
					POut.Int   (chartView.ItemOrder),
					POut.Int   ((int)chartView.ProcStatuses),
					POut.Int   ((int)chartView.ObjectTypes),
					POut.Bool  (chartView.ShowProcNotes),
					POut.Bool  (chartView.IsAudit),
					POut.Bool  (chartView.SelectedTeethOnly),
					POut.Int   ((int)chartView.OrionStatusFlags),
					POut.Int   ((int)chartView.DatesShowing),
					POut.Bool  (chartView.IsTpCharting),
				});
			}
			return table;
		}

		///<summary>Inserts one ChartView into the database.  Returns the new priKey.</summary>
		public static long Insert(ChartView chartView) {
			return Insert(chartView,false);
		}

		///<summary>Inserts one ChartView into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ChartView chartView,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				chartView.ChartViewNum=ReplicationServers.GetKey("chartview","ChartViewNum");
			}
			string command="INSERT INTO chartview (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ChartViewNum,";
			}
			command+="Description,ItemOrder,ProcStatuses,ObjectTypes,ShowProcNotes,IsAudit,SelectedTeethOnly,OrionStatusFlags,DatesShowing,IsTpCharting) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(chartView.ChartViewNum)+",";
			}
			command+=
				 "'"+POut.String(chartView.Description)+"',"
				+    POut.Int   (chartView.ItemOrder)+","
				+    POut.Int   ((int)chartView.ProcStatuses)+","
				+    POut.Int   ((int)chartView.ObjectTypes)+","
				+    POut.Bool  (chartView.ShowProcNotes)+","
				+    POut.Bool  (chartView.IsAudit)+","
				+    POut.Bool  (chartView.SelectedTeethOnly)+","
				+    POut.Int   ((int)chartView.OrionStatusFlags)+","
				+    POut.Int   ((int)chartView.DatesShowing)+","
				+    POut.Bool  (chartView.IsTpCharting)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				chartView.ChartViewNum=Db.NonQ(command,true,"ChartViewNum","chartView");
			}
			return chartView.ChartViewNum;
		}

		///<summary>Inserts one ChartView into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChartView chartView) {
			return InsertNoCache(chartView,false);
		}

		///<summary>Inserts one ChartView into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChartView chartView,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO chartview (";
			if(!useExistingPK && isRandomKeys) {
				chartView.ChartViewNum=ReplicationServers.GetKeyNoCache("chartview","ChartViewNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ChartViewNum,";
			}
			command+="Description,ItemOrder,ProcStatuses,ObjectTypes,ShowProcNotes,IsAudit,SelectedTeethOnly,OrionStatusFlags,DatesShowing,IsTpCharting) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(chartView.ChartViewNum)+",";
			}
			command+=
				 "'"+POut.String(chartView.Description)+"',"
				+    POut.Int   (chartView.ItemOrder)+","
				+    POut.Int   ((int)chartView.ProcStatuses)+","
				+    POut.Int   ((int)chartView.ObjectTypes)+","
				+    POut.Bool  (chartView.ShowProcNotes)+","
				+    POut.Bool  (chartView.IsAudit)+","
				+    POut.Bool  (chartView.SelectedTeethOnly)+","
				+    POut.Int   ((int)chartView.OrionStatusFlags)+","
				+    POut.Int   ((int)chartView.DatesShowing)+","
				+    POut.Bool  (chartView.IsTpCharting)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				chartView.ChartViewNum=Db.NonQ(command,true,"ChartViewNum","chartView");
			}
			return chartView.ChartViewNum;
		}

		///<summary>Updates one ChartView in the database.</summary>
		public static void Update(ChartView chartView) {
			string command="UPDATE chartview SET "
				+"Description      = '"+POut.String(chartView.Description)+"', "
				+"ItemOrder        =  "+POut.Int   (chartView.ItemOrder)+", "
				+"ProcStatuses     =  "+POut.Int   ((int)chartView.ProcStatuses)+", "
				+"ObjectTypes      =  "+POut.Int   ((int)chartView.ObjectTypes)+", "
				+"ShowProcNotes    =  "+POut.Bool  (chartView.ShowProcNotes)+", "
				+"IsAudit          =  "+POut.Bool  (chartView.IsAudit)+", "
				+"SelectedTeethOnly=  "+POut.Bool  (chartView.SelectedTeethOnly)+", "
				+"OrionStatusFlags =  "+POut.Int   ((int)chartView.OrionStatusFlags)+", "
				+"DatesShowing     =  "+POut.Int   ((int)chartView.DatesShowing)+", "
				+"IsTpCharting     =  "+POut.Bool  (chartView.IsTpCharting)+" "
				+"WHERE ChartViewNum = "+POut.Long(chartView.ChartViewNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ChartView in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ChartView chartView,ChartView oldChartView) {
			string command="";
			if(chartView.Description != oldChartView.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(chartView.Description)+"'";
			}
			if(chartView.ItemOrder != oldChartView.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(chartView.ItemOrder)+"";
			}
			if(chartView.ProcStatuses != oldChartView.ProcStatuses) {
				if(command!="") { command+=",";}
				command+="ProcStatuses = "+POut.Int   ((int)chartView.ProcStatuses)+"";
			}
			if(chartView.ObjectTypes != oldChartView.ObjectTypes) {
				if(command!="") { command+=",";}
				command+="ObjectTypes = "+POut.Int   ((int)chartView.ObjectTypes)+"";
			}
			if(chartView.ShowProcNotes != oldChartView.ShowProcNotes) {
				if(command!="") { command+=",";}
				command+="ShowProcNotes = "+POut.Bool(chartView.ShowProcNotes)+"";
			}
			if(chartView.IsAudit != oldChartView.IsAudit) {
				if(command!="") { command+=",";}
				command+="IsAudit = "+POut.Bool(chartView.IsAudit)+"";
			}
			if(chartView.SelectedTeethOnly != oldChartView.SelectedTeethOnly) {
				if(command!="") { command+=",";}
				command+="SelectedTeethOnly = "+POut.Bool(chartView.SelectedTeethOnly)+"";
			}
			if(chartView.OrionStatusFlags != oldChartView.OrionStatusFlags) {
				if(command!="") { command+=",";}
				command+="OrionStatusFlags = "+POut.Int   ((int)chartView.OrionStatusFlags)+"";
			}
			if(chartView.DatesShowing != oldChartView.DatesShowing) {
				if(command!="") { command+=",";}
				command+="DatesShowing = "+POut.Int   ((int)chartView.DatesShowing)+"";
			}
			if(chartView.IsTpCharting != oldChartView.IsTpCharting) {
				if(command!="") { command+=",";}
				command+="IsTpCharting = "+POut.Bool(chartView.IsTpCharting)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE chartview SET "+command
				+" WHERE ChartViewNum = "+POut.Long(chartView.ChartViewNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ChartView,ChartView) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ChartView chartView,ChartView oldChartView) {
			if(chartView.Description != oldChartView.Description) {
				return true;
			}
			if(chartView.ItemOrder != oldChartView.ItemOrder) {
				return true;
			}
			if(chartView.ProcStatuses != oldChartView.ProcStatuses) {
				return true;
			}
			if(chartView.ObjectTypes != oldChartView.ObjectTypes) {
				return true;
			}
			if(chartView.ShowProcNotes != oldChartView.ShowProcNotes) {
				return true;
			}
			if(chartView.IsAudit != oldChartView.IsAudit) {
				return true;
			}
			if(chartView.SelectedTeethOnly != oldChartView.SelectedTeethOnly) {
				return true;
			}
			if(chartView.OrionStatusFlags != oldChartView.OrionStatusFlags) {
				return true;
			}
			if(chartView.DatesShowing != oldChartView.DatesShowing) {
				return true;
			}
			if(chartView.IsTpCharting != oldChartView.IsTpCharting) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ChartView from the database.</summary>
		public static void Delete(long chartViewNum) {
			string command="DELETE FROM chartview "
				+"WHERE ChartViewNum = "+POut.Long(chartViewNum);
			Db.NonQ(command);
		}

	}
}