using System;

namespace OpenDentBusiness
{
    public class Patient : IComparable
    {
        public long PatNum;

        /// <summary>
        /// The last name of the patient.
        /// </summary>
        public string LName;

        /// <summary>
        /// The first name of the patient.
        /// </summary>
        public string FName;

        /// <summary>
        /// The middle initial or name of the patient.
        /// </summary>
        public string MiddleI;

        /// <summary>
        /// The preferred name of the patient, aka nickname.
        /// </summary>
        public string Preferred;

        /// <summary>
        /// The status of the patient.
        /// </summary>
        public PatientStatus PatStatus;

        /// <summary>
        /// The patient gender.
        /// </summary>
        public PatientGender Gender;

        /// <summary>
        /// Enum:PatientPosition Marital status would probably be a better name for this column.
        /// </summary>
        public PatientPosition Position;

        /// <summary>
        /// The birth date of the patient.
        /// </summary>
        public DateTime Birthdate;

        /// <summary>
        ///     <para>
        ///         The SSN of the patient.
        ///     </para>
        ///     <para>
        ///         In the US, this is 9 digits, no dashes. For all other countries, any punctuation or format is allowed.
        ///     </para>
        /// </summary>
        public string SSN;

        /// <summary>
        /// The first address line of the patient.
        /// </summary>
        public string Address;

        /// <summary>
        /// The (optional) second address line of the patient.
        /// </summary>
        public string Address2;

        /// <summary>
        /// The city of the patient.
        /// </summary>
        public string City;

        /// <summary>
        ///     <para>
        ///         The state of the patient.
        ///     </para>
        ///     <para>
        ///         Stores the 2 character state code in USA. Stores province for Canadian users.
        ///     </para>
        /// </summary>
        public string State;

        /// <summary>
        ///     <para>
        ///         The postal code of the patient.
        ///     </para>
        ///     <para>
        ///         For Canadian claims, it must be ANANAN.  No validation gets done except there.
        ///     </para>
        /// </summary>
        public string Zip;

        /// <summary>
        /// The home phone number of the patient.
        /// </summary>
        public string HmPhone;

        /// <summary>
        /// The work phone number of the patient.
        /// </summary>
        public string WkPhone;

        /// <summary>
        /// </summary>
        public string WirelessPhone;

        /// <summary>FK to patient.PatNum.  Head of household.</summary>
        public long Guarantor;

        /// <summary>
        /// The age of the patient.
        /// </summary>
        private int _age; // TODO: Should be a property...

        /// <summary>
        /// Single char. Shows at upper right corner of appointments. 
        /// Suggested use is A,B,or C to designate creditworthiness, 
        /// but it can actually be used for any purpose.
        /// </summary>
        public string CreditType;

        /// <summary>
        /// The e-mail address of the patient.
        /// </summary>
        public string Email;

        /// <summary>Dear __.  This field does not include the "Dear" or a trailing comma.  If this field is blank, then the typical salutation is 
        /// FName.  Or, if a Preferred name is present, that is used instead of FName.</summary>
        public string Salutation;

        /// <summary>Current patient balance.(not family). Never subtracts insurance estimates.</summary>
        public double EstBalance;

        /// <summary>
        /// FK to provider.ProvNum.  The patient's primary provider.  Required.  
        /// </summary>
        public long PriProv;

        /// <summary>
        /// FK to provider.ProvNum.  Secondary provider (hygienist). Optional.
        /// </summary>
        public long SecProv;

        /// <summary>FK to feesched.FeeSchedNum.  Fee schedule for this patient.  Usually not used.  If missing, the practice default fee schedule is 
        /// used. If patient has insurance, then the fee schedule for the insplan is used.</summary>
        public long FeeSched;

        /// <summary>
        /// FK to definition.DefNum.  Must have a value, or the patient will not show on some reports.
        /// </summary>
        public long BillingType;

        /// <summary>
        /// Name of folder where images will be stored. Not editable for now.
        /// </summary>
        public string ImageFolder; // TODO: Should be a property: patient.LName + patient.FName + patient.PatNum;
        
        /// <summary>
        /// Address or phone note.  Unlimited length in order to handle data from other programs during a conversion.
        /// </summary>
        public string AddrNote;

        /// <summary>
        /// Family financial urgent note.  Only stored with guarantor, and shared for family.
        /// </summary>
        public string FamFinUrgNote;

        /// <summary>
        /// Individual patient note for Urgent medical.
        /// </summary>
        public string MedUrgNote;

