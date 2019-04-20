using System;
using System.Collections;

namespace OpenDentBusiness {
	///<summary>A manually created autograph that can be inserted at the bottom of an outgoing email.</summary>
	[Serializable]
	public class EmailAutograph:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long EmailAutographNum;
		///<summary>Description of the autograph.  This is what the user sees when picking an autograph.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Description;
		///<summary>Email address(es) that this autograph is associated with.  An autograph can be associated with multiple addresses.</summary>
		public string EmailAddress;
		///<summary>The actual text of the autograph.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string AutographText;
		
		///<summary>Returns a copy of this EmailAutograph.</summary>
		public EmailAutograph Copy() {
			return (EmailAutograph)this.MemberwiseClone();
		}	
		
	}
}
