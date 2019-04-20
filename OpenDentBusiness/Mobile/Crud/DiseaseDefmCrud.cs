//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Mobile.Crud{
	internal class DiseaseDefmCrud {
		///<summary>Gets one DiseaseDefm object from the database using primaryKey1(CustomerNum) and primaryKey2.  Returns null if not found.</summary>
		internal static DiseaseDefm SelectOne(long customerNum,long diseaseDefNum){
			string command="SELECT * FROM diseasedefm "
				+"WHERE CustomerNum = "+POut.Long(customerNum)+" AND DiseaseDefNum = "+POut.Long(diseaseDefNum);
			List<DiseaseDefm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one DiseaseDefm object from the database using a query.</summary>
		internal static DiseaseDefm SelectOne(string command){
			
			List<DiseaseDefm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of DiseaseDefm objects from the database using a query.</summary>
		internal static List<DiseaseDefm> SelectMany(string command){
			
			List<DiseaseDefm> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<DiseaseDefm> TableToList(DataTable table){
			List<DiseaseDefm> retVal=new List<DiseaseDefm>();
			DiseaseDefm diseaseDefm;
			for(int i=0;i<table.Rows.Count;i++) {
				diseaseDefm=new DiseaseDefm();
				diseaseDefm.CustomerNum  = PIn.Long  (table.Rows[i]["CustomerNum"].ToString());
				diseaseDefm.DiseaseDefNum= PIn.Long  (table.Rows[i]["DiseaseDefNum"].ToString());
				diseaseDefm.DiseaseName  = PIn.String(table.Rows[i]["DiseaseName"].ToString());
				retVal.Add(diseaseDefm);
			}
			return retVal;
		}

		///<summary>Usually set useExistingPK=true.  Inserts one DiseaseDefm into the database.</summary>
		internal static long Insert(DiseaseDefm diseaseDefm,bool useExistingPK){
			if(!useExistingPK) {
				diseaseDefm.DiseaseDefNum=ReplicationServers.GetKey("diseasedefm","DiseaseDefNum");
			}
			string command="INSERT INTO diseasedefm (";
			command+="DiseaseDefNum,";
			command+="CustomerNum,DiseaseName) VALUES(";
			command+=POut.Long(diseaseDefm.DiseaseDefNum)+",";
			command+=
				     POut.Long  (diseaseDefm.CustomerNum)+","
				+"'"+POut.String(diseaseDefm.DiseaseName)+"')";
			Db.NonQ(command);//There is no autoincrement in the mobile server.
			return diseaseDefm.DiseaseDefNum;
		}

		///<summary>Updates one DiseaseDefm in the database.</summary>
		internal static void Update(DiseaseDefm diseaseDefm){
			string command="UPDATE diseasedefm SET "
				+"DiseaseName  = '"+POut.String(diseaseDefm.DiseaseName)+"' "
				+"WHERE CustomerNum = "+POut.Long(diseaseDefm.CustomerNum)+" AND DiseaseDefNum = "+POut.Long(diseaseDefm.DiseaseDefNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one DiseaseDefm from the database.</summary>
		internal static void Delete(long customerNum,long diseaseDefNum){
			string command="DELETE FROM diseasedefm "
				+"WHERE CustomerNum = "+POut.Long(customerNum)+" AND DiseaseDefNum = "+POut.Long(diseaseDefNum);
			Db.NonQ(command);
		}

		///<summary>Converts one DiseaseDef object to its mobile equivalent.  Warning! CustomerNum will always be 0.</summary>
		internal static DiseaseDefm ConvertToM(DiseaseDef diseaseDef){
			DiseaseDefm diseaseDefm=new DiseaseDefm();
			//CustomerNum cannot be set.  Remains 0.
			diseaseDefm.DiseaseDefNum=diseaseDef.DiseaseDefNum;
			diseaseDefm.DiseaseName  =diseaseDef.DiseaseName;
			return diseaseDefm;
		}

	}
}