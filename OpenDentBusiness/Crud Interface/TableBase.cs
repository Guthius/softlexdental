using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// The base class for classes that correspond to a table in the database.  Make sure to mark each derived class [Serializable].
    /// </summary>
    abstract public class ODTable
    {
        static long maxAllowedPacketCount = 0;

        /// <summary>
        /// We cannot make the returned value too large, because we want to allow the server to 
        /// process information from the previous packet while downloading the next packet in 
        /// parallel.
        /// </summary>
        public static long MaxAllowedPacketCount
        {
            get
            {
                if (maxAllowedPacketCount > 0)
                {
                    return maxAllowedPacketCount;
                }

                int kilobyte = 1024; // 1KB
                int megabyte = kilobyte * kilobyte; // 1MB

                // Minus 8KB to allow for MySQL header information. Ex see PrefL.CopyFromHereToUpdateFiles()
                var retVal = MiscData.GetMaxAllowedPacket() - 8 * kilobyte;

                // Minimum of 8K (for network packet headers), maximum of 1MB for parallel.
                maxAllowedPacketCount = Math.Min(Math.Max(retVal, 8 * kilobyte), megabyte);

                return maxAllowedPacketCount;
            }
        }
    }
}