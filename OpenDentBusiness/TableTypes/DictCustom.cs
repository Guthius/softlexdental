using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Spell check custom dictionary, shared by the whole office.</summary>
	[Serializable()]
	public class DictCustom:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long DictCustomNum;
		/// <summary>No space or punctuation allowed.</summary>
		public string WordText;

		public DictCustom Copy() {
			return (DictCustom)this.MemberwiseClone();
		}
	}
}
