using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.Mobile {

		///<summary>Links allergies to patients. Patient portal version</summary>
	[Serializable]
	[ODTable(IsMobile=true)]
	public class Allergym:ODTable {
		///<summary>Primary key 1.</summary>
		[ODTableColumn(IsPriKeyMobile1=true)]
		public long CustomerNum;
		///<summary>Primary key 2.</summary>
		[ODTableColumn(IsPriKeyMobile2=true)]
		public long AllergyNum;
		///<summary>FK to allergydef.AllergyDefNum</summary>
		public long AllergyDefNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary></summary>
		public string Reaction;
		///<summary></summary>
		public bool StatusIsActive;
		///<summary>The historical date that the patient had the adverse reaction to this agent.</summary>
		public DateTime DateAdverseReaction;

		///<summary></summary>
		public Allergym Copy() {
			return (Allergym)this.MemberwiseClone();
		}

	}
}
