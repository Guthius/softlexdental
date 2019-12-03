namespace OpenDentBusiness
{
    /// <summary>
    /// Spell check custom dictionary, shared by the whole office.
    /// </summary>
    public class DictCustom
    {
        // TODO: Is this being used?

        public long DictCustomNum;

        /// <summary>No space or punctuation allowed.</summary>
        public string WordText;

        public DictCustom Copy()
        {
            return (DictCustom)MemberwiseClone();
        }
    }
}
