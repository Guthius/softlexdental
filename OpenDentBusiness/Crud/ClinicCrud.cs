//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ClinicCrud {
		///<summary>Gets one Clinic object from the database using the primary key.  Returns null if not found.</summary>
		public static Clinic SelectOne(long clinicNum) {
			string command="SELECT * FROM clinic "
				+"WHERE ClinicNum = "+POut.Long(clinicNum);
			List<Clinic> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Clinic object from the database using a query.</summary>
		public static Clinic SelectOne(string command) {
			
			List<Clinic> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Clinic objects from the database using a query.</summary>
		public static List<Clinic> SelectMany(string command) {
			
			List<Clinic> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Clinic> TableToList(DataTable table) {
			List<Clinic> retVal=new List<Clinic>();
			Clinic clinic;
			foreach(DataRow row in table.Rows) {
				clinic=new Clinic();
				clinic.ClinicNum           = PIn.Long  (row["ClinicNum"].ToString());
				clinic.Description         = PIn.String(row["Description"].ToString());
				clinic.Address             = PIn.String(row["Address"].ToString());
				clinic.Address2            = PIn.String(row["Address2"].ToString());
				clinic.City                = PIn.String(row["City"].ToString());
				clinic.State               = PIn.String(row["State"].ToString());
				clinic.Zip                 = PIn.String(row["Zip"].ToString());
				clinic.BillingAddress      = PIn.String(row["BillingAddress"].ToString());
				clinic.BillingAddress2     = PIn.String(row["BillingAddress2"].ToString());
				clinic.BillingCity         = PIn.String(row["BillingCity"].ToString());
				clinic.BillingState        = PIn.String(row["BillingState"].ToString());
				clinic.BillingZip          = PIn.String(row["BillingZip"].ToString());
				clinic.PayToAddress        = PIn.String(row["PayToAddress"].ToString());
				clinic.PayToAddress2       = PIn.String(row["PayToAddress2"].ToString());
				clinic.PayToCity           = PIn.String(row["PayToCity"].ToString());
				clinic.PayToState          = PIn.String(row["PayToState"].ToString());
				clinic.PayToZip            = PIn.String(row["PayToZip"].ToString());
				clinic.Phone               = PIn.String(row["Phone"].ToString());
				clinic.BankNumber          = PIn.String(row["BankNumber"].ToString());
				clinic.DefaultPlaceService = (OpenDentBusiness.PlaceOfService)PIn.Int(row["DefaultPlaceService"].ToString());
				clinic.InsBillingProv      = PIn.Long  (row["InsBillingProv"].ToString());
				clinic.Fax                 = PIn.String(row["Fax"].ToString());
				clinic.EmailAddressNum     = PIn.Long  (row["EmailAddressNum"].ToString());
				clinic.DefaultProv         = PIn.Long  (row["DefaultProv"].ToString());
				clinic.SmsContractDate     = PIn.DateT (row["SmsContractDate"].ToString());
				clinic.SmsMonthlyLimit     = PIn.Double(row["SmsMonthlyLimit"].ToString());
				clinic.IsMedicalOnly       = PIn.Bool  (row["IsMedicalOnly"].ToString());
				clinic.UseBillAddrOnClaims = PIn.Bool  (row["UseBillAddrOnClaims"].ToString());
				clinic.Region              = PIn.Long  (row["Region"].ToString());
				clinic.ItemOrder           = PIn.Int   (row["ItemOrder"].ToString());
				clinic.IsInsVerifyExcluded = PIn.Bool  (row["IsInsVerifyExcluded"].ToString());
				clinic.Abbr                = PIn.String(row["Abbr"].ToString());
				clinic.MedLabAccountNum    = PIn.String(row["MedLabAccountNum"].ToString());
				clinic.IsConfirmEnabled    = PIn.Bool  (row["IsConfirmEnabled"].ToString());
				clinic.IsConfirmDefault    = PIn.Bool  (row["IsConfirmDefault"].ToString());
				clinic.IsNewPatApptExcluded= PIn.Bool  (row["IsNewPatApptExcluded"].ToString());
				clinic.IsHidden            = PIn.Bool  (row["IsHidden"].ToString());
				clinic.ExternalID          = PIn.Long  (row["ExternalID"].ToString());
				clinic.SchedNote           = PIn.String(row["SchedNote"].ToString());
				clinic.HasProcOnRx         = PIn.Bool  (row["HasProcOnRx"].ToString());
				retVal.Add(clinic);
			}
			return retVal;
		}

		///<summary>Converts a list of Clinic into a DataTable.</summary>
		public static DataTable ListToTable(List<Clinic> listClinics,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Clinic";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ClinicNum");
			table.Columns.Add("Description");
			table.Columns.Add("Address");
			table.Columns.Add("Address2");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("BillingAddress");
			table.Columns.Add("BillingAddress2");
			table.Columns.Add("BillingCity");
			table.Columns.Add("BillingState");
			table.Columns.Add("BillingZip");
			table.Columns.Add("PayToAddress");
			table.Columns.Add("PayToAddress2");
			table.Columns.Add("PayToCity");
			table.Columns.Add("PayToState");
			table.Columns.Add("PayToZip");
			table.Columns.Add("Phone");
			table.Columns.Add("BankNumber");
			table.Columns.Add("DefaultPlaceService");
			table.Columns.Add("InsBillingProv");
			table.Columns.Add("Fax");
			table.Columns.Add("EmailAddressNum");
			table.Columns.Add("DefaultProv");
			table.Columns.Add("SmsContractDate");
			table.Columns.Add("SmsMonthlyLimit");
			table.Columns.Add("IsMedicalOnly");
			table.Columns.Add("UseBillAddrOnClaims");
			table.Columns.Add("Region");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("IsInsVerifyExcluded");
			table.Columns.Add("Abbr");
			table.Columns.Add("MedLabAccountNum");
			table.Columns.Add("IsConfirmEnabled");
			table.Columns.Add("IsConfirmDefault");
			table.Columns.Add("IsNewPatApptExcluded");
			table.Columns.Add("IsHidden");
			table.Columns.Add("ExternalID");
			table.Columns.Add("SchedNote");
			table.Columns.Add("HasProcOnRx");
			foreach(Clinic clinic in listClinics) {
				table.Rows.Add(new object[] {
					POut.Long  (clinic.ClinicNum),
					            clinic.Description,
					            clinic.Address,
					            clinic.Address2,
					            clinic.City,
					            clinic.State,
					            clinic.Zip,
					            clinic.BillingAddress,
					            clinic.BillingAddress2,
					            clinic.BillingCity,
					            clinic.BillingState,
					            clinic.BillingZip,
					            clinic.PayToAddress,
					            clinic.PayToAddress2,
					            clinic.PayToCity,
					            clinic.PayToState,
					            clinic.PayToZip,
					            clinic.Phone,
					            clinic.BankNumber,
					POut.Int   ((int)clinic.DefaultPlaceService),
					POut.Long  (clinic.InsBillingProv),
					            clinic.Fax,
					POut.Long  (clinic.EmailAddressNum),
					POut.Long  (clinic.DefaultProv),
					POut.DateT (clinic.SmsContractDate,false),
					POut.Double(clinic.SmsMonthlyLimit),
					POut.Bool  (clinic.IsMedicalOnly),
					POut.Bool  (clinic.UseBillAddrOnClaims),
					POut.Long  (clinic.Region),
					POut.Int   (clinic.ItemOrder),
					POut.Bool  (clinic.IsInsVerifyExcluded),
					            clinic.Abbr,
					            clinic.MedLabAccountNum,
					POut.Bool  (clinic.IsConfirmEnabled),
					POut.Bool  (clinic.IsConfirmDefault),
					POut.Bool  (clinic.IsNewPatApptExcluded),
					POut.Bool  (clinic.IsHidden),
					POut.Long  (clinic.ExternalID),
					            clinic.SchedNote,
					POut.Bool  (clinic.HasProcOnRx),
				});
			}
			return table;
		}

		///<summary>Inserts one Clinic into the database.  Returns the new priKey.</summary>
		public static long Insert(Clinic clinic) {
			return Insert(clinic,false);
		}

		///<summary>Inserts one Clinic into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Clinic clinic,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				clinic.ClinicNum=ReplicationServers.GetKey("clinic","ClinicNum");
			}
			string command="INSERT INTO clinic (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClinicNum,";
			}
			command+="Description,Address,Address2,City,State,Zip,BillingAddress,BillingAddress2,BillingCity,BillingState,BillingZip,PayToAddress,PayToAddress2,PayToCity,PayToState,PayToZip,Phone,BankNumber,DefaultPlaceService,InsBillingProv,Fax,EmailAddressNum,DefaultProv,SmsContractDate,SmsMonthlyLimit,IsMedicalOnly,UseBillAddrOnClaims,Region,ItemOrder,IsInsVerifyExcluded,Abbr,MedLabAccountNum,IsConfirmEnabled,IsConfirmDefault,IsNewPatApptExcluded,IsHidden,ExternalID,SchedNote,HasProcOnRx) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(clinic.ClinicNum)+",";
			}
			command+=
				 "'"+POut.String(clinic.Description)+"',"
				+"'"+POut.String(clinic.Address)+"',"
				+"'"+POut.String(clinic.Address2)+"',"
				+"'"+POut.String(clinic.City)+"',"
				+"'"+POut.String(clinic.State)+"',"
				+"'"+POut.String(clinic.Zip)+"',"
				+"'"+POut.String(clinic.BillingAddress)+"',"
				+"'"+POut.String(clinic.BillingAddress2)+"',"
				+"'"+POut.String(clinic.BillingCity)+"',"
				+"'"+POut.String(clinic.BillingState)+"',"
				+"'"+POut.String(clinic.BillingZip)+"',"
				+"'"+POut.String(clinic.PayToAddress)+"',"
				+"'"+POut.String(clinic.PayToAddress2)+"',"
				+"'"+POut.String(clinic.PayToCity)+"',"
				+"'"+POut.String(clinic.PayToState)+"',"
				+"'"+POut.String(clinic.PayToZip)+"',"
				+"'"+POut.String(clinic.Phone)+"',"
				+"'"+POut.String(clinic.BankNumber)+"',"
				+    POut.Int   ((int)clinic.DefaultPlaceService)+","
				+    POut.Long  (clinic.InsBillingProv)+","
				+"'"+POut.String(clinic.Fax)+"',"
				+    POut.Long  (clinic.EmailAddressNum)+","
				+    POut.Long  (clinic.DefaultProv)+","
				+    POut.DateT (clinic.SmsContractDate)+","
				+"'"+POut.Double(clinic.SmsMonthlyLimit)+"',"
				+    POut.Bool  (clinic.IsMedicalOnly)+","
				+    POut.Bool  (clinic.UseBillAddrOnClaims)+","
				+    POut.Long  (clinic.Region)+","
				+    POut.Int   (clinic.ItemOrder)+","
				+    POut.Bool  (clinic.IsInsVerifyExcluded)+","
				+"'"+POut.String(clinic.Abbr)+"',"
				+"'"+POut.String(clinic.MedLabAccountNum)+"',"
				+    POut.Bool  (clinic.IsConfirmEnabled)+","
				+    POut.Bool  (clinic.IsConfirmDefault)+","
				+    POut.Bool  (clinic.IsNewPatApptExcluded)+","
				+    POut.Bool  (clinic.IsHidden)+","
				+    POut.Long  (clinic.ExternalID)+","
				+"'"+POut.String(clinic.SchedNote)+"',"
				+    POut.Bool  (clinic.HasProcOnRx)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				clinic.ClinicNum=Db.NonQ(command,true,"ClinicNum","clinic");
			}
			return clinic.ClinicNum;
		}

		///<summary>Inserts one Clinic into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Clinic clinic) {
			return InsertNoCache(clinic,false);
		}

		///<summary>Inserts one Clinic into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Clinic clinic,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO clinic (";
			if(!useExistingPK && isRandomKeys) {
				clinic.ClinicNum=ReplicationServers.GetKeyNoCache("clinic","ClinicNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ClinicNum,";
			}
			command+="Description,Address,Address2,City,State,Zip,BillingAddress,BillingAddress2,BillingCity,BillingState,BillingZip,PayToAddress,PayToAddress2,PayToCity,PayToState,PayToZip,Phone,BankNumber,DefaultPlaceService,InsBillingProv,Fax,EmailAddressNum,DefaultProv,SmsContractDate,SmsMonthlyLimit,IsMedicalOnly,UseBillAddrOnClaims,Region,ItemOrder,IsInsVerifyExcluded,Abbr,MedLabAccountNum,IsConfirmEnabled,IsConfirmDefault,IsNewPatApptExcluded,IsHidden,ExternalID,SchedNote,HasProcOnRx) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(clinic.ClinicNum)+",";
			}
			command+=
				 "'"+POut.String(clinic.Description)+"',"
				+"'"+POut.String(clinic.Address)+"',"
				+"'"+POut.String(clinic.Address2)+"',"
				+"'"+POut.String(clinic.City)+"',"
				+"'"+POut.String(clinic.State)+"',"
				+"'"+POut.String(clinic.Zip)+"',"
				+"'"+POut.String(clinic.BillingAddress)+"',"
				+"'"+POut.String(clinic.BillingAddress2)+"',"
				+"'"+POut.String(clinic.BillingCity)+"',"
				+"'"+POut.String(clinic.BillingState)+"',"
				+"'"+POut.String(clinic.BillingZip)+"',"
				+"'"+POut.String(clinic.PayToAddress)+"',"
				+"'"+POut.String(clinic.PayToAddress2)+"',"
				+"'"+POut.String(clinic.PayToCity)+"',"
				+"'"+POut.String(clinic.PayToState)+"',"
				+"'"+POut.String(clinic.PayToZip)+"',"
				+"'"+POut.String(clinic.Phone)+"',"
				+"'"+POut.String(clinic.BankNumber)+"',"
				+    POut.Int   ((int)clinic.DefaultPlaceService)+","
				+    POut.Long  (clinic.InsBillingProv)+","
				+"'"+POut.String(clinic.Fax)+"',"
				+    POut.Long  (clinic.EmailAddressNum)+","
				+    POut.Long  (clinic.DefaultProv)+","
				+    POut.DateT (clinic.SmsContractDate)+","
				+"'"+POut.Double(clinic.SmsMonthlyLimit)+"',"
				+    POut.Bool  (clinic.IsMedicalOnly)+","
				+    POut.Bool  (clinic.UseBillAddrOnClaims)+","
				+    POut.Long  (clinic.Region)+","
				+    POut.Int   (clinic.ItemOrder)+","
				+    POut.Bool  (clinic.IsInsVerifyExcluded)+","
				+"'"+POut.String(clinic.Abbr)+"',"
				+"'"+POut.String(clinic.MedLabAccountNum)+"',"
				+    POut.Bool  (clinic.IsConfirmEnabled)+","
				+    POut.Bool  (clinic.IsConfirmDefault)+","
				+    POut.Bool  (clinic.IsNewPatApptExcluded)+","
				+    POut.Bool  (clinic.IsHidden)+","
				+    POut.Long  (clinic.ExternalID)+","
				+"'"+POut.String(clinic.SchedNote)+"',"
				+    POut.Bool  (clinic.HasProcOnRx)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				clinic.ClinicNum=Db.NonQ(command,true,"ClinicNum","clinic");
			}
			return clinic.ClinicNum;
		}

		///<summary>Updates one Clinic in the database.</summary>
		public static void Update(Clinic clinic) {
			string command="UPDATE clinic SET "
				+"Description         = '"+POut.String(clinic.Description)+"', "
				+"Address             = '"+POut.String(clinic.Address)+"', "
				+"Address2            = '"+POut.String(clinic.Address2)+"', "
				+"City                = '"+POut.String(clinic.City)+"', "
				+"State               = '"+POut.String(clinic.State)+"', "
				+"Zip                 = '"+POut.String(clinic.Zip)+"', "
				+"BillingAddress      = '"+POut.String(clinic.BillingAddress)+"', "
				+"BillingAddress2     = '"+POut.String(clinic.BillingAddress2)+"', "
				+"BillingCity         = '"+POut.String(clinic.BillingCity)+"', "
				+"BillingState        = '"+POut.String(clinic.BillingState)+"', "
				+"BillingZip          = '"+POut.String(clinic.BillingZip)+"', "
				+"PayToAddress        = '"+POut.String(clinic.PayToAddress)+"', "
				+"PayToAddress2       = '"+POut.String(clinic.PayToAddress2)+"', "
				+"PayToCity           = '"+POut.String(clinic.PayToCity)+"', "
				+"PayToState          = '"+POut.String(clinic.PayToState)+"', "
				+"PayToZip            = '"+POut.String(clinic.PayToZip)+"', "
				+"Phone               = '"+POut.String(clinic.Phone)+"', "
				+"BankNumber          = '"+POut.String(clinic.BankNumber)+"', "
				+"DefaultPlaceService =  "+POut.Int   ((int)clinic.DefaultPlaceService)+", "
				+"InsBillingProv      =  "+POut.Long  (clinic.InsBillingProv)+", "
				+"Fax                 = '"+POut.String(clinic.Fax)+"', "
				+"EmailAddressNum     =  "+POut.Long  (clinic.EmailAddressNum)+", "
				+"DefaultProv         =  "+POut.Long  (clinic.DefaultProv)+", "
				+"SmsContractDate     =  "+POut.DateT (clinic.SmsContractDate)+", "
				+"SmsMonthlyLimit     = '"+POut.Double(clinic.SmsMonthlyLimit)+"', "
				+"IsMedicalOnly       =  "+POut.Bool  (clinic.IsMedicalOnly)+", "
				+"UseBillAddrOnClaims =  "+POut.Bool  (clinic.UseBillAddrOnClaims)+", "
				+"Region              =  "+POut.Long  (clinic.Region)+", "
				+"ItemOrder           =  "+POut.Int   (clinic.ItemOrder)+", "
				+"IsInsVerifyExcluded =  "+POut.Bool  (clinic.IsInsVerifyExcluded)+", "
				+"Abbr                = '"+POut.String(clinic.Abbr)+"', "
				+"MedLabAccountNum    = '"+POut.String(clinic.MedLabAccountNum)+"', "
				+"IsConfirmEnabled    =  "+POut.Bool  (clinic.IsConfirmEnabled)+", "
				+"IsConfirmDefault    =  "+POut.Bool  (clinic.IsConfirmDefault)+", "
				+"IsNewPatApptExcluded=  "+POut.Bool  (clinic.IsNewPatApptExcluded)+", "
				+"IsHidden            =  "+POut.Bool  (clinic.IsHidden)+", "
				+"ExternalID          =  "+POut.Long  (clinic.ExternalID)+", "
				+"SchedNote           = '"+POut.String(clinic.SchedNote)+"', "
				+"HasProcOnRx         =  "+POut.Bool  (clinic.HasProcOnRx)+" "
				+"WHERE ClinicNum = "+POut.Long(clinic.ClinicNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Clinic in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Clinic clinic,Clinic oldClinic) {
			string command="";
			if(clinic.Description != oldClinic.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(clinic.Description)+"'";
			}
			if(clinic.Address != oldClinic.Address) {
				if(command!="") { command+=",";}
				command+="Address = '"+POut.String(clinic.Address)+"'";
			}
			if(clinic.Address2 != oldClinic.Address2) {
				if(command!="") { command+=",";}
				command+="Address2 = '"+POut.String(clinic.Address2)+"'";
			}
			if(clinic.City != oldClinic.City) {
				if(command!="") { command+=",";}
				command+="City = '"+POut.String(clinic.City)+"'";
			}
			if(clinic.State != oldClinic.State) {
				if(command!="") { command+=",";}
				command+="State = '"+POut.String(clinic.State)+"'";
			}
			if(clinic.Zip != oldClinic.Zip) {
				if(command!="") { command+=",";}
				command+="Zip = '"+POut.String(clinic.Zip)+"'";
			}
			if(clinic.BillingAddress != oldClinic.BillingAddress) {
				if(command!="") { command+=",";}
				command+="BillingAddress = '"+POut.String(clinic.BillingAddress)+"'";
			}
			if(clinic.BillingAddress2 != oldClinic.BillingAddress2) {
				if(command!="") { command+=",";}
				command+="BillingAddress2 = '"+POut.String(clinic.BillingAddress2)+"'";
			}
			if(clinic.BillingCity != oldClinic.BillingCity) {
				if(command!="") { command+=",";}
				command+="BillingCity = '"+POut.String(clinic.BillingCity)+"'";
			}
			if(clinic.BillingState != oldClinic.BillingState) {
				if(command!="") { command+=",";}
				command+="BillingState = '"+POut.String(clinic.BillingState)+"'";
			}
			if(clinic.BillingZip != oldClinic.BillingZip) {
				if(command!="") { command+=",";}
				command+="BillingZip = '"+POut.String(clinic.BillingZip)+"'";
			}
			if(clinic.PayToAddress != oldClinic.PayToAddress) {
				if(command!="") { command+=",";}
				command+="PayToAddress = '"+POut.String(clinic.PayToAddress)+"'";
			}
			if(clinic.PayToAddress2 != oldClinic.PayToAddress2) {
				if(command!="") { command+=",";}
				command+="PayToAddress2 = '"+POut.String(clinic.PayToAddress2)+"'";
			}
			if(clinic.PayToCity != oldClinic.PayToCity) {
				if(command!="") { command+=",";}
				command+="PayToCity = '"+POut.String(clinic.PayToCity)+"'";
			}
			if(clinic.PayToState != oldClinic.PayToState) {
				if(command!="") { command+=",";}
				command+="PayToState = '"+POut.String(clinic.PayToState)+"'";
			}
			if(clinic.PayToZip != oldClinic.PayToZip) {
				if(command!="") { command+=",";}
				command+="PayToZip = '"+POut.String(clinic.PayToZip)+"'";
			}
			if(clinic.Phone != oldClinic.Phone) {
				if(command!="") { command+=",";}
				command+="Phone = '"+POut.String(clinic.Phone)+"'";
			}
			if(clinic.BankNumber != oldClinic.BankNumber) {
				if(command!="") { command+=",";}
				command+="BankNumber = '"+POut.String(clinic.BankNumber)+"'";
			}
			if(clinic.DefaultPlaceService != oldClinic.DefaultPlaceService) {
				if(command!="") { command+=",";}
				command+="DefaultPlaceService = "+POut.Int   ((int)clinic.DefaultPlaceService)+"";
			}
			if(clinic.InsBillingProv != oldClinic.InsBillingProv) {
				if(command!="") { command+=",";}
				command+="InsBillingProv = "+POut.Long(clinic.InsBillingProv)+"";
			}
			if(clinic.Fax != oldClinic.Fax) {
				if(command!="") { command+=",";}
				command+="Fax = '"+POut.String(clinic.Fax)+"'";
			}
			if(clinic.EmailAddressNum != oldClinic.EmailAddressNum) {
				if(command!="") { command+=",";}
				command+="EmailAddressNum = "+POut.Long(clinic.EmailAddressNum)+"";
			}
			if(clinic.DefaultProv != oldClinic.DefaultProv) {
				if(command!="") { command+=",";}
				command+="DefaultProv = "+POut.Long(clinic.DefaultProv)+"";
			}
			if(clinic.SmsContractDate != oldClinic.SmsContractDate) {
				if(command!="") { command+=",";}
				command+="SmsContractDate = "+POut.DateT(clinic.SmsContractDate)+"";
			}
			if(clinic.SmsMonthlyLimit != oldClinic.SmsMonthlyLimit) {
				if(command!="") { command+=",";}
				command+="SmsMonthlyLimit = '"+POut.Double(clinic.SmsMonthlyLimit)+"'";
			}
			if(clinic.IsMedicalOnly != oldClinic.IsMedicalOnly) {
				if(command!="") { command+=",";}
				command+="IsMedicalOnly = "+POut.Bool(clinic.IsMedicalOnly)+"";
			}
			if(clinic.UseBillAddrOnClaims != oldClinic.UseBillAddrOnClaims) {
				if(command!="") { command+=",";}
				command+="UseBillAddrOnClaims = "+POut.Bool(clinic.UseBillAddrOnClaims)+"";
			}
			if(clinic.Region != oldClinic.Region) {
				if(command!="") { command+=",";}
				command+="Region = "+POut.Long(clinic.Region)+"";
			}
			if(clinic.ItemOrder != oldClinic.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(clinic.ItemOrder)+"";
			}
			if(clinic.IsInsVerifyExcluded != oldClinic.IsInsVerifyExcluded) {
				if(command!="") { command+=",";}
				command+="IsInsVerifyExcluded = "+POut.Bool(clinic.IsInsVerifyExcluded)+"";
			}
			if(clinic.Abbr != oldClinic.Abbr) {
				if(command!="") { command+=",";}
				command+="Abbr = '"+POut.String(clinic.Abbr)+"'";
			}
			if(clinic.MedLabAccountNum != oldClinic.MedLabAccountNum) {
				if(command!="") { command+=",";}
				command+="MedLabAccountNum = '"+POut.String(clinic.MedLabAccountNum)+"'";
			}
			if(clinic.IsConfirmEnabled != oldClinic.IsConfirmEnabled) {
				if(command!="") { command+=",";}
				command+="IsConfirmEnabled = "+POut.Bool(clinic.IsConfirmEnabled)+"";
			}
			if(clinic.IsConfirmDefault != oldClinic.IsConfirmDefault) {
				if(command!="") { command+=",";}
				command+="IsConfirmDefault = "+POut.Bool(clinic.IsConfirmDefault)+"";
			}
			if(clinic.IsNewPatApptExcluded != oldClinic.IsNewPatApptExcluded) {
				if(command!="") { command+=",";}
				command+="IsNewPatApptExcluded = "+POut.Bool(clinic.IsNewPatApptExcluded)+"";
			}
			if(clinic.IsHidden != oldClinic.IsHidden) {
				if(command!="") { command+=",";}
				command+="IsHidden = "+POut.Bool(clinic.IsHidden)+"";
			}
			if(clinic.ExternalID != oldClinic.ExternalID) {
				if(command!="") { command+=",";}
				command+="ExternalID = "+POut.Long(clinic.ExternalID)+"";
			}
			if(clinic.SchedNote != oldClinic.SchedNote) {
				if(command!="") { command+=",";}
				command+="SchedNote = '"+POut.String(clinic.SchedNote)+"'";
			}
			if(clinic.HasProcOnRx != oldClinic.HasProcOnRx) {
				if(command!="") { command+=",";}
				command+="HasProcOnRx = "+POut.Bool(clinic.HasProcOnRx)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE clinic SET "+command
				+" WHERE ClinicNum = "+POut.Long(clinic.ClinicNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Clinic,Clinic) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Clinic clinic,Clinic oldClinic) {
			if(clinic.Description != oldClinic.Description) {
				return true;
			}
			if(clinic.Address != oldClinic.Address) {
				return true;
			}
			if(clinic.Address2 != oldClinic.Address2) {
				return true;
			}
			if(clinic.City != oldClinic.City) {
				return true;
			}
			if(clinic.State != oldClinic.State) {
				return true;
			}
			if(clinic.Zip != oldClinic.Zip) {
				return true;
			}
			if(clinic.BillingAddress != oldClinic.BillingAddress) {
				return true;
			}
			if(clinic.BillingAddress2 != oldClinic.BillingAddress2) {
				return true;
			}
			if(clinic.BillingCity != oldClinic.BillingCity) {
				return true;
			}
			if(clinic.BillingState != oldClinic.BillingState) {
				return true;
			}
			if(clinic.BillingZip != oldClinic.BillingZip) {
				return true;
			}
			if(clinic.PayToAddress != oldClinic.PayToAddress) {
				return true;
			}
			if(clinic.PayToAddress2 != oldClinic.PayToAddress2) {
				return true;
			}
			if(clinic.PayToCity != oldClinic.PayToCity) {
				return true;
			}
			if(clinic.PayToState != oldClinic.PayToState) {
				return true;
			}
			if(clinic.PayToZip != oldClinic.PayToZip) {
				return true;
			}
			if(clinic.Phone != oldClinic.Phone) {
				return true;
			}
			if(clinic.BankNumber != oldClinic.BankNumber) {
				return true;
			}
			if(clinic.DefaultPlaceService != oldClinic.DefaultPlaceService) {
				return true;
			}
			if(clinic.InsBillingProv != oldClinic.InsBillingProv) {
				return true;
			}
			if(clinic.Fax != oldClinic.Fax) {
				return true;
			}
			if(clinic.EmailAddressNum != oldClinic.EmailAddressNum) {
				return true;
			}
			if(clinic.DefaultProv != oldClinic.DefaultProv) {
				return true;
			}
			if(clinic.SmsContractDate != oldClinic.SmsContractDate) {
				return true;
			}
			if(clinic.SmsMonthlyLimit != oldClinic.SmsMonthlyLimit) {
				return true;
			}
			if(clinic.IsMedicalOnly != oldClinic.IsMedicalOnly) {
				return true;
			}
			if(clinic.UseBillAddrOnClaims != oldClinic.UseBillAddrOnClaims) {
				return true;
			}
			if(clinic.Region != oldClinic.Region) {
				return true;
			}
			if(clinic.ItemOrder != oldClinic.ItemOrder) {
				return true;
			}
			if(clinic.IsInsVerifyExcluded != oldClinic.IsInsVerifyExcluded) {
				return true;
			}
			if(clinic.Abbr != oldClinic.Abbr) {
				return true;
			}
			if(clinic.MedLabAccountNum != oldClinic.MedLabAccountNum) {
				return true;
			}
			if(clinic.IsConfirmEnabled != oldClinic.IsConfirmEnabled) {
				return true;
			}
			if(clinic.IsConfirmDefault != oldClinic.IsConfirmDefault) {
				return true;
			}
			if(clinic.IsNewPatApptExcluded != oldClinic.IsNewPatApptExcluded) {
				return true;
			}
			if(clinic.IsHidden != oldClinic.IsHidden) {
				return true;
			}
			if(clinic.ExternalID != oldClinic.ExternalID) {
				return true;
			}
			if(clinic.SchedNote != oldClinic.SchedNote) {
				return true;
			}
			if(clinic.HasProcOnRx != oldClinic.HasProcOnRx) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Clinic from the database.</summary>
		public static void Delete(long clinicNum) {
			string command="DELETE FROM clinic "
				+"WHERE ClinicNum = "+POut.Long(clinicNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<Clinic> listNew,List<Clinic> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Clinic> listIns    =new List<Clinic>();
			List<Clinic> listUpdNew =new List<Clinic>();
			List<Clinic> listUpdDB  =new List<Clinic>();
			List<Clinic> listDel    =new List<Clinic>();
			listNew.Sort((Clinic x,Clinic y) => { return x.ClinicNum.CompareTo(y.ClinicNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((Clinic x,Clinic y) => { return x.ClinicNum.CompareTo(y.ClinicNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			Clinic fieldNew;
			Clinic fieldDB;
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
				else if(fieldNew.ClinicNum<fieldDB.ClinicNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ClinicNum>fieldDB.ClinicNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ClinicNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}