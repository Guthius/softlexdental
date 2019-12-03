using System;

namespace OpenDentBusiness
{
    ///<summary>For internal use only.</summary>
    public class CustRefEntry
    {
        public long CustRefEntryNum;

        ///<summary>FK to patient.PatNum.  The customer seeking a reference.</summary>
        public long PatNumCust;

        ///<summary>FK to patient.PatNum.  The chosen reference.  This is the customer who was given as a reference to the new customer.</summary>
        public long PatNumRef;

        ///<summary>Date the reference was chosen.</summary>
        public DateTime DateEntry;

        ///<summary>Notes specific to this particular reference entry, mostly for a special reference situation.</summary>
        public string Note;
    }
}
