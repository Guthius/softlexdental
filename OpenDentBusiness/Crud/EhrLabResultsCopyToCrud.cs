//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using EhrLaboratories;

namespace OpenDentBusiness.Crud{
	public class EhrLabResultsCopyToCrud {
		///<summary>Gets one EhrLabResultsCopyTo object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrLabResultsCopyTo SelectOne(long ehrLabResultsCopyToNum) {
			string command="SELECT * FROM ehrlabresultscopyto "
				+"WHERE EhrLabResultsCopyToNum = "+POut.Long(ehrLabResultsCopyToNum);
			List<EhrLabResultsCopyTo> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrLabResultsCopyTo object from the database using a query.</summary>
		public static EhrLabResultsCopyTo SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabResultsCopyTo> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrLabResultsCopyTo objects from the database using a query.</summary>
		public static List<EhrLabResultsCopyTo> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabResultsCopyTo> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrLabResultsCopyTo> TableToList(DataTable table) {
			List<EhrLabResultsCopyTo> retVal=new List<EhrLabResultsCopyTo>();
			EhrLabResultsCopyTo ehrLabResultsCopyTo;
			foreach(DataRow row in table.Rows) {
				ehrLabResultsCopyTo=new EhrLabResultsCopyTo();
				ehrLabResultsCopyTo.EhrLabResultsCopyToNum             = PIn.Long  (row["EhrLabResultsCopyToNum"].ToString());
				ehrLabResultsCopyTo.EhrLabNum                          = PIn.Long  (row["EhrLabNum"].ToString());
				ehrLabResultsCopyTo.CopyToID                           = PIn.String(row["CopyToID"].ToString());
				ehrLabResultsCopyTo.CopyToLName                        = PIn.String(row["CopyToLName"].ToString());
				ehrLabResultsCopyTo.CopyToFName                        = PIn.String(row["CopyToFName"].ToString());
				ehrLabResultsCopyTo.CopyToMiddleNames                  = PIn.String(row["CopyToMiddleNames"].ToString());
				ehrLabResultsCopyTo.CopyToSuffix                       = PIn.String(row["CopyToSuffix"].ToString());
				ehrLabResultsCopyTo.CopyToPrefix                       = PIn.String(row["CopyToPrefix"].ToString());
				ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID= PIn.String(row["CopyToAssigningAuthorityNamespaceID"].ToString());
				ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID= PIn.String(row["CopyToAssigningAuthorityUniversalID"].ToString());
				ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType     = PIn.String(row["CopyToAssigningAuthorityIDType"].ToString());
				string copyToNameTypeCode=row["CopyToNameTypeCode"].ToString();
				if(copyToNameTypeCode=="") {
					ehrLabResultsCopyTo.CopyToNameTypeCode               =(HL70200)0;
				}
				else try{
					ehrLabResultsCopyTo.CopyToNameTypeCode               =(HL70200)Enum.Parse(typeof(HL70200),copyToNameTypeCode);
				}
				catch{
					ehrLabResultsCopyTo.CopyToNameTypeCode               =(HL70200)0;
				}
				string copyToIdentifierTypeCode=row["CopyToIdentifierTypeCode"].ToString();
				if(copyToIdentifierTypeCode=="") {
					ehrLabResultsCopyTo.CopyToIdentifierTypeCode         =(HL70203)0;
				}
				else try{
					ehrLabResultsCopyTo.CopyToIdentifierTypeCode         =(HL70203)Enum.Parse(typeof(HL70203),copyToIdentifierTypeCode);
				}
				catch{
					ehrLabResultsCopyTo.CopyToIdentifierTypeCode         =(HL70203)0;
				}
				retVal.Add(ehrLabResultsCopyTo);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrLabResultsCopyTo into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrLabResultsCopyTo> listEhrLabResultsCopyTos,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrLabResultsCopyTo";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrLabResultsCopyToNum");
			table.Columns.Add("EhrLabNum");
			table.Columns.Add("CopyToID");
			table.Columns.Add("CopyToLName");
			table.Columns.Add("CopyToFName");
			table.Columns.Add("CopyToMiddleNames");
			table.Columns.Add("CopyToSuffix");
			table.Columns.Add("CopyToPrefix");
			table.Columns.Add("CopyToAssigningAuthorityNamespaceID");
			table.Columns.Add("CopyToAssigningAuthorityUniversalID");
			table.Columns.Add("CopyToAssigningAuthorityIDType");
			table.Columns.Add("CopyToNameTypeCode");
			table.Columns.Add("CopyToIdentifierTypeCode");
			foreach(EhrLabResultsCopyTo ehrLabResultsCopyTo in listEhrLabResultsCopyTos) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrLabResultsCopyTo.EhrLabResultsCopyToNum),
					POut.Long  (ehrLabResultsCopyTo.EhrLabNum),
					            ehrLabResultsCopyTo.CopyToID,
					            ehrLabResultsCopyTo.CopyToLName,
					            ehrLabResultsCopyTo.CopyToFName,
					            ehrLabResultsCopyTo.CopyToMiddleNames,
					            ehrLabResultsCopyTo.CopyToSuffix,
					            ehrLabResultsCopyTo.CopyToPrefix,
					            ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID,
					            ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID,
					            ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType,
					POut.Int   ((int)ehrLabResultsCopyTo.CopyToNameTypeCode),
					POut.Int   ((int)ehrLabResultsCopyTo.CopyToIdentifierTypeCode),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrLabResultsCopyTo into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrLabResultsCopyTo ehrLabResultsCopyTo) {
			return Insert(ehrLabResultsCopyTo,false);
		}

