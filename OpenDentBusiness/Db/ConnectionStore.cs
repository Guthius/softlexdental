using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Thread-safe access to a list of connection store object which is 
    /// retreived from a given file. If Init is not called then looks for 
    /// ConnectionStore.xml in working directory.
    /// </summary>
    public static class ConnectionStore
    {
        static readonly object _lock = new object();

        /// <summary>
        /// Only used by _dictCentConnSafe. Do not use elsewhere in this class.
        /// </summary>
        private static Dictionary<ConnectionNames, CentralConnection> _dictCentConnUnsafe;

        /// <summary>
        /// Sets the current dictionary of connections to null so that it 
        /// reinitializes all connections the next time it is accessed. This is
        /// mainly used for connections that utilize preferences so that the
        /// dictionary can be up to date.
        /// </summary>
        public static void ClearConnectionDictionary()
        {
            lock (_lock)
            {
                _dictCentConnUnsafe = null;
            }
        }
    }
}