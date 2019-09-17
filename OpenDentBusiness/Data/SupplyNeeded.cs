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
    /// A supply freeform typed in by a user.
    /// </summary>
    public class SupplyNeeded : DataRecord
    {
        public string Description;
        public DateTime DateAdded = DateTime.Today;

        /// <summary>
        /// Constructs a new instance of the <see cref="SupplyNeeded"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="SupplyNeeded"/> instance.</returns>
        private static SupplyNeeded FromReader(MySqlDataReader dataReader)
        {
            return new SupplyNeeded
            {
                Id = (long)dataReader["id"],
                Description = (string)dataReader["description"],
                DateAdded = (DateTime)dataReader["date_added"]
            };
        }

        /// <summary>
        /// Gets a list of all needed supply.
        /// </summary>
        /// <returns>A list of needed supply.</returns>
        public static List<SupplyNeeded> All() =>
            SelectMany("SELECT * FROM `supply_needed` ORDER BY `date_added`", FromReader);

        /// <summary>
        /// Inserts the specified needed supply into the database.
        /// </summary>
        /// <param name="supplyNeeded">The supply needed.</param>
        /// <returns></returns>
        public static long Insert(SupplyNeeded supplyNeeded) =>
            supplyNeeded.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `supply_needed` (`description`, `date_added`) VALUES (?description, ?date_added)",
                    new MySqlParameter("description", supplyNeeded.Description ?? ""),
                    new MySqlParameter("date_added", supplyNeeded.DateAdded));

        /// <summary>
        /// Updates the specified neded supply in the database.
        /// </summary>
        /// <param name="supplyNeeded">The supply needed.</param>
        public static void Update(SupplyNeeded supplyNeeded) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `supply_needed` SET `description` = ?description, `date_added` = ?date_added WHERE `id` = ?id",
                    new MySqlParameter("description", supplyNeeded.Description ?? ""),
                    new MySqlParameter("date_added", supplyNeeded.DateAdded),
                    new MySqlParameter("id", supplyNeeded.Id));

        /// <summary>
        /// Deletes the specified needed supply from the database.
        /// </summary>
        /// <param name="supplyNeeded">The supply needed.</param>
        public static void Delete(SupplyNeeded supplyNeeded) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `supply_needed` WHERE `id` = " + supplyNeeded.Id);
    }
}