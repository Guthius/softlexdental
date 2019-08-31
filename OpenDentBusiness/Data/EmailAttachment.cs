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
using System.IO;

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

            string attachmentPath = FileSystem.Combine(GetAttachmentPath(), outbound ? "Out" : "In");
            if (!FileSystem.DirectoryExists(attachmentPath))
            {
                FileSystem.CreateDirectory(attachmentPath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                while (string.IsNullOrEmpty(emailAttachment.FileName) || FileSystem.FileExists(FileSystem.Combine(attachmentPath, emailAttachment.FileName)))
                {
                    emailAttachment.FileName =
                        Path.Combine(attachmentPath,
                            DateTime.UtcNow.ToString("yyyyMMdd") + "_" + DateTime.UtcNow.TimeOfDay.Ticks.ToString() + "_" + emailAttachment.Description);
                }
            }
            else
            {
                emailAttachment.FileName = FileSystem.Combine(attachmentPath, fileName);
            }

            string attachmentFilePath = FileSystem.Combine(attachmentPath, emailAttachment.FileName);
            if (FileSystem.FileExists(attachmentFilePath))
            {
                throw new DataException("Email attachment could not be saved because a file with the same name already exists.");
            }

            try
            {
                FileSystem.WriteAllBytes(attachmentFilePath, fileContent);
            }
            catch (Exception exception)
            {
                try
                {
                    if (FileSystem.FileExists(attachmentFilePath))
                    {
                        FileSystem.Delete(attachmentFilePath);
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
            string attachmentPath;
            if (Preferences.AtoZfolderUsed == DataStorageType.LocalAtoZ)
            {
                attachmentPath = Path.Combine(ImageStore.GetPreferredAtoZpath(), "EmailAttachments");
                if (!Directory.Exists(attachmentPath))
                {
                    Directory.CreateDirectory(attachmentPath);
                }
            }
            else if (CloudStorage.IsCloudStorage)
            {
                attachmentPath = Path.Combine(ImageStore.GetPreferredAtoZpath(), "EmailAttachments");
            }
            else
            {
                attachmentPath = Path.Combine(Path.GetTempPath(), "OpenDental");
            }

            return attachmentPath;
        }
    }
}