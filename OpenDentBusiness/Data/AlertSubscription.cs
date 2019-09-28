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
    /// Subscribes a user and optional clinic to specifc alert types.
    /// Users will not get alerts unless they have an entry in this table.
    /// </summary>
    public class AlertSubscription : DataRecordBase
    {
        public long UserId;
        public long? ClinicId;
        public long AlertCategoryId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertSubscription"/> class.
        /// </summary>
        public AlertSubscription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertSubscription"/> class.
        /// </summary>
        public AlertSubscription(long userNum, long clinicNum, long alertCatNum)
        {
            UserId = userNum;
            ClinicId = clinicNum;
            AlertCategoryId = alertCatNum;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AlertRead"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AlertRead"/> instance.</returns>
        private static AlertSubscription FromReader(MySqlDataReader dataReader)
        {
            return new AlertSubscription
            {
                UserId = (long)dataReader[0],
                ClinicId = dataReader[1] as long?,
                AlertCategoryId = (long)dataReader[1]
            };
        }

        /// <summary>
        /// Deletes all subscriptions for the specified users and reinserts the specified subscriptions.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="alertSubscriptions"></param>
        public static void DeleteAndInsertForSuperUsers(List<User> users, List<AlertSubscription> alertSubscriptions)
        {
            if (users == null || users.Count == 0)  return;
            
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alert_subscriptions` WHERE `user_id` IN(" + string.Join(", ", users.Select(x => x.Id)) + ")");

            foreach (var alertSubscription in alertSubscriptions)
            {
                Insert(alertSubscription);
            }
        }

        /// <summary>
        /// Gets a list of all alert subscriptions from the database.
        /// </summary>
        /// <returns>A list of alert subscriptions.</returns>
        public static List<AlertSubscription> All() => 
            SelectMany("SELECT * FROM `alert_subscriptions`", FromReader);

        /// <summary>
        /// Gets a list of all alert subscriptions for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="clinicId">The ID of the clinic (optional).</param>
        /// <returns>A list of alert subscriptions.</returns>
        public static List<AlertSubscription> GetByUser(long userId, long? clinicId = null)
        {
            var commandText = "SELECT * FROM `alert_subscriptions` WHERE `user_id` = " + userId;
            if (clinicId.HasValue)
            {
                commandText += " AND `clinic_id` = " + clinicId.Value;
            }

            return SelectMany(commandText, FromReader);
        }

        /// <summary>
        /// Inserts the specified alert subscription into the database.
        /// </summary>
        /// <param name="alertSubscription">The alert subscription.</param>
        public static void Insert(AlertSubscription alertSubscription) =>
            DataConnection.ExecuteNonQuery(
                "INSERT IGNORE INTO `alert_subscriptions` (`user_id`, `clinic_id`, `alert_category_id`) " +
                "VALUES (?user_id, ?clinic_id, ?alert_category_id)",
                    new MySqlParameter("user_id", alertSubscription.UserId),
                    new MySqlParameter("clinic_id", alertSubscription.ClinicId.HasValue ? (object)alertSubscription.ClinicId.Value : DBNull.Value),
                    new MySqlParameter("alert_category_id", alertSubscription.AlertCategoryId));

        /// <summary>
        /// Deletes the specified alert subscription from the database.
        /// </summary>
        /// <param name="alertSubscription">The alert subscription.</param>
        public static void Delete(AlertSubscription alertSubscription) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alert_subscriptions` WHERE `user_id` = " + alertSubscription.UserId + " AND `clinic_id` = ?clinic_id AND `alert_category_id` = " + alertSubscription.AlertCategoryId,
                    new MySqlParameter("clinic_id", alertSubscription.ClinicId.HasValue ? (object)alertSubscription.ClinicId.Value : DBNull.Value));
    }
}
