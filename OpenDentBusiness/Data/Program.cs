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
using OpenDentBusiness.Bridges;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Each row is a bridge to an outside program, frequently an imaging program.
    ///         Most of the bridges are hard coded, and simply need to be enabled. But user can 
    ///         also add their own custom bridge.
    ///     </para>
    /// </summary>
    public class Program : DataRecord
    {
        private static readonly DataRecordCache<Program> cache =
            new DataRecordCache<Program>(
                "SELECT * FROM `programs` WHERE `computer` = '" + MySqlHelper.EscapeString(Environment.MachineName) + "'", FromReader);

        /// <summary>
        /// This is the full type name of the .NET class that represents the program.
        /// </summary>
        public string TypeName;

        /// <summary>
        /// A description of the program.
        /// </summary>
        public string Description;

        /// <summary>
        /// A value indicating whether the program is enabled.
        /// </summary>
        public bool Enabled;

        /// <summary>
        /// Notes about this program link. Peculiarities, etc.
        /// </summary>
        public string Note;

        /// <summary>
        /// The name of the computer the program is installed on.
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// Constructs a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Program"/> instance.</returns>
        public static Program FromReader(MySqlDataReader dataReader)
        {
            return new Program
            {
                Id = (long)dataReader["id"],
                TypeName = (string)dataReader["type"],
                Description = (string)dataReader["description"],
                Enabled = (bool)dataReader["enabled"],
                Note = (string)dataReader["note"],
                ComputerName = (string)dataReader["computer"]
            };
        }

        /// <summary>
        ///     <para>
        ///         Gets the program with the specified ID from the database.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <returns>The program.</returns>
        public static Program GetById(long programId) => 
            cache.SelectOne(program => program.Id == programId);

        /// <summary>
        ///     <para>
        ///         Gets the program with the specified type name from the database.
        ///     </para>
        /// </summary>
        /// <param name="typeName">The type name of the program.</param>
        /// <returns>The program with the specified type name.</returns>
        public static Program GetByType(string typeName) => 
            cache.SelectOne(program => program.TypeName == typeName);

        /// <summary>
        ///     <para>
        ///         Gets the program for the specified type from the database.
        ///     </para>
        /// </summary>
        /// <typeparam name="T">The program type.</typeparam>
        /// <returns>The program for the specified type.</returns>
        public static Program GetByType<T>() where T : Bridge =>
            GetByType(typeof(T).FullName);

        /// <summary>
        /// Gets all programs from the database.
        /// </summary>
        /// <returns>A list of programs.</returns>
        public static IEnumerable<Program> All => cache.All();

        /// <summary>
        /// Inserts the specified program into the database.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>The ID assigned to the program.</returns>
        public static long Insert(Program program) =>
            program.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `programs` (`type`, `description`, `enabled`, `note`, `computer`) VALUES (?type, ?description, ?enabled, ?note, ?computer)",
                    new MySqlParameter("type", program.TypeName ?? ""),
                    new MySqlParameter("description", program.Description ?? ""),
                    new MySqlParameter("enabled", program.Enabled),
                    new MySqlParameter("note", program.Note ?? ""),
                    new MySqlParameter("computer", program.ComputerName ?? ""));

        /// <summary>
        /// Updates the specified program in the database.
        /// </summary>
        /// <param name="program">The program.</param>
        public static void Update(Program program) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `programs` SET `type` = ?type, `description` = ?description, `enabled` = ?enabled, `note` = ?note, `computer` = ?computer WHERE `id` = ?id",
                    new MySqlParameter("type", program.TypeName ?? ""),
                    new MySqlParameter("description", program.Description ?? ""),
                    new MySqlParameter("enabled", program.Enabled),
                    new MySqlParameter("note", program.Note ?? ""),
                    new MySqlParameter("computer", program.ComputerName ?? ""),
                    new MySqlParameter("id", program.Id));

        /// <summary>
        /// Deletes the program with the specified ID from the database.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        public static void Delete(long programId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `programs` WHERE `id` = " + programId);

        /// <summary>
        /// Gets a value indicating whether the program with the specified type name is enabled.
        /// </summary>
        /// <param name="typeName">the type name of the program.</param>
        /// <returns>True if the program is enabled; otherwise, false.</returns>
        public static bool IsEnabled(string typeName) =>
            GetByType(typeName)?.Enabled ?? false;

        /// <summary>
        /// Gets a value indicating whether the program with the specified typ is enabled.
        /// </summary>
        /// <typeparam name="T">The program type.</typeparam>
        /// <returns>True if the program is enabled; otherwise, false.</returns>
        public static bool IsEnabled<T>() where T : Bridge => IsEnabled(typeof(T).FullName);
    }
}
