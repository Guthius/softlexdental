using System;
using System.Collections.Generic;
using System.Text;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class Icd10T {

		///<summary>Inserts the new Icd10 code and returns it.</summary>
		public static ICD10 CreateIcd10(string icd10code = "") {
			ICD10 icd10=new ICD10();
			icd10.Code=icd10code;
			if(icd10code=="") {
				icd10.Code="Z.100.100";
			}
			icd10.Code=icd10code;
			Icd10s.Insert(icd10);
			return icd10;
		}

	}
}
