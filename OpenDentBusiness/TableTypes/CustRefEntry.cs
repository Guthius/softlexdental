using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {

	///<summary>For internal use only.</summary>
	[Serializable]
	public class CustRefEntry:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long CustRefEntryNum;
		///<summary>FK to patient.PatNum.  The customer seeking a reference.</summary>
		public long PatNumCust;
		///<summary>FK to patient.PatNum.  The chosen reference.  This is the customer who was given as a reference to the new customer.</summary>
		public long PatNumRef;
		///<summary>Date the reference was chosen.</summary>
		public DateTime DateEntry;
		///<summary>Notes specific to this particular reference entry, mostly for a special reference situation.</summary>
		public string Note;

		///<summary></summary>
		public CustRefEntry Clone() {
			return (CustRefEntry)this.MemberwiseClone();
		}

	}

}