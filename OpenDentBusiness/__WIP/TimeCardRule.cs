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
    public class TimeCardRule : DataRecord
    {
        /// <summary>
        /// The ID of the employee the rule applies to. If null, the rule applies to all employees.
        /// </summary>
        public long? EmployeeId;

        /// <summary>
        /// Indicates the working hours in a day. A typical example is 08:00. In California, any 
        /// work after the first 8 hours is overtime. All time after the specified hours is 
        /// overtime.
        /// </summary>
        public TimeSpan Hours;

        /// <summary>
        /// Indicates the regular work start time. All time worked before this time is at a differential rate.
        /// </summary>
        public TimeSpan TimeStart;

        /// <summary>
        /// Indicates the regular work end time. All time worked after this time is at a differntial rate.
        /// </summary>
        public TimeSpan TimeEnd;

        /// <summary>
        /// Indicates if the employee should have overtime calculated for their hours worked in a pay period.
        /// </summary>
        public bool IsOvertimeExempt;

        /// <summary>
        /// The earliest time at which an employee can clock in. If set to <see cref="TimeSpan.Zero"/> the employee can clock in at any time.
        /// </summary>
        public TimeSpan ClockInTime;

        /// <summary>
        /// Constructs a new instance of the <see cref="TimeCardRule"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="TimeCardRule"/> instance.</returns>
        public static TimeCardRule FromReader(MySqlDataReader dataReader)
        {
            return new TimeCardRule
            {
                Id = Convert.ToInt64(dataReader["id"]),
                EmployeeId = dataReader["employee_id"] as long?,
                Hours = (TimeSpan)dataReader["hours"],
                TimeStart = (TimeSpan)dataReader["time_start"],
                TimeEnd = (TimeSpan)dataReader["time_end"],
                IsOvertimeExempt = Convert.ToBoolean(dataReader["overtime_exempt"]),
                ClockInTime = (TimeSpan)dataReader["clockin_time"]
            };
        }

        public static List<TimeCardRule> All() =>
            SelectMany("SELECT * FROM `time_card_rules`", FromReader);

        /// <summary>
        /// Gets the time card rule with the specified ID from the database.
        /// </summary>
        /// <param name="timeCardRuleId">The ID of the time card rule.</param>
        /// <returns>The time card rule with the specified ID.</returns>
        public static TimeCardRule GetById(long timeCardRuleId) =>
            SelectOne("SELECT * FROM `time_card_rules` WHERE `id` = " + timeCardRuleId, FromReader);

        /// <summary>
        /// Inserts the specified time card rule in the database.
        /// </summary>
        /// <param name="timeCardRule">The time card rule.</param>
        /// <returns>The ID assigned to the time card rule.</returns>
        public static long Insert(TimeCardRule timeCardRule) =>
            timeCardRule.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `time_card_rules` (`employee_id`, `hours`, `time_start`, `time_end`, `overtime_exempt`, `clockin_time`) " +
                "VALUES (?employee_id, ?hours, ?time_start, ?time_end, ?overtime_exempt, ?clockin_time)",
                    new MySqlParameter("employee_id", timeCardRule.EmployeeId),
                    new MySqlParameter("hours", timeCardRule.Hours),
                    new MySqlParameter("time_start", timeCardRule.TimeStart),
                    new MySqlParameter("time_end", timeCardRule.TimeEnd),
                    new MySqlParameter("overtime_exempt", timeCardRule.IsOvertimeExempt),
                    new MySqlParameter("clockin_time", timeCardRule.ClockInTime));

        /// <summary>
        /// Updates the specified time card rule in the database.
        /// </summary>
        /// <param name="timeCardRule">The time card rule.</param>
        public static void Update(TimeCardRule timeCardRule) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE time_card_rules SET employee_id = ?employee_id, hours = ?hours, ",
                    new MySqlParameter("employee_id", timeCardRule.EmployeeId),
                    new MySqlParameter("hours", timeCardRule.Hours),
                    new MySqlParameter("time_start", timeCardRule.TimeStart),
                    new MySqlParameter("time_end", timeCardRule.TimeEnd),
                    new MySqlParameter("overtime_exempt", timeCardRule.IsOvertimeExempt),
                    new MySqlParameter("clockin_time", timeCardRule.ClockInTime),
                    new MySqlParameter("id", timeCardRule.ClockInTime));

        /// <summary>
        /// Deletes the time card rule with the specified ID from the database.
        /// </summary>
        /// <param name="timeCardRuleId">The ID of the time card rule to delete.</param>
        public static void Delete(long timeCardRuleId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `time_card_rules` WHERE `id` = " + timeCardRuleId);

        /// <summary>
        /// Deletes all time card rules with the specified ID's from the database.
        /// </summary>
        /// <param name="timeCardRuleIds">The ID's of the time card rules to delete.</param>
        public static void DeleteMany(IEnumerable<long> timeCardRuleIds)
        {
            if (timeCardRuleIds != null && timeCardRuleIds.Count() > 0)
            {
                DataConnection.ExecuteNonQuery("DELETE FROM `time_card_rules` WHERE `id` IN (" + string.Join(", ", timeCardRuleIds) + ")");
            }
        }








        /// <summary>
        /// Clears automatic adjustment/adjustOT values and deletes automatic TimeAdjusts for period.
        /// </summary>
        public static void ClearAuto(long employeeId, DateTime dateStart, DateTime dateEnd)
        {
            var clockEvents = ClockEvents.GetSimpleList(employeeId, dateStart, dateEnd);
            foreach (var clockEvent in clockEvents)
            {
                clockEvent.AdjustAuto = TimeSpan.Zero;
                clockEvent.OvertimeAuto = TimeSpan.Zero;
                clockEvent.Rate2Auto = TimeSpan.Zero;

                ClockEvents.Update(clockEvent);
            }

            var timeAdjusts = TimeAdjusts.GetSimpleListAuto(employeeId, dateStart, dateEnd);
            foreach (var timeAdjust in timeAdjusts)
            {
                TimeAdjusts.Delete(timeAdjust);
            }
        }

        /// <summary>
        /// Clears all manual adjustments/Adjust OT values from clock events. Does not alter
        /// adjustments to clockevent.TimeDisplayed1/2 nor does it delete or alter any 
        /// TimeAdjusts.
        /// </summary>
        public static void ClearManual(long employeeId, DateTime dateStart, DateTime dateEnd)
        {
            var clockEvents = ClockEvents.GetSimpleList(employeeId, dateStart, dateEnd);
            foreach (var clockEvent in clockEvents)
            {
                clockEvent.Adjust = TimeSpan.Zero;
                clockEvent.AdjustOverridden = false;
                clockEvent.Overtime = TimeSpan.FromHours(-1);
                clockEvent.Rate2 = TimeSpan.FromHours(-1);

                ClockEvents.Update(clockEvent);
            }
        }

    }
}
