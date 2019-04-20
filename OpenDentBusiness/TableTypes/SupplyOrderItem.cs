using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>One item on one supply order.  This table links supplies to orders as well as storing a small amount of additional info.</summary>
	[Serializable()]
	public class SupplyOrderItem : ODTable {
		/// <summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long SupplyOrderItemNum;
		/// <summary>FK to supplyorder.supplyOrderNum.</summary>
		public long SupplyOrderNum;
		/// <summary>FK to supply.SupplyNum.</summary>
		public long SupplyNum;
		/// <summary>How many were ordered.</summary>
		public int Qty;
		/// <summary>Price per unit on this order.</summary>
		public double Price;

			
	}

	

}









