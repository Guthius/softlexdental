using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores all the connection info for one email address. Linked to clinic by clinic.EmailAddressNum. Sends email based on patient's clinic.
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

        public override string ToString() => SmtpUsername ?? "";

        /// <summary>
        /// We assume the email settings are implicit if the server port is 465.
        /// </summary>
        public bool IsImplicitSsl => SmtpPort == 465;

        /// <summary>
        /// Returns the SenderAddress if it is not blank, otherwise returns the EmailUsername.
        /// </summary>
        public string GetFrom() => string.IsNullOrEmpty(Sender) ? SmtpUsername : Sender;

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

        public static List<EmailAddress> All() =>
            SelectMany("SELECT * FROM `email_addresses`", FromReader);

        public static EmailAddress GetById(long emailAddressId) =>
            SelectOne("SELECT * FROM `email_addresses` WHERE `id` = " + emailAddressId, FromReader);

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

        public static EmailAddress GetByUser(long userId) =>
            SelectOne("SELECT * FROM `email_addresses` WHERE `user_id` = " + userId, FromReader);

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

        public static void Delete(long emailAdressId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_addresses` WHERE `id` = " + emailAdressId);

        public static bool ExistsValidEmail() => // TODO: Rename this...
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM `email_addresses` WHERE `smtp_server` != ''") > 0;

        public static EmailAddress GetDefault(long userId, long clinicId) =>
            GetByUser(userId) ?? GetByClinic(clinicId);
    }
}