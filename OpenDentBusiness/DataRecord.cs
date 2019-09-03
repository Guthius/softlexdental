using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public delegate T DataRecordBuilder<T>(MySqlDataReader dataReader);

    /// <summary>
    /// Represents a data record from the database with a unique ID.
    /// </summary>
    public class DataRecord : DataRecordBase
    {
        /// <summary>
        /// Gets or sets the ID of the record.
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the record is new (i.e. not saved to the database). A record is considered new when it has not been assigned a ID yet.
        /// </summary>
        public bool IsNew => Id == 0;
    }

    /// <summary>
    /// Represents a data record from the database.
    /// </summary>
    public class DataRecordBase
    {
        /// <summary>
        /// Selects a list of records from the database.
        /// </summary>
        /// <typeparam name="T">The data record type.</typeparam>
        /// <param name="commandText">The command.</param>
        /// <param name="dataRecordBuilder">Delegate to convert data from a <see cref="NpgsqlDataReader"/> to a <typeparamref name="T"/>.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>A list of records.</returns>
        protected static List<T> SelectMany<T>(string commandText, DataRecordBuilder<T> dataRecordBuilder, params MySqlParameter[] parameters)
        {
            if (dataRecordBuilder == null) throw new ArgumentNullException("dataRecordBuilder");

            List<T> records = new List<T>();

            DataConnection.Execute(connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddRange(parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = dataRecordBuilder(reader);
                            if (record != null)
                            {
                                records.Add(record);
                            }
                        }
                    }
                }
            });

            return records;
        }

        /// <summary>
        /// Selects a single record from the database.
        /// </summary>
        /// <typeparam name="T">The data record type.</typeparam>
        /// <param name="commandText">The command.</param>
        /// <param name="dataRecordBuilder">Delegate to convert data from a <see cref="NpgsqlDataReader"/> to a <typeparamref name="T"/>.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>A single record.</returns>
        public static T SelectOne<T>(string commandText, DataRecordBuilder<T> dataRecordBuilder, params MySqlParameter[] parameters)
        {
            if (dataRecordBuilder == null) throw new ArgumentNullException("dataRecordBuilder");

            return DataConnection.Execute(connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddRange(parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T record = dataRecordBuilder(reader);
                            if (record != null)
                            {
                                return record;
                            }
                        }
                    }
                }

                return default;
            });
        }
    }
}