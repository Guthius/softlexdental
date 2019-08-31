/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// In the accounting section, this automates entries into the database when user enters a payment into a patient account.
    /// This table presents the user with a picklist specific to that payment type.
    /// For example, a cash payment would create a picklist of cashboxes for user to put the cash into.
    /// </summary>
    public class AccountAutoPay : DataRecord
    {
        /// <summary>
        /// Foreign key to <see cref="Definition.Id"/>.
        /// </summary>
        public long PayType;

        /// <summary>
        /// A comma delimited list of account ID's.
        /// </summary>
        public string AccountIds;

        /// <summary>
        /// Constructs a new instance of the <see cref="AccountAutoPay"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AccountAutoPay"/> instance.</returns>
        static AccountAutoPay FromReader(MySqlDataReader dataReader)
        {
            return new AccountAutoPay
            {
                Id = Convert.ToInt64(dataReader["id"]),
                PayType = Convert.ToInt64(dataReader["pay_type"]),
                AccountIds = Convert.ToString(dataReader["account_ids"])
            };
        }

        /// <summary>
        /// Gets a list of all auto pays.
        /// </summary>
        /// <returns>A list of auto pays.</returns>
        public static List<AccountAutoPay> All() =>
            SelectMany("SELECT * FROM `accounts_autopay`", FromReader);

        /// <summary>
        /// Gets the auto pay with the specified ID.
        /// </summary>
        /// <param name="accountAutoPayId">The ID of the auto pay.</param>
        /// <returns>The auto pay with the specified ID.</returns>
        public static AccountAutoPay GetById(long accountAutoPayId) =>
            SelectOne("SELECT * FROM `accounts_autopay` WHERE `id` = " + accountAutoPayId, FromReader);

        /// <summary>
        /// Gets the auto pay with the specified pay type.
        /// </summary>
        /// <param name="payType">The pay type of the auto pay.</param>
        /// <returns>The auto pay with the specified pay type.</returns>
        public static AccountAutoPay GetByPayType(long payType) => 
            SelectOne("SELECT * FROM `accounts_autopay` WHERE `pay_type` = " + payType, FromReader);

        /// <summary>
        /// Inserts the specified auto pay into the database.
        /// </summary>
        /// <param name="accountAutoPay">The auto pay.</param>
        /// <returns>The ID assigned to the auto pay.</returns>
        public static long Insert(AccountAutoPay accountAutoPay) =>
            accountAutoPay.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `accounts_autopay` (`pay_type`, `account_ids`) VALUES (?pay_type, ?account_ids)",
                    new MySqlParameter("pay_type", accountAutoPay.PayType),
                    new MySqlParameter("account_ids", accountAutoPay.AccountIds ?? ""));

        /// <summary>
        /// Updates the specified auto pay in the database.
        /// </summary>
        /// <param name="accountAutoPay">The auto pay.</param>
        public static void Update(AccountAutoPay accountAutoPay) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `accounts_autopay` SET `pay_type` = ?pay_type, `account_ids` = ?account_ids WHERE `id` = ?id",
                    new MySqlParameter("pay_type", accountAutoPay.PayType),
                    new MySqlParameter("account_ids", accountAutoPay.AccountIds ?? ""),
                    new MySqlParameter("id", accountAutoPay.Id));

        /// <summary>
        /// Deletes the auto pay with the specified ID from the database.
        /// </summary>
        /// <param name="accountAutoPayId">The ID of the auto pay.</param>
        public static void Delete(int accountAutoPayId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `accounts_autopay` WHERE `id` = " + accountAutoPayId);

        /// <summary>
        /// Gets the total number of auto pays.
        /// </summary>
        /// <returns>The total number of auto pays.</returns>
        public static long GetCount() =>
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM `accounts_autopay`");

        #region CLEANUP

        public static string GetPickListDescription(AccountAutoPay accountAutoPay)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var accountIds = accountAutoPay.AccountIds.Split(',');
            foreach (var accountId in accountIds)
            {
                if (!string.IsNullOrEmpty(accountId) && int.TryParse(accountId, out var result) && result > 0)
                {
                    stringBuilder.AppendLine(Account.GetDescription(result));
                }
            }

            return stringBuilder.ToString().Trim();
        }

        public static IEnumerable<long> GetPickListAccounts(AccountAutoPay accountAutoPay)
        {
            var accountIds = accountAutoPay.AccountIds.Split(',');

            foreach (var accountId in accountIds)
            {
                if (!string.IsNullOrEmpty(accountId) && long.TryParse(accountId, out var result) && result > 0)
                {
                    yield return result;
                }
            }
        }

        public static void SaveList(List<AccountAutoPay> accountAutoPayList)
        {
            DataConnection.ExecuteNonQuery("DELETE FROM accounts_autopay");

            foreach (var accountAutoPay in accountAutoPayList)
            {
                Insert(accountAutoPay);
            }
        }

        #endregion
    }
}