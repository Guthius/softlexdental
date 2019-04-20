using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Data;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class DatabaseMaintenance:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long DatabaseMaintenanceNum;
		///<summary>The name of the databasemaintenance name.</summary>
		public string MethodName;
		///<summary>Set to true to indicate that the method is hidden.</summary>
		public bool IsHidden;
		///<summary>Set to true to indicate that the method is old.</summary>
		public bool IsOld;
		///<summary>Updates the date and time they run the method.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateLastRun;

		///<summary></summary>
		public DatabaseMaintenance Copy() {
			return (DatabaseMaintenance)this.MemberwiseClone();
		}
	}

	///<summary></summary>
	public enum DbmMode {
		///<summary></summary>
		Check = 0,
		///<summary></summary>
		Breakdown = 1,
		///<summary></summary>
		Fix = 2
	}




    public interface IDatabaseMaintenanceCheck
    {
        /// <summary>
        /// Gets the name of the check.
        /// </summary>
        string Name { get;  }

        /// <summary>
        /// Gets a breakdown of the detected issues.
        /// </summary>
        string Breakdown { get; }

        /// <summary>
        /// Execute the database check.
        /// </summary>
        /// <returns>True if issues were detected; otherwise, false.</returns>
        bool Check();

        /// <summary>
        /// Attempt to resolve the detected issues.
        /// </summary>
        /// <returns>True if the issues were resolved; otherwise, false.</returns>
        bool Resolve(out string result);
    }

    public class DatabaseMaintenanceCheck : IDatabaseMaintenanceCheck
    {
        /// <summary>
        /// Gets the name of the check.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a breakdown of the detected issues.
        /// </summary>
        public string Breakdown => OnBreakdown() ?? "";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMaintenanceCheck"/> class.
        /// </summary>
        public DatabaseMaintenanceCheck() => Name = GetType().Name;

        /// <summary>
        /// Execute the database check.
        /// </summary>
        /// <returns>True if issues were detected; otherwise, false.</returns>
        public bool Check() => OnCheck();

        /// <summary>
        /// Attempt to resolve the detected issues.
        /// </summary>
        /// <returns>True if the issues were resolved; otherwise, false.</returns>
        public bool Resolve(out string result) => OnResolve(out result);

        /// <summary>
        /// Perform the check.
        /// </summary>
        /// <returns>True if issues were detected; otherwise, false.</returns>
        protected virtual bool OnCheck() => false;

        /// <summary>
        /// Resolve detected issues.
        /// </summary>
        /// <returns>True if the issues were resolved; otherwise, false.</returns>
        protected virtual bool OnResolve(out string result)
        {
            result = string.Empty;
            return true;
        }
        
        /// <summary>
        /// Try to get a breakdown of the issues.
        /// </summary>
        /// <returns>A breakdown of the issues.</returns>
        protected virtual string OnBreakdown() => string.Empty;
    }


    //class ClaimProcAttachedToPatientPaymentPlans : DatabaseMaintenanceCheck
    //{
    //    protected override bool OnCheck()
    //    {
    //        DataTable table = GetClaimProcsAttachedToPatientPaymentPlans();
    //
    //        return table.Rows.Count > 0;
    //    }
    //
    //    protected override bool OnResolve(out string result)
    //    {
    //        DataTable table = GetClaimProcsAttachedToPatientPaymentPlans();
    //
    //        result = "";
    //        if (table.Rows.Count > 0)
    //        {
    //            result = "Manual fix needed. Double click to see a break down.";
    //
    //            return false;
    //        }
    //
    //        return true;
    //    }
    //
    //    protected override string OnBreakdown()
    //    {
    //        DataTable table = GetClaimProcsAttachedToPatientPaymentPlans();
    //
    //        var log = "";
    //        if (table.Rows.Count > 0)
    //        {
    //            log = "ClaimProcs attached to insurance payment plans, including:\r\n";
    //            for (int i = 0; i < table.Rows.Count; i++)
    //            {
    //                log += 
    //                    "\r\n  Patient #" + table.Rows[i]["PatNum"].ToString() + " " +
    //                    "has a payment amount for" + " " + PIn.Double(table.Rows[i]["InsPayAmt"].ToString()).ToString("c") + " " +
    //                    "on date" + " " + PIn.Date(table.Rows[i]["DateCP"].ToString()).ToShortDateString() + " " +
    //                    "attached to patient payment plan #" + table.Rows[i]["PayPlanNum"];
    //            }
    //
    //            log += "\r\nRun 'Pay Plan Payments' in the Tools tab to fix these payments.";
    //        }
    //
    //        return log;
    //    }
    //}







    ///<summary>An attribute that should get applied to any method that needs to show up in the main grid of FormDatabaseMaintenance.
    ///Also, an attribute that identifies methods that require a userNum parameter for sending the current user through the middle tier to set the
    ///SecUserNumEntry field.</summary>
    [System.AttributeUsage(System.AttributeTargets.Method,AllowMultiple = false)]
	public class DbmMethodAttr:System.Attribute {
		private bool _hasBreakDown;
		private bool _hasPatNum;
		private bool _isCanada;

		///<summary>Set to true if this dbm method needs to be able to show the user a list or break down of items that need manual attention.</summary>
		public bool HasBreakDown {
			get {
				return _hasBreakDown;
			}
			set {
				_hasBreakDown=value;
			}
		}

		/////<summary>Not needed anymore. The usernum can be set from Security.CurUser.UserNum on both Middle Tier client and server (and direct connection).</summary>
		//public bool HasUserNum {
		//	get { return _hasUserNum; }
		//	set { _hasUserNum=value; }
		//}

		///<summary>Set to true if this dbm method needs to be able to run for a specific patient.</summary>
		public bool HasPatNum {
			get {
				return _hasPatNum;
			}
			set {
				_hasPatNum=value;
			}
		}

		///<summary>Set to true if this DBM is only for Canadian customers.</summary>
		public bool IsCanada {
			get {
				return _isCanada;
			}
			set {
				_isCanada=value;
			}
		}

		public DbmMethodAttr() {
			this._hasBreakDown=false;
			this._hasPatNum=false;
			this._isCanada=false;
		}

	}

	///<summary>Sorting class used to sort a MethodInfo list by Name.</summary>
	public class MethodInfoComparer:IComparer<MethodInfo> {

		public MethodInfoComparer() {
		}

		public int Compare(MethodInfo x,MethodInfo y) {
			return x.Name.CompareTo(y.Name);
		}
	}
}
