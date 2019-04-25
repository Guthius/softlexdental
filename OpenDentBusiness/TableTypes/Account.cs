using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    public partial class Account : ODTable
    {
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