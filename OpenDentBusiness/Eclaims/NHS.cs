namespace OpenDentBusiness.Eclaims
{
    /// <summary>
    /// United Kindgdom National Health Service (NHS).
    /// </summary>
    public class NHS
    {
        // TODO: Implement me.

        public static string ErrorMessage = "";

        /// <summary>
        /// Returns true if the communications were successful, and false if they failed. 
        /// If they failed, a rollback will happen automatically by deleting the previously
        /// created FP17 file. The batchnum is supplied for the possible rollback.
        /// </summary>
        public static bool Launch(Clearinghouse clearinghouseClin, int batchNum) => true;
    }
}