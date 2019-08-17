//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CarrierCrud {
		///<summary>Gets one Carrier object from the database using the primary key.  Returns null if not found.</summary>
		public static Carrier SelectOne(long carrierNum) {
			string command="SELECT * FROM carrier "
				+"WHERE CarrierNum = "+POut.Long(carrierNum);
			List<Carrier> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Carrier object from the database using a query.</summary>
		public static Carrier SelectOne(string command) {
			
			List<Carrier> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Carrier objects from the database using a query.</summary>
		public static List<Carrier> SelectMany(string command) {
			
			List<Carrier> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Carrier> TableToList(DataTable table) {
			List<Carrier> retVal=new List<Carrier>();
			Carrier carrier;
			foreach(DataRow row in table.Rows) {
				carrier=new Carrier();
				carrier.CarrierNum              = PIn.Long  (row["CarrierNum"].ToString());
				carrier.CarrierName             = PIn.String(row["CarrierName"].ToString());
				carrier.Address                 = PIn.String(row["Address"].ToString());
				carrier.Address2                = PIn.String(row["Address2"].ToString());
				carrier.City                    = PIn.String(row["City"].ToString());
				carrier.State                   = PIn.String(row["State"].ToString());
				carrier.Zip                     = PIn.String(row["Zip"].ToString());
				carrier.Phone                   = PIn.String(row["Phone"].ToString());
				carrier.ElectID                 = PIn.String(row["ElectID"].ToString());
				carrier.NoSendElect             = (OpenDentBusiness.NoSendElectType)PIn.Int(row["NoSendElect"].ToString());
				carrier.IsCDA                   = PIn.Bool  (row["IsCDA"].ToString());
				carrier.CDAnetVersion           = PIn.String(row["CDAnetVersion"].ToString());
				carrier.CanadianNetworkNum      = PIn.Long  (row["CanadianNetworkNum"].ToString());
				carrier.IsHidden                = PIn.Bool  (row["IsHidden"].ToString());
				carrier.CanadianEncryptionMethod= PIn.Byte  (row["CanadianEncryptionMethod"].ToString());
				carrier.CanadianSupportedTypes  = (OpenDentBusiness.CanSupTransTypes)PIn.Int(row["CanadianSupportedTypes"].ToString());
				carrier.SecUserNumEntry         = PIn.Long  (row["SecUserNumEntry"].ToString());
				carrier.SecDateEntry            = PIn.Date  (row["SecDateEntry"].ToString());
				carrier.SecDateTEdit            = PIn.DateT (row["SecDateTEdit"].ToString());
				carrier.TIN                     = PIn.String(row["TIN"].ToString());
				carrier.CarrierGroupName        = PIn.Long  (row["CarrierGroupName"].ToString());
				carrier.ApptTextBackColor       = Color.FromArgb(PIn.Int(row["ApptTextBackColor"].ToString()));
				carrier.IsCoinsuranceInverted   = PIn.Bool  (row["IsCoinsuranceInverted"].ToString());
				retVal.Add(carrier);
			}
			return retVal;
		}

		///<summary>Converts a list of Carrier into a DataTable.</summary>
		public static DataTable ListToTable(List<Carrier> listCarriers,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Carrier";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CarrierNum");
			table.Columns.Add("CarrierName");
			table.Columns.Add("Address");
			table.Columns.Add("Address2");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("Phone");
			table.Columns.Add("ElectID");
			table.Columns.Add("NoSendElect");
			table.Columns.Add("IsCDA");
			table.Columns.Add("CDAnetVersion");
			table.Columns.Add("CanadianNetworkNum");
			table.Columns.Add("IsHidden");
			table.Columns.Add("CanadianEncryptionMethod");
			table.Columns.Add("CanadianSupportedTypes");
			table.Columns.Add("SecUserNumEntry");
			table.Columns.Add("SecDateEntry");
			table.Columns.Add("SecDateTEdit");
			table.Columns.Add("TIN");
			table.Columns.Add("CarrierGroupName");
			table.Columns.Add("ApptTextBackColor");
			table.Columns.Add("IsCoinsuranceInverted");
			foreach(Carrier carrier in listCarriers) {
				table.Rows.Add(new object[] {
					POut.Long  (carrier.CarrierNum),
					            carrier.CarrierName,
					            carrier.Address,
					            carrier.Address2,
					            carrier.City,
					            carrier.State,
					            carrier.Zip,
					            carrier.Phone,
					            carrier.ElectID,
					POut.Int   ((int)carrier.NoSendElect),
					POut.Bool  (carrier.IsCDA),
					            carrier.CDAnetVersion,
					POut.Long  (carrier.CanadianNetworkNum),
					POut.Bool  (carrier.IsHidden),
					POut.Byte  (carrier.CanadianEncryptionMethod),
					POut.Int   ((int)carrier.CanadianSupportedTypes),
					POut.Long  (carrier.SecUserNumEntry),
					POut.DateT (carrier.SecDateEntry,false),
					POut.DateT (carrier.SecDateTEdit,false),
					            carrier.TIN,
					POut.Long  (carrier.CarrierGroupName),
					POut.Int   (carrier.ApptTextBackColor.ToArgb()),
					POut.Bool  (carrier.IsCoinsuranceInverted),
				});
			}
			return table;
		}

		///<summary>Inserts one Carrier into the database.  Returns the new priKey.</summary>
		public static long Insert(Carrier carrier) {
			return Insert(carrier,false);
		}

		///<summary>Inserts one Carrier into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Carrier carrier,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				carrier.CarrierNum=ReplicationServers.GetKey("carrier","CarrierNum");
			}
			string command="INSERT INTO carrier (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="CarrierNum,";
			}
			command+="CarrierName,Address,Address2,City,State,Zip,Phone,ElectID,NoSendElect,IsCDA,CDAnetVersion,CanadianNetworkNum,IsHidden,CanadianEncryptionMethod,CanadianSupportedTypes,SecUserNumEntry,SecDateEntry,TIN,CarrierGroupName,ApptTextBackColor,IsCoinsuranceInverted) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(carrier.CarrierNum)+",";
			}
			command+=
				 "'"+POut.String(carrier.CarrierName)+"',"
				+"'"+POut.String(carrier.Address)+"',"
				+"'"+POut.String(carrier.Address2)+"',"
				+"'"+POut.String(carrier.City)+"',"
				+"'"+POut.String(carrier.State)+"',"
				+"'"+POut.String(carrier.Zip)+"',"
				+"'"+POut.String(carrier.Phone)+"',"
				+"'"+POut.String(carrier.ElectID)+"',"
				+    POut.Int   ((int)carrier.NoSendElect)+","
				+    POut.Bool  (carrier.IsCDA)+","
				+"'"+POut.String(carrier.CDAnetVersion)+"',"
				+    POut.Long  (carrier.CanadianNetworkNum)+","
				+    POut.Bool  (carrier.IsHidden)+","
				+    POut.Byte  (carrier.CanadianEncryptionMethod)+","
				+    POut.Int   ((int)carrier.CanadianSupportedTypes)+","
				+    POut.Long  (carrier.SecUserNumEntry)+","
				+    DbHelper.Now()+","
				//SecDateTEdit can only be set by MySQL
				+"'"+POut.String(carrier.TIN)+"',"
				+    POut.Long  (carrier.CarrierGroupName)+","
				+    POut.Int   (carrier.ApptTextBackColor.ToArgb())+","
				+    POut.Bool  (carrier.IsCoinsuranceInverted)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				carrier.CarrierNum=Db.NonQ(command,true,"CarrierNum","carrier");
			}
			return carrier.CarrierNum;
		}

		///<summary>Inserts one Carrier into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Carrier carrier) {
			return InsertNoCache(carrier,false);
		}

		///<summary>Inserts one Carrier into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Carrier carrier,bool useExistingPK) {
			bool isRandomKeys=Preference.GetBoolNoCache(PreferenceName.RandomPrimaryKeys);
			string command="INSERT INTO carrier (";
			if(!useExistingPK && isRandomKeys) {
				carrier.CarrierNum=ReplicationServers.GetKeyNoCache("carrier","CarrierNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CarrierNum,";
			}
			command+="CarrierName,Address,Address2,City,State,Zip,Phone,ElectID,NoSendElect,IsCDA,CDAnetVersion,CanadianNetworkNum,IsHidden,CanadianEncryptionMethod,CanadianSupportedTypes,SecUserNumEntry,SecDateEntry,TIN,CarrierGroupName,ApptTextBackColor,IsCoinsuranceInverted) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(carrier.CarrierNum)+",";
			}
			command+=
				 "'"+POut.String(carrier.CarrierName)+"',"
				+"'"+POut.String(carrier.Address)+"',"
				+"'"+POut.String(carrier.Address2)+"',"
				+"'"+POut.String(carrier.City)+"',"
				+"'"+POut.String(carrier.State)+"',"
				+"'"+POut.String(carrier.Zip)+"',"
				+"'"+POut.String(carrier.Phone)+"',"
				+"'"+POut.String(carrier.ElectID)+"',"
				+    POut.Int   ((int)carrier.NoSendElect)+","
				+    POut.Bool  (carrier.IsCDA)+","
				+"'"+POut.String(carrier.CDAnetVersion)+"',"
				+    POut.Long  (carrier.CanadianNetworkNum)+","
				+    POut.Bool  (carrier.IsHidden)+","
				+    POut.Byte  (carrier.CanadianEncryptionMethod)+","
				+    POut.Int   ((int)carrier.CanadianSupportedTypes)+","
				+    POut.Long  (carrier.SecUserNumEntry)+","
				+    DbHelper.Now()+","
				//SecDateTEdit can only be set by MySQL
				+"'"+POut.String(carrier.TIN)+"',"
				+    POut.Long  (carrier.CarrierGroupName)+","
				+    POut.Int   (carrier.ApptTextBackColor.ToArgb())+","
				+    POut.Bool  (carrier.IsCoinsuranceInverted)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				carrier.CarrierNum=Db.NonQ(command,true,"CarrierNum","carrier");
			}
			return carrier.CarrierNum;
		}

		///<summary>Updates one Carrier in the database.</summary>
		public static void Update(Carrier carrier) {
			string command="UPDATE carrier SET "
				+"CarrierName             = '"+POut.String(carrier.CarrierName)+"', "
				+"Address                 = '"+POut.String(carrier.Address)+"', "
				+"Address2                = '"+POut.String(carrier.Address2)+"', "
				+"City                    = '"+POut.String(carrier.City)+"', "
				+"State                   = '"+POut.String(carrier.State)+"', "
				+"Zip                     = '"+POut.String(carrier.Zip)+"', "
				+"Phone                   = '"+POut.String(carrier.Phone)+"', "
				+"ElectID                 = '"+POut.String(carrier.ElectID)+"', "
				+"NoSendElect             =  "+POut.Int   ((int)carrier.NoSendElect)+", "
				+"IsCDA                   =  "+POut.Bool  (carrier.IsCDA)+", "
				+"CDAnetVersion           = '"+POut.String(carrier.CDAnetVersion)+"', "
				+"CanadianNetworkNum      =  "+POut.Long  (carrier.CanadianNetworkNum)+", "
				+"IsHidden                =  "+POut.Bool  (carrier.IsHidden)+", "
				+"CanadianEncryptionMethod=  "+POut.Byte  (carrier.CanadianEncryptionMethod)+", "
				+"CanadianSupportedTypes  =  "+POut.Int   ((int)carrier.CanadianSupportedTypes)+", "
				//SecUserNumEntry excluded from update
				//SecDateEntry not allowed to change
				//SecDateTEdit can only be set by MySQL
				+"TIN                     = '"+POut.String(carrier.TIN)+"', "
				+"CarrierGroupName        =  "+POut.Long  (carrier.CarrierGroupName)+", "
				+"ApptTextBackColor       =  "+POut.Int   (carrier.ApptTextBackColor.ToArgb())+", "
				+"IsCoinsuranceInverted   =  "+POut.Bool  (carrier.IsCoinsuranceInverted)+" "
				+"WHERE CarrierNum = "+POut.Long(carrier.CarrierNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Carrier in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Carrier carrier,Carrier oldCarrier) {
			string command="";
			if(carrier.CarrierName != oldCarrier.CarrierName) {
				if(command!="") { command+=",";}
				command+="CarrierName = '"+POut.String(carrier.CarrierName)+"'";
			}
			if(carrier.Address != oldCarrier.Address) {
				if(command!="") { command+=",";}
				command+="Address = '"+POut.String(carrier.Address)+"'";
			}
			if(carrier.Address2 != oldCarrier.Address2) {
				if(command!="") { command+=",";}
				command+="Address2 = '"+POut.String(carrier.Address2)+"'";
			}
			if(carrier.City != oldCarrier.City) {
				if(command!="") { command+=",";}
				command+="City = '"+POut.String(carrier.City)+"'";
			}
			if(carrier.State != oldCarrier.State) {
				if(command!="") { command+=",";}
				command+="State = '"+POut.String(carrier.State)+"'";
			}
			if(carrier.Zip != oldCarrier.Zip) {
				if(command!="") { command+=",";}
				command+="Zip = '"+POut.String(carrier.Zip)+"'";
			}
			if(carrier.Phone != oldCarrier.Phone) {
				if(command!="") { command+=",";}
				command+="Phone = '"+POut.String(carrier.Phone)+"'";
			}
			if(carrier.ElectID != oldCarrier.ElectID) {
				if(command!="") { command+=",";}
				command+="ElectID = '"+POut.String(carrier.ElectID)+"'";
			}
			if(carrier.NoSendElect != oldCarrier.NoSendElect) {
				if(command!="") { command+=",";}
				command+="NoSendElect = "+POut.Int   ((int)carrier.NoSendElect)+"";
			}
			if(carrier.IsCDA != oldCarrier.IsCDA) {
				if(command!="") { command+=",";}
				command+="IsCDA = "+POut.Bool(carrier.IsCDA)+"";
			}
			if(carrier.CDAnetVersion != oldCarrier.CDAnetVersion) {
				if(command!="") { command+=",";}
				command+="CDAnetVersion = '"+POut.String(carrier.CDAnetVersion)+"'";
			}
			if(carrier.CanadianNetworkNum != oldCarrier.CanadianNetworkNum) {
				if(command!="") { command+=",";}
				command+="CanadianNetworkNum = "+POut.Long(carrier.CanadianNetworkNum)+"";
			}
			if(carrier.IsHidden != oldCarrier.IsHidden) {
				if(command!="") { command+=",";}
				command+="IsHidden = "+POut.Bool(carrier.IsHidden)+"";
			}
			if(carrier.CanadianEncryptionMethod != oldCarrier.CanadianEncryptionMethod) {
				if(command!="") { command+=",";}
				command+="CanadianEncryptionMethod = "+POut.Byte(carrier.CanadianEncryptionMethod)+"";
			}
			if(carrier.CanadianSupportedTypes != oldCarrier.CanadianSupportedTypes) {
				if(command!="") { command+=",";}
				command+="CanadianSupportedTypes = "+POut.Int   ((int)carrier.CanadianSupportedTypes)+"";
			}
			//SecUserNumEntry excluded from update
			//SecDateEntry not allowed to change
			//SecDateTEdit can only be set by MySQL
			if(carrier.TIN != oldCarrier.TIN) {
				if(command!="") { command+=",";}
				command+="TIN = '"+POut.String(carrier.TIN)+"'";
			}
			if(carrier.CarrierGroupName != oldCarrier.CarrierGroupName) {
				if(command!="") { command+=",";}
				command+="CarrierGroupName = "+POut.Long(carrier.CarrierGroupName)+"";
			}
			if(carrier.ApptTextBackColor != oldCarrier.ApptTextBackColor) {
				if(command!="") { command+=",";}
				command+="ApptTextBackColor = "+POut.Int(carrier.ApptTextBackColor.ToArgb())+"";
			}
			if(carrier.IsCoinsuranceInverted != oldCarrier.IsCoinsuranceInverted) {
				if(command!="") { command+=",";}
				command+="IsCoinsuranceInverted = "+POut.Bool(carrier.IsCoinsuranceInverted)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE carrier SET "+command
				+" WHERE CarrierNum = "+POut.Long(carrier.CarrierNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Carrier,Carrier) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Carrier carrier,Carrier oldCarrier) {
			if(carrier.CarrierName != oldCarrier.CarrierName) {
				return true;
			}
			if(carrier.Address != oldCarrier.Address) {
				return true;
			}
			if(carrier.Address2 != oldCarrier.Address2) {
				return true;
			}
			if(carrier.City != oldCarrier.City) {
				return true;
			}
			if(carrier.State != oldCarrier.State) {
				return true;
			}
			if(carrier.Zip != oldCarrier.Zip) {
				return true;
			}
			if(carrier.Phone != oldCarrier.Phone) {
				return true;
			}
			if(carrier.ElectID != oldCarrier.ElectID) {
				return true;
			}
			if(carrier.NoSendElect != oldCarrier.NoSendElect) {
				return true;
			}
			if(carrier.IsCDA != oldCarrier.IsCDA) {
				return true;
			}
			if(carrier.CDAnetVersion != oldCarrier.CDAnetVersion) {
				return true;
			}
			if(carrier.CanadianNetworkNum != oldCarrier.CanadianNetworkNum) {
				return true;
			}
			if(carrier.IsHidden != oldCarrier.IsHidden) {
				return true;
			}
			if(carrier.CanadianEncryptionMethod != oldCarrier.CanadianEncryptionMethod) {
				return true;
			}
			if(carrier.CanadianSupportedTypes != oldCarrier.CanadianSupportedTypes) {
				return true;
			}
			//SecUserNumEntry excluded from update
			//SecDateEntry not allowed to change
			//SecDateTEdit can only be set by MySQL
			if(carrier.TIN != oldCarrier.TIN) {
				return true;
			}
			if(carrier.CarrierGroupName != oldCarrier.CarrierGroupName) {
				return true;
			}
			if(carrier.ApptTextBackColor != oldCarrier.ApptTextBackColor) {
				return true;
			}
			if(carrier.IsCoinsuranceInverted != oldCarrier.IsCoinsuranceInverted) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Carrier from the database.</summary>
		public static void Delete(long carrierNum) {
			string command="DELETE FROM carrier "
				+"WHERE CarrierNum = "+POut.Long(carrierNum);
			Db.NonQ(command);
		}

	}
}