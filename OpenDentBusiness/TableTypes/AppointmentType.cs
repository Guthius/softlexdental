using System.Drawing;

namespace OpenDentBusiness
{
    /// <summary>
    /// Appointment type is used to override appointment color. 
    /// Might control other properties on appointments in the future.
    /// </summary>
    public class AppointmentType : DataRecord
    {
        public string Name;

        public Color Color;

        /// <summary>
        /// <para>Time pattern, 'X' for doctor time, '/' for assist time.</para>
        /// <para>
        /// Stored in 5 minute increments. Convert as needed to 10 or 15 minute representations for
        /// display. Will be blank if the pattern should be dynamically calculated via the 
        /// procedures specified by <see cref="ProcedureCodes"/>.
        /// </para>
        /// </summary>
        public string Pattern;

        /// <summary>
        /// Comma delimited list of procedure codes.  E.g. T1234,T4321,N3214
        /// </summary>
        public string ProcedureCodes;

        public int SortOrder;

        public bool Hidden;

        public AppointmentType Copy()
        {
            return (AppointmentType)MemberwiseClone();
        }
    }
}
