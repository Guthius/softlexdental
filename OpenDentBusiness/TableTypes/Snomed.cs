namespace OpenDentBusiness
{
    /// <summary>
    /// We do not import synonyms, only "Fully Specified Name records".
    /// Snomed for holding a large list of codes. Codes in use are copied into the DiseaseDef table.
    /// SNOMED CT maintained, owned and copyright International Health Terminology Standards 
    /// Development Organisation (IHTSDO).
    /// </summary>
    public class Snomed
    {
        public long SnomedNum;
        
        /// <summary>
        /// Used as FK by other tables. Also called the Concept ID.
        /// Not allowed to edit this column once saved in the database.
        /// </summary>
        public string SnomedCode;
        
        /// <summary>
        /// Also called "Term", "Name", or "Fully Specified Name".
        /// Not editable and doesn't change.
        /// </summary>
        public string Description;
    }
}