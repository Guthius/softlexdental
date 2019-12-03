namespace OpenDentBusiness
{
    ///<summary>Entries in this table will represent procedurecodes that the insurance plan wants to SKIP when considering substitution codes.</summary>
    public class SubstitutionLink
    {
        public long SubstitutionLinkNum;

        ///<summary>FK to insplan.PlanNum.</summary>
        public long PlanNum;

        ///<summary>FK to procedurecode.CodeNum.</summary>
        public long CodeNum;

        ///<summary>FK to procedurecode.ProcCode.</summary>
        public string SubstitutionCode;

        ///<summary>Enum:SubstitutionCondition </summary>
        public SubstitutionCondition SubstOnlyIf;
    }
}
