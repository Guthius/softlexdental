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
    /// These are templates that are used to send simple letters to patients.
    /// </summary>
    public class Letter : DataRecord
    {
        /// <summary>
        /// A description of the letter.
        /// </summary>
        public string Description;

        /// <summary>
        /// Text of the letter
        /// </summary>
        public string Body;

        /// <summary>
        /// Constructs a new instance of the <see cref="Letter"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Letter"/> instance.</returns>
        static Letter FromReader(MySqlDataReader dataReader)
        {
            return new Letter
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Body = Convert.ToString(dataReader["body"])
            };
        }
        
        /// <summary>
        /// Gets a list containing all letters.
        /// </summary>
        /// <returns>A list of letters.</returns>
        public static List<Letter> All() =>
            SelectMany("SELECT * FROM `letters`", FromReader);

        /// <summary>
        /// Inserts the specified letter into the database.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <returns>The ID assigned to the letter.</returns>
        public static long Insert(Letter letter) =>
            letter.Id = DataConnection.ExecuteInsert(
                "INSERT INTO letters (`description`, `body`) VALUES (:description, :body)",
                    new MySqlParameter("description", letter.Description ?? ""),
                    new MySqlParameter("body", letter.Body ?? ""));

        /// <summary>
        /// Updates the specified letter in the database.
        /// </summary>
        /// <param name="letter">The letter.</param>
        public static void Update(Letter letter) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `letters` SET `description` = :description, `body` = :body WHERE `id` = :id",
                    new MySqlParameter("description", letter.Description ?? ""),
                    new MySqlParameter("body", letter.Body ?? ""),
                    new MySqlParameter("id", letter.Id));

        /// <summary>
        /// Deletes the letter with the specified ID.
        /// </summary>
        /// <param name="letterId">The ID of the letter.</param>
        public static void Delete(long letterId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `letters` WHERE `id` = " + letterId);
    }
}