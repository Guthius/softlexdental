//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RefAttachCrud {
		///<summary>Gets one RefAttach object from the database using the primary key.  Returns null if not found.</summary>
		public static RefAttach SelectOne(long refAttachNum) {
			string command="SELECT * FROM refattach "
				+"WHERE RefAttachNum = "+POut.Long(refAttachNum);
			List<RefAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RefAttach object from the database using a query.</summary>
		public static RefAttach SelectOne(string command) {
			
			List<RefAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RefAttach objects from the database using a query.</summary>
		public static List<RefAttach> SelectMany(string command) {
			
			List<RefAttach> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RefAttach> TableToList(DataTable table) {
			List<RefAttach> retVal=new List<RefAttach>();
			RefAttach refAttach;
			foreach(DataRow row in table.Rows) {
				refAttach=new RefAttach();
				refAttach.RefAttachNum      = PIn.Long  (row["RefAttachNum"].ToString());
				refAttach.ReferralNum       = PIn.Long  (row["ReferralNum"].ToString());
				refAttach.PatNum            = PIn.Long  (row["PatNum"].ToString());
				refAttach.ItemOrder         = PIn.Int   (row["ItemOrder"].ToString());
				refAttach.RefDate           = PIn.Date  (row["RefDate"].ToString());
				refAttach.RefType           = (OpenDentBusiness.ReferralType)PIn.Int(row["RefType"].ToString());
				refAttach.RefToStatus       = (OpenDentBusiness.ReferralToStatus)PIn.Int(row["RefToStatus"].ToString());
				refAttach.Note              = PIn.String(row["Note"].ToString());
				refAttach.IsTransitionOfCare= PIn.Bool  (row["IsTransitionOfCare"].ToString());
				refAttach.ProcNum           = PIn.Long  (row["ProcNum"].ToString());
				refAttach.DateProcComplete  = PIn.Date  (row["DateProcComplete"].ToString());
				refAttach.ProvNum           = PIn.Long  (row["ProvNum"].ToString());
				refAttach.DateTStamp        = PIn.DateT (row["DateTStamp"].ToString());
				retVal.Add(refAttach);
			}
			return retVal;
		}

		///<summary>Converts a list of RefAttach into a DataTable.</summary>
		public static DataTable ListToTable(List<RefAttach> listRefAttachs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RefAttach";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RefAttachNum");
			table.Columns.Add("ReferralNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("RefDate");
			table.Columns.Add("RefType");
			table.Columns.Add("RefToStatus");
			table.Columns.Add("Note");
			table.Columns.Add("IsTransitionOfCare");
			table.Columns.Add("ProcNum");
			table.Columns.Add("DateProcComplete");
			table.Columns.Add("ProvNum");
			table.Columns.Add("DateTStamp");
			foreach(RefAttach refAttach in listRefAttachs) {
				table.Rows.Add(new object[] {
					POut.Long  (refAttach.RefAttachNum),
					POut.Long  (refAttach.ReferralNum),
					POut.Long  (refAttach.PatNum),
					POut.Int   (refAttach.ItemOrder),
					POut.DateT (refAttach.RefDate,false),
					POut.Int   ((int)refAttach.RefType),
					POut.Int   ((int)refAttach.RefToStatus),
					            refAttach.Note,
					POut.Bool  (refAttach.IsTransitionOfCare),
					POut.Long  (refAttach.ProcNum),
					POut.DateT (refAttach.DateProcComplete,false),
					POut.Long  (refAttach.ProvNum),
					POut.DateT (refAttach.DateTStamp,false),
				});
			}
			return table;
		}

		///<summary>Inserts one RefAttach into the database.  Returns the new priKey.</summary>
		public static long Insert(RefAttach refAttach) {
			return Insert(refAttach,false);
		}

		///<summary>Inserts one RefAttach into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RefAttach refAttach,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				refAttach.RefAttachNum=ReplicationServers.GetKey("refattach","RefAttachNum");
			}
			string command="INSERT INTO refattach (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="RefAttachNum,";
			}
			command+="ReferralNum,PatNum,ItemOrder,RefDate,RefType,RefToStatus,Note,IsTransitionOfCare,ProcNum,DateProcComplete,ProvNum) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(refAttach.RefAttachNum)+",";
			}
			command+=
				     POut.Long  (refAttach.ReferralNum)+","
				+    POut.Long  (refAttach.PatNum)+","
				+    POut.Int   (refAttach.ItemOrder)+","
				+    POut.Date  (refAttach.RefDate)+","
				+    POut.Int   ((int)refAttach.RefType)+","
				+    POut.Int   ((int)refAttach.RefToStatus)+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Bool  (refAttach.IsTransitionOfCare)+","
				+    POut.Long  (refAttach.ProcNum)+","
				+    POut.Date  (refAttach.DateProcComplete)+","
				+    POut.Long  (refAttach.ProvNum)+")";
				//DateTStamp can only be set by MySQL
			if(refAttach.Note==null) {
				refAttach.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(refAttach.Note));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				refAttach.RefAttachNum=Db.NonQ(command,true,"RefAttachNum","refAttach",paramNote);
			}
			return refAttach.RefAttachNum;
		}

		///<summary>Inserts one RefAttach into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RefAttach refAttach) {
			return InsertNoCache(refAttach,false);
		}

		///<summary>Inserts one RefAttach into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RefAttach refAttach,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO refattach (";
			if(!useExistingPK && isRandomKeys) {
				refAttach.RefAttachNum=ReplicationServers.GetKeyNoCache("refattach","RefAttachNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RefAttachNum,";
			}
			command+="ReferralNum,PatNum,ItemOrder,RefDate,RefType,RefToStatus,Note,IsTransitionOfCare,ProcNum,DateProcComplete,ProvNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(refAttach.RefAttachNum)+",";
			}
			command+=
				     POut.Long  (refAttach.ReferralNum)+","
				+    POut.Long  (refAttach.PatNum)+","
				+    POut.Int   (refAttach.ItemOrder)+","
				+    POut.Date  (refAttach.RefDate)+","
				+    POut.Int   ((int)refAttach.RefType)+","
				+    POut.Int   ((int)refAttach.RefToStatus)+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Bool  (refAttach.IsTransitionOfCare)+","
				+    POut.Long  (refAttach.ProcNum)+","
				+    POut.Date  (refAttach.DateProcComplete)+","
				+    POut.Long  (refAttach.ProvNum)+")";
				//DateTStamp can only be set by MySQL
			if(refAttach.Note==null) {
				refAttach.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(refAttach.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				refAttach.RefAttachNum=Db.NonQ(command,true,"RefAttachNum","refAttach",paramNote);
			}
			return refAttach.RefAttachNum;
		}

		///<summary>Updates one RefAttach in the database.</summary>
		public static void Update(RefAttach refAttach) {
			string command="UPDATE refattach SET "
				+"ReferralNum       =  "+POut.Long  (refAttach.ReferralNum)+", "
				+"PatNum            =  "+POut.Long  (refAttach.PatNum)+", "
				+"ItemOrder         =  "+POut.Int   (refAttach.ItemOrder)+", "
				+"RefDate           =  "+POut.Date  (refAttach.RefDate)+", "
				+"RefType           =  "+POut.Int   ((int)refAttach.RefType)+", "
				+"RefToStatus       =  "+POut.Int   ((int)refAttach.RefToStatus)+", "
				+"Note              =  "+DbHelper.ParamChar+"paramNote, "
				+"IsTransitionOfCare=  "+POut.Bool  (refAttach.IsTransitionOfCare)+", "
				+"ProcNum           =  "+POut.Long  (refAttach.ProcNum)+", "
				+"DateProcComplete  =  "+POut.Date  (refAttach.DateProcComplete)+", "
				+"ProvNum           =  "+POut.Long  (refAttach.ProvNum)+" "
				//DateTStamp can only be set by MySQL
				+"WHERE RefAttachNum = "+POut.Long(refAttach.RefAttachNum);
			if(refAttach.Note==null) {
				refAttach.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(refAttach.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one RefAttach in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RefAttach refAttach,RefAttach oldRefAttach) {
			string command="";
			if(refAttach.ReferralNum != oldRefAttach.ReferralNum) {
				if(command!="") { command+=",";}
				command+="ReferralNum = "+POut.Long(refAttach.ReferralNum)+"";
			}
			if(refAttach.PatNum != oldRefAttach.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(refAttach.PatNum)+"";
			}
			if(refAttach.ItemOrder != oldRefAttach.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(refAttach.ItemOrder)+"";
			}
			if(refAttach.RefDate.Date != oldRefAttach.RefDate.Date) {
				if(command!="") { command+=",";}
				command+="RefDate = "+POut.Date(refAttach.RefDate)+"";
			}
			if(refAttach.RefType != oldRefAttach.RefType) {
				if(command!="") { command+=",";}
				command+="RefType = "+POut.Int   ((int)refAttach.RefType)+"";
			}
			if(refAttach.RefToStatus != oldRefAttach.RefToStatus) {
				if(command!="") { command+=",";}
				command+="RefToStatus = "+POut.Int   ((int)refAttach.RefToStatus)+"";
			}
			if(refAttach.Note != oldRefAttach.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(refAttach.IsTransitionOfCare != oldRefAttach.IsTransitionOfCare) {
				if(command!="") { command+=",";}
				command+="IsTransitionOfCare = "+POut.Bool(refAttach.IsTransitionOfCare)+"";
			}
			if(refAttach.ProcNum != oldRefAttach.ProcNum) {
				if(command!="") { command+=",";}
				command+="ProcNum = "+POut.Long(refAttach.ProcNum)+"";
			}
			if(refAttach.DateProcComplete.Date != oldRefAttach.DateProcComplete.Date) {
				if(command!="") { command+=",";}
				command+="DateProcComplete = "+POut.Date(refAttach.DateProcComplete)+"";
			}
			if(refAttach.ProvNum != oldRefAttach.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(refAttach.ProvNum)+"";
			}
			//DateTStamp can only be set by MySQL
			if(command=="") {
				return false;
			}
			if(refAttach.Note==null) {
				refAttach.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(refAttach.Note));
			command="UPDATE refattach SET "+command
				+" WHERE RefAttachNum = "+POut.Long(refAttach.RefAttachNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(RefAttach,RefAttach) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(RefAttach refAttach,RefAttach oldRefAttach) {
			if(refAttach.ReferralNum != oldRefAttach.ReferralNum) {
				return true;
			}
			if(refAttach.PatNum != oldRefAttach.PatNum) {
				return true;
			}
			if(refAttach.ItemOrder != oldRefAttach.ItemOrder) {
				return true;
			}
			if(refAttach.RefDate.Date != oldRefAttach.RefDate.Date) {
				return true;
			}
			if(refAttach.RefType != oldRefAttach.RefType) {
				return true;
			}
			if(refAttach.RefToStatus != oldRefAttach.RefToStatus) {
				return true;
			}
			if(refAttach.Note != oldRefAttach.Note) {
				return true;
			}
			if(refAttach.IsTransitionOfCare != oldRefAttach.IsTransitionOfCare) {
				return true;
			}
			if(refAttach.ProcNum != oldRefAttach.ProcNum) {
				return true;
			}
			if(refAttach.DateProcComplete.Date != oldRefAttach.DateProcComplete.Date) {
				return true;
			}
			if(refAttach.ProvNum != oldRefAttach.ProvNum) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			return false;
		}

		///<summary>Deletes one RefAttach from the database.</summary>
		public static void Delete(long refAttachNum) {
			string command="DELETE FROM refattach "
				+"WHERE RefAttachNum = "+POut.Long(refAttachNum);
			Db.NonQ(command);
		}

	}
}