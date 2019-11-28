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

namespace OpenDentBusiness
{
    public class ProgramPreference : DataRecordBase
    {
        /// <summary>
        ///     <para>
        ///         The optional ID of the clinic the preference applies to. Can be used to 
        ///         override non clinic specific preferences. Null for preferences that apply to
        ///         all clinics.
        ///     </para>
        /// </summary>
        public long? ClinicId;

        /// <summary>
        /// The ID of the program the preference belongs to.
        /// </summary>
        public long ProgramId;

        /// <summary>
        /// The description or prompt for this property.
        /// Blank for workstation overrides of program path.
        /// Many bridges use this description as an "internal description".
        /// This way it can act like a FK in order to look up this particular property.
        /// Users cannot edit.
        /// </summary>
        public string Key;

        /// <summary>
        /// The value of the preference.
        /// </summary>
        public string Value;

        /// <summary>
        ///     <para>
        ///         The human-readable name of the computer on the network (not the IP address)
        ///         the preference applies to. Can be used to override non computer specific
        ///         preferences. Null for preferences that apply to all computers.
        ///     </para>
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// Gets the preference with the specified key for the program with the specified ID from
        /// the database.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="preferenceKey">The preference key.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>
        ///     The value of the preference if one has been set; otherwise the default value.
        /// </returns>
        public static string GetString(long programId, string preferenceKey, string defaultValue = "")
        {
            var result = DataConnection.ExecuteScalar(
                "SELECT `value` FROM `program_preferences` WHERE `program_id` = " + programId + " AND `key` = ?key",
                    new MySqlParameter("key", preferenceKey ?? ""));

            if (result == null)
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Gets the preference with the specified key for the program with the specified ID from
        /// the database.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="preferenceKey">The preference key.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>
        ///     The value of the preference if one has been set; otherwise the default value.
        /// </returns>
        public static long GetLong(long programId, string preferenceKey, long defaultValue = 0)
        {
            if (long.TryParse(GetString(programId, preferenceKey), out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the preference with the specified key for the program with the specified ID from
        /// the database.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="preferenceKey">The preference key.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>
        ///     The value of the preference if one has been set; otherwise the default value.
        /// </returns>
        public static bool GetBool(long programId, string preferenceKey, bool defaultValue = false)
        {
            if (bool.TryParse(GetString(programId, preferenceKey), out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the preference with the specified key for the program with the specified ID from
        /// the database.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="preferenceKey">The preference key.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>
        ///     The value of the preference if one has been set; otherwise the default value.
        /// </returns>
        public static DateTime GetDateTime(long programId, string preferenceKey, DateTime defaultValue = default)
        {
            // TODO: Should return a nullable datetime...

            if (DateTime.TryParse(GetString(programId, preferenceKey), out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the value of the preference with the specified key for the specified program.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="preferenceKey">The preference key.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <param name="clinicId">The (optional) ID of the clinic the preference applies to.</param>
        /// <param name="computerName">The (optional) name of the computer the preference applies to.</param>
        public static void Set(long programId, string preferenceKey, string value, long? clinicId = null, string computerName = null)
        {
            DataConnection.ExecuteNonQuery(
                "INSERT INTO `program_preferences` (?clinic_id, ?program_id, ?key, ?value, ?computer_name) " +
                "ON DUPLICATE KEY UPDATE value = ?value",
                    new MySqlParameter("clinic_id", clinicId.HasValue ? (object)clinicId.Value : DBNull.Value),
                    new MySqlParameter("program_id", programId),
                    new MySqlParameter("key", preferenceKey ?? ""),
                    new MySqlParameter("value", value ?? ""),
                    new MySqlParameter("computer_name", string.IsNullOrEmpty(computerName) ? DBNull.Value : (object)computerName));
        }
    }
}
