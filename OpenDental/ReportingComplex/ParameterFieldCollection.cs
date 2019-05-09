using System.Collections;

namespace OpenDental.ReportingComplex
{
    public class ParameterFieldCollection : CollectionBase
    {
        /// <summary>
        /// Returns the ParameterField with the given index.
        /// </summary>
        public ParameterField this[int index]
        {
            get => (ParameterField)List[index];
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Returns the ParameterDefinition with the given name.
        /// </summary>
        public ParameterField this[string name]
        {
            get
            {
                foreach (ParameterField parameterField in List)
                {
                    if (parameterField.Name == name)
                    {
                        return parameterField;
                    }
                }
                return null;
            }
        }

        public int Add(ParameterField value) => List.Add(value);

        public int IndexOf(ParameterField value) => List.IndexOf(value);

        public void Insert(int index, ParameterField value) => List.Insert(index, value);
    }
}