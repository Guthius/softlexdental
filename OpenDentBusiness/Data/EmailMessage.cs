/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using CodeBase;
using MySql.Data.MySqlClient;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores both sent and received emails, as well as saved emails which are still in composition.
    /// </summary>
    public class EmailMessage : DataRecord
    {
        static readonly List<long> receivingEmailAddressIdList = new List<long>();

        /// <summary>
        /// The ID of the address that sent or received the message.
        /// </summary>
        public long EmailAddressId;
        public long? PatientId;
        public long? AppointmentId;

        /// <summary>
        /// The ID of the user that sent the messsage.
        /// </summary>
        public long? UserId;

        /// <summary>
        /// The ID of the provider that sent the message.
        /// </summary>
        public long? ProviderId;

        public string FromName;
        public string FromAddress;
        public string ToAddress;
        public string CcAddress;
        public string BccAddress;
        public string Subject;
        public string Body;
        public byte[] BodyRaw;

        /// <summary>
        /// The date on which the message was sent or received.
        /// </summary>
        public DateTime Date;
        public EmailMessageFlags Flags;
        public EmailMessageStatus Status;
        public List<EmailAttachment> Attachments = new List<EmailAttachment>();

        static EmailMessage FromReader(MySqlDataReader dataReader)
        {
            var emailMessage = new EmailMessage
            {
                Id = Convert.ToInt64(dataReader["id"]),
                EmailAddressId = Convert.ToInt64(dataReader["email_address_id"]),
                FromName = Convert.ToString(dataReader["from_name"]),
                FromAddress = Convert.ToString(dataReader["from_address"]),
                ToAddress = Convert.ToString(dataReader["to_address"]),
                CcAddress = Convert.ToString(dataReader["cc_address"]),
                BccAddress = Convert.ToString(dataReader["bcc_address"]),
                Subject = Convert.ToString(dataReader["subject"]),
                Body = Convert.ToString(dataReader["body"]),
                Date = Convert.ToDateTime(dataReader["date"]),
                Flags = (EmailMessageFlags)Convert.ToInt32(dataReader["flags"]),
                Status = (EmailMessageStatus)Convert.ToInt32(dataReader["status"])
            };

            var patientId = dataReader["patient_id"];
            if (patientId != DBNull.Value)
            {
                emailMessage.PatientId = Convert.ToInt64(patientId);
            }

            var appointmentId = dataReader["appointment_id"];
            if (appointmentId != DBNull.Value)
            {
                emailMessage.AppointmentId = Convert.ToInt64(appointmentId);
            }

            var userId = dataReader["user_id"];
            if (userId != DBNull.Value)
            {
                emailMessage.UserId = Convert.ToInt64(userId);
            }

            var providerId = dataReader["provider_id"];
            if (providerId != DBNull.Value)
            {
                emailMessage.ProviderId = Convert.ToInt64(providerId);
            }

            var bodyRaw = dataReader["body_raw"];
            if (bodyRaw != DBNull.Value)
            {
                emailMessage.BodyRaw = (byte[])bodyRaw;
            }

            return emailMessage;
        }

        public static EmailMessage GetById(long emailMessageId)
        {
            var emailMessage = SelectOne("SELECT * FROM `emails`", FromReader);

            if (emailMessage != null)
            {
                emailMessage.Attachments.AddRange(EmailAttachment.GetByEmail(emailMessageId));
            }

            return emailMessage;
        }

        public static List<EmailMessage> GetByEmailAddress(EmailAddress emailAddress, DateTime dateFrom, DateTime dateTo, params EmailMessageStatus[] messageStatusArray)
        {
            var commandText = "SELECT * FROM `emails` WHERE `email_address_id` = ?email_address_id `date` BETWEEN ?date_from AND ?date_to";
            if (messageStatusArray.Length > 0)
            {
                commandText += " AND `status` IN (" + string.Join("", messageStatusArray.Select(messageStatus => (int)messageStatus)) + ")";
            }
            commandText += " ORDER BY `date`";

            var emailMessageList = 
                SelectMany(
                    commandText, FromReader,
                        new MySqlParameter("email_address_id", emailAddress.Id),
                        new MySqlParameter("date_from", dateFrom),
                        new MySqlParameter("date_to", dateTo.AddDays(1)));


            var emailAttachmentList = EmailAttachment.GetByEmails(emailMessageList.Select(emailMessage => emailMessage.Id).ToList());

            // Construct a dictionary mapping all the attachments to the emails.
            Dictionary<long, List<EmailAttachment>> emailAttachmentDict = new Dictionary<long, List<EmailAttachment>>();
            foreach (var emailAttachment in emailAttachmentList)
            {
                if (emailAttachment.EmailId.HasValue)
                {
                    if (!emailAttachmentDict.ContainsKey(emailAttachment.EmailId.Value))
                    {
                        emailAttachmentDict[emailAttachment.EmailId.Value] = new List<EmailAttachment>();
                    }
                    emailAttachmentDict[emailAttachment.EmailId.Value].Add(emailAttachment);
                }
            }

            // Append the attachments to each message.
            foreach (var emailMessage in emailMessageList)
            {
                if (emailAttachmentDict.ContainsKey(emailMessage.Id))
                {
                    emailMessage.Attachments.AddRange(emailAttachmentDict[emailMessage.Id]);
                }
            }

            return emailMessageList;
        }

        /// <summary>
        /// Finds all messages matching the specified search criteria.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="address"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchText"></param>
        /// <param name="hasAttachments"></param>
        /// <returns></returns>
        public static List<EmailMessage> Find(long? patientId, string address, DateTime? fromDate, DateTime? toDate, string searchText, bool hasAttachments)
        {
            var conditions = new List<string>();
            var parameters = new List<MySqlParameter>();

            if (patientId.HasValue && patientId.Value > 0)
            {
                conditions.Add("`patient_id` = " + patientId.Value);
            }

            if (!string.IsNullOrEmpty(address))
            {
                parameters.Add(new MySqlParameter("address", $"%{address}%"));

                conditions.Add(
                    "(`from_address` LIKE ?address OR" +
                    " `to_address` LIKE ?address OR" +
                    " `cc_address` LIKE ?address OR" +
                    " `bcc_address` LIKE ?address)");
            }

            if (fromDate.HasValue && fromDate.Value != DateTime.MinValue)
            {
                parameters.Add(new MySqlParameter("from_date", fromDate.Value));

                conditions.Add("DATE(`date`) >= ?from_date");
            }

            if (toDate.HasValue && toDate.Value != DateTime.MinValue)
            {
                parameters.Add(new MySqlParameter("to_date", toDate.Value));

                conditions.Add("DATE(`date`) <= ?to_date");
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                parameters.Add(new MySqlParameter("search_text", $"%{searchText}%"));

                conditions.Add("(`subject` LIKE ?search_text OR `body` LIKE ?search_text)");
            }

            string command = "SELECT * FROM `emails`";
            if (conditions.Count > 0)
            {
                command += " WHERE " + string.Join(" AND ", conditions);
            }

            var emailMessageList = SelectMany(command, FromReader, parameters.ToArray());
            foreach (var emailMessage in emailMessageList)
            {
                emailMessage.Attachments.AddRange(EmailAttachment.GetByEmail(emailMessage.Id));
            }

            return hasAttachments ?
                emailMessageList.Where(emailMessage => emailMessage.Attachments.Count > 0).ToList() :
                emailMessageList;
        }

        public static long Insert(EmailMessage emailMessage)
        {
            emailMessage.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `emails` (`email_address_id`, `patient_id`, `appointment_id`, `user_id`, `provider_id`, `from_name`, `from_address`, `to_address`, `cc_address`, `bcc_address`, " +
                "`subject`, `body`, `body_raw`, `date`, `flags`, `status`) VALUES (?email_address_id, ?patient_id, ?appointment_id, ?user_id, ?provider_id, ?from_name, ?from_address, ?to_address, " +
                "?cc_addresss, ?bcc_address, ?subject, ?body, ?body_raw, ?date, ?flags, ?status)",
                    new MySqlParameter("email_address_id", emailMessage.EmailAddressId),
                    new MySqlParameter("patient_id", emailMessage.PatientId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("appointment_id", emailMessage.AppointmentId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("user_id", emailMessage.UserId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("provider_id", emailMessage.ProviderId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("from_name", emailMessage.FromName ?? ""),
                    new MySqlParameter("from_address", emailMessage.FromAddress ?? ""),
                    new MySqlParameter("to_address", emailMessage.ToAddress ?? ""),
                    new MySqlParameter("cc_address", emailMessage.CcAddress ?? ""),
                    new MySqlParameter("bcc_address", emailMessage.BccAddress ?? ""),
                    new MySqlParameter("subject", emailMessage.Subject ?? ""),
                    new MySqlParameter("body", emailMessage.Body ?? ""),
                    new MySqlParameter("body_raw", emailMessage.BodyRaw ?? (object)DBNull.Value),
                    new MySqlParameter("date", emailMessage.Date),
                    new MySqlParameter("flags", (int)emailMessage.Flags),
                    new MySqlParameter("status", (int)emailMessage.Status));

            foreach (var emailAttachment in emailMessage.Attachments)
            {
                EmailAttachment.Insert(emailAttachment);
            }

            return emailMessage.Id;
        }

        public static void Update(EmailMessage emailMessage) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `emails` SET `email_address_id` = ?email_address_id, `patient_id` = ?patient_id, `appointment_id` = ?appointment_id, `user_id` = ?user_id, `provider_id` = ?provider_id, " +
                "`from_name` = ?from_name, `from_address` = ?from_address, `to_address` = ?to_address, `cc_address` = ?cc_address, `bcc_address` = ?bcc_address, `subject` = ?subject, " +
                "`body` = ?body, `body_raw` = ?body_raw, `flags` = ?flags, `status` = ?status WHERE `id` = ?id",
                    new MySqlParameter("email_address_id", emailMessage.EmailAddressId),
                    new MySqlParameter("patient_id", emailMessage.PatientId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("appointment_id", emailMessage.AppointmentId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("user_id", emailMessage.UserId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("provider_id", emailMessage.ProviderId.HasValue ? (object)emailMessage.PatientId.Value : DBNull.Value),
                    new MySqlParameter("from_name", emailMessage.FromName ?? ""),
                    new MySqlParameter("from_address", emailMessage.FromAddress ?? ""),
                    new MySqlParameter("to_address", emailMessage.ToAddress ?? ""),
                    new MySqlParameter("cc_address", emailMessage.CcAddress ?? ""),
                    new MySqlParameter("bcc_address", emailMessage.BccAddress ?? ""),
                    new MySqlParameter("subject", emailMessage.Subject ?? ""),
                    new MySqlParameter("body", emailMessage.Body ?? ""),
                    new MySqlParameter("body_raw", emailMessage.BodyRaw ?? (object)DBNull.Value),
                    new MySqlParameter("flags", (int)emailMessage.Flags),
                    new MySqlParameter("status", (int)emailMessage.Status),
                    new MySqlParameter("id", emailMessage.Id));

        public static void Delete(long emailId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `emails` WHERE `id` = " + emailId);

        public static void Delete(EmailMessage emailMessage) => Delete(emailMessage.Id);

        /// <summary>
        /// Marks the specified message as read.
        /// </summary>
        /// <param name="emailMessage">The message to update.</param>
        /// <returns>The new status of the message.</returns>
        public static EmailMessageStatus MarkAsRead(EmailMessage emailMessage)
        {
            if (emailMessage.Status == EmailMessageStatus.Received)
            {
                emailMessage.Status = EmailMessageStatus.Read;

                if (emailMessage.Id > 0)
                {
                    DataConnection.ExecuteNonQuery(
                        "UPDATE `emails` SET `status` = ?status WHERE `id` = " + emailMessage.Id,
                            new MySqlParameter("status", (int)emailMessage.Status));
                }
            }

            return emailMessage.Status;
        }

        /// <summary>
        /// Marks the specified message as unread.
        /// </summary>
        /// <param name="emailMessage">The message to update.</param>
        /// <returns>The new status of the message.</returns>
        public static EmailMessageStatus MarkAsNotRead(EmailMessage emailMessage)
        {
            if (emailMessage.Status == EmailMessageStatus.Read)
            {
                emailMessage.Status = EmailMessageStatus.Received;

                if (emailMessage.Id > 0)
                {
                    DataConnection.ExecuteNonQuery(
                        "UPDATE `emails` SET `status` = ?status WHERE `id` = " + emailMessage.Id,
                            new MySqlParameter("status", (int)emailMessage.Status));
                }
            }

            return emailMessage.Status;
        }

        /// <summary>
        /// Assigns the specified mail message to the patient with the specified ID.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="patientId">The ID of the patient.</param>
        public static void AssignToPatient(EmailMessage emailMessage, long patientId)
        {
            if (emailMessage.PatientId == patientId) return;

            emailMessage.PatientId = patientId;
            if (emailMessage.Id > 0)
            {
                DataConnection.ExecuteNonQuery(
                    "UPDATE `emails` SET `patient_id` = ?patient_id WHERE `id` = " + emailMessage.Id,
                        new MySqlParameter("patient_id", patientId));

                if (emailMessage.Attachments != null)
                {
                    foreach (var emailAttachment in emailMessage.Attachments)
                    {
                        var ehrSummaryCcd = EhrSummaryCcds.GetOneForEmailAttach(emailAttachment.Id);
                        if (ehrSummaryCcd != null)
                        {
                            ehrSummaryCcd.PatNum = patientId;

                            EhrSummaryCcds.Update(ehrSummaryCcd);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If EmailDisclaimerIsOn is false then returns emailBody unedited. Otherwise appends EmailDisclaimerTemplate to bottom of emailBody and returns.
        /// Considers clinic postal address when necessary. Defaults to practice postal address if clinics are turned off or current clinic addres is not available.
        /// </summary>
        public static string FindAndReplacePostalAddressTag(string emailBody, long clinicNum)
        {
            // TODO: Rename this method to something more appropriate...

            string disclaimer = GetEmailDisclaimer(clinicNum);
            if (string.IsNullOrEmpty(disclaimer))
            {
                return emailBody;
            }
            return emailBody + "\r\n\r\n\r\n" + disclaimer.ToString();
        }

        /// <summary>
        /// Gets the value in the EmailDisclaimerTemplate preference with the [PostalAddress] 
        /// replaced. Returns an empty string if the pref is off.
        /// </summary>
        /// <param name="clinicId"></param>
        public static string GetEmailDisclaimer(long clinicId)
        {
            if (!Preference.GetBool(PreferenceName.EmailDisclaimerIsOn)) return "";

            var disclaimer = Preference.GetString(PreferenceName.EmailDisclaimerTemplate);
            if (string.IsNullOrEmpty(disclaimer))
            {
                return "";
            }

            string address =
                Preference.GetString(PreferenceName.PracticeTitle) + "\r\n" +
                    Patients.GetAddressFull(
                        Preference.GetString(PreferenceName.PracticeAddress),
                        Preference.GetString(PreferenceName.PracticeAddress2),
                        Preference.GetString(PreferenceName.PracticeCity),
                        Preference.GetString(PreferenceName.PracticeST),
                        Preference.GetString(PreferenceName.PracticeZip));

            if (Preferences.HasClinicsEnabled)
            {
                var clinic = Clinics.GetClinic(clinicId);
                if (clinic != null)
                {
                    string clinicAddress = Patients.GetAddressFull(clinic.Address, clinic.Address2, clinic.City, clinic.State, clinic.Zip);
                    if (!string.IsNullOrWhiteSpace(clinicAddress.Replace(" ", "").Replace("\r\n", "").Replace(",", "")))
                    {
                        address = clinic.Description + "\r\n" + clinicAddress;
                    }
                }
            }

            return disclaimer.Replace("[PostalAddress]", address);
        }

        /// <summary>
        /// Sends a e-mail message.
        /// </summary>
        /// <param name="emailAddress">The address from which the send the e-mail message.</param>
        /// <param name="emailMessage">The e-mail message to send.</param>
        /// <param name="emailMessageHeaders">Optional additional headers to include with the message.</param>
        public static void Send(EmailAddress emailAddress, EmailMessage emailMessage, NameValueCollection emailMessageHeaders = null)
        {
            if (!Security.IsAuthorized(Permissions.EmailSend, true)) return;

            emailMessage.UserId = Security.CurUser.UserNum;

            Send(
                emailAddress,
                emailMessage,
                emailMessageHeaders,
                GetAndDownloadAttachmentList(emailMessage));

            SecurityLogs.MakeLogEntry(Permissions.EmailSend, emailMessage.PatientId.Value, "Email Sent");
        }

        /// <summary>
        /// Sends a e-mail message.
        /// </summary>
        /// <param name="emailAddress">The address from which the send the e-mail message.</param>
        /// <param name="emailMessage">The e-mail message to send.</param>
        /// <param name="emailMessageHeaders">Optional additional headers to include with the message.</param>
        /// <param name="attachments"></param>
        static void Send(EmailAddress emailAddress, EmailMessage emailMessage, NameValueCollection emailMessageHeaders, List<Tuple<string, string>> attachments)
        {
            if (string.IsNullOrWhiteSpace(emailMessage.FromAddress)) return;

            using (var smtpClient = new SmtpClient(emailAddress.SmtpServer, emailAddress.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(emailAddress.SmtpUsername, MiscUtils.Decrypt(emailAddress.SmtpPassword, true));
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = emailAddress.UseSsl;
                smtpClient.Timeout = 180000; // 3 Minutes

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(emailMessage.FromAddress.Trim());
                    if (!string.IsNullOrWhiteSpace(emailMessage.ToAddress))
                    {
                        mailMessage.To.Add(emailMessage.ToAddress.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(emailMessage.CcAddress))
                    {
                        mailMessage.CC.Add(emailMessage.CcAddress.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(emailMessage.BccAddress))
                    {
                        mailMessage.Bcc.Add(emailMessage.BccAddress.Trim());
                    }

                    mailMessage.Subject = emailMessage.Subject ?? "";
                    mailMessage.IsBodyHtml = false;
                    mailMessage.Body = emailMessage.Body;

                    if (emailMessageHeaders != null) mailMessage.Headers.Add(emailMessageHeaders);

                    if (!emailMessage.Attachments.IsNullOrEmpty())
                    {
                        foreach (var attachmentData in attachments)
                        {
                            var attachment = new Attachment(attachmentData.Item1)
                            {
                                Name = attachmentData.Item2
                            };
                            mailMessage.Attachments.Add(attachment);
                        }
                    }

                    smtpClient.Send(mailMessage);
                }
            }
        }

        /// <summary>
        /// Receives message from the specified e-mail address.
        /// </summary>
        /// <param name="emailAddress">The e-mail address.</param>
        /// <returns>A list of received e-mail messages.</returns>
        public static IEnumerable<EmailMessage> Receive(EmailAddress emailAddress)
        {
            lock (receivingEmailAddressIdList)
            {
                if (receivingEmailAddressIdList.Contains(emailAddress.Id))
                {
                    yield break;
                }

                receivingEmailAddressIdList.Add(emailAddress.Id);
            }

            foreach (var emailMessage in ReceiveInternal(emailAddress))
            {
                yield return emailMessage;
            }

            lock (receivingEmailAddressIdList) receivingEmailAddressIdList.Remove(emailAddress.Id);
        }

        /// <summary>
        /// Receives message from the specified e-mail address.
        /// </summary>
        /// <param name="emailAddress">The e-mail address.</param>
        /// <returns>A list of received e-mail messages.</returns>
        static IEnumerable<EmailMessage> ReceiveInternal(EmailAddress emailAddress)
        {
            var newMessages = new List<EmailMessage>();

            using (var pop3Client = new Pop3Client())
            {
                pop3Client.Connect(emailAddress.Pop3Server, emailAddress.Pop3Port, emailAddress.UseSsl, 180000, 180000, null); // 3 minute timeout, just as for sending emails.

                // Authenticate with the POP3 server.
                pop3Client.Authenticate(emailAddress.SmtpUsername.Trim(),
                    MiscUtils.Decrypt(emailAddress.SmtpPassword), AuthenticationMethod.UsernameAndPassword);

                // Get the UIDs of all messages.
                var uids = pop3Client.GetMessageUids();
                var seenUids = EmailMessageUid.GetByEmailAddress(emailAddress.Id);

                // Fetch all the messages that we haven't seen yet.
                for (int i = 0; i < uids.Count; i++)
                {
                    var uid = uids[i];

                    if (uid.Length == 0)
                    {
                        var headers = pop3Client.GetMessageHeaders(i + 1);
                        if (!string.IsNullOrEmpty(headers.MessageId))
                        {
                            uid = headers.MessageId;
                        }
                        else
                        {
                            uid = headers.DateSent.ToString("yyyyMMddHHmmss") + emailAddress.SmtpUsername.Trim() + headers.From.Address + headers.Subject;
                        }
                    }
                    if (uid.Length > 4000) uid = uid.Substring(4000);

                    if (seenUids.Contains(uid)) continue;

                    var message = ParseMessage(pop3Client.GetMessage(i + 1));
                    if (message != null)
                    {
                        newMessages.Add(message);
                    }

                    EmailMessageUid.Insert(emailAddress.Id, uid);
                    seenUids.Add(uid);

                    yield return message;
                }
            }
        }

        /// <summary>
        /// Parses the specified <see cref="OpenPop.Mime.Message"/> instance and creates a new <see cref="EmailMessage"/> instance.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <returns></returns>
        static EmailMessage ParseMessage(OpenPop.Mime.Message message)
        {
            string body = "";

            var messagePart = message.FindFirstPlainTextVersion();
            if (messagePart != null)
            {
                body = messagePart.GetBodyAsText();
            }
            else
            {
                messagePart = message.FindFirstHtmlVersion();
                if (messagePart != null)
                {
                    body = messagePart.GetBodyAsText();
                }
            }

            var emailMessage = new EmailMessage
            {
                FromName = message.Headers.From.DisplayName,
                FromAddress = message.Headers.From.Address,
                ToAddress = string.Join("; ", message.Headers.To.Select(a => a.Raw)),
                CcAddress = string.Join("; ", message.Headers.Cc.Select(a => a.Raw)),
                BccAddress = string.Join("; ", message.Headers.Bcc.Select(a => a.Raw)),
                Subject = message.Headers.Subject,
                Body = body,
                BodyRaw = null,
                Date = message.Headers.DateSent,
                Flags = EmailMessageFlags.None,
                Status = EmailMessageStatus.Received
            };

            var attachments = message.FindAllAttachments();
            foreach (var attachment in attachments)
            {
                try
                {
                    var emailAttachment = EmailAttachment.CreateAttachment(attachment.FileName, attachment.Body);
                    if (emailAttachment != null)
                    {
                        emailMessage.Attachments.Add(emailAttachment);
                    }
                }
                catch
                {
                }
            }

            Insert(emailMessage);

            return emailMessage;
        }

        /// <summary>
        /// Fetches all attachments from the specified message and returns a list of tuples representing the 
        /// attachments. The first item is the full path of the attachment, and the second item is the display
        /// name of the attachment.
        /// 
        /// When sending mails with attachments the attachments must exist on the local filesystem. That
        /// means in cases were cloud bases storage is used we first need to download the attachments from
        /// the cloud before we can send them.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        static List<Tuple<string, string>> GetAndDownloadAttachmentList(EmailMessage emailMessage)
        {
            // TODO: Fix this...


            if (emailMessage.Attachments.IsNullOrEmpty())
            {
                return null;
            }

            var attachmentPathList = new List<Tuple<string, string>>();
            var attachmentPath = EmailAttachment.GetAttachmentPath();

            foreach (var emailAttachment in emailMessage.Attachments)
            {
                string attachFullPath;

                if (CloudStorage.IsCloudStorage)
                {
                    OpenDentalCloud.Core.TaskStateDownload state = CloudStorage.Download(attachmentPath, emailAttachment.FileName);
                    attachFullPath = Preferences.GetRandomTempFile(Path.GetExtension(emailAttachment.FileName));
                    File.WriteAllBytes(attachFullPath, state.FileContent);
                }
                else
                {
                    attachFullPath = Path.Combine(attachmentPath, emailAttachment.FileName);
                }

                attachmentPathList.Add(new Tuple<string, string>(attachFullPath, emailAttachment.Description));
            }
            return attachmentPathList;
        }

        public static string GetAddressSimple(string address) => new MailAddress(address).Address;

        public static EmailMessage CreateReply(EmailMessage emailMessage, EmailAddress emailAddress, bool isReplyAll = false)
        {
            var newEmailMessage = new EmailMessage
            {
                PatientId = emailMessage.PatientId,
                FromAddress = emailAddress.Sender,
                ToAddress = emailMessage.FromAddress,
                Subject = emailMessage.Subject.Trim()
            };

            if (isReplyAll)
            {
                emailMessage.ToAddress.Split(';').ToList()
                    .FindAll(x => x != emailAddress.SmtpUsername && x != emailAddress.Sender)
                    .ForEach(x => newEmailMessage.ToAddress += "," + x);
            }

            if (newEmailMessage.Subject.Length >= 3 &&
                newEmailMessage.Subject.Trim().Substring(0, 3).ToLower() != "re:")
            {
                newEmailMessage.Subject = "RE: " + emailMessage.Subject;
            }

            newEmailMessage.Body =
                "\r\n\r\n\r\n" + "On " + emailMessage.Date.ToString() + " " + emailMessage.FromAddress + " sent:\r\n>" + emailMessage.Body;

            return newEmailMessage;
        }

        public static EmailMessage CreateForward(EmailMessage emailMessage, EmailAddress emailAddress)
        {
            var newEmailMessage = new EmailMessage
            {
                PatientId = emailMessage.PatientId,
                FromAddress = emailAddress.Sender,
                ToAddress = emailMessage.FromAddress,
                Subject = emailMessage.Subject.Trim()
            };

            if (newEmailMessage.Subject.Length >= 4 &&
                newEmailMessage.Subject.Trim().Substring(0, 4).ToLower() != "fwd:")
            {
                newEmailMessage.Subject = "FWD: " + emailMessage.Subject;
            }

            newEmailMessage.Body =
                "\r\n\r\n\r\n" + "On " + emailMessage.Date.ToString() + " " + emailMessage.FromAddress + " sent:\r\n>" + emailMessage.Body;

            return newEmailMessage;
        }

        /// <summary>
        /// Gets all the distinc e-mail addresses from the specified list of e-mail messages.
        /// </summary>
        /// <param name="emailMessages">The list of e-mail messages to retrieve the addresses from.</param>
        /// <returns>A list of e-mail addresses.</returns>
        public static IEnumerable<string> GetAddresses(IEnumerable<EmailMessage> emailMessages) =>
            emailMessages
                .Where(emailMessage => !string.IsNullOrEmpty(emailMessage.FromAddress))
                    .Select(emailMessage => emailMessage.FromAddress)
                .Union(
                    emailMessages.Where(emailMessage => !string.IsNullOrWhiteSpace(emailMessage.ToAddress))
                        .Select(emailMessage => emailMessage.ToAddress))
                .Union(
                    emailMessages.Where(emailMessage => !string.IsNullOrWhiteSpace(emailMessage.CcAddress))
                        .Select(emailMessage => emailMessage.CcAddress))
                .Union(
                    emailMessages.Where(emailMessage => !string.IsNullOrWhiteSpace(emailMessage.BccAddress))
                        .Select(emailMessage => emailMessage.BccAddress))
                .Distinct();

        //public EmailMessage Copy()
        //{
        //    EmailMessage e = (EmailMessage)this.MemberwiseClone();
        //    e.Attachments = new List<EmailAttachment>();
        //    for (int i = 0; i < Attachments.Count; i++)
        //    {
        //        e.Attachments.Add(Attachments[i].Copy());
        //    }
        //    return e;
        //}

        // TODO: We removed the WebMail and Direct





        public bool IsUnsent => Status == EmailMessageStatus.Unknown;

        public bool IsReceived => Status == EmailMessageStatus.Received;
    }

    /// <summary>
    /// Identifies the status of a e-mail message.
    /// </summary>
    public enum EmailMessageStatus
    {
        Unknown = 0, // TOOO: Rename to draft
        
        /// <summary>
        /// Indicates whether a <see cref="EmailMessage"/> was sent.
        /// </summary>
        Sent,
        
        /// <summary>
        /// Indicates whether a <see cref="EmailMessage"/> was received.
        /// </summary>
        Received,

        /// <summary>
        /// Indicates whether a <see cref="EmailMessage"/> has been read.
        /// </summary>
        Read,
    }

    [Flags]
    public enum EmailMessageFlags
    {
        None = 0,

        /// <summary>
        /// Indicates whehter a <see cref="EmailMessage"/> has been hidden.
        /// </summary>
        Hidden = 1,
    }
}