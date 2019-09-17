/**
* Copyright (C) 2019 Dental Stars SRL
* Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
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
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// A company that provides supplies for the office, typically dental supplies.
    /// </summary>
    public class Supplier : DataRecord
    {
        public string Name;
        public string Phone;

        /// <summary>
        /// The customer ID that this office uses for transactions with the supplier
        /// </summary>
        public string CustomerId;

        /// <summary>
        /// Full address to website.  We might make it clickable.
        /// </summary>
        public string Website;

        /// <summary>
        /// The username used to log in to the supplier website.
        /// </summary>
        public string UserName;

        /// <summary>
        /// The password to log in to the supplier website. Not encrypted or hidden in any way.
        /// </summary>
        public string Password;

        /// <summary>
        /// Any note regarding supplier. Could hold address, CC info, etc.
        /// </summary>
        public string Note;

        /// <summary>
        /// Constructs a new instance of the <see cref="Supplier"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Supplier"/> instance.</returns>
        private static Supplier FromReader(MySqlDataReader dataReader)
        {
            return new Supplier
            {
                Id = (long)dataReader["id"],
                Name = (string)dataReader["name"],
                Phone = (string)dataReader["phone"],
                CustomerId = (string)dataReader["customer_id"],
                Website = (string)dataReader["website"],
                UserName = (string)dataReader["username"],
                Password = (string)dataReader["password"],
                Note = (string)dataReader["note"]
            };
        }

        /// <summary>
        /// Gets a list of all suppliers from the database.
        /// </summary>
        /// <returns>A list of suppliers.</returns>
        public static List<Supplier> All() =>
            SelectMany("SELECT * FROM `suppliers` ORDER BY `name`", FromReader);

        /// <summary>
        /// Gets the supplier with the specified ID from the database.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>The supplier with the specified ID.</returns>
        public static Supplier GetById(long supplierId) =>
            SelectOne("SELECT * FROM `suppliers` WHERE `id` = " + supplierId, FromReader);

        /// <summary>
        /// Inserts the specified supplier into the database.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        /// <returns>The ID assigned to the supplier.</returns>
        public static long Insert(Supplier supplier) =>
            supplier.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `suppliers` (`name`, `phone`, `customer_id`, `website`, `username`, `password`, `note`) " +
                "VALUES (?name, ?phone, ?customer_id, ?website, ?username, ?password, ?note)",
                    new MySqlParameter("name", supplier.Name ?? ""),
                    new MySqlParameter("phone", supplier.Phone ?? ""),
                    new MySqlParameter("customer_id", supplier.CustomerId ?? ""),
                    new MySqlParameter("website", supplier.Website ?? ""),
                    new MySqlParameter("username", supplier.UserName ?? ""),
                    new MySqlParameter("password", supplier.Password ?? ""),
                    new MySqlParameter("note", supplier.Note ?? ""));

        /// <summary>
        /// Updates the specified supplier in the database.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        public static void Update(Supplier supplier) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `suppliers` SET `name` = ?name, `phone` = ?phone, `customer_id` = ?customer_id, `website` = ?website, " +
                "`username` = ?username, `password` = ?password, `note` = ?note WHERE `id` = ?id",
                    new MySqlParameter("name", supplier.Name ?? ""),
                    new MySqlParameter("phone", supplier.Phone ?? ""),
                    new MySqlParameter("customer_id", supplier.CustomerId ?? ""),
                    new MySqlParameter("website", supplier.Website ?? ""),
                    new MySqlParameter("username", supplier.UserName ?? ""),
                    new MySqlParameter("password", supplier.Password ?? ""),
                    new MySqlParameter("note", supplier.Note ?? ""),
                    new MySqlParameter("id", supplier.Id));

        /// <summary>
        /// Deletes the specified supplier from the database.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        /// <exception cref="Exception">If the supplier is in use and cannot be deleted.</exception>
        public static void Delete(Supplier supplier)
        {
            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `supply_orders` WHERE `supplier_id` = " + supplier.Id);

            if (count > 0)
                throw new Exception("Supplier is already in use on an order. Not allowed to delete.");

            count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `supplies` WHERE `supplier_id` = " + supplier.Id);

            if (count > 0)
                throw new Exception("Supplier is already in use on a supply. Not allowed to delete.");

            DataConnection.ExecuteNonQuery("DELETE FROM `suppliers` WHERE `id` = " + supplier.Id);
        }

        /// <summary>
        /// Gets the name of the supplier with the specified ID from the given list.
        /// </summary>
        /// <param name="suppliers">A list of suppliers.</param>
        /// <param name="supplierId">The ID of the supplier whose name to get.</param>
        /// <returns>
        /// The name of the supplier with the specified ID; or a empty string if no supplier with
        /// the given ID exists in the specified list.
        /// </returns>
        public static string GetName(IEnumerable<Supplier> suppliers, long supplierId)
        {
            foreach (var supplier in suppliers)
            {
                if (supplier.Id == supplierId)
                {
                    return supplier.Name;
                }
            }

            return "";
        }
    }
}
