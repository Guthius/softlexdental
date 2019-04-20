using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrNotPerformeds{
		///<summary></summary>
		public static List<EhrNotPerformed> Refresh(long patNum){
			string command="SELECT * FROM ehrnotperformed WHERE PatNum = "+POut.Long(patNum)+" ORDER BY DateEntry";
			return Crud.EhrNotPerformedCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EhrNotPerformed ehrNotPerformed){
			return Crud.EhrNotPerformedCrud.Insert(ehrNotPerformed);
		}

		///<summary></summary>
		public static void Update(EhrNotPerformed ehrNotPerformed){
			Crud.EhrNotPerformedCrud.Update(ehrNotPerformed);
		}

		///<summary></summary>
		public static void Delete(long ehrNotPerformedNum) {
			string command= "DELETE FROM ehrnotperformed WHERE EhrNotPerformedNum = "+POut.Long(ehrNotPerformedNum);
			Db.NonQ(command);
		}

		///<summary>Gets one EhrNotPerformed from the db.</summary>
		public static EhrNotPerformed GetOne(long ehrNotPerformedNum) {
			return Crud.EhrNotPerformedCrud.SelectOne(ehrNotPerformedNum);
		}
	}
}