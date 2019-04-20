using System;
using System.Collections;

namespace OpenDentBusiness{
	
	///<summary>A supply freeform typed in by a user.</summary>
	[Serializable()]
	public class SupplyNeeded : ODTable {
		/// <summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long SupplyNeededNum;
		/// <summary>.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Description;
		/// <summary>.</summary>
		public DateTime DateAdded;

		

			
	}

	

}









