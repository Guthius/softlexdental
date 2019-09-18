namespace OpenDentBusiness
{
    /// <summary>
    /// A single autonote template.
    /// </summary>
    public class AutoNote : DataRecord
    {
        public string Name;
        public string MainText;

        /// <summary>
        /// FK to definition.DefNum.
        /// This is the AutoNoteCat definition category (DefCat=41), for categorizing autonotes.
        /// Uncategorized autonotes will be set to null.
        /// </summary>
        public long? CategoryId;

        public AutoNote Copy()
        {
            return (AutoNote)this.MemberwiseClone();
        }
    }
}
