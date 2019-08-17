using System;
using System.Collections;

namespace OpenDentBusiness
{
    ///<summary>An optional field on insplan and claims.  This lets user customize so that they can track insurance types.</summary>
    public class InsFilingCode : ODTable
    {
        ///<summary>Primary key.</summary>
        [ODTableColumn(PrimaryKey = true)]
        public long InsFilingCodeNum;
        ///<summary>Description of the insurance filing code.</summary>
        public string Descript;
        ///<summary>Code for electronic claim.</summary>
        public string EclaimCode;
        ///<summary>Display order for this filing code within the UI.  0-indexed.</summary>
        public int ItemOrder;
        ///<summary>FK to definition.DefNum.  Reporting Group.</summary>
        public long GroupType;

        public InsFilingCode Clone()
        {
            return (InsFilingCode)this.MemberwiseClone();
        }

    }
}