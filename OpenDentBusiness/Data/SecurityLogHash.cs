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
using System.Security.Cryptography;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores a SHA-256 hash of a <see cref="SecurityLog"/>. Used to detect security log tampering.
    /// </summary>
    public class SecurityLogHash : DataRecordBase
    {
        private static readonly SHA256 sha256 = SHA256.Create(); // TODO: Is this thread safe?

        /// <summary>
        /// The ID of the security log.
        /// </summary>
        public long SecurityLogId;
        
        /// <summary>
        /// The SHA-256 hash of the security log.
        /// </summary>
        public string Hash;

        /// <summary>
        /// Inserst the specified security log hash into the database.
        /// </summary>
        /// <param name="securityLogHash">The security log hash.</param>
        public static void Insert(SecurityLogHash securityLogHash) =>
            DataConnection.ExecuteNonQuery(
                "INSERT INTO `security_log_hashes` (`security_log_id`, `hash`) VALUES (?security_log_id, ?hash) ON DUPLICATE KEY UPDATE `hash` = ?hash",
                    new MySqlParameter("security_log_id", securityLogHash.SecurityLogId),
                    new MySqlParameter("hash", securityLogHash.Hash ?? ""));
        
        /// <summary>
        /// Calculates the hash of the specified security log and inserts it into the database.
        /// </summary>
        /// <param name="securityLog">The security log to hash.</param>
        public static void Insert(SecurityLog securityLog) =>
            Insert(new SecurityLogHash
            {
                SecurityLogId = securityLog.Id,
                Hash = GetHashString(securityLog)
            });

        /// <summary>
        /// Calculates the SHA256 hash of the specified security log.
        /// </summary>
        /// <param name="securityLog">The security log.</param>
        /// <returns>The hash of the security log.</returns>
        public static string GetHashString(SecurityLog securityLog)
        {
            // TODO: Move this to the SecurityLog class...

            string logString =
                securityLog.Event +
                securityLog.UserId +
                securityLog.LogDate.ToString("yyyyMMddHHmmss") +
                securityLog.LogMessage +
                securityLog.PatientId;

            if (securityLog.ExternalDate.HasValue)
            {
                logString += securityLog.ExternalDate.Value.ToString("yyyyMMddHHmmss");
            }

            return Convert.ToBase64String(
                sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(logString)));
        }
    }
}
