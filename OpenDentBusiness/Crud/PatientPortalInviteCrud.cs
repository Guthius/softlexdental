//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness.Crud{
	public class PatientPortalInviteCrud {
		///<summary>Gets one PatientPortalInvite object from the database using the primary key.  Returns null if not found.</summary>
		public static PatientPortalInvite SelectOne(long patientPortalInviteNum) {
			string command="SELECT * FROM patientportalinvite "
				+"WHERE PatientPortalInviteNum = "+POut.Long(patientPortalInviteNum);
			List<PatientPortalInvite> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatientPortalInvite object from the database using a query.</summary>
		public static PatientPortalInvite SelectOne(string command) {
			
			List<PatientPortalInvite> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatientPortalInvite objects from the database using a query.</summary>
		public static List<PatientPortalInvite> SelectMany(string command) {
			
			List<PatientPortalInvite> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatientPortalInvite> TableToList(DataTable table) {
			List<PatientPortalInvite> retVal=new List<PatientPortalInvite>();
			PatientPortalInvite patientPortalInvite;
			foreach(DataRow row in table.Rows) {
				patientPortalInvite=new PatientPortalInvite();
				patientPortalInvite.PatientPortalInviteNum= PIn.Long  (row["PatientPortalInviteNum"].ToString());
				patientPortalInvite.PatNum                = PIn.Long  (row["PatNum"].ToString());
				patientPortalInvite.AptNum                = PIn.Long  (row["AptNum"].ToString());
				patientPortalInvite.ClinicNum             = PIn.Long  (row["ClinicNum"].ToString());
				patientPortalInvite.DateTimeEntry         = PIn.DateT (row["DateTimeEntry"].ToString());
				patientPortalInvite.TSPrior               = TimeSpan.FromTicks(PIn.Long(row["TSPrior"].ToString()));
				patientPortalInvite.EmailSendStatus       = (OpenDentBusiness.AutoCommStatus)PIn.Int(row["EmailSendStatus"].ToString());
				patientPortalInvite.EmailMessageNum       = PIn.Long  (row["EmailMessageNum"].ToString());
				patientPortalInvite.TemplateEmail         = PIn.String(row["TemplateEmail"].ToString());
				patientPortalInvite.TemplateEmailSubj     = PIn.String(row["TemplateEmailSubj"].ToString());
				patientPortalInvite.Note                  = PIn.String(row["Note"].ToString());
				retVal.Add(patientPortalInvite);
			}
			return retVal;
		}

		///<summary>Converts a list of PatientPortalInvite into a DataTable.</summary>
		public static DataTable ListToTable(List<PatientPortalInvite> listPatientPortalInvites,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PatientPortalInvite";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PatientPortalInviteNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("AptNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("DateTimeEntry");
			table.Columns.Add("TSPrior");
			table.Columns.Add("EmailSendStatus");
			table.Columns.Add("EmailMessageNum");
			table.Columns.Add("TemplateEmail");
			table.Columns.Add("TemplateEmailSubj");
			table.Columns.Add("Note");
			foreach(PatientPortalInvite patientPortalInvite in listPatientPortalInvites) {
				table.Rows.Add(new object[] {
					POut.Long  (patientPortalInvite.PatientPortalInviteNum),
					POut.Long  (patientPortalInvite.PatNum),
					POut.Long  (patientPortalInvite.AptNum),
					POut.Long  (patientPortalInvite.ClinicNum),
					POut.DateT (patientPortalInvite.DateTimeEntry,false),
					POut.Long (patientPortalInvite.TSPrior.Ticks),
					POut.Int   ((int)patientPortalInvite.EmailSendStatus),
					POut.Long  (patientPortalInvite.EmailMessageNum),
					            patientPortalInvite.TemplateEmail,
					            patientPortalInvite.TemplateEmailSubj,
					            patientPortalInvite.Note,
				});
			}
			return table;
		}

		///<summary>Inserts one PatientPortalInvite into the database.  Returns the new priKey.</summary>
		public static long Insert(PatientPortalInvite patientPortalInvite) {
			return Insert(patientPortalInvite,false);
		}

		///<summary>Inserts one PatientPortalInvite into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatientPortalInvite patientPortalInvite,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				patientPortalInvite.PatientPortalInviteNum=ReplicationServers.GetKey("patientportalinvite","PatientPortalInviteNum");
			}
			string command="INSERT INTO patientportalinvite (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatientPortalInviteNum,";
			}
			command+="PatNum,AptNum,ClinicNum,DateTimeEntry,TSPrior,EmailSendStatus,EmailMessageNum,TemplateEmail,TemplateEmailSubj,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patientPortalInvite.PatientPortalInviteNum)+",";
			}
			command+=
				     POut.Long  (patientPortalInvite.PatNum)+","
				+    POut.Long  (patientPortalInvite.AptNum)+","
				+    POut.Long  (patientPortalInvite.ClinicNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.Long  (patientPortalInvite.TSPrior.Ticks)+"',"
				+    POut.Int   ((int)patientPortalInvite.EmailSendStatus)+","
				+    POut.Long  (patientPortalInvite.EmailMessageNum)+","
				+    DbHelper.ParamChar+"paramTemplateEmail,"
				+"'"+POut.String(patientPortalInvite.TemplateEmailSubj)+"',"
				+    DbHelper.ParamChar+"paramNote)";
			if(patientPortalInvite.TemplateEmail==null) {
				patientPortalInvite.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(patientPortalInvite.TemplateEmail));
			if(patientPortalInvite.Note==null) {
				patientPortalInvite.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(patientPortalInvite.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramTemplateEmail,paramNote);
			}
			else {
				patientPortalInvite.PatientPortalInviteNum=Db.NonQ(command,true,"PatientPortalInviteNum","patientPortalInvite",paramTemplateEmail,paramNote);
			}
			return patientPortalInvite.PatientPortalInviteNum;
		}

		///<summary>Inserts many PatientPortalInvites into the database.</summary>
		public static void InsertMany(List<PatientPortalInvite> listPatientPortalInvites) {
			InsertMany(listPatientPortalInvites,false);
		}

		///<summary>Inserts many PatientPortalInvites into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<PatientPortalInvite> listPatientPortalInvites,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				foreach(PatientPortalInvite patientPortalInvite in listPatientPortalInvites) {
					Insert(patientPortalInvite);
				}
			}
			else {
				StringBuilder sbCommands=null;
				int index=0;
				while(index < listPatientPortalInvites.Count) {
					PatientPortalInvite patientPortalInvite=listPatientPortalInvites[index];
					StringBuilder sbRow=new StringBuilder("(");
					bool hasComma=false;
					if(sbCommands==null) {
						sbCommands=new StringBuilder();
						sbCommands.Append("INSERT INTO patientportalinvite (");
						if(useExistingPK) {
							sbCommands.Append("PatientPortalInviteNum,");
						}
						sbCommands.Append("PatNum,AptNum,ClinicNum,DateTimeEntry,TSPrior,EmailSendStatus,EmailMessageNum,TemplateEmail,TemplateEmailSubj,Note) VALUES ");
					}
					else {
						hasComma=true;
					}
					if(useExistingPK) {
						sbRow.Append(POut.Long(patientPortalInvite.PatientPortalInviteNum)); sbRow.Append(",");
					}
					sbRow.Append(POut.Long(patientPortalInvite.PatNum)); sbRow.Append(",");
					sbRow.Append(POut.Long(patientPortalInvite.AptNum)); sbRow.Append(",");
					sbRow.Append(POut.Long(patientPortalInvite.ClinicNum)); sbRow.Append(",");
					sbRow.Append(DbHelper.Now()); sbRow.Append(",");
					sbRow.Append("'"+POut.Long  (patientPortalInvite.TSPrior.Ticks)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Int((int)patientPortalInvite.EmailSendStatus)); sbRow.Append(",");
					sbRow.Append(POut.Long(patientPortalInvite.EmailMessageNum)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(patientPortalInvite.TemplateEmail)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(patientPortalInvite.TemplateEmailSubj)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(patientPortalInvite.Note)+"'"); sbRow.Append(")");
					if(sbCommands.Length+sbRow.Length+1 > TableBase.MaxAllowedPacketCount) {
						Db.NonQ(sbCommands.ToString());
						sbCommands=null;
					}
					else {
						if(hasComma) {
							sbCommands.Append(",");
						}
						sbCommands.Append(sbRow.ToString());
						if(index==listPatientPortalInvites.Count-1) {
							Db.NonQ(sbCommands.ToString());
						}
						index++;
					}
				}
			}
		}

		///<summary>Inserts one PatientPortalInvite into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientPortalInvite patientPortalInvite) {
			return InsertNoCache(patientPortalInvite,false);
		}

		///<summary>Inserts one PatientPortalInvite into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientPortalInvite patientPortalInvite,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO patientportalinvite (";
			if(!useExistingPK && isRandomKeys) {
				patientPortalInvite.PatientPortalInviteNum=ReplicationServers.GetKeyNoCache("patientportalinvite","PatientPortalInviteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatientPortalInviteNum,";
			}
			command+="PatNum,AptNum,ClinicNum,DateTimeEntry,TSPrior,EmailSendStatus,EmailMessageNum,TemplateEmail,TemplateEmailSubj,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patientPortalInvite.PatientPortalInviteNum)+",";
			}
			command+=
				     POut.Long  (patientPortalInvite.PatNum)+","
				+    POut.Long  (patientPortalInvite.AptNum)+","
				+    POut.Long  (patientPortalInvite.ClinicNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.Long(patientPortalInvite.TSPrior.Ticks)+"',"
				+    POut.Int   ((int)patientPortalInvite.EmailSendStatus)+","
				+    POut.Long  (patientPortalInvite.EmailMessageNum)+","
				+    DbHelper.ParamChar+"paramTemplateEmail,"
				+"'"+POut.String(patientPortalInvite.TemplateEmailSubj)+"',"
				+    DbHelper.ParamChar+"paramNote)";
			if(patientPortalInvite.TemplateEmail==null) {
				patientPortalInvite.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(patientPortalInvite.TemplateEmail));
			if(patientPortalInvite.Note==null) {
				patientPortalInvite.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(patientPortalInvite.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramTemplateEmail,paramNote);
			}
			else {
				patientPortalInvite.PatientPortalInviteNum=Db.NonQ(command,true,"PatientPortalInviteNum","patientPortalInvite",paramTemplateEmail,paramNote);
			}
			return patientPortalInvite.PatientPortalInviteNum;
		}

		///<summary>Updates one PatientPortalInvite in the database.</summary>
		public static void Update(PatientPortalInvite patientPortalInvite) {
			string command="UPDATE patientportalinvite SET "
				+"PatNum                =  "+POut.Long  (patientPortalInvite.PatNum)+", "
				+"AptNum                =  "+POut.Long  (patientPortalInvite.AptNum)+", "
				+"ClinicNum             =  "+POut.Long  (patientPortalInvite.ClinicNum)+", "
				//DateTimeEntry not allowed to change
				+"TSPrior               =  "+POut.Long  (patientPortalInvite.TSPrior.Ticks)+", "
				+"EmailSendStatus       =  "+POut.Int   ((int)patientPortalInvite.EmailSendStatus)+", "
				+"EmailMessageNum       =  "+POut.Long  (patientPortalInvite.EmailMessageNum)+", "
				+"TemplateEmail         =  "+DbHelper.ParamChar+"paramTemplateEmail, "
				+"TemplateEmailSubj     = '"+POut.String(patientPortalInvite.TemplateEmailSubj)+"', "
				+"Note                  =  "+DbHelper.ParamChar+"paramNote "
				+"WHERE PatientPortalInviteNum = "+POut.Long(patientPortalInvite.PatientPortalInviteNum);
			if(patientPortalInvite.TemplateEmail==null) {
				patientPortalInvite.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(patientPortalInvite.TemplateEmail));
			if(patientPortalInvite.Note==null) {
				patientPortalInvite.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(patientPortalInvite.Note));
			Db.NonQ(command,paramTemplateEmail,paramNote);
		}

		///<summary>Updates one PatientPortalInvite in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatientPortalInvite patientPortalInvite,PatientPortalInvite oldPatientPortalInvite) {
			string command="";
			if(patientPortalInvite.PatNum != oldPatientPortalInvite.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(patientPortalInvite.PatNum)+"";
			}
			if(patientPortalInvite.AptNum != oldPatientPortalInvite.AptNum) {
				if(command!="") { command+=",";}
				command+="AptNum = "+POut.Long(patientPortalInvite.AptNum)+"";
			}
			if(patientPortalInvite.ClinicNum != oldPatientPortalInvite.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(patientPortalInvite.ClinicNum)+"";
			}
			//DateTimeEntry not allowed to change
			if(patientPortalInvite.TSPrior != oldPatientPortalInvite.TSPrior) {
				if(command!="") { command+=",";}
				command+="TSPrior = '"+POut.Long  (patientPortalInvite.TSPrior.Ticks)+"'";
			}
			if(patientPortalInvite.EmailSendStatus != oldPatientPortalInvite.EmailSendStatus) {
				if(command!="") { command+=",";}
				command+="EmailSendStatus = "+POut.Int   ((int)patientPortalInvite.EmailSendStatus)+"";
			}
			if(patientPortalInvite.EmailMessageNum != oldPatientPortalInvite.EmailMessageNum) {
				if(command!="") { command+=",";}
				command+="EmailMessageNum = "+POut.Long(patientPortalInvite.EmailMessageNum)+"";
			}
			if(patientPortalInvite.TemplateEmail != oldPatientPortalInvite.TemplateEmail) {
				if(command!="") { command+=",";}
				command+="TemplateEmail = "+DbHelper.ParamChar+"paramTemplateEmail";
			}
			if(patientPortalInvite.TemplateEmailSubj != oldPatientPortalInvite.TemplateEmailSubj) {
				if(command!="") { command+=",";}
				command+="TemplateEmailSubj = '"+POut.String(patientPortalInvite.TemplateEmailSubj)+"'";
			}
			if(patientPortalInvite.Note != oldPatientPortalInvite.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(command=="") {
				return false;
			}
			if(patientPortalInvite.TemplateEmail==null) {
				patientPortalInvite.TemplateEmail="";
			}
			OdSqlParameter paramTemplateEmail=new OdSqlParameter("paramTemplateEmail",OdDbType.Text,POut.StringParam(patientPortalInvite.TemplateEmail));
			if(patientPortalInvite.Note==null) {
				patientPortalInvite.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(patientPortalInvite.Note));
			command="UPDATE patientportalinvite SET "+command
				+" WHERE PatientPortalInviteNum = "+POut.Long(patientPortalInvite.PatientPortalInviteNum);
			Db.NonQ(command,paramTemplateEmail,paramNote);
			return true;
		}

		///<summary>Returns true if Update(PatientPortalInvite,PatientPortalInvite) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PatientPortalInvite patientPortalInvite,PatientPortalInvite oldPatientPortalInvite) {
			if(patientPortalInvite.PatNum != oldPatientPortalInvite.PatNum) {
				return true;
			}
			if(patientPortalInvite.AptNum != oldPatientPortalInvite.AptNum) {
				return true;
			}
			if(patientPortalInvite.ClinicNum != oldPatientPortalInvite.ClinicNum) {
				return true;
			}
			//DateTimeEntry not allowed to change
			if(patientPortalInvite.TSPrior != oldPatientPortalInvite.TSPrior) {
				return true;
			}
			if(patientPortalInvite.EmailSendStatus != oldPatientPortalInvite.EmailSendStatus) {
				return true;
			}
			if(patientPortalInvite.EmailMessageNum != oldPatientPortalInvite.EmailMessageNum) {
				return true;
			}
			if(patientPortalInvite.TemplateEmail != oldPatientPortalInvite.TemplateEmail) {
				return true;
			}
			if(patientPortalInvite.TemplateEmailSubj != oldPatientPortalInvite.TemplateEmailSubj) {
				return true;
			}
			if(patientPortalInvite.Note != oldPatientPortalInvite.Note) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PatientPortalInvite from the database.</summary>
		public static void Delete(long patientPortalInviteNum) {
			string command="DELETE FROM patientportalinvite "
				+"WHERE PatientPortalInviteNum = "+POut.Long(patientPortalInviteNum);
			Db.NonQ(command);
		}

	}
}