		///<summary>Inserts one EhrLabResultsCopyTo into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrLabResultsCopyTo ehrLabResultsCopyTo,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrLabResultsCopyTo.EhrLabResultsCopyToNum=ReplicationServers.GetKey("ehrlabresultscopyto","EhrLabResultsCopyToNum");
			}
			string command="INSERT INTO ehrlabresultscopyto (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrLabResultsCopyToNum,";
			}
			command+="EhrLabNum,CopyToID,CopyToLName,CopyToFName,CopyToMiddleNames,CopyToSuffix,CopyToPrefix,CopyToAssigningAuthorityNamespaceID,CopyToAssigningAuthorityUniversalID,CopyToAssigningAuthorityIDType,CopyToNameTypeCode,CopyToIdentifierTypeCode) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrLabResultsCopyTo.EhrLabResultsCopyToNum)+",";
			}
			command+=
				     POut.Long  (ehrLabResultsCopyTo.EhrLabNum)+","
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToLName)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToFName)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToMiddleNames)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToSuffix)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToPrefix)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToNameTypeCode.ToString())+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToIdentifierTypeCode.ToString())+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrLabResultsCopyTo.EhrLabResultsCopyToNum=Db.NonQ(command,true,"EhrLabResultsCopyToNum","ehrLabResultsCopyTo");
			}
			return ehrLabResultsCopyTo.EhrLabResultsCopyToNum;
		}

		///<summary>Inserts one EhrLabResultsCopyTo into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrLabResultsCopyTo ehrLabResultsCopyTo) {
			return InsertNoCache(ehrLabResultsCopyTo,false);
		}

		///<summary>Inserts one EhrLabResultsCopyTo into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrLabResultsCopyTo ehrLabResultsCopyTo,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrlabresultscopyto (";
			if(!useExistingPK && isRandomKeys) {
				ehrLabResultsCopyTo.EhrLabResultsCopyToNum=ReplicationServers.GetKeyNoCache("ehrlabresultscopyto","EhrLabResultsCopyToNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrLabResultsCopyToNum,";
			}
			command+="EhrLabNum,CopyToID,CopyToLName,CopyToFName,CopyToMiddleNames,CopyToSuffix,CopyToPrefix,CopyToAssigningAuthorityNamespaceID,CopyToAssigningAuthorityUniversalID,CopyToAssigningAuthorityIDType,CopyToNameTypeCode,CopyToIdentifierTypeCode) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrLabResultsCopyTo.EhrLabResultsCopyToNum)+",";
			}
			command+=
				     POut.Long  (ehrLabResultsCopyTo.EhrLabNum)+","
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToLName)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToFName)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToMiddleNames)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToSuffix)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToPrefix)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType)+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToNameTypeCode.ToString())+"',"
				+"'"+POut.String(ehrLabResultsCopyTo.CopyToIdentifierTypeCode.ToString())+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrLabResultsCopyTo.EhrLabResultsCopyToNum=Db.NonQ(command,true,"EhrLabResultsCopyToNum","ehrLabResultsCopyTo");
			}
			return ehrLabResultsCopyTo.EhrLabResultsCopyToNum;
		}

		///<summary>Updates one EhrLabResultsCopyTo in the database.</summary>
		public static void Update(EhrLabResultsCopyTo ehrLabResultsCopyTo) {
			string command="UPDATE ehrlabresultscopyto SET "
				+"EhrLabNum                          =  "+POut.Long  (ehrLabResultsCopyTo.EhrLabNum)+", "
				+"CopyToID                           = '"+POut.String(ehrLabResultsCopyTo.CopyToID)+"', "
				+"CopyToLName                        = '"+POut.String(ehrLabResultsCopyTo.CopyToLName)+"', "
				+"CopyToFName                        = '"+POut.String(ehrLabResultsCopyTo.CopyToFName)+"', "
				+"CopyToMiddleNames                  = '"+POut.String(ehrLabResultsCopyTo.CopyToMiddleNames)+"', "
				+"CopyToSuffix                       = '"+POut.String(ehrLabResultsCopyTo.CopyToSuffix)+"', "
				+"CopyToPrefix                       = '"+POut.String(ehrLabResultsCopyTo.CopyToPrefix)+"', "
				+"CopyToAssigningAuthorityNamespaceID= '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID)+"', "
				+"CopyToAssigningAuthorityUniversalID= '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID)+"', "
				+"CopyToAssigningAuthorityIDType     = '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType)+"', "
				+"CopyToNameTypeCode                 = '"+POut.String(ehrLabResultsCopyTo.CopyToNameTypeCode.ToString())+"', "
				+"CopyToIdentifierTypeCode           = '"+POut.String(ehrLabResultsCopyTo.CopyToIdentifierTypeCode.ToString())+"' "
				+"WHERE EhrLabResultsCopyToNum = "+POut.Long(ehrLabResultsCopyTo.EhrLabResultsCopyToNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrLabResultsCopyTo in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrLabResultsCopyTo ehrLabResultsCopyTo,EhrLabResultsCopyTo oldEhrLabResultsCopyTo) {
			string command="";
			if(ehrLabResultsCopyTo.EhrLabNum != oldEhrLabResultsCopyTo.EhrLabNum) {
				if(command!="") { command+=",";}
				command+="EhrLabNum = "+POut.Long(ehrLabResultsCopyTo.EhrLabNum)+"";
			}
			if(ehrLabResultsCopyTo.CopyToID != oldEhrLabResultsCopyTo.CopyToID) {
				if(command!="") { command+=",";}
				command+="CopyToID = '"+POut.String(ehrLabResultsCopyTo.CopyToID)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToLName != oldEhrLabResultsCopyTo.CopyToLName) {
				if(command!="") { command+=",";}
				command+="CopyToLName = '"+POut.String(ehrLabResultsCopyTo.CopyToLName)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToFName != oldEhrLabResultsCopyTo.CopyToFName) {
				if(command!="") { command+=",";}
				command+="CopyToFName = '"+POut.String(ehrLabResultsCopyTo.CopyToFName)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToMiddleNames != oldEhrLabResultsCopyTo.CopyToMiddleNames) {
				if(command!="") { command+=",";}
				command+="CopyToMiddleNames = '"+POut.String(ehrLabResultsCopyTo.CopyToMiddleNames)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToSuffix != oldEhrLabResultsCopyTo.CopyToSuffix) {
				if(command!="") { command+=",";}
				command+="CopyToSuffix = '"+POut.String(ehrLabResultsCopyTo.CopyToSuffix)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToPrefix != oldEhrLabResultsCopyTo.CopyToPrefix) {
				if(command!="") { command+=",";}
				command+="CopyToPrefix = '"+POut.String(ehrLabResultsCopyTo.CopyToPrefix)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID) {
				if(command!="") { command+=",";}
				command+="CopyToAssigningAuthorityNamespaceID = '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID) {
				if(command!="") { command+=",";}
				command+="CopyToAssigningAuthorityUniversalID = '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityIDType) {
				if(command!="") { command+=",";}
				command+="CopyToAssigningAuthorityIDType = '"+POut.String(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType)+"'";
			}
			if(ehrLabResultsCopyTo.CopyToNameTypeCode != oldEhrLabResultsCopyTo.CopyToNameTypeCode) {
				if(command!="") { command+=",";}
				command+="CopyToNameTypeCode = '"+POut.String(ehrLabResultsCopyTo.CopyToNameTypeCode.ToString())+"'";
			}
			if(ehrLabResultsCopyTo.CopyToIdentifierTypeCode != oldEhrLabResultsCopyTo.CopyToIdentifierTypeCode) {
				if(command!="") { command+=",";}
				command+="CopyToIdentifierTypeCode = '"+POut.String(ehrLabResultsCopyTo.CopyToIdentifierTypeCode.ToString())+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE ehrlabresultscopyto SET "+command
				+" WHERE EhrLabResultsCopyToNum = "+POut.Long(ehrLabResultsCopyTo.EhrLabResultsCopyToNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EhrLabResultsCopyTo,EhrLabResultsCopyTo) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrLabResultsCopyTo ehrLabResultsCopyTo,EhrLabResultsCopyTo oldEhrLabResultsCopyTo) {
			if(ehrLabResultsCopyTo.EhrLabNum != oldEhrLabResultsCopyTo.EhrLabNum) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToID != oldEhrLabResultsCopyTo.CopyToID) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToLName != oldEhrLabResultsCopyTo.CopyToLName) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToFName != oldEhrLabResultsCopyTo.CopyToFName) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToMiddleNames != oldEhrLabResultsCopyTo.CopyToMiddleNames) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToSuffix != oldEhrLabResultsCopyTo.CopyToSuffix) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToPrefix != oldEhrLabResultsCopyTo.CopyToPrefix) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityNamespaceID) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityUniversalID) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToAssigningAuthorityIDType != oldEhrLabResultsCopyTo.CopyToAssigningAuthorityIDType) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToNameTypeCode != oldEhrLabResultsCopyTo.CopyToNameTypeCode) {
				return true;
			}
			if(ehrLabResultsCopyTo.CopyToIdentifierTypeCode != oldEhrLabResultsCopyTo.CopyToIdentifierTypeCode) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrLabResultsCopyTo from the database.</summary>
		public static void Delete(long ehrLabResultsCopyToNum) {
			string command="DELETE FROM ehrlabresultscopyto "
				+"WHERE EhrLabResultsCopyToNum = "+POut.Long(ehrLabResultsCopyToNum);
			Db.NonQ(command);
		}

	}
}