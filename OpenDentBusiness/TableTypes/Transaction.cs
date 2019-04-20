using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Used in the accounting section of the program.  Each row is one transaction in the ledger, and must always have at least two splits.  All splits must always add up to zero.</summary>
	[Serializable]
	public class Transaction:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long TransactionNum;
		///<summary>Not user editable.  Server time.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>FK to userod.UserNum. The user that entered this transaction.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.ExcludeFromUpdate)]
		public long UserNum;
		///<summary>FK to deposit.DepositNum.  Will eventually be replaced by a source document table, and deposits will just be one of many types.</summary>
		public long DepositNum;
		///<summary>FK to payment.PayNum.  Like DepositNum, it will eventually be replaced by a source document table, and payments will just be one of many types.</summary>
		public long PayNum;
		///<summary>FK to userod.UserNum. The user who last edited this transaction.</summary>
		public long SecUserNumEdit;
		///<summary>The last time this transaction was edited.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime SecDateTEdit;

		///<summary></summary>
		public Transaction Copy() {
			return (Transaction)MemberwiseClone();
		}


	}
}