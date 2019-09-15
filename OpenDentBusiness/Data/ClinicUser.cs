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
    public class ClinicUser : DataRecordBase
    {
        private static readonly IDataRecordCacheBase<ClinicUser> cache =
            new DataRecordCacheBase<ClinicUser>("SELECT * FROM `clinic_users`", FromReader);

        public long ClinicId;
        public long UserId;

        /// <summary>
        /// Constructs a new instance of the <see cref="ClinicUser"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="ClinicUser"/> instance.</returns>
        private static ClinicUser FromReader(MySqlDataReader dataReader) =>
            new ClinicUser(
                dataReader.GetInt64(0),
                dataReader.GetInt64(1));

        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicUser"/> class.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <param name="userId">The ID of the user.</param>
        public ClinicUser(long clinicId, long userId)
        {
            ClinicId = clinicId;
            UserId = userId;
        }

        /// <summary>
        /// Gets all clinics for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of clinic users.</returns>
        public static IEnumerable<ClinicUser> GetForUser(long userId) =>
            cache.SelectMany(userClinic => userClinic.UserId == userId);

        /// <summary>
        /// Gets all users for the specified clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>A list of clinic users.</returns>
        public static IEnumerable<ClinicUser> GetForClinic(long clinicId) =>
            cache.SelectMany(userClinic => userClinic.ClinicId == clinicId);

        /// <summary>
        /// Inserts the specified clinic user in the database.
        /// </summary>
        /// <param name="userClinic">The clinic user.</param>
        public static void Insert(ClinicUser userClinic) =>
            DataConnection.ExecuteNonQuery("INSERT IGNORE INTO `clinic_users` VALUES (?clinic_id, ?user_id)",
                new MySqlParameter("clinic_id", userClinic.ClinicId),
                new MySqlParameter("user_id", userClinic.UserId));

        /// <summary>
        /// Deletes the specified clinic user.
        /// </summary>
        /// <param name="userClinic">The clinic user.</param>
        public static void Delete(ClinicUser userClinic) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `clinic_users` WHERE `clinic_id` = ?clinic_id AND `user_id` = ?user_id",
                new MySqlParameter("clinic_id", userClinic.ClinicId),
                new MySqlParameter("user_id", userClinic.UserId));
    }
}
