using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// One form or questionnaire filled out by a patient. 
    /// Each patient can have multiple forms.
    /// </summary>
    public class FormPat
    {
        public long FormPatNum;

        ///<summary>FK to patient.PatNum.</summary>
        public long PatNum;

        ///<summary>The date and time that this questionnaire was filled out.</summary>
        public DateTime FormDateTime;

        ///<summary>Not a database field.</summary>
        public List<Question> QuestionList = new List<Question>();
    }
}
