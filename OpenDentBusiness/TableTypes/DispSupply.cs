using System;

namespace OpenDentBusiness
{
    ///<summary>A dental supply or office supply item that has been dispensed.</summary>
    public class DispSupply
    {
        public long DispSupplyNum;

        /// <summary>FK to supply.SupplyNum</summary>
        public long SupplyNum;

        /// <summary>FK to provider.ProvNum</summary>
        public long ProvNum;

        /// <summary></summary>
        public DateTime DateDispensed;

        /// <summary>Quantity given out.</summary>
        public float DispQuantity;

        ///<summary>Notes on the dispensed supply.</summary>
        public string Note;
    }
}
