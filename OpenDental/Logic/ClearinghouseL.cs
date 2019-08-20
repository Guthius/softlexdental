using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental
{
    public class ClearinghouseL
    {
        /// <summary>
        /// Returns the clearinghouse specified by the given num. 
        /// Will only return an HQ-level clearinghouse.
        /// Do not attempt to pass in a clinic-level clearinghouseNum.
        /// </summary>
        public static Clearinghouse GetClearinghouseHq(long clearinghouseId) => GetClearinghouseHq(clearinghouseId, false);

        /// <summary>
        /// Returns the clearinghouse specified by the given num. 
        /// Will only return an HQ-level clearinghouse.
        /// Do not attempt to pass in a clinic-level clearinghouseNum. 
        /// Can return null if no match found.
        /// </summary>
        public static Clearinghouse GetClearinghouseHq(long clearinghouseId, bool suppressError)
        {
            var clearinghouse = Clearinghouses.GetClearinghouse(clearinghouseId);
            if (clearinghouse == null && !suppressError)
            {
                MessageBox.Show("Error. Could not locate Clearinghouse.", "Clearinghouses");
            }
            return clearinghouse;
        }

        public static string GetDescript(long clearinghouseId)
        {
            if (clearinghouseId == 0)
            {
                return "";
            }

            return GetClearinghouseHq(clearinghouseId).Description;
        }
    }
}