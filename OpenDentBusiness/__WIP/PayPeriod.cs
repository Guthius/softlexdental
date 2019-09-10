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
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used to view employee timecards. 
    /// Timecard entries are not linked to a pay period. 
    /// Instead, payperiods are setup, and the user can only view specific pay periods. 
    /// So it feels like they are linked, but it's date based.
    /// </summary>
    public class PayPeriod : DataRecord
    {
        private static readonly DataRecordCache<PayPeriod> cache = 
            new DataRecordCache<PayPeriod>("SELECT * FROM `pay_periods` ORDER BY `date_start`", FromReader);

        /// <summary>
        /// The first day of the pay period.
        /// </summary>
        public DateTime DateStart;

        /// <summary>
        /// The last day of the pay period. Inclusive, ignoring time of day.
        /// </summary>
        public DateTime DateEnd;

        /// <summary>
        /// The date that paychecks will be dated. A few days after the <see cref="DateEnd"/>.
        /// </summary>
        public DateTime DatePaycheck;

        public static PayPeriod FromReader(MySqlDataReader dataReader)
        {
            return new PayPeriod
            {
                Id = Convert.ToInt64(dataReader["id"]),
                DateStart = Convert.ToDateTime(dataReader["date_start"]),
                DateEnd = Convert.ToDateTime(dataReader["date_end"]),
                DatePaycheck = Convert.ToDateTime(dataReader["date_paycheck"])
            };
        }

        public static List<PayPeriod> All() =>
            cache.All().ToList();

        public static PayPeriod GetById(long payPeriodId) =>
            cache.FirstOrDefault(payPeriod => payPeriod.Id == payPeriodId);

        public static PayPeriod GetByDate(DateTime date) =>
            cache.FirstOrDefault(x => date.Date >= x.DateStart.Date && date.Date <= x.DateEnd.Date);

        public static PayPeriod GetMostRecent() =>
            SelectOne("SELECT * FROM `pay_periods` WHERE `date_end` = (SELECT MAX(`date_end`) FROM `pay_periods`)", FromReader);

        public static long Insert(PayPeriod payPeriod) =>
            payPeriod.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `pay_periods` (`date_start`, `date_end`, `date_paycheck`) VALUES (?date_start, ?date_end, ?date_paycheck)",
                    new MySqlParameter("date_start", payPeriod.DateStart),
                    new MySqlParameter("date_end", payPeriod.DateEnd),
                    new MySqlParameter("date_paycheck", payPeriod.DatePaycheck));

        public static void Update(PayPeriod payPeriod) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `pay_periods` SET `date_start` = ?date_start, `date_end` = ?date_end, `date_paycheck` = ?date_paycheck WHERE `id` = ?id",
                    new MySqlParameter("date_start", payPeriod.DateStart),
                    new MySqlParameter("date_end", payPeriod.DateEnd),
                    new MySqlParameter("date_paycheck", payPeriod.DatePaycheck),
                    new MySqlParameter("id", payPeriod.Id));

        public static void Delete(PayPeriod payPeriod) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `pay_periods` WHERE `id` = " + payPeriod.Id);

        /// <summary>
        /// Checks whether there is a pay period available for the specified date.
        /// </summary>
        /// <param name="date">The date to check for.</param>
        /// <returns>True if a pay period exists for the given date; otherwise, false.</returns>
        public static bool HasPeriodForDate(DateTime date) =>
            cache.Any(payPeriod => date.Date >= payPeriod.DateStart && date.Date <= payPeriod.DateEnd);

        /// <summary>
        /// Determines whether there is any overlap in dates between the two passed-in list of pay periods.
        /// Same-date overlaps are not allowed (e.g. you cannot have a pay period that ends the same day as the next one starts).
        /// </summary>
        public static bool AreAnyOverlapping(List<PayPeriod> firstList, List<PayPeriod> secondList)
        {
            foreach (var x in firstList)
            {
                var overlap =
                    secondList.Where(
                        y =>
                            x.Id != y.Id &&
                            ((x.DateEnd   >= y.DateStart && x.DateEnd   <= y.DateEnd) ||
                             (x.DateStart >= y.DateStart && x.DateStart <= y.DateEnd)));

                if (overlap.Count() > 0) return true;
            }

            return false;
        }
    }
}
