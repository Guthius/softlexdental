namespace OpenDentBusiness
{
    /// <summary>
    /// Keeps track of the last appointment view used on a per user basis. 
    /// Users can have multiple rows in this table when using clinics.
    /// </summary>
    public class UserodApptView 
    {
        public long UserodApptViewNum;

        ///<summary>FK to userod.UserNum.</summary>
        public long UserNum;

        ///<summary>FK to clinic.ClinicNum.  0 if clinics is not being used or if the user has not been assigned a clinic.</summary>
        public long ClinicNum;

        ///<summary>FK to apptview.ApptViewNum.</summary>
        public long ApptViewNum;
    }
}