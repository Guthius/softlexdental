using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness
{
    public partial class Account : DataRecord
    {
        /// <summary>
        /// The account description.
        /// </summary>
        public string Description;

        /// <summary>
        /// The account type.
        /// </summary>
        public AccountType Type;

        /// <summary>
        /// The bank account number.
        /// </summary>
        public string BankNumber;

        /// <summary>
        /// A value indicating whether the account is active.
        /// </summary>
        public bool Inactive;

        /// <summary>
        /// The (optional) color of the account.
        /// </summary>
        public Color Color;

        /// <summary>
        /// Constructs a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Account"/> instance.</returns>
        static Account FromReader(MySqlDataReader dataReader)
        {
            var account = new Account
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                Type = (AccountType)Convert.ToInt32(dataReader["type"]),
                BankNumber = Convert.ToString(dataReader["bank_number"]),
                Inactive = Convert.ToBoolean(dataReader["inactive"])
            };

            var color = dataReader["color"];
            if (color != DBNull.Value)
            {
                account.Color = 
                    ColorTranslator.FromHtml(
                        Convert.ToString(color));
            }

            return account;
        }

        /// <summary>
        /// Gets all the accounts.
        /// </summary>
        /// <returns>A list of accounts.</returns>
        public static List<Account> All() =>
            SelectMany("SELECT * FROM accounts", FromReader);

        /// <summary>
        /// Gets the account with the specified ID.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The account with the specified ID.</returns>
        public static Account GetById(long accountId) =>
            SelectOne("SELECT * FROM accounts WHERE id = " + accountId, FromReader);

        /// <summary>
        /// Gets the description of the account with the specified ID.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The description of the account with the specified ID.</returns>
        public static string GetDescription(long accountId) =>
            GetById(accountId)?.Description ?? "";

        /// <summary>
        /// Inserts the specified account into the database.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>The ID assigned to the account.</returns>
        public static long Insert(Account account) =>
            account.Id = DataConnection.ExecuteInsert(
                "INSERT INTO accounts (description, type, bank_number, inactive, color) VALUES (@description, @type, @bank_number, @inactive, @color)",
                    new MySqlParameter("description", account.Description ?? ""),
                    new MySqlParameter("type", (int)account.Type),
                    new MySqlParameter("bank_number", account.BankNumber ?? ""),
                    new MySqlParameter("inactive", account.Inactive),
                    new MySqlParameter("color", ColorTranslator.ToHtml(account.Color)));
        
        /// <summary>
        /// Updates the specified account in the database.
        /// </summary>
        /// <param name="account">The account.</param>
        public static void Update(Account account) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE accounts SET description = @description, type = @type, bank_number = @bank_number, inactive = @inactive, color = @color WHERE id = @id",
                    new MySqlParameter("description", account.Description ?? ""),
                    new MySqlParameter("type", (int)account.Type),
                    new MySqlParameter("bank_number", account.BankNumber ?? ""),
                    new MySqlParameter("inactive", account.Inactive),
                    new MySqlParameter("color", ColorTranslator.ToHtml(account.Color)),
                    new MySqlParameter("id", account.Id));

        /// <summary>
        /// Deletes the specified account from the database.
        /// </summary>
        /// <param name="account">The account.</param>
        public static void Delete(Account account)
        {
            // TODO: Fix me

            // Check if there are any journal entries assigned to the account.
            var count = DataConnection.ExecuteLong("SELECT COUNT(*) FROM journalentry WHERE AccountNum =" + account.Id);
            if (count > 0)
            {
                throw new DataException("Not allowed to delete an account with existing journal entries.");
            }

            // Check if there are any preferences referencing this account.
            var accountIds = Preference.GetString(PreferenceName.AccountingDepositAccounts).Split(',');
            foreach (var accountId in accountIds)
            {
                if (long.TryParse(accountId, out var result) && result == account.Id)
                {
                    throw new DataException("Account is in use in the setup section.");
                }
            }

            if (Preference.GetLong(PreferenceName.AccountingIncomeAccount) == account.Id)
                throw new DataException("Account is in use in the setup section.");

            if (Preference.GetLong(PreferenceName.AccountingCashIncomeAccount) == account.Id)
                throw new DataException("Account is in use in the setup section.");

            // Check if there are any auto pay entries that are referencing this account.
            var accountAutoPayList = AccountAutoPay.All();
            foreach (var accountAutoPay in accountAutoPayList)
            {
                accountIds = accountAutoPay.AccountIds.Split(',');
                foreach (var accountId in accountIds)
                {
                    if (long.TryParse(accountId, out var result) && result == account.Id)
                    {
                        throw new DataException("Account is in use in the setup section.");
                    }
                }
            }

            DataConnection.ExecuteNonQuery("DELETE FROM accounts WHERE id = " + account.Id);
        }

        #region CLEANUP

        /// <summary>
        /// Updates the splits on the journal entries whose indexes are passed in.
        /// </summary>
        /// <param name="listJournalEntries">All journal entries for a particular account.</param>
        /// <param name="listIndexesForTrans">The index of the journal entries in listJournalEntries. These are the ones that will be updated.</param>
        /// <param name="dictJournalEntryDescriptions">A dictionary where the key is the JournalEntryNum and the value is the journal entry's account description.</param>
        /// <param name="acct">The account that whose description is being updates.</param>
        static void UpdateJournalEntrySplits(List<JournalEntry> listJournalEntries, List<int> listIndexesForTrans, Dictionary<long, string> dictJournalEntryDescriptions, Account acct)
        {
            foreach (int index in listIndexesForTrans.Where(x => listJournalEntries[x].AccountNum != acct.Id))
            {
                JournalEntry journalEntry = listJournalEntries[index];
                if (listIndexesForTrans.Count <= 2)
                {
                    // When a transaction only has two splits, the Splits column will simply be the name of the account of the other split.
                    journalEntry.Splits = acct.Description;
                }
                else
                {
                    // When a transaction has three or more splits, the Splits column will be the names of the account and the amount of the other splits.
                    // Ex.: 
                    // Patient Fee Income 85.00
                    // Supplies 110.00
                    journalEntry.Splits = string.Join("\r\n", listIndexesForTrans
                        .Where(x => listJournalEntries[x].JournalEntryNum != journalEntry.JournalEntryNum)
                        .Select(x => dictJournalEntryDescriptions[listJournalEntries[x].JournalEntryNum] + " " +
                        (listJournalEntries[x].DebitAmt > 0 ?
                            listJournalEntries[x].DebitAmt.ToString("n") :
                            listJournalEntries[x].CreditAmt.ToString("n"))));
                }
                JournalEntries.Update(journalEntry);
            }
        }

        /// <summary>
        /// Used to test the sign on debits and credits for the five different account types
        /// </summary>
        public static bool DebitIsPos(AccountType type)
        {
            // No need to check RemotingRole; no call to db.
            switch (type)
            {
                case AccountType.Asset:
                case AccountType.Expense:
                    return true;
                case AccountType.Liability:
                case AccountType.Equity: //Because liabilities and equity are treated the same
                case AccountType.Income:
                    return false;
            }
            return true; // Will never happen
        }

        /// <summary>
        /// Gets the balance of an account directly from the database.
        /// </summary>
        public static double GetBalance(long accountNum, AccountType acctType)
        {
            DataTable table = Db.GetTable(
                "SELECT SUM(DebitAmt),SUM(CreditAmt) " +
                "FROM journalentry " +
                "WHERE AccountNum=" + POut.Long(accountNum) + " " +
                "GROUP BY AccountNum");

            double debit = 0;
            double credit = 0;
            if (table.Rows.Count > 0)
            {
                debit = PIn.Double(table.Rows[0][0].ToString());
                credit = PIn.Double(table.Rows[0][1].ToString());
            }
            if (DebitIsPos(acctType))
            {
                return debit - credit;
            }
            else
            {
                return credit - debit;
            }
        }

        /// <summary>
        /// Checks the loaded prefs to see if user has setup deposit linking.
        /// Returns true if so.
        /// </summary>
        public static bool DepositsLinked()
        {
            // No need to check RemotingRole; no call to db.
            if (Preference.GetInt(PreferenceName.AccountingSoftware) == (int)AccountingSoftware.QuickBooks)
            {
                if (Preference.GetString(PreferenceName.QuickBooksDepositAccounts) == "")
                {
                    return false;
                }
                if (Preference.GetString(PreferenceName.QuickBooksIncomeAccount) == "")
                {
                    return false;
                }
            }
            else
            {
                if (Preference.GetString(PreferenceName.AccountingDepositAccounts) == "")
                {
                    return false;
                }
                if (Preference.GetLong(PreferenceName.AccountingIncomeAccount) == 0)
                {
                    return false;
                }
            }

            // Might add a few more checks later.
            return true;
        }

        /// <summary>
        /// Checks the loaded prefs and accountingAutoPays to see if user has setup auto pay linking.
        /// Returns true if so.
        /// </summary>
        public static bool PaymentsLinked()
        {
            if (AccountAutoPay.GetCount() == 0)
            {
                return false;
            }
            if (Preference.GetLong(PreferenceName.AccountingCashIncomeAccount) == 0)
            {
                return false;
            }
            return true;
        }

        public static long[] GetDepositAccounts()
        {
            string depStr = Preference.GetString(PreferenceName.AccountingDepositAccounts);
            string[] depStrArray = depStr.Split(new char[] { ',' });
            List<long> depAL = new List<long>();
            for (int i = 0; i < depStrArray.Length; i++)
            {
                if (depStrArray[i] == "")
                {
                    continue;
                }
                depAL.Add(PIn.Long(depStrArray[i]));
            }
            return depAL.ToArray();
        }

        public static List<string> GetDepositAccountsQB()
        {
            string depStr = Preference.GetString(PreferenceName.QuickBooksDepositAccounts);
            string[] depStrArray = depStr.Split(new char[] { ',' });
            List<string> retVal = new List<string>();
            for (int i = 0; i < depStrArray.Length; i++)
            {
                if (depStrArray[i] == "")
                {
                    continue;
                }
                retVal.Add(depStrArray[i]);
            }
            return retVal;
        }

        public static List<string> GetIncomeAccountsQB()
        {
            string incomeStr = Preference.GetString(PreferenceName.QuickBooksIncomeAccount);
            string[] incomeStrArray = incomeStr.Split(new char[] { ',' });
            List<string> retVal = new List<string>();
            for (int i = 0; i < incomeStrArray.Length; i++)
            {
                if (incomeStrArray[i] == "")
                {
                    continue;
                }
                retVal.Add(incomeStrArray[i]);
            }
            return retVal;
        }

        /// <summary>
        /// Gets the full list to display in the Chart of Accounts, including balances.
        /// </summary>
        public static DataTable GetFullList(DateTime asOfDate, bool showInactive)
        {
            DataTable table = new DataTable("Accounts");
            DataRow row;
            //columns that start with lowercase are altered for display rather than being raw data.
            table.Columns.Add("type");
            table.Columns.Add("Description");
            table.Columns.Add("balance");
            table.Columns.Add("BankNumber");
            table.Columns.Add("inactive");
            table.Columns.Add("color");
            table.Columns.Add("AccountNum");
            //but we won't actually fill this table with rows until the very end.  It's more useful to use a List<> for now.
            List<DataRow> rows = new List<DataRow>();
            //first, the entire history for the asset, liability, and equity accounts (except Retained Earnings)-----------
            string command = "SELECT account.AcctType, account.Description, account.AccountNum, "
                + "SUM(DebitAmt) AS SumDebit, SUM(CreditAmt) AS SumCredit, account.BankNumber, account.Inactive, account.AccountColor "
                + "FROM account "
                + "LEFT JOIN journalentry ON journalentry.AccountNum=account.AccountNum AND "
                + "DateDisplayed <= " + POut.Date(asOfDate) + " WHERE AcctType<=2 ";
            if (!showInactive)
            {
                command += "AND Inactive=0 ";
            }
            command += "GROUP BY account.AccountNum, account.AcctType, account.Description, account.BankNumber,"
                + "account.Inactive, account.AccountColor ORDER BY AcctType, Description";
            DataTable rawTable = Db.GetTable(command);
            AccountType aType;
            decimal debit = 0;
            decimal credit = 0;
            for (int i = 0; i < rawTable.Rows.Count; i++)
            {
                row = table.NewRow();
                aType = (AccountType)PIn.Long(rawTable.Rows[i]["AcctType"].ToString());
                row["type"] = Lans.g("enumAccountType", aType.ToString());
                row["Description"] = rawTable.Rows[i]["Description"].ToString();
                debit = PIn.Decimal(rawTable.Rows[i]["SumDebit"].ToString());
                credit = PIn.Decimal(rawTable.Rows[i]["SumCredit"].ToString());
                if (DebitIsPos(aType))
                {
                    row["balance"] = (debit - credit).ToString("N");
                }
                else
                {
                    row["balance"] = (credit - debit).ToString("N");
                }
                row["BankNumber"] = rawTable.Rows[i]["BankNumber"].ToString();
                if (rawTable.Rows[i]["Inactive"].ToString() == "0")
                {
                    row["inactive"] = "";
                }
                else
                {
                    row["inactive"] = "X";
                }
                row["color"] = rawTable.Rows[i]["AccountColor"].ToString();//it will be an unsigned int at this point.
                row["AccountNum"] = rawTable.Rows[i]["AccountNum"].ToString();
                rows.Add(row);
            }
            //now, the Retained Earnings (auto) account-----------------------------------------------------------------
            DateTime firstofYear = new DateTime(asOfDate.Year, 1, 1);
            command = "SELECT AcctType, SUM(DebitAmt) AS SumDebit, SUM(CreditAmt) AS SumCredit "
                + "FROM account,journalentry "
                + "WHERE journalentry.AccountNum=account.AccountNum "
                + "AND DateDisplayed < " + POut.Date(firstofYear)//all from previous years
                + " AND (AcctType=3 OR AcctType=4) "//income or expenses
                + "GROUP BY AcctType ORDER BY AcctType";//income first, but could return zero rows.
            rawTable = Db.GetTable(command);
            decimal balance = 0;
            for (int i = 0; i < rawTable.Rows.Count; i++)
            {
                aType = (AccountType)PIn.Long(rawTable.Rows[i]["AcctType"].ToString());
                debit = PIn.Decimal(rawTable.Rows[i]["SumDebit"].ToString());
                credit = PIn.Decimal(rawTable.Rows[i]["SumCredit"].ToString());
                //this works for both income and expenses, because we are subracting expenses, so signs cancel
                balance += credit - debit;
            }
            row = table.NewRow();
            row["type"] = Lans.g("enumAccountType", AccountType.Equity.ToString());
            row["Description"] = Lans.g("Accounts", "Retained Earnings (auto)");
            row["balance"] = balance.ToString("N");
            row["BankNumber"] = "";
            row["color"] = System.Drawing.Color.White.ToArgb();
            row["AccountNum"] = "0";
            rows.Add(row);
            //finally, income and expenses------------------------------------------------------------------------------
            command = "SELECT account.AcctType, account.Description, account.AccountNum, "
                + "SUM(DebitAmt) AS SumDebit, SUM(CreditAmt) AS SumCredit, account.BankNumber, account.Inactive, account.AccountColor "
                + "FROM account "
                + "LEFT JOIN journalentry ON journalentry.AccountNum=account.AccountNum "
                + "AND DateDisplayed <= " + POut.Date(asOfDate)
                + " AND DateDisplayed >= " + POut.Date(firstofYear)//only for this year
                + " WHERE (AcctType=3 OR AcctType=4) ";
            if (!showInactive)
            {
                command += "AND Inactive=0 ";
            }
            command += "GROUP BY account.AccountNum, account.AcctType, account.Description, account.BankNumber,"
                + "account.Inactive, account.AccountColor ORDER BY AcctType, Description";
            rawTable = Db.GetTable(command);
            for (int i = 0; i < rawTable.Rows.Count; i++)
            {
                row = table.NewRow();
                aType = (AccountType)PIn.Long(rawTable.Rows[i]["AcctType"].ToString());
                row["type"] = Lans.g("enumAccountType", aType.ToString());
                row["Description"] = rawTable.Rows[i]["Description"].ToString();
                debit = PIn.Decimal(rawTable.Rows[i]["SumDebit"].ToString());
                credit = PIn.Decimal(rawTable.Rows[i]["SumCredit"].ToString());
                if (DebitIsPos(aType))
                {
                    row["balance"] = (debit - credit).ToString("N");
                }
                else
                {
                    row["balance"] = (credit - debit).ToString("N");
                }
                row["BankNumber"] = rawTable.Rows[i]["BankNumber"].ToString();
                if (rawTable.Rows[i]["Inactive"].ToString() == "0")
                {
                    row["inactive"] = "";
                }
                else
                {
                    row["inactive"] = "X";
                }
                row["color"] = rawTable.Rows[i]["AccountColor"].ToString();//it will be an unsigned int at this point.
                row["AccountNum"] = rawTable.Rows[i]["AccountNum"].ToString();
                rows.Add(row);
            }
            for (int i = 0; i < rows.Count; i++)
            {
                table.Rows.Add(rows[i]);
            }
            return table;
        }

        /// <summary>
        /// Gets the full GeneralLedger list.
        /// </summary>
        public static DataTable GetGeneralLedger(DateTime dateStart, DateTime dateEnd)
        {
            string queryString = @"SELECT DATE(" + POut.Date(new DateTime(dateStart.Year - 1, 12, 31)) + @") DateDisplayed,
				'' Memo,
				'' Splits,
				'' CheckNumber,
				startingbals.SumTotal DebitAmt,
				0 CreditAmt,
				'' Balance,
				startingbals.Description,
				startingbals.AcctType,
				startingbals.AccountNum
				FROM (
					SELECT account.AccountNum,
					account.Description,
					account.AcctType,
					ROUND(SUM(journalentry.DebitAmt-journalentry.CreditAmt),2) SumTotal
					FROM account
					INNER JOIN journalentry ON journalentry.AccountNum=account.AccountNum
					AND journalentry.DateDisplayed < " + POut.Date(dateStart) + @" 
					AND account.AcctType IN (0,1,2)/*assets,liablities,equity*/
					GROUP BY account.AccountNum
				) startingbals

				UNION ALL
	
				SELECT journalentry.DateDisplayed,
				journalentry.Memo,
				journalentry.Splits,
				journalentry.CheckNumber,
				journalentry.DebitAmt, 
				journalentry.CreditAmt,
				'' Balance,
				account.Description,
				account.AcctType,
				account.AccountNum 
				FROM account
				LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum 
					AND journalentry.DateDisplayed >= " + POut.Date(dateStart) + @" 
					AND journalentry.DateDisplayed <= " + POut.Date(dateEnd) + @" 
				WHERE account.AcctType IN(0,1,2)
				
				UNION ALL 
				
				SELECT journalentry.DateDisplayed, 
				journalentry.Memo, 
				journalentry.Splits, 
				journalentry.CheckNumber,
				journalentry.DebitAmt, 
				journalentry.CreditAmt, 
				'' Balance,
				account.Description, 
				account.AcctType,
				account.AccountNum 
				FROM account 
				LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum 
					AND journalentry.DateDisplayed >= " + POut.Date(dateStart) + @"  
					AND journalentry.DateDisplayed <= " + POut.Date(dateEnd) + @"  
				WHERE account.AcctType IN(3,4)
				
				ORDER BY AcctType, Description, DateDisplayed;";
            return Db.GetTable(queryString);
        }

        /// <summary>
        /// Gets the full list to display in the Chart of Accounts, including balances.
        /// </summary>
        public static DataTable GetAssetTable(DateTime asOfDate) => GetAccountTotalByType(asOfDate, AccountType.Asset);

        /// <summary>
        /// Gets the full list to display in the Chart of Accounts, including balances.
        /// </summary>
        public static DataTable GetLiabilityTable(DateTime asOfDate) => GetAccountTotalByType(asOfDate, AccountType.Liability);

        /// <summary>
        /// Gets the full list to display in the Chart of Accounts, including balances.
        /// </summary>
        public static DataTable GetEquityTable(DateTime asOfDate) => GetAccountTotalByType(asOfDate, AccountType.Equity);

        public static DataTable GetAccountTotalByType(DateTime asOfDate, AccountType acctType)
        {
            string sumTotalStr = "";
            if (acctType == AccountType.Asset)
            {
                sumTotalStr = "SUM(ROUND(DebitAmt,3)-ROUND(CreditAmt,3))";
            }
            else
            {
                sumTotalStr = "SUM(ROUND(CreditAmt,3)-ROUND(DebitAmt,3))";
            }

            return Db.GetTable(
                "SELECT Description, " + sumTotalStr + " SumTotal, AcctType " +
                "FROM account, journalentry " +
                "WHERE account.AccountNum=journalentry.AccountNum AND DateDisplayed <= " + POut.Date(asOfDate) + " " +
                "AND AcctType=" + POut.Int((int)acctType) + " " +
                "GROUP BY account.AccountNum " +
                "ORDER BY Description, DateDisplayed");
        }

        /// <Summary>
        /// Gets sum of all income-expenses for all previous years. asOfDate could be any date
        /// </Summary>
        public static double RetainedEarningsAuto(DateTime asOfDate)
        {
            DateTime firstOfYear = new DateTime(asOfDate.Year, 1, 1);

            DataTable table = Db.GetTable(
                "SELECT SUM(ROUND(CreditAmt,3)), SUM(ROUND(DebitAmt,3)), AcctType " +
                "FROM journalentry,account " +
                "WHERE journalentry.AccountNum=account.AccountNum " +
                "AND DateDisplayed < " + POut.Date(firstOfYear) + " " +
                "GROUP BY AcctType");

            double retVal = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][2].ToString() == "3"//income
                    || table.Rows[i][2].ToString() == "4")//expense
                {
                    retVal += PIn.Double(table.Rows[i][0].ToString());//add credit
                    retVal -= PIn.Double(table.Rows[i][1].ToString());//subtract debit
                                                                      //if it's an expense, we are subtracting (income-expense), but the signs cancel.
                }
            }
            return retVal;
        }

        /// <Summary>
        /// asOfDate is typically 12/31/...  
        /// </Summary>
        public static double NetIncomeThisYear(DateTime asOfDate)
        {
            DateTime firstOfYear = new DateTime(asOfDate.Year, 1, 1);

            DataTable table = Db.GetTable(
                "SELECT SUM(ROUND(CreditAmt,3)), SUM(ROUND(DebitAmt,3)), AcctType " +
                "FROM journalentry,account " +
                "WHERE journalentry.AccountNum=account.AccountNum " +
                "AND DateDisplayed >= " + POut.Date(firstOfYear) + " " +
                "AND DateDisplayed <= " + POut.Date(asOfDate) + " " +
                "GROUP BY AcctType");

            double retVal = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][2].ToString() == "3"//income
                    || table.Rows[i][2].ToString() == "4")//expense
                {
                    retVal += PIn.Double(table.Rows[i][0].ToString());//add credit
                    retVal -= PIn.Double(table.Rows[i][1].ToString());//subtract debit
                                                                      //if it's an expense, we are subtracting (income-expense), but the signs cancel.
                }
            }
            return retVal;
        }

        #endregion
    }
}