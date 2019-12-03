namespace OpenDentBusiness
{
    ///<summary>For EHR module, the specimen upon which the lab orders were/are to be performed on.  SPM.24</summary>
    public class EhrLabSpecimenCondition
    {
        public long EhrLabSpecimenConditionNum;

        ///<summary>FK to ehrlabspecimen.EhrLabSpecimenNum.</summary>
        public long EhrLabSpecimenNum;

        ///<summary>SPM.24.1</summary>
        public string SpecimenConditionID;

        ///<summary>Description of SpecimenConditionId.  SPM.24.2</summary>
        public string SpecimenConditionText;

        ///<summary>CodeSystem that SpecimenConditionId came from.  SPM.24.3</summary>
        public string SpecimenConditionCodeSystemName;

        ///<summary>SPM.24.4</summary>
        public string SpecimenConditionIDAlt;

        ///<summary>Description of SpecimenConditionIdAlt.  SPM.24.5</summary>
        public string SpecimenConditionTextAlt;

        ///<summary>CodeSystem that SpecimenConditionId came from.  SPM.24.6</summary>
        public string SpecimenConditionCodeSystemNameAlt;

        ///<summary>Optional text that describes the original text used to encode the values above.  SPM.24.7</summary>
        public string SpecimenConditionTextOriginal;
    }
}

namespace EhrLaboratories
{
    ///<summary>Order Control Code.  We only use RE.  OID:2.16.840.1.113883.12.119  HL70369 code:HL70119.  Source phinvads.cdc.gov</summary>
    public enum HL70493
    {
        //TODO
    }
}
