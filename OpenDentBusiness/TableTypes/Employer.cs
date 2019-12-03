namespace OpenDentBusiness
{
    /// <summary>
    /// Most insurance plans are organized by employer.
    /// This table keeps track of the list of employers. 
    /// The address fields were added at one point, but I don't know why they don't show in the program in order to edit.
    /// Nobody has noticed their absence even though it's been a few years, so for now we are just using the EmpName and not the address.
    /// </summary>
    public class Employer
    {
        public long EmployerNum;

        /// <summary>
        /// The name of the employer.
        /// </summary>
        public string EmpName;

        /// <summary>
        /// The first address line of the employer.
        /// </summary>
        public string Address;

        /// <summary>
        /// The second address line of the employer.
        /// </summary>
        public string Address2;

        /// <summary>
        /// The city of the employer.
        /// </summary>
        public string City;

        public string State;
        public string Zip;
        public string Phone;

        public Employer Copy()
        {
            return (Employer)MemberwiseClone();
        }
    }
}
