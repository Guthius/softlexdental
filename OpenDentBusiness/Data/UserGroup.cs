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
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// A group of users. Security permissions are determined by the usergroup of a user.
    /// </summary>
    public class UserGroup : DataRecord
    {
        /// <summary>
        /// The description of the group.
        /// </summary>
        public string Description;

        /// <summary>
        /// Returns a string representation of the user group.
        /// </summary>
        public override string ToString() => Description;

        /// <summary>
        /// Constructs a new instance of the <see cref="UserGroup"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="UserGroup"/> instance.</returns>
        static UserGroup FromReader(MySqlDataReader dataReader)
        {
            return new UserGroup
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"])
            };
        }

        /// <summary>
        /// Gets a list of all user groups.
        /// </summary>
        /// <returns>A list of user groups.</returns>
        public static List<UserGroup> All() =>
            SelectMany("SELECT * FROM `user_groups`", FromReader);

        /// <summary>
        /// Gets the user group with the specified ID.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>The user group with the specified ID.</returns>
        public static UserGroup GetById(long userGroupId) =>
            SelectOne("SELECT * FROM `user_groups` WHERE `id` = " + userGroupId, FromReader);

        /// <summary>
        /// Gets a list of all user groups the user with the specified ID is a member of.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of user groups.</returns>
        public static List<UserGroup> GetByUser(long userId)
        {
            // TODO: Implement me

            // return GetList(UserGroupAttaches.GetForUser(userNum).Select(x => x.UserGroupNum).ToList(), includeCEMT);

            return new List<UserGroup>();
        }

        /// <summary>
        /// Checks whether atleast one of the user groups in the last has the <see cref="Permissions.SecurityAdmin"/> permission.
        /// </summary>
        public static bool IsAdminGroup(List<long> userGroupIdList)
        {
            if (userGroupIdList.Any(userGroupId => UserGroupPermission.HasPermission(userGroupId, Permissions.SecurityAdmin, null)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts the specified user group into the database.
        /// </summary>
        /// <param name="userGroup">The user group.</param>
        /// <returns>The ID assigned to the user group.</returns>
        public static long Insert(UserGroup userGroup) =>
            userGroup.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `user_groups` (`description`) VALUES (?description)", 
                    new MySqlParameter("description", userGroup.Description));

        /// <summary>
        /// Updates the specified user group in the database.
        /// </summary>
        /// <param name="userGroup">The user group.</param>
        public static void Update(UserGroup userGroup) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `user_groups` SET `description` = ?description WHERE `id` = ?id", 
                    new MySqlParameter("description", userGroup.Description), 
                    new MySqlParameter("id", userGroup.Id));

        /// <summary>
        /// Deletes the user group with the specified ID from the database.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        public static void Delete(long userGroupId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `user_groups` WHERE `id` = " + userGroupId);

        /// <summary>
        /// Deletes the specified user group from the database.
        /// </summary>
        /// <param name="userGroup">The user group.</param>
        public static void Delete(UserGroup userGroup)
        {
            // TODO: Fix me

            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM usergroupattach WHERE user_group_id = @user_group_id",
                        new MySqlParameter("user_group_id", userGroup.Id));

            if (count > 0) throw new DataException("Must move users to another group first.");

            if (Preference.GetLong(PreferenceName.SecurityGroupForStudents) == userGroup.Id)
                throw new DataException("Group is the default group for students and cannot be deleted. Change the default student group before deleting.");

            if (Preference.GetLong(PreferenceName.SecurityGroupForInstructors) == userGroup.Id)
                throw new DataException("Group is the default group for instructors and cannot be deleted. Change the default instructors group before deleting.");

            DataConnection.ExecuteNonQuery("DELETE FROM user_groups WHERE id = @user_group_id", new MySqlParameter("user_group_id", userGroup.Id));
            DataConnection.ExecuteNonQuery("DELETE FROM grouppermission WHERE user_group_id = @user_group_id", new MySqlParameter("user_group_id", userGroup.Id));
        }
    }
}