        /// <summary>
        /// Individual patient note for Appointment module note.
        /// </summary>
        public string ApptModNote;

        /// <summary>Single char.  Nonstudent='N' or blank, Parttime='P', Fulltime='F'.</summary>
        public string StudentStatus;

        /// <summary>
        /// College name. 
        /// If Canadian, then this is field C10 and must be filled if C9 (patient.CanadianEligibilityCode) is 1 and patient is 18 or older.
        /// </summary>
        public string SchoolName;

        /// <summary>
        ///     <para>
        ///         The chart number of the patient.
        ///     </para>
        ///     <para>
        ///         Typically filled only when the patient was imported from another system, in 
        ///         which case chart number will hold the ID of this patient in that system.
        ///     </para>
        /// </summary>
        public string ChartNumber;

        /// <summary>
        /// Optional. The Medicaid ID for this patient.
        /// </summary>
        public string MedicaidID;

        /// <summary>
        /// Aged balance from 0 to 30 days old. Aging numbers are for entire family. 
        /// Only stored with guarantor.
        /// </summary>
        public double Bal_0_30;

        /// <summary>
        /// Aged balance from 31 to 60 days old. Aging numbers are for entire family. 
        /// Only stored with guarantor.
        /// </summary>
        public double Bal_31_60;

        /// <summary>
        /// Aged balance from 61 to 90 days old. Aging numbers are for entire family. 
        /// Only stored with guarantor.
        /// </summary>
        public double Bal_61_90;

        /// <summary>
        /// Aged balance over 90 days old. Aging numbers are for entire family. 
        /// Only stored with guarantor.
        /// </summary>
        public double BalOver90;

        /// <summary>
        /// Insurance Estimate for entire family.
        /// </summary>
        public double InsEst;

        /// <summary>
        /// Total balance for entire family before insurance estimate. 
        /// Not the same as the sum of the 4 aging balances because this can be negative. 
        /// Only stored with guarantor.
        /// </summary>
        public double BalTotal;

        /// <summary>
        /// FK to employer.EmployerNum.
        /// </summary>
        public long EmployerNum;

        /// <summary>
        /// Not used since version 2.8.
        /// </summary>
        public string EmploymentNote;

        /// <summary>
        /// FK to county.CountyName, although it will not crash if key absent.
        /// </summary>
        public string County;

        /// <summary>
        /// Enum:PatientGrade Gradelevel.
        /// </summary>
        public PatientGrade GradeLevel;

        /// <summary>
        /// Enum:TreatmentUrgency Used in public health screenings.
        /// </summary>
        public TreatmentUrgency Urgency;

        /// <summary>
        /// The date that the patient first visited the office. Automated.
        /// </summary>
        public DateTime DateFirstVisit;

        /// <summary>
        /// FK to clinic.ClinicNum. Can be zero if not attached to a clinic or no clinics set up.
        /// </summary>
        public long ClinicNum;

        /// <summary>
        /// For now, an 'I' indicates that the patient has insurance. 
        /// This is only used when displaying appointments. It will later be expanded. User can't edit.
        /// </summary>
        public string HasIns;

        /// <summary>
        /// The Trophy bridge is inadequate, this attempts to make it usable for offices that have invested in Trophy hardware.
        /// </summary>
        public string TrophyFolder;

        /// <summary>
        /// This simply indicates whether the 'done' box is checked in the chart module.
        /// Used to be handled as a -1 in the NextAptNum field, but now that field is unsigned.
        /// </summary>
        public bool PlannedIsDone;

        /// <summary>
        /// Set to true if patient needs to be premedicated for appointments, includes PAC, halcion, etc.
        /// </summary>
        public bool Premed;

        /// <summary>
        /// Only used in hospitals.
        /// </summary>
        public string Ward;

        /// <summary>
        /// Enum:ContactMethod Used for eCR, which includes eReminders eConfirmations.
        /// </summary>
        public ContactMethod PreferConfirmMethod;

        /// <summary>
        /// Enum:ContactMethod
        /// </summary>
        public ContactMethod PreferContactMethod;

        /// <summary>
        /// Enum:ContactMethod
        /// </summary>
        public ContactMethod PreferRecallMethod;

        /// <summary>.</summary>
        public TimeSpan SchedBeforeTime;

        /// <summary>.</summary>
        public TimeSpan SchedAfterTime;

