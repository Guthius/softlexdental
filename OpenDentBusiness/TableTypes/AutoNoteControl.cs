namespace OpenDentBusiness
{
    /// <summary>
    /// In the program, this is now called an autonote prompt.
    /// </summary>
    public class AutoNoteControl : DataRecord
    {
        public string Description;

        /// <summary>
        /// 'Text', 'OneResponse', or 'MultiResponse'.  More types to be added later.
        /// </summary>
        public string ControlType;

        /// <summary>The prompt text.</summary>
        public string ControlLabel;

        /// <summary>
        /// For TextBox, this is the default text.
        /// For a ComboBox, this is the list of possible responses, one per line.
        /// </summary>
        public string ControlOptions;

        public AutoNoteControl Copy()
        {
            return (AutoNoteControl)MemberwiseClone();
        }
    }
}
