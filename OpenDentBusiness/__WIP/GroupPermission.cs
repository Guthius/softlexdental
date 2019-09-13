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
    public class GroupPermission : DataRecordBase
    {
        private static readonly IDataRecordCacheBase<GroupPermission> cache =
            new DataRecordCacheBase<GroupPermission>("SELECT * FROM `user_group_permissions`", FromReader);

        /// <summary>
        /// The ID of the user group the permission is granted to.
        /// </summary>
        public long UserGroupId;

        /// <summary>
        /// The permission.
        /// </summary>
        public string Permission;

        /// <summary>
        /// Only granted permission if newer than this date.  Can be Minimum (01-01-0001) to always grant permission.
        /// </summary>
        public DateTime? NewerDate;

        /// <summary>
        /// Can be 0 to always grant permission.  Otherwise, only granted permission if item is newer than the given number of days.  1 would mean only if entered today.
        /// </summary>
        public int? NewerDays;

        /// <summary>
        /// Optional foreign key ID to a record in another table.
        /// </summary>
        public long? ExternalId;

        static GroupPermission FromReader(MySqlDataReader dataReader)
        {
            return new GroupPermission
            {
                UserGroupId = (long)dataReader["user_group_id"],
                Permission = (string)dataReader["permission"],
                NewerDate = dataReader["newer_date"] as DateTime?,
                NewerDays = dataReader["newer_days"] as int?,
                ExternalId = dataReader["external_id"] as long?
            };
        }

        public static void Insert(GroupPermission groupPermission)
        {
            if (groupPermission.NewerDate.HasValue && groupPermission.NewerDays.HasValue)
            {
                throw new Exception("Date or days can be set, but not both.");
            }

            if (groupPermission.Permission == Permissions.SecurityAdmin)
            {
                // Make sure there are no hidden users in the group that is about to get the Security Admin permission.
                var count = DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `users` " +
                    "INNER JOIN `user_group_users` ON `user_group_users`.`user_id` = `users`.`id` " +
                    "WHERE `users`.`hidden` = 1 " +
                    "AND `user_group_users`.`user_group_id` = " + groupPermission.UserGroupId);

                if (count > 0)
                {
                    throw new Exception(
                        "The Security Admin permission cannot be given to a user group with hidden users.");
                }
            }

            DataConnection.ExecuteNonQuery(
                "INSERT INTO `user_group_permissions` (?user_group_id, ?permission, ?newer_date, ?newer_days, ?external_id) " +
                "ON DUPLICATE KEY UPDATE `newer_date` = ?newer_date, `newer_days` = ?newer_days",
                    new MySqlParameter("user_group_id", groupPermission.UserGroupId),
                    new MySqlParameter("permission", groupPermission.Permission ?? ""),
                    new MySqlParameter("newer_date", groupPermission.NewerDate.HasValue ? (object)groupPermission.NewerDate.Value : DBNull.Value),
                    new MySqlParameter("newer_days", groupPermission.NewerDays.HasValue ? (object)groupPermission.NewerDays.Value : DBNull.Value),
                    new MySqlParameter("external_id", groupPermission.ExternalId.HasValue ? (object)groupPermission.ExternalId.Value : DBNull.Value));
        }

        /// <summary>
        /// Deletes GroupPermissions based on primary key.
        /// Do not call this method unless you have checked specific dependencies first.
        /// E.g. after deleting this permission, there will still be a security admin user.
        /// This method is only called from the CEMT sync.
        /// RemovePermission should probably be used instead.
        /// </summary>
        public static void Delete(GroupPermission groupPermission) => // TODO: Delete from the local cache...
            DataConnection.ExecuteNonQuery("DELETE FROM `group_permissions` WHERE `user_group_id` = " + groupPermission.UserGroupId + " AND `permission` = ?permission",
                new MySqlParameter("permission", groupPermission.Permission ?? ""));

        /// <summary>
        /// Gets the specified permission for the specified user group. Returns null if the user 
        /// group does not have the permission.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        public static GroupPermission GetPermission(long userGroupId, string permission) =>
            cache.SelectOne(groupPermission => groupPermission.UserGroupId == userGroupId && groupPermission.Permission == permission);

        /// <summary>
        /// Gets all permissions of the specified user group.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>All permissions of the specified user group.</returns>
        public static IEnumerable<GroupPermission> GetPermissions(long userGroupId) =>
            cache.SelectMany(groupPermission => groupPermission.UserGroupId == userGroupId);

        /// <summary>
        /// Checks whether the specified user group has the specified permission.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public static bool HasPermission(long userGroupId, string permission, long? externalId) =>
            cache.Any(
                groupPermission => 
                    groupPermission.UserGroupId == userGroupId && 
                    groupPermission.Permission == permission && 
                    groupPermission.ExternalId == externalId);

        /// <summary>
        /// Checks whether the user has the specified permission
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public static bool HasPermission(User user, string permission, long? externalId) =>
            cache.Any(
                groupPermission =>
                    groupPermission.Permission == permission &&
                    groupPermission.ExternalId == externalId &&
                    user.IsInUserGroup(groupPermission.UserGroupId));

        /// <summary>
        /// The maximum number of days allowed for the NewerDays column.
        /// Setting a NewerDays to a value higher than this will cause an exception to be thrown in the program.
        /// There is a DBM that will correct invalid NewerDays in the database.
        /// </summary>
        public const double NewerDaysMax = 3000;

        public static IEnumerable<GroupPermission> GetByUserGroups(List<long> userGroupIds, string permission = "")
        {
            if (string.IsNullOrEmpty(permission))
            {
                return cache.Where(groupPermission => userGroupIds.Contains(groupPermission.UserGroupId));
            }

            return cache.Where(groupPermission => groupPermission.Permission == permission && userGroupIds.Contains(groupPermission.UserGroupId));
        }

        public static string GetDescription(string permission) => permission;

        public static void RemovePermission(long userGroupId, string permission)
        {
            if (permission == Permissions.SecurityAdmin)
            {
                var count =
                    DataConnection.ExecuteLong(
                        "SELECT COUNT(*) FROM (SELECT DISTINCT user_group_permissions.user_group_id " +
                        "FROM user_group_permissions " +
                        "INNER JOIN user_group_users ON user_group_users.user_group_id = user_group_permissions.user_group_id " +
                        "WHERE user_group_permissions.permission = '" + permission + "' " +
                        "AND user_group_permissions.user_group_id != " + userGroupId + ")");

                if (count == 0)
                {
                    throw new Exception(
                        "There must always be at least one user in a user group that has the Security Admin permission.");
                }
            }

            if (permission == Permissions.Reports)
            {
                DataConnection.ExecuteNonQuery("DELETE FROM `user_group_permissions` WHERE `user_group_id` = " + userGroupId + " AND `permission` = ?permission AND `external_id` IS NULL",
                    new MySqlParameter("permission", permission ?? ""));
            }
            else
            {
                DataConnection.ExecuteNonQuery("DELETE FROM `user_group_permissions` WHERE `user_group_id` = " + userGroupId + " AND `permission` = ?permission",
                    new MySqlParameter("permission", permission ?? ""));
            }
        }

        public static IEnumerable<GroupPermission> GetPermissionsForReports() => 
            cache.Where(groupPermission => 
                groupPermission.Permission == Permissions.Reports && 
                groupPermission.ExternalId != null);
        

        public static DateTime? GetDateRestrictedForPermission(string permission, List<long> userGroupIds)
        { 
            DateTime GetCurrentDate(DateTime? now)
            {
                if (!now.HasValue)
                {
                    return MiscData.GetNowDateTime().Date;
                }

                return now.Value.Date;
            }

            DateTime? currentDate = DateTime.MinValue;

            var groupPermissions = GetByUserGroups(userGroupIds, permission);
            var groupPermission = groupPermissions.OrderBy(x =>
            {
                if (!x.NewerDays.HasValue && !x.NewerDate.HasValue)
                    return DateTime.MinValue;

                if (!x.NewerDays.HasValue)
                    return x.NewerDate;
                
                return GetCurrentDate(currentDate).AddDays(-x.NewerDays.Value);
            }).FirstOrDefault();

            if (groupPermission != null && (groupPermission.NewerDate.HasValue || groupPermission.NewerDays.HasValue))
            {
                if (groupPermission.NewerDate.HasValue)
                {
                    return groupPermission.NewerDate.Value;
                }

                return GetCurrentDate(currentDate).AddDays(-groupPermission.NewerDays.Value);
            }

            return null;
        }
    }
}
