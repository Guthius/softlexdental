namespace OpenDentBusiness
{
    /// <summary>
    /// Rx definitions. Can safely delete or alter, because they get copied to the rxPat table, not referenced.
    /// </summary>
    public class RxDef
    {
        public long RxDefNum;
        
        /// <summary>
        /// The name of the drug.
        /// </summary>
        public string Drug;
        
        /// <summary>
        /// Directions intended for the pharmacist.
        /// </summary>
        public string Sig;
        
        /// <summary>
        /// Amount to dispense.
        /// </summary>
        public string Disp;
        
        /// <summary>
        /// Number of refills.
        /// </summary>
        public string Refills;
        
        /// <summary>
        /// Notes about this drug. Will not be copied to the rxpat.
        /// </summary>
        public string Notes;
        
        /// <summary>
        /// Is a controlled substance. This will affect the way it prints.
        /// </summary>
        public bool IsControlled;
        
        /// <summary>
        /// RxNorm Code identifier. Copied down into medicationpat.RxCui (medical order) when a prescription is written.
        /// </summary>
        public string RxCui;
        
        /// <summary>
        /// If true will require procedure be attached to this prescription when printed.  Usually true if IsControlled is true.
        /// </summary>
        public bool IsProcRequired;
        
        /// <summary>
        /// Directions intended for the patient.
        /// </summary>
        public string PatientInstruction;

        public RxDef Copy() => (RxDef)MemberwiseClone();
    }
}