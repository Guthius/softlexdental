using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental
{
    public class PatRestrictionL
    {
        /// <summary>
        /// Checks for an existing patrestriction for the specified patient and PatRestrictType. 
        /// </summary>
        public static bool IsRestricted(long patientId, PatRestrict patRestrictType, bool suppressMessage = false)
        {
            if (PatRestrictions.IsRestricted(patientId, patRestrictType))
            {
                if (!suppressMessage)
                {
                    MessageBox.Show(
                        "Not allowed due to patient restriction.\r\n" + PatRestrictions.GetPatRestrictDesc(patRestrictType),
                        "Appointment", // TODO: Correct caption?
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                return true;
            }

            return false;
        }
    }
}
