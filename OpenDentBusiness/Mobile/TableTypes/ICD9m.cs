using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.Mobile {
	[Serializable]
	[ODTable(IsMobile=true)]
	public class ICD9m:ODTable {
		///<summary>Primary key 1.</summary>
		[ODTableColumn(IsPriKeyMobile1=true)]
		public long CustomerNum;
		///<summary>Primary key 2.</summary>
		[ODTableColumn(IsPriKeyMobile2=true)]
		public long ICD9Num;
		///<summary>Not allowed to edit this column once saved in the database.</summary>
		public string ICD9Code;
		///<summary>Description.</summary>
		public string Description;

		///<summary></summary>
		public ICD9m Copy() {
			return (ICD9m)this.MemberwiseClone();
		}



	}
}
