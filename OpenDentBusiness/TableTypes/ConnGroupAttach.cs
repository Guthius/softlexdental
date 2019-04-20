using System;

namespace OpenDentBusiness {
	///<summary>Used in the Central Enterprise Management Tool to link CentralConnections and ConnectionGroups.</summary>
	[Serializable()]
	[ODTable(IsSynchable=true)]
	public class ConnGroupAttach:ODTable {
		///<summary>Primary Key</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ConnGroupAttachNum;
		///<summary>FK to connectiongroup.ConnectionGroupNum</summary>
		public long ConnectionGroupNum;
		///<summary>FK to centralconnection.CentralConnectionNum</summary>
		public long CentralConnectionNum;

		///<summary></summary>
		public ConnGroupAttach Clone() {
			return (ConnGroupAttach)this.MemberwiseClone();
		}

	}

}
