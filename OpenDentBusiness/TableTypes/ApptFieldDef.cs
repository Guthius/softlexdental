namespace OpenDentBusiness
{
    /// <summary>
    /// These are the definitions for the custom patient fields added and managed by the user.
    /// </summary>
    public class ApptFieldDef : DataRecord
    {
        /// <summary>
        /// The name of the field that the user will be allowed to fill in the appt edit window.  Duplicates are prevented.
        /// </summary>
        public string FieldName;

        /// <summary>
        /// Enum:ApptFieldType Text=0,PickList=1
        /// </summary>
        public ApptFieldType FieldType;

        /// <summary>
        /// The text that contains pick list values.  Length 4000.
        /// </summary>
        public string PickList;

        public ApptFieldDef Clone()
        {
            return (ApptFieldDef)this.MemberwiseClone();
        }
    }

    public enum ApptFieldType
    {
        Text,
        PickList
    }
}
