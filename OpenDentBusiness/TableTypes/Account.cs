using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used in the accounting section in chart of accounts. Not related to patient accounts in any way.
    /// </summary>
    public class Account : ODTable
    {
        [ODTableColumn(PrimaryKey = true)]
        public long AccountNum;

        public string Description;

        public AccountType AcctType;

        public string BankNumber;

        public bool Inactive;

        public Color AccountColor;

        public Account Clone()
        {
            return (Account)this.MemberwiseClone();
        }
    }
}