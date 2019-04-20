//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobNoteCrud {
		///<summary>Gets one JobNote object from the database using the primary key.  Returns null if not found.</summary>
		public static JobNote SelectOne(long jobNoteNum) {
			string command="SELECT * FROM jobnote "
				+"WHERE JobNoteNum = "+POut.Long(jobNoteNum);
			List<JobNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobNote object from the database using a query.</summary>
		public static JobNote SelectOne(string command) {
			
			List<JobNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobNote objects from the database using a query.</summary>
		public static List<JobNote> SelectMany(string command) {
			
			List<JobNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobNote> TableToList(DataTable table) {
			List<JobNote> retVal=new List<JobNote>();
			JobNote jobNote;
			foreach(DataRow row in table.Rows) {
				jobNote=new JobNote();
				jobNote.JobNoteNum  = PIn.Long  (row["JobNoteNum"].ToString());
				jobNote.JobNum      = PIn.Long  (row["JobNum"].ToString());
				jobNote.UserNum     = PIn.Long  (row["UserNum"].ToString());
				jobNote.DateTimeNote= PIn.DateT (row["DateTimeNote"].ToString());
				jobNote.Note        = PIn.String(row["Note"].ToString());
				jobNote.NoteType    = (OpenDentBusiness.JobNoteTypes)PIn.Int(row["NoteType"].ToString());
				retVal.Add(jobNote);
			}
			return retVal;
		}

		///<summary>Converts a list of JobNote into a DataTable.</summary>
		public static DataTable ListToTable(List<JobNote> listJobNotes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="JobNote";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("JobNoteNum");
			table.Columns.Add("JobNum");
			table.Columns.Add("UserNum");
			table.Columns.Add("DateTimeNote");
			table.Columns.Add("Note");
			table.Columns.Add("NoteType");
			foreach(JobNote jobNote in listJobNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (jobNote.JobNoteNum),
					POut.Long  (jobNote.JobNum),
					POut.Long  (jobNote.UserNum),
					POut.DateT (jobNote.DateTimeNote,false),
					            jobNote.Note,
					POut.Int   ((int)jobNote.NoteType),
				});
			}
			return table;
		}

		///<summary>Inserts one JobNote into the database.  Returns the new priKey.</summary>
		public static long Insert(JobNote jobNote) {
			return Insert(jobNote,false);
		}

		///<summary>Inserts one JobNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobNote jobNote,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				jobNote.JobNoteNum=ReplicationServers.GetKey("jobnote","JobNoteNum");
			}
			string command="INSERT INTO jobnote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobNoteNum,";
			}
			command+="JobNum,UserNum,DateTimeNote,Note,NoteType) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobNote.JobNoteNum)+",";
			}
			command+=
				     POut.Long  (jobNote.JobNum)+","
				+    POut.Long  (jobNote.UserNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Int   ((int)jobNote.NoteType)+")";
			if(jobNote.Note==null) {
				jobNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(jobNote.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				jobNote.JobNoteNum=Db.NonQ(command,true,"JobNoteNum","jobNote",paramNote);
			}
			return jobNote.JobNoteNum;
		}

		///<summary>Inserts one JobNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobNote jobNote) {
			return InsertNoCache(jobNote,false);
		}

		///<summary>Inserts one JobNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobNote jobNote,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO jobnote (";
			if(!useExistingPK && isRandomKeys) {
				jobNote.JobNoteNum=ReplicationServers.GetKeyNoCache("jobnote","JobNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobNoteNum,";
			}
			command+="JobNum,UserNum,DateTimeNote,Note,NoteType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobNote.JobNoteNum)+",";
			}
			command+=
				     POut.Long  (jobNote.JobNum)+","
				+    POut.Long  (jobNote.UserNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Int   ((int)jobNote.NoteType)+")";
			if(jobNote.Note==null) {
				jobNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(jobNote.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				jobNote.JobNoteNum=Db.NonQ(command,true,"JobNoteNum","jobNote",paramNote);
			}
			return jobNote.JobNoteNum;
		}

		///<summary>Updates one JobNote in the database.</summary>
		public static void Update(JobNote jobNote) {
			string command="UPDATE jobnote SET "
				+"JobNum      =  "+POut.Long  (jobNote.JobNum)+", "
				+"UserNum     =  "+POut.Long  (jobNote.UserNum)+", "
				+"DateTimeNote=  "+POut.DateT (jobNote.DateTimeNote)+", "
				+"Note        =  "+DbHelper.ParamChar+"paramNote, "
				+"NoteType    =  "+POut.Int   ((int)jobNote.NoteType)+" "
				+"WHERE JobNoteNum = "+POut.Long(jobNote.JobNoteNum);
			if(jobNote.Note==null) {
				jobNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(jobNote.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one JobNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobNote jobNote,JobNote oldJobNote) {
			string command="";
			if(jobNote.JobNum != oldJobNote.JobNum) {
				if(command!="") { command+=",";}
				command+="JobNum = "+POut.Long(jobNote.JobNum)+"";
			}
			if(jobNote.UserNum != oldJobNote.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(jobNote.UserNum)+"";
			}
			if(jobNote.DateTimeNote != oldJobNote.DateTimeNote) {
				if(command!="") { command+=",";}
				command+="DateTimeNote = "+POut.DateT(jobNote.DateTimeNote)+"";
			}
			if(jobNote.Note != oldJobNote.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(jobNote.NoteType != oldJobNote.NoteType) {
				if(command!="") { command+=",";}
				command+="NoteType = "+POut.Int   ((int)jobNote.NoteType)+"";
			}
			if(command=="") {
				return false;
			}
			if(jobNote.Note==null) {
				jobNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(jobNote.Note));
			command="UPDATE jobnote SET "+command
				+" WHERE JobNoteNum = "+POut.Long(jobNote.JobNoteNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(JobNote,JobNote) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(JobNote jobNote,JobNote oldJobNote) {
			if(jobNote.JobNum != oldJobNote.JobNum) {
				return true;
			}
			if(jobNote.UserNum != oldJobNote.UserNum) {
				return true;
			}
			if(jobNote.DateTimeNote != oldJobNote.DateTimeNote) {
				return true;
			}
			if(jobNote.Note != oldJobNote.Note) {
				return true;
			}
			if(jobNote.NoteType != oldJobNote.NoteType) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one JobNote from the database.</summary>
		public static void Delete(long jobNoteNum) {
			string command="DELETE FROM jobnote "
				+"WHERE JobNoteNum = "+POut.Long(jobNoteNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobNote> listNew,List<JobNote> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<JobNote> listIns    =new List<JobNote>();
			List<JobNote> listUpdNew =new List<JobNote>();
			List<JobNote> listUpdDB  =new List<JobNote>();
			List<JobNote> listDel    =new List<JobNote>();
			listNew.Sort((JobNote x,JobNote y) => { return x.JobNoteNum.CompareTo(y.JobNoteNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((JobNote x,JobNote y) => { return x.JobNoteNum.CompareTo(y.JobNoteNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			JobNote fieldNew;
			JobNote fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.JobNoteNum<fieldDB.JobNoteNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.JobNoteNum>fieldDB.JobNoteNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].JobNoteNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}