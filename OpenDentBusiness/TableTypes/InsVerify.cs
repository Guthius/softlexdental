using System;

namespace OpenDentBusiness
{
    public class InsVerify
    {
        public long InsVerifyNum;

        ///<summary>The date of the last successful verification.</summary>
        public DateTime DateLastVerified;

        ///<summary>FK to userod.UserNum.  This is the assigned user for this verification.</summary>
        public long UserNum;

        ///<summary>Enum:VerifyTypes The type of verification.</summary>
        public VerifyTypes VerifyType;

        ///<summary>Foreign key to any table defined in the VerifyType Enumeration.</summary>
        public long FKey;

        ///<summary>FK to definition.DefNum.  Links to the category InsVerifyStatus</summary>
        public long DefNum;

        ///<summary>The date of the last assignment of this verification.</summary>
        public DateTime DateLastAssigned;

        ///<summary>Note for this insurance verification.</summary>
        public string Note;

        ///<summary>DateTime the row was added.</summary>
        public DateTime DateTimeEntry;

        ///<summary>Number of hours that were available from the time the insurance needed verified to the date of the appointment.
        ///Includes minutes if applicable.</summary>
        public double HoursAvailableForVerification;

        ///<summary>Not a database column.</summary>
        public long PatNum;

        ///<summary>Not a database column.</summary>
        public long PlanNum;

        ///<summary>Not a database column.</summary>
        public long PatPlanNum;

        ///<summary>Not a database column.</summary>
        public string ClinicName;

        ///<summary>Not a database column.</summary>
        public string PatientName;

        ///<summary>Not a database column.</summary>
        public string CarrierName;

        ///<summary>Not a database column.</summary>
        public DateTime AppointmentDateTime;

        ///<summary>Not a database column.</summary>
        public long AptNum;

        public InsVerify Clone()
        {
            return (InsVerify)this.MemberwiseClone();
        }
    }

    public class InsVerifyGridObject
    {
        public InsVerifyGridObject(InsVerify pat = null, InsVerify plan = null)
        {
            if (pat != null)
            {
                PatInsVerify = pat;
            }
            if (plan != null)
            {
                PlanInsVerify = plan;
            }
        }

        public InsVerify PatInsVerify;
        public InsVerify PlanInsVerify;

        /// <summary>Return the PatInsVerify.PatPlanNum if PatInsVerify is not null, the PlanInsVerify.PatPlanNum if PlanInsVerify isn't null, else 0.</summary>
        public long GetPatPlanNum()
        {
            if (PatInsVerify != null)
            {
                return PatInsVerify.PatPlanNum;
            }
            else if (PlanInsVerify != null)
            {
                return PlanInsVerify.PatPlanNum;
            }
            return 0;
        }

        public long GetPatNum()
        {
            if (PatInsVerify != null)
            {
                return PatInsVerify.PatNum;
            }
            else if (PlanInsVerify != null)
            {
                return PlanInsVerify.PatNum;
            }
            return 0;
        }

        public bool IsPatAndInsRow()
        {
            if (PlanInsVerify != null && PatInsVerify != null)
            {
                return true;
            }
            return false;
        }

        public bool IsOnlyPatRow()
        {
            if (PlanInsVerify == null && PatInsVerify != null)
            {
                return true;
            }
            return false;
        }

        public bool IsOnlyInsRow()
        {
            if (PlanInsVerify != null && PatInsVerify == null)
            {
                return true;
            }
            return false;
        }
    }

    public enum VerifyTypes
    {
        ///<summary>0.  This means FKey should be 0.</summary>
        None,
        ///<summary>1.  This means FKey will link to insplan.InsPlanNum</summary>
        InsuranceBenefit,
        ///<summary>2.  This means FKey will link to patplan.PatPlanNum</summary>
        PatientEnrollment
    }
}
