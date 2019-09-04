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

namespace OpenDentBusiness
{
    /// <summary>
    /// Unified Code for Units of Measure.
    /// <para>UCUM is not a stricly defined list of codes but is instead a language definition 
    /// that allows for all units and derived units to be named.</para>
    /// <para>Examples: g (grams), g/L (grams per liter), g/L/s (grams per liter per second), 
    /// g/L/s/s (grams per liter per second per second), etc... are all allowed units meaning 
    /// there is an infinite number of units that can be defined using UCUM conventions.</para>
    /// </summary>
    public class Ucum : DataRecord
    {
        /// <summary>
        /// Also called concept code. Example: mol/mL
        /// </summary>
        public string Code;

        /// <summary>
        /// Also called Concept Name. Human readable form of the UCUM code.
        /// </summary>
        /// <remarks>Example: Moles Per MilliLiter [Substance Concentration Units]</remarks>
        public string Description;

        /// <summary>
        /// Constructs a new instance of the <see cref="Ucum"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Ucum"/> instance.</returns>
        static Ucum FromReader(MySqlDataReader dataReader)
        {
            return new Ucum
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Code = Convert.ToString(dataReader["code"]),
                Description = Convert.ToString(dataReader["description"])
            };
        }

        /// <summary>
        /// Gets a list containing all UCUM codes.
        /// </summary>
        /// <returns>A list of UCUM codes.</returns>
        public static List<Ucum> All() =>
            SelectMany("SELECT * FROM `ucum`", FromReader);

        /// <summary>
        /// Gets the UCUM with the specified ID.
        /// </summary>
        /// <param name="ucumId">The ID of the UCUM code.</param>
        /// <returns>The UCUM with the specified ID.</returns>
        public static Ucum GetById(long ucumId) =>
            SelectOne("SELECT * FROM ucum WHERE id = " + ucumId, FromReader);

        /// <summary>
        /// Gets the UCUM with the specified code.
        /// </summary>
        /// <param name="code">The UCUM Code.</param>
        /// <returns>The UCUM with the specified code.</returns>
        public static Ucum GetByCode(string code) =>
            SelectOne("SELECT * FROM ucum WHERE code = @code", FromReader,
                new MySqlParameter("code", code));

        /// <summary>
        /// Searches for all UCUM codes matching the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of UCUM codes matching the search text.</returns>
        public static List<Ucum> Find(string searchText) =>
            SelectMany("SELECT * FROM ucum WHERE code LIKE @search_text OR description LIKE @search_text", FromReader,
                new MySqlParameter("search_text", $"%{searchText}%"));

        /// <summary>
        /// Inserts the specified UCUM into the database.
        /// </summary>
        /// <param name="ucum">The UCUM.</param>
        /// <returns>The ID assigned to the UCUM.</returns>
        public static long Insert(Ucum ucum) =>
            ucum.Id = DataConnection.ExecuteInsert(
                "INSERT INTO ucum (code, description) VALUES (@code, @description) RETURNING id", 
                    new MySqlParameter("code", ucum.Code ?? ""), 
                    new MySqlParameter("description", ucum.Description ?? ""));

        /// <summary>
        /// Updates the specified UCUM in the database.
        /// </summary>
        /// <param name="ucum">The UCUM.</param>
        public static void Update(Ucum ucum) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE ucum SET code = @code, description = @description WHERE id = @id",
                    new MySqlParameter("code", ucum.Code ?? ""),
                    new MySqlParameter("description", ucum.Description ?? ""),
                    new MySqlParameter("id", ucum.Id));

        /// <summary>
        /// Deletes the UCUM code with the specified ID.
        /// </summary>
        /// <param name="ucumId">The ID of the UCUM.</param>
        public static void Delete(long ucumId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM ucum WHERE id = " + ucumId);

        /// <summary>
        /// Gets the total amount of UCUM codes in the database.
        /// </summary>
        /// <returns>The total number of UCUM codes.</returns>
        public static long GetCount() =>
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM ucum");
    }
}
