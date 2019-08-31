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
    public class EmailTemplate : DataRecord
    {
        /// <summary>
        /// A description of the template.
        /// </summary>
        public string Description;

        /// <summary>
        /// The subject of the template.
        /// </summary>
        public string Subject;

        /// <summary>
        /// The e-mail template body.
        /// </summary>
        public string Body;

        /// <summary>
        /// Returns a string representation of the e-mail template.
        /// </summary>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="EmailTemplate"/> instance.</returns>
        static EmailTemplate FromReader(MySqlDataReader dataReader)
        {
            return new EmailTemplate
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Subject = Convert.ToString(dataReader["subject"]),
                Body = Convert.ToString(dataReader["body"])
            };
        }

        /// <summary>
        /// Gets a list containing all e-mail templates.
        /// </summary>
        /// <returns></returns>
        public static List<EmailTemplate> All() =>
            SelectMany("SELECT * FROM `email_templates`", FromReader);

        /// <summary>
        /// Gets the e-mail template with the specified ID.
        /// </summary>
        /// <param name="emailTemplateId"></param>
        /// <returns></returns>
        public static EmailTemplate GetById(long emailTemplateId) =>
            SelectOne("SELECT * FROM `email_templates` WHERE `id` = " + emailTemplateId, FromReader);

        /// <summary>
        /// Inserts the specified e-mail template into the database.
        /// </summary>
        /// <param name="emailTemplate">The e-mail template.</param>
        /// <returns>The ID assigned to the e-mail template.</returns>
        public static long Insert(EmailTemplate emailTemplate) =>
            emailTemplate.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_templates` (`description`, `subject`, `body`) VALUES (?description, ?subject, ?body)",
                    new MySqlParameter("description", emailTemplate.Description ?? ""),
                    new MySqlParameter("subject", emailTemplate.Subject ?? ""),
                    new MySqlParameter("body", emailTemplate.Body ?? ""));

        /// <summary>
        /// Updates the specified e-mail template in the database.
        /// </summary>
        /// <param name="emailTemplate">The e-mail template.</param>
        public static void Update(EmailTemplate emailTemplate) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_templates` SET `description` = ?description, `subject` = ?subject, `body` = ?body WHERE `id` = ?id",
                    new MySqlParameter("description", emailTemplate.Description ?? ""),
                    new MySqlParameter("subject", emailTemplate.Subject ?? ""),
                    new MySqlParameter("body", emailTemplate.Body ?? ""),
                    new MySqlParameter("id", emailTemplate.Id));

        /// <summary>
        /// Deletes the e-mail template with the specified ID from the database.
        /// </summary>
        /// <param name="emailTemplateId">The ID of the e-mail template.</param>
        public static void Delete(long emailTemplateId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_templates` WHERE `id` = " + emailTemplateId);
    }
}