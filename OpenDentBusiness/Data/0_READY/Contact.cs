using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Like a rolodex for businesses that the office interacts with. 
    /// 
    /// Used to store pharmacies, etc.
    /// </summary>
    public class Contact : DataRecord
    {
        /// <summary>
        /// Last name of the contact or, frequently, the entire name.
        /// </summary>
        public string LastName;
        
        /// <summary>
        /// First name of the contact (optional).
        /// </summary>
        public string FirstName;
        
        /// <summary>
        /// Work phone number.
        /// </summary>
        public string Phone;
        
        /// <summary>
        /// Fax number.
        /// </summary>
        public string Fax;
        
        /// <summary>
        /// Foreign key to <see cref="Definition.Id"/>.
        /// </summary>
        public long? Category;
        
        /// <summary>
        /// Notes for the contact.
        /// </summary>
        public string Notes;

        /// <summary>
        /// Constructs a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Contact"/> instance.</returns>
        static Contact FromReader(MySqlDataReader dataReader)
        {
            var contact = new Contact
            {
                Id = Convert.ToInt64(dataReader["id"]),
                LastName = Convert.ToString(dataReader["lastname"]),
                FirstName = Convert.ToString(dataReader["firstname"]),
                Phone = Convert.ToString(dataReader["phone"]),
                Fax = Convert.ToString(dataReader["fax"]),
                Notes = Convert.ToString(dataReader["notes"])
            };

            var category = dataReader["category"];
            if (category != DBNull.Value)
            {
                contact.Category = Convert.ToInt32(category);
            }

            return contact;
        }

        /// <summary>
        /// Gets a list of all contacts.
        /// </summary>
        /// <returns>A list of contacts.</returns>
        public static List<Contact> All() =>
            SelectMany("SELECT * FROM contacts", FromReader);

        /// <summary>
        /// Gets the contact with the specified ID.
        /// </summary>
        /// <param name="contactId">The ID of the contact.</param>
        /// <returns>The contact with the specified ID.</returns>
        public static Contact GetById(long contactId) =>
            SelectOne("SELECT * FROM contacts WHERE id = " + contactId, FromReader);

        /// <summary>
        /// Gets a list of all contacts in the specified category.
        /// </summary>
        /// <param name="category">The ID of the category.</param>
        /// <returns>A list of contacts.</returns>
        public static List<Contact> GetByCategory(int category) =>
            SelectMany("SELECT * FROM contacts WHERE category = @category ORDER BY lastname", FromReader,
                new MySqlParameter("category", category));

        /// <summary>
        /// Inserts the specified contact into the database.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>The ID assigned to the contact.</returns>
        public static long Insert(Contact contact) =>
            contact.Id = DataConnection.ExecuteInsert(
                "INSERT INTO contacts (lastname, firstname, phone, fax, category_id, notes) VALUES (@lastname, @firstname, @phone, @fax, @category_id, @notes)",
                    new MySqlParameter("lastname", contact.LastName ?? ""),
                    new MySqlParameter("firstname", contact.FirstName ?? ""),
                    new MySqlParameter("phone", contact.Phone ?? ""),
                    new MySqlParameter("fax", contact.Fax ?? ""),
                    new MySqlParameter("category_id", contact.Category.HasValue ? (object)contact.Category.Value : DBNull.Value),
                    new MySqlParameter("notes", contact.Notes ?? ""));

        /// <summary>
        /// Updates the specified contact in the database.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public static void Update(Contact contact) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE contacts SET lastname = @lastname, firstname = @firstname, phone = @phone, fax = @fax, category_id = @category_id, notes = @notes WHERE id = @id",
                    new MySqlParameter("lastname", contact.LastName ?? ""),
                    new MySqlParameter("firstname", contact.FirstName ?? ""),
                    new MySqlParameter("phone", contact.Phone ?? ""),
                    new MySqlParameter("fax", contact.Fax ?? ""),
                    new MySqlParameter("category_id", contact.Category.HasValue ? (object)contact.Category.Value : DBNull.Value),
                    new MySqlParameter("notes", contact.Notes ?? ""),
                    new MySqlParameter("id", contact.Id));

        /// <summary>
        /// Deletes the specified contact from the database.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public static void Delete(Contact contact)
        {
            if (contact != null && contact.Id > 0)
            {
                DataConnection.ExecuteNonQuery("DELETE FROM contacts WHERE id = " + contact.Id);

                contact.Id = 0;
            }
        }
    }
}