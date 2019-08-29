using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used to track which email messages have been downloaded into the inbox for a particular recipient address.
    /// Not linked to the email message itself because no link is needed.
    /// If we decide to add a foreign key to a EmailMessage later, we should consider what do to when an email message is deleted (set the foreign key to 0 perhaps).
    /// </summary>
    public class EmailMessageUid : DataRecord
    {
        public long EmailAddressId;
        public string Uid;

        public static List<string> GetByEmailAddress(long emailAddressId) =>
            SelectMany("SELECT `uid` FROM `email_uids` WHERE `email_address_id` = " + emailAddressId, 
                (dataReader) => Convert.ToString(dataReader[0]));

        public static void Insert(long emailAddressId, string uid) =>
            DataConnection.ExecuteNonQuery("INSERT IGNORE INTO `email_uids` (`email_address_id`, `uid`) VALUES (?email_address_id, ?uid)",
                new MySqlParameter("email_address_id", emailAddressId),
                new MySqlParameter("uid", uid ?? ""));
    }
}