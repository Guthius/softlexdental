using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace OpenDentBusiness
{
    public delegate void DatabaseAction(MySqlConnection connection);
    public delegate T DatabaseAction<T>(MySqlConnection connection);

    public class DataConnection
    {
        static MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
        static string connectionString;
        static uint commandTimeout = 3600;
        static string database;
        static string host;
        static string username;
        static string password;

        /// <summary>
        /// Updates the connection string.
        /// </summary>
        static void UpdateConnectionString()
        {
            connectionStringBuilder.Server = Host;
            connectionStringBuilder.Database = Database;
            connectionStringBuilder.UserID = Username;
            connectionStringBuilder.Password = Password;
            connectionStringBuilder.Pooling = true;
            connectionStringBuilder.DefaultCommandTimeout = CommandTimout;
            connectionStringBuilder.Port = 3306;
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
        /// Gets or sets the hostname or IP address of the PostrgreSQL server.
        /// </summary>
        public static string Host
        {
            get => host;
            set
            {
                if (value!= host)
                {
                    host = value ?? "";

                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the database username.
        /// </summary>
        public static string Username
        {
            get => username;
            set
            {
                if (value != username)
                {
                    username = value ?? "";

                    UpdateConnectionString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the database password.
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
        /// Configures the database connection.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Configure(string host, string database, string username, string password)
        {
            Database = database;
            Host = host;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Tests the connection with the database.
        /// </summary>
        public static bool Test()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a connection with the database.
        /// </summary>
        public static MySqlConnection Connection => new MySqlConnection(ConnectionString);

        /// <summary>
        /// Executes the specified query and returns the resulting data reader.
        /// </summary>
        /// <param name="commandText">The SQL command to execute.</param>
        /// <returns></returns>
        public static MySqlDataReader GetDataReader(string commandText, params MySqlParameter[] parameters)
        {
            using (var connection = Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    return command.ExecuteReader();
                }
            }
        }

        /// <summary>
        /// Executes a query and fills a <see cref="DataTable"/> with the resulting record set.
        /// </summary>
        /// <param name="commandText">The SQL command to execute.</param>
        /// <returns></returns>
        public static DataTable GetTable(string commandText, params MySqlParameter[] parameters)
        {
            var dataTable = new DataTable();

            using (var connection = Connection)
            {
                connection.Open();

                dataTable.BeginLoadData();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                dataTable.EndLoadData();
            }

            return dataTable;
        }

        /// <summary>
        /// Executes a non query command and returns the number of rows affected. 
        /// </summary>
        /// <param name="commandText">The SQL command to execute.</param>
        /// <param name="parameters"></param>
        /// <returns>The number of rows affected.</returns>
        public static long ExecuteNonQuery(string commandText, params MySqlParameter[] parameters)
        {
            using (var connection = Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        public static long ExecuteInsert(string commandText, params MySqlParameter[] parameters)
        {
            using (var connection = Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    command.ExecuteNonQuery();

                    return command.LastInsertedId;
                }
            }
        }

        public static int ExecuteInt(string commandText, params MySqlParameter[] parameters) =>
            Convert.ToInt32(ExecuteScalar(commandText, parameters));

        public static long ExecuteLong(string commandText, params MySqlParameter[] parameters) =>
            Convert.ToInt64(ExecuteScalar(commandText, parameters));

        public static string ExecuteString(string commandText, params MySqlParameter[] parameters) =>
            Convert.ToString(ExecuteScalar(commandText, parameters));

        /// <summary>
        /// Executes a scalar query and returns the resulting scalar value.
        /// </summary>
        /// <param name="commandText">The SQL command to execute</param>
        /// <returns></returns>
        public static string ExecuteScalar(string commandText, params MySqlParameter[] parameters)
        {
            object scalar = null;

            using (var connection = Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    scalar = command.ExecuteScalar();
                }
            }

            return scalar == null ? "" : scalar.ToString();
        }

        /// <summary>
        /// Executes the specified database action.
        /// </summary>
        /// <param name="databaseAction">The SQL command to execute.</param>
        public static void Execute(DatabaseAction databaseAction)
        {
            if (databaseAction != null)
            {
                //try
                //{
                    using (var connection = Connection)
                    {
                        connection.Open();

                        databaseAction(connection);
                    }
                //}
                //catch { }
            }
        }

        /// <summary>
        /// Executes the specified database action.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the action.</typeparam>
        /// <param name="databaseAction">The SQL command to execute.</param>
        /// <returns>The action result.</returns>
        public static T Execute<T>(DatabaseAction<T> databaseAction)
        {
            if (databaseAction != null)
            {
                using (var connection = Connection)
                {
                    connection.Open();

                    return databaseAction(connection);
                }
            }

            return default;
        }
    }
}