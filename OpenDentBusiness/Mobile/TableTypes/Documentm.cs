using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.Mobile {

		///<summary>Links allergies to patients. Patient portal version</summary>
	[Serializable]
	[ODTable(IsMobile=true)]
	public class Documentm:ODTable {
		///<summary>Primary key 1.</summary>
		[ODTableColumn(IsPriKeyMobile1=true)]
		public long CustomerNum;
		///<summary>Primary key 2.</summary>
		[ODTableColumn(IsPriKeyMobile2=true)]
		public long DocNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>The raw file data encoded as base64.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string RawBase64;
		///<summary></summary>
		public Documentm Copy() {
			return (Documentm)this.MemberwiseClone();
		}

	}
}

