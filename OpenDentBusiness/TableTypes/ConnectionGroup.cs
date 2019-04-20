using System;

namespace OpenDentBusiness {
	///<summary>Used in the Central Enterprise Management Tool for creating a group of connections.</summary>
	[Serializable()]
	[ODTable(IsSynchable=true)]
	public class ConnectionGroup:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ConnectionGroupNum;
		///<summary>Description of the connection group</summary>
		public string Description;

		///<summary></summary>
		public ConnectionGroup Clone() {
			return (ConnectionGroup)this.MemberwiseClone();
		}

		public ConnectionGroup Copy() {
			ConnectionGroup connGroup=new ConnectionGroup();
			connGroup.ConnectionGroupNum=this.ConnectionGroupNum;
			connGroup.Description=this.Description;
			return connGroup;
		}

	}

}
