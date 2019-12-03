namespace OpenDentBusiness
{
    public class StmtLink
    {
        public long StmtLinkNum;

        ///<summary>FK to statement.StatementNum.</summary>
        public long StatementNum;

        ///<summary>Enum:StmtLinkTypes Represents what object FKey corresponds to.</summary>
        public StmtLinkTypes StmtLinkType;

        ///<summary>FK to type of PK of another object depending on StmtLinkType value. E.g. procedurelog.ProcNum, paysplit.PaySplitNum, adjustment.AdjNum, etc.</summary>
        public long FKey;
    }

    public enum StmtLinkTypes
    {
        ///<summary>Procedure</summary>
        Proc,

        ///<summary>Pay split</summary>
        PaySplit,

        ///<summary>Adjustment</summary>
        Adj,

        ///<summary>ClaimPay</summary>
        ClaimPay
    }
}
