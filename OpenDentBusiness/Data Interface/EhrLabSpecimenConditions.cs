using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabSpecimenConditions {
		///<summary></summary>
		public static List<EhrLabSpecimenCondition> GetForEhrLabSpecimen(long ehrLabSpecimenNum) {
			string command="SELECT * FROM ehrlabspecimencondition WHERE EhrLabSpecimenNum="+POut.Long(ehrLabSpecimenNum);
			return Crud.EhrLabSpecimenConditionCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void DeleteForLab(long ehrLabNum) {
			string command="DELETE FROM ehrlabspecimencondition WHERE EhrLabSpecimenNum IN (SELECT EhrLabSpecimenNum FROM ehrlabspecimen WHERE EhrLabNum="+POut.Long(ehrLabNum)+")";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void DeleteForLabSpecimen(long ehrLabSpecimenNum) {
			string command="DELETE FROM ehrlabspecimencondition WHERE EhrLabSpecimenNum="+POut.Long(ehrLabSpecimenNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(EhrLabSpecimenCondition ehrLabSpecimenCondition) {
			return Crud.EhrLabSpecimenConditionCrud.Insert(ehrLabSpecimenCondition);
		}
	}
}