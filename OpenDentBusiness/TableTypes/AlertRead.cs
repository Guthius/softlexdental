using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	[ODTable(IsSynchable=true)]
	public class AlertRead:ODTable{
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long AlertReadNum;
		///<summary>FK to alertitem.AlertItemNum.</summary>
		public long AlertItemNum;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNum;

		public AlertRead() {
			
		}

		public AlertRead(long alertItemNum,long userNum) {
			this.AlertItemNum=alertItemNum;
			this.UserNum=userNum;
		}

		///<summary></summary>
		public AlertRead Copy() {
			return (AlertRead)this.MemberwiseClone();
		}
	}
}
