namespace OpenDentBusiness
{
    ///<summary>And other kinds of units.  We will only prefill this list with units needed for the tests.  Users would have to manually add any other units.</summary>
    public class DrugUnit
    {
        public long DrugUnitNum;

        ///<summary>Example ml, capitalization not critical. Usually entered as lowercase except for L.</summary>
        public string UnitIdentifier;

        ///<summary>Example milliliter.</summary>
        public string UnitText;

        public DrugUnit Copy()
        {
            return (DrugUnit)this.MemberwiseClone();
        }
    }
}
