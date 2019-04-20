//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatientNoteCrud {
		///<summary>Gets one PatientNote object from the database using the primary key.  Returns null if not found.</summary>
		public static PatientNote SelectOne(long patNum) {
			string command="SELECT * FROM patientnote "
				+"WHERE PatNum = "+POut.Long(patNum);
			List<PatientNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatientNote object from the database using a query.</summary>
		public static PatientNote SelectOne(string command) {
			
			List<PatientNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatientNote objects from the database using a query.</summary>
		public static List<PatientNote> SelectMany(string command) {
			
			List<PatientNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatientNote> TableToList(DataTable table) {
			List<PatientNote> retVal=new List<PatientNote>();
			PatientNote patientNote;
			foreach(DataRow row in table.Rows) {
				patientNote=new PatientNote();
				patientNote.PatNum                    = PIn.Long  (row["PatNum"].ToString());
				patientNote.FamFinancial              = PIn.String(row["FamFinancial"].ToString());
				patientNote.ApptPhone                 = PIn.String(row["ApptPhone"].ToString());
				patientNote.Medical                   = PIn.String(row["Medical"].ToString());
				patientNote.Service                   = PIn.String(row["Service"].ToString());
				patientNote.MedicalComp               = PIn.String(row["MedicalComp"].ToString());
				patientNote.Treatment                 = PIn.String(row["Treatment"].ToString());
				patientNote.ICEName                   = PIn.String(row["ICEName"].ToString());
				patientNote.ICEPhone                  = PIn.String(row["ICEPhone"].ToString());
				patientNote.OrthoMonthsTreatOverride  = PIn.Int   (row["OrthoMonthsTreatOverride"].ToString());
				patientNote.DateOrthoPlacementOverride= PIn.Date  (row["DateOrthoPlacementOverride"].ToString());
				retVal.Add(patientNote);
			}
			return retVal;
		}

		///<summary>Converts a list of PatientNote into a DataTable.</summary>
		public static DataTable ListToTable(List<PatientNote> listPatientNotes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PatientNote";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PatNum");
			table.Columns.Add("FamFinancial");
			table.Columns.Add("ApptPhone");
			table.Columns.Add("Medical");
			table.Columns.Add("Service");
			table.Columns.Add("MedicalComp");
			table.Columns.Add("Treatment");
			table.Columns.Add("ICEName");
			table.Columns.Add("ICEPhone");
			table.Columns.Add("OrthoMonthsTreatOverride");
			table.Columns.Add("DateOrthoPlacementOverride");
			foreach(PatientNote patientNote in listPatientNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (patientNote.PatNum),
					            patientNote.FamFinancial,
					            patientNote.ApptPhone,
					            patientNote.Medical,
					            patientNote.Service,
					            patientNote.MedicalComp,
					            patientNote.Treatment,
					            patientNote.ICEName,
					            patientNote.ICEPhone,
					POut.Int   (patientNote.OrthoMonthsTreatOverride),
					POut.DateT (patientNote.DateOrthoPlacementOverride,false),
				});
			}
			return table;
		}

		///<summary>Inserts one PatientNote into the database.  Returns the new priKey.</summary>
		public static long Insert(PatientNote patientNote) {
			return Insert(patientNote,false);
		}

		///<summary>Inserts one PatientNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatientNote patientNote,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				patientNote.PatNum=ReplicationServers.GetKey("patientnote","PatNum");
			}
			string command="INSERT INTO patientnote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatNum,";
			}
			command+="FamFinancial,ApptPhone,Medical,Service,MedicalComp,Treatment,ICEName,ICEPhone,OrthoMonthsTreatOverride,DateOrthoPlacementOverride) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patientNote.PatNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramFamFinancial,"
				+    DbHelper.ParamChar+"paramApptPhone,"
				+    DbHelper.ParamChar+"paramMedical,"
				+    DbHelper.ParamChar+"paramService,"
				+    DbHelper.ParamChar+"paramMedicalComp,"
				+    DbHelper.ParamChar+"paramTreatment,"
				+"'"+POut.String(patientNote.ICEName)+"',"
				+"'"+POut.String(patientNote.ICEPhone)+"',"
				+    POut.Int   (patientNote.OrthoMonthsTreatOverride)+","
				+    POut.Date  (patientNote.DateOrthoPlacementOverride)+")";
			if(patientNote.FamFinancial==null) {
				patientNote.FamFinancial="";
			}
			OdSqlParameter paramFamFinancial=new OdSqlParameter("paramFamFinancial",OdDbType.Text,POut.StringNote(patientNote.FamFinancial));
			if(patientNote.ApptPhone==null) {
				patientNote.ApptPhone="";
			}
			OdSqlParameter paramApptPhone=new OdSqlParameter("paramApptPhone",OdDbType.Text,POut.StringParam(patientNote.ApptPhone));
			if(patientNote.Medical==null) {
				patientNote.Medical="";
			}
			OdSqlParameter paramMedical=new OdSqlParameter("paramMedical",OdDbType.Text,POut.StringNote(patientNote.Medical));
			if(patientNote.Service==null) {
				patientNote.Service="";
			}
			OdSqlParameter paramService=new OdSqlParameter("paramService",OdDbType.Text,POut.StringNote(patientNote.Service));
			if(patientNote.MedicalComp==null) {
				patientNote.MedicalComp="";
			}
			OdSqlParameter paramMedicalComp=new OdSqlParameter("paramMedicalComp",OdDbType.Text,POut.StringNote(patientNote.MedicalComp));
			if(patientNote.Treatment==null) {
				patientNote.Treatment="";
			}
			OdSqlParameter paramTreatment=new OdSqlParameter("paramTreatment",OdDbType.Text,POut.StringNote(patientNote.Treatment));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
			}
			else {
				patientNote.PatNum=Db.NonQ(command,true,"PatNum","patientNote",paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
			}
			return patientNote.PatNum;
		}

		///<summary>Inserts one PatientNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientNote patientNote) {
			return InsertNoCache(patientNote,false);
		}

		///<summary>Inserts one PatientNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatientNote patientNote,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO patientnote (";
			if(!useExistingPK && isRandomKeys) {
				patientNote.PatNum=ReplicationServers.GetKeyNoCache("patientnote","PatNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatNum,";
			}
			command+="FamFinancial,ApptPhone,Medical,Service,MedicalComp,Treatment,ICEName,ICEPhone,OrthoMonthsTreatOverride,DateOrthoPlacementOverride) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patientNote.PatNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramFamFinancial,"
				+    DbHelper.ParamChar+"paramApptPhone,"
				+    DbHelper.ParamChar+"paramMedical,"
				+    DbHelper.ParamChar+"paramService,"
				+    DbHelper.ParamChar+"paramMedicalComp,"
				+    DbHelper.ParamChar+"paramTreatment,"
				+"'"+POut.String(patientNote.ICEName)+"',"
				+"'"+POut.String(patientNote.ICEPhone)+"',"
				+    POut.Int   (patientNote.OrthoMonthsTreatOverride)+","
				+    POut.Date  (patientNote.DateOrthoPlacementOverride)+")";
			if(patientNote.FamFinancial==null) {
				patientNote.FamFinancial="";
			}
			OdSqlParameter paramFamFinancial=new OdSqlParameter("paramFamFinancial",OdDbType.Text,POut.StringNote(patientNote.FamFinancial));
			if(patientNote.ApptPhone==null) {
				patientNote.ApptPhone="";
			}
			OdSqlParameter paramApptPhone=new OdSqlParameter("paramApptPhone",OdDbType.Text,POut.StringParam(patientNote.ApptPhone));
			if(patientNote.Medical==null) {
				patientNote.Medical="";
			}
			OdSqlParameter paramMedical=new OdSqlParameter("paramMedical",OdDbType.Text,POut.StringNote(patientNote.Medical));
			if(patientNote.Service==null) {
				patientNote.Service="";
			}
			OdSqlParameter paramService=new OdSqlParameter("paramService",OdDbType.Text,POut.StringNote(patientNote.Service));
			if(patientNote.MedicalComp==null) {
				patientNote.MedicalComp="";
			}
			OdSqlParameter paramMedicalComp=new OdSqlParameter("paramMedicalComp",OdDbType.Text,POut.StringNote(patientNote.MedicalComp));
			if(patientNote.Treatment==null) {
				patientNote.Treatment="";
			}
			OdSqlParameter paramTreatment=new OdSqlParameter("paramTreatment",OdDbType.Text,POut.StringNote(patientNote.Treatment));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
			}
			else {
				patientNote.PatNum=Db.NonQ(command,true,"PatNum","patientNote",paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
			}
			return patientNote.PatNum;
		}

		///<summary>Updates one PatientNote in the database.</summary>
		public static void Update(PatientNote patientNote) {
			string command="UPDATE patientnote SET "
				//FamFinancial excluded from update
				+"ApptPhone                 =  "+DbHelper.ParamChar+"paramApptPhone, "
				+"Medical                   =  "+DbHelper.ParamChar+"paramMedical, "
				+"Service                   =  "+DbHelper.ParamChar+"paramService, "
				+"MedicalComp               =  "+DbHelper.ParamChar+"paramMedicalComp, "
				+"Treatment                 =  "+DbHelper.ParamChar+"paramTreatment, "
				+"ICEName                   = '"+POut.String(patientNote.ICEName)+"', "
				+"ICEPhone                  = '"+POut.String(patientNote.ICEPhone)+"', "
				+"OrthoMonthsTreatOverride  =  "+POut.Int   (patientNote.OrthoMonthsTreatOverride)+", "
				+"DateOrthoPlacementOverride=  "+POut.Date  (patientNote.DateOrthoPlacementOverride)+" "
				+"WHERE PatNum = "+POut.Long(patientNote.PatNum);
			if(patientNote.FamFinancial==null) {
				patientNote.FamFinancial="";
			}
			OdSqlParameter paramFamFinancial=new OdSqlParameter("paramFamFinancial",OdDbType.Text,POut.StringNote(patientNote.FamFinancial));
			if(patientNote.ApptPhone==null) {
				patientNote.ApptPhone="";
			}
			OdSqlParameter paramApptPhone=new OdSqlParameter("paramApptPhone",OdDbType.Text,POut.StringParam(patientNote.ApptPhone));
			if(patientNote.Medical==null) {
				patientNote.Medical="";
			}
			OdSqlParameter paramMedical=new OdSqlParameter("paramMedical",OdDbType.Text,POut.StringNote(patientNote.Medical));
			if(patientNote.Service==null) {
				patientNote.Service="";
			}
			OdSqlParameter paramService=new OdSqlParameter("paramService",OdDbType.Text,POut.StringNote(patientNote.Service));
			if(patientNote.MedicalComp==null) {
				patientNote.MedicalComp="";
			}
			OdSqlParameter paramMedicalComp=new OdSqlParameter("paramMedicalComp",OdDbType.Text,POut.StringNote(patientNote.MedicalComp));
			if(patientNote.Treatment==null) {
				patientNote.Treatment="";
			}
			OdSqlParameter paramTreatment=new OdSqlParameter("paramTreatment",OdDbType.Text,POut.StringNote(patientNote.Treatment));
			Db.NonQ(command,paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
		}

		///<summary>Updates one PatientNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatientNote patientNote,PatientNote oldPatientNote) {
			string command="";
			//FamFinancial excluded from update
			if(patientNote.ApptPhone != oldPatientNote.ApptPhone) {
				if(command!="") { command+=",";}
				command+="ApptPhone = "+DbHelper.ParamChar+"paramApptPhone";
			}
			if(patientNote.Medical != oldPatientNote.Medical) {
				if(command!="") { command+=",";}
				command+="Medical = "+DbHelper.ParamChar+"paramMedical";
			}
			if(patientNote.Service != oldPatientNote.Service) {
				if(command!="") { command+=",";}
				command+="Service = "+DbHelper.ParamChar+"paramService";
			}
			if(patientNote.MedicalComp != oldPatientNote.MedicalComp) {
				if(command!="") { command+=",";}
				command+="MedicalComp = "+DbHelper.ParamChar+"paramMedicalComp";
			}
			if(patientNote.Treatment != oldPatientNote.Treatment) {
				if(command!="") { command+=",";}
				command+="Treatment = "+DbHelper.ParamChar+"paramTreatment";
			}
			if(patientNote.ICEName != oldPatientNote.ICEName) {
				if(command!="") { command+=",";}
				command+="ICEName = '"+POut.String(patientNote.ICEName)+"'";
			}
			if(patientNote.ICEPhone != oldPatientNote.ICEPhone) {
				if(command!="") { command+=",";}
				command+="ICEPhone = '"+POut.String(patientNote.ICEPhone)+"'";
			}
			if(patientNote.OrthoMonthsTreatOverride != oldPatientNote.OrthoMonthsTreatOverride) {
				if(command!="") { command+=",";}
				command+="OrthoMonthsTreatOverride = "+POut.Int(patientNote.OrthoMonthsTreatOverride)+"";
			}
			if(patientNote.DateOrthoPlacementOverride.Date != oldPatientNote.DateOrthoPlacementOverride.Date) {
				if(command!="") { command+=",";}
				command+="DateOrthoPlacementOverride = "+POut.Date(patientNote.DateOrthoPlacementOverride)+"";
			}
			if(command=="") {
				return false;
			}
			if(patientNote.FamFinancial==null) {
				patientNote.FamFinancial="";
			}
			OdSqlParameter paramFamFinancial=new OdSqlParameter("paramFamFinancial",OdDbType.Text,POut.StringNote(patientNote.FamFinancial));
			if(patientNote.ApptPhone==null) {
				patientNote.ApptPhone="";
			}
			OdSqlParameter paramApptPhone=new OdSqlParameter("paramApptPhone",OdDbType.Text,POut.StringParam(patientNote.ApptPhone));
			if(patientNote.Medical==null) {
				patientNote.Medical="";
			}
			OdSqlParameter paramMedical=new OdSqlParameter("paramMedical",OdDbType.Text,POut.StringNote(patientNote.Medical));
			if(patientNote.Service==null) {
				patientNote.Service="";
			}
			OdSqlParameter paramService=new OdSqlParameter("paramService",OdDbType.Text,POut.StringNote(patientNote.Service));
			if(patientNote.MedicalComp==null) {
				patientNote.MedicalComp="";
			}
			OdSqlParameter paramMedicalComp=new OdSqlParameter("paramMedicalComp",OdDbType.Text,POut.StringNote(patientNote.MedicalComp));
			if(patientNote.Treatment==null) {
				patientNote.Treatment="";
			}
			OdSqlParameter paramTreatment=new OdSqlParameter("paramTreatment",OdDbType.Text,POut.StringNote(patientNote.Treatment));
			command="UPDATE patientnote SET "+command
				+" WHERE PatNum = "+POut.Long(patientNote.PatNum);
			Db.NonQ(command,paramFamFinancial,paramApptPhone,paramMedical,paramService,paramMedicalComp,paramTreatment);
			return true;
		}

		///<summary>Returns true if Update(PatientNote,PatientNote) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PatientNote patientNote,PatientNote oldPatientNote) {
			//FamFinancial excluded from update
			if(patientNote.ApptPhone != oldPatientNote.ApptPhone) {
				return true;
			}
			if(patientNote.Medical != oldPatientNote.Medical) {
				return true;
			}
			if(patientNote.Service != oldPatientNote.Service) {
				return true;
			}
			if(patientNote.MedicalComp != oldPatientNote.MedicalComp) {
				return true;
			}
			if(patientNote.Treatment != oldPatientNote.Treatment) {
				return true;
			}
			if(patientNote.ICEName != oldPatientNote.ICEName) {
				return true;
			}
			if(patientNote.ICEPhone != oldPatientNote.ICEPhone) {
				return true;
			}
			if(patientNote.OrthoMonthsTreatOverride != oldPatientNote.OrthoMonthsTreatOverride) {
				return true;
			}
			if(patientNote.DateOrthoPlacementOverride.Date != oldPatientNote.DateOrthoPlacementOverride.Date) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PatientNote from the database.</summary>
		public static void Delete(long patNum) {
			string command="DELETE FROM patientnote "
				+"WHERE PatNum = "+POut.Long(patNum);
			Db.NonQ(command);
		}

	}
}