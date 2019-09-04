using System;
using System.Net;

namespace OpenDentBusiness
{
    public class MiscData
    {
        /// <summary>
        /// Gets the current date/Time direcly from the server.
        /// Mostly used to prevent uesr from altering the workstation date to bypass security.
        /// </summary>
        public static DateTime GetNowDateTime()
        {
            var result = DataConnection.ExecuteScalar("SELECT NOW()");

            return DateTime.Parse(result);
        }

        /// <summary>
        /// Gets the current date/Time with milliseconds directly from server.
        /// In Mysql we must query the server until the second rolls over, which may take up to one second.
        /// Used to confirm synchronization in time for EHR.
        /// </summary>
        public static DateTime GetNowDateTimeWithMilli()
        {
            var now = GetNowDateTime();
            var second = now.Second;

            do
            {
                now = GetNowDateTime();
            }
            while (second == now.Second);

            return now;
        }

        public static string GetCurrentDatabase() => 
            DataConnection.ExecuteString("SELECT DATABASE()");

        /// <summary>
        /// Returns the major and minor version of MySQL for the current connection.
        /// Returns a version of 0.0 if the MySQL version cannot be determined.
        /// </summary>
        public static string GetMySqlVersion() => DataConnection.ExecuteString("SELECT @@version");

        /// <summary>
        /// Gets the human readable host name of the database server.
        /// This will return an empty string if Dns lookup fails.
        /// </summary>
        public static string GetServerName()
        {
            string hostName = DataConnection.Host;

            try
            {
                var hostEntry = Dns.GetHostEntry(hostName);
                if (hostEntry != null)
                {
                    hostName = hostEntry.HostName;
                }
            }
            catch { }

            return hostName;
        }

        /// <summary>
        /// Returns the current value in the GLOBAL max_allowed_packet variable.
        /// max_allowed_packet is stored as an integer in multiples of 1,024 with a min 
        /// value of 1,024 and a max value of 1,073,741,824.
        /// </summary>
        public static long GetMaxAllowedPacket() =>
            DataConnection.ExecuteLong("SHOW GLOBAL VARIABLES WHERE variable_name = 'max_allowed_packet'");
    }
}
