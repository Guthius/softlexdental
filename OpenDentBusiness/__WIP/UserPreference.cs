/**
* Copyright (C) 2019 Dental Stars SRL
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

namespace OpenDentBusiness
{
    public class UserPreference : DataRecordBase
    {
        static readonly IDataRecordCacheBase<UserPreference> cache =
            new DataRecordCacheBase<UserPreference>("SELECT * FROM `user_preferences`", FromReader);

        // TODO: Ideally we don't want the entire cache to be loaded at startup.
        //       We should make a cache that can respond to events, that way we could
        //       make the above cache fill only after a succesful login and only with
        //       the preferences of the logged in user.
        //
        //        For example something like:
        //
        // static UserPreference()
        // {
        //     cache = new DynamicDataRecordCacheBase<UserPreference>(FromReader);
        //     cache.On(EventName.Login, (c, args) =>
        //     {
        //         if (args.Length > 0 && args[0] is int userId)
        //         {
        //             c.Fill("SELECT * FROM `user_preferences` WHERE `user_id` = " + userId);
        //         }
        //     });
        //     cache.On(EventName.Logout, (c, args) =>
        //     {
        //         c.Clear();
        //     });
        // }

        public long? ClinicId;
        public long UserId;

        /// <summary>
        /// The preference key.
        /// </summary>
        public string Key;

        /// <summary>
        /// The preference value.
        /// </summary>
        public string Value;

        /// <summary>
        /// Reads a preference from the specified data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>The preference.</returns>
        static UserPreference FromReader(MySqlDataReader dataReader)
        {
            var userPreference = new UserPreference
            {
                UserId = dataReader.GetInt64("user_id"),
                Key = Convert.ToString(dataReader["key"]),
                Value = Convert.ToString(dataReader["value"])
            };

            if (dataReader.IsDBNull(0))
            {
                userPreference.ClinicId = dataReader.GetInt64(0);
            }

            return userPreference;
        }

        public static UserPreference GetByKey(long userId, string preferenceKey) =>
            cache.SelectOne(p => p.ClinicId == null && p.UserId == userId && p.Key == preferenceKey);

        public static UserPreference GetByKey(long clinicId, long userId, string preferenceKey) =>
            cache.SelectOne(p => p.ClinicId == clinicId && p.UserId == userId && p.Key == preferenceKey) ??
                GetByKey(userId, preferenceKey);

        public static string GetString(long userId, string preferenceKey, string defaultValue = "") =>
            GetByKey(userId, preferenceKey)?.Value ?? defaultValue;

        public static string GetString(long clinicId, long userId, string preferenceKey, string defaultValue = "") =>
            GetByKey(userId, clinicId, preferenceKey)?.Value ?? defaultValue;


        /// <summary>
        /// Inserts the specified user preference into the database.
        /// </summary>
        /// <param name="userPreference">The user preference to insert.</param>
        public static void Insert(UserPreference userPreference)
        {
            DataConnection.ExecuteNonQuery(
                "INSERT INTO `user_preferences` VALUES (?clinic_id, ?user_id, ?key, ?value) ON DUPLICATE KEY UPDATE `value` = ?value",
                    new MySqlParameter("clinic_id", userPreference.ClinicId.HasValue ? (object)userPreference.ClinicId.Value : DBNull.Value),
                    new MySqlParameter("user_id", userPreference.UserId),
                    new MySqlParameter("key", userPreference.Key ?? ""),
                    new MySqlParameter("value", userPreference.Value ?? ""));
        }

        /// <summary>
        /// Deletes the specified user preference from the database.
        /// </summary>
        /// <param name="userPreference">The user preference to delete.</param>
        public static void Delete(UserPreference userPreference)
        {
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `user_preferences` WHERE `clinic_id` = ?clinic_id AND `user_id` = ?user_id AND `key` = ?key",
                    new MySqlParameter("clinic_id", userPreference.ClinicId.HasValue ? (object)userPreference.ClinicId.Value : DBNull.Value),
                    new MySqlParameter("user_id", userPreference.UserId),
                    new MySqlParameter("key", userPreference.Key ?? ""));
        }
    }
}
