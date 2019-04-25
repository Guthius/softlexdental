using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness
{
    public partial class Account
    {
        /// <summary>
        /// Gets one Account object from the database using the primary key. Returns null if not found.
        /// </summary>
        public static Account SelectOne(long accountNum)
        {
            var list = TableToList(Db.GetTable("SELECT * FROM account WHERE AccountNum = " + accountNum));
            if (list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// Gets one Account object from the database using a query.
        /// </summary>
        public static Account SelectOne(string command)
        {
            var list = TableToList(Db.GetTable(command));
            if (list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// Gets a list of Account objects from the database using a query.
        /// </summary>
        public static List<Account> SelectMany(string command) => TableToList(Db.GetTable(command));

        /// <summary>
        /// Converts a DataTable to a list of objects.
        /// </summary>
        public static List<Account> TableToList(DataTable table)
        {
            var accounts = new List<Account>();

            foreach (DataRow row in table.Rows)
            {
                var account = new Account
                {
                    AccountNum      = Convert.ToInt64(row["AccountNum"]),
                    Description     = Convert.ToString(row["Description"]),
                    AcctType        = (AccountType)Convert.ToInt32(row["AcctType"]),
                    BankNumber      = Convert.ToString(row["BankNumber"]),
                    Inactive        = Convert.ToBoolean(row["Inactive"]),
                    AccountColor    = Color.FromArgb(Convert.ToInt32(row["AccountColor"]))
                };
                accounts.Add(account);
            }

            return accounts;
        }

        /// <summary>
        /// Converts a list of Account into a DataTable.
        /// </summary>
        public static DataTable ListToTable(List<Account> listAccounts, string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = "Account";
            }
            DataTable table = new DataTable(tableName);
            table.Columns.Add("AccountNum");
            table.Columns.Add("Description");
            table.Columns.Add("AcctType");
            table.Columns.Add("BankNumber");
            table.Columns.Add("Inactive");
            table.Columns.Add("AccountColor");
            foreach (Account account in listAccounts)
            {
                table.Rows.Add(new object[] {
                    POut.Long  (account.AccountNum),
                                account.Description,
                    POut.Int   ((int)account.AcctType),
                                account.BankNumber,
                    POut.Bool  (account.Inactive),
                    POut.Int   (account.AccountColor.ToArgb()),
                });
            }
            return table;
        }

        /// <summary>
        /// Inserts one Account into the database.  Returns the new priKey.
        /// </summary>
        public static long Insert(Account account) => Insert(account, false);
        
        /// <summary>
        /// Inserts one Account into the database.  Provides option to use the existing priKey.
        /// </summary>
        public static long Insert(Account account, bool useExistingPK)
        {
            if (!useExistingPK && PrefC.RandomKeys)
            {
                account.AccountNum = ReplicationServers.GetKey("account", "AccountNum");
            }
            string command = "INSERT INTO account (";
            if (useExistingPK || PrefC.RandomKeys)
            {
                command += "AccountNum,";
            }
            command += "Description,AcctType,BankNumber,Inactive,AccountColor) VALUES(";
            if (useExistingPK || PrefC.RandomKeys)
            {
                command += POut.Long(account.AccountNum) + ",";
            }
            command +=
                 "'" + POut.String(account.Description) + "',"
                + POut.Int((int)account.AcctType) + ","
                + "'" + POut.String(account.BankNumber) + "',"
                + POut.Bool(account.Inactive) + ","
                + POut.Int(account.AccountColor.ToArgb()) + ")";
            if (useExistingPK || PrefC.RandomKeys)
            {
                Db.NonQ(command);
            }
            else
            {
                account.AccountNum = Db.NonQ(command, true, "AccountNum", "account");
            }
            return account.AccountNum;
        }

        /// <summary>
        /// Updates one Account in the database.
        /// </summary>
        public static void Update(Account account)
        {
            string command = "UPDATE account SET "
                + "Description = '" + POut.String(account.Description) + "', "
                + "AcctType    =  " + POut.Int((int)account.AcctType) + ", "
                + "BankNumber  = '" + POut.String(account.BankNumber) + "', "
                + "Inactive    =  " + POut.Bool(account.Inactive) + ", "
                + "AccountColor=  " + POut.Int(account.AccountColor.ToArgb()) + " "
                + "WHERE AccountNum = " + POut.Long(account.AccountNum);
            Db.NonQ(command);
        }

        ///<summary>Updates one Account in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
        public static bool Update(Account account, Account oldAccount)
        {
            string command = "";
            if (account.Description != oldAccount.Description)
            {
                if (command != "") { command += ","; }
                command += "Description = '" + POut.String(account.Description) + "'";
            }
            if (account.AcctType != oldAccount.AcctType)
            {
                if (command != "") { command += ","; }
                command += "AcctType = " + POut.Int((int)account.AcctType) + "";
            }
            if (account.BankNumber != oldAccount.BankNumber)
            {
                if (command != "") { command += ","; }
                command += "BankNumber = '" + POut.String(account.BankNumber) + "'";
            }
            if (account.Inactive != oldAccount.Inactive)
            {
                if (command != "") { command += ","; }
                command += "Inactive = " + POut.Bool(account.Inactive) + "";
            }
            if (account.AccountColor != oldAccount.AccountColor)
            {
                if (command != "") { command += ","; }
                command += "AccountColor = " + POut.Int(account.AccountColor.ToArgb()) + "";
            }
            if (command == "")
            {
                return false;
            }

            Db.NonQ("UPDATE account SET " + command + " WHERE AccountNum = " + POut.Long(account.AccountNum));
            return true;
        }

        ///<summary>Returns true if Update(Account,Account) would make changes to the database.
        ///Does not make any changes to the database and can be called before remoting role is checked.</summary>
        public static bool UpdateComparison(Account account, Account oldAccount)
        {
            if (account.Description != oldAccount.Description)
            {
                return true;
            }
            if (account.AcctType != oldAccount.AcctType)
            {
                return true;
            }
            if (account.BankNumber != oldAccount.BankNumber)
            {
                return true;
            }
            if (account.Inactive != oldAccount.Inactive)
            {
                return true;
            }
            if (account.AccountColor != oldAccount.AccountColor)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes one Account from the database.
        /// </summary>
        public static void Delete(long accountNum)
        {
            Db.NonQ("DELETE FROM account WHERE AccountNum = " + POut.Long(accountNum));
        }
    }
}