        /// <summary>
        /// We do not use this, but some users do, so here it is. 0=none. Otherwise, 1-7 for day.
        /// </summary>
        public byte SchedDayOfWeek;

        /// <summary>
        /// The primary language of the patient. Typically eng (English), fra (French), 
        /// spa (Spanish), or similar. If it's a custom language, then it might look like Tahitian.
        /// </summary>
        public string Language;

        /// <summary>
        /// Used in hospitals. It can be before the first visit date. It typically gets set 
        /// automatically by the hospital system.
        /// </summary>
        public DateTime AdmitDate;

        /// <summary>
        /// Includes any punctuation.  For example, Mr., Mrs., Miss, Dr., etc.  
        /// There is no selection mechanism yet for user; they must simply type it in.
        /// </summary>
        public string Title;

        /// <summary>
        /// Amount "due now" for all payment plans such that someone in this family is the payment plan guarantor.  
        /// This is the total of all payment plan charges past due (taking into account the PayPlansBillInAdvanceDays setting) subtract the amount 
        /// already paid for the payment plans.  Only stored with family guarantor.
        /// </summary>
        public double PayPlanDue;

        /// <summary>
        /// FK to site.SiteNum. Can be zero. Replaces the old GradeSchool field with a proper foreign key.
        /// </summary>
        public long SiteNum;

        /// <summary>
        /// Automatically updated by MySQL every time a row is added or changed. Could be changed due to user editing,
        /// custom queries or program updates.  Not user editable.
        /// </summary>
        public DateTime DateTStamp;

        /// <summary>
        /// FK to patient.PatNum. Can be zero. 
        /// Person responsible for medical decisions rather than finances. 
        /// Guarantor is still responsible for finances. 
        /// This is useful for nursing home residents. 
        /// Part of public health.
        /// </summary>
        public long ResponsParty;

        /// <summary>
        /// C09.  Eligibility Exception Code.  
        /// A number between 1-4. 0 is not acceptable for e-claims.
        /// 1=FT student, 2=disabled, 3=disabled student, 4=code not applicable. Warning. 
        /// 4 is a 0 if using CDAnet version 02. 
        /// This column should have been created as an int.
        /// </summary>
        public byte CanadianEligibilityCode;

        /// <summary>
        /// Number of minutes patient is asked to come early to appointments.
        /// </summary>
        public int AskToArriveEarly;

        /// <summary>
        /// Enum:ContactMethod  Used for EHR.
        /// </summary>
        public ContactMethod PreferContactConfidential;

        /// <summary>
        /// FK to patient.PatNum.  If this is the same as PatNum, then this is a SuperHead.  If zero, then not part of a superfamily.  
        /// Synched for entire family.  If family is part of a superfamily, then the guarantor for this family will show in the superfamily list in the
        /// Family module for anyone else who is in the superfamily.  Only a guarantor can be a superfamily head.
        /// </summary>
        public long SuperFamily;

        /// <summary>
        /// Enum:YN
        /// </summary>
        public YN TxtMsgOk;

        /// <summary>
        /// EHR smoking status as a SNOMED code.  Will always be the most recent smoking status for the patient.
        /// </summary>
        public string SmokingSnoMed;

        /// <summary>
        /// Country name.  Only used by HQ to add country names to statements.
        /// </summary>
        public string Country;

        ///<summary>Needed for EHR syndromic surveillance messaging.  Used in HL7 PID-29.  Also for feature request #3040.  Date and time because we 
        ///need precision to the minute in syndromic surveillence messging.</summary>
        public DateTime DateTimeDeceased;

        /// <summary>
        /// A number between 1 and 31 that is the day of month that repeat charges should be applied to this account. 
        /// Previously this was determined by the start date of the repeate charges.
        /// </summary>
        public int BillingCycleDay;

        /// <summary>
        /// FK to userod.UserNum.  Set to the user logged in when the row was inserted at SecDateEntry date and time.
        /// </summary>
        public long SecUserNumEntry;

        /// <summary>
        /// Timestamp automatically generated and user not allowed to change.  he actual date of entry.
        /// </summary>
        public DateTime SecDateEntry;

        /// <summary>
        /// 0 by default.  If true, this guarantor should be included in superbilling statements.
        /// </summary>
        public bool HasSuperBilling;

        /// <summary>
        /// Deprecated, use patientlink table instead.
        /// Indicates if this patient should act as a clone of another patient. 
        /// Previously, ortho cloned patients were signified by capitalizing the name fields of the newly cloned patient.
        /// This field will allow for an explicit flag to be set to indicate cloned status.
        /// </summary>
        public long PatNumCloneFrom;

