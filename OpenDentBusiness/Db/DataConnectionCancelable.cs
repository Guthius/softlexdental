using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// Should only be used for running user queries.
    /// A list of current MySqlConnections is kept so that we can cancel queries running on specific server threads in that list if need be.
    /// </summary>
    public class DataConnectionCancelable
    {
        /// <summary>
        /// A static dictionary of connections via their ServerThread IDs.
        /// Necessary for Middle Tier to be able to close the corresponding connection.
        /// </summary>
        static readonly Dictionary<int, MySqlConnection> serverConnections = new Dictionary<int, MySqlConnection>();

        /// <summary>
        /// The lock object that is used to lock the dictionary of MySqlConnections.
        /// Only used when adding and removing from the dict.
        /// </summary>
        static readonly object lockObj = new object();

        /// <summary>
        /// Turns "pooling" off, then opens the current database connection, adds that connection 
        /// to the dictionary of MySqlConnections, then returns the unique ServerThread. The 
        /// returned ServerThread can then be used later in order to stop the query in the middle
        /// of executing. A non-pooled connection will NOT attempt to re-use connections to the DB 
        /// that already exist but are idle, rather it will create a brand new connection that no 
        /// other connection can use. This is so that user queries can be safely cancelled if 
        /// needed. Required as a first step for user queries (and ONLY user queries).
        /// </summary>
        public static int GetServerThread(bool isReportServer)
        {
            var connection = new MySqlConnection(DataConnection.ConnectionString);
            connection.Open();

            int serverThread = connection.ServerThread;

            // If the dictionary already contains the ServerThread key, then something went wrong. Just stop and throw.
            if (serverConnections.ContainsKey(serverThread))
            {
                connection.Close();

                throw new ApplicationException("Critical error in GetServerThread: A duplicate connection was found via the server thread ID.");
            }

            lock (lockObj)
            {
                serverConnections[serverThread] = connection;
            }
            return serverThread;
        }

        /// <summary>
        /// Currently only for user queries. The connection must already be opened before calling this method.
        /// Fills and returns a DataTable from the database. Throws an exception if a connection could not be 
        /// found via the passed in server thread.
        /// </summary>
        public static DataTable GetTableConAlreadyOpen(int serverThread, string commandText, bool isValidated)
        {
            // If the dictionary does not contain the ServerThread key, then something went wrong. Just stop and throw.
            if (!serverConnections.TryGetValue(serverThread, out var connection))
            {
                throw new ApplicationException("Critical error in GetTableConAlreadyOpen: A connection could not be found via the given server thread ID.");
            }

            // Throws Exception if SQL is not allowed, which is handled by the ExceptionThreadHandler and output in a MsgBox.
            if (!isValidated && !Db.IsSqlAllowed(commandText))
            {
                throw new ApplicationException("Error: Command is either not safe or user does not have permission.");
            }

            var dataTable = new DataTable();

            using (var dataAdapter = new MySqlDataAdapter(new MySqlCommand(commandText, connection)))
            {
                try
                {
                    dataAdapter.Fill(dataTable);
                }
                finally
                {
                    connection.Close(); //if the query was stopped or has finished executing, this will close the connection that it was executing on.
                    lock (lockObj)
                    {
                        serverConnections.Remove(serverThread);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Currently only for user queries. 
        /// Tries to cancel the connection that corresponds to the passed in server thread. 
        /// Does not close the connection as that is taken care of in GetTableConAlreadyOpen() in a finally statement.
        /// Optionally throws an exception if a connection could not be found via the passed in server thread.
        /// </summary>
        public static void CancelQuery(int serverThread, bool hasExceptions = true)
        {
            // This could happen if the user clicked 'Cancel' and by the time it got here, the query finished executing on a different thread.
            // Since this race condition could happen more frequently within the middle tier environment, we just won't do anything about it.
            if (!serverConnections.ContainsKey(serverThread)) return;

            try
            {
                DataConnection.ExecuteNonQuery("KILL QUERY " + serverThread);
            }
            catch (MySqlException mySqlException)
            {
                // Suppress errors about the thread not existing, that means the query finished executing already.
                if (mySqlException.Number == 1094) return; 
                else if (hasExceptions)
                {
                    throw mySqlException;
                }
            }
            catch (Exception exception)
            {
                if (hasExceptions)
                {
                    throw exception;
                }
            }
        }
    }
}