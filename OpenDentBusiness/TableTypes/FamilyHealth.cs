namespace OpenDentBusiness
{
    /// <summary>
    /// For EHR, this lets us record medical problems for family members.
    /// These family members will usually not be in our database, and they are just recorded by relationship.
    /// </summary>
    public class FamilyHealth 
    {
        public long FamilyHealthNum;

        ///<summary>FK to patient.PatNum.</summary>
        public long PatNum;

        ///<summary>Enum:FamilyRelationship </summary>
        public FamilyRelationship Relationship;

        ///<summary>FK to diseasedef.DiseaseDefNum, which will have a SnoMed associated with it.</summary>
        public long DiseaseDefNum;

        ///<summary>Name of the family member.</summary>
        public string PersonName;
    }

    public enum FamilyRelationship
    {
        Parent,
        Sibling,
        Offspring,
    }
}
