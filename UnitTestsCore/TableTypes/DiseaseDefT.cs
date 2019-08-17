using System;
using System.Collections.Generic;
using System.Text;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class DiseaseDefT {

		///<summary>Inserts the new DiseaseDef address and returns it.</summary>
		public static DiseaseDef CreateDiseaseDef(string diseaseName="",string icd10code="") {
			DiseaseDef diseaseDef=new DiseaseDef();
			diseaseDef.Name=diseaseName;
			if(diseaseName=="") {
				diseaseDef.Name="Fatal Illness";
			}
			diseaseDef.ICD10Code=icd10code;
			DiseaseDefs.Insert(diseaseDef);
			DiseaseDefs.RefreshCache();
			return diseaseDef;
		}
		
	}
}
