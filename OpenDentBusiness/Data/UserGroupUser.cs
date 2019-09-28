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
using System.Linq;

namespace OpenDentBusiness
{
    public class UserGroupUser : DataRecordBase
    {
        private static IDataRecordCacheBase<UserGroupUser> cache =
            new DataRecordCacheBase<UserGroupUser>("SELECT * FROM `user_group_users`", FromReader);

        public long UserGroupId;
        public long UserId;

        /// <summary>
        /// Constructs a new instance of the <see cref="UserGroupUser"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="UserGroupUser"/> instance.</returns>
        static UserGroupUser FromReader(MySqlDataReader dataReader) =>
            new UserGroupUser(
                dataReader.GetInt64(0), 
                dataReader.GetInt64(1));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupUser"/> class.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        public UserGroupUser(long userGroupId, long userId)
        {
            UserGroupId = userGroupId;
            UserId = userId;
        }

        /// <summary>
        /// Gets a list of all user group users for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of user group users.</returns>
        public static IEnumerable<UserGroupUser> GetByUser(long userId) => 
            cache.Where(userGroupUser => userGroupUser.UserId == userId);

        /// <summary>
        /// Gets a list of all user group users for the specified user group.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>A list of user group users.</returns>
        public static IEnumerable<UserGroupUser> GetByUserGroup(long userGroupId) =>
            cache.Where(userGroupUser => userGroupUser.UserGroupId == userGroupId);

        /// <summary>
        /// Inserts the specified user group user into the database.
        /// </summary>
        /// <param name="userGroupUser"></param>
        public static void Insert(UserGroupUser userGroupUser) =>
            DataConnection.ExecuteNonQuery(
                "INSERT IGNORE INTO `user_group_users` (?user_group_id, ?user_id)",
                    new MySqlParameter("user_group_id", userGroupUser.UserGroupId),
                    new MySqlParameter("user_id", userGroupUser.UserId));

        /// <summary>
        /// Creates a new user group user with the specified details and inserts it into the database.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        public static void Insert(long userGroupId, long userId) =>
            Insert(new UserGroupUser(userGroupId, userId));

        /// <summary>
        /// Delete the specified user group user from the database.
        /// </summary>
        /// <param name="userGroupUser">The user group user.</param>
        public static void Delete(UserGroupUser userGroupUser) =>
           DataConnection.ExecuteNonQuery(
               "DELETE FROM `user_group_users` WHERE `user_group_id` = ?user_group_id AND `user_id` = ?user_id",
                   new MySqlParameter("user_group_id", userGroupUser.UserGroupId),
                   new MySqlParameter("user_id", userGroupUser.UserId));

        /// <summary>
        /// Synchronizes the list of user groups for the specified user in the database.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userGroupIds">The ID's of the user groups the user is a member of.</param>
        public static void Synchronize(User user, List<long> userGroupIds)
        {
            var userGroupUsers = GetByUser(user.Id);

            foreach (int userGroupId in userGroupIds)
            {
                if (!userGroupUsers.Any(userGroupUser => userGroupUser.UserGroupId == userGroupId))
                {
                    Insert(new UserGroupUser(userGroupId, user.Id));
                }
            }

            foreach (var userGroupUser in userGroupUsers)
            {
                if (!userGroupIds.Contains(userGroupUser.UserGroupId))
                {
                    Delete(userGroupUser);
                }
            }
        }
    }
}
