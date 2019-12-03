using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Essentially more columns in the patient table. 
    /// They are stored here because these fields can contain a lot of information, and we want to try to keep the size of the patient table a bit smaller.
    /// </summary>
    public class PatientNote
    {
        public long PatNum;

        ///<summary>Only one note per family stored with guarantor.</summary>
        public string FamFinancial;

        ///<summary>No longer used.</summary>
        public string ApptPhone;

        ///<summary>Medical Summary</summary>
        public string Medical;

        ///<summary>Service notes</summary>
        public string Service;

        ///<summary>Complete current Medical History</summary>
        public string MedicalComp;

        ///<summary>Shows in the Chart module just below the graphical tooth chart.</summary>
        public string Treatment;

        ///<summary>In Case of Emergency Name.</summary>
        public string ICEName;

        ///<summary>In Case of Emergency Phone.</summary>
        public string ICEPhone;

        ///<summary>-1 by default. Overrides the default number of months for an ortho treatment for this patient.
        ///Gets automatically set to the current value found in the pref OrthoClaimMonthsTreatment when the first placement procedure has been completed and this value is -1.
        ///This column is an integer instead of a byte because it needs to store -1 so that users can override with the value of 0.
        ///When set to -1 the default practice value for the pref OrthoClaimMonthsTreatment is used.</summary>
        public int OrthoMonthsTreatOverride = -1;

        ///<summary>Overrides the date of the first ortho procedure for this patient to use for ortho case patients. 
        ///If MinDate, then the date is derived by looking at the first ortho procedure for this patient.</summary>
        public DateTime DateOrthoPlacementOverride;

        public PatientNote Copy()
        {
            return (PatientNote)MemberwiseClone();
        }
    }
}
