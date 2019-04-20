//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class VaccinePatCrud {
		///<summary>Gets one VaccinePat object from the database using the primary key.  Returns null if not found.</summary>
		public static VaccinePat SelectOne(long vaccinePatNum) {
			string command="SELECT * FROM vaccinepat "
				+"WHERE VaccinePatNum = "+POut.Long(vaccinePatNum);
			List<VaccinePat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one VaccinePat object from the database using a query.</summary>
		public static VaccinePat SelectOne(string command) {
			List<VaccinePat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of VaccinePat objects from the database using a query.</summary>
		public static List<VaccinePat> SelectMany(string command) {
			List<VaccinePat> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<VaccinePat> TableToList(DataTable table) {
			List<VaccinePat> retVal=new List<VaccinePat>();
			VaccinePat vaccinePat;
			foreach(DataRow row in table.Rows) {
				vaccinePat=new VaccinePat();
				vaccinePat.VaccinePatNum         = PIn.Long  (row["VaccinePatNum"].ToString());
				vaccinePat.VaccineDefNum         = PIn.Long  (row["VaccineDefNum"].ToString());
				vaccinePat.DateTimeStart         = PIn.DateT (row["DateTimeStart"].ToString());
				vaccinePat.DateTimeEnd           = PIn.DateT (row["DateTimeEnd"].ToString());
				vaccinePat.AdministeredAmt       = PIn.Float (row["AdministeredAmt"].ToString());
				vaccinePat.DrugUnitNum           = PIn.Long  (row["DrugUnitNum"].ToString());
				vaccinePat.LotNumber             = PIn.String(row["LotNumber"].ToString());
				vaccinePat.PatNum                = PIn.Long  (row["PatNum"].ToString());
				vaccinePat.Note                  = PIn.String(row["Note"].ToString());
				vaccinePat.FilledCity            = PIn.String(row["FilledCity"].ToString());
				vaccinePat.FilledST              = PIn.String(row["FilledST"].ToString());
				vaccinePat.CompletionStatus      = (OpenDentBusiness.VaccineCompletionStatus)PIn.Int(row["CompletionStatus"].ToString());
				vaccinePat.AdministrationNoteCode= (OpenDentBusiness.VaccineAdministrationNote)PIn.Int(row["AdministrationNoteCode"].ToString());
				vaccinePat.UserNum               = PIn.Long  (row["UserNum"].ToString());
				vaccinePat.ProvNumOrdering       = PIn.Long  (row["ProvNumOrdering"].ToString());
				vaccinePat.ProvNumAdminister     = PIn.Long  (row["ProvNumAdminister"].ToString());
				vaccinePat.DateExpire            = PIn.Date  (row["DateExpire"].ToString());
				vaccinePat.RefusalReason         = (OpenDentBusiness.VaccineRefusalReason)PIn.Int(row["RefusalReason"].ToString());
				vaccinePat.ActionCode            = (OpenDentBusiness.VaccineAction)PIn.Int(row["ActionCode"].ToString());
				vaccinePat.AdministrationRoute   = (OpenDentBusiness.VaccineAdministrationRoute)PIn.Int(row["AdministrationRoute"].ToString());
				vaccinePat.AdministrationSite    = (OpenDentBusiness.VaccineAdministrationSite)PIn.Int(row["AdministrationSite"].ToString());
				retVal.Add(vaccinePat);
			}
			return retVal;
		}

		///<summary>Converts a list of VaccinePat into a DataTable.</summary>
		public static DataTable ListToTable(List<VaccinePat> listVaccinePats,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="VaccinePat";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("VaccinePatNum");
			table.Columns.Add("VaccineDefNum");
			table.Columns.Add("DateTimeStart");
			table.Columns.Add("DateTimeEnd");
			table.Columns.Add("AdministeredAmt");
			table.Columns.Add("DrugUnitNum");
			table.Columns.Add("LotNumber");
			table.Columns.Add("PatNum");
			table.Columns.Add("Note");
			table.Columns.Add("FilledCity");
			table.Columns.Add("FilledST");
			table.Columns.Add("CompletionStatus");
			table.Columns.Add("AdministrationNoteCode");
			table.Columns.Add("UserNum");
			table.Columns.Add("ProvNumOrdering");
			table.Columns.Add("ProvNumAdminister");
			table.Columns.Add("DateExpire");
			table.Columns.Add("RefusalReason");
			table.Columns.Add("ActionCode");
			table.Columns.Add("AdministrationRoute");
			table.Columns.Add("AdministrationSite");
			foreach(VaccinePat vaccinePat in listVaccinePats) {
				table.Rows.Add(new object[] {
					POut.Long  (vaccinePat.VaccinePatNum),
					POut.Long  (vaccinePat.VaccineDefNum),
					POut.DateT (vaccinePat.DateTimeStart,false),
					POut.DateT (vaccinePat.DateTimeEnd,false),
					POut.Float (vaccinePat.AdministeredAmt),
					POut.Long  (vaccinePat.DrugUnitNum),
					            vaccinePat.LotNumber,
					POut.Long  (vaccinePat.PatNum),
					            vaccinePat.Note,
					            vaccinePat.FilledCity,
					            vaccinePat.FilledST,
					POut.Int   ((int)vaccinePat.CompletionStatus),
					POut.Int   ((int)vaccinePat.AdministrationNoteCode),
					POut.Long  (vaccinePat.UserNum),
					POut.Long  (vaccinePat.ProvNumOrdering),
					POut.Long  (vaccinePat.ProvNumAdminister),
					POut.DateT (vaccinePat.DateExpire,false),
					POut.Int   ((int)vaccinePat.RefusalReason),
					POut.Int   ((int)vaccinePat.ActionCode),
					POut.Int   ((int)vaccinePat.AdministrationRoute),
					POut.Int   ((int)vaccinePat.AdministrationSite),
				});
			}
			return table;
		}

		///<summary>Inserts one VaccinePat into the database.  Returns the new priKey.</summary>
		public static long Insert(VaccinePat vaccinePat) {
			return Insert(vaccinePat,false);
		}

		///<summary>Inserts one VaccinePat into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(VaccinePat vaccinePat,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				vaccinePat.VaccinePatNum=ReplicationServers.GetKey("vaccinepat","VaccinePatNum");
			}
			string command="INSERT INTO vaccinepat (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="VaccinePatNum,";
			}
			command+="VaccineDefNum,DateTimeStart,DateTimeEnd,AdministeredAmt,DrugUnitNum,LotNumber,PatNum,Note,FilledCity,FilledST,CompletionStatus,AdministrationNoteCode,UserNum,ProvNumOrdering,ProvNumAdminister,DateExpire,RefusalReason,ActionCode,AdministrationRoute,AdministrationSite) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(vaccinePat.VaccinePatNum)+",";
			}
			command+=
				     POut.Long  (vaccinePat.VaccineDefNum)+","
				+    POut.DateT (vaccinePat.DateTimeStart)+","
				+    POut.DateT (vaccinePat.DateTimeEnd)+","
				+    POut.Float (vaccinePat.AdministeredAmt)+","
				+    POut.Long  (vaccinePat.DrugUnitNum)+","
				+"'"+POut.String(vaccinePat.LotNumber)+"',"
				+    POut.Long  (vaccinePat.PatNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(vaccinePat.FilledCity)+"',"
				+"'"+POut.String(vaccinePat.FilledST)+"',"
				+    POut.Int   ((int)vaccinePat.CompletionStatus)+","
				+    POut.Int   ((int)vaccinePat.AdministrationNoteCode)+","
				+    POut.Long  (vaccinePat.UserNum)+","
				+    POut.Long  (vaccinePat.ProvNumOrdering)+","
				+    POut.Long  (vaccinePat.ProvNumAdminister)+","
				+    POut.Date  (vaccinePat.DateExpire)+","
				+    POut.Int   ((int)vaccinePat.RefusalReason)+","
				+    POut.Int   ((int)vaccinePat.ActionCode)+","
				+    POut.Int   ((int)vaccinePat.AdministrationRoute)+","
				+    POut.Int   ((int)vaccinePat.AdministrationSite)+")";
			if(vaccinePat.Note==null) {
				vaccinePat.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(vaccinePat.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				vaccinePat.VaccinePatNum=Db.NonQ(command,true,"VaccinePatNum","vaccinePat",paramNote);
			}
			return vaccinePat.VaccinePatNum;
		}

		///<summary>Inserts one VaccinePat into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(VaccinePat vaccinePat) {
			return InsertNoCache(vaccinePat,false);
		}

		///<summary>Inserts one VaccinePat into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(VaccinePat vaccinePat,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO vaccinepat (";
			if(!useExistingPK && isRandomKeys) {
				vaccinePat.VaccinePatNum=ReplicationServers.GetKeyNoCache("vaccinepat","VaccinePatNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="VaccinePatNum,";
			}
			command+="VaccineDefNum,DateTimeStart,DateTimeEnd,AdministeredAmt,DrugUnitNum,LotNumber,PatNum,Note,FilledCity,FilledST,CompletionStatus,AdministrationNoteCode,UserNum,ProvNumOrdering,ProvNumAdminister,DateExpire,RefusalReason,ActionCode,AdministrationRoute,AdministrationSite) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(vaccinePat.VaccinePatNum)+",";
			}
			command+=
				     POut.Long  (vaccinePat.VaccineDefNum)+","
				+    POut.DateT (vaccinePat.DateTimeStart)+","
				+    POut.DateT (vaccinePat.DateTimeEnd)+","
				+    POut.Float (vaccinePat.AdministeredAmt)+","
				+    POut.Long  (vaccinePat.DrugUnitNum)+","
				+"'"+POut.String(vaccinePat.LotNumber)+"',"
				+    POut.Long  (vaccinePat.PatNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(vaccinePat.FilledCity)+"',"
				+"'"+POut.String(vaccinePat.FilledST)+"',"
				+    POut.Int   ((int)vaccinePat.CompletionStatus)+","
				+    POut.Int   ((int)vaccinePat.AdministrationNoteCode)+","
				+    POut.Long  (vaccinePat.UserNum)+","
				+    POut.Long  (vaccinePat.ProvNumOrdering)+","
				+    POut.Long  (vaccinePat.ProvNumAdminister)+","
				+    POut.Date  (vaccinePat.DateExpire)+","
				+    POut.Int   ((int)vaccinePat.RefusalReason)+","
				+    POut.Int   ((int)vaccinePat.ActionCode)+","
				+    POut.Int   ((int)vaccinePat.AdministrationRoute)+","
				+    POut.Int   ((int)vaccinePat.AdministrationSite)+")";
			if(vaccinePat.Note==null) {
				vaccinePat.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(vaccinePat.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				vaccinePat.VaccinePatNum=Db.NonQ(command,true,"VaccinePatNum","vaccinePat",paramNote);
			}
			return vaccinePat.VaccinePatNum;
		}

		///<summary>Updates one VaccinePat in the database.</summary>
		public static void Update(VaccinePat vaccinePat) {
			string command="UPDATE vaccinepat SET "
				+"VaccineDefNum         =  "+POut.Long  (vaccinePat.VaccineDefNum)+", "
				+"DateTimeStart         =  "+POut.DateT (vaccinePat.DateTimeStart)+", "
				+"DateTimeEnd           =  "+POut.DateT (vaccinePat.DateTimeEnd)+", "
				+"AdministeredAmt       =  "+POut.Float (vaccinePat.AdministeredAmt)+", "
				+"DrugUnitNum           =  "+POut.Long  (vaccinePat.DrugUnitNum)+", "
				+"LotNumber             = '"+POut.String(vaccinePat.LotNumber)+"', "
				+"PatNum                =  "+POut.Long  (vaccinePat.PatNum)+", "
				+"Note                  =  "+DbHelper.ParamChar+"paramNote, "
				+"FilledCity            = '"+POut.String(vaccinePat.FilledCity)+"', "
				+"FilledST              = '"+POut.String(vaccinePat.FilledST)+"', "
				+"CompletionStatus      =  "+POut.Int   ((int)vaccinePat.CompletionStatus)+", "
				+"AdministrationNoteCode=  "+POut.Int   ((int)vaccinePat.AdministrationNoteCode)+", "
				+"UserNum               =  "+POut.Long  (vaccinePat.UserNum)+", "
				+"ProvNumOrdering       =  "+POut.Long  (vaccinePat.ProvNumOrdering)+", "
				+"ProvNumAdminister     =  "+POut.Long  (vaccinePat.ProvNumAdminister)+", "
				+"DateExpire            =  "+POut.Date  (vaccinePat.DateExpire)+", "
				+"RefusalReason         =  "+POut.Int   ((int)vaccinePat.RefusalReason)+", "
				+"ActionCode            =  "+POut.Int   ((int)vaccinePat.ActionCode)+", "
				+"AdministrationRoute   =  "+POut.Int   ((int)vaccinePat.AdministrationRoute)+", "
				+"AdministrationSite    =  "+POut.Int   ((int)vaccinePat.AdministrationSite)+" "
				+"WHERE VaccinePatNum = "+POut.Long(vaccinePat.VaccinePatNum);
			if(vaccinePat.Note==null) {
				vaccinePat.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(vaccinePat.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one VaccinePat in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(VaccinePat vaccinePat,VaccinePat oldVaccinePat) {
			string command="";
			if(vaccinePat.VaccineDefNum != oldVaccinePat.VaccineDefNum) {
				if(command!="") { command+=",";}
				command+="VaccineDefNum = "+POut.Long(vaccinePat.VaccineDefNum)+"";
			}
			if(vaccinePat.DateTimeStart != oldVaccinePat.DateTimeStart) {
				if(command!="") { command+=",";}
				command+="DateTimeStart = "+POut.DateT(vaccinePat.DateTimeStart)+"";
			}
			if(vaccinePat.DateTimeEnd != oldVaccinePat.DateTimeEnd) {
				if(command!="") { command+=",";}
				command+="DateTimeEnd = "+POut.DateT(vaccinePat.DateTimeEnd)+"";
			}
			if(vaccinePat.AdministeredAmt != oldVaccinePat.AdministeredAmt) {
				if(command!="") { command+=",";}
				command+="AdministeredAmt = "+POut.Float(vaccinePat.AdministeredAmt)+"";
			}
			if(vaccinePat.DrugUnitNum != oldVaccinePat.DrugUnitNum) {
				if(command!="") { command+=",";}
				command+="DrugUnitNum = "+POut.Long(vaccinePat.DrugUnitNum)+"";
			}
			if(vaccinePat.LotNumber != oldVaccinePat.LotNumber) {
				if(command!="") { command+=",";}
				command+="LotNumber = '"+POut.String(vaccinePat.LotNumber)+"'";
			}
			if(vaccinePat.PatNum != oldVaccinePat.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(vaccinePat.PatNum)+"";
			}
			if(vaccinePat.Note != oldVaccinePat.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(vaccinePat.FilledCity != oldVaccinePat.FilledCity) {
				if(command!="") { command+=",";}
				command+="FilledCity = '"+POut.String(vaccinePat.FilledCity)+"'";
			}
			if(vaccinePat.FilledST != oldVaccinePat.FilledST) {
				if(command!="") { command+=",";}
				command+="FilledST = '"+POut.String(vaccinePat.FilledST)+"'";
			}
			if(vaccinePat.CompletionStatus != oldVaccinePat.CompletionStatus) {
				if(command!="") { command+=",";}
				command+="CompletionStatus = "+POut.Int   ((int)vaccinePat.CompletionStatus)+"";
			}
			if(vaccinePat.AdministrationNoteCode != oldVaccinePat.AdministrationNoteCode) {
				if(command!="") { command+=",";}
				command+="AdministrationNoteCode = "+POut.Int   ((int)vaccinePat.AdministrationNoteCode)+"";
			}
			if(vaccinePat.UserNum != oldVaccinePat.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(vaccinePat.UserNum)+"";
			}
			if(vaccinePat.ProvNumOrdering != oldVaccinePat.ProvNumOrdering) {
				if(command!="") { command+=",";}
				command+="ProvNumOrdering = "+POut.Long(vaccinePat.ProvNumOrdering)+"";
			}
			if(vaccinePat.ProvNumAdminister != oldVaccinePat.ProvNumAdminister) {
				if(command!="") { command+=",";}
				command+="ProvNumAdminister = "+POut.Long(vaccinePat.ProvNumAdminister)+"";
			}
			if(vaccinePat.DateExpire.Date != oldVaccinePat.DateExpire.Date) {
				if(command!="") { command+=",";}
				command+="DateExpire = "+POut.Date(vaccinePat.DateExpire)+"";
			}
			if(vaccinePat.RefusalReason != oldVaccinePat.RefusalReason) {
				if(command!="") { command+=",";}
				command+="RefusalReason = "+POut.Int   ((int)vaccinePat.RefusalReason)+"";
			}
			if(vaccinePat.ActionCode != oldVaccinePat.ActionCode) {
				if(command!="") { command+=",";}
				command+="ActionCode = "+POut.Int   ((int)vaccinePat.ActionCode)+"";
			}
			if(vaccinePat.AdministrationRoute != oldVaccinePat.AdministrationRoute) {
				if(command!="") { command+=",";}
				command+="AdministrationRoute = "+POut.Int   ((int)vaccinePat.AdministrationRoute)+"";
			}
			if(vaccinePat.AdministrationSite != oldVaccinePat.AdministrationSite) {
				if(command!="") { command+=",";}
				command+="AdministrationSite = "+POut.Int   ((int)vaccinePat.AdministrationSite)+"";
			}
			if(command=="") {
				return false;
			}
			if(vaccinePat.Note==null) {
				vaccinePat.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(vaccinePat.Note));
			command="UPDATE vaccinepat SET "+command
				+" WHERE VaccinePatNum = "+POut.Long(vaccinePat.VaccinePatNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(VaccinePat,VaccinePat) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(VaccinePat vaccinePat,VaccinePat oldVaccinePat) {
			if(vaccinePat.VaccineDefNum != oldVaccinePat.VaccineDefNum) {
				return true;
			}
			if(vaccinePat.DateTimeStart != oldVaccinePat.DateTimeStart) {
				return true;
			}
			if(vaccinePat.DateTimeEnd != oldVaccinePat.DateTimeEnd) {
				return true;
			}
			if(vaccinePat.AdministeredAmt != oldVaccinePat.AdministeredAmt) {
				return true;
			}
			if(vaccinePat.DrugUnitNum != oldVaccinePat.DrugUnitNum) {
				return true;
			}
			if(vaccinePat.LotNumber != oldVaccinePat.LotNumber) {
				return true;
			}
			if(vaccinePat.PatNum != oldVaccinePat.PatNum) {
				return true;
			}
			if(vaccinePat.Note != oldVaccinePat.Note) {
				return true;
			}
			if(vaccinePat.FilledCity != oldVaccinePat.FilledCity) {
				return true;
			}
			if(vaccinePat.FilledST != oldVaccinePat.FilledST) {
				return true;
			}
			if(vaccinePat.CompletionStatus != oldVaccinePat.CompletionStatus) {
				return true;
			}
			if(vaccinePat.AdministrationNoteCode != oldVaccinePat.AdministrationNoteCode) {
				return true;
			}
			if(vaccinePat.UserNum != oldVaccinePat.UserNum) {
				return true;
			}
			if(vaccinePat.ProvNumOrdering != oldVaccinePat.ProvNumOrdering) {
				return true;
			}
			if(vaccinePat.ProvNumAdminister != oldVaccinePat.ProvNumAdminister) {
				return true;
			}
			if(vaccinePat.DateExpire.Date != oldVaccinePat.DateExpire.Date) {
				return true;
			}
			if(vaccinePat.RefusalReason != oldVaccinePat.RefusalReason) {
				return true;
			}
			if(vaccinePat.ActionCode != oldVaccinePat.ActionCode) {
				return true;
			}
			if(vaccinePat.AdministrationRoute != oldVaccinePat.AdministrationRoute) {
				return true;
			}
			if(vaccinePat.AdministrationSite != oldVaccinePat.AdministrationSite) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one VaccinePat from the database.</summary>
		public static void Delete(long vaccinePatNum) {
			string command="DELETE FROM vaccinepat "
				+"WHERE VaccinePatNum = "+POut.Long(vaccinePatNum);
			Db.NonQ(command);
		}

	}
}