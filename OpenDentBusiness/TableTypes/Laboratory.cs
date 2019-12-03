namespace OpenDentBusiness
{
    ///<summary>A dental laboratory. Will be attached to lab cases.</summary>
    public class Laboratory
    {
        public long LaboratoryNum;

        ///<summary>Description of lab.</summary>
        public string Description;

        ///<summary>Freeform text includes punctuation.</summary>
        public string Phone;

        ///<summary>Any notes.  No practical limit to amount of text.</summary>
        public string Notes;

        ///<summary>FK to sheetdef.SheetDefNum.  Lab slips can be set for individual laboratories.  If zero, then the default internal lab slip will be used instead of a custom lab slip.</summary>
        public long Slip;

        ///<summary>.</summary>
        public string Address;

        ///<summary>.</summary>
        public string City;

        ///<summary>.</summary>
        public string State;

        ///<summary>.</summary>
        public string Zip;

        ///<summary>.</summary>
        public string Email;

        ///<summary>.</summary>
        public string WirelessPhone;

        ///<summary></summary>
        public bool IsHidden;
    }
}
