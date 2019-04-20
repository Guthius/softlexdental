using System;

namespace OpenDentBusiness{

	///<summary>Allows multiple groups to be attached to a user.  Security permissions are determined by the usergroups of a user.</summary>
	[Serializable]
	public class UserGroupAttach:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long UserGroupAttachNum;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNum;
		///<summary>FK to usergroup.UserGroupNum. </summary>
		public long UserGroupNum;

		public UserGroupAttach Copy() {
			return (UserGroupAttach)this.MemberwiseClone();
		}
		

	}
 
	

	
}













