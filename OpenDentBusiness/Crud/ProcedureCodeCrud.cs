//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcedureCodeCrud {
		///<summary>Gets one ProcedureCode object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcedureCode SelectOne(long codeNum) {
			string command="SELECT * FROM procedurecode "
				+"WHERE CodeNum = "+POut.Long(codeNum);
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcedureCode object from the database using a query.</summary>
		public static ProcedureCode SelectOne(string command) {
			
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcedureCode objects from the database using a query.</summary>
		public static List<ProcedureCode> SelectMany(string command) {
			
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcedureCode> TableToList(DataTable table) {
			List<ProcedureCode> retVal=new List<ProcedureCode>();
			ProcedureCode procedureCode;
			foreach(DataRow row in table.Rows) {
				procedureCode=new ProcedureCode();
				procedureCode.CodeNum           = PIn.Long  (row["CodeNum"].ToString());
				procedureCode.ProcCode          = PIn.String(row["ProcCode"].ToString());
				procedureCode.Descript          = PIn.String(row["Descript"].ToString());
				procedureCode.AbbrDesc          = PIn.String(row["AbbrDesc"].ToString());
				procedureCode.ProcTime          = PIn.String(row["ProcTime"].ToString());
				procedureCode.ProcCat           = PIn.Long  (row["ProcCat"].ToString());
				procedureCode.TreatArea         = (OpenDentBusiness.TreatmentArea)PIn.Int(row["TreatArea"].ToString());
				procedureCode.NoBillIns         = PIn.Bool  (row["NoBillIns"].ToString());
				procedureCode.IsProsth          = PIn.Bool  (row["IsProsth"].ToString());
				procedureCode.DefaultNote       = PIn.String(row["DefaultNote"].ToString());
				procedureCode.IsHygiene         = PIn.Bool  (row["IsHygiene"].ToString());
				procedureCode.GTypeNum          = PIn.Int   (row["GTypeNum"].ToString());
				procedureCode.AlternateCode1    = PIn.String(row["AlternateCode1"].ToString());
				procedureCode.MedicalCode       = PIn.String(row["MedicalCode"].ToString());
				procedureCode.IsTaxed           = PIn.Bool  (row["IsTaxed"].ToString());
				procedureCode.PaintType         = (OpenDentBusiness.ToothPaintingType)PIn.Int(row["PaintType"].ToString());
				procedureCode.GraphicColor      = Color.FromArgb(PIn.Int(row["GraphicColor"].ToString()));
				procedureCode.LaymanTerm        = PIn.String(row["LaymanTerm"].ToString());
				procedureCode.IsCanadianLab     = PIn.Bool  (row["IsCanadianLab"].ToString());
				procedureCode.PreExisting       = PIn.Bool  (row["PreExisting"].ToString());
				procedureCode.BaseUnits         = PIn.Int   (row["BaseUnits"].ToString());
				procedureCode.SubstitutionCode  = PIn.String(row["SubstitutionCode"].ToString());
				procedureCode.SubstOnlyIf       = (OpenDentBusiness.SubstitutionCondition)PIn.Int(row["SubstOnlyIf"].ToString());
				procedureCode.DateTStamp        = PIn.DateT (row["DateTStamp"].ToString());
				procedureCode.IsMultiVisit      = PIn.Bool  (row["IsMultiVisit"].ToString());
				procedureCode.DrugNDC           = PIn.String(row["DrugNDC"].ToString());
				procedureCode.RevenueCodeDefault= PIn.String(row["RevenueCodeDefault"].ToString());
				procedureCode.ProvNumDefault    = PIn.Long  (row["ProvNumDefault"].ToString());
				procedureCode.CanadaTimeUnits   = PIn.Double(row["CanadaTimeUnits"].ToString());
				procedureCode.IsRadiology       = PIn.Bool  (row["IsRadiology"].ToString());
				procedureCode.DefaultClaimNote  = PIn.String(row["DefaultClaimNote"].ToString());
				procedureCode.DefaultTPNote     = PIn.String(row["DefaultTPNote"].ToString());
				procedureCode.BypassGlobalLock  = (OpenDentBusiness.BypassLockStatus)PIn.Int(row["BypassGlobalLock"].ToString());
				procedureCode.TaxCode           = PIn.String(row["TaxCode"].ToString());
				retVal.Add(procedureCode);
			}
			return retVal;
		}

		///<summary>Converts a list of ProcedureCode into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcedureCode> listProcedureCodes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProcedureCode";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CodeNum");
			table.Columns.Add("ProcCode");
			table.Columns.Add("Descript");
			table.Columns.Add("AbbrDesc");
			table.Columns.Add("ProcTime");
			table.Columns.Add("ProcCat");
			table.Columns.Add("TreatArea");
			table.Columns.Add("NoBillIns");
			table.Columns.Add("IsProsth");
			table.Columns.Add("DefaultNote");
			table.Columns.Add("IsHygiene");
			table.Columns.Add("GTypeNum");
			table.Columns.Add("AlternateCode1");
			table.Columns.Add("MedicalCode");
			table.Columns.Add("IsTaxed");
			table.Columns.Add("PaintType");
			table.Columns.Add("GraphicColor");
			table.Columns.Add("LaymanTerm");
			table.Columns.Add("IsCanadianLab");
			table.Columns.Add("PreExisting");
			table.Columns.Add("BaseUnits");
			table.Columns.Add("SubstitutionCode");
			table.Columns.Add("SubstOnlyIf");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("IsMultiVisit");
			table.Columns.Add("DrugNDC");
			table.Columns.Add("RevenueCodeDefault");
			table.Columns.Add("ProvNumDefault");
			table.Columns.Add("CanadaTimeUnits");
			table.Columns.Add("IsRadiology");
			table.Columns.Add("DefaultClaimNote");
			table.Columns.Add("DefaultTPNote");
			table.Columns.Add("BypassGlobalLock");
			table.Columns.Add("TaxCode");
			foreach(ProcedureCode procedureCode in listProcedureCodes) {
				table.Rows.Add(new object[] {
					POut.Long  (procedureCode.CodeNum),
					            procedureCode.ProcCode,
					            procedureCode.Descript,
					            procedureCode.AbbrDesc,
					            procedureCode.ProcTime,
					POut.Long  (procedureCode.ProcCat),
					POut.Int   ((int)procedureCode.TreatArea),
					POut.Bool  (procedureCode.NoBillIns),
					POut.Bool  (procedureCode.IsProsth),
					            procedureCode.DefaultNote,
					POut.Bool  (procedureCode.IsHygiene),
					POut.Int   (procedureCode.GTypeNum),
					            procedureCode.AlternateCode1,
					            procedureCode.MedicalCode,
					POut.Bool  (procedureCode.IsTaxed),
					POut.Int   ((int)procedureCode.PaintType),
					POut.Int   (procedureCode.GraphicColor.ToArgb()),
					            procedureCode.LaymanTerm,
					POut.Bool  (procedureCode.IsCanadianLab),
					POut.Bool  (procedureCode.PreExisting),
					POut.Int   (procedureCode.BaseUnits),
					            procedureCode.SubstitutionCode,
					POut.Int   ((int)procedureCode.SubstOnlyIf),
					POut.DateT (procedureCode.DateTStamp,false),
					POut.Bool  (procedureCode.IsMultiVisit),
					            procedureCode.DrugNDC,
					            procedureCode.RevenueCodeDefault,
					POut.Long  (procedureCode.ProvNumDefault),
					POut.Double(procedureCode.CanadaTimeUnits),
					POut.Bool  (procedureCode.IsRadiology),
					            procedureCode.DefaultClaimNote,
					            procedureCode.DefaultTPNote,
					POut.Int   ((int)procedureCode.BypassGlobalLock),
					            procedureCode.TaxCode,
				});
			}
			return table;
		}

		///<summary>Inserts one ProcedureCode into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcedureCode procedureCode) {
			return Insert(procedureCode,false);
		}

		///<summary>Inserts one ProcedureCode into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcedureCode procedureCode,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				procedureCode.CodeNum=ReplicationServers.GetKey("procedurecode","CodeNum");
			}
			string command="INSERT INTO procedurecode (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="CodeNum,";
			}
			command+="ProcCode,Descript,AbbrDesc,ProcTime,ProcCat,TreatArea,NoBillIns,IsProsth,DefaultNote,IsHygiene,GTypeNum,AlternateCode1,MedicalCode,IsTaxed,PaintType,GraphicColor,LaymanTerm,IsCanadianLab,PreExisting,BaseUnits,SubstitutionCode,SubstOnlyIf,IsMultiVisit,DrugNDC,RevenueCodeDefault,ProvNumDefault,CanadaTimeUnits,IsRadiology,DefaultClaimNote,DefaultTPNote,BypassGlobalLock,TaxCode) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(procedureCode.CodeNum)+",";
			}
			command+=
				 "'"+POut.String(procedureCode.ProcCode)+"',"
				+"'"+POut.String(procedureCode.Descript)+"',"
				+"'"+POut.String(procedureCode.AbbrDesc)+"',"
				+"'"+POut.String(procedureCode.ProcTime)+"',"
				+    POut.Long  (procedureCode.ProcCat)+","
				+    POut.Int   ((int)procedureCode.TreatArea)+","
				+    POut.Bool  (procedureCode.NoBillIns)+","
				+    POut.Bool  (procedureCode.IsProsth)+","
				+    DbHelper.ParamChar+"paramDefaultNote,"
				+    POut.Bool  (procedureCode.IsHygiene)+","
				+    POut.Int   (procedureCode.GTypeNum)+","
				+"'"+POut.String(procedureCode.AlternateCode1)+"',"
				+"'"+POut.String(procedureCode.MedicalCode)+"',"
				+    POut.Bool  (procedureCode.IsTaxed)+","
				+    POut.Int   ((int)procedureCode.PaintType)+","
				+    POut.Int   (procedureCode.GraphicColor.ToArgb())+","
				+"'"+POut.String(procedureCode.LaymanTerm)+"',"
				+    POut.Bool  (procedureCode.IsCanadianLab)+","
				+    POut.Bool  (procedureCode.PreExisting)+","
				+    POut.Int   (procedureCode.BaseUnits)+","
				+"'"+POut.String(procedureCode.SubstitutionCode)+"',"
				+    POut.Int   ((int)procedureCode.SubstOnlyIf)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (procedureCode.IsMultiVisit)+","
				+"'"+POut.String(procedureCode.DrugNDC)+"',"
				+"'"+POut.String(procedureCode.RevenueCodeDefault)+"',"
				+    POut.Long  (procedureCode.ProvNumDefault)+","
				+"'"+POut.Double(procedureCode.CanadaTimeUnits)+"',"
				+    POut.Bool  (procedureCode.IsRadiology)+","
				+    DbHelper.ParamChar+"paramDefaultClaimNote,"
				+    DbHelper.ParamChar+"paramDefaultTPNote,"
				+    POut.Int   ((int)procedureCode.BypassGlobalLock)+","
				+"'"+POut.String(procedureCode.TaxCode)+"')";
			if(procedureCode.DefaultNote==null) {
				procedureCode.DefaultNote="";
			}
			OdSqlParameter paramDefaultNote=new OdSqlParameter("paramDefaultNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultNote));
			if(procedureCode.DefaultClaimNote==null) {
				procedureCode.DefaultClaimNote="";
			}
			OdSqlParameter paramDefaultClaimNote=new OdSqlParameter("paramDefaultClaimNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultClaimNote));
			if(procedureCode.DefaultTPNote==null) {
				procedureCode.DefaultTPNote="";
			}
			OdSqlParameter paramDefaultTPNote=new OdSqlParameter("paramDefaultTPNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultTPNote));
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command,paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
			}
			else {
				procedureCode.CodeNum=Db.NonQ(command,true,"CodeNum","procedureCode",paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
			}
			return procedureCode.CodeNum;
		}

		///<summary>Inserts one ProcedureCode into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcedureCode procedureCode) {
			return InsertNoCache(procedureCode,false);
		}

		///<summary>Inserts one ProcedureCode into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcedureCode procedureCode,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO procedurecode (";
			if(!useExistingPK && isRandomKeys) {
				procedureCode.CodeNum=ReplicationServers.GetKeyNoCache("procedurecode","CodeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CodeNum,";
			}
			command+="ProcCode,Descript,AbbrDesc,ProcTime,ProcCat,TreatArea,NoBillIns,IsProsth,DefaultNote,IsHygiene,GTypeNum,AlternateCode1,MedicalCode,IsTaxed,PaintType,GraphicColor,LaymanTerm,IsCanadianLab,PreExisting,BaseUnits,SubstitutionCode,SubstOnlyIf,IsMultiVisit,DrugNDC,RevenueCodeDefault,ProvNumDefault,CanadaTimeUnits,IsRadiology,DefaultClaimNote,DefaultTPNote,BypassGlobalLock,TaxCode) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procedureCode.CodeNum)+",";
			}
			command+=
				 "'"+POut.String(procedureCode.ProcCode)+"',"
				+"'"+POut.String(procedureCode.Descript)+"',"
				+"'"+POut.String(procedureCode.AbbrDesc)+"',"
				+"'"+POut.String(procedureCode.ProcTime)+"',"
				+    POut.Long  (procedureCode.ProcCat)+","
				+    POut.Int   ((int)procedureCode.TreatArea)+","
				+    POut.Bool  (procedureCode.NoBillIns)+","
				+    POut.Bool  (procedureCode.IsProsth)+","
				+    DbHelper.ParamChar+"paramDefaultNote,"
				+    POut.Bool  (procedureCode.IsHygiene)+","
				+    POut.Int   (procedureCode.GTypeNum)+","
				+"'"+POut.String(procedureCode.AlternateCode1)+"',"
				+"'"+POut.String(procedureCode.MedicalCode)+"',"
				+    POut.Bool  (procedureCode.IsTaxed)+","
				+    POut.Int   ((int)procedureCode.PaintType)+","
				+    POut.Int   (procedureCode.GraphicColor.ToArgb())+","
				+"'"+POut.String(procedureCode.LaymanTerm)+"',"
				+    POut.Bool  (procedureCode.IsCanadianLab)+","
				+    POut.Bool  (procedureCode.PreExisting)+","
				+    POut.Int   (procedureCode.BaseUnits)+","
				+"'"+POut.String(procedureCode.SubstitutionCode)+"',"
				+    POut.Int   ((int)procedureCode.SubstOnlyIf)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (procedureCode.IsMultiVisit)+","
				+"'"+POut.String(procedureCode.DrugNDC)+"',"
				+"'"+POut.String(procedureCode.RevenueCodeDefault)+"',"
				+    POut.Long  (procedureCode.ProvNumDefault)+","
				+"'"+POut.Double(procedureCode.CanadaTimeUnits)+"',"
				+    POut.Bool  (procedureCode.IsRadiology)+","
				+    DbHelper.ParamChar+"paramDefaultClaimNote,"
				+    DbHelper.ParamChar+"paramDefaultTPNote,"
				+    POut.Int   ((int)procedureCode.BypassGlobalLock)+","
				+"'"+POut.String(procedureCode.TaxCode)+"')";
			if(procedureCode.DefaultNote==null) {
				procedureCode.DefaultNote="";
			}
			OdSqlParameter paramDefaultNote=new OdSqlParameter("paramDefaultNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultNote));
			if(procedureCode.DefaultClaimNote==null) {
				procedureCode.DefaultClaimNote="";
			}
			OdSqlParameter paramDefaultClaimNote=new OdSqlParameter("paramDefaultClaimNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultClaimNote));
			if(procedureCode.DefaultTPNote==null) {
				procedureCode.DefaultTPNote="";
			}
			OdSqlParameter paramDefaultTPNote=new OdSqlParameter("paramDefaultTPNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultTPNote));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
			}
			else {
				procedureCode.CodeNum=Db.NonQ(command,true,"CodeNum","procedureCode",paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
			}
			return procedureCode.CodeNum;
		}

		///<summary>Updates one ProcedureCode in the database.</summary>
		public static void Update(ProcedureCode procedureCode) {
			string command="UPDATE procedurecode SET "
				//ProcCode excluded from update
				+"Descript          = '"+POut.String(procedureCode.Descript)+"', "
				+"AbbrDesc          = '"+POut.String(procedureCode.AbbrDesc)+"', "
				+"ProcTime          = '"+POut.String(procedureCode.ProcTime)+"', "
				+"ProcCat           =  "+POut.Long  (procedureCode.ProcCat)+", "
				+"TreatArea         =  "+POut.Int   ((int)procedureCode.TreatArea)+", "
				+"NoBillIns         =  "+POut.Bool  (procedureCode.NoBillIns)+", "
				+"IsProsth          =  "+POut.Bool  (procedureCode.IsProsth)+", "
				+"DefaultNote       =  "+DbHelper.ParamChar+"paramDefaultNote, "
				+"IsHygiene         =  "+POut.Bool  (procedureCode.IsHygiene)+", "
				+"GTypeNum          =  "+POut.Int   (procedureCode.GTypeNum)+", "
				+"AlternateCode1    = '"+POut.String(procedureCode.AlternateCode1)+"', "
				+"MedicalCode       = '"+POut.String(procedureCode.MedicalCode)+"', "
				+"IsTaxed           =  "+POut.Bool  (procedureCode.IsTaxed)+", "
				+"PaintType         =  "+POut.Int   ((int)procedureCode.PaintType)+", "
				+"GraphicColor      =  "+POut.Int   (procedureCode.GraphicColor.ToArgb())+", "
				+"LaymanTerm        = '"+POut.String(procedureCode.LaymanTerm)+"', "
				+"IsCanadianLab     =  "+POut.Bool  (procedureCode.IsCanadianLab)+", "
				+"PreExisting       =  "+POut.Bool  (procedureCode.PreExisting)+", "
				+"BaseUnits         =  "+POut.Int   (procedureCode.BaseUnits)+", "
				+"SubstitutionCode  = '"+POut.String(procedureCode.SubstitutionCode)+"', "
				+"SubstOnlyIf       =  "+POut.Int   ((int)procedureCode.SubstOnlyIf)+", "
				//DateTStamp can only be set by MySQL
				+"IsMultiVisit      =  "+POut.Bool  (procedureCode.IsMultiVisit)+", "
				+"DrugNDC           = '"+POut.String(procedureCode.DrugNDC)+"', "
				+"RevenueCodeDefault= '"+POut.String(procedureCode.RevenueCodeDefault)+"', "
				+"ProvNumDefault    =  "+POut.Long  (procedureCode.ProvNumDefault)+", "
				+"CanadaTimeUnits   = '"+POut.Double(procedureCode.CanadaTimeUnits)+"', "
				+"IsRadiology       =  "+POut.Bool  (procedureCode.IsRadiology)+", "
				+"DefaultClaimNote  =  "+DbHelper.ParamChar+"paramDefaultClaimNote, "
				+"DefaultTPNote     =  "+DbHelper.ParamChar+"paramDefaultTPNote, "
				+"BypassGlobalLock  =  "+POut.Int   ((int)procedureCode.BypassGlobalLock)+", "
				+"TaxCode           = '"+POut.String(procedureCode.TaxCode)+"' "
				+"WHERE CodeNum = "+POut.Long(procedureCode.CodeNum);
			if(procedureCode.DefaultNote==null) {
				procedureCode.DefaultNote="";
			}
			OdSqlParameter paramDefaultNote=new OdSqlParameter("paramDefaultNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultNote));
			if(procedureCode.DefaultClaimNote==null) {
				procedureCode.DefaultClaimNote="";
			}
			OdSqlParameter paramDefaultClaimNote=new OdSqlParameter("paramDefaultClaimNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultClaimNote));
			if(procedureCode.DefaultTPNote==null) {
				procedureCode.DefaultTPNote="";
			}
			OdSqlParameter paramDefaultTPNote=new OdSqlParameter("paramDefaultTPNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultTPNote));
			Db.NonQ(command,paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
		}

		///<summary>Updates one ProcedureCode in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcedureCode procedureCode,ProcedureCode oldProcedureCode) {
			string command="";
			//ProcCode excluded from update
			if(procedureCode.Descript != oldProcedureCode.Descript) {
				if(command!="") { command+=",";}
				command+="Descript = '"+POut.String(procedureCode.Descript)+"'";
			}
			if(procedureCode.AbbrDesc != oldProcedureCode.AbbrDesc) {
				if(command!="") { command+=",";}
				command+="AbbrDesc = '"+POut.String(procedureCode.AbbrDesc)+"'";
			}
			if(procedureCode.ProcTime != oldProcedureCode.ProcTime) {
				if(command!="") { command+=",";}
				command+="ProcTime = '"+POut.String(procedureCode.ProcTime)+"'";
			}
			if(procedureCode.ProcCat != oldProcedureCode.ProcCat) {
				if(command!="") { command+=",";}
				command+="ProcCat = "+POut.Long(procedureCode.ProcCat)+"";
			}
			if(procedureCode.TreatArea != oldProcedureCode.TreatArea) {
				if(command!="") { command+=",";}
				command+="TreatArea = "+POut.Int   ((int)procedureCode.TreatArea)+"";
			}
			if(procedureCode.NoBillIns != oldProcedureCode.NoBillIns) {
				if(command!="") { command+=",";}
				command+="NoBillIns = "+POut.Bool(procedureCode.NoBillIns)+"";
			}
			if(procedureCode.IsProsth != oldProcedureCode.IsProsth) {
				if(command!="") { command+=",";}
				command+="IsProsth = "+POut.Bool(procedureCode.IsProsth)+"";
			}
			if(procedureCode.DefaultNote != oldProcedureCode.DefaultNote) {
				if(command!="") { command+=",";}
				command+="DefaultNote = "+DbHelper.ParamChar+"paramDefaultNote";
			}
			if(procedureCode.IsHygiene != oldProcedureCode.IsHygiene) {
				if(command!="") { command+=",";}
				command+="IsHygiene = "+POut.Bool(procedureCode.IsHygiene)+"";
			}
			if(procedureCode.GTypeNum != oldProcedureCode.GTypeNum) {
				if(command!="") { command+=",";}
				command+="GTypeNum = "+POut.Int(procedureCode.GTypeNum)+"";
			}
			if(procedureCode.AlternateCode1 != oldProcedureCode.AlternateCode1) {
				if(command!="") { command+=",";}
				command+="AlternateCode1 = '"+POut.String(procedureCode.AlternateCode1)+"'";
			}
			if(procedureCode.MedicalCode != oldProcedureCode.MedicalCode) {
				if(command!="") { command+=",";}
				command+="MedicalCode = '"+POut.String(procedureCode.MedicalCode)+"'";
			}
			if(procedureCode.IsTaxed != oldProcedureCode.IsTaxed) {
				if(command!="") { command+=",";}
				command+="IsTaxed = "+POut.Bool(procedureCode.IsTaxed)+"";
			}
			if(procedureCode.PaintType != oldProcedureCode.PaintType) {
				if(command!="") { command+=",";}
				command+="PaintType = "+POut.Int   ((int)procedureCode.PaintType)+"";
			}
			if(procedureCode.GraphicColor != oldProcedureCode.GraphicColor) {
				if(command!="") { command+=",";}
				command+="GraphicColor = "+POut.Int(procedureCode.GraphicColor.ToArgb())+"";
			}
			if(procedureCode.LaymanTerm != oldProcedureCode.LaymanTerm) {
				if(command!="") { command+=",";}
				command+="LaymanTerm = '"+POut.String(procedureCode.LaymanTerm)+"'";
			}
			if(procedureCode.IsCanadianLab != oldProcedureCode.IsCanadianLab) {
				if(command!="") { command+=",";}
				command+="IsCanadianLab = "+POut.Bool(procedureCode.IsCanadianLab)+"";
			}
			if(procedureCode.PreExisting != oldProcedureCode.PreExisting) {
				if(command!="") { command+=",";}
				command+="PreExisting = "+POut.Bool(procedureCode.PreExisting)+"";
			}
			if(procedureCode.BaseUnits != oldProcedureCode.BaseUnits) {
				if(command!="") { command+=",";}
				command+="BaseUnits = "+POut.Int(procedureCode.BaseUnits)+"";
			}
			if(procedureCode.SubstitutionCode != oldProcedureCode.SubstitutionCode) {
				if(command!="") { command+=",";}
				command+="SubstitutionCode = '"+POut.String(procedureCode.SubstitutionCode)+"'";
			}
			if(procedureCode.SubstOnlyIf != oldProcedureCode.SubstOnlyIf) {
				if(command!="") { command+=",";}
				command+="SubstOnlyIf = "+POut.Int   ((int)procedureCode.SubstOnlyIf)+"";
			}
			//DateTStamp can only be set by MySQL
			if(procedureCode.IsMultiVisit != oldProcedureCode.IsMultiVisit) {
				if(command!="") { command+=",";}
				command+="IsMultiVisit = "+POut.Bool(procedureCode.IsMultiVisit)+"";
			}
			if(procedureCode.DrugNDC != oldProcedureCode.DrugNDC) {
				if(command!="") { command+=",";}
				command+="DrugNDC = '"+POut.String(procedureCode.DrugNDC)+"'";
			}
			if(procedureCode.RevenueCodeDefault != oldProcedureCode.RevenueCodeDefault) {
				if(command!="") { command+=",";}
				command+="RevenueCodeDefault = '"+POut.String(procedureCode.RevenueCodeDefault)+"'";
			}
			if(procedureCode.ProvNumDefault != oldProcedureCode.ProvNumDefault) {
				if(command!="") { command+=",";}
				command+="ProvNumDefault = "+POut.Long(procedureCode.ProvNumDefault)+"";
			}
			if(procedureCode.CanadaTimeUnits != oldProcedureCode.CanadaTimeUnits) {
				if(command!="") { command+=",";}
				command+="CanadaTimeUnits = '"+POut.Double(procedureCode.CanadaTimeUnits)+"'";
			}
			if(procedureCode.IsRadiology != oldProcedureCode.IsRadiology) {
				if(command!="") { command+=",";}
				command+="IsRadiology = "+POut.Bool(procedureCode.IsRadiology)+"";
			}
			if(procedureCode.DefaultClaimNote != oldProcedureCode.DefaultClaimNote) {
				if(command!="") { command+=",";}
				command+="DefaultClaimNote = "+DbHelper.ParamChar+"paramDefaultClaimNote";
			}
			if(procedureCode.DefaultTPNote != oldProcedureCode.DefaultTPNote) {
				if(command!="") { command+=",";}
				command+="DefaultTPNote = "+DbHelper.ParamChar+"paramDefaultTPNote";
			}
			if(procedureCode.BypassGlobalLock != oldProcedureCode.BypassGlobalLock) {
				if(command!="") { command+=",";}
				command+="BypassGlobalLock = "+POut.Int   ((int)procedureCode.BypassGlobalLock)+"";
			}
			if(procedureCode.TaxCode != oldProcedureCode.TaxCode) {
				if(command!="") { command+=",";}
				command+="TaxCode = '"+POut.String(procedureCode.TaxCode)+"'";
			}
			if(command=="") {
				return false;
			}
			if(procedureCode.DefaultNote==null) {
				procedureCode.DefaultNote="";
			}
			OdSqlParameter paramDefaultNote=new OdSqlParameter("paramDefaultNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultNote));
			if(procedureCode.DefaultClaimNote==null) {
				procedureCode.DefaultClaimNote="";
			}
			OdSqlParameter paramDefaultClaimNote=new OdSqlParameter("paramDefaultClaimNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultClaimNote));
			if(procedureCode.DefaultTPNote==null) {
				procedureCode.DefaultTPNote="";
			}
			OdSqlParameter paramDefaultTPNote=new OdSqlParameter("paramDefaultTPNote",OdDbType.Text,POut.StringParam(procedureCode.DefaultTPNote));
			command="UPDATE procedurecode SET "+command
				+" WHERE CodeNum = "+POut.Long(procedureCode.CodeNum);
			Db.NonQ(command,paramDefaultNote,paramDefaultClaimNote,paramDefaultTPNote);
			return true;
		}

		///<summary>Returns true if Update(ProcedureCode,ProcedureCode) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProcedureCode procedureCode,ProcedureCode oldProcedureCode) {
			//ProcCode excluded from update
			if(procedureCode.Descript != oldProcedureCode.Descript) {
				return true;
			}
			if(procedureCode.AbbrDesc != oldProcedureCode.AbbrDesc) {
				return true;
			}
			if(procedureCode.ProcTime != oldProcedureCode.ProcTime) {
				return true;
			}
			if(procedureCode.ProcCat != oldProcedureCode.ProcCat) {
				return true;
			}
			if(procedureCode.TreatArea != oldProcedureCode.TreatArea) {
				return true;
			}
			if(procedureCode.NoBillIns != oldProcedureCode.NoBillIns) {
				return true;
			}
			if(procedureCode.IsProsth != oldProcedureCode.IsProsth) {
				return true;
			}
			if(procedureCode.DefaultNote != oldProcedureCode.DefaultNote) {
				return true;
			}
			if(procedureCode.IsHygiene != oldProcedureCode.IsHygiene) {
				return true;
			}
			if(procedureCode.GTypeNum != oldProcedureCode.GTypeNum) {
				return true;
			}
			if(procedureCode.AlternateCode1 != oldProcedureCode.AlternateCode1) {
				return true;
			}
			if(procedureCode.MedicalCode != oldProcedureCode.MedicalCode) {
				return true;
			}
			if(procedureCode.IsTaxed != oldProcedureCode.IsTaxed) {
				return true;
			}
			if(procedureCode.PaintType != oldProcedureCode.PaintType) {
				return true;
			}
			if(procedureCode.GraphicColor != oldProcedureCode.GraphicColor) {
				return true;
			}
			if(procedureCode.LaymanTerm != oldProcedureCode.LaymanTerm) {
				return true;
			}
			if(procedureCode.IsCanadianLab != oldProcedureCode.IsCanadianLab) {
				return true;
			}
			if(procedureCode.PreExisting != oldProcedureCode.PreExisting) {
				return true;
			}
			if(procedureCode.BaseUnits != oldProcedureCode.BaseUnits) {
				return true;
			}
			if(procedureCode.SubstitutionCode != oldProcedureCode.SubstitutionCode) {
				return true;
			}
			if(procedureCode.SubstOnlyIf != oldProcedureCode.SubstOnlyIf) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(procedureCode.IsMultiVisit != oldProcedureCode.IsMultiVisit) {
				return true;
			}
			if(procedureCode.DrugNDC != oldProcedureCode.DrugNDC) {
				return true;
			}
			if(procedureCode.RevenueCodeDefault != oldProcedureCode.RevenueCodeDefault) {
				return true;
			}
			if(procedureCode.ProvNumDefault != oldProcedureCode.ProvNumDefault) {
				return true;
			}
			if(procedureCode.CanadaTimeUnits != oldProcedureCode.CanadaTimeUnits) {
				return true;
			}
			if(procedureCode.IsRadiology != oldProcedureCode.IsRadiology) {
				return true;
			}
			if(procedureCode.DefaultClaimNote != oldProcedureCode.DefaultClaimNote) {
				return true;
			}
			if(procedureCode.DefaultTPNote != oldProcedureCode.DefaultTPNote) {
				return true;
			}
			if(procedureCode.BypassGlobalLock != oldProcedureCode.BypassGlobalLock) {
				return true;
			}
			if(procedureCode.TaxCode != oldProcedureCode.TaxCode) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProcedureCode from the database.</summary>
		public static void Delete(long codeNum) {
			ClearFkey(codeNum);
			string command="DELETE FROM procedurecode "
				+"WHERE CodeNum = "+POut.Long(codeNum);
			Db.NonQ(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching codeNum as FKey and are related to ProcedureCode.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the ProcedureCode table type.</summary>
		public static void ClearFkey(long codeNum) {
			if(codeNum==0) {
				return;
			}
			string command="UPDATE securitylog SET FKey=0 WHERE FKey="+POut.Long(codeNum)+" AND PermType IN (64)";
			Db.NonQ(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching codeNums as FKey and are related to ProcedureCode.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the ProcedureCode table type.</summary>
		public static void ClearFkey(List<long> listCodeNums) {
			if(listCodeNums==null || listCodeNums.FindAll(x => x != 0).Count==0) {
				return;
			}
			string command="UPDATE securitylog SET FKey=0 WHERE FKey IN("+String.Join(",",listCodeNums.FindAll(x => x != 0))+") AND PermType IN (64)";
			Db.NonQ(command);
		}

	}
}