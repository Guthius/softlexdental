using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class EmailAutograph : DataRecord
    {
        public string Description;
        public string Autograph;
        public string EmailAddress;

        public override string ToString() => Description ?? "";

        static EmailAutograph FromReader(MySqlDataReader dataReader)
        {
            return new EmailAutograph
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                EmailAddress = Convert.ToString(dataReader["address"]),
                Autograph = Convert.ToString(dataReader["autograph"])
            };
        }

        public static List<EmailAutograph> All() =>
            SelectMany("SELECT * FROM `email_autographs`", FromReader);

        public static EmailAutograph GetById(long emailAutographId) =>
            SelectOne("SELECT * FROM `email_autographs` WHERE `id` = " + emailAutographId, FromReader);

        public static void Insert(EmailAutograph emailAutograph) =>
            emailAutograph.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `email_autographs` (`description`, `address`, `autograph`) VALUES (?description, ?address, ?autograph)",
                    new MySqlParameter("description", emailAutograph.Description ?? ""),
                    new MySqlParameter("address", emailAutograph.EmailAddress ?? ""),
                    new MySqlParameter("autograph", emailAutograph.Autograph ?? ""));

        public static void Update(EmailAutograph emailAutograph) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `email_autographs` SET `description` = ?description, `address` = ?address, `autograph` = ?autograph WHERE `id` = ?id",
                    new MySqlParameter("description", emailAutograph.Description ?? ""),
                    new MySqlParameter("address", emailAutograph.EmailAddress ?? ""),
                    new MySqlParameter("autograph", emailAutograph.Autograph ?? ""),
                    new MySqlParameter("id", emailAutograph.Id));

        public static void Delete(long emailAutographId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `email_autographs` WHERE `id` = " + emailAutographId);
    }
}