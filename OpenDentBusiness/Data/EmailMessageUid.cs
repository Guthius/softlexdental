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