        /// <summary>
        /// FK to discountplan.DiscountPlanNum. Will be 0 if there is no DiscountPlan
        /// </summary>
        public long DiscountPlanNum;

        /////<summary>Decided not to add since this data is already available and synchronizing would take too much time.  Will add later.  
        /////Not editable. If the patient happens to have a future appointment, this will contain the date of that appointment.  
        /////Once appointment is set complete, this date is deleted.  If there is more than one appointment scheduled, this will only contain the 
        /////earliest one.  Used mostly to exclude patients from recall lists.  If you want all future appointments, use Appointments.GetForPat() 
        /////instead. You can loop through that list and exclude appointments with dates earlier than today.</summary>
        //public DateTime DateScheduled;

        public Patient()
        {
            BillingCycleDay = 1;
        }

        public Patient Copy()
        {
            return (Patient)MemberwiseClone();
        }

        /// <summary>
        /// Gets the age of the patient.
        /// </summary>
        public int Age
        {
            get
            {
                _age = Patients.DateToAge(Birthdate);
                return _age;
            }
        }

        /// <summary>
        /// Returns a string representation of the patient.
        /// </summary>
        public override string ToString() => "Patient: " + GetNameLF();

        public string GetNameLF()
        {
            return Patients.GetNameLF(LName, FName, Preferred, MiddleI);
        }

        ///<summary>LName, FName M</summary>
        public string GetNameLFnoPref()
        {
            return Patients.GetNameLFnoPref(LName, FName, MiddleI);
        }

        ///<summary>FName 'Preferred' M LName</summary>
        public string GetNameFL()
        {
            return Patients.GetNameFL(LName, FName, Preferred, MiddleI);
        }

        ///<summary>FName M LName</summary>
        public string GetNameFLnoPref()
        {
            return Patients.GetNameFLnoPref(LName, FName, MiddleI);
        }

        ///<summary>FName/Preferred LName</summary>
        public string GetNameFirstOrPrefL()
        {
            return Patients.GetNameFirstOrPrefL(LName, FName, Preferred);
        }

        ///<summary>FName/Preferred M. LName</summary>
        public string GetNameFirstOrPrefML()
        {
            return Patients.GetNameFirstOrPrefML(LName, FName, Preferred, MiddleI);
        }

        ///<summary>Title FName M LName</summary>
        public string GetNameFLFormal()
        {
            return Patients.GetNameFLFormal(LName, FName, MiddleI, Title);
        }

        ///<summary>Includes preferred.</summary>
        public string GetNameFirst()
        {
            return Patients.GetNameFirst(FName, Preferred);
        }

        public string GetNameFirstOrPreferred()
        {
            return Patients.GetNameFirstOrPreferred(FName, Preferred);
        }

        public string GetNameFirstOrPreferredOrLast()
        {
            return Patients.GetNameFirstOrPreferredOrLast(FName, Preferred, LName);
        }

        ///<summary>Dear __.  Does not include the "Dear" or the comma.</summary>
        public string GetSalutation()
        {
            return Patients.GetSalutation(Salutation, Preferred, FName);
        }

        /// <summary>
        /// Useful for sorting and binary searching. 
        /// The X12 834 implementation uses this for binary searching to improve efficiency.
        /// If this function is changed in the future, it will heavily impact our X12 834 implementation. 
        /// Be cautious. In the end, this function will probably not need to change anyway, since it will only be 
        /// used for comparing patients when the PatNums are not known.
        /// </summary>
        public int CompareTo(object patOther)
        {
            Patient p1 = this;
            Patient p2 = (Patient)patOther;
            string lname1 = (p1.LName == null) ? "" : p1.LName;
            string lname2 = (p2.LName == null) ? "" : p2.LName;
            int comp = lname1.Trim().ToLower().CompareTo(lname2.Trim().ToLower());
            if (comp != 0)
            {
                return comp;
            }
            string fname1 = (p1.FName == null) ? "" : p1.FName;
            string fname2 = (p2.FName == null) ? "" : p2.FName;

            comp = fname1.Trim().ToLower().CompareTo(fname2.Trim().ToLower());
            if (comp != 0)
            {
                return comp;
            }

            return p1.Birthdate.Date.CompareTo(p2.Birthdate.Date);
        }
    }
}
