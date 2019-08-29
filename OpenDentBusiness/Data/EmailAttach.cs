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
        public long? EmailId;
        public long? EmailTemplateId;
        public string Description;
        public string FileName;

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

        public static EmailAttachment GetById(long emailAttachmentId) =>
            SelectOne("SELECT * FROM `email_attachments` WHERE `id` = " + emailAttachmentId, FromReader);

        public static List<EmailAttachment> GetByEmail(long emailId) =>
            SelectMany("SELECT * FROM `email_attachments` WHERE `email_id` = " + emailId, FromReader);

        public static List<EmailAttachment> GetByEmails(List<long> emailIds)
        {
            if (emailIds.Count > 0)
            {
                return SelectMany("SELECT * FROM `email_attachments` WHERE `email_id` IN (" + string.Join(", ", emailIds) + ")", FromReader);
            }
            return new List<EmailAttachment>();
        }

        public static List<EmailAttachment> GetByTemplate(long emailTemplateId) =>
            SelectMany("SELECT * FROM `email_attachments` WHERE `email_template_id` = " + emailTemplateId, FromReader);

        public static long Insert(EmailAttachment emailAttachment) =>
            emailAttachment.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_attachments` (`email_id`, `email_template_id`, `description`, `filename`) VALUES (?email_id, ?email_template_id, ?description, ?filename)",
                    new MySqlParameter("email_id", emailAttachment.EmailId.HasValue ? (object)emailAttachment.EmailId.Value : DBNull.Value),
                    new MySqlParameter("email_template_id", emailAttachment.EmailTemplateId.HasValue ? (object)emailAttachment.EmailTemplateId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAttachment.Description ?? ""),
                    new MySqlParameter("filename", emailAttachment.FileName ?? ""));

        public static void Update(EmailAttachment emailAttachment) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_attachments` SET `email_id` = ?email_id, `email_template_id` = ?email_template_id, `description` = ?description, `filename` = ?filename WHERE `id` = ?id",
                    new MySqlParameter("email_id", emailAttachment.EmailId.HasValue ? (object)emailAttachment.EmailId.Value : DBNull.Value),
                    new MySqlParameter("email_template_id", emailAttachment.EmailTemplateId.HasValue ? (object)emailAttachment.EmailTemplateId.Value : DBNull.Value),
                    new MySqlParameter("description", emailAttachment.Description ?? ""),
                    new MySqlParameter("filename", emailAttachment.FileName ?? ""),
                    new MySqlParameter("id", emailAttachment.Id));

        public static void Delete(long emailAttachmentId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_attachments` WHERE `id` = " + emailAttachmentId);

        /// <summary>
        /// Creates a new attachment with the specified details.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <returns>The new attachment.</returns>
        public static EmailAttachment CreateAttachment(string fileName, byte[] fileContent) =>
            CreateAttachment(fileName, fileName, fileContent, true);

        /// <summary>
        /// Creates a new attachment with the specified details.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <param name="outbound">Value indicating whether the attachment is a outbound attachment.</param>
        /// <returns>The new attachment.</returns>
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
                throw new ApplicationException("Email attachment could not be saved because a file with the same name already exists.");
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
                attachmentPath = Path.Combine(ImageStore.GetPreferredAtoZpath(), "EmailAttachments"); //Gets Cloud path with EmailAttachments folder.
            }
            else
            {
                attachmentPath = Path.Combine(Path.GetTempPath(), "OpenDental");
            }

            return attachmentPath;
        }


        public EmailAttachment Copy()
        {
            return (EmailAttachment)MemberwiseClone();
        }
    }
}