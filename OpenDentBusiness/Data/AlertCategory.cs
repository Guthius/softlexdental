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
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class AlertCategory : DataRecord
    {
        /// <summary>
        /// The name used to identify the type of alert category this started as, allows us to associate new alerts.
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The name displayed to user when subscribing to alerts categories.
        /// </summary>
        public string Description;

        /// <summary>
        /// Indicates whether the category is locked. Locked categories cannot be edited or deleted.
        /// </summary>
        public bool Locked;

        /// <summary>
        /// Constructs a new instance of the <see cref="AlertCategory"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AlertCategory"/> instance.</returns>
        static AlertCategory FromReader(MySqlDataReader dataReader)
        {
            return new AlertCategory
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Name = Convert.ToString(dataReader["name"]),
                Description = Convert.ToString(dataReader["description"]),
                Locked = Convert.ToBoolean(dataReader["locked"])
            };
        }

        /// <summary>
        /// Gets a list containing all alert categories.
        /// </summary>
        /// <returns>A list of alert categories.</returns>
        public static List<AlertCategory> All() =>
            SelectMany("SELECT * FROM `alert_categories`", FromReader);

        /// <summary>
        /// Gets the alert category with the specified ID.
        /// </summary>
        /// <param name="alertCategoryId">The ID of the alert category.</param>
        /// <returns></returns>
        public static AlertCategory GetById(long alertCategoryId) =>
            SelectOne("SELECT * FROM `alert_categories` WHERE `id` = " + alertCategoryId, FromReader);

        /// <summary>
        /// Inserts the specified alert category into the database.
        /// </summary>
        /// <param name="alertCategory">The alert category.</param>
        /// <returns>The ID assigned to the alert category.</returns>
        public static long Insert(AlertCategory alertCategory) =>
            alertCategory.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `alert_categories` (`name`, `description`, `locked`) VALUES (?name, ?description, ?locked)",
                    new MySqlParameter("name", alertCategory.Name),
                    new MySqlParameter("description", alertCategory.Description),
                    new MySqlParameter("locked", alertCategory.Locked));

        /// <summary>
        /// Updates the specified alert category in the database.
        /// </summary>
        /// <param name="alertCategory">The alert category to update.</param>
        public static void Update(AlertCategory alertCategory) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `alert_categories` SET `name` = ?name, `description` = ?description, `locked` = ?locked WHERE `id` = ?id",
                    new MySqlParameter("name", alertCategory.Name),
                    new MySqlParameter("description", alertCategory.Description),
                    new MySqlParameter("locked", alertCategory.Locked),
                    new MySqlParameter("id", alertCategory.Id));

        /// <summary>
        /// Deletes the alert category with the specified ID from the database.
        /// </summary>
        /// <param name="alertCategoryId">The ID of the alert category.</param>
        public static void Delete(long alertCategoryId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `alert_categories` WHERE `id` = " + alertCategoryId);
    }
}