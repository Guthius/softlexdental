using OpenDentBusiness;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace OpenDental.ReportingComplex
{
    /// <summary>
    /// Holds information about a parameter used in the report.
    /// </summary>
    /// <remarks>
    /// A parameter is a string that can be used in a query that will be replaced by user-provided data before the query is sent.
    /// For instance, "?date1" might be replaced with "(ProcDate = '2004-02-17' OR ProcDate = '2004-02-18')".
    /// The output value can be multiple items connected with OR's as in the example, or it can be a single value.
    /// The Snippet represents one of the multiple values.  In this example, it would be "ProcDate = '?'".
    /// The ? in the Snippet will be replaced by the values provided by the user.
    /// </remarks>
    public class ParameterField
    {
        /// <summary>
        /// This is the name as it will show in the query, but without the preceding question mark.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The value, in text form, as it will be substituted into the main query and sent to the database.
        /// </summary>
        public string OutputValue { get; set; }
        
        /// <summary>
        /// The type of value that the parameter can accept.
        /// </summary>
        public FieldValueType ValueType { get; set; }
        
        /// <summary>
        /// The values of the parameter, typically just one. Each value can be a string, date, number, currency, Boolean, etc.
        /// If the length of the ArrayList is 0, then the value is blank and will not be used in the query.
        /// </summary>
        public ArrayList CurrentValues { get; set; }
        
        /// <summary>
        /// These values are stored between sessions in the database and will show in the dialog prefilled when it asks the user for values.
        /// The length of the ArrayList can be 0 to specify that the initial value is blank.
        /// </summary>
        public ArrayList DefaultValues { get; set; }
        
        /// <summary>
        /// The text that prompts the user what to enter for this parameter.</summary>
        public string PromptingText { get; set; }
        
        /// <summary>
        /// The snippet of SQL that will be repeated once for each value supplied by the user, connected with OR's, and surrounded by parentheses.
        /// </summary>
        public string Snippet { get; set; }
        
        /// <summary>
        /// If the ValueKind is EnumField, then this specifies which type of enum. It is the string name of the type.
        /// </summary>
        public EnumType EnumerationType { get; set; }
        
        ///<summary>If ValueKind is DefParameter, then this specifies which DefCat.
        ///</summary>
        public DefinitionCategory DefCategory { get; set; }
        
        ///<summary>If ValueKind is ForeignKey, then this specifies which one.
        ///</summary>
        public ReportFKType FKeyType { get; set; }

        /// <summary>
        /// Default constructor. Used when retrieving data from db.
        /// </summary>
        public ParameterField()
        {
        }

        /// <summary>
        /// This is how parameters are generally added. 
        /// The currentValues and outputValue will be determined during the Report.SubmitQuery call.
        /// </summary>
        public ParameterField(string thisName, FieldValueType thisValueType, object thisDefaultValue, string thisPromptingText, string thisSnippet)
        {
            Name = thisName;
            ValueType = thisValueType;
            DefaultValues = new ArrayList();
            DefaultValues.Add(thisDefaultValue);
            PromptingText = thisPromptingText;
            Snippet = thisSnippet;
        }

        public ParameterField(string thisName, FieldValueType thisValueType, ArrayList theseDefaultValues, string thisPromptingText, string thisSnippet, EnumType thisEnumerationType)
        {
            Name = thisName;
            ValueType = thisValueType;
            DefaultValues = theseDefaultValues;
            PromptingText = thisPromptingText;
            Snippet = thisSnippet;
            EnumerationType = thisEnumerationType;
        }

        public ParameterField(string thisName, FieldValueType thisValueType, ArrayList theseDefaultValues, string thisPromptingText, string thisSnippet, DefinitionCategory thisDefCategory)
        {
            Name = thisName;
            ValueType = thisValueType;
            DefaultValues = theseDefaultValues;
            PromptingText = thisPromptingText;
            Snippet = thisSnippet;
            DefCategory = thisDefCategory;
        }

        public ParameterField(string thisName, FieldValueType thisValueType, ArrayList theseDefaultValues, string thisPromptingText, string thisSnippet, ReportFKType thisReportFKType)
        {
            Name = thisName;
            ValueType = thisValueType;
            DefaultValues = theseDefaultValues;
            PromptingText = thisPromptingText;
            Snippet = thisSnippet;
            FKeyType = thisReportFKType;
        }

        /// <summary>
        /// Applies a value to the specified parameter field of a report. 
        /// The currentValues is usually just a single value. 
        /// The only time there will be multiple values is for a def or an enum. 
        /// For example, if a user selects multiple items from a dropdown box for this parameter, then each item is connected by an OR. 
        /// The entire output value is surrounded by parentheses.</summary>
        public void ApplyParamValues()
        {
            OutputValue = "(";
            if (CurrentValues.Count == 0)
            {//if there are no values
                OutputValue += "1";//display a 1 (true) to effectively exclude this snippet
            }
            for (int i = 0; i < CurrentValues.Count; i++)
            {
                if (i > 0)
                {
                    OutputValue += " OR";
                }
                if (ValueType == FieldValueType.Boolean)
                {
                    if ((bool)CurrentValues[i])
                    {
                        OutputValue += Snippet;//snippet will show. There is no ? substitution
                    }
                    else
                    {
                        OutputValue += "1";//instead of the snippet, a 1 will show
                    }
                }
                else if (ValueType == FieldValueType.Date)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.Date((DateTime)CurrentValues[i], false));
                }
                else if (ValueType == FieldValueType.Def)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.Long((int)CurrentValues[i]));
                }
                else if (ValueType == FieldValueType.Enum)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.Long((int)CurrentValues[i]));
                }
                else if (ValueType == FieldValueType.Integer)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.Long((int)CurrentValues[i]));
                }
                else if (ValueType == FieldValueType.String)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.String((string)CurrentValues[i]));
                }
                else if (ValueType == FieldValueType.Number)
                {
                    OutputValue += " " + Regex.Replace(Snippet, @"\?", POut.Double((double)CurrentValues[i]));
                }
            }
            OutputValue += ")";
        }
    }

    /// <summary>
    /// Specifies the type of value that the field will accept.
    /// Used in ParameterDef.ValueType and ReportObject.ValueType properties.
    /// Also used in the ContrMultInput control to determine what kind of input to display.
    /// </summary>
    public enum FieldValueType
    {
        /// <summary>
        /// Field takes a date value.
        /// </summary>
        Date,

        /// <summary>
        /// Field takes a string value.
        /// </summary>
        String,
        
        /// <summary>
        /// Field takes a boolean value.
        /// For a Parameter, if false, then the snippet will not even be included. Because of the way this is implemented, 
        /// the snippet can specify a true or false value, and the user can select whether to include the snippet. 
        /// So the parameter can specify whether to include a false value among many other possibilities. 
        /// There should not be a ? in a boolean snippet.
        /// </summary>
        Boolean,
        
        /// <summary>
        /// Field takes an integer value.
        /// </summary>
        Integer,
        
        /// <summary>
        /// Field takes a number(double) value which can include a decimal.
        /// </summary>
        Number,
       
        /// <summary>
        /// Field takes an enumeration value(s), usually in int form from a dropdown list.
        /// </summary>
        Enum,
        
        /// <summary>
        /// Field takes definition.DefNum value from a def category. 
        /// Presented to user as a dropdown list for that category.
        /// </summary>
        Def,
        
        /// <summary>
        /// Only used in ReportObject. When a table comes back from the database, if the expected value is an age, then this column type should be used.
        /// Just retreive the birthdate and the program will convert it to an age.
        /// </summary>
        Age,
       
        /// <summary></summary>
        ForeignKey,
        
        /// <summary>
        /// Only used in FormQuestionnaire.
        /// </summary>
        YesNoUnknown
    }
}