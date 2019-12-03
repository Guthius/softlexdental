namespace OpenDentBusiness
{
    ///<summary>A code system used in EHR.  Healhtcare Common Procedure Coding System.  Another system used to describe procedure codes.</summary>
    public class Hcpcs
    {
        public long HcpcsNum;

        ///<summary>Examples: AQ, J1040</summary>
        public string HcpcsCode;

        ///<summary>Short description.  This is the HCPCS supplied abbreviated description.</summary>
        public string DescriptionShort;
    }
}