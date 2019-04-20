using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>Links Procedures(groupnotes) to Procedures in a 1-n relationship.</summary>
	[Serializable]
	[ODTable(HasBatchWriteMethods=true)]
	public class ProcGroupItem:ODTable{
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ProcGroupItemNum;
		///<summary>FK to procedurelog.ProcNum.</summary>
		public long ProcNum;
		///<summary>FK to procedurelog.ProcNum.</summary>
		public long GroupNum;

		///<summary></summary>
		public ProcGroupItem Clone() {
			return (ProcGroupItem)this.MemberwiseClone();
		}

	}
}