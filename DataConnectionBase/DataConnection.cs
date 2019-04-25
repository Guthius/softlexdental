using CodeBase;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataConnectionBase
{
    public class DataConnection
    {
        static MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
        static string connectionString;
        static uint commandTimeout = 3600;
        static string database;
        static string server;
        static string userid;
        static string password;

        /// <summary>
        /// After inserting a row, this variable will contain the primary key for the newly inserted row.
        /// This can frequently save an additional query to the database.
        /// </summary>
        public static long InsertID { get; private set; }

        public static bool CanReconnect = true;

        ///<summary>Dictates if the automatic retry threads should quit once the "retry timeout" has been reached
        ///in the event of a database exception that is explicitly handled (e.g. lost connection).
        ///When set to false, the automatic retry threads will keep the current thread "locked" here waiting on outside entites to "unlock" them once
        ///the corresponding timeout has passed.  This is usually desireable for applications with a UI.
        ///When set to true, the automatic retry threads will bubble up any UE that caused them to start once the timeout has been reached.
        ///This is desireable for applications without a UI (e.g. the eConnector).</summary>
        public static bool DoThrowOnAutoRetryTimeout = false;
        
        
        /// <summary>
        /// Updates the connection string.
        /// </summary>
        static void UpdateConnectionString()
        {
            connectionStringBuilder.Server = Server;
            connectionStringBuilder.Database = Database;
            connectionStringBuilder.UserID = UserID;
            connectionStringBuilder.Password = Password;
            connectionStringBuilder.Pooling = true;
            connectionStringBuilder.DefaultCommandTimeout = CommandTimout;
            connectionStringBuilder.Port = 3306;
            connectionStringBuilder.CharacterSet = "utf8";
            connectionStringBuilder.AllowUserVariables = true;
            connectionStringBuilder.TreatTinyAsBoolean = true;
            connectionStringBuilder.ConvertZeroDateTime = true;
            connectionStringBuilder.SslMode = MySqlSslMode.None;
            connectionString = connectionStringBuilder.ToString();
        }

        /// <summary>
        /// Gets or sets the default command timeout (in seconds).
        /// </summary>
        public static uint CommandTimout
        {
            get => commandTimeout;
            set
            {
                var newCommandTimeout = value;
                if (newCommandTimeout <= 0)
                {
                    newCommandTimeout = 3600;
                }

                if (newCommandTimeout != commandTimeout)
                {
                    commandTimeout = newCommandTimeout;
                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public static string Database
        {
            get => database;
            set
            {
                if (value != database)
                {
                    database = value ?? "";

                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the database server name.
        /// </summary>
        public static string Server
        {
            get => server;
            set
            {
                if (value!= server)
                {
                    server = value ?? "";

                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the MySQL username.
        /// </summary>
        public static string UserID
        {
            get => userid;
            set
            {
                if (value != userid)
                {
                    userid = value ?? "";

                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the MySQL password.
        /// </summary>
        public static string Password
        {
            get => password;
            set
            {
                if (value != password)
                {
                    password = value ?? "";

                    UpdateConnectionString();
                }
            }
        }
        
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    UpdateConnectionString();
                }
                return connectionString;
            }
        }

        /// <summary>
        /// The number of seconds that the thread will automatically retry connection to the database when the connection has been lost. Defaults to 0 seconds.
        /// </summary>
        public static int ConnectionRetryTimeoutSeconds { get; set; } = 0;

        public static double CrashedTableTimeoutSeconds { get; set; } = 0;

        public static bool GetCanReconnect()
        {
            return CanReconnect;
        }

        public static void SetDb(string server, string db, string user, string password, bool skipValidation = false)
        {
            Database = db;
            Server = server;
            UserID = user;
            Password = password;

            TestConnection(skipValidation);
        }

        static void TestConnection(bool skipValidation)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    if (!skipValidation)
                    {
                        command.CommandText = "UPDATE `preference` SET `ValueString` = '0' WHERE `ValueString` = '0'";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static MySqlDataReader GetDataReader(string commandText)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    return command.ExecuteReader();
                }
            }
        }



        public static bool IsTableCrashed(string tableName, bool doRetryConn = false)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"CHECK TABLE `{tableName}`";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["Msg_text"].ToString().Trim().ToLower() == "ok")
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Fills table with data from the database.
        /// </summary>
        public static DataTable GetTable(string command, bool doAutoRetry = true)
        {
            var table = new DataTable();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var dataAdapter = new MySqlDataAdapter(command, connection))
                    {
                        dataAdapter.Fill(table);
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (doAutoRetry && IsErrorHandled(ex))
                {
                    return GetTable(command, doAutoRetry);
                }
                throw ex;
            }
            return table;
        }

        /// <summary>
        /// Sends a non query command to the database and returns the number of rows affected. 
        /// If getInsertID is true, then InsertID will be set to the value of the primary key of the newly inserted row.   
        /// WILL NOT RETURN CORRECT PRIMARY KEY for MySQL if the query specifies the primary key.
        /// Pass in the PK column and table names so that Oracle can correctly lock the table and know which column to return for the Insert ID.
        /// </summary>
        public static long NonQ(string commands, bool getInsertID, string columnNamePK, string tableName, bool doRetryConn, params MySqlParameter[] parameters)
        {
            long rowsChanged = 0;

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = commands;
                        for (int p = 0; p < parameters.Length; p++)
                        {
                            command.Parameters.Add(parameters[p]).Value = parameters[p].Value;
                        }

                        RunDbAction(new Action(() => rowsChanged = command.ExecuteNonQuery()));
                        InsertID = command.LastInsertedId;
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1153)
                {
                    throw new ApplicationException("Please add the following to your my.ini file: max_allowed_packet=40000000");
                }

                if (doRetryConn && IsErrorHandled(ex))
                {
                    return NonQ(commands, getInsertID, "", "", doRetryConn, parameters);
                }
                throw ex;
            }
            return rowsChanged;
        }

        ///<summary>Sends a non query command to the database and returns the number of rows affected. If true, then InsertID will be set to the value of the primary key of the newly inserted row.   WILL NOT RETURN CORRECT PRIMARY KEY if the query specifies the primary key.</summary>
        public static long NonQ(string commands, bool getInsertID, params MySqlParameter[] parameters)
        {
            return NonQ(commands, getInsertID, "", "", true, parameters);
        }

        ///<summary>Sends a non query command to the database and returns the number of rows affected. If true, then InsertID will be set to the value of the primary key of the newly inserted row.</summary>
        public static long NonQ(string command)
        {
            return NonQ(command, false);
        }

        ///<summary>Use this for count(*) queries.  They are always guaranteed to return one and only one value.  Uses datareader instead of datatable, so faster.  Can also be used when retrieving prefs manually, since they will also return exactly one value</summary>
        public static string GetCount(string query, bool doRetryConn = true)
        {
            string retVal = "";
            MySqlDataReader dataReader = null;

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        RunDbAction(new Action(() => dataReader = command.ExecuteReader()));

                        dataReader.Read();
                        retVal = dataReader[0].ToString();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (doRetryConn && IsErrorHandled(ex))
                {
                    return GetCount(query, doRetryConn);
                }
                throw ex;
            }
            return retVal;
        }

        public static string GetScalar(string query, bool doRetryConn = true)
        {
            object scalar = null;

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        RunDbAction(new Action(() => scalar = command.ExecuteScalar()));
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (doRetryConn && IsErrorHandled(ex))
                {
                    return GetScalar(query, doRetryConn);
                }
                throw ex;
            }

            return scalar == null ? "" : scalar.ToString();
        }

        ///<summary>Run an action using the given connection and close the connection if the action throws an exception. Re-throws the exception.
        ///This is strictly used to close the orphaned connection and to handle special exceptions.
        ///Do not call this directly, instead use RunMySqlAction() or RunOracleAction().</summary>
        private static void RunDbAction(Action actionDb)
        {
            try
            {
                actionDb();
            }
            catch (MySqlException mySqlEx)
            {
                //#region Database Storage Too Full
                ////This occurs when the servers storage is too full. Instead of saying, "got error 28 from storage engine", we will catch the error
                ////and give them a better message to avoid them from calling us.
                //if (mySqlEx.Number == 1030 && mySqlEx.Message.ToLower() == "got error 28 from storage engine")
                //{
                //    throw new Exception("The server's storage is full. Free space to avoid this error.", mySqlEx);
                //}
                //#endregion
                //#region Retry Read Only Table Once
                ////Recently we have been getting UEs from both HQ and customers that occurs when a table is marked as read-only by MySQL. Because we
                ////generally only get one UE when this occurs, we can assume the table is marked as read-only for a very short time. Because of this, 
                ////we decided to retry the query once instead of crashing the program.
                //else if (mySqlEx.Number == 1036)
                //{
                //    RetryQuery(actionDb, connection, cmd, mySqlEx, "Query Read-only Table");
                //    return;
                //}
                //#endregion
                //#region Retry Fatal Error & Host Failed to Respond Once
                ////Users have been complaining about the "Query Execution Error" which is a customer error we throw when MySQL gives us the
                ////"Fatal error encountered during command execution".  We have decided to simply "try again" in specific scenarios.
                ////Related to the fatal error queries. Connection refuses or fails to respond for whatever reason. As the fatal errors have shown some 
                ////success in working, we will try these once as well. Full exception text is:
                ////Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond 
                ////after a period of time, or established connection failed because connected host has failed to respond.
                //else if ((mySqlEx.Message.ToLower().Contains("fatal error") || mySqlEx.Message.ToLower().Contains("transport connection"))
                //    && (cmd.CommandText.ToLower().StartsWith("select") || cmd.CommandText.ToLower().StartsWith("insert into securitylog")))
                //{
                //    string exceptionMessage = "Query Execution Error - ";
                //    if (cmd.CommandText.ToLower().StartsWith("select"))
                //    {
                //        exceptionMessage += "'SELECT'";
                //    }
                //    else
                //    {//insert
                //     //The command text was checked for starting with "insert into securitylog" so there is a chance that the error is for securityloghash.
                //     //Get the first three words of the query so that we aren't mislead into thinking the failure was explicitly for securitylog if it wasn't.
                //        string[] commandWords = cmd.CommandText.Split(' ');
                //        if (commandWords.Length >= 3)
                //        {
                //            exceptionMessage += "'" + commandWords[0] + " " + commandWords[1] + " " + commandWords[2] + "'";
                //        }
                //        else
                //        {
                //            exceptionMessage += "'INSERT INTO securitylog...'";//unknown table specificity.
                //        }
                //    }
                //    exceptionMessage += " statement at " + DateTime.Now.ToString() + " local time - ";
                //    RetryQuery(actionDb, connection, cmd, mySqlEx, exceptionMessage);
                //    return;
                //}
                //#endregion
                ////Close() will throw a different exception if it cannot close the connection. Swallow this and move on.
                //ODException.SwallowAnyException(() => connection.Close());
                throw;//A different MySQL Exception, bubble it up.
            }
            catch 
            {
                //Close() will throw a different exception if it cannot close the connection. Swallow this and move on.
                //ODException.SwallowAnyException(() => connection.Close());
                throw;
            }
        }

        #region MySQL Error Detection / Handling

        ///<summary>Listens to see if the connection has been restored.</summary>
        //private void DataConnectionEvent_Fired(DataConnectionEventArgs e)
        //{
        //    if (e.IsConnectionRestored && e.ConnectionString == _con.ConnectionString)
        //    {
        //        _isConnectionRestored = true;
        //    }
        //}

        ///<summary>Listens to see if the table has reported the status OK.</summary>
        //private void CrashedTableEvent_Fired(CrashedTableEventArgs e)
        //{
        //    if (!e.IsTableCrashed && e.TableName == _crashedTableName)
        //    {
        //        _isCrashedTableOK = true;
        //        _crashedTableName = "";
        //    }
        //}

        ///<summary>Handles certain types of MySQL errors. The application may pause here to wait for the error to be resolved.
        ///Returns true if the calling method should retry the db action that just failed.  E.g. recursively invoke GetTable()
        ///Returns false if the exception passed in is not specifically handled and the program should crash.</summary>
        static bool IsErrorHandled(MySqlException ex)
        {
            // TODO: Fix this...

            //Don't catch error 1153 (max_allowed_packet error), it will change program behavior if we do.
            //if (IsConnectionLost(ex))
            //{
            //    //Pause the application here for a specified amount of time.
            //    return StartConnectionErrorRetry(_con.ConnectionString, DataConnectionEventType.ConnectionLost);
            //}
            //if (IsTooManyConnections(ex))
            //{
            //    //Pause the application here for a specified amount of time.
            //    return StartConnectionErrorRetry(_con.ConnectionString, DataConnectionEventType.TooManyConnections);
            //}
            //if (IsCrashedTable(ex, out _crashedTableName))
            //{
            //    //Pause the application here for a specified amount of time.
            //    return StartCrashedTableMonitor();
            //}
            return false;
        }


        public delegate void QueryAction(MySqlConnection connection);

        public void Execute(QueryAction queryAction)
        {
            if (queryAction != null)
            {
                try
                {
                    using (var connection = GetConnection())
                    {
                        connection.Open();

                        RunDbAction(() => queryAction(connection));
                    }
                }
                catch { }
            }
        }

        // TODO: Implement the connection retry system using plugin triggers.
        // database_connection_lost
        // database_connection_restored

        ///<summary>Fires an event to launch the LostConnection window and freezes the calling thread until connection has been restored or the timeout
        ///has been reached. This is only a blocking call when ConnectionRetrySeconds is greater than 0 or not the Middle Tier.
        ///Immediately returns if ConnectionRetrySeconds is 0 or this is the Middle Tier that has lost connection to the database.
        ///Returns true if the calling method should retry the db action that just failed.  E.g. recursively invoke GetTable()
        ///Returns false if the calling method should instead bubble up the original exception.</summary>
        //static bool StartConnectionErrorRetry(string connectionString, DataConnectionEventType errorType)
        //{
        //    if (ConnectionRetryTimeoutSeconds == 0 || !GetCanReconnect())
        //    {-
        //        return false;
        //    }
        //    bool doRetry = true;
        //    //Register for the DataConnection event just in case another thread notices that the connection has come back before this method does.
        //    //DataConnectionEvent.Fired += DataConnectionEvent_Fired;
        //    //Notify anyone that cares that we are waiting here until the connection comes back online.
        //    //Typical consumers of this event will launch a connection lost window and turn off threads / timers until the connection has come back.
        //    DataConnectionEvent.Fire(new DataConnectionEventArgs(errorType, false, connectionString));
        //
        //    //Keep the current thread stuck here while automatically retrying the db connection up until the timeout specified.
        //    DateTime beginning = DateTime.Now;
        //    ODThread threadRetry = new ODThread(500, (o) =>
        //    {
        //        if (_isConnectionRestored)
        //        {
        //            o.QuitAsync();//Have this thread exit and join back with the main application.
        //            return;
        //        }
        //        if ((DateTime.Now - beginning).TotalSeconds >= ConnectionRetryTimeoutSeconds)
        //        {//We have reached or exceeded the timeout.
        //         //Stop automatically retrying and bubble up the exception OR leave it up to a manual retry from the user.
        //            if (DoThrowOnAutoRetryTimeout)
        //            {
        //                doRetry = false;
        //                o.QuitAsync();
        //            }
        //            return;
        //        }
        //        try
        //        {
        //            TestConnection(false);
        //
        //            //An exception was not thrown by TestConnection() so the connection has been restored.
        //            _isConnectionRestored = true;
        //            //Also fire a DataConnectionEvent letting everyone who cares know that the connection has been restored.
        //            DataConnectionEvent.Fire(new DataConnectionEventArgs(DataConnectionEventType.ConnectionRestored, true, connectionString));
        //            o.QuitAsync();
        //        }
        //        catch
        //        {
        //        }
        //    });
        //    threadRetry.Name = "DataConnectionAutoRetryThread";
        //    threadRetry.AddExceptionHandler((e) =>
        //    {
        //        //Unhandled exception so tell caller to throw. Letting caller continue would lead us right back here and potentially cause a stack overflow.
        //        doRetry = false;
        //    });
        //    threadRetry.Start();
        //    //Wait here until the automatic retry thread or the manual retry button has detected the connection as being restored.
        //    threadRetry.Join(Timeout.Infinite);
        //    DataConnectionEvent.Fired -= DataConnectionEvent_Fired;
        //    return doRetry;
        //}



        // TODO: Implement a new crashed table monitor, using plugin triggers.
        // database_table_crashed
        // database_table_recovered

        ///<summary></summary>
        //private bool StartCrashedTableMonitor()
        //{
        //    if (string.IsNullOrEmpty(_crashedTableName) || CrashedTableTimeoutSeconds == 0)
        //    {
        //        return false;
        //    }
        //    bool doRetry = true;
        //    //Register for CrashedTableEvent events just in case another thread notices that the table is OK before this method does.
        //    CrashedTableEvent.Fired += CrashedTableEvent_Fired;
        //    //Notify anyone that cares that we are waiting here until the table is OK.
        //    //Typical consumers of this event will launch a connection lost window and wait until the table is OK.
        //    CrashedTableEvent.Fire(new CrashedTableEventArgs(true, _crashedTableName));
        //    //Keep the current thread stuck here while automatically checking the status of the table up until the timeout specified.
        //    DateTime beginning = DateTime.Now;
        //    ODThread threadCrashedTableMonitor = new ODThread(500, (o) =>
        //    {
        //        if (_isCrashedTableOK)
        //        {
        //            o.QuitAsync();//Have this thread exit and join back with the main application.
        //            return;
        //        }
        //        if ((DateTime.Now - beginning).TotalSeconds >= CrashedTableTimeoutSeconds)
        //        {
        //            //Stop automatically retrying and bubble up the exception OR leave it up to a manual retry from the user.
        //            if (DoThrowOnAutoRetryTimeout)
        //            {
        //                doRetry = false;
        //                o.QuitAsync();
        //            }
        //            return;
        //        }
        //        if (!IsTableCrashed(_crashedTableName))
        //        {
        //            _isCrashedTableOK = true;
        //            CrashedTableEvent.Fire(new CrashedTableEventArgs(false, _crashedTableName));
        //            o.QuitAsync();
        //        }
        //    });
        //    threadCrashedTableMonitor.Name = "CrashedTableAutoRetryThread";
        //    threadCrashedTableMonitor.AddExceptionHandler((e) =>
        //    {
        //        //Unhandled exception so tell caller to throw. Letting caller continue would lead us right back here and potentially cause a stack overflow.
        //        doRetry = false;
        //    });
        //    threadCrashedTableMonitor.Start();
        //    //Wait here until the retry thread has finished which is either due to connection being restored or the timeout was reached.
        //    threadCrashedTableMonitor.Join(Timeout.Infinite);//Wait forever because the retry thread has a timeout within itself.
        //    CrashedTableEvent.Fired -= CrashedTableEvent_Fired;
        //    return doRetry;
        //}

        ///<summary>Returns true if the MySQL connection has been lost.</summary>
        static bool IsConnectionLost(MySqlException ex)
        {
            if ((ex.Message.ToLower().Contains("stream") && ex.Message.ToLower().Contains("failed"))//Reading from the stream has failed 
                    || ex.Number == 1042//Unable to connect to any of the specified MySQL hosts
                    || ex.Number == 1045)//Access denied)
            {
                return true;
            }
            return false;
        }

        ///<summary>Returns true if the MySQL connection could not connect because there were too many connections to the server.</summary>
        static bool IsTooManyConnections(MySqlException ex)
        {
            //error from MySQL is "Too Many Connections" with a code of 1040
            if (ex.Message.ToLower().Contains("too many connections") && ex.Number == 1040)
            {
                return true;
            }
            return false;
        }

        ///<summary>Returns true if the MySQL exception contains text that is common for a crashed table error and sets the tableName parameter.
        ///Otherwise; false.</summary>
        static bool IsCrashedTable(MySqlException ex, out string tableName)
        {
            tableName = "";
            //Error number: 1194; Symbol: ER_CRASHED_ON_USAGE; SQLSTATE: HY000
            //Message: Table '%s' is marked as crashed and should be repaired
            if (ex.ErrorCode != 1194 && !ex.Message.ToLower().Contains("is marked as crashed and should be repaired"))
            {
                return false;
            }
            //Try to extract the name of the table from the error messsage which will be between two single quotes.
            //If the name of the table could not be extracted then simply return false and allow the program to crash.
            Match match = Regex.Match(ex.Message, "\'(.*?)\'");
            if (!match.Success)
            {
                return false;
            }
            //Set the tableName parameter so that the calling method knows which table to monitor.
            //Sometimes the table name will be in an absolute format.  E.g. '.\databasename\tablename' 
            //Other times the table name will be alone.  E.g. 'tablename'
            tableName = match.Groups[1].Value.Split('\\').Last();
            return true;
        }

        ///<summary>This method will retry a query once. This should only be used in special cases as generally if a query causes an exception,
        ///there is a good reason for doing so.</summary>
        ///<param name="actionDb">The given action that caused the exception. Will be ran again.</param>
        ///<param name="connection">The connection used to run the query.</param>
        ///<param name="cmd">The command object that will contain the query text being executed.</param>
        ///<param name="mySqlEx">The original MySQL exception that was thrown.</param>
        ///<param name="exceptionMessage">This is the message that will appear in HQ's bug submissions. The message will be followed by either 
        ///Retry Successful or Retry Failure. E.g. Query Execution Error Retry Successful.</param>
        private static void RetryQuery(Action actionDb, System.Data.Common.DbConnection connection, System.Data.Common.DbCommand cmd,MySqlException mySqlEx, string exceptionMessage)
        {
            string commandTextRaw = "";
            ODException.SwallowAnyException(() =>
            {
                if (mySqlEx.Message.ToLower().Contains("fatal error"))
                {
                    //The entire securityloghash query can be logged because there is never any PHI present.
                    if (cmd.CommandText.ToLower().StartsWith("insert into securityloghash "))
                    {
                        commandTextRaw = cmd.CommandText;
                    }
                    //Make sure that the LogText column is set to @paramLogText.  Otherwise, do not 
                    else if (cmd.CommandText.ToLower().StartsWith("insert into securitylog "))
                    {
                        //The LogText column is the only column that can contain PHI which is a query parameter when our CRUD is making the insert.
                        //This will show up in the query string as "@paramLogText".  This is safe to upload the entire command to HQ in raw format.
                        //Queries that are generated by our CRUD are formatted in a very predictable fashion;
                        //^The 4th item will always be the column names definition.  E.g. "(SecurityLogNum,PermType,UserNum...)"
                        //^The 5th item will always be the VALUES definition.  E.g. "VALUES(897489,83,1,NOW(),...)"
                        //^^We REQUIRE the 5th item (values) to contain @paramLogText otherwise we will not log the query in raw format.
                        //^^We know the 5th item will always contain the log text value because we split by space and our CRUD never has a space before LogText.
                        string[] commandSplitBySpaces = cmd.CommandText.Split(' ');
                        if (commandSplitBySpaces.Length >= 5 && commandSplitBySpaces[4].Contains(",@paramLogText,"))
                        {
                            commandTextRaw = cmd.CommandText;
                        }
                        else
                        {
                            commandTextRaw = "Non-CRUD insert statement trying to execute, not logging command on purpose.";
                        }
                    }
                }
            });
            //Surround the following in a try / catch so that we can tell HQ what happened regardless of any errors.
            try
            {
                //First, we will close the connection as depending on the situation, the connection may have already been forcefully closed.
                connection.Close();
                connection.Open();//Re-open the connection.
                actionDb();//Try the query one more time.
                return;//The retry attempt worked, so do not continue to bubble up the exception.  Let the user go back to work.
            }
            catch (Exception ex)
            {
                //Close() will throw a different exception if it cannot close the connection. Swallow this and move on.
                ODException.SwallowAnyException(() => connection.Close());
                //The retry attempt failed.  Pass along whatever UE occurred so that HQ can know about it as well (might be different, might be the same).
                //Throw a special type of ODException with a custom message in order to preserve the original StackTrace.
                //The first line of the custom UE's Message property will turn into the BugSubmission's ExceptionMessageText field.
                //All subsequent lines will become the BugSubmission's ExceptionStackTrace field.
                string message = exceptionMessage + " Retry Failure\r\n"//First Line, thus the ExceptionMessageText field...
                                                                        //Subsequent lines, thus the ExceptionStackTrace field...
                        + (string.IsNullOrEmpty(commandTextRaw) ? "" : "Query Info: " + commandTextRaw + "\r\n")
                    + "====================Retry Exception Information====================\r\n" + MiscUtils.GetExceptionText(ex) + "\r\n"
                    + "====================MySQL Exception Information====================\r\n" + MiscUtils.GetExceptionText(mySqlEx);
                throw new ODException(message, ODException.ErrorCodes.BugSubmissionMessage);
            }
        }

        #endregion
    }
}