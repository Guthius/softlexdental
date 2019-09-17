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
    /// A dental supply or office supply item.
    /// </summary>
    public class Supply : DataRecord
    {
        public long SupplierId;
        public string CatalogNumber;

        /// <summary>
        /// The description can be similar to the catalog, but not required.  Typically includes qty per box/case, etc.
        /// </summary>
        public string Description;

        /// <summary>
        /// Scanned code from a reader.
        /// </summary>
        public string BarCode;

        /// <summary>
        /// The ID of a <see cref="Definition"/> that represents the supply category.
        /// </summary>
        public long CategoryId;

        /// <summary>
        /// Quantity of the supply available.
        /// </summary>
        public float LevelOnHand;

        /// <summary>
        /// The level that a fresh order should bring item back up to. Can include fractions. 
        /// If this is 0, then it will be displayed as having this field blank rather than showing 0.
        /// </summary>
        public float LevelDesired;

        /// <summary>
        /// The price per unit that the supplier charges for this supply. 
        /// If this is 0, then no price will be displayed.
        /// </summary>
        public double Price;

        /// <summary>
        /// If hidden, then this supply item won't normally show in the main list.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// Constructs a new instance of the <see cref="Supply"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Supply"/> instance.</returns>
        private static Supply FromReader(MySqlDataReader dataReader)
        {
            return new Supply
            {
                Id = (long)dataReader["id"],
                SupplierId = (long)dataReader["supplier_id"],
                CatalogNumber = (string)dataReader["catalog_number"],
                Description = (string)dataReader["description"],
                BarCode = (string)dataReader["barcode"],
                CategoryId = (long)dataReader["category_id"],
                LevelOnHand = Convert.ToSingle(dataReader["level_on_hand"]),
                LevelDesired = Convert.ToSingle(dataReader["level_desired"]),
                Price = (double)dataReader["price"],
                Hidden = (bool)dataReader["hidden"]
            };
        }

        /// <summary>
        /// Gets a list of all supplies from the database.
        /// </summary>
        /// <returns>A list of supplies.</returns>
        public static List<Supply> All() =>
            SelectMany(
                "SELECT `supplies`.* FROM `supplies`, `definitions` WHERE `definitions`.`id` = `supplies`.`category_id` " +
                "ORDER BY `definitions`.`sort_order`",
                FromReader);

        /// <summary>
        /// Gets the supply with the specified ID from the database.
        /// </summary>
        /// <param name="supplyId">The ID of the supply.</param>
        /// <returns>The supply with the specified ID.</returns>
        public static Supply GetById(long supplyId) =>
            SelectOne("SELECT * FROM `supplies` WHERE `id` = " + supplyId, FromReader);
       
        /// <summary>
        /// Inserts the specified supply into the database.
        /// </summary>
        /// <param name="supply">The supply.</param>
        /// <returns>The ID assigned to the supply.</returns>
        public static long Insert(Supply supply) =>
            supply.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `supplies` (`supplier_id`, `catalog_number`, `description`, `barcode`, `category_id`, `level_on_hand`, " +
                "`level_desired`, `price`, `hidden`) VALUSE (?supplier_id, ?catalog_number, ?description, ?barcode, ?category_id, " +
                "?level_on_hand, ?level_desired, ?price, ?hidden)",
                    new MySqlParameter("supplier_id", supply.SupplierId),
                    new MySqlParameter("catalog_number", supply.CatalogNumber ?? ""),
                    new MySqlParameter("description", supply.Description ?? ""),
                    new MySqlParameter("barcode", supply.BarCode ?? ""),
                    new MySqlParameter("category_id", supply.CategoryId),
                    new MySqlParameter("level_on_hand", supply.LevelOnHand),
                    new MySqlParameter("level_desired", supply.LevelDesired),
                    new MySqlParameter("price", supply.Price),
                    new MySqlParameter("hidden", supply.Hidden));

        /// <summary>
        /// Updates the specified supply in the database.
        /// </summary>
        /// <param name="supply">The supply.</param>
        public static void Update(Supply supply) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `supplies` SET `supplier_id` = ?supplier_id, `catalog_number` = ?catalog_number, `description` = ?description, " +
                "`barcode` = ?barcode, `category_id` = ?category_id, `level_on_hand` = ?level_on_hand, `level_desired` = ?level_desired, " +
                "`price` = ?price, `hidden` = ?hidden WHERE `id` = ?id",
                    new MySqlParameter("supplier_id", supply.SupplierId),
                    new MySqlParameter("catalog_number", supply.CatalogNumber ?? ""),
                    new MySqlParameter("description", supply.Description ?? ""),
                    new MySqlParameter("barcode", supply.BarCode ?? ""),
                    new MySqlParameter("category_id", supply.CategoryId),
                    new MySqlParameter("level_on_hand", supply.LevelOnHand),
                    new MySqlParameter("level_desired", supply.LevelDesired),
                    new MySqlParameter("price", supply.Price),
                    new MySqlParameter("hidden", supply.Hidden),
                    new MySqlParameter("id", supply.Id));

        /// <summary>
        /// Deletes the specified supply from the database.
        /// </summary>
        /// <param name="supply">The supply.</param>
        /// <exception cref="Exception">If the supply is in use and cannot be deleted.</exception>
        public static void Delete(Supply supply)
        {
            var count = 
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `supply_order_item` WHERE `supply_id` = " + supply.Id);

            if (count > 0)
                throw new Exception("Supply is already in use on an order. Not allowed to delete.");

            DataConnection.ExecuteNonQuery("DELETE FROM `supplies` WHERE `id` = " + supply.Id);
        }
    }
}
