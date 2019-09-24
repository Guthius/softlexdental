using System;
using System.Collections.Generic;

namespace OpenDentBusiness.X12
{
    public class X12ClaimMatch
    {
        public string ClaimIdentifier;
        public double ClaimFee;
        public string PatFname;
        public string PatLname;
        public string SubscriberId;
        public DateTime DateServiceStart;
        public DateTime DateServiceEnd;
        public long EtransNum;
        ///<summary>A reversal is a special type of supplemental payment.</summary>
        public bool Is835Reversal;
        ///<summary>All non-zero procedures found on the ERA.  Excludes procedures which could not be identified from the ERA.
        ///This list of procedures might be shorter than the list of procedures on the ERA claim.</summary>
        public List<Hx835_Proc> List835Procs = new List<Hx835_Proc>();
        ///<summary>Indicates Primary, Secondary, or Preauth.  Claim type from CLP segment element 02.</summary>
        public string CodeClp02 = "";
    }
}
