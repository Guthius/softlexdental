using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabSpecimenRejectReasons {
		///<summary></summary>
		public static List<EhrLabSpecimenRejectReason> GetForEhrLabSpecimen(long ehrLabSpecimenNum) {
			string command="SELECT * FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum = "+POut.Long(ehrLabSpecimenNum);
			return Crud.EhrLabSpecimenRejectReasonCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void DeleteForLab(long ehrLabNum) {
			string command="DELETE FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum IN (SELECT EhrLabSpecimenNum FROM ehrlabspecimen WHERE EhrLabNum="+POut.Long(ehrLabNum)+")";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void DeleteForLabSpecimen(long ehrLabSpecimenNum) {
			string command="DELETE FROM ehrlabspecimenrejectreason WHERE EhrLabSpecimenNum="+POut.Long(ehrLabSpecimenNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(EhrLabSpecimenRejectReason ehrLabSpecimenRejectReason) {
			return Crud.EhrLabSpecimenRejectReasonCrud.Insert(ehrLabSpecimenRejectReason);
		}
	}
}