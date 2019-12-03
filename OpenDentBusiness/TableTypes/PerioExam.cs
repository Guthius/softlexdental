using System;

namespace OpenDentBusiness
{
    ///<summary>One perio exam for one patient on one date.  Has lots of periomeasures attached to it.</summary>
    public class PerioExam
    {
        public long PerioExamNum;

        ///<summary>FK to patient.PatNum.</summary>
        public long PatNum;

        ///<summary>.</summary>
        public DateTime ExamDate;

        ///<summary>FK to provider.ProvNum.</summary>
        public long ProvNum;

        ///<summary>Date and time PerioExam was created or modified, including the associated PerioMeasure rows.</summary>
        public DateTime DateTMeasureEdit;
    }
}
