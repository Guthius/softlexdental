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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class ProgramProperty : DataRecordBase
    {
        public long? ClinicId;

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
        /// The value of the property.
        /// </summary>
        public string Value;

        /// <summary>
        /// The human-readable name of the computer on the network (not the IP address).
        /// Only used when overriding program path.
        /// Blank for typical Program Properties.
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static ProgramProperty FromReader(MySqlDataReader dataReader)
        {
            return new ProgramProperty
            {
                // TODO: Implement...
            };
        }

        /// <summary>
        /// Gets a collection of properties for the specified program.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <returns>A collection of properties.</returns>
        public static ProgramPropertyCollection GetByProgram(long programId) =>
            new ProgramPropertyCollection(
                SelectMany("SELECT * FROM `program_properties` WHERE `program_id` = " + programId, FromReader));

        /// <summary>
        /// Updates a program property in the database.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="key">The property key.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <param name="computerName">The name of the computer.</param>
        public static void Update(long programId, string key, string value, long? clinicId = null, string computerName = null) =>
            DataConnection.ExecuteNonQuery(
                "INSERT INTO `program_properties` (?clinic_id, ?program_id, ?key, ?value, ?computer_name) " +
                "ON DUPLICATE KEY UPDATE value = ?value",
                    new MySqlParameter("clinic_id", clinicId.HasValue ? (object)clinicId.Value : DBNull.Value),
                    new MySqlParameter("program_id", programId),
                    new MySqlParameter("key", key),
                    new MySqlParameter("value", value ?? ""),
                    new MySqlParameter("computer_name", string.IsNullOrEmpty(computerName) ? DBNull.Value : (object)computerName));
    }

    public sealed class ProgramPropertyCollection : IEnumerable<ProgramProperty>
    {
        private readonly List<ProgramProperty> cache;

        /// <summary>
        /// Gets the number of properties in the collection.
        /// </summary>
        public int Count => cache.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramPropertyCollection"/> class.
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="programProperties"></param>
        internal ProgramPropertyCollection(IEnumerable<ProgramProperty> programProperties)
        {
            cache = new List<ProgramProperty>(programProperties);
        }

        public ProgramProperty GetByKey(string key) =>
            cache.SingleOrDefault(
                programProperty => 
                    programProperty.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        

        public ProgramProperty GetByKey(string key, long clinicId) =>
            cache.SingleOrDefault(
                programProperty =>
                    programProperty.ClinicId == clinicId &&
                    programProperty.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

        public ProgramProperty GetByKey(string key, string computerName) =>
            cache.SingleOrDefault(
                programProperty =>
                    programProperty.ComputerName.Equals(computerName, StringComparison.Ordinal) &&
                    programProperty.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

        public ProgramProperty GetByKey(string key, long clinicId, string computerName) =>
            cache.SingleOrDefault(
                programProperty =>
                    programProperty.ClinicId == clinicId &&
                    programProperty.ComputerName.Equals(computerName, StringComparison.Ordinal) &&
                    programProperty.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) ??
                GetByKey(key, clinicId);

        public string GetString(string key, string defaultValue = "") =>
            GetByKey(key)?.Value ?? defaultValue;

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (bool.TryParse(GetString(key), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            if (long.TryParse(GetString(key), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (int.TryParse(GetString(key), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public IEnumerator<ProgramProperty> GetEnumerator()
        {
            return ((IEnumerable<ProgramProperty>)cache).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ProgramProperty>)cache).GetEnumerator();
        }
    }
}