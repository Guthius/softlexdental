using System;
using System.Collections;

namespace OpenDentBusiness{
	///<summary>Only used internally by OpenDental, Inc.  Not used by anyone else.</summary>
	[Serializable()]
	[ODTable(IsMissingInGeneral=true)]//It actually is present, but the s classs is in a weird place.
	public class PhoneNumber : ODTable{
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long PhoneNumberNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>The actual phone number for the patient.  Includes any punctuation.  No leading 1 or plus, so almost always 10 digits.</summary>
		public string PhoneNumberVal;

	
	}
}















