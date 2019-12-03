using System.Drawing;

namespace OpenDentBusiness
{
    /// <summary>An individual procedure code color range.</summary>
    public class ProcApptColor
    {
        public long ProcApptColorNum;

        ///<summary>Procedure code range defined by user.  Includes commas and dashes, but no spaces.  The codes need not be valid since they are ranges.</summary>
        public string CodeRange;

        ///<summary>Adds most recent completed date to ProcsColored</summary>
        public bool ShowPreviousDate;

        ///<summary>Color that shows in appointments</summary>
        public Color ColorText;

        public ProcApptColor Copy()
        {
            return (ProcApptColor)this.MemberwiseClone();
        }
    }
}
