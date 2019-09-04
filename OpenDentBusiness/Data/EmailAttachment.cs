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
using SLDental.Storage;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Keeps track of one file attached to an email. Multiple files can be attached to an email using this method.
    /// </summary>
    public class EmailAttachment : DataRecord
    {
        /// <summary>
        /// The ID of the e-mail the attachment is bound to.
        /// </summary>
        public long? EmailId;

        /// <summary>
        /// The ID of the e-mail template the attachment is bound to.
        /// </summary>
        public long? EmailTemplateId;

        /// <summary>
        /// A description of the attachment.
        /// </summary>
        public string Description;

        /// <summary>
        /// The filename of the attachment.
        /// </summary>
        public string FileName;

        /// <summary>
        /// Constructs a new instance of the <see cref="EmailAttachment"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="EmailAttachment"/> instance.</returns>
        static EmailAttachment FromReader(MySqlDataReader dataReader)
        {
            var emailAttachment = new EmailAttachment
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                FileName = Convert.ToString(dataReader["filename"])
            };

            var emailId = dataReader["email_id"];
            if (emailId != DBNull.Value)
            {
                emailAttachment.EmailId = Convert.ToInt64(emailId);
            }

            var emailTemplateId = dataReader["email_template_id"];
            if (emailTemplateId != DBNull.Value)
            {
                emailAttachment.EmailTemplateId = Convert.ToInt64(emailTemplateId);
            }

            return emailAttachment;
        }

        /// <summary>
        /// Gets the attachment with the specified ID.
        /// </summary>
        /// <param name="emailAttachmentId">The ID of the attachment.</param>
        /// <returns>The attachment with the specified ID.</returns>
        public static EmailAttachment GetById(long emailAttachmentId) =>
            SelectOne("SELECT * FROM `email_attachments` WHERE `id` = " + emailAttachmentId, FromReader);

        /// <summary>
        /// Gets all attachments for the e-mail with the specified ID.
        /// </summary>
        /// <param name="emailId">The ID of the e-mail.</param>
        /// <returns>A list of attachments.</returns>
        public static List<EmailAttachment> GetByEmail(long emailId) =>
            SelectMany("SELECT * FROM `email_attachments` WHERE `email_id` = " + emailId, FromReader);

        /// <summary>
        /// Gets all attachments for the specified e-mails.
        /// </summary>
        /// <param name="emailIds">A list of e-mail ID's.</param>
        /// <returns>A list of attachments.</returns>
        public static List<EmailAttachment> GetByEmails(List<long> emailIds)
        {
            if (emailIds.Count > 0)
            {
                return SelectMany("SELECT * FROM `email_attachments` WHERE `email_id` IN (" + string.Join(", ", emailIds) + ")", FromReader);
            }
            return new List<EmailAttachment>();
        }

        /// <summary>
        /// Gets all attachments for template with the specified ID.
        /// </summary>
        /// <param name="emailTemplateId">The ID of the template.</param>
        /// <returns>A list of attachments.</returns>
        public static List<EmailAttachment> GetByTemplate(long emailTemplateId) =>
            SelectMany("SELECT * FROM `email_attachments` WHERE `email_template_id` = " + emailTemplateId, FromReader);

        /// <summary>
        /// Inserts the specified attachment into the database.
        /// </summary>
        /// <param name="emailAttachment">The attachment.</param>
        /// <returns>The ID assigned to the attachment.</returns>
        public static long Insert(EmailAttachment emailAttachment) =>
            emailAttachment.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_attachments` (`email_id`, `email_template_id`, `description`, `filename`) VALUES (?email_id, ?email_template_id, ?description, ?filename)",
                    new MySqlParameter("email_id", emailAttachment.EmailId.HasValue ? (object)emailAttachment.EmailId.Value : DBNull.Value),
                    new MySqlParameter("email_template_id", emailAttachment.EmailTemplateId.HasValue ? (object)emailAttachment.EmailTemplateId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAttachment.Description ?? ""),
                    new MySqlParameter("filename", emailAttachment.FileName ?? ""));

        /// <summary>
        /// Updates the specified attachment in the database.
        /// </summary>
        /// <param name="emailAttachment">The attachment.</param>
        public static void Update(EmailAttachment emailAttachment) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_attachments` SET `email_id` = ?email_id, `email_template_id` = ?email_template_id, `description` = ?description, `filename` = ?filename WHERE `id` = ?id",
                    new MySqlParameter("email_id", emailAttachment.EmailId.HasValue ? (object)emailAttachment.EmailId.Value : DBNull.Value),
                    new MySqlParameter("email_template_id", emailAttachment.EmailTemplateId.HasValue ? (object)emailAttachment.EmailTemplateId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAttachment.Description ?? ""),
                    new MySqlParameter("filename", emailAttachment.FileName ?? ""),
                    new MySqlParameter("id", emailAttachment.Id));

        /// <summary>
        /// Deletes the attachment with the specified ID from the database.
        /// </summary>
        /// <param name="emailAttachmentId">The ID of the attachment.</param>
        public static void Delete(long emailAttachmentId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_attachments` WHERE `id` = " + emailAttachmentId);

        /// <summary>
        /// Creates a new attachment with the specified details.
        /// </summary>
        /// <param name="fileName">The attachment filename.</param>
        /// <param name="fileContent">the content of the attachment.</param>
        /// <returns>The new attachment.</returns>
        public static EmailAttachment CreateAttachment(string fileName, byte[] fileContent) =>
            CreateAttachment(fileName, fileName, fileContent, true);

        /// <summary>
        /// Creates a new attachment with the specified details.
        /// </summary>
        /// <param name="description">A description of the attachment.</param>
        /// <param name="fileName">The attachment filename.</param>
        /// <param name="fileContent">the content of the attachment.</param>
        /// <param name="outbound">Value indicating whether the attachment is a outbound attachment.</param>
        /// <returns>The new attachment.</returns>
        /// <exception cref="DataException">When a attachment with the specified filename already exists.</exception>
        public static EmailAttachment CreateAttachment(string description, string fileName, byte[] fileContent, bool outbound)
        {
            var emailAttachment = new EmailAttachment
            {
                Description = description
            };

            if (string.IsNullOrEmpty(emailAttachment.Description)) emailAttachment.Description = "[Attachment]";

            string attachmentPath = Storage.Default.CombinePath(GetAttachmentPath(), outbound ? "Out" : "In");
            if (!Storage.Default.DirectoryExists(attachmentPath))
            {
                Storage.Default.CreateDirectory(attachmentPath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                while (string.IsNullOrEmpty(emailAttachment.FileName) || Storage.Default.FileExists(Storage.Default.CombinePath(attachmentPath, emailAttachment.FileName)))
                {
                    emailAttachment.FileName =
                        Storage.Default.CombinePath(attachmentPath,
                            DateTime.UtcNow.ToString("yyyyMMdd") + "_" + DateTime.UtcNow.TimeOfDay.Ticks.ToString() + "_" + emailAttachment.Description);
                }
            }
            else
            {
                emailAttachment.FileName = Storage.Default.CombinePath(attachmentPath, fileName);
            }

            string attachmentFilePath = Storage.Default.CombinePath(attachmentPath, emailAttachment.FileName);
            if (Storage.Default.FileExists(attachmentFilePath))
            {
                throw new DataException("Email attachment could not be saved because a file with the same name already exists.");
            }

            try
            {
                Storage.Default.WriteAllBytes(attachmentFilePath, fileContent);
            }
            catch (Exception exception)
            {
                try
                {
                    if (Storage.Default.FileExists(attachmentFilePath))
                    {
                        Storage.Default.DeleteFile(attachmentFilePath);
                    }
                }
                catch
                {
                }
                throw exception;
            }

            return emailAttachment;
        }

        /// <summary>
        /// Gets the full path of the location where attachments should be stored.
        /// </summary>
        /// <returns>The storage path for attachments.</returns>
        public static string GetAttachmentPath()
        {
            var attachmentPath = Storage.Default.CombinePath("EmailAttachments");
            if (!Storage.Default.DirectoryExists(attachmentPath))
            {
                Storage.Default.CreateDirectory(attachmentPath);
            }

            return attachmentPath;
        }
    }
}