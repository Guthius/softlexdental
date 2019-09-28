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
    public class User : DataRecord
    {
        /// <summary>
        /// Default clinic for this user.  
        /// It causes new patients to default to this clinic when entered by this user.  
        /// If null, then user has no default clinic or default clinic is HQ if clinics are enabled.
        /// </summary> 		
        public long? ClinicId;

        /// <summary>
        /// A value indicating whether the access of the user is restricted to a single clinic.
        /// This only applies when <see cref="ClinicId"/> is not null.
        /// </summary>
        public bool ClinicRestricted { get => ClinicId.HasValue; }

        /// <summary>
        /// FK to employee.EmployeeNum. Cannot be used if provnum is used. Used for timecards to block access by other users.
        /// </summary>
        public long? EmployeeId;

        /// <summary>
        /// FK to provider.ProvNum. Cannot be used if EmployeeNum is used. 
        /// It is possible to have multiple userods attached to a single provider.
        /// </summary>
        public long? ProviderId;

        /// <summary>
        /// FK to tasklist.TaskListNum.  0 if no inbox setup yet. It is assumed that the TaskList 
        /// is in the main trunk, but this is not strictly enforced. User can't delete an 
        /// attached TaskList, but they could move it.
        /// </summary>
        public long? TaskListId;

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string UserName;

        /// <summary>
        /// The password details in a "HashType$Salt$Hash" format, separating the different fields by '$'.
        /// This is NOT the actual password but the encoded password hash.
        /// If the contents of this variable are not in the aforementioned format, it is assumed to be a legacy password hash (MD5).
        /// </summary>
        public string PasswordDigest;

        /// <summary>
        /// Gets set to true if strong passwords are turned on, and this user changes their 
        /// password to a strong password.  We don't store actual passwords, so this flag is the 
        /// only way to tell.
        /// </summary>
        public bool PasswordIsStrong;

        /// <summary>
        /// Boolean.  If true, the user's password needs to be reset on next login.
        /// </summary>
        public bool PasswordResetRequired;

        /// <summary>
        /// The name of the Active Directory user account the user is linked to.
        /// </summary>
        public string DomainUser;

        /// <summary>
        /// Defaults to 3 (regular user) unless specified. Helps populates the Anesthetist, 
        /// Surgeon, Assistant and Circulator dropdowns properly on FormAnestheticRecord
        /// </summary>
        public int AnesthProvType;

        /// <summary>
        /// If set to true, the BlockSubsc button will start out pressed for this user.
        /// </summary>
        public bool DefaultHidePopups;

        /// <summary>
        /// If set to true, the BlockInbox button will start out pressed for this user.
        /// </summary>
        public bool InboxHidePopups;

        /// <summary>
        /// The date and time of the last failed login attempt.
        /// </summary>
        public DateTime? FailedDateTime;
        
        /// <summary>
        /// The number of failed login attempts.
        /// </summary>
        public int FailedAttempts;

        /// <summary>
        /// Set true to hide user from login list.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// The getter will return a struct created from the database-ready password which is stored in the Password field.
        /// The setter will manipulate the Password variable to the string representation of this PasswordContainer object.
        /// </summary>
        public PasswordContainer Password
        {
            get => Authentication.DecodePass(PasswordDigest);
            set
            {
                PasswordDigest = value.ToString();
            }
        }

        /// <summary>
        /// The password hash, not the actual password.
        /// </summary>
        public string PasswordHash => Password.Hash;

        [Obsolete] public User Copy() => (User)MemberwiseClone();

        /// <summary>
        /// Returns a string represention of the user.
        /// </summary>
        public override string ToString() => UserName;

        /// <summary>
        /// Constructs a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="User"/> instance.</returns>
        private static User FromReader(MySqlDataReader dataReader)
        {
            return new User
            {
                Id  = (long)dataReader["id"],
                ClinicId = dataReader["clinic_id"] as long?,
                EmployeeId = dataReader["employee_id"] as long?,
                ProviderId = dataReader["provider_id"] as long?,
                TaskListId = dataReader["task_list_id"] as long?,
                UserName = (string)dataReader["username"],
                PasswordDigest = (string)dataReader["password_digest"],
                PasswordIsStrong = (bool)dataReader["password_is_strong"],
                PasswordResetRequired = (bool)dataReader["password_reset_required"],
                DomainUser = (string)dataReader["domain_user"],
                AnesthProvType = Convert.ToInt32(dataReader["anesthesia_provider_type"]),
                DefaultHidePopups = (bool)dataReader["hide_popups_default"],
                InboxHidePopups = (bool)dataReader["hide_popups_inbox"],
                FailedDateTime = dataReader["failed_date"] as DateTime?,
                FailedAttempts = Convert.ToInt32(dataReader["failed_attempts"]),
                Hidden = (bool)dataReader["hidden"]
            };
        }

        /// <summary>
        /// Get a list of all users from the database.
        /// </summary>
        /// <returns>A list of users.</returns>
        public static List<User> All() =>
            SelectMany("SELECT * FROM `users` ORDER BY `username`", FromReader);

        /// <summary>
        /// Get a list of all active (non-hidden) users from the database.
        /// </summary>
        /// <returns>A list of users.</returns>
        public static List<User> AllActive() =>
            SelectMany("SELECT * FROM `users` WHERE `hidden` = 0 ORDER BY `username`", FromReader);

        /// <summary>
        /// Gets a list of all non-hidden users that has been associated with a provider.
        /// </summary>
        /// <returns>A list of users.</returns>
        public static List<User> AllProviders() =>
            SelectMany("SELECT * FROM `users` WHERE `hidden` = 0 AND `provider_id` IS NOT NULL", FromReader);

        /// <summary>
        /// Gets the user with the specified ID from the database.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user with the specifeid ID; or null if no user with the specified ID exists.</returns>
        public static User GetById(long userId) => 
            SelectOne("SELECT * FROM `users` WHERE `id` = " + userId, FromReader);

        /// <summary>
        /// Gets all users with the specified ID's.
        /// </summary>
        /// <param name="userIds">A list of user ID's.</param>
        /// <returns>A list of users.</returns>
        public static List<User> GetById(List<long> userIds)
        {
            if (userIds.Count > 0)
            {
                return
                    SelectMany(
                        $"SELECT * FROM `users` WHERE `id` IN ({string.Join(", ", userIds)})", FromReader);
            }

            return new List<User>();
        }

        /// <summary>
        /// Gets the user with the specified username.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>The user with the specified username; or null if no user with the specified username exists.</returns>
        public static User GetByUserName(string userName) =>
            SelectOne("SELECT * FROM `users` WHERE `hidden` = 0 AND `username` = ?username", FromReader,
                new MySqlParameter("username", userName));

        /// <summary>
        /// Gets a list of all users that are members of the specified user group.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>A list of users.</returns>
        public static List<User> GetByGroup(long userGroupId) => default; // TODO: Implement me...

        /// <summary>
        /// Gets a list of all users associated with the specified employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A list of users.</returns>
        public static List<User> GetByEmployee(long employeeId) =>
            SelectMany(
                "SELECT * FROM `users` WHERE `hidden` = 0 AND `employee_id` = " + employeeId, FromReader);

        /// <summary>
        /// Gets a list of all users associated with the specified provider.
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        /// <returns>A list of users.</returns>
        public static List<User> GetByProvider(long providerId) =>
            SelectMany("SELECT * FROM `users` WHERE `hidden` = 0 AND `provider_id` = " + providerId, FromReader);

        /// <summary>
        /// Checks whether there exists atleast 1 user that has the 
        /// <see cref="Permissions.SecurityAdmin"/> permission.
        /// </summary>
        public static bool HasSecurityAdminUser() =>
            DataConnection.ExecuteLong(
                @"CALL `usp_security_has_admin_user`()") > 0;

        /// <summary>
        /// Gets the username of the user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns></returns>
        public static string GetUserName(long userId) =>
            DataConnection.ExecuteString("SELECT username FROM users WHERE id =" + userId);

        /// <summary>
        /// Gets a list of all usernames from the database.
        /// </summary>
        /// <returns>A list of usernames.</returns>
        public static List<string> GetUserNames() =>
            SelectMany("SELECT `username` FROM `users` WHERE `hidden` = 0 ORDER BY `username`", reader => reader.GetString(0));

        /// <summary>
        /// Inserts the specified user into the database.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userGroupIds">A list of ID's of the user groups the user is a member of.</param>
        /// <returns>The ID assigned to the user.</returns>
        /// <exception cref="Exception">If there was a problem during user validation.</exception>
        public static long Insert(User user, List<long> userGroupIds)
        {
            if (user.Hidden && UserGroup.IsAdminGroup(userGroupIds))
                throw new Exception("Admins cannot be hidden.");

            Validate(user, userGroupIds);

            user.Id = DataConnection.ExecuteInsert(""); // TODO: Implement me.

            UserGroupUser.Synchronize(user, userGroupIds);

            return user.Id;
        }

        /// <summary>
        /// Updates the data of the specified user in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="userGroupIds">A list of ID's of the user groups the user is a member of.</param>
        public static void Update(User user, List<long> userGroupIds = null)
        {
            Validate(user, userGroupIds);

            // TODO: Execute UPDATE query...

            if (userGroupIds != null)
            {
                UserGroupUser.Synchronize(user, userGroupIds);
            }
        }

        public static bool IsUserCPOE(User user)
        {
            if (user != null && user.ProviderId.HasValue)
            {
                var provider = Providers.GetProv(user.ProviderId.Value);
                if (provider != null)
                {
                    return EhrProvKeys.HasProvHadKey(provider.LName, provider.FName);
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all of the usergroups attached to this user.
        /// </summary>
        public List<UserGroup> GetGroups() => UserGroup.GetByUser(Id);

        /// <summary>
        /// Checks whether the user is a member of the user group with the specified ID.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>True if the user is a member; otherwise, false.</returns>
        public bool IsInUserGroup(long userGroupId) => IsInUserGroup(Id, userGroupId);

        /// <summary>
        /// <para>Searches the database for a corresponding user by username (not case 
        /// sensitive).</para>
        /// 
        /// <para>Once a user has been found, if the number of failed log in attempts exceeds the 
        /// limit an exception is thrown with a message to display to the user.</para>
        /// 
        /// Then the hash of the plaintext password is checked against the password hash that is 
        /// currently in the database. Once the plaintext password passed in is validated, this 
        /// method will upgrade the hashing algorithm for the password (if necessary) and then 
        /// returns the entire user object for the corresponding user found.
        /// 
        /// Throws exceptions with error message to display to the user if anything goes wrong.
        /// Manipulates the appropriate log in failure columns in the db as needed.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The plaintext password of the user.</param>
        /// <returns>The user.</returns>
        /// <exception cref="Exception">If either the username of password is invalid.</exception>
        public static User CheckUserAndPassword(string username, string password)
        {
            var user = GetByUserName(username) ?? throw new Exception("Invalid username or password.");

            var serverDateTime = MiscData.GetNowDateTime();

            // We found a user via matching just the username passed in. Now we need to check to 
            // see if they have exceeded the log in failure attempts. For now we are hardcoding a 5
            // minute delay when the user has failed to log in 5 times in a row. An admin user can 
            // reset the password or the failure attempt count for the user failing to log in via 
            // the Security window.

            if (user.FailedDateTime.HasValue  && serverDateTime.Subtract(user.FailedDateTime.Value) < TimeSpan.FromMinutes(5) && user.FailedAttempts >= 5) 
            {
                throw new Exception( 
                    "Account has been locked due to failed log in attempts.\r\n" +
                    "Call your security admin to unlock your account or wait at least 5 minutes.");
            }

            var passwordOk = Authentication.CheckPassword(user, password);

            // If the last failed log in attempt was more than 5 minutes ago, reset the columns in
            // the database so the user can try 5 more times.
            if (user.FailedDateTime.HasValue && serverDateTime.Subtract(user.FailedDateTime.Value) > TimeSpan.FromMinutes(5))
            {
                user.FailedAttempts = 0;
                user.FailedDateTime = null;
            }

            if (!passwordOk)
            {

                user.FailedAttempts += 1;
                user.FailedDateTime = serverDateTime;
            }

            Update(user);

            if (passwordOk)
            {
                // Upgrade the encryption for the password if this is not an eCW user and the 
                // password is using an outdated hashing algorithm.
                if (!string.IsNullOrEmpty(password) && user.Password.HashType != HashTypes.SHA3_512)
                {
                    // Update the password to the default hash type which should be the most secure
                    // hashing algorithm possible.
                    if (Authentication.UpdatePassword(user, password, HashTypes.SHA3_512))
                    {
                        // The above method is almost guaranteed to have changed the password for user
                        // so go back out the database and get the changes that were made.
                        user = GetById(user.Id);
                    }
                }
                return user;
            }

            throw new Exception("Invalid username or password.");
        }

        /// <summary>
        /// Updates the password of the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordContainer">The new password of the user.</param>
        /// <param name="isStrongPassword">A value indicating whether the specified password is considered a strong password.</param>
        public static void UpdatePassword(User user, PasswordContainer passwordContainer, bool isStrongPassword)
        {
            Validate(user, user.GetGroups().Select(userGroup => userGroup.Id).ToList());

            user.Password = passwordContainer;
            user.PasswordIsStrong = isStrongPassword;

            Update(user);
        }

        /// <summary>
        /// Validates the specified user. Throws a <see cref="Exception"/> when validation fails.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <param name="userGroupIds">The ID's of the user groups the user is a member of.</param>
        /// <exception cref="Exception">If there was a problem during user validation.</exception>
        private static void Validate(User user, List<long> userGroupIds)
        {
            if (!user.Hidden)
            {
                var otherUser = GetByUserName(user.UserName);
                if (otherUser != null && otherUser.Id != user.Id)
                {
                    throw new Exception("Username already in use.");
                }
            }

            if (userGroupIds != null)
            {
                if (userGroupIds.Count == 0)
                {
                    throw new Exception("The current user must be in at least one user group.");
                }

                var isAdmin =
                    DataConnection.ExecuteLong(
                        "SELECT COUNT(*) FROM `user_group_permissions` " +
                        "WHERE `permission` = '" + Permissions.SecurityAdmin + "' " +
                        "AND `user_group_id` IN (" + string.Join(", ", userGroupIds) + ")") > 0;

                if (!user.IsNew && !isAdmin && !IsSomeoneElseSecurityAdmin(user))
                    throw new Exception("At least one user must have Security Admin permission.");

                if (user.Hidden && isAdmin)
                    throw new Exception("Admins cannot be hidden.");
            }
        }


        /// <summary>
        /// Returns true if there is at least one user part of the SecurityAdmin permission excluding the user passed in.
        /// </summary>
        public static bool IsSomeoneElseSecurityAdmin(User excludeUser)
        {
            var command = 
                "SELECT COUNT(*) FROM userod " +
                "INNER JOIN user_user_groups ON user_user_groups.user_id = users.id " +
                "INNER JOIN group_permissions ON user_user_groups.user_group_id = group_permissions.user_group_id " +
                "WHERE group_permissions.type = '" + Permissions.SecurityAdmin + "' " +
                "AND users.hidden = 0 " +
                "AND users.id != " + excludeUser.Id;

            return DataConnection.ExecuteLong(command) > 0;
        }

        /// <summary>
        /// Returns empty string if the password is strong enough. Otherwise, returns explanation 
        /// of why it's not strong enough.
        /// </summary>
        /// <param name="password">The password to test.</param>
        public static string IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password)) return "Password may not be blank when the strong password feature is turned on.";
            
            if (password.Length < 8)
                return "Password must be at least eight characters long when the strong password feature is turned on.";

            int countUpper = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsUpper(password[i]))
                {
                    countUpper++;
                }
            }

            if (countUpper == 0)
                return "Password must contain at least one capital letter when the strong password feature is turned on.";

            int countLower = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsLower(password[i]))
                {
                    countLower++;
                }
            }

            if (countLower == 0)
                return "Password must contain at least one lower case letter when the strong password feature is turned on.";
    
            if (Preference.GetBool(PreferenceName.PasswordsStrongIncludeSpecial))
            {
                int countSpecial = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    if (!char.IsLetterOrDigit(password[i]))
                    {
                        countSpecial++;
                    }
                }

                if (countSpecial == 0)
                    return "Password must contain at least one special character when the 'strong passwords require a special character' feature is turned on.";
            }

            int countNumber = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsNumber(password[i]))
                {
                    countNumber++;
                }
            }

            if (countNumber == 0)
                return "Password must contain at least one number when the strong password feature is turned on.";
            
            return "";
        }

        /// <summary>
        /// Gts the name of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The name of the user.</returns>
        public static string GetName(long userId) => GetById(userId)?.UserName ?? "";

        /// <summary>
        /// Checks wether the user is a member of the specified group.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="userGroupId">The ID of the user group.</param>
        /// <returns>True if the user is a member of the specified group; otherwise, false.</returns>
        public static bool IsInUserGroup(long userId, long userGroupId) =>
            UserGroupUser.GetByUser(userId).Select(x => x.UserGroupId).Contains(userGroupId);

        /// <summary>
        /// Returns all users that are associated to the permission passed in. Returns empty list if no matches found.
        /// </summary>
        public static List<User> GetByPermission(string permission, bool showHidden)
        {
            var users = new List<User>();

            foreach (var user in All().Where(user => showHidden || !user.Hidden))
            {
                if (UserGroupPermission.HasPermission(user, permission, null))
                {
                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// Returns all users selectable for the insurance verification list.  
        /// Pass in an empty list to not filter by clinic.  
        /// Set isAssigning to false to return only users who have an insurance already assigned.
        /// </summary>
        public static List<User> GetUsersForVerifyList(List<long> clinicIds, bool isAssigning)
        {
            var listUserNumsInInsVerify = InsVerifies.GetAllInsVerifyUserNums();
            var listUserNumsInClinic = new List<long>();
            if (clinicIds.Count > 0)
            {
                var listUserClinics = new List<ClinicUser>();
                for (int i = 0; i < clinicIds.Count; i++)
                {
                    listUserNumsInClinic.AddRange(ClinicUser.GetForClinic(clinicIds[i]).Select(y => y.UserId).Distinct().ToList());
                }

                listUserNumsInClinic.AddRange(AllActive().FindAll(x => !x.ClinicRestricted).Select(x => x.Id).Distinct().ToList()); // Always add unrestricted users into the list.
                listUserNumsInClinic = listUserNumsInClinic.Distinct().ToList(); // Remove duplicates that could possibly be in the list.
                if (listUserNumsInClinic.Count > 0)
                {
                    listUserNumsInInsVerify = listUserNumsInInsVerify.FindAll(x => listUserNumsInClinic.Contains(x));
                }

                listUserNumsInInsVerify.AddRange(GetById(listUserNumsInInsVerify).FindAll(x => !x.ClinicRestricted).Select(x => x.Id).Distinct().ToList()); // Always add unrestricted users into the list.
                listUserNumsInInsVerify = listUserNumsInInsVerify.Distinct().ToList();
            }

            var listUsersWithPerm = GetByPermission(Permissions.InsPlanVerifyList, false);
            if (isAssigning)
            {
                if (clinicIds.Count == 0)
                {
                    return listUsersWithPerm; // Return unfiltered list of users with permission
                }

                // Don't limit user list to already assigned insurance verifications.
                return listUsersWithPerm.FindAll(x => listUserNumsInClinic.Contains(x.Id)); // Return users with permission, limited by their clinics
            }

            return listUsersWithPerm.FindAll(x => listUserNumsInInsVerify.Contains(x.Id)); // Return users limited by permission, clinic, and having an insurance already assigned.
        }

        /// <summary>
        /// Gets a list of users that only have access to the specified clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>A list of users.</returns>
        public static List<User> GetUsersOnlyThisClinic(long clinicId) =>
            SelectMany(
                "SELECT `users`.* FROM (" +
                    "SELECT `user_clinics`.`user_id`, COUNT(`user_clinics`.`clinic_id`) AS `clinics` " +
                    "FROM `user_clinics` " +
                    "GROUP BY `user_id` " +
                    "HAVING `clinics` = 1" +
                ") `users` " +
                "INNER JOIN `user_clinics` ON (`user_clinics`.`user_id` = `users`.`id` AND `user_clinics`.`clinic_id` = " + clinicId + ") " +
                "INNER JOIN `users` ON `users`.`id` = `user_clinics`.`user_id`", FromReader);

        /// <summary>
        /// Checks whether a user with the specified name exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the username exists; otherwise, false.</returns>
        public static bool UserNameExists(string username) =>
            DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `users` WHERE `hidden` = 0 AND `username` = ?username", 
                new MySqlParameter("username", username)) > 0;
    }
}
