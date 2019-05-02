//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class UserodCrud {





		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<User> TableToList(DataTable table) {
			List<User> retVal=new List<User>();
			User userod;
			foreach(DataRow row in table.Rows) {
				userod=new User();
				userod.UserNum                = PIn.Long  (row["UserNum"].ToString());
				userod.UserName               = PIn.String(row["UserName"].ToString());
				userod.Password               = PIn.String(row["Password"].ToString());
				userod.UserGroupNum           = PIn.Long  (row["UserGroupNum"].ToString());
				userod.EmployeeNum            = PIn.Long  (row["EmployeeNum"].ToString());
				userod.ClinicNum              = PIn.Long  (row["ClinicNum"].ToString());
				userod.ProvNum                = PIn.Long  (row["ProvNum"].ToString());
				userod.IsHidden               = PIn.Bool  (row["IsHidden"].ToString());
				userod.TaskListInBox          = PIn.Long  (row["TaskListInBox"].ToString());
				userod.AnesthProvType         = PIn.Int   (row["AnesthProvType"].ToString());
				userod.DefaultHidePopups      = PIn.Bool  (row["DefaultHidePopups"].ToString());
				userod.PasswordIsStrong       = PIn.Bool  (row["PasswordIsStrong"].ToString());
				userod.ClinicIsRestricted     = PIn.Bool  (row["ClinicIsRestricted"].ToString());
				userod.InboxHidePopups        = PIn.Bool  (row["InboxHidePopups"].ToString());
				userod.UserNumCEMT            = PIn.Long  (row["UserNumCEMT"].ToString());
				userod.DateTFail              = PIn.DateT (row["DateTFail"].ToString());
				userod.FailedAttempts         = PIn.Byte  (row["FailedAttempts"].ToString());
				userod.DomainUser             = PIn.String(row["DomainUser"].ToString());
				userod.IsPasswordResetRequired= PIn.Bool  (row["IsPasswordResetRequired"].ToString());
				retVal.Add(userod);
			}
			return retVal;
		}

		///<summary>Converts a list of Userod into a DataTable.</summary>
		public static DataTable ListToTable(List<User> listUserods,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Userod";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("UserNum");
			table.Columns.Add("UserName");
			table.Columns.Add("Password");
			table.Columns.Add("UserGroupNum");
			table.Columns.Add("EmployeeNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("IsHidden");
			table.Columns.Add("TaskListInBox");
			table.Columns.Add("AnesthProvType");
			table.Columns.Add("DefaultHidePopups");
			table.Columns.Add("PasswordIsStrong");
			table.Columns.Add("ClinicIsRestricted");
			table.Columns.Add("InboxHidePopups");
			table.Columns.Add("UserNumCEMT");
			table.Columns.Add("DateTFail");
			table.Columns.Add("FailedAttempts");
			table.Columns.Add("DomainUser");
			table.Columns.Add("IsPasswordResetRequired");
			foreach(User userod in listUserods) {
				table.Rows.Add(new object[] {
					POut.Long  (userod.UserNum),
					            userod.UserName,
					            userod.Password,
					POut.Long  (userod.UserGroupNum),
					POut.Long  (userod.EmployeeNum),
					POut.Long  (userod.ClinicNum),
					POut.Long  (userod.ProvNum),
					POut.Bool  (userod.IsHidden),
					POut.Long  (userod.TaskListInBox),
					POut.Int   (userod.AnesthProvType),
					POut.Bool  (userod.DefaultHidePopups),
					POut.Bool  (userod.PasswordIsStrong),
					POut.Bool  (userod.ClinicIsRestricted),
					POut.Bool  (userod.InboxHidePopups),
					POut.Long  (userod.UserNumCEMT),
					POut.DateT (userod.DateTFail,false),
					POut.Byte  (userod.FailedAttempts),
					            userod.DomainUser,
					POut.Bool  (userod.IsPasswordResetRequired),
				});
			}
			return table;
		}

		///<summary>Inserts one Userod into the database.  Returns the new priKey.</summary>
		public static long Insert(User userod) {
			return Insert(userod,false);
		}

		///<summary>Inserts one Userod into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(User userod,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				userod.UserNum=ReplicationServers.GetKey("userod","UserNum");
			}
			string command="INSERT INTO userod (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="UserNum,";
			}
			command+="UserName,Password,UserGroupNum,EmployeeNum,ClinicNum,ProvNum,IsHidden,TaskListInBox,AnesthProvType,DefaultHidePopups,PasswordIsStrong,ClinicIsRestricted,InboxHidePopups,UserNumCEMT,DateTFail,FailedAttempts,DomainUser,IsPasswordResetRequired) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(userod.UserNum)+",";
			}
			command+=
				 "'"+POut.String(userod.UserName)+"',"
				+"'"+POut.String(userod.Password)+"',"
				+    POut.Long  (userod.UserGroupNum)+","
				+    POut.Long  (userod.EmployeeNum)+","
				+    POut.Long  (userod.ClinicNum)+","
				+    POut.Long  (userod.ProvNum)+","
				+    POut.Bool  (userod.IsHidden)+","
				+    POut.Long  (userod.TaskListInBox)+","
				+    POut.Int   (userod.AnesthProvType)+","
				+    POut.Bool  (userod.DefaultHidePopups)+","
				+    POut.Bool  (userod.PasswordIsStrong)+","
				+    POut.Bool  (userod.ClinicIsRestricted)+","
				+    POut.Bool  (userod.InboxHidePopups)+","
				+    POut.Long  (userod.UserNumCEMT)+","
				+    POut.DateT (userod.DateTFail)+","
				+    POut.Byte  (userod.FailedAttempts)+","
				+"'"+POut.String(userod.DomainUser)+"',"
				+    POut.Bool  (userod.IsPasswordResetRequired)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				userod.UserNum=Db.NonQ(command,true,"UserNum","userod");
			}
			return userod.UserNum;
		}

		///<summary>Inserts one Userod into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(User userod) {
			return InsertNoCache(userod,false);
		}

		///<summary>Inserts one Userod into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(User userod,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO userod (";
			if(!useExistingPK && isRandomKeys) {
				userod.UserNum=ReplicationServers.GetKeyNoCache("userod","UserNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="UserNum,";
			}
			command+="UserName,Password,UserGroupNum,EmployeeNum,ClinicNum,ProvNum,IsHidden,TaskListInBox,AnesthProvType,DefaultHidePopups,PasswordIsStrong,ClinicIsRestricted,InboxHidePopups,UserNumCEMT,DateTFail,FailedAttempts,DomainUser,IsPasswordResetRequired) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(userod.UserNum)+",";
			}
			command+=
				 "'"+POut.String(userod.UserName)+"',"
				+"'"+POut.String(userod.Password)+"',"
				+    POut.Long  (userod.UserGroupNum)+","
				+    POut.Long  (userod.EmployeeNum)+","
				+    POut.Long  (userod.ClinicNum)+","
				+    POut.Long  (userod.ProvNum)+","
				+    POut.Bool  (userod.IsHidden)+","
				+    POut.Long  (userod.TaskListInBox)+","
				+    POut.Int   (userod.AnesthProvType)+","
				+    POut.Bool  (userod.DefaultHidePopups)+","
				+    POut.Bool  (userod.PasswordIsStrong)+","
				+    POut.Bool  (userod.ClinicIsRestricted)+","
				+    POut.Bool  (userod.InboxHidePopups)+","
				+    POut.Long  (userod.UserNumCEMT)+","
				+    POut.DateT (userod.DateTFail)+","
				+    POut.Byte  (userod.FailedAttempts)+","
				+"'"+POut.String(userod.DomainUser)+"',"
				+    POut.Bool  (userod.IsPasswordResetRequired)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				userod.UserNum=Db.NonQ(command,true,"UserNum","userod");
			}
			return userod.UserNum;
		}

		///<summary>Updates one Userod in the database.</summary>
		public static void Update(User userod) {
			string command="UPDATE userod SET "
				+"UserName               = '"+POut.String(userod.UserName)+"', "
				+"Password               = '"+POut.String(userod.Password)+"', "
				+"UserGroupNum           =  "+POut.Long  (userod.UserGroupNum)+", "
				+"EmployeeNum            =  "+POut.Long  (userod.EmployeeNum)+", "
				+"ClinicNum              =  "+POut.Long  (userod.ClinicNum)+", "
				+"ProvNum                =  "+POut.Long  (userod.ProvNum)+", "
				+"IsHidden               =  "+POut.Bool  (userod.IsHidden)+", "
				+"TaskListInBox          =  "+POut.Long  (userod.TaskListInBox)+", "
				+"AnesthProvType         =  "+POut.Int   (userod.AnesthProvType)+", "
				+"DefaultHidePopups      =  "+POut.Bool  (userod.DefaultHidePopups)+", "
				+"PasswordIsStrong       =  "+POut.Bool  (userod.PasswordIsStrong)+", "
				+"ClinicIsRestricted     =  "+POut.Bool  (userod.ClinicIsRestricted)+", "
				+"InboxHidePopups        =  "+POut.Bool  (userod.InboxHidePopups)+", "
				+"UserNumCEMT            =  "+POut.Long  (userod.UserNumCEMT)+", "
				+"DateTFail              =  "+POut.DateT (userod.DateTFail)+", "
				+"FailedAttempts         =  "+POut.Byte  (userod.FailedAttempts)+", "
				+"DomainUser             = '"+POut.String(userod.DomainUser)+"', "
				+"IsPasswordResetRequired=  "+POut.Bool  (userod.IsPasswordResetRequired)+" "
				+"WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Userod in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(User userod,User oldUserod) {
			string command="";
			if(userod.UserName != oldUserod.UserName) {
				if(command!="") { command+=",";}
				command+="UserName = '"+POut.String(userod.UserName)+"'";
			}
			if(userod.Password != oldUserod.Password) {
				if(command!="") { command+=",";}
				command+="Password = '"+POut.String(userod.Password)+"'";
			}
			if(userod.UserGroupNum != oldUserod.UserGroupNum) {
				if(command!="") { command+=",";}
				command+="UserGroupNum = "+POut.Long(userod.UserGroupNum)+"";
			}
			if(userod.EmployeeNum != oldUserod.EmployeeNum) {
				if(command!="") { command+=",";}
				command+="EmployeeNum = "+POut.Long(userod.EmployeeNum)+"";
			}
			if(userod.ClinicNum != oldUserod.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(userod.ClinicNum)+"";
			}
			if(userod.ProvNum != oldUserod.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(userod.ProvNum)+"";
			}
			if(userod.IsHidden != oldUserod.IsHidden) {
				if(command!="") { command+=",";}
				command+="IsHidden = "+POut.Bool(userod.IsHidden)+"";
			}
			if(userod.TaskListInBox != oldUserod.TaskListInBox) {
				if(command!="") { command+=",";}
				command+="TaskListInBox = "+POut.Long(userod.TaskListInBox)+"";
			}
			if(userod.AnesthProvType != oldUserod.AnesthProvType) {
				if(command!="") { command+=",";}
				command+="AnesthProvType = "+POut.Int(userod.AnesthProvType)+"";
			}
			if(userod.DefaultHidePopups != oldUserod.DefaultHidePopups) {
				if(command!="") { command+=",";}
				command+="DefaultHidePopups = "+POut.Bool(userod.DefaultHidePopups)+"";
			}
			if(userod.PasswordIsStrong != oldUserod.PasswordIsStrong) {
				if(command!="") { command+=",";}
				command+="PasswordIsStrong = "+POut.Bool(userod.PasswordIsStrong)+"";
			}
			if(userod.ClinicIsRestricted != oldUserod.ClinicIsRestricted) {
				if(command!="") { command+=",";}
				command+="ClinicIsRestricted = "+POut.Bool(userod.ClinicIsRestricted)+"";
			}
			if(userod.InboxHidePopups != oldUserod.InboxHidePopups) {
				if(command!="") { command+=",";}
				command+="InboxHidePopups = "+POut.Bool(userod.InboxHidePopups)+"";
			}
			if(userod.UserNumCEMT != oldUserod.UserNumCEMT) {
				if(command!="") { command+=",";}
				command+="UserNumCEMT = "+POut.Long(userod.UserNumCEMT)+"";
			}
			if(userod.DateTFail != oldUserod.DateTFail) {
				if(command!="") { command+=",";}
				command+="DateTFail = "+POut.DateT(userod.DateTFail)+"";
			}
			if(userod.FailedAttempts != oldUserod.FailedAttempts) {
				if(command!="") { command+=",";}
				command+="FailedAttempts = "+POut.Byte(userod.FailedAttempts)+"";
			}
			if(userod.DomainUser != oldUserod.DomainUser) {
				if(command!="") { command+=",";}
				command+="DomainUser = '"+POut.String(userod.DomainUser)+"'";
			}
			if(userod.IsPasswordResetRequired != oldUserod.IsPasswordResetRequired) {
				if(command!="") { command+=",";}
				command+="IsPasswordResetRequired = "+POut.Bool(userod.IsPasswordResetRequired)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE userod SET "+command
				+" WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Userod,Userod) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(User userod,User oldUserod) {
			if(userod.UserName != oldUserod.UserName) {
				return true;
			}
			if(userod.Password != oldUserod.Password) {
				return true;
			}
			if(userod.UserGroupNum != oldUserod.UserGroupNum) {
				return true;
			}
			if(userod.EmployeeNum != oldUserod.EmployeeNum) {
				return true;
			}
			if(userod.ClinicNum != oldUserod.ClinicNum) {
				return true;
			}
			if(userod.ProvNum != oldUserod.ProvNum) {
				return true;
			}
			if(userod.IsHidden != oldUserod.IsHidden) {
				return true;
			}
			if(userod.TaskListInBox != oldUserod.TaskListInBox) {
				return true;
			}
			if(userod.AnesthProvType != oldUserod.AnesthProvType) {
				return true;
			}
			if(userod.DefaultHidePopups != oldUserod.DefaultHidePopups) {
				return true;
			}
			if(userod.PasswordIsStrong != oldUserod.PasswordIsStrong) {
				return true;
			}
			if(userod.ClinicIsRestricted != oldUserod.ClinicIsRestricted) {
				return true;
			}
			if(userod.InboxHidePopups != oldUserod.InboxHidePopups) {
				return true;
			}
			if(userod.UserNumCEMT != oldUserod.UserNumCEMT) {
				return true;
			}
			if(userod.DateTFail != oldUserod.DateTFail) {
				return true;
			}
			if(userod.FailedAttempts != oldUserod.FailedAttempts) {
				return true;
			}
			if(userod.DomainUser != oldUserod.DomainUser) {
				return true;
			}
			if(userod.IsPasswordResetRequired != oldUserod.IsPasswordResetRequired) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Userod from the database.</summary>
		public static void Delete(long userNum) {
			string command="DELETE FROM userod "
				+"WHERE UserNum = "+POut.Long(userNum);
			Db.NonQ(command);
		}

	}
}