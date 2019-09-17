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
    /// One supply order to one supplier.
    /// </summary>
    public class SupplyOrder : DataRecord
    {
        /// <summary>
        /// The ID of the supplier to order was placed with.
        /// </summary>
        public long SupplierId;

        /// <summary>
        /// The ID of the user that placed the order.
        /// </summary>
        public long? UserId;

        /// <summary>
        /// The date the order was placed. Null when the order is started but has not yet been placed.
        /// </summary>
        public DateTime? DatePlaced;

        public string Note;
        
        /// <summary>
        /// The sum of all the amounts of each item on the order. 
        /// If any of the item prices are zero, then it won't auto calculate this total. 
        /// This will allow the user to manually put in the total without having it get deleted.
        /// </summary>
        public double AmountTotal;

        /// <summary>
        /// The order's shipping charge.
        /// </summary>
        public double ShippingCharge;

        /// <summary>
        /// Constructs a new instance of the <see cref="SupplyOrder"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="SupplyOrder"/> instance.</returns>
        private static SupplyOrder FromReader(MySqlDataReader dataReader)
        {
            return new SupplyOrder
            {
                Id = (long)dataReader["id"],
                SupplierId = (long)dataReader["supplier_id"],
                UserId = dataReader["user_id"] as long?,
                DatePlaced = dataReader["date_placed"] as DateTime?,
                Note = (string)dataReader["note"],
                AmountTotal = (double)dataReader["amount_total"],
                ShippingCharge = (double)dataReader["shipping_charge"]
            };
        }

        /// <summary>
        /// Gets a list of all orders from the database.
        /// </summary>
        /// <returns>A list of supply orders.</returns>
        public static List<SupplyOrder> All() =>
            SelectMany("SELECT * FROM `supply_orders` ORDER BY `date_placed`", FromReader);

        /// <summary>
        /// Gets a list of all orders placed with the specified supplier from the database.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>A list of supply orders.</returns>
        public static List<SupplyOrder> GetBySupplier(long supplierId) =>
            SelectMany(
                "SELECT * FROM `supply_orders` WHERE `supplier_id` = " + supplierId + " ORDER BY `date_placed`", FromReader);

        /// <summary>
        /// Inserts the specified supply order into the database.
        /// </summary>
        /// <param name="supplyOrder">The supply order.</param>
        /// <returns>The ID assigned to the supply order.</returns>
        public static long Insert(SupplyOrder supplyOrder) =>
            supplyOrder.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `supply_orders` (`supplier_id`, `user_id`, `date_placed`, `note`, `amount_total`, `shipping_charge`) " +
                "VALUES (?supplier_id, ?user_id, ?date_placed, ?note, ?amount_total, ?shipping_charge)",
                    new MySqlParameter("supplier_id", supplyOrder.SupplierId),
                    new MySqlParameter("user_id", supplyOrder.UserId.HasValue ? (object)supplyOrder.UserId : DBNull.Value),
                    new MySqlParameter("date_placed", supplyOrder.DatePlaced.HasValue ? (object)supplyOrder.DatePlaced : DBNull.Value),
                    new MySqlParameter("note", supplyOrder.Note ?? ""),
                    new MySqlParameter("amount_total", supplyOrder.AmountTotal),
                    new MySqlParameter("shipping_charge", supplyOrder.ShippingCharge));

        /// <summary>
        /// Updates the specified supply order in the database.
        /// </summary>
        /// <param name="supplyOrder">The supply order.</param>
        public static void Update(SupplyOrder supplyOrder) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `supply_orders` SET `supplier_id` = ?supplier_id, `user_id` = ?user_id, `date_placed` = ?date_placed, `note` = ?note, " +
                "`amount_total` = ?amount_total, `shipping_charge` = ?shipping_charge WHERE `id` = ?id",
                    new MySqlParameter("supplier_id", supplyOrder.SupplierId),
                    new MySqlParameter("user_id", supplyOrder.UserId.HasValue ? (object)supplyOrder.UserId : DBNull.Value),
                    new MySqlParameter("date_placed", supplyOrder.DatePlaced.HasValue ? (object)supplyOrder.DatePlaced : DBNull.Value),
                    new MySqlParameter("note", supplyOrder.Note ?? ""),
                    new MySqlParameter("amount_total", supplyOrder.AmountTotal),
                    new MySqlParameter("shipping_charge", supplyOrder.ShippingCharge),
                    new MySqlParameter("id", supplyOrder.Id));

        /// <summary>
        /// Deletes the specified supply order from the database.
        /// </summary>
        /// <param name="supplyOrder">The supply order.</param>
        public static void Delete(SupplyOrder supplyOrder) => 
            DataConnection.ExecuteNonQuery("DELETE FROM supply_orders WHERE id = " + supplyOrder.Id);
        
        /// <summary>
        /// Recalculates the total of the specified supply order.
        /// </summary>
        /// <param name="supplyOrderId">The ID of the supply order.</param>
        /// <returns>The new total.</returns>
        public static double UpdateOrderPrice(long supplyOrderId)
        {
            var amountTotal = 
                DataConnection.ExecuteDouble(
                    "SELECT SUM(`quantity` * `price`) FROM `supply_order_items` WHERE `supply_order_id` = " + supplyOrderId);

            DataConnection.ExecuteNonQuery(
                "UPDATE `supply_orders` SET `amount_total` = ?total WHERE id = " + supplyOrderId, 
                    new MySqlParameter("total", amountTotal));

            return amountTotal;
        }
    }
}
