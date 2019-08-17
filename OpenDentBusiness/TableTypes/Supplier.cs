using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// A company that provides supplies for the office, typically dental supplies.
    /// </summary>
    public class Supplier
    {
        public long SupplierNum;
        public string Name;
        public string Phone;

        /// <summary>
        /// The customer ID that this office uses for transactions with the supplier
        /// </summary>
        public string CustomerId;

        /// <summary>
        /// Full address to website.  We might make it clickable.
        /// </summary>
        public string Website;

        /// <summary>
        /// The username used to log in to the supplier website.
        /// </summary>
        public string UserName;

        /// <summary>
        /// The password to log in to the supplier website. Not encrypted or hidden in any way.
        /// </summary>
        public string Password;

        /// <summary>
        /// Any note regarding supplier. Could hold address, CC info, etc.
        /// </summary>
        public string Note;
    }
}