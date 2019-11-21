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
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Uses to keep track of alerts that have been read per user.
    ///     </para>
    /// </summary>
    public class AlertRead : DataRecordBase
    {
        public long AlertItemId;
        public long UserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertRead"/> class.
        /// </summary>
        public AlertRead()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertRead"/> class.
        /// </summary>
        public AlertRead(long alertItemId, long userId)
        {
            AlertItemId = alertItemId;
            UserId = userId;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AlertRead"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AlertRead"/> instance.</returns>
        private static AlertRead FromReader(MySqlDataReader dataReader) =>
            new AlertRead(
                (long)dataReader[0], 
                (long)dataReader[1]);
       
        /// <summary>
        /// Gets a list of all alerts read by the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of alerts read.</returns>
        public static List<AlertRead> GetByUser(long userId) =>
            SelectMany("SELECT * FROM `alerts_read` WHERE `user_id` = " + userId, FromReader);

        /// <summary>
        ///     <para>
        ///         Gets a list of all alerts read by the specified user.
        ///     </para>
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="alertItemIds">The ID's of the alert items to check.</param>
        /// <returns>A list of alerts read.</returns>
        public static List<AlertRead> GetByUser(long userId, List<long> alertItemIds)
        {
            if (alertItemIds == null || alertItemIds.Count == 0)
                return new List<AlertRead>();

            return SelectMany(
                "SELECT * FROM `alerts_read` " +
                "WHERE `user_id` = " + userId + " " +
                "AND `alert_item_id` IN (" + string.Join(",", alertItemIds) + ")", 
                FromReader);
        }

        /// <summary>
        /// Inserts the specified alert read into the database.
        /// </summary>
        /// <param name="alertRead"></param>
        /// <returns></returns>
        public static long Insert(AlertRead alertRead) =>
            DataConnection.ExecuteNonQuery(
                "INSERT IGNORE INTO `alerts_read` (" + alertRead.AlertItemId + ", " + alertRead.UserId + ")");

        /// <summary>
        /// Deletes the specified alert read from the database.
        /// </summary>
        /// <param name="alertRead"></param>
        public static void Delete(AlertRead alertRead) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alerts_read` " +
                "WHERE `alert_item_id` = " + alertRead.AlertItemId + " " +
                "AND `user_id` = " + alertRead.UserId);
    }
}
