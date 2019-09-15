/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores an ongoing record of database activity for security purposes.
    /// </summary>
    public class SecurityLog : DataRecord
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public long UserId;

        /// <summary>
        /// The ID of the patient.
        /// </summary>
        public long? PatientId;

        /// <summary>
        /// The name of the event that created the security log.
        /// </summary>
        public string Event;

        /// <summary>
        /// The date and time of the entry.
        /// </summary>
        public DateTime LogDate;

        /// <summary>
        /// The description of exactly what was done.
        /// </summary>
        public string LogMessage;

        public string ComputerName;
        public string Source;
        public long? ExternalId;
        public DateTime? ExternalDate;

        /// <summary>
        /// Gets the hash of the log entry.
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="SecurityLog"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="SecurityLog"/> instance.</returns>
        private static SecurityLog FromReader(MySqlDataReader dataReader)
        {
            return new SecurityLog
            {
                Id = (long)dataReader["id"],
                UserId = (long)dataReader["user_id"],
                PatientId = dataReader["patient_id"] as long?,
                Event = (string)dataReader["event"],
                LogDate = (DateTime)dataReader["log_date"],
                LogMessage = (string)dataReader["log_message"],
                ComputerName = (string)dataReader["computer_name"],
                Source = (string)dataReader["source"],
                ExternalId = dataReader["external_id"] as long?,
                ExternalDate = dataReader["external_date"] as DateTime?,
                Hash = dataReader["hash"] as string
            };
        }

        /// <summary>
        /// Inserts the specified security log into the database.
        /// </summary>
        /// <param name="securityLog"></param>
        /// <returns>The ID assigned to the security log.</returns>
        public static long Insert(SecurityLog securityLog)
        {
            securityLog.Id = DataConnection.ExecuteInsert(
                "INSERT INTO security_logs (`user_id`, `patient_id`, `event`, `log_date`, `log_message`, `computer_name`, `source`, `external_id`, `external_date`) " +
                "VALUES (?user_id, ?patient_id, ?event, ?log_date, ?log_message, ?computer_name, ?source, ?external_id, ?external_date)",
                    new MySqlParameter("user_id", securityLog.UserId),
                    new MySqlParameter("patient_id", securityLog.PatientId.HasValue ? (object)securityLog.PatientId.Value : DBNull.Value),
                    new MySqlParameter("event", securityLog.Event ?? ""),
                    new MySqlParameter("log_date", securityLog.LogDate),
                    new MySqlParameter("log_message", securityLog.LogMessage ?? ""),
                    new MySqlParameter("computer_name", securityLog.ComputerName ?? ""),
                    new MySqlParameter("source", securityLog.Source ?? ""),
                    new MySqlParameter("external_id", securityLog.ExternalId.HasValue ? (object)securityLog.ExternalId.Value : DBNull.Value),
                    new MySqlParameter("external_date", securityLog.ExternalDate.HasValue ? (object)securityLog.ExternalDate.Value : DBNull.Value));

            SecurityLogHash.Insert(securityLog);

            return securityLog.Id;
        }
            
        public static List<SecurityLog> Find(long? patientId, List<string> events, long externalId) =>
            Find(patientId, events, new List<long>() { externalId });

        public static List<SecurityLog> Find(long? patientId, List<string> events, List<long> externalIds)
        {
            var filters = new List<string>();

            if (events != null && events.Count > 0)
                filters.Add("(" + string.Join(", ", events.Select(e => "`event` = '" + MySqlHelper.EscapeString(e) + "'")) + ")");

            if (externalIds != null && externalIds.Count > 0)
                filters.Add("`external_id` IN (" + string.Join(", ", externalIds) + ")");

            if (patientId.HasValue)
                filters.Add("`patient_id` IN (" +
                        string.Join(",", PatientLinks.GetPatNumsLinkedToRecursive(patientId.Value, PatientLinkType.Merge)) + ")");

            var commandText = "SELECT `sl`.*, `slh`.`hash` FROM `security_logs` `sl` LEFT JOIN `security_log_hashes` `slh` ON `slh`.`security_log_id`";
            if (filters.Count > 0)
                commandText += " WHERE " + string.Join(" AND ", filters);

            commandText += " ORDER BY `log_date`";

            return SelectMany(commandText, FromReader);
        }

        /// <summary>
        /// Gets a list of security log entries matching the specified search criteria.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="eventName"></param>
        /// <param name="previousFromDate"></param>
        /// <param name="previousToDate"></param>
        /// <param name="limit"></param>
        /// <returns>A list of log entries.</returns>
        public static List<SecurityLog> Find(long? userId, long? patientId, DateTime fromDate, DateTime toDate, string eventName, DateTime previousFromDate, DateTime previousToDate, int limit = 0)
        {
            string commandText =
                "SELECT `security_logs`.*, LName,FName,Preferred,MiddleI, `security_log_hashes`.`hash` " +
                "FROM `security_logs` " +
                "LEFT JOIN `patients` ON `patients`.`id` = `security_logs`.`patient_id` " +
                "LEFT JOIN `security_log_hashes` ON `security_log_hashes`.`security_log_id` = `security_logs`.`id` " +
                "WHERE `log_date` >= ?from_date " +
                "AND `log_date` <= ?to_date " +
                "AND `external_date` >= ?previous_from_date " +
                "AND `external_date` <= ?previous_to_date";

            var filters = new List<string>();

            if (userId.HasValue)
                filters.Add("`user_id` = " + userId.Value);

            if (patientId.HasValue)
                filters.Add("`patient_id` IN (" + string.Join(",", PatientLinks.GetPatNumsLinkedToRecursive(patientId.Value, PatientLinkType.Merge)) + ")");

            if (!string.IsNullOrEmpty(eventName))
                filters.Add("`event` = '" + MySqlHelper.EscapeString(eventName) + "'");

            if (filters.Count > 0)
                commandText += " AND " + string.Join(" AND ", filters);

            commandText += " ORDER BY `log_date` DESC";
            if (limit > 0)
            {
                commandText += " LIMIT " + limit;
            }

            return SelectMany(
                commandText, FromReader,
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)),
                    new MySqlParameter("previous_from_date", previousFromDate),
                    new MySqlParameter("previous_to_date", previousToDate.AddDays(1)));
        }

        /// <summary>
        /// Creates a new log entry with the specified details.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log entry event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <returns>The new log entry.</returns>
        private static SecurityLog MakeEntry(long? patientId, string eventName, string logMessage) =>
            MakeEntry(Security.CurrentUser.Id, patientId, eventName, logMessage, SecurityLogSource.None, null, null);

        /// <summary>
        /// Creates a new log entry with the specified details.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log entry event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <param name="source">The log entry source.</param>
        /// <param name="externalId"></param>
        /// <param name="externalDate"></param>
        /// <returns>The new log entry.</returns>
        private static SecurityLog MakeEntry(long? patientId, string eventName, string logMessage, string source, long? externalId = null, DateTime? externalDate = null) =>
            MakeEntry(Security.CurrentUser.Id, patientId, eventName, logMessage, source, externalId, externalDate);

        /// <summary>
        /// Creates a new log entry with the specified details.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log entry event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <param name="source">The log entry source.</param>
        /// <param name="externalId"></param>
        /// <param name="externalDate"></param>
        /// <returns>The new log entry.</returns>
        private static SecurityLog MakeEntry(long userId, long? patientId, string eventName, string logMessage, string source, long? externalId = null, DateTime? externalDate = null)
        {
            return new SecurityLog
            {
                UserId = userId,
                PatientId = patientId.HasValue ? (patientId == 0 ? null : patientId) : patientId, // TODO: Simplify this...
                Event = eventName,
                LogMessage = logMessage,
                ComputerName = Environment.MachineName,
                Source = source,
                ExternalId = externalId,
                ExternalDate = externalDate
            };
        }

        [Obsolete]
        public static long Write(string eventName, long? patientId, string logMessage) 
            => Write(patientId, eventName, logMessage);

        [Obsolete]
        public static long Write(string eventName, long? patientId, string logMessage, long? externalId, DateTime? externalDate) 
            => Write(patientId, eventName, logMessage, externalId, externalDate);

        /// <summary>
        /// Writes a entry to the security log.
        /// </summary>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <returns>The ID of assigned to the log entry.</returns>
        public static long Write(string eventName, string logMessage) =>
            Write(null, eventName, logMessage, SecurityLogSource.None, null, null);

        /// <summary>
        /// Writes a entry to the security log.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <returns>The ID of assigned to the log entry.</returns>
        public static long Write(long? patientId, string eventName, string logMessage) =>
            Write(patientId, eventName, logMessage, SecurityLogSource.None, null, null);

        /// <summary>
        /// Writes a entry to the security log.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <param name="source">The log source.</param>
        /// <returns>The ID of assigned to the log entry.</returns>
        public static long Write(long? patientId, string eventName, string logMessage, string source) =>
            Write(patientId, eventName, logMessage, source, null, null);

        /// <summary>
        /// Writes a entry to the security log.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <param name="externalId">The ID of the external record that relates to the log entry.</param>
        /// <param name="externalDate">Date and time relavant to the external record (e.g. creation date or modification date).</param>
        /// <returns>The ID of assigned to the log entry.</returns>
        public static long Write(long? patientId, string eventName, string logMessage, long? externalId, DateTime? externalDate) =>
            Write(patientId, eventName, logMessage, SecurityLogSource.None, externalId, externalDate);

        /// <summary>
        /// Writes a entry to the security log.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        /// <param name="source">The log source.</param>
        /// <param name="externalId">The ID of the external record that relates to the log entry.</param>
        /// <param name="externalDate">Date and time relavant to the external record (e.g. creation date or modification date).</param>
        /// <returns>The ID of assigned to the log entry.</returns>
        public static long Write(long? patientId, string eventName, string logMessage, string source, long? externalId, DateTime? externalDate) =>
            Insert(MakeEntry(patientId, eventName, logMessage, source, externalId, externalDate));

        /// <summary>
        /// Writes the specified log messages.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessages">The log messages.</param>
        public static void WriteMany(long? patientId, string eventName, List<string> logMessages)
        {
            foreach (var logMessage in logMessages)
            {
                Write(patientId, eventName, logMessage);
            }
        }

        /// <summary>
        /// Writes a log entry for a list of patients.
        /// </summary>
        /// <param name="patientIds">The ID's of the patients.</param>
        /// <param name="eventName">The log event.</param>
        /// <param name="logMessage">The log message.</param>
        public static void WriteMany(List<long> patientIds, string eventName, string logMessage)
        {
            foreach (var patientId in patientIds)
            {
                Write(patientId, eventName, logMessage);
            }
        }
    }
}
