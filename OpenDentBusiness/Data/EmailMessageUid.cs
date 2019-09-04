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
    /// Used to track which email messages have been downloaded into the inbox for a particular recipient address.
    /// </summary>
    public class EmailMessageUid : DataRecord
    {
        /// <summary>
        /// The ID of the e-mail address the UID is linked to.
        /// </summary>
        public long EmailAddressId;

        /// <summary>
        /// The UID.
        /// </summary>
        public string Uid;

        /// <summary>
        /// Gets a list of all UID's linked to the e-mail address with the specified ID.
        /// </summary>
        /// <param name="emailAddressId">The ID of the e-mail address.</param>
        /// <returns>A list of UID's.</returns>
        public static List<string> GetByEmailAddress(long emailAddressId) =>
            SelectMany("SELECT `uid` FROM `email_uids` WHERE `email_address_id` = " + emailAddressId, 
                (dataReader) => Convert.ToString(dataReader[0]));

        /// <summary>
        /// Inserts the specified UID into the database.
        /// </summary>
        /// <param name="emailAddressId">The ID of the e-mail address</param>
        /// <param name="uid">The UID.</param>
        public static void Insert(long emailAddressId, string uid) =>
            DataConnection.ExecuteNonQuery("INSERT IGNORE INTO `email_uids` (`email_address_id`, `uid`) VALUES (?email_address_id, ?uid)",
                new MySqlParameter("email_address_id", emailAddressId),
                new MySqlParameter("uid", uid ?? ""));
    }
}