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
    public class UserClinic : DataRecordBase
    {
        private static readonly IDataRecordCacheBase<UserClinic> cache =
            new DataRecordCacheBase<UserClinic>("SELECT * FROM `user_clinics`", FromReader);

        public long UserId;
        public long ClinicId;

        private static UserClinic FromReader(MySqlDataReader dataReader) =>
            new UserClinic(
                dataReader.GetInt64(0),
                dataReader.GetInt64(1));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClinic"/> class.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <param name="userId">The ID of the user.</param>
        public UserClinic(long clinicId, long userId)
        {
            UserId = userId;
            ClinicId = clinicId;
        }

        public static IEnumerable<UserClinic> GetForUser(long userId) =>
            cache.SelectMany(userClinic => userClinic.UserId == userId);

        public static IEnumerable<UserClinic> GetForClinic(long clinicId) =>
            cache.SelectMany(userClinic => userClinic.ClinicId == clinicId);

        public static void Insert(UserClinic userClinic) =>
            DataConnection.ExecuteNonQuery("INSERT IGNORE INTO `user_clinics` VALUES (?user_id, ?clinic_id)",
                new MySqlParameter("user_id", userClinic.UserId),
                new MySqlParameter("clinic_id", userClinic.ClinicId));

        public static void Delete(UserClinic userClinic) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `user_clinics` WHERE `user_id` = ?user_id AND `clinic_id` = ?clinic_id",
                new MySqlParameter("user_id", userClinic.UserId),
                new MySqlParameter("clinic_id", userClinic.ClinicId));
    }
}