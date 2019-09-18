namespace OpenDentBusiness
{
    public class ApptField : DataRecord
    {
        public long AppointmentId;

        ///<summary>FK to apptfielddef.FieldName.  The full name is shown here for ease of use when running queries.  But the user is only allowed to change fieldNames in the patFieldDef setup window.</summary>
        public string FieldName;

        ///<summary>Any text that the user types in.  Will later allow some automation.</summary>
        public string FieldValue;
    }
}
