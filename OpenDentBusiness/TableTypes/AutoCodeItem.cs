using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Corresponds to the autocodeitem table in the database.
    /// There are multiple AutoCodeItems for a given AutoCode.
    /// Each Item has one ADA code.
    /// </summary>
    public class AutoCodeItem : DataRecord
    {
        /// <summary>FK to autocode.AutoCodeNum</summary>
        public long AutoCodeId;

        /// <summary>Do not use</summary>
        public string OldCode;

        /// <summary>FK to procedurecode.CodeNum</summary>
        public long ProcedureCodeId;

        /// <summary>
        /// Only used in the validation section when closing FormAutoCodeEdit.
        /// Will normally be empty.
        /// </summary>
        public List<AutoCodeCond> Conditions;

        public AutoCodeItem Copy()
        {
            return (AutoCodeItem)this.MemberwiseClone();
        }
    }
}
