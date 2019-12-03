namespace OpenDentBusiness
{
    /// <summary>
    /// A vaccine definition.  Should not be altered once linked to VaccinePat.
    /// </summary>
    public class VaccineDef
    {
        public long VaccineDefNum;

        ///<summary>RXA-5-1.</summary>
        public string CVXCode;

        ///<summary>Name of vaccine.  RXA-5-2.</summary>
        public string VaccineName;

        ///<summary>FK to drugmanufacturer.DrugManufacturerNum.</summary>
        public long DrugManufacturerNum;
    }
}
