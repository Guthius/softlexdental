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
    public class EmailAutograph : DataRecord
    {
        /// <summary>
        /// The ID of the e-mail address the autograph is assigned to.
        /// </summary>
        public long? EmailAddressId;

        /// <summary>
        /// A description of the autograph.
        /// </summary>
        public string Description;

        /// <summary>
        /// The actual autograph.
        /// </summary>
        public string Autograph;

        /// <summary>
        /// Returns a string representation of the autograph.
        /// </summary>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="EmailAutograph"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="EmailAutograph"/> instance.</returns>
        static EmailAutograph FromReader(MySqlDataReader dataReader)
        {
            var emailAutograph = new EmailAutograph
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Autograph = Convert.ToString(dataReader["autograph"])
            };

            var emailAddressId = dataReader["email_address_id"];
            if (emailAddressId != DBNull.Value)
            {
                emailAutograph.EmailAddressId = Convert.ToInt64(emailAddressId);
            }

            return emailAutograph;
        }

        /// <summary>
        /// Gets a list containing all autographs.
        /// </summary>
        /// <returns>A list of autographs.</returns>
        public static List<EmailAutograph> All() =>
            SelectMany("SELECT * FROM `email_autographs`", FromReader);

        /// <summary>
        /// Gets the autograph with the specified ID.
        /// </summary>
        /// <param name="emailAutographId">The ID of the autograph.</param>
        /// <returns>The autograph with the specified ID.</returns>
        public static EmailAutograph GetById(long emailAutographId) =>
            SelectOne("SELECT * FROM `email_autographs` WHERE `id` = " + emailAutographId, FromReader);

        /// <summary>
        /// Inserts the specified autograph into the database.
        /// </summary>
        /// <param name="emailAutograph">The autograph.</param>
        /// <returns>The ID assigned to the autograph.</returns>
        public static void Insert(EmailAutograph emailAutograph) =>
            emailAutograph.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_autographs` (`email_address_id`, `description`, `autograph`) VALUES (?email_address_id, ?description, ?autograph)",
                    new MySqlParameter("email_address_id", emailAutograph.EmailAddressId.HasValue ? (object)emailAutograph.EmailAddressId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAutograph.Description ?? ""),
                    new MySqlParameter("autograph", emailAutograph.Autograph ?? ""));

        /// <summary>
        /// Updates the specified in the database.
        /// </summary>
        /// <param name="emailAutograph">The autograph.</param>
        public static void Update(EmailAutograph emailAutograph) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_autographs` SET `email_address_id` = ?email_address_id, `description` = ?description, `autograph` = ?autograph WHERE `id` = ?id",
                    new MySqlParameter("email_address_id", emailAutograph.EmailAddressId.HasValue ? (object)emailAutograph.EmailAddressId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAutograph.Description ?? ""),
                    new MySqlParameter("autograph", emailAutograph.Autograph ?? ""),
                    new MySqlParameter("id", emailAutograph.Id));

        /// <summary>
        /// Deletes the autograph with the specified ID.
        /// </summary>
        /// <param name="emailAutographId">The ID of the autograph.</param>
        public static void Delete(long emailAutographId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_autographs` WHERE `id` = " + emailAutographId);
    }
}