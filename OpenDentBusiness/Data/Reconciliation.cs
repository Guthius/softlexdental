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
    /// <summary>
    /// In accounting, reconciliation is the process of ensuring that two sets of records (usually
    /// the balances of two accounts) are in agreement. Reconciliation is used to ensure that the
    /// money leaving an account matches the actual money spent. This is done by making sure the
    /// balances match at the end of a particular accounting period.
    /// 
    /// Used in the Accounting section. Transactions will be attached to it.
    /// </summary>
    public class Reconciliation : DataRecord
    {
        /// <summary>
        /// Foreign key to <see cref="Account.Id"/>.
        /// </summary>
        public long AccountId;
        
        /// <summary>
        /// User enters starting balance here.
        /// </summary>
        public double StartBalance;
        
        ///<summary>
        /// User enters ending balance here.
        ///</summary>
        public double EndBalance;
        
        /// <summary>
        /// The date on which the recnciliation was performed.
        /// </summary>
        public DateTime? ReconciliationDate;

        /// <summary>
        /// If StartBalance + sum of entries selected = EndBalance, the user can lock. Unlock 
        /// requires special permission, which nobody will have by default.
        /// </summary>
        public bool Locked;

        /// <summary>
        /// Constructs a new instance of the <see cref="Reconciliation"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Reconciliation"/> instance.</returns>
        static Reconciliation FromReader(MySqlDataReader dataReader)
        {
            var reconciliation = new Reconciliation
            {
                Id = Convert.ToInt64(dataReader["id"]),
                AccountId = Convert.ToInt64(dataReader["account_id"]),
                StartBalance = Convert.ToDouble(dataReader["start_balance"]),
                EndBalance = Convert.ToDouble(dataReader["end_balance"]),
                Locked = Convert.ToBoolean(dataReader["locked"])
            };

            var reconciliationDate = dataReader["reconciliation_date"];
            if (reconciliationDate != DBNull.Value)
            {
                reconciliation.ReconciliationDate = Convert.ToDateTime(reconciliationDate);
            }

            return reconciliation;
        }

        /// <summary>
        /// Gets the reconciliation with the specified ID.
        /// </summary>
        /// <param name="reconciliationId">The ID of the reconciliation.</param>
        /// <returns>The reconciliation with the specified ID.</returns>
        public static Reconciliation GetById(long reconciliationId) => 
            SelectOne("SELECT * FROM reconciliations WHERE id = " + reconciliationId, FromReader);

        /// <summary>
        /// Gets a list of all reconciliations associated with the account with the specified ID.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>A list of reconciliations.</returns>
        public static List<Reconciliation> GetByAccountId(long accountId) =>
            SelectMany("SELECT * FROM `reconciliations` WHERE `account_id` = ?account_id ORDER BY `reconciliation_date`", FromReader,
                new MySqlParameter("account_id", accountId));

        /// <summary>
        /// Inserts the specified reconciliation into the database.
        /// </summary>
        /// <param name="reconciliation">The reconciliation.</param>
        /// <returns>The ID assigned to the reconciliation.</returns>
        public static long Insert(Reconciliation reconciliation) =>
            reconciliation.Id = DataConnection.ExecuteInsert(
                "INSERT INTO reconciliations (`account_id`, `start_balance`, `end_balance`, `reconciliation_date`, `locked`) VALUES (?account_id, ?start_balance, ?end_balance, ?reconciliation_date, ?locked)",
                    new MySqlParameter("account_id", reconciliation.AccountId),
                    new MySqlParameter("start_balance", reconciliation.StartBalance),
                    new MySqlParameter("end_balance", reconciliation.EndBalance),
                    new MySqlParameter("reconciliation_date", reconciliation.ReconciliationDate.HasValue ? (object)reconciliation.ReconciliationDate.Value : DBNull.Value),
                    new MySqlParameter("locked", reconciliation.Locked));

        /// <summary>
        /// Updates the specified reconciliation in the database.
        /// </summary>
        /// <param name="reconciliation">The reconciliation.</param>
        public static void Update(Reconciliation reconciliation) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `reconciliations` SET `account_id` = ?account_id, `start_balance` = ?start_balance, `end_balance` = ?end_balance, `reconciliation_date` = ?reconciliation_date, `locked` = ?locked WHERE `id` = ?id",
                    new MySqlParameter("account_id", reconciliation.AccountId),
                    new MySqlParameter("start_balance", reconciliation.StartBalance),
                    new MySqlParameter("end_balance", reconciliation.EndBalance),
                    new MySqlParameter("reconciliation_date", reconciliation.ReconciliationDate.HasValue ? (object)reconciliation.ReconciliationDate.Value : DBNull.Value),
                    new MySqlParameter("locked", reconciliation.Locked),
                    new MySqlParameter("id", reconciliation.Id));

        /// <summary>
        /// Deletes the specified reconciliation from the database.
        /// </summary>
        /// <param name="reconciliation">The reconciliation.</param>
        /// <exception cref="DataException">If there are journal entries attached to the reconciliation.</exception>
        public static void Delete(Reconciliation reconciliation)
        {
            // Check to see if any journal entries are attached to the reconciliation.
            var count = 
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `journal_entries` WHERE `reconciliation_id` = " + reconciliation.Id);

            if (count > 0) throw new DataException("Not allowed to delete a reconciliation with existing journal entries.");

            // Delete the reconciliation.
            DataConnection.ExecuteNonQuery("DELETE FROM `reconciliations` WHERE `id` = " + reconciliation.Id);

            reconciliation.Id = 0;
        }
    }
}