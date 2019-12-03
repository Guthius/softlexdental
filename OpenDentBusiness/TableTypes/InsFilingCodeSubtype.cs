namespace OpenDentBusiness
{
    ///<summary>Stores the list of insurance filing code subtypes.</summary>
    public class InsFilingCodeSubtype
    {
        public long InsFilingCodeSubtypeNum;

        ///<summary>FK to insfilingcode.insfilingcodenum</summary>
        public long InsFilingCodeNum;

        ///<summary>The description of the insurance filing code subtype.</summary>
        public string Descript;

        public InsFilingCodeSubtype Clone()
        {
            return (InsFilingCodeSubtype)this.MemberwiseClone();
        }
    }
}
