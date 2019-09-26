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
using System;

namespace CodeBase
{
    public class DateTimeOD
    {
        /// <summary>
        /// We are switching to using this method instead of DateTime.Today.
        /// You can track actual Year/Month/Date differences by creating an instance of this class and passing in the two dates to compare.
        /// The values will be stored in YearsDiff, MonthsDiff, and DaysDiff.
        /// </summary> 
        [Obsolete] 
        public static DateTime Today =>
            new DateTime(
                DateTime.Today.Year, 
                DateTime.Today.Month, 
                DateTime.Today.Day, 0, 0, 0, 
                DateTimeKind.Unspecified);

        /// <summary>
        /// Returns the most recent valid date possible based on the year and month passed in.
        /// E.g. y:2017,m:4,d:31 is passed in (an invalid date) which will return a date of 
        /// "04/30/2017" which is the most recent 'valid' date. Throws an exception if the year is 
        /// not between 1 and 9999, and if the month is not between 1 and 12.
        /// </summary>
        public static DateTime GetMostRecentValidDate(int year, int month, int day) =>
            new DateTime(
                year, month, Math.Min(day, DateTime.DaysInMonth(year, month)));
        
    }
}
