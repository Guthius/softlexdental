﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.Mobile {
	[Serializable()]
	[ODTable(IsMobile=true)]
	public class Pharmacym:ODTable {
		///<summary>Primary key 1.</summary>
		[ODTableColumn(IsPriKeyMobile1=true)]
		public long CustomerNum;
		///<summary>Primary key 2.</summary>
		[ODTableColumn(IsPriKeyMobile2=true)]
		public long PharmacyNum;
		///<summary>For now, it can just be a common description.  Later, it might have to be an official designation.</summary>
		public string StoreName;
		///<summary>Includes all punctuation.</summary>
		public string Phone;
		///<summary>Includes all punctuation.</summary>
		public string Fax;
		///<summary></summary>
		public string Address;
		///<summary>Optional.</summary>
		public string Address2;
		///<summary></summary>
		public string City;
		///<summary>Two char, uppercase.</summary>
		public string State;
		///<summary></summary>
		public string Zip;
		///<summary>A freeform note for any info that is needed about the pharmacy, such as hours.</summary>
		public string Note;

		public Pharmacym Copy() {
			return (Pharmacym)this.MemberwiseClone();
		}
	}
}
