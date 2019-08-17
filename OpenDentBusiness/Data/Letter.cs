using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// These are templates that are used to send simple letters to patients.
    /// </summary>
    public class Letter : DataRecord
    {
        /// <summary>
        /// A description of the letter.
        /// </summary>
        public string Description;

        /// <summary>
        /// Text of the letter
        /// </summary>
        public string Body;

        static Letter FromReader(MySqlDataReader dataReader)
        {
            return new Letter
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Body = Convert.ToString(dataReader["body"])
            };
        }
        
        public static List<Letter> All() =>
            SelectMany("SELECT * FROM letters", FromReader);

        public static long Insert(Letter letter) =>
            letter.Id = DataConnection.ExecuteLong(
                "INSERT INTO letters (description, body) VALUES (:description, :body)",
                    new MySqlParameter("description", letter.Description ?? ""),
                    new MySqlParameter("body", letter.Body ?? ""));

        public static void Update(Letter letter) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE letters SET description = :description, body = :body WHERE id = :id",
                    new MySqlParameter("description", letter.Description ?? ""),
                    new MySqlParameter("body", letter.Body ?? ""),
                    new MySqlParameter("id", letter.Id));

        public static void Delete(long letterId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM letters WHERE id = " + letterId);

        public Letter Copy() => (Letter)MemberwiseClone();
    }
}