using System;
using System.Diagnostics;

namespace OpenDentBusiness.Eclaims
{
    public class ClaimX
    {
        public static string ErrorMessage = "";

        public static bool Launch(Clearinghouse clearinghouseClin, int batchNum)
        {
            try
            {
                Process.Start(clearinghouseClin.ClientProgram);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }
    }
}
