using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class EmailTemplate : DataRecord
    {
        public string Description;
        public string Subject;
        public string Body;
        public bool IsHtml;

        public override string ToString() => Description ?? "";

        static EmailTemplate FromReader(MySqlDataReader dataReader)
        {
            return new EmailTemplate
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Subject = Convert.ToString(dataReader["subject"]),
                Body = Convert.ToString(dataReader["body"]),
                IsHtml = Convert.ToBoolean(dataReader["is_html"])
            };
        }

        public static List<EmailTemplate> All() =>
            SelectMany("SELECT * FROM `email_templates`", FromReader);

        public static EmailTemplate GetById(long emailTemplateId) =>
            SelectOne("SELECT * FROM `email_templates` WHERE `id` = " + emailTemplateId, FromReader);

        public static long Insert(EmailTemplate emailTemplate) =>
            emailTemplate.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_templates` (`description`, `subject`, `body`, `is_html`) VALUES (?description, ?subject, ?body, ?is_html)",
                    new MySqlParameter("description", emailTemplate.Description ?? ""),
                    new MySqlParameter("subject", emailTemplate.Subject ?? ""),
                    new MySqlParameter("body", emailTemplate.Body ?? ""),
                    new MySqlParameter("is_html", emailTemplate.IsHtml));

        public static void Update(EmailTemplate emailTemplate) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_templates` SET `description` = ?description, `subject` = ?subject, `body` = ?body, `is_html` = ?is_html WHERE `id` = ?id",
                    new MySqlParameter("description", emailTemplate.Description ?? ""),
                    new MySqlParameter("subject", emailTemplate.Subject ?? ""),
                    new MySqlParameter("body", emailTemplate.Body ?? ""),
                    new MySqlParameter("is_html", emailTemplate.IsHtml),
                    new MySqlParameter("id", emailTemplate.Id));

        public static void Delete(long emailTemplateId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_templates` WHERE `id` = " + emailTemplateId);
    }
}