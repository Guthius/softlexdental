 using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Links one schedule block to one operatory.  So for a schedule block to show, it must be linked to one or more operatories.</summary>
	[Serializable]
	[ODTable(HasBatchWriteMethods=true,IsLargeTable=true)]
	public class ScheduleOp:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ScheduleOpNum;
		///<summary>FK to schedule.ScheduleNum.</summary>
		public long ScheduleNum;
		///<summary>FK to operatory.OperatoryNum.</summary>
		public long OperatoryNum;

		public ScheduleOp Copy(){
			return (ScheduleOp)this.MemberwiseClone();
		}

	
		
	}

	

	

}













