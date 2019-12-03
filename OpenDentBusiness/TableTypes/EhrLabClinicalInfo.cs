using EhrLaboratories;
using System;

namespace OpenDentBusiness
{
    ///<summary>For EHR module, lab request that contains all required fields for HL7 Lab Reporting Interface (LRI).  OBR.13.*</summary>
    public class EhrLabClinicalInfo
    {
        public long EhrLabClinicalInfoNum;

        ///<summary>FK to ehrlab.EhrLabNum.</summary>
        public long EhrLabNum;

        ///<summary>OBR.13.*.1</summary>
        public string ClinicalInfoID;

        ///<summary>Description of ClinicalInfoId.  OBR.13.*.2</summary>
        public string ClinicalInfoText;

        ///<summary>CodeSystem that ClinicalInfoId came from.  OBR.13.*.3</summary>
        public string ClinicalInfoCodeSystemName;

        ///<summary>OBR.13.*.4</summary>
        public string ClinicalInfoIDAlt;

        ///<summary>Description of ClinicalInfoIdAlt.  OBR.13.*.5</summary>
        public string ClinicalInfoTextAlt;

        ///<summary>CodeSystem that ClinicalInfoId came from.  OBR.13.*.6</summary>
        public string ClinicalInfoCodeSystemNameAlt;

        ///<summary>Optional text that describes the original text used to encode the values above.  OBR.13.*.7</summary>
        public string ClinicalInfoTextOriginal;
    }
}
