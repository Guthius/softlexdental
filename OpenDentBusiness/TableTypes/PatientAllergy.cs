using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// An allergy attached to a patient and linked to an AllergyDef.
    /// </summary>
    public class PatientAllergy : DataRecord
    {
        public long AllergyId;
        public long PatientId;
        
        /// <summary>
        /// Adverse reaction description.
        /// </summary>
        public string Reaction;

        /// <summary>
        /// Snomed code for reaction. 
        /// Optional and independent of the Reaction text field.
        /// Not needed for reporting.  
        /// Only used for CCD export/import.
        /// </summary>
        public string SnomedReaction;

        /// <summary>
        /// To be used for synch with web server for CertTimelyAccess.
        /// </summary>
        public DateTime DateTStamp;
        
        /// <summary>
        /// The historical date that the patient had the adverse reaction to this agent.
        /// </summary>
        public DateTime DateAdverseReaction;
        
        /// <summary>
        /// True if still an active allergy.
        /// False helps hide it from the list of active allergies.
        /// </summary>
        public bool Active;
    }
}
