using System;
using System.Diagnostics;

namespace OpenDentBusiness.Eclaims
{
    public class Inmediata
    {
        public static string ErrorMessage = "";

        public static bool Launch(Clearinghouse clearinghouse, int batchNum)
        {
            try
            {
                Process.Start(clearinghouse.ClientProgram);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;

                return false;
            }
            return true;
        }
    }
}