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
    /// Stores all the connection info for one e-mail address.
    /// </summary>
    public class EmailAddress : DataRecord
    {
        public long? UserId;
        public string Sender;
        public string SmtpServer;
        public string SmtpUsername;
        public string SmtpPassword;
        public int SmtpPort = 25;
        public bool UseSsl = true;
        public string Pop3Server = "pop.gmail.com";
        public int Pop3Port = 110;

        /// <summary>
        /// Returns a string representation of the e-mail address.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => SmtpUsername ?? "";

        /// <summary>
        /// We assume the email settings are implicit if the server port is 465.
        /// </summary>
        public bool IsImplicitSsl => SmtpPort == 465;

        /// <summary>
        /// Returns the SenderAddress if it is not blank, otherwise returns the EmailUsername.
        /// </summary>
        public string GetFrom() => string.IsNullOrEmpty(Sender) ? SmtpUsername : Sender;

        /// <summary>
        /// Constructs a new instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="EmailAddress"/> instance.</returns>
        static EmailAddress FromReader(MySqlDataReader dataReader)
        {
            var emailAddress = new EmailAddress
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Sender = Convert.ToString(dataReader["sender"]),
                SmtpServer = Convert.ToString(dataReader["smtp_server"]),
                SmtpUsername = Convert.ToString(dataReader["smtp_username"]),
                SmtpPassword = Convert.ToString(dataReader["smtp_password"]),
                SmtpPort = Convert.ToInt32(dataReader["smtp_port"]),
                UseSsl = Convert.ToBoolean(dataReader["use_ssl"]),
                Pop3Server = Convert.ToString(dataReader["pop3_server"]),
                Pop3Port = Convert.ToInt32(dataReader["pop3_port"])
            };

            var userId = dataReader["user_id"];
            if (userId != DBNull.Value)
            {
                emailAddress.UserId = Convert.ToInt64(userId);
            }

            return emailAddress;
        }

        /// <summary>
        /// Gets a list containing all e-mail addresses.
        /// </summary>
        /// <returns>A list of e-mail addresses.</returns>
        public static List<EmailAddress> All() =>
            SelectMany("SELECT * FROM `email_addresses`", FromReader);
        
        /// <summary>
        /// Gets the e-mail address with the specified ID.
        /// </summary>
        /// <param name="emailAddressId">The ID of the e-mail address.</param>
        /// <returns>The e-mail address with the specified ID.</returns>
        public static EmailAddress GetById(long emailAddressId) =>
            SelectOne("SELECT * FROM `email_addresses` WHERE `id` = " + emailAddressId, FromReader);

        /// <summary>
        /// Gets the e-mail address associated wth the clinic with the specified ID.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>The e-mail address assigned to the clinic with the specified ID; or null if no e-mail address is assigned.</returns>
        public static EmailAddress GetByClinic(long clinicId)
        {
            EmailAddress emailAddress = null;

            if (Preferences.HasClinicsEnabled)
            {
                var clinic = Clinics.GetClinic(clinicId);
                if (clinic != null)
                {
                    emailAddress = GetById(clinic.EmailAddressNum);
                }
            }

            return emailAddress ?? GetById(Preference.GetLong(PreferenceName.EmailDefaultAddressNum));
        }

        /// <summary>
        /// Gets the e-mail address assocated with the user with the specified ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>The e-mail address assigned to the user with the specified ID; or null if no e-mail address assigned.</returns>
        public static EmailAddress GetByUser(long userId) =>
            SelectOne("SELECT * FROM `email_addresses` WHERE `user_id` = " + userId, FromReader);

        /// <summary>
        /// Inserts the specified e-mail address into the database.
        /// </summary>
        /// <param name="emailAddress">The e-mail address.</param>
        /// <returns>The ID assigned to the e-mail address.</returns>
        public static long Insert(EmailAddress emailAddress) => // TODO: user_id column should be unique and Insert should check for duplicates...
            emailAddress.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_addresses` (`user_id`, `sender`, `smtp_server`, `smpt_username`, `smpt_password`, `smtp_port`, `use_ssl`, `pop3_server`, `pop3_port`) VALUES (?user_id, ?sender, ?smtp_server, ?smtp_username, ?smtp_password, ?smtp_port, ?use_ssl, ?pop3_server, ?pop3_port)",
                    new MySqlParameter("user_id", emailAddress.UserId.HasValue ? (object)emailAddress.UserId.Value : DBNull.Value),
                    new MySqlParameter("sender", emailAddress.Sender ?? ""),
                    new MySqlParameter("smtp_server", emailAddress.SmtpServer ?? ""),
                    new MySqlParameter("smtp_username", emailAddress.SmtpUsername ?? ""),
                    new MySqlParameter("smtp_password", emailAddress.SmtpPassword ?? ""),
                    new MySqlParameter("smtp_port", emailAddress.SmtpPort),
                    new MySqlParameter("use_ssl", emailAddress.UseSsl),
                    new MySqlParameter("pop3_server", emailAddress.Pop3Server ?? ""),
                    new MySqlParameter("pop3_port", emailAddress.Pop3Port));

        /// <summary>
        /// Updates the specified e-mail address in the database.
        /// </summary>
        /// <param name="emailAddress">The e-mail address.</param>
        public static void Update(EmailAddress emailAddress) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_addresses` SET `user_id` = ?user_id, `sender` = ?sender, `smtp_server` = ?smtp_server, `smtp_username` = ?smtp_username, `smtp_password` = ?smtp_password, `smtp_port` = ?smtp_port, `use_ssl` = ?use_ssl, `pop3_server` = ?pop3_server, `pop3_port` = ?pop3_port WHERE `id` = ?id",
                    new MySqlParameter("user_id", emailAddress.UserId.HasValue ? (object)emailAddress.UserId.Value : DBNull.Value),
                    new MySqlParameter("sender", emailAddress.Sender ?? ""),
                    new MySqlParameter("smtp_server", emailAddress.SmtpServer ?? ""),
                    new MySqlParameter("smtp_username", emailAddress.SmtpUsername ?? ""),
                    new MySqlParameter("smtp_password", emailAddress.SmtpPassword ?? ""),
                    new MySqlParameter("smtp_port", emailAddress.SmtpPort),
                    new MySqlParameter("use_ssl", emailAddress.UseSsl),
                    new MySqlParameter("pop3_server", emailAddress.Pop3Server ?? ""),
                    new MySqlParameter("pop3_port", emailAddress.Pop3Port),
                    new MySqlParameter("id", emailAddress.Id));

        /// <summary>
        /// Deletes the e-mail address with the specified ID from the database.
        /// </summary>
        /// <param name="emailAdressId">The ID of the e-mail address.</param>
        public static void Delete(long emailAdressId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_addresses` WHERE `id` = " + emailAdressId);

        /// <summary>
        /// Checks whether a valid e-mail address exists in the database.
        /// </summary>
        /// <returns>True if a valid e-mail address is available; otherwise, false.</returns>
        public static bool ExistsValidEmail() => // TODO: Rename this...
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM `email_addresses` WHERE `smtp_server` != ''") > 0;

        /// <summary>
        /// Gets the default e-mail address. Returns the e-mail of assigned to the user with the
        /// specified ID if one has been assigned. If no e-mail address has been assigned to the 
        /// user the e-mail address assigned to the clinic with the specified ID will be returned.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>The default e-mail address.</returns>
        public static EmailAddress GetDefault(long userId, long clinicId) =>
            GetByUser(userId) ?? GetByClinic(clinicId);
    }
}