using System;
using System.Data;
using System.Runtime.ExceptionServices;

namespace OpenDentBusiness
{
    public class ReportsComplex
    {
        /// <summary>
        /// Gets a table of data using normal permissions.
        /// </summary>
        public static DataTable GetTable(string command)
        {
            return Db.GetTable(command);
        }

        /// <summary>Wrapper method to call the passed-in func in a seperate thread connected to the reporting server.
        ///This method should only be used for SELECT, with the exception DashboardAR. Using this for create/update/delete may cause duplicates.
        ///The return type of this function is whatever the return type of the method you passed in is.
        ///Throws an exception if anything went wrong executing func within the thread.</summary>
        ///<param name="doRunOnReportServer">If this false, the func will run against the currently connected server.</param>
        public static T RunFuncOnReportServer<T>(Func<T> func, bool doRunOnReportServer = true)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex.InnerException ?? ex);

                throw exceptionDispatchInfo.SourceException;
            }
        }
    }
}