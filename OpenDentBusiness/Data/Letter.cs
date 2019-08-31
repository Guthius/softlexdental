/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
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