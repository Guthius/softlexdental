using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	///<summary>One file attached to an eob (claimpayment).  Multiple files can be attached to an eob using this method.  Order shown will be based on date/time scanned.</summary>
	[Serializable]
	public class EobAttach:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long EobAttachNum;
		///<summary>FK to claimpayment.ClaimPaymentNum</summary>
		public long ClaimPaymentNum;
		///<summary>Date/time created.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTCreated;
		///<summary>The file is stored in the A-Z folder in 'EOBs' folder.  This field stores the name of the file.  The files are named automatically based on Date/time along with EobAttachNum for uniqueness.</summary>
		public string FileName;
		///<summary>The raw file data encoded as base64.  Only used if there is no AtoZ folder.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string RawBase64;
		

		public EobAttach Copy() {
			return (EobAttach)MemberwiseClone();
		}
	}




}
