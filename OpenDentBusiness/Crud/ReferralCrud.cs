//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ReferralCrud {
		///<summary>Gets one Referral object from the database using the primary key.  Returns null if not found.</summary>
		public static Referral SelectOne(long referralNum) {
			string command="SELECT * FROM referral "
				+"WHERE ReferralNum = "+POut.Long(referralNum);
			List<Referral> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Referral object from the database using a query.</summary>
		public static Referral SelectOne(string command) {
			
			List<Referral> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Referral objects from the database using a query.</summary>
		public static List<Referral> SelectMany(string command) {
			
			List<Referral> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Referral> TableToList(DataTable table) {
			List<Referral> retVal=new List<Referral>();
			Referral referral;
			foreach(DataRow row in table.Rows) {
				referral=new Referral();
				referral.ReferralNum    = PIn.Long  (row["ReferralNum"].ToString());
				referral.LName          = PIn.String(row["LName"].ToString());
				referral.FName          = PIn.String(row["FName"].ToString());
				referral.MName          = PIn.String(row["MName"].ToString());
				referral.SSN            = PIn.String(row["SSN"].ToString());
				referral.UsingTIN       = PIn.Bool  (row["UsingTIN"].ToString());
				referral.Specialty      = PIn.Long  (row["Specialty"].ToString());
				referral.ST             = PIn.String(row["ST"].ToString());
				referral.Telephone      = PIn.String(row["Telephone"].ToString());
				referral.Address        = PIn.String(row["Address"].ToString());
				referral.Address2       = PIn.String(row["Address2"].ToString());
				referral.City           = PIn.String(row["City"].ToString());
				referral.Zip            = PIn.String(row["Zip"].ToString());
				referral.Note           = PIn.String(row["Note"].ToString());
				referral.Phone2         = PIn.String(row["Phone2"].ToString());
				referral.IsHidden       = PIn.Bool  (row["IsHidden"].ToString());
				referral.NotPerson      = PIn.Bool  (row["NotPerson"].ToString());
				referral.Title          = PIn.String(row["Title"].ToString());
				referral.EMail          = PIn.String(row["EMail"].ToString());
				referral.PatNum         = PIn.Long  (row["PatNum"].ToString());
				referral.NationalProvID = PIn.String(row["NationalProvID"].ToString());
				referral.Slip           = PIn.Long  (row["Slip"].ToString());
				referral.IsDoctor       = PIn.Bool  (row["IsDoctor"].ToString());
				referral.IsTrustedDirect= PIn.Bool  (row["IsTrustedDirect"].ToString());
				referral.DateTStamp     = PIn.DateT (row["DateTStamp"].ToString());
				referral.IsPreferred    = PIn.Bool  (row["IsPreferred"].ToString());
				retVal.Add(referral);
			}
			return retVal;
		}

		///<summary>Converts a list of Referral into a DataTable.</summary>
		public static DataTable ListToTable(List<Referral> listReferrals,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Referral";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ReferralNum");
			table.Columns.Add("LName");
			table.Columns.Add("FName");
			table.Columns.Add("MName");
			table.Columns.Add("SSN");
			table.Columns.Add("UsingTIN");
			table.Columns.Add("Specialty");
			table.Columns.Add("ST");
			table.Columns.Add("Telephone");
			table.Columns.Add("Address");
			table.Columns.Add("Address2");
			table.Columns.Add("City");
			table.Columns.Add("Zip");
			table.Columns.Add("Note");
			table.Columns.Add("Phone2");
			table.Columns.Add("IsHidden");
			table.Columns.Add("NotPerson");
			table.Columns.Add("Title");
			table.Columns.Add("EMail");
			table.Columns.Add("PatNum");
			table.Columns.Add("NationalProvID");
			table.Columns.Add("Slip");
			table.Columns.Add("IsDoctor");
			table.Columns.Add("IsTrustedDirect");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("IsPreferred");
			foreach(Referral referral in listReferrals) {
				table.Rows.Add(new object[] {
					POut.Long  (referral.ReferralNum),
					            referral.LName,
					            referral.FName,
					            referral.MName,
					            referral.SSN,
					POut.Bool  (referral.UsingTIN),
					POut.Long  (referral.Specialty),
					            referral.ST,
					            referral.Telephone,
					            referral.Address,
					            referral.Address2,
					            referral.City,
					            referral.Zip,
					            referral.Note,
					            referral.Phone2,
					POut.Bool  (referral.IsHidden),
					POut.Bool  (referral.NotPerson),
					            referral.Title,
					            referral.EMail,
					POut.Long  (referral.PatNum),
					            referral.NationalProvID,
					POut.Long  (referral.Slip),
					POut.Bool  (referral.IsDoctor),
					POut.Bool  (referral.IsTrustedDirect),
					POut.DateT (referral.DateTStamp,false),
					POut.Bool  (referral.IsPreferred),
				});
			}
			return table;
		}

		///<summary>Inserts one Referral into the database.  Returns the new priKey.</summary>
		public static long Insert(Referral referral) {
			return Insert(referral,false);
		}

		///<summary>Inserts one Referral into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Referral referral,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				referral.ReferralNum=ReplicationServers.GetKey("referral","ReferralNum");
			}
			string command="INSERT INTO referral (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ReferralNum,";
			}
			command+="LName,FName,MName,SSN,UsingTIN,Specialty,ST,Telephone,Address,Address2,City,Zip,Note,Phone2,IsHidden,NotPerson,Title,EMail,PatNum,NationalProvID,Slip,IsDoctor,IsTrustedDirect,IsPreferred) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(referral.ReferralNum)+",";
			}
			command+=
				 "'"+POut.String(referral.LName)+"',"
				+"'"+POut.String(referral.FName)+"',"
				+"'"+POut.String(referral.MName)+"',"
				+"'"+POut.String(referral.SSN)+"',"
				+    POut.Bool  (referral.UsingTIN)+","
				+    POut.Long  (referral.Specialty)+","
				+"'"+POut.String(referral.ST)+"',"
				+"'"+POut.String(referral.Telephone)+"',"
				+"'"+POut.String(referral.Address)+"',"
				+"'"+POut.String(referral.Address2)+"',"
				+"'"+POut.String(referral.City)+"',"
				+"'"+POut.String(referral.Zip)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(referral.Phone2)+"',"
				+    POut.Bool  (referral.IsHidden)+","
				+    POut.Bool  (referral.NotPerson)+","
				+"'"+POut.String(referral.Title)+"',"
				+"'"+POut.String(referral.EMail)+"',"
				+    POut.Long  (referral.PatNum)+","
				+"'"+POut.String(referral.NationalProvID)+"',"
				+    POut.Long  (referral.Slip)+","
				+    POut.Bool  (referral.IsDoctor)+","
				+    POut.Bool  (referral.IsTrustedDirect)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (referral.IsPreferred)+")";
			if(referral.Note==null) {
				referral.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(referral.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				referral.ReferralNum=Db.NonQ(command,true,"ReferralNum","referral",paramNote);
			}
			return referral.ReferralNum;
		}

		///<summary>Inserts one Referral into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Referral referral) {
			return InsertNoCache(referral,false);
		}

		///<summary>Inserts one Referral into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Referral referral,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO referral (";
			if(!useExistingPK && isRandomKeys) {
				referral.ReferralNum=ReplicationServers.GetKeyNoCache("referral","ReferralNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ReferralNum,";
			}
			command+="LName,FName,MName,SSN,UsingTIN,Specialty,ST,Telephone,Address,Address2,City,Zip,Note,Phone2,IsHidden,NotPerson,Title,EMail,PatNum,NationalProvID,Slip,IsDoctor,IsTrustedDirect,IsPreferred) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(referral.ReferralNum)+",";
			}
			command+=
				 "'"+POut.String(referral.LName)+"',"
				+"'"+POut.String(referral.FName)+"',"
				+"'"+POut.String(referral.MName)+"',"
				+"'"+POut.String(referral.SSN)+"',"
				+    POut.Bool  (referral.UsingTIN)+","
				+    POut.Long  (referral.Specialty)+","
				+"'"+POut.String(referral.ST)+"',"
				+"'"+POut.String(referral.Telephone)+"',"
				+"'"+POut.String(referral.Address)+"',"
				+"'"+POut.String(referral.Address2)+"',"
				+"'"+POut.String(referral.City)+"',"
				+"'"+POut.String(referral.Zip)+"',"
				+    DbHelper.ParamChar+"paramNote,"
				+"'"+POut.String(referral.Phone2)+"',"
				+    POut.Bool  (referral.IsHidden)+","
				+    POut.Bool  (referral.NotPerson)+","
				+"'"+POut.String(referral.Title)+"',"
				+"'"+POut.String(referral.EMail)+"',"
				+    POut.Long  (referral.PatNum)+","
				+"'"+POut.String(referral.NationalProvID)+"',"
				+    POut.Long  (referral.Slip)+","
				+    POut.Bool  (referral.IsDoctor)+","
				+    POut.Bool  (referral.IsTrustedDirect)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (referral.IsPreferred)+")";
			if(referral.Note==null) {
				referral.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(referral.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				referral.ReferralNum=Db.NonQ(command,true,"ReferralNum","referral",paramNote);
			}
			return referral.ReferralNum;
		}

		///<summary>Updates one Referral in the database.</summary>
		public static void Update(Referral referral) {
			string command="UPDATE referral SET "
				+"LName          = '"+POut.String(referral.LName)+"', "
				+"FName          = '"+POut.String(referral.FName)+"', "
				+"MName          = '"+POut.String(referral.MName)+"', "
				+"SSN            = '"+POut.String(referral.SSN)+"', "
				+"UsingTIN       =  "+POut.Bool  (referral.UsingTIN)+", "
				+"Specialty      =  "+POut.Long  (referral.Specialty)+", "
				+"ST             = '"+POut.String(referral.ST)+"', "
				+"Telephone      = '"+POut.String(referral.Telephone)+"', "
				+"Address        = '"+POut.String(referral.Address)+"', "
				+"Address2       = '"+POut.String(referral.Address2)+"', "
				+"City           = '"+POut.String(referral.City)+"', "
				+"Zip            = '"+POut.String(referral.Zip)+"', "
				+"Note           =  "+DbHelper.ParamChar+"paramNote, "
				+"Phone2         = '"+POut.String(referral.Phone2)+"', "
				+"IsHidden       =  "+POut.Bool  (referral.IsHidden)+", "
				+"NotPerson      =  "+POut.Bool  (referral.NotPerson)+", "
				+"Title          = '"+POut.String(referral.Title)+"', "
				+"EMail          = '"+POut.String(referral.EMail)+"', "
				+"PatNum         =  "+POut.Long  (referral.PatNum)+", "
				+"NationalProvID = '"+POut.String(referral.NationalProvID)+"', "
				+"Slip           =  "+POut.Long  (referral.Slip)+", "
				+"IsDoctor       =  "+POut.Bool  (referral.IsDoctor)+", "
				+"IsTrustedDirect=  "+POut.Bool  (referral.IsTrustedDirect)+", "
				//DateTStamp can only be set by MySQL
				+"IsPreferred    =  "+POut.Bool  (referral.IsPreferred)+" "
				+"WHERE ReferralNum = "+POut.Long(referral.ReferralNum);
			if(referral.Note==null) {
				referral.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(referral.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one Referral in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Referral referral,Referral oldReferral) {
			string command="";
			if(referral.LName != oldReferral.LName) {
				if(command!="") { command+=",";}
				command+="LName = '"+POut.String(referral.LName)+"'";
			}
			if(referral.FName != oldReferral.FName) {
				if(command!="") { command+=",";}
				command+="FName = '"+POut.String(referral.FName)+"'";
			}
			if(referral.MName != oldReferral.MName) {
				if(command!="") { command+=",";}
				command+="MName = '"+POut.String(referral.MName)+"'";
			}
			if(referral.SSN != oldReferral.SSN) {
				if(command!="") { command+=",";}
				command+="SSN = '"+POut.String(referral.SSN)+"'";
			}
			if(referral.UsingTIN != oldReferral.UsingTIN) {
				if(command!="") { command+=",";}
				command+="UsingTIN = "+POut.Bool(referral.UsingTIN)+"";
			}
			if(referral.Specialty != oldReferral.Specialty) {
				if(command!="") { command+=",";}
				command+="Specialty = "+POut.Long(referral.Specialty)+"";
			}
			if(referral.ST != oldReferral.ST) {
				if(command!="") { command+=",";}
				command+="ST = '"+POut.String(referral.ST)+"'";
			}
			if(referral.Telephone != oldReferral.Telephone) {
				if(command!="") { command+=",";}
				command+="Telephone = '"+POut.String(referral.Telephone)+"'";
			}
			if(referral.Address != oldReferral.Address) {
				if(command!="") { command+=",";}
				command+="Address = '"+POut.String(referral.Address)+"'";
			}
			if(referral.Address2 != oldReferral.Address2) {
				if(command!="") { command+=",";}
				command+="Address2 = '"+POut.String(referral.Address2)+"'";
			}
			if(referral.City != oldReferral.City) {
				if(command!="") { command+=",";}
				command+="City = '"+POut.String(referral.City)+"'";
			}
			if(referral.Zip != oldReferral.Zip) {
				if(command!="") { command+=",";}
				command+="Zip = '"+POut.String(referral.Zip)+"'";
			}
			if(referral.Note != oldReferral.Note) {
				if(command!="") { command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(referral.Phone2 != oldReferral.Phone2) {
				if(command!="") { command+=",";}
				command+="Phone2 = '"+POut.String(referral.Phone2)+"'";
			}
			if(referral.IsHidden != oldReferral.IsHidden) {
				if(command!="") { command+=",";}
				command+="IsHidden = "+POut.Bool(referral.IsHidden)+"";
			}
			if(referral.NotPerson != oldReferral.NotPerson) {
				if(command!="") { command+=",";}
				command+="NotPerson = "+POut.Bool(referral.NotPerson)+"";
			}
			if(referral.Title != oldReferral.Title) {
				if(command!="") { command+=",";}
				command+="Title = '"+POut.String(referral.Title)+"'";
			}
			if(referral.EMail != oldReferral.EMail) {
				if(command!="") { command+=",";}
				command+="EMail = '"+POut.String(referral.EMail)+"'";
			}
			if(referral.PatNum != oldReferral.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(referral.PatNum)+"";
			}
			if(referral.NationalProvID != oldReferral.NationalProvID) {
				if(command!="") { command+=",";}
				command+="NationalProvID = '"+POut.String(referral.NationalProvID)+"'";
			}
			if(referral.Slip != oldReferral.Slip) {
				if(command!="") { command+=",";}
				command+="Slip = "+POut.Long(referral.Slip)+"";
			}
			if(referral.IsDoctor != oldReferral.IsDoctor) {
				if(command!="") { command+=",";}
				command+="IsDoctor = "+POut.Bool(referral.IsDoctor)+"";
			}
			if(referral.IsTrustedDirect != oldReferral.IsTrustedDirect) {
				if(command!="") { command+=",";}
				command+="IsTrustedDirect = "+POut.Bool(referral.IsTrustedDirect)+"";
			}
			//DateTStamp can only be set by MySQL
			if(referral.IsPreferred != oldReferral.IsPreferred) {
				if(command!="") { command+=",";}
				command+="IsPreferred = "+POut.Bool(referral.IsPreferred)+"";
			}
			if(command=="") {
				return false;
			}
			if(referral.Note==null) {
				referral.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringParam(referral.Note));
			command="UPDATE referral SET "+command
				+" WHERE ReferralNum = "+POut.Long(referral.ReferralNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(Referral,Referral) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Referral referral,Referral oldReferral) {
			if(referral.LName != oldReferral.LName) {
				return true;
			}
			if(referral.FName != oldReferral.FName) {
				return true;
			}
			if(referral.MName != oldReferral.MName) {
				return true;
			}
			if(referral.SSN != oldReferral.SSN) {
				return true;
			}
			if(referral.UsingTIN != oldReferral.UsingTIN) {
				return true;
			}
			if(referral.Specialty != oldReferral.Specialty) {
				return true;
			}
			if(referral.ST != oldReferral.ST) {
				return true;
			}
			if(referral.Telephone != oldReferral.Telephone) {
				return true;
			}
			if(referral.Address != oldReferral.Address) {
				return true;
			}
			if(referral.Address2 != oldReferral.Address2) {
				return true;
			}
			if(referral.City != oldReferral.City) {
				return true;
			}
			if(referral.Zip != oldReferral.Zip) {
				return true;
			}
			if(referral.Note != oldReferral.Note) {
				return true;
			}
			if(referral.Phone2 != oldReferral.Phone2) {
				return true;
			}
			if(referral.IsHidden != oldReferral.IsHidden) {
				return true;
			}
			if(referral.NotPerson != oldReferral.NotPerson) {
				return true;
			}
			if(referral.Title != oldReferral.Title) {
				return true;
			}
			if(referral.EMail != oldReferral.EMail) {
				return true;
			}
			if(referral.PatNum != oldReferral.PatNum) {
				return true;
			}
			if(referral.NationalProvID != oldReferral.NationalProvID) {
				return true;
			}
			if(referral.Slip != oldReferral.Slip) {
				return true;
			}
			if(referral.IsDoctor != oldReferral.IsDoctor) {
				return true;
			}
			if(referral.IsTrustedDirect != oldReferral.IsTrustedDirect) {
				return true;
			}
			//DateTStamp can only be set by MySQL
			if(referral.IsPreferred != oldReferral.IsPreferred) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Referral from the database.</summary>
		public static void Delete(long referralNum) {
			string command="DELETE FROM referral "
				+"WHERE ReferralNum = "+POut.Long(referralNum);
			Db.NonQ(command);
		}

	}
}