using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class AccountingAutoPays
    {
        class AccountingAutoPayCache : CacheListAbs<AccountingAutoPay>
        {
            protected override AccountingAutoPay Copy(AccountingAutoPay accountingAutoPay) => accountingAutoPay.Clone();

            protected override void FillCacheIfNeeded() => AccountingAutoPays.GetTableFromCache(false);

            protected override List<AccountingAutoPay> GetCacheFromDb()
            {
                return Crud.AccountingAutoPayCrud.SelectMany("SELECT * FROM accountingautopay");
            }

            protected override DataTable ListToTable(List<AccountingAutoPay> listAccountingAutoPays)
            {
                return Crud.AccountingAutoPayCrud.ListToTable(listAccountingAutoPays, "AccountingAutoPay");
            }

            protected override List<AccountingAutoPay> TableToList(DataTable table) => Crud.AccountingAutoPayCrud.TableToList(table);
        }

        static AccountingAutoPayCache _accountingAutoPayCache = new AccountingAutoPayCache();

        public static List<AccountingAutoPay> GetDeepCopy(bool isShort = false)
        {
            return _accountingAutoPayCache.GetDeepCopy(isShort);
        }

        public static AccountingAutoPay GetFirstOrDefault(Func<AccountingAutoPay, bool> match, bool isShort = false)
        {
            return _accountingAutoPayCache.GetFirstOrDefault(match, isShort);
        }

        public static int GetCount(bool isShort = false) => _accountingAutoPayCache.GetCount(isShort);
        
        public static DataTable RefreshCache() => GetTableFromCache(true);
        
        public static void FillCacheFromTable(DataTable table)
        {
            _accountingAutoPayCache.FillCacheFromTable(table);
        }

        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _accountingAutoPayCache.GetTableFromCache(doRefreshCache);
        }

        public static long Insert(AccountingAutoPay pay) => Crud.AccountingAutoPayCrud.Insert(pay);
        
        /// <summary>
        /// Converts the comma delimited list of AccountNums into full descriptions separated by carriage returns.
        /// </summary>
        public static string GetPickListDesc(AccountingAutoPay pay)
        {
            string[] numArray = pay.PickList.Split(new char[] { ',' });
            string retVal = "";
            for (int i = 0; i < numArray.Length; i++)
            {
                if (numArray[i] == "")
                {
                    continue;
                }
                if (retVal != "")
                {
                    retVal += "\r\n";
                }
                retVal += Account.GetDescript(PIn.Long(numArray[i]));
            }
            return retVal;
        }

        /// <summary>
        /// Converts the comma delimited list of AccountNums into an array of AccountNums.
        /// </summary>
        public static long[] GetPickListAccounts(AccountingAutoPay pay)
        {
            string[] numArray = pay.PickList.Split(new char[] { ',' });
            List<long> AL = new List<long>();
            for (int i = 0; i < numArray.Length; i++)
            {
                if (numArray[i] == "")
                {
                    continue;
                }
                AL.Add(PIn.Long(numArray[i]));
            }
            return AL.ToArray();
        }

        /// <summary>
        /// Loops through the AList to find one with the specified payType (defNum).
        /// If none is found, then it returns null.
        /// </summary>
        public static AccountingAutoPay GetForPayType(long payType) => GetFirstOrDefault(x => x.PayType == payType);
        
        /// <summary>
        /// Saves the list of accountingAutoPays to the database. Deletes all existing ones first.
        /// </summary>
        public static void SaveList(List<AccountingAutoPay> list)
        {
            Db.NonQ("DELETE FROM accountingautopay");
            for (int i = 0; i < list.Count; i++)
            {
                Insert(list[i]);
            }
        }
    }
}