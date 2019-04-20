using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>A subscription of one user to either a tasklist or to a task.</summary>
	[Serializable()]
	public class TaskSubscription : ODTable {
		/// <summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long TaskSubscriptionNum;
		/// <summary>FK to userod.UserNum</summary>
		public long UserNum;
		/// <summary>FK to tasklist.TaskListNum  When this is not 0 then TaskNum will be 0.</summary>
		public long TaskListNum;
		/// <summary>FK to task.TaskNum.  When this is not 0 then TaskListNum will be 0.</summary>
		public long TaskNum;

		
		public TaskSubscription Copy() {
			return (TaskSubscription)MemberwiseClone();
		}
		
		

			
	}

	

}









