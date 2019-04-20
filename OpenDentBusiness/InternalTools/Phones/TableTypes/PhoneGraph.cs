using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[ODTable(IsMissingInGeneral=true)]
	public class PhoneGraph:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long PhoneGraphNum;
		///<summary>FK to employee.EmployeeNum.</summary>
		public long EmployeeNum;
		///<summary>Ammends PhoneEmpDefault.IsGraphed for the given DateEntry</summary>
		public bool IsGraphed;
		///<summary>Date pertaining to this entry.</summary>
		public DateTime DateEntry;

		public Phone Copy() {
			return (Phone)this.MemberwiseClone();
		}
	}
}
