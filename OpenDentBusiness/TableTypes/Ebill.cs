using System;

namespace OpenDentBusiness
{
    ///<summary>Keeps track of account details of e-statements per clinic.</summary>
    public class Ebill
    {
        public long EbillNum;

        ///<summary>FK to clinic.ClinicNum</summary>
        public long ClinicNum;

        ///<summary>The account number for the e-statement client.</summary>
        public string ClientAcctNumber;

        ///<summary>The user name for this particular account.</summary>
        public string ElectUserName;

        ///<summary>The password for this particular account.</summary>
        public string ElectPassword;

        ///<summary></summary>
        public EbillAddress PracticeAddress;

        ///<summary></summary>
        public EbillAddress RemitAddress;

        public Ebill Copy()
        {
            return (Ebill)this.MemberwiseClone();
        }
    }

    public enum EbillAddress
    {
        PracticePhysical,
        PracticeBilling,
        PracticePayTo,
        ClinicPhysical,
        ClinicBilling,
        ClinicPayTo
    }
}
