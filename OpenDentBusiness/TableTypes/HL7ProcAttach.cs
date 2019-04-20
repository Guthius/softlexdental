using System;

namespace OpenDentBusiness {
	///<summary>Keeps track of whether procedures have been sent in an HL7 message.</summary>
	[Serializable()]
	public class HL7ProcAttach:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long HL7ProcAttachNum;
		///<summary>FK to hl7msg.HL7MsgNum.</summary>
		public long HL7MsgNum;
		///<summary>FK to procedurelog.ProcNum.</summary>
		public long ProcNum;

		public HL7ProcAttach Clone() {
			return (HL7ProcAttach)this.MemberwiseClone();
		}
	}
}
