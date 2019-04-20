using System;
using System.Collections;

namespace OpenDentBusiness{
	///<summary>Used in public health.</summary>
	[Serializable]
	public class County:ODTable {
		///<summary>Primary Key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long CountyNum;
		///<summary>Frequently used as the primary key of this table.  But it's allowed to change.  Change is programmatically synchronized.</summary>
		public string CountyName;
		///<summary>Optional. Usage varies.</summary>
		public string CountyCode;
		///<summary>Not a database field. This is the unaltered CountyName. Used for Update.</summary>
		[ODTableColumn(IsNotDbColumn=true)]
		public string OldCountyName;
	}


	

}













