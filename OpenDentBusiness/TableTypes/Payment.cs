using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>A patient payment.  Always has at least one split.</summary>
	[Serializable]
	[ODTable(IsSecurityStamped=true)]
	public class Payment:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long PayNum;
		///<summary>FK to definition.DefNum.  This will be 0 if this is an income transfer to another provider.</summary>
		public long PayType;
		///<summary>The date that the payment displays on the patient account.</summary>
		public DateTime PayDate;
		///<summary>Amount of the payment.  Must equal the sum of the splits.</summary>
		public double PayAmt;
		///<summary>Check number is optional.</summary>
		public string CheckNum;
		///<summary>Bank-branch for checks.</summary>
		public string BankBranch;
		///<summary>Any admin note.  Not for patient to see.  Length 4000.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob | CrudSpecialColType.CleanText)]
		public string PayNote;
		///<summary>No longer used.  Set to true to indicate that a payment has more than one paysplit.</summary>
		public bool IsSplit;
		///<summary>FK to patient.PatNum.  The patient where the payment entry will show.  But only the splits affect account balances.  This has a value even if the 'payment' is actually an income transfer to another provider.</summary>
		public long PatNum;
		///<summary>FK to clinic.ClinicNum.  Can be 0 to indicate no clinic (unassigned). Copied from patient.ClinicNum when creating payment, but user can override.  Not used in provider income transfers.  Cannot be used in financial reporting when grouping by clinic, because payments may be split between clinics.</summary>
		public long ClinicNum;
		///<summary>The date that this payment was entered.  Not user editable.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.DateEntry)]
		public DateTime DateEntry;
		///<summary>FK to deposit.DepositNum.  0 if not attached to any deposits.  Cash does not usually get attached to a deposit; only checks.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.ExcludeFromUpdate)]
		public long DepositNum;
		///<summary>Text of printed receipt if the payment was done electronically. Allows reprinting if needed.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Receipt;
		///<summary>True if this was an automatically added recurring CC charge rather then one entered by the user.  This was set to true for all historical entries before version 11.1, but will be accurate after that.</summary>
		public bool IsRecurringCC;
		///<summary>FK to userod.UserNum.  Set to the user logged in when the row was inserted at SecDateEntry date and time.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.ExcludeFromUpdate)]
		public long SecUserNumEntry;
		//No SecDateEntry, DateEntry already exists and is set by MySQL when the row is inserted and never updated
		///<summary>Automatically updated by MySQL every time a row is added or changed. Could be changed due to user editing, custom queries or program
		///updates.  Not user editable with the UI.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime SecDateTEdit;
		///<summary>Enum:CreditCardSource Indicates the origin of the payment if the payment came from a credit card. Will be 'None' if this payment 
		///did not use a credit card.</summary>
		public CreditCardSource PaymentSource;
		///<summary>Enum:ProcessStat Flags whether a payment came from online and needs to be processed.</summary>
		public ProcessStat ProcessStatus;
		///<summary>The date of the recurring charge that this payment applies to.</summary>
		public DateTime RecurringChargeDate;

		public Payment Clone() {
			return (Payment)this.MemberwiseClone();
		}
		

	}

	public enum ProcessStat {
			/// <summary>0 - Payment made within the OD program.</summary>
			OfficeProcessed,
			/// <summary>1 - Payment made from the Patient Portal and has been processed within OD.</summary>
			OnlineProcessed,
			/// <summary>2 - Payment made from the Patient Portal and needs to be processed within OD.</summary>
			OnlinePending
	}

	public enum PayClinicSetting {
		///<summary>0 - Defaults payments clinic to the current selected clinic.</summary>
		SelectedClinic,
		///<summary>1 - Defaults payments clinic to the patient's default clinic.</summary>
		PatientDefaultClinic,
		///<summary>2 - Defaults payments clinic to the current selected clinic except if HQ or ClinicNum=0, then use the patients default clinic.</summary>
		SelectedExceptHQ
	}



}










