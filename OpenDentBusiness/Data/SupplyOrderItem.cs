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
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// One item on one supply order.
    /// This table links supplies to orders as well as storing a small amount of additional info.
    /// </summary>
    public class SupplyOrderItem : DataRecord
    {
        public long SupplyOrderId;
        public long SupplyId;

        /// <summary>
        /// The number of items ordered.
        /// </summary>
        public int Quantity;

        /// <summary>
        /// The unit price.
        /// </summary>
        public double Price;

        /// <summary>
        /// Constructs a new instance of the <see cref="SupplyOrderItem"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="SupplyOrderItem"/> instance.</returns>
        private static SupplyOrderItem FromReader(MySqlDataReader dataReader)
        {
            return new SupplyOrderItem
            {
                Id = (long)dataReader["id"],
                SupplyOrderId = (long)dataReader["supply_order_id"],
                SupplyId = (long)dataReader["supply_id"],
                Quantity = Convert.ToInt32(dataReader["quantity"]),
                Price = (double)dataReader["price"]
            };
        }

        /// <summary>
        /// Gets all order lines for the specified supply order.
        /// </summary>
        /// <param name="supplyOrderId">The ID of the supply order.</param>
        /// <returns>A data table containing the order lines.</returns>
        public static DataTable GetOrderLines(long supplyOrderId)
        {
            return DataConnection.ExecuteDataTable(
                "SELECT `catalog_number`, `description`, `quantity`, `supply_order_items`.`price`, " +
                "`supply_order_item_id`, `supply_order_items`.`supply_id` " +
                "FROM `supply_order_items`, `definitions`, `supplies` " +
                "WHERE `definitions`.`id` = `supplies`.`category_id` " +
                "AND `supplies`.`id` = `supply_order_items`.`supply_id` " +
                "AND `supply_order_items`.`supply_order_id` =" + supplyOrderId + " " +
                "ORDER BY `definitions`.`sort_order`");
        }

        /// <summary>
        /// Gets the supply order item with the specified ID.
        /// </summary>
        /// <param name="supplyOrderItemId">The ID of the supply order item.</param>
        /// <returns>The supply order item with the specified ID.</returns>
        public static SupplyOrderItem GetById(long supplyOrderItemId) =>
            SelectOne("SELECT * FROM supply_order_items WHERE id = " + supplyOrderItemId, FromReader);

        /// <summary>
        /// Inserts the specified supply order item into the daabase.
        /// </summary>
        /// <param name="supplyOrderItem">The supply order item.</param>
        /// <returns></returns>
        public static long Insert(SupplyOrderItem supplyOrderItem) =>
            supplyOrderItem.Id = DataConnection.ExecuteInsert(
                "INSERT INTO supply_order_items (supply_order_id, supply_id, quantity, price) VALUES (?supply_order_id, ?supply_id, ?quantity, ?price)",
                    new MySqlParameter("supply_order_id", supplyOrderItem.SupplyOrderId),
                    new MySqlParameter("supply_id", supplyOrderItem.SupplyId),
                    new MySqlParameter("quantity", supplyOrderItem.Quantity),
                    new MySqlParameter("price", supplyOrderItem.Price));

        /// <summary>
        /// Updates the specified supply order item in the database.
        /// </summary>
        /// <param name="supplyOrderItem">The supply order item.</param>
        public static void Update(SupplyOrderItem supplyOrderItem) =>
            DataConnection.ExecuteInsert(
                "UPDATE `supply_order_items` SET `supply_order_id` = ?supply_order_id, `supply_id` = ?supply_id, `quantity` = ?quantity, " +
                "`price` = ?price WHERE `id` = ?id",
                    new MySqlParameter("supply_order_id", supplyOrderItem.SupplyOrderId),
                    new MySqlParameter("supply_id", supplyOrderItem.SupplyId),
                    new MySqlParameter("quantity", supplyOrderItem.Quantity),
                    new MySqlParameter("price", supplyOrderItem.Price),
                    new MySqlParameter("id", supplyOrderItem.Id));

        /// <summary>
        /// Deletes the specified supply order item from the database.
        /// </summary>
        /// <param name="supplyOrderItem">The supply order item.</param>
        public static void Delete(SupplyOrderItem supplyOrderItem) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `supply_order_items` WHERE `id` = " + supplyOrderItem.Id);
    